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
    public partial class RebillDetailReport : PageBase
    {

        public DataReader rs;
        public DataReader UnitRS;
        public DataReader MatRS;
        public DataReader rsAttachments;
        
        public int rs_numRows = 0;
        public int UnitRS_numRows;
        public int MatRS_numRows;

        public string sselFacilities, sfromDateDetail, stoDateDetail, sselTasks, sselCustomers, sRptType;
        public string sselSubTasks,sRebillType,wRebilled,wFacilities,wDateRange,wTasks,wSubTasks,wCustomers;

        public string sPageBreak = "";

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin, User");


            sselFacilities = cStr(Request["selFacilities"]);
            sfromDateDetail = cStr(Request["fromDateDetail"]);
            stoDateDetail = cStr(Request["toDateDetail"]);
            sselTasks = cStr(Request["selTasks"]);
            sselSubTasks = cStr(Request["selSubTasks"]);
            sselCustomers = cStr(Request["selCustomers"]);
            sRptType = cStr(Request["rptType"]);
            sRebillType = cStr(Request["RebillType"]);

            Session["LastStartDate"] = sfromDateDetail;
            Session["LastEndDate"] = stoDateDetail;

            if (sRebillType == "Rebilled")
            {
                wRebilled = " AND (RebillDetail.Rebilled != 0) ";
            }
            if (sRebillType == "NotRebilled")
            {
                wRebilled = " AND (RebillDetail.Rebilled = 0) ";
            }
            if (sRebillType == "RebilledBoth")
            {
                wRebilled = "  ";
            }

            if (sselFacilities != "")
            {
                wFacilities = "  AND (RebillDetail.FacilityID IN  (" + sselFacilities + ") ) ";
            }
            else
            {
                wFacilities = "  AND (RebillDetail.FacilityID IN  (" + Session["FacilityID"] + ") ) ";
            }

            wDateRange = " AND (WorkDate Between '" + sfromDateDetail + "' AND '" + stoDateDetail + "') ";

            if (sselTasks != "")
            {
                wTasks = "  AND ( FacilityTasks.TaskId IN  (" + sselTasks + ") ) ";
            }
            else
            {
                wTasks = "   ";
            }

            if (sselSubTasks != "")
            {
                wSubTasks = "  AND ( RebillDetail.RebillSubTasksID IN (" + sselSubTasks + ") ) ";
            }
            else
            {
                wSubTasks = "   ";
            }

            if (sselCustomers != "")
            {
                wCustomers = "  AND (FacilityCustomerID IN  (" + sselCustomers + ") ) ";
            }
            else
            {
                wCustomers = "   ";
            }

            //This query does !have a server behavior ---;
            string rsSource = " ";
            rsSource += " SELECT  WorkDate, WDesc=IsNull(WorkDescription, 'N/A'), TotalHours, Rebilled, CustomerCode,";
            rsSource += "         ContactName, MaterialCosts=IsNull(MaterialCosts,0), Vendors, InvoiceNumber, ";
            rsSource += "         TaskCode, TaskDescription, Description, FacilityTasks.TaskId, TotalUnits,";
            rsSource += "         FacilityCustomerId, RebillDetail.FacilityID, Name, CustomerName,         ";
            rsSource += "         ISNULL((SELECT  TOP 1 RebillRate FROM RebillSubTaskRates                ";
            rsSource += "         WHERE RebillDetail.WorkDate BETWEEN EffectiveDate AND ExpirationDate     ";
            rsSource += "           AND RebillSubTaskRates.RebillSubTasksID = RebillDetail.RebillSubTasksID";
            rsSource += "         ORDER BY ExpirationDate DESC), 0) AS Rate, RebillDetail.ID,              ";
            rsSource += "         RebillSubTasks.HoursOrUnits                                              ";
            rsSource += " FROM    RebillDetail                                                             ";
            rsSource += "         INNER JOIN RebillSubTasks ON RebillSubTasksId = RebillSubTasks.Id        ";
            rsSource += "         INNER JOIN Facility ON RebillDetail.FacilityID = Facility.Id             ";
            rsSource += "         INNER JOIN FacilityTasks ON Facility.Id = FacilityTasks.FacilityID       ";
            rsSource += "         INNER JOIN Tasks ON FacilityTasks.TaskId = Tasks.Id                      ";
            rsSource += "         AND dbo.RebillSubTasks.TaskID = Tasks.Id                                 ";
            rsSource += "         INNER JOIN FacilityCustomer ON Facility.Id = FacilityCustomer.FacilityId ";
            rsSource += "         AND RebillSubTasks.FacilityCustomerId = FacilityCustomer.Id              ";
            rsSource += " WHERE  (TotalHours>0)  AND  (((MaterialCosts = 0) OR (MaterialCosts is  null)) AND TotalUnits = 0) ";
            rsSource += wDateRange + wFacilities + wTasks + wCustomers + wSubTasks + wRebilled;
            rsSource += " Order By CustomerName, Description, WorkDate, Name                               ";

            rs = new DataReader(rsSource);
            rs.Open();

            if (sselFacilities != "")
            {
                wFacilities = "  AND (RebillDetail.FacilityID IN  (" + sselFacilities + ") ) ";
            }
            else
            {
                wFacilities = "  AND (RebillDetail.FacilityID IN  (" + Session["FacilityID"] + ") ) ";
            }

            wDateRange = " AND (WorkDate Between '" + sfromDateDetail + "' AND '" + stoDateDetail + "') ";

            if (sselTasks != "")
            {
                wTasks = "  AND ( FacilityTasks.TaskId IN  (" + sselTasks + ") ) ";
            }
            else
            {
                wTasks = "   ";
            }

            if (sselSubTasks != "")
            {
                wSubTasks = "  AND ( RebillDetail.RebillSubTasksID IN (" + sselSubTasks + ") ) ";
            }
            else
            {
                wSubTasks = "   ";
            }

            if (sselCustomers != "")
            {
                wCustomers = "  AND (FacilityCustomerID IN  (" + sselCustomers + ") ) ";
            }
            else
            {
                wCustomers = "   ";
            }

            //This query does !have a server behavior ---;
            string UnitRSSource = " ";
            UnitRSSource += " SELECT  WorkDate, WDesc=IsNull(WorkDescription, 'N/A'), TotalHours, Rebilled, CustomerCode,";
            UnitRSSource += "         ContactName, MaterialCosts=IsNull(MaterialCosts,0), Vendors, InvoiceNumber, ";
            UnitRSSource += "         TaskCode, TaskDescription, Description, FacilityTasks.TaskId, TotalUnits,";
            UnitRSSource += "         FacilityCustomerId, RebillDetail.FacilityID, Name, CustomerName,         ";
            UnitRSSource += "         ISNULL((SELECT   TOP 1 RebillRate FROM RebillSubTaskRates                ";
            UnitRSSource += "         WHERE RebillDetail.WorkDate BETWEEN EffectiveDate AND ExpirationDate     ";
            UnitRSSource += "           AND RebillSubTaskRates.RebillSubTasksID = RebillDetail.RebillSubTasksID";
            UnitRSSource += "         ORDER BY ExpirationDate DESC), 0) AS Rate,                               ";
            UnitRSSource += "         RebillDetail.ID,                                                         ";
            UnitRSSource += "         RebillSubTasks.HoursOrUnits                                              ";
            UnitRSSource += " FROM    RebillDetail                                                             ";
            UnitRSSource += "         INNER JOIN RebillSubTasks ON RebillSubTasksId = RebillSubTasks.Id        ";
            UnitRSSource += "         INNER JOIN Facility ON RebillDetail.FacilityID = Facility.Id             ";
            UnitRSSource += "         INNER JOIN FacilityTasks ON Facility.Id = FacilityTasks.FacilityID       ";
            UnitRSSource += "         INNER JOIN Tasks ON FacilityTasks.TaskId = Tasks.Id                      ";
            UnitRSSource += "         AND dbo.RebillSubTasks.TaskID = Tasks.Id                                 ";
            UnitRSSource += "         INNER JOIN FacilityCustomer ON Facility.Id = FacilityCustomer.FacilityId ";
            UnitRSSource += "         AND RebillSubTasks.FacilityCustomerId = FacilityCustomer.Id              ";
            UnitRSSource += " WHERE  ( TotalUnits > 0 )           ";
            UnitRSSource += wDateRange + wFacilities + wTasks + wCustomers + wSubTasks + wRebilled;
            UnitRSSource += " Order By CustomerName, Description, WorkDate, Name                               ";

            UnitRS = new DataReader(UnitRSSource);
            UnitRS.Open();
            UnitRS_numRows = 0;


            if (sselFacilities != "")
            {
                wFacilities = "  AND (RebillDetail.FacilityID IN  (" + sselFacilities + ") ) ";
            }
            else
            {
                wFacilities = "  AND (RebillDetail.FacilityID IN  (" + Session["FacilityID"] + ") ) ";
            }

            wDateRange = " AND (WorkDate Between '" + sfromDateDetail + "' AND '" + stoDateDetail + "') ";

            if (sselTasks != "")
            {
                wTasks = "  AND ( FacilityTasks.TaskId IN  (" + sselTasks + ") ) ";
            }
            else
            {
                wTasks = "   ";
            }

            if (sselSubTasks != "")
            {
                wSubTasks = "  AND ( RebillDetail.RebillSubTasksID IN (" + sselSubTasks + ") ) ";
            }
            else
            {
                wSubTasks = "   ";
            }

            if (sselCustomers != "")
            {
                wCustomers = "  AND (FacilityCustomerID IN  (" + sselCustomers + ") ) ";
            }
            else
            {
                wCustomers = "   ";
            }

            //This query does !have a server behavior ---;
            string MatRSSource = " ";
            MatRSSource += "  SELECT  WorkDate, WDesc=IsNull(WorkDescription, 'N/A'), TotalHours, CustomerCode,";
            MatRSSource += "          ContactName, MaterialCosts=IsNull(MaterialCosts,0), Vendors, InvoiceNumber, ";
            MatRSSource += "          TaskCode, TaskDescription, Description, FacilityTasks.TaskId, TotalUnits,";
            MatRSSource += "          FacilityCustomerId, RebillDetail.FacilityID, Name, CustomerName,         ";
            MatRSSource += "          ISNULL((SELECT   TOP 1 RebillRate FROM RebillSubTaskRates                ";
            MatRSSource += "          WHERE RebillDetail.WorkDate BETWEEN EffectiveDate AND ExpirationDate     ";
            MatRSSource += "            AND RebillSubTaskRates.RebillSubTasksID = RebillDetail.RebillSubTasksID";
            MatRSSource += "          ORDER BY ExpirationDate DESC), 0) AS Rate,                               ";
            MatRSSource += "          RebillDetail.ID,                                                         ";
            MatRSSource += "          RebillSubTasks.HoursOrUnits                                              ";
            MatRSSource += "  FROM    RebillDetail                                                             ";
            MatRSSource += "          INNER JOIN RebillSubTasks ON RebillSubTasksId = RebillSubTasks.Id        ";
            MatRSSource += "          INNER JOIN Facility ON RebillDetail.FacilityID = Facility.Id             ";
            MatRSSource += "          INNER JOIN FacilityTasks ON Facility.Id = FacilityTasks.FacilityID       ";
            MatRSSource += "          INNER JOIN Tasks ON FacilityTasks.TaskId = Tasks.Id                      ";
            MatRSSource += "          AND dbo.RebillSubTasks.TaskID = Tasks.Id                                 ";
            MatRSSource += "          INNER JOIN FacilityCustomer ON Facility.Id = FacilityCustomer.FacilityId ";
            MatRSSource += "          AND RebillSubTasks.FacilityCustomerId = FacilityCustomer.Id              ";
            MatRSSource += "  WHERE  (((MaterialCosts > 0) AND (MaterialCosts IS NOT Null)) ) ";
            MatRSSource += wDateRange + wFacilities + wTasks + wCustomers + wSubTasks + wRebilled;
            MatRSSource += "  Order By CustomerName, Description, WorkDate, Name                               ";


            MatRS = new DataReader(MatRSSource);

            MatRS.Open();
            MatRS_numRows = 0;

        }

        public void PrintMaterialTaskTotals (string lastTaskDescriptionPR,string  subtotalHoursPR,string  subtotalAmountPR,string  subtotalMCPR){

              Response.Write("<tr class='reportTotalLine'>");
              Response.Write("    <td align='right'  class='cellTopBottomBorder' width='7%'>&nbsp;</td>");
              Response.Write("    <td align='Left'  colspan='2'  class='cellTopBottomBorder' width='33%'>Total for " + lastTaskDescriptionPR  + "</td>");
              Response.Write("    <td align='right'  class='cellTopBottomBorder' width='8%'>" + cStr(FormatNumber(subtotalHoursPR, 2, 0)) + "&nbsp;</td>");
              Response.Write("    <td align='right'  class='cellTopBottomBorder' width='4%'>&nbsp;&nbsp;</td>");
              Response.Write("    <td align='right'  class='cellTopBottomBorder' width='10%'>" + cStr(FormatNumber(subtotalAmountPR, 2, 0)) + "&nbsp;</td>");
              Response.Write("    <td align='right'  class='cellTopBottomBorder' colspan='1' >" + cStr(FormatNumber(subtotalMCPR, 2, 0)) + "&nbsp;</td>");
              Response.Write("    <td align='Left'   class='cellTopBottomBorder' colspan='3'  width='30%'>&nbsp;&nbsp;(" + cStr(FormatNumber(cStr(cDbl(subtotalAmountPR) + cDbl(subtotalMCPR)), 2, 0)) + ")</td>");
              Response.Write("</tr>");
              Response.Write("<tr><td colspan='10'>&nbsp;</td></tr>");

        }

        public void ShowRebillMaterialReport(){

          string sFrom, sTo;
          //Dim  subtotalHours;
          //Dim  subtotalAmount, subtotalMC;

          double subtotalHours = 0;
          double subtotalAmount = 0;
          double subtotalMC = 0;

           //  sPreviewLink = "javascript:document.form1.submit();";
          string sPreviewLink = "RebillDetailReport.aspx?PrintPreview=0&selFacilities=" + sselFacilities + "&fromDateDetail=" + sfromDateDetail + "&toDateDetail=" + stoDateDetail + "&selTasks=" + sselTasks + "&selSubTasks=" + sselSubTasks + "&selCustomers=" + sselCustomers + "&rptType=" + sRptType + "";

          string sTitle = sRptType + " Rebill Report<br>" + sfromDateDetail + " - " + stoDateDetail;

          if (cStr(Request["PrintPreview"]).Length > 0) {
            Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='480' align='center'>");
          }else{
            Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='600' align='center'>");
          }

          Response.Write("  <tr><td colspan='8'>&nbsp;</td></tr>");

          if(MatRS.EOF){
            // Response.Write("  <tr><td colspan='4'><b>" & sTitle & "</b></td></tr>"
            Response.Write ("<tr><td colspan='4'>No Rebill Material Cost found.</td></tr>");
          }else{

            string sFacility   = "";
            string sDate = "";
            string sPageBreak = "";
            int IRow = 0;
            string rowColor = "";
            string lastTaskDescription = "";

            while(! MatRS.EOF){

               MatRS.Read();

               lastTaskDescription  = MatRS.Item("Description");

              IRow = IRow + 1;
              if (IRow % 2 == 0) {
                rowColor = "reportOddLine";
              }else{
                rowColor = "reportEvenLine";
              }

              if (sFacility != MatRS.Item("FacilityID")) {

                if (cInt(Request["PrintPreview"]) == 0 ){
                  Response.Write("  <tr><td colspan='10' align='center'><b>" + sPageBreak + sTitle + "</b></td></tr>");
                }

                Response.Write("<tr><td align='left'   colspan=10><br><b><font color='green' size=-1>" + MatRS.Item("Name") + " - Material Costs</td></tr>");
                Response.Write("<tr><td align='Left'   width='7%' class='cellTopBottomBorder'>Date&nbsp;</td>");
                Response.Write("    <td align='Left'   width='25%' class='cellTopBottomBorder'>SubTask</td>");
                Response.Write("    <td align='Left'   width='8%' class='cellTopBottomBorder'>Att.</td>");
                Response.Write("    <td align='right'   width='8%' class='cellTopBottomBorder'>Hours&nbsp;</td>");
	            Response.Write("    <td align='right'   width='4%' class='cellTopBottomBorder'>Rate&nbsp;</td>");
                Response.Write("    <td align='right'  colspan='1' class='cellTopBottomBorder'>Amount&nbsp;</td>");
                Response.Write("    <td align='right'  colspan='1' class='cellTopBottomBorder'>Material Costs&nbsp;&nbsp;</td>");
                Response.Write("    <td align='left'   colspan='3' width='30%' class='cellTopBottomBorder'>&nbsp;Customer<br>&nbsp;Contact</td>");
                Response.Write("</tr>");
                sFacility = MatRS.Item("FacilityID");
                sPageBreak = "<h3>&nbsp;</h3>";

              }

              Response.Write("<tr class='" + rowColor + "'>");
              Response.Write("    <td align='right'  width='7%'>" + FormatDate(MatRS.Item("WorkDate")) + "&nbsp;</td>");
              //Response.Write("    <td align='Left'   width='33%'>" & MatRS.Item("Description")  & " (" & MatRS("TaskCode") & ")</td>");
              Response.Write("    <td align='Left'   width='33%'>" + MatRS.Item("Vendors")  + "</td>");

              rsAttachments = new DataReader("SELECT path, title FROM RebillAttachments WHERE RebillDetailId = "  + cStr(MatRS.Item("ID")) );
              rsAttachments.Open();
              string strAttList = "";
              int attcount = 0;

              while(! rsAttachments.EOF){
                 rsAttachments.Read();
                 attcount = attcount + 1;
                 if (attcount > 1 ){
                    strAttList = strAttList + "<BR>";
                 }
                 strAttList = strAttList + "<a href='../Rebilling/" + cStr(rsAttachments.Item("Path")) + "' target=_blank>" + cStr(MatRS.Item("ID")) + "</a>";
              }

                Response.Write("    <td align='Left'   width='8%' >" + strAttList + "</td>");
                Response.Write("    <td align='right'  width='8%'>" + cStr(FormatNumber(MatRS.Item("TotalHours"), 2, 0)) + "&nbsp;</td>");
                string sAmount = cStr(cDbl(MatRS.Item("Rate")) * cDbl(MatRS.Item("TotalHours")));
                subtotalHours = subtotalHours + cDbl(MatRS.Item("TotalHours"));
                Response.Write("    <td align='right'  width='4%'>&nbsp;" + MatRS.Item("Rate") + "&nbsp;</td>");


               string sMatCosts = cStr(MatRS.Item("MaterialCosts"));

              subtotalAmount = subtotalAmount + cDbl(sAmount);
              subtotalMC = subtotalMC + cDbl(sMatCosts);

              Response.Write("    <td align='right'  colspan='1' >" + cStr(FormatNumber(sAmount, 2, 0)) + "&nbsp;</td>");
              Response.Write("    <td align='right'  colspan='1' >" + cStr(FormatNumber(sMatCosts, 2, 0)) + "&nbsp;</td>");
              Response.Write("    <td align='Left'   colspan='3'  width='30%'>&nbsp;" + MatRS.Item("CustomerCode") + "-&nbsp;" + MatRS.Item("ContactName") + "</td> ");
              Response.Write("</tr>");

              if (MatRS.EOF ){
                 PrintMaterialTaskTotals(lastTaskDescription, cStr(subtotalHours), cStr(subtotalAmount), cStr(subtotalMC));
                 subtotalHours = 0;
                 subtotalAmount = 0;
                 subtotalMC = 0;
              }else{
                 if (lastTaskDescription != MatRS.Item("Description") ){

                    PrintMaterialTaskTotals(lastTaskDescription, cStr(subtotalHours), cStr(subtotalAmount), cStr(subtotalMC));
                    subtotalHours = 0;
                    subtotalAmount = 0;
                    subtotalMC = 0;

                    if (sRptType == "Detail")
                    {
                        if(cInt(Request["PrintPreview"]) == 0) {
                            Response.Write("  <tr><td colspan='8' align='center'><b>" + sPageBreak + sTitle + "</b></td></tr>");
                            Response.Write("<tr><td align='left'   colspan=10><br><b><font color='green' size=-1>" + MatRS.Item("Name") + " - Material Costs</td></tr>");
                            Response.Write("<tr><td align='Left'   width='7%' class='cellTopBottomBorder'>Date&nbsp;</td>");
                            Response.Write("    <td align='Left'   width='25%' class='cellTopBottomBorder'>SubTask</td>");
                            Response.Write("    <td align='Left'   width='8%' class='cellTopBottomBorder'>Att.</td>");
                            Response.Write("    <td align='right'   width='8%' class='cellTopBottomBorder'>Hours&nbsp;</td>");
	                        Response.Write("    <td align='right'   width='4%' class='cellTopBottomBorder'>Rate&nbsp;</td>");
                            Response.Write("    <td align='right'  colspan='1' class='cellTopBottomBorder'>Amount&nbsp;</td>");
                            Response.Write("    <td align='right'  colspan='1' class='cellTopBottomBorder'>Material Costs&nbsp;&nbsp;</td>");
                            Response.Write("    <td align='left'   colspan='3' width='30%' class='cellTopBottomBorder'>&nbsp;Customer<br>&nbsp;Contact</td>");
                            Response.Write("</tr>");
                            sPageBreak = "<h3>&nbsp;</h3>";
                        }
                    }
                 }
              }

              if( ! MatRS.EOF){
                 lastTaskDescription = MatRS.Item("Description");
              }

            } //Loop
          }

          if(cStr(Request["PrintPreview"]).Length > 0) {
            Response.Write("  <tr><td colspan='8' align='right'><a href='" + sPreviewLink + "'>Printer Friendly</a></td></tr>");
          }else{
            Response.Write("  <tr><td colspan='8'>&nbsp;</td></tr>");
          }
          Response.Write("</table>");

        }

        public void PrintTaskTotals (string lastTaskDescription, string  subtotalHours,string  subtotalAmount,string  subtotalMC){

              Response.Write("<tr class='reportTotalLine'>");
              Response.Write("    <td align='right'  class='cellTopBottomBorder' width='7%'>&nbsp;</td>");
              Response.Write("    <td align='Left'   class='cellTopBottomBorder' width='33%'>Total for " + lastTaskDescription  + "</td>");
              Response.Write("    <td align='right'  class='cellTopBottomBorder' width='8%'>" + cStr(FormatNumber(subtotalHours, 2, 0)) + "&nbsp;&nbsp;&nbsp;</td>");
              Response.Write("    <td align='right'  class='cellTopBottomBorder' width='4%'>&nbsp;&nbsp;</td>");
              Response.Write("    <td align='right'  class='cellTopBottomBorder' width='10%'>" + cStr(FormatNumber(subtotalAmount, 2, 0)) + "&nbsp;&nbsp;</td>");
              Response.Write("    <td align='right'  class='cellTopBottomBorder' width='8%'>&nbsp;&nbsp;</td>");
              //      Response.Write("    <td align=//right//  class=//cellTopBottomBorder// width=//8%//>" + cStr(FormatNumber(subtotalMC, 2, 0)) + "&nbsp;&nbsp;</td>");
              Response.Write("    <td align='Left'   class='cellTopBottomBorder' colspan='3'  width='30%'>&nbsp;&nbsp;(" + cStr(FormatNumber(subtotalAmount, 2, 0)) + ")</td>");
              Response.Write("</tr>");
              Response.Write("<tr><td colspan='8'>&nbsp;</td></tr>");

        }

        public void  ShowRebillDetailReport(){

          string sFrom, sTo;
          //Dim  subtotalHours;
          //Dim  subtotalAmount, subtotalMC;

          double subtotalHours = 0;
          double subtotalAmount = 0;
          double subtotalMC = 0;

          //  sPreviewLink = "javascript:document.form1.submit();";
          string sPreviewLink = "RebillDetailReport.aspx?PrintPreview=0&selFacilities=" + sselFacilities + "&fromDateDetail=" + sfromDateDetail + "&toDateDetail=" + stoDateDetail + "&selTasks=" + sselTasks + "&selSubTasks=" + sselSubTasks + "&selCustomers=" + sselCustomers + "&rptType=" + sRptType + "" ;

          string sTitle = sRptType + " Rebill Report<br>" + sfromDateDetail + " - " + stoDateDetail;

          if(System.Convert.ToString(Request["PrintPreview"]).Length > 0 ){
            Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='480' align='center'>");
          }else{
            Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='600' align='center'>");
         }

          Response.Write("  <tr><td colspan='8'>&nbsp;</td></tr>");
          if(System.Convert.ToString(Request["PrintPreview"]).Length > 0){
            Response.Write("  <tr><td colspan='8' align='right'><a href='" + sPreviewLink + "'>Printer Friendly</a></td></tr>");
            Response.Write("  <tr><td colspan='8' align='center'><b>" + sTitle + "</b></td></tr>");
          }else{
            Response.Write("  <tr><td colspan='8'>&nbsp;</td></tr>");
         }

          if(rs.EOF){
            Response.Write("  <tr><td colspan='4'>&nbsp;</td></tr>");
            Response.Write("  <tr><td colspan='4'>No Hourly records found.</td></tr>");
          }else{

            string sFacility = "";
            string sDate = "";
            string sPageBreak = "";
            int IRow = 0;
            string rowColor;
            string sAmount;
            string lastTaskDescription = null;
            string sMatCosts = "";

            while (true){
                if (IRow == 0)
                {
                    rs.Read();
                    lastTaskDescription = rs.Item("Description");
                }

              IRow = IRow + 1;
              if(IRow % 2 == 0){
                rowColor = "reportOddLine";
              }else{
                rowColor = "reportEvenLine";
             }

              if(sFacility != rs.Item("FacilityID")){
                if((System.Convert.ToString(Request["PrintPreview"]) == "0") ){
                  Response.Write("  <tr><td colspan='8' align='center'><b>" + sPageBreak + sTitle + "</b></td></tr>");
               }

                Response.Write("<tr><td align='left'   colspan=8><br><b><font color='green' size=-1>" + rs.Item("Name") + " - Labor Costs</td></tr>");
                Response.Write("<tr><td align='Left'   width='7%' class='cellTopBottomBorder'>Date&nbsp;</td>");
                Response.Write("    <td align='Left'   width='33%' class='cellTopBottomBorder'>SubTask</td>");
                Response.Write("    <td align='right'   width='8%' class='cellTopBottomBorder'>Hours&nbsp;</td>");
                Response.Write("    <td align='right'   width='4%' class='cellTopBottomBorder'>&nbsp;Rate&nbsp;</td>");
                Response.Write("    <td align='right' width='10%' class='cellTopBottomBorder'>Amount&nbsp;&nbsp;</td>");
                Response.Write("    <td align='right' width='8%' class='cellTopBottomBorder'>Rebilled</td>");
                Response.Write("    <td align='left'   colspan='3' width='30%' class='cellTopBottomBorder'>&nbsp;Customer<br>&nbsp;Contact</td>");
                Response.Write("</tr>");
                sFacility = rs.Item("FacilityID");
                sPageBreak = "<h3>&nbsp;</h3>";
             }

              Response.Write("<tr class='" + rowColor + "'>");
              Response.Write("    <td align='right'  width='7%'>" + FormatDate(rs.Item("WorkDate")) + "&nbsp;</td>");
              Response.Write("    <td align='Left'   width='33%'>" + rs.Item("Description")  + " (" + rs.Item("TaskCode") + ")</td>");
              if(rs.Item("HoursOrUnits") == "H"){
                 Response.Write("    <td align='right'  width='8%'>" + cStr(FormatNumber(rs.Item("TotalHours"), 2, 0)) + "&nbsp;&nbsp;&nbsp;</td>");
                 sAmount = cStr(cDbl(rs.Item("Rate")) * cDbl(rs.Item("TotalHours")));
                 subtotalHours = subtotalHours + cDbl(rs.Item("TotalHours"));
              }else{
                 Response.Write("    <td align='right'  width='8%'>" + cStr(FormatNumber(rs.Item("TotalUnits"), 0, 0)) + "&nbsp;&nbsp;&nbsp;</td>");
                 sAmount = cStr(cDbl(rs.Item("Rate")) * cDbl(rs.Item("TotalUnits")));
                 subtotalHours = subtotalHours + cDbl(rs.Item("TotalUnits"));
             }

              sMatCosts = cStr(rs.Item("MaterialCosts"));

              Response.Write("    <td align='right'  width='4%'>&nbsp;" + rs.Item("Rate") + "&nbsp;</td>");
              subtotalAmount = subtotalAmount + cDbl(sAmount);
              subtotalMC = cDbl(subtotalMC) + cDbl(sMatCosts);

              Response.Write("    <td align='right'  width='10%'>" + cStr(FormatNumber(sAmount, 2, 0)) + "&nbsp;&nbsp;</td>");
              //Response.Write("    <td align=//right//  width=//8%//>" + cStr(FormatNumber(sMatCosts, 2, 0)) + "&nbsp;&nbsp;</td>");
              Response.Write("    <td align='right'  width='8%'>");
                 if (rs.Item("Rebilled") == "True"){
                   Response.Write("<img src='../images/check.gif' />");
                }
              Response.Write(" </td>");
              Response.Write("    <td align='Left'   colspan='3'  width='30%'>&nbsp;" + rs.Item("CustomerCode") + "-&nbsp;" + rs.Item("ContactName") + "</td> ");
              Response.Write("</tr>");

              if((Len(rs.Item("WDesc")) > 0) && (sRptType == "Detail")){
                Response.Write("<tr class='" + rowColor + "'><td colspan='9'>&nbsp;</td></tr>");
                Response.Write("<tr class='" + rowColor + "'>");
                Response.Write("    <td align='right'  valign='top' width='7%' >&nbsp;</td>");
                Response.Write("    <td align='Left'   colspan='8'  width='75%'><b><u>Work Description:</u>&nbsp;&nbsp;</b>" + rs.Item("WDesc") + "</td>");
                Response.Write("</tr>");
             }


              if(sRptType == "Detail"){


                string sSQL = "    SELECT  LastName, FirstName, HoursWorked, RebillDetailID                ";
                sSQL += "            FROM  EmployeeTaskWorked                                              ";
                sSQL += "                  INNER JOIN RebillDetail ON RebillDetailID = RebillDetail.Id     ";
                sSQL += "                  INNER JOIN Employee ON EmployeeId = Employee.Id                 ";
                sSQL += "                  WHERE EmployeeId = " + rs.Item("ID");

                DataReader rsEmp = new DataReader(sSQL);
                rsEmp.Open();

                if (! rsEmp.EOF){

                  Response.Write("<tr class='" + rowColor + "'><td colspan='9'>&nbsp;</td></tr>");
                  Response.Write("<tr class='" + rowColor + "'>");
                  Response.Write("    <td align='right'  valign='top' width='7%' ><b>&nbsp;</b></td>");
                  Response.Write("    <td align='Left'   colspan='2' class='cellTopBottomBorder'>Employee Name</td>");
                  Response.Write("    <td align='right'   colspan='2'class='cellTopBottomBorder'>&nbsp;Hours Worked</td>");
                  Response.Write("    <td align='left'   colspan='4' width='30%'>&nbsp;</td>");
                  Response.Write("</tr>");

                  while (! rsEmp.EOF){
                    rsEmp.Read();

                    Response.Write("<tr class='" + rowColor + "'>");
                    Response.Write("    <td align='right'  valign='top' width='7%' ><b>&nbsp;</b></td>");
                    Response.Write("    <td align='Left'   colspan='2'>" + rsEmp.Item("LastName") + ", " + rsEmp.Item("FirstName") + "</td>");
                    Response.Write("    <td align='right'   colspan='2'>&nbsp;" + cStr(FormatNumber(rsEmp.Item("HoursWorked"), 2, 0)) + "</td>");
                    Response.Write("    <td align='left'   colspan='4' width='30%'>&nbsp;</td>");
                    Response.Write("</tr>");

                  } //End Loop

                  Response.Write("<tr class='" + rowColor + "'><td colspan='9'>&nbsp;</td></tr>");
               }
              }

              bool morerecords = rs.Read();
              if (!morerecords)
              {
                 PrintTaskTotals(cStr(lastTaskDescription), cStr(subtotalHours), cStr(subtotalAmount), cStr(subtotalMC));
                 subtotalHours = 0;
                 subtotalAmount = 0;
                 subtotalMC = 0;
              }else{
                 if (lastTaskDescription != rs.Item("Description")){
                    PrintTaskTotals (cStr(lastTaskDescription), cStr(subtotalHours), cStr(subtotalAmount), cStr(subtotalMC));
                    subtotalHours = 0;
                    subtotalAmount = 0;
                    subtotalMC = 0;

                    if ( sRptType == "Detail"){
                        if((System.Convert.ToString(Request["PrintPreview"]) == "0") ){
                          Response.Write("  <tr><td colspan='8' align='center'><b>" + sPageBreak + sTitle + "</b></td></tr>");
                          Response.Write("<tr><td align='left'   colspan=8><br><b><font color='green' size=-1>" + rs.Item("Name") + "</td></tr>");
                          Response.Write("<tr><td align='Left'   width='7%' class='cellTopBottomBorder'>Date&nbsp;</td>");
                          Response.Write("    <td align='Left'   width='33%' class='cellTopBottomBorder'>SubTask</td>");
                          Response.Write("    <td align='right'   width='8%' class='cellTopBottomBorder'>Hours/Units&nbsp;&nbsp;&nbsp;</td>");
                          Response.Write("    <td align='right'   width='4%' class='cellTopBottomBorder'>&nbsp;Rate&nbsp;</td>");
                          Response.Write("    <td align='right' width='10%' class='cellTopBottomBorder'>Amount&nbsp;&nbsp;</td>");
                          Response.Write("    <td align='right' width='8%' class='cellTopBottomBorder'>&nbsp;</td>");
                          Response.Write("    <td align='left'   colspan='3' width='30%' class='cellTopBottomBorder'>&nbsp;Customer</td>");
                          Response.Write("</tr>");
                          sPageBreak = "<h3>&nbsp;</h3>";
                      }
                   }
                }
             }
              if (morerecords)
              {
                  lastTaskDescription = rs.Item("Description");
              }
              else
              {
                  break;
              }

            } //End Loop
         }

          Response.Write("</table>");

        }

        public void PrintUnitTaskTotals (string lastTaskDescriptionPR,string  subtotalHoursPR,string  subtotalAmountPR,string  subtotalMCPR){

              Response.Write("<tr class='reportTotalLine'>");
              Response.Write("    <td align='right'  class='cellTopBottomBorder' width='7%'>&nbsp;</td>");
              Response.Write("    <td align='Left'  colspan='2'  class='cellTopBottomBorder' width='33%'>Total for " + lastTaskDescriptionPR  + "</td>");
              Response.Write("    <td align='right'  class='cellTopBottomBorder' width='8%'>" + cStr(FormatNumber(subtotalHoursPR, 2, 0)) + "&nbsp;&nbsp;&nbsp;</td>");
              Response.Write("    <td align='right'  class='cellTopBottomBorder' width='4%'>&nbsp;&nbsp;</td>");
              Response.Write("    <td align='right'  class='cellTopBottomBorder' width='10%'>" + cStr(FormatNumber(subtotalAmountPR, 2, 0)) + "&nbsp;&nbsp;</td>");
              Response.Write("    <td align='right'  class='cellTopBottomBorder' colspan='1' >&nbsp;&nbsp;</td>");
              Response.Write("    <td align='Left'   class='cellTopBottomBorder' colspan='3'  width='30%'>&nbsp;&nbsp;</td>");
              Response.Write("</tr>");
              Response.Write("<tr><td colspan='8'>&nbsp;</td></tr>");

        }

        public void ShowRebillUnitReport(){

          //string sFrom, sTo;
          //Dim  subtotalHours;
          //Dim  subtotalAmount, subtotalMC;

          double subtotalHours = 0;
          double subtotalAmount = 0;
          double subtotalMC = 0;

           //  sPreviewLink = "javascript:document.form1.submit();";
          string sPreviewLink = "RebillDetailReport.aspx?PrintPreview=0&selFacilities=" + sselFacilities + "&fromDateDetail=" + sfromDateDetail + "&toDateDetail=" + stoDateDetail + "&selTasks=" + sselTasks + "&selSubTasks=" + sselSubTasks + "&selCustomers=" + sselCustomers + "&rptType=" + sRptType + "";

          string sTitle = sRptType + " Rebill Report<br>" + sfromDateDetail + " - " + stoDateDetail;

          if(System.Convert.ToString(Request["PrintPreview"]).Length >  0){
            Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='480' align='center'>");
          }else{
            Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='600' align='center'>");
         }

          Response.Write("  <tr><td colspan='8'>&nbsp;</td></tr>");

          if(UnitRS.EOF){
             //    Response.Write("  <tr><td colspan=//4//><b>" + sTitle + "</b></td></tr>");
            Response.Write("  <tr><td colspan='4'>No Rebill Unit Cost found.</td></tr>");
          }else{

            string sFacility   = "";
            string sDate = "";
            string sPageBreak = "";
            int IRow = 0;
            string rowColor = "";
            string sAmount = "";
            string sMatCosts = "";

            string lastTaskDescription = "";

            while (! UnitRS.EOF){
              UnitRS.Read();

              lastTaskDescription = UnitRS.Item("Description");

              IRow = IRow+1;
              if(IRow % 2 == 0){
                rowColor = "reportOddLine";
              }else{
                rowColor = "reportEvenLine";
             }

              if(sFacility != UnitRS.Item("FacilityID")){
                if((System.Convert.ToString(Request["PrintPreview"]) =="0") ){
                  Response.Write("  <tr><td colspan='10' align='center'><b>" + sPageBreak + sTitle + "</b></td></tr>");
               }

                Response.Write("<tr><td align='left'   colspan=10><br><b><font color='green' size=-1>" + UnitRS.Item("Name") + " - Unit Costs</td></tr>");
                Response.Write("<tr><td align='Left'   width='7%' class='cellTopBottomBorder'>Date&nbsp;</td>");
                Response.Write("    <td align='Left'   width='25%' class='cellTopBottomBorder'>SubTask</td>");
                Response.Write("    <td align='Left'   width='8%' class='cellTopBottomBorder'>&nbsp;</td>");
                Response.Write("    <td align='right'   width='8%' class='cellTopBottomBorder'>Units&nbsp;</td>");
		        Response.Write("    <td align='right'   width='4%' class='cellTopBottomBorder'>&nbsp;Rate&nbsp;</td>");

                Response.Write("    <td align='right'  colspan='1' class='cellTopBottomBorder'>Amount&nbsp;&nbsp;</td>");
                Response.Write("    <td align='right'  colspan='1' class='cellTopBottomBorder'>&nbsp;&nbsp;</td>");
                Response.Write("    <td align='left'   colspan='3' width='30%' class='cellTopBottomBorder'>&nbsp;Customer<br>&nbsp;Contact</td>");
                Response.Write("</tr>");
                sFacility = UnitRS.Item("FacilityID");
                sPageBreak = "<h3>&nbsp;</h3>";
             }

              Response.Write("<tr class='" + rowColor + "'>");
              Response.Write("    <td align='right'  width='7%'>" + FormatDate(UnitRS.Item("WorkDate")) + "&nbsp;</td>");
              Response.Write("    <td align='Left'   width='33%'>" + UnitRS.Item("Description")  + " (" + UnitRS.Item("TaskCode") + ")</td>");

              Response.Write("    <td align='Left'   width='8%' ></td>");

                if(UnitRS.Item("HoursOrUnits") == "H"){
	                Response.Write("    <td align='right'  width='8%'>" + cStr(FormatNumber(UnitRS.Item("TotalHours"), 2, 0)) + "&nbsp;&nbsp;&nbsp;</td>");
	                sAmount = cStr(cDbl(UnitRS.Item("Rate")) * cDbl(UnitRS.Item("TotalHours")));
	                subtotalHours = subtotalHours + cDbl(UnitRS.Item("TotalHours"));
	            }else{
	                Response.Write("    <td align='right'  width='8%'>" + cStr(FormatNumber(UnitRS.Item("TotalUnits"), 0, 0)) + "&nbsp;&nbsp;&nbsp;</td>");
	                sAmount = cStr(cDbl(UnitRS.Item("Rate")) * cDbl(UnitRS.Item("TotalUnits")));
	                subtotalHours = subtotalHours + cDbl(UnitRS.Item("TotalUnits"));
	            }


               Response.Write("    <td align='right'  width='4%'>&nbsp;" + UnitRS.Item("Rate") + "&nbsp;</td>");

               sMatCosts = cStr(UnitRS.Item("MaterialCosts"));

              subtotalAmount = cDbl(subtotalAmount) + cDbl(sAmount);
              subtotalMC = cDbl(subtotalMC) + cDbl(sMatCosts);

              Response.Write("    <td align='right'  colspan='1' >" + cStr(FormatNumber(sAmount, 2, 0)) + "&nbsp;&nbsp;</td>");
              Response.Write("    <td align='right'  colspan='1' > ");
              if (UnitRS.Item("Rebilled") =="True"){
                 Response.Write("<img src='../images/check.gif' />");
              }
              Response.Write(" </td>");
              Response.Write("    <td align='Left'   colspan='3'  width='30%'>&nbsp;" + UnitRS.Item("CustomerCode") + "-&nbsp;" + UnitRS.Item("ContactName") + "</td> ");
              Response.Write("</tr>");




              if(UnitRS.EOF){
                 PrintUnitTaskTotals (cStr(lastTaskDescription), cStr(subtotalHours), cStr(subtotalAmount), cStr(subtotalMC));
                 subtotalHours = 0;
                 subtotalAmount = 0;
                 subtotalMC = 0;
              }else{
                 if (lastTaskDescription != UnitRS.Item("Description")){
                    PrintUnitTaskTotals (cStr(lastTaskDescription), cStr(subtotalHours), cStr(subtotalAmount), cStr(subtotalMC));
                    subtotalHours = 0;
                    subtotalAmount = 0;
                    subtotalMC = 0;

                    if ( sRptType == "Detail"){
                        if((System.Convert.ToString(Request["PrintPreview"])=="0") ){

                          Response.Write("  <tr><td colspan='8' align='center'><b>" + sPageBreak + sTitle + "</b></td></tr>");
                          Response.Write("<tr><td align='left'   colspan=8><br><b><font color='green' size=-1>" + UnitRS.Item("Name") + "</td></tr>");
                          Response.Write("<tr><td align='Left'   width='7%' class='cellTopBottomBorder'>Date&nbsp;</td>");
                          Response.Write("    <td align='Left'   width='33%' class='cellTopBottomBorder'>SubTask</td>");
                          Response.Write("    <td align='right' colspan='4'  class='cellTopBottomBorder'>&nbsp;&nbsp;</td>");
                          Response.Write("    <td align='left'   colspan='3' width='30%' class='cellTopBottomBorder'>&nbsp;Customer</td>");
                          Response.Write("</tr>");
                          sPageBreak = "<h3>&nbsp;</h3>";
                      }
                   }
                }
             }
              if( !UnitRS.EOF){
                 lastTaskDescription = UnitRS.Item("Description");
             }

            } //End Loop
         }

          Response.Write("</table>");

        }

    }
}