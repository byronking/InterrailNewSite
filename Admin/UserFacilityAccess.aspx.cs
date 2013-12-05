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

namespace InterrailPPRS.Admin
{
    public partial class UserFacilityAccess : PageBase
    {

        public DataReader rs;
        public DataReader rsUserName;
        public int Repeat1__numRows;
        public int Repeat1__index;
        public int rsUserName_numRows = 0;
        public int rs_numRows = 0;
        public string MM_editRedirectUrl = "";
        public string MM_editAction = "";

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin");


            // *** Edit Operations: declare variables;

            MM_editAction = cStr(Request["URL"]);
            if(Request.QueryString.Count > 0){
             //  MM_editAction = MM_editAction + "?" + Request.QueryString;
            }

            // boolean to abort record edit;
            bool MM_abortEdit = false;

            // query string to execute;
            string MM_editQuery = "";


            // *** Update Record: variables;

            if(cStr(Request["MM_update"]) != "" && cStr(Request["MM_recordId"]) != ""){


              string MM_editTable = "dbo.UserRights";
              string MM_editColumn = "UserProfileId";
              string MM_recordId = "" + Request.Form["MM_recordId"] + "";
              MM_editRedirectUrl = "User.aspx";

              // append the query string to the redirect URL;
              if(MM_editRedirectUrl != "" && Request.QueryString.Count > 0){
                if(InStr(0, MM_editRedirectUrl, "?", 1) == 0 && Request.QueryString.Count > 0){
                  MM_editRedirectUrl = MM_editRedirectUrl + "?" + Request.QueryString;
                }else{
                  MM_editRedirectUrl = MM_editRedirectUrl + "&" + Request.QueryString;
               }
             }

            }


            // *** Update Record: construct a sql update statement && execute it;

            if(cStr(Request["MM_update"]) != "" && cStr(Request["MM_recordId"]) != ""){

              // create the sql update statement;
              MM_editQuery = "delete from UserRights where UserProfileID = " + cStr(Request["MM_RecordID"]) ;

              string[] checkboxArray = Split(Request["checkbox"],",");
              for( int i = LBound(checkboxArray); i < UBound(checkboxArray); i++){
                  MM_editQuery = MM_editQuery + " Insert into UserRights (UserProfileID, FacilityID, LastModifiedBy, LastModifiedOn) ";
                  MM_editQuery = MM_editQuery + " Values ( " +  cStr(Request["MM_RecordID"]) + ", " + checkboxArray[i] + ", '" + Session["UserName"] + "', '" + System.DateTime.Now.ToString() + "' ) " ;
               }

              if(!MM_abortEdit){
                // execute the update;
                this.Execute(MM_editQuery);
                if(MM_editRedirectUrl != ""){
                  Response.Redirect(MM_editRedirectUrl);
               }
             }

            }


            string rs__MMColParam;
            rs__MMColParam = "1";
            if(Request.QueryString["Id"] != ""){rs__MMColParam = Request.QueryString["Id"];}


            rs = new DataReader("SELECT ur.UserProfileId, ur.FacilityId, ur.LastModifiedBy, ur.LastModifiedOn, ur.RegionalRight, f.Name, f.Id  FROM dbo.UserRights ur, dbo.Facility f  WHERE f.ID *= ur.FacilityID  AND UserProfileId = " + Replace(rs__MMColParam, "'", "''") + "");
            rs.Open();
            rs_numRows = 0;


            string rsUserName__MMColParam;
            rsUserName__MMColParam = "1";
            if(Request.QueryString["Id"] != ""){rsUserName__MMColParam = Request.QueryString["Id"];}



            rsUserName = new DataReader( "SELECT Id, UserID, UserName FROM dbo.UserProfile WHERE Id = " + Replace(rsUserName__MMColParam, "'", "''") + "");
            rsUserName.Open();
            rsUserName.Read();
            rsUserName_numRows = rsUserName.RecordCount;


            Repeat1__numRows = -1;
            Repeat1__index = 0;
            rs_numRows = rs_numRows + Repeat1__numRows;



        }
    }
}