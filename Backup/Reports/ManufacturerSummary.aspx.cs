﻿using System;
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
    public partial class ManufacturerSummary : PageBase
    {
        public DataReader rsFac;
        public string rsFac__PUserID;
        public string rsFac__PUserType;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin, User");

            rsFac__PUserID = "17";
            if (Session["UserID"] != "") { rsFac__PUserID = cStr(Session["UserID"]); }

            rsFac__PUserType = "super";
            if(cStr(Session["UserType"]) != ""){rsFac__PUserType = cStr(Session["UserType"]);}

            rsFac = new DataReader( "SELECT Distinct f.Id, f.Name, f.AlphaCode  FROM dbo.UserRights r RIGHT OUTER JOIN  dbo.Facility f ON r.FacilityId = f.Id  WHERE f.Active=1 AND (r.UserProfileId = " + Replace(rsFac__PUserID, "'", "''") + "  OR '" + Replace(rsFac__PUserType, "'", "''") + "' = 'Admin' OR '" + Replace(rsFac__PUserType, "'", "''") + "'='Super')  Order By Name");
            rsFac.Open();

        }
    }
}