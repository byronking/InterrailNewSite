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
    public partial class GenerateOrigin : PageBase
    {
        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


  GrantAccess("Super, Admin, User");



string sStartDate, sEndDate;


sStartDate = Request["fromDate"];
sEndDate   = Request["toDate"];

Session["LastStartDate"] = sStartDate;
Session["LastEndDate"]   = sEndDate;


// Main Report Recordset;
Set Session["Origin"] = generateRS(sStartDate, sEndDate);

// Create Main Report;
CreateMainReport("Origin");
SetStringParameter 1, sStartDate;
SetStringParameter 2, sEndDate;

// Sub-Report Recordset;
Set Session["TaskTotals"] = generateSubRS(sStartDate, sEndDate);

// Create Sub Report;
CreateSubReport("TaskTotals");

SendReportToClient();




public void generateRS(StartDate, EndDate){
  string sStartDate, sEndDate;
  DataReader rst, rs;
    string strSQL;
  //;
  //;
  sStartDate = "Convert(Char(8), Cast('" + StartDate + "' As DateTime), 112) ";
  sEndDate   = "Convert(Char(8), Cast('" + EndDate   + "' As DateTime), 112) ";
  //;
  strSQL =          " Select TaskDescription, Name AS FacilityName, ManufacturerName, Origin=IsNull(OriginName, 'N/A'), ";
  strSQL +=  "       SUM (CASE LevelType WHEN 'T' Then Units Else 0 END) AS TUnits,                     ";
  strSQL +=  "       SUM (CASE LevelType WHEN 'T' Then 1 Else 0 END) AS TRC,                            ";
  strSQL +=  "       SUM (CASE LevelType WHEN 'B' Then Units Else 0 END) AS BUnits,                     ";
  strSQL +=  "       SUM (CASE LevelType WHEN 'B'  Then 1 Else 0 END) AS BRC,                            ";
  strSQL +=  "       SUM (CASE WHEN LevelType NOT IN ('T', 'B') Then Units Else 0 END) AS OtherUnits,   ";
  strSQL +=  "       SUM (CASE WHEN LevelType NOT IN ('T', 'B') Then 1 Else 0 END) AS OtherRC           ";
  strSQL +=  "  From IRGOrigin                                                                          ";
  strSQL +=  "       INNER JOIN FacilityProductionDetail ON IRGOrigin.ID = FacilityProductionDetail.OriginID ";
  strSQL +=  "       INNER JOIN Facility ON FacilityProductionDetail.FacilityID = Facility.Id                ";
  strSQL +=  "       INNER JOIN Tasks ON FacilityProductionDetail.TaskId = Tasks.Id                          ";
  strSQL +=  "       INNER JOIN IRGManufacturer ON FacilityProductionDetail.ManufacturerID = IRGManufacturer.ID ";
  if (IsEmpty(Request["selFacilities"]){
     strSQL +=  " Where 1=1 ";
  }else{
     strSQL +=  " Where FacilityID IN (" + Request["selFacilities"] + ") ";
 }
  strSQL +=  "    AND Convert(Char(8), WorkDate, 112) Between  "  + sStartDate + " AND " + sEndDate + " ";
  strSQL +=  "  Group BY TaskCode, TaskDescription, Name,  IsNull(OriginName, 'N/A'), ManufacturerName     ";
  strSQL +=  " HAVING (TaskCode = 'LO') OR  (TaskCode = 'UL')                               ";
  strSQL +=  "  Order BY TaskCode Desc, TaskDescription, Name, IsNull(OriginName, 'N/A'), ManufacturerName ";

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
  rs.Fields.Append "Task",         129,   100, 64;
  rs.Fields.Append "Facility",     129,   100, 64;
  rs.Fields.Append "Manufacturer", 129,   100, 64;
  rs.Fields.Append "Origin",       129,   100, 64;
  rs.Fields.Append "TRC",            3,     4, 64;
  rs.Fields.Append "TUnits",         3,     4, 64;
  rs.Fields.Append "BRC",            3,     4, 64;
  rs.Fields.Append "BUnits",         3,     4, 64;
  rs.Fields.Append "OtherRC",        3,     4, 64;
  rs.Fields.Append "OtherUnits",     3,     4, 64;
  rs.Open;

  if(rst.EOF){
    Response.Write("No records found....";
    Response.End;
  }else{
    while (!rst.EOF ){
      rs.AddNew;
      rs.Item("Task")         = rst("TaskDescription");
      rs.Item("Facility")     = rst("FacilityName");
      rs.Item("Manufacturer") = rst("ManufacturerName");
      if ( Trim(rst("Origin")) = ""{
        rs.Item("Origin")       = "N/A";
      }else{
        rs.Item("Origin")       = rst("Origin");
     }
      rs.Item("TRC")          = rst("TRC");
      rs.Item("TUnits")       = rst("TUnits");
      rs.Item("BRC")          = rst("BRC");
      rs.Item("BUnits")       = rst("BUnits");
      rs.Item("OtherRC")      = rst("OtherRC");
      rs.Item("OtherUnits")   = rst("OtherUnits");
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


public generateSubRS(string StartDate,string  EndDate){
  string sStartDate, sEndDate;
  DataReader rst, rs;
  string strSQL;
  //;
  //;
  sStartDate = "Convert(Char(8), Cast('" + StartDate + "' As DateTime), 112) ";
  sEndDate   = "Convert(Char(8), Cast('" + EndDate   + "' As DateTime), 112) ";
  //;
  strSQL =          " Select TaskDescription, Name AS FacilityName, ManufacturerName,                           ";
  strSQL +=  "       RCS=(SUM (CASE LevelType WHEN 'B' Then 1 Else 0 END) + SUM (CASE LevelType WHEN 'T' Then 1 Else 0 END)), ";
  strSQL +=  "       UNITS=(SUM (CASE LevelType WHEN 'T'){Units }else{ 0 END) + SUM (CASE LevelType WHEN 'B'){Units }else{ 0 END)) ";
  strSQL +=  "  From IRGOrigin                                                                          ";
  strSQL +=  "       INNER JOIN FacilityProductionDetail ON IRGOrigin.ID = FacilityProductionDetail.OriginID ";
  strSQL +=  "       INNER JOIN Facility ON FacilityProductionDetail.FacilityID = Facility.Id                ";
  strSQL +=  "       INNER JOIN Tasks ON FacilityProductionDetail.TaskId = Tasks.Id                          ";
  strSQL +=  "       INNER JOIN IRGManufacturer ON FacilityProductionDetail.ManufacturerID = IRGManufacturer.ID ";
  if (IsEmpty(Request["selFacilities"]){
     strSQL +=  " Where 1=1 ";
  }else{
     strSQL +=  " Where FacilityID IN (" + Request["selFacilities"] + ") ";
 }
  strSQL +=  "    AND Convert(Char(8), WorkDate, 112) Between  "  + sStartDate + " AND " + sEndDate + " ";
  strSQL +=  "  Group BY TaskCode, TaskDescription, Name,  ManufacturerName     ";
  strSQL +=  " HAVING (TaskCode = 'LO') OR  (TaskCode = 'UL')                               ";
  strSQL +=  "  Order BY TaskCode Desc, TaskDescription, Name, ManufacturerName ";

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
  rs.Fields.Append "Task",         129,   100, 64;
  rs.Fields.Append "Facility",     129,   100, 64;
  rs.Fields.Append "Manufacturer", 129,   100, 64;
  rs.Fields.Append "RC",             3,     4, 64;
  rs.Fields.Append "Units",          3,     4, 64;
  rs.Open;

  if(rst.EOF){
  }else{
    while (!rst.EOF ){
      rs.AddNew;
      rs.Item("Task")         = rst("TaskDescription");
      rs.Item("Facility")     = rst("FacilityName");
      rs.Item("Manufacturer") = Trim(rst("ManufacturerName"));
      rs.Item("RC")           = rst("RCS");
      rs.Item("Units")        = rst("Units");
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


  generateSubRS = rs;


}



        }
    }
}