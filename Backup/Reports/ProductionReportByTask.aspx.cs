using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

using Interrial.PPRS.Dal.TypedListClasses;
using Interrial.PPRS.Dal.EntityClasses;
using Interrial.PPRS.Dal.FactoryClasses;
using Interrial.PPRS.Dal.CollectionClasses;
using Interrial.PPRS.Dal.HelperClasses;
using Interrial.PPRS.Dal;

using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.DQE.SqlServer;

namespace InterrailPPRS.Reports
{
    public partial class ProductionReportByTask : PageBase
    {

        public DataReader rs;
        public string sMonth,sYear,sFrom,sTo;
        public string sSelectedShifts;
        public string sSQL;
        public int rs_numRows = 0;
        public string sWorkDates;
        public string sRptType;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin, User, Production");


            //This query does !have a server behavior ---;

            sRptType = UCase(Request["ReportType"]);

            sSelectedShifts = Request["SelectedShifts"];

            Session["LastShiftList"] = sSelectedShifts;


            if(sRptType == "MONTHLY"){
              sMonth = Request["Month"];
              sYear  = Request["Year"];
              sFrom = sMonth + "/01/" + sYear;
              sTo   = cStr(cDate(sFrom).AddMonths(1));  
              sTo   = cStr(cDate(sTo).AddDays(-1));  
            }else{
              sFrom = Request["From"];
              sTo   = Request["To"];
              Session["LastStartDate"] = sFrom;
              Session["LastEndDate"]   = sTo;
            }

            sWorkDates = " '" + cStr(sFrom) + "' AND '" + cStr(sTo) + "' ";


            if(sRptType == "MONTHLY"){
              sSQL =  "  SELECT Distinct Tasks.ID, TaskDescription, WorkDate='" + MonthName(cDate(sTo).Month) + "'      ";
            }else{
              sSQL =  "  SELECT Distinct Tasks.ID, TaskDescription, WorkDate                    ";
            }

            sSQL +=  "            FROM FacilityProductionDetail INNER JOIN Tasks                          ";
            sSQL +=  "               ON FacilityProductionDetail.TaskID = Tasks.ID                        ";
            sSQL +=  "            WHERE FacilityProductionDetail.WorkDate Between " + sWorkDates;
            sSQL +=  "              AND FacilityProductionDetail.FacilityID = " + Session["FacilityID"];
            sSQL +=  "              AND FacilityProductionDetail.ShiftID in (" + sSelectedShifts + ") ";
            sSQL +=  "         ORDER BY WorkDate, TaskDescription                                         ";

            rs = new DataReader(sSQL);
            rs.Open();
            rs_numRows = 0;

        }
        
