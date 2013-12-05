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
    public partial class TeamsEdit : PageBase
    {

        public DataReader rs;
        public int rs_numRows = 0;
        public string TeamMembers = "";

        public DataReader rsMembers;
        public int rsMembers_numRows = 0;

        public DataReader rsEmp;
        public int rsEmp_numRows= 0;
        public string MM_editAction = "";
        public string MM_editRedirectUrl = "";
        
        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin, User");


            // *** Edit Operations: declare variables;

            MM_editAction = cStr(Request["URL"]);
            if(Request.QueryString.Count > 0){
              //MM_editAction = MM_editAction + "?" + Request.QueryString;
            }

            // boolean to abort record edit;
            bool MM_abortEdit = false;

            // query string to execute;
            string MM_editQuery = "";


            // *** Update Record: variables;


              string MM_editTable = "dbo.Teams";
              string MM_editColumn = "ID";
              string MM_recordId = "" + Request.Form["MM_recordId"] + "";
              MM_editRedirectUrl = "Teams.aspx";
              string MM_fieldsStr  = "TeamName|value|Active|value|TheFacID|value|LastModifiedOn|value|LastModifiedBy|value|TeamMembers|value";
              string MM_columnsStr = "TeamName|',none,''|Active|none,1,0|FacilityID|none,none,NULL|LastModifiedOn|',none,NULL|LastModifiedBy|',none,''|TeamMembers|',none,''";

              // create the MM_fields && MM_columns arrays;
              string[] MM_fields = Split(MM_fieldsStr, "|");
              string[] MM_columns = Split(MM_columnsStr, "|");

              // the form values;
              for(int i = LBound(MM_fields); i < UBound(MM_fields); i = i + 2){ 
                MM_fields[i+1] = cStr(Request.Form[MM_fields[i]]);
              }

              // append the query string to the redirect URL;
              if(MM_editRedirectUrl != "" && Request.QueryString.Count > 0){
                if(InStr(0, MM_editRedirectUrl, "?", 1) == 0 && Request.QueryString.Count > 0){
                  MM_editRedirectUrl = MM_editRedirectUrl + "?" + Request.QueryString;
                }else{
                  MM_editRedirectUrl = MM_editRedirectUrl + "&" + Request.QueryString;
               }
             }



            // *** Update Record: construct a sql update statement && execute it;

            if(cStr(Request["MM_update"]) != "" && cStr(Request["MM_recordId"]) != ""){

              // create the sql update statement;
              MM_editQuery = "update " + MM_editTable + " set ";
              for(int i = LBound(MM_fields); i < UBound(MM_fields); i = i + 2){ 

                string FormVal = MM_fields[i+1];
                string[] MM_typeArray = Split(MM_columns[i+1],",");
                string Delim = MM_typeArray[0];
                if(Delim == "none"){Delim = "";}
                string AltVal = MM_typeArray[1];
                if(AltVal == "none"){AltVal = "";}
                string EmptyVal = MM_typeArray[2];
                if(EmptyVal == "none"){EmptyVal = "";}
                if(FormVal == ""){
                  FormVal = EmptyVal;
                }else{
                  if(AltVal != ""){
                    FormVal = AltVal;
                  }else{
                      if(Delim == "'"){ // escape quotes;
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
              }
              MM_editQuery = MM_editQuery + " where " + MM_editColumn + " = " + MM_recordId;

              if(!MM_abortEdit){
                // execute the update;
                this.Execute(MM_editQuery);

                  if(MM_editRedirectUrl != ""){
                  Response.Redirect(MM_editRedirectUrl);
               }
             }

            }else{
                
                if(cStr(Request["MM_update"]) != "" && cStr(Request["MM_recordId"]) == ""){

              // create the sql insert statement;
              string MM_tableValues = "";
              string MM_dbValues = "";

              for(int i = LBound(MM_fields); i < UBound(MM_fields); i = i + 2){ 

                string FormVal = MM_fields[i+1];
                string[] MM_typeArray = Split(MM_columns[i+1],",");
                string Delim = MM_typeArray[0];
                if(Delim == "none"){Delim = "";}
                string AltVal = MM_typeArray[1];
                if(AltVal == "none"){AltVal = "";}
                string EmptyVal = MM_typeArray[2];
                if(EmptyVal == "none"){EmptyVal = "";}
                if(FormVal == ""){
                  FormVal = EmptyVal;
                }else{
                  if(AltVal != ""){
                    FormVal = AltVal;
                  }else{ 
                      if(Delim == "'"){ // escape quotes;
                        FormVal = "'" + Replace(FormVal,"'","''") + "'";
                      }else{
                        FormVal = Delim + FormVal + Delim;
                      }
                 }
               }
                if(i != LBound(MM_fields)){
                  MM_tableValues = MM_tableValues + ",";
                  MM_dbValues = MM_dbValues + ",";
                }
                MM_tableValues = MM_tableValues + MM_columns[i];
                MM_dbValues = MM_dbValues + FormVal;
              }

              MM_editQuery = "insert into " + MM_editTable + " (" + MM_tableValues + ") values (" + MM_dbValues + ")";

              if(!MM_abortEdit){
                // execute the update;
                this.Execute(MM_editQuery);

                if(MM_editRedirectUrl != ""){
                  Response.Redirect(MM_editRedirectUrl);
               }
             }

            }
            }

            string rs__MMColParam;
            rs__MMColParam = "1";
            if(Request.QueryString["ID"] != ""){rs__MMColParam = Request.QueryString["ID"];}


            string rs__MMFACID;
            rs__MMFACID = "3";
            if(Session["FacilityID"]  != ""){rs__MMFACID = System.Convert.ToString(Session["FacilityID"]);}


            if(Request["ID"] == "0"){
              //string strSQL;
              //rs = Server.CreateObject("ADODB.Recordset");
              //strSQL = "SELECT ID, TeamName, FacilityID, TeamMembers, LastModifiedBy, LastModifiedOn, Active  FROM dbo.Teams  WHERE ID = -1";
              //rs.CursorLocation = 3;
              //rs.Open strSQL, MM_Main_STRING, 1, 3, 1;
              //rs.AddNew;
              rs = new DataReader();

            }else{
              rs = new DataReader("SELECT ID, TeamName, FacilityID, TeamMembers, LastModifiedBy, LastModifiedOn, Active  FROM dbo.Teams  WHERE ID = " + Replace(rs__MMColParam, "'", "''") + " AND FacilityID=" + Replace(rs__MMFACID, "'", "''") + "");
              rs.Open();
              rs.Read();
              rs_numRows = 0;
            }

            if(rs.EOF){
              TeamMembers = "0";
            }else{
              TeamMembers = rs.Item("TeamMembers");
              if(TeamMembers == ""){
                TeamMembers = "0";
             }
            }


            string rsMembers__PTM;
            rsMembers__PTM = "0";
            if(TeamMembers != ""){rsMembers__PTM = TeamMembers;}



            rsMembers = new DataReader( "SELECT Id, EmployeeNumber = Case When TempEmployee=0 Then EmployeeNumber Else TempNumber End , LastName, FirstName  FROM dbo.Employee  WHERE Active=1 AND ID In (" + Replace(rsMembers__PTM, "'", "''") + ")");
            rsMembers.Open();
            rsMembers_numRows = 0;


            string rsEmp__MMFACID;
            rsEmp__MMFACID = "2";
            if(Session["FacilityID"]   != ""){rsEmp__MMFACID = System.Convert.ToString(Session["FacilityID"]);}


            string rsEmp__MMColParam;
            rsEmp__MMColParam = "0";
            if(TeamMembers != ""){rsEmp__MMColParam = TeamMembers;}


            rsEmp = new DataReader( "SELECT Id, LastName, FirstName, EmployeeNumber = Case When TempEmployee=0 Then EmployeeNumber Else TempNumber End  FROM Employee  WHERE Employee.ID NOT IN (" + Replace(rsEmp__MMColParam, "'", "''") + ") AND FacilityID = " + Replace(rsEmp__MMFACID, "'", "''") + " AND Employee.Active=1  ORDER BY LastName, FirstName, EmployeeNumber");
            rsEmp.Open();
            rsEmp_numRows = 0;



        }
    }
}