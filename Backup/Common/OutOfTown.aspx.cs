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
    public partial class OutOfTown : PageBase
    {

        public DataReader rs;
        public int rs_numRows = 0;
        public string sFrom = "";

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

          
            GrantAccess("Super, Admin, User");



            string PFacilityID;
            PFacilityID = cStr(Session["FacilityID"]);  
            string rsTeams__PFacID;
            rsTeams__PFacID = "0";
            if (cStr(Session["FacilityID"]) != "") { rsTeams__PFacID = cStr(Session["FacilityID"]) ;}  


            string rsEmp__PIRGID;
            rsEmp__PIRGID = "1";
            if (cStr(Session["CompanyID"])  !=  "") { rsEmp__PIRGID = cStr(Session["CompanyID"]) ;}


            string rsEmp__PFACID;
            rsEmp__PFACID = "2";
            if (cStr(Session["FacilityID"]) != "") { rsEmp__PFACID = cStr(Session["FacilityID"]);}


            rs = new DataReader("SELECT Employee.Id,  LastName, FirstName, EmployeeNumber = Case When TempEmployee=0 Then EmployeeNumber Else TempNumber End  FROM Employee INNER JOIN Facility ON Employee.FacilityID = Facility.Id  WHERE ( FacilityID <> " + Replace(rsEmp__PFACID, "'", "''") + "  AND Employee.Active=1)  ORDER BY LastName, FirstName, EmployeeNumber");
            rs.Open();
            rs_numRows = 0;


        }
    }
}