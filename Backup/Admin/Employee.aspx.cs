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
    public partial class Employee : PageBase
    {

        public DataReader rs;
        public int Repeat2__numRows;
        public int Repeat2__index;
        public int MM_offset = 0;
        public bool MM_atTotal = false;
        public int rs_first = 0;
        public int rs_last = 0;
        public int rs_total = 0;

        public string MM_keepURL = "";
        public string MM_keepForm = "";
        public string MM_keepBoth = "";
        public string MM_keepNone = "";
        public string MM_moveFirst = "";
        public string MM_moveLast = "";
        public string MM_moveNext = "";
        public string MM_movePrev = "";

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

            GrantAccess("Super, Admin, User");


            string rs__MMPARAM;
            rs__MMPARAM = "0";
            if (System.Convert.ToString(Session["FacilityID"]) != "") { rs__MMPARAM = System.Convert.ToString(Session["FacilityID"]); }


            string strsearch = "";
            if (Len(Request["Search"]) > 0 ){
            strsearch = " (Employee.LastName Like '" + Replace(Request["Search"],  "'", "''") + "%' OR Employee.SSN = '" + Replace(Request["Search"],  "'", "''") + "' ) AND ";
            }



            string strSQL = "" ;
            strSQL +=  "SELECT dbo.Employee.EmployeeNumber, TempEmployee, TempNumber,  dbo.Employee.LastName, ";
            strSQL +=  "dbo.Employee.FirstName, dbo.Employee.Id, dbo.Employee.FacilityId, ";
            strSQL +=  "Case when FacilityID <>  " + Replace(rs__MMPARAM, "'", "''") + " then 1 else 0 end as FromAssociatedFacility  ";
            strSQL +=  "FROM dbo.Employee  WHERE " + strsearch ;
            strSQL +=  " ( dbo.Employee.FacilityID = " + Replace(rs__MMPARAM, "'", "''") ;
            strSQL +=  "   OR  ( FacilityID IN ( Select AssociatedFacilityID from AssociatedFacility where FacilityID = " + rs__MMPARAM + ")) )  " ;
            strSQL +=  "ORDER BY FromAssociatedFacility, TempEmployee, LastName, FirstName";


            rs = new DataReader(strSQL);
            rs.Open();

            

        Repeat2__numRows = 10;
        Repeat2__index = 0;
        int rs_numRows = Repeat2__numRows;


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
          if(rs_numRows > rs_total){rs_numRows = rs_total;}
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

        DataReader MM_rs    = rs;
        int MM_rsCount   = rs_total;
        int MM_size      = rs_numRows;
        string MM_uniqueCol = "";
        string MM_paramName = "";
        bool MM_paramIsDefined = false;

        if(MM_paramName != ""){
          MM_paramIsDefined = (Request.QueryString[MM_paramName] != "");
        }


        // *** Move To Record: handle //index// or //offset// parameter;

        if(!MM_paramIsDefined && MM_rsCount != 0){

          // use index parameter if defined, otherwise use offset parameter;
            string r = Request.QueryString["index"];
          if(r == null || r == "" ){r = Request.QueryString["offset"];}
          if( r != ""){MM_offset = System.Convert.ToInt32(r);}

          // if we have a record count, check if we are past the end of the recordset;
          if(MM_rsCount != -1){
            if(MM_offset >= MM_rsCount || MM_offset == -1){ 
                // past end or move last;
              if((MM_rsCount % MM_size) > 0){ 
                  // last page !a full repeat region;
                  MM_offset = MM_rsCount - (MM_rsCount % MM_size);
              }else{
                MM_offset = MM_rsCount - MM_size;
             }
           }
         }

          // move the cursor to the selected record;
          int i = 0;
          while ((!MM_rs.EOF) && (i < MM_offset || MM_offset == -1)){
              rs.Read();
            i = i + 1;
          }

          if(MM_rs.EOF){MM_offset = i ;} //  MM_off to the last possible record;

        }


        // *** Move To Record: if we dont know the record count, check the display range;

        if(MM_rsCount == -1){

          // walk to the end of the display range for this page;
          int i = MM_offset;
          while (!MM_rs.EOF && (MM_size < 0 || i < MM_offset + MM_size)){
              rs.Read();
            i = i + 1;
          }

          // if we walked off the end of the recordset,  MM_rsCount && MM_size;
          if(MM_rs.EOF){
            MM_rsCount = i;
            if(MM_size < 0 || MM_size > MM_rsCount){MM_size = MM_rsCount;}
         }

          // if we walked off the end,  the off based on page size;
          if(MM_rs.EOF && !MM_paramIsDefined){
            if(MM_offset > (MM_rsCount - MM_size) || MM_offset == -1){
              if((MM_rsCount % MM_size) > 0){
                MM_offset = MM_rsCount - (MM_rsCount % MM_size);
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
              rs.Read();
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


        // *** Go To Record && Move To Record: create strings for maintaining URL && Form parameters;

        // create the list of parameters which should !be maintained;
        string MM_removeList = "&index=";
        if(MM_paramName != ""){MM_removeList = MM_removeList + "&" + MM_paramName + "=";}


        // add the URL parameters to the MM_keepURL string;
        for(int x=0; x< Request.QueryString.Count; x++){
          string NextItem = "&" + Request.QueryString[x] + "=";
          if(InStr(0,MM_removeList,NextItem,1) == 0){
            MM_keepURL = MM_keepURL + NextItem + Server.UrlEncode(Request.QueryString[x]);
         }
        }

        // add the Form variables to the MM_keepForm string;
        for(int x=0; x< Request.Form.Count; x++){
          string NextItem = "&" + Request.Form[x] + "=";
          if(InStr(0,MM_removeList,NextItem,1) == 0){
            MM_keepForm = MM_keepForm + NextItem + Server.UrlEncode(Request.Form[x]);
         }
        }

        // create the Form + URL string && remove the intial //&// from each of the strings;
        MM_keepBoth = MM_keepURL + MM_keepForm;
        if(MM_keepBoth != ""){MM_keepBoth = Right(MM_keepBoth, Len(MM_keepBoth) - 1);}
        if(MM_keepURL != ""){MM_keepURL  = Right(MM_keepURL, Len(MM_keepURL) - 1);}
        if(MM_keepForm != ""){MM_keepForm = Right(MM_keepForm, Len(MM_keepForm) - 1);}



        // *** Move To Record:  the strings for the first, last, next, && previous links;

        string MM_keepMove = MM_keepBoth;
        string MM_moveParam = "index";

        // if the page has a repeated region, remove //offset// from the maintained parameters;
        if(MM_size > 0){
          MM_moveParam = "offset";
          if(MM_keepMove != ""){
            string[] param = Split(MM_keepMove, "&");
            MM_keepMove = "";
            int i;
            for(i = 0; i  < UBound(param);i++){
              string nextItem = Left(param[i], InStr(0,param[i],"=",0) - 1);
              if(StrComp(nextItem,MM_moveParam,1) != 0){
                MM_keepMove = MM_keepMove + "&" + param[i];
              }
            }
            if(MM_keepMove != ""){
              MM_keepMove = Right(MM_keepMove, Len(MM_keepMove) - 1);
           }
         }
        }

        //  the strings for the move to links;
        if(MM_keepMove != ""){MM_keepMove = MM_keepMove + "&";}
        string urlStr = Request.ServerVariables["URL"] + "?" + MM_keepMove + MM_moveParam + "=";
        MM_moveFirst = urlStr + "0";
        MM_moveLast  = urlStr + "-1";
        MM_moveNext  = urlStr + System.Convert.ToString(MM_offset + MM_size);
        int prev = MM_offset - MM_size;
        if(prev < 0){prev = 0;}
        MM_movePrev  = urlStr + System.Convert.ToString(prev);


        }


    }
}