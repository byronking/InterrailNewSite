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
    public partial class EmployeeEdit : PageBase
    {

        public string ErrMsg;
        public string ThisFacID;
        public DataReader  rs;
        public DataReader  nrs;
        public DataReader rsEmploymentSource;
        public DataReader rsEmpNumber;
        public DataReader rsTempNumber;
        public string MM_editAction = "";
        public int ThisMonth;
        public string YYMM_1 ="";
        public string YYMM_2 ="";
        public string YYMM_3 ="";
        public string YYMM_4 ="";
        public string YYMM_5 ="";
        public string MM_1 ="";
        public string NewTempNumber = "";

        public string NewEmpNumber;
        public string LastEmpNumber;
        public int NewNumber;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

             GrantAccess("Super, Admin, User");


               string YY = cStr(Mid(System.DateTime.Now.Year.ToString(),2, 2));
               ThisMonth = System.DateTime.Now.Month;
               if(ThisMonth < 10){
                 MM_1 = "0" + cStr(ThisMonth);
               }else{
                 MM_1 = cStr(ThisMonth);
              }

               YYMM_1 = YY + MM_1;
               YYMM_2 = YY + cStr(ThisMonth+20);
               YYMM_3 = YY + cStr(ThisMonth+40);
               YYMM_4 = YY + cStr(ThisMonth+60);
               YYMM_5 = YY + cStr(ThisMonth+80);


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

              //MM_editConnection = MM_Main_STRING;
              string MM_editTable = "dbo.Employee";
              string MM_editColumn = "Id";
              string MM_recordId = "" + Request.Form["MM_recordId"] + "";
              string MM_editRedirectUrl = "Employee.aspx";
              string MM_fieldsStr  = "LastName|value|MiddleInitial|value|FirstName|value|SSN|value|BirthDate|value|EmpPhone|value|Address1|value|Address2|value|City|value|State|value|Zip|value|EmergencyContact|value|ContactPhone|value|Active|value|EmpStatus|value|EmployeeNumber|value|TempNumber|value|HireDate|value|TempStartDate|value|InactiveDate|value|select2|value|TerminationDate|value|Salaried|value|LastModifiedOn|value|LastModifiedBy|value|ThisFacID|value";
              string MM_columnsStr = "LastName|',none,''|MiddleInitial|',none,''|FirstName|',none,''|SSN|',none,''|BirthDate|',none,NULL|EmployeePhone|',none,''|Address1|',none,''|Address2|',none,''|City|',none,''|State|',none,''|Zip|',none,''|EmergencyContact|',none,''|ContactPhone|',none,''|Active|none,1,0|TempEmployee|none,none,NULL|EmployeeNumber|',none,NULL|TempNumber|',none,NULL|HireDate|',none,NULL|TempStartDate|',none,NULL|InactiveDate|',none,NULL|EmploymentSourceId|none,none,NULL|TerminationDate|',none,NULL|Salaried|none,1,0|LastModifiedOn|',none,NULL|LastModifiedBy|',none,''|FacilityID|none,none,NULL";

              // create the MM_fields && MM_columns arrays;
              string[] MM_fields = Split(MM_fieldsStr, "|");
              string[] MM_columns = Split(MM_columnsStr, "|"); 

              //  the form values;
              for(int i = LBound(MM_fields) ; i< UBound(MM_fields); i = i + 2){
                MM_fields[i+1] = cStr(Request.Form[MM_fields[i]]);
              }

              // append the query string to the redirect URL;
              if(MM_editRedirectUrl != "" && Request.QueryString.Count > 0){
                if(InStr(0, MM_editRedirectUrl, "?", 0) == 0 && Request.QueryString.Count > 0){
                  MM_editRedirectUrl = MM_editRedirectUrl + "?" + Request.QueryString;
                }else{
                  MM_editRedirectUrl = MM_editRedirectUrl + "&" + Request.QueryString;
               }
             }

            

            // *** Update Record: construct a sql update statement && execute it;
                // execute the update;
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
                  } //End For


                  MM_editQuery = MM_editQuery + " where " + MM_editColumn + " = " + MM_recordId;


                  if (Request["EmpStatus"] == "0")
                  {
                      nrs = new DataReader("Select NRec=Count(*) From Employee Where ID <> " + Request["MM_recordId"] + " AND (EmployeeNumber='" + Request["EmployeeNumber"] + "')");
                  }
                  else
                  {
                      nrs = new DataReader("Select NRec=Count(*) From Employee Where ID <> " + Request["MM_recordId"] + " AND (TempNumber='" + Request["TempNumber"] + "')");
                  }

                  nrs.Open();
                  nrs.Read();

                  if (System.Convert.ToInt32(nrs.Item("NRec")) != 0)
                  {
                      MM_abortEdit = true;
                      ErrMsg = "Employee number must be unique!";
                  }

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
                              MM_tableValues = MM_tableValues + ",";
                              MM_dbValues = MM_dbValues + ",";
                          }
                          MM_tableValues = MM_tableValues + MM_columns[i];
                          MM_dbValues = MM_dbValues + FormVal;

                      } //End For

                      MM_editQuery = "insert into " + MM_editTable + " (" + MM_tableValues + ") values (" + MM_dbValues + ")";

                      if (Request["EmpStatus"] == "0")
                      {
                          nrs = new DataReader("Select NRec=Count(*) From Employee Where (EmployeeNumber='" + Request["EmployeeNumber"] + "')");
                      }
                      else
                      {
                          nrs = new DataReader("Select NRec=Count(*) From Employee Where (TempNumber='" + Request["TempNumber"] + "')");
                      }

                      nrs.Open();
                      nrs.Read();

                      if (System.Convert.ToInt32(nrs.Item("NRec")) != 0)
                      {
                          MM_abortEdit = true;
                          ErrMsg = "Employee number must be unique!";
                      }

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


        string rs__MMColParam;
         rs__MMColParam = "1";
        if(cStr(Request.QueryString["Id"]) != ""){rs__MMColParam = Request.QueryString["Id"];}
        if(Request["MM_recordId"] != null && cStr(Request["MM_recordId"]) != ""){rs__MMColParam = Request["MM_recordId"];}


        if(cStr(Request["ID"]) == "0"  || (cStr(Request["ID"]) == "" && cStr(Request["MM_recordId"]) == "")){
          string strSQL;
           //rs = Server.CreateObject("ADODB.Recordset");
          //strSQL = "SELECT * FROM dbo.Employee WHERE Id =-1 ";
          //rs.CursorLocation = 3;
          //rs.Open strSQL, MM_Main_STRING, 1, 3, 1;
          //rs.AddNew;
          rs = new DataReader();

          ThisFacID = "0";
        }else{
          rs = new DataReader("SELECT Id, EmployeeNumber, TempNumber, LastName, FirstName, MiddleInitial, HireDate, InactiveDate, TempStartDate, TerminationDate, Address1, Address2, City, State, Zip, SSN, BirthDate, EmployeePhone, EmergencyContact, ContactPhone, TempEmployee, EmploymentSourceId, FacilityID, LastModifiedBy, LastModifiedOn, Active, Salaried  FROM dbo.Employee  WHERE Id = " + Replace(rs__MMColParam, "'", "''") + "");
          rs.Open();
          rs.Read();
          ThisFacID = rs.Item("FacilityID");
        }


        string rsEmploymentSource__PFacilityID;
        rsEmploymentSource__PFacilityID = "6";
        if(Session["FacilityID"] != ""){rsEmploymentSource__PFacilityID = System.Convert.ToString(Session["FacilityID"]);}


        string rsEmploymentSource__PCFacID;
        rsEmploymentSource__PCFacID = "0";
        if(ThisFacID != ""){rsEmploymentSource__PCFacID = ThisFacID;}

        rsEmploymentSource = new DataReader("SELECT EmploymentSource.Id, EmploymentSource.SourceName  FROM EmploymentSource  WHERE FacilityID = " + Replace(rsEmploymentSource__PFacilityID, "'", "''") + " Or FacilityID=" + Replace(rsEmploymentSource__PCFacID, "'", "''") + "  ORDER BY EmploymentSource.SourceName");
        rsEmploymentSource.Open();
        int rsEmploymentSource_numRows = rsEmploymentSource.RecordCount;


        string rsEmpNumber__POne;
        rsEmpNumber__POne = "0110";
        if(YYMM_1 != ""){rsEmpNumber__POne = YYMM_1;}


        string rsEmpNumber__PTwo;
        rsEmpNumber__PTwo = "0130";
        if(YYMM_2 != ""){rsEmpNumber__PTwo = YYMM_2;}


        string rsEmpNumber__PThree;
        rsEmpNumber__PThree = "0150";
        if(YYMM_3 != ""){rsEmpNumber__PThree = YYMM_3;}


        string rsEmpNumber__PFour;
        rsEmpNumber__PFour = "0170";
        if(YYMM_4 != ""){rsEmpNumber__PFour = YYMM_4;}


        string rsEmpNumber__PFive;
        rsEmpNumber__PFive = "0180";
        if(YYMM_5 != ""){rsEmpNumber__PFive = YYMM_5;}


        rsEmpNumber = new DataReader("SELECT Top 1  EmployeeNumber  FROM dbo.Employee   Where EmployeeNumber Like '" + Replace(rsEmpNumber__POne, "'", "''") + "%'    Or EmployeeNumber Like '" + Replace(rsEmpNumber__PTwo, "'", "''") + "%'  Or EmployeeNumber Like '" + Replace(rsEmpNumber__PThree, "'", "''") + "%'  Or EmployeeNumber Like '" + Replace(rsEmpNumber__PFour, "'", "''") + "%'  Or EmployeeNumber Like '" + Replace(rsEmpNumber__PFive, "'", "''") + "%'  ORDER BY EmployeeNumber DESC");
        rsEmpNumber.Open();
        rsEmpNumber.Read();
        int rsEmpNumber_numRows =  rsEmpNumber.RecordCount;



        if(rsEmpNumber.EOF){
          NewEmpNumber = YYMM_1 + "00";
        }else{
          LastEmpNumber = rsEmpNumber.Item("EmployeeNumber");
          NewNumber = cInt(Mid(LastEmpNumber, 5, 2)) + 1;
          if(NewNumber < 10){
            NewEmpNumber = Mid(LastEmpNumber, 1, 4) + "0" + cStr(NewNumber);
          }else{
            if(NewNumber > 99){

                string val = Mid(LastEmpNumber, 1, 4);
                if(val == YYMM_1){  
                    NewEmpNumber = YYMM_2 + "00"; 
                }else{ 
                if(val == YYMM_2){ 
                        NewEmpNumber = YYMM_3 + "00"; 
                }else{
                if(val == YYMM_3){ 
                    NewEmpNumber = YYMM_4 + "00"; 
                }else{
                if(val == YYMM_4){ 
                    NewEmpNumber = YYMM_5 + "00"; 
                }else{
                  NewEmpNumber = "000000";
                }
                }}}
      
            }else{
                NewEmpNumber = Mid(LastEmpNumber, 1, 4) + cStr(NewNumber);
            }
         }
        }


        rsTempNumber = new DataReader("SELECT MaxTemp=Convert(char(10), Max(TempNumber)+10001)  FROM dbo.Employee");
        rsTempNumber.Open();
        rsTempNumber.Read();
        int rsTempNumber_numRows = rsTempNumber.RecordCount;

          if(System.Convert.ToInt32(rsTempNumber.Item("MaxTemp")) > 19999){
            NewTempNumber = "0000";
          }else{
            NewTempNumber = (Mid(rsTempNumber.Item("MaxTemp"), 1, 4));
         }

             

        }
    }
}