        public void ShowProductionReportSummary(){
 
                      DataReader rstCustomers, rstTasks;
                      string sDateRange, sPageBreak, sTask;
                      string sDate, sCustomer, sCustomerID;
                      string sPreviewLink;
                      string sManufacturer;
                      string sTitle;
                      int iTotalCars = 0;
                      int iTotalUnits = 0;

                      if(sRptType == "MONTHLY"){
                        sDateRange = MonthName(cInt(sMonth)) + " " + sYear;
                        sPreviewLink = "ProductionReportByTask.aspx?PrintPreview=0&ReportType=Monthly&Month=" + sMonth + "&Year=" + sYear + "&SelectedShifts=" + sSelectedShifts;
                        sTitle = "Monthly Production Report<br>By Task for Shifts " + sSelectedShifts + " : ";
                      }else{
                        if(cDate(sFrom) == cDate(sTo)){
                          sDateRange = sFrom;
                        }else{
                          sDateRange = sFrom + " - " + sTo;
                        }
                        sPreviewLink = "ProductionReportByTask.aspx?PrintPreview=0&ReportType=Daily&From=" + sFrom + "&To=" + sTo + "&SelectedShifts=" + sSelectedShifts;
                        sTitle = "Daily Production Report<br>By Task for Shifts " + sSelectedShifts + " : ";
                      }

                      iTotalCars = 0;
                      iTotalUnits = 0;

                      if(cStr(Request["PrintPreview"]).Length > 0){
                        Response.Write("<table border='0' width='300' align='center'>");
                        Response.Write("  <tr><td colspan='3'>&nbsp;</td></tr>");
                        Response.Write("  <tr><td colspan='3' align='right'><a href='" + sPreviewLink + "'>Printer Friendly</a></td></tr>");
                      }else{
                        Response.Write("<table border='0' width='420' align='center'>");
                        Response.Write("  <tr><td colspan='3'>&nbsp;</td></tr>");
                     }

                      if(rs.EOF){
                        Response.Write("  <tr><td colspan='3'>&nbsp;</td></tr>");
                        Response.Write("  <tr><td colspan='3'>No records found.</td></tr>");
                      }else{
                        sDate = "";
                        sTask = "";
                        sPageBreak = "";

                        while (! rs.EOF){
                            rs.Read();
                            if( (sTask != rs.Item("TaskDescription")) || (sDate != rs.Item("WorkDate")) ){
                              if( (sTask != "") ){
                                Response.Write("<tr class='ReportOddLine'><td align='right'>Totals for " + sTask + " - " + FormatDate(sDate) +  "</td><td align='right' class='cellTopBottomBorder'>" + cStr(iTotalCars) + "</td><td align='right' class='cellTopBottomBorder'>" + cStr(iTotalUnits) + "</td></tr>");
                                iTotalCars  = 0;
                                iTotalUnits = 0;
                             }
                              sTask = rs.Item("TaskDescription");
                           }

                          if(sDate != rs.Item("WorkDate")){
                            if( (sDate != "") && (iTotalCars != 0 && iTotalUnits != 0) ){
                              Response.Write("<tr><td colspan=3 align='center'> Date - totals here...</td></tr>");
                           }
                              if( (sDate == "") || Request["PrintPreview"] == "0"){
                                Response.Write("  <tr><td colspan='3' align='center'><b>"  + sPageBreak  + sTitle + sDateRange + "</b></td></tr>");
                                Response.Write("  <tr><td colspan='3' align='center'><div align='center'><b>" + Session["FacilityName"] + "</b></td></tr>");
                              }
                            Response.Write("<tr><td colspan=3>&nbsp;</td></tr>");
                            Response.Write("<tr><td colspan=3 class='cellTopBottomBorder'>Work Date:&nbsp;" + FormatDate(rs.Item("WorkDate")) + "</td></tr>");
                            sPageBreak = "<h3>&nbsp;</h3>";
                            sDate = rs.Item("WorkDate");
                          }else{
                            Response.Write("<tr><td>&nbsp;</td></tr>");
                         }

                          Response.Write("<tr><td align='left'><font color=green>&nbsp;&nbsp;" + rs.Item("TaskDescription") + "</font></td><td class='cellTopBottomBorder' align='center'>Items</td><td class='cellTopBottomBorder' align='center'>Units</td></tr>");
  
                          sSQL = "    SELECT CustomerName, Units=Sum(Units), TaskID,                  ";
                          if(sRptType == "MONTHLY"){
                            sSQL +=  "  c=Count(*), WorkDate='" + MonthName(cDate(sTo).Month) + "'      ";
                          }else{
                            sSQL +=  "  c=Count(*), WorkDate  ";
                          }
                          sSQL +=  "    FROM FacilityProductionDetail  INNER JOIN                       ";
                          sSQL +=  "    Tasks ON FacilityProductionDetail.TaskId = Tasks.Id INNER JOIN    ";
                          sSQL +=  "    FacilityCustomer ON FacilityCustomerId = FacilityCustomer.Id      ";
                          sSQL +=  "    WHERE FacilityProductionDetail.WorkDate Between " + sWorkDates;
                          sSQL +=  "    AND FacilityProductionDetail.FacilityID = " + Session["FacilityID"];
                          sSQL +=  "    AND FacilityProductionDetail.ShiftID in (" + sSelectedShifts + ") ";

                          if(sRptType == "MONTHLY"){
                            sSQL +=  "  GROUP BY CustomerName, TaskID ";
                          }else{
                            sSQL +=  "  GROUP BY WorkDate, CustomerName, TaskID ";
                          }


                          rstCustomers = new DataReader(sSQL);
                          rstCustomers.Open();
 
                          while (! rstCustomers.EOF){
                            rstCustomers.Read();
                            if(rstCustomers.Item("WorkDate") == rs.Item("WorkDate") ){
                              Response.Write("<tr><td align='right'>" + rstCustomers.Item("CustomerName") + "</td><td align='right'>" + rstCustomers.Item("c") + "</td><td align='right'>" + rstCustomers.Item("Units") + "</td></tr>");
                              iTotalCars  += cInt(rstCustomers.Item("c"));
                              iTotalUnits += cInt(rstCustomers.Item("Units"));
                            }

                          } //End Loop


                      } //End Loop

                        if(sTask != ""){
                          Response.Write("<tr class='ReportOddLine'><td align='right'>Totals for " + sTask + " - " + FormatDate(sDate) + " </td><td align='right' class='cellTopBottomBorder'>" + cStr(iTotalCars) + "</td><td align='right' class='cellTopBottomBorder'>" + cStr(iTotalUnits) + "</td></tr>");
                          iTotalCars  = 0;
                          iTotalUnits = 0;
                       }
                     }

                      if(cStr(Request["PrintPreview"]).Length > 0){
                        Response.Write("  <tr><td colspan='3' align='right'><a href='" + sPreviewLink + "'>Printer Friendly</a></td></tr>");
                      }else{
                        Response.Write("  <tr><td colspan='3'>&nbsp;</td></tr>");
                      }

                      Response.Write("</table>");

                    }


    }
}