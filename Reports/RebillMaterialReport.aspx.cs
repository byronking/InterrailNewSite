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
    public partial class RebillMaterialReport : PageBase
    {

        public DataReader rs;
        public int rs_numRows;
        public string sselFacilities, sfromDateDetail, stoDateDetail, sselTasks, sselCustomers, sRptType, sselSubTasks;
        public string sRebillType, wRebilled, wFacilities, wDateRange, wTasks, wSubTasks, wCustomers, sAmount;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

           
            GrantAccess("Super, Admin, User");


            sselFacilities    = cStr(Request["selFacilities"]);
            sfromDateDetail   = cStr(Request["fromDateDetail"]);
            stoDateDetail     = cStr(Request["toDateDetail"]);
            sselTasks         = cStr(Request["selTasks"]);
            sselSubTasks      = cStr(Request["selSubTasks"]);
            sselCustomers     = cStr(Request["selCustomers"]);
            sRptType          = cStr(Request["rptType"]);
            sRebillType       = cStr(Request["RebillType"]);
            wRebilled = "";
            wFacilities = "";
            wDateRange = "";
            wTasks = "";
            wSubTasks = "";
            wCustomers = "";


                  Session["LastStartDate"] = sfromDateDetail;
                  Session["LastEndDate"]   = stoDateDetail;

                if(sRebillType == "Rebilled"){
                  wRebilled = " AND (RebillDetail.Rebilled != 0) ";
                }
                if(sRebillType == "NotRebilled"){
                  wRebilled = " AND (RebillDetail.Rebilled = 0) ";
                }
                if(sRebillType == "RebilledBoth"){
                  wRebilled = "  ";
                }

                if(sselFacilities != ""){
                  wFacilities = "  AND (RebillDetail.FacilityID IN  (" + sselFacilities + ") ) ";
                }else{
                  wFacilities = "  AND (RebillDetail.FacilityID IN  (" + Session["FacilityID"] + ") ) ";
                }

                wDateRange = " AND (WorkDate Between '" + sfromDateDetail + "' AND '" + stoDateDetail + "') ";

                if(sselTasks != ""){
                  wTasks = "  AND ( FacilityTasks.TaskId IN  (" + sselTasks + ") ) ";
                }else{
                  wTasks = "   ";
                }

                if(sselSubTasks != ""){
                  wSubTasks = "  AND ( RebillDetail.RebillSubTasksID IN (" + sselSubTasks + ") ) ";
                }else{
                  wSubTasks = "   ";
                }

                if(sselCustomers != ""){
                  wCustomers = "  AND (FacilityCustomerID IN  (" + sselCustomers + ") ) ";
                }else{
                  wCustomers = "   ";
                }

                //This query does !have a server behavior ---;
                string rsSource = " ";
                rsSource += " SELECT  WorkDate, WDesc=IsNull(WorkDescription, 'N/A'), TotalHours, CustomerCode, ";
                rsSource += "         ContactName, MaterialCosts=IsNull(MaterialCosts,0), Vendors, InvoiceNumber, ";
                rsSource += "         TaskCode, TaskDescription, Description, FacilityTasks.TaskId, TotalUnits, ";
                rsSource += "         FacilityCustomerId, RebillDetail.FacilityID, Name, CustomerName,         ";
                rsSource += "         ISNULL((SELECT   TOP 1 RebillRate FROM RebillSubTaskRates                ";
                rsSource += "         WHERE RebillDetail.WorkDate BETWEEN EffectiveDate AND ExpirationDate     ";
                rsSource += "           AND RebillSubTaskRates.RebillSubTasksID = RebillDetail.RebillSubTasksID ";
                rsSource += "         ORDER BY ExpirationDate DESC), 0) AS Rate,                               ";
                rsSource += "         RebillDetail.ID,                                                         ";
                rsSource += "         RebillSubTasks.HoursOrUnits                                              ";
                rsSource += " FROM    RebillDetail                                                             ";
                rsSource += "         INNER JOIN RebillSubTasks ON RebillSubTasksId = RebillSubTasks.Id        ";
                rsSource += "         INNER JOIN Facility ON RebillDetail.FacilityID = Facility.Id             ";
                rsSource += "         INNER JOIN FacilityTasks ON Facility.Id = FacilityTasks.FacilityID       ";
                rsSource += "         INNER JOIN Tasks ON FacilityTasks.TaskId = Tasks.Id                      ";
                rsSource += "         AND dbo.RebillSubTasks.TaskID = Tasks.Id                                 ";
                rsSource += "         INNER JOIN FacilityCustomer ON Facility.Id = FacilityCustomer.FacilityId ";
                rsSource += "         AND RebillSubTasks.FacilityCustomerId = FacilityCustomer.Id              ";
                rsSource += " WHERE  ((MaterialCosts > 0) AND (MaterialCosts IS NOT null))                     ";
                rsSource +=          wDateRange + wFacilities + wTasks + wCustomers + wSubTasks + wRebilled;
                rsSource += " Order By CustomerName, Description, WorkDate, Name   ";


                rs = new DataReader(rsSource);
                rs.Open();
                rs_numRows = 0;


                string sPageBreak;

        }

        public void PrintTaskTotals(string lastTaskDescriptionPR, string subtotalHoursPR, string subtotalAmountPR, string subtotalMCPR)
        {

            Response.Write("<tr class='reportTotalLine'>");
            Response.Write("    <td align='right'  class='cellTopBottomBorder' width='7%'>&nbsp;</td>");
            Response.Write("    <td align='Left'  colspan='2'  class='cellTopBottomBorder' width='33%'>Total for " + lastTaskDescriptionPR + "</td>");
            Response.Write("    <td align='right'  class='cellTopBottomBorder' width='8%'>" + cStr(FormatNumber(subtotalHoursPR, 2, 0)) + "&nbsp;&nbsp;&nbsp;</td>");
            Response.Write("    <td align='right'  class='cellTopBottomBorder' width='4%'>&nbsp;&nbsp;</td>");
            Response.Write("    <td align='right'  class='cellTopBottomBorder' width='10%'>" + cStr(FormatNumber(subtotalAmountPR, 2, 0)) + "&nbsp;&nbsp;</td>");


            Response.Write("    <td align='right'  class='cellTopBottomBorder' colspan='1' >" + cStr(FormatNumber(subtotalMCPR, 2, 0)) + "&nbsp;&nbsp;</td>");
            Response.Write("    <td align='Left'   class='cellTopBottomBorder' colspan='3'  width='30%'>&nbsp;&nbsp;</td>");
            Response.Write("</tr>");
            Response.Write("<tr><td colspan='8'>&nbsp;</td></tr>");

        }
        
        public void ShowRebillMaterialReport()
        {

            //string sFrom, sTo;
            double subtotalHours;
            double subtotalAmount, subtotalMC;

            subtotalHours = 0;
            subtotalAmount = 0;
            subtotalMC = 0;

            //  sPreviewLink = "javascript:document.form1.submit();";
            string sPreviewLink = "RebillMaterialReport.aspx?PrintPreview=0&selFacilities=" + sselFacilities + "&fromDateDetail=" + sfromDateDetail + "&toDateDetail=" + stoDateDetail + "&selTasks=" + sselTasks + "&selSubTasks=" + sselSubTasks + "&selCustomers=" + sselCustomers + "&rptType=" + sRptType + "";

            string sTitle = sRptType + " Rebill Report<br>" + sfromDateDetail + " - " + stoDateDetail;

            if (System.Convert.ToString(Request["PrintPreview"]).Length > 0)
            {
                Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='480' align='center'>");
            }
            else
            {
                Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='600' align='center'>");
            }

            Response.Write("  <tr><td colspan='8'>&nbsp;</td></tr>");
            if (System.Convert.ToString(Request["PrintPreview"]).Length > 0)
            {
                Response.Write("  <tr><td colspan='8' align='right'><a href='" + sPreviewLink + "'>Printer Friendly</a></td></tr>");
                Response.Write("  <tr><td colspan='8' align='center'><b>" + sTitle + "</b></td></tr>");
            }
            else
            {
                Response.Write("  <tr><td colspan='8'>&nbsp;</td></tr>");
            }

            if (rs.EOF)
            {
                Response.Write("  <tr><td colspan='4'>&nbsp;</td></tr>");
                Response.Write("  <tr><td colspan='4'>No records found.</td></tr>");
            }
            else
            {
                string sFacility = "";
                string sDate = "";
                string sPageBreak = "";
                int IRow = 0;
                string rowColor = "";
                string sMatCosts;
                string lastTaskDescription;

                while (!rs.EOF)
                {
                    rs.Read();

                    lastTaskDescription = rs.Item("Description");

                    IRow = IRow + 1;
                    if (IRow % 2 == 0)
                    {
                        rowColor = "reportOddLine";
                    }
                    else
                    {
                        rowColor = "reportEvenLine";
                    }

                    if (sFacility != rs.Item("FacilityID"))
                    {
                        if ((System.Convert.ToString(Request["PrintPreview"]) == "0"))
                        {
                            Response.Write("  <tr><td colspan='10' align='center'><b>" + sPageBreak + sTitle + "</b></td></tr>");
                        }

                        Response.Write("<tr><td align='left'   colspan=10><br><b><font color='green' size=-1>" + rs.Item("Name") + "</td></tr>");
                        Response.Write("<tr><td align='Left'   width='7%' class='cellTopBottomBorder'>Date&nbsp;</td>");
                        Response.Write("    <td align='Left'   width='25%' class='cellTopBottomBorder'>SubTask</td>");
                        Response.Write("    <td align='Left'   width='8%' class='cellTopBottomBorder'>Att.</td>");
                        Response.Write("    <td align='right'   width='8%' class='cellTopBottomBorder'>Hours / Units&nbsp;</td>");
                        Response.Write("    <td align='right'   width='4%' class='cellTopBottomBorder'>&nbsp;Rate&nbsp;</td>");

                        Response.Write("    <td align='right'  colspan='1' class='cellTopBottomBorder'>Amount&nbsp;&nbsp;</td>");
                        Response.Write("    <td align='right'  colspan='1' class='cellTopBottomBorder'>Material Costs&nbsp;&nbsp;</td>");
                        Response.Write("    <td align='left'   colspan='3' width='30%' class='cellTopBottomBorder'>&nbsp;Customer<br>&nbsp;Contact</td>");
                        Response.Write("</tr>");
                        sFacility = rs.Item("FacilityID");
                        sPageBreak = "<h3>&nbsp;</h3>";
                    }

                    Response.Write("<tr class='" + rowColor + "'>");
                    Response.Write("    <td align='right'  width='7%'>" + FormatDate(rs.Item("WorkDate")) + "&nbsp;</td>");
                    Response.Write("    <td align='Left'   width='33%'>" + rs.Item("Description") + " (" + rs.Item("TaskCode") + ")</td>");

                    DataReader rsAttachments = new DataReader("SELECT path, title FROM RebillAttachments WHERE RebillDetailId = " + cStr(rs.Item("ID")));
                    rsAttachments.Open();

                    string strAttList = "";
                    int attcount = 0;

                    while (!rsAttachments.EOF)
                    {
                        rsAttachments.Read();

                        attcount = attcount + 1;
                        if (attcount > 1)
                        {
                            strAttList = strAttList + ",";
                        }
                        strAttList = strAttList + "<a href='../Rebilling/" + cStr(rsAttachments.Item("Path")) + "' target=_blank>" + cStr(attcount) + "</a>";

                    } //End Loop
                    Response.Write("    <td align='Left'   width='8%' >" + strAttList + "</td>");


                    if (rs.Item("HoursOrUnits") == "H")
                    {
                        Response.Write("    <td align='right'  width='8%'>" + cStr(FormatNumber(rs.Item("TotalHours"), 2, 0)) + "&nbsp;&nbsp;&nbsp;</td>");
                        sAmount = cStr(cDbl(rs.Item("Rate")) * cDbl(rs.Item("TotalHours")));
                        subtotalHours = subtotalHours + cDbl(rs.Item("TotalHours"));
                    }
                    else
                    {
                        Response.Write("    <td align='right'  width='8%'>" + cStr(FormatNumber(rs.Item("TotalUnits"), 0, 0)) + "&nbsp;&nbsp;&nbsp;</td>");
                        sAmount = cStr(cDbl(rs.Item("Rate")) * cDbl(rs.Item("TotalUnits")));
                        subtotalHours = subtotalHours + cDbl(rs.Item("TotalUnits"));
                    }


                    Response.Write("    <td align='right'  width='4%'>&nbsp;" + rs.Item("Rate") + "&nbsp;</td>");

                    sMatCosts = cStr(rs.Item("MaterialCosts"));

                    subtotalAmount = subtotalAmount + cDbl(sAmount);
                    subtotalMC = cDbl(subtotalMC) + cDbl(sMatCosts);

                    Response.Write("    <td align='right'  colspan='1' >" + cStr(FormatNumber(sAmount, 2, 0)) + "&nbsp;&nbsp;</td>");
                    Response.Write("    <td align='right'  colspan='1' >" + cStr(FormatNumber(sMatCosts, 2, 0)) + "&nbsp;&nbsp;</td>");
                    Response.Write("    <td align='Left'   colspan='3'  width='30%'>&nbsp;" + rs.Item("CustomerCode") + "-&nbsp;" + rs.Item("ContactName") + "</td> ");
                    Response.Write("</tr>");




                    if (rs.EOF)
                    {
                        PrintTaskTotals(cStr(lastTaskDescription), cStr(subtotalHours), cStr(subtotalAmount), cStr(subtotalMC));
                        subtotalHours = 0;
                        subtotalAmount = 0;
                        subtotalMC = 0;
                    }
                    else
                    {
                        if (lastTaskDescription != rs.Item("Description"))
                        {
                            PrintTaskTotals(cStr(lastTaskDescription), cStr(subtotalHours), cStr(subtotalAmount), cStr(subtotalMC));
                            subtotalHours = 0;
                            subtotalAmount = 0;
                            subtotalMC = 0;

                            if (sRptType == "Detail")
                            {
                                if ((System.Convert.ToString(Request["PrintPreview"]) == "0"))
                                {
                                    Response.Write("  <tr><td colspan='8' align='center'><b>" + sPageBreak + sTitle + "</b></td></tr>");
                                    Response.Write("<tr><td align='left'   colspan=8><br><b><font color='green' size=-1>" + rs.Item("Name") + "</td></tr>");
                                    Response.Write("<tr><td align='Left'   width='7%' class='cellTopBottomBorder'>Date&nbsp;</td>");
                                    Response.Write("    <td align='Left'   width='33%' class='cellTopBottomBorder'>SubTask</td>");
                                    Response.Write("    <td align='right' colspan='4'  class='cellTopBottomBorder'>Mat. Costs&nbsp;&nbsp;</td>");
                                    Response.Write("    <td align='left'   colspan='3' width='30%' class='cellTopBottomBorder'>&nbsp;Customer</td>");
                                    Response.Write("</tr>");
                                    sPageBreak = "<h3>&nbsp;</h3>";
                                }
                            }
                        }
                    }
                    if (!rs.EOF)
                    {
                        lastTaskDescription = rs.Item("Description");
                    }

                } //End Loop
            }

            if (System.Convert.ToString(Request["PrintPreview"]).Length > 0)
            {
                Response.Write("  <tr><td colspan='8' align='right'><a href='" + sPreviewLink + "'>Printer Friendly</a></td></tr>");
            }
            else
            {
                Response.Write("  <tr><td colspan='8'>&nbsp;</td></tr>");
            }
            Response.Write("</table>");

        }

    }
}