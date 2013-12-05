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
    public partial class FacilityDailyCPU : PageBase
    {

          public DataReader rsPay, rsUnits, rsCPU, rsUnLoadHours,rsUnLoadUnits,rsLoadHours,rsLoadUnits,rsShuttlingHours,rsShuttlingUnits;
          public  DataReader rsSpottingTime,rsSpottingRCIn,rsSpottingRCOut,rsSpottingDOWN_TIME,rsSpottingHOURS,rsTrainingHours,rsClericalHours, rsRBHours,rsUAHours,rsUM,rsUMLO,rsSelfAudit,rsKPI,rsBudgetCPU,rsATHours,rsATUnits,rsPrepHours, rsPrepUnits;

          public string rc;

          public string selYear  = "";
          public string selMonth = "";
          public string sFac  = "";


          public string PageOneData = "";
          public string PageTwoData = "";
          public string PageTwoTotals ="";
          public string PageOneTotals = "";

          public string[,] arData = new string[31,21];
          public string[,] ar2Data = new string[31,22];

          public bool bPay, bUnits, bCPU, bUnLoadHours, bUnLoadUnits, bLoadHours, bLoadUnits, bShuttlingHours, bShuttlingUnits;
          public bool bSpottingTime, bSpottingRCIn, bSpottingRCOut, bSpottingDOWN_TIME, bSpottingHOURS, bTrainingHours, bClericalHours, bRBHours, bUAHours, bUM, bUMLO, bSelfAudit, bKPI, bBudgetCPU, bATHours, bATUnits, bPrepHours, bPrepUnits;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

           GrantAccess("Super, Admin, User");


          selYear  = Request["selYear2"];
          selMonth = Request["selMonth"];
          sFac  = System.Convert.ToString(Session["FacilityID"]);


 
          // Get Total Hours for( UNLOAD;
          rsUnLoadHours = new DataReader(getSQL(sFac, selMonth, selYear,"UL", "HOURS"));
          rsUnLoadHours.Open();
          bUnLoadHours = rsUnLoadHours.Read();
        
            
 
          // Get Total Units for( UNLOAD;
          rsUnLoadUnits = new DataReader(getSQL(sFac, selMonth, selYear,"UL", "UNITS"));
          rsUnLoadUnits.Open();
          bUnLoadUnits = rsUnLoadUnits.Read();

          // Get Total Hours for( LOAD;
          rsLoadHours = new DataReader(getSQL(sFac, selMonth, selYear,"LO", "HOURS"));
          rsLoadHours.Open();
          bLoadHours = rsLoadHours.Read();


          // Get Total Units for( LOAD;
          rsLoadUnits = new DataReader(getSQL(sFac, selMonth, selYear,"LO", "UNITS"));
          rsLoadUnits.Open();
          bLoadUnits = rsLoadUnits.Read();


           // Get Total Hours for( SHUTTLING;
           rsShuttlingHours = new DataReader(getSQL(sFac, selMonth, selYear,"SH", "HOURS"));
           rsShuttlingHours.Open();
           bShuttlingHours = rsShuttlingHours.Read();

           // Get Total Units for( SHUTTLING;
           rsShuttlingUnits = new DataReader(getSQL(sFac, selMonth, selYear,"SH", "UNITS"));
           rsShuttlingUnits.Open();
           bShuttlingUnits = rsShuttlingUnits.Read();
  
           // Get SPOTTING RC//s IN/OUT && Down Time;
           rsSpottingTime = new DataReader(getSpotSQL(sFac, selMonth, selYear, "SPOTTIME"));
           rsSpottingTime.Open();
           bSpottingTime = rsSpottingTime.Read();

           rsSpottingRCIn = new DataReader(getSpotSQL(sFac, selMonth, selYear, "RC_IN"));
           rsSpottingRCIn.Open();
           bSpottingRCIn = rsSpottingRCIn.Read();

           rsSpottingRCOut = new DataReader(getSpotSQL(sFac, selMonth, selYear, "RC_OUT"));
           rsSpottingRCOut.Open();
           bSpottingRCOut = rsSpottingRCOut.Read();
 
           rsSpottingDOWN_TIME = new DataReader(getSpotSQL(sFac, selMonth, selYear, "DOWN_TIME"));
           rsSpottingDOWN_TIME.Open();
           bSpottingDOWN_TIME = rsSpottingDOWN_TIME.Read();

           rsSpottingHOURS = new DataReader(getSpotTimeSQL(sFac,  selMonth, selYear));
           rsSpottingHOURS.Open();
           bSpottingHOURS = rsSpottingHOURS.Read();


         // Get Total Hours for( Training;

         rsTrainingHours = new DataReader(getSQL(sFac, selMonth, selYear,"TR", "HOURS"));
         rsTrainingHours.Open();
         bTrainingHours = rsTrainingHours.Read();

         // Get Total Hours for( Clerical;

         rsClericalHours = new DataReader(getSQL(sFac, selMonth, selYear,"CL", "HOURS"));
         rsClericalHours.Open();
         bClericalHours = rsClericalHours.Read();

         // Get Total Hours for( Misc. (Rebilling);

         rsRBHours = new DataReader(getSQL(sFac, selMonth, selYear,"RB", "HOURS"));
         rsRBHours.Open();
         bRBHours = rsRBHours.Read();

         // Get Total Hours for( UA;

         rsUAHours = new DataReader(getSQL(sFac, selMonth, selYear,"UA", "HOURS"));
         rsUAHours.Open();
         bUAHours = rsUAHours.Read();

         // Get Total Units/Man for( UL;

         rsUM = new DataReader(getUMSQL(sFac, selMonth, selYear, "UL"));
         rsUM.Open();
         bUM = rsUM.Read();

         // Get Total Units/Man for( UL;
         rsUMLO = new DataReader(getUMSQL(sFac, selMonth, selYear, "LO"));
         rsUMLO.Open();
         bUMLO = rsUMLO.Read();

         // Get Self Audit;
         rsSelfAudit = new DataReader(getSelfAuditSQL(sFac, selMonth, selYear));
         rsSelfAudit.Open();
         bSelfAudit = rsSelfAudit.Read();

         // Get KPIs;
         rsKPI = new DataReader(getKPISQL(sFac, selMonth, selYear));
         rsKPI.Open();
         bKPI = rsKPI.Read();

         // Get Budget/Cost Per Unit;
         rsBudgetCPU = new DataReader(getBCPUSQL(sFac, selMonth, selYear));
         rsBudgetCPU.Open();
         //rsBudgetCPU.Read();

         // Get Total Hours for( Air Test;
         rsATHours = new DataReader(getSQL(sFac, selMonth, selYear,"AT", "HOURS"));
         rsATHours.Open();
         bATHours = rsATHours.Read();

         // Get Total Units for( Air Test;
         rsATUnits = new DataReader(getSQL(sFac, selMonth, selYear,"AT", "UNITS"));
         rsATUnits.Open();
         bATUnits = rsATUnits.Read();

         // Get Total Hours for( Prepping;
         rsPrepHours = new DataReader(getSQL(sFac, selMonth, selYear,"RP", "HOURS"));
         rsPrepHours.Open();
         bPrepHours = rsPrepHours.Read();

         // Get Total Units for( Prepping;
         rsPrepUnits = new DataReader(getSQL(sFac, selMonth, selYear,"RP", "UNITS"));
         rsPrepUnits.Open();
         bPrepUnits = rsPrepUnits.Read();


          for(int i = 0 ; i < 31; i ++){
             for( int j = 0; j < 21; j++){
                arData[i,j] = "";
            }
          }

          for( int i = 0 ; i < 31; i++){
             for( int j = 0; j < 22;j++){
                ar2Data[i,j] = "";
             }
         }

          // Unloading;
          // Total Hours && Total Units;

          for( int i = 0 ; i < (cInt(Day(LastDayOfMonth(cDate(selMonth + "/1/" + selYear))))); i++){

                        arData[i, 0] = (i+1) + "-" + Left(MonthName(cInt(selMonth)), 3) + "  ";
                        ar2Data[i, 0] = arData[i, 0];
                        arData[i, 2] = FNumSP(rsUM.Fields(1), 0);

                        if (bUnLoadUnits ){
                                if((cInt(Day(cDate(rsUnLoadUnits.Fields(1))))-1) == i){
                                        arData[i, 3] = FNumSP(rsUnLoadUnits.Fields(0), 0);
                                        bUnLoadUnits = rsUnLoadUnits.Read();

                               }
                       }

                        if (bUnLoadHours ){
                                if((cInt(Day(cDate(rsUnLoadHours.Fields(1))))-1) == i){
                                        arData[i, 1] = FNumSP(rsUnLoadHours.Fields(0),2);
                                        arData[i, 4] = FNumSP(cStr(SafeDbl(arData[i, 3]) / SafeDbl(arData[i, 1])), 2);
                                        bUnLoadHours = rsUnLoadHours.Read();

                               }
                       }

                        if( SafeDbl(rsUM.Fields(1)) != 0){
                                arData[i, 5] =  FNumSP(cStr(SafeDbl(arData[i, 3]) / SafeDbl(rsUM.Fields(1))), 0);
                        }

                        bUM = rsUM.Read();
          }



         //////// Loading;
         //////// Total Hours && Total Units;

         for(int i = 0 ; i < (cInt(Day(LastDayOfMonth(cDate(selMonth + "/1/" + selYear))))); i++){

                        arData[i, 7]= FNumSP(rsUMLO.Fields(1),0);

                        if (bLoadUnits ){
                                if((cInt(Day(cDate(rsLoadUnits.Fields(1))))-1) == i){
                                        arData[i, 8] = FNumSP(rsLoadUnits.Fields(0),0);
                                        bLoadUnits = rsLoadUnits.Read();

                               }
                       }

                        if (bLoadHours ){
                                if((cInt(Day(cDate(rsLoadHours.Fields(1))))-1) == i){
                                        arData[i, 6]= FNumSP(rsLoadHours.Fields(0),2);
                                        arData[i, 9] =   FNumSP(cStr(SafeDbl(arData[i, 8])  /  SafeDbl(arData[i, 6])), 2);
                                        bLoadHours = rsLoadHours.Read();

                               }
                       }

                        if (SafeDbl(rsUMLO.Fields(1)) != 0){
                                arData[i,10] =  FNumSP(cStr(SafeDbl(arData[i, 8])   /  SafeDbl(rsUMLO.Fields(1))), 0);
                       }

                        arData[i,11] =  cStr(SafeDbl(arData[i,5]) + SafeDbl(arData[i,10]));

                        bUMLO = rsUMLO.Read();


         }


         // Shuttling;

         // Total Hours && Total Units;
 
         for(int i = 0 ; i < (cInt(Day(LastDayOfMonth(cDate(selMonth + "/1/" + selYear)))));i++){

                        if (bShuttlingUnits ){
                                if((cInt(Day(cDate(rsShuttlingUnits.Fields(1))))-1) == i){
                                        arData[i, 13] = FNumSP(rsShuttlingUnits.Fields(0), 0);
                                        bShuttlingUnits = rsShuttlingUnits.Read();

                               }
                       }

                        if (bShuttlingHours ){
                                if((cInt(Day(cDate(rsShuttlingHours.Fields(1))))-1) == i){
                                        arData[i, 12] = FNumSP(rsShuttlingHours.Fields(0), 2);
                                        arData[i, 14] = FNumSP(cStr(SafeDbl(arData[i, 13])  /  SafeDbl(arData[i, 12])), 2);
                                        bShuttlingHours = rsShuttlingHours.Read();

                               }
                       }

         }


          // Soptting;
          for( int i = 0 ; i <  (cInt(Day(LastDayOfMonth(cDate(selMonth + "/1/" + selYear))))); i++){

                        if (bSpottingTime ){
                                if((cInt(Day(cDate(rsSpottingTime.Fields(1))))-1) == i){
                                        arData[i, 15] = rsSpottingTime.Fields(0);
                                        bSpottingTime = rsSpottingTime.Read();

                               }
                       }

                        if (bSpottingRCIn ){
                                if((cInt(Day(cDate(rsSpottingRCIn.Fields(1))))-1) == i){
                                        arData[i, 16] = rsSpottingRCIn.Fields(0);
                                        bSpottingRCIn = rsSpottingRCIn.Read();

                               }
                       }

                        if (bSpottingRCOut ){
                                if((cInt(Day(cDate(rsSpottingRCOut.Fields(1))))-1) == i){
                                        arData[i, 17] = rsSpottingRCOut.Fields(0);
                                        bSpottingRCOut = rsSpottingRCOut.Read();

                               }
                       }

                        if (bSpottingDOWN_TIME ){
                                if((cInt(Day(cDate(rsSpottingDOWN_TIME.Fields(1))))-1) == i){
                                        arData[i, 18] = rsSpottingDOWN_TIME.Fields(0);
                                        bSpottingDOWN_TIME = rsSpottingDOWN_TIME.Read();

                               }
                       }

                        if (bSpottingHOURS ){
                                if((cInt(Day(cDate(rsSpottingHOURS.Fields(1))))-1) == i){
                                        arData[i, 19] = rsSpottingHOURS.Fields(0);
                                        if (SafeDbl(arData[i, 19]) != 0){
                                                arData[i, 20] = cStr((SafeDbl(arData[i, 16]) + SafeDbl(arData[i, 17])) / SafeDbl(arData[i, 19]));
                                       }
                                       bSpottingHOURS = rsSpottingHOURS.Read();

                               }
                       }

          }

         // Training  Total Hours;


         for(int i = 0 ; i< (cInt(Day(LastDayOfMonth(cDate(selMonth + "/1/" + selYear))))); i++){

                if(bTrainingHours){
                        if((cInt(Day(cDate(rsTrainingHours.Fields(1))))-1) == i){
                                ar2Data[i,  1] = FNumSP(rsTrainingHours.Fields(0), 2);
                                bTrainingHours = rsTrainingHours.Read();

                       }
               }

                if(bClericalHours){
                        if((cInt(Day(cDate(rsClericalHours.Fields(1))))-1) == i){
                                ar2Data[i,  2] = FNumSP(rsClericalHours.Fields(0), 2);
                                bClericalHours = rsClericalHours.Read();

                       }
               }

                if( bRBHours){
                        if((cInt(Day(cDate(rsRBHours.Fields(1))))-1) == i){
                                ar2Data[i,  3] = FNumSP(rsRBHours.Fields(0), 2);
                                bRBHours = rsRBHours.Read();

                       }
               }

                if( bUAHours){
                        if((cInt(Day(cDate(rsUAHours.Fields(1))))-1) == i){
                                ar2Data[i,  4] = FNumSP(rsUAHours.Fields(0), 2);
                                bUAHours = rsUAHours.Read();

                       }
               }

         }


        // KPI here;


         for( int i = 0 ; i < (cInt(Day(LastDayOfMonth(cDate(selMonth + "/1/" + selYear))))); i++){

                if( bKPI) {
                        if((cInt(Day(cDate(rsKPI.Fields(4))))-1) == i){
                           ar2Data[i, 5] = FNumSP(rsKPI.Fields(0),0);
                           ar2Data[i, 6] = FNumSP(rsKPI.Fields(1),2);
                           ar2Data[i, 7] = FNumSP(rsKPI.Fields(2),2);
                           ar2Data[i, 8] = FNumSP(rsKPI.Fields(3),2);

                           bKPI = rsKPI.Read();
                        }
               }

         }



         for( int i = 0 ; i < (cInt(Day(LastDayOfMonth(cDate(selMonth + "/1/" + selYear))))); i++){

                 if (bSelfAudit ){
                        if((cInt(Day(cDate(rsSelfAudit.Fields(2))))-1) == i){
                           ar2Data[i, 9] = FPerc(SafeDbl(rsSelfAudit.Fields(0)));
                           ar2Data[i, 10] = FPerc(SafeDbl(rsSelfAudit.Fields(1)));
                           bSelfAudit = rsSelfAudit.Read();

                       }
                }

         }



         // Budget / Cost Per Unit;
         int x = 0;
         while (rsBudgetCPU.Read()){
             
                   ar2Data[x, 11] = FNumSP(rsBudgetCPU.Fields(0),0);
                   ar2Data[x, 12] = FNumSP(rsBudgetCPU.Fields(1),2);
                   ar2Data[x, 13] = FNumSP(rsBudgetCPU.Fields(2),2);

                   ar2Data[x, 14] = FNumSP(cStr(SafeDbl(rsBudgetCPU.Fields(1)) + SafeDbl(rsBudgetCPU.Fields(2))),2);
                   if (SafeDbl(rsBudgetCPU.Fields(0)) != 0) {
                          ar2Data[x, 15] = FNumSP(cStr(SafeDbl(ar2Data[x, 14]) / SafeDbl(ar2Data[x, 11])),2);
                  }

                   x = x + 1;

         } //End Loop


         // Air Test Total Hours && Total Units;

         for( int i = 0 ; i < (cInt(Day(LastDayOfMonth(cDate(selMonth + "/1/" + selYear)))));i++){

                 if (bATUnits ){
                        if((cInt(Day(cDate(rsATUnits.Fields(1))))-1) == i){
                           ar2Data[i, 16] = FNumSP(rsATUnits.Fields(0),0);
                           bATUnits = rsATUnits.Read();

                       }
                }

                 if (bATHours ){
                        if((cInt(Day(cDate(rsATHours.Fields(1))))-1) == i){
                                ar2Data[i, 17] = FNumSP(rsATHours.Fields(0),2);
                                        if( SafeDbl(ar2Data[i, 17]) != 0){
                                                ar2Data[i, 18] = FNumSP(cStr(SafeDbl(ar2Data[i, 16]) / SafeDbl(ar2Data[i, 17])),2);
                                        }
                                        bATHours = rsATHours.Read();

                       }
                }

         }



         // Prepping Total Hours && Total Units;
         for(int i = 0; i < (cInt(Day(LastDayOfMonth(cDate(selMonth + "/1/" + selYear))))); i ++){

                  if (bPrepUnits ){
                                if((cInt(Day(cDate(rsPrepUnits.Fields(1))))-1) == i){
                                   ar2Data[i, 19] = rsPrepUnits.Fields(0);
                                   bPrepUnits = rsPrepUnits.Read();

                               }
                 }

                  if (bPrepHours ){
                                if((cInt(Day(cDate(rsPrepHours.Fields(1))))-1) == i){
                                        ar2Data[i, 20] = rsPrepHours.Fields(0);
                                                if(SafeDbl(ar2Data[i, 20]) != 0){
                                                        ar2Data[i, 21] = cStr(SafeDbl(ar2Data[i, 19]) / SafeDbl(ar2Data[i, 20]));
                                               }
                                                bPrepHours = rsPrepHours.Read();

                               }
                 }

         }

        }


        public string getSpotSQL(string sFac, string selMonth,string  sYear,string sFld){

          string strSQL = "";
          DateTime ThisMonthStart, ThisMonthEnd;

          ThisMonthStart = cDate(selMonth + "/1/" + selYear);
          ThisMonthEnd = ThisMonthStart.AddMonths(1);
          int nDays = ThisMonthEnd.Subtract(ThisMonthStart).Days;

            strSQL  = " SELECT IsNULL(DataValue, '') AS " + sFld + ", WorkDate  ";
            strSQL +=  " FROM FacilityMonitoringDataEntry ";
            strSQL +=  " WHERE FacilityID = " + sFac;
            strSQL +=  "      AND FieldName = '" + sFld + "' ";
            strSQL +=  "      AND WorkDate Between '" + cStr(selMonth) + "/1/" + cStr(selYear) + "' AND '" + cStr(cDate(cStr(selMonth) + "/1/" + cStr(selYear)).AddMonths(1).AddDays(-1)) + "' ";
            strSQL +=  " GROUP BY WorkDate, DataValue ";
            strSQL +=  " ORDER BY WorkDate ";

          return strSQL;

        }


        public string getSpotTimeSQL(string sFac,string selMonth,string sYear){

          string strSQL = "";
          DateTime ThisMonthStart, ThisMonthEnd;

          ThisMonthStart = cDate(selMonth + "/1/" + selYear);
          ThisMonthEnd = ThisMonthStart.AddMonths(1);
          int nDays = ThisMonthEnd.Subtract(ThisMonthStart).Days;


            strSQL  = " SELECT Sum(HoursPaid) AS TotalHours, WorkDate";
            strSQL +=  "   FROM Facility f INNER JOIN EmployeeTaskWorked ";
            strSQL +=  "       ON f.Id = EmployeeTaskWorked.FacilityID INNER JOIN Tasks ";
            strSQL +=  "        ON EmployeeTaskWorked.TaskID = Tasks.Id LEFT OUTER JOIN EmployeeTaskWorkedPay ";
            strSQL +=  "        ON EmployeeTaskWorked.Id = EmployeeTaskWorkedPay.EmployeeTaskWorkedId ";
            strSQL +=  "        WHERE (Tasks.TaskCode = 'SP') ";
            strSQL +=  "      AND  F.ID = " + sFac;
            strSQL +=  "      AND WorkDate Between '" + cStr(selMonth) + "/1/" + cStr(selYear) + "' AND '" + cStr(cDate(cStr(selMonth) + "/1/" + cStr(selYear)).AddMonths(1).AddDays(-1)) + "' ";
            strSQL +=  " GROUP BY WorkDate ";
            strSQL +=  " ORDER BY WorkDate ";

          return strSQL;

        }


        public string getSelfAuditSQL(string sFac,string selMonth,string sYear){

          string strSQL = "";
          DateTime ThisMonthStart, ThisMonthEnd;

          ThisMonthStart = cDate(selMonth + "/1/" + selYear);
          ThisMonthEnd = ThisMonthStart.AddMonths(1);
          int nDays = ThisMonthEnd.Subtract(ThisMonthStart).Days;

            strSQL = " SELECT  IsNULL(SUM(CASE FieldName WHEN 'BAY'        Then Cast(DataValue as float) Else 0 END), 0) AS IN_BAY,     ";
            strSQL +=  "       IsNULL(SUM(CASE FieldName WHEN 'FACILITY'   Then Cast(DataValue as float) Else  0 END), 0) AS FAC,    ";
            strSQL +=  "       WorkDate  ";
            strSQL +=  " FROM FacilityMonitoringDataEntry ";
            strSQL +=  " WHERE FacilityID = " + sFac;
            strSQL += "      AND WorkDate Between '" + cStr(selMonth) + "/1/" + cStr(selYear) + "' AND '" + cStr(cDate(cStr(selMonth) + "/1/" + cStr(selYear)).AddMonths(1).AddDays(-1)) + "' ";
            strSQL +=  " GROUP BY WorkDate ";
            strSQL +=  " ORDER BY WorkDate ";

          return strSQL;

        }


        public string getKPISQL(string sFac,string selMonth,string sYear){

          string strSQL = "";
          DateTime ThisMonthStart, ThisMonthEnd;

          ThisMonthStart = cDate(selMonth + "/1/" + selYear);
          ThisMonthEnd = ThisMonthStart.AddMonths(1);
          int nDays = ThisMonthEnd.Subtract(ThisMonthStart).Days;

            strSQL = " SELECT  IsNULL(SUM(CASE FieldName WHEN 'ACCIDENTS'   Then Cast(DataValue as float) Else 0 END), 0) AS ACC,     ";
            strSQL +=  "       IsNULL(SUM(CASE FieldName WHEN 'REP_INJURIES' Then Cast(DataValue as float) Else 0 END), 0) AS INJURIES,";
            strSQL +=  "       IsNULL(SUM(CASE FieldName WHEN 'LOST_TIME'   Then Cast(DataValue as float) Else 0 END), 0) AS LOSTTIME,";
            strSQL +=  "       IsNULL(SUM(CASE FieldName WHEN 'NON_MEDICAL' Then Cast(DataValue as float) Else 0 END), 0) AS NONMED,  ";
            strSQL +=  "       WorkDate  ";
            strSQL +=  " FROM FacilityMonitoringDataEntry ";
            strSQL +=  " WHERE FacilityID = " + sFac;
            strSQL += "      AND WorkDate BETWEEN '" + cStr(selMonth) + "/1/" + cStr(selYear) + "' AND '" + cStr(cDate(cStr(selMonth) + "/1/" + cStr(selYear)).AddMonths(1).AddDays(-1)) + "' ";
            strSQL +=  " GROUP BY WorkDate  ";
            strSQL +=  " ORDER BY WorkDate ";

          return strSQL;

        }


        public string getSQL(string sFac,string  selMonth,string selYear,string sTask,string sPayOrUnits){

          string strSQL = "";
          DateTime ThisMonthStart, ThisMonthEnd;

          ThisMonthStart = cDate(selMonth + "/1/" + selYear);
          ThisMonthEnd =  cDate(ThisMonthStart).AddMonths(1);
          int nDays = ThisMonthEnd.Subtract(ThisMonthStart).Days;

            if(sPayOrUnits == "UNITS"){
              strSQL  = " SELECT SUM(d.Units) AS TotalUnits, WorkDate ";
              strSQL +=  " FROM Facility f INNER JOIN FacilityProductionDetail d ON f.Id = d.FacilityID ";
              strSQL +=  "      INNER JOIN Tasks ON d.TaskId = Tasks.Id  ";
            }else{
               strSQL  = " SELECT SUM(HoursPaid) AS TotalHours, WorkDate ";
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
            strSQL +=  "      AND WorkDate Between '" + cStr(selMonth) + "/1/" + cStr(selYear) + "' AND '" + cStr(cDate(cStr(selMonth) + "/1/" + cStr(selYear)).AddMonths(1).AddDays(-1))  + "' ";
            strSQL +=  " GROUP BY WorkDate ";

          strSQL +=  " ORDER BY WorkDate ";
          return strSQL;

        }


        public string getUMSQL(string sFac,string selMonth,string selYear,string sTask){

          string strSQL = "";
          string sWhere = "";
          DateTime ThisMonthStart, ThisMonthEnd;

          ThisMonthStart = cDate(selMonth + "/1/" + selYear);
          ThisMonthEnd = cDate(ThisMonthStart).AddMonths(1);
          int nDays = ThisMonthEnd.Subtract(ThisMonthStart).Days;

          for( int I=1; I <= nDays;I++){
            if(sTask == "LO"){
              sWhere = " Where Datepart(yyyy,WorkDate)=" + selYear + " AND DatePart(m,Workdate)=" + cStr(selMonth) + " AND DatePart(d,Workdate)=" + cStr(I) + " AND FacilityID=" + sFac +  " AND TaskID IN (1)) ";
            }else{
              sWhere = " Where Datepart(yyyy,WorkDate)=" + selYear + " AND DatePart(m,Workdate)=" + cStr(selMonth) + " AND DatePart(d,Workdate)=" + cStr(I) + " AND FacilityID=" + sFac +  " AND TaskID IN (2)) ";
           }
            strSQL +=  " SELECT TotalUnits=IsNULL((Select Sum(Units) From FacilityProductionDetail " + sWhere + ",0), ";
            strSQL +=  " TotalMen=IsNULL((Select Count(Distinct EmployeeID) From EmployeeTaskWorked "  + sWhere + ",0), ";
            strSQL +=  " TotalDays=IsNULL((Select Count(Distinct Datepart(d,WorkDate)) From EmployeeTaskWorked " + sWhere + ",0), TheDay=" + cStr(I);

            if( I < ( nDays)){
              strSQL +=  " UNION ";
            }

          }

          strSQL +=  " ORDER BY TheDay ";
          return strSQL;

        }

        public string getBCPUSQL(string sFac,string selMonth,string selYear){

          string strSQL = "";
          string sWhereUnits, sWherePay;
          DateTime ThisMonthStart, ThisMonthEnd;

          ThisMonthStart = cDate(selMonth + "/1/" + selYear);
          ThisMonthEnd = cDate(ThisMonthStart).AddMonths(1);
          int nDays = ThisMonthEnd.Subtract(ThisMonthStart).Days;

          for(int I=1 ; I <= nDays; I ++){

            sWhereUnits = "  Datepart(yyyy,WorkDate)=" + selYear + " AND DatePart(m,Workdate)=" + cStr(selMonth) + " AND DatePart(d,Workdate)=" + cStr(I) + " AND FacilityID=" + sFac +  " AND TaskID IN (1,2)) ";
            sWherePay   = "  Datepart(yyyy,WorkDate)=" + selYear + " AND DatePart(m,Workdate)=" + cStr(selMonth) + " AND DatePart(d,Workdate)=" + cStr(I) + " AND FacilityID=" + sFac +  " ) ";

            strSQL +=  " SELECT TotalUnits=IsNULL((Select Sum(Units) From FacilityProductionDetail WHERE " + sWhereUnits + ",0), ";
            strSQL +=  "  TotalRegHours=IsNULL((Select Sum(PayAmount) ";
            strSQL +=  "  FROM EmployeeTaskWorkedPay INNER JOIN EmployeeTaskWorked ON EmployeeTaskWorkedPay.EmployeeTaskWorkedId = EmployeeTaskWorked.Id ";
            strSQL +=  "  WHERE PayMultiplier=1 AND "  + sWherePay + ",0), ";
            strSQL +=  "  TotalOTHours=IsNULL((Select Sum(PayAmount) ";
            strSQL +=  "  FROM EmployeeTaskWorkedPay INNER JOIN EmployeeTaskWorked ON EmployeeTaskWorkedPay.EmployeeTaskWorkedId = EmployeeTaskWorked.Id ";
            strSQL +=  "  WHERE PayMultiplier <> 1 AND "  + sWherePay + ",0),  TheDay=" + cStr(I);

            if( I < (nDays)){
              strSQL +=  " UNION ";
           }
          }

          strSQL +=  " ORDER BY TheDay ";
          return strSQL;

        }

        //Returns Last Thurs. of the Month;
        public DateTime LastDayOfMonth(DateTime mydate){

                DateTime tmpLastDayOfMonth = mydate;

                while (Month(tmpLastDayOfMonth) == Month(mydate)){
                   tmpLastDayOfMonth = tmpLastDayOfMonth.AddDays(1);
                }

                return tmpLastDayOfMonth.AddDays(-1);

        }
        
        public string FNumSP(string num, int dec){

            string FNumSP = "";

            if(! (num == null) ){
                if (isNumeric(cStr(num))){
                  if  (cDbl(num) != 0){
                     FNumSP = FormatNumber(cStr(num), dec);
                 }
               }
           }

            return FNumSP;

        }

        public string FCurSP(string num){
            string FCurSP = "";
            if (! (num == null)){
                if (isNumeric(cStr(num))){
                  if (cDbl(num) != 0){
                      FCurSP = "$ " + cStr(FormatNumber(num, 2));
                 }
               }
           }

           return  FCurSP;
 
        }

        public string FPerc(double num){
                if(num != 0.0){
                        return FormatNumber(cStr(num), 2) + "%";
                }else{
                        return "";
               }
        }

        public Double Sum(string[,] ar,int col){
            Double sum = 0;
          for( int i = 0 ; i < 31;i++){
              sum = sum + SafeDbl(ar[i,col]);
          }
          return sum;
        }

        public int Count(string[,] ar,int col){
          int count = 0;
          for( int i = 0 ; i < 31; i ++){
                if ( Trim(ar[i,col]) != ""){
                    count = count + 1;
                }
          }
            return count;
        }

        public double Ave(string[,] ar,int col){

            double localcount = 0;
            double localAve = 0;
            double locallast = 0;

           for( int i = 0 ; i < 31; i++){
             locallast = 0;
             if(col == 9 || col == 10){
                        locallast = SafeDbl(Replace(cStr(ar[i,col]),"%",""));
             }else{
                if (SafeDbl(ar[i,col]) != 0){
                        locallast = SafeDbl(ar[i,col]);
               }
            }


             localAve = localAve + locallast;
             if (locallast != 0){
                localcount = localcount + 1;
            }
           } //Next

           if (localcount > 0){
              localAve = localAve / localcount;
          }
           return  localAve;
        }

        public string FTime(string x){
                if( Trim(x) != ""){
                        return Left(x,Len(x)-3);
                }else{
                        return "";
               }
        }

        public void  WritePageOneData(){
           Response.Write(PageOneData);
        }

        public void  CreatePageOneData(){

          string rc;

          for( int i = 0 ; i < 31;i++){

              if(i % 2 == 0){
                rc = "#DEDFFF";
              }else{
                rc = "#FFFFFF";
             }
                  PageOneData =    PageOneData + "<tr height=17 style='height:11.00pt' bgcolor='" + rc + "'>";
                  PageOneData =    PageOneData + "<td height=17 class=xl71 style='height:11.00pt' align='right'>" + arData[i,0] + "</td>";
                  PageOneData =    PageOneData + "<td class=xl65 style='border-top:none' align='right'>" + FNumSP(arData[i,1], 2) + "</td>";
                  PageOneData =    PageOneData + "<td class=xl67 style='border-top:none' align='right'>" + FNumSP(arData[i,2], 0) + "</td>";
                  PageOneData =    PageOneData + "<td class=xl67 style='border-top:none' align='right'>" + FNumSP(arData[i,3], 0) + "</td>";
                  PageOneData =    PageOneData + "<td class=xl72 style='border-top:none' align='right'>" + FNumSP(arData[i,4], 2) + "</td>";
                  PageOneData =    PageOneData + "<td class=xl81 style='border-top:none' align='right'>" + FNumSP(arData[i,5], 0) + "</td>";
                  PageOneData =    PageOneData + "<td class=xl103 align='right'>" + FNumSP(arData[i,6], 2) + "</td>";
                  PageOneData =    PageOneData + "<td class=xl67 style='border-top:none' align='right'>" + FNumSP(arData[i,7], 0) + "</td>";
                  PageOneData =    PageOneData + "<td class=xl67 style='border-top:none' align='right'>" + FNumSP(arData[i,8], 0) + "</td>";
                  PageOneData =    PageOneData + "<td class=xl73 align='right'>" + FNumSP(arData[i,9], 0) + "</td>";
                  PageOneData =    PageOneData + "<td class=xl81 style='border-top:none;border-left:none' align='right'>" + FNumSP(arData[i,10], 0) + "</td>";
                  PageOneData =    PageOneData + "<td class=xl83 style='border-top:none' align='right'>" + FNumSP(arData[i,11], 0) + "</td>";
                  PageOneData =    PageOneData + "<td class=xl87 style='border-left:none' align='right'>" + FNumSP(arData[i,12], 2) + "</td>";
                  PageOneData =    PageOneData + "<td class=xl86 align='right'>" + FNumSP(arData[i,13], 0) + "</td>";
                  PageOneData =    PageOneData + "<td class=xl73 align='right'>" + FNumSP(arData[i,14], 2) + "</td>";
                  PageOneData =    PageOneData + "<td class=xl39 style='border-top:none' align='right'>" + arData[i,15] + "</td>";
                  PageOneData =    PageOneData + "<td class=xl32 style='border-top:none' align='right'>" + FNumSP(arData[i,16], 0) + "</td>";
                  PageOneData =    PageOneData + "<td class=xl32 style='border-top:none' align='right'>" + FNumSP(arData[i,17], 0) + "</td>";
                  PageOneData =    PageOneData + "<td class=xl32 style='border-top:none' align='right'>" + FNumSP(arData[i,18], 2) + "</td>";
                  PageOneData =    PageOneData + "<td class=xl32 style='border-top:none' align='right'>" + FNumSP(arData[i,19], 2) + "</td>";
                  PageOneData =    PageOneData + "<td class=xl47 style='border-top:none' align='right'>" + FNumSP(arData[i,20], 2) + "</td>";
                  PageOneData =    PageOneData + "</tr>";
          } //Next
        } //End Function


        public void  WritePageOneTotals(){

                    string tempTotal1, tempTotal2;

                    PageOneTotals =    PageOneTotals + " <tr height=19 style='height:14.25pt'>";
                    PageOneTotals =    PageOneTotals + " <td height=19 class=xl105 style='height:14.25pt' align='center'>TOTAL</td>";
                    PageOneTotals =    PageOneTotals + " <td class=xl69 align='right'>" + FNumSP(cStr(Sum(arData, 1)), 2) + "</td>";
                    PageOneTotals =    PageOneTotals + " <td class=xl77 align='right'>&nbsp;</td>";
                    PageOneTotals =    PageOneTotals + " <td class=xl70 align='right'>" + FNumSP(cStr(Sum(arData, 3)), 0) + "</td>";
                    PageOneTotals =    PageOneTotals + " <td class=xl74 align='right'>" + FNumSP(cStr(SafeDiv(cStr(Sum(arData, 3)), cStr(Sum(arData, 1)))), 2) + "</td>";
                    PageOneTotals =    PageOneTotals + " <td class=xl76 align='right'>&nbsp;</td>";
                    PageOneTotals =    PageOneTotals + " <td class=xl69 align='right'>" + FNumSP(cStr(Sum(arData, 6)), 2) + "</td>";
                    PageOneTotals =    PageOneTotals + " <td class=xl77 align='right'>&nbsp;</td>";
                    PageOneTotals =    PageOneTotals + " <td class=xl70 align='right'>" + FNumSP(cStr(Sum(arData, 8)), 2) + "</td>";
                    PageOneTotals =    PageOneTotals + " <td class=xl74 align='right'>" + FNumSP(cStr(SafeDiv(cStr(Sum(arData, 8)), cStr(Sum(arData, 6)))),2) + "</td>";
                    PageOneTotals =    PageOneTotals + " <td class=xl76 align='right'>&nbsp;</td>";
                    PageOneTotals =    PageOneTotals + " <td class=xl91 align='right'  >" + FNumSP(cStr(Ave(arData, 11)),0) + "</td>";
                    PageOneTotals =    PageOneTotals + " <td class=xl69 align='right'>" + FNumSP(cStr(Sum(arData, 12)), 2) + "</td>";
                    PageOneTotals =    PageOneTotals + " <td class=xl70 align='right'>" + FNumSP(cStr(Sum(arData, 13)), 0) + "</td>";
                    PageOneTotals =    PageOneTotals + " <td class=xl74 align='right'>" + FNumSP(cStr(SafeDiv(cStr(Sum(arData, 13)), cStr(Sum(arData, 12)))), 2) + "</td>";
                    PageOneTotals =    PageOneTotals + " <td class=xl50>&nbsp;</td>";
                    PageOneTotals =    PageOneTotals + " <td class=xl49 align='right'>" + FNumSP(cStr(Sum(arData, 16)), 0) + "</td>";
                    PageOneTotals =    PageOneTotals + " <td class=xl49 align='right'>" + FNumSP(cStr(Sum(arData, 17)), 0) + "</td>";
                    PageOneTotals =    PageOneTotals + " <td class=xl49 align='right'>" + FNumSP(cStr(Sum(arData, 18)), 2) + "</td>";
                    PageOneTotals =    PageOneTotals + " <td class=xl49 align='right'>" + FNumSP(cStr(Sum(arData, 19)), 2) + "</td>";
                    PageOneTotals =    PageOneTotals + " <td class=xl51 align='right'>" + FNumSP(cStr(SafeDiv(cStr(Sum(arData, 16)), cStr(Sum(arData, 19)))),2) + "</td>";
                    PageOneTotals =    PageOneTotals + " </tr>";
                    PageOneTotals =    PageOneTotals + " <tr height=5 style='height:5pt'>";
                    PageOneTotals =    PageOneTotals + "  <td height=5 colspan=24 style='height:5pt;mso-ignore:colspan'></td>";
                    PageOneTotals =    PageOneTotals + " </tr>";

                    Response.Write(PageOneTotals);

        }

        public void WritePageTwoData(){
            Response.Write(PageTwoData);
        }

        public void CreatePageTwoData(){

            string rc;
 
            for( int i = 0; i < 31; i++){

                if(i % 2 == 0){
                rc = "#DEDFFF";
                }else{
                rc = "#FFFFFF";
                }

                PageTwoData = PageTwoData  + "<tr height=17 style='height:11.00pt' bgcolor='" + rc + "'>";
                PageTwoData = PageTwoData  + "  <td height=17 class=xl71 style='height:11.00pt'>" + ar2Data[i,0] + "</td>";
                PageTwoData = PageTwoData  + "  <td class=xl88 align=right style='border-top:none'>" + FNumSP(ar2Data[i,1], 2) + "</td>";
                PageTwoData = PageTwoData  + "  <td class=xl72 align=right style='border-top:none'>" + FNumSP(ar2Data[i,2], 2) + "</td>";
                PageTwoData = PageTwoData  + "  <td class=xl72 align=right style='border-top:none'>" + FNumSP(ar2Data[i,3], 2) + "</td>";
                PageTwoData = PageTwoData  + "  <td class=xl72 align=right style='border-top:none'>" + FNumSP(ar2Data[i,4], 2) + "</td>";
                PageTwoData = PageTwoData  + "  <td class=xl53 align=right style='border-top:none'>" + FNumSP(ar2Data[i,5], 0) + "</td>";
                PageTwoData = PageTwoData  + "  <td class=xl34 align=right style='border-top:none'>" + FNumSP(ar2Data[i,6], 2) + "</td>";
                PageTwoData = PageTwoData  + "  <td class=xl32 align=right style='border-top:none'>" + FNumSP(ar2Data[i,7], 2) + "</td>";
                PageTwoData = PageTwoData  + "  <td class=xl32 align=right style='border-top:none'>" + FNumSP(ar2Data[i,8], 0) + "</td>";
                PageTwoData = PageTwoData  + "  <td class=xl32 align=right style='border-top:none'>" + ar2Data[i,9] + "</td>";
                PageTwoData = PageTwoData  + "  <td class=xl32 align=right style='border-top:none'>" + ar2Data[i,10] + "</td>";
                PageTwoData = PageTwoData  + "  <td class=xl92 align=right style='border-top:none'>" + FNumSP(ar2Data[i,11], 0) + "</td>";
                PageTwoData = PageTwoData  + "  <td class=xl94 align=right style='border-top:none'>" + FNumSP(ar2Data[i,12], 2) + "</td>";
                PageTwoData = PageTwoData  + "  <td class=xl94 align=right style='border-top:none'>" + FNumSP(ar2Data[i,13], 2) + "</td>";
                PageTwoData = PageTwoData  + "  <td class=xl94 align=right style='border-top:none'>" + FNumSP(ar2Data[i,14], 2) + "</td>";
                PageTwoData = PageTwoData  + "  <td class=xl95 align=right style='border-top:none'>" + FNumSP(ar2Data[i,15], 2) + "</td>";
                PageTwoData = PageTwoData  + "  <td class=xl101  align=right style='border-top:none'>" + FNumSP(ar2Data[i,16], 0) + "</td>";
                PageTwoData = PageTwoData  + "  <td class=xl89 align=right>" + FNumSP(ar2Data[i,17], 2) + "</td>";
                PageTwoData = PageTwoData  + "  <td class=xl75  align=right>" + FNumSP(ar2Data[i,18], 2) + "</td>";
                PageTwoData = PageTwoData  + "  <td class=xl101 align=right style='border-left:none'>" + FNumSP(ar2Data[i,19], 0) + "</td>";
                PageTwoData = PageTwoData  + "  <td class=xl89 align=right>" + FNumSP(ar2Data[i,20], 2) + "</td>";
                PageTwoData = PageTwoData  + "  <td class=xl75 align=right>" + FNumSP(ar2Data[i,21], 2) + "</td>";
                PageTwoData = PageTwoData  + " </tr>";
            } //Next

            }

        public void WritePageTwoTotals(){

            PageTwoTotals =    PageTwoTotals + "<tr height=19 style='height:14.25pt'>";
            PageTwoTotals =    PageTwoTotals + "<td height=19 class=xl105 style='height:14.25pt' align='center'>TOTAL</td>";
            PageTwoTotals =    PageTwoTotals + "<td class=xl90 align=center >" + FNumSP(cStr(Sum(ar2Data, 1)), 2) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td class=xl90 align=center >" + FNumSP(cStr(Sum(ar2Data, 2)), 2) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td class=xl90 align=center >" + FNumSP(cStr(Sum(ar2Data, 3)), 2) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td class=xl105 align=center style='border-top:1.5pt solid windowtext'>" + FNumSP(cStr(Sum(ar2Data, 4)), 2) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td class=x149 align=center style='border-top:1.5pt solid windowtext'>" + FNumSP(cStr(Sum(ar2Data, 5)), 0) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td class=xl49 align=center>" + FNumSP(cStr(Sum(ar2Data, 6)), 2) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td class=xl49 align=center>" + FNumSP(cStr(Sum(ar2Data, 7)), 2) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td class=xl49 align=center>" + FNumSP(cStr(Sum(ar2Data, 8)), 0) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td class=xl49 align=center>" + Count(ar2Data, 9) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td class=xl49 align=center>" + Count(ar2Data, 10) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td class=xl96 align=center >" + FNumSP(cStr(Sum(ar2Data, 11)), 0) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td class=xl97 align=center  >" + FCurSP(cStr(Sum(ar2Data, 12))) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td class=xl97 align=center  >" + FCurSP(cStr(Sum(ar2Data, 13))) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td class=xl97 align=center  >" + FCurSP(cStr(Sum(ar2Data, 14))) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td class=xl97 align=center  >" + FCurSP(cStr(SafeDiv(cStr(Sum(ar2Data, 14)),cStr(Sum(ar2Data, 11))))) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td class=xl96 align=center >" + FNumSP(cStr(Sum(ar2Data, 16)), 0) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td class=xl90 align=center >" + FNumSP(cStr(Sum(ar2Data, 17)), 2) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td class=xl102 align=center >" + FNumSP(cStr(SafeDiv(cStr(Sum(ar2Data, 16)),cStr(Sum(ar2Data, 17)))), 2) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td class=xl96 align=center >" + FNumSP(cStr(Sum(ar2Data, 19)), 0) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td class=xl90 align=center >" + FNumSP(cStr(Sum(ar2Data, 20)), 2) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td class=xl102 align=center >" + FNumSP(cStr(SafeDiv(cStr(Sum(ar2Data, 19)),cStr(Sum(ar2Data, 20)))), 2) + "</td>";
            PageTwoTotals =    PageTwoTotals + "</tr>";
            PageTwoTotals =    PageTwoTotals + "<tr height=19 style='height:14.25pt'>";
            PageTwoTotals =    PageTwoTotals + "<td class=xl105></td>";
            PageTwoTotals =    PageTwoTotals + "<td height=19 colspan=4 class=xl105 style='height:14.25pt' align=center>&nbsp;&nbsp;&nbsp;&nbsp;Accident / Injury per 10,000 =</td>";
            PageTwoTotals =    PageTwoTotals + "<td height=19 class=xl97 align=center><b>" + FNumSP(cStr(Ave(ar2Data, 5)), 2) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td height=19 class=xl97 align=center><b>" + FNumSP(cStr(Ave(ar2Data, 6)), 2) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td height=19 class=xl97 align=center><b>" + FNumSP(cStr(Ave(ar2Data, 7)), 2) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td height=19 class=xl97 align=center><b>" + FNumSP(cStr(Ave(ar2Data, 8)), 2) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td height=19 class=xl97 align=center><b>" + FPerc(SafeDbl(cStr(Ave(ar2Data, 9)))) + "</td>";
            PageTwoTotals =    PageTwoTotals + "<td height=19 class=xl105 align=center><b>" + FPerc(SafeDbl(cStr(Ave(ar2Data, 10)))) + "</td>";
            PageTwoTotals =    PageTwoTotals + "</tr>";

            Response.Write(PageTwoTotals);
        }

    }
}