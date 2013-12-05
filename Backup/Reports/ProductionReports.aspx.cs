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
    public partial class ProductionReports : PageBase
    {

        public DataReader rsShifts,rsTask,rsCustomers,rsFacCustomers,rsMan,rsOrigin,rsFac;

        int rsShifts_numRows = 0;
        int rsTask_numRows = 0;
        int rsCustomers_numRows = 0;
        int rsFacCustomers_numRows = 0;
        int rsMan_numRows = 0;
        int rsOrigin_numRows = 0;
        int rsFac_numRows = 0;


        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin, User, Production");

            string rsTask__PUserID;
            rsTask__PUserID = "1";
            if(Session["UserID"]  != null && cStr(Session["UserID"]) != ""){rsTask__PUserID = cStr(Session["UserID"]);}


            string rsTask__PUserType;
            rsTask__PUserType = "admin";
            if (Session["UserType"] != null && cStr(Session["UserType"]) != "") { rsTask__PUserType = cStr(Session["UserType"]); }


            rsTask = new DataReader( "SELECT Distinct dbo.Tasks.Id, dbo.Tasks.TaskCode, dbo.Tasks.TaskDescription  FROM dbo.FacilityTasks INNER JOIN  dbo.Tasks ON dbo.FacilityTasks.TaskId = dbo.Tasks.Id  WHERE (Tasks.Rebillable=0) AND  (FacilityTasks.FacilityId IN  (  (SELECT Distinct f.Id  FROM dbo.UserRights r RIGHT OUTER JOIN dbo.Facility f ON r.FacilityId = f.Id  WHERE f.Active=1 AND r.UserProfileId = " + Replace(rsTask__PUserID, "'", "''") + "  OR '" + Replace(rsTask__PUserType, "'", "''") + "' = 'Admin' OR '" + Replace(rsTask__PUserType, "'", "''") + "'='Super') ))  ORDER BY TaskDescription ASC");

            rsTask.Open();
            rsTask_numRows = 0;


            string rsCustomers__PUserType;
            rsCustomers__PUserType = "admin";
            if (Session["UserType"] != null && cStr(Session["UserType"]) != "") { rsCustomers__PUserType = cStr(Session["UserType"]); }


            string rsCustomers__PUserID;
            rsCustomers__PUserID = "1";
            if (Session["UserID"] != null && cStr(Session["UserID"]) != "") { rsCustomers__PUserID = cStr(Session["UserID"]); }


            rsCustomers = new DataReader("SELECT Id, CustomerCode, CustomerName  FROM dbo.FacilityCustomer  WHERE FacilityId IN  (  (SELECT Distinct f.Id  FROM dbo.UserRights r RIGHT OUTER JOIN dbo.Facility f ON r.FacilityId = f.Id  WHERE f.Active=1 AND (r.UserProfileId = " + Replace(rsCustomers__PUserID, "'", "''") + "  OR '" + Replace(rsCustomers__PUserType, "'", "''") + "' = 'Admin' OR '" + Replace(rsCustomers__PUserType, "'", "''") + "'='Super') )) AND FacilityCustomer.Active=1  ORDER BY CustomerName ASC");
            rsCustomers.Open();
            rsCustomers_numRows = 0;


            rsFacCustomers = new DataReader( "SELECT Id, CustomerCode, CustomerName  FROM dbo.FacilityCustomer  WHERE FacilityId = " + Session["facilityID"] + " AND Active=1  ORDER BY CustomerName ASC");
            rsFacCustomers.Open();
            rsFacCustomers_numRows = 0;


            rsMan = new DataReader("SELECT ID, ManufacturerCode, ManufacturerName  FROM dbo.IRGManufacturer  ORDER BY ManufacturerName  ASC");
            rsMan.Open();
            rsMan_numRows = 0;


            rsOrigin = new DataReader( "SELECT ID, OriginCode, OriginName  FROM dbo.IRGOrigin  ORDER BY OriginName  ASC");
            rsOrigin.Open();
            rsOrigin_numRows = 0;


            string rsFac__PUserID;
            rsFac__PUserID = "17";
            if (Session["UserID"] != null &&  cStr(Session["UserID"]) != "") { rsFac__PUserID = cStr(Session["UserID"]); }


            string rsFac__PUserType;
            rsFac__PUserType = "super";
            if (Session["UserType"] != null && cStr(Session["UserType"]) != "") { rsFac__PUserType = cStr(Session["UserType"]); }


            rsFac = new DataReader( "SELECT Distinct f.Id, f.Name, f.AlphaCode  FROM dbo.UserRights r RIGHT OUTER JOIN   dbo.Facility f ON r.FacilityId = f.Id  WHERE f.Active=1 AND (r.UserProfileId = " + Replace(rsFac__PUserID, "'", "''") + "  OR '" + Replace(rsFac__PUserType, "'", "''") + "' = 'Admin' OR '" + Replace(rsFac__PUserType, "'", "''") + "'='Super')  Order By Name");
            rsFac.Open();
            rsFac_numRows = 0;

        }

         public string GetShiftCheckBoxes(string name){

           string strShiftCheckBoxes = "";
           rsShifts = new DataReader(" select id,shift from shifts order by shift ");
           rsShifts.Open();

            while (!rsShifts.EOF){

                rsShifts.Read();

               strShiftCheckBoxes += "<input type='checkbox' name='" + name + "' value='" + cStr(rsShifts.Item("id")) + "' ";
               if (Len(cStr(Session["LastShiftList"])) == 0 || cInt(InStr(0, cStr(Session["LastShiftList"]), cStr(rsShifts.Item("id")),0))  > 0){
                  strShiftCheckBoxes = strShiftCheckBoxes +    "  CHECKED ";
               }
               strShiftCheckBoxes = strShiftCheckBoxes + ">" + cStr(rsShifts.Item("shift"))  + "&nbsp;" ;
            } 
   
            return strShiftCheckBoxes;

        }


    }
}