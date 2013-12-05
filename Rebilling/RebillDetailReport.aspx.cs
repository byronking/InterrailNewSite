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

namespace InterrailPPRS.Rebilling
{
    public partial class RebillDetailReport : PageBase
    {

        public DataReader rs;
        public int rs_numRows = 0;
        public string sselFacilities, sfromDateDetail, stoDateDetail, sselTasks, sselCustomers, sRptType, sselSubTasks;
        public string wFacilities, wDateRange, wTasks, wSubTasks, wCustomers;
        public string sPageBreak;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin, User");


            sselFacilities    = Request["selFacilities"];
            sfromDateDetail   = Request["fromDateDetail"];
            stoDateDetail     = Request["toDateDetail"];
            sselTasks         = Request["selTasks"];
            sselSubTasks      = Request["selSubTasks"];
            sselCustomers     = Request["selCustomers"];
            sRptType          = Request["rptType"];

            if(sselFacilities != null && sselFacilities != ""){
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

            string strSQL;

            //This query does !have a server behavior ---;
            strSQL = " ";
            strSQL +=  "SELECT  WorkDate, WDesc=IsNull(WorkDescription, 'N/A'), TotalHours, CustomerCode,";
            strSQL +=  "        ContactName,";
            strSQL +=  "        TaskCode, TaskDescription, Description, FacilityTasks.TaskId, TotalUnits,";
            strSQL +=  "        FacilityCustomerId, RebillDetail.FacilityID, Name, CustomerName,         ";
            strSQL +=  "        ISNULL((SELECT   TOP 1 RebillRate FROM RebillSubTaskRates                ";
            strSQL += "        WHERE RebillDetail.WorkDate BETWEEN EffectiveDate AND ExpirationDate     ";
            strSQL += "          AND RebillSubTaskRates.RebillSubTasksID = RebillDetail.RebillSubTasksID";
            strSQL +=  "        ORDER BY ExpirationDate DESC), 0) AS Rate, RebillDetail.ID,              ";
            strSQL +=  "        RebillSubTasks.HoursOrUnits                                              ";
            strSQL +=  "FROM    RebillDetail                                                             ";
            strSQL +=  "        INNER JOIN RebillSubTasks ON RebillSubTasksId = RebillSubTasks.Id        ";
            strSQL +=  "        INNER JOIN Facility ON RebillDetail.FacilityID = Facility.Id             ";
            strSQL +=  "        INNER JOIN FacilityTasks ON Facility.Id = FacilityTasks.FacilityID       ";
            strSQL +=  "        INNER JOIN Tasks ON FacilityTasks.TaskId = Tasks.Id                      ";
            strSQL += "        AND dbo.RebillSubTasks.TaskID = Tasks.Id                                 ";
            strSQL +=  "        INNER JOIN FacilityCustomer ON Facility.Id = FacilityCustomer.FacilityId ";
            strSQL += "        AND RebillSubTasks.FacilityCustomerId = FacilityCustomer.Id              ";
            strSQL +=  "WHERE  (1=1)                                                                     ";
            strSQL = strSQL +          wDateRange + wFacilities + wTasks + wCustomers + wSubTasks;
            strSQL +=  "Order By CustomerName, Description, WorkDate, Name                               ";


            rs = new DataReader(); 
            rs.Open();
            rs_numRows = rs.RecordCount;

       }

        public void PrintTaskTotals(string lastTaskDescription, string subtotalHours, string subtotalAmount)
        {
            Response.Write("<tr class='reportTotalLine'>");
            Response.Write("    <td align='right'  class='cellTopBottomBorder' width='7%'>&nbsp;</td>");
            Response.Write("    <td align='Left'   class='cellTopBottomBorder' width='33%'>Total for " + lastTaskDescription + "</td>");
            Response.Write("    <td align='right'  class='cellTopBottomBorder' width='8%'>" + cStr(FormatNumber(subtotalHours, 2, 0)) + "&nbsp;&nbsp;&nbsp;</td>");
            Response.Write("    <td align='right'  class='cellTopBottomBorder' width='4%'>&nbsp;&nbsp;</td>");
            Response.Write("    <td align='right'  class='cellTopBottomBorder' width='10%'>" + cStr(FormatNumber(subtotalAmount, 2, 0)) + "&nbsp;&nbsp;</td>");
            Response.Write("    <td align='Left'   class='cellTopBottomBorder' colspan='3'  width='30%'>&nbsp;</td>");
            Response.Write("</tr>");
            Response.Write("<tr><td colspan='8'>&nbsp;</td></tr>");

        }

