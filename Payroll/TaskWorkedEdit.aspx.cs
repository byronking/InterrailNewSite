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

namespace InterrailPPRS.Payroll
{
    public partial class TaskWorkedEdit : PageBase
    {

        public string MM_editAction;
        public bool MM_abortEdit;
        public string MM_editQuery;
        public string MM_editRedirectUrl;
        public DataReader rs;
        public DataReader rsEmp;
        public DataReader rsTaskList;
        public DataReader rsShifts;

        public string rbDesc = "";
        public int rbTaskID = 0;
        public int rbSubTaskID = 0;
        public bool Rebill = false;
        public string sql;
        public string rbReturnTo = "";
        public DataReader localRS;
        public string strStatus = "";

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            GrantAccess("Super, Admin, User");

            MM_editAction = cStr(Request["URL"]);
            if((Request.QueryString.Count > 0) ){
              MM_editAction = MM_editAction + "?" + Request.QueryString;
            }

            // boolean to abort record edit;
            MM_abortEdit = false;

            // query string to execute;
            MM_editQuery = "";

            if(System.Convert.ToString(Request["Type"]) == "Rebill" ){

               Rebill = true;
               sql = "";
               sql = sql + " SELECT  dbo.RebillSubTasks.Description, dbo.RebillSubTasks.TaskID, dbo.RebillDetail.RebillSubTasksId ";
               sql = sql + " FROM    dbo.RebillDetail INNER JOIN ";
               sql = sql + "   dbo.RebillSubTasks ON dbo.RebillDetail.RebillSubTasksId = dbo.RebillSubTasks.Id ";
               sql = sql + " WHERE     dbo.RebillDetail.Id = " + Request["RebillDetailID"];
   
                localRS = new DataReader(sql);
                localRS.Open();

               rbDesc = "";
               rbTaskID = 0;
               rbSubTaskID = 0;
               if(localRS.Read()){
                  rbDesc = System.Convert.ToString(localRS.Item("Description"));
                  rbTaskID = System.Convert.ToInt32(localRS.Item("TaskID"));
                  rbSubTaskID = System.Convert.ToInt32(localRS.Item("RebillSubTasksId"));
               }
   
               rbReturnTo = "payroll.aspx?workdate=" + Request["WorkDate"] + "&shift=" + Request["Shift"];   
            }

            if(Request["Delete"] != null && System.Convert.ToString(Request["Delete"]).ToUpper() == "YES"){

                Execute("Delete EmployeeTaskWorked Where ID = " + System.Convert.ToString(Request["ID"]));
     
                if( System.Convert.ToString(Request["ReturnTo"]) != ""){
                  MM_editRedirectUrl = Request["ReturnTo"];
                }else{
                  MM_editRedirectUrl = "TaskWorkedEdit.aspx";
                }

                var dateWorked = Request["dateworked"].ToString();
                var taskId = Convert.ToInt32(Request["TaskID"]);
                var selShift = Request["selShift"].ToString();
                var facilityId = Convert.ToInt32(Session["FacilityID"]);
                var testy = Request["RebillDetailID"];

                // Set the RebillDetailID to 0, unless there is a value for RebillDetailID in the request object.
                var rebillDetailId = 0;

                if (Request["RebillDetailID"] != string.Empty)
                {
                    rebillDetailId = Convert.ToInt32(Request["RebillDetailID"]);
                }

                UpdateUPM(dateWorked, taskId, selShift, facilityId, rebillDetailId);
        
                if(cStr(Request["RebillDetailID"]) != "" ){
                    UpdateRebillHours(System.Convert.ToInt32(Request["RebillDetailID"]));
                }

                Response.Redirect(MM_editRedirectUrl);
            }

        // *** Edit Operations: declare variables;

        MM_editAction = MM_editRedirectUrl;

        // boolean to abort record edit;
        MM_abortEdit = false;

        // query string to execute;
        MM_editQuery = "";

        // *** Update Record:  variables;

          string MM_editColumn = "Id";
          string MM_editTable = "dbo.EmployeeTaskWorked";
          MM_editColumn = "Id";
          string MM_recordId = "0";
          MM_editRedirectUrl = "TaskWorked.aspx";
          string MM_fieldsStr  = "dateworked|value|selShift|value|selEmp|value|TaskID|value|OtherTaskID|value|RebillDetailID|value|txtHours|value|txtUnits|value|selOutofTown|value|txtnotes|value|LastModifiedOn|value|LastModifiedBy|value|hFacilityID|value|SubTaskID|value";
          string MM_columnsStr = "WorkDate|',none,''|ShiftID|none,none,NULL|EmployeeId|none,none,NULL|TaskID|none,none,NULL|OtherTaskID|none,none,NULL|RebillDetailID|none,none,NULL|HoursWorked|none,none,NULL|UPM|none,none,NULL|OutOfTownType|',none,''|Notes|',none,''|LastModifiedOn|',none,''|LastModifiedBy|',none,''|FacilityID|none,none,NULL|RebillSubTaskID|none,none,NULL";

          if (Request.Form["MM_recordId"] != null) { MM_recordId = Request.Form["MM_recordId"].ToString(); }

          // create the MM_fields && MM_columns arrays;
          string[] MM_fields = Split(MM_fieldsStr, "|");
          string[] MM_columns = Split(MM_columnsStr, "|");
  
          //  the form values;
          for( int i = LBound(MM_fields); i < UBound(MM_fields); i += 2){
            MM_fields[i+1] = cStr(Request.Form[MM_fields[i]]);
          } 


        // *** Update Record: construct a sql update statement and execute it;

          if (Request["MM_update"] != null && Request["MM_recordId"] != null && (System.Convert.ToString(Request["MM_update"]) != "") && (System.Convert.ToString(Request["MM_recordId"]) != ""))
          {

          // create the sql update statement;
          MM_editQuery = "update " + MM_editTable + " SET ";

          for(int i = LBound(MM_fields); i < UBound(MM_fields); i += 2){

            string FormVal = MM_fields[i+1];
            string[] MM_typeArray = Split(MM_columns[i+1],",");
            string Delim = MM_typeArray[0];

            if(Delim == "none"){ Delim = "";}
            string AltVal = MM_typeArray[1];
            if(AltVal == "none"){ AltVal = "";}
            string EmptyVal = MM_typeArray[2];
            if(EmptyVal == "none"){ EmptyVal = "";}

            if(FormVal == ""){
              FormVal = EmptyVal;
            }else{
              if(AltVal != ""){
                FormVal = AltVal;
              }else{
                  if(Delim == "'"){  // escape quotes;
                      FormVal = "'" + Replace(FormVal,"'","''") + "'";
                  }else{
                      FormVal = Delim + FormVal + Delim;
                  }
              }
            }
 
            if(i != LBound(MM_fields)){
              MM_editQuery = MM_editQuery + ",";
            }

            MM_editQuery = MM_editQuery + MM_columns[i] + " = " + FormVal;
          } //End For

          MM_editQuery = MM_editQuery + " where " + MM_editColumn + " = " + MM_recordId;

          if(!MM_abortEdit){

              // execute the update;
     
              Execute(MM_editQuery);


                if(System.Convert.ToString(Request["ReturnTo"]) != ""){
                  MM_editRedirectUrl = System.Convert.ToString(Request["ReturnTo"]);
                }else{
                  MM_editRedirectUrl = "TaskWorked.aspx";
                }
    
                UpdateUPM(System.Convert.ToString(Request["dateworked"]), System.Convert.ToInt32(Request["TaskID"]),System.Convert.ToString(Request["selShift"]),System.Convert.ToInt32(Session["FacilityID"]) ,System.Convert.ToInt32(Request["RebillDetailID"]));
        
                if(cStr(Request["RebillDetailID"]) != "" ){
                   UpdateRebillHours(System.Convert.ToInt32(Request["RebillDetailID"]));
                }
        
              Response.Redirect(MM_editRedirectUrl);
          }

        }else{

            if (Request["MM_update"] != null  && (System.Convert.ToString(Request["MM_update"]) != "") && (System.Convert.ToString(Request["MM_recordId"]) == "" || Request["MM_recordId"] == null))
            {
      
               // create the sql insert statement;
              string MM_tableValues = "";
              string MM_dbValues = "";

              for( int i = LBound(MM_fields); i< UBound(MM_fields); i += 2){

                string FormVal = MM_fields[i+1];
                string[] MM_typeArray = Split(MM_columns[i+1],",");
                string Delim = MM_typeArray[0];
                if(Delim == "none"){ Delim = "";}
                string AltVal = MM_typeArray[1];
                if(AltVal == "none"){ AltVal = "";}
                string EmptyVal = MM_typeArray[2];
                if(EmptyVal == "none"){ EmptyVal = "";}
                if(FormVal == ""){
                  FormVal = EmptyVal;
                }else{
                  if(AltVal != ""){
                    FormVal = AltVal;
                  }else{
                      if(Delim == "'"){  // escape quotes;
                         FormVal = "'" + Replace(FormVal,"'","''") + "'";
                      }else{
                         FormVal = Delim + FormVal + Delim;
                      }
                  }
                }
                if(i != LBound(MM_fields) ){
                  MM_tableValues = MM_tableValues + ",";
                  MM_dbValues = MM_dbValues + ",";
                }
                MM_tableValues = MM_tableValues + MM_columns[i];
                MM_dbValues = MM_dbValues + FormVal;

              } //End for

               MM_editQuery = "insert into " + MM_editTable + " (" + MM_tableValues + ") values (" + MM_dbValues  + ")";


              if(!MM_abortEdit){

                 // execute the insert;

                  Execute( MM_editQuery);

                  UpdateUPM(System.Convert.ToString(Request["dateworked"]), System.Convert.ToInt32(Request["TaskID"]), System.Convert.ToString(Request["selShift"]), System.Convert.ToInt32(Session["FacilityID"]), System.Convert.ToInt32(Request["RebillDetailID"]) );

                    if(cStr(Request["RebillDetailID"]) != "" ){
                       UpdateRebillHours(System.Convert.ToInt32(Request["RebillDetailID"]));
                    }

                    if(Rebill ){
                      Response.Redirect(rbReturnTo);
                    }else{
                      Response.Redirect(Request["ReturnTo"]);
                    }
              }

            }


            string rs__MMColParam;
            rs__MMColParam = "30";
            if(Request.QueryString["Id"]   != "") { rs__MMColParam = System.Convert.ToString(Request.QueryString["Id"] );}


            if(cStr(Request["ID"]) == "0") {

              string strSQL = "";
              strSQL = "SELECT * FROM dbo.EmployeeTaskWorked WHERE Id =-1 ";

              /* BRETT
              rs.CursorLocation = 3;
              rs.Open strSQL, MM_Main_STRING, 1, 3, 1;
              rs.AddNew;
              **/

            }else{

              rs = new DataReader("SELECT Id, TaskID, OtherTaskID, FacilityID, EmployeeId, RebillDetailID, WorkDate, ShiftID, UPM, HoursWorked, PayrollStatus,  OutOfTownType, LastModifiedBy, LastModifiedOn, Notes  FROM dbo.EmployeeTaskWorked  WHERE Id = " + Replace(rs__MMColParam, "'", "''") + "");
              rs.Open();
              rs.Read();
            }


            string rsEmp__PFACID;
            rsEmp__PFACID = "5";
            if(cStr(Session["FacilityID"]) != ""){ rsEmp__PFACID = System.Convert.ToString(Session["FacilityID"]) ;}

            rsEmp = new DataReader("SELECT Id, IsNull(EmployeeNumber,'') as EmployeeNumber, TempNumber, LastName=Case When FacilityID=" + Replace(rsEmp__PFACID, "'", "''") + " Then ' '+LastName Else '*'+LastName End, FirstName, MiddleInitial, FacilityID  FROM dbo.Employee Where Active <> 0 ORDER BY LastName, FirstName, EmployeeNumber");
            rsEmp.Open();
            
            string rsTaskList__PFACID;
            rsTaskList__PFACID = "5";
            if(cStr(Session["FacilityID"]) != ""){ rsTaskList__PFACID = System.Convert.ToString(Session["FacilityID"]);}

            rsTaskList = new DataReader("SELECT TaskID=dbo.Tasks.Id, OtherTaskID=0,dbo.Tasks.TaskCode, dbo.Tasks.TaskDescription  FROM dbo.Tasks INNER JOIN dbo.FacilityTasks ON dbo.Tasks.Id = dbo.FacilityTasks.TaskId  WHERE (dbo.FacilityTasks.FacilityID = " + Replace(rsTaskList__PFACID, "'", "''") + ")  UNION  SELECT TaskID=0, OtherTaskID=Id, TaskCode, TaskDescription=' * '+TaskDescription  FROM dbo.OtherTasks  ORDER BY OtherTaskID, TaskDescription");
            rsTaskList.Open();

            rsShifts = new DataReader("Select ID, Shift  From Shifts");
            rsShifts.Open();


         }

       } //Page Load

    }
}