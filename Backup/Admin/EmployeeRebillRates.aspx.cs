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


    public partial class EmployeeRebillRates : PageBase
    {

        public int Repeat1__numRows;
        public int Repeat1__index;
        public int rs_total = 0;
        public int rs_first = 0;
        public int rs_last = 0;

        public DataReader rs;
        public DataReader rsTasks;
        public DataReader rsEmployee;
        public DataReader MM_rs;
        public DataReader rsShifts;
        public int rsTasks_numRows;
        public int rs_numRows = 0;
        public int rsEmployee_numRows = 0;
        public int rsShifts_numRows = 0;
        public int MM_offset = 0;
        public bool MM_atTotal = false;
        public bool MM_paramIsDefined = false;
        public string MM_editAction = "";

        public string MM_keepURL = "";
        public string MM_keepForm = "";
        public string MM_keepBoth = "";
        public string MM_keepNone = "";
        public string MM_moveFirst = "";
        public string MM_moveLast = "";
        public string MM_moveNext = "";
        public string MM_movePrev = "";

        public string sDefShiftID = "";


        protected override void Page_Load(object sender, EventArgs e)
        {


            base.Page_Load(sender, e);

            GrantAccess("Super, Admin, User");


            sDefShiftID = Request["ShiftID"];

            if (sDefShiftID == null || sDefShiftID == "")
            {
               sDefShiftID = "1";
            }

            // *** Update Record: construct a sql update statement && execute it;


        if((cStr(Request["MM_update"]) != "") && (cStr(Request["MM_recordId"]) != "")) {

          //MM_editAction = cStr(Request["URL"]);
          MM_editAction = "EmployeeRebillRates.aspx";
          string MM_editRedirectUrl = "EmployeeRebillRates.aspx?ID=" + cStr(Request["MM_recordId"] + "&ShiftID=" + sDefShiftID);

 
          // create the sql update statement
            string MM_editQuery;
          if (Request["Action"] == "ADDNEWRATE") {
                MM_editQuery =                  " Insert Into EmployeeRebillRates " ;
                MM_editQuery = MM_editQuery   + "   (FacilityID, TaskID, ShiftID, EmployeeID," ;
                MM_editQuery = MM_editQuery   + "    UnitsPayRate, HoursPayRate, EffectiveDate, " ;
                MM_editQuery = MM_editQuery   + "    LastModifiedBy, LastModifiedOn ) " ;
                MM_editQuery = MM_editQuery   + "   VALUES (" ;
                MM_editQuery = MM_editQuery   + "   " + Session["FacilityID"]         + ", " ;
                MM_editQuery = MM_editQuery   + "   " + cStr(Request["newTask"])      + ", " ; 
                MM_editQuery = MM_editQuery   + "   " + sDefShiftID                   + ", " ; 
                MM_editQuery = MM_editQuery   + "   " + cStr(Request["MM_recordId"])  + ", " ; 

                if (Request["newPType"] == "EITHER") {
                  MM_editQuery = MM_editQuery + "   " + cStr(Request["newUR"])  + ", "  ;
                  MM_editQuery = MM_editQuery + "   " + cStr(Request["newHR"])  + ", "  ;
                }else{
                    if (Request["newPType"] == "UNITS") {
                       MM_editQuery = MM_editQuery + "   " +  cStr(Request["newUR"])  + ", " ; 
                       MM_editQuery = MM_editQuery + "   " + "0"                     + ", "  ;
                    }else{
                        if (Request["newPType"] == "HOURS") {
                          MM_editQuery = MM_editQuery + "   " + "0"                     + ", "  ;
                          MM_editQuery = MM_editQuery + "   " + cStr(Request["newHR"])  + ", "  ;
                        }
                    }
                }   

                MM_editQuery = MM_editQuery   + "  '" + cStr(Request["newEFD"])  + "', "  ;
                MM_editQuery = MM_editQuery   + "  '" + Session["UserName"]      + "', "  ;
                MM_editQuery = MM_editQuery   + "  '" + cStr(Date())             + "') "  ;
          }else{
            MM_editQuery = "";
          }

          int numRec = System.Convert.ToInt32(Request["NRec"]);
          if(numRec > 0){
            for(int I=1; I < numRec; I++){
                   if(Request["DELETE_" + cStr(I)] == "on"){
                     MM_editQuery = MM_editQuery + " Delete From EmployeeRebillRates Where ID = " + cStr(Request["RecID_" + cStr(I)]) + " AND ShiftID = " + sDefShiftID ;
                   }else{
                     MM_editQuery = MM_editQuery + " Update EmployeeRebillRates " ;
                         MM_editQuery = MM_editQuery + "     FacilityID = " + Session["FacilityID"]         + ", " ;
                         MM_editQuery = MM_editQuery + "            TaskID = " + cStr(Request["TID_"+cStr(I)]) + ", " ;
                         MM_editQuery = MM_editQuery + "           ShiftID = " + sDefShiftID                   + ", " ;
                         MM_editQuery = MM_editQuery + "        EmployeeID = " + cStr(Request["MM_recordId"])  + ", " ;
                         MM_editQuery = MM_editQuery + "      UnitsPayRate = " + cStr(Request["UR_"+cStr(I)])  + ", " ;
                         MM_editQuery = MM_editQuery + "      HoursPayRate = " + cStr(Request["HR_"+cStr(I)])  + ", " ;
                         MM_editQuery = MM_editQuery + "     EffectiveDate ='" + cStr(Request["EFD_"+cStr(I)]) + "'," ;
                         MM_editQuery = MM_editQuery + "    LastModifiedBy ='" + Session["UserName"]           + "' ";
                         MM_editQuery = MM_editQuery + "  Where ID = " + cStr(Request["RecID_"+cStr(I)]) + " AND ShiftID = " + sDefShiftID ;
                  }
                }
             //Response.Write(MM_editQuery;
          }else{
             //Response.Write("No Records...";
         }


          bool MM_abortEdit = false;
          if(!MM_abortEdit){
            // execute the update;
             this.Execute( MM_editQuery);
            if(MM_editRedirectUrl != ""){
              Response.Redirect(MM_editRedirectUrl);
            }
          }

     }
            

    string rs__MMColParam;
    rs__MMColParam = "2";
    if(Request.QueryString["ID"] != ""){rs__MMColParam = Request.QueryString["ID"];}

    rs = new DataReader("SELECT dbo.EmployeeRebillRates.ID,  dbo.RebillSubTasks.Description, dbo.EmployeeRebillRates.FacilityID, dbo.EmployeeRebillRates.TaskID, dbo.EmployeeRebillRates.EmployeeID,  dbo.EmployeeRebillRates.UnitsPayRate, dbo.EmployeeRebillRates.HoursPayRate, dbo.EmployeeRebillRates.EffectiveDate,   dbo.EmployeeRebillRates.LastModifiedBy, dbo.EmployeeRebillRates.LastModifiedOn,  dbo.Tasks.TaskCode, dbo.Tasks.PayType  FROM dbo.EmployeeRebillRates INNER JOIN  dbo.RebillSubTasks ON     dbo.EmployeeRebillRates.TaskID = dbo.RebillSubTasks.Id  Inner Join  dbo.Tasks ON     dbo.RebillSubTasks.TaskID = dbo.Tasks.Id    WHERE dbo.Tasks.Rebillable = 1  AND TaskCode <>'RB' AND dbo.EmployeeRebillRates.EmployeeID = " + Replace(rs__MMColParam, "'", "''")  + " AND dbo.EmployeeRebillRates.ShiftID = " + sDefShiftID + " AND dbo.EmployeeRebillRates.FacilityID = " + Session["FacilityID"] + "   ORDER BY TaskCode ASC, EffectiveDate DESC");
    rs.Open();
    rs_numRows = 0;


    string rsTasks__PFacID;
    rsTasks__PFacID = "3";
    if(Session["FacilityID"] != ""){rsTasks__PFacID = System.Convert.ToString(Session["FacilityID"]);}



    rsTasks = new DataReader("SELECT Id = 0, Description = 'Task', TaskCode = ' Task? ', PayType = 'EITHER' UNION SELECT  RebillSubTasks.Id, RebillSubTasks.Description, Tasks.TaskCode, PayType = 'EITHER' FROM  Tasks, FacilityTasks, RebillSubTasks, FacilityCustomer, Facility WHERE     Tasks.Rebillable = 1 AND TaskCode <> 'RB' AND Tasks.ID = RebillSubTasks.TaskID AND Tasks.Id = FacilityTasks.TaskId AND  RebillSubTasks.FacilityCustomerId = FacilityCustomer.Id AND FacilityCustomer.FacilityId = Facility.Id AND Facility.ID = " + Replace(rsTasks__PFacID, "'", "''") + " ORDER BY TaskCode ");
    rsTasks.Open();
    rsTasks_numRows = 0;


    string rsEmployee__PFACID;
    rsEmployee__PFACID = "11";
    if(Session["FacilityID"]  != ""){rsEmployee__PFACID = System.Convert.ToString(Session["FacilityID"]);}


    rsEmployee= new DataReader("SELECT Id,  Case when FacilityID <>  " + Session["FacilityID"] + " then 1 else 0 end as FromAssociatedFacility  , Case When (TempEmployee=0) Then EmployeeNumber Else TempNumber End As EmpNum, LastName, FirstName,  Case When (Salaried<>0) Then ' (Salaried) ' Else '' End As SalariedEmployee, Case When (TempEmployee=0) Then ' ' Else ' (Temp. Employee) ' End As EmpStatus  FROM dbo.Employee WHERE ( FacilityID = " + Replace(rsEmployee__PFACID, "'", "''") + "  OR  ( FacilityID IN ( Select AssociatedFacilityID from AssociatedFacility where FacilityID = " + rsEmployee__PFACID + ")) )    ORDER BY FromAssociatedFacility, LastName, FirstName");
    rsEmployee.Open();
    rsEmployee_numRows = 0;



    Repeat1__numRows = -1;
    Repeat1__index = 0;
    rs_numRows = Repeat1__numRows;

 
    //  *** Record Stats, Move To Record, && Go To Record: declare stats variables;

    //  the record count;
    rs_total = rs.RecordCount;

    //  the number of rows displayed on this page;
    if(rs_numRows < 0){
      rs_numRows = rs_total;
    }else{
        if(rs_numRows == 0){
          rs_numRows = 1;
        }
    }

    
   //  the first && last displayed record;
   rs_first = 1;
   rs_last  = rs_first + rs_numRows - 1;

   // if we have the correct record count, check the other stats;
   if(rs_total != -1){
     if(rs_first > rs_total){rs_first = rs_total;}
     if(rs_last > rs_total){rs_last = rs_total;}
     if (rs_numRows > rs_total) { rs_numRows = rs_total; }
   }

 
    
      // *** Record Stats: if we don//t know the record count, manually count them;

       if(rs_total == -1){

         // count the total records by iterating through the recordset;
         rs_total=0;
         while (!rs.EOF){
           rs.Read();
           rs_total = rs_total + 1;

         }

         // re the cursor to the beginning;
         rs.Requery();

         //  the number of rows displayed on this page;
         if(rs_numRows < 0 || rs_numRows > rs_total){
           rs_numRows = rs_total;
        }

         //  the first && last displayed record;
         rs_first = 1;
         rs_last = rs_first + rs_numRows - 1;
         if(rs_first > rs_total){rs_first = rs_total;}
         if(rs_last > rs_total){rs_last = rs_total;}

       }
         

       // *** Move To Record && Go To Record: declare variables;

        MM_rs    = rs;
       int MM_rsCount   = rs_total;
       int MM_size      = rs_numRows;
       string MM_uniqueCol = "";
       string MM_paramName = "";
       MM_offset = 0;
       MM_atTotal = false;
       MM_paramIsDefined = false;

       if(MM_paramName != ""){
         MM_paramIsDefined = (Request.QueryString[MM_paramName] != "");
       }

         
       // *** Move To Record: handle //index// || //offset// parameter;

       if(!MM_paramIsDefined && MM_rsCount != 0){

           // use index parameter if defined, otherwise use off parameter;
           string r = Request.QueryString["index"];
           if (r == null || r == "") { r = Request.QueryString["offset"]; }
           if( r != ""){MM_offset = System.Convert.ToInt32(r);}

           // if we have a record count, check if we are past the end of the recordset;
           if(MM_rsCount != -1){
           if(MM_offset >= MM_rsCount || MM_offset == -1){ // past end || move last;
               if((MM_rsCount%MM_size) > 0){ 
                   // last page !a full repeat region;
                   MM_offset = MM_rsCount - (MM_rsCount%MM_size);
               }else{
                   MM_offset = MM_rsCount - MM_size;
               }
           }
           }

           // move the cursor to the selected record;
           int i = 0;
           while ((!MM_rs.EOF) && (i < MM_offset || MM_offset == -1)){
               MM_rs.Read();
               i = i + 1;
           }
           if(MM_rs.EOF){MM_offset = i ;} //  MM_offset to the last possible record;

       }
         

       // *** Move To Record: if we dont know the record count, check the display range;

       if(MM_rsCount == -1){

             // walk to the end of the display range for( this page;
             int i = MM_offset;
             while (!MM_rs.EOF && (MM_size < 0 || i < MM_offset + MM_size)){
                 MM_rs.Read();
               i = i + 1;
             }

             // if we walked off the end of the recordset,  MM_rsCount && MM_size;
             if(MM_rs.EOF){
               MM_rsCount = i;
               if(MM_size < 0 || MM_size > MM_rsCount){MM_size = MM_rsCount;}
             }

             // if we walked off the end,  the off based on page size;
             if(MM_rs.EOF && !MM_paramIsDefined){
               if(MM_offset > MM_rsCount - MM_size || MM_offset == -1){
                 if((MM_rsCount%MM_size) > 0){
                   MM_offset = MM_rsCount - (MM_rsCount%MM_size);
                 }else{
                   MM_offset = MM_rsCount - MM_size;
                }
              }
            }

             // re the cursor to the beginning;
             MM_rs.Requery();

             // move the cursor to the selected record;
             i = 0;
             while (!MM_rs.EOF && i < MM_offset){
                 MM_rs.Read();
               i = i + 1;
              }

        }

       // *** Move To Record: update record stats;

       //  the first && last displayed record;
       rs_first = MM_offset + 1;
       rs_last  = MM_offset + MM_size;
       if(MM_rsCount != -1){
         if(rs_first > MM_rsCount){rs_first = MM_rsCount;}
         if(rs_last > MM_rsCount){rs_last = MM_rsCount;}
       }

       //  the boolean used by hide region to check if we are on the last record;
       MM_atTotal = (MM_rsCount != -1 && MM_offset + MM_size >= MM_rsCount);


       // *** Go To Record && Move To Record: create strings for( maintaining URL && Form parameters;

       // create the list of parameters which should !be maintained;
       string MM_removeList = "&index=";
       if(MM_paramName != ""){MM_removeList = MM_removeList + "&" + MM_paramName + "=";}
       MM_keepURL="";
       MM_keepForm="";
       MM_keepBoth="";
       MM_keepNone="";

       // add the URL parameters to the MM_keepURL string;
       for(int x = 0; x < Request.QueryString.Count; x++){
         string NextItem = "&" +  Request.QueryString[x] + "=";
         if (InStr(0, MM_removeList, NextItem, 1) == 0)
         {
           MM_keepURL = MM_keepURL + NextItem + Server.UrlEncode(Request.QueryString[x]);
        }
       }

       // add the Form variables to the MM_keepForm string;
       for( int x = 0; x < Request.Form.Count; x++){
         string NextItem = "&" + Request.Form[x] + "=";
         if (InStr(0, MM_removeList, NextItem, 1) == 0) {
           MM_keepForm = MM_keepForm + NextItem + Server.UrlEncode(Request.Form[x]);
         }
       }

       // create the Form + URL string && remove the intial //&// from each of the strings;
       MM_keepBoth = MM_keepURL + MM_keepForm;
       if(MM_keepBoth != ""){MM_keepBoth = Right(MM_keepBoth, Len(MM_keepBoth) - 1);}
       if(MM_keepURL != ""){MM_keepURL  = Right(MM_keepURL, Len(MM_keepURL) - 1);}
       if(MM_keepForm != ""){MM_keepForm = Right(MM_keepForm, Len(MM_keepForm) - 1);}


       // *** Move To Record:  the strings for( the first, last, next, && previous links;

       string MM_keepMove = MM_keepBoth;
       string MM_moveParam = "index";

       // if the page has a repeated region, remove //offset// from the maintained parameters;
       if(MM_size > 0){
         MM_moveParam = "offset";
         if(MM_keepMove != ""){
           string[] param = Split(MM_keepMove, "&");
           MM_keepMove = "";
           for(int i = 0 ; i < UBound(param);i++){
             string nextItem = Left(param[i], InStr(0,param[i],"=",1) - 1);
             if(StrComp(nextItem,MM_moveParam,1) != 0){
               MM_keepMove = MM_keepMove + "&" + param[i];
             }
           }
           if(MM_keepMove != ""){
             MM_keepMove = Right(MM_keepMove, Len(MM_keepMove) - 1);
           }
        }
       }

       //  the strings for( the move to links;
       if(MM_keepMove != ""){MM_keepMove = MM_keepMove + "&";}
       string urlStr = Request.ServerVariables["URL"] + "?" + MM_keepMove + MM_moveParam + "=";
       MM_moveFirst = urlStr + "0";
       MM_moveLast  = urlStr + "-1";
       MM_moveNext  = urlStr + System.Convert.ToString(MM_offset + MM_size);
       int prev = MM_offset - MM_size;
       if(prev < 0){prev = 0;}
       MM_movePrev  = urlStr + System.Convert.ToString(prev);


       rsShifts = new DataReader( "Select ID, Shift  From Shifts");
       rsShifts.Open();
       rsShifts_numRows = 0;


      }

    }
}