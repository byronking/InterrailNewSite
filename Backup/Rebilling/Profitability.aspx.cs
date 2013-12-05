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
    public partial class Profitability : PageBase
    {

        public DataReader rs;
        public DataReader rsFac;
        public int rsFac_numRows;
        public DataReader rsRates;
        public DataReader rst;
        public DataReader rstPay;
         
        public string sFrom;
        public string sTo;
        public string sSubTasks;
        public string sWorkDates;
        public string sWhere;
        public string sRBSubTaskID;
        public string sReturnTo;
        public string selFacilities;
        public string sFacilities;
        public bool sViewReport;


        public string sTitle;
        public string sPageBreak;
        public string[] arFac;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

            GrantAccess("Super, Admin, User");


             sFrom         = Request["fromDate"];
             sTo           = Request["toDate"];
             selFacilities = Request["selFacilities"];

            sReturnTo = "&ReturnTo=%2FRebilling%2FProfitability%2Easp%3FfromDate%3D" + sFrom + "%26toDate%3D" + sTo + "%26selSubTasks%3D" + sSubTasks;

            if(sFrom != ""){
              sViewReport = true;
              sWorkDates = " '" + cStr(sFrom) + "' AND '" + cStr(sTo) + "' ";

              sFacilities = "(" + selFacilities +  ")";

              //This query does !have a server behavior ---;
              string strSQL = "";
              strSQL +=  " SELECT rbd.WorkDate, rbd.FacilityID, ";
              strSQL +=  "        SUM(rbd.TotalHours) AS TH, SUM(rbd.TotalUnits) AS TU, HoursOrUnits,  ";
              strSQL +=  "        rbs.Id AS RBSubTaskID, rbs.Description, c.CustomerName, ";
              strSQL +=  "        c.ID As CustID, c.CustomerCode, Facility.Name, AlphaCode";
              strSQL +=  "  FROM  RebillDetail rbd INNER JOIN RebillSubTasks rbs          ";
              strSQL +=  "        ON rbd.RebillSubTasksId = rbs.Id INNER JOIN Tasks t     ";
              strSQL +=  "        ON rbs.TaskID = t.Id INNER JOIN FacilityCustomer c      ";
              strSQL +=  "        ON rbs.FacilityCustomerId = c.Id INNER JOIN             ";
              strSQL +=  "        Facility ON rbd.FacilityID = Facility.Id                ";
              strSQL +=  "  WHERE rbd.FacilityID IN " + sFacilities;
              strSQL +=  "   AND  (WorkDate BETWEEN " + sWorkDates + ")                   ";
              strSQL += " GROUP BY rbd.FacilityID, c.ID, c.CustomerName, c.CustomerCode, rbs.Id, ";
              strSQL +=  "        rbs.Description, WorkDate, Facility.Name, AlphaCode, HoursOrUnits ";
              strSQL += " ORDER BY rbd.FacilityID, rbd.WorkDate             ";

              rs = new DataReader(strSQL);
              rs.Open();

              strSQL = "";
              strSQL +=  " SELECT RebillSubTasksID, EffectiveDate, RebillRate, ";
              strSQL +=  "        ExpDate=IsNull(ExpirationDate, '12/31/2077') ";
              strSQL +=  "   FROM RebillSubTaskRates               ";
              strSQL += " ORDER BY EffectiveDate Desc               ";

              rsRates = new DataReader(strSQL);
              rsRates.Open();

              strSQL = " ";
              strSQL +=  "  SELECT etw.ID, etw.FacilityID, etw.WorkDate,      ";
              strSQL +=  "         t.TaskCode, t.TaskDescription              ";
              strSQL +=  "    FROM EmployeeTaskWorked etw INNER JOIN Tasks t  ";
              strSQL +=  "         ON etw.TaskID = t.Id                       ";
              strSQL +=  "   WHERE etw.FacilityID IN " + sFacilities;
              strSQL +=  "     AND  (t.Rebillable = 1) AND (WorkDate BETWEEN " + sWorkDates + ") ";
              strSQL +=  "ORDER BY etw.FacilityID                             ";

              rst = new DataReader(strSQL);
              rst.Open();

              strSQL = " ";
              strSQL +=  " SELECT SUM(PayAmount) AS Pay, SUM(HoursPaid) As HP,";
              strSQL +=  "        AVG(PayRate) As Rate, EmployeeTaskWorkedId  ";
              strSQL +=  "   FROM EmployeeTaskWorkedPay  INNER JOIN           ";
              strSQL +=  "        EmployeeTaskWorked ON ";
              strSQL +=  "        EmployeeTaskWorkedPay.EmployeeTaskWorkedId = EmployeeTaskWorked.Id ";
              strSQL +=  "        INNER JOIN  ";
              strSQL +=  "        Tasks ON EmployeeTaskWorked.TaskID = Tasks.Id  ";
              strSQL +=  "   WHERE EmployeeTaskWorked.FacilityID IN " + sFacilities;
              strSQL +=  "     AND (Tasks.Rebillable = 1) AND (WorkDate BETWEEN " + sWorkDates + ") ";
              strSQL += " GROUP BY EmployeeTaskWorkedId                        ";
              rstPay = new DataReader(strSQL);
              rstPay.Open();


            }else{
              sViewReport = false;
              sFrom = System.DateTime.Now.ToString();
              sTo = System.DateTime.Now.ToString();
            }




            arFac = Split(selFacilities, ",");
            sTitle = "Re-Billing Profitability Analysis Report<br>" + sFrom + " - " + sTo;
            sPageBreak = "";


            string rsFac__PUserID;
            rsFac__PUserID = "17";
            if( Session["UserID"] != null && cStr(Session["UserID"]) != ""){rsFac__PUserID = cStr(Session["UserID"]);}


            string rsFac__PUserType;
            rsFac__PUserType = "super";
            if(Session["UserType"] != null && cStr(Session["UserType"]) != ""){rsFac__PUserType = cStr(Session["UserType"]);}



            rsFac = new DataReader( "SELECT Distinct f.Id, f.Name, f.AlphaCode  FROM dbo.UserRights r RIGHT OUTER JOIN  dbo.Facility f ON r.FacilityId = f.Id  WHERE f.Active=1 AND (r.UserProfileId = " + Replace(rsFac__PUserID, "'", "''") + "  OR '" + Replace(rsFac__PUserType, "'", "''") + "' = 'Admin' OR '" + Replace(rsFac__PUserType, "'", "''") + "'='Super')  Order By Name");
            rsFac.Open();
            rsFac_numRows = rsFac.RecordCount;

        }

        public void ShowProfitabilityReport()
        {

            string sPreviewLink = "javascript:document.form1.submit();";
            double RebillAmount = 0.00;

            Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='500' align='center'>");
            if (cStr(Request["PrintPreview"]).Length > 0)
            {
                Response.Write("  <tr><td colspan='7' align='right'><a href='" + sPreviewLink + "'>Printer Friendly</a></td></tr>");
            }
            if ((rs.EOF) && (rst.EOF))
            {
                Response.Write("   <tr>");
                Response.Write("       <td colspan='7' align='center'><b>No records found.</b></td>");
                Response.Write("   </tr>");
            }
            else
            {
                string sFacilityID;
                for (int I = LBound(arFac); I < UBound(arFac); I++)
                {

                    sFacilityID = Trim(cStr(arFac[I]));
                    double RebillTotal = 0.0;
                    double FacilityPay = 0.0;
                    bool WriteTotals = false;
                    double CustTotal = 0.00; 
                    string sRow = "<tr>";
                    string lc = "";
                    int iRow = 0;

                    // Rebilling;
                    if (!rs.EOF)
                    {
                        sRow = sRow + "<td colspan='3' valign='top'>";
                        sRow = sRow + "  <table width=100% border ='0'>";
                        bool WroteHeader = false;
                        int sCustomerID = 0;
                        while (!rs.EOF)
                        {
                            rs.Read();

                            if (Trim(cStr(rs.Item("FacilityID"))) == sFacilityID)
                            {
                                WriteTotals = true;
                                double RRate = 0.0;

                                while (!rsRates.EOF)
                                {
                                    rsRates.Read();

                                    if (cInt(rs.Item("RBSubTaskID")) == cInt(rsRates.Item("RebillSubTasksID")) && (cDate(rs.Item("WorkDate")) >= cDate(rsRates.Item("EffectiveDate"))) && (cDate(rs.Item("WorkDate")) <= cDate(rsRates.Item("ExpDate"))))
                                    {
                                        RRate = cDbl(rsRates.Item("RebillRate"));
                                        break;
                                    }

                                } //Loop

                                if (WroteHeader == false)
                                {
                                    string sName = rs.Item("Name") + " (" + rs.Item("AlphaCode") + ")";
                                    WriteHeader(sName);
                                    WroteHeader = true;
                                }
                                if (cInt(rs.Item("CustID")) == sCustomerID)
                                {
                                    if (Trim(rs.Item("HoursOrUnits")) == "H")
                                    {
                                        sRow = sRow + "    <tr class='" + lc + "'><td width='30%' align='right'>&nbsp;</td> <td width='70%' align='left'>" + cStr(FormatNumber(rs.Item("TH"), 2)) + " hrs. @ $" + cStr(FormatNumber(cStr(RRate), 2)) + " </td></tr>";
                                        RebillAmount = cDbl(FormatNumber(cStr(cDbl(rs.Item("TH")) * RRate), 2));
                                    }
                                    else
                                    {
                                        sRow = sRow + "    <tr class='" + lc + "'><td width='30%' align='right'>&nbsp;</td> <td width='70%' align='left'>" + cStr(FormatNumber(rs.Item("TU"), 0)) + " units @ $" + cStr(FormatNumber(cStr(RRate), 2)) + " </td></tr>";
                                        RebillAmount = cDbl(FormatNumber(cStr(cDbl(rs.Item("TU")) * RRate), 2));
                                    }
                                    CustTotal = CustTotal + RebillAmount;
                                    RebillTotal = cDbl(RebillTotal) + cDbl(RebillAmount);
                                }
                                else
                                {
                                    if (sCustomerID != 0)
                                    {
                                        sRow = sRow + "    <tr class='" + lc + "'><td width='30%' align='right' class='lblColor'>Rebill:&nbsp;</td><td width='70%' align='left'>$" + cStr(CustTotal) + "</td></tr>";
                                        sRow = sRow + "    <tr><td>&nbsp;</td></tr>";
                                    }
                                    else
                                    {
                                        iRow = 0;
                                    }

                                    iRow = iRow + 1;
                                    if (iRow % 2 == 1)
                                    {
                                        lc = "reportEvenLine";
                                    }
                                    else
                                    {
                                        lc = "reportOddLine";
                                    }

                                    sRow = sRow + "    <tr class='" + lc + "'><td width='30%' align='right' class='lblColor'>Customer:&nbsp;</td><td width='70%' align='left'>" + rs.Item("CustomerName") + "</td></tr>";
                                    sRow = sRow + "    <tr class='" + lc + "'><td width='30%' align='right' class='lblColor'>SubTask:&nbsp;</td> <td width='70%' align='left'>" + rs.Item("Description") + "</td></tr>";

                                    if (Trim(rs.Item("HoursOrUnits")) == "H")
                                    {
                                        sRow = sRow + "    <tr class='" + lc + "'><td width='30%' align='right'>&nbsp;</td> <td width='70%' align='left'>" + cStr(FormatNumber(rs.Item("TH"), 2)) + " hrs. @ $" + cStr(FormatNumber(cStr(RRate), 2)) + " </td></tr>";
                                        RebillAmount = cDbl(FormatNumber(cStr(cDbl(rs.Item("TH")) * RRate), 2));
                                    }
                                    else
                                    {
                                        sRow = sRow + "    <tr class='" + lc + "'><td width='30%' align='right'>&nbsp;</td> <td width='70%' align='left'>" + cStr(FormatNumber(rs.Item("TU"), 0)) + " units @ $" + cStr(FormatNumber(cStr(RRate), 2)) + " </td></tr>";
                                        RebillAmount = cDbl(FormatNumber(cStr(cDbl(rs.Item("TU")) * RRate), 2));
                                    }
                                    CustTotal = CustTotal + RebillAmount;
                                    RebillTotal = cDbl(RebillTotal) + cDbl(RebillAmount);


                                    sCustomerID = cInt(rs.Item("CustID"));
                                    string sOldCustName = rs.Item("CustomerName");
                                    string sOldTask = rs.Item("Description");
                                    double CustToTal = cDbl(RebillAmount);
                                    RebillTotal = cDbl(RebillTotal) + cDbl(RebillAmount);
                                }
                            }

                        } //Loop

                        if (WriteTotals == true)
                        {
                            sRow = sRow + "    <tr class='" + lc + "'><td width='30%' align='right' class='lblColor'>Rebill:&nbsp;</td><td width='70%' align='left'>$" + cStr(FormatNumber(cStr(CustTotal), 2)) + "</td></tr>";
                        }
                        sRow = sRow + "  </table>";
                        sRow = sRow + "</td>";
                    }
                    else
                    {
                        sRow = sRow + "<td colspan='3'>No rebill data.</td>";
                    }

                    if (!rst.EOF)
                    {
                        sRow = sRow + "<td colspan='4' valign='top'>";
                        //sRow = sRow + "  <table width=100% border =//0//>";
                        while (!rst.EOF)
                        {
                            rst.Read();

                            if (cStr(rst.Item("FacilityID")) == sFacilityID)
                            {
                                double TotalPay = 0.0;
                                WriteTotals = true;
                                while (!rstPay.EOF)
                                {
                                    rstPay.Read();

                                    if (rst.Item("ID") == rstPay.Item("EmployeeTaskWorkedID"))
                                    {
                                        TotalPay = TotalPay + cDbl(rstPay.Item("Pay"));
                                    }

                                } //loop
                                FacilityPay = FacilityPay + TotalPay;
                            }
                        } //loop

                        sRow = sRow + "</td>";
                    }
                    else
                    {
                        sRow = sRow + "<td colspan='3'>No payroll data.</td>";
                    }

                    sRow = sRow + "</tr>";

                    if (WriteTotals == true)
                    {
                        sRow = sRow + "<tr class='reportTotalLine'>";
                        sRow = sRow + "  <td colspan='3' class='cellTopBottomBorder'>Total Rebill:&nbsp;$" + cStr(FormatNumber(cStr(RebillTotal), 2)) + "</td>";
                        sRow = sRow + "  <td class='cellTopBottomBorder'>&nbsp;</td>";
                        sRow = sRow + "  <td colspan='3'class='cellTopBottomBorder'>Total Labor:&nbsp;$" + cStr(FormatNumber(cStr(FacilityPay), 2)) + "</td>";
                        sRow = sRow + "</tr>";
                    }
                    else
                    {
                        rsFac.Requery();
                        while (!rsFac.EOF)
                        {
                            rsFac.Read();

                            if (cInt(rsFac.Item("ID")) == cInt(sFacilityID))
                            {
                                break;
                            }

                        } //loop
                        string sFacName = cStr(rsFac.Item("Name")) + " (" + cStr(rsFac.Item("AlphaCode")) + ")";
                        WriteHeader(sFacName);
                        sRow = sRow + "<tr><td colspan='4'>No data for: " + sFacName + "</td></tr>";
                    }

                    Response.Write(sRow);

                    rs.Requery();
                    rst.Requery();

                }
            }

            if (cStr(Request["PrintPreview"]).Length > 0)
            {
                Response.Write("  <tr><td colspan='7' align='right'><a href='" + sPreviewLink + "'>Printer Friendly</a></td></tr>");
            }
            Response.Write("</table>");

        }
        public void  WriteHeader(string Facility){
          Response.Write("   <tr><td colspan='7' align='center'><b>" + sPageBreak + sTitle + "</b></td></tr>");
          Response.Write("   <tr><td align='left' colspan='7' class='lblColorBold'>Facility: " + cStr(Facility) + "</td></tr>");
          Response.Write("   <tr>");
          Response.Write("       <td colspan='3' align='center' Width='60%' class='cellTopBottomBorder'><b>Rebilling</b></td>");
          //Response.Write("       <td colspan=//1// align=//center// Width=//10%// class=//cellTopBottomBorder//>&nbsp;</td>");
          Response.Write("       <td colspan='4' align='center' Width='30%' class='cellTopBottomBorder'><b>Payroll</b></td>");
          Response.Write("   </tr>");
          Response.Write("   <tr><td  colspan=7>&nbsp;</td></tr>");
          sPageBreak = "<h3>&nbsp;</h3>";
        }


    }
}