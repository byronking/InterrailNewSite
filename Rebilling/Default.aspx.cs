using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InterrailPPRS.Rebilling
{
    public partial class Default : PageBase
    {
        public string ApStartDate;
        public string ApEndDate;
        public DataReader localRS;
        public DataReader rsStatus;
        public string sPayRange = "";
        public string[] sRange = new string[2];
        public string strSQL = "";
        public int rsStatus_numRows = 0;

        public string[] sDays = new string[7];
        public string sNoData;
        public int nRows;

        public string startDate = "";
        public string sWhere = "";
        public string sFacName = "";

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            GrantAccess("Super, Admin, User");            

            startDate = System.Convert.ToString(Request["StartDate"]);

            if (Request["Type"] == "Approve")
            {
                ApStartDate = Request["StartDate"];
                ApEndDate = Request["EndDate"];

                if (isDate(ApStartDate) && isDate(ApEndDate))
                {
                    string apStatus = "CORPORATE";
                    strSQL = "";
                    strSQL += " Update RebillDetail Set ";
                    strSQL += "  RebillStatus = '" + apStatus + "' ";
                    strSQL += " where (WorkDate Between '" + ApStartDate + "'  AND '" + ApEndDate + "') ";
                    strSQL += " And FacilityID = " + Request["FID"];
                    strSQL += " And RebillStatus = 'FACILITY' ";

                    // Response.Write(strSQL);

                    localRS = new DataReader(strSQL);
                    localRS.Open();
                }
            }

            if (startDate == null || startDate == "")
            {
                startDate = "6/1/2014"; //System.DateTime.Now.ToShortDateString();
            }

            sRange[0] = getStartPay(startDate);
            sRange[1] = cStr(cDate(sRange[0]).AddDays(6));
            sPayRange = sRange[0] + " - " + sRange[1];

            if (UCase(System.Convert.ToString(Session["UserType"])) == "USER")
            {
                sWhere = "    WHERE (FacilityID = " + cStr(Session["FacilityID"]) + ")";
            }
            else
            {
                sWhere = "    WHERE (1=1)";
            }

            strSQL = "";
            if (UCase(System.Convert.ToString(Session["UserType"])) != "USER" && Request["rdoFac"] == "Show")
            {
                strSQL += "  SELECT WorkDate='1/1/1900', AlphaCode, FacilityNumber, ID AS FacilityID, Name, ApprovalStatus= '?'";
                strSQL += "  FROM Facility ";
                strSQL += "UNION ";
            }

            strSQL += "  SELECT WorkDate, AlphaCode, FacilityNumber, FacilityID, Name,          ";
            strSQL += "         ApprovalStatus=Case RebillStatus When 'CORPORATE' Then 'CORP'   ";
            strSQL += "                            When 'FACILITY' Then 'FAC' Else 'OPEN' END  ";
            strSQL += "    FROM Facility INNER JOIN                                             ";
            strSQL += "         RebillDetail ON Facility.Id = FacilityID            ";
            strSQL = strSQL + sWhere;
            strSQL += "     AND (WorkDate Between '" + cStr(sRange[0]) + "'  AND '" + cStr(sRange[1]) + "')";
            strSQL += " GROUP BY WorkDate, AlphaCode, FacilityNumber, RebillStatus, FacilityID, Name ";
            strSQL += " ORDER BY Name, AlphaCode, WorkDate, ApprovalStatus ";
            //Response.Write(strSQL;
            //Response.End;

            rsStatus = new DataReader(strSQL);
            rsStatus.Open();
            rsStatus.Read();
            rsStatus_numRows = rsStatus.RecordCount;

            sNoData = "<font color='red'><b>-</b></font>";
            nRows = 0;

            for (int i = 0; i < 6; i++)
            {
                sDays[i] = sNoData;
            }

            PopulateDateRangeDropdown();
        }

        private void PopulateDateRangeDropdown()
        {
            var todayName = DateTime.Now.DayOfWeek;
            var lastNearestFridayDate = GetLastNearestFriday(todayName);
            var nextNearestThursday = lastNearestFridayDate.AddDays(6);

            var payPeriodStart = lastNearestFridayDate;
            var payPeriodEnd = nextNearestThursday;

            GetPayPeriods3(0, 12, payPeriodStart, payPeriodEnd);
        }

        public void ShowApprovalStatus()
        {
            var todayName = DateTime.Now.DayOfWeek;
            var lastNearestFridayDate = GetLastNearestFriday(todayName);
            var nextNearestThursday = lastNearestFridayDate.AddDays(6);

            var payPeriodStart = lastNearestFridayDate;
            var payPeriodEnd = nextNearestThursday;

            Response.Write("<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
            Response.Write("  <tr> ");
            Response.Write("    <td width='10%'>&nbsp;</td><td width='10%'>&nbsp;</td>");
            Response.Write("    <td width='10%'>&nbsp;</td><td width='10%'>&nbsp;</td>");
            Response.Write("    <td width='10%'>&nbsp;</td><td width='10%'>&nbsp;</td>");
            Response.Write("    <td width='10%'>&nbsp;</td><td width='10%'>&nbsp;</td>");
            Response.Write("    <td width='10%'>&nbsp;</td><td width='10%'>&nbsp;</td>");
            Response.Write(" </tr>");
            //Response.Write(" <tr class='pageTitle'>");
            //Response.Write("   <td colspan='10' align='center'  valign='top'>Rebilling Approval Status</td>");
            //Response.Write(" </tr>");
            //Response.Write(" <tr class='pageTitle'>");
            //Response.Write("   <td colspan='10' align='center'  valign='top'>for Period</td>");
            //Response.Write(" </tr>");
            Response.Write(" <form name='form1' action=''>");
            Response.Write(" <tr class='pageTitle'>");
            Response.Write("   <td colspan='10' align='center'  valign='top'>");
            //Response.Write("     <select name='selDateRange' onChange='goNewDate();'>" + getPayPeriods(0, 12, startDate) + "</select>");
            //Response.Write("     <select name='selDateRange' id='selDateRange' onChange='goNewDate();'>" + GetPayPeriods2(0, 12, payPeriodStart, payPeriodEnd) + "</select>");
            Response.Write("     <input type='hidden' name='startDate'>");
            Response.Write("   </td>");
            Response.Write(" </tr>");

            //if (UCase(System.Convert.ToString(Session["UserType"])) != "USER")
            //{
            //    Response.Write(" <tr>");
            //    Response.Write("   <td colspan='10' align='center'  valign='top'>");
            //    if (System.Convert.ToString(Request["rdoFac"]) == "Show")
            //    {
            //        Response.Write("     <input id='rdoShow' name='rdoFac' type='radio' value='Show' checked onClick='goNewDate();'><label for='rdoShow'>Show All Facilities.</label>");
            //        Response.Write("     <input id='rdoHide' name='rdoFac' type='radio' value='Hide' onClick='goNewDate();'><label for='rdoHide'>Hide</label>");
            //    }
            //    else
            //    {
            //        Response.Write("     <input id='rdoShow' name='rdoFac' type='radio' value='Show' onClick='goNewDate();'><label for='rdoShow'>Show</label>");
            //        Response.Write("     <input id='rdoHide' name='rdoFac' type='radio' value='Hide' checked onClick='goNewDate();'><label for='rdoHide'>Hide, when no data.</label>");
            //    }
            //    Response.Write("   </td>");
            //    Response.Write(" </tr>");
            //}

            if (rsStatus.EOF)
            {
                Response.Write("  <tr><td colspan='10'>&nbsp;</td>");
                Response.Write("<tr>");
                Response.Write("   <td colspan='10' align='left'  valign='top' class='required'>No rebilling data found this period.</td>");
                Response.Write("</tr>");
            }
            else
            {

                //WriteHeadings();
                WriteHeadings(payPeriodStart);

                string sFacID = "0";

                while (!rsStatus.EOF)
                {
                    rsStatus.Read();
                    if (sFacID != rsStatus.Item("FacilityID"))
                    {
                        if (sFacID != "0")
                        {
                            WriteRow(sFacName, sFacID);
                        }
                        for (int i = 0; i < 7; i++)
                        {
                            sDays[i] = sNoData;
                        }
                        sFacName = rsStatus.Item("Name");
                        sFacID = rsStatus.Item("FacilityID");
                    }

                    if (cDate(rsStatus.Item("WorkDate")) == cDate("1/1/1900"))
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            sDays[i] = sNoData;
                        }
                    }
                    else
                    {

                        for (int i = 0; i < 7; i++)
                        {
                            //if (cDate(sRange[0]).AddDays(i) == cDate(rsStatus.Item("WorkDate")))
                            if (cDate(ApStartDate).AddDays(i) == cDate(rsStatus.Item("WorkDate")))
                            {
                                string sStatus = rsStatus.Item("ApprovalStatus");
                                if (sStatus == "CORP")
                                {
                                    sStatus = "<font color='green'>" + sStatus + "</font>";
                                }
                                else
                                {
                                    if (sStatus == "FAC")
                                    {
                                        sStatus = "<font color='blue'>" + sStatus + "</font>";
                                    }
                                    else
                                    {
                                        sStatus = "<font color='red'>" + sStatus + "</font>";
                                    }
                                }

                                if (sDays[i] == sNoData)
                                {
                                    sDays[i] = sStatus;
                                }
                                else
                                {
                                    sDays[i] = sDays[i] + "<br>" + sStatus;
                                }

                            }
                        } //End For 
                    }

                } //End Loop

                // Write row for last facility;
                WriteRow(sFacName, sFacID);

            }
            Response.Write("</table>");
        }

        protected void WriteHeadings()
        {
            Response.Write("  <tr><td colspan='10'>&nbsp;</td>");
            Response.Write("  <tr> ");
            Response.Write("  <td class='cellTopBottomBorder'>&nbsp;</td> <td colspan='3' class='cellTopBottomBorder' align='center'>FACILITY</td>");
            for (int i = 0; i < 7; i++)
            {
                Response.Write("    <td width='10%' class='cellTopBottomBorder' align='center'>" + cStr(Day(cDate(sRange[0]).AddDays(i))) + "</td>");
            }
            Response.Write(" </tr>");
        }

        protected void WriteHeadings(DateTime startDate)
        {
            Response.Write("  <tr><td colspan='10'>&nbsp;</td>");
            Response.Write("  <tr> ");
            Response.Write("  <td class='cellTopBottomBorder'>&nbsp;</td> <td colspan='3' class='cellTopBottomBorder' align='center'>FACILITY</td>");
            for (int i = 0; i < 7; i++)
            {
                //Response.Write(" <td width='10%' class='cellTopBottomBorder' align='center'>" + cStr(Day(cDate(sRange[0]).AddDays(I))) + "</td>");
                Response.Write(" <td width='10%' class='cellTopBottomBorder' align='center'>" + Convert.ToDateTime(ApStartDate).AddDays(i).Day + "</td>");
            }
            Response.Write(" </tr>");
        }

        protected void WriteRow(string FacilityName, string FacilityID)
        {
            nRows = nRows + 1;
            string lc;
            if (nRows % 2 == 0)
            {
                lc = "reportEvenLine";
            }
            else
            {
                lc = "reportOddLine";
            }

            Response.Write("<tr class='" + lc + "'>");
            Response.Write(" <td>");
            if (UCase(System.Convert.ToString(Session["UserType"])) != "USER")
            {
                var startDate = ApStartDate;
                var endDate = ApEndDate;
                Response.Write("<a href='default.aspx?Type=Approve&FID=" + FacilityID + "&StartDate=" + ApStartDate + "&EndDate=" + ApEndDate + "'>Approve</a> ");
                //Response.Write("<a href='default.aspx?Type=Approve&FID=" + FacilityID + "&StartDate=" + sRange[0] + "&EndDate=" + sRange[1] + "'>Approve</a> ");
            }
            else
            {
                Response.Write("&nbsp;");
            }
            Response.Write(" </td> ");
            Response.Write("   <td colspan='3' align='right'  valign='top'><b>" + FacilityName + "&nbsp;:</b>&nbsp;</td>");
            for (int i = 0; i < 7; i++)
            {
                Response.Write("    <td width='10%' align='center'>" + sDays[i] + "</td>");
            }
            Response.Write("</tr>");
            Response.Write("<tr class='" + lc + "'><td colspan='11'>&nbsp;</td>");
        }

        private DateTime GetLastNearestFriday(DayOfWeek day)
        {
            // What the fridayDate is set to initially is irrelevant.
            DateTime fridayDate = DateTime.Now;

            switch (day)
            {
                case DayOfWeek.Sunday:
                    {
                        fridayDate = DateTime.Now.AddDays(-2);
                        break;
                    }
                case DayOfWeek.Monday:
                    {
                        fridayDate = DateTime.Now.AddDays(-3);
                        break;
                    }
                case DayOfWeek.Tuesday:
                    {
                        fridayDate = DateTime.Now.AddDays(-4);
                        break;
                    }
                case DayOfWeek.Wednesday:
                    {
                        fridayDate = DateTime.Now.AddDays(-5);
                        break;
                    }
                case DayOfWeek.Thursday:
                    {
                        fridayDate = DateTime.Now.AddDays(-6);
                        break;
                    }
                case DayOfWeek.Friday:
                    {
                        fridayDate = DateTime.Now.AddDays(-7);
                        break;
                    }
                case DayOfWeek.Saturday:
                    {
                        fridayDate = DateTime.Now.AddDays(-8);
                        break;
                    }
            }

            return fridayDate;
        }

        public string GetPayPeriods2(int nStart, int nPeriods, DateTime payPeriodStart, DateTime payPeriodEnd)
        {
            string strHTML = string.Empty;

            if (!Page.IsPostBack)
            {              
                for (int i = nStart; i < nPeriods; i++)
                {
                    var selectStartDate = payPeriodStart.AddDays(-7 * i);
                    var selectEndDate = payPeriodEnd.AddDays(-7 * i);

                    strHTML = strHTML + @"<option value=" + selectStartDate.ToShortDateString() + "," + selectEndDate.ToShortDateString() + ">" + selectStartDate.ToShortDateString() + " - " + selectEndDate.ToShortDateString() + "</option>";
                }
            }

            return strHTML;
        }

        public void GetPayPeriods3(int nStart, int nPeriods, DateTime payPeriodStart, DateTime payPeriodEnd)
        {
            if (!Page.IsPostBack)
            {
                for (int i = nStart; i < nPeriods; i++)
                {
                    var selectStartDate = payPeriodStart.AddDays(-7 * i);
                    var selectEndDate = payPeriodEnd.AddDays(-7 * i);

                    var listItem = new ListItem();
                    listItem.Text = selectStartDate.ToShortDateString() + " - " + selectEndDate.ToShortDateString();
                    listItem.Value = selectStartDate.ToShortDateString() + "," + selectEndDate.ToShortDateString();
                    ddlDateRange.Items.Add(listItem);
                }
            }
        }

        protected void ddlDateRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedRange = ddlDateRange.SelectedValue;
            ApStartDate = Split(selectedRange, ",")[0];
            ApEndDate = Split(selectedRange, ",")[1];

            //startDate = System.Convert.ToString(Request["StartDate"]);

            if (Request["Type"] == "Approve")
            {
                //ApStartDate = Request["StartDate"];
                //ApEndDate = Request["EndDate"];

                if (isDate(ApStartDate) && isDate(ApEndDate))
                {
                    string apStatus = "CORPORATE";
                    strSQL = "";
                    strSQL += " Update RebillDetail Set ";
                    strSQL += " RebillStatus = '" + apStatus + "' ";
                    strSQL += " where (WorkDate Between '" + ApStartDate + "'  AND '" + ApEndDate + "') ";
                    strSQL += " And FacilityID = " + Request["FID"];
                    strSQL += " And RebillStatus = 'FACILITY' ";

                    localRS = new DataReader(strSQL);
                    localRS.Open();
                }
            }

            //if (startDate == null || startDate == "")
            //{
            //    startDate = System.DateTime.Now.ToShortDateString();
            //}

            //sRange[0] = getStartPay(startDate);
            //sRange[1] = cStr(cDate(sRange[0]).AddDays(6));
            //sPayRange = sRange[0] + " - " + sRange[1];

            if (UCase(System.Convert.ToString(Session["UserType"])) == "USER")
            {
                sWhere = " WHERE (FacilityID = " + cStr(Session["FacilityID"]) + ")";
            }
            else
            {
                sWhere = " WHERE (1=1)";
            }

            strSQL = "";
            if (UCase(System.Convert.ToString(Session["UserType"])) != "USER" && Request["rdoFac"] == "Show")
            {
                strSQL += " SELECT WorkDate='1/1/1900', AlphaCode, FacilityNumber, ID AS FacilityID, Name, ApprovalStatus= '?'";
                strSQL += " FROM Facility ";
                strSQL += " UNION ";
            }
            strSQL += " SELECT WorkDate, AlphaCode, FacilityNumber, FacilityID, Name, ";
            strSQL += " ApprovalStatus=Case RebillStatus When 'CORPORATE' Then 'CORP' ";
            strSQL += " When 'FACILITY' Then 'FAC' Else 'OPEN' END ";
            strSQL += " FROM Facility INNER JOIN ";
            strSQL += " RebillDetail ON Facility.Id = FacilityID ";
            strSQL = strSQL + sWhere;
            //strSQL += " AND (WorkDate Between '" + cStr(sRange[0]) + "'  AND '" + cStr(sRange[1]) + "')";
            strSQL += " AND (WorkDate Between '" + ApStartDate + "'  AND '" + ApEndDate + "')";
            strSQL += " GROUP BY WorkDate, AlphaCode, FacilityNumber, RebillStatus, FacilityID, Name ";
            strSQL += " ORDER BY Name, AlphaCode, WorkDate, ApprovalStatus ";

            rsStatus = new DataReader(strSQL);
            rsStatus.Open();
            rsStatus.Read();
            rsStatus_numRows = rsStatus.RecordCount;

            sNoData = "<font color='red'><b>-</b></font>";
            nRows = 0;

            for (int i = 0; i < 6; i++)
            {
                sDays[i] = sNoData;
            }
        }
    }
}