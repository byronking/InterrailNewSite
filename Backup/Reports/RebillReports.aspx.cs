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
    public partial class RebillReports : PageBase
    {

        public DataReader rsTask;
        public int rsTask_numRows = 0;
        public DataReader rsSubTask;
        public int rsSubTask_numRows = 0;
        public DataReader rsCustomers;
        public int rsCustomers_numRows = 0;
        public DataReader rsFac;
        public int rsFac_numRows = 0;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin, User");


            string rsTask__PUserID;
            rsTask__PUserID = "1";
            if(cStr(Session["UserID"]) != ""){
                rsTask__PUserID = cStr(Session["UserID"]);
            }


            string rsTask__PUserType;
            rsTask__PUserType = "admin";
            if(cStr(Session["UserType"]) != ""){rsTask__PUserType = cStr(Session["UserType"]);}


            rsTask=  new DataReader( "SELECT Distinct dbo.Tasks.Id, dbo.Tasks.TaskCode, dbo.Tasks.TaskDescription  FROM dbo.FacilityTasks INNER JOIN  dbo.Tasks ON dbo.FacilityTasks.TaskId = dbo.Tasks.Id  WHERE (Tasks.Rebillable=0) AND  (FacilityTasks.FacilityId IN  (  (SELECT Distinct f.Id  FROM dbo.UserRights r RIGHT OUTER JOIN dbo.Facility f ON r.FacilityId = f.Id  WHERE f.Active=1 AND r.UserProfileId = " + Replace(rsTask__PUserID, "'", "''") + "  OR '" + Replace(rsTask__PUserType, "'", "''") + "' = 'Admin' OR '" + Replace(rsTask__PUserType, "'", "''") + "'='Super') ))  ORDER BY TaskDescription ASC");
            rsTask.Open();
            rsTask_numRows = 0;



            string rsSubTaskSource =  " SELECT Distinct dbo.RebillSubTasks.Id, dbo.RebillSubTasks.Description, dboFacility.Name, dbo.Tasks.TaskCode  ";
            rsSubTaskSource += " FROM dbo.RebillSubTasks ";
            rsSubTaskSource += " INNER JOIN  dbo.FacilityCustomer ON dbo.FacilityCustomer.Id = dbo.RebillSubTasks.FacilityCustomerId ";
            rsSubTaskSource += " Inner Join  dbo.Facility on dbo.Facility.Id = dbo.FacilityCustomer.FacilityId  ";
            rsSubTaskSource += " Inner Join  dbo.Tasks  on  dbo.Tasks.id  = dbo.RebillSubTasks.TaskID ";
            rsSubTaskSource += " WHERE  (FacilityCustomer.FacilityId IN  (  ";
            rsSubTaskSource += "  (SELECT Distinct f.Id  FROM dbo.UserRights r RIGHT OUTER JOIN dbo.Facility f ON r.FacilityId = f.Id  ";
            rsSubTaskSource += "       WHERE f.Active=1 AND r.UserProfileId =  " + Replace(rsTask__PUserID, "'", "''") + "  OR '" + Replace(rsTask__PUserType, "'", "''") + "' = 'Admin' OR '" + Replace(rsTask__PUserType, "'", "''") + "'='Super') )) ";
            rsSubTaskSource += " ORDER BY Description ASC ";

            rsSubTask = new DataReader(rsSubTaskSource);
            rsSubTask.Open();
            rsSubTask_numRows = 0;


            string rsCustomers__PUserType;
            rsCustomers__PUserType = "admin";
            if(cStr(Session["UserType"]) != ""){rsCustomers__PUserType = cStr(Session["UserType"]);}


            string rsCustomers__PUserID;
            rsCustomers__PUserID = "1";
            if(cStr(Session["UserID"]) != ""){rsCustomers__PUserID = cStr(Session["UserID"]);}



            rsCustomers = new DataReader( "SELECT Distinct FacilityCustomer.Id, CustomerCode, CustomerName, ContactName=IsNull(ContactName, 'N/A'), AlphaCode  FROM dbo.FacilityCustomer  INNER JOIN Facility ON FacilityCustomer.FacilityId = Facility.Id WHERE FacilityId IN  (  (SELECT Distinct f.Id  FROM dbo.UserRights r RIGHT OUTER JOIN dbo.Facility f ON r.FacilityId = f.Id  WHERE f.Active=1 AND (r.UserProfileId = " + Replace(rsCustomers__PUserID, "'", "''") + "  OR '" + Replace(rsCustomers__PUserType, "'", "''") + "' = 'Admin' OR '" + Replace(rsCustomers__PUserType, "'", "''") + "'='Super') )) AND FacilityCustomer.Active=1  ORDER BY AlphaCode, CustomerName ASC");
            rsCustomers.Open();
            rsCustomers_numRows = 0;


            string rsFac__PUserID;
            rsFac__PUserID = "17";
            if(cStr(Session["UserID"]) != ""){rsFac__PUserID = cStr(Session["UserID"]);}


            string rsFac__PUserType;
            rsFac__PUserType = "super";
            if (cStr(Session["UserType"]) != "") { rsFac__PUserType = cStr(Session["UserType"]); }



            rsFac = new DataReader("SELECT Distinct f.Id, f.Name, f.AlphaCode  FROM dbo.UserRights r RIGHT OUTER JOIN  dbo.Facility f ON r.FacilityId = f.Id  WHERE f.Active=1 AND (r.UserProfileId = " + Replace(rsFac__PUserID, "'", "''") + "  OR '" + Replace(rsFac__PUserType, "'", "''") + "' = 'Admin' OR '" + Replace(rsFac__PUserType, "'", "''") + "'='Super')  Order By Name");
            rsFac.Open();
            rsFac_numRows = 0;




        }
    }
}