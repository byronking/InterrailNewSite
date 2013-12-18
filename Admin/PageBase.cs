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


namespace InterrailPPRS.Admin
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
            return System.Convert.ToString(val);
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

        public string Left(string val,int? length)
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

        public string[] Split(string val, string schar){
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

        public string Mid(string val,int start,int length)
        {
            return val.Substring(start, length);
        }

        public string Date()
        {
            return System.DateTime.Now.ToString();
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
                return string.Format("{0:n}", System.Convert.ToDecimal(val.Replace(",", "")));
            }
        }

        public string FormatDate(DateTime val)
        {
            return (val == null) ? "" : val.ToString("MM/dd/yyyy");
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
    }
}