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
    public partial class GenerateManufacturerSummary : PageBase
    {
        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


  GrantAccess("Super, Admin, User");




Session["LastStartDate"] = Request["FromDate"];
Session["LastEndDate"]   = Request["ToDate"];


// Main Report Recordset;
Set Session["Manufacturer"] = generateRS();

// Create Main Report;
CreateMainReport("Manufacturer");
SetStringParameter 1, Request["FromDate"];
SetStringParameter 2, Request["ToDate"];
SendReportToClient();




public generateRS(){

  string strSQL, rst, sWhere;

  sWhere =          " WHERE TaskCode IN ('LO', 'UL') ";
  sWhere = sWHere + "   AND (WorkDate Between '" + Request["FromDate"] + "' AND '" + Request["ToDate"] + "' ) ";
  //;
  if(IsEmpty(Request["selFacilities"]){
    sWhere = sWhere + "  AND  1=1 ";
  }else{
    sWhere = sWhere + "  AND FacilityID IN (" + Request["selFacilities"] + ") ";
 }
  //;

  strSQL +=  "SELECT  Task=Case TaskCode When 'LO'){'Load' When 'UL'){'Unload' }else{ 'N/A' End,  ";
  strSQL +=  "        Manufacturer=IsNull(Case RTrim(ManufacturerName) When ''){'N/A' }else{ ManufacturerName END, 'N/A'), SUM(Units) AS TU, COUNT(*) AS RCs,  ";
  strSQL +=  "        NU = Case NewUsed When 'N'){'New' When 'U'){'Used' }else{ 'N/A' End,        ";
  strSQL +=  "        CompanyName, RegionCode, Facility.Name                                            ";
  strSQL +=  "  FROM  IRGCompany RIGHT OUTER JOIN Facility ON IRGCompany.Id = Facility.IRGCompanyId     ";
  strSQL +=  "                   RIGHT OUTER JOIN FacilityProductionDetail ON Facility.Id = FacilityID  ";
  strSQL +=  "                   LEFT OUTER JOIN Tasks ON TaskId = Tasks.Id                             ";
  strSQL +=  "                   LEFT OUTER JOIN IRGManufacturer ON ManufacturerID = IRGManufacturer.ID ";
  strSQL +=  "                   LEFT OUTER JOIN IRGRegion ON Facility.RegionID = IRGRegion.ID          ";
  strSQL = strSQL + sWhere;
  strSQL +=  "  GROUP BY CompanyName, RegionCode, TaskCode, Facility.Name, IsNull(Case RTrim(ManufacturerName) When ''){'N/A' }else{ ManufacturerName END, 'N/A'), NewUsed    ";
  strSQL +=  "  ORDER BY CompanyName, RegionCode, TaskCode, Facility.Name, IsNull(Case RTrim(ManufacturerName) When ''){'N/A' }else{ ManufacturerName END, 'N/A'), NewUsed    ";
  //;
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
  rs.Fields.Append "Company",      129,   30, 64;
  rs.Fields.Append "Region",       129,   10, 64;
  rs.Fields.Append "Facility",     129,   30, 64;
  rs.Fields.Append "Task",         129,    6, 64;
  rs.Fields.Append "Manufacturer", 129,   60, 64;
  rs.Fields.Append "UsedNew",      129,    4, 64;
  rs.Fields.Append "Units",          3,    4, 64;
  rs.Fields.Append "RCs",            3,    4, 64;
  rs.Open;

  if(rst.EOF){
  }else{
    while (!rst.EOF ){
      rs.AddNew;
      rs.Item("Company")      = rst("CompanyName");
      rs.Item("Region")       = rst("RegionCode");
      rs.Item("Facility")     = rst("Name");
      rs.Item("Task")         = rst("Task");
      rs.Item("Manufacturer") = rst("Manufacturer");
      rs.Item("UsedNew")      = rst("NU");
      rs.Item("Units")        = rst("TU");
      rs.Item("RCs")          = rst("RCs");
      rs.Update;

    } //End Loop

    DBug = True;
    DBug = False;

    if(DBug){
      rs.MoveFirst;
      Response.Write(" ===  Start ===<br>");
      while (!rs.EOF){
        for( Each f in rs.Fields;
          Response.Write(f.Name + " = " + (f.Value) + " &nbsp; * &nbsp;&nbsp;");
        }
        Response.Write("<br>");

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