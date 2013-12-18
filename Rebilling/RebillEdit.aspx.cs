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

namespace InterrailPPRS.Rebilling
{
    public partial class RebillEdit : PageBase
    {        
        public string MM_editAction = "";
        public bool MM_abortEdit = false;
        public string MM_editQuery = "";
        public string MM_paramName = "";

        public DataReader rsNewID;
        public DataReader rsDef;
        public DataReader rs;
        public DataReader rsTask;
        public DataReader rsTeams;
        public DataReader rsEmp;
        public DataReader rsMembers;
        public DataReader rsETW;
        public DataReader rsLast;
        public DataReader rsShifts;
        public DataReader rsAttach;

        public int rsDef_numRows;
        public int rs_numRows;
        public int rsTask_numRows;
        public int rsTeams_numRows;
        public int rsEmp_numRows;
        public int rsMembers_numRows;
        public int rsETW_numRows;
        public int rsLast_numRows;
        public int rsShifts_numRows;
        public int rsAttach_numRows;

        public string SubTaskID = "";
        public string strSQL = "";

        public string MM_keepURL = "";
        public string MM_keepForm = "";
        public string MM_keepBoth = "";
        public string MM_keepNone = "";

        public int Repeat1__index;
        public int Repeat1__numRows;
        public int Repeat2__index;
        public int Repeat2__numRows;

        public string sReturnTo = "";
        public string MM_editRedirectUrl = "";

        public string nUnits = "";

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            GrantAccess("Super, Admin, User");

            // *** Edit Operations: declare variables;

            sReturnTo = "&ReturnTo=..%2FRebilling%2FRebillEdit%2Easp%3FID%3D";
            MM_editRedirectUrl = "";

            if(Request["Delete"] != null && cStr(Request["Delete"]) == "YES"){


                strSQL = "Delete RebillDetail Where ID = " + Request["ID"];
                this.Execute(strSQL);
                strSQL = "Delete EmployeeTaskWorked Where RebillDetailID = " + Request["ID"];
                this.Execute(strSQL);

                if(Request["ReturnTo"] != null && cStr(Request["ReturnTo"]) != ""){
                    MM_editRedirectUrl = cStr(Request["ReturnTo"]);
                }else{
                    MM_editRedirectUrl = "RebillDetail.aspx";
                }

                    UpdateUPM (cStr(Request["WorkDate"]),  cStr(Request["allTasks"]), cStr(Request["selShift"]), cStr(Session["FacilityID"]),  cInt(Request["ID"]));

                    Response.Redirect(MM_editRedirectUrl);

            }

            MM_editAction = cStr(Request["URL"]);

            if(Request.QueryString.Count > 0){
              MM_editAction = MM_editAction + "?" + Request.QueryString;
            }


            // *** Update Record: variables;
          string MM_editTable = "dbo.RebillDetail";
          string MM_editColumn = "Id";
          string MM_recordId = "" + Request.Form["MM_rec"] + "";
          MM_editRedirectUrl = "";
          string MM_fieldsStr  = "WorkDate|value|select|value|selShift|value|TotalHours|value|TotalUnits|value|WorkDesc|value|MaterialCosts|value|Vendors|value|InvoiceNumber|value|LastModifiedOn|value|LastModifiedBy|value|FacilityID|value|Rebilled|value";
          string MM_columnsStr = "WorkDate|',none,''|RebillSubTasksId|none,none,NULL|ShiftID|none,none,NULL|TotalHours|none,none,NULL|TotalUnits|none,none,NULL|WorkDescription|',none,''|MaterialCosts|none,none,NULL|Vendors|',none,''|InvoiceNumber|',none,''|LastModifiedOn|',none,''|LastModifiedBy|',none,''|FacilityID|none,none,NULL|Rebilled|none,1,0";

          // create the MM_fields AND MM_columns arrays;
          string[] MM_fields = Split(MM_fieldsStr, "|");
          string[] MM_columns = Split(MM_columnsStr, "|");

          // the form values;
          for(int i = LBound(MM_fields); i < UBound(MM_fields); i = i + 2){ 
            MM_fields[i+1] = cStr(Request.Form[MM_fields[i]]);
          }

          if(cStr(Request["ReturnTo"]) != ""){
             MM_editRedirectUrl = Request["ReturnTo"];
          }else{
             MM_editRedirectUrl = "";
         }


          string TeamMembers = "0";

        // *** Update Record: construct a sql update statement AND execute it;

        if(cStr(Request["MM_update"]) != "" && cStr(Request["MM_rec"]) != ""){

          // create the sql update statement;
          string MM_editQuery = "update " + MM_editTable + " set ";
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
                UpdateUPM (cStr(Request["WorkDate"]),  cStr(Request["allTasks"]), cStr(Request["selShift"]), cStr(Session["FacilityID"]),  cInt(Request["ID"]));

                if (MM_editRedirectUrl != null && MM_editRedirectUrl != "")
                {
                  Response.Redirect(MM_editRedirectUrl);
                }
         }

        }


       // *** Update Record: construct a sql update statement AND execute it;

        if (UCase(Request["URL"]) != "TASKWORKEDEDIT.ASPX")
        {
            if (cStr(Request["MM_upd"]) != "" && cStr(Request["MM_rec"]) != "")
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

                string TM_InsertSQL = InsertTeamMembers(Request["TeamMembers"], cInt(Trim(MM_recordId)));

                MM_editQuery = MM_editQuery + TM_InsertSQL;

                if (!MM_abortEdit)
                {
                    // execute the update;

                    this.Execute(MM_editQuery);

                    UpdateUPM(cStr(Request["WorkDate"]), cStr(Request["allTasks"]), cStr(Request["selShift"]), cStr(Session["FacilityID"]), cInt(Request["ID"]));

                    if (MM_editRedirectUrl != "")
                    {
                        Response.Redirect(MM_editRedirectUrl);
                    }
                }

            }
            else
            {

                if (cStr(Request["MM_upd"]) != "" && cStr(Request["MM_rec"]) == "")
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
                    }

                    MM_editQuery = "insert into " + MM_editTable + " (" + MM_tableValues + ") values (" + MM_dbValues + ")";

                    if (!MM_abortEdit)
                    {

                        // execute the insert;
                        this.Execute(MM_editQuery);


                        rsNewID = new DataReader("Select ID=Max(ID) From  " + MM_editTable);
                        rsNewID.Open();
                        rsNewID.Read();
                        int FPDID = cInt(rsNewID.Item("ID"));

                        string TM_InsertSQL = InsertTeamMembers(Request["TeamMembers"], FPDID);

                        if (TM_InsertSQL != "")
                            this.Execute(TM_InsertSQL);

                        UpdateUPM(cStr(Request["WorkDate"]),cStr(Request["allTasks"]), cStr(Request["selShift"]), cStr(Session["FacilityID"]), FPDID);

                        if (MM_editRedirectUrl != "")
                        {
                            Response.Redirect(MM_editRedirectUrl);
                        }
                        else
                        {
                            Response.Redirect("RebillEdit.aspx?id=" + FPDID);
                        }
                    }

                }
            }

        }


        string rsDef__PFacilityID;
        rsDef__PFacilityID = "0";
        if(Session["FacilityID"] != null && cStr(Session["FacilityID"]) != ""){rsDef__PFacilityID = cStr(Session["FacilityID"]);}


        rsDef = new DataReader( "SELECT Top 1  Id, WorkDate, ShiftID, WorkDescription, TotalHours, TotalUnits, RebillStatus,  RebillSubTasksID  FROM dbo.RebillDetail  Where FacilityID = " + Replace(rsDef__PFacilityID, "'", "''") + "  ORDER BY WorkDate DESC, ID  DESC");
        rsDef.Open();
        rsDef_numRows = rsDef.RecordCount;
        rsDef.Read();
        if(rsDef.EOF){
          SubTaskID = "0";
        }else{
          SubTaskID = rsDef.Item("RebillSubTasksID");
        }


        string rs__MMColParam;
        rs__MMColParam = "1";
        if(Request.QueryString["Id"] != ""){rs__MMColParam = Request.QueryString["Id"];}


        if(cStr(Request["ID"]) == "0"){

          //strSQL =  "SELECT Id, MaterialCosts, Vendors, InvoiceNumber, WorkDate, ShiftID, WorkDescription, TotalHours, TotalUnits, RebillStatus,  RebillSubTasksID, LastModifiedBy, LastModifiedOn, Rebilled  FROM dbo.RebillDetail  WHERE Id  = -1";
          //rs.CursorLocation = 3;
          //rs.Open strSQL, MM_Main_STRING, 1, 3, 1;
          //rs.AddNew;

            rs = new DataReader();

            //FacilityCustID = 0;
            //FacID = 0;
            //TaskID = 0;

        }else{
          rs = new DataReader( "SELECT * FROM dbo.RebillDetail WHERE Id = " + Replace(rs__MMColParam, "'", "''") + "");
          rs.Open();
          rs_numRows = rs.RecordCount;
          rs.Read();
          if (rs.EOF){
            SubTaskID = "0";
          }else{
            SubTaskID = rs.Item("RebillSubTasksID");
         }
        }


        string rsTask__PFacilityID;
        rsTask__PFacilityID = "2";
        if(Session["FacilityID"] != null && cStr(Session["FacilityID"]) != ""){rsTask__PFacilityID = cStr(Session["FacilityID"]);}


        string rsTask__PTaskID;
        rsTask__PTaskID = "1";
        if(SubTaskID  != ""){rsTask__PTaskID = SubTaskID;}


        rsTask = new DataReader("SELECT dbo.RebillSubTasks.ID, dbo.RebillSubTasks.HoursOrUnits, dbo.RebillSubTasks.Description, dbo.FacilityCustomer.CustomerName, dbo.RebillSubTasks.TaskID, dbo.Tasks.TaskDescription, dbo.Tasks.TaskCode,   dbo.FacilityCustomer.CustomerCode, dbo.RebillSubTasks.Active, dbo.FacilityCustomer.FacilityId, dbo.RebillSubTasks.FacilityCustomerId   FROM dbo.RebillSubTasks LEFT OUTER JOIN  dbo.Tasks ON dbo.RebillSubTasks.TaskID = dbo.Tasks.Id LEFT OUTER JOIN dbo.FacilityCustomer ON dbo.RebillSubTasks.FacilityCustomerId = dbo.FacilityCustomer.Id  WHERE RebillSubTasks.Active != 0 AND (FacilityID = " + Replace(rsTask__PFacilityID, "'", "''") + ") OR (FacilityID = " + Replace(rsTask__PFacilityID, "'", "''") + " AND RebillSubTasks.TaskID=" + Replace(rsTask__PTaskID, "'", "''") + ")");
        rsTask.Open();
        rsTask_numRows = rsTask.RecordCount;


        string rsTeams__PFacID;
        rsTeams__PFacID = "0";
        if(Session["FacilityID"] != null && cStr(Session["FacilityID"]) != ""){rsTeams__PFacID = cStr(Session["FacilityID"]);}


        rsTeams = new DataReader("SELECT ID=0, TeamName='  Select Members', TeamMembers='0'  UNION   SELECT ID, TeamName, TeamMembers  FROM dbo.Teams  WHERE FacilityID=" + Replace(rsTeams__PFacID, "'", "''") + " AND Teams.Active=1  ORDER BY TeamName ASC");
        rsTeams.Open();
        rsTeams_numRows = rsTeams.RecordCount;
        rsTeams.Read();

        if ( rsTeams.EOF ){
          TeamMembers="0";
        }else{
          TeamMembers=rsTeams.Item("TeamMembers");
        }

        string AllTeamMembers = "";

        while (!rsTeams.EOF){
          rsTeams.Read(); 
          if( (Len(AllTeamMembers) > 0) && ( Len(Trim(rsTeams.Item("TeamMembers")))>0) ){
             AllTeamMembers = AllTeamMembers + ", ";
          }
          AllTeamMembers = AllTeamMembers  +  rsTeams.Item("TeamMembers");

        }
        rsTeams.Requery();


        string rsEmp__PFACID;
        rsEmp__PFACID = "2";
        if(Session["FacilityID"] != null && cStr(Session["FacilityID"]) != ""){rsEmp__PFACID = cStr(Session["FacilityID"]);}

        strSQL = "";
        strSQL = "SELECT ( Case When ( FacilityID = " + Replace(rsEmp__PFACID, "'", "''") + " ) THEN 0 ELSE 1 End  ), " +
        "FacilityID, Employee.Id,  LastName, FirstName, EmployeeNumber = Case When TempEmployee=0 THEN EmployeeNumber ELSE " +
        "TempNumber End  FROM Employee   WHERE ( (FacilityID = " + Replace(rsEmp__PFACID, "'", "''") + " )  OR  ( FacilityID IN " +
        "( Select AssociatedFacilityID from AssociatedFacility where FacilityID = " + rsEmp__PFACID + "))) AND (Employee.Active=1) ";
        strSQL +=  "UNION ";
        strSQL +=  "SELECT Distinct ( Case When ( FacilityID = " + Replace(rsEmp__PFACID, "'", "''") + " ) THEN 0 ELSE 1 End  ), " +
        "FacilityID, Employee.Id,  LastName, FirstName, EmployeeNumber = Case When TempEmployee=0 THEN EmployeeNumber ELSE " +
        "TempNumber End  FROM Employee  WHERE ";
        if (AllTeamMembers != string.Empty)
        {
            strSQL += "(Employee.Active=1 and employee.id in ";
            strSQL += "( ";
            strSQL = strSQL + AllTeamMembers;
            strSQL += "))";
        }
        else
        {
            strSQL += "(Employee.Active=1)";
        }
        strSQL +=  " ORDER BY ( Case When ( FacilityID = " + Replace(rsEmp__PFACID, "'", "''") + " ) THEN 0 ELSE 1 End  ), " +
        "FacilityID, LastName, FirstName, EmployeeNumber ";

        rsEmp =  new DataReader(strSQL);
        rsEmp.Open();
        rsEmp_numRows = rsEmp.RecordCount;


        DataReader rsMembers;
        rsMembers = new DataReader( "SELECT Id, EmployeeNumber, LastName, FirstName  FROM dbo.Employee  ORDER BY LastName, FirstName, EmployeeNumber");
        rsMembers.Open();
        rsMembers_numRows = rsMembers.RecordCount;


        string rsLast__PFacilityID;
        rsLast__PFacilityID = "2";
        if(Session["FacilityID"] != null && cStr(Session["FacilityID"]) != ""){rsLast__PFacilityID = cStr(Session["FacilityID"]);}

        rsLast = new DataReader("SELECT TOP 5 dbo.RebillDetail.Id, dbo.RebillDetail.WorkDate, dbo.RebillSubTasks.Description  FROM dbo.RebillDetail INNER JOIN  dbo.RebillSubTasks ON dbo.RebillDetail.RebillSubTasksId = dbo.RebillSubTasks.Id  WHERE (dbo.RebillDetail.FacilityID = " + Replace(rsLast__PFacilityID, "'", "''") + ") AND   (dbo.RebillDetail.RebillStatus = 'OPEN')  ORDER BY dbo.RebillDetail.WorkDate DESC, dbo.RebillDetail.Id DESC");
        rsLast.Open();
        rsLast_numRows = rsLast.RecordCount;


        Repeat1__numRows = -1;
        Repeat1__index = 0;
        rsLast_numRows = rsLast_numRows + Repeat1__numRows;


        string rsETW__PFPDID;
        rsETW__PFPDID = "-1";
        if(rs.Item("ID") != ""){rsETW__PFPDID = rs.Item("ID");}


        rsETW = new DataReader("SELECT EmployeeTaskWorked.ID, EmployeeID, HoursWorked, LastName, FirstName, RebillDetailID, WorkDate, ShiftID  FROM EmployeeTaskWorked INNER JOIN Employee ON EmployeeTaskWorked.EmployeeId = Employee.Id  WHERE RebillDetailID = " + Replace(rsETW__PFPDID, "'", "''") + "");
        rsETW.Open();
        rsETW.Read();
        rsETW_numRows = rsETW.RecordCount;


        rsShifts = new DataReader("Select ID, Shift  From Shifts");
        rsShifts.Open();
        rsShifts_numRows = rsShifts.RecordCount;

        Repeat2__numRows = -1;
        Repeat1__index = 0;
        rsETW_numRows = rsETW_numRows + Repeat2__numRows;


        // *** Go To Record AND Move To Record: create strings for maintaining URL AND Form parameters;

        // create the list of parameters which should !be maintained;
        string MM_removeList = "&index=";
        if(MM_paramName != ""){MM_removeList = MM_removeList + "&" + MM_paramName + "=";}
        

        // add the URL parameters to the MM_keepURL string;
        for( int x = 0; x < Request.QueryString.Count; x++){
          string NextItem = "&" + Request.QueryString[x] + "=";
          if(InStr(0,MM_removeList,NextItem,1) == 0){
            MM_keepURL = MM_keepURL + NextItem + Server.UrlEncode(Request.QueryString[x]);
         }
        }

        // add the Form variables to the MM_keepForm string;
        for( int z = 0; z < Request.Form.Count; z++){
          string NextItem = "&" + Request.Form[z] + "=";
          if(InStr(0,MM_removeList,NextItem,1) == 0){
            MM_keepForm = MM_keepForm + NextItem + Server.UrlEncode(Request.Form[z]);
         }
        }

        // create the Form + URL string AND remove the intial 'AND' from each of the strings;
        MM_keepBoth = MM_keepURL + MM_keepForm;
        if(MM_keepBoth != ""){MM_keepBoth = Right(MM_keepBoth, Len(MM_keepBoth) - 1);}
        if(MM_keepURL != ""){MM_keepURL  = Right(MM_keepURL, Len(MM_keepURL) - 1);}
        if(MM_keepForm != ""){MM_keepForm = Right(MM_keepForm, Len(MM_keepForm) - 1);}


        if(cStr(Request["ID"]) != "0"){
          rsAttach = new DataReader("SELECT path, title FROM RebillAttachments WHERE RebillDetailId = " + Request["ID"] + "");
          rsAttach.Open();
          rsAttach_numRows = rsAttach.RecordCount;
        }




        }

        public string InsertTeamMembers(string TM,int FPDID){

              string Insert_Query = "";
              string outoftowntype = "";
 
              if(TM == "0"){
                // Employee task worked records will !be generated;
              }else{

                string[] arEmp;
                double nHours;

                arEmp = Split(TM, ",");
                nHours =  cDbl(Request["TotalHours"]) / (UBound(arEmp));

                for( int I=0; I < UBound(arEmp);I ++){

                     if (isOutofTown(arEmp[I], cStr(Session["FacilityID"]))){
                        outoftowntype = "O";
                     }else{
                        outoftowntype = "N";
                    }

                  Insert_Query = Insert_Query +  " Insert Into EmployeeTaskWorked ";
                  Insert_Query = Insert_Query +  "   (TaskID, RebillSubTaskID, FacilityID, EmployeeID, RebillDetailID, WorkDate, ShiftID,  OutOfTownType,  " ;
                  Insert_Query = Insert_Query +  "    HoursWorked, PayrollStatus, LastModifiedBy, LastModifiedOn, Notes ) ";
                  Insert_Query = Insert_Query +  "    VALUES ( " ;
                  Insert_Query = Insert_Query +  "      " + Request["allTasks"] + " , " + Request["select"] + " , " + cStr(Session["FacilityID"]) + " , " + cStr(arEmp[I]) + ",  " + cStr(FPDID)  + ", ";
                  Insert_Query = Insert_Query +  "     '" + Request["WorkDate"] + "','" + Request["selShift"] + "', '" + outoftowntype + "', "  +  cStr(nHours) + ", " ;
                  Insert_Query = Insert_Query +  "     'OPEN', '" +  Request["LastModifiedBy"] + "', '" + Request["LastModifiedOn"]  + "', 'Rebill Detail - Generated')" ;

                }
             }

              return Insert_Query;

       }

    }
}