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
    public partial class FacilityEdit : PageBase
    {

        public DataReader rsFac;
        public DataReader rsCompany;
        public DataReader rsRegion;
        public DataReader rsTasks;
        public DataReader rsOTBasis;



        public string TheTaskID = "";
        public int rsFac_numRows = 0;
        public int rsCompany_numRows = 0;
        public int rsRegion_numRows = 0;
        public int rsTasks_numRows = 0;
        public int rsOTBasis_numRows = 0;
        public string MM_editAction = "";

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


 
            GrantAccess("Super, Admin");


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


              string MM_editTable = "dbo.Facility";
              string MM_editColumn = "Id";
              string MM_recordId = "" + Request.Form["MM_recordId"] + "";
              string MM_editRedirectUrl = "Facility.aspx";
              string MM_fieldsStr  = "FacilityNumber|value|AlphaCode|value|Name|value|Address1|value|Address2|value|Address3|value|select|value|DefaultShiftID|value|Region|value|OTCalcBasis|value|GLCostCenter|value|Company|value|checkbox|value|LastModifiedOn|value|LastModifiedBy|value";
              string MM_columnsStr = "FacilityNumber|',none,''|AlphaCode|',none,''|Name|',none,''|Address1|',none,''|Address2|',none,''|Address3|',none,''|DefaultTaskID|none,none,NULL|DefaultShiftID|',none,''|RegionID|none,none,NULL|OvertimeCalcBasis|none,none,NULL|GLCostCenter|',none,''|IRGCompanyId|none,none,NULL|Active|none,1,0|LastModifiedOn|',none,''|LastModifiedBy|',none,''";

              // create the MM_fields && MM_columns arrays;
              string[] MM_fields = Split(MM_fieldsStr, "|");
              string[] MM_columns = Split(MM_columnsStr, "|");

              // the form values;
              for(int i = LBound(MM_fields) ; i < UBound(MM_fields); i = i + 2){
                MM_fields[i+1] = cStr(Request.Form[MM_fields[i]]);
              }

              // append the query string to the redirect URL;
              if(MM_editRedirectUrl != "" && Request.QueryString.Count  > 0){
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
                      if(Delim == "'"){ 
                          // escape quotes;
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
                      if(Delim == "'"){ 
                          // escape quotes;
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
              } //End For

              MM_editQuery = "insert into " + MM_editTable + " (" + MM_tableValues + ") values (" + MM_dbValues + ") ";
              MM_editQuery = MM_editQuery  + "insert into FacilityAnnualBudget(facilityId, ReportingYear) values (@@Identity, '" + System.DateTime.Now.Year.ToString() + "') ";

              if(!MM_abortEdit){
                // execute the insert;
                this.Execute(MM_editQuery);
                if(MM_editRedirectUrl != ""){
                  Response.Redirect(MM_editRedirectUrl);
                }
              }

            }

            }


            string rsFac__MM_ColParam;
            rsFac__MM_ColParam = "1";
            if (Request.QueryString["Id"] != "") { rsFac__MM_ColParam = cStr(Request.QueryString["Id"]); }


            if(cStr(Request["ID"]) == "0"){
              //string strSQL;
              //rsFac = Server.CreateObject("ADODB.Recordset");
              //strSQL = "SELECT * FROM dbo.Facility WHERE Id =-1 ";
              //rsFac.CursorLocation = 3;
              //rsFac.Open strSQL, MM_Main_STRING, 1, 3, 1;
              //rsFac.AddNew;
              rsFac = new DataReader();
              TheTaskID = "0";

            }else{

              rsFac = new DataReader("SELECT Id, FacilityNumber, AlphaCode, Name, DefaultTaskID, DefaultShiftID, RegionID, OvertimeCalcBasis, GLCostCenter, IRGCompanyId, LastModifiedBy, LastModifiedOn, Active, Address1, Address2, Address3  FROM dbo.Facility  WHERE Id = " + Replace(rsFac__MM_ColParam, "'", "''") + "");
              rsFac.Open();
              rsFac_numRows = rsFac.RecordCount;
              rsFac.Read();
              TheTaskID = rsFac.Item("DefaultTaskID");
            }


            rsCompany = new DataReader( "SELECT Id, CompanyName FROM dbo.IRGCompany ORDER BY CompanyName ASC");
            rsCompany.Open();
            rsCompany_numRows = 0;


            rsRegion = new DataReader( "SELECT ID, RegionDescription  FROM dbo.IRGRegion  ORDER BY RegionDescription ASC");
            rsRegion.Open();
            rsRegion_numRows = 0;


            string rsTasks__PTaskID;
            rsTasks__PTaskID = "1";
            if(TheTaskID != ""){rsTasks__PTaskID = TheTaskID;}


            //rsTasks.Source = "SELECT Id, TaskCode, TaskDescription  FROM dbo.Tasks  WHERE (Active=1 AND PayType <> 'HOURS')  OR (ID=" + Replace(rsTasks__PTaskID, "'", "''") + ")  Order By TaskDescription";
            rsTasks = new DataReader( "SELECT Id, TaskCode, TaskDescription  FROM dbo.Tasks  WHERE (Active=1)  OR (ID=" + Replace(rsTasks__PTaskID, "'", "''") + ")  Order By TaskDescription");
            rsTasks.Open();
            rsTasks_numRows = 0;


            rsOTBasis = new DataReader("SELECT * FROM dbo.OvertimeBasis");
            rsOTBasis.Open();
            rsOTBasis_numRows = 0;



        }
    }
}