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
    public partial class Facility : PageBase
    {
                
        public DataReader rsFac;
        public DataReader MM_rs;
        public int rsFac_total = 0;
        public int rsFac_first = 0;
        public int rsFac_last = 0;
        public int rsFac_numRows = 0;
        public int Repeat1__numRows;
        public int Repeat1__index;
        public int MM_offset = 0;
        public int MM_rsCount = 0;
        public int MM_size = 0;
        public bool MM_atTotal = false;
        public bool MM_paramIsDefined = false;
        public string MM_paramName = "";

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


            GrantAccess("Super, Admin");


            rsFac = new DataReader("SELECT Id, FacilityNumber, AlphaCode, Name, DefaultTaskID, DefaultShiftID, OvertimeCalcBasis, GLCostCenter, IRGCompanyId, LastModifiedBy, LastModifiedOn  FROM dbo.Facility  ORDER BY Name, AlphaCode");
            rsFac.Open();
            rsFac_numRows = 0;



            Repeat1__numRows = 10;
            Repeat1__index = 0;
            rsFac_numRows = rsFac_numRows + Repeat1__numRows;


            //  *** Recordset Stats, Move To Record, && Go To Record: declare stats variables;

            // the record count;
            rsFac_total = rsFac.RecordCount;

            // the number of rows displayed on this page;
            if(rsFac_numRows < 0){
              rsFac_numRows = rsFac_total;
            }else{
                 if(rsFac_numRows == 0){
                    rsFac_numRows = 1;
                 }
            }

            // the first && last displayed record;
            rsFac_first = 1;
            rsFac_last  = rsFac_first + rsFac_numRows - 1;

            // if we have the correct record count, check the other stats;
            if(rsFac_total != -1){
              if(rsFac_first > rsFac_total){rsFac_first = rsFac_total;}
              if(rsFac_last > rsFac_total){rsFac_last = rsFac_total;}
              if(rsFac_numRows > rsFac_total){rsFac_numRows = rsFac_total;}
            }


            // *** Recordset Stats: if we don//t know the record count, manually count them;

            if(rsFac_total == -1){

              // count the total records by iterating through the recordset;
              rsFac_total=0;
              while (!rsFac.EOF){
                  rsFac.Read();
                rsFac_total = rsFac_total + 1;

              }

              // reset the cursor to the beginning;
              rsFac.Requery();

              // the number of rows displayed on this page;
              if(rsFac_numRows < 0 || rsFac_numRows > rsFac_total){
                rsFac_numRows = rsFac_total;
             }

              // the first && last displayed record;
              rsFac_first = 1;
              rsFac_last = rsFac_first + rsFac_numRows - 1;
              if(rsFac_first > rsFac_total){rsFac_first = rsFac_total;}
              if(rsFac_last > rsFac_total){rsFac_last = rsFac_total;}

            }


            // *** Move To Record && Go To Record: declare variables;

            MM_rs    = rsFac;
            int MM_rsCount   = rsFac_total;
            int MM_size      = rsFac_numRows;
            string MM_uniqueCol = "";
            MM_paramName = "";
            int MM_offset = 0;
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
              if (r != null && r != ""){MM_offset = System.Convert.ToInt32(r);}

              // if we have a record count, check if we are past the end of the recordset;
              if(MM_rsCount != -1){
                if(MM_offset >= MM_rsCount || MM_offset == -1){ // past end or move last;
                  if((MM_rsCount % MM_size) > 0){        // last page !a full repeat region;
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
              if(MM_rs.EOF){MM_offset = i ;} // MM_offset to the last possible record;

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

              // reset the cursor to the beginning;
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
            rsFac_first = MM_offset + 1;
            rsFac_last  = MM_offset + MM_size;
            if(MM_rsCount != -1){
              if(rsFac_first > MM_rsCount){rsFac_first = MM_rsCount;}
              if(rsFac_last > MM_rsCount){rsFac_last = MM_rsCount;}
            }

            // the boolean used by hide region to check if we are on the last record;
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
            for( int x = 0; x < Request.QueryString.Count; x++){
              string NextItem = "&" + Request.QueryString[x] + "=";
              if(InStr(0,MM_removeList,NextItem,1) == 0){
                MM_keepURL = MM_keepURL + NextItem + Server.UrlEncode(Request.QueryString[x]);
             }
            }

            // add the Form variables to the MM_keepForm string;
            for( int z = 0; z < Request.Form.Count; z++){
              string NextItem = "&" + Request.Form[z] + "=";
              if(InStr(1,MM_removeList,NextItem,1) == 0){
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
                for(int i = 0; i < UBound(param); i++){
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

            // the strings for( the move to links;
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