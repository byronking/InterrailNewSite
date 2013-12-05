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
using System.Collections.Specialized;

using Interrial.PPRS.Dal.TypedListClasses;
using Interrial.PPRS.Dal.EntityClasses;
using Interrial.PPRS.Dal.FactoryClasses;
using Interrial.PPRS.Dal.CollectionClasses;
using Interrial.PPRS.Dal.HelperClasses;
using Interrial.PPRS.Dal;

using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.DQE.SqlServer;

namespace InterrailPPRS.Payroll
{
    public partial class TaskWorked : PageBase
    {

        public string rs__PFacilityID = "5";
        public int Repeat1__numRows = 10;
        //int Repeat1__index = 0;
        public int rs_numRows = 0;
        public int rs_total;
        public int rs_first;
        public int rs_last;
        //string url = "";

        public DataReader rs;

        public string MM_removeList = "";
        public string NextItem = "";
        public string MM_keepURL = "";
        public string MM_keepForm = "";
        public string MM_keepBoth = "";
        //string MM_keepNone="";
        public string MM_keepMove = "";
        public string MM_moveParam = "";
        public string MM_moveFirst = "";
        public string MM_moveLast = "";
        public string MM_moveNext = "";
        public string MM_movePrev = "";
        public string urlStr = "";
        public int prev = 0;
        public string MM_paramName = "";
        public int MM_offset = 0;
        public bool MM_atTotal = false;
        public bool MM_paramIsDefined = false;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

            GrantAccess("Super, Admin, User");

            

            string[] param;


            if(Session["FacilityID"]  != "") { rs__PFacilityID = Session["FacilityID"].ToString();} 

            rs =  new DataReader("SELECT dbo.Employee.FacilityID, dbo.Employee.LastName + ', ' + dbo.Employee.FirstName AS FullName, dbo.EmployeeTaskWorked.Id,  dbo.EmployeeTaskWorked.WorkDate, dbo.EmployeeTaskWorked.HoursWorked, LTRIM(ISNULL(dbo.Tasks.TaskDescription, ''))                         +  LTRIM(ISNULL(dbo.OtherTasks.TaskDescription, '')) AS TaskDescription, LTRIM(ISNULL(dbo.Tasks.TaskCode, ''))                         + LTRIM(ISNULL(dbo.OtherTasks.TaskCode, '')) AS TaskCode  FROM dbo.EmployeeTaskWorked INNER JOIN                        dbo.Employee ON dbo.EmployeeTaskWorked.EmployeeId = dbo.Employee.Id LEFT OUTER JOIN                        dbo.OtherTasks ON dbo.EmployeeTaskWorked.OtherTaskID = dbo.OtherTasks.Id LEFT OUTER JOIN                        dbo.Tasks ON dbo.EmployeeTaskWorked.TaskID = dbo.Tasks.Id  WHERE (dbo.EmployeeTaskWorked.FacilityID = " + rs__PFacilityID.Replace( "'", "''") + ")  ORDER BY dbo.EmployeeTaskWorked.WorkDate DESC");
            rs.Open();

            rs_numRows = rs_numRows + Repeat1__numRows;


            //  *** Recordset Stats, Move To Record, and Go To Record: declare stats variables
            // set the record count
            rs_total = rs.RecordCount;

            // set the number of rows displayed on this page
            if(rs_numRows < 0) {
              rs_numRows = rs_total;
            }else{
                if(rs_numRows == 0) {
                   rs_numRows = 1;
                }
            }

            // set the first and last displayed record
            rs_first = 1;
            rs_last  = rs_first + rs_numRows - 1;

            // if we have the correct record count, check the other stats
            if(rs_total != -1) {
              if(rs_first > rs_total) { rs_first = rs_total;}
              if(rs_last > rs_total) { rs_last = rs_total;}
              if(rs_numRows > rs_total) { rs_numRows = rs_total;}
            }


            // *** Recordset Stats: if we don't know the record count, manually count them

            if(rs_total == -1) {

                  // count the total records by iterating through the recordset
                  rs_total=0;
                  while(rs.Read()){

                    rs_total = rs_total + 1;
    
                  }

                  //reset the cursor to the beginning
                  rs.Requery();

                  // set the number of rows displayed on this page
                  if(rs_numRows < 0 || rs_numRows > rs_total) {
                    rs_numRows = rs_total;
                  }

                  // set the first and last displayed record
                  rs_first = 1;
                  rs_last = rs_first + rs_numRows - 1;
                  if(rs_first > rs_total) { rs_first = rs_total;}
                  if(rs_last > rs_total) { rs_last = rs_total;}

            }


            // *** Move To Record and Go To Record: declare variables

            DataReader MM_rs    = rs;
            int MM_rsCount   = rs_total;
            int MM_size      = rs_numRows;
            string MM_uniqueCol = "";
            MM_paramName = "";
            MM_offset = 0;
            MM_atTotal = false;
            MM_paramIsDefined = false;

            if(MM_paramName != "") {
              MM_paramIsDefined = (Request.QueryString[MM_paramName] != "");
            }


            // *** Move To Record: handle 'index' or 'offset' parameter

            if(! MM_paramIsDefined && MM_rsCount != 0) {

              // use index parameter if defined, otherwise use offset parameter
              string r = Request.QueryString["index"];
              if( r == "" ){ r = Request.QueryString["offset"];}
              if (r != ""){ MM_offset = System.Convert.ToInt32(r);};

              // if we have a record count, check if we are past the end of the recordset
              if(MM_rsCount != -1) {
                if(MM_offset >= MM_rsCount || MM_offset == -1) {  // past end or move last
                  if((MM_rsCount % MM_size) > 0) {         // last page not a full repeat region
                    MM_offset = MM_rsCount - (MM_rsCount % MM_size);
                  }else{
                    MM_offset = MM_rsCount - MM_size;
                  }
                }
              }

              // move the cursor to the selected record
              int i = 0;
              while ( MM_rs.Read() && (i < MM_offset || MM_offset == -1)){
                i = i + 1;
              }

              if(MM_rs.Read()) { MM_offset = i;} // set MM_offset to the last possible record

            }


            // *** Move To Record: if we dont know the record count, check the display range

            if(MM_rsCount == -1) {

              // walk to the end of the display range for this page
              int i = MM_offset;
              while (MM_rs.Read() && (MM_size < 0 || i < MM_offset + MM_size)){
                i = i + 1;
              }

              // if we walked off the end of the recordset, set MM_rsCount and MM_size
              if(MM_rs.Read()) {
                MM_rsCount = i;
                if(MM_size < 0 || MM_size > MM_rsCount) { MM_size = MM_rsCount;}
              }

              // if we walked off the end, set the offset based on page size
              if(MM_rs.Read() == false && ! MM_paramIsDefined) {
                if(MM_offset > (MM_rsCount - MM_size) || MM_offset == -1) {
                  if((MM_rsCount % MM_size) > 0) {
                    MM_offset = MM_rsCount - (MM_rsCount % MM_size);
                  }else{
                    MM_offset = MM_rsCount - MM_size;
                  }
                }
              }

              // reset the cursor to the beginning
              MM_rs.Requery();

              // move the cursor to the selected record
              i = 0;
              while (MM_rs.Read() && i < MM_offset){
                i = i + 1;
              }
            }


            // *** Move To Record: update recordset stats
            // set the first and last displayed record
            rs_first = MM_offset + 1;
            rs_last  = MM_offset + MM_size;
            if(MM_rsCount != -1) {
              if(rs_first > MM_rsCount) { rs_first = MM_rsCount;}
              if(rs_last > MM_rsCount) { rs_last = MM_rsCount;}
            }

            // set the boolean used by hide region to check if we are on the last record
            MM_atTotal = (MM_rsCount != -1 && MM_offset + MM_size >= MM_rsCount);


            // *** Go To Record and Move To Record: create strings for maintaining URL and Form parameters
            // create the list of parameters which should not be maintained
            MM_removeList = "&index=";
            if(MM_paramName != "") { MM_removeList = MM_removeList + "&" +  MM_paramName + "=";}
            MM_keepURL="";
            MM_keepForm="";
            MM_keepBoth="";
            //MM_keepNone="";

            // add the URL parameters to the MM_keepURL string
            NameValueCollection queryParameters = new NameValueCollection();
            queryParameters = Request.QueryString;
            for(int x=0; x <  queryParameters.Count; x++) 
            { 
                  string key = queryParameters.GetKey(x);
                  NextItem = "&" + key + "=";
                   if(InStr(1,MM_removeList,NextItem,1) == 0){
                       MM_keepURL = MM_keepURL + NextItem + Server.UrlEncode(Request.QueryString[key]);
                   }

            } 


            // add the Form variables to the MM_keepForm string
            foreach(string key in Request.Form.AllKeys){
              NextItem = "&" + key + "=";
              if(InStr(1,MM_removeList,NextItem,1) == 0) {
                MM_keepForm = MM_keepForm + NextItem + Server.UrlEncode(Request.Form[key]);
              }
            }

            // create the Form + URL string and remove the intial '&' from each of the strings
            MM_keepBoth = MM_keepURL + MM_keepForm;
            if(MM_keepBoth != "") { MM_keepBoth = Right(MM_keepBoth, Len(MM_keepBoth) - 1);}
            if(MM_keepURL != "")  { MM_keepURL  = Right(MM_keepURL, Len(MM_keepURL) - 1);}
            if(MM_keepForm != "") { MM_keepForm = Right(MM_keepForm, Len(MM_keepForm) - 1);}


            // *** Move To Record: set the strings for the first, last, next, and previous links

            MM_keepMove = MM_keepBoth;
            MM_moveParam = "index";

            // if the page has a repeated region, remove 'offset' from the maintained parameters
            if(MM_size > 0) {
              MM_moveParam = "offset";
              if(MM_keepMove != "") {
                param = Split(MM_keepMove, "&");
                MM_keepMove = "";

                for (int i = 0; i < UBound(param);i++ )
                {
                    NextItem = Left(param[i], InStr(0, param[i], "=", 0) - 1);
                    if (StrComp(NextItem, MM_moveParam, 1) != 0)
                    {
                        MM_keepMove = MM_keepMove + "&" + param[i];
                    }
                }

                if(MM_keepMove != "") {
                  MM_keepMove = Right(MM_keepMove, Len(MM_keepMove) - 1);
                }
              }
            }

            // set the strings for the move to links
            if(MM_keepMove != "") { 
                MM_keepMove = MM_keepMove + "&";
                urlStr = Request.ServerVariables["URL"] + "?" + MM_keepMove + MM_moveParam + "=";
                MM_moveFirst = urlStr + "0";
                MM_moveLast  = urlStr + "-1";
                MM_moveNext  = urlStr + Cstr(MM_offset + MM_size);
                prev = MM_offset - MM_size;
                if(prev < 0) { 
                    prev = 0;
                    MM_movePrev  = urlStr + Cstr(prev);
                }
            }

        }
    }
}