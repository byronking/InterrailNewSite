using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;
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


namespace InterrailPPRS.Common
{
    public class PageBase : System.Web.UI.Page
    {


        public int PayPeriodEnd = 0;

        protected virtual void Page_Load(object sender, EventArgs e)
        {

            GrantAccess("Super, Admin, User");

            PayPeriodEntity ppe = new PayPeriodEntity(1);

            if (ppe.IsNew)
            {
                PayPeriodEnd = 5;
            }
            else
            {
                PayPeriodEnd = System.Convert.ToInt32(ppe.PayPeriodEnd);
            }

        }

        public string ChangeFacilityLink()
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!-- Start Change Default Facility - Include -->");
            if ((int)Session["Facilities"] > 1)
            {
                sb.AppendLine("     <tr> ");
                sb.AppendLine("        <td width=\"8%\">&nbsp;</td>");
                sb.AppendLine("       <td width=\"13%\"><img src=\"../Images/SmallRedArrow.gif\" width=\"10\" height=\"12\"></td>");
                sb.AppendLine("       <td width=\"79%\"><a href=\"../Common/SetDefaultFacility.aspx\">Change Facility</a></td>");
                sb.AppendLine("     </tr>");
                sb.AppendLine("     <tr> ");
                sb.AppendLine("       <td width=\"8%\">&nbsp;</td>");
                sb.AppendLine("       <td width=\"13%\">&nbsp;</td>");
                sb.AppendLine("       <td width=\"79%\">" + (string)Session["FacilityName"] + "</td>");
                sb.AppendLine("     </tr>");
            }
            else
            {
                sb.AppendLine("     <tr> ");
                sb.AppendLine("        <td width=\"8%\">&nbsp;</td>");
                sb.AppendLine("       <td width=\"13%\">&nbsp;</td>");
                sb.AppendLine("       <td width=\"79%\">" + (string)Session["FacilityName"] + "</td>");
                sb.AppendLine("     </tr>");

            }
            sb.AppendLine("<!-- End Change Default Facility - Include -->");
            return sb.ToString();
        }

