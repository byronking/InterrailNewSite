﻿using System;
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
    public partial class ProductionReportByCustomer : PageBase
    {
        public DataReader rs;
        public string sMonth,sYear,sFrom,sTo;
        public string sSelectedShifts;
        public string sSQL;
        public int rs_numRows = 0;

        public string sWorkDates = "";
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
              sSQL =  " SELECT Distinct FacilityCustomer.ID, CustomerCode=CustomerName + ' - ' + ContactName, WorkDate='" + MonthName(cDate(sTo).Month) + "'      ";
            }else{
              sSQL =  " SELECT Distinct FacilityCustomer.ID, CustomerCode=CustomerName + ' - ' + ContactName, WorkDate            ";
            }

            sSQL +=  "             FROM FacilityProductionDetail INNER JOIN FacilityCustomer              ";
            sSQL +=  "               ON FacilityProductionDetail.FacilityCustomerID = FacilityCustomer.ID ";
            sSQL +=  "            WHERE FacilityProductionDetail.WorkDate Between " + sWorkDates;
            sSQL +=  "              AND FacilityProductionDetail.FacilityID = " + Session["FacilityID"];
            sSQL +=  "              AND FacilityProductionDetail.ShiftID in (" + sSelectedShifts + ") ";
            sSQL +=  "         ORDER BY WorkDate, FacilityCustomer.ID                                            ";

            rs = new DataReader(sSQL);

            rs.Open();
            rs_numRows = 0;

        }

        public void ShowProductionReportSummary(){

            DataReader rstTasks;
            string sDate, sCustomer, sCustomerID;
            string sDateRange, sPageBreak;
            string sPreviewLink;
            string sTitle;
            string sRptType = "";
            int iTotalCars = 0;
            int iTotalUnits = 0;

            if(sRptType == "MONTHLY"){

                sDateRange = MonthName(cDate(sMonth).Month) + " " + sYear;
                sPreviewLink = "ProductionReportByCustomer.aspx?PrintPreview=0&ReportType=Monthly&Month=" + sMonth + "&Year=" + sYear + "&SelectedShifts=" + sSelectedShifts;
                sTitle = "Monthly Production Report<br>By Customer for Shifts " + sSelectedShifts + " : ";

            }else{

                if(cDate(sFrom) == cDate(sTo)){
                    sDateRange = sFrom;
                }else{
                    sDateRange = sFrom + " - " + sTo;
                }

                sPreviewLink = "ProductionReportByCustomer.aspx?PrintPreview=0&ReportType=Daily&From=" + sFrom + "&To=" + sTo + "&SelectedShifts=" + sSelectedShifts;
                sTitle = "Daily Production Report<br>By Customer for Shifts " + sSelectedShifts + " : ";
            }

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
              sCustomer = "";
              sCustomerID = "";
              sPageBreak = "";

              while (! rs.EOF){
                rs.Read();
                if( (sCustomerID != rs.Item("ID")) || (sDate != rs.Item("WorkDate"))){

                    if( (sCustomerID != "") ){
                      Response.Write("<tr class='ReportOddLine'><td align='right'>Totals for " + sCustomer + " - " + FormatDate(sDate) +  "</td><td align='right' class='cellTopBottomBorder'>" + cStr(iTotalCars) + "</td><td align='right' class='cellTopBottomBorder'>" + cStr(iTotalUnits) + "</td></tr>");
                      iTotalCars  = 0;
                      iTotalUnits = 0;
                    }

                    sCustomer = rs.Item("CustomerCode");
                    sCustomerID = rs.Item("ID");

                }

                if(sDate != rs.Item("WorkDate")){
                    if( (sDate == "") || Request["PrintPreview"] == "0" ){
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

            Response.Write("<tr><td align='left'><font color=green>&nbsp;&nbsp;" + rs.Item("CustomerCode") + "</font></td><td class='cellTopBottomBorder' align='center'>Items</td><td class='cellTopBottomBorder' align='center'>Units</td></tr>");
            
            sSQL =  " SELECT TaskDescription, Units=Sum(Units), FacilityCustomerID, ";
            if(sRptType == "MONTHLY"){
               sSQL +=  "                  c=Count(*), WorkDate='" + cStr(cDate(sTo).Month) + "' ";
            }else{
               sSQL +=  "                  c=Count(*), WorkDate  ";
            }
            sSQL +=  "             FROM FacilityProductionDetail  INNER JOIN ";
            sSQL +=  "                  Tasks ON FacilityProductionDetail.TaskId = Tasks.Id INNER JOIN  ";
            sSQL +=  "                  FacilityCustomer ON FacilityCustomerId = FacilityCustomer.Id ";
            sSQL +=  "            WHERE FacilityProductionDetail.WorkDate Between " + sWorkDates;
            sSQL +=  "              AND FacilityProductionDetail.FacilityID = " + Session["FacilityID"];
            sSQL +=  "              AND FacilityProductionDetail.ShiftID in (" + sSelectedShifts + ") ";
            sSQL +=  "              AND FacilityCustomerID = " + rs.Item("ID");

            if(sRptType == "MONTHLY"){
               sSQL +=  "         GROUP BY FacilityCustomerID, TaskDescription ";
               sSQL +=  "         Order BY FacilityCustomerID, TaskDescription  ";
            }else{
               sSQL +=  "         GROUP BY WorkDate, FacilityCustomerID, TaskDescription ";
               sSQL +=  "         Order BY WorkDate, FacilityCustomerID, TaskDescription ";
            }      

            rstTasks = new DataReader(sSQL);
            rstTasks.Open();

            while (! rstTasks.EOF){
                rstTasks.Read();
                if(rstTasks.Item("WorkDate") == rs.Item("WorkDate") ){
                   Response.Write("<tr><td align='right'>" + rstTasks.Item("TaskDescription") + "</td><td align='right'>" + rstTasks.Item("c") + "</td><td align='right'>" + rstTasks.Item("Units") + "</td></tr>");
                   iTotalCars  = iTotalCars  + cInt(rstTasks.Item("c"));
                   iTotalUnits = iTotalUnits + cInt(rstTasks.Item("Units"));
                }

            } //End Loop


            } //End Loop

            if( (sCustomerID != "") ){
                Response.Write("<tr class='ReportOddLine'><td align='right'>Totals for " + sCustomer + " - " + FormatDate(sDate) + " </td><td align='right' class='cellTopBottomBorder'>" + cStr(iTotalCars) + "</td><td align='right' class='cellTopBottomBorder'>" + cStr(iTotalUnits) + "</td></tr>");
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