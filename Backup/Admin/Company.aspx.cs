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
    public partial class Company : PageBase
    {
        public string MM_moveFirst = "";
        public string MM_moveLast = "";
        public string MM_moveNext = "";
        public string MM_movePrev = "";
        public int MM_offset = 0;
        public bool MM_atTotal = false;
        public string MM_keepBoth = ""; 
        protected int MM_size = 0;
        public DataReader rsCompany;
        public int Repeat1__index;
        public int Repeat1__numRows;
        public string rowColor = "";
        public int rsCompany_first;
        public int rsCompany_last;
        public int rsCompany_total;


        protected override void Page_Load(object sender, EventArgs e)
        {
             
            base.Page_Load(sender, e);

            GrantAccess("Super, Admin, User");

            rsCompany = new DataReader("SELECT Id, CompanyID, CompanyName  FROM dbo.IRGCompany  ORDER BY CompanyName ASC");
            rsCompany.Open();

            Repeat1__numRows = 10;
            Repeat1__index = 0;

            int rsCompany_numRows = 0;
            rsCompany_numRows = rsCompany_numRows + Repeat1__numRows;


            //  *** Record Stats, Move To Record, && Go To Record: declare stats variables;

            //  the record count;
            rsCompany_total = rsCompany.RecordCount;

            //  the number of rows displayed on this page;
            if(rsCompany_numRows < 0){
              rsCompany_numRows = rsCompany_total;
            }else{
                if(rsCompany_numRows == 0){
                    rsCompany_numRows = 1;
                }
            }

            //  the first && last displayed record;
            rsCompany_first = 1;
            rsCompany_last  = rsCompany_first + rsCompany_numRows - 1;

            // if we have the correct record count, check the other stats;
            if(rsCompany_total != -1) {
              if(rsCompany_first > rsCompany_total) { rsCompany_first = rsCompany_total;}
              if(rsCompany_last > rsCompany_total) { rsCompany_last = rsCompany_total;}
              if(rsCompany_numRows > rsCompany_total) { rsCompany_numRows = rsCompany_total;}
            }


            // *** Record Stats: if we don//t know the record count, manually count them;

            if(rsCompany_total == -1){

              // count the total records by iterating through the recordset;
                rsCompany_total = rsCompany.RecordCount;

              //  the number of rows displayed on this page;
              if(rsCompany_numRows < 0 || rsCompany_numRows > rsCompany_total){
                rsCompany_numRows = rsCompany_total;
              }

              //  the first && last displayed record;
              rsCompany_first = 1;
              rsCompany_last = rsCompany_first + rsCompany_numRows - 1;
              if(rsCompany_first > rsCompany_total){ rsCompany_first = rsCompany_total;}
              if(rsCompany_last > rsCompany_total){ rsCompany_last = rsCompany_total;}

            }


            // *** Move To Record && Go To Record: declare variables;

            DataReader MM_rs  = rsCompany;
            int MM_rsCount   = rsCompany_total;
            int MM_size      = rsCompany_numRows;
            string MM_uniqueCol = "";
            string MM_paramName = "";
            bool MM_paramIsDefined = false;
            string MM_keepURL = "";
            string MM_keepForm = "";
            string MM_keepMove = "";
            string MM_moveParam = "";
            string MM_keepNone = "";

            if(MM_paramName != "") {
              MM_paramIsDefined = (Request.QueryString[MM_paramName] != "");
            }


            // *** Move To Record: handle //index// or //offset// parameter;

            if(!MM_paramIsDefined && MM_rsCount != 0){

              // use index parameter if defined, otherwise use off parameter;
              string r = System.Convert.ToString(Request.QueryString["index"]);
              if(r == null || r == "" ){ r = System.Convert.ToString(Request.QueryString["offset"]);}
              if( r!= null && r != "" ){ MM_offset = System.Convert.ToInt32(r);}

              // if we have a record count, check if we are past the end of the recordset;
              if(MM_rsCount != -1){
                if(MM_offset >= MM_rsCount || MM_offset == -1){  // past end or move last;
                  if((MM_rsCount % MM_size) > 0){         // last page !a full repeat region;
                    MM_offset = MM_rsCount - (MM_rsCount % MM_size);
                  }else{
                    MM_offset = MM_rsCount - MM_size;
                  }
                }
              }

              // move the cursor to the selected record;
              int i = 0;
              while ((MM_rs.Read()) && (i < MM_offset || MM_offset == -1)){
                MM_rs.Read(); 
                i = i + 1;
              }
              if(MM_rs.Read() == false){ MM_offset = i;} //  MM_off to the last possible record;

            }


            // *** Move To Record: if we dont know the record count, check the display range;

            if(MM_rsCount == -1){

              // walk to the end of the display range for this page;
              int i = MM_offset;
              while (MM_rs.Read() && (MM_size < 0 || i < MM_offset + MM_size)){

                i = i + 1;
              }

              // if we walked off the end of the recordset,  MM_rsCount && MM_size;
              if(MM_rs.Read() == false){
                MM_rsCount = i;
                if(MM_size < 0 || MM_size > MM_rsCount){ MM_size = MM_rsCount;}
              }

              // if we walked off the end,  the off based on page size;
              if(MM_rs.Read() && !MM_paramIsDefined){
                if(MM_offset > (MM_rsCount - MM_size) || MM_offset == -1){
                  if((MM_rsCount % MM_size) > 0){
                    MM_offset = MM_rsCount - (MM_rsCount % MM_size);
                  }else{
                    MM_offset = MM_rsCount - MM_size;
                  }
                }
              }

              // re the cursor to the beginning;
              rsCompany.Requery();
  
              // move the cursor to the selected record;
              i = 0;
              while (MM_rs.Read() && i < MM_offset){

                i = i + 1;
              }
            }


            // *** Move To Record: update record stats;

            //  the first && last displayed record;
            rsCompany_first = MM_offset + 1;
            rsCompany_last  = MM_offset + MM_size;
            if(MM_rsCount != -1){
              if(rsCompany_first > MM_rsCount){ rsCompany_first = MM_rsCount;}
              if (rsCompany_last > MM_rsCount) { rsCompany_last = MM_rsCount;}
            }

            //  the boolean used by hide region to check if we are on the last record;
            MM_atTotal = (MM_rsCount != -1 && MM_offset + MM_size >= MM_rsCount);


            // *** Go To Record && Move To Record: create strings for maintaining URL && Form parameters;

            // create the list of parameters which should !be maintained;
            string MM_removeList = "&index=";
            if(MM_paramName != "") { MM_removeList = MM_removeList + "&" + MM_paramName + "=";}
            MM_keepURL = "";
            MM_keepForm = "";
            MM_keepBoth = "";
            MM_keepNone = "";

            string NextItem = "";
            // add the URL parameters to the MM_keepURL string;
            for( int z = 0; z < Request.QueryString.Count; z++){
              NextItem = "&" + Request.QueryString[z] + "=";
              if( InStr(1,MM_removeList,NextItem,1) == 0){
                MM_keepURL = MM_keepURL + NextItem + Server.UrlEncode(Request.QueryString[z]);
              }
            } 

            // add the Form variables to the MM_keepForm string;
            for(int z = 0; z < Request.Form.Count; z++){
              NextItem = "&" + Request.Form[z] + "=";
              if(InStr(1,MM_removeList,NextItem,1) == 0){
                MM_keepForm = MM_keepForm + NextItem + Server.UrlEncode(Request.Form[z]);
              }
            } 

            // create the Form + URL string and remove the intial //&// from each of the strings;
            MM_keepBoth = MM_keepURL + MM_keepForm;
            if(MM_keepBoth != ""){ MM_keepBoth = Right(MM_keepBoth, Len(MM_keepBoth) - 1);}
            if(MM_keepURL != ""){ MM_keepURL  = Right(MM_keepURL, Len(MM_keepURL) - 1);}
            if(MM_keepForm != ""){ MM_keepForm = Right(MM_keepForm, Len(MM_keepForm) - 1);}


            // *** Move To Record:  the strings for the first, last, } , && previous links;

            MM_keepMove = MM_keepBoth;
            MM_moveParam = "index";

            // if the page has a repeated region, remove //offset// from the maintained parameters;
            if(MM_size > 0){
              MM_moveParam = "offset";
              if(MM_keepMove != "") {
                string[] param = Split(MM_keepMove, "&");
                MM_keepMove = "";
                for(int i = 0 ; i <  UBound(param); i++){
                  NextItem = Left(param[i], InStr(0,param[i],"=",0) - 1);
                  if(StrComp(NextItem,MM_moveParam,1) != 0){
                    MM_keepMove = MM_keepMove + "&" + param[i];
                  }
                } 
                if(MM_keepMove != ""){
                  MM_keepMove = Right(MM_keepMove, Len(MM_keepMove) - 1);
                }
              }
            }

            //  the strings for the move to links;
            if(MM_keepMove != ""){ MM_keepMove = MM_keepMove + "&";}
            string urlStr = Request.ServerVariables["URL"] + "?" + MM_keepMove + MM_moveParam + "=";
            MM_moveFirst = urlStr + "0";
            MM_moveLast  = urlStr + "-1";
            MM_moveNext   = urlStr + System.Convert.ToString(MM_offset + MM_size);
            int prev = MM_offset - MM_size;
            if(prev < 0){ prev = 0;}
            MM_movePrev  = urlStr + System.Convert.ToString(prev);


        }



    }
}