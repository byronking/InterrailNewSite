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

namespace InterrailPPRS.Rebilling
{
    public partial class RebillEntryDetail : PageBase
    {

        public DataTableReader rsAttachments;
        public string sselFacilities, sfromDateDetail, stoDateDetail, sselTasks, sselCustomers, sRptType;
        public string rebilledvalue;
        public string rebillid;

        public DataReader rs;
        public int rs_numRows = 0;

        public int Repeat1__index;
        public int Repeat1__numRows;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

            GrantAccess("Super, Admin");

            string strSQL = "";
            string rs__PFacilityID;

            rs__PFacilityID = "0";
            if(Session["FacilityID"]  != ""){rs__PFacilityID = cStr(Session["FacilityID"]);}

            if (Request["Mode"] == "Cancel"){
               Response.Redirect("RebillEntry.aspx");
            }

            if (Request["Mode"] == "Save"){

   
               for( int i = 1; i <  cInt(Request["MaxRDOCount"]); i++){
                  if (Request["rbRebill_" + cStr(i)] == "on"){
                     rebilledvalue = "1";
                  }else{
                     rebilledvalue = "0";
                 }
                  rebillid = Request["rbID_" + cStr(i)] + "";
                  strSQL += " Update RebillDetail Set Rebilled = " + rebilledvalue + " where ID = " + rebillid;

               }

               this.Execute(strSQL);

               Response.Redirect("RebillEntry.aspx");
            }

            sselFacilities    = Request["selFacilities"];
            sfromDateDetail   = Request["fromDateDetail"];
            stoDateDetail     = Request["toDateDetail"];
            sselTasks         = Request["selTasks"];
            string sselSubTasks      = Request["selSubTasks"];
            sselCustomers     = Request["selCustomers"];
            string sRebillType       = Request["RebillType"];

            Session["LastStartDate"] = sfromDateDetail;
            Session["LastEndDate"]   = stoDateDetail;

            string wRebilled = "";
            string wFacilities = "";
            string wDateRange = "";
            string wTasks = "";
            string wSubTasks = "";
            string wCustomers = "";

            if(sRebillType == "Rebilled"){
              wRebilled = " AND (RebillDetail.Rebilled != 0) ";
            }
            if(sRebillType == "NotRebilled"){
              wRebilled = " AND (RebillDetail.Rebilled = 0) ";
            }
            if(sRebillType == "RebilledBoth"){
              wRebilled = "  ";
            }

            if(sselFacilities != ""){
              wFacilities = "  AND (RebillDetail.FacilityID IN  (" + sselFacilities + ") ) ";
            }else{
              wFacilities = "  AND (RebillDetail.FacilityID IN  (" + Session["FacilityID"] + ") ) ";
            }

            wDateRange = " AND (WorkDate Between '" + sfromDateDetail + "' AND '" + stoDateDetail + "') ";

            if(sselTasks != ""){
              wTasks = "  AND ( FacilityTasks.TaskId IN  (" + sselTasks + ") ) ";
            }else{
              wTasks = "   ";
            }

            if(sselSubTasks != ""){
              wSubTasks = "  AND ( RebillDetail.RebillSubTasksID IN (" + sselSubTasks + ") ) ";
            }else{
              wSubTasks = "   ";
            }

            if(sselCustomers != ""){
              wCustomers = "  AND (FacilityCustomerID IN  (" + sselCustomers + ") ) ";
            }else{
              wCustomers = "   ";
            }


            strSQL = "";
            strSQL +=  "SELECT  RebillDetail.Id, RebillDetail.WorkDate, RebillDetail.Rebilled, ";
            strSQL +=  "        RebillDetail.TotalHours, RebillDetail.RebillStatus, RebillSubTasks.Description,     ";
            strSQL +=  "        Tasks.TaskCode, FacilityCustomer.CustomerCode, FacilityCustomer.CustomerName  ";
            strSQL +=  "        FROM RebillDetail INNER JOIN   ";
            strSQL +=  "        RebillSubTasks ON RebillDetail.RebillSubTasksId = RebillSubTasks.Id INNER JOIN    ";
            strSQL +=  "        Tasks ON RebillSubTasks.TaskID = Tasks.Id INNER JOIN      ";
            strSQL +=  "        FacilityCustomer ON RebillSubTasks.FacilityCustomerId = FacilityCustomer.Id  ";
            strSQL +=  "         ";
            strSQL +=  "        WHERE 1=1 ";
            strSQL = strSQL + wRebilled + wFacilities + wDateRange + wTasks + wSubTasks + wCustomers;
            strSQL +=  "        ORDER BY WorkDate DESC, RebillDetail.Id DESC";


            rs = new DataReader(strSQL);
            rs.Open();
            rs_numRows = 0;

        }
    }
}