        public void ShowRebillDetailReport(){

          string sFrom, sTo;
          double subtotalHours = 0.0;
          double subtotalAmount = 0.0;
          double subTotalHours = 0.0;


          string sPreviewLink = "javascript:document.form1.submit();";
          string sTitle = sRptType + " Rebill Report<br>" + sfromDateDetail + " - " + stoDateDetail;

          if( Request["PrintPreview"] != null && cStr(Request["PrintPreview"]).Length > 0){
            Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='480' align='center'>");
          }else{
            Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='600' align='center'>");
         }

          Response.Write("  <tr><td colspan='8'>&nbsp;</td></tr>");
          if( Request["PrintPreview"] != null && cStr(Request["PrintPreview"]).Length > 0){
            Response.Write("  <tr><td colspan='8' align='right'><a href='" + sPreviewLink + "'>Printer Friendly</a></td></tr>");
            Response.Write("  <tr><td colspan='8' align='center'><b>" + sTitle + "</b></td></tr>");
          }else{
            Response.Write("  <tr><td colspan='8'>&nbsp;</td></tr>");
          }

          if(rs.EOF){
            Response.Write("  <tr><td colspan='4'>&nbsp;</td></tr>");
            Response.Write("  <tr><td colspan='4'>No records found.</td></tr>");
          }else{

            string sFacility   = "";
            string sDate = "";
            string sPageBreak = "";
            int IRow = 0;
            string rowColor;
            double sAmount;
            string lastTaskDescription;

            lastTaskDescription = rs.Item("Description");

            while (!rs.EOF){
                rs.Read();

              IRow = IRow+1;
              if(IRow % 2 == 0){
                rowColor = "reportOddLine";
              }else{
                rowColor = "reportEvenLine";
             }

              if(sFacility != rs.Item("FacilityID")){

                if((Request["PrintPreview"]=="0") ){
                    Response.Write("  <tr><td colspan='8' align='center'><b>" + sPageBreak + sTitle + "</b></td></tr>");
                }

                Response.Write("<tr><td align='left'   colspan=8><br><b><font color='green' size=-1>" + rs.Item("Name") + "</td></tr>");
                Response.Write("<tr><td align='Left'   width='7%' class='cellTopBottomBorder'>Date&nbsp;</td>");
                Response.Write("    <td align='Left'   width='33%' class='cellTopBottomBorder'>SubTask</td>");
                Response.Write("    <td align='right'   width='8%' class='cellTopBottomBorder'>Hours/Units&nbsp;</td>");
                Response.Write("    <td align='right'   width='4%' class='cellTopBottomBorder'>&nbsp;Rate&nbsp;</td>");
                Response.Write("    <td align='right' width='10%' class='cellTopBottomBorder'>Amount&nbsp;&nbsp;</td>");
                Response.Write("    <td align='left'   colspan='3' width='30%' class='cellTopBottomBorder'>&nbsp;Customer<br>&nbsp;Contact</td>");
                Response.Write("</tr>");
                sFacility = rs.Item("FacilityID");
                sPageBreak = "<h3>&nbsp;</h3>";

             }

              Response.Write("<tr class='" + rowColor + "'>");
              Response.Write("    <td align='right'  width='7%'>" + rs.Item("WorkDate") + "&nbsp;</td>");
              Response.Write("    <td align='Left'   width='33%'>" + rs.Item("Description")  + " (" + rs.Item("TaskCode") + ")</td>");

              if(rs.Item("HoursOrUnits") == "H"){
                 Response.Write("    <td align='right'  width='8%'>" + cStr(FormatNumber(rs.Item("TotalHours"), 2, 0)) + "&nbsp;&nbsp;&nbsp;</td>");
                 sAmount = cDbl(rs.Item("Rate")) * cDbl(rs.Item("TotalHours"));
                 subtotalHours = subTotalHours + cDbl(rs.Item("TotalHours"));
              }else{
                 Response.Write("    <td align='right'  width='8%'>" + cStr(FormatNumber(rs.Item("TotalUnits"), 0, 0)) + "&nbsp;&nbsp;&nbsp;</td>");
                 sAmount = cDbl(rs.Item("Rate")) * cDbl(rs.Item("TotalUnits"));
                 subtotalHours = subtotalHours + cDbl(rs.Item("TotalUnits"));
              }

              Response.Write("    <td align='right'  width='4%'>&nbsp;" + rs.Item("Rate") + "&nbsp;</td>");

              subtotalAmount = subtotalAmount + sAmount;
              Response.Write("    <td align='right'  width='10%'>" + cStr(FormatNumber(cStr(sAmount), 2, 0)) + "&nbsp;&nbsp;</td>");

              Response.Write("    <td align='Left'   colspan='3'  width='30%'>&nbsp;" + rs.Item("CustomerCode") + "-&nbsp;" + rs.Item("ContactName") + "</td> ");
              Response.Write("</tr>");

              if( (Len(rs.Item("WDesc")) > 0) && (sRptType == "Detail")){

                Response.Write("<tr class='" + rowColor + "'><td colspan='8'>&nbsp;</td></tr>");
                Response.Write("<tr class='" + rowColor + "'>");
                Response.Write("    <td align='right'  valign='top' width='7%' >&nbsp;</td>");
                Response.Write("    <td align='Left'   colspan='7'  width='75%'><b><u>Work Description:</u>&nbsp;&nbsp;</b>" + rs.Item("WDesc") + "</td>");
                Response.Write("</tr>");
              }

                  if(sRptType == "Detail"){

                    string strSQL=  "";
                    strSQL +=  "SELECT  LastName, FirstName, HoursWorked, RebillDetailID  ";
                    strSQL +=  "FROM  EmployeeTaskWorked ";
                    strSQL +=  "INNER JOIN RebillDetail ON RebillDetailID = RebillDetail.Id ";
                    strSQL +=  "INNER JOIN Employee ON EmployeeId = Employee.Id  ";
                    strSQL +=  "WHERE " + rs.Item("RebillDetailID") + " = ID; ";

                    DataReader rsEmp = new DataReader(strSQL); rs.Item("rstEmp");
                    rsEmp.Open();

                    if(!rsEmp.EOF){

                        Response.Write("<tr class='" + rowColor + "'><td colspan='8'>&nbsp;</td></tr>");
                        Response.Write("<tr class='" + rowColor + "'>");
                        Response.Write("    <td align='right'  valign='top' width='7%' ><b>&nbsp;</b></td>");
                        Response.Write("    <td align='Left'   colspan='2' class='cellTopBottomBorder'>Employee Name</td>");
                        Response.Write("    <td align='right'   colspan='2'class='cellTopBottomBorder'>&nbsp;Hours Worked</td>");
                        Response.Write("    <td align='left'   colspan='3' width='30%'>&nbsp;</td>");
                        Response.Write("</tr>");

                        while(!rsEmp.EOF){
                          rsEmp.Read();
                          Response.Write("<tr class='" + rowColor + "'>");
                          Response.Write("    <td align='right'  valign='top' width='7%' ><b>&nbsp;</b></td>");
                          Response.Write("    <td align='Left'   colspan='2'>" + rsEmp.Item("LastName") + ", " + rsEmp.Item("FirstName") + "</td>");
                          Response.Write("    <td align='right'   colspan='2'>&nbsp;" + cStr(FormatNumber(rsEmp.Item("HoursWorked"), 2, 0)) + "</td>");
                          Response.Write("    <td align='left'   colspan='3' width='30%'>&nbsp;</td>");
                          Response.Write("</tr>");

                        } //loop

                        Response.Write("<tr class='" + rowColor + "'><td colspan='8'>&nbsp;</td></tr>");
                    }

                  }


              if (rs.EOF){

                 PrintTaskTotals(cStr(lastTaskDescription), cStr(subtotalHours), cStr(subtotalAmount));
                 subtotalHours = 0;
                 subtotalAmount = 0;

              }else{
                 if (lastTaskDescription != rs.Item("Description")){
                    PrintTaskTotals (cStr(lastTaskDescription), cStr(subtotalHours), cStr(subtotalAmount));
                    subtotalHours = 0;
                    subtotalAmount = 0;
                    if  (sRptType == "Detail"){
                        if((Request["PrintPreview"]=="0") ){
                          Response.Write("  <tr><td colspan='8' align='center'><b>" + sPageBreak + sTitle + "</b></td></tr>");
                          Response.Write("<tr><td align='left'   colspan=8><br><b><font color='green' size=-1>" + rs.Item("Name") + "</td></tr>");
                          Response.Write("<tr><td align='Left'   width='7%' class='cellTopBottomBorder'>Date&nbsp;</td>");
                          Response.Write("    <td align='Left'   width='33%' class='cellTopBottomBorder'>SubTask</td>");
                          Response.Write("    <td align='right'   width='8%' class='cellTopBottomBorder'>Hours/Units&nbsp;&nbsp;&nbsp;</td>");
                          Response.Write("    <td align='right'   width='4%' class='cellTopBottomBorder'>&nbsp;Rate&nbsp;</td>");
                          Response.Write("    <td align='right' width='10%' class='cellTopBottomBorder'>Amount&nbsp;&nbsp;</td>");
                          Response.Write("    <td align='left'   colspan='3' width='30%' class='cellTopBottomBorder'>&nbsp;Customer</td>");
                          Response.Write("</tr>");
                          sPageBreak = "<h3>&nbsp;</h3>";
                      }
                   }
                }
             }

             if (!rs.EOF){
                 lastTaskDescription = rs.Item("Description");
             }

            } //LOOP;
         }

          if(Request["PrintPreview"] != null && cStr(Request["PrintPreview"]).Length > 0){
            Response.Write("  <tr><td colspan='8' align='right'><a href='" + sPreviewLink + "'>Printer Friendly</a></td></tr>");
          }else{
            Response.Write("<tr><td colspan='8'>&nbsp;</td></tr>");
         }
          Response.Write("</table>");

        }


    }
}