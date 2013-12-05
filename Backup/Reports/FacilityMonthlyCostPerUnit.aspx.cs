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
    public partial class FacilityMonthlyCostPerUnit : PageBase
    {

         public DataReader rsPay, rsUnits, rsCPU, rsUnLoadHours, rsUnLoadUnits, rsLoadHours, rsLoadUnits, rsShuttlingHours;
         public DataReader rsShuttlingUnits, rsTrainingHours,rsClericalHours,rsRBHours,rsUAHours,rsUM,rsBudgetCPU,rsATHours,rsATUnits,rsPrepHours,rsPrepUnits;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


          GrantAccess("Super, Admin, User");


          string selYear = Request["selYear"];
          string sFac    = System.Convert.ToString(Session["FacilityID"]);
          //;
          // Get Total Hours for( UNLOAD;

          rsUnLoadHours = new DataReader( getSQL(sFac, selYear,"UL", "HOURS"));
          rsUnLoadHours.Open();
          //;
          // Get Total Units for( UNLOAD;

          rsUnLoadUnits = new DataReader( getSQL(sFac, selYear,"UL", "UNITS"));
          rsUnLoadUnits.Open();
          //;
          // Get Total Hours for( LOAD;

          rsLoadHours= new DataReader( getSQL(sFac, selYear,"LO", "HOURS"));
          rsLoadHours.Open();
          //;
          // Get Total Units for( LOAD;

          rsLoadUnits= new DataReader(getSQL(sFac, selYear,"LO", "UNITS"));
          rsLoadUnits.Open();

          //;
          // Get Total Hours for( SHUTTLING;

          rsShuttlingHours= new DataReader(getSQL(sFac, selYear,"SH", "HOURS"));

          rsShuttlingHours.Open();
          //;
          // Get Total Units for( SHUTTLING;

          rsShuttlingUnits= new DataReader( getSQL(sFac, selYear,"SH", "UNITS"));

          rsShuttlingUnits.Open();
          //;
          // Get Total Hours for( Training;

          rsTrainingHours= new DataReader( getSQL(sFac, selYear,"TR", "HOURS"));
          rsTrainingHours.Open();
          //;
          // Get Total Hours for( Clerical;

          rsClericalHours= new DataReader(getSQL(sFac, selYear,"CL", "HOURS"));
           rsClericalHours.Open();
          //;
          // Get Total Hours for( Misc. (Rebilling);

          rsRBHours= new DataReader( getSQL(sFac, selYear,"RB", "HOURS"));
          rsRBHours.Open();
          //;
          // Get Total Hours for( UA;

          rsUAHours= new DataReader(getSQL(sFac, selYear,"UA", "HOURS"));
           rsUAHours.Open();
          //;
          // Get Total Units/Man;

          rsUM= new DataReader( getUMSQL(sFac, selYear));
          rsUM.Open();

          //;
          // Get Budget/Cost Per Unit;
          rsBudgetCPU= new DataReader( getBCPUSQL(sFac, selYear));
          rsBudgetCPU.Open();

          //;
          // Get Total Hours for( Air Test;

          rsATHours= new DataReader( getSQL(sFac, selYear,"AT", "HOURS"));
          rsATHours.Open();
          //;
          // Get Total Units for( Air Test;

          rsATUnits= new DataReader(getSQL(sFac, selYear,"AT", "UNITS"));
          rsATUnits.Open();

          //;
          // Get Total Hours for( Prepping;
          rsPrepHours= new DataReader( getSQL(sFac, selYear,"RP", "HOURS"));
          rsPrepHours.Open();
          //;
          // Get Total Units for( Prepping;
 
          rsPrepUnits= new DataReader( getSQL(sFac, selYear,"RP", "UNITS"));
          rsPrepUnits.Open();

          //// ==============================================================================;
          //// ==============================================================================;
          //// ==============================================================================;

          string sSourceXLS;
          string sDestXLS;
          string sGUID, sFileName;

          sGUID =  Guid.NewGuid().ToString();
          sFileName = "SpreadSheets\\_IRT_" + Mid(sGUID,2, 36) + ".xls";


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
          sSourceXLS = Server.MapPath(".") + "\\SpreadSheets\\Template_YTD_12.xls";
          sDestXLS = Server.MapPath(".") + "\\" + sFileName;
          File.Copy(sSourceXLS,sDestXLS);


          string OleDbConnectionSrtring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sDestXLS + ";Extended Properties=\"Excel 8.0;HDR=NO;\"";

          // FacilityName;
          DataWriterOleDb oCPU = new  DataWriterOleDb(OleDbConnectionSrtring);
          oCPU.Fill("Select * From FacilityAndYear");
  
          oCPU.Fields(0,cStr( Session["FacilityName"]));
          oCPU.Fields(6,"Year - ");
          oCPU.Fields(8, selYear);
          oCPU.Update();
          oCPU.Close();


          // Unloading;
          //;
          // Total Hours AND Total Units;
          //;
          oCPU = new  DataWriterOleDb(OleDbConnectionSrtring);
          oCPU.Fill("Select * From Unloading");

          while (! rsUnLoadHours.EOF){
            rsUnLoadHours.Read();
            oCPU.Fields(0,rsUnLoadHours.Fields(0));
            oCPU.Fields(1,rsUnLoadUnits.Fields(0));
            oCPU.Update();
          } //End Loop
          oCPU.Close();


          // Loading;
          //;
          // Total Hours AND Total Units;

          oCPU = new  DataWriterOleDb(OleDbConnectionSrtring);             
          oCPU.Fill("Select * From Loading");

          while (! rsLoadHours.EOF){
            rsLoadHours.Read();
            oCPU.Fields(0,rsLoadHours.Fields(0));
            oCPU.Fields(1, rsLoadUnits.Fields(0));
            oCPU.Update();

          } //End Loop
          oCPU.Close();

          //;
          // Total Hours AND Total Units;
          oCPU = new  DataWriterOleDb(OleDbConnectionSrtring); 
          oCPU.Fill("Select * From UM");

          while (! rsUM.EOF){
            rsUM.Read(); 
            if (cDbl(rsUM.Fields(0)) != 0 && cDbl(rsUM.Fields(1)) != 0 && cDbl(rsUM.Fields(2)) != 0){
              oCPU.Fields(0,cStr(cDbl(rsUM.Fields(0)) / cDbl(rsUM.Fields(1)) / cDbl(rsUM.Fields(2))));
              oCPU.Update();
           }

         } //End Loop
          oCPU.Close();

          // Shuttling;
          //;
          // Total Hours AND Total Units;
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
          oCPU.Fill("Select * From Training");

          while (! rsTrainingHours.EOF){
            rsTrainingHours.Read();
            oCPU.Fields(0, rsTrainingHours.Fields(0));
            oCPU.Update();
          } //End Loop
          oCPU.Close();

          //;
          // Clerical  Total Hours;
          oCPU = new  DataWriterOleDb(OleDbConnectionSrtring);
          oCPU.Fill("Select * From Clerical");

          while (! rsClericalHours.EOF){
            rsClericalHours.Read();
            oCPU.Fields(0,rsClericalHours.Fields(0));
            oCPU.Update();
          } //End Loop
          oCPU.Close();

          //;
          // Rebill  Total Hours;
          oCPU = new  DataWriterOleDb(OleDbConnectionSrtring);
          oCPU.Fill("Select * From Misc");

          while (! rsRBHours.EOF){
            rsRBHours.Read();
            oCPU.Fields(0,rsRBHours.Fields(0));
            oCPU.Update();
          } //End Loop
          oCPU.Close();

          //;
          // Non Rev.  Total Hours;
          oCPU = new  DataWriterOleDb(OleDbConnectionSrtring);
          oCPU.Fill("Select * From NonRev");

          while (! rsUAHours.EOF){
            rsUAHours.Read();
            oCPU.Fields(0,rsUAHours.Fields(0));
            oCPU.Update();
          } //End Loop
          oCPU.Close();
 
           //;
          // Budget / Cost Per Unit;
          oCPU = new  DataWriterOleDb(OleDbConnectionSrtring);
          oCPU.Fill("Select * From BudgetCPU");

          while (! rsBudgetCPU.EOF){
            rsBudgetCPU.Read();
            oCPU.Fields(0,rsBudgetCPU.Fields(0));
            oCPU.Fields(2,rsBudgetCPU.Fields(1));
            oCPU.Fields(4,rsBudgetCPU.Fields(2));
            oCPU.Update();

          } //End Loop
          oCPU.Close();

          //;
          // Air Test Total Hours AND Total Units;
          oCPU = new  DataWriterOleDb(OleDbConnectionSrtring);
          oCPU.Fill( "Select * From AirTest");

          while (! rsATHours.EOF){
            rsATHours.Read();
            oCPU.Fields(0, rsATUnits.Fields(0));
            oCPU.Fields(1, rsATHours.Fields(0));
            oCPU.Update();
          } //End Loop
          oCPU.Close();

          //;
          // Prepping Total Hours AND Total Units;
          oCPU = new  DataWriterOleDb(OleDbConnectionSrtring);
          oCPU.Fill("Select * From Prepping");

          while (! rsPrepHours.EOF){
            rsPrepHours.Read();
            oCPU.Fields(0,rsPrepUnits.Fields(0));
            oCPU.Fields(1, rsPrepHours.Fields(0));
            oCPU.Update();
          } //End Loop
          oCPU.Close();

          Response.Redirect(sFileName);

            if ( File.Exists(sDestXLS)){
               File.Delete(sDestXLS);
            }

          }
                
        public string getSQL(string sFac,string sYear,string sTask,string sPayOrUnits){

          string strSQL = "";

          for( int I=1; I < 12; I ++){
            if(sPayOrUnits == "UNITS"){
              strSQL += "SELECT SUM(d.Units) AS TotalUnits, Mon=" + cStr(I);
              strSQL +=  " FROM Facility f INNER JOIN FacilityProductionDetail d ON f.Id = d.FacilityID ";
              strSQL +=  "      INNER JOIN Tasks ON d.TaskId = Tasks.Id  ";
            }else{
               strSQL += "SELECT SUM(HoursPaid) AS TotalHours, Mon=" + cStr(I);
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
            strSQL +=  "      AND DatePart(m,WorkDate) = " + cStr(I) + " AND  DatePart(yyyy,WorkDate) = " + cStr(sYear);

            if (I < 12){
              strSQL +=  " UNION ";
           }
          }

          strSQL +=  "ORDER BY Mon ";

          return strSQL;

        }

        public string getUMSQL(string sFac,string sYear){

          string strSQL = "";
          string sWhere = "";

          for(int I=1; I <12; I ++){
            sWhere = " Where Datepart(yyyy,WorkDate)=" + sYear + " AND DatePart(m,Workdate)=" + cStr(I) + " AND FacilityID=" + sFac +  " AND TaskID IN (1,2)) ";

            strSQL +=  " SELECT TotalUnits=IsNULL((Select Sum(Units) From FacilityProductionDetail " + sWhere + ",0), ";
            strSQL +=  " TotalMen=IsNULL((Select Count(Distinct EmployeeID) From EmployeeTaskWorked "  + sWhere + ",0), ";
            strSQL +=  " TotalDays=IsNULL((Select Count(Distinct Datepart(d,WorkDate)) From EMployeeTaskWorked " + sWhere + ",0), Mon=" + cStr(I);

            if (I < 12 ){
              strSQL +=  " UNION ";
            }
          }

          strSQL +=  "ORDER BY Mon ";

          return strSQL;

        }

        public string getBCPUSQL(string sFac,string sYear){

          string strSQL = "";
          string sWhereUnits, sWherePay;

          for(int  I=1; I < 12; I++){
            sWhereUnits = "  Datepart(yyyy,WorkDate)=" + sYear + " AND DatePart(m,Workdate)=" + cStr(I) + " AND FacilityID=" + sFac +  " AND TaskID IN (1,2)) ";
            sWherePay = "  Datepart(yyyy,WorkDate)=" + sYear + " AND DatePart(m,Workdate)=" + cStr(I) + " AND FacilityID=" + sFac +  " ) ";

            strSQL +=  "SELECT TotalUnits=IsNULL((Select Sum(Units) From FacilityProductionDetail WHERE " + sWhereUnits + ",0), ";
            strSQL +=  "       TotalRegHours=IsNULL((Select Sum(PayAmount) ";
            strSQL +=  "          FROM EmployeeTaskWorkedPay INNER JOIN EmployeeTaskWorked ON EmployeeTaskWorkedPay.EmployeeTaskWorkedId = EmployeeTaskWorked.Id ";
            strSQL +=  "         WHERE PayMultiplier=1 AND "  + sWherePay + ",0), ";
            strSQL +=  "       TotalOTHours=IsNULL((Select Sum(PayAmount) ";
            strSQL +=  "          FROM EmployeeTaskWorkedPay INNER JOIN EmployeeTaskWorked ON EmployeeTaskWorkedPay.EmployeeTaskWorkedId = EmployeeTaskWorked.Id ";
            strSQL +=  "         WHERE PayMultiplier!=1 AND "  + sWherePay + ",0),  Mon=" + cStr(I);

            if (I < 12){
              strSQL +=  " UNION ";
            }
          }

          strSQL +=  "ORDER BY Mon ";
          return strSQL;

        }

    }
}