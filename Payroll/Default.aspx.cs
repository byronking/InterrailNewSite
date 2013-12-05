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

namespace InterrailPPRS.Payroll
{
    public partial class Default : PageBase
    {
        public SqlDataReader rsNewEmp;
        public SqlDataReader rsPreEmp;
        public string sPayRange;
        public string sPrePayRange;

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            GrantAccess("Super, Admin, User");


            string strSQL;
            string[] sRange = new string[2];
            string[] sPreRange = new string[2];

            SqlConnection sc = new SqlConnection(HttpContext.Current.Session["dbPath"].ToString());
            sc.Open();

            sRange[0] = getStartPay(System.DateTime.Now.ToShortDateString());
            sRange[1] = System.Convert.ToDateTime(sRange[0]).AddDays(6).ToShortDateString();
            sPayRange = sRange[0] + " - " + sRange[1];


            strSQL =  " SELECT Id, EmployeeNumber As EmpNum, LastName, FirstName, ";
            strSQL = strSQL + "        HireDate,  FacilityID  FROM dbo.Employee  ";
            strSQL = strSQL + " WHERE (FacilityID = " + Session["FacilityID"].ToString() + ")";
            strSQL = strSQL + "   AND (HireDate Between '" + sRange[0].ToString() + "'  AND '" + sRange[1].ToString() + "')";
            strSQL = strSQL + "   AND (TempEmployee = 0)";

            SqlCommand comNewEmp = new SqlCommand(strSQL, sc);
            comNewEmp.CommandType = CommandType.Text;
            rsNewEmp = comNewEmp.ExecuteReader();


            SqlConnection sc2 = new SqlConnection(HttpContext.Current.Session["dbPath"].ToString());
            sc2.Open();
            sPreRange[0] = getStartPay(System.DateTime.Now.AddDays(-7).ToShortDateString());
            sPreRange[1] = System.Convert.ToDateTime(sPreRange[0]).AddDays(6).ToString();
            sPrePayRange = sPreRange[0] + " - " + sPreRange[1];

            strSQL = " SELECT Id, EmployeeNumber As EmpNum, LastName, FirstName, ";
            strSQL = strSQL + "        HireDate,  FacilityID  FROM dbo.Employee  ";
            strSQL = strSQL + " WHERE (FacilityID = " + Session["FacilityID"].ToString() + ")";
            strSQL = strSQL + "   AND (HireDate Between '" + sPreRange[0].ToString() + "'  AND '" + sPreRange[1].ToString() + "')";
            strSQL = strSQL + "   AND (TempEmployee = 0)";

            SqlCommand comPreEmp = new SqlCommand(strSQL, sc2);
            comPreEmp.CommandType = CommandType.Text;
            rsPreEmp = comPreEmp.ExecuteReader();



        }
    }
}