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

namespace InterrailPPRS.Common
{
    public partial class SetDefaultFacility : PageBase
    {

        public DataReader rs;
        public int rs_numRows = 0;
        public string sFrom = "";

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

           // Facility session variables && return to caller.;
           if(Request["goTo"] != null && Request["goTo"] != ""){
             Session["FacilityID"]   = Request["FacilityID"];
             Session["FacilityName"] = Request["FacilityName"];
             Response.Redirect(Request["goTo"]);
           }

            string rs__PUserID;
            rs__PUserID = "17";
            if (Session["UserID"] != "") { rs__PUserID = System.Convert.ToString(Session["UserID"]); }


            string rs__PUserType;
            rs__PUserType = "Super";
            if(Session["UserType"] != ""){rs__PUserType = System.Convert.ToString(Session["UserType"]);}


            rs = new DataReader("SELECT Distinct f.Id, f.Name, f.AlphaCode  FROM dbo.UserRights r RIGHT OUTER JOIN  dbo.Facility f ON r.FacilityId = f.Id  WHERE f.Active=1 AND (r.UserProfileId = " + Replace(rs__PUserID, "'", "''") + "  OR '" + Replace(rs__PUserType, "'", "''") + "' = 'Admin' OR '" + Replace(rs__PUserType, "'", "''") + "'='Super')  Order By Name");
            rs.Open();
            rs_numRows = rs.RecordCount;


            GrantAccess("Super, Admin, User");
            
            sFrom = Request.ServerVariables["HTTP_REFERER"];

           if(InStr(0, UCase(sFrom), "/ADMIN/",1) >= 1){
             sFrom = "../Admin/Default.aspx";
           }else{
               if(InStr(0, UCase(sFrom), "/PRODUCTION/",1) >= 1){
                   sFrom = "../Production/Default.aspx";
               }else{
                    if(InStr(0, UCase(sFrom), "/PAYROLL/",1)    >= 1){
                      sFrom = "../Payroll/Default.aspx";
                    }else{
                        if(InStr(0, UCase(sFrom), "/REBILLING/",1)  >= 1){
                            sFrom = "../Rebilling/Default.aspx";
                       }else{
                             sFrom = "../Default.aspx";
                       }
                    }
               }
            }

        }
    }
}