        public bool CheckSecurity()
        {
            string UserType = "Super, Admin, User";
            if (UserType.Contains((string)Session["UserType"]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public int WeekDay(DateTime dt)
        {
           if(dt.DayOfWeek ==  DayOfWeek.Sunday){ return 1;}
           if (dt.DayOfWeek == DayOfWeek.Monday) { return 2; }
           if (dt.DayOfWeek == DayOfWeek.Tuesday) { return 3; }
           if (dt.DayOfWeek == DayOfWeek.Wednesday) { return 4; }
           if (dt.DayOfWeek == DayOfWeek.Thursday) { return 5; }
           if (dt.DayOfWeek == DayOfWeek.Friday) { return 6; }
           if (dt.DayOfWeek == DayOfWeek.Saturday) { return 7; }
           return 0;
        }

        public bool isArray(object val)
        {
            if (val is System.Array)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string cStr(object val)
        {
            return System.Convert.ToString(val);
        }

        public string Cstr(int val)
        {
            return val.ToString();
        }

        public string cstr(int val)
        {
            return val.ToString();
        }

        public string cStr(int val)
        {
            return val.ToString();
        }

        public int cInt(object val)
        {
            return System.Convert.ToInt32(val);
        }
        
        public DateTime cDate(object val)
        {
            return System.Convert.ToDateTime(val);
        }

        public bool isDate(string value)
        {
            DateTime date;
            return DateTime.TryParse(value, out date);
        }
        public double cDbl(object val)
        {
            return System.Convert.ToDouble(val);
        }

        // Returns a disconnected recordset.
        public DataTableReader getRS(string strSQL)
        {
            DataTableReader dr = null;

            SqlConnection sc = new SqlConnection(HttpContext.Current.Session["dbPath"].ToString());
            sc.Open();
            SqlCommand scom = new SqlCommand(strSQL, sc);
            SqlDataAdapter da = new SqlDataAdapter(scom);
            DataSet ds = new DataSet();
            da.Fill(ds, "Data");
            sc.Close();


            if (ds.Tables.Count > 0)
            {
               dr = ds.Tables[0].CreateDataReader();
            }
            
            return dr;

        }


        public void Execute(string strSQL)
        {

            SqlConnection sc = new SqlConnection(HttpContext.Current.Session["dbPath"].ToString());
            sc.Open();
            SqlCommand scom = new SqlCommand(strSQL, sc);
            scom.ExecuteNonQuery();
            sc.Close();

        }

        
        // Returns a disconnected recordset with Shape Connection String.
        public SqlDataReader getShapeRS(string strSQL)
        {

            SqlConnection sc = new SqlConnection(ConfigurationManager.AppSettings["MM_Shape_STRING"]);
            SqlCommand scom = new SqlCommand(strSQL, sc);
            sc.Open();
            SqlDataReader sdr = scom.ExecuteReader();
            sc.Close();
            return sdr;
        }

        public DataTableReader UpdateUPM(string dw, int taskId, string Shift, int FacilityID, int RebillDetailId)
        {

            //  sum units from production detail where dw taskid shift and facility
            //  sum hours from etw where dw taskid shift and facility
            //  UMP = total units /  total hours
            //   update etw set UPM  where dw taskid shift and facility

            double UPM;
            double UnitCount;
            double TotalHours;
            string sql;
            DataTableReader rsUnitCount;
            DataTableReader rsHourCount;
            string Update_Query;


            if (RebillDetailId == 0)
            {

                sql = "SELECT isNull(SUM(Units), 0) as UnitCount from FacilityProductionDetail ";
                sql = sql + " WHERE (TaskID = " + taskId + ") AND (FacilityID = " + FacilityID + ") AND (WorkDate = '" + dw + "') AND (ShiftID = '" + Shift + "') ";

            }
            else
            {

                sql = "SELECT isNull(SUM(TotalUnits), 0) as UnitCount from RebillDetail ";
                sql = sql + " WHERE (ID = " + RebillDetailId + ") ";

            }

            rsUnitCount = getRS(sql);
            UnitCount = 0.0;
            if (!rsUnitCount.Read())
            {
                UnitCount = System.Convert.ToDouble(rsUnitCount["UnitCount"]);
            }

            sql = "SELECT isNull(SUM(HoursWorked), 0) as TotalHours from EmployeeTaskWorked ";
            sql = sql + " WHERE (TaskID = " + taskId + ") AND (FacilityID = " + FacilityID + ") AND (WorkDate = '" + dw + "') AND (ShiftID = '" + Shift + "') ";
            if (RebillDetailId != 0)
            {
                sql = sql + " AND (RebillDetailId = " + RebillDetailId + ") ";
            }

            rsHourCount = getRS(sql);
            TotalHours = 0.0;
            if (!rsHourCount.Read())
            {
                TotalHours = System.Convert.ToDouble(rsHourCount["TotalHours"]);
            }

            UPM = 0.0;
            if (TotalHours > 0)
            {
                UPM = UnitCount / TotalHours;
            }

            Update_Query = "";
            Update_Query = Update_Query + " Update EmployeeTaskWorked Set ";
            Update_Query = Update_Query + "     UPM = " + System.Convert.ToString(UPM) + ", PayrollStatus = 'OPEN', LastModifiedBy = '" + Request["LastModifiedBy"] + "', LastModifiedOn =  '" + Request["LastModifiedOn"] + "'";
            Update_Query = Update_Query + "  WHERE (PayrollStatus <> 'LOCKED')  AND (TaskID = " + taskId + ") AND (FacilityID = " + FacilityID + ") AND (WorkDate = '" + dw + "') AND (ShiftID = '" + Shift + "')  ";

            if (RebillDetailId != 0)
            {
                Update_Query = Update_Query + " AND (RebillDetailId = " + RebillDetailId + ")";
            }

            return getRS(Update_Query);

        }
        
        public void UpdateRebillHours(int rbdID)
        {

            string sql;
            double TotalHours;
            DataTableReader rsHourCount;
            string Update_Query;
            DataTableReader rsIns;


            if (rbdID != 0)
            {

                sql = " SELECT     isNull(SUM(HoursWorked), 0) as TotalHours from EmployeeTaskWorked where RebillDetailID = " + rbdID;
                rsHourCount = getRS(sql);
                TotalHours = 0.0;


                if (!rsHourCount.Read())
                {
                    TotalHours = System.Convert.ToDouble(rsHourCount["TotalHours"]);
                }

                Update_Query = " Update RebillDetail  Set TotalHours = " + TotalHours.ToString() + " where ID = " + rbdID;
                rsIns = getRS(Update_Query);

            }

        }

        public bool isOutofTown(int empID, int facID)
        {

            string sql;
            DataTableReader rs;

            sql = "";
            sql = sql + " SELECT   isNull(count(*), 0) as cnt from Employee where ID = " + empID + " AND facilityid = " + facID + " OR facilityid in (Select AssociatedFacilityID from AssociatedFacility where FacilityID = " + facID + " ) ";
            rs = getRS(sql);

            if (System.Convert.ToInt32(rs["cnt"]) > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        
        public void GrantAccess(string AccessType)
        {

            string currentapppath = "";
            string authorizedUsers = "";
            string authFailedURL = "";
            bool grantAccess = false;
            string WhyNoAccess = "";
            string qsChar = "";
            string referrer = "";

            if (Page.Request.ServerVariables["APPL_MD_PATH"].ToString().Length > 0)
            {
                currentapppath = Page.Request.ServerVariables["APPL_MD_PATH"].ToString().Substring(Request.ServerVariables["APPL_MD_PATH"].ToString().LastIndexOf("/"));
            }

            if (currentapppath.Length == 0 || currentapppath == "/Root")
            {
                currentapppath = "/";
            }
            else
            {
                currentapppath = currentapppath + "/";
            }

            authorizedUsers = "Super";
            authFailedURL = currentapppath + "Login.aspx";
            grantAccess = false;

            if (Session["UserType"] != null && Session["UserType"].ToString() != "")
            {
                if (AccessType.IndexOf((string)Session["UserType"]) >= 1)
                {
                    grantAccess = true;
                }
                else
                {
                    grantAccess = false;
                    authFailedURL = currentapppath + "AccessDenied.aspx";
                }
            }

            if (!grantAccess)
            {
                qsChar = "?";
                if (authFailedURL.IndexOf("?") >= 1)
                {
                    qsChar = "&";
                    referrer = Request.ServerVariables["URL"];
                    if (Request.QueryString.Keys.Count > 0)
                    {
                        referrer = referrer + "?" + Request.QueryString.ToString();
                        if (Session["UserType"].ToString() != "")
                        {
                            WhyNoAccess = "why=noaccess&";
                        }
                        else
                        {
                            WhyNoAccess = "";
                        }
                        authFailedURL = authFailedURL + qsChar + WhyNoAccess + "accessdenied=" + Server.UrlEncode(referrer);
                        Response.Redirect(authFailedURL);
                    }

                }
            }

        }

        public string IIf(bool condition, string value1, string value2)
        {
            if (condition) { return value1; } else { return value2; }
        }

        public int Len(string val)
        {
            return val.Length;
        }

        public string Right(string val, int start )
        {
            return val.Substring(start);
        }

        public string Left(string val,int? length)
        {
            int len = 0;

            if (length != null)
            {
                len = System.Convert.ToInt32(length);
            }

            return val.Substring(0, len);
 
        }

        public int? InStr(int start, string val, string find, int compareType){

            if (val == null || find == null) { return null; }
            if (val == "" && find == "") { return 0; }
            if (val.Length > 0 && find == "") { return 1; }

            int position = val.IndexOf(find, start);
            return (position + 1);

        }

        public string[] Split(string val,string schar){

            return val.Split(new char[] { System.Convert.ToChar(schar) });
        }

        public string[] Split(string val, string schar,int count)
        {
            //Count is the number of strings to return.
            string[] items  = val.Split(new char[] { System.Convert.ToChar(schar) });
            string[] newItems = new string[count];
            for (int x = 0; x < count; x++)
            {
                newItems[x] = items[x];
            }

            return newItems;
        }

        public int UBound(string[] arry)
        {
            return arry.Length;
        }
        public int LBound(string[] arry)
        {
            return 0;
        }


        public string UCase(string val)
        {
            return val.ToUpper();
        }

        public string LCase(string val)
        {
            return val.ToLower();
        }

        public string Trim(string val)
        {
            return val.Trim();
        }

        public string Replace(string val, string find, string replacewith)
        {
            return val.Replace(find, replacewith);
        }

        public string FormatNumber(string val, int decplaces)
        {
           return string.Format("{0:n}", System.Convert.ToDecimal(val.Replace(",","")));
        }

        public int? StrComp(string val1,string val2, int textorbin){

            if (val1 == null || val2 == null) { return null; }
            if(textorbin == 1){
                if(val1.ToLower() == val2.ToLower()){return 0;}else{return -1;}
            }else{
                if(val1 == val2){return 0;}else{return -1;}
             }
        }

        // a utility function used for adding additional parameters to these strings
        public string MM_joinChar(string firstItem)
        {
            if (firstItem != "")
            {
                return "&";
            }
            else
            {
                return "";
            }
        }

        public string getPayRollStatus(string strDate, int Fid)
        {

            int iOpen;
            int iPayroll;
            int iApproved;
            int iLocked;
            string strSQL;
            DataTableReader rs;
            DateTime LastEnd;
            DateTime LastStart;

            if (!isDate(strDate))
            {
                strDate = DateTime.Now.ToShortDateString();
            }


            int val = ((((int)(System.Convert.ToDateTime(strDate).DayOfWeek)) + 1) + (7 - (PayPeriodEnd + 1))) % 7;
            LastStart = System.Convert.ToDateTime(strDate).AddDays(val = -(val));
            //LastStart = System.Convert.ToString(strDate) - ( (WeekDay(System.Convert.ToString(strDate))+(7-(PayPeriodEnd+1))) % 7 );

            LastEnd = LastStart.AddDays(6);

            iOpen = 0;
            iPayroll = 0;
            iApproved = 0;
            iLocked = 0;

            strSQL = "";
            strSQL +=  "SELECT PayrollStatus, COUNT(PayrollStatus) AS StatusCount ";
            strSQL +=  "FROM  dbo.EmployeeTaskWorked ";
            strSQL +=  "WHERE (WorkDate BETWEEN '" + LastStart.ToString() + "' AND '" + LastEnd.ToString() + "') AND FacilityID = " + Fid.ToString();
            strSQL +=  "GROUP BY PayrollStatus ";

            rs = getRS(strSQL);


            while (rs.Read())
            {
                if (System.Convert.ToString(rs["PayrollStatus"]).Trim() == "OPEN")
                {
                    iOpen = System.Convert.ToInt32(rs["StatusCount"]);
                }
                if (System.Convert.ToString(rs["PayrollStatus"]).Trim() == "PAYROLL")
                {
                    iPayroll = System.Convert.ToInt32(rs["StatusCount"]);
                }
                if (System.Convert.ToString(rs["PayrollStatus"]).Trim() == "APPROVED")
                {
                    iApproved = System.Convert.ToInt32(rs["StatusCount"]);
                }
                if (System.Convert.ToString(rs["PayrollStatus"]).Trim() == "LOCKED")
                {
                    iLocked = System.Convert.ToInt32(rs["StatusCount"]);
                }
            }

            strSQL = "";

            if (iOpen > 0)
            {
                return "OPEN";
            }
            if (iOpen == 0 && iPayroll > 0)
            {
                return "PAYROLL";
            }
            if (iOpen == 0 && iPayroll == 0 && iApproved > 0)
            {
                return "APPROVED";
            }
            if (iOpen == 0 && iPayroll == 0 && iApproved == 0 && iLocked > 0)
            {
                return "LOCKED";
            }

            return "OPEN";

        }
        public string getPayRollStatus(string strDate,string Fid){

          int iOpen;
          int iPayroll;
          int iApproved;
          int iLocked;
          string strSQL;
          DataTableReader rs;
          DateTime LastEnd;
          DateTime LastStart;
          string PayRollStatus;
 
          if(! isDate(strDate) ){
              strDate = cStr(System.DateTime.Now);
          }

          LastStart = (cDate(strDate)).AddDays( - ( (WeekDay(cDate(strDate))+(7-(PayPeriodEnd+1))) % 7 ));
          LastEnd = LastStart.AddDays(6);


          iOpen = 0;
          iPayroll = 0;
          iApproved = 0;
          iLocked = 0;

          strSQL =  "";
          strSQL +=  "SELECT     PayrollStatus, COUNT(PayrollStatus) AS StatusCount ";
          strSQL +=  "FROM         dbo.EmployeeTaskWorked ";
          strSQL +=  "WHERE     (WorkDate BETWEEN '" + cStr(LastStart) + "' AND '" + cStr(LastEnd) + "') AND FacilityID = " + cStr(Fid);
          strSQL +=  "GROUP BY PayrollStatus ";

          rs = getRS(strSQL);


          while(rs.Read()){
            if ( Trim(rs["PayrollStatus"].ToString()) == "OPEN" ){
                   iOpen = cInt(rs["StatusCount"]);
            }
            if ( Trim(rs["PayrollStatus"].ToString()) == "PAYROLL" ){
                   iPayroll = cInt(rs["StatusCount"]);
            }
            if ( Trim(rs["PayrollStatus"].ToString()) == "APPROVED" ){
                   iApproved = cInt(rs["StatusCount"]);
            }
            if ( Trim(rs["PayrollStatus"].ToString()) == "LOCKED" ){
                   iLocked = cInt(rs["StatusCount"]);
            }

          }

          strSQL =  "";


          PayRollStatus = "OPEN";

          if (iOpen > 0 ){
              PayRollStatus = "OPEN";
          }
          if (iOpen==0 && iPayroll>0 ){
              PayRollStatus = "PAYROLL";
          }
          if ( iOpen==0 && iPayroll==0 && iApproved>0 ){
              PayRollStatus = "APPROVED";
          }
          if (iOpen==0 && iPayroll==0 && iApproved==0 && iLocked>0  ){
              PayRollStatus = "LOCKED";
          }

            return PayRollStatus;

        }
        public string getStartPay(string TheDate)
        {

            DateTime LastStart;
            string sDay;
            string sMon;
            string sFrom;
            string dDate;

            if (isDate(TheDate))
            {
                dDate = System.Convert.ToDateTime(TheDate).ToShortDateString();
            }
            else
            {
                dDate = DateTime.Now.ToShortDateString();
            }

            int val = ((((int)(System.Convert.ToDateTime(dDate).DayOfWeek)) + 1) + (7 - (PayPeriodEnd + 1))) % 7;
            LastStart = System.Convert.ToDateTime(dDate).AddDays(val = -(val));

            //LastStart = dDate - ( (WeekDay(dDate) + (7-(PayPeriodEnd+1))) % 7 )

            sDay = LastStart.Day.ToString();

            if (System.Convert.ToInt32(sDay) < 10)
            {
                sDay = "0" + System.Convert.ToString(sDay);
            }
            else
            {
                //sDay = sDay;
            }

            sMon = LastStart.Month.ToString();
            if (System.Convert.ToInt32(sMon) < 10)
            {
                sMon = "0" + System.Convert.ToString(sMon);
            }
            else
            {
                sMon = System.Convert.ToString(sMon);
            }

            sFrom = sMon + "/" + sDay + "/" + LastStart.Year.ToString();
            return sFrom;

        }
        public string getPayPeriods(int nStart, int nPeriods, string strSelectStartDate)
        {

            DateTime LastEnd;
            DateTime LastStart;
            string strHTML;
            string sDay;
            string sMon;
            string strSelect;
            string sFrom;
            string sTo;

            LastStart = System.Convert.ToDateTime(getStartPay(DateTime.Now.ToShortDateString()));
            LastEnd = LastStart.AddDays(6);

            strHTML = "";

            for (int i = nStart; i < nPeriods; i++)
            {
                sDay = LastStart.AddDays(System.Convert.ToInt32("-" + (7 * i).ToString())).Day.ToString();
                if (System.Convert.ToInt32(sDay) < 10)
                {
                    sDay = "0" + sDay.ToString();
                }
                else
                {
                    sDay = sDay.ToString();
                }

                sMon = LastStart.AddDays(System.Convert.ToInt32("-" + (7 * i).ToString())).Month.ToString();
                if (System.Convert.ToInt32(sMon) < 10)
                {
                    sMon = "0" + sMon.ToString();
                }
                else
                {
                    sMon = sMon.ToString();
                }

                sFrom = sMon + "/" + sDay + "/" + LastStart.AddDays(System.Convert.ToInt32("-" + (7 * i).ToString())).Year.ToString();

                sDay = LastEnd.AddDays(System.Convert.ToInt32("-" + (7 * i).ToString())).Day.ToString();
                if (System.Convert.ToInt32(sDay) < 10)
                {
                    sDay = "0" + sDay.ToString();
                }
                else
                {
                    sDay = sDay.ToString();
                }

                sMon = LastEnd.AddDays(System.Convert.ToInt32("-" + (7 * i).ToString())).Month.ToString();
                if (System.Convert.ToInt32(sMon) < 10)
                {
                    sMon = "0" + sMon.ToString();
                }
                else
                {
                    sMon = sMon.ToString();
                }

                sTo = sMon + "/" + sDay + "/" + LastEnd.AddDays(System.Convert.ToInt32("-" + (7 * i).ToString())).Year.ToString();

                strSelect = " ";
                if (isDate(strSelectStartDate))
                {
                    if ((System.Convert.ToDateTime(sFrom) <= System.Convert.ToDateTime(strSelectStartDate)) && (System.Convert.ToDateTime(strSelectStartDate) <= System.Convert.ToDateTime(sTo)))
                    {
                        strSelect = " SELECTED ";
                    }
                }
                strHTML = strHTML + "<option value=\"" + sFrom + "," + sTo + "\"" + strSelect + ">" + sFrom + " - " + sTo + "</option>";
            }

            return strHTML;

        }
        


    }
}