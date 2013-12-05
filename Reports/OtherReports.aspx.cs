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

namespace InterrailPPRS.Reports
{
    public partial class OtherReports : PageBase
    {

        public DataReader rsFac;
        public string sURL;
        public string sFacilities = "0";

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin, User");



            string rsFac__PUserID;
            rsFac__PUserID = "17";
            if(cStr(Session["UserID"]) != ""){rsFac__PUserID = cStr(Session["UserID"]);}

             
            string rsFac__PUserType;
            rsFac__PUserType = "super";
            if(Session["UserType"] != ""){rsFac__PUserType = cStr(Session["UserType"]);}




            rsFac = new DataReader( "SELECT Distinct f.Id, f.Name, f.AlphaCode  FROM dbo.UserRights r RIGHT OUTER JOIN  dbo.Facility f ON r.FacilityId = f.Id  WHERE f.Active=1 AND (r.UserProfileId = " + Replace(rsFac__PUserID, "'", "''") + "  OR '" + Replace(rsFac__PUserType, "'", "''") + "' = 'Admin' OR '" + Replace(rsFac__PUserType, "'", "''") + "'='Super')  Order By Name");
            rsFac.Open();

               sFacilities = "0";
               while (!rsFac.EOF){
                   rsFac.Read();
                   sFacilities += ", " + cStr(rsFac.Item("Id"));

               }
               rsFac.Requery();

            if(cStr(Request["RptType"]) == "USERSBYFACILITY"){
              if(cStr(Session["UserType"]).ToUpper() == "USER"){
                sURL = "GenerateUsersByFacility.aspx?selFacilities=" + sFacilities;
              }else{
                sURL = "GenerateUsersByFacility.aspx";
              }

              Response.Redirect(sURL);
            }


            if(cStr(Request["RptType"]) == "TEMPORARY"){
              if(cStr(Session["UserType"]).ToUpper() == "USER"){
                sURL = "GenerateTemporary.aspx?selFacilities="+sFacilities;
              }else{
                sURL = "GenerateTemporary.aspx";
              }

              Response.Redirect(sURL);
            }


        }
    }
}