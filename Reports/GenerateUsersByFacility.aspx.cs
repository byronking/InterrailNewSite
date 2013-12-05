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

namespace InterrailPPRS.Reports
{
    public partial class GenerateUsersByFacility : PageBase
    {

        public DataReader rst;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin, User");

            // Main Report Recordset;
            Session["UsersByFacility"] = generateRS();

            // Create Main Report;
            CreateMainReport("UsersByFacility");
            SendReportToClient();

        }

        public string generateRS(){

           string strSQL, rst, sWhere;

              if (cStr(Request["selFacilities"]).Length == 0){
                sWhere = sWhere + "  Where 1=1 ";
              }else{
                sWhere = sWhere + "  Where FacilityID IN (" + cStr(Request["selFacilities"]) + ") ";
             }
              //;


          strSQL = " ";
          strSQL +=  "SELECT UserID, UserName, UserLongName, UserType, Facility=IsNull(Name, 'Corporate')   ";
          strSQL +=  "  FROM Facility  ";
          strSQL +=  "             INNER JOIN UserRights ON Facility.Id = UserRights.FacilityId ";
          strSQL +=  "       RIGHT OUTER JOIN UserProfile ON UserRights.UserProfileId = UserProfile.Id ";
          strSQL +=  " Order By IsNull(Name, 'Corporate'), UserType, UserID, UserName, UserLongName ";


          bool DBug = true;
          DBug = false;

          if(DBug){
            Response.Write(strSQL);
            Response.End;
         }

  rst = getRs(strSQL);
  //;
  // Build recordset to send to Crystal Reports;
  //;
  rs =  Server.CreateObject("ADODB.Recordset");
  rs.Fields.Append "Facility",      129,   30, 64;
  rs.Fields.Append "UserLongName",  129,   30, 64;
  rs.Fields.Append "UserName",      129,   10, 64;
  rs.Fields.Append "UserType",      129,   10, 64;
  rs.Fields.Append "UserID",        129,   10, 64;
  rs.Open;

  if(rst.EOF){

  }else{
    while (!rst.EOF ){
      rs.AddNew;
      rs.Item("Facility")     = rst("Facility");
      rs.Item("UserLongName") = rst("UserLongName");
      rs.Item("UserName")     = rst("UserName");
      rs.Item("UserType")     = rst("UserType");
      rs.Item("UserID")       = rst("UserID");
      rs.Update;

    } //End Loop

    DBug = True;
    DBug = False;

    if(DBug){
      rs.MoveFirst;
      Response.Write(" ===  Start ===<br>");
      while (!rs.EOF){
        for( Each f in rs.Fields){
          Response.Write(f.Name + " = " + (f.Value) + " &nbsp; * &nbsp;&nbsp;");
        }
        Response.Write("<br>");

      } //End Loop
      Response.Write(" ===  End === ");
      Response.End;
   }

    rs.MoveFirst;

    rst.Close;


 }


  return rs;


}
    }
}