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
    public partial class PayrollDetailReport2 : PageBase
    {

        public string sselFacilities, sfromDateDetail, stoDateDetail, wFacilities, wDateRange;
        public string RType,strSQL,sWhere;
        public int rs_numRow;
        public int rs_numRows;
        public int rsSum_numRows;
        public int iOpen;
        public DataReader rs; 
        public DataReader rsSum;
        public DataReader rsTemp;
        public DataReader rsTempSum;
        public int rsTempSum_numRows; 

        public string  SummaryEmpNumber;
        public string SummaryLastName;
        public string SummaryFirstName;

        public double  TnRegHP,    TnRegPA, TnOTHP, TnOTPA, TnHP, TnPA;
        public double  TnsEmp, TnsRegHP, TnsRegPA, TnsOTHP, TnsOTPA;
        public double  TnfEmp, TnfRegHP, TnfRegPA, TnfOTHP, TnfOTPA;
        public string sfHTML;

        public double  nRegHP, nRegPA, nOTHP,  nOTPA, nHP, nPA;
        public double  nfEmp, nfRegHP, nfRegPA, nfOTHP, nfOTPA;
        public string sTitle, sPageBreak;
        public string sWroteDetail;
        public int sumiRow;

        public bool isSummary = false;
        public string sOpenRemoteFac = "";

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin, User");

            RType = cStr(Request["Type"]);


            sselFacilities    = cStr(Request["selFacilities"]);
            sfromDateDetail   = cStr(Request["fromDateDetail"]);
            stoDateDetail = cStr(Request["toDateDetail"]);

            Session["LastStartDate"] = sfromDateDetail;
            Session["LastEndDate"]   = stoDateDetail;

            isSummary = false;

            if (Len(cStr(Request["summary"])) > 0){
              isSummary = true;
            }

            if(sselFacilities != ""){
              wFacilities = "  AND (EmployeeTaskWorked.FacilityID IN  (" + sselFacilities + ") ) ";
            }else{
              wFacilities = "  AND (EmployeeTaskWorked.FacilityID IN  (" + Session["FacilityID"] + ") ) ";
            }

            wDateRange = " AND (WorkDate Between '" + sfromDateDetail + "' AND '" + stoDateDetail + "') ";

            sWhere = wFacilities + wDateRange;



            strSQL = "";
            strSQL +=  "         SELECT IsNull(Count(*), 0) as theCount from  EmployeeTaskWorked   ";
            strSQL +=  "          WHERE PayrollStatus = 'OPEN'  ";
            strSQL = strSQL + sWhere;
            rs = new DataReader(strSQL);
            rs.Open();

            rs_numRows = 0;

            iOpen = 0;
            if( !rs.EOF){
                rs.Read();
              iOpen = cInt(rs.Item("theCount"));
            }

            if (iOpen > 0){
     
                 strSQL = "";
                 strSQL +=  "   SELECT     Facility.Name ";
                 strSQL +=  "   FROM         EmployeeTaskWorked INNER JOIN ";
                 strSQL +=  "    Employee ON EmployeeTaskWorked.EmployeeId = Employee.Id INNER JOIN ";
                 strSQL +=  "    Facility ON Employee.FacilityID = Facility.Id ";
                 strSQL +=  "   WHERE EmployeeTaskWorked.PayrollStatus = 'OPEN'  ";
     
                 if(sselFacilities != ""){
                   strSQL +=  " AND (Employee.FacilityID  NOT IN  (" + sselFacilities + ") ) ";
                 }else{
                   strSQL +=  " AND (Employee.FacilityID  <> " + Session["FacilityID"] + " ) ";
                }
     
                 strSQL = strSQL + sWhere;
                 rs = new DataReader(strSQL);
                 rs.Open();
                 rs_numRows = 0;
     
                 sOpenRemoteFac = "";
                 if( !rs.EOF){
                     rs.Read();
                   sOpenRemoteFac = rs.GetString(2,0,"", "<br>", "");
                 }
     
            }

            strSQL = "";
            strSQL +=  "         SELECT EmployeeTaskWorked.ID, EmployeeTaskWorked.FacilityID,   ";
            strSQL +=  "                WorkDate, UPM=IsNull(UPM,0), TaskCode, Name, TaskType=0,";
            strSQL +=  "                EmployeeNumber, LastName, FirstName, ShiftID, OverTimeCalcBasis  ";
            strSQL +=  "           FROM EmployeeTaskWorked                                      ";
            strSQL +=  "                INNER JOIN Employee ON EmployeeId = Employee.Id         ";
            strSQL +=  "                INNER JOIN Tasks ON TaskID = Tasks.Id                   ";
            strSQL +=  "                INNER JOIN Facility                                     ";
            strSQL +=  "                ON EmployeeTaskWorked.FacilityID = Facility.Id          ";
            strSQL +=  "          WHERE (HireDate <= WorkDate AND HireDate Is NOT NULL)         ";
            strSQL +=  "            AND (TaskID <> 0 AND OtherTaskID=0)                         ";
            strSQL +=  "            AND (EmployeeTaskWorked.PayrollStatus <> 'OPEN')            ";
            strSQL = strSQL + sWhere;
            strSQL +=  "     UNION                                                              ";
            strSQL +=  "         SELECT EmployeeTaskWorked.ID, EmployeeTaskWorked.FacilityID,   ";
            strSQL +=  "                WorkDate, UPM=IsNull(UPM,0), TaskCode, Name, TaskType=1,";
            strSQL +=  "                EmployeeNumber, LastName, FirstName, ShiftID, OverTimeCalcBasis ";
            strSQL +=  "           FROM EmployeeTaskWorked                                      ";
            strSQL +=  "                INNER JOIN Employee ON EmployeeId = Employee.Id         ";
            strSQL +=  "                INNER JOIN OtherTasks ON OtherTaskID = OtherTasks.Id    ";
            strSQL +=  "                INNER JOIN Facility                                     ";
            strSQL +=  "                ON EmployeeTaskWorked.FacilityID = Facility.Id          ";
            strSQL +=  "          WHERE (HireDate <= WorkDate AND HireDate Is NOT NULL)         ";
            strSQL +=  "            AND (OtherTaskID <> 0 AND TaskID=0)                         ";
            strSQL +=  "            AND (EmployeeTaskWorked.PayrollStatus <> 'OPEN')            ";
            strSQL = strSQL + sWhere;
            strSQL +=  "       Order By EmployeeTaskWorked.FacilityID, LastName, FirstName, WorkDate, TaskCode ";


            if (RType == "All" || RType == "Perm"){
               rs = new DataReader(strSQL);
               rs.Open();
            }

            rs_numRows = 0;



            strSQL = "";
            strSQL +=  "         SELECT EmployeeTaskWorked.ID, EmployeeTaskWorked.FacilityID,   ";
            strSQL +=  "                WorkDate, UPM=IsNull(UPM,0), TaskCode, Name, TaskType=0,";
            strSQL +=  "                TempNumber, LastName, FirstName, OverTimeCalcBasis,     ";
            strSQL +=  "                SourceName, EmploymentSource.Id AS SourceID, ShiftID    ";
            strSQL +=  "           FROM EmployeeTaskWorked                                      ";
            strSQL +=  "                INNER JOIN Employee ON EmployeeId = Employee.Id         ";
            strSQL +=  "                INNER JOIN Tasks ON TaskID = Tasks.Id                   ";
            strSQL +=  "                INNER JOIN Facility                                     ";
            strSQL +=  "                ON EmployeeTaskWorked.FacilityID = Facility.Id          ";
            strSQL +=  "                INNER JOIN EmploymentSource                             ";
            strSQL +=  "                ON EmploymentSourceId = EmploymentSource.Id             ";
            strSQL +=  "          WHERE (HireDate Is NULL OR HireDate > WorkDate)               ";
            strSQL +=  "            AND (TaskID <> 0 AND OtherTaskID=0)                         ";
            strSQL +=  "            AND (EmployeeTaskWorked.PayrollStatus <> 'OPEN')            ";
            strSQL = strSQL + sWhere;
            strSQL +=  "     UNION                                                              ";
            strSQL +=  "         SELECT EmployeeTaskWorked.ID, EmployeeTaskWorked.FacilityID,   ";
            strSQL +=  "                WorkDate, UPM=IsNull(UPM,0), TaskCode, Name, TaskType=1,";
            strSQL +=  "                TempNumber, LastName, FirstName, OverTimeCalcBasis,     ";
            strSQL +=  "                SourceName, EmploymentSource.Id AS SourceID, ShiftID    ";
            strSQL +=  "           FROM EmployeeTaskWorked                                      ";
            strSQL +=  "                INNER JOIN Employee ON EmployeeId = Employee.Id         ";
            strSQL +=  "                INNER JOIN OtherTasks ON OtherTaskID = OtherTasks.Id    ";
            strSQL +=  "                INNER JOIN Facility                                     ";
            strSQL +=  "                ON EmployeeTaskWorked.FacilityID = Facility.Id          ";
            strSQL +=  "                INNER JOIN EmploymentSource                             ";
            strSQL +=  "                ON EmploymentSourceId = EmploymentSource.Id             ";
            strSQL +=  "          WHERE (HireDate Is NULL OR HireDate > WorkDate)               ";
            strSQL +=  "            AND (OtherTaskID <> 0 AND TaskID=0)                         ";

            strSQL +=  "            AND (EmployeeTaskWorked.PayrollStatus <> 'OPEN')            ";
            strSQL = strSQL + sWhere;
            strSQL +=  "       Order By EmployeeTaskWorked.FacilityID, SourceName, SourceID,    ";
            strSQL +=  "                LastName, FirstName, WorkDate, TaskCode                 ";

            if (RType == "All" || RType == "Temp"){
               rsTemp = new DataReader(strSQL);
               rsTemp.Open();
            }
            rs_numRows = 0;


            string sSumSQL = "";
            sSumSQL = sSumSQL + "   SELECT EmployeeTaskWorked.FacilityID, TaskCode, GLAcctNumber,                    ";
            sSumSQL = sSumSQL + "          SUM (CASE PayMultiplier WHEN 1 Then HoursPaid Else 0 END) AS 'RegHrs',    ";
            sSumSQL = sSumSQL + "          SUM (CASE PayMultiplier WHEN 1 Then PayAmount Else 0 END) AS 'RegAmount', ";
            sSumSQL = sSumSQL + "          SUM (CASE PayMultiplier WHEN 1 Then 0 Else HoursPaid END) AS 'OTHrs',     ";
            sSumSQL = sSumSQL + "          SUM (CASE PayMultiplier WHEN 1 Then 0 Else PayAmount END) AS 'OTAmount',  ";
            sSumSQL = sSumSQL + "          SUM (HoursPaid) AS 'TotalHrs',                                            ";
            sSumSQL = sSumSQL + "          Sum (PayAmount) AS 'TotalAmount'                                          ";
            sSumSQL = sSumSQL + "     FROM EmployeeTaskWorkedPay INNER JOIN                                          ";
            sSumSQL = sSumSQL + "          EmployeeTaskWorked ON EmployeeTaskWorkedId = EmployeeTaskWorked.Id        ";
            sSumSQL = sSumSQL + "          INNER JOIN Employee ON EmployeeTaskWorked.EmployeeId = Employee.Id        ";
            sSumSQL = sSumSQL + "          INNER JOIN Tasks ON TaskID = Tasks.Id                                     ";

            sSumSQL = sSumSQL + "      WHERE (HireDate <= WorkDate AND HireDate Is NOT NULL)                           ";
            sSumSQL = sSumSQL + "        AND (TaskID <> 0 AND OtherTaskID=0)                                           ";

            sSumSQL = sSumSQL + "      AND (EmployeeTaskWorked.PayrollStatus <> 'OPEN')                              ";
            sSumSQL = sSumSQL + sWhere;
            sSumSQL = sSumSQL + " Group By EmployeeTaskWorked.FacilityID, TaskCode, GLAcctNumber                     ";
            sSumSQL = sSumSQL + "   UNION                    ";
            sSumSQL = sSumSQL + "   SELECT EmployeeTaskWorked.FacilityID, TaskCode, GLAcctNumber,                    ";
            sSumSQL = sSumSQL + "          SUM (CASE PayMultiplier WHEN 1 THEN HoursPaid ELSE 0 END) AS 'RegHrs',    ";
            sSumSQL = sSumSQL + "          SUM (CASE PayMultiplier WHEN 1 THEN PayAmount ELSE 0 END) AS 'RegAmount', ";
            sSumSQL = sSumSQL + "          SUM (CASE PayMultiplier WHEN 1 THEN 0 ELSE HoursPaid END) AS 'OTHrs',     ";
            sSumSQL = sSumSQL + "          SUM (CASE PayMultiplier WHEN 1 THEN 0 ELSE PayAmount END) AS 'OTAmount',  ";
            sSumSQL = sSumSQL + "          SUM (HoursPaid) AS 'TotalHrs',                                            ";
            sSumSQL = sSumSQL + "          Sum (PayAmount) AS 'TotalAmount'                                          ";
            sSumSQL = sSumSQL + "     FROM EmployeeTaskWorkedPay INNER JOIN                                          ";
            sSumSQL = sSumSQL + "          EmployeeTaskWorked ON EmployeeTaskWorkedId = EmployeeTaskWorked.Id        ";
            sSumSQL = sSumSQL + "          INNER JOIN Employee ON EmployeeTaskWorked.EmployeeId = Employee.Id        ";
            sSumSQL = sSumSQL + "          INNER JOIN OtherTasks ON OtherTaskID = OtherTasks.Id                      ";

            sSumSQL = sSumSQL + "    WHERE (HireDate <= WorkDate AND HireDate Is NOT NULL)                           ";
            sSumSQL = sSumSQL + "      AND (OtherTaskID <> 0 AND TaskID=0)                                           ";

            sSumSQL = sSumSQL + "      AND (EmployeeTaskWorked.PayrollStatus <> 'OPEN')                              ";
            sSumSQL = sSumSQL + sWhere;
            sSumSQL = sSumSQL + " Group By EmployeeTaskWorked.FacilityID, TaskCode, GLAcctNumber                     ";

            if (RType == "All" || RType == "Perm"){
               rsSum = new DataReader(sSumSQL);
               rsSum.Open();
            }
            rsSum_numRows = 0;



            sSumSQL = "";
            sSumSQL = sSumSQL + "   SELECT EmployeeTaskWorked.FacilityID, TaskCode, GLAcctNumber,                    ";
            sSumSQL = sSumSQL + "          SUM (CASE PayMultiplier WHEN 1 Then HoursPaid Else 0 END) AS 'RegHrs',    ";
            sSumSQL = sSumSQL + "          SUM (CASE PayMultiplier WHEN 1 Then PayAmount Else 0 END) AS 'RegAmount', ";
            sSumSQL = sSumSQL + "          SUM (CASE PayMultiplier WHEN 1 Then 0 Else HoursPaid END) AS 'OTHrs',     ";
            sSumSQL = sSumSQL + "          SUM (CASE PayMultiplier WHEN 1 Then 0 Else PayAmount END) AS 'OTAmount'   ";
            sSumSQL = sSumSQL + "     FROM EmployeeTaskWorkedPay INNER JOIN                                          ";
            sSumSQL = sSumSQL + "          EmployeeTaskWorked ON EmployeeTaskWorkedId = EmployeeTaskWorked.Id        ";
            sSumSQL = sSumSQL + "          INNER JOIN Employee ON EmployeeTaskWorked.EmployeeId = Employee.Id        ";
            sSumSQL = sSumSQL + "          INNER JOIN Tasks ON TaskID = Tasks.Id                                     ";
            //sSumSQL = sSumSQL + "    WHERE (TempEmployee=1) AND (TaskID <> 0 AND OtherTaskID=0)                      ";

            sSumSQL = sSumSQL + "      WHERE (HireDate Is NULL OR HireDate > WorkDate)               ";
            sSumSQL = sSumSQL + "             AND (TaskID <> 0 AND OtherTaskID=0)                         ";

            sSumSQL = sSumSQL + "      AND (EmployeeTaskWorked.PayrollStatus <> 'OPEN')                              ";
            sSumSQL = sSumSQL + sWhere;
            sSumSQL = sSumSQL + " Group By EmployeeTaskWorked.FacilityID, TaskCode, GLAcctNumber                     ";
            sSumSQL = sSumSQL + "   UNION                    ";
            sSumSQL = sSumSQL + "   SELECT EmployeeTaskWorked.FacilityID, TaskCode, GLAcctNumber,                    ";
            sSumSQL = sSumSQL + "          SUM (CASE PayMultiplier WHEN 1 Then HoursPaid Else 0 END) AS 'RegHrs',    ";
            sSumSQL = sSumSQL + "          SUM (CASE PayMultiplier WHEN 1 Then PayAmount Else 0 END) AS 'RegAmount', ";
            sSumSQL = sSumSQL + "          SUM (CASE PayMultiplier WHEN 1 Then 0 Else HoursPaid END) AS 'OTHrs',     ";
            sSumSQL = sSumSQL + "          SUM (CASE PayMultiplier WHEN 1 Then 0 Else PayAmount END) AS 'OTAmount'   ";
            sSumSQL = sSumSQL + "     FROM EmployeeTaskWorkedPay INNER JOIN                                          ";
            sSumSQL = sSumSQL + "          EmployeeTaskWorked ON EmployeeTaskWorkedId = EmployeeTaskWorked.Id        ";
            sSumSQL = sSumSQL + "          INNER JOIN Employee ON EmployeeTaskWorked.EmployeeId = Employee.Id        ";
            sSumSQL = sSumSQL + "          INNER JOIN OtherTasks ON TaskID = OtherTasks.Id                           ";

            sSumSQL = sSumSQL + "      WHERE (HireDate Is NULL OR HireDate > WorkDate)               ";
            sSumSQL = sSumSQL + "             AND (OtherTaskID <> 0 AND TaskID=0)                         ";

            sSumSQL = sSumSQL + "      AND (EmployeeTaskWorked.PayrollStatus <> 'OPEN')                              ";
            sSumSQL = sSumSQL + sWhere;
            sSumSQL = sSumSQL + " Group By EmployeeTaskWorked.FacilityID, TaskCode, GLAcctNumber                     ";


            if ( RType == "All" || RType == "Temp"){
               rsTempSum = new DataReader(sSumSQL);
               rsTempSum.Open();
            }

            rsTempSum_numRows = 0;

            int sumiRow = 0;

        }

        public void ShowDetailReport(){

          string sFrom = "";
          string sTo = "";
          int iRow = 0;
          int iRec = 0;
          string sTitle = "Permanent Employee Payroll Report <br>" + sfromDateDetail + " - " + stoDateDetail;

          string sPageBreak = "";
          string sPreviewLink = "javascript:document.form1.submit();";

          if(cStr(Request["PrintPreview"]).Length > 0){
            Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='480' align='center'>");
            Response.Write("  <tr><td colspan='10'>&nbsp;</td></tr>");
            Response.Write("  <tr><td colspan='10' align='right'><a href='" + sPreviewLink + "'>Printer Friendly</a></td></tr>");
           }else{
            Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='600' align='center'>");
            Response.Write("  <tr><td colspan='10'>&nbsp;</td></tr>");
          }

          if(rs.EOF){
            Response.Write("  <tr><td colspan='10'>&nbsp;</td></tr>");
            Response.Write("  <tr><td colspan='10'>No Permanent Employee records found.</td></tr>");
          }else{

            string sFacility   = "";
            int sFacilityID = 0;
            int sEmpNum = 0;
            int SumiRow = 0;

            while (! rs.EOF){
              rs.Read();

             string strSQL = ""; 
             strSQL +=  "         SELECT EmployeeTaskWorkedId, PayMultiplier,                    ";
             strSQL +=  "                PM=Case When PayMultiplier=1 Then 1 Else 2 End,         ";
             strSQL +=  "                PR=IsNull(PayRate,0), HP=IsNull(HoursPaid,0),           ";
             strSQL +=  "                PA=IsNull(PayAmount,0)                                  ";
             strSQL +=  "           FROM EmployeeTaskWorkedPay                                   ";
             strSQL +=  "       Order By EmployeeTaskWorkedId, PayMultiplier                     ";
             strSQL +=  "       WHERE EmployeeTaskWorkedId = " + rs.Item("ID");

             DataReader rsPay = new DataReader(strSQL);

              if(sFacility != rs.Item("Name") ){

                    if(sEmpNum != 0){
                        WriteEmployeeTotals();
                        WriteFacilityTotals(sFacility, cStr(sFacilityID));
                        iRow = 0;
                        SumiRow = 0;
                    }
                    sFacility   = rs.Item("Name");
                    sFacilityID = cInt(rs.Item("FacilityID"));
                    sEmpNum = 0;

              if (!isSummary){
                  Response.Write("  <tr><td colspan='9' align='center'><b>" + sPageBreak + sTitle + "</b></td><td>&nbsp;</td></tr>");
                  Response.Write("  <tr><td colspan='9' align='center'><b>" + sFacility + "</b></td><td>&nbsp;</td></tr>");
              }else{
                Response.Write("  <tr><td colspan='9' align='center'><b>" +  sTitle + "</b></td><td>&nbsp;</td></tr>");
                Response.Write("  <tr><td colspan='9' align='center'><b>" + sFacility + "</b></td><td>&nbsp;</td></tr>");
                Response.Write("<tr><td colspan='10'>&nbsp;</td></tr>");
                Response.Write("    <td align='Left'   width='10%' class='cellTopBottomBorder'>&nbsp;</td>");
                Response.Write("    <td align='Left'   width='10%' class='cellTopBottomBorder'>&nbsp;Name</td>");
                Response.Write("    <td align='right'  width='10%' class='cellTopBottomBorder'>&nbsp;</td>");
                Response.Write("    <td align='right'  width='10%' class='cellTopBottomBorder'>&nbsp;</td>");
                Response.Write("    <td align='right'  colspan='2' class='cellTopBottomBorder'>Regular Pay</td>");
                Response.Write("    <td align='right'  colspan='2' class='cellTopBottomBorder'>Overtime Pay</td>");
                Response.Write("    <td align='right'  colspan='2'  class='cellTopBottomBorder'>&nbsp;Total</td>");
                Response.Write("</tr>");
             }

              if(rsPay.EOF){
                  Response.Write("  <tr><td colspan='10'>&nbsp;</td></tr>");
                  Response.Write("  <tr><td colspan='10'>Payroll needs to be re-calculated for this period.</td></tr>");
              }
                sPageBreak = "<h3>&nbsp;</h3>";
              }

              if (! rsPay.EOF){
                rsPay.Read();
                if( (sEmpNum != cInt(rs.Item("EmployeeNumber")))){
                          if(sEmpNum != 0){
                                WriteEmployeeTotals();
                                iRow = 0;
                 }

                 if (isSummary){
                    //Response.Write("<tr><td colspan=//10//>&nbsp;</td></tr>");
                    //Response.Write("<tr>";
                    //Response.Write("    <td align=//Left// ><b>" + rs.Item("EmployeeNumber") + "</b></td>";
                    //Response.Write("    <td align=//Left// colspan=//9//><b>" + rs.Item("LastName") + ", " + rs.Item("FirstName") + "</b></td>";
                    //Response.Write("</tr>";

                }else{

                    Response.Write("<tr><td colspan='10'>&nbsp;</td></tr>");
                    Response.Write("<tr>");
                    Response.Write("    <td align='Left' ><b>" + rs.Item("EmployeeNumber") + "</b></td>");
                    Response.Write("    <td align='Left' colspan='9'><b>" + rs.Item("LastName") + ", " + rs.Item("FirstName") + "</b></td>");
                    Response.Write("</tr>");

                    Response.Write("<tr><td colspan='10'>&nbsp;</td></tr>");
                    Response.Write("    <td align='Left'   width='10%' class='cellTopBottomBorder'>&nbsp;</td>");
                    Response.Write("    <td align='Left'   width='10%' class='cellTopBottomBorder'>&nbsp;Task</td>");
                    Response.Write("    <td align='right'  width='10%' class='cellTopBottomBorder'>Rate</td>");
                    Response.Write("    <td align='right'  width='10%' class='cellTopBottomBorder'>UPM</td>");
                    Response.Write("    <td align='right'  colspan='2' class='cellTopBottomBorder'>Regular Pay</td>");
                    Response.Write("    <td align='right'  colspan='2' class='cellTopBottomBorder'>Overtime Pay</td>");
                    Response.Write("    <td align='right'  colspan='2'>&nbsp;</td>");
                    Response.Write("</tr>");

                }

                    sEmpNum = cInt(rs.Item("EmployeeNumber"));

                    SummaryEmpNumber = rs.Item("EmployeeNumber");
                    SummaryLastName = rs.Item("LastName");
                    SummaryFirstName = rs.Item("FirstName");

               }

                string rowColor;  
                iRow=iRow+1;
                if(iRow % 2 == 0){
                  rowColor = "reportOddLine";
                }else{
                  rowColor = "reportEvenLine";
               }

                 string sHTML = "";
                    sHTML = sHTML + "<tr class='" + rowColor + "'>";
                    sHTML = sHTML +  "    <td align='Left'    width='10%'>" + rs.Item("WorkDate")   + "</td>";
                    sHTML = sHTML +  "    <td align='Left'    width='10%'>&nbsp;" + rs.Item("TaskCode");
                    if (cInt(rs.Item("ShiftID")) > 1){
                       sHTML = sHTML + " (" + cStr(rs.Item("ShiftID")) + ")";
                   }
                    sHTML = sHTML +  "</td>";
                    sHTML = sHTML +  "    <td align='right'   width='10%'>" + cStr(FormatNumber(rsPay.Item("PR"), 2, 0)) + "</td>";
                    sHTML = sHTML +  "    <td align='right'   width='10%'>" + cStr(FormatNumber(rs.Item("UPM"), 2, 0))   + "</td>";
                        nHP = 0;
                        nPA = 0;
                        iRec = 0;
                    while (!rsPay.EOF ){
                          rsPay.Read();
                          iRec = iRec + 1;
                          if(rsPay.Item("PayMultiplier") == "1"){

                             sHTML = sHTML +  "    <td align='right' width='10%'>" + cStr(FormatNumber(rsPay.Item("HP"), 2, 0)) + "</td>";
                             sHTML = sHTML +  "    <td align='right' width='10%'>" + cStr(FormatNumber(rsPay.Item("PA"), 2, 0)) + "</td>";
                             if(rs.Item("TaskType") == "0"){
                              nRegHP = nRegHP + cDbl(rsPay.Item("HP"));
                             }
                             nRegPA = nRegPA + cDbl(rsPay.Item("PA"));

                          }else{

                            if(iRec == 1){
                                sHTML = sHTML +  "    <td align='right' width='10%'>.00</td>";
                                sHTML = sHTML +  "    <td align='right' width='10%'>.00</td>";
                            }
                            if(rs.Item("TaskType") == "0"){
                                nHP = nHP + cDbl(rsPay.Item("HP"));
                            }
                            nPA = nPA + cDbl(rsPay.Item("PA"));
                            if(rs.Item("TaskType") == "0"){
                                nOTHP = nOTHP + cDbl(rsPay.Item("HP"));
                            }
                            nOTPA = nOTPA + cDbl(rsPay.Item("PA"));
                          }

                } //End Loop

                sHTML = sHTML +  "    <td align='right' width='10%'>" + cStr(FormatNumber(cStr(nHP), 2, 0)) + "</td>";
                sHTML = sHTML +  "    <td align='right' width='10%'>" + cStr(FormatNumber(cStr(nPA), 2, 0)) + "</td>";
                sHTML = sHTML +  "</tr>";
                if (!isSummary ){
                        Response.Write(sHTML);
                 }

                if(sFacility != rs.Item("Name")){
                  sFacility = Trim(rs.Item("Name"));
                  sFacilityID = cInt(rs.Item("FacilityID"));
                  sEmpNum = cInt(rs.Item("EmployeeNumber"));
                }

                 }


            } //End Loop

            if(iRow > 0){
              WriteEmployeeTotals();
                  WriteFacilityTotals(sFacility, cStr(sFacilityID));
               }
            Response.Write("<tr><td colspan='10'>&nbsp;</td></tr>");

         }

          if( RType != "All" && cStr(Request["PrintPreview"]).Length > 0){
            Response.Write("  <tr><td colspan='10' align='right'><a href='" + sPreviewLink + "'>Printer Friendly</a></td></tr>");
          }else{
            Response.Write("  <tr><td colspan='10'>&nbsp;</td></tr>");
         }
          Response.Write("</table>");

        }

        public void WriteEmployeeTotals(){

          string sumrowColor;
          string sTPay = cStr(FormatNumber(cStr(nRegPA + nOTPA), 2, 0));

          if (isSummary){

            sumiRow=sumiRow + 1;
            if(sumiRow % 2 == 0){
               sumrowColor = "reportOddLine";
            }else{
               sumrowColor = "reportEvenLine";
            }


             Response.Write(" <tr class='" + sumrowColor + "'>");
             Response.Write("    <td align='Left' ><b>" + SummaryEmpNumber + "</b></td>");
             Response.Write("    <td align='Left' colspan='3'><b>" + SummaryLastName + ", " + SummaryFirstName + "</b></td>");
             Response.Write("    <td align='right' width='10%' >" + cStr(FormatNumber(cStr(nRegHP), 2, 0)) + "</td>");
             Response.Write("    <td align='right' width='10%' >" + cStr(FormatNumber(cStr(nRegPA), 2, 0)) + "</td>");
             Response.Write("    <td align='right' width='10%' >" + cStr(FormatNumber(cStr(nOTHP), 2, 0)) + "</td>");
             Response.Write("    <td align='right' width='10%' >" + cStr(FormatNumber(cStr(nOTPA), 2, 0)) + "</td>");
             Response.Write("    <td colspan='2'   width='20%' align='right'> ( " + sTPay +     " )&nbsp;</td>");
             Response.Write("</tr>");

          }else{

          Response.Write(" <tr class='reportTotalLine'>");
          Response.Write("    <td colspan='3' align='center' class='cellTopBottomBorder'> ( " + sTPay +     " )&nbsp;</td>");
          Response.Write("    <td align='right' width='10%' class='cellTopBottomBorder'>" + "&nbsp;"           + "</td>");
          Response.Write("    <td align='right' width='10%' class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(nRegHP), 2, 0)) + "</td>");
          Response.Write("    <td align='right' width='10%' class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(nRegPA), 2, 0)) + "</td>");
          Response.Write("    <td align='right' width='10%' class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(nOTHP), 2, 0)) + "</td>");
          Response.Write("    <td align='right' width='10%' class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(nOTPA), 2, 0)) + "</td>");
          Response.Write("    <td align='right' width='10%' class='reportEvenLine'>" + "&nbsp;" + "</td>");
          Response.Write("    <td align='right' width='10%' class='reportEvenLine'>" + "&nbsp;" + "</td>");
          Response.Write("</tr>");

         }

          nfEmp   = nfEmp + 1;
          nfRegHP = nfRegHP + nRegHP;
          nfRegPA = nfRegPA + nRegPA;
          nfOTHP  = nfOTHP  + nOTHP;
          nfOTPA  = nfOTPA  + nOTPA;

          nRegHP = 0;
          nRegPA = 0;
          nHP    = 0;
          nPA    = 0;
          nOTHP  = 0;
          nOTPA  = 0;

        }

        public void WriteFacilityTotals(string OldFac, string OldFacID){

          string fEmps = cStr(nfEmp);
          string lc = "";


          Response.Write("<tr><td colspan='10'><table border='0' cellspacing='0' cellpadding='0' width='100%' align='center'>");

          if (isSummary){
             Response.Write("<tr><td colspan='10'>&nbsp;</td></tr>");
          }else{
             Response.Write("  <tr><td>&nbsp;</td><td colspan='9' align='center'><b>" + sPageBreak + sTitle + "</b></td></tr>");
             Response.Write("  <tr><td>&nbsp;</td><td colspan='9' align='center'><b>Summary for:&nbsp;" + OldFac + "</b></td></tr>");
             Response.Write("<tr><td colspan='10'>&nbsp;</td></tr>");
         }


          Response.Write("<tr>");
          // <td align=//right//  colspan=//2//>&nbsp;</td>";
          Response.Write("    <td align='right'  colspan='2' class='cellTopBottomBorder'>Employees</td>");
          Response.Write("    <td align='right'  colspan='2' class='cellTopBottomBorder'>Regular Pay</td>");
          Response.Write("    <td align='right'  colspan='2' class='cellTopBottomBorder'>Overtime Pay</td>");
          Response.Write("    <td align='right'  colspan='2' class='cellTopBottomBorder'>Total Pay</td>");
          Response.Write("    <td align='right'  colspan='2'>&nbsp;</td>");
          Response.Write("</tr>");

          Response.Write(" <tr class='reportTotalLine'>");
          //Response.Write("    <td align=//right// colspan=//2// class=//reportEvenLine//>&nbsp;</td>";
          Response.Write("    <td align='right' Width='15%' colspan='2' class='cellBottomBorder'>&nbsp;(" + fEmps +  ")&nbsp;</td>");
          Response.Write("    <td align='right' width='10%' class='cellBottomBorder'>" + cStr(FormatNumber(cStr(nfRegHP), 2, 0)) + "</td>");
          Response.Write("    <td align='right' width='12%' class='cellBottomBorder'>" + cStr(FormatNumber(cStr(nfRegPA), 2, 0)) + "</td>");
          Response.Write("    <td align='right' width='10%' class='cellBottomBorder'>" + cStr(FormatNumber(cStr(nfOTHP), 2, 0)) + "</td>");
          Response.Write("    <td align='right' width='12%' class='cellBottomBorder'>" + cStr(FormatNumber(cStr(nfOTPA), 2, 0)) + "</td>");
          Response.Write("    <td align='right' width='10%' class='cellBottomBorder'>" + cStr(FormatNumber(cStr(nfRegHP + nfOTHP), 2, 0)) + "</td>");
          Response.Write("    <td align='right' width='10%' class='cellBottomBorder'>" + cStr(FormatNumber(cStr(nfRegPA + nfOTPA), 2, 0)) + "</td>");
          Response.Write("    <td align='right' width='10%' class='reportEvenLine'>" + "&nbsp;" + "</td>");
          Response.Write("    <td align='right' width='10%' class='reportEvenLine'>" + "&nbsp;" + "</td>");
          Response.Write("</tr>");

          nfEmp   = 0;
          nfRegHP = 0;
          nfRegPA = 0;
          nfOTHP  = 0;
          nfOTPA  = 0;

          // Summary By Task ---;

          Response.Write("<tr><td colspan='10'>&nbsp;</td></tr>");
          Response.Write("<tr> ");
          Response.Write("    <td align='left'   class='cellTopBottomBorder'>Task</td>");
          Response.Write("    <td align='left'   class='cellTopBottomBorder'>GL Code</td>");
          Response.Write("    <td align='right'  colspan='2' width='15%' class='cellTopBottomBorder'>Regular Pay</td>");
          Response.Write("    <td align='right'  colspan='2' width='15%' class='cellTopBottomBorder'>Overtime Pay</td>");
          Response.Write("    <td align='right'  colspan='2' width='25%' class='cellTopBottomBorder'>Total Pay</td>");
          Response.Write("    <td align='right'  colspan='2'>&nbsp;</td>");
          Response.Write("</tr>");

          rsSum.Requery();
          double isRow = 0;
          double nRegHrs = 0;
          double nRegAmt = 0;
          double nOTHrs  = 0;
          double nOTAmt  = 0;
          double nTotalHrs  = 0;
          double nTotalAmt  = 0;


          while (! rsSum.EOF){

            rsSum.Read();

            if(cInt(OldFacID) == cInt(rsSum.Item("FacilityID"))){
               isRow=isRow+1;
               if(isRow % 2 == 0){
                  lc = "reportOddLine";
                }else{
                  lc = "reportEvenLine";
               }

                  double RegHrs = cDbl(rsSum.Item("RegHrs"));
                  double RegAmt = cDbl(rsSum.Item("RegAmount"));
                  double OTHrs = cDbl(rsSum.Item("OTHrs"));
                  double OTAmt = cDbl(rsSum.Item("OTAmount"));
                  double TotalHrs = cDbl(rsSum.Item("TotalHrs"));
                  double TotalAmt = cDbl(rsSum.Item("TotalAmount"));

                  nRegHrs = nRegHrs + RegHrs;
                  nRegAmt = nRegAmt + RegAmt;
                  nOTHrs  = nOTHrs  + OTHrs;
                  nOTAmt  = nOTAmt  + OTAmt;
                  nTotalHrs  = nTotalHrs  + TotalHrs;
                  nTotalAmt  = nTotalAmt  + TotalAmt;


              Response.Write("<tr class='" + lc + "'>");
              // <td align=//right//  colspan=//2// class=//reportEvenLine//>&nbsp;</td>";
              Response.Write("    <td align='left' width='08%'  >" + rsSum.Item("TaskCode") + "</td>");
              Response.Write("    <td align='left' width='08%'  >" + rsSum.Item("GLAcctNumber") + "</td>");
              Response.Write("    <td align='right' width='08%' >" + cStr(FormatNumber(cStr(RegHrs), 2)) + "</td>");
              Response.Write("    <td align='right'  width='08%'>" + cStr(FormatNumber(cStr(RegAmt), 2)) + "</td>");
              Response.Write("    <td align='right'  width='12%'>" + cStr(FormatNumber(cStr(OTHrs),  2)) + "</td>");
              Response.Write("    <td align='right'  width='13%'>" + cStr(FormatNumber(cStr(OTAmt),  2)) + "</td>");
              Response.Write("    <td align='right'  width='12%'>" + cStr(FormatNumber(cStr(TotalHrs),  2)) + "</td>");
              Response.Write("    <td align='right'  width='13%'>" + cStr(FormatNumber(cStr(TotalAmt),  2)) + "</td>");
              Response.Write("    <td align='right'  colspan='2' class='reportEvenLine'>&nbsp;</td>");
              Response.Write("</tr>");
                }else{
               }

          } //Loop

          Response.Write("<tr class='reportTotalLine'>");
          //<td align=//right//  colspan=//2// class=//reportEvenLine//>&nbsp;</td>");
          Response.Write("    <td align='right'  colspan='2' class='cellTopBottomBorder'>Totals:&nbsp;</td>");
          Response.Write("    <td align='right'  class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(nRegHrs), 2)) + "</td>");
          Response.Write("    <td align='right'  width='08%' class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(nRegAmt), 2)) + "</td>");
          Response.Write("    <td align='right'  width='12%' class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(nOTHrs),  2)) + "</td>");
          Response.Write("    <td align='right'  width='13%' class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(nOTAmt),  2)) + "</td>");
          Response.Write("    <td align='right'  width='12%' class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(nTotalHrs),  2)) + "</td>");
          Response.Write("    <td align='right'  width='13%' class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(nTotalAmt),  2)) + "</td>");
          Response.Write("    <td align='right'  colspan='2' class='reportEvenLine'>&nbsp;</td>");
          Response.Write("</tr>");

          Response.Write("</table>  </td></tr>");



        }

        public void ShowTempDetailReport(){

          string sFrom, sTo;
          int SumiRow = 0;
          int iRow = 0;

          string sPageBreak = "";
          string sPreviewLink = "javascript:document.form1.submit();";

          sTitle = "Temp Employee Payroll Report <br>" + sfromDateDetail + " - " + stoDateDetail;

          if(cStr(Request["PrintPreview"]).Length > 0){
            Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='480' align='center'>");
            Response.Write("  <tr><td colspan='10'>&nbsp;</td></tr>");
            if (RType != "All"){
               Response.Write("  <tr><td colspan='10' align='right'><a href='" + sPreviewLink + "'>Printer Friendly</a></td></tr>");
            }else{
               Response.Write("  <tr><td colspan='10' align='right'>&nbsp;</td></tr>");
           }
          }else{
            Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='600' align='center'>");
            Response.Write("  <tr><td colspan='10'>&nbsp;</td></tr>");
         }

          if(rsTemp.EOF){
            Response.Write("  <tr><td colspan='10'>&nbsp;</td></tr>");
            Response.Write("  <tr><td colspan='10'>No Temp Employee records found.</td></tr>");
          }else{

            string sFacility   = "";
            string sSource     = "";
            double sFacilityID = 0;
            double sSourceID   = 0;
            double sEmpNum = 0;

            while(!rsTemp.EOF){

                rsTemp.Read();

                string strSQL = "  SELECT EmployeeTaskWorkedId, PayMultiplier,               ";
                strSQL +=  "                PM=Case When PayMultiplier=1){1 }else{ 2 End,    ";
                strSQL +=  "                PR=IsNull(PayRate,0), HP=IsNull(HoursPaid,0),    ";
                strSQL +=  "                PA=IsNull(PayAmount,0)                           ";
                strSQL +=  "           FROM EmployeeTaskWorkedPay                            ";
                strSQL +=  "       Order By EmployeeTaskWorkedId, PayMultiplier              ";
                strSQL +=  "       WHERE EmployeeTaskWorkedID = " + rsTemp.Item("ID");

                DataReader rsTempPay = new DataReader(strSQL);
                rsTempPay.Open();

              if(sFacility != rsTemp.Item("Name") || sSource != rsTemp.Item("SourceName")){
                 if(sEmpNum != 0){

                          WriteTempEmpTotals();
                          SumiRow = 0;
                          iRow = 0;
                          WriteSourceTotals(sSource, sFacility, cStr(sFacilityID), rsTemp.Item("Name"));
                }

                sFacility = rsTemp.Item("Name");
                sFacilityID = cDbl(rsTemp.Item("FacilityID"));
                sSource   = rsTemp.Item("SourceName");
                sEmpNum   = 0;


               if (!isSummary){

                   Response.Write("  <tr><td colspan='9' align='center'><b>" + sPageBreak + sTitle + "</b></td><td>&nbsp;</td></tr>");
                   Response.Write("  <tr><td colspan='9' align='center'><b>" + sFacility + "</b></td><td>&nbsp;</td></tr>");
                   Response.Write("  <tr><td colspan='9' align='Left' class='lblColorBold'><b>Employment Source:&nbsp;" + sSource + "</b></td><td>&nbsp;</td></tr>");

               }else{
                    Response.Write("  <tr><td colspan='10'>&nbsp;</td></tr>");
                    Response.Write("  <tr><td colspan='9' align='center'><b>" +  sTitle + "</b></td><td>&nbsp;</td></tr>");
                    Response.Write("  <tr><td colspan='9' align='center'><b>" + sFacility + "</b></td><td>&nbsp;</td></tr>");
                    Response.Write("  <tr><td colspan='9' align='Left' class='lblColorBold'><b>Employment Source:&nbsp;" + sSource + "</b></td><td>&nbsp;</td></tr>");
                    Response.Write("<tr><td colspan='10'>&nbsp;</td></tr>");
                    Response.Write("    <td align='Left'   width='10%' class='cellTopBottomBorder'>&nbsp;</td>");
                    Response.Write("    <td align='Left'   width='10%' class='cellTopBottomBorder'>&nbsp;Name</td>");
                    Response.Write("    <td align='right'  width='10%' class='cellTopBottomBorder'>&nbsp;</td>");
                    Response.Write("    <td align='right'  width='10%' class='cellTopBottomBorder'>&nbsp;</td>");
                    Response.Write("    <td align='right'  colspan='2' class='cellTopBottomBorder'>Regular Pay</td>");
                    Response.Write("    <td align='right'  colspan='2' class='cellTopBottomBorder'>Overtime Pay</td>");
                    Response.Write("    <td align='right'  colspan='2'  class='cellTopBottomBorder'>&nbsp;Total</td>");
                    Response.Write("</tr>");

              }

              if(rsTempPay.EOF){

                  Response.Write("  <tr><td colspan='10'>&nbsp;</td></tr>");
                  Response.Write("  <tr><td colspan='10'>Payroll needs to be re-calculated for this period.</td></tr>");
              }
                sPageBreak = "<h3>&nbsp;</h3>";
             }

              if(! rsTempPay.EOF){
                rsTempPay.Read(); 
                if( sEmpNum != cDbl(rsTemp.Item("TempNumber"))){
                          if(sEmpNum != 0){
                                WriteTempEmpTotals();
                                iRow = 0;
                          } 


                  if (isSummary){

                    //Response.Write("<tr><td colspan=//10//>&nbsp;</td></tr>";
                    //Response.Write("<tr>";
                    //Response.Write("    <td align=//Left// ><b>" + rs("EmployeeNumber") + "</b></td>";
                    //Response.Write("    <td align=//Left// colspan=//9//><b>" + rs("LastName") + ", " + rs("FirstName") + "</b></td>";
                    //Response.Write("</tr>";

                }else{


                    Response.Write("<tr><td colspan='10'>&nbsp;</td></tr>");
                    Response.Write("<tr>");
                    Response.Write("    <td align='Left' ><b>" + rsTemp.Item("TempNumber") + "</b></td>");
                    Response.Write("    <td align='Left' colspan='9'><b>" + rsTemp.Item("LastName") + ", " + rsTemp.Item("FirstName") + "</b></td>");
                    Response.Write("</tr>");

                    Response.Write("<tr><td colspan='10'>&nbsp;</td></tr>");
                    Response.Write("    <td align='Left'   width='10%' class='cellTopBottomBorder'>&nbsp;</td>");
                    Response.Write("    <td align='Left'   width='10%' class='cellTopBottomBorder'>&nbsp;Task</td>");
                    Response.Write("    <td align='right'  width='10%' class='cellTopBottomBorder'>Rate</td>");
                    Response.Write("    <td align='right'  width='10%' class='cellTopBottomBorder'>UPM</td>");
                    Response.Write("    <td align='right'  colspan='2' class='cellTopBottomBorder'>Regular Pay</td>");
                    Response.Write("    <td align='right'  colspan='2' class='cellTopBottomBorder'>Overtime Pay</td>");
                    Response.Write("    <td align='right'  colspan='2'>&nbsp;</td>");
                    Response.Write("</tr>");

                }

                    SummaryEmpNumber = rsTemp.Item("TempNumber");
                    SummaryLastName = rsTemp.Item("LastName");
                    SummaryFirstName = rsTemp.Item("FirstName");

                  sEmpNum = cDbl(rsTemp.Item("TempNumber"));
               }

               string rowColor; 
               iRow=iRow+1;
               if(iRow % 2 == 0){
                  rowColor = "reportOddLine";
                }else{
                  rowColor = "reportEvenLine";
               }

                    string sHTML = "";
                    sHTML = sHTML + "<tr class='" + rowColor + "'>";
                    sHTML = sHTML +  "    <td align='Left'    width='10%'>" + rsTemp.Item("WorkDate")   + "</td>";
                    sHTML = sHTML +  "    <td align='Left'    width='10%'>&nbsp;" + rsTemp.Item("TaskCode");
                    if (cDbl(rsTemp.Item("ShiftID")) > 1){
                       sHTML = sHTML + " (" + cStr(rsTemp.Item("ShiftID")) + ")";
                   }
                    sHTML = sHTML +  "</td>";
                    sHTML = sHTML +  "    <td align='right'   width='10%'>" + cStr(FormatNumber(rsTempPay.Item("PR"), 2, 0)) + "</td>";
                    sHTML = sHTML +  "    <td align='right'   width='10%'>" + cStr(FormatNumber(rsTemp.Item("UPM"), 2, 0))   + "</td>";

                    TnHP = 0;
                    TnPA = 0;
                    double TiRec = 0;

                    while(!rsTempPay.EOF){
                          TiRec = TiRec + 1;
                          if(cDbl(rsTempPay.Item("PayMultiplier")) == 1){
                            sHTML = sHTML +  "    <td align='right' width='10%'>" + cStr(FormatNumber(rsTempPay.Item("HP"), 2, 0)) + "</td>";
                            sHTML = sHTML +  "    <td align='right' width='10%'>" + cStr(FormatNumber(rsTempPay.Item("PA"), 2, 0)) + "</td>";
                            if(cInt(rsTemp.Item("TaskType")) == 0){
                               TnRegHP = TnRegHP + cDbl(rsTempPay.Item("HP"));
                           }
                            TnRegPA = TnRegPA + cDbl(rsTempPay.Item("PA"));
                          }else{
                            if(TiRec == 1){
                               sHTML = sHTML +  "    <td align='right' width='10%'>.00</td>";
                               sHTML = sHTML +  "    <td align='right' width='10%'>.00</td>";
                           }
                            if(cDbl(rsTemp.Item("TaskType")) == 0){
                               TnHP = TnHP + cDbl(rsTempPay.Item("HP"));
                           }
                            TnPA = TnPA + cDbl(rsTempPay.Item("PA"));
                            if(cDbl(rsTemp.Item("TaskType")) == 0){
                               TnOTHP = TnOTHP + cDbl(rsTempPay.Item("HP"));
                           }
                            TnOTPA = TnOTPA + cDbl(rsTempPay.Item("PA"));
                         }

                    } //Loop

                sHTML = sHTML +  "    <td align='right' width='10%'>" + cStr(FormatNumber(cStr(TnHP), 2, 0)) + "</td>";
                sHTML = sHTML +  "    <td align='right' width='10%'>" + cStr(FormatNumber(cStr(TnPA), 2, 0)) + "</td>";
                sHTML = sHTML +  "</tr>";
                if (!isSummary){
                        Response.Write(sHTML);
               }


                if(sFacility != rsTemp.Item("Name")){
                  sFacility = Trim(rsTemp.Item("Name"));
                  sFacilityID = cDbl(rsTemp.Item("FacilityID"));
                  sEmpNum = cDbl(rsTemp.Item("TempNumber"));
               }

                 }


            }

            if(iRow > 0){
              WriteTempEmpTotals();
            }

              if(TnsEmp > 0) {
                  WriteSourceTotals(sSource, sFacility, cStr(sFacilityID), "");
               }

            Response.Write("<tr><td colspan='10'>&nbsp;</td></tr>");

         }

          if(cStr(Request["PrintPreview"]).Length > 0){
            Response.Write("  <tr><td colspan='10' align='right'><a href='" + sPreviewLink + "'>Printer Friendly</a></td></tr>");
          }else{
            Response.Write("  <tr><td colspan='10'>&nbsp;</td></tr>");
         }
          Response.Write("</table>");

        }

        public void  WriteTempEmpTotals(){

          string sTPay = cStr(FormatNumber(cStr(TnRegPA + TnOTPA), 2, 0));
          string sumrowColor;

          if (isSummary){

            sumiRow=sumiRow+1;
            if(sumiRow % 2 == 0){
               sumrowColor = "reportOddLine";
            }else{
               sumrowColor = "reportEvenLine";
           }


             Response.Write(" <tr class='" + sumrowColor + "'>");
             Response.Write("    <td align='Left' ><b>" + SummaryEmpNumber + "</b></td>");
             Response.Write("    <td align='Left' colspan='3'><b>" + SummaryLastName + ", " + SummaryFirstName + "</b></td>");
             Response.Write("    <td align='right' width='10%' >" + cStr(FormatNumber(cStr(TnRegHP), 2, 0)) + "</td>");
             Response.Write("    <td align='right' width='10%' >" + cStr(FormatNumber(cStr(TnRegPA), 2, 0)) + "</td>");
             Response.Write("    <td align='right' width='10%' >" + cStr(FormatNumber(cStr(TnOTHP), 2, 0)) + "</td>");
             Response.Write("    <td align='right' width='10%' >" + cStr(FormatNumber(cStr(TnOTPA), 2, 0)) + "</td>");
             Response.Write("    <td colspan='2'   width='20%' align='right'> ( " + sTPay +     " )&nbsp;</td>");
             Response.Write("</tr>");

          }else{


          Response.Write(" <tr class='reportTotalLine'>");
          Response.Write("    <td colspan='3' align='center' class='cellTopBottomBorder'> ( " + sTPay +     " )&nbsp;</td>");
          Response.Write("    <td align='right' width='10%' class='cellTopBottomBorder'>" + "&nbsp;"           + "</td>");
          Response.Write("    <td align='right' width='10%' class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(TnRegHP), 2, 0)) + "</td>");
          Response.Write("    <td align='right' width='10%' class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(TnRegPA), 2, 0)) + "</td>");
          Response.Write("    <td align='right' width='10%' class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(TnOTHP), 2, 0)) + "</td>");
          Response.Write("    <td align='right' width='10%' class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(TnOTPA), 2, 0)) + "</td>");
          Response.Write("    <td align='right' width='10%' class='reportEvenLine'>" + "&nbsp;" + "</td>");
          Response.Write("    <td align='right' width='10%' class='reportEvenLine'>" + "&nbsp;" + "</td>");
          Response.Write("</tr>");
          Response.Write("<tr><td colspan='9' align='Center'>&nbsp;</td></tr>");

         }


          TnsEmp   = TnsEmp + 1;
          TnsRegHP = TnsRegHP + TnRegHP;
          TnsRegPA = TnsRegPA + TnRegPA;
          TnsOTHP  = TnsOTHP  + TnOTHP;
          TnsOTPA  = TnsOTPA  + TnOTPA;

          TnRegHP = 0;
          TnRegPA = 0;
          TnHP    = 0;
          TnPA    = 0;
          TnOTHP  = 0;
          TnOTPA  = 0;

        }

        public void WriteSourceTotals(string sName,string  OldFac,string  OldFacID,string  NewFac){

          string sHTML;
          string sEmps = cStr(TnsEmp);
          sHTML = "";
          sHTML = sHTML + " <tr class='reportOddLine'>";
          sHTML = sHTML + "    <td colspan='4' align='right' class='lblColorBold'><b>"  + sName + "</b> :&nbsp;(" + sEmps +  ")&nbsp;</td>";
          sHTML = sHTML + "    <td align='right' width='10%'>" + cStr(FormatNumber(cStr(TnsRegHP), 2, 0)) + "</td>";
          sHTML = sHTML + "    <td align='right' width='10%'>" + cStr(FormatNumber(cStr(TnsRegPA), 2, 0)) + "</td>";
          sHTML = sHTML + "    <td align='right' width='10%'>" + cStr(FormatNumber(cStr(TnsOTHP), 2, 0)) + "</td>";
          sHTML = sHTML + "    <td align='right' width='10%'>" + cStr(FormatNumber(cStr(TnsOTPA), 2, 0)) + "</td>";
          sHTML = sHTML + "    <td align='right' width='10%' class='reportOddLine'>" + "&nbsp;" + "</td>";
          sHTML = sHTML + "    <td align='right' width='10%' class='reportOddLine'>" + "&nbsp;" + "</td>";
          sHTML = sHTML + "</tr>";

          Response.Write(sHTML);

          TnfEmp   = TnfEmp + TnsEmp;
          TnfRegHP = TnfRegHP + TnsRegHP;
          TnfRegPA = TnfRegPA + TnsRegPA;
          TnfOTHP  = TnfOTHP  + TnsOTHP;
          TnfOTPA  = TnfOTPA  + TnsOTPA;
          sfHTML  = sfHTML + sHTML;

          if(OldFac != NewFac){
            WriteTempFacilityTotals(OldFac, OldFacID);
         }

          TnsEmp   = 0;
          TnsRegHP = 0;
          TnsRegPA = 0;
          TnsOTHP  = 0;
          TnsOTPA  = 0;

        }

        public void WriteTempFacilityTotals(string OldFac,string OldFacID){

          string TfEmps = cStr(TnfEmp);
          string lc;

         if (isSummary){
            Response.Write("  <tr><td colspan='9' align='center'><b>"  + sTitle + "</b></td><td>&nbsp;</td></tr>");
         }else{
          Response.Write("  <tr><td colspan='9' align='center'><b>" + sPageBreak + sTitle + "</b></td><td>&nbsp;</td></tr>");
         }

          Response.Write("  <tr><td colspan='9' align='center'><b>Summary for:&nbsp;" + OldFac + "</b></td><td>&nbsp;</td></tr>");
          Response.Write("<tr><td colspan='10'>&nbsp;</td></tr>");
          Response.Write("    <td align='right'  colspan='2' class='cellTopBottomBorder'>Emp. Source</td>");
          Response.Write("    <td align='right'  colspan='2' class='cellTopBottomBorder'>Employees</td>");
          Response.Write("    <td align='right'  colspan='2' class='cellTopBottomBorder'>Regular Pay</td>");
          Response.Write("    <td align='right'  colspan='2' class='cellTopBottomBorder'>Overtime Pay</td>");
          Response.Write("    <td align='right'  colspan='2' class='cellTopBottomBorder'>Total Pay</td>");
          Response.Write("    <td align='right'  colspan='2'>&nbsp;</td>");
          Response.Write("</tr>");
          Response.Write(sfHTML);
          Response.Write(" <tr class='reportTotalLine'>");
          Response.Write("    <td align='right' colspan='2' class='cellTopBottomBorder'>&nbsp;</td>");
          Response.Write("    <td align='right' colspan='2' class='cellTopBottomBorder'>&nbsp;(" + TfEmps +  ")&nbsp;</td>");
          Response.Write("    <td align='right' width='10%' class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(TnfRegHP), 2, 0)) + "</td>");
          Response.Write("    <td align='right' width='10%' class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(TnfRegPA), 2, 0)) + "</td>");
          Response.Write("    <td align='right' width='10%' class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(TnfOTHP), 2, 0)) + "</td>");
          Response.Write("    <td align='right' width='10%' class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(TnfOTPA), 2, 0)) + "</td>");
          Response.Write("    <td align='right' width='10%' class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(TnfRegHP + TnfOTHP), 2, 0)) + "</td>");
          Response.Write("    <td align='right' width='10%' class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(TnfRegPA + TnfOTPA), 2, 0)) + "</td>");
          Response.Write("    <td align='right' width='10%' class='reportEvenLine'>" + "&nbsp;" + "</td>");
          Response.Write("    <td align='right' width='10%' class='reportEvenLine'>" + "&nbsp;" + "</td>");
          Response.Write("</tr>");

          TnfEmp   = 0;
          TnfRegHP = 0;
          TnfRegPA = 0;
          TnfOTHP  = 0;
          TnfOTPA  = 0;
          sfHTML = "";

          // Summary By Task ---;

          Response.Write("<tr><td colspan='10'>&nbsp;</td></tr>");
          Response.Write("<tr><td colspan='10'><table border='0' cellspacing='0' cellpadding='0' width='100%' align='center'>");

          Response.Write("<tr><td align='right'  colspan='2'>&nbsp;</td>");
          Response.Write("    <td align='left'   class='cellTopBottomBorder'>Task</td>");
          Response.Write("    <td align='left'   class='cellTopBottomBorder'>GL Code</td>");
          Response.Write("    <td align='right'  colspan='2' class='cellTopBottomBorder'>Regular Pay</td>");
          Response.Write("    <td align='right'  colspan='2' class='cellTopBottomBorder'>Overtime Pay</td>");
          Response.Write("    <td align='right'  colspan='2' class='cellTopBottomBorder'>Total Pay</td>");
          Response.Write("    <td align='right'  colspan='2'>&nbsp;</td>");
          Response.Write("</tr>");

          rsTempSum.Requery();
          double isRow = 0;
          double TnRegHrs = 0;
          double TnRegAmt = 0;
          double TnOTHrs  = 0;
          double TnOTAmt  = 0;

          while(!rsTempSum.EOF){
              rsTempSum.Read();
            if(cInt(OldFacID) == cInt(rsTempSum.Item("FacilityID"))){

                isRow=isRow+1;
                if(isRow % 2 == 0){
                  lc = "reportOddLine";
                }else{
                  lc = "reportEvenLine";
                }

                double TRegHrs = cDbl(rsTempSum.Item("RegHrs"));
                double TRegAmt = cDbl(rsTempSum.Item("RegAmount"));
                double TOTHrs = cDbl(rsTempSum.Item("OTHrs"));
                double TOTAmt = cDbl(rsTempSum.Item("OTAmount"));

                TnRegHrs = TnRegHrs + TRegHrs;
                TnRegAmt = TnRegAmt + TRegAmt;
                TnOTHrs  = TnOTHrs  + TOTHrs;
                TnOTAmt  = TnOTAmt  + TOTAmt;

              Response.Write("<tr class='" + lc + "'><td align='right'  colspan='2' class='reportEvenLine'>&nbsp;</td>");
              Response.Write("    <td align='left'   >" + rsTempSum.Item("TaskCode") + "</td>");
              Response.Write("    <td align='left'   >" + rsTempSum.Item("GLAcctNumber") + "</td>");
              Response.Write("    <td align='right'  >" + cStr(FormatNumber(cStr(TRegHrs), 2)) + "</td>");
              Response.Write("    <td align='right'  >" + cStr(FormatNumber(cStr(TRegAmt), 2)) + "</td>");
              Response.Write("    <td align='right'  >" + cStr(FormatNumber(cStr(TOTHrs),  2)) + "</td>");
              Response.Write("    <td align='right'  >" + cStr(FormatNumber(cStr(TOTAmt),  2)) + "</td>");
              Response.Write("    <td align='right'  >" + cStr(FormatNumber(cStr(TRegHrs + TOTHrs),  2)) + "</td>");
              Response.Write("    <td align='right'  >" + cStr(FormatNumber(cStr(TRegAmt + TOTAmt),  2)) + "</td>");



              Response.Write("    <td align='right'  colspan='2' class='reportEvenLine'>&nbsp;</td>");
              Response.Write("</tr>");
                }else{
               }

          } //Loop

          Response.Write("<tr class='reportTotalLine'><td align='right'  colspan='2' class='reportEvenLine'>&nbsp;</td>");
          Response.Write("    <td align='right'  colspan='2' class='cellTopBottomBorder'>Totals:&nbsp;</td>");
          Response.Write("    <td align='right'  class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(TnRegHrs), 2)) + "</td>");
          Response.Write("    <td align='right'  class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(TnRegAmt), 2)) + "</td>");
          Response.Write("    <td align='right'  class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(TnOTHrs),  2)) + "</td>");
          Response.Write("    <td align='right'  class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(TnOTAmt),  2)) + "</td>");
          Response.Write("    <td align='right'  class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(TnRegHrs + TnOTHrs),  2)) + "</td>");
          Response.Write("    <td align='right'  class='cellTopBottomBorder'>" + cStr(FormatNumber(cStr(TnRegAmt + TnOTAmt),  2)) + "</td>");
          Response.Write("    <td align='right'  colspan='2' class='reportEvenLine'>&nbsp;</td>");
          Response.Write("</tr>");
          Response.Write("</table>  </td></tr>");

        }




    }
}