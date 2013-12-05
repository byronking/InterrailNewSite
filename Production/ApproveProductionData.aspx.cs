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
    public partial class ApproveProductionData : PageBase
    {

        public string MM_moveFirst = "";
        public string MM_moveLast = "";
        public string MM_moveNext = "";
        public string MM_movePrev = "";
        public int MM_offset = 0;
        public bool MM_atTotal = false;
        public string MM_keepBoth = ""; 
        protected int MM_size = 0;

        public DataReader MM_rs;
        public int MM_rsCount;
        public string MM_uniqueCol = "";
        public string MM_paramName = "";
        public bool MM_paramIsDefined = false;

        public DataReader rsProdRange;
        public int rsProdRange_numRows = 0;

        public DataReader rsProdDates;
        public int rsProdDates_numRows = 0;

        public DataReader rsNoProdDates;
        public int rsNoProdDates_numRows = 0;

        public DataReader rs;
        public int rs_numRows = 0;
 
        public DataReader rsAllDates;
        public int rsAllDates_numRows = 0;

        public DataReader rsDates;
        public int rsDates_numRows = 0;

        public DataReader rst;
        
        public int Repeat1__index;
        public int Repeat1__numRows;
        public string rowColor = "";
        public int rs_first;
        public int rs_last;
        public int rs_total;

        public bool ApproveAll = false;
        public string sDType = "";
        public string rs__PDate = "";
        public string rs__PFacilityID = "";

        public string sDuplicates = "";
        public string sNotIn = "";


        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin, User, Production");



            string rsProdRange__PFACID;
            rsProdRange__PFACID = "2";
            if(Session["FacilityID"] != null && cStr(Session["FacilityID"])  != ""){rsProdRange__PFACID = System.Convert.ToString(Session["FacilityID"]) ;}


            rsProdRange = new DataReader("SELECT Min(WorkDate) AS MinDate, Max(WorkDate) AS MaxDate  FROM FacilityProductionDetail  WHERE FacilityID = " + Replace(rsProdRange__PFACID, "'", "''") + "");
            rsProdRange.Open();
            rsProdRange_numRows = 0;

            string sMinDate = System.DateTime.Now.ToShortDateString();
            string sMaxDate = System.DateTime.Now.ToShortDateString();

            if (! rsProdRange.EOF){
               rsProdRange.Read();
              if(rsProdRange.Item("MinDate") != ""){
                sMinDate = cStr(rsProdRange.Item("MinDate"));
                sMaxDate = cStr(rsProdRange.Item("MaxDate"));
             }
            }



            string rsProdDates__PFACID;
            rsProdDates__PFACID = "2";
            if(Session["FacilityID"] != null && cStr(Session["FacilityID"]) != "" ){rsProdDates__PFACID = System.Convert.ToString(Session["FacilityID"]);}



            rsProdDates = new DataReader( "SELECT Distinct  WorkDate, CONVERT(Char(10), WorkDate, 101) AS WDate  FROM FacilityProductionDetail  WHERE FacilityID = " + Replace(rsProdDates__PFACID, "'", "''") + "");
            rsProdDates.Open();
            rsProdDates_numRows = 0;

            sNotIn = "";

            if (! rsProdDates.EOF){
              while ( !rsProdDates.EOF) {
                    rsProdDates.Read(); 
                    sNotIn = sNotIn + "|" + cStr(rsProdDates.Item("WorkDate")) + "|" + cStr(rsProdDates.Item("WDate"));

              }
              sNotIn  = sNotIn + "|";
              rsProdDates.Requery();
            }


            string rsNoProdDates__PFACID;
            rsNoProdDates__PFACID = "2";
            if( Session["FacilityID"] != null && cStr(Session["FacilityID"]) != ""){rsNoProdDates__PFACID = System.Convert.ToString(Session["FacilityID"]);}


            rsNoProdDates = new DataReader( "SELECT WorkDate, ApprovalStatus,  CONVERT(Char(10), WorkDate, 101) AS WDate  FROM NoProductionData  WHERE FacilityID = " + Replace(rsNoProdDates__PFACID, "'", "''") + "  ORDER BY WorkDate");
            rsNoProdDates.Open();
            rsNoProdDates_numRows = 0;

            if(! rsNoProdDates.EOF){
              while(!rsNoProdDates.EOF){
                    rsNoProdDates.Read();
                    sNotIn = sNotIn + "|" + cStr(rsNoProdDates.Item("WorkDate")) + "|" + cStr(rsNoProdDates.Item("WDate"));

              }
              sNotIn = sNotIn + "|";
              rsNoProdDates.Requery();
            }



          string strSQL;
          if(cStr(Request["Generate"]) == "NEW"){

            string sFrom = Request["From"];
            string sTo   = Request["To"];
            int nDays = (cDate(sTo) - cDate(sFrom)).Days;
            nDays++;
            strSQL = "";

            for( int I=0 ; I < nDays; I ++){
              string sDate = cStr(cDate(sFrom).AddDays(I));
              int? iPos = InStr(0,sNotIn, "|" + sDate + "|",0);
              if(iPos > 0){
                //Response.Write("Date found = " + sDate + "<br>";
              }else{
                strSQL +=  " Insert Into NoProductionData (WorkDate, FacilityID, ApprovalStatus, ";
                strSQL +=  "                              LastModifiedBy, LastModifiedOn)       ";
                strSQL +=  "        Values ('" + sDate               + "', " + cStr(Session["FacilityID"]) + ", 'OPEN', ";
                strSQL +=  "                '" + Session["UserName"]  + "', '" + cStr(System.DateTime.Now) + "') ";
             }
            }//End For

             if(strSQL != ""){
                this.Execute(strSQL);
             }

             Response.Redirect("ApproveProductionData.aspx");

          }else{

          }


            if(cStr(Request["Approval"]) != ""){
              string sApprovalStatus = UCase(Trim(Request["Approval"])); 
  
               string sWhere = "";

              if(sApprovalStatus == "FACILITY"){
                sWhere = " WHERE WorkDate = '" + cStr(Request["forDate"]) + "' AND ApprovalStatus='OPEN' AND FacilityID = " + cStr(Session["FacilityID"]);
              }else{
                sWhere = " WHERE WorkDate = '" + cStr(Request["forDate"]) + "' AND FacilityID = " + cStr(Session["FacilityID"]);
             }

              if(cStr(Request["Which"]) == "NOPROD"){
                  strSQL = "  Update NoProductionData Set ";
                strSQL +=  "  ApprovalStatus = '" + sApprovalStatus     + "',";
                strSQL +=  "      LastModifiedOn = '" + cStr(System.DateTime.Now.ToShortDateString())           + "',";
                strSQL +=  "      LastModifiedBy = '" + Session["UserName"] + "' ";
                strSQL = strSQL + sWhere;
              }else{ 
                if(cStr(Request["Which"]) == "CHECKED"){
                  if(cStr(Request["cbxApprove"]) != ""){
                    sWhere = sWhere + " AND TaskID IN (0, " + Request["cbxApprove"] + ")";
                  }else{
                    sWhere = sWhere + " AND TaskID IN (0) ";
                  }
               }

                strSQL = "  Update FacilityProductionDetail Set ";
                strSQL +=  "  ApprovalStatus = '" + sApprovalStatus     + "',";
                strSQL +=  "      LastModifiedOn = '" + cStr(System.DateTime.Now.ToShortDateString())  + "',";
                strSQL +=  "      LastModifiedBy = '" + Session["UserName"] + "' ";
                strSQL = strSQL + sWhere;
             }
  

              this.Execute(strSQL);

              Response.Redirect("ApproveProductionData.aspx?forDate=" + cStr(Request["forDate"]) + "&DType=" + Request["DType"]);

            }


            if(cStr(Request["OpenTask"]) != ""){
              string sTaskID = Request["OpenTask"];
              string sWhere = " WHERE TaskID = " + sTaskID + " AND WorkDate = '" + cStr(Request["WDate"]) + "'  AND FacilityID = " + Session["FacilityID"];
   
              strSQL =   "  Update FacilityProductionDetail Set ";
              strSQL +=  "  ApprovalStatus =  'OPEN', ";
              strSQL +=  "      LastModifiedOn = '" + cStr(System.DateTime.Now.ToShortDateString())           + "',";
              strSQL +=  "      LastModifiedBy = '" + Session["UserName"] + "' ";
              strSQL = strSQL + sWhere;


              this.Execute( strSQL);

              Response.Redirect("ApproveProductionData.aspx?forDate=" + cStr(Request["WDate"]) + "&DType=" + Request["DType"] );

            }
    
            if(cStr(Request["OpenNoProdData"]) != ""){

                string sWhere = " WHERE WorkDate = '" + cStr(Request["forDate"]) + "'  AND FacilityID = " + cStr(Session["FacilityID"]);

                strSQL = " Update NoProductionData Set ";
                strSQL +=  "  ApprovalStatus = '" + "OPEN"              + "',";
                strSQL +=  "      LastModifiedOn = '" + cStr(System.DateTime.Now.ToShortDateString())  + "',";
                strSQL +=  "      LastModifiedBy = '" + cStr(Session["UserName"]) + "' ";
                strSQL = strSQL + sWhere;


               this.Execute(strSQL);


              Response.Redirect("ApproveProductionData.aspx?forDate=" + cStr(Request["forDate"]) + "&DType=NOPROD" );
            }



            
            rs__PFacilityID = "2";
            if(Session["FacilityID"] != null && cStr(Session["FacilityID"]) != ""){rs__PFacilityID = cStr(Session["FacilityID"]); }


            
            rs__PDate = "1/1/2001";
            if(cStr(Request["forDate"]) != ""){
                rs__PDate = Request["forDate"]  ;
            }else{
                rs__PDate = sMaxDate;
                //rs__PDate = cDate(System.DateTime.Now);
            }



            sDType = UCase(cStr(Request["DType"]));

            if(sDType == "NOPROD"){
              strSQL = " ";
              strSQL +=  " SELECT WorkDate, FacilityID, ApprovalStatus  "  ;
              strSQL +=  "  FROM NoProductionData                      "  ;
              strSQL +=  " WHERE FacilityID = " + cStr(Session["FacilityID"]);
              strSQL +=  "   AND WorkDate = '" + cStr(Request["forDate"]) + "'";
            }else{
              strSQL = "";
              strSQL +=  "  SELECT TaskCode, FacilityProductionDetail.TaskID, COUNT(*) AS NRec,     ";
              strSQL +=  "              SUM(FacilityProductionDetail.Units) AS TU, ApprovalStatus        ";
              strSQL +=  "         FROM FacilityProductionDetail INNER JOIN Tasks ON                     ";
              strSQL +=  "              TaskId = Tasks.Id RIGHT OUTER JOIN FacilityTasks ON              ";
              strSQL +=  "              FacilityProductionDetail.FacilityID = FacilityTasks.FacilityID   ";
              strSQL +=  "              AND Tasks.Id = FacilityTasks.TaskId                              ";
              strSQL +=  "        WHERE FacilityProductionDetail.FacilityID = " + cStr(rs__PFacilityID);
              strSQL +=  "              AND FacilityProductionDetail.WorkDate = '" + cStr(rs__PDate) + "'";
              strSQL +=  "     GROUP BY TaskCode, FacilityProductionDetail.TaskID, ApprovalStatus        ";
              strSQL +=  "     ORDER BY TaskCode, ApprovalStatus Desc                                    ";
              
            }

            rs = new DataReader(strSQL);
            rs.Open();
            rs_numRows = 0;

            if(! rs.EOF){
              if(sDType == "NOPROD"){

              }else{

               ApproveAll = true;
               while ( !rs.EOF){
                  rs.Read();

                  strSQL = "     SELECT Distinct FacilityProductionDetail.TaskID, COUNT(*) AS NRec       ";
                  strSQL +=  "              FROM FacilityProductionDetail                                    ";
                  strSQL +=  "              INNER JOIN Tasks ON FacilityProductionDetail.TaskId = Tasks.Id   ";
                  strSQL +=  "              RIGHT OUTER JOIN FacilityTasks ON                                ";
                  strSQL +=  "              FacilityProductionDetail.FacilityID = FacilityTasks.FacilityID   ";
                  strSQL +=  "              AND Tasks.Id = FacilityTasks.TaskId                              ";
                  strSQL +=  "        WHERE FacilityProductionDetail.FacilityID = " + cStr(rs__PFacilityID);
                  strSQL +=  "              AND FacilityProductionDetail.WorkDate = '" + cStr(rs__PDate) + "'";
                  strSQL +=  "              AND FacilityProductionDetail.TaskId = " + rs.Item("TaskId");
                  strSQL +=  "     GROUP BY TaskCode, FacilityProductionDetail.TaskID, CarTypeId, RailCarNumber  ";
                  strSQL +=  "     ORDER BY NRec Desc                                                        ";

                  rst = new DataReader(strSQL);
                  rst.Open();
                  rst.Read();

                    if(cInt(rst.Item("Nrec")) > 1){
                      ApproveAll = false;
                    }
               }
              rs.Requery();
             }
            }



            string rsDates__PFacilityID;
            rsDates__PFacilityID = "2";
            if(cStr(Session["FacilityID"]) != ""){rsDates__PFacilityID = System.Convert.ToString(Session["FacilityID"]);};



            rsDates = new DataReader("SELECT CONVERT(Char(10), dbo.FacilityProductionDetail.WorkDate, 101) AS WDate  FROM dbo.FacilityProductionDetail   WHERE (dbo.FacilityProductionDetail.FacilityID = " + Replace(rsDates__PFacilityID, "'", "''") + ")  GROUP BY dbo.FacilityProductionDetail.WorkDate  UNION  SELECT     Wdate = CONVERT(Char(10), getDate(), 101)  ORDER BY WDate Desc");
            rsDates.Open();
            rsDates_numRows = 0;


            string rsAllDates__PFacilityID;
            rsAllDates__PFacilityID = "2";
            if(cStr(Session["FacilityID"]) != ""){rsAllDates__PFacilityID = System.Convert.ToString(Session["FacilityID"]);}


            rsAllDates = new DataReader( "SELECT Distinct CONVERT(Char(10),WorkDate, 101) AS WDate, WorkDate, DType='PROD', DStatus='ALL'  FROM FacilityProductionDetail  WHERE (FacilityID = " + Replace(rsAllDates__PFacilityID, "'", "''") + ")    UNION    SELECT Distinct CONVERT(Char(10),WorkDate, 101) AS WDate, WorkDate, DType='NOPROD', DStatus=ApprovalStatus  FROM NoProductionData  WHERE (FacilityID = " + Replace(rsAllDates__PFacilityID, "'", "''") + ")    ORDER BY WorkDate Desc");
            rsAllDates.Open();
            rsAllDates_numRows = 0;



            Repeat1__numRows = 10;
            Repeat1__index = 0;
            rs_numRows = rs_numRows + Repeat1__numRows;


            //  *** Recordset Stats, Move To Record, && Go To Record: declare stats variables;

            // the record count;
            rs_total = rs.RecordCount;

            // the number of rows displayed on this page;
            if(rs_numRows < 0){
              rs_numRows = rs_total;
            }else{
                if(rs_numRows == 0){
                   rs_numRows = 1;
                }
            }

            // the first && last displayed record;
            rs_first = 1;
            rs_last  = rs_first + rs_numRows - 1;

            // if we have the correct record count, check the other stats;
            if(rs_total != -1){
              if(rs_first > rs_total){rs_first = rs_total;}
              if(rs_last > rs_total){rs_last = rs_total;}
              if(rs_numRows > rs_total){rs_numRows = rs_total;}
            }


            // *** Recordset Stats: if we don//t know the record count, manually count them;

            if(rs_total == -1){

              // count the total records by iterating through the recordset;
              rs_total=0;
              while (!rs.EOF){
                rs.Read();
                rs_total = rs_total + 1;

              }


              rs.Requery();

              // the number of rows displayed on this page;
              if(rs_numRows < 0 || rs_numRows > rs_total){
                rs_numRows = rs_total;
              }

              // the first && last displayed record;
              rs_first = 1;
              rs_last = rs_first + rs_numRows - 1;
              if(rs_first > rs_total){rs_first = rs_total;}
              if(rs_last > rs_total){rs_last = rs_total;}

            }


            // *** Move To Record && Go To Record: declare variables;

            MM_rs    = rs;
            MM_rsCount   = rs_total;
            MM_size      = rs_numRows;
            MM_uniqueCol = "";
            MM_paramName = "";
            MM_offset = 0;
            MM_atTotal = false;
            MM_paramIsDefined = false;
            if(MM_paramName != ""){
              MM_paramIsDefined = (Request.QueryString[MM_paramName] != "");
            }


            // *** Move To Record: handle //index// or //offset// parameter;

            if(!MM_paramIsDefined && MM_rsCount != 0){

              // use index parameter if defined, otherwise use offset parameter;
              string r = Request.QueryString["index"];
              if (r == null || r == "") { r = Request.QueryString["offset"]; }
              if( r != null && r != ""){MM_offset = System.Convert.ToInt32(r);}

              // if we have a record count, check if we are past the end of the recordset;
              if(MM_rsCount != -1){
                if(MM_offset >= MM_rsCount || MM_offset == -1){ // past end or move last;
                  if((MM_rsCount % MM_size) > 0){        //last page !a full repeat region;
                    MM_offset = MM_rsCount - (MM_rsCount % MM_size);
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
              if(MM_rs.EOF){MM_offset = i; }  // MM_offset to the last possible record;

            }


            // *** Move To Record: if we dont know the record count, check the display range;

            if(MM_rsCount == -1){

              // walk to the end of the display range for( this page;
              int i = MM_offset;
              while (!MM_rs.EOF && (MM_size < 0 || i < MM_offset + MM_size)){
                  MM_rs.Read(); 
                  i = i + 1;
              }

              // if we walked off the end of the recordset, MM_rsCount && MM_size;
              if(MM_rs.EOF){
                MM_rsCount = i;
                if(MM_size < 0 || MM_size > MM_rsCount){MM_size = MM_rsCount;}
              }

              // if we walked off the end, the offset based on page size;
              if(MM_rs.EOF && !MM_paramIsDefined){
                if(MM_offset > MM_rsCount - MM_size || MM_offset == -1){
                  if((MM_rsCount % MM_size) > 0){
                    MM_offset = MM_rsCount - (MM_rsCount % MM_size);
                  }else{
                    MM_offset = MM_rsCount - MM_size;
                 }
               }
             }

              MM_rs.Requery();

              // move the cursor to the selected record;
              i = 0;
              while (!MM_rs.EOF && i < MM_offset){
                MM_rs.Read();
                i = i + 1;
              }
            }


            // *** Move To Record: update recordset stats;

            // the first && last displayed record;
            rs_first = MM_offset + 1;
            rs_last  = MM_offset + MM_size;
            if(MM_rsCount != -1){
              if(rs_first > MM_rsCount){rs_first = MM_rsCount;}
              if(rs_last > MM_rsCount){rs_last = MM_rsCount;}
            }

            // the boolean used by hide region to check if we are on the last record;
            MM_atTotal = (MM_rsCount != -1 && MM_offset + MM_size >= MM_rsCount);


            // *** Go To Record && Move To Record: create strings for( maintaining URL && Form parameters;

            // create the list of parameters which should !be maintained;
            string MM_removeList = "&index=";
            if(MM_paramName != ""){MM_removeList = MM_removeList + "&" + MM_paramName + "=";}
            string MM_keepURL=""; string MM_keepForm=""; string MM_keepBoth=""; string MM_keepNone="";

            //add the URL parameters to the MM_keepURL string;
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


            // *** Move To Record: the strings for( the first, last, next, && previous links;

            string MM_keepMove = MM_keepBoth;
            string MM_moveParam = "index";

            // if the page has a repeated region, remove //offset// from the maintained parameters;
            if(MM_size > 0){
              MM_moveParam = "offset";
              if(MM_keepMove != ""){
                string[] param = Split(MM_keepMove, "&");
                MM_keepMove = "";
                for( int i = 0; i < UBound(param); i ++){
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

            // the strings for the move to links;
            if(MM_keepMove != ""){MM_keepMove = MM_keepMove + "&";}
            string urlStr = Request.ServerVariables["URL"] + "?" + MM_keepMove + MM_moveParam + "=";
            MM_moveFirst = urlStr + "0";
            MM_moveLast  = urlStr + "-1";
            MM_moveNext  = urlStr + cStr(MM_offset + MM_size);
            int prev = MM_offset - MM_size;
            if(prev < 0){prev = 0;}
            MM_movePrev  = urlStr + cStr(prev);

     }
   
    }

 }