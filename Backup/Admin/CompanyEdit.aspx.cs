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
    public partial class CompanyEdit : PageBase
    {
        public bool MM_abortEdit;
        public string MM_editQuery;
        public string MM_editAction;

        public string MM_editConnection = "";
        public string MM_editTable = "";
        public string MM_editColumn = "";
        public string MM_recordId = "";
        public string MM_editRedirectUrl = "";
        public string MM_fieldsStr = "";
        public string MM_columnsStr = "";

        public string[] MM_fields;
        public string[] MM_columns;

        public DataReader rsPayPeriod;
        public DataReader rs;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

            GrantAccess("Super");


            MM_editAction = cStr(Request["URL"]);

            // boolean to abort record edit;
            MM_abortEdit = false;

            // query string to execute;
            MM_editQuery = "";

            // *** Insert/Update Record:  variables;

            //if(cStr(Request["MM_update"]) != "" && cStr(Request["MM_recordId")) != "0") ){

          //MM_editConnection = MM_Main_STRING;
          MM_editTable = "dbo.IRGCompany";
          MM_editColumn = "Id";
          MM_recordId = "" + Request.Form["MM_recordId"] + "";
          MM_editRedirectUrl = "Company.aspx";
          MM_fieldsStr  = "CompanyID|value|CompanyName|value|PayrollCompanyCode|value|LogoPath|value|select|value|OutOfTownRate|value|OutOfTownHoursPerDay|value|checkbox|value|LastModifiedOn|value|LastModifiedBy|value";
          MM_columnsStr = "CompanyID|',none,''|CompanyName|',none,''|PayrollCompanyCode|',none,''|LogoPath|',none,''|PayPeriodID|none,none,NULL|OutOfTownRate|none,none,NULL|OutOfTownHoursPerDay|none,none,NULL|Active|none,1,0|LastModifiedOn|',none,NULL|LastModifiedBy|',none,''";

          // create the MM_fields && MM_columns arrays;
          MM_fields = Split(MM_fieldsStr, "|");
          MM_columns = Split(MM_columnsStr, "|");
  
          //  the form values;
          for( int i = LBound(MM_fields); i < UBound(MM_fields); i = i + 2){
            MM_fields[i+1] = cStr(Request.Form[MM_fields[i]]);
          } 

          // append the query string to the redirect URL;
          if(MM_editRedirectUrl != "" && Request.QueryString.Count != 0){
            if(InStr(1, MM_editRedirectUrl, "?", 0) == 0 && Request.QueryString.Count != 0){
              MM_editRedirectUrl = MM_editRedirectUrl + "?" + Request.QueryString;
            }else{
              MM_editRedirectUrl = MM_editRedirectUrl + "&" + Request.QueryString;
            }
          }

         //}


        // *** Update Record: construct a sql update statement && execute it;
        string FormVal = "";
        string AltVal = "";
        string EmptyVal = "";
        string Delim = "";
        string[] MM_typeArray;

        if((cStr(Request["MM_update"]) != "") && (cStr(Request["MM_recordId"]) != "")){
          // create the sql update statement;
          MM_editQuery = "update " + MM_editTable + " set ";
          for( int i = LBound(MM_fields); i < UBound(MM_fields); i = i + 2){
            FormVal = MM_fields[i+1];
            MM_typeArray = Split(MM_columns[i+1],",");
            Delim = MM_typeArray[0];
            if(Delim == "none"){ Delim = "";}
            AltVal = MM_typeArray[1];
            if(AltVal == "none"){ AltVal = "";}
            EmptyVal = MM_typeArray[2];
            if(EmptyVal == "none"){ EmptyVal = "";}
            if(FormVal == ""){
              FormVal = EmptyVal;
            }else{
              if(AltVal != "") {
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
          } // End For
          MM_editQuery = MM_editQuery + " where " + MM_editColumn + " = " + MM_recordId;

          if(!MM_abortEdit){

            // execute the update;
              Execute(MM_editQuery);
            //MM_editCmd = Server.CreateObject("ADODB.Command");
            //MM_editCmd.ActiveConnection = MM_editConnection;
            //MM_editCmd.CommandText = MM_editQuery;
            //MM_editCmd.Execute;
            //MM_editCmd.ActiveConnection.Close;

            if(MM_editRedirectUrl != ""){
              Response.Redirect(MM_editRedirectUrl);
            }
          }

        }else{
            if ((cStr(Request["MM_update"]) != "") && (cStr(Request["MM_recordId"]) == ""))
            {
                // create the sql insert statement;
                string MM_tableValues = "";
                string MM_dbValues = "";
                for (int i = LBound(MM_fields); i < UBound(MM_fields); i = i + 2)
                {
                    FormVal = MM_fields[i + 1];
                    MM_typeArray = Split(MM_columns[i + 1], ",");
                    Delim = MM_typeArray[0];
                    if (Delim == "none") { Delim = ""; }
                    AltVal = MM_typeArray[1];
                    if (AltVal == "none") { AltVal = ""; }
                    EmptyVal = MM_typeArray[2];
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
                    Execute(MM_editQuery);
                    //MM_editCmd = Server.CreateObject("ADODB.Command");
                    //MM_editCmd.ActiveConnection = MM_editConnection;
                    //MM_editCmd.CommandText = MM_editQuery;
                    //MM_editCmd.Execute;
                    //MM_editCmd.ActiveConnection.Close;

                    if (MM_editRedirectUrl != "")
                    {
                        Response.Redirect(MM_editRedirectUrl);
                    }
                }
            }
        }



        string rs__MMColParam;
        rs__MMColParam = "1";
        if(Request.QueryString["Id"] != ""){ rs__MMColParam = Request.QueryString["Id"];}


        if(cStr(Request["ID"]) == "0"){
          string strSQL;
          //rs = Server.CreateObject("ADODB.Recordset");
          //strSQL = "SELECT * FROM dbo.IRGCompany WHERE Id =-1 ";
          //rs.CursorLocation = 3;
          //rs.Open strSQL, MM_Main_STRING, 1, 3, 1;
          //rs.AddNew;
          rs = new DataReader();
        }else{
            rs = new DataReader("SELECT Id,CompanyID,CompanyName,LogoPath,PayPeriodID,PayrollCompanyCode,OutOfTownRate,OutOfTownHoursPerDay,LastModifiedBy,LastModifiedOn,Active FROM dbo.IRGCompany WHERE Id = " + Replace(rs__MMColParam, "'", "''") + " ORDER BY CompanyName ASC");
            rs.Open();
            rs.Read();
        }


        rsPayPeriod = new DataReader("SELECT Id, Description FROM dbo.PayPeriod");
        rsPayPeriod.Open();




        }
    }
}