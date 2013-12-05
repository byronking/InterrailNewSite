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
    public partial class MAS90Select : PageBase
    {
        public DataReader rsFac;
        public DataReader rsComp;
        public int rsFac_numRows = 0;
 
        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin, User");


            string rsFac__PUserID;
            rsFac__PUserID = "17";
            if(System.Convert.ToString(Session["UserID"]) != ""){rsFac__PUserID = System.Convert.ToString(Session["UserID"]);}


            string rsFac__PUserType;
            rsFac__PUserType = "super";
            if (System.Convert.ToString(Session["UserType"]) != "") { rsFac__PUserType = System.Convert.ToString(Session["UserType"]); }


            rsFac = new DataReader( "SELECT Distinct f.Id, f.Name, f.AlphaCode  FROM dbo.UserRights r RIGHT OUTER JOIN  dbo.Facility f ON r.FacilityId = f.Id  WHERE f.Active=1 AND (r.UserProfileId = " + Replace(rsFac__PUserID, "'", "''") + "  Or '" + Replace(rsFac__PUserType, "'", "''") + "' = 'Admin' Or '" + Replace(rsFac__PUserType, "'", "''") + "'='Super')");
            rsFac.Open();
            rsFac_numRows = 0;

            rsComp = new DataReader("SELECT  Id, CompanyName FROM IRGCompany Order By CompanyName");
            rsComp.Open();



        }
    }
}