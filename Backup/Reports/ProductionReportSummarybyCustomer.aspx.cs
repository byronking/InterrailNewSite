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
    public partial class ProductionReportSummarybyCustomer : PageBase
    {

        public DataReader rs, rstbl, rstMan;
        public string strSQL, sFrom, sTo, sWorkDates;
        public string sPreviewLink, sPageBreak;
        public string sSelectedShifts,wCustomers;
        public string sselCustomerIDandName;
        public string sselCustomerID;
        public string sselCustomerName;
 
        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin, User, Production");

            sselCustomerIDandName  = Request["CustomerIDandName"];

            string[] temp;

            temp = Split(sselCustomerIDandName, "|");
            sselCustomerID = temp[0];

            temp = Split(sselCustomerIDandName, "|");
            sselCustomerName = temp[1];

            sFrom = Request["From"];
            sTo   = Request["To"];
            sWorkDates = " WHERE (WorkDate Between '" + cStr(sFrom) + "' AND '" + cStr(sTo) + "' ) ";

              Session["LastStartDate"] = sFrom;
              Session["LastEndDate"]   = sTo;


            if(sselCustomerID != null){
              wCustomers = "  AND (FacilityCustomerID =  " + sselCustomerID + " ) ";
            }else{
              wCustomers = "   ";
            }


            sSelectedShifts = Request["SelectedShifts"];

            Session["LastShiftList"] = sSelectedShifts;


            strSQL =  " SELECT TaskCode, SUM(FacilityProductionDetail.Units) AS TU ";
            strSQL +=  "  FROM FacilityProductionDetail INNER JOIN FacilityTasks   ";
            strSQL +=  "       ON FacilityProductionDetail.FacilityID = FacilityTasks.FacilityID ";
            strSQL +=  "       AND FacilityProductionDetail.TaskId = FacilityTasks.TaskId ";
            strSQL +=  "       INNER JOIN Tasks ON FacilityTasks.TaskId = Tasks.Id ";
            strSQL = strSQL + sWorkDates;
            strSQL +=  " AND (FacilityProductionDetail.FacilityID = " + cStr(Session["FacilityID"]) + ") ";
            strSQL +=  "             AND FacilityProductionDetail.ShiftID in (" + sSelectedShifts + ") ";
            strSQL = strSQL + wCustomers;
            strSQL +=  " GROUP BY TaskCode ";


            rs = new DataReader(strSQL);
            rs.Open();

            if(rs.EOF){
              //Response.Write("No data");
            }else{
              strSQL = "";
              strSQL +=  " SELECT WorkDate, ";
              while (! rs.EOF){
                rs.Read();
                strSQL +=  " SUM (CASE TaskCode WHEN '" + rs.Item("TaskCode") + "' Then Units Else 0 END) AS [" + Trim(rs.Item("TaskCode")) + "], ";

              } //End Loop
              strSQL = Left(strSQL, Len(strSQL)-2);

              strSQL +=  " FROM   FacilityProductionDetail ";
              strSQL +=  "       INNER JOIN Facility ON FacilityProductionDetail.FacilityID = Facility.Id ";
              strSQL +=  "       INNER JOIN Tasks ON FacilityProductionDetail.TaskId = Tasks.Id ";
              strSQL = strSQL + sWorkDates;
              strSQL +=  " AND (FacilityProductionDetail.FacilityID = " + cStr(Session["FacilityID"]) + ") ";
              strSQL +=  "             AND FacilityProductionDetail.ShiftID in (" + sSelectedShifts + ") ";
              strSQL = strSQL + wCustomers;
              strSQL +=  " GROUP BY WorkDate ";
              //;
              // Add No days where there is no Production Data;
              strSQL +=  " UNION ";
              strSQL +=  " SELECT WorkDate, ";

              rs.Requery();
              while (! rs.EOF){
                rs.Read();
                strSQL +=  " 0, ";

              } //End Loop
              rs.Requery();

              strSQL = Left(strSQL, Len(strSQL)-2);

              strSQL +=  " FROM   NoProductionData ";
              strSQL = strSQL + sWorkDates;
              strSQL +=  " AND (FacilityID = " + cStr(Session["FacilityID"]) + ") ";

              strSQL +=  "ORDER BY WorkDate ";


              rstbl = new DataReader( strSQL);
              rstbl.Open();

              // Manufacturer Summary;
              string sMSQL = "";   
              sMSQL = sMSQL + " SELECT TC = Case TaskCode When 'LO' Then 'LOAD'                 ";
              sMSQL = sMSQL + "                          When 'UL' Then 'UNLOAD' ELSE 'N/A' End,";
              sMSQL = sMSQL + "        ManufacturerName, SUM(Units) AS TU,  Count(*) As RCs,    ";
              sMSQL = sMSQL + "        NU = Case NewUsed When 'N' Then 'New'                    ";
              sMSQL = sMSQL + "                          When 'U' Then 'Used' ELSE 'N/A' End    ";
              sMSQL = sMSQL + "   FROM FacilityProductionDetail INNER JOIN Tasks                ";
              sMSQL = sMSQL + "        ON FacilityProductionDetail.TaskId = Tasks.Id INNER JOIN ";
              sMSQL = sMSQL + "        IRGManufacturer ON ManufacturerID = IRGManufacturer.ID   ";
              sMSQL = sMSQL + sWorkDates;
              sMSQL = sMSQL + "  AND (TaskCode IN ('LO', 'UL'))                                 ";
              sMSQL = sMSQL + "  AND (FacilityID = " + cStr(Session["FacilityID"]) + ")         ";
              sMSQL = sMSQL + "  AND FacilityProductionDetail.ShiftID in (" + sSelectedShifts + ") ";
              sMSQL = sMSQL + wCustomers;
              sMSQL = sMSQL + " GROUP BY TaskCode, ManufacturerName, NewUsed                    ";
              sMSQL = sMSQL + " ORDER BY TaskCode, ManufacturerName, NewUsed                    ";

              rstMan = new DataReader(sMSQL);
              rstMan.Open();

            sPreviewLink = "ProductionReportSummarybyCustomer.aspx?PrintPreview=0&From=" + sFrom + "&To=" + sTo  + "&SelectedShifts=" + sSelectedShifts + "&CustomerIDandName=" + Server.UrlEncode(sselCustomerIDandName);
            sPageBreak = "<h3>&nbsp;</h3>";

            }
           }

        public string QuestionOrZero(DateTime thisdate){

            string results;
            string strSQL;
            DataReader rstQ;
            double ProductionCount, NoProductionCount;
   
            results = "?";
   
            strSQL = "";
            strSQL +=  " Select count(*) as thecount from  FacilityProductionDetail  WHERE WorkDate = '" + cStr(thisdate) + "' AND FacilityID = " + cStr(Session["FacilityID"]) ;

            rstQ = new DataReader(strSQL);
            rstQ.Open();
  
            ProductionCount = 0;
            if ( !rstQ.EOF ){
                rstQ.Read();
                ProductionCount = cDbl(rstQ.Item("thecount"));
            }
   
            NoProductionCount = 0;
            if ( ProductionCount == 0){
  
                strSQL = "";
                strSQL +=  " Select count(*) AS thecount FROM  NoProductionData  WHERE WorkDate = '" + cStr(thisdate) + "' AND FacilityID = " + cStr(Session["FacilityID"]) ;

                rstQ = new DataReader(strSQL);
                rstQ.Open();
  
                NoProductionCount = 0;
                if (!rstQ.EOF ){
                    rstQ.Read();
                    NoProductionCount = cDbl(rstQ.Item("thecount"));
                }
            }


            if (ProductionCount == 0 && NoProductionCount == 0){
                results = "<span class='required'>?</span>";
            }else{
                results = "0";
            }
            return results;
        }

        public void ShowProductionReportSummarybyCustomer(){

            DataReader rstCustomers;
            string sDateRange;
            string sDate, sTask, iTotalCars, iTotalUnits;
            double TotalUnits = 0.0;
            string sPreviousDate = "";


            if(cDate(sFrom) == cDate(sTo)){
                sDateRange = sFrom;
            }else{
                sDateRange = sFrom + " - " + sTo;
            }

            string sTitle = "Production Reporting for " + sselCustomerName + " && Shifts " + sSelectedShifts + " ";

            if(rs.EOF){
            Response.Write("No data");
            }else{

            if(cStr(Request["PrintPreview"]).Length > 0){
                Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='480' align='center'>");
                SetCellsWidth();
                Response.Write("  <tr><td align='right' colspan='" + cStr(rstbl.FieldCount()) + "'><a href='" + sPreviewLink + "'>Printer Friendly</a></td></tr>");
            }else{
                Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='600' align='center'>");
                SetCellsWidth();
            }

            Response.Write("  <tr><td align='center' colspan='" + cStr(rstbl.FieldCount()) + "'><b>" + sTitle + "</b></td></tr>");
            Response.Write("  <tr><td align='center' colspan='" + cStr(rstbl.FieldCount()) + "'><b>Facility: &nbsp;" + Session["FacilityName"] + "</b></td></tr>");
            Response.Write("  <tr><td align='center' colspan='" + cStr(rstbl.FieldCount()) + "'><b>For: &nbsp;" + sDateRange + "</b></td></tr>");

            if(rstbl.EOF){
                Response.Write("  <tr><td colspan='" + cStr(rstbl.FieldCount()) + "'>No data found for the date range selected.</td></tr>");
            }else{

                Response.Write("  <tr><td align='center' colspan='" + cStr(rstbl.FieldCount()) + "'>&nbsp;</td></tr>");
                Response.Write("<tr>");

                int ifield = 0;
                for(int x=0 ; x < rstbl.FieldCount(); x ++){
                ifield = ifield + 1;
                if ( ifield == 1){
                    Response.Write("<td align='left' class='cellTopBottomBorder'>" + rstbl.FieldName(x) + "</td>");
                }else{
                    Response.Write("<td align='right' class='cellTopBottomBorder'>" + rstbl.FieldName(x) + "</td>");
                }
                }

                Response.Write("</tr>");
                int iRow = 0;
                sPreviousDate = sFrom;
                string lc = ""; 
      
                while (! rstbl.EOF){
                    rstbl.Read();
                iRow = iRow + 1;
                if(iRow % 2 == 0){
                    lc = "reportOddLine";
                }else{
                    lc = "reportEvenLine";
                }

                ifield = 0;
                while (cDate(sPreviousDate) < cDate(rstbl.Item("WorkDate")) ){

                    Response.Write("<tr class='" + lc + "'>");

                    ifield = 0;
                    for (int y = 0; y < rstbl.FieldCount(); y++)
                    {
                        ifield = ifield + 1;
                        if (ifield == 1)
                        {
                            Response.Write("<td align='left'>" + FormatDate(cStr(sPreviousDate)) + "</td>");
                        }
                        else
                        {
                            Response.Write("<td align='right'> " + QuestionOrZero(cDate(sPreviousDate)) + " </td>");
                        }
                    }
        
                    sPreviousDate = cDate(sPreviousDate).AddDays(1).ToString();
                    Response.Write("</tr>");
                    iRow = iRow + 1;

                    if(iRow % 2 == 0){
                        lc = "reportOddLine";
                    }else{
                        lc = "reportEvenLine";
                    }
                } //End Loop
        

                Response.Write("<tr class='" + lc + "'>");
                ifield=0;

                for( int x=0; x < rstbl.FieldCount(); x++){
                    ifield = ifield + 1;
                    if (ifield == 1){
                    Response.Write("<td align='left'>"  + FormatDate(cStr(rstbl.Fields(x))) +  "</td>");
                    }else{
                    Response.Write("<td align='right'>" + cStr(rstbl.Fields(x)) +  "</td>");
                    }
                }

                Response.Write("</tr>");
                sPreviousDate = cDate(rstbl.Item("WorkDate")).AddDays(1).ToString();

                } //End Loop

                while ( cDate(sPreviousDate) <= cDate(sTo)){

                iRow = iRow + 1;
                if(iRow % 2 == 0){
                    lc = "reportOddLine";
                }else{
                    lc = "reportEvenLine";
                }

                Response.Write("<tr class='" + lc + "'>");
                ifield = 0;

                    for( int x = 0; x < rstbl.FieldCount(); x++){
                    ifield = ifield + 1;
                    if ( ifield == 1){
                        Response.Write("<td align='left'>"  + FormatDate(cStr(sPreviousDate))  +  "</td>");
                    }else{
                        Response.Write("<td align='right'> " + QuestionOrZero(cDate(sPreviousDate)) + " </td>");
                    }
                    }

                sPreviousDate = cDate(sPreviousDate).AddDays(1).ToString();
                Response.Write("</tr>");

                } //End Loop

                // Totals;
                Response.Write("<tr class='reportTotalLine'>");
                rs.Requery();

                while (! rs.EOF){
                    rs.Read();
                    TotalUnits += cDbl(rs.Item("TU"));
                } //End Loop

                Response.Write("<td align='left' class='cellTopBottomBorder'>(" + cStr(TotalUnits) + ")&nbsp;</td>");
                rs.Requery();

                while (! rs.EOF){
                    rs.Read();
                    Response.Write("<td align='right' class='cellTopBottomBorder'>" +  cStr(rs.Item("TU")) + "</td>");
                } //End Loop
                Response.Write("</tr>");

            }

            WriteSummary();

            Response.Write("</table>");

            }
        }

        public void SetCellsWidth(){

            Response.Write("  <tr>");
            int ifield = 0;

            for (int x = 0; x < rstbl.FieldCount(); x++ )
            {
                ifield = ifield + 1;
                if (ifield == 1)
                {
                    Response.Write("  <td width='15%'>&nbsp;</td>");
                }
                else
                {
                    Response.Write("  <td width='" + cStr(cInt(85 / (rstbl.FieldCount() - 1))) + "%'>&nbsp;</td>");
                }
            }

            Response.Write("  </tr>");
            Response.Write("  <tr><td colspan='" + cStr(rstbl.FieldCount()) + "'>&nbsp;</td></tr>");
        }

        public void WriteSummary(){

                double nVeh = 0;
                double nRCs = 0;

            if(rstMan.EOF){
                Response.Write("  <tr><td align='right' colspan='" + cStr(rstbl.FieldCount()) + "'>&nbsp;</td></tr>");
                Response.Write("  <tr><td align='center' colspan='" + cStr(rstbl.FieldCount()) + "'>No Load/Unload during this period.</td></tr>");
            }else{

                Response.Write("  <tr><td align='right' colspan='" + cStr(rstbl.FieldCount()) + "'>&nbsp;</td></tr>");
                Response.Write("  <tr><td align='center' colspan='" + cStr(rstbl.FieldCount()) + "'>");
                Response.Write("   <table border='0' cellpadding='0' cellspacing='0' width='60%'>");
                Response.Write("     <tr class='reportOddLine'><td colspan='5' class='cellTopBottomBorder' align='center'>Manufacturer Summary</td></tr>");
                string sTask = "";

                while (! rstMan.EOF){
                    rstMan.Read();
                if(sTask != rstMan.Item("TC")){
                    if(sTask != "" && rstMan.Item("TC") != sTask){
                    Response.Write("     <tr class='reportTotalLine'>");
                    Response.Write("       <td colspan='3' class='cellTopBottomBorder' align='center'>Totals:&nbsp;</td>");
                    Response.Write("       <td class='cellTopBottomBorder' align='right'>" + cStr(FormatNumber(cStr(nVeh),0)) + "</td>");
                    Response.Write("       <td class='cellTopBottomBorder' align='right'>" + cStr(FormatNumber(cStr(nRCs),0)) + "</td>");
                    Response.Write("     </tr>");
                    nVeh = 0;
                    nRCs = 0;
                    }

                    sTask = rstMan.Item("TC");

                    Response.Write("     <tr><td colspan='5' align='center'>&nbsp;</td></tr>");
                    Response.Write("     <tr class='reportOddLine'>");
                    Response.Write("       <td colspan='3' class='cellTopBottomBorder' align='center'>" + sTask + "</td>");
                    Response.Write("       <td class='cellTopBottomBorder' align='right'>VEHICLES</td>");
                    Response.Write("       <td class='cellTopBottomBorder' align='right'>RCs</td>");
                    Response.Write("     </tr>");
                }

                Response.Write("     <tr>");
                Response.Write("       <td colspan='3' align='Left'> " + rstMan.Item("ManufacturerName") + ", " + rstMan.Item("NU") + "</td>");
                Response.Write("       <td align='right'>" + cStr(FormatNumber(rstMan.Item("TU"), 0)) + "</td>");
                Response.Write("       <td align='right'>" + cStr(FormatNumber(rstMan.Item("RCs"), 0)) + "</td>");
                Response.Write("     </tr>");
                nVeh = nVeh + cDbl(rstMan.Item("TU"));
                nRCs = nRCs + cDbl(rstMan.Item("RCs"));

                } //End Loop

                Response.Write("     <tr class='reportTotalLine'>");
                Response.Write("       <td colspan='3' class='cellTopBottomBorder' align='center'>Totals:&nbsp;</td>");
                Response.Write("       <td class='cellTopBottomBorder' align='right'>" + cStr(FormatNumber(cStr(nVeh),0)) + "</td>");
                Response.Write("       <td class='cellTopBottomBorder' align='right'>" + cStr(FormatNumber(cStr(nRCs),0)) + "</td>");
                Response.Write("     </tr>");

                Response.Write("   </table>");
                Response.Write("  </td></tr>");
            }

            if(cStr(Request["PrintPreview"]).Length > 0){
                Response.Write("  <tr><td align='right' colspan='" + cStr(rstbl.FieldCount()) + "'><a href='" + sPreviewLink + "'>Printer Friendly</a></td></tr>");
            }

        }


    }
}