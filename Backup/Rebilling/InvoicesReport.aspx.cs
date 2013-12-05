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
    public partial class InvoicesReport : PageBase
    {

        public DataReader rs;
        public int rs_numRows;

        public string sselFacilities;
        public string sfromDateDetail;
        public string stoDateDetail;
        public string sselTasks;
        public string sselCustomers;
        public string sRptType;
        public string wFacilities;
        public string wDateRange;
        public string wTasks;
        public string sPageBreak;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

            GrantAccess("Super, Admin, User");

            sselFacilities    = Request["selFacilities"];
            sfromDateDetail   = Request["fromDateDetail"];
            stoDateDetail     = Request["toDateDetail"];
            sRptType          = Request["rptType"];

            Session["LastStartDate"] = sfromDateDetail;
            Session["LastEndDate"]   = stoDateDetail;


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

            string strSQL = "";
            //This query does !have a server behavior ---;
            strSQL +=  " SELECT  WorkDate, WDesc=IsNull(WorkDescription, 'N/A'), TotalHours, CustomerCode,";
            strSQL +=  "        TaskCode, TaskDescription, Description, FacilityTasks.TaskId, TotalUnits,";
            strSQL +=  "        FacilityCustomerId, RebillDetail.FacilityID, Name, CustomerName,         ";
            strSQL +=  "        ISNULL((SELECT   TOP 1 RebillRate FROM RebillSubTaskRates                ";
            strSQL +=  "        WHERE RebillDetail.WorkDate BETWEEN EffectiveDate AND ExpirationDate     ";
            strSQL += "          AND RebillSubTaskRates.RebillSubTasksID = RebillDetail.RebillSubTasksID";
            strSQL +=  "        ORDER BY ExpirationDate DESC), 0) AS Rate, RebillDetail.ID,              ";
            strSQL +=  "        ContactName, ContactAddress1,  ContactAddress2,  ContactAddress3,        ";
            strSQL +=  "              (Select Count(*) From EmployeeTaskWorked                           ";
            strSQL +=  "               Where RebilldetailID = RebillDetail.ID) As NbrEmployees,          ";
            strSQL +=  "        CompanyName, CompanyID, LogoPath, RebillSubTasks.HoursOrUnits,           ";
            strSQL +=  "        Facility.Address1, Facility.Address2, Facility.Address3,                  ";
            strSQL +=  "        MC=IsNull(MaterialCosts,0), Vendors, InvoiceNumber                       ";
            strSQL +=  " FROM    RebillDetail                                                             ";
            strSQL +=  "        INNER JOIN RebillSubTasks ON RebillSubTasksId = RebillSubTasks.Id        ";
            strSQL +=  "        INNER JOIN Facility ON RebillDetail.FacilityID = Facility.Id             ";
            strSQL +=  "        INNER JOIN FacilityTasks ON Facility.Id = FacilityTasks.FacilityID       ";
            strSQL +=  "        INNER JOIN Tasks ON FacilityTasks.TaskId = Tasks.Id                      ";
            strSQL += "        AND dbo.RebillSubTasks.TaskID = Tasks.Id                                 ";
            strSQL +=  "        INNER JOIN FacilityCustomer ON Facility.Id = FacilityCustomer.FacilityId ";
            strSQL += "        AND RebillSubTasks.FacilityCustomerId = FacilityCustomer.Id              ";
            strSQL +=  "        INNER JOIN IRGCompany ON IRGCompanyId = IRGCompany.Id                    ";
            strSQL +=  " WHERE  (1=1)                                                                     ";
            strSQL = strSQL +          wDateRange + wFacilities;
            strSQL +=  "Order By Name , WorkDate, CustomerName, Description                              ";

            rs = new DataReader(strSQL);
            rs.Open();
            rs_numRows = rs.RecordCount;

}

        public void ShowReport(){

          string sFrom, sTo;

          string sPreviewLink = "javascript:document.form1.submit();";
          sPageBreak = "";

          if(cStr(Request["PrintPreview"]).Length > 0){
            Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='480' align='center'>");
          }else{
            Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='600' valign='top' align='center'>");
         }
          Response.Write("  <tr><td colspan='5'>&nbsp;</td></tr>");

          if(cStr(Request["PrintPreview"]).Length > 0){
            Response.Write("  <tr><td colspan='5' align='right'><a href='" + sPreviewLink + "'>Printer Friendly</a></td></tr>");
         }

          if(rs.EOF){
            Response.Write("  <tr><td colspan='5'>&nbsp;</td></tr>");
            Response.Write("  <tr><td colspan='5'>No records found.</td></tr>");
          }else{

             while ( ! rs.EOF) {
                  rs.Read();
                string sFormsTitle = "<img src='" + rs.Item("LogoPath") + "'><br>" + rs.Item("CompanyName");
                string sInvoicesTitle =  rs.Item("CompanyName") + "<br>" + rs.Item("Address1") + "<br>" + rs.Item("Address2") + "<br>" + rs.Item("Address3") + "<br>";

                  if( (sRptType == "FormsOnly") || (sRptType == "Both")){

                Response.Write(" <tr><td colspan='5' align='center'><b>" + sPageBreak + sFormsTitle + "</b></td></tr>");
                Response.Write(" <tr><td colspan='5' align='RIGHT'><b>RB-1</b></td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");

                Response.Write(" <tr><td width='20%'><b>DATE: </b></td><td width='20%' class='thinBB'>" + rs.Item("WorkDate") + "</td>");
                Response.Write("     <td width='20%' align='right'><b>INVOICE: &nbsp;</b></td><td colspan='2' class='thinBB'>" + rs.Item("CompanyID") + "</td></tr>");

                Response.Write(" <tr><td width='20%'><b>VENDOR </b></td><td colspan='4' class='thinBB'>" + rs.Item("Vendors") + "</td></tr>");
                Response.Write(" <tr><td colspan='2'><b>VENDOR INVOICE NUMBER </b></td><td class='thinBB'>" + rs.Item("InvoiceNumber") + "</td><td width='20%' align='right'><b>DATE: </b></td><td width='20%' class='thinBB'>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='3'><b>TERMINAL LOCATION WORK PERFORMED </b></td><td colspan='2' class='thinBB'>" + rs.Item("Name") + "</td></tr>");
                Response.Write(" <tr><td colspan='3'><b>WORK TO BE BILLED TO: </b></td><td colspan='2' class='thinBB'>" + rs.Item("ContactName") + "</td></tr>");
                Response.Write(" <tr><td colspan='3'>&nbsp;</td><td colspan='2' class='thinBB'>" + rs.Item("ContactAddress1") + "</td></tr>");
                Response.Write(" <tr><td colspan='3'>&nbsp;</td><td colspan='2' class='thinBB'>" + rs.Item("ContactAddress2") + "</td></tr>");
                Response.Write(" <tr><td colspan='3'>&nbsp;</td><td colspan='2' class='thinBB'>" + rs.Item("ContactAddress3") + "</td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                if(rs.Item("HoursOrUnits") == "H"){
                  Response.Write(" <tr><td colspan='3'><b>WORK PERFORMED AT CONTRACT RATE OF </b></td><td colspan='2' class='thinBB'>$&nbsp;" + cStr(FormatNumber(rs.Item("Rate"), 2, 0)) + "&nbsp; per hour </td></tr>");
                }else{
                  Response.Write(" <tr><td colspan='3'><b>WORK PERFORMED AT CONTRACT RATE OF </b></td><td colspan='2' class='thinBB'>$&nbsp;" + cStr(FormatNumber(rs.Item("Rate"), 2, 0)) + "&nbsp; per unit </td></tr>");
               }
                if(rs.Item("HoursOrUnits") == "H"){
                  Response.Write(" <tr><td colspan='2'><b>HOURS WORKED </b></td><td width='20%' class='thinBB'>" + cStr(FormatNumber(rs.Item("TotalHours"), 2, 0)) + " total hours</td><td width='20%'><b>NUMBER OF MEN&nbsp;</b></td><td width='20%' class='thinBB'>" + rs.Item("NbrEmployees") + " total</td></tr>");
                }else{
                  Response.Write(" <tr><td colspan='2'><b>UNITS WORKED </b></td><td width='20%' class='thinBB'>" + cStr(FormatNumber(rs.Item("TotalUnits"), 0, 0)) + " total units</td><td width='20%'><b>NUMBER OF MEN&nbsp;</b></td><td width='20%' class='thinBB'>&nbsp;</td></tr>");
               }
                Response.Write(" <tr><td colspan='2'><b>TOTAL LABOR COSTS </b></td><td width='20%' class='thinBB'>$" + "&nbsp;" + "</td><td width='25%'><b>EQUIPMENT COST&nbsp;</b></td><td width='15%' class='thinBB'>$" + "&nbsp;" + "</td></tr>");
                Response.Write(" <tr><td colspan='2'><b>MATERIAL COSTS </b></td><td width='20%' class='thinBB'>$" + cStr(FormatNumber(rs.Item("MC"), 2, 0)) + "</td><td width='25%'><b>OTHER COST&nbsp;</b></td><td width='15%' class='thinBB'>$" + "&nbsp;" + "</td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='3'><b>DESCRIBE WORK OR SERVICE PERFORMED: </b></td><td colspan='2'>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='4' rowspan='6'>" + rs.Item("WDesc") + "</td><td>&nbsp;</td></tr>");
                Response.Write(" <tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr>");
                Response.Write(" <tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='5' class='thinBB'>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                string sAmount = "";
                if(rs.Item("HoursOrUnits") == "H"){
                  if((rs.Item("Rate") == "0" || rs.Item("TotalHours")=="0")  && (rs.Item("MC") == "0")){
                    sAmount = "<font color='red'>N/A</font>";
                  }else{
                    sAmount = cStr(FormatNumber(cStr(cDbl(rs.Item("Rate")) * cDbl(rs.Item("TotalHours"))), 2) + rs.Item("MC") );
                    if(rs.Item("Rate") == "0" || rs.Item("TotalHours") == "0"){
                       sAmount = "<font color='red'>" + sAmount + "</font>";
                   }
                 }

                }else{
                  if((rs.Item("Rate") == "0" || rs.Item("TotalUnits")=="0") && (rs.Item("MC") == "0")){
                    sAmount = "<font color='red'>N/A</font>";
                  }else{
                    sAmount = cStr(FormatNumber(cStr(cDbl(rs.Item("Rate")) * cDbl(rs.Item("TotalUnits"))), 2)  + rs.Item("MC"));
                    if(rs.Item("Rate") == "0" || rs.Item("TotalHours")=="0"){
                       sAmount = "<font color='red'>$" + sAmount + "</font>" ;
                   }
                 }
               }

                Response.Write(" <tr><td colspan='4' align='Right'><b>TOTAL AMOUNT OF INVOICE: &nbsp;</b></td><td class='thinBB'>$ " + sAmount + "</td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                Response.Write(" <tr><td width='20%'><b>LOCATION </b></td><td colspan='2' class='thinBB'>" + rs.Item("Name") + "</td><td width='25%' align='right'><b>DATE: &nbsp;</b></td><td width='15%' class='thinBB'>" + cStr(System.DateTime.Now) + "</td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='2'><b>MANAGER'S SIGNATURE </b></td><td colspan='3' class='thinBB'>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='5'><b>THIS FORM WILL BE USED FOR ALL WORK OR SERVICES PERFORMED FOR OTHER THAN</b></td></tr>");
                Response.Write(" <tr><td colspan='5'><b>LOADING/UNLOADING AT ALL TERMINAL OR OTHER LOCATION.  BE BRIEF IN YOUR</b></td></tr>");
                Response.Write(" <tr><td colspan='5'><b>EXPLANATION BUT EXPLICIT AS TO WORK OR SERVICES PERFORMED.  ATTACH 2 COPIES</b></td></tr>");
                Response.Write(" <tr><td colspan=//5//><b>OF THE INVOICE TO THIS FORM && SUBMIT PROMPTLY FOR PROCESSING.</b></td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                sPageBreak = "<h3>&nbsp;</h3>";
                 }

             if( (sRptType == "InvoicesOnly") || (sRptType == "Both") ){
                Response.Write(" <tr><td colspan='5' align='center'><b>" + sPageBreak + sInvoicesTitle + "</b></td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");

                Response.Write(" <tr><td width='20%'><b>Date: </b></td><td width='20%' class='thinBB'>" + rs.Item("WorkDate") + "</td><td colspan='3'>&nbsp;</td><td widht='15%'>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='5'>" + Str(120, "*") + "</td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");

                string sHours;
                string sLine;
                string sUnits;
                string sAmount;

                if(rs.Item("HoursOrUnits") == "H"){
                
                        if( (System.Convert.ToInt32(rs.Item("NbrEmployees")) > 0) ){
                          sHours = cStr(cDbl(rs.Item("TotalHours")) / cDbl(rs.Item("NbrEmployees")));
                        }else{
                          sHours = "0";
                       }

                        sLine = rs.Item("NbrEmployees") + " men @ " + cStr(FormatNumber(sHours, 2, 0)) + " hrs";
                }else{
                        sUnits = rs.Item("TotalUnits");

                        sLine = sUnits + " units @ " + cStr(FormatNumber(rs.Item("Rate"), 2, 0)) + " per unit";
               }

                if(System.Convert.ToInt32(rs.Item("MC")) > 0){
                  sLine = sLine + "<br>" + "Material Costs: $" + FormatNumber(rs.Item("MC"),2);
               }
                if(rs.Item("InvoiceNumber") != ""){
                  sLine = sLine + "<br>" + "Invoice Number: " + rs.Item("InvoiceNumber") + " - Vendors: " + rs.Item("Vendors") + "<br>";
               }

                Response.Write(" <tr><td colspan='5'>" + sLine + "</td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='4' rowspan='6'>" + rs.Item("WDesc") + "</td><td width='10%'>&nbsp;</td></tr>");
                Response.Write(" <tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr>");
                Response.Write(" <tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr>");

                if(rs.Item("HoursOrUnits") == "H"){
                  if((rs.Item("Rate") == "0" || rs.Item("TotalHours") == "0")  && (rs.Item("MC") == "0")){
                    sAmount = "<font color='red'>N/A</font>";
                  }else{
                    sAmount = cStr(FormatNumber(cStr(cDbl(rs.Item("Rate")) * cDbl(rs.Item("TotalHours"))), 2) + rs.Item("MC") );
                    if(rs.Item("Rate") == "0" || rs.Item("TotalHours") =="0"){
                       sAmount = "<font color='red'>$" + sAmount + "</font>";
                   }
                 }
                }else{
                  if((rs.Item("Rate") == "0" || rs.Item("TotalHours") == "0")  && (rs.Item("MC") == "0")){
                    sAmount = "<font color='red'>N/A</font>";
                  }else{
                    sAmount = cStr(FormatNumber(cStr(cDbl(rs.Item("Rate")) * cDbl(rs.Item("TotalUnits"))), 2) + rs.Item("MC")  );
                    if((rs.Item("Rate") == "0" || rs.Item("TotalHours")=="0")){
                       sAmount = "<font color='red'>$" + sAmount + "</font>";
                   }
                 }
               }

                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='5'>" + Str(120, "*") + "</td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                if(rs.Item("HoursOrUnits") == "H"){
                  Response.Write(" <tr><td width='20%'><b>Total Units: </b></td><td width='20%' class='thinBB'>" + "&nbsp;" + "</td><td colspan='3'>&nbsp;</td></tr>");
                  Response.Write(" <tr><td width='20%'><b>Total Hours: </b></td><td width='20%' class='thinBB'>" + cStr(FormatNumber(rs.Item("TotalHours"), 2, 0)) + "</td><td colspan='3'>&nbsp;</td></tr>");
                  Response.Write(" <tr><td width='20%'><b>Total Invoice: </b></td><td  width='20%' class='thinBB'>" + sAmount + "</td><td colspan='3'>&nbsp;</td></tr>");
                }else{
                  Response.Write(" <tr><td width='20%'><b>Total Units: </b></td><td width='20%' class='thinBB'>" + cStr(FormatNumber(rs.Item("TotalUnits"), 0, 0)) + "</td><td colspan='3'>&nbsp;</td></tr>");
                  Response.Write(" <tr><td width='20%'><b>Total Hours: </b></td><td width='20%' class='thinBB'>" + "&nbsp;" + "</td><td colspan='3'>&nbsp;</td></tr>");
                  Response.Write(" <tr><td width='20%'><b>Total Invoice: </b></td><td  width='20%' class='thinBB'>" + sAmount + "</td><td colspan='3'>&nbsp;</td></tr>");
               }
                Response.Write(" <tr><td colspan='5'>&nbsp;</td></tr>");
                Response.Write(" <tr><td width='20%'><b>Facility Manager: </b></td><td colspan='2' class='thinBB'>&nbsp;</td><td colspan='2'>&nbsp;</td></tr>");

                ListAttachments(rs.Item("Id"));


                sPageBreak = "<h3>&nbsp;</h3>";
                 }


             }
         }

          if(cStr(Request["PrintPreview"]).Length > 0){
            Response.Write("  <tr><td colspan='5' align='right'><a href='" + sPreviewLink + "'>Printer Friendly</a></td></tr>");
          }else{
            Response.Write("  <tr><td colspan='5'>&nbsp;</td></tr>");
         }
          Response.Write("</table>");

        }

        public void ListAttachments(string RebillId){

            DataReader rsATT;
            int rsAtt_numRows = 0;

            string strSQL = " ";
            strSQL +=  "SELECT  Title, Path ";
            strSQL +=  "FROM    RebillAttachments ";
            strSQL +=  "WHERE  RebillDetailId = " + RebillId;

            rsATT = new DataReader(strSQL);
            rsATT.Open();
            rsAtt_numRows = rsATT.RecordCount;

            bool attFirstRow = true;

              while(! rsATT.EOF){
                  rsATT.Read();
                  if(cStr(Request["PrintPreview"]).Length > 0){

                    if (attFirstRow){
                         attFirstRow = false;
                         Response.Write("  <tr><td colspan='5'>&nbsp;</td></tr><tr>");
                         Response.Write("  <td colspan='5' class='cellTopBottomBorder'>Documents</td>");
                         Response.Write("  </tr>");
                    }

                     Response.Write("  <tr><td colspan='5'><a href='" + rsATT.Item("Path") + "' target=_blank>" + rsATT.Item("Title") + "</a></td></tr>");

                  }else{

                        Response.Write("<tr><td colspan='5'><iframe style='page-break-before: always; height:10in; width:7.75in;' frameBorder=0 SCROLLING=no  src='" + rsATT.Item("Path") + "'></iframe></td></tr>" );
                 }
               }  

        }
    }
}