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
using System.IO;

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
    public partial class FacilityDailyCostPerUnit : PageBase
    {

        public DataReader rsPay, rsUnits, rsCPU;
        public DataReader rsUnLoadHours,rsUnLoadUnits,rsLoadHours,rsLoadUnits,rsShuttlingHours,rsShuttlingUnits,rsTrainingHours,rsClericalHours;
        public DataReader rsRBHours,rsUAHours,rsUM,rsUMLO,rsBudgetCPU,rsATHours,rsATUnits,rsPrepHours,rsPrepUnits;
        public string selYear, selMonth, sFac;
        public string sSourceXLS;
        public string sDestXLS;
        public string sGUID, sFileName;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


          GrantAccess("Super, Admin, User");

          selYear  = Request["selYear2"];
          selMonth = Request["selMonth"];
          sFac     = cStr(Session["FacilityID"]);

          //;
          // Get Total Hours for UNLOAD;
          //;

          rsUnLoadHours = new DataReader(getSQL(sFac, selMonth, selYear,"UL", "HOURS"));
          rsUnLoadHours.Open();
          //;
          // Get Total Units for UNLOAD;
          //;

          rsUnLoadUnits = new DataReader(getSQL(sFac, selMonth, selYear,"UL", "UNITS"));
          rsUnLoadUnits.Open();
         ////////;
         //////// Get Total Hours for LOAD;
         ////////;

         rsLoadHours = new DataReader(getSQL(sFac, selMonth, selYear,"LO", "HOURS"));
         rsLoadHours.Open();
         //;
         // Get Total Units for LOAD;
         //;

         rsLoadUnits = new DataReader(getSQL(sFac, selMonth, selYear,"LO", "UNITS"));
         rsLoadUnits.Open();

         //;
         // Get Total Hours for SHUTTLING;
         //;

         rsShuttlingHours = new DataReader(getSQL(sFac, selMonth, selYear,"SH", "HOURS"));
         rsShuttlingHours.Open();
         //;
         // Get Total Units for SHUTTLING;
         //;

         rsShuttlingUnits = new DataReader(getSQL(sFac, selMonth, selYear,"SH", "UNITS"));
         rsShuttlingUnits.Open();
         //;
         // Get Total Hours for( Training;
         //;

         rsTrainingHours = new DataReader(getSQL(sFac, selMonth, selYear,"TR", "HOURS"));
         rsTrainingHours.Open();
         //;
         // Get Total Hours for Clerical;
         //;

         rsClericalHours = new DataReader(getSQL(sFac, selMonth, selYear,"CL", "HOURS"));
         rsClericalHours.Open();
         //;
         // Get Total Hours for Misc. (Rebilling);
         //;

         rsRBHours = new DataReader(getSQL(sFac, selMonth, selYear,"RB", "HOURS"));

         rsRBHours.Open();
         //;
         // Get Total Hours for UA;
         //;

         rsUAHours = new DataReader(getSQL(sFac, selMonth, selYear,"UA", "HOURS"));
         rsUAHours.Open();
         //;
         // Get Total Units/Man for UL;
         //;

         rsUM = new DataReader(getUMSQL(sFac, selMonth, selYear, "UL"));
         rsUM.Open();
         //;
         // Get Total Units/Man for UL;
         //;
         rsUMLO = new DataReader(getUMSQL(sFac, selMonth, selYear, "LO"));
         rsUMLO.Open();

         //;
         // Get Budget/Cost Per Unit;
         //;
         rsBudgetCPU = new DataReader(getBCPUSQL(sFac, selMonth, selYear));
         rsBudgetCPU.Open();

         //;
         // Get Total Hours for Air Test;
         //;
         rsATHours = new DataReader(getSQL(sFac, selMonth, selYear,"AT", "HOURS"));
         rsATHours.Open();
         //;
         // Get Total Units for Air Test;
         //;

         rsATUnits = new DataReader(getSQL(sFac, selMonth, selYear,"AT", "UNITS"));
         rsATUnits.Open();

         //;
         // Get Total Hours for Prepping;
         //;

         rsPrepHours = new DataReader(getSQL(sFac, selMonth, selYear,"RP", "HOURS"));
         rsPrepHours.Open();
         //;
         // Get Total Units for Prepping;
         //;

         rsPrepUnits = new DataReader(getSQL(sFac, selMonth, selYear,"RP", "UNITS"));
         rsPrepUnits.Open();

         ////////// ==============================================================================;

          string sGUID = new Guid().ToString(); // Server.CreateObject("Scriptlet.TypeLib").GUID;
          string sFileName = "SpreadSheets\\_IRT_" + Mid(sGUID,2, 36) + ".xls";


          // Delete old temp spreadsheets;
          string path = (Server.MapPath(".") + "\\SpreadSheets\\");
          foreach(string file in Directory.GetFiles(path)){
            if(Left(file, 5) == "_IRT_"){
                if( System.DateTime.Now.Subtract(File.GetCreationTime(path + "\\" + file)).Days >= 1){
                    File.Delete(path + "\\" +  file);
                }
            }
          }

          //Copy the source workbook file (the "template") to the destination filename;
          sSourceXLS = Server.MapPath(".") + "\\SpreadSheets\\Template_Daily.xls";
          sDestXLS = Server.MapPath(".") + "\\" + sFileName;
          File.Copy(sSourceXLS,sDestXLS);


          string OleDbConnectionSrtring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sDestXLS + ";Extended Properties=\"Excel 8.0;HDR=NO;\"";

          // FacilityName;
          DataWriterOleDb oCPU = new  DataWriterOleDb(OleDbConnectionSrtring);
          oCPU.Fill("Select * From DateAndFacility");
  
          oCPU.Fields(0,Left(MonthName(cInt(selMonth)), 3));
          oCPU.Fields(1,selYear);
          oCPU.Fields(3, cStr(Session["FacilityName"]));
          oCPU.Update();
          oCPU.Close();


          // Unloading;
          //;
          // Total Hours AND Total Units;
          oCPU = new  DataWriterOleDb(OleDbConnectionSrtring);
          oCPU.Fill("Select * From Unloading");

          while (! rsUnLoadHours.EOF){
            rsUnLoadHours.Read();
            oCPU.Fields(0,rsUnLoadHours.Fields(1) + "-" + Left(MonthName(cInt(selMonth)), 3) + "  ");
            oCPU.Fields(1, rsUnLoadHours.Fields(0));
            oCPU.Fields(2, rsUM.Fields(1));
            oCPU.Fields(3, rsUnLoadUnits.Fields(0));
            oCPU.Update();

          } //End Loop
          oCPU.Close();

         //////// Loading;
         //////// Total Hours AND Total Units;
         oCPU = new  DataWriterOleDb(OleDbConnectionSrtring);
         oCPU.Fill("Select * From Loading");

         while (! rsLoadHours.EOF){
            rsLoadHours.Read();
            oCPU.Fields(0,rsLoadHours.Fields(0));
            oCPU.Fields(1,rsUMLO.Fields(1));
            oCPU.Fields(2, rsLoadUnits.Fields(0));
            oCPU.Update();
         } //End Loop
         oCPU.Close();

         //;
         // Shuttling;
         //;
         // Total Hours && Total Units;
         oCPU = new  DataWriterOleDb(OleDbConnectionSrtring);
         oCPU.Fill("Select * From Shuttling");

         while (! rsShuttlingHours.EOF){
           rsShuttlingHours.Read();
           oCPU.Fields(0,rsShuttlingHours.Fields(0));
           oCPU.Fields(1,rsShuttlingUnits.Fields(0));
           oCPU.Update();
         } //End Loop
         oCPU.Close();

         //;
         // Training  Total Hours;
         oCPU = new  DataWriterOleDb(OleDbConnectionSrtring);
         oCPU.Fill("Select * From MiscellaneousHours");

         while (! rsTrainingHours.EOF){
           rsTrainingHours.Read();
           oCPU.Fields(0, rsTrainingHours.Fields(0));
           oCPU.Fields(1, rsClericalHours.Fields(0));
           oCPU.Fields(2, rsRBHours.Fields(0));
           oCPU.Fields(3, rsUAHours.Fields(0));
           oCPU.Update();
         } //End Loop
         oCPU.Close();


         //;
         // Budget / Cost Per Unit;
         oCPU = new  DataWriterOleDb(OleDbConnectionSrtring);
         oCPU.Fill("Select * From Budget");

         while (! rsBudgetCPU.EOF){
           rsBudgetCPU.Read();
           oCPU.Fields(0, rsBudgetCPU.Fields(0));
           oCPU.Fields(1, rsBudgetCPU.Fields(1));
           oCPU.Fields(2, rsBudgetCPU.Fields(2));
           oCPU.Update();
         } //End Loop
         oCPU.Close();

         //;
         // Air Test Total Hours && Total Units;
         oCPU = new  DataWriterOleDb(OleDbConnectionSrtring);
         oCPU.Fill("Select * From AirTest");

         while (! rsATHours.EOF){
           rsATHours.Read();
           oCPU.Fields(0, rsATUnits.Fields(0));
           oCPU.Fields(1, rsATHours.Fields(0));
           oCPU.Update();
         } //End Loop
         oCPU.Close();

         //;
         // Prepping Total Hours && Total Units;
         oCPU = new  DataWriterOleDb(OleDbConnectionSrtring);
         oCPU.Fill("Select * From Prepping");

         while (! rsPrepHours.EOF){
           rsPrepHours.Read(); 
           oCPU.Fields(0, rsPrepUnits.Fields(0));
           oCPU.Fields(1, rsPrepHours.Fields(0));
           oCPU.Update();
         } //End Loop
         oCPU.Close();

         Response.Redirect(sFileName);


         if (File.Exists(sDestXLS)){
            File.Delete(sDestXLS);
         }

         Response.End();

}
        
        public string  getSQL(string sFac,string  selMonth,string  selYear,string  sTask,string  sPayOrUnits){

          string strSQL = "";
          DateTime ThisMonthStart, ThisMonthEnd;

          ThisMonthStart = cDate(selMonth + "/1/" + selYear);
          ThisMonthEnd = ThisMonthStart.AddMonths(1);
          int nDays = ThisMonthStart.Subtract(ThisMonthEnd).Days;

          for( int I=1; I < nDays; I ++){
            if(sPayOrUnits == "UNITS"){
              strSQL = " SELECT SUM(d.Units) AS TotalUnits, TheDay=" + cStr(I);
              strSQL +=  " FROM Facility f INNER JOIN FacilityProductionDetail d ON f.Id = d.FacilityID ";
              strSQL +=  "      INNER JOIN Tasks ON d.TaskId = Tasks.Id  ";
            }else{
               strSQL = " SELECT SUM(HoursPaid) AS TotalHours, TheDay=" + cStr(I);
               strSQL +=  " FROM Facility f INNER JOIN EmployeeTaskWorked ON f.Id = EmployeeTaskWorked.FacilityID ";
               strSQL +=  "      INNER JOIN Tasks ON EmployeeTaskWorked.TaskID = Tasks.Id   ";
               strSQL +=  "      LEFT OUTER JOIN EmployeeTaskWorkedPay ON EmployeeTaskWorked.Id = EmployeeTaskWorkedPay.EmployeeTaskWorkedId ";
           }

            if(sTask == "ALL"){
               strSQL +=  "WHERE (1=1) ";
            }else{
                if(sTask == "SH"){
                   strSQL +=  "WHERE (Tasks.TaskCode In ('SH', 'SU') ) ";
                }else{
                   if(sTask == "RB"){
                        strSQL +=  "WHERE (Tasks.Rebillable = 1) ";
                   }else{
                        strSQL +=  "WHERE (Tasks.TaskCode = '" + sTask + "') ";
                    }
                }
             }
            strSQL +=  "      AND f.ID = " + sFac;
            strSQL +=  "      AND DatePart(d,WorkDate) = " + cStr(I);
            strSQL +=  "      AND DatePart(m,WorkDate) = " + cStr(selMonth);
            strSQL +=  "      AND DatePart(yyyy,WorkDate) = " + cStr(selYear);

            if ( I < nDays ){
              strSQL +=  " UNION ";
            }
          }

          strSQL +=  "ORDER BY TheDay ";

          return strSQL;

        }

        public string getUMSQL(string sFac,string  selMonth,string  selYear,string  sTask){

          string strSQL = "";
          string sWhere = "";

          DateTime ThisMonthStart, ThisMonthEnd;

          ThisMonthStart = cDate(selMonth + "/1/" + selYear);
          ThisMonthEnd = ThisMonthStart.AddMonths(1);
          int nDays = ThisMonthStart.Subtract(ThisMonthEnd).Days;

          for(int I=1; I < nDays; I++){
            if (sTask == "LO"){
              sWhere = " Where Datepart(yyyy,WorkDate)=" + selYear + " AND DatePart(m,Workdate)=" + cStr(selMonth) + " AND DatePart(d,Workdate)=" + cStr(I) + " AND FacilityID=" + sFac +  " AND TaskID IN (1)) ";
            }else{
              sWhere = " Where Datepart(yyyy,WorkDate)=" + selYear + " AND DatePart(m,Workdate)=" + cStr(selMonth) + " AND DatePart(d,Workdate)=" + cStr(I) + " AND FacilityID=" + sFac +  " AND TaskID IN (2)) ";
           }
            strSQL +=  "SELECT TotalUnits=IsNULL((Select Sum(Units) From FacilityProductionDetail " + sWhere + ",0), ";
            strSQL +=  "       TotalMen=IsNULL((Select Count(Distinct EmployeeID) From EmployeeTaskWorked "  + sWhere + ",0), ";
            strSQL +=  "       TotalDays=IsNULL((Select Count(Distinct Datepart(d,WorkDate)) From EmployeeTaskWorked " + sWhere + ",0), TheDay=" + cStr(I);

            if (I < nDays){
              strSQL +=  " UNION ";
            }
          }

          strSQL +=  "ORDER BY TheDay ";
          return strSQL;

        }

        public string getBCPUSQL(string sFac,string  selMonth,string  selYear){

          string strSQL = "";
          string sWhereUnits, sWherePay;

          DateTime ThisMonthStart, ThisMonthEnd;

          ThisMonthStart = cDate(selMonth + "/1/" + selYear);
          ThisMonthEnd = ThisMonthStart.AddMonths(1);
          int nDays = ThisMonthStart.Subtract(ThisMonthEnd).Days;

          for(int I=1; I < nDays; I++){

            sWhereUnits = "  Datepart(yyyy,WorkDate)=" + selYear + " AND DatePart(m,Workdate)=" + cStr(selMonth) + " AND DatePart(d,Workdate)=" + cStr(I) + " AND FacilityID=" + sFac +  " AND TaskID IN (1,2)) ";
            sWherePay   = "  Datepart(yyyy,WorkDate)=" + selYear + " AND DatePart(m,Workdate)=" + cStr(selMonth) + " AND DatePart(d,Workdate)=" + cStr(I) + " AND FacilityID=" + sFac +  " ) ";

            strSQL +=  "SELECT TotalUnits = IsNULL((Select Sum(Units) From FacilityProductionDetail WHERE " + sWhereUnits + ",0), ";
            strSQL +=  "       TotalRegHours = IsNULL((Select Sum(PayAmount) ";
            strSQL +=  "          FROM EmployeeTaskWorkedPay INNER JOIN EmployeeTaskWorked ON EmployeeTaskWorkedPay.EmployeeTaskWorkedId = EmployeeTaskWorked.Id ";
            strSQL +=  "         WHERE PayMultiplier = 1 AND "  + sWherePay + ",0), ";
            strSQL +=  "       TotalOTHours = IsNULL((Select Sum(PayAmount) ";
            strSQL +=  "          FROM EmployeeTaskWorkedPay INNER JOIN EmployeeTaskWorked ON EmployeeTaskWorkedPay.EmployeeTaskWorkedId = EmployeeTaskWorked.Id ";
            strSQL +=  "         WHERE PayMultiplier <> 1 AND "  + sWherePay + ",0),  TheDay=" + cStr(I);

            if ( I < nDays ){
              strSQL +=  " UNION ";
            }
          }

          strSQL +=  "ORDER BY TheDay ";
          return strSQL;

        }

    }
}