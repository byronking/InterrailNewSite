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
    public partial class ProductionDetailReport : PageBase
    {

        public string sselFacilities, sfromDateDetail, stoDateDetail, sselTasks,sSelectedShifts,wDateRange, wTasks;
        public string sselCustomers, sselOrigins, sselManufacturers,wFacilities,wCustomers,wOrigins,wManufacturers;

        public string hMan, sLRcNNum, sLRcNU, sLRcUNum, sLRcUU;
        public string sSpace, sURcNNum, sURcNU, sURcUNum, sURcUU;
        public string sHTML, sBy1, sBy2;
        public int IRow;

        public DataReader rs;
        public DataReader rsSumLvl;
        public DataReader rsSumAll;
        public DataReader rsSumNU; 

        public int rs_numRows = 0;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin, User, Production");

            sselFacilities    = cStr(Request["selFacilities"]);
            sfromDateDetail   = cStr(Request["fromDateDetail"]);
            stoDateDetail     = cStr(Request["toDateDetail"]);
            sselTasks         = cStr(Request["selTasks"]);
            sselCustomers     = cStr(Request["selCustomers"]);
            sselOrigins = cStr(Request["selOrigins"]);
            sselManufacturers = cStr(Request["selManufacturers"]);

            sSelectedShifts = Request["SelectedShifts"];
            Session["LastShiftList"] = sSelectedShifts;



            if(sselFacilities != ""){
              wFacilities = "  AND (FacilityProductionDetail.FacilityID IN  (" + sselFacilities + ") ) ";
            }else{
              wFacilities = "  AND (FacilityProductionDetail.FacilityID IN  (" + Session["FacilityID"] + ") ) ";
            }

            wDateRange = " AND (WorkDate Between '" + sfromDateDetail + "' AND '" + stoDateDetail + "') ";

            if(sselTasks != ""){
              wTasks = "  AND (TaskID IN  (" + sselTasks + ") ) ";
            }else{
              wTasks = "   ";
            }

            if(sselCustomers != ""){
              wCustomers = "  AND (FacilityCustomerID IN  (" + sselCustomers + ") ) ";
            }else{
              wCustomers = "   ";
            }

            if(sselOrigins != ""){
              wOrigins = "  AND (OriginID IN  (" + sselOrigins + ") ) ";
            }else{
              wOrigins = "   ";
            }

            if(sselManufacturers != ""){
              wManufacturers = "  AND (ManufacturerID IN  (" + sselManufacturers + ") ) ";
            }else{
              wManufacturers = "   ";
            }

            //This query does !have a server behavior ---;

            string strSQL = " ";
            strSQL +=  "SELECT WorkDate, OriginName, Name, TaskCode, FacilityProductionDetail.FacilityID, ";
            strSQL +=  "       ManufacturerName, CarTypeCode,          ";
            strSQL +=  "       RailCarNumber=Case When RailCarNumber='' Then 'N/A'   ";
            strSQL +=  "                          ELSE RailCarNumber End ,           ";
            strSQL +=  "       NewUsed, LevelType, Units, ApprovalStatus             ";
            strSQL +=  "  FROM FacilityProductionDetail                    INNER JOIN IRGOrigin       ";
            strSQL +=  "       ON OriginID           = IRGOrigin.ID        INNER JOIN Facility        ";
            strSQL +=  "       ON FacilityID         = Facility.Id         INNER JOIN Tasks           ";
            strSQL +=  "       ON TaskId             = Tasks.Id            INNER JOIN IRGManufacturer ";
            strSQL +=  "       ON ManufacturerID     = IRGManufacturer.ID  INNER JOIN FacilityCustomer";
            strSQL +=  "       ON FacilityCustomerId = FacilityCustomer.Id INNER JOIN IRGRailCarType  ";
            strSQL +=  "       ON CarTypeId          = IRGRailCarType.Id                              ";
            strSQL +=  " WHERE (Tasks.Rebillable=0) ";
            strSQL = strSQL +          wDateRange + wFacilities + wTasks + wCustomers + wManufacturers + wOrigins;
            strSQL +=  "             AND FacilityProductionDetail.ShiftID in (" + sSelectedShifts + ") ";
            strSQL +=  " ORDER BY FacilityProductionDetail.FacilityID, WorkDate, TaskCode,            ";
            strSQL +=  "           RailCarNumber, ManufacturerName, OriginName                        ";

             rs = new DataReader(strSQL);
             rs.Open();
             rs_numRows = 0;



            strSQL = " ";
            strSQL +=  "SELECT FacilityProductionDetail.TaskId, TaskCode, COUNT(*) AS RCs,        ";
            strSQL +=  "       SUM(Units) AS TU, ManufacturerName, NewUsed As RptType, WorkDate,  ";
            strSQL +=  "       FacilityTasks.FacilityID                                           ";
            strSQL +=  "FROM   FacilityProductionDetail                                           ";
            strSQL +=  "       INNER JOIN Tasks                                                   ";
            strSQL +=  "       ON FacilityProductionDetail.TaskId = Tasks.Id                      ";
            strSQL +=  "       INNER JOIN IRGManufacturer                                         ";
            strSQL +=  "       ON ManufacturerID = IRGManufacturer.ID                             ";
            strSQL +=  "       INNER JOIN Facility                                                ";
            strSQL +=  "       ON FacilityProductionDetail.FacilityID = Facility.Id               ";
            strSQL +=  "       RIGHT OUTER JOIN FacilityTasks                                     ";
            strSQL +=  "       ON FacilityProductionDetail.FacilityID = FacilityTasks.FacilityID  ";
            strSQL +=  "       AND Tasks.Id = FacilityTasks.TaskId                                ";
            strSQL +=  "WHERE  (Tasks.TaskCode IN ('LO', 'UL'))                                   ";
            strSQL +=  "                                                                          ";
            strSQL = strSQL + wDateRange + wFacilities;
            strSQL +=  "             AND FacilityProductionDetail.ShiftID in (" + sSelectedShifts + ") ";
            //  Do !filter Customers Manufactures AND Origins ---- BXM;
            strSQL +=  "                                                                          ";
            strSQL +=  "GROUP BY FacilityProductionDetail.TaskId, TaskCode, ManufacturerName,     ";
            strSQL +=  "         NewUsed, WorkDate, FacilityTasks.FacilityID                      ";
            strSQL +=  "ORDER BY FacilityTasks.FacilityID, WorkDate, ManufacturerName, TaskCode, NewUsed ";

             rsSumNU = new DataReader(strSQL);
             rsSumNU.Open();



            strSQL = " ";
            strSQL +=  "SELECT FacilityProductionDetail.TaskId, TaskCode, COUNT(*) AS RCs,        ";
            strSQL +=  "       SUM(Units) AS TU, ManufacturerName, LevelType As RptType, WorkDate,";
            strSQL +=  "       FacilityTasks.FacilityID                                           ";
            strSQL +=  "FROM   FacilityProductionDetail                                           ";
            strSQL +=  "       INNER JOIN Tasks                                                   ";
            strSQL +=  "       ON FacilityProductionDetail.TaskId = Tasks.Id                      ";
            strSQL +=  "       INNER JOIN IRGManufacturer                                         ";
            strSQL +=  "       ON ManufacturerID = IRGManufacturer.ID                             ";
            strSQL +=  "       INNER JOIN Facility                                                ";
            strSQL +=  "       ON FacilityProductionDetail.FacilityID = Facility.Id               ";
            strSQL +=  "       RIGHT OUTER JOIN FacilityTasks                                     ";
            strSQL +=  "       ON FacilityProductionDetail.FacilityID = FacilityTasks.FacilityID  ";
            strSQL +=  "       AND Tasks.Id = FacilityTasks.TaskId                                ";
            strSQL +=  "WHERE  (Tasks.TaskCode IN ('LO', 'UL'))                                   ";
            strSQL +=  "                                                                          ";
            strSQL = strSQL + wDateRange + wFacilities;
            strSQL +=  "             AND FacilityProductionDetail.ShiftID in (" + sSelectedShifts + ") ";
            //  Do !filter Customers Manufactures AND Origins ---- BXM + wCustomers + wManufacturers + wOrigins;
            strSQL +=  "                                                                          ";
            strSQL +=  "GROUP BY FacilityProductionDetail.TaskId, TaskCode, ManufacturerName,     ";
            strSQL +=  "         LevelType, WorkDate, FacilityTasks.FacilityID                    ";
            strSQL +=  "ORDER BY FacilityTasks.FacilityID, WorkDate, ManufacturerName, TaskCode, LevelType ";

            rsSumLvl = new DataReader(strSQL);
            rsSumLvl.Open();



            wDateRange = " AND ((WorkDate Between '" + sfromDateDetail + "' AND '" + stoDateDetail + "'))";
            if(sselFacilities != ""){
              wFacilities = "  AND (FacilityTasks.FacilityID IN  (" + sselFacilities + ") ) ";
            }else{
              wFacilities = "  AND (FacilityTasks.FacilityID IN  (" + Session["FacilityID"] + ") ) ";
            }


            strSQL = " ";
            strSQL +=  "SELECT  FacilityProductionDetail.TaskId, TaskCode, TaskDescription,       ";
            strSQL +=  "        COUNT(*) AS RCs, SUM(ISNULL(Units, 0)) AS TU, WorkDate,           ";
            strSQL +=  "        FacilityTasks.FacilityID                                          ";
            strSQL +=  "FROM    FacilityTasks                                                     ";
            strSQL +=  "        INNER JOIN Tasks ON FacilityTasks.TaskId = Tasks.Id               ";
            strSQL +=  "        LEFT OUTER JOIN FacilityProductionDetail                          ";
            strSQL +=  "        ON FacilityTasks.FacilityID = FacilityProductionDetail.FacilityID ";
            strSQL +=  "        AND Tasks.Id = FacilityProductionDetail.TaskId                    ";
            strSQL +=  "WHERE  (Tasks.Rebillable=0)                                               ";
            strSQL = strSQL + wDateRange + wFacilities;
            strSQL +=  "             AND FacilityProductionDetail.ShiftID in (" + sSelectedShifts + ") ";
            strSQL +=  "                                                                          ";
            strSQL +=  "GROUP BY FacilityProductionDetail.TaskId, TaskCode, TaskDescription,      ";
            strSQL +=  "         WorkDate, FacilityTasks.FacilityID                               ";
            strSQL +=  "ORDER BY FacilityTasks.FacilityID, WorkDate, TaskCode                     ";

            rsSumAll  = new DataReader(strSQL);
            rsSumAll.Open();

            //Response.Write(" All = " + rsSumAll.Source + "<br><br><br>";
            //Response.End;

        }

        public void ShowProductionDetailReport(){

          string sFrom, sTo, sPageBreak, sSumFac, sSumDate, rowColor;

          string sPreviewLink = "javascript:document.form1.submit();";
          string sTitle = "Detailed Production Report for Shifts " + sSelectedShifts + " <br>" + sfromDateDetail + " - " + stoDateDetail;

          double iTotalCars = 0;
          double iTotalUnits = 0.0;

          Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='480' align='center'>");
          Response.Write("  <tr><td colspan='8'>&nbsp;</td></tr>");
          //if(cStr(Request["PrintPreview"]).Length > 0){
          if (Request["PrintPreview"] != "0"){
            Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='480' align='center'>");
            Response.Write("  <tr><td colspan='8'>&nbsp;</td></tr>");
            Response.Write("  <tr><td colspan='8' align='right'><a href='" + sPreviewLink + "'>Printer Friendly</a></td></tr>");
            Response.Write("  <tr><td colspan='8' align='center'><b>" + sTitle + "</b></td></tr>");
          }else{
            Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='600' align='center'>");
            Response.Write("  <tr><td colspan='8'>&nbsp;</td></tr>");
            Response.Write("  <tr><td colspan='8'>&nbsp;</td></tr>");
         }

          if(rs.EOF){
            Response.Write("  <tr><td colspan='4'>&nbsp;</td></tr>");
            Response.Write("  <tr><td colspan='4'>No records found.</td></tr>");
          }else{

            string sFacility   = "";
            int sFacilityID = 0;
            string sDate = "";
            sPageBreak = "";

            int sSumFacID = 0;
            sSumFac   = "";
            sSumDate  = "";

            string sPreviousDate = sfromDateDetail;
            bool lastReadSuccess = rs.Read();
            while ( cDate(sPreviousDate) < cDate(rs.Item("WorkDate"))){
              Response.Write("<tr><td align='left' colspan=8><br><b><font color='green' size=-1>All Facilties: &nbsp;</font>&nbsp;&nbsp;-&nbsp;<b><span class='required'>No Data on: &nbsp;" + sPreviousDate + "</span></b></td></tr>");
              sPreviousDate = cStr(cDate(sPreviousDate).AddDays(1));
            } //End Loop

            while (lastReadSuccess){
               
              iTotalCars  = iTotalCars  + 1;
              iTotalUnits = iTotalUnits + cDbl(rs.Item("Units"));

              sSumFac   = sFacility;
              sSumFacID = sFacilityID;

              if(sFacility != rs.Item("Name")){
                sFacility = Trim(rs.Item("Name"));
                sFacilityID = cInt(rs.Item("FacilityID"));
                sDate = " ";
                iTotalCars = 0;
             }

              if(iTotalCars % 2 == 0){
                rowColor = "reportOddLine";
              }else{
                rowColor = "reportEvenLine";
             }


              if(sDate != rs.Item("WorkDate")){

                // Write summary for the day/facility;
                if(sSumDate != ""){
                  DoAllSummaries(sSumFac, cStr(sSumFacID), sSumDate);
                  if (cDate(sPreviousDate) < cDate(rs.Item("WorkDate"))){
                    while (  cDate(sPreviousDate) < cDate(rs.Item("WorkDate"))){
                      Response.Write("<tr><td align='left' colspan=8><br><b><font color='green' size=-1>" + rs.Item("Name") + "</font>&nbsp;&nbsp;-&nbsp;<b><span class='required'>" + sPreviousDate + " - No Data </span></b></td></tr>");
                      sPreviousDate = cStr(cDate(sPreviousDate).AddDays(1));
                    } //End Loop
                 }
               }
                sSumDate = FormatDate(rs.Item("WorkDate"));

                if(cStr(Request["PrintPreview"]) == "0" ){
                  Response.Write("  <tr><td colspan='8' align='center'><b>" + sPageBreak + sTitle + "</b></td></tr>");
               }

                if(1==1){
                  Response.Write("<tr><td align='left' colspan=8><br><b><font color='green' size=-1>" + rs.Item("Name") + "</font>&nbsp;&nbsp;-&nbsp;<b>" + FormatDate(rs.Item("WorkDate")) + "</b></td></tr>");
               }
                Response.Write("<tr><td align='Left'   width='20' class='cellTopBottomBorder'>Task</td>");
                Response.Write("    <td align='center' width='30' class='cellTopBottomBorder'>&nbsp;Lvl</td>");
                Response.Write("    <td align='Left'   width='110' class='cellTopBottomBorder'>&nbsp;Manufacturer</td>");
                Response.Write("    <td align='center' width='30' class='cellTopBottomBorder'>&nbsp;N/U</td>");
                Response.Write("    <td align='Left'   width='110' class='cellTopBottomBorder'>&nbsp;Rail Car</td>");
                Response.Write("    <td align='right'  width='50' class='cellTopBottomBorder'>&nbsp;Units</td>");
                Response.Write("    <td align='Left'   width='110' class='cellTopBottomBorder'>&nbsp;Origin</td>");
                Response.Write("    <td align='Left'   width='10' class='cellTopBottomBorder'>&nbsp;</td>");
                //Response.Write("    <td align=//Left//   width=//90// class=//cellTopBottomBorder//>&nbsp;Customer</td>");
                Response.Write("</tr>");
                sDate = (rs.Item("WorkDate"));
                sPageBreak = "<h3>&nbsp;</h3>";
              }else{
                sPreviousDate = cStr(cDate(rs.Item("WorkDate")).AddDays(1));
             }

              Response.Write("<tr class='" + rowColor + "'><td align='Left'   width='20'>" + rs.Item("TaskCode") + "</td>");
              Response.Write("    <td align='center' width='30'>" + rs.Item("LevelType") + "</td>");
              Response.Write("    <td align='Left'   width='110'>" + rs.Item("ManufacturerName") + "</td>");
              Response.Write("    <td align='center' width='30'>" + rs.Item("NewUsed") + "</td>");
              Response.Write("    <td align='Left'   width='110'>&nbsp;" + rs.Item("CarTypeCode") + " - " + rs.Item("RailCarNumber") + "</td>");
              Response.Write("    <td align='right'  width='50'>" + rs.Item("Units") + "&nbsp;</td>");
              Response.Write("    <td align='Left'   width='110'>&nbsp;" + rs.Item("OriginName") + "</td>");
              Response.Write("    <td align='Left'   width='10'>&nbsp;</td>");
              //Response.Write("    <td align=//Left//   width=//90//>&nbsp;" + rs.Item("CustomerName") + "</td>");
              Response.Write("</tr>");

              sSumFac   = rs.Item("Name");
              sSumFacID = cInt(rs.Item("FacilityID"));

              lastReadSuccess = rs.Read();

            } //End Loop

            if(sSumDate != ""){
              DoAllSummaries( sSumFac, cStr(sSumFacID), sSumDate);
           }

            while ( cDate(sPreviousDate) <= cDate(stoDateDetail)){
              Response.Write("<tr><td align='left' colspan=8><br><b><font color='green' size=-1>All Facilties: &nbsp;</font>&nbsp;&nbsp;-&nbsp;<b><span class='required'>No Data on: &nbsp;" + sPreviousDate + "</span></b></td></tr>");
              sPreviousDate = cStr(cDate(sPreviousDate).AddDays(1));
            } //End Loop

         }


          //if(cStr(Request["PrintPreview"]).Length > 0){
          if (Request["PrintPreview"] != "0")
          {
            Response.Write("  <tr><td colspan='8' align='right'><a href='" + sPreviewLink + "'>Printer Friendly</a></td></tr>");
          }else{
            Response.Write("  <tr><td colspan='8'>&nbsp;</td></tr>");
          }
          Response.Write("</table>");

        }

        public void DoAllSummaries(string sSumFac,string  sSumFacId,string  sSumDate){

          DataReader rsSum = new DataReader(rsSumNU.SQL);
          rsSum.Open(); 
          if(rsSum.EOF){
          }else{
            Response.Write("<tr><td colspan='8' align='left'><br><b>Load/Unload Summary By Car Type (New/Used)</b><br></td></tr>");
            ShowSummary ("NewUsed", rsSum, sSumFacId, sSumDate);
         }

          rsSum = new DataReader(rsSumLvl.SQL);
          rsSum.Open();
          if(rsSum.EOF){

          }else{
            Response.Write("<tr><td colspan='8' align='left'><br><b>Load/Unload Summary By Level (BI/TRI)</b><br></td></tr>");
            ShowSummary ("Level", rsSum, sSumFacId, sSumDate);
         }

          rsSum = new DataReader(rsSumAll.SQL);
          rsSum.Open();
          if(rsSum.EOF || sSumFac == ""){
          }else{
            Response.Write("<tr><td colspan='8' align='right'><br><b><u>Summary For: " + sSumFac + " - " + sSumDate + "</u></b></td></tr>");
            ShowAllSummary (rsSum, sSumFacId, sSumDate);

         }
        }

        public void ShowSummary (string sBy,DataReader rsSum,string  sFacID,string  sDate){

          string sHTML = "";
          int iRow = 0;
          string sType1 = "";
          string sType2 = "";

          if(UCase(sBy) == "NEWUSED"){
            sBy1 = "New";
                sBy2 = "Used";
                sType1 = "N";
                sType2 = "U";
          }else{
            sBy1 = "BIs";
                sBy2 = "TRIs";
                sType1 = "B";
                sType2 = "T";
         }

          Response.Write("<tr><td colspan='8'>");
          Response.Write("    <table border='0' cellpadding='0' cellspacing='0' width='100%'>");
          Response.Write("      <tr><td colspan='10'>");
          WriteTheHeadings();
          Response.Write("      </td></tr>");

          if (rsSum.EOF ){
            Response.Write("      <tr><td colspan='10'>No (LO) or (UL) tasks.</td></tr>");
          }else{

            rsSum.Requery();
            ClearTheRow();

            while (!rsSum.EOF ){
                rsSum.Read();
               int ThisFacID = cInt(rsSum.Item("FacilityID"));
               DateTime ThisDate  = cDate(rsSum.Item("WorkDate"));

              if(ThisFacID == cInt(sFacID)){
                if(ThisDate == cDate(sDate)){
                  if((Trim(rsSum.Item("ManufacturerName")) != hMan) && (hMan != "") ){
                    WriteTheRow();
                    ClearTheRow();
                    hMan = Trim(rsSum.Item("ManufacturerName"));
                 }

                  if(hMan == ""){
                    hMan = Trim(rsSum.Item("ManufacturerName"));
                 }

                  if(Trim(rsSum.Item("TaskCode")) == "LO"){
                    if(Trim(rsSum.Item("RptType")) == sType1 ){
                      sLRcNNum = cStr(rsSum.Item("RCs"));
                      sLRcNU   = cStr(rsSum.Item("TU"));
                   }
                    if(Trim(rsSum.Item("RptType")) == sType2){
                      sLRcUNum = cStr(rsSum.Item("RCs"));
                      sLRcUU   = cStr(rsSum.Item("TU"));
                    }
                 }

                  if(Trim(rsSum.Item("TaskCode")) == "UL"){
                    if(Trim(rsSum.Item("RptType")) == sType1){
                      sURcNNum = cStr(rsSum.Item("RCs"));
                      sURcNU   = cStr(rsSum.Item("TU"));
                   }
                    if(Trim(rsSum.Item("RptType")) == sType2){
                      sURcUNum = cStr(rsSum.Item("RCs"));
                      sURcUU   = cStr(rsSum.Item("TU"));
                   }
                 }
               }
             }

            } //End Loop

            WriteTheRow();
         }
          Response.Write("    </table>");
          Response.Write("    </td></tr>");
        }

        public void  ShowAllSummary (DataReader rsSum, string sFacID, string sDate){

          string  sHTML = "";
          int iRow = 0;

          Response.Write("<tr><td colspan='8' align='right'>");
          Response.Write("    <table border='0' cellpadding='0' cellspacing='0' width='50%'>");
          //Response.Write("      <tr><td colspan=//4//>&nbsp;</td></tr>");

          if (rsSum.EOF ){
            Response.Write("      <tr><td colspan='4'>No records found.</td></tr>");
          }else{

            rsSum.Requery();

            double nRCs   = 0;
            double nUnits = 0.0;
            bool bWriteEmptyLOLine = true;
            bool bWriteEmptyULLine = true;

            while (!rsSum.EOF ){
                rsSum.Read();
                  int ThisFacID = cInt(rsSum.Item("FacilityID"));
                  DateTime ThisDate;

                  if(cStr(rsSum.Item("WorkDate")) != ""){
                    ThisDate  = cDate(rsSum.Item("WorkDate"));
                  }else{
                    ThisDate  =  cDate(sDate);
                 }

                if( (Trim(rsSum.Item("TaskCode")) == "LO") || (Trim(rsSum.Item("TaskCode")) == "UL") ){
                if(ThisFacID == cInt(sFacID)){
                  if(ThisDate == cDate(sDate)){
                             if(Trim(rsSum.Item("TaskCode")) == "LO"){
                                    Response.Write(" <tr><td Width='40%' align='right' class='lblColorBold'>RCs Loaded:&nbsp;</td>");
                                   bWriteEmptyLOLine = false;
                                 }else{
                                   bWriteEmptyULLine = false;
                                   if(bWriteEmptyLOLine){
                                      Response.Write(" <tr><td Width='40%' align='right' class='lblColorBold'>RCs Loaded:&nbsp;</td>");
                                      Response.Write("     <td Width='20%' align='right'>0</td>");
                                      Response.Write("     <td Width='20%' align='right' class='lblColorBold'>VINs:&nbsp;</td>");
                                      Response.Write("     <td Width='20%' align='right'>0</td>");
                                      Response.Write("   </tr>");
                                    }
                                    Response.Write(" <tr><td Width='40%' align='right' class='lblColorBold'>Unloaded:&nbsp;</td>");
                                }
                                 Response.Write("     <td Width='20%' align='right'>" + rsSum.Item("RCs")        + "</td>");
                                 Response.Write("     <td Width='20%' align='right' class='lblColorBold'>VINs:&nbsp;</td>");
                                 Response.Write("     <td Width='20%' align='right'>" + cStr(rsSum.Item("TU")) + "</td>");
                                 Response.Write("   </tr>");
                                 nRCs   = nRCs + cDbl(rsSum.Item("RCs"));
                                 nUnits = nUnits + cDbl(rsSum.Item("TU"));
                 }
               }
             }

            } //End Loop


            if(bWriteEmptyULLine){
              if(bWriteEmptyLOLine){
                Response.Write(" <tr><td Width='40%' align='right' class='lblColorBold'>RCs Loaded:&nbsp;</td>");
                Response.Write("     <td Width='20%' align='right'>0</td>");
                Response.Write("     <td Width='20%' align='right' class='lblColorBold'>VINs:&nbsp;</td>");
                Response.Write("     <td Width='20%' align='right'>0</td>");
                Response.Write("   </tr>");
             }
              Response.Write(" <tr><td Width='40%' align='right' class='lblColorBold'>Unloaded:&nbsp;</td>");
              Response.Write("     <td Width='20%' align='right'>0</td>");
              Response.Write("     <td Width='20%' align='right' class='lblColorBold'>VINs:&nbsp;</td>");
              Response.Write("     <td Width='20%' align='right'>0</td>");
              Response.Write("   </tr>");
           }

            Response.Write(" <tr class='reportTotalLine'><td Width='40%' align='right' class='cellTopBottomBorder'>RCs:&nbsp;</td>");
            Response.Write("     <td Width='20%' align='right' class='cellTopBottomBorder'>" + cStr(nRCs)   + "</td>");
            Response.Write("     <td Width='20%' align='right' class='cellTopBottomBorder'>UNITs:&nbsp;</td>");
            Response.Write("     <td Width='20%' align='right' class='cellTopBottomBorder'>" + cStr(nUnits) + "</td>");
            Response.Write(" </tr>");
            Response.Write(" <tr><td colspan='4'>&nbsp;</td></tr>");

            rsSum.Requery();
            while (!rsSum.EOF ){
                rsSum.Read();

                  int ThisFacID = cInt(rsSum.Item("FacilityID"));
                  DateTime ThisDate;
                  if(cStr(rsSum.Item("WorkDate")) != ""){
                    ThisDate  = cDate(rsSum.Item("WorkDate"));
                  }else{
                    ThisDate  =  cDate(sDate);
                 }
                  if( (Trim(rsSum.Item("TaskCode")) != "LO") && (Trim(rsSum.Item("TaskCode")) != "UL") ){
                if(ThisFacID == cInt(sFacID)){
                  if(ThisDate == cDate(sDate)){
                     Response.Write(" <tr><td colspan='3' align='right' class='lblColorBold'>" + rsSum.Item("TaskDescription") + "(" + Trim(rsSum.Item("TaskCode")) + "):&nbsp;</td>");
                     Response.Write("     <td Width='20%' align='right'>" + cStr(rsSum.Item("TU")) + "</td>");
                     Response.Write(" </tr>");
                 }
               }
                  }else{
             }

            } //End Loop
         }

          Response.Write("    </table>");
          Response.Write("    </td></tr>");

        }

        public void ClearTheRow(){

           hMan     = "";
           sLRcNNum = "0";
           sLRcNU   = "0";
           sLRcUNum = "0";
           sLRcUU   = "0";
           sSpace   = "<td width='10%'>&nbsp;</td>";
           sURcNNum = "0";
           sURcNU   = "0";
           sURcUNum = "0";
           sURcUU   = "0";

        }

        public void  WriteTheRow(){

              IRow =IRow + 1;
              string rowColor ;

              if(IRow % 2 == 0){
                rowColor = "reportEvenLine";
              }else{
                rowColor = "reportOddLine";
             }

           sHTML =         "<tr class='" + rowColor + "'>";
           sHTML = sHTML + "<td width='10%' align='left'>" + hMan + "</td>";
           sHTML = sHTML + "<td width='10%' align='right'>" + sLRcNNum + "</td>";
           sHTML = sHTML + "<td width='10%' align='right'>" + sLRcNU   + "</td>";
           sHTML = sHTML + "<td width='10%' align='right'>" + sLRcUNum + "</td>";
           sHTML = sHTML + "<td width='10%' align='right'>" + sLRcUU   + "</td>";
           sHTML = sHTML + sSpace;
           sHTML = sHTML + "<td width='10%' align='right'>" + sURcNNum + "</td>";
           sHTML = sHTML + "<td width='10%' align='right'>" + sURcNU   + "</td>";
           sHTML = sHTML + "<td width='10%' align='right'>" + sURcUNum + "</td>";
           sHTML = sHTML + "<td width='10%' align='right'>" + sURcUU   + "</td>";

           sHTML = sHTML + "</tr>";
           Response.Write(sHTML);
        }

        public void WriteTheHeadings(){

          Response.Write("    <table border='0' cellpadding='0' cellspacing='0' width='100%'>");
          Response.Write("      <tr>");
          Response.Write("         <td width='10%' class='cellTopBorder'>Manuf</td>");
          Response.Write("         <td colspan='4' class='cellTopBorder' align='center'>LOAD</td>");
          Response.Write("         <td width='10%'>&nbsp;</td>");
          Response.Write("         <td colspan='4' class='cellTopBorder' align='center'>UNLOAD</td>");
          Response.Write("      </tr>");
          Response.Write("      <tr>");
          Response.Write("         <td width='10%'>&nbsp;</td>");
          Response.Write("         <td colspan='2' class='cellTopBorder' align='center'>" + sBy1 + "</td>");
          Response.Write("         <td colspan='2' class='cellTopBorder' align='center'>" + sBy2 + "</td>");
          Response.Write("         <td width='10%'>&nbsp;</td>");
          Response.Write("         <td colspan='2' class='cellTopBorder' align='center'>" + sBy1 + "</td>");
          Response.Write("         <td colspan='2' class='cellTopBorder' align='center'>" + sBy2 + "</td>");
          Response.Write("      </tr>");
          Response.Write("      <tr>");
          Response.Write("         <td width='10%' align='Center'>&nbsp;</td>");
          Response.Write("         <td width='10%' class='cellBottomBorder' align='Right'>RCs</td>");
          Response.Write("         <td width='10%' class='cellBottomBorder' align='Right'>Units</td>");
          Response.Write("         <td width='10%' class='cellBottomBorder' align='Right'>RCs</td>");
          Response.Write("         <td width='10%' class='cellBottomBorder' align='Right'>Units</td>");
          Response.Write("         <td width='10%' align='Center'>&nbsp;</td>");
          Response.Write("         <td width='10%' class='cellBottomBorder' align='Right'>RCs</td>");
          Response.Write("         <td width='10%' class='cellBottomBorder' align='Right'>Units</td>");
          Response.Write("         <td width='10%' class='cellBottomBorder' align='Right'>RCs</td>");
          Response.Write("         <td width='10%' class='cellBottomBorder' align='Right'>Units</td>");
          Response.Write("      </tr>");
          Response.Write("    </table>");
        }




    }
}