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
    public partial class CustomerEdit : PageBase
    {

        public DataReader rs;
        public DataReader rsRC;
        public string MM_editAction = "";

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



              // *** Update Record:  variables;


              string MM_editTable = "dbo.FacilityCustomer";
              string MM_editColumn = "Id";
              string MM_recordId = "" + Request.Form["MM_recordId"] + "";
              string MM_editRedirectUrl = "Customer.aspx";
              string MM_fieldsStr  = "CustomerCode|value|CustomerName|value|defaultcustomer|value|Active|value|contactname|value|address1|value|address2|value|address3|value|LastModifiedOn|value|LastModifiedBy|value|ThisFacID|value";
              string MM_columnsStr = "CustomerCode|',none,''|CustomerName|',none,''|DefaultCustomer|none,1,0|Active|none,1,0|ContactName|',none,''|ContactAddress1|',none,''|ContactAddress2|',none,''|ContactAddress3|',none,''|LastModifiedOn|',none,NULL|LastModifiedBy|',none,''|FacilityId|none,none,NULL";

              // create the MM_fields && MM_columns arrays;
              string[] MM_fields = Split(MM_fieldsStr, "|");
              string[] MM_columns = Split(MM_columnsStr, "|");
  
              //  the form values;
              for( int i = LBound(MM_fields); i < UBound(MM_fields) - 1; i++){
                MM_fields[i+1] = cStr(Request.Form[MM_fields[i]]);
              }

              // append the query string to the redirect URL;
              if(MM_editRedirectUrl != "" && Request.QueryString.Count > 0){
                if(InStr(1, MM_editRedirectUrl, "?", 0) == 0 && Request.QueryString.Count > 0){
                  MM_editRedirectUrl = MM_editRedirectUrl + "?" + Request.QueryString;
                }else{
                  MM_editRedirectUrl = MM_editRedirectUrl + "&" + Request.QueryString;
               }
             }




            // *** Update Record: construct a sql update statement && execute it;

            if(cStr(Request["MM_update"]) != "" && cStr(Request["MM_recordId"]) != ""){

              // create the sql update statement;
              MM_editQuery = "update " + MM_editTable + " set ";
              for( int i = LBound(MM_fields); i < UBound(MM_fields); i = i + 2){
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

                  for(int i = LBound(MM_fields) ; i < UBound(MM_fields); i= i + 2){

                    string FormVal = MM_fields[i+1];
                    string[] MM_typeArray = Split(MM_columns[i+1],",");
                    string Delim = MM_typeArray[0];
                    if(Delim == "none"){ Delim = ""; }
                    string AltVal = MM_typeArray[1];
                    if(AltVal == "none")
                        AltVal = "";
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

                  } //End For


                  MM_editQuery = "insert into " + MM_editTable + " (" + MM_tableValues + ") values (" + MM_dbValues + ")";

                  if(!MM_abortEdit){
                    // execute the insert;
                    this.Execute(MM_editQuery);
                    if(MM_editRedirectUrl != ""){
                      Response.Redirect(MM_editRedirectUrl);
                    }
                  }

            }
         }


        string rs__MMColParam;
         rs__MMColParam = "1";

        if(Request.QueryString["Id"] != ""){rs__MMColParam = Request.QueryString["Id"];}


        if(cStr(Request["ID"]) == "0"){
          //rs = new DataReader("SELECT * FROM dbo.FacilityCustomer WHERE Id =-1 "); 
          //rs.AddNew;
          rs = new DataReader();
        }else{
           rs = new DataReader("SELECT Id, FacilityId, CustomerCode, CustomerName, ContactName, ContactAddress1, ContactAddress2, ContactAddress3, DefaultCustomer, LastModifiedBy, LastModifiedOn, Active  FROM dbo.FacilityCustomer  WHERE Id = " + Replace(rs__MMColParam, "'", "''") + "");
           rs.Open();
           rs.Read();
        }

         rsRC = new DataReader("SELECT RailCarrierCode, RailCarrierName  FROM dbo.IRGRailCarrier  Order By RailCarrierName");
         rsRC.Open();

        }
    }
}