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
    public partial class RebillSubTaskRatesEdit : PageBase
    {

        public DataReader rsRebill;
        public DataReader rsCustomer;
        public DataReader rsDelete;
        public DataReader rsTasks;
        public DataReader rsHU;
        public int rsRebill_numRows = 0;
        public int rsCustomer_numRows = 0;
        public int rsDelete_numRows = 0;
        public int rsTasks_numRows = 0;
        public int rsHU_numRows = 0;
        public string TheTaskID;
        public string sHoursOrUnits = "";
        public string MM_editAction = "";

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


              //yep yep;
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

              string MM_editTable = "dbo.RebillSubTaskRates";
              string MM_editColumn = "Id";
              string MM_recordId = "" + Request.Form["MM_recordId"] + "";
              string MM_editRedirectUrl = "RebillSubTaskRates.aspx?Id=" + Request["RebillSubTaskId"];
              string MM_fieldsStr  = "EffectiveDate|value|txtHoursOrUnits|value|Rate|value|RateType|value|ExpirationDate|value|LastModifiedOn|value|LastModifiedBy|value|RebillSubTaskId|value";
              string MM_columnsStr = "EffectiveDate|',none,''|HoursOrUnits|',none,'H'|RebillRate|none,none,NULL|RebillType|',none,''|ExpirationDate|',none,NULL|LastModifiedOn|',none,NULL|LastModifiedBy|',none,''|RebillSubTasksId|none,none,NULL";

              // create the MM_fields && MM_columns arrays;
              string[] MM_fields = Split(MM_fieldsStr, "|");
              string[] MM_columns = Split(MM_columnsStr, "|");

              // the form values;
              for(int i = LBound(MM_fields); i < UBound(MM_fields); i = i + 2){ 
                MM_fields[i+1] = cStr(Request.Form[MM_fields[i]]);
              }

              // append the query string to the redirect URL;
              if(MM_editRedirectUrl != "" && Request.QueryString.Count > 0){
                if(InStr(0, MM_editRedirectUrl, "?",1) == 0 && Request.QueryString.Count > 0){
                  MM_editRedirectUrl = MM_editRedirectUrl + "?" + Request.QueryString;
                }else{
                  MM_editRedirectUrl = MM_editRedirectUrl + "&" + Request.QueryString;
               }
             }

            //}


            // *** Update Record: construct a sql update statement && execute it;

              if (cStr(Request["MM_update"]) != "" && cStr(Request["MM_recordId"]) != "")
              {

                  // create the sql update statement;
                  MM_editQuery = "update " + MM_editTable + " set ";
                  for (int i = LBound(MM_fields); i < UBound(MM_fields); i = i + 2)
                  {
                      string FormVal = MM_fields[i + 1];
                      string[] MM_typeArray = Split(MM_columns[i + 1], ",");
                      string Delim = MM_typeArray[0];
                      if (Delim == "none") { Delim = ""; }
                      string AltVal = MM_typeArray[1];
                      if (AltVal == "none") { AltVal = ""; }
                      string EmptyVal = MM_typeArray[2];
                      if (EmptyVal == "none") { EmptyVal = ""; }
                      if (FormVal == "")
                      {
                          FormVal = EmptyVal;
                      }
                      else
                      {
                          if (AltVal != "")
                          {
                              FormVal = AltVal;
                          }
                          else
                          {
                              if (Delim == "'")
                              { // escape quotes;
                                  FormVal = "'" + Replace(FormVal, "'", "''") + "'";
                              }
                              else
                              {
                                  FormVal = Delim + FormVal + Delim;
                              }
                          }
                      }
                      if (i != LBound(MM_fields))
                      {
                          MM_editQuery = MM_editQuery + ",";
                      }
                      MM_editQuery = MM_editQuery + MM_columns[i] + " = " + FormVal;
                  }
                  MM_editQuery = MM_editQuery + " where " + MM_editColumn + " = " + MM_recordId;

                  if (!MM_abortEdit)
                  {
                      // execute the update;
                      this.Execute(MM_editQuery);
                      if (MM_editRedirectUrl != "")
                      {
                          Response.Redirect(MM_editRedirectUrl);
                      }
                  }

              }
              else
              {
                  if (cStr(Request["MM_update"]) != "" && cStr(Request["MM_recordId"]) == "")
                  {
                      // create the sql insert statement;
                      string MM_tableValues = "";
                      string MM_dbValues = "";

                      for (int i = LBound(MM_fields); i < UBound(MM_fields); i = i + 2)
                      {
                          string FormVal = MM_fields[i + 1];
                          string[] MM_typeArray = Split(MM_columns[i + 1], ",");
                          string Delim = MM_typeArray[0];
                          if (Delim == "none") { Delim = ""; }
                          string AltVal = MM_typeArray[1];
                          if (AltVal == "none") { AltVal = ""; }
                          string EmptyVal = MM_typeArray[2];
                          if (EmptyVal == "none") { EmptyVal = ""; }
                          if (FormVal == "")
                          {
                              FormVal = EmptyVal;
                          }
                          else
                          {
                              if (AltVal != "")
                              {
                                  FormVal = AltVal;
                              }
                              else
                              {
                                  if (Delim == "'")
                                  {
                                      // escape quotes;
                                      FormVal = "'" + Replace(FormVal, "'", "''") + "'";
                                  }
                                  else
                                  {
                                      FormVal = Delim + FormVal + Delim;
                                  }
                              }
                          }
                          if (i != LBound(MM_fields))
                          {
                              MM_tableValues = MM_tableValues + ",";
                              MM_dbValues = MM_dbValues + ",";
                          }
                          MM_tableValues = MM_tableValues + MM_columns[i];
                          MM_dbValues = MM_dbValues + FormVal;
                      }
                      MM_editQuery = "insert into " + MM_editTable + " (" + MM_tableValues + ") values (" + MM_dbValues + ")";

                      if (!MM_abortEdit)
                      {
                          // execute the insert;
                          this.Execute(MM_editQuery);

                          if (MM_editRedirectUrl != "")
                          {
                              Response.Redirect(MM_editRedirectUrl);
                          }
                      }

                  }

              }

            string rsRebill__MM_ColParam;
            rsRebill__MM_ColParam = "1";
            if(Request.QueryString["Id"]  != ""){rsRebill__MM_ColParam = Request.QueryString["Id"];}


            if(cStr(Request["ID"]) == "0"){
              //string strSQL;
              //rsRebill = Server.CreateObject("ADODB.Recordset");
              //strSQL = "SELECT * FROM dbo.RebillSubTaskRates WHERE Id =-1 ";
              //rsRebill.CursorLocation = 3;
              //rsRebill.Open strSQL, MM_Main_STRING, 1, 3, 1;
              //rsRebill.AddNew;
                rsRebill = new DataReader();

            }else{
                rsRebill = new DataReader("SELECT Id, RebillSubTasksId, CONVERT(varchar(10), EffectiveDate, 101) AS EffectiveDate, HoursOrUnits, RebillRate, RebillType, CONVERT(varchar(10), ExpirationDate, 101) AS ExpirationDate, LastModifiedBy, LastModifiedOn  FROM dbo.RebillSubTaskRates  WHERE Id = " + Replace(rsRebill__MM_ColParam, "'", "''") + "");
              rsRebill.Open();
              rsRebill.Read();
              rsRebill_numRows = rsRebill.RecordCount;
            }


            rsHU = new DataReader("SELECT HoursOrUnits=IsNull(HoursOrUnits, 'H')  FROM RebillSubTasks  WHERE Id = " + cStr(Request["RBTask"]));
            rsHU.Open();
            rsHU.Read();
            rsHU_numRows = rsHU.RecordCount;
            if(rsHU.EOF){
              sHoursOrUnits = "H";
            }else{
              sHoursOrUnits = rsHU.Item("HoursOrUnits");
            }

            rsCustomer = new DataReader("SELECT Id, FacilityId, CustomerCode, CustomerName FROM dbo.FacilityCustomer");
            rsCustomer.Open();
            rsCustomer_numRows = 0;

            rsDelete = new DataReader("SELECT * FROM dbo.Facility");
            rsDelete.Open();
            rsDelete_numRows = 0;



        }
    }
}