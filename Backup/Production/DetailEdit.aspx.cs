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

namespace InterrailPPRS.Production
{
    public partial class DetailEdit : PageBase
    {

        public string[] arRecords;
        public string[] arF;

        public DataReader localrs;
        public DataReader rsDef;
        public int rsDef_numRows = 0;

        public DataReader rsDef2;
        public int rsDef2_numRows = 0;

        public DataReader rs;
        public int rs_numRows = 0;

        public DataReader rsOrigin;
        public int rsOrigin_numRows = 0;

        public DataReader rsRailCarType;
        public int rsRailCarType_numRows = 0;

        public DataReader rsMan;
        public int rsMan_numRows = 0;


        public DataReader rsCustomer;
        public int rsCustomer_numRows = 0;

        public DataReader rsTask;
        public int rsTask_numRows = 0;

        public DataReader rsTotals;
        public int rsTotals_numRows = 0;

        public DataReader rsLast;
        public int rsLast_numRows = 0;

        public DataReader rsShifts;
        public int rsShifts_numRows = 0;

        public string MM_editRedirectUrl;

        public string FacilityCustID = "0";
        public string FacID = "0";
        public string TaskID = "0";

        public int Repeat1__numRows = 0;
        public int Repeat1__index = 0;

        public string MM_removeList = "";
        public string MM_paramName = "";
        public string MM_editAction = "";
        public string MM_keepURL=""; string MM_keepForm=""; string MM_keepBoth=""; string MM_keepNone="";

        public string sReturnTo = "";
        public string sCarTypeID = "";
        public string sDefLevel = "";
        public string sManID = "";
        public string sDefNU = "";
        public string sOriginID = "";
        public string sDefCustID = "";
        public string sDefShiftID = "";
        public string sDefTaskID = "";



        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);
            

            //** Save All;
            if(cStr(Request["SaveType"]) == "SaveAll"){

              arRecords = Split(Request["Data"], "|");
              string strSQL = "";

              for( int i = LBound(arRecords); i < UBound(arRecords); i ++){
                arF = Split(arRecords[i], "~");
                strSQL +=  " Insert FacilityProductionDetail " ;       // 3       4            5            6        7           8          9           10      11;
                strSQL +=  "(WorkDate, ShiftID, FacilityCustomerId, TaskId, OriginID, ManufacturerID, NewUsed, LevelType, CarTypeID, RailCarNumber, Units, Notes, LastModifiedOn, LastModifiedBy, FacilityID) "  ;
                strSQL +=  "VALUES " ;
                strSQL +=  "('" + arF[0] + "', '" + arF[1] + "', " + arF[2] + ", " + arF[3] + ", " + arF[4] + ", " + arF[5] + ", '" + arF[6] + "', '" + arF[7] + "', " + arF[8] + ", '" + arF[9] + "', " + arF[10] + ", '" + Replace(arF[11], "'", "''") + "', '" + System.DateTime.Now.ToShortDateString() + "', '" + Session["UserName"] + "', " + Session["FacilityID"] + " ) "   ;
                // Delete the "No Productiond data" for the day, if any;
                strSQL +=  " Delete NoProductionData Where WorkDate = '"  + arF[0] + "' AND FacilityID = " + cStr(Session["FacilityID"]) + "   ";

              } //Next



              this.Execute(strSQL);

              string lastTaskId = "";

              for( int i = LBound(arRecords); i < UBound(arRecords); i++){
                  arF = Split(arRecords[i], "~");
                 if (lastTaskId != arF[3]){
                    UpdateUPM ( arF[0], arF[3], arF[1], cStr(Session["FacilityID"]), 0);
                 }
                 lastTaskId  =  arF[3];
              } //End Next

            } //End If

            // *** Edit Operations: declare variables;

            sReturnTo = "&ReturnTo=%2FPPRS%2FProduction%2FDetailEdit%2Easpx%3FID%3D";

            if(cStr(Request["Delete"]) == "YES"){

                 localrs = new DataReader("Select WorkDate, ShiftID, TaskID from FacilityProductionDetail Where ID = " + Request["ID"] );
                 localrs.Open();
                 localrs.Read();
                 string delWorkDate = localrs.Item("WorkDate");
                 string delTaskID   = localrs.Item("TaskID");
                 string delShiftID  = localrs.Item("ShiftID");

                this.Execute("Delete FacilityProductionDetail Where ID = " + Request["ID"]);

                if(cStr(Request["ReturnTo"]) != ""){
                   MM_editRedirectUrl = Request["ReturnTo"];
                }else{
                  MM_editRedirectUrl = "Detail.aspx";
                }

                UpdateUPM (delWorkDate,  delTaskID,  delShiftID, cStr(Session["FacilityID"]) , 0);

                Response.Redirect(MM_editRedirectUrl);

            }

