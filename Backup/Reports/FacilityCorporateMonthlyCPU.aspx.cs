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
    public partial class FacilityCorporateMonthlyCPU : PageBase
    {

          public DataReader rsPay, rsUnits, rsCPU, regionName;
          public DataReader rsUnLoadHours,rsUnLoadUnits,rsLoadHours, rsLoadUnits, rsUM, rsShuttlingHours;
          public DataReader rsShuttlingUnits, rsSpottingHours, rsSpotting, rsTrainingHours, rsClericalHours, rsRBHours, rsUAHours, rsSelfAudit, rsKPI;
          public DataReader rsBudgetCPUTU, rsBudgetCPURH, rsBudgetCPUOT, rsATHours, rsATUnits, rsPrepHours, rsPrepUnits, rsTotalUnits;
          public string[] arrTotalAccidents = new string[12];
          public string[] arrTotalInjuryRep = new string[12];
          public string[] arrTotalLostTime = new string[12];
          public string[] arrTotalNonMed = new string[12];
          public string selYear;

        public double unLoadTotalHours = 0.0;
        public double unLoadTotalUnits = 0.0;

        public double LoadTotalHours = 0.0;
        public double LoadTotalUnits = 0.0;

        public double UMTotals  = 0.0;
        public double nUMTotals = 0.0;

        public double ShuttlingTotalHours = 0.0;
        public double ShuttlingTotalUnits = 0.0;

        public double TrainingTotalHours = 0.0;
        public double ClericalTotalHours = 0.0;
        public double RBTotalHours = 0.0;
        public double UATotalHours = 0.0;

        public double BudgetTotalUnits = 0.0;
        public double BudgetTotalHours = 0.0;
        public double BudgetTotalOTHrs = 0.0;

        public double ATTotalUnits = 0.0;
        public double ATTotalHours = 0.0;

        public double PrepTotalUnits = 0.0;
        public double PrepTotalHours = 0.0;

        public double RCs_In   = 0.0;
        public double RCs_Out  = 0.0;
        public double DownTime = 0.0;
        public double SpottingTotalHours = 0.0;
        public double Total_RC_Hr = 0.0;

        public double SA_Bay         = 0.0;
        public double SA_Fac         = 0.0;
        public double Total_SA_Bay   = 0.0;
        public double Total_SA_Fac   = 0.0;
        public double Total_SC_Bay   = 0.0;
        public double Total_SC_Fac   = 0.0;

        public double Total_Accidents = 0.0;
        public double Total_Injuries  = 0.0;
        public double Total_LostTime  = 0.0;
        public double Total_NonMed   = 0.0;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


          GrantAccess("Super, Admin, User");

          selYear = Request["selYear"];


          //;
          // Get Total Hours for UNLOAD;
          //;
          rsUnLoadHours = new DataReader(getSQL(selYear,"UL", "HOURS"));
          rsUnLoadHours.Open();
          rsUnLoadHours.Read();


          //;
          // Get Total Units for UNLOAD;
          //;
          rsUnLoadUnits = new DataReader(getSQL(selYear,"UL", "UNITS"));
          rsUnLoadUnits.Open();
          rsUnLoadUnits.Read();



          //;
          // Get Total Hours for LOAD;
          //;
          rsLoadHours = new DataReader(getSQL(selYear,"LO", "HOURS"));
          rsLoadHours.Open();
          rsLoadHours.Read();


          //;
          // Get Total Units for LOAD;
          //;
          rsLoadUnits = new DataReader(getSQL(selYear,"LO", "UNITS"));
          rsLoadUnits.Open();
          rsLoadUnits.Read();


          //;
          // Get Total Units/Man;
          //;
          rsUM = new DataReader(getUMSQL(selYear));
          rsUM.Open();
          rsUM.Read();



          //;
          // Get Total Hours for SHUTTLING;
          //;
          rsShuttlingHours = new DataReader(getSQL(selYear,"SH", "HOURS"));
          rsShuttlingHours.Open();
          rsShuttlingHours.Read();



          //;
          // Get Total Units for SHUTTLING;
          //;
          rsShuttlingUnits = new DataReader(getSQL(selYear,"SH", "UNITS"));
          rsShuttlingUnits.Open();
          rsShuttlingUnits.Read();



          //;
          // Get Total Hours for SPOTTING;
          //;
          rsSpottingHours = new DataReader(getSQL(selYear,"SP", "HOURS"));
          rsSpottingHours.Open();
          rsSpottingHours.Read();



          //;
          //;
          // Get SPOTTING RC's IN/OUT and Down Time;
          rsSpotting = new DataReader(getSpotSQL(selYear));
          rsSpotting.Open();
          rsSpotting.Read();


          //;
          // Get Total Hours for Training;
          //;
          rsTrainingHours = new DataReader(getSQL(selYear,"TR", "HOURS"));
          rsTrainingHours.Open();
          rsTrainingHours.Read();


          //;
          // Get Total Hours for Clerical;
          //;
          rsClericalHours = new DataReader(getSQL(selYear,"CL", "HOURS"));
          rsClericalHours.Open();
          rsClericalHours.Read();


          //;
          // Get Total Hours for Misc. (Rebilling);
          //;
          rsRBHours = new DataReader(getSQL(selYear,"RB", "HOURS"));
          rsRBHours.Open();
          rsRBHours.Read();


          //;
          // Get Total Hours for UA;
          //;
          rsUAHours = new DataReader(getSQL(selYear,"UA", "HOURS"));
          rsUAHours.Open();
          rsUAHours.Read();


          //;
          //;
          // Get Self Audit;
          rsSelfAudit = new DataReader(getSelfAuditSQL(selYear));
          rsSelfAudit.Open();
          rsSelfAudit.Read();


          //;
          //;
          // Get KPIs;
          rsKPI = new DataReader(getKPISQL(selYear));
          rsKPI.Open();
          rsKPI.Read();


          //;
          // Get Budget/Cost Per Unit;
          rsBudgetCPUTU = new DataReader(getBCPUTotalUnitstrSQL(selYear));
          rsBudgetCPUTU.Open();
          rsBudgetCPUTU.Read();

          rsBudgetCPURH = new DataReader(getBCPURegHourstrSQL(selYear));
          rsBudgetCPURH.Open();
          rsBudgetCPURH.Read();

          rsBudgetCPUOT = new DataReader(getBCPUOTHourstrSQL(selYear));
          rsBudgetCPUOT.Open();
          rsBudgetCPUOT.Read();

          // Get Total Hours for Air Test;
          rsATHours = new DataReader(getSQL(selYear,"AT", "HOURS"));
          rsATHours.Open();
          rsATHours.Read();

          // Get Total Units for( Air Test;
          rsATUnits = new DataReader(getSQL(selYear,"AT", "UNITS"));
          rsATUnits.Open();
          rsATUnits.Read();

          // Get Total Hours for Prepping;
          rsPrepHours = new DataReader(getSQL(selYear,"RP", "HOURS"));
          rsPrepHours.Open();
          rsPrepHours.Read();

          // Get Total Units for( Prepping;
          rsPrepUnits = new DataReader(getSQL(selYear,"RP", "UNITS"));
          rsPrepUnits.Open();
          rsPrepUnits.Read();

          // Get Total Units;
          rsTotalUnits = new DataReader(getSQL(selYear,"ALL", "UNITS"));
          rsTotalUnits.Open();
          rsTotalUnits.Read();


    }

        public string getSQL(string sYear,string  sTask,string sPayOrUnits){

            string strSQL = "";

            if(sPayOrUnits == "UNITS"){
              strSQL = " SELECT IsNULL(SUM(Units), 0) AS TotalUnits, MONTH(WorkDate) As Month ";
              strSQL +=  " FROM FacilityProductionDetail ";
              strSQL +=  "      INNER JOIN Tasks ON FacilityProductionDetail.TaskId = Tasks.Id  ";
            }else{
               strSQL = " SELECT IsNULL(SUM(HoursPaid),0) AS TotalHours, MONTH(WorkDate) As Month ";
               strSQL +=  " FROM Facility f INNER JOIN EmployeeTaskWorked ON f.Id = EmployeeTaskWorked.FacilityID ";
               strSQL +=  "      INNER JOIN Tasks ON EmployeeTaskWorked.TaskID = Tasks.Id   ";
               strSQL +=  "      LEFT OUTER JOIN EmployeeTaskWorkedPay ON EmployeeTaskWorked.Id = EmployeeTaskWorkedPay.EmployeeTaskWorkedId ";
            }

            if(sTask == "ALL"){
               //strSQL +=  "WHERE (1=1) ";
	            strSQL +=  "WHERE (Tasks.Id IN (1,2) ) ";
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

            strSQL +=  "      AND WorkDate BETWEEN '" + "1/1/" + cStr(sYear) + "' AND '12/31/" + cStr(sYear) + "' ";
            strSQL +=  "GROUP BY Month(WorkDate) ";
            strSQL +=  "ORDER BY Month ";

             return strSQL;

        }


        public string getUMSQL(string sYear){

          string strSQL = "";
          string sWhere = "";

          for( int I=1 ; I <= 12; I ++){
            sWhere = " Where Datepart(yyyy,WorkDate)=" + sYear + " AND DatePart(m,Workdate)=" + cStr(I) + " AND TaskID IN (1,2) ) ";

            strSQL +=  "SELECT TotalUnits=IsNULL((Select Sum(Units) From FacilityProductionDetail " + sWhere + ",0), ";
            strSQL +=  "       TotalMen=IsNULL((Select Count(Distinct EmployeeID) From EmployeeTaskWorked "  + sWhere + ",0), ";
            strSQL +=  "       TotalDays=IsNULL((Select Count(Distinct Datepart(d,WorkDate)) From EMployeeTaskWorked " + sWhere + ",0), Mon=" + cStr(I);

            if (I < 12 ){
              strSQL +=  " UNION ";
            }
          }

          strSQL +=  "ORDER BY Mon ";
          return strSQL;

        }


        public string getBCPUTotalUnitstrSQL(string sYear){

          string strSQL = "";
          string sWhereUnits = "";

          for( int I=1 ; I <= 12; I++){
            sWhereUnits = "  Datepart(yyyy,WorkDate)=" + sYear + " AND DatePart(m,Workdate)=" + cStr(I) + " AND TaskID IN (1,2)   ";

            strSQL +=  "SELECT IsNULL(Sum(Units),0), Mon=" + cStr(I) + " FROM FacilityProductionDetail WHERE " + sWhereUnits;

            if (I < 12){
              strSQL +=  " UNION ";
            }
          }

          strSQL +=  "ORDER BY Mon ";
         return strSQL;

        }


        public string getBCPURegHourstrSQL(string sYear){

          string strSQL = "";
          string sWherePay = "";
          int i;
 
          for( i=1 ; i <= 12; i++){

            sWherePay   = "  Datepart(yyyy,WorkDate)=" + sYear + " AND DatePart(m,Workdate)=" + cStr(i) + "  ";

            strSQL +=  "SELECT IsNull(Sum(PayAmount),0), Mon=" + cStr(i);
            strSQL +=  "       FROM EmployeeTaskWorkedPay INNER JOIN EmployeeTaskWorked ON EmployeeTaskWorkedPay.EmployeeTaskWorkedId = EmployeeTaskWorked.Id ";
            strSQL +=  "       WHERE PayMultiplier=1 AND "  + sWherePay;

            if (i < 12){
              strSQL +=  " UNION ";
            }

          }

          strSQL +=  " ORDER BY Mon ";
          return strSQL;

        }


        public string  getBCPUOTHourstrSQL(string sYear){

          string strSQL = "";
          string sWhereUnits = "";
          string sWherePay = "";

          for( int I=1 ; I <= 12; I++){
            sWherePay   = "  Datepart(yyyy,WorkDate)=" + sYear + " AND DatePart(m,Workdate)=" + cStr(I) + "  ";

            strSQL +=  "SELECT IsNULL(Sum(PayAmount),0),  Mon=" + cStr(I);
            strSQL +=  "          FROM EmployeeTaskWorkedPay INNER JOIN EmployeeTaskWorked ON EmployeeTaskWorkedPay.EmployeeTaskWorkedId = EmployeeTaskWorked.Id ";
            strSQL +=  "          WHERE PayMultiplier <> 1 AND "  + sWherePay;

            if (I < 12 ){
              strSQL +=  " UNION ";
            }
          }

          strSQL +=  " ORDER BY Mon ";
          return strSQL;

        }


        public string getSpotSQL(string sYear){

          string strSQL;

            strSQL = " SELECT  IsNULL(SUM(CASE FieldName WHEN 'RC_IN'    Then Cast(DataValue as float) Else 0 END), 0) AS RCs_IN,     ";
            strSQL +=  "       IsNULL(SUM(CASE FieldName WHEN 'RC_OUT'   Then Cast(DataValue as float) Else 0 END), 0) AS RCs_OUT,    ";
            strSQL +=  "       IsNULL(SUM(CASE FieldName WHEN 'DOWN_TIME' Then Cast(DataValue as float) Else 0 END), 0) AS DOWN_TIME,  ";
            strSQL +=  "               MONTH(WorkDate) As Month ";
            strSQL +=  " FROM FacilityMonitoringDataEntry ";
            strSQL += " WHERE WorkDate BETWEEN '" + "1/1/" + cStr(sYear) + "' AND '12/31/" + cStr(sYear) + "' ";
            strSQL +=  " GROUP BY MONTH(WorkDate) ";
            strSQL +=  " ORDER BY Month ";

          return strSQL;

        }


        public string getSelfAuditSQL(string sYear){

          string strSQL ="";

          for( int I=1 ; I <= 12; I ++){
                strSQL += " SELECT  (SELECT IsNULL(COUNT(DataValue),0) FROM FacilityMonitoringDataEntry ";
                strSQL +=  "                        WHERE DatePart(m,WorkDate) = " + cStr(I) + " AND DatePart(yyyy,WorkDate) = ";
                strSQL += "                        " + cStr(sYear) + " AND LTRIM(RTRIM(DataValue)) <> '' AND DataValue <> '.' AND DataValue <> '0' AND DataValue IS  NOT NULL AND FieldName = 'BAY') AS IN_BAY,     ";
                strSQL +=  "                (SELECT ISNULL(COUNT(DataValue),0) FROM FacilityMonitoringDataEntry ";
                strSQL +=  "                        WHERE DatePart(m,WorkDate) = " + cStr(I) + " AND DatePart(yyyy,WorkDate) = ";
                strSQL += "                        " + cStr(sYear) + " AND LTRIM(RTRIM(DataValue)) <> '' AND DataValue <> '.' AND DataValue <> '0' AND DataValue IS NOT NULL AND FieldName = 'FACILITY') AS FAC,     ";
                strSQL +=  "                (SELECT Cast(IsNULL(AVG(Cast(DataValue as decimal(5,2))),0) AS decimal(5,2)) FROM FacilityMonitoringDataEntry ";
                strSQL +=  "                        WHERE DatePart(m,WorkDate) = " + cStr(I) + " AND DatePart(yyyy,WorkDate) = ";
                strSQL += "                        " + cStr(sYear) + " AND LTRIM(RTRIM(DataValue)) <> '' AND DataValue <> '.' AND FieldName = 'BAY') AS IN_BAY_AVG,     ";
                strSQL +=  "                (SELECT CAST(IsNULL(AVG(Cast(DataValue as decimal(5,2))),0) AS decimal(5,2)) FROM FacilityMonitoringDataEntry ";
                strSQL +=  "                        WHERE DatePart(m,WorkDate) = " + cStr(I) + " AND DatePart(yyyy,WorkDate) = ";
                strSQL += "                        " + cStr(sYear) + " AND LTRIM(RTRIM(DataValue)) <> '' AND DataValue <> '.' AND FieldName = 'FACILITY') AS FAC_AVG, ";
                strSQL += "                        CAST(IsNULL(SUM(CASE FieldName WHEN 'BAY'  Then Cast((CASE  WHEN LTRIM(RTRIM(DataValue)) = '' OR DataValue ='.' THEN cast(0 as float) ELSE DataValue END) as float) Else 0 END), 0) AS float) AS TOTAL_BAY,    ";
                strSQL += "                        CAST(IsNULL(SUM(CASE FieldName WHEN 'FACILITY' Then Cast((CASE WHEN LTRIM(RTRIM(DataValue)) = '' OR DataValue ='.'  THEN cast(0 as float) ELSE DataValue END) as float) Else 0 END), 0) AS float) AS TOTAL_FAC,    ";
                strSQL +=  "       Mon=" + cStr(I);
                strSQL +=  " FROM FacilityMonitoringDataEntry  ";
                strSQL +=  "WHERE  DatePart(m,WorkDate) = " + cStr(I) + " AND  DatePart(yyyy,WorkDate) = " + cStr(sYear);

            if (I < 12){
              strSQL +=  " UNION ";
            }
          }

          strSQL +=  " ORDER BY Mon ";
          return strSQL;

        }


        public string getKPISQL(string sYear){

          string strSQL;

            strSQL = " SELECT  IsNULL(SUM(CASE FieldName WHEN 'ACCIDENTS' Then Cast(DataValue as float) Else 0 END), 0) AS ACC,     ";
            strSQL +=  "       IsNULL(SUM(CASE FieldName WHEN 'REP_INJURIES' Then Cast(DataValue as float) Else 0 END), 0) AS INJURIES,";
            strSQL +=  "       IsNULL(SUM(CASE FieldName WHEN 'LOST_TIME' Then Cast(DataValue as float) Else 0 END), 0) AS LOSTTIME,";
            strSQL +=  "       IsNULL(SUM(CASE FieldName WHEN 'NON_MEDICAL' Then Cast(DataValue as float) Else 0 END), 0) AS NONMED,  ";
            strSQL +=  "       MONTH(WorkDate) As Month ";
            strSQL +=  " FROM FacilityMonitoringDataEntry  ";
            strSQL +=  "      WHERE WorkDate BETWEEN '" + "1/1/" + cStr(sYear) + "' AND '12/31/" + cStr(sYear) + "' ";
            strSQL +=  "GROUP BY Month(WorkDate) ";
            strSQL +=  "ORDER BY Month ";

          return strSQL;

        }


        public void WriteTopRows(){

          string rc;
          string UnLoadUnits,UnLoadHours,LoadUnits,LoadHours;

          for( int i=1; i <= 12; i++){
            if(i % 2 == 0){
              rc = "reportEvenLine";
            }else{
              rc = "reportOddLine";
           }

            Response.Write ("<tr height=17 style='height:12.75pt' class='" + rc + "'>");
            Response.Write ("  <td height=17 class=xl6620928 style='height:12.75pt' style='align:right' style='border-bottom:1pt solid black;' ><b>" + Left(MonthName(i),3) + "</b></td>");


            //;
            // Unloading;
            //;
            if ( rsUnLoadUnits.LastReadSuccess ){
                        if (rsUnLoadUnits.Fields(1) ==  cStr(i) ){
                                UnLoadUnits = cStr(rsUnLoadUnits.Fields(0));
                                rsUnLoadUnits.Read();

                        }else{
                                UnLoadUnits = "";
                       }
                }else{
                        UnLoadUnits = "";
               }

            if (rsUnLoadHours.LastReadSuccess ){
                        if (rsUnLoadHours.Fields(1) == cStr(i) ){
                                UnLoadHours = cStr(rsUnLoadHours.Fields(0));
                                rsUnLoadHours.Read();

                        }else{
                                UnLoadHours = "";
                       }
                }else{
                        UnLoadHours = "";
               }

              Response.Write ("  <td class=xl5520928 style='border-top:none;border-left:none' align='right'>" +  FNum(cStr(SafeDbl(UnLoadHours)), 2) + "</td>");
              Response.Write ("  <td class=xl5620928 style='border-top:none;border-left:none' align='right'>" +  FNum(cStr(SafeDbl(UnLoadUnits)), 0) + "</td>");
              if(SafeDbl(UnLoadHours) != 0 && SafeDbl(UnLoadUnits) != 0){
                Response.Write ("  <td class=xl4620928 style='border-top:none;border-left:none' align='right'>" +  FNum(cStr(SafeDiv(cStr(SafeDbl(UnLoadUnits)) , cStr(SafeDbl(UnLoadHours)))), 2)  + "</td>");
              }else{
                Response.Write ("  <td class=xl4620928 style='border-top:none;border-left:none' >&nbsp;</td>");
             }

              unLoadTotalHours = SafeDbl(cStr(unLoadTotalHours)) + SafeDbl(UnLoadHours);
              unLoadTotalUnits = SafeDbl(cStr(unLoadTotalUnits)) + SafeDbl(UnLoadUnits);



            //;
            // Loading;
            //;
            if (rsLoadUnits.LastReadSuccess ){
                        if (rsLoadUnits.Fields(1) == cStr(i) ){
                                LoadUnits = cStr(rsLoadUnits.Fields(0));
                                rsLoadUnits.Read();

                        }else{
                                LoadUnits = "";
                       }
                }else{
                        LoadUnits = "";
               }

            if (rsLoadHours.LastReadSuccess ){
                        if (rsLoadHours.Fields(1) == cStr(i) ){
                                LoadHours = cStr(rsLoadHours.Fields(0));
                                rsLoadHours.Read();

                        }else{
                                LoadHours = "";
                       }
                }else{
                        LoadHours = "";
               }

            Response.Write ("  <td class=xl5520928 style='border-top:none;border-left:none' align='right'>" +  FNum(cStr(SafeDbl(LoadHours)), 2) + "</td>");
            Response.Write ("  <td class=xl5620928 style='border-top:none;border-left:none' align='right'>" +  FNum(cStr(SafeDbl(LoadUnits)), 0) + "</td>");
            if(SafeDbl(cStr(SafeDbl(LoadHours))) != 0 && SafeDbl(cStr(SafeDbl(LoadUnits))) != 0){
                Response.Write ("  <td class=xl4620928 style='border-top:none;border-left:none' align='right'>" +  FNum(cStr(SafeDiv(cStr(SafeDbl(LoadUnits)) , cStr(SafeDbl(LoadHours)))), 2)  + "</td>");
            }else{
                Response.Write ("  <td class=xl4620928 style='border-top:none;border-left:none' >&nbsp;</td>");
           }

            LoadTotalHours = SafeDbl(cStr(LoadTotalHours)) + SafeDbl(LoadHours);
            LoadTotalUnits = SafeDbl(cStr(LoadTotalUnits)) + SafeDbl(LoadUnits);

     
              string sUM,sUM1,sUM2,sUM3;

            //;
            // Total Units / Man;
            //;
            if (rsUM.LastReadSuccess ){
                        if (rsUM.Fields(3) == cStr(i) ){
                                sUM = "";
                                sUM1 = cStr(rsUM.Fields(0));
                                sUM2 = cStr(rsUM.Fields(1));
                                sUM3 = cStr(rsUM.Fields(2));
                                rsUM.Read();

                        }else{
                                sUM = "";
                                sUM1 = "";
                                sUM2 = "";
                                sUM3 = "";
                       }
                }else{
                        sUM = "";
                        sUM1 = "";
                        sUM2 = "";
                        sUM3 = "";
               }

            if (SafeDbl(sUM1) != 0 && SafeDbl(sUM2) != 0 && SafeDbl(sUM3) != 0){
                sUM = cStr(SafeDbl(sUM1) / SafeDbl(sUM2)  / SafeDbl(sUM3));
                UMTotals = SafeDbl(cStr(UMTotals)) + SafeDbl(sUM);
                nUMTotals = nUMTotals + 1;
                Response.Write ("  <td class=xl5620928 style='border-top:none;border-left:none' >" +  FNum(sUM, 0) + "</td>");
            }else{
                Response.Write ("  <td class=xl5620928 style='border-top:none;border-left:none' >&nbsp;</td>");
           }


             string shuttlingUnits,shuttlingHours;
            //;
            // Shuttling;
            //;
            if (rsShuttlingUnits.LastReadSuccess ){
                        if (rsShuttlingUnits.Fields(1) == cStr(i) ){
                                shuttlingUnits = cStr(rsShuttlingUnits.Fields(0));
                                rsShuttlingUnits.Read();

                        }else{
                                shuttlingUnits = "";
                       }
                }else{
                        shuttlingUnits = "";
               }

            if (rsShuttlingHours.LastReadSuccess ){
                        if (rsShuttlingHours.Fields(1) == cStr(i) ){
                                shuttlingHours = cStr(rsShuttlingHours.Fields(0));
                                rsShuttlingHours.Read();

                        }else{
                                shuttlingHours = "";
                       }
                }else{
                        shuttlingHours = "";
               }

            Response.Write ("  <td class=xl4620928 style='border-top:none;border-left:none' align='right'>" +  FNum(cStr(SafeDbl(shuttlingHours)), 2) + "</td>");
            Response.Write ("  <td class=xl5720928 style='border-top:none;border-left:none' align='right'>" +  FNum(cStr(SafeDbl(shuttlingUnits)), 0) + "</td>");

            if(SafeDbl(shuttlingHours) != 0 && SafeDbl(shuttlingUnits) != 0){
               Response.Write ("  <td class=xl4620928 style='border-top:none;border-left:none' align='right'>" +  FNum(cStr(SafeDiv(cStr(SafeDbl(shuttlingUnits)), cStr(SafeDbl(shuttlingHours)))), 2) + "</td>");
            }else{
               Response.Write ("  <td class=xl4620928 style='border-top:none;border-left:none' align='right'>&nbsp;</td>");
           }

            ShuttlingTotalHours = SafeDbl(cStr(ShuttlingTotalHours)) + SafeDbl(shuttlingHours);
            ShuttlingTotalUnits = SafeDbl(cStr(ShuttlingTotalUnits)) + SafeDbl(shuttlingUnits);


            string spotting1,spotting2,spotting3;

            //;
            // Spotting;
            //;
            if (rsSpotting.LastReadSuccess ){
                        if (rsSpotting.Fields(3) == cStr(i) ){
                                spotting1 = cStr(rsSpotting.Fields(0));
                                spotting2 = cStr(rsSpotting.Fields(1));
                                spotting3 = cStr(rsSpotting.Fields(2));
                                rsSpotting.Read();

                        }else{
                                spotting1 = "";
                                spotting2 = "";
                                spotting3 = "";
                       }
                }else{
                        spotting1 = "";
                        spotting2 = "";
                        spotting3 = "";
               }

            Response.Write ("  <td class=xl5620928 style='border-top:none;border-left:none' >" +  FNum(cStr(SafeDbl(spotting1)), 0) + "</td>");
            Response.Write ("  <td class=xl5520928 style='border-top:none;border-left:none' >" +  FNum(cStr(SafeDbl(spotting2)), 0) + "</td>");
            Response.Write ("  <td class=xl5520928 style='border-top:none;border-left:none' >" +  FNum(cStr(SafeDbl(spotting3)), 2) + "</td>");

            RCs_In   = RCs_In + SafeDbl(spotting1);
            RCs_Out  = RCs_Out + SafeDbl(spotting2);
            DownTime = DownTime + SafeDbl(spotting3);


            string spottingHours;

            //;
            // Spotting Hours;
            //;
            if (rsSpottingHours.LastReadSuccess ){
                        if (rsSpottingHours.Fields(1) == cStr(i) ){
                                spottingHours = cStr(rsSpottingHours.Fields(0));
                                rsSpottingHours.Read();

                        }else{
                                spottingHours = "";
                       }
                }else{
                        spottingHours = "";
               }

            Response.Write ("  <td class=xl5820928 style='border-top:none;border-left:none' align='right'>" +  FNum(cStr(SafeDbl(spottingHours)), 2) + "</td>");

            SpottingTotalHours = SafeDbl(cStr(SpottingTotalHours)) + SafeDbl(spottingHours);

            string RC_Hr; 
            if(SafeDbl(spottingHours) != 0){
                  RC_Hr = cStr(SafeDiv(cStr((SafeDbl(spotting1) + SafeDbl(spotting1))) , cStr(SafeDbl(spottingHours))));
                  Response.Write ("  <td class=xl4620928 style='border-top:none;border-left:none' >" +  FNum(RC_Hr, 2) + "</td>");
            }else{
                  Response.Write ("  <td class=xl4620928 style='border-top:none;border-left:none' >&nbsp;</td>");
           }


            string trainingHours;

            //;
            // Training Hours;
            //;
            if (rsTrainingHours.LastReadSuccess ){
                        if (cInt(rsTrainingHours.Fields(1)) == i ){
                                trainingHours = cStr(rsTrainingHours.Fields(0));
                                rsTrainingHours.Read();

                        }else{
                                trainingHours = "";
                       }
                }else{
                        trainingHours = "";
               }

            Response.Write ("  <td class=xl5820928 style='border-top:none;border-left:none'>" +  FNum(cStr(SafeDbl(trainingHours)), 2) + "</td>");
            TrainingTotalHours = SafeDbl(cStr(TrainingTotalHours)) + SafeDbl(trainingHours);

            string clericalHours;

            //;
            // Clerical Hours;
            //;
            if (rsClericalHours.LastReadSuccess ){
                        if (rsClericalHours.Fields(1) == cStr(i) ){
                                clericalHours = cStr(rsClericalHours.Fields(0));
                                rsClericalHours.Read();

                        }else{
                                clericalHours = "";
                       }
                }else{
                        clericalHours = "";
               }

            Response.Write ("  <td class=xl5620928 style='border-top:none;border-left:none'>" +  FNum(cStr(SafeDbl(clericalHours)), 2) + "</td>");
            ClericalTotalHours = SafeDbl(cStr(ClericalTotalHours)) + SafeDbl(clericalHours);


            //;
                // Misc. (Rebilling) Hours;
            //;
            string RBHours;
            if (rsRBHours.LastReadSuccess ){
                        if (rsRBHours.Fields(1) == cStr(i) ){
                                RBHours = cStr(rsRBHours.Fields(0));
                                rsRBHours.Read();

                        }else{
                                RBHours = "";
                       }
                }else{
                        RBHours = "";
               }

            Response.Write ("  <td class=xl5520928 style='border-top:none;border-left:none'>" +  FNum(cStr(SafeDbl(RBHours)), 2) + "</td>");
            RBTotalHours = SafeDbl(cStr(RBTotalHours)) + SafeDbl(RBHours);


            string UAHours;

            if (rsUAHours.LastReadSuccess ){
                        if (rsUAHours.Fields(1) == cStr(i) ){
                                UAHours = cStr(rsUAHours.Fields(0));
                                rsUAHours.Read();

                        }else{
                                UAHours = "";
                       }
                }else{
                        UAHours = "";
               }

            Response.Write ("  <td class=xl5520928 style='border-top:none;border-left:none'>" +  FNum(cStr(SafeDbl(UAHours)), 2) + "</td>");
            UATotalHours = SafeDbl(cStr(UATotalHours)) + SafeDbl(UAHours);



            //;
            // Self Audits;
            //;
            if (rsSelfAudit.LastReadSuccess ){
                           Response.Write ("  <td class=xl5820928 style='border-top:none;border-left:none' >" +  FNum(rsSelfAudit.Fields(0), 0) + "&nbsp;</td>");
                           Total_SA_Bay = SafeDbl(cStr(Total_SA_Bay)) + SafeDbl(rsSelfAudit.Fields(0));
                           Total_SC_Bay = SafeDbl(cStr(Total_SC_Bay)) + SafeDbl(rsSelfAudit.Fields(4));
                           Response.Write ("  <td class=xl5820928 style='border-top:none;border-left:none' >" +  FNum(rsSelfAudit.Fields(2), 2) + "&nbsp;</td>");
                           Response.Write ("  <td class=xl6720928 style='border-top:none;border-left:none' >" +  FNum(rsSelfAudit.Fields(1), 0) + "&nbsp;</td>");
                           Total_SA_Fac = SafeDbl(cStr(Total_SA_Fac)) + SafeDbl(rsSelfAudit.Fields(1));
                           Total_SC_Fac = SafeDbl(cStr(Total_SC_Fac)) + SafeDbl(rsSelfAudit.Fields(5));
                           Response.Write ("  <td class=xl5620928 style='border-top:none;border-left:none' >" +  FNum(rsSelfAudit.Fields(3), 2) + "&nbsp;</td>");
                           rsSelfAudit.Read();
           }

            Response.Write ("</tr>");

          }

        }


        public void WriteTopTotal(){

           Response.Write ("<tr height=18 style='height:13.5pt'>");
           Response.Write ("  <td height=18 class=xl4120928 style='height:13.5pt;border-top:none;border-right:.5pt solid black;'><b>Total</b></td>");
           //;
           // UnLoading;
           //;
           Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + FNum(cStr(unLoadTotalHours), 2) + "</td>");
           Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + FNum(cStr(unLoadTotalUnits), 0) + "</td>");
           if(SafeDbl(cStr(unLoadTotalHours)) != 0 && SafeDbl(cStr(unLoadTotalUnits)) != 0){
             Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;' >" + FNum(cStr(SafeDiv(cStr(unLoadTotalUnits), cStr(unLoadTotalHours))), 2) + "</td>");
           }else{
             Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;'>&nbsp;</td>");
          }
           //;
           // Loading;
           //;
           Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + FNum(cStr(LoadTotalHours), 2) + "</td>");
           Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + FNum(cStr(LoadTotalUnits), 0) + "</td>");
           if(SafeDbl(cStr(LoadTotalHours)) != 0 && SafeDbl(cStr(LoadTotalUnits)) != 0){
             Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;' >" + FNum(cStr(SafeDiv(cStr(LoadTotalUnits), cStr(LoadTotalHours))), 2) + "</td>");
           }else{
             Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;'>&nbsp;</td>");
          }
           //;
           // Total Units / Man;
           //;
           if(SafeDbl(cStr(nUMTotals)) != 0){
              UMTotals = SafeDbl(cStr(UMTotals)) / SafeDbl(cStr(nUMTotals));
              Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;' >" + FNum(cStr(UMTotals), 0) + "</td>");
           }else{
              Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;'>&nbsp;</td>");
          }

           //;
           // Shuttling;
           //;
           Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;' align='right'>" + FNum(cStr(ShuttlingTotalHours), 2) + "</td>");
           Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;' align='right'>" + FNum(cStr(ShuttlingTotalUnits), 0) + "</td>");
           if(SafeDbl(cStr(ShuttlingTotalUnits)) != 0 && SafeDbl(cStr(ShuttlingTotalHours)) != 0){
             Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;' align='right'>" + FNum(cStr(SafeDiv(cStr(ShuttlingTotalUnits), cStr(ShuttlingTotalHours))), 2) + "</td>");
           }else{
             Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;'>&nbsp;</td>");
          }
           //;
           // Spotting;
           //;
           Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + FNum(cStr(RCs_In), 0) + "</td>");
           Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + FNum(cStr(RCs_Out), 0) + "</td>");
           Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + FNum(cStr(DownTime), 2) + "</td>");
           Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + FNum(cStr(SpottingTotalHours), 2) + "</td>");

           if(SpottingTotalHours > 0){
             Total_RC_Hr = (RCs_In + RCs_Out) / SpottingTotalHours;
             Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + FNum(cStr(Total_RC_Hr), 2) + "</td>");
           }else{
             Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;'>&nbsp;</td>");
          }
           //;
           // Miscellaneous (Training, Clerical, RB, UA);
           //;
           Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + FNum(cStr(TrainingTotalHours), 2) + "</td>");
           Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + FNum(cStr(ClericalTotalHours), 2) + "</td>");
           Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + FNum(cStr(RBTotalHours), 2) + "</td>");
           Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + FNum(cStr(UATotalHours), 2) + "</td>");

           Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + FNum(cStr(Total_SA_Bay), 0) + "</td>");
           Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + FNum(cStr(SafeDiv(FNum(cStr(Total_SC_Bay), 2),cStr(Total_SA_Bay))),2) + "</td>");
           Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + FNum(cStr(Total_SA_Fac), 0) + "</td>");
           Response.Write ("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + FNum(cStr(SafeDiv(FNum(cStr(Total_SC_Fac), 2),cStr(Total_SA_Fac))),2) + "</td>");
           Response.Write ("</tr>");
           //;
           Response.Write ("<tr height=18 style='height:13.5pt'>");
           Response.Write ("  <td height=18 class=xl9120928 style='height:13.5pt;border-right:none;border-top:none'>&nbsp;</td>");
           Response.Write ("  <td class=xl9120928 colspan=23 style='border-right:none' >&nbsp;</td>");
           Response.Write ("  <td class=xl1520928 style='border-right:none' >&nbsp;</td>");
           Response.Write ("</tr>");
        }


        public void WriteBottomRows(){

          string rc;

          for(int i=1 ; i <= 12; i ++){
            if(i % 2 == 0){
              rc = "reportEvenLine";
            }else{
              rc = "reportOddLine";
           }

            Response.Write ("<tr height=17 style='height:12.75pt' class='" + rc + "'>");
            Response.Write ("  <td height=17 class=xl6620928 style='height:12.75pt' style='border-bottom:1pt solid black;' ><b>" + Left(MonthName(i),3) + "</b></td>");
            //;
            // Accidents / Injuries;
            //;
            string KPI1,KPI2,KPI3,KPI4,totalUnits;

            if (rsKPI.LastReadSuccess ){
                        if (rsKPI.Fields(4) == cStr(i) ){
                                KPI1 = cStr(rsKPI.Fields(0));
                                KPI2 = cStr(rsKPI.Fields(1));
                                KPI3 = cStr(rsKPI.Fields(2));
                                KPI4 = cStr(rsKPI.Fields(3));
                                rsKPI.Read();

                        }else{
                                KPI1 = "";
                                KPI2 = "";
                                KPI3 = "";
                                KPI4 = "";
                       }
                }else{
                        KPI1 = "";
                        KPI2 = "";
                        KPI3 = "";
                        KPI4 = "";
               }

            if (rsTotalUnits.LastReadSuccess ){
                        if (rsTotalUnits.Fields(1) == cStr(i)){
                                totalUnits = cStr(rsTotalUnits.Fields(0));
                                rsTotalUnits.Read();

                        }else{
                                totalUnits = "";
                       }
            }else{
                        totalUnits = "";
            }

            Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none'>" +  FNum(cStr(SafeDbl(KPI1)), 0) + "</td>");
            Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none'>" +  FNum(cStr(SafeDiv(FNum(cStr(SafeDbl(KPI1)), 2),cStr(SafeDbl(totalUnits)))*10000),2) + "</td>");
	
            Total_Accidents = Total_Accidents + SafeDbl(KPI1);
            arrTotalAccidents[i-1] = FNum(cStr(SafeDiv(FNum(cStr(SafeDbl(KPI1)), 2),cStr(SafeDbl(totalUnits)))*10000),2);
            Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none'>" +  FNum(cStr(SafeDbl(KPI2)), 0) + "</td>");
            Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none'>" +  FNum(cStr(SafeDiv(FNum(cStr(SafeDbl(KPI2)), 2),cStr(SafeDbl(totalUnits)))*10000),2) + "</td>");
            Total_Injuries = Total_Injuries + SafeDbl(KPI2);
            arrTotalInjuryRep[i-1] = FNum(cStr(SafeDiv(FNum(cStr(SafeDbl(KPI2)), 2),cStr(SafeDbl(totalUnits)))*10000),2);
            Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none'>" +  FNum(cStr(SafeDbl(KPI3)), 2) + "</td>");
            Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none'>" +  FNum(cStr(SafeDiv(FNum(cStr(SafeDbl(KPI3)), 2),cStr(SafeDbl(totalUnits)))*10000),2) + "</td>");
            Total_LostTime = Total_LostTime + SafeDbl(KPI3);
            arrTotalLostTime[i-1] = FNum(cStr(SafeDiv(FNum(cStr(SafeDbl(KPI3)), 2),cStr(SafeDbl(totalUnits)))*10000),2);
            Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none'>" +  FNum(cStr(SafeDbl(KPI4)), 0) + "</td>");
            Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none'>" +  FNum(cStr(SafeDiv(FNum(cStr(SafeDbl(KPI4)), 2),cStr(SafeDbl(totalUnits)))*10000),2) + "</td>");
            Total_NonMed = Total_NonMed + SafeDbl(KPI4);
            arrTotalNonMed[i-1] = FNum(cStr(SafeDiv(FNum(cStr(SafeDbl(KPI4)), 2),cStr(SafeDbl(totalUnits)))*10000),2);


            //
            // Budget / Cost Per Unit
            //
            string CPU = "";
            double TotalLabor = 0.0;

            if( rsBudgetCPUTU.LastReadSuccess){
              Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none' colspan=2 align='right'>" +  FNum(rsBudgetCPUTU.Fields(0), 0) + "</td>");
              Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none' colspan=2 align='right'>" +  FCur(rsBudgetCPURH.Fields(0), 2) + "</td>");
              Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none' colspan=2 align='right'>" +  FCur(rsBudgetCPUOT.Fields(0), 2) + "</td>");
              BudgetTotalUnits = SafeDbl(cStr(BudgetTotalUnits)) + SafeDbl(rsBudgetCPUTU.Fields(0));
              BudgetTotalHours = SafeDbl(cStr(BudgetTotalHours)) + SafeDbl(rsBudgetCPURH.Fields(0));
              BudgetTotalOTHrs = SafeDbl(cStr(BudgetTotalOTHrs)) + SafeDbl(rsBudgetCPUOT.Fields(0));
              TotalLabor  = SafeDbl(rsBudgetCPURH.Fields(0)) + SafeDbl(rsBudgetCPUOT.Fields(0));
              if (SafeDbl(rsBudgetCPUTU.Fields(0)) != 0 && TotalLabor != 0) {
                CPU = cStr(SafeDiv(cStr(TotalLabor), cStr(SafeDbl(rsBudgetCPUTU.Fields(0)))));
                CPU = FCur(CPU, 2);
              }else{
                CPU = "&nbsp;";
              }
              rsBudgetCPUTU.Read();
              rsBudgetCPURH.Read();
              rsBudgetCPUOT.Read();
            }

            Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none' colspan=2 align='right'>" +  FCur(cStr(TotalLabor), 2) + "</td>");
            Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none' colspan=2 align='right'>" +  CPU + "</td>");


            //
            // Air Test
            //
            string ATUnits,ATHours;
            if ( rsATUnits.LastReadSuccess){
                        if(rsATUnits.Fields(1) == cStr(i)){
                                ATUnits = cStr(rsATUnits.Fields(0));
                                rsATUnits.Read();
                        }else{
                                ATUnits = "";
                        }
            }else{
                        ATUnits = "";
            }

            if(rsATHours.LastReadSuccess){
                        if(rsATHours.Fields(1) == cStr(i)){
                                ATHours = cStr(rsATHours.Fields(0));
                                rsATHours.Read();
                        }else{
                                ATHours = "";
                        }
            }else{
                        ATHours = "";
            }

            Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none; text-align:right'>" + FNum(cStr(SafeDbl(ATUnits)), 0) + "</td>");
            Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none; text-align:right'>" + FNum(cStr(SafeDbl(ATHours)), 2) + "</td>");

            ATTotalUnits = SafeDbl(cStr(ATTotalUnits)) + SafeDbl(cStr(ATUnits));
            ATTotalHours = SafeDbl(cStr(ATTotalHours)) + SafeDbl(cStr(ATHours));

            if (SafeDbl(ATUnits) != 0 && SafeDbl(ATHours) != 0) {
               Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none; text-align:right' >" + FNum(cStr(SafeDiv(cStr(SafeDbl(ATUnits)) , cStr(SafeDbl(ATHours)))), 2) + "</td>");
            }else{
               Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none; text-align:right' >&nbsp;</td>");
            }



            // Prepping
            //
            //  oCPU.Fields(0).Value = rsPrepUnits.Fields(0).Value
            //  oCPU.Fields(1).Value = rsPrepHours.Fields(0).Value
            //
            string PrepUnits,PrepHours;
            if(rsPrepUnits.LastReadSuccess){
                        if( rsPrepUnits.Fields(1) == cStr(i) ){
                                PrepUnits = cStr(rsPrepUnits.Fields(0));
                                rsPrepUnits.Read();
                        }else{
                                PrepUnits = "";
                        }
            }else{
                        PrepUnits = "";
            }

            if( rsPrepHours.LastReadSuccess){
                        if(rsPrepHours.Fields(1) == cStr(i)){
                                PrepHours = cStr(rsPrepHours.Fields(0));
                                rsPrepHours.Read();
                        }else{
                                PrepHours = "";
                        }
            }else{
                        PrepHours = "";
            }

            Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none; text-align:right'>" + FNum(cStr(SafeDbl(PrepUnits)), 0) + "</td>");
            Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none; text-align:right'>" + FNum(cStr(SafeDbl(PrepHours)), 2) + "</td>");

            PrepTotalUnits = SafeDbl(cStr(PrepTotalUnits)) + SafeDbl(PrepUnits);
            PrepTotalHours = SafeDbl(cStr(PrepTotalHours)) + SafeDbl(PrepHours);

            if(SafeDbl(PrepUnits) != 0 && SafeDbl(PrepHours) != 0) {
                Response.Write("<td class=xl11020928 style='border-right:.5pt solid black; border-left:none; text-align:right' >" + FNum(cStr(SafeDiv(cStr(SafeDbl(PrepUnits)), cStr(SafeDbl(PrepHours)))), 2) + "</td>");
            }else{
                Response.Write("<td class=xl11020928 style='border-right:.5pt solid black; border-left:none; text-align:right' >&nbsp;</td>");
            }

            Response.Write("</tr>");

          } //Next

        }


        public void WriteBottomTotal(){

          Response.Write("<tr height=18 style='height:13.5pt'>");
          Response.Write("  <td height=18 class=xl4120928 style='height:13.5pt;border-top:none;border-right:.5pt solid black;'><b>Total</b></td>");
          Response.Write("  <td class=xl9120928 >" + FNum(cStr(Total_Accidents), 0) + "</td>");
          //Response.Write("  <td class=xl9120928 >" + FNum(Ave(arrTotalAccidents),2) + "</td>");
          Response.Write("  <td class=xl9120928 >" + FNum(cStr(SafeDiv(FNum(cStr(Total_Accidents), 0),(FNum(cStr(BudgetTotalUnits), 0)))*10000),2) + "</td>");  
  
          Response.Write("  <td class=xl9120928 >" + FNum(cStr(Total_Injuries), 0) + "</td>");
          //Response.Write("  <td class=xl9120928 >" +  FNum(Ave(arrTotalInjuryRep),2) + "</td>");
          Response.Write("  <td class=xl9120928 >" + FNum(cStr(SafeDiv(FNum(cStr(Total_Injuries), 0),(FNum(cStr(BudgetTotalUnits), 0)))*10000),2) + "</td>");  
	
          Response.Write("  <td class=xl9120928 >" + FNum(cStr(Total_LostTime), 2) + "</td>");
          //Response.Write("  <td class=xl9120928 >" +  FNum(Ave(arrTotalLostTime),2) + "</td>");
          Response.Write("  <td class=xl9120928 >" + FNum(cStr(SafeDiv(FNum(cStr(Total_LostTime), 2),(FNum(cStr(BudgetTotalUnits), 0)))*10000),2) + "</td>");  
  
          Response.Write("  <td class=xl9120928 >" + FNum(cStr(Total_NonMed), 0) + "</td>");
          //Response.Write("  <td class=xl9120928 >" +  FNum(Ave(arrTotalNonMed),2) + "</td>");
          Response.Write("  <td class=xl9120928 >" + FNum(cStr(SafeDiv(FNum(cStr(Total_NonMed), 0),(FNum(cStr(BudgetTotalUnits), 0)))*10000),2) + "</td>");  

          Response.Write("  <td class=xl9120928 colspan=2 >" + FNum(cStr(BudgetTotalUnits), 0) + "</td>");
          Response.Write("  <td class=xl9120928 colspan=2 >" + FCur(cStr(BudgetTotalHours), 2) + "</td>");
          Response.Write("  <td class=xl9120928 colspan=2 >" + FCur(cStr(BudgetTotalOTHrs), 2) + "</td>");
          double BudgetTotalLabor = BudgetTotalHours+BudgetTotalOTHrs;
          Response.Write("  <td class=xl9120928 colspan=2 >" + FCur(cStr(BudgetTotalLabor), 2) + "</td>");
          if (BudgetTotalUnits != 0 && BudgetTotalLabor != 0) {
            Response.Write("  <td class=xl9120928 colspan=2 >" + FCur(cStr(BudgetTotalLabor/BudgetTotalUnits), 2) + "</td>");
          }else{
            Response.Write("  <td class=xl9120928 colspan=2 >&nbsp;</td>");
          }

          //
          // Air Test
          //
          Response.Write("  <td class=xl9120928           >" + FNum(cStr(ATTotalUnits), 0) + "</td>");
          Response.Write("  <td class=xl9120928           >" + FNum(cStr(ATTotalHours), 2) + "</td>");
          if (ATTotalUnits != 0 && ATTotalHours != 0) {
            Response.Write("  <td class=xl9120928>" + FNum(cStr(ATTotalUnits/ATTotalHours), 2) + "</td>");
          }else{
            Response.Write("  <td class=xl9120928>&nbsp;</td>");
          }

          //
          // Prepping
          //
          Response.Write("  <td class=xl9120928           >" + FNum(cStr(PrepTotalUnits), 0) + "</td>");
          Response.Write("  <td class=xl9120928           >" + FNum(cStr(PrepTotalHours), 2) + "</td>");
          if (PrepTotalUnits != 0 && PrepTotalHours != 0) {
            Response.Write("  <td class=xl9120928>" + FNum(cStr(PrepTotalUnits/PrepTotalHours), 2) + "</td>");
          }else{
            Response.Write("  <td class=xl9120928>&nbsp;</td>");
          }
          Response.Write("</tr>");

          Response.Write("<tr height=18 style='height:13.5pt'>");
          Response.Write("  <td height=18 class=xl9120928 style='height:13.5pt;border-right:none;border-top:none'>&nbsp;</td>");
          Response.Write("  <td class=xl9120928 colspan=24 style='border-right:none'>&nbsp;</td>");
          Response.Write("</tr>");
        }

    }
}