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
using System.Text;

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
    public partial class UserFacilityMonitor : PageBase
    {
        public DataReader rsInsertFac;
        public string MM_editRedirectUrl = "";

        public DataReader rsUserName;
        public int rsUserName_numRows = 0;

        public int Repeat1__numRows;
        public int Repeat1__index;

        public int rs_numRows = 0;
        public string rsUserName__MMColParam;
        public string MM_editAction = "";

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin");


            // *** Edit Operations: declare variables;

            MM_editAction = cStr(Request["URL"]);
            if(Request.QueryString.Count > 0){
            //  MM_editAction = MM_editAction + "?" + Request.QueryString;
            }

            // boolean to abort record edit;
            bool MM_abortEdit = false;

            // query string to execute;
            string MM_editQuery = "";


            // *** Update Record: variables;

            if(cStr(Request["MM_update"]) != "" && cStr(Request["MM_recordId"]) != ""){

              string MM_editTable = "dbo.UserRights";
              string MM_editColumn = "UserProfileId";
              string MM_recordId = "" + Request.Form["MM_recordId"] + "";
              MM_editRedirectUrl = "User.aspx";

              // append the query string to the redirect URL;
              if(MM_editRedirectUrl != "" && Request.QueryString.Count > 0){
                if(InStr(0, MM_editRedirectUrl, "?", 1) == 0 && Request.QueryString.Count > 0){
                  MM_editRedirectUrl = MM_editRedirectUrl + "?" + Request.QueryString;
                }else{
                  MM_editRedirectUrl = MM_editRedirectUrl + "&" + Request.QueryString;
               }
             }

            }


            // *** Update Record: construct a sql update statement && execute it;

            if(cStr(Request["MM_update"]) != "" && cStr(Request["MM_recordId"]) != ""){


              // create the sql update statement;
              MM_editQuery = MM_editQuery + "delete from RegionalRights where UserProfileID = " + cStr(Request["MM_RecordID"]) ;
              MM_editQuery = MM_editQuery + "delete from UserRights where RegionalRight = 1 AND UserProfileID = " + cStr(Request["MM_RecordID"]);
              //MM_editQuery = MM_editQuery + " Go " + vbCrLF;

              if (Request["checkboxRegion"] != null)
              {
                  string[] checkboxArray = Split(Request["checkboxRegion"], ",");
                  for (int i = LBound(checkboxArray); i < UBound(checkboxArray); i++)
                  {
                      MM_editQuery = MM_editQuery + "Insert into RegionalRights (UserProfileID, RegionID, LastModifiedBy, LastModifiedOn) ";
                      MM_editQuery = MM_editQuery + " Values ( " + cStr(Request["MM_RecordID"]) + ", " + checkboxArray[i] + ", '" + Session["UserName"] + "', '" + System.DateTime.Now.ToString() + "' ) ";

                      rsInsertFac = new DataReader("SELECT Id FROM facility WHERE regionId = " + checkboxArray[i]);
                      rsInsertFac.Open();

                      while (rsInsertFac.Read())
                      {
                          MM_editQuery = MM_editQuery + "Insert into UserRights (UserProfileID, FacilityID, LastModifiedBy, LastModifiedOn, RegionalRight) ";
                          MM_editQuery = MM_editQuery + " Values ( " + cStr(Request["MM_RecordID"]) + ", " + rsInsertFac.Item("Id") + ", '" + Session["UserName"] + "', '" + System.DateTime.Now.ToString() + "', 1 ) ";
                      }


                  }
              }


              if(!MM_abortEdit){
                // execute the update;
                this.Execute(MM_editQuery);

                if(MM_editRedirectUrl != ""){
                  Response.Redirect(MM_editRedirectUrl);
                }
              }

            }


            string rs__MMColParam;
            rs__MMColParam = "1";
            if(Request.QueryString["Id"] != ""){rs__MMColParam = Request.QueryString["Id"];}

            //set rs = Server.CreateObject("ADODB.Recordset");
            //rs.ActiveConnection = MM_Main_STRING;
            //rs.Source = "SELECT ur.UserProfileId, ur.FacilityId, ur.LastModifiedBy, ur.LastModifiedOn, f.Name, f.Id FROM dbo.UserRights ur, dbo.Facility f  WHERE f.ID *= ur.FacilityID AND UserProfileId = " + Replace(rs__MMColParam, "'", "''") + "";
            //rs.CursorType = 0;
            //rs.CursorLocation = 2;
            //rs.LockType = 3;
            //rs.Open();
            //rs_numRows = 0;


            
            rsUserName__MMColParam = "1";
            if(Request.QueryString["Id"] != ""){rsUserName__MMColParam = Request.QueryString["Id"];}


            rsUserName = new DataReader( "SELECT Id, UserID, UserName FROM dbo.UserProfile WHERE Id = " + Replace(rsUserName__MMColParam, "'", "''") + "");
            rsUserName.Open();
            rsUserName.Read();
            rsUserName_numRows = rsUserName.RecordCount;


            Repeat1__numRows = -1;
            Repeat1__index = 0;
            rs_numRows = rs_numRows + Repeat1__numRows;

}

    public string ShowRegions(){

    StringBuilder sb = new StringBuilder();

    DataReader rsRegions;
    string rowColor = "";

	int rs_numRows = 0;
	Repeat1__numRows = -1;
	Repeat1__index = 0;
	rs_numRows = rs_numRows + Repeat1__numRows;

	rsRegions = new DataReader(" SELECT id, RegionDescription, (SELECT 'YES' FROM RegionalRights WHERE UserProfileId = " + rsUserName__MMColParam + " AND RegionId = IRGRegion.Id) As Region FROM IRGRegion ORDER BY RegionDescription ");
	rsRegions.Open();


	
	     sb.Append("<tr>");
	     sb.Append("    <td colspan='4' class='pageTitle' align='center'> ");
	     sb.Append("    <div class='cellTopBottomBorder'>By Region</div> ");
	     sb.Append("</td> ");
         sb.Append("</tr>");
	

	    while ((Repeat1__numRows != 0) && (!rsRegions.EOF)){

              rsRegions.Read();

		     if((Repeat1__index % 2) == 0){
			    rowColor = "reportEvenLine";
		     }else{
			    rowColor = "reportOddLine";
		     }
	
             sb.Append("   <tr class='=rowColor'> ");
             sb.Append("       <td width='10%'> ");
             sb.Append("       <div align='right'> ");
             sb.Append("           <input ");
                 if(rsRegions.Item("Region") == "YES"){
                     sb.Append(" CHECKED ");
                 }
             sb.Append(" type='checkbox' name='checkboxRegion' value='" + rsRegions.Item("Id") + "' >");
             sb.Append("       </div> ");
             sb.Append("       </td> ");
             sb.Append("       <td width='47%' align='left'>" + rsRegions.Item("RegionDescription") + " </td> ");
             sb.Append("       <td width='43%'> ");
             sb.Append("       </td> ");
             sb.Append("   </tr> ");
                                
		      Repeat1__index=Repeat1__index+1;
		      Repeat1__numRows=Repeat1__numRows-1;

             

	    }

        return sb.ToString();

       }

    }
}