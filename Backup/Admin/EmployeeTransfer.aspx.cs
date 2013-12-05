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
    public partial class EmployeeTransfer : PageBase
    {
        public DataReader rs;
        public DataReader rsEmployees;
        public DataReader rsFac;
        public int rsFac_numRows = 0;
        public int rsEmployees_numRows = 0;
        public string sMessage;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

            GrantAccess("Super, Admin");



            if (Request["MODE"] == "TRANSFER"){

               // First Change Facility to which employee belongs to;

               string sEmployee = Request["selEmployees"];
               string sFacility = Request["selFacilities"];
               string strSQL = " Update Employee Set FacilityID = " + sFacility + "  Where ID = " + sEmployee;
               this.Execute(strSQL);

               // Now remove employee from all the teams he/she belongs;


               strSQL = "Update Teams Set TeamMembers = LTrim(Replace(Replace(' '+LTrim(TeamMembers)+',~~', ' " + sEmployee + ",', ''),',~~','')) ";
               strSQL = strSQL + " Where ' '+LTrim(RTrim(TeamMembers))+',' Like '% " + sEmployee + ",%'";
               this.Execute(strSQL);

               sMessage = Request["EMPNAME"] + " moved to " + Request["FACNAME"];

            }else{
               sMessage = "Select employee to move to which facility....";
            }


            string rsEmployees__PFACID;
            rsEmployees__PFACID = "1";
            if(Session["FacilityID"] != ""){rsEmployees__PFACID = System.Convert.ToString(Session["FacilityID"]);}



            rsEmployees = new DataReader("SELECT Id, LastName, FirstName, FacilityID  FROM dbo.Employee  WHERE FacilityID = " + Replace(rsEmployees__PFACID, "'", "''") + " AND Active=1  ORDER BY LastName, FirstName");
            rsEmployees.Open();
            rsEmployees_numRows = 0;


            string rsFac__PFACID;
            rsFac__PFACID = "1";
            if (Session["FacilityID"] != "") { rsFac__PFACID = System.Convert.ToString(Session["FacilityID"]); }

            rsFac = new DataReader("SELECT Id, Name  FROM dbo.Facility  WHERE Active=1 AND ID != " + Replace(rsFac__PFACID, "'", "''") + "  ORDER BY Name");
            rsFac.Open();
            rsFac_numRows = 0;



        }
    }
}