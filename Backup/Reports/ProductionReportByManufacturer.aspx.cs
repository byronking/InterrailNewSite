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
    public partial class ProductionReportByManufacturer : PageBase
    {

        public DataReader rs;
        public string sMonth,sYear,sFrom,sTo;
        public string sSelectedShifts;
        public string sSQL;
        public int rs_numRows = 0;
        public string sWorkDates;
        public string sRptType = "";

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
              sSQL = "   SELECT Distinct IRGManufacturer.ID, ManufacturerName, ManufacturerCode, WorkDate='" + MonthName(cDate(sTo).Month) + "'      ";
            }else{
              sSQL = "   SELECT Distinct IRGManufacturer.ID, ManufacturerName, ManufacturerCode, WorkDate  ";
            }

            sSQL += "             FROM FacilityProductionDetail INNER JOIN IRGManufacturer              ";
            sSQL += "               ON FacilityProductionDetail.ManufacturerID = IRGManufacturer.ID     ";
            sSQL += "            WHERE FacilityProductionDetail.WorkDate Between " + sWorkDates;
            sSQL += "              AND FacilityProductionDetail.FacilityID = " + Session["FacilityID"]   ;
            sSQL += "              AND FacilityProductionDetail.ShiftID in (" + sSelectedShifts + ")    ";
            sSQL += "            ORDER BY WorkDate, ManufacturerName                                    ";

            rs = new DataReader(sSQL);
            rs.Open();
            rs_numRows = 0;

            }

            public void ShowProductionReportSummary(){

              DataReader rstCustomers, rstTasks;
              string sDateRange, sPageBreak;
              string sDate, sCustomer, sCustomerID;
              string sPreviewLink;
              string sManufacturer;
              string sTitle;
              int iTotalCars = 0;
              int iTotalUnits = 0;

              if (sRptType == "MONTHLY")
              {
                sDateRange = MonthName(cInt(sMonth)) + " " + sYear;
                sPreviewLink = "ProductionReportByManufacturer.aspx?PrintPreview=0&ReportType=Monthly&Month=" + sMonth + "&Year=" + sYear + "&SelectedShifts=" + sSelectedShifts;
                sTitle = "Monthly Production Report<br>By Manufacturer for Shifts " + sSelectedShifts + " : ";
              }else{

                if(cDate(sFrom) == cDate(sTo)){
                   sDateRange = sFrom;
                 }else{
                   sDateRange = sFrom + " - " + sTo;
                }

                 sPreviewLink = "ProductionReportByManufacturer.aspx?PrintPreview=0&ReportType=Daily&From=" + sFrom + "&To=" + sTo + "&SelectedShifts=" + sSelectedShifts;
                 sTitle = "Daily Production Report<br>By Manufacturer for Shifts " + sSelectedShifts + " : ";
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
                sManufacturer = "";
                sPageBreak = "";

                while (! rs.EOF){
                    rs.Read();
                    if((sManufacturer != rs.Item("ManufacturerName")) || (sDate != rs.Item("WorkDate"))){

                      if( (sManufacturer != "") ){
                        Response.Write("<tr class='ReportOddLine'><td align='right'>Totals for " + sManufacturer + " - " + FormatDate(sDate) +  "</td><td align='right' class='cellTopBottomBorder'>" + cStr(iTotalCars) + "</td><td align='right' class='cellTopBottomBorder'>" + cStr(iTotalUnits) + "</td></tr>");
                        iTotalCars  = 0;
                        iTotalUnits = 0;
                      }
                      sManufacturer = rs.Item("ManufacturerName");
                   }

                   if(sDate != rs.Item("WorkDate")){

                     if( (sDate == "") || Request["PrintPreview"]== "0"){
                       Response.Write("  <tr><td colspan='3' align='center'><b>"  + sPageBreak + sTitle + sDateRange + "</b></td></tr>");
                       Response.Write("  <tr><td colspan='3' align='center'><div align='center'><b>" + Session["FacilityName"] + "</b></td></tr>");
                    }
                     Response.Write("<tr><td colspan=3>&nbsp;</td></tr>");
                     Response.Write("<tr><td colspan=3 class='cellTopBottomBorder'>Work Date:&nbsp;" + FormatDate(rs.Item("WorkDate")) + "</td></tr>");
                     sPageBreak = "<h3>&nbsp;</h3>";
                     sDate = rs.Item("WorkDate");
                  }else{
                    Response.Write("<tr><td>&nbsp;</td></tr>");
                 }

                Response.Write("<tr><td align='left'><font color=green>&nbsp;&nbsp;" + rs.Item("ManufacturerName")  + " (" + rs.Item("ManufacturerCode") +  ")</font></td><td class='cellTopBottomBorder' align='center'>Items</td><td class='cellTopBottomBorder' align='center'>Units</td></tr>");

                string sSQL = " ";
                sSQL = "  SELECT TaskDescription, Units=Sum(Units), ManufacturerID,                        ";
                if (sRptType == "MONTHLY") {
                    sSQL += "        c=Count(*), WorkDate='" + MonthName(cDate(sTo).Month) + "'   ";
                } else {
                    sSQL += "        c=Count(*), WorkDate  ";
                }
                sSQL += "             FROM FacilityProductionDetail  INNER JOIN                              ";
                sSQL += "                  Tasks ON FacilityProductionDetail.TaskId = Tasks.Id INNER JOIN    ";
                sSQL += "                  IRGManufacturer ON ManufacturerID = IRGManufacturer.Id            ";
                sSQL += "            WHERE FacilityProductionDetail.WorkDate Between " + sWorkDates;
                sSQL += "              AND FacilityProductionDetail.FacilityID = " + Session["FacilityID"];
                sSQL += "              AND FacilityProductionDetail.ShiftID in (" + sSelectedShifts + ")     ";
                if (sRptType == "MONTHLY"){
                    sSQL += "         GROUP BY ManufacturerID, TaskDescription                               ";
                } else {
                    sSQL += "         GROUP BY WorkDate, ManufacturerID, TaskDescription                     ";
                }

                rstTasks = new DataReader(sSQL);
                rstTasks.Open();

                  while (! rstTasks.EOF){
                      rstTasks.Read();

                      if(rstTasks.Item("WorkDate") == rs.Item("WorkDate")){
                         Response.Write("<tr><td align='right'>" + rstTasks.Item("TaskDescription") + "</td><td align='right'>" + rstTasks.Item("c") + "</td><td align='right'>" + rstTasks.Item("Units") + "</td></tr>");
                         iTotalCars  += cInt(rstTasks.Item("c"));
                         iTotalUnits +=  cInt(rstTasks.Item("Units"));
                       }

                  } //End Loop


              } //End Loop

                if(sManufacturer != ""){
                   Response.Write("<tr class='ReportOddLine'><td align='right'>Totals for " + sManufacturer + " - " + FormatDate(sDate) + " </td><td align='right' class='cellTopBottomBorder'>" + cStr(iTotalCars) + "</td><td align='right' class='cellTopBottomBorder'>" + cStr(iTotalUnits) + "</td></tr>");
                   iTotalCars  = 0;
                   iTotalUnits = 0;
                }
             }

              if(cStr(Request["PrintPreview"]).Length > 0){
                Response.Write("  <tr><td colspan='3'>&nbsp;</td></tr>");
                Response.Write("  <tr><td colspan='3' align='right'><a href='" + sPreviewLink + "'>Printer Friendly</a></td></tr>");
              }else{
                Response.Write("  <tr><td colspan='3'>&nbsp;</td></tr>");
              }

              Response.Write("</table>");

            }



    }
}