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
    public partial class GenerateCostPerUnit : PageBase
    {

        public string sStartDate,sEndDate;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

GrantAccess("Super, Admin, User");

sStartDate = Request["fromDate"];
sEndDate   = Request["toDate"];

Session["LastStartDate"] = sStartDate;
Session["LastEndDate"]   = sEndDate;


// Main Report Recordset;
Session["CostPerUnit"] = generateRS(sStartDate, sEndDate);

// Create Main Report;
CreateMainReport("CostPerUnit");
SetStringParameter 1, Request["FromDate"];
SetStringParameter 2, Request["ToDate"];
SendReportToClient();



}

public generateRS(string StartDate,string EndDate) {

  string sStartDate, sEndDate, strEnd;
  string strSQL, rst, sWhere;

  sStartDate = "Convert(Char(8), Cast('" + StartDate + "' As DateTime), 112) ";
  sEndDate   = "Convert(Char(8), Cast('" + EndDate   + "' As DateTime), 112) ";
  strEnd     = "Convert(Char(10), DateAdd(day,-1,DateAdd(wk, (DateDiff(Day,'07/28/2000', WorkDate)/7)+1,'07/28/2000')), 101) ";



  sWhere =          " Where (Tasks.Rebillable = 0) AND Convert(Char(8), WorkDate, 112) Between "  + sStartDate + " AND " + sEndDate + " ";

  strSQL =          " SELECT  Name As Facility, TaskCode,  ";
  strSQL +=  "        EndDate=Convert(Char(10), DateAdd(day,-1,DateAdd(wk, (DateDiff(Day,'07/28/2000', WorkDate)/7)+1,'07/28/2000')), 101), ";
  strSQL +=  "        Cost = IsNull(SUM(IsNull(PayAmount, 0)), 0),  ";
  strSQL +=  "        Units = Convert(int, SUM(ISNULL(dbo.EmployeeTaskWorked.HoursWorked, 0) * ISNULL(dbo.EmployeeTaskWorked.UPM, 0))) ";
  strSQL +=  "FROM    Facility INNER JOIN  ";
  strSQL +=  "        EmployeeTaskWorked ON dbo.Facility.Id = EmployeeTaskWorked.FacilityID ";
  strSQL +=  "        INNER JOIN Tasks ON EmployeeTaskWorked.TaskID = Tasks.Id ";
  strSQL +=  "        LEFT OUTER JOIN EmployeeTaskWorkedPay ON EmployeeTaskWorked.Id = EmployeeTaskWorkedPay.EmployeeTaskWorkedId ";
  if (Request["selFacilities"] == ""){
     strSQL = strSQL + sWhere;
  }else{
     strSQL = strSQL + sWhere + " AND FacilityID IN (" + Request["selFacilities"] + ") ";
 }
  strSQL +=  "Group By Name, TaskCode, " + strEnd;
  strSQL +=  "Order By Facility, "  + strEnd + ", Case TaskCode When 'LO'){'0' When 'UL'){'1' }else{ TaskCode End";

  bool DBug = false;   
  if(DBug){
    Response.Write(strSQL);
    Response.End();
 }

  DataReader rst = new DataReader(strSQL);

  sWhere = sWhere  + " AND TaskCode IN ('LO', 'UL') ";

  strSQL =          " SELECT  Name As Facility, ";
  strSQL +=  "        EndDate=Convert(Char(10), DateAdd(day,-1,DateAdd(wk, (DateDiff(Day,'07/28/2000', WorkDate)/7)+1,'07/28/2000')), 101), ";
  strSQL +=  "        LO_UL_Units = Convert(int, SUM(ISNULL(dbo.EmployeeTaskWorked.HoursWorked, 0) * ISNULL(dbo.EmployeeTaskWorked.UPM, 0))) ";
  strSQL +=  "FROM    Facility INNER JOIN  ";
  strSQL +=  "        EmployeeTaskWorked ON dbo.Facility.Id = EmployeeTaskWorked.FacilityID ";
  strSQL +=  "        INNER JOIN Tasks ON EmployeeTaskWorked.TaskID = Tasks.Id ";
  strSQL +=  "        LEFT OUTER JOIN EmployeeTaskWorkedPay ON EmployeeTaskWorked.Id = EmployeeTaskWorkedPay.EmployeeTaskWorkedId ";
  if (Request["selFacilities"] == ""){
     strSQL = strSQL + sWhere;
  }else{
     strSQL = strSQL + sWhere + " AND FacilityID IN (" + Request["selFacilities"] + ") ";
 }
  strSQL +=  "Group By Name, " + strEnd;
  strSQL +=  "Order By Facility, "  + strEnd;

  DataReader rstTotals = new DataReader(strSQL);

  //;
  // Build recordset to send to Crystal Reports;
  //;
  rs =  Server.CreateObject("ADODB.Recordset");
  rs.Fields.Append "Facility",        129,   30, 64;
  rs.Fields.Append "Task",            129,    2, 64;
  rs.Fields.Append "EndDate",         129,   10, 64;
  rs.Fields.Append "Cost",              5,    4, 64;
  rs.Fields.Append "Units",             5,    4, 64;
  rs.Fields.Append "LO_UL",             5,    4, 64;

  rs.Open;
  Dim iRec;
  iRec = 0;

  if(rst.EOF){

  }else{
    while (!rst.EOF ){
      rs.AddNew;
      rs.Item("Facility")  = rst("Facility");
      rs.Item("Task")      = Trim(rst("TaskCode"));
      rs.Item("EndDate")   = rst("EndDate");
      rs.Item("Cost")      = rst("Cost");
      rs.Item("Units")     = rst("Units");
      rs.Item("LO_UL")     = GetUnits(rstTotals, rst("Facility"), rst("EndDate") );
      rs.Update;

    } //End Loop

    bool DBug = false;

    if(DBug){
      rs.MoveFirst;
      Response.Write(" ===  Start ===<br>";
      while (!rs.EOF){
        for( Each f in rs.Fields;
          Response.Write(vbTab + f.Name + " = " + vbTab + (f.Value) + " &nbsp; * &nbsp;&nbsp;";
        }
        Response.Write("<br>";

      } //End Loop
      Response.Write(" ===  End === ";
      Response.End;
   }

    rs.MoveFirst;

    rst.Close;


    rstTotals.Close;


 }


  generateRS = rs;


}

public int GetUnits(rstFrom, string sFacility, string sEndDate){

  string sFilter;

  sFilter = "Facility = '" + sFacility + "' AND EndDate = '" + sEndDate + "'";
  rstFrom.Filter = sFilter;
  if (rstFrom.EOF ){
    return 0;
  }else{
    return cInt(rstFrom("LO_UL_Units"));
 }

}





    }
}