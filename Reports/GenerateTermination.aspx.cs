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
    public partial class GenerateTermination : PageBase
    {
        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


  GrantAccess("Super, Admin, User");




sStartDate = Request["fromDate"];
sEndDate   = Request["toDate"];

Session["LastStartDate"] = sStartDate;
Session["LastEndDate"]   = sEndDate;


// Main Report Recordset;
Set Session["Termination"] = generateRS(sStartDate, sEndDate);

// Create Main Report;
CreateMainReport("Termination");
SetStringParameter 1, Request["FromDate"];
SetStringParameter 2, Request["ToDate"];
SendReportToClient();




public generateRS(StartDate, EndDate)
  string sStartDate, sEndDate;
  string strSQL, rst, sWhere;

  sStartDate = "Convert(Char(8), Cast('" + StartDate + "' As DateTime), 112) ";
  sEndDate   = "Convert(Char(8), Cast('" + EndDate   + "' As DateTime), 112) ";

  //;
  strSQL +=  "SELECT LastName, FirstName, MiddleInitial, Name,    ";
  strSQL +=  "       HireDate=Convert(Char(10), HireDate, 101),   ";
  strSQL +=  "       TerminationDate=Convert(Char(10), TerminationDate, 101)  ";
  strSQL +=  "  FROM Employee LEFT OUTER JOIN Facility ON Employee.FacilityID = Facility.Id ";
  if (IsEmpty(Request["selFacilities"]){
     strSQL +=  " Where 1=1 ";
  }else{
     strSQL +=  " Where FacilityID IN (" + Request["selFacilities"] + ") ";
 }
  strSQL +=  "    AND Convert(Char(8), TerminationDate, 112) Between  "  + sStartDate + " AND " + sEndDate + " ";

  strSQL +=  " ORDER BY Name, TerminationDate, LastName, FirstName                          ";

  DBug = True;
  DBug = False;

  if(DBug){
    Response.Write(strSQL;
    Response.End;
 }

  rst = getRs(strSQL);
  //;
  // Build recordset to send to Crystal Reports;
  //;
  rs =  Server.CreateObject("ADODB.Recordset");
  rs.Fields.Append "Facility",        129,   30, 64;
  rs.Fields.Append "TerminationDate", 129,   10, 64;
  rs.Fields.Append "LastName",        129,   30, 64;
  rs.Fields.Append "FirstName",       129,   30, 64;
  rs.Fields.Append "MiddleInitial",   129,    1, 64;
  rs.Fields.Append "HireDate",        129,   10, 64;

  rs.Open;

  if(rst.EOF){
  }else{
    while (!rst.EOF ){
      rs.AddNew;
      rs.Item("Facility")        = rst("Name");
      rs.Item("TerminationDate") = rst("TerminationDate");
      rs.Item("LastName")        = rst("LastName");
      rs.Item("FirstName")       = rst("FirstName");
      rs.Item("MiddleInitial")   = rst("MiddleInitial");
      rs.Item("HireDate")        = rst("HireDate");
      rs.Update;

    } //End Loop

    DBug = True;
    DBug = False;

    if(DBug){
      rs.MoveFirst;
      Response.Write(" ===  Start ===<br>";
      while (!rs.EOF){
        for( Each f in rs.Fields;
          Response.Write(f.Name + " = " + (f.Value) + " &nbsp; * &nbsp;&nbsp;";
        }
        Response.Write("<br>";

      } //End Loop
      Response.Write(" ===  End === ";
      Response.End;
   }

    rs.MoveFirst;

    rst.Close;


 }


  generateRS = rs;


}



        }
    }
}