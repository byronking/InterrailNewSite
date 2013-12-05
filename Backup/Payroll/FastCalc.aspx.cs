using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Mail;
using System.Collections.Specialized;
using System.Configuration;

namespace InterrailPPRS.Payroll
{
    public partial class FastCalc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here

            // Load NameValueCollection object.
            NameValueCollection coll = Request.QueryString;

            string startdate = coll["StartDate"];
            string enddate = coll["EndDate"];
            string connectstring = coll["ConnectString"];
            string facilityID = coll["FacilityID"];
            string user = coll["User"];


            TravlersCalc(facilityID, startdate, enddate, connectstring, user);

            CalcClass c = new CalcClass(System.Convert.ToInt32(facilityID), startdate, enddate, connectstring, user);

            Label2.Text = "Done";

            //Response.Redirect("Calc.aspx", true);
            Literal1.Text = "<script>location = \"calc.aspx\"</script>";

        }

        private void TravlersCalc(string facilityID, string startdate, string enddate, string connectstring, string user)
        {

            string sql = "";
            sql = sql + " SELECT DISTINCT Employee.FacilityID, Facility.Name, Facility_1.Name AS ForeignFacilityName ";
            sql = sql + " FROM         Facility INNER JOIN ";
            sql = sql + "  EmployeeTaskWorked ON Facility.Id = EmployeeTaskWorked.FacilityID INNER JOIN ";
            sql = sql + "  Employee ON EmployeeTaskWorked.EmployeeId = Employee.Id INNER JOIN ";
            sql = sql + "  Facility Facility_1 ON Employee.FacilityID = Facility_1.Id ";
            sql = sql + " WHERE     EmployeeTaskWorked.Facilityid = " + facilityID + " AND (Employee.FacilityID <> " + facilityID + ") AND (EmployeeTaskWorked.WorkDate BETWEEN '" + startdate + "' AND '" + enddate + "') ";
            sql = sql + " order by Employee.Facilityid ";

            DataReader reader = new DataReader(sql);
            reader.Open();

            string faclist = "";

            while (reader.Read())
            {
                string ForeignFacilityID = reader.Item("FacilityID");
                string ForeignFacilityName = reader.Item("ForeignFacilityName");
                // write out what we are doing

                // append to an email
                string st = getPayRollStatus(startdate, enddate, ForeignFacilityID, connectstring);
                if ((st == "APPROVED") || (st == "LOCKED"))
                {
                    if (faclist.Length > 0)
                        faclist = faclist + "<br /> ";
                    faclist = faclist + ForeignFacilityName;
                }

                // call do calc
                CalcClass c = new CalcClass(System.Convert.ToInt32(ForeignFacilityID), startdate, enddate, connectstring, user + " Travel");

            }


            if (faclist.Length > 0)
            {
                //SendForeignCalcMail(faclist);
            }

        }

        private void SendForeignCalcMail(string faclist)
        {
            MailMessage message = new MailMessage();

            message.BodyFormat = MailFormat.Html;
            message.From = ConfigurationManager.AppSettings["EmailFromAddress"];
            message.To = ConfigurationManager.AppSettings["TravelerPayrollCalcWarnAddress"];
            message.Subject = "PPRS: Recalculating for Travelers";
            message.Body = "<HTML><BODY>Recalculated payroll for these Facilities with Approved or Closed status: <p> " + faclist + "</p></BODY></HTML>";

            SmtpMail.SmtpServer = ConfigurationManager.AppSettings["SMTPServer"];
            SmtpMail.Send(message);


        }


        private string getPayRollStatus(string startdate, string enddate, string Fid, string connectionstring)
        {
            int iOpen;
            int iPayroll;
            int iApproved;
            int iLocked;
            string strSQL;
            string results;

            iOpen = 0;
            iPayroll = 0;
            iApproved = 0;
            iLocked = 0;

            strSQL = "";
            strSQL +=  "SELECT     PayrollStatus, COUNT(PayrollStatus) AS StatusCount ";
            strSQL +=  "FROM         dbo.EmployeeTaskWorked ";
            strSQL +=  "WHERE     (WorkDate BETWEEN '" + startdate + "' AND '" + enddate + "') AND FacilityID = " + Fid;
            strSQL +=  "GROUP BY PayrollStatus ";

            DataReader reader = new DataReader(strSQL);
            reader.Open();


            while (reader.Read())
            {
                if (((string)reader.Item("PayrollStatus")).Trim() == "OPEN")
                {
                    iOpen = System.Convert.ToInt32(reader.Item("StatusCount"));
                }
                if (((string)reader.Item("PayrollStatus")).Trim() == "PAYROLL")
                {
                    iPayroll = System.Convert.ToInt32(reader.Item("StatusCount"));
                }
                if (((string)reader.Item("PayrollStatus")).Trim() == "APPROVED")
                {
                    iApproved = System.Convert.ToInt32(reader.Item("StatusCount"));
                }
                if (((string)reader.Item("PayrollStatus")).Trim() == "LOCKED")
                {
                    iLocked = System.Convert.ToInt32(reader.Item("StatusCount"));
                }
            }
            results = "OPEN";
            if (iOpen > 0)
            {
                results = "OPEN";
            }
            if ((iOpen == 0) && (iPayroll > 0))
            {
                results = "PAYROLL";
            }
            if ((iOpen == 0) && (iPayroll == 0) && (iApproved > 0))
            {
                results = "APPROVED";
            }
            if ((iOpen == 0) && (iPayroll == 0) && (iApproved == 0) && (iLocked > 0))
            {
                results = "LOCKED";
            }

            return (results);
        }
    }
}