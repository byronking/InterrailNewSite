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
    public partial class GenerateTemporary : PageBase
    {
        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


  GrantAccess("Super, Admin, User");




// Main Report Recordset;
Set Session["Temporary"] = generateRS();

// Create Main Report;
CreateMainReport("Temporary");
SendReportToClient();




public DataReader generateRS(){

  string strSQL, rst, sWhere;

  //;
  sWhere =          "WHERE (TempEmployee=1 OR HireDate Is NULL) ";
  sWhere = sWhere + "  AND Employee.Active=1 AND DateDiff(Day, GetDate(), DateAdd(Day, 30, TempStartDate)) <=30 ";

  if (IsEmpty(Request["selFacilities"]){
  }else{
    sWhere = sWhere + "  AND FacilityID IN (" + Request["selFacilities"] + ") ";
 }
  //;

  strSQL = " ";
  strSQL +=  "SELECT  Facility=IsNull(Name, 'Corporate'), LastName, FirstName,            ";
  strSQL +=  "        StartDate=Convert(Char(10), TempStartDate, 101),                    ";
  strSQL +=  "        EndProbation=Convert(Char(10),DateAdd(Day, 30, TempStartDate),101), ";
  strSQL +=  "        DaysLeft=DateDiff(Day, GetDate(), DateAdd(Day, 30, TempStartDate))  ";
  strSQL +=  "  FROM Employee  ";
  strSQL +=  "       LEFT OUTER JOIN Facility ON Employee.FacilityID = Facility.Id ";
  strSQL = strSQL + sWhere;
  strSQL +=  " Order By Facility, DaysLeft, LastName, FirstName";

  //;
  DBug = true;
  DBug = false;

  if(DBug){
    Response.Write(strSQL;
    Response.End;
 }

  rst = getRs(strSQL);
  //;
  // Build recordset to send to Crystal Reports;
  //;
  rs =  Server.CreateObject("ADODB.Recordset");
  rs.Fields.Append "Facility",      129,   30, 64;
  rs.Fields.Append "LastName",      129,   30, 64;
  rs.Fields.Append "FirstName",     129,   30, 64;
  rs.Fields.Append "StartDate",     129,   10, 64;
  rs.Fields.Append "EndProbation",  129,   10, 64;
  rs.Fields.Append "DaysLeft",        3,    4, 64;
  rs.Open;

  if(rst.EOF){

  }else{
    while (!rst.EOF ){
      rs.AddNew;
      rs.Item("Facility")     = rst("Facility");
      rs.Item("LastName")     = rst("LastName");
      rs.Item("FirstName")    = rst("FirstName");
      rs.Item("StartDate")    = rst("StartDate");
      rs.Item("EndProbation") = rst("EndProbation");
      rs.Item("DaysLeft")     = rst("DaysLeft");
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