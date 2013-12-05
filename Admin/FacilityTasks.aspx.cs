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
    public partial class FacilityTasks : PageBase
    {

        public DataReader rs;
        public int rs_numRows = 0;
        public int Repeat1__numRows = 0;
        public int  Repeat1__index = 0;
        public string MM_editAction = "";
        public string rs__PFacID;


        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin, User");

            // *** Update Record: construct a sql update statement && execute it;

            if(cStr(Request["MM_update"]) != "" && cStr(Request["MM_recordId"]) != ""){

              //MM_editAction = cStr(Request["URL"]);
              MM_editAction = "FacilityTasks.aspx";
              string MM_editRedirectUrl = "Facility.aspx";

              // create the sql update statement;
              string MM_editQuery = "Delete From FacilityTasks Where FacilityID = " + cStr(Request["MM_RecordID"]) ;
  
              string[] checkboxArray = Split(Request["checkbox"],",");
              for( int i = LBound(checkboxArray); i < UBound(checkboxArray); i ++){
                  MM_editQuery = MM_editQuery + "Insert Into FacilityTasks (FacilityID, TaskID, LastModifiedBy, LastModifiedOn) " ;
                  MM_editQuery = MM_editQuery + " Values ( " +  cStr(Request["MM_RecordID"]) + ", " + checkboxArray[i] + ", '" + Session["UserName"] + "', '" + System.DateTime.Now.ToString() + "' ) " ;
              }
  
              //Response.Write(MM_EditQuery;
              //Response.End;
                bool MM_abortEdit = false;
  
              if(!MM_abortEdit){
                // execute the update;
                this.Execute(MM_editQuery);
                if(MM_editRedirectUrl != ""){
                  Response.Redirect(MM_editRedirectUrl);
                }
              }

            }



            rs__PFacID = "5";
            if(cStr(Request["ID"]) != ""){rs__PFacID = cStr(Request["ID"]);}


            rs = new DataReader("SELECT dbo.Tasks.Id, dbo.Tasks.TaskDescription, dbo.Tasks.TaskCode,  dbo.FacilityTasks.TaskId, FacilityID, FacilityTasks.LastModifiedOn, FacilityTasks.LastModifiedBy  FROM dbo.Tasks , dbo.FacilityTasks  WHERE dbo.Tasks.Id *= dbo.FacilityTasks.TaskId  AND FacilityID = " + Replace(rs__PFacID, "'", "''") + "  AND Tasks.Active=1  ORDER BY TaskDescription");
            rs.Open();
            rs_numRows = rs.RecordCount;

            Repeat1__numRows = rs.RecordCount;
            Repeat1__index = 0;



        }
    }
}