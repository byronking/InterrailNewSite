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
    public partial class Settings : PageBase
    {

        public string rs__PFacilityID;
        public DataReader rs;
        public DataReader MM_rs;    
        public int rs_numRows;
        public int rs_rows;
        public int rs_total;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

            GrantAccess("Super, Admin, User");

            rs__PFacilityID = "3";
            if(Session["FacilityID"] != null && Session["FacilityID"]  != ""){ rs__PFacilityID = cStr(Session["FacilityID"]); }

            if (Session["dbMode"].ToString() == "Archive")
            {
                rdoDB_Archive.Checked = true;
            }
            else
            {
                rdoDB_Production.Checked = true;
            }

        }

        protected void rdoDB_Production_CheckedChanged(object sender, EventArgs e)
        {
            Session["dbPath"] = ConfigurationManager.AppSettings["MM_Main_STRING"];
            Session["dbMode"] = "Production";
        }

        protected void rdoDB_Archive_CheckedChanged(object sender, EventArgs e)
        {
            Session["dbPath"] = ConfigurationManager.AppSettings["MM_Archive_STRING"];
            Session["dbMode"] = "Archive";
        }


    }
}