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
    public partial class FacilityWeeklyCostPerUnit  : PageBase
    {

        public DataReader rs;
        public DataReader rsPay, rsUnits, rsCPU, rsFacTasks, rsTaskPay;
        public int rs_numRows = 0;
        public string sFirstDay, sLastDay, selYear, sFac;
        public double fBudgetedCPU;
        public bool bHaveData = false;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin, User");

            selYear = Request["selYear"];
            sFac    = cStr(Session["FacilityID"]);
            //;
            // First get the First day && Last day for( the weekly CPU range;
            // First Friday of the year && last Thursday following Last Friday of the year;
            //;
            rs = new DataReader(" Exec CPURange " + selYear);
            rs_numRows = 0;

            sFirstDay = rs.Item("FirstDay");
            sLastDay  = rs.Item("LastDay");


              rsCPU = new DataReader(" SELECT SUM(BudgetedCPU) AS BudgetedCPU FROM  FacilityAnnualBudgetTask Where facilityID = " + sFac + " AND ReportingYear = '" + selYear + "' ");

              rsCPU.Open();

              if (!rsCPU.EOF ){
                 rsCPU.Read();
                 fBudgetedCPU = cDbl(rsCPU.Item("BudgetedCPU"));
              }else{
                 fBudgetedCPU = 0.0;
              }


              //;
              // Get Total Units for( Load && Unload Tasks;
              //;
              rsUnits = new DataReader(getSQL(sFac, sFirstDay, sLastDay, "'LO', 'UL'", "UNITS"));
              rsUnits.Open();

  

              while (!rsUnits.EOF ){
                rsUnits.Read();
                if (rsUnits.Item("TotalUnits") == "" || cInt(rsUnits.Item("TotalUnits")) > 0){
                   bHaveData = true;
                }
              } //End Loop

              if ( bHaveData ){
                rsUnits.Requery();
              }else{

                rsUnits = new DataReader(getSQL(sFac, sFirstDay, sLastDay, "ALL", "UNITS"));
                rsUnits.Open();
             }

              //;
              // Get all OTHER tasks (exclude LO && UL) for( this facility;
              //;
              string strSQL = " Select TASK=RTrim(TaskCode) + ' - ' + RTrim(TaskDescription) ";
              strSQL +=  "  FROM FacilityTasks INNER JOIN Tasks ON TaskId = Tasks.Id ";
              strSQL +=  " WHERE TaskCode NOT IN ('LO', 'UL') ";
              strSQL +=  " AND FacilityID = " + sFac;
              strSQL +=  " Order By TaskCode ";

              rsCPU = new DataReader(strSQL);
              rsCPU.Open();

              int i = 0;            
              string[] arFac = new string[19];
              for(i=0; i < UBound(arFac); i ++){
                arFac[i] = "";
              }

              if( bHaveData){
                arFac[0] = "UL - Unload";
                arFac[1] = "LO - Load";
                i=2;
              }else{
                i=0;
              }

              while (!rsCPU.EOF ){
                  rsCPU.Read(); 
                arFac[i] = rsCPU.Item("Task");
                i=i+1;
              } //End Loop

              int nFacTasks = i;


              // Get Total Pay for( ALL Tasks;
              //;
              rsPay = new DataReader(getSQL(sFac, sFirstDay, sLastDay, "ALL", "PAY"));
              rsPay.Open();

              string sSourceXLS;
              string sDestXLS;
              string sGUID, sFileName;

              sGUID = System.Guid.NewGuid().ToString();
              sFileName = "SpreadSheets\\_IRT_" + Mid(sGUID,2, 36) + ".xls";

              // Delete old temp spreadsheets >= a day old.
              string path = (Server.MapPath(".") + "\\SpreadSheets\\");
              foreach(string file in Directory.GetFiles(path)){
                  if(Left(file, 5) == "_IRT_"){
                      if( System.DateTime.Now.Subtract(File.GetCreationTime(path + "\\" + file)).Days >= 1){
                          File.Delete(path + "\\" +  file);
                      }
                  }
              }

              //Copy the source workbook file (the "template") to the destination filename;
              sSourceXLS = Server.MapPath(".") + "\\SpreadSheets\\Template_FacilityWeeklyCPU.xls";
              sDestXLS = Server.MapPath(".") + "\\" + sFileName;

              File.Copy(sSourceXLS,sDestXLS);

              string OleDbConnectionSrtring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sDestXLS + ";Extended Properties=\"Excel 8.0;HDR=NO;\"";

              DataWriterOleDb oCPU = new  DataWriterOleDb(OleDbConnectionSrtring);
              oCPU.Fill("Select * From FacilityName");

              // FacilityName;
              //;
              oCPU.Fields(0, cStr(Session["FacilityName"]));
              oCPU.Update();
              oCPU.Close();


              // BudgetdCPU value;
              oCPU.Fill("Select * From BudgetedCPU");
              oCPU.Fields(0, cStr(fBudgetedCPU));
              oCPU.Update();
              oCPU.Close();


              // YearCPU value;
              oCPU.Fill("Select * From YearCPU");
              oCPU.Fields(0 , selYear);
              oCPU.Update();
              oCPU.Close();


              // TotalPay && TotalUnits;
              oCPU.Fill("Select * From WeeklyCPU");

              while (! rsPay.EOF){
                  rsPay.Read();
                oCPU.Fields(0, rsPay.Fields(1));
                oCPU.Fields(1,  cStr(cDate(rsPay.Fields(1)).AddDays(6)));
                oCPU.Fields(2, rsPay.Fields(0));
                oCPU.Fields(3, rsUnits.Fields(0));
                oCPU.Update();
              } //End Loop
              oCPU.Close();


              //;
              //Get Pay for( all tasks;
              //;
              int nFacTasksWithValues = nFacTasks;

              string[] arFacWithValues = new string[19];
              for(i=0 ; i < UBound(arFacWithValues); i++){
                arFacWithValues[i] = "";
              }

              int k = -1;
              int l = 1;

              for(i=0 ; i < nFacTasks;i ++){

                rsTaskPay = new DataReader( getSQL(sFac, sFirstDay, sLastDay, "'" + Left(arFac[i],2) + "'", "PAY"));
                rsTaskPay.Open();

                //;
                // Add TotalPay -;
                //;
                oCPU.Fill("Select * From Task" + cStr(l));
                bHaveData = false;

                 while ( !rsTaskPay.EOF){
                     rsTaskPay.Read();
                     if (rsTaskPay.Item("TotalPay") == "" || cInt(rsTaskPay.Item("TotalPay")) > 0){
                         bHaveData = true;
                    }
                } //End Loop

                if ( bHaveData){
                  rsTaskPay.Requery();
                  while (! rsTaskPay.EOF){
                    rsTaskPay.Read();
                    oCPU.Fields(0,rsTaskPay.Fields(0));
                    oCPU.Update();
                  } //End Loop

                  k = k + 1;
                  arFacWithValues[k] = arFac[i];
                  l=l+1;
               }
                oCPU.Close();

              } //End For

              //;
              // Task Headings;
              //;
              oCPU.Fill("Select * From TaskTitles");

              i=2;
              int j=-1;

              for( i=0 ; i < UBound(arFacWithValues); i ++){

                j=j+1;

                //Response.Write("i= " + i + " --- " + j  + ", " + (j+1) + "<br>";
                oCPU.Fields(j, arFacWithValues[i]);
                oCPU.Fields((j+1) ,arFacWithValues[i]);
                j=j+1;

              }
              oCPU.Update();
              oCPU.Close();

              //;
              // Clear Extra headers (beyond existing tasks for( facility);
              //;
              oCPU.Fill("Select * From Headers");

              for (int z = 0; z < oCPU.RecordCount; z++ )
              {
                  // Skip over headers for( which we have a task;
                  //;
                  for (j = (k + 1) * 2; j < oCPU.Columns.Count - 1; j++)
                  {
                      oCPU.Fields(j, "DELETE_THIS");
                  }
                  oCPU.Update();
              } //End Loop


              Response.Redirect(sFileName);


            }

        public string getSQL(string sFac,string sFirstDay,string  sLastDay,string  sTask,string sPayOrUnits) {

          string strSQL = "";
          string sStartDate, sEndDate;

          sStartDate = sFirstDay;
          sEndDate   = cStr(cDate(sFirstDay).AddDays(6));

           while (cDate(sEndDate) <= cDate(sLastDay)){

            if(sPayOrUnits == "UNITS"){
               strSQL = " SELECT SUM(d.Units) AS TotalUnits, StartDate=Convert(SmallDateTime,'" + sStartDate + "' ,101)";
               strSQL +=  " FROM Facility f INNER JOIN FacilityProductionDetail d ON f.Id = d.FacilityID ";
               strSQL +=  "      INNER JOIN Tasks ON d.TaskId = Tasks.Id  ";
            }else{
               strSQL = " SELECT SUM(PayAmount) AS TotalPay, StartDate=Convert(SmallDateTime,'" + sStartDate + "' ,101)";
               strSQL +=  " FROM Facility f INNER JOIN EmployeeTaskWorked ON f.Id = EmployeeTaskWorked.FacilityID ";
               strSQL +=  "      INNER JOIN Tasks ON EmployeeTaskWorked.TaskID = Tasks.Id   ";
               strSQL +=  "      LEFT OUTER JOIN EmployeeTaskWorkedPay ON EmployeeTaskWorked.Id = EmployeeTaskWorkedPay.EmployeeTaskWorkedId ";
           }

            if(sTask == "ALL"){
               strSQL +=  " WHERE (1=1) ";
            }else{
               strSQL +=  " WHERE (Tasks.TaskCode IN (" + sTask + ") ) ";
           }
            strSQL +=  "      AND f.ID = " + sFac;
            strSQL +=  "      AND (WorkDate Between '" + sStartDate + "' AND '" + sEndDate + "') ";

            sStartDate = cStr(cDate(sEndDate).AddDays(1));
            sEndDate   = cStr(cDate(sStartDate).AddDays(6));

            if (cDate(sEndDate) <= cDate(sLastDay)){
              strSQL +=  " UNION ";
           }
          } //End Loop

          strSQL +=  "ORDER BY StartDate ";

          return strSQL;

        }


    }
}