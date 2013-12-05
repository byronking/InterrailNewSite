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
    public partial class CalcWait : PageBase
    {
        protected override  void Page_Load(object sender, EventArgs e){

            base.Page_Load(sender, e);

            GrantAccess("Super, Admin, User");

            string MM_Main_STRING = "";

           // MM_Main_STRING = System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"];

            if(isDate(Request["StartDate"]) && isDate(Request["EndDate"]) ){

                Response.Write("<script type='text/javascript'> setTimeout(function() {location.href = 'FastCalc.aspx?StartDate=" + Request["StartDate"] + "&EndDate=" + Request["EndDate"] + "&ConnectString=" + Replace(MM_Main_STRING, "Provider=SQLOLEDB;", "") + "&FacilityID=" + Session["FacilityID"] + "&User=" + Session["UserName"] + "';},3000); </script>");
            } 

        }
    }
}