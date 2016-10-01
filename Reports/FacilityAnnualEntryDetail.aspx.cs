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
    public partial class FacilityAnnualEntryDetail : PageBase
    {

            public string sField, sValue, i;
            public string[] TaskCode = new string[200];
            public string[] TaskID = new string[200];
            public string[] BudgetedCPU = new string[200];

            public string item, itemName, facilityname;

            public string sMode;
            public string selYear;
            public string sFac;

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            GrantAccess("Super, Admin, User");

            sMode 			= Request["MODE"];
            selYear  		= Request["selYear"];
            Session["Year"] = selYear;
            sFac     		= Request["FID"];
            facilityname = Request["FNAME"];

            if(sMode == "SAVE")
            {
                if (Session["Year"] != null)
                {
                    selYear = Session["Year"].ToString();
                }

                DataReader rsSave;
                string strSQL, strLoad, strUnload, strBudgetedCPU, strMiscellaneousCPU;

                strSQL =   " DELETE FROM FacilityAnnualBudgetTask ";
                strSQL +=  " WHERE ReportingYear = '" + Request["selYear"] + "' ";
                strSQL +=  "   And FacilityID = " + sFac + "; ";
  
                strSQL +=  " DELETE FROM FacilityAnnualBudget ";
                strSQL +=  " WHERE ReportingYear = '" + Request["selYear"] + "' ";
                strSQL +=  "   And FacilityID = " + sFac + "; ";

                strLoad = Request["LoadTotal"] + "";
                string strUnLoad = Request["UnloadTotal"] + "";
                string strSpotting = Request["SpottingTotal"] + "";

                if (strLoad == ""){strLoad = "NULL";}
                if (strUnLoad == ""){strUnLoad = "NULL";}
                if (strSpotting == ""){strSpotting = "NULL";}

                strSQL +=  "";
                strSQL +=  " INSERT INTO FacilityAnnualBudget ";
                strSQL +=  "   (FacilityId, LoadTotal, UnloadTotal, ReportingYear, SpottingTotal ) ";
                strSQL +=  " VALUES (";
                strSQL +=  " " + sFac + ", " + strLoad + ", " + strUnLoad + ", '" + selYear + "', " + strSpotting + " ";
                strSQL +=  ");  ";

                int intNumOfTasks = cInt(Request["NumOfTasks"]);
  
                for( int x = 1; x < intNumOfTasks; x++)
                {
  
                    string strCPU = Request["BudgetedCPU_" + cStr(x)] + "";
                    if (! isNumeric(strCPU)){
                    strCPU = "NULL";
                    }
                    strSQL +=  "";
                    strSQL +=  "INSERT INTO FacilityAnnualBudgetTask ";
                    strSQL +=  "   (FacilityId, ReportingYear, TaskID, BudgetedCPU ) ";
                    strSQL +=  "VALUES (";
                    strSQL +=  " " + sFac + ", '" + selYear + "', " + Request["TaskID_"] + cStr(x) + ", "  + strCPU +  " ";
                    strSQL +=  ")  ";
                }

                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
                using (SqlCommand cmd = new SqlCommand(strSQL, conn))
                {
                    try
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                    }
                }

                //rsSave = new DataReader(strSQL);
                //rsSave.Open();

                Response.Redirect("FacilityAnnualEntry.aspx?selYear=" + selYear);
            }

            //Response.Redirect("FacilityAnnualEntry.aspx?selYear=" + selYear);
        }
        
        public DataReader getData(string sDataSection) {

          DataReader rsData;
          string strSQL = "";

          if(sDataSection == "FACILITYBUDGETTASK"){
	
	        strSQL = " 	SELECT     FacilityTasks.TaskId, Tasks.TaskCode, Tasks.TaskDescription, ";
            strSQL +=  "      (select  FacilityAnnualBudgetTask.BudgetedCPU from  FacilityAnnualBudgetTask where  FacilityAnnualBudgetTask.TaskID = Tasks.ID AND  FacilityAnnualBudgetTask.FacilityID = " + sFac + " AND ReportingYear =  " + selYear + "  ) as BudgetedCPU  ";
            strSQL +=  " FROM         FacilityTasks INNER JOIN ";
            strSQL +=  "                       Tasks ON FacilityTasks.TaskId = Tasks.Id ";
            strSQL +=  " WHERE     (FacilityTasks.FacilityID = " + sFac + " ) ";

          }else{
              if ( sDataSection == "FACILITYLOAD"){
  	                strSQL = " SELECT     LoadTotal, UnloadTotal, SpottingTotal ";
	                strSQL +=  "FROM         FacilityAnnualBudget ";
	                strSQL +=  " WHERE     (FacilityID = " + sFac + " ) ";
	                strSQL +=  "    AND (ReportingYear = " + selYear + " ) ";
              }

         }

          rsData = new DataReader(strSQL);
          rsData.Open();

          return rsData;

        }

        public void  ShowFacilityLoad(){

              DataReader rs;
              int i;

              rs = getData("FACILITYLOAD");

              Response.Write("<tr><td width='100%' colspan='2'>&nbsp;</td></tr>");
              Response.Write("<tr><td colspan='2' class='pageTitle' align='Left'><div class='cellBottomBorder'>Annual Budget</div>&nbsp;&nbsp;&nbsp;<font size=1></font></td></tr>");
              //rs.MoveFirst;
              string strLoadTotal = "";
              string strUnLoadTotal = "";
              string strSpottingTotal = "";

              if (!rs.EOF){
                 rs.Read();
                 strLoadTotal = rs.Item("LoadTotal");
                 strUnLoadTotal = rs.Item("UnLoadTotal");
                 strSpottingTotal = rs.Item("SpottingTotal");
              }


              Response.Write("<tr><td><b><u></u></b></td><td><b><u></u></b></td></tr>");
              Response.Write("<tr><td colspan='2' height='10'></td></tr>");

              Response.Write("<tr><td align='right' width='23%'>Load Quantity:&nbsp;</td><td width='20%'><input type='text' name='LoadTotal' value='" + strLoadTotal + "' maxlength=6 size=6 onblur='CheckValue(this)'></td>");
              Response.Write("<tr><td align='right' width='23%'>Unload Quantity:&nbsp;</td><td width='20%'><input type='text' name='UnLoadTotal' value='" + strUnLoadTotal + "' maxlength=6 size=6 onblur='CheckValue(this)'></td>");
              Response.Write("<tr><td align='right' width='23%'>Spotting Quantity:&nbsp;</td><td width='20%'><input type='text' name='SpottingTotal' value='" + strSpottingTotal + "' maxlength=6 size=6 onblur='CheckValue(this)'></td>");

              Response.Write("<tr><td  height='7'></td></tr>");
              Response.Write("<tr><td  height='7'></td></tr>");


            } //End Function

        public void ShowFacilityBudgetTask(){

                  DataReader rs;
                  int i = 0;

                  rs = getData("FACILITYBUDGETTASK");

                  Response.Write("<tr><td width='100%' colspan='2'>&nbsp;</td></tr>");

                  if (!rs.EOF){

                    Response.Write("<tr><td align='right'><b><u>Task</u></b>&nbsp;</td><td><b><u>Budgeted CPU</u></b></td></tr>");
                    Response.Write("<tr><td colspan='2' height='10'></td></tr>");

                    i = 1;
                    while (!rs.EOF){
                      rs.Read();
	                  Response.Write("<tr><td align='right' width='23%'>" + rs.Item("TaskDescription") + " (" + rs.Item("TaskCode") + "):&nbsp;</td><td width='20%'><input type='hidden' name='TaskID_" + cStr(i) + "' value='" + rs.Item("TaskID") + "' /><input type='text' name='BudgetedCPU_" + cStr(i) + "' value='" + rs.Item("BudgetedCPU") + "' maxlength=6 size=6 onblur='CheckValue(this)' /></td>");
                      i = i + 1;

                    } //End Loop

                  }else{
                    Response.Write("<tr><td colspan='2' height='10'>No Tasks for this Facility</td></tr>");
                 }
  

                  Response.Write("<tr><td  height='7'><input type='hidden' name='NumOfTasks' value='" + cStr(i-1) + "' /></td></tr>");
                  Response.Write("<tr><td  height='7'></td></tr>");

            }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataReader rsSave;
            string strSQL, strLoad, strUnload, strBudgetedCPU, strMiscellaneousCPU;

            strSQL = " DELETE FROM FacilityAnnualBudgetTask ";
            strSQL += " WHERE ReportingYear = '" + Request["selYear"] + "' ";
            strSQL += "   And FacilityID = " + sFac + "; ";

            strSQL += " DELETE FROM FacilityAnnualBudget ";
            strSQL += " WHERE ReportingYear = '" + Request["selYear"] + "' ";
            strSQL += "   And FacilityID = " + sFac + "; ";


            strLoad = Request["LoadTotal"] + "";
            string strUnLoad = Request["UnloadTotal"] + "";
            string strSpotting = Request["SpottingTotal"] + "";

            if (strLoad == "") { strLoad = "NULL"; }
            if (strUnLoad == "") { strUnLoad = "NULL"; }
            if (strSpotting == "") { strSpotting = "NULL"; }

            strSQL += "";
            strSQL += " INSERT INTO FacilityAnnualBudget ";
            strSQL += "   (FacilityId, LoadTotal, UnloadTotal, ReportingYear, SpottingTotal ) ";
            strSQL += " VALUES (";
            strSQL += " " + sFac + ", " + strLoad + ", " + strUnLoad + ", '" + selYear + "', " + strSpotting + " ";
            strSQL += ");  ";

            int intNumOfTasks = cInt(Request["NumOfTasks"]);

            for (int x = 1; x < intNumOfTasks; x++)
            {
                string strCPU = Request["BudgetedCPU_" + cStr(x)] + "";
                if (!isNumeric(strCPU))
                {
                    strCPU = "NULL";
                }
                strSQL += "";
                strSQL += "INSERT INTO FacilityAnnualBudgetTask ";
                strSQL += "   (FacilityId, ReportingYear, TaskID, BudgetedCPU ) ";
                strSQL += "VALUES (";
                strSQL += " " + sFac + ", '" + selYear + "', " + Request["TaskID_"] + cStr(x) + ", " + strCPU + " ";
                strSQL += ")  ";
            }

            //using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            //using (SqlCommand cmd = new SqlCommand(strSQL, conn))
            //{
            //    try
            //    {
            //        cmd.CommandType = CommandType.Text;
            //        cmd.Connection.Open();
            //        cmd.ExecuteNonQuery();
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.Write(ex.ToString());
            //    }
            //}

            try
            {
                rsSave = new DataReader(strSQL);
                rsSave.Open();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            Response.Redirect("FacilityAnnualEntry.aspx?selYear=" + selYear);
        }
    }
}