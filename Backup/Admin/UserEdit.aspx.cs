﻿using System;
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
    public partial class UserEdit : PageBase
    {
          
         public DataReader rs;
         public int rs_numRows = 0;

         public DataReader Recordset1;
         public DataReader rsDelete;
         public DataReader rsUserTypes;

         public int rsDelete_numRows = 0;
         public int rsUserTypes_numRows = 0;
         public int Recordset1_numRows = 0;
         public string MM_editAction = "";

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin");


            // *** Edit Operations: declare variables;

            MM_editAction = cStr(Request["URL"]);
            if(Request.QueryString.Count > 0){
            //  MM_editAction = MM_editAction + "?" + Request.QueryString;
            }

            // boolean to abort record edit;
            bool MM_abortEdit = false;

            // query string to execute;
            string MM_editQuery = "";



            // *** Update Record: variables;


              string MM_editTable = "dbo.UserProfile";
              string MM_editColumn = "Id";
              string MM_recordId = "" + Request.Form["MM_recordId"] + "";
              string MM_editRedirectUrl = "User.aspx";
              string MM_fieldsStr  = "UserID|value|UserName|value|Password|value|select|value|UserLongName|value|LastModifiedBy|value|LastModifiedOn|value";
              string MM_columnsStr = "UserID|',none,''|UserName|',none,''|Password|',none,''|UserType|',none,''|UserLongName|',none,''|LastModifiedBy|',none,''|LastModifiedOn|',none,NULL";

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


            // *** Delete Record: declare variables;

            if(cStr(Request["MM_delete"]) != "" && cStr(Request["MM_recordId"]) != ""){


              MM_editTable = "dbo.UserProfile";
              MM_editColumn = "Id";
              MM_recordId = "" + Request.Form["MM_recordId"] + "";
              MM_editRedirectUrl = "User.aspx";

              // append the query string to the redirect URL;
              if(MM_editRedirectUrl != "" && Request.QueryString.Count > 0){
                if(InStr(0, MM_editRedirectUrl, "?", 1) == 0 && Request.QueryString.Count > 0){
                  MM_editRedirectUrl = MM_editRedirectUrl + "?" + Request.QueryString;
                }else{
                  MM_editRedirectUrl = MM_editRedirectUrl + "&" + Request.QueryString;
               }
             }
  
            }


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
                            { //escape quotes;
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
                                { //escape quotes;
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



            // *** Delete Record: construct a sql delete statement && execute it;

            if(cStr(Request["MM_delete"]) != "" && cStr(Request["MM_recordId"]) != ""){

              // create the sql delete statement;
              MM_editQuery = "delete from " + MM_editTable + " where " + MM_editColumn + " = " + MM_recordId;

              if(!MM_abortEdit){
                // execute the delete;
                this.Execute(MM_editQuery);
    
	            // additionaly delete all records in union table for( user rights;
	            this.Execute("delete from UserRights where UserProfileID =  " + MM_recordId);

                if(MM_editRedirectUrl != ""){
                  Response.Redirect(MM_editRedirectUrl);
                }
               }
            }


            string rs__MM_ColParam;
            rs__MM_ColParam = "1";
            if(Request.QueryString["Id"] != ""){rs__MM_ColParam = Request.QueryString["Id"];}


            if(Request["ID"] == "0"){
              //string strSQL;
              //rs = Server.CreateObject("ADODB.Recordset");
              //strSQL = "SELECT * FROM dbo.UserProfile WHERE Id =-1 ";
              //rs.CursorLocation = 3;
              //rs.Open strSQL, MM_Main_STRING, 1, 3, 1;
              //rs.AddNew;
                rs = new DataReader();


              //rsCompany.Update;
            }else{

              rs = new DataReader( "SELECT Id, UserID, UserName, Password, UserType, UserLongName, LastModifiedBy, LastModifiedOn  FROM dbo.UserProfile  WHERE Id = " + Replace(rs__MM_ColParam, "'", "''") + "  ORDER BY UserName ASC");
              rs.Open();
              rs.Read();
              rs_numRows = rs.RecordCount;

            }
  

            string Recordset1__MM_ColParam;
            Recordset1__MM_ColParam = "1";
            if(Request.QueryString["Id"] != ""){Recordset1__MM_ColParam = Request.QueryString["Id"];}


            Recordset1 = new DataReader("SELECT Id, UserID, UserName, UserLongName, LastModifiedBy, LastModifiedOn  FROM dbo.UserProfile  WHERE Id = " + Replace(Recordset1__MM_ColParam, "'", "''") + "  ORDER BY UserName ASC");
            Recordset1.Open();
            Recordset1_numRows = 0;


            rsUserTypes = new DataReader( "SELECT id, Type FROM dbo.UserTypes");
            rsUserTypes.Open();
            rsUserTypes_numRows = 0;


            string rsDelete__MMColParam;
            rsDelete__MMColParam = "1";
            if(Request.QueryString["Id"] != ""){rsDelete__MMColParam = Request.QueryString["Id"];}


            rsDelete = new DataReader("SELECT Id, UserID, UserName, Password, UserType, UserLongName, LastModifiedBy, LastModifiedOn FROM dbo.UserProfile WHERE Id = " + Replace(rsDelete__MMColParam, "'", "''") + "");
            rsDelete.Open();
            rsDelete.Read();
            rsDelete_numRows = rsDelete.RecordCount;



        }
    }
}