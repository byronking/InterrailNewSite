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


namespace InterrailPPRS.Reports
{
    public class PageBase : System.Web.UI.Page
    {


        public int PayPeriodEnd = 0;

        protected virtual void Page_Load(object sender, EventArgs e)
        {

            GrantAccess("Super, Admin, User");

           
        }

        public string ChangeFacilityLink()
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!-- Start Change Default Facility - Include -->");
            if (Session["Facilities"] != null && (int)Session["Facilities"] > 1)
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

        public bool CheckSecurity(string list)
        {
            if (list.Contains((string)Session["UserType"]))
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
            if (val != null)
            {
                return System.Convert.ToString(val);
            }
            else
            {
                return "";
            }

        }

        public int cInt(object val)
        {
            return System.Convert.ToInt32(val);
        }
        
        public DateTime cDate(object val)
        {
            DateTime outDate = new DateTime();

            if (DateTime.TryParse(val.ToString(), out outDate))
            {
                outDate = System.Convert.ToDateTime(val);
            }
            else
            {
                var error = val;
            }

            return outDate;
            //return System.Convert.ToDateTime(val);
        }

        public bool isDate(string value)
        {
            DateTime date;
            return DateTime.TryParse(value, out date);
        }
        public bool isNumeric(string value)
        {
            double num;
            return Double.TryParse(value,out num);
        }

        public double cDbl(object val)
        {
            if (val == "")
            {
                return 0.0;
            }
            else
            {
                return System.Convert.ToDouble(val);
            }
        }

        // Returns a disconnected recordset.
        public DataTableReader getRS(string strSQL)
        {

            SqlConnection sc = new SqlConnection(HttpContext.Current.Session["dbPath"].ToString());
            sc.Open();
            SqlCommand scom = new SqlCommand(strSQL, sc);
            SqlDataAdapter da = new SqlDataAdapter(scom);
            DataSet ds = new DataSet();
            da.Fill(ds, "Data");
            sc.Close();

            DataTableReader dr = ds.Tables[0].CreateDataReader();
            
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
            if (val == null)
            {
                return 0;
            }
            else
            {
                return val.Length;
            }
        }

        public string Right(string val, int start )
        {

            int len = 0;

            if (start != null)
            {
                len = System.Convert.ToInt32(start);
            }

            return val.Substring(len);
        }

        public string Left(string val, int? length)
        {
            int len = 0;

            if (length != null)
            {
                len = System.Convert.ToInt32(length);
            }

            if (length > -1)
            {
                return val.Substring(0, len);
            }
            else
            {
                return val;
            }
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
        public int UBound(string[,] arry,int dim)
        {
            return arry.GetLength(dim);
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
            if (val == null) return "";
            return val.Trim();
        }

        public string Mid(string val,int start,int length)
        {
            return val.Substring(start, length);
        }

        public string Date()
        {
            return System.DateTime.Now.ToString("MM/dd/yyyy");
        }

        public string Replace(string val, string find, string replacewith)
        {
            return val.Replace(find, replacewith);
        }

        public string FormatNumber(string val, int decplaces,int optional1 = -2,int optional2 = -2,int optional3 = -2 )
        {
            if (val == "") { return val; }
            else
            {
                return string.Format("{0:C" + decplaces + "}", System.Convert.ToDouble(val.Replace(",", "")));
            }
        }

        public string FormatDate(string sDate){

            if( isDate(sDate) ){
                return System.Convert.ToDateTime(sDate).ToString("MM/dd/yyyy");
            }else{
                return Date();
            }

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


        public string Month(DateTime val)
        {
            return val.Month.ToString();
        }
        public string Day(DateTime val)
        {
            return val.Day.ToString();
        }
        public string Year(DateTime val)
        {
            return val.Year.ToString();
        }
        public string MonthName(int month)
        {
            DateTime dt = new DateTime(2012, month, 1);
            return dt.ToString("MMM");
        }
        public int Weekday(DateTime dt)
        {
            return ((int)dt.DayOfWeek) + 1;
        }


        public string Str(int number,string val )
        {
            StringBuilder sb = new StringBuilder();
            for(int x=0; x < number; x++){
                sb.Append(val);
            }

            return sb.ToString();
        }

        public string getPayRollStatus(string strDate, int Fid)
        {

            int iOpen;
            int iPayroll;
            int iApproved;
            int iLocked;
            string strSQL;
            DataReader rs;
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

            rs = new DataReader(strSQL);
            rs.Open();

            while (rs.Read())
            {
                if (System.Convert.ToString(rs.Item("PayrollStatus")).Trim() == "OPEN")
                {
                    iOpen = System.Convert.ToInt32(rs.Item("StatusCount"));
                }
                if (System.Convert.ToString(rs.Item("PayrollStatus")).Trim() == "PAYROLL")
                {
                    iPayroll = System.Convert.ToInt32(rs.Item("StatusCount"));
                }
                if (System.Convert.ToString(rs.Item("PayrollStatus")).Trim() == "APPROVED")
                {
                    iApproved = System.Convert.ToInt32(rs.Item("StatusCount"));
                }
                if (System.Convert.ToString(rs.Item("PayrollStatus")).Trim() == "LOCKED")
                {
                    iLocked = System.Convert.ToInt32(rs.Item("StatusCount"));
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
        public string getPayRollStatus(string strDate, string Fid)
        {

            int iOpen;
            int iPayroll;
            int iApproved;
            int iLocked;
            string strSQL;
            DataReader rs;
            DateTime LastEnd;
            DateTime LastStart;
            string PayRollStatus;

            if (!isDate(strDate))
            {
                strDate = cStr(System.DateTime.Now);
            }

            LastStart = (cDate(strDate)).AddDays(-((WeekDay(cDate(strDate)) + (7 - (PayPeriodEnd + 1))) % 7));
            LastEnd = LastStart.AddDays(6);


            iOpen = 0;
            iPayroll = 0;
            iApproved = 0;
            iLocked = 0;

            strSQL = "";
            strSQL +=  "SELECT     PayrollStatus, COUNT(PayrollStatus) AS StatusCount ";
            strSQL +=  "FROM         dbo.EmployeeTaskWorked ";
            strSQL +=  "WHERE     (WorkDate BETWEEN '" + cStr(LastStart) + "' AND '" + cStr(LastEnd) + "') AND FacilityID = " + cStr(Fid);
            strSQL +=  "GROUP BY PayrollStatus ";

            rs = new DataReader(strSQL);
            rs.Open();

            while (rs.Read())
            {
                if (Trim(rs.Item("PayrollStatus").ToString()) == "OPEN")
                {
                    iOpen = cInt(rs.Item("StatusCount"));
                }
                if (Trim(rs.Item("PayrollStatus").ToString()) == "PAYROLL")
                {
                    iPayroll = cInt(rs.Item("StatusCount"));
                }
                if (Trim(rs.Item("PayrollStatus").ToString()) == "APPROVED")
                {
                    iApproved = cInt(rs.Item("StatusCount"));
                }
                if (Trim(rs.Item("PayrollStatus").ToString()) == "LOCKED")
                {
                    iLocked = cInt(rs.Item("StatusCount"));
                }

            }

            strSQL = "";


            PayRollStatus = "OPEN";

            if (iOpen > 0)
            {
                PayRollStatus = "OPEN";
            }
            if (iOpen == 0 && iPayroll > 0)
            {
                PayRollStatus = "PAYROLL";
            }
            if (iOpen == 0 && iPayroll == 0 && iApproved > 0)
            {
                PayRollStatus = "APPROVED";
            }
            if (iOpen == 0 && iPayroll == 0 && iApproved == 0 && iLocked > 0)
            {
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

        public void  UpdateUPM(string dw,string  taskId,string Shift, string FacilityID,int  RebillDetailId){

                //  sum units from production detail where dw taskid shift and facility
                //  sum hours from etw where dw taskid shift and facility
                //  UMP = total units /  total hours
                //  update etw set UPM  where dw taskid shift and facility

               double  UPM = 0.0;
               double  UnitCount = 0.0;
               double TotalHours = 0.0;
               string  sql = "";
               DataReader rsUnitCount;
               DataReader rsHourCount;
               string  Update_Query = "";
  
   
               if (RebillDetailId == 0) {

               sql = "";
               sql = sql + " SELECT     isNull(SUM(Units), 0) as UnitCount from FacilityProductionDetail ";
               sql = sql + " WHERE (TaskID = " + taskId + ") AND (FacilityID = " + FacilityID + ") AND (WorkDate = '" + dw + "') AND (ShiftID = '" + Shift + "')  ";
   
               }else{
               sql = "";
               sql = sql + " SELECT  isNull(SUM(TotalUnits), 0) as UnitCount from RebillDetail ";
               sql = sql + " WHERE (ID = " + RebillDetailId + ") ";
   
               } 
   
               rsUnitCount = new DataReader(sql);
               rsUnitCount.Open();
               UnitCount = 0.0;
               if (! rsUnitCount.EOF){
                  rsUnitCount.Read();
                  UnitCount = cDbl(rsUnitCount.Item("UnitCount"));
               }


               sql = "";
               sql = sql + " SELECT     isNull(SUM(HoursWorked), 0) as TotalHours from EmployeeTaskWorked ";
               sql = sql + " WHERE (TaskID = " + taskId + ") AND (FacilityID = " + FacilityID + ") AND (WorkDate = '" + dw + "') AND (ShiftID = '" + Shift + "')  ";
               if ( RebillDetailId != 0 ){
                   sql = sql +  " AND (RebillDetailId = " + RebillDetailId +") ";
               }
   
               rsHourCount = new DataReader(sql);
               rsHourCount.Open();
               TotalHours = 0.0;
               if (!rsHourCount.EOF ){
                  rsHourCount.Read();
                  TotalHours = cDbl(rsHourCount.Item("TotalHours"));
               }


                UPM = 0.0;
                if( TotalHours > 0 ){
                    UPM   =  UnitCount  / TotalHours;
                }

                Update_Query = "";
                Update_Query = Update_Query + " Update EmployeeTaskWorked Set ";
                Update_Query = Update_Query +  " UPM = "  + cStr(UPM) + ", PayrollStatus = 'OPEN', LastModifiedBy = '" +  Request["LastModifiedBy"] + "', LastModifiedOn =  '" + Request["LastModifiedOn"]  + "'" ;
                Update_Query = Update_Query + "  WHERE (PayrollStatus <> 'LOCKED')  AND (TaskID = " + taskId + ") AND (FacilityID = " + FacilityID + ") AND (WorkDate = '" + dw + "') AND (ShiftID = '" + Shift + "')  ";
                if (RebillDetailId != 0 ){
                   Update_Query = Update_Query +  " AND (RebillDetailId = " + RebillDetailId + ") ";
                }
    
                this.Execute(Update_Query);


        }

        public bool isOutofTown( string empID,string facID ){

           string sql = "";
           sql = sql + " SELECT   isNull(count(*), 0) as cnt from Employee where ID = " + empID + " AND facilityid = " + facID + " OR facilityid in (Select AssociatedFacilityID from AssociatedFacility where FacilityID = " + facID + " ) ";
           DataReader rs = new DataReader(sql);
           if (cInt(rs.Item("cnt")) > 0 ){
             return  false;
           }else{
             return true;
           }

        }


        public double Ave(string[] ar)
        {

            double localSum = 0;
            double localCount = 0;

            for (int i = 0; i < (UBound(ar) - 1); i++)
            {
                if (Trim(ar[i]) != "")
                {
                    localSum = localSum + SafeDbl(ar[i]);
                    localCount = localCount + 1;
                }
            }
            if (localCount > 0)
            {
                return localSum / localCount;
            }
            else
            {
                return 0;
            }
        }

        public double Ave(string[,] ar, int col){

              int i;
              double localSum = 0;
              double localCount = 0;
              for (i = 0; i < (UBound(ar, 1) - 1); i++ )
              {
                  if (Trim(ar[col, i]) != "")
                  {
                      localSum = localSum + SafeDbl(ar[col, i]);
                      localCount = localCount + 1;
                  }
              } //Next
            if(localCount > 0){
                    return ( localSum / localCount);
            }else{
                    return 0;
            }

        }

        public string FPerc(double num)
        {

            if (num != 0.0)
            {
                return cStr(FormatNumber(cStr(num*100), 2)) + "%";
            }
            else
            {
                return "0%";
            }
        }

        public double SafeDbl(string num)
        {

            if (num != null)
            {
                if (isNumeric(cStr(num)))
                {
                    return cDbl(num);
                }
            }

            return 0.0;
        }

        public double SafeDiv(string n, string d)
        {

            if (SafeDbl(d) != 0)
            {
                return SafeDbl(n) / SafeDbl(d);
            }

            return 0;
        }

        public string FormatTheDate(DateTime inDate)
        {
            /*string smDate, sdDate, syDate;

            smDate = Right(cStr(inDate.Month + 100), 2);
            sdDate = Right(cStr(inDate.Day + 100), 2);
            syDate = cStr(inDate.Year);
            return smDate + "/" + sdDate + "/" + syDate;*/

            return inDate.ToString("MM/dd/yyyy");

        }

        public string FNum(string num, int dec)
        {

            if (cDbl(num) == 0)
            {
                return "0";
            }
            else
            {
                return FormatNumber(num, dec);
            }

        }

        public string FCur(string num, int dec)
        {
            if (cDbl(num) == 0)
            {
                //return "$0";
                return "0";
            }
            else
            {
                //return "$ " + cStr(FormatNumber(num, dec));
                return cStr(FormatNumber(num, dec));
            }
        }
    }
}