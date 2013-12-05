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
    public partial class TaskRatesEdit : PageBase {

        
        public DataReader rsEmp;
        public DataReader rs;
        public DataReader rsTasks;
        public int rsEmp_numRows = 0;
        public int rs_numRows = 0;
        public int rsTasks_numRows = 0;
        public string sPayType;
        public string MM_editAction = "";

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


             GrantAccess("Super, Admin, User");



            // *** Edit Operations: declare variables;

            MM_editAction = cStr(Request["URL"]) + "?TID=" + Request["TID"]  + "&Task=" + Request["Task"];
            if(Request.QueryString.Count > 0){
              //MM_editAction = MM_editAction + "?" + Request.QueryString;
            }

            // boolean to abort record edit;
            bool MM_abortEdit = false;

            // query string to execute;
            string MM_editQuery = "";



            // *** Update Record: variables;


              string MM_editTable = "dbo.TaskRates";
              string MM_editColumn = "ID";
              string MM_recordId = "" + Request.Form["MM_recordId"] + "";
              string MM_editRedirectUrl = "TaskRates.aspx"+ "?ID=" + Request["TID"] + "&Task=" + Request["Task"];
              string MM_fieldsStr  = "selTaskID|value|select2|value|HoursPayRate|value|UnitsPayRate|value|EffectiveDate|value|ExpirationDate|value|LastModifiedOn|value|LastModifiedBy|value";
              string MM_columnsStr = "FaciltityTaskID|none,none,NULL|EmployeeID|none,none,NULL|HoursPayRate|none,none,NULL|UnitsPayRate|none,none,NULL|EffectiveDate|',none,NULL|ExpirationDate|',none,NULL|LastModifiedOn|',none,NULL|LastModifiedBy|',none,''";

              // create the MM_fields && MM_columns arrays;
              string[] MM_fields = Split(MM_fieldsStr, "|");
              string[] MM_columns = Split(MM_columnsStr, "|");

              // the form values;
              for(int i = LBound(MM_fields); i < UBound(MM_fields); i = i + 2){ 
                MM_fields[i+1] = cStr(Request.Form[MM_fields[i]]);
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

            rsTasks = new DataReader( "SELECT Id, TaskDescription, PayType  FROM dbo.FacilityTask  WHERE Active=1 AND ID=" + cStr(Request["TID"]));
            rsTasks.Open();
            rsTasks_numRows = 0;
             while ( !rsTasks.EOF){
               if(cStr(rsTasks.Item("Id")) == Request["TID"]){
                  sPayType = UCase(Trim(rsTasks.Item("PayType")));
              }

             }
            rsTasks.Requery();




            string rs__MMPARAM;
            rs__MMPARAM = "1";
            if(Request["ID"] != ""){rs__MMPARAM = Request["ID"];}


            if(Request["ID"] == "0"){
              //string strSQL, sPayType;
              //rs = Server.CreateObject("ADODB.Recordset");
              //strSQL = "SELECT tr.ID, tr.FaciltityTaskID, tr.EmployeeID, PayType='" + sPayType + "', tr.HoursPayRate, tr.UnitsPayRate, tr.EffectiveDate, tr.ExpirationDate, tr.LastModifiedBy, tr.LastModifiedOn, ft.FacilityId,                         ft.TaskDescription, e.LastName, e.FirstName, e.EmployeeNumber  FROM dbo.TaskRates tr LEFT OUTER JOIN                        dbo.Employee e ON tr.EmployeeID = e.Id LEFT OUTER JOIN                        dbo.FacilityTask ft ON tr.FaciltityTaskID = ft.Id  WHERE (e.Active = 1) AND (ft.Active = 1)      AND tr.ID = -1";
              //rs.CursorLocation = 3;
              //rs.Open strSQL, MM_Main_STRING, 1, 3, 1;
              //rs.AddNew;
              //rsCompany.Update;
                rs = new DataReader();
            }else{
              rs = new DataReader( "SELECT tr.ID, tr.FaciltityTaskID, tr.EmployeeID, ft.PayType, tr.HoursPayRate, tr.UnitsPayRate, tr.EffectiveDate, tr.ExpirationDate, tr.LastModifiedBy, tr.LastModifiedOn, ft.FacilityId,  ft.TaskDescription, e.LastName, e.FirstName, e.EmployeeNumber  FROM dbo.TaskRates tr LEFT OUTER JOIN  dbo.Employee e ON tr.EmployeeID = e.Id LEFT OUTER JOIN  dbo.FacilityTask ft ON tr.FaciltityTaskID = ft.Id  WHERE (e.Active = 1) AND (ft.Active = 1) AND tr.ID = " + Replace(rs__MMPARAM, "'", "''") + "");
              rs.Open();
              rs_numRows = 0;
            }


            rsEmp = new DataReader( " SELECT dbo.Employee.Id, dbo.Employee.LastName, dbo.Employee.FirstName, dbo.Employee.EmployeeNumber, dbo.EmploymentSource.FacilityId " + "   FROM dbo.Employee LEFT OUTER JOIN dbo.EmploymentSource ON dbo.Employee.Id = dbo.EmploymentSource.Id " + "  WHERE dbo.Employee.Active = 1 AND dbo.EmploymentSource.FacilityId = " + cStr(Session["FacilityID"]));

            rsEmp.Open();
            rsEmp_numRows = 0;



        }
    }
}