            MM_editAction = cStr(Request["URL"]);

            if(Request.QueryString.Count > 0){
              MM_editAction = MM_editAction + "?" + Request.QueryString;
            }

            // boolean to abort record edit;
            bool MM_abortEdit = false;

            // query string to execute;
            string MM_editQuery = "";


            // *** Update Record: variables;


              string MM_editTable = "dbo.FacilityProductionDetail";
              string MM_editColumn = "Id";
              string MM_recordId = "" + Request.Form["MM_recordId"] + "";
              string MM_fieldsStr  = "WorkDate|value|selShift|value|selCustomer|value|selTask|value|selOrigin|value|selManufacturer|value|selNewUsed|value|selLevel|value|selCarType|value|RailCarNumber|value|Units|value|Notes|value|LastModifiedOn|value|LastModifiedBy|value|FacilityID|value";
              string MM_columnsStr = "WorkDate|',none,NULL|ShiftID|none,none,NULL|FacilityCustomerId|none,none,NULL|TaskId|none,none,NULL|OriginID|none,none,NULL|ManufacturerID|none,none,NULL|NewUsed|',none,''|LevelType|',none,''|CarTypeID|none,none,NULL|RailCarNumber|',none,''|Units|none,none,NULL|Notes|',none,''|LastModifiedOn|',none,NULL|LastModifiedBy|',none,''|FacilityID|none,none,NULL";

              // create the MM_fields && MM_columns arrays;
              string[] MM_fields = Split(MM_fieldsStr, "|");
              string[] MM_columns = Split(MM_columnsStr, "|");

              // the form values;
              for(int i = LBound(MM_fields); i < UBound(MM_fields); i = i + 2){ 
                MM_fields[i+1] = cStr(Request.Form[MM_fields[i]]);
              }

              if(cStr(Request["ReturnTo"]) != ""){
                 MM_editRedirectUrl = Request["ReturnTo"];
              }else{
                 MM_editRedirectUrl = "" + Request["MM_recordId"] + "&WDate=" + Request["Wdate"];
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

               }// End For

               MM_editQuery = MM_editQuery + " where " + MM_editColumn + " = " + MM_recordId;

              if(!MM_abortEdit){
                // execute the update;

                this.Execute(MM_editQuery + " Delete NoProductionData Where WorkDate = '"  + Request["WDate"] + "' AND FacilityID = " + cStr(Session["FacilityID"]));

                if(cStr(Request["ReturnTo"]) != ""){
                  MM_editRedirectUrl = Request["ReturnTo"];
                }else{
                  MM_editRedirectUrl = "DetailEdit.aspx?ID=" + Request["MM_recordId"] + "&WDate=" + Request["Wdate"];
                }

                    UpdateUPM (cStr(Request["WorkDate"]),  cStr(Request["selTask"]),  cStr(Request["selShift"]), cStr(Session["FacilityID"]), 0);


                Response.Redirect(MM_editRedirectUrl);
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

              MM_editQuery = "insert into " + MM_editTable + " (" + MM_tableValues + ") values (" + MM_dbValues + ")";

              //if (in NoProductionData){remove;
              MM_editQuery = MM_editQuery + " Delete NoProductionData Where WorkDate = '"  + Request["WDate"] + "' AND FacilityID = " + cStr(Session["FacilityID"]);

              if(!MM_abortEdit){
                // execute the insert;
               this.Execute( MM_editQuery);

                MM_editRedirectUrl = "DetailEdit.aspx?ID=" + Request["MM_recordId"];

                UpdateUPM (cStr(Request["WorkDate"]),  cStr(Request["selTask"]),  cStr(Request["selShift"]), cStr(Session["FacilityID"]), 0);

                Response.Redirect("DetailEdit.aspx?ID=0" + "&WDate=" + Request["WDate"] );

             }

            }
            }


            string  rsDef__PFacilityID;
            rsDef__PFacilityID = "1";
            if(cStr(Session["FacilityID"]) != ""){rsDef__PFacilityID = System.Convert.ToString(Session["FacilityID"]);}


            string rsDef__PDate;
            rsDef__PDate = "11/11/2001";
            if(cStr(Request["WDate"]) != ""){rsDef__PDate = cStr(Request["WDate"]);}



            rsDef = new DataReader("SELECT Top 1 *  FROM dbo.FacilityProductionDetail  WHERE FacilityID=" + Replace(rsDef__PFacilityID, "'", "''") + " AND WorkDate='" + Replace(rsDef__PDate, "'", "''") + "'  Order By WorkDate Desc, ID Desc");
            rsDef.Open();
            rsDef_numRows = 0;

              if (!rsDef.Read()){

                 rsDef2 = new DataReader("SELECT dbo.FacilityCustomer.Id AS CustID, dbo.Facility.Id, dbo.Facility.DefaultTaskID FROM dbo.Facility LEFT OUTER JOIN dbo.FacilityCustomer ON dbo.Facility.Id = dbo.FacilityCustomer.FacilityId WHERE     (dbo.FacilityCustomer.DefaultCustomer = 1) AND Facility.ID=" + Replace(rsDef__PFacilityID, "'", "''") + " ");
                 rsDef2.Open();
                 rsDef2_numRows = 0;
                 rsDef2.Read();

                 if( !rsDef2.LastReadSuccess){
                    FacilityCustID = "0";
                    FacID = "0";
                    TaskID = "0";
                 }else{

                   FacilityCustID = cStr(rsDef2.Item("CustID"));
                   FacID = rsDef2.Item("ID");
                   TaskID = rsDef2.Item("DefaultTaskID");
                }

              }else{

                
                FacilityCustID = cStr(rsDef.Item("FacilityCustomerID"));
                FacID = rsDef.Item("FacilityID");
                TaskID = rsDef.Item("TaskID");

             }


            string rs__MMColParam;
            rs__MMColParam = "19";
            if(Request.QueryString["Id"]  != ""){rs__MMColParam = Request.QueryString["Id"];}


            if(cStr(Request["ID"]) == "0"){

              rs = new DataReader( "SELECT Id, WorkDate, ShiftID, RailCarNumber, ManufacturerID, Units, OriginID, Notes, CarTypeID, FacilityCustomerId, FacilityID, TaskId, LastModifiedBy,  LevelType, NewUsed, LastModifiedOn, ApprovalStatus  FROM dbo.FacilityProductionDetail  WHERE Id = -1");
              rs.Open();


            }else{

              rs = new DataReader( "SELECT Id, WorkDate, ShiftID, RailCarNumber, ManufacturerID, Units, OriginID, Notes, CarTypeID, FacilityCustomerId, FacilityID, TaskId, LastModifiedBy,  LevelType, NewUsed, LastModifiedOn, ApprovalStatus  FROM dbo.FacilityProductionDetail  WHERE Id = " + Replace(rs__MMColParam, "'", "''") + "");
              rs.Open();
              rs_numRows = 0;

              if( !rs.Read()) {
                FacilityCustID = "0";
                FacID = "0";
                TaskID = "0";
              }else{
                
                FacilityCustID = rs.Item("FacilityCustomerID");
                FacID = rs.Item("FacilityID");
                TaskID = rs.Item("TaskID");
             }
            }



            rsOrigin = new DataReader( "SELECT ID, OriginCode, OriginName  FROM dbo.IRGOrigin  ORDER BY OriginName  ASC");
            rsOrigin.Open();
            rsOrigin_numRows = 0;



            rsRailCarType = new DataReader( "SELECT ID, CarTypeCode, CarTypeDescription  FROM dbo.IRGRailCarType  ORDER BY SortOrder, CarTypeCode ASC");
            rsRailCarType.Open();
            rsRailCarType_numRows = 0;



            rsMan = new DataReader( "SELECT ID, ManufacturerCode, ManufacturerName  FROM dbo.IRGManufacturer  ORDER BY ManufacturerName  ASC");
            rsMan.Open();
            rsMan_numRows = 0;


            string rsCustomer__PFacilityID;
            rsCustomer__PFacilityID = "5";
            if(Session["FacilityID"]  != ""){rsCustomer__PFacilityID = System.Convert.ToString(Session["FacilityID"]);}


            string rsCustomer__CustID;
            rsCustomer__CustID = "0";
            if(FacilityCustID != ""){rsCustomer__CustID = FacilityCustID;}


            rsCustomer = new DataReader("SELECT Id, CustomerCode, CustomerName  FROM dbo.FacilityCustomer  WHERE ( dbo.FacilityCustomer.FacilityId = '" + Replace(rsCustomer__PFacilityID, "'", "''") + "' AND FacilityCustomer.Active=1)    OR  ID=" + Replace(rsCustomer__CustID, "'", "''") + "  ORDER BY CustomerName ASC");
            rsCustomer.Open();
            rsCustomer_numRows = 0;


            string rsTask__PFacilityID;
            rsTask__PFacilityID = "3";
            if(cStr(Session["FacilityID"])  != ""){rsTask__PFacilityID = System.Convert.ToString(Session["FacilityID"]);}


            string rsTask__PTaskID;
            rsTask__PTaskID = "2";
            if(TaskID  != ""){rsTask__PTaskID = TaskID;}


            string rsTask__PFacID;
            rsTask__PFacID = "3";
            if(FacID != ""){rsTask__PFacID = FacID;}



            rsTask = new DataReader("SELECT dbo.Tasks.Id, dbo.FacilityTasks.TaskId, dbo.FacilityTasks.FacilityID, dbo.Tasks.TaskCode, dbo.Tasks.TaskDescription  FROM dbo.FacilityTasks INNER JOIN                        dbo.Tasks ON dbo.FacilityTasks.TaskId = dbo.Tasks.Id  WHERE (dbo.FacilityTasks.FacilityID = " + Replace(rsTask__PFacilityID, "'", "''") + " AND Tasks.Rebillable=0 AND Tasks.Active=1) OR (dbo.FacilityTasks.TaskId=" + Replace(rsTask__PTaskID, "'", "''") + " AND dbo.FacilityTasks.FacilityID = " + Replace(rsTask__PFacID, "'", "''") + "  AND Tasks.Rebillable=0)  ORDER BY TaskDescription ASC");
            rsTask.Open();
            rsTask_numRows = 0;


            string rsLast__PFacilityID;
            rsLast__PFacilityID = "2";
            if(Session["FacilityID"] != ""){rsLast__PFacilityID = System.Convert.ToString(Session["FacilityID"]);}


            string rsLast__PDate;
            rsLast__PDate = "11/05/2001";
            if(cStr(Request["WDate"])  != ""){rsLast__PDate = cStr(Request["WDate"]);}

            rsLast = new DataReader( "SELECT dbo.FacilityProductionDetail.Id, dbo.FacilityProductionDetail.WorkDate, dbo.Tasks.TaskCode, dbo.Tasks.TaskDescription,    ApprovalStatus, RailCarNumber,                     dbo.FacilityProductionDetail.Units, dbo.IRGManufacturer.ManufacturerName, dbo.IRGManufacturer.ManufacturerCode,                         dbo.IRGRailCarType.CarTypeDescription, dbo.IRGRailCarType.CarTypeCode, dbo.IRGOrigin.OriginName, dbo.IRGOrigin.OriginCode,                         dbo.FacilityProductionDetail.NewUsed, dbo.FacilityProductionDetail.LevelType  FROM dbo.FacilityProductionDetail INNER JOIN                        dbo.Tasks ON dbo.FacilityProductionDetail.TaskId = dbo.Tasks.Id INNER JOIN dbo.IRGManufacturer ON dbo.FacilityProductionDetail.ManufacturerID = dbo.IRGManufacturer.ID INNER JOIN                        dbo.IRGRailCarType ON dbo.FacilityProductionDetail.CarTypeID = dbo.IRGRailCarType.ID INNER JOIN                        dbo.IRGOrigin ON dbo.FacilityProductionDetail.OriginID = dbo.IRGOrigin.ID  WHERE (dbo.FacilityProductionDetail.FacilityID =" + Replace(rsLast__PFacilityID, "'", "''") + ")   AND (WorkDate='" + Replace(rsLast__PDate, "'", "''") + "')  ORDER BY  dbo.FacilityProductionDetail.Id  DESC, RailCarNumber, dbo.Tasks.TaskCode ");
            rsLast.Open();
            rsLast_numRows = 0;


            string rsTotals__PFacilityID;
            rsTotals__PFacilityID = "2";
            if(Session["FacilityID"] != ""){rsTotals__PFacilityID = System.Convert.ToString(Session["FacilityID"]);}


            string rsTotals__PDate;
            rsTotals__PDate = "11/5/2001";
            if(cStr(Request["WDate"])  != ""){rsTotals__PDate = cStr(Request["WDate"]);}


            rsTotals = new DataReader("SELECT Tasks.TaskCode,  Tasks.TaskDescription, COUNT(*) AS NRec, SUM(dbo.FacilityProductionDetail.Units) AS TotalUnits  FROM dbo.FacilityProductionDetail INNER JOIN                        dbo.Tasks ON dbo.FacilityProductionDetail.TaskId = dbo.Tasks.Id RIGHT OUTER JOIN                        dbo.FacilityTasks ON dbo.FacilityProductionDetail.FacilityID = dbo.FacilityTasks.FacilityID AND dbo.Tasks.Id = dbo.FacilityTasks.TaskId  WHERE (dbo.FacilityProductionDetail.FacilityID = " + Replace(rsTotals__PFacilityID, "'", "''") + ") AND (dbo.FacilityProductionDetail.WorkDate = '" + Replace(rsTotals__PDate, "'", "''") + "')  GROUP BY dbo.Tasks.TaskCode, dbo.Tasks.TaskDescription");
            rsTotals.Open();
            rsTotals_numRows = 0;


            rsShifts = new DataReader("Select ID, Shift  From Shifts");
            rsShifts.Open();
            rsShifts_numRows = 0;



            Repeat1__numRows = -1;
            Repeat1__index = 0;
            rsLast_numRows = rsLast_numRows + Repeat1__numRows;


            // *** Go To Record and Move To Record: create strings for maintaining URL and Form parameters;

            // create the list of parameters which should !be maintained;
            MM_removeList = "&index=";
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

            // create the Form + URL string && remove the intial //&// from each of the strings;
            MM_keepBoth = MM_keepURL + MM_keepForm;
            if(MM_keepBoth != ""){MM_keepBoth = Right(MM_keepBoth, Len(MM_keepBoth) - 1);}
            if(MM_keepURL != ""){MM_keepURL  = Right(MM_keepURL, Len(MM_keepURL) - 1);}
            if(MM_keepForm != ""){MM_keepForm = Right(MM_keepForm, Len(MM_keepForm) - 1);}



}

        public string ShowTotals(){

          string sHTML;

          sHTML = "";

          if(rsTotals.EOF){
              sHTML = sHTML + "<div class='required'>No totals found for:&nbsp;&nbsp;" + cStr(System.DateTime.Today.ToString("MM/dd/yyyy")) + "</div>";
          }else{

                sHTML = "<b><font color='green'><u>Saved Totals</u>:&nbsp;&nbsp;</font></b>";
            while (!rsTotals.EOF){
                rsTotals.Read();
                  sHTML = sHTML + Trim(rsTotals.Item("TaskCode")) + ":&nbsp;" + rsTotals.Item("NRec") + "&nbsp;(" + rsTotals.Item("TotalUnits") + ").  ";

            }//End Loop

                sHTML = sHTML + "<br>";
         }
          return sHTML;
        }

        public string ShowDetailForTheDay (){

          string sHTML;
          sHTML = "";

          if(rsLast.EOF){
            sHTML = sHTML + "<table border='0' width='100%'>" ;
            sHTML = sHTML + "<tr>" ;
            sHTML = sHTML + "  <td class='cellTopBottomBorder'><div  class='smallerText'>Task</div></td>" ;
            sHTML = sHTML + "  <td class='cellTopBottomBorder'><div  class='smallerText'>Man.</div></td>" ;
            sHTML = sHTML + "  <td class='cellTopBottomBorder'><div  class='smallerText'>N/U</div></td>" ;
            sHTML = sHTML + "  <td class='cellTopBottomBorder'><div  class='smallerText'>RailCar</div></td>" ;
            sHTML = sHTML + "  <td class='cellTopBottomBorder'><div  class='smallerText'>L</div></td>" ;
            sHTML = sHTML + "  <td class='cellTopBottomBorder'><div  class='smallerText'>Units</div></td>" ;
            sHTML = sHTML + "  <td class='cellTopBottomBorder'><div  class='smallerText'>Origin</div></td>" ;
            sHTML = sHTML + "</tr>" ;
            sHTML = sHTML + "<tr><td colspan=7><div id='divUnsaved'>No records found for: " + System.DateTime.Now.ToShortDateString() +  "</div></td></tr>" ;
            sHTML = sHTML + "</table>" ;

          }else{
            sHTML = sHTML + "<table border='0' width='100%'>" ;
            sHTML = sHTML + "<tr>" ;
            sHTML = sHTML + "  <td class='cellTopBottomBorder'><div  class='smallerText'>Task</div></td>" ;
            sHTML = sHTML + "  <td class='cellTopBottomBorder'><div  class='smallerText'>Man.</div></td>" ;
            sHTML = sHTML + "  <td class='cellTopBottomBorder'><div  class='smallerText'>N/U</div></td>" ;
            sHTML = sHTML + "  <td class='cellTopBottomBorder'><div  class='smallerText'>RailCar</div></td>" ;
            sHTML = sHTML + "  <td class='cellTopBottomBorder'><div  class='smallerText'>L</div></td>" ;
            sHTML = sHTML + "  <td class='cellTopBottomBorder'><div  class='smallerText'>Units</div></td>" ;
            sHTML = sHTML + "  <td class='cellTopBottomBorder'><div  class='smallerText'>Origin</div></td>" ;
            sHTML = sHTML + "</tr>" ;
            sHTML = sHTML + "<tr><td colspan=7><div id='divUnsaved'></div></td></tr>" ;

           int iRow = 0;
           string lc;
           string sHRef;
           string sReturnTo = "";

           string RailCarList = "";

            while (rsLast.Read()){
               

                  iRow = iRow+1;

                  if( (iRow % 2) == 0){
                    lc = "reportEvenLine";
                  }else{
                    lc = "reportOddLine";
                  }
                  if(Trim(rsLast.Item("ApprovalStatus")) == "OPEN"){
                    sHRef="DetailEdit.aspx?ID=" + cStr(rsLast.Item("ID")) + "&WDate=" + Request["WDate"];
                  }else{
                    sHRef="ApproveProductionDataEdit.aspx?ID=" + cStr(rsLast.Item("ID")) + "&WDate=" + Request["WDate"] + sReturnTo + Request["ID"];
                  }

                  if(iRow != 1){
                    if( InStr(0,RailCarList, rsLast.Item("TaskCode") + rsLast.Item("CarTypeCode") + rsLast.Item("RailCarNumber"),0) > 0 ){
                          lc="reportVeryOddLine";
                       }
                  }

                  sHTML = sHTML + "<tr class='" + lc + "'>" ;
                  sHTML = sHTML + "  <td><div  class='smallerText'>" + rsLast.Item("TaskCode")  + "</div></td>" ;
                  sHTML = sHTML + "  <td><div  class='smallerText'>" + rsLast.Item("ManufacturerCode") + "</div></td>" ;
                  sHTML = sHTML + "  <td align='center'><div  class='smallerText'>" + rsLast.Item("NewUsed") + "</div></td>" ;
                  sHTML = sHTML + "  <td><div  class='smallerText'><a href='" + sHRef + "'>" + rsLast.Item("CarTypeCode")  + "-" + rsLast.Item("RailCarNumber")  + "</a></div></td>" ;
                  sHTML = sHTML + "  <td align='center'><div  class='smallerText'>" + rsLast.Item("LevelType")  + "</div></td>" ;
                  sHTML = sHTML + "  <td align='right'><div  class='smallerText'>" + rsLast.Item("Units")   + "</div></td>" ;
                  sHTML = sHTML + "  <td><div  class='smallerText'>" + rsLast.Item("OriginCode")  + "</div></td>" ;
                  sHTML = sHTML + "</tr>" ;

                   RailCarList = RailCarList +  rsLast.Item("TaskCode") + rsLast.Item("CarTypeCode") + rsLast.Item("RailCarNumber") ;

            } //End Loop
            sHTML = sHTML + "</table>" ;
         }
          return sHTML;
        }

        public string FormatTheDate(DateTime inDate){

          /*string smDate, sdDate, syDate;

          smDate = Right(  cStr(inDate.AddMonths(100)), 2);
          sdDate = Right(cStr(inDate.AddDays(100)), 2);
          syDate = cStr(inDate.Year);

          return ( smDate + "/" + sdDate + "/" + syDate);*/
            return inDate.ToString("MM/dd/yyyy");

        }

    }
}