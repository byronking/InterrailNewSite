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

    public partial class FacilityRegionalCPU : PageBase
    {

        public string selYear, sFirstDay, sLastDay;
        public string selRegion, facMonth;
        public string[,] arRowHeader = new string[53, 4];
        public string strColor;
        public DataReader rsTitles, rsLoadBudget;
        public string pageSetNumber, stringBuffer,regionName;
        public int i, j, pageIndex, pageCount, weekIndex, pageSize, facilityCount, weekCount,selMonth;

        public string[] loadBudget = new string[50];
        public string[] unloadBudget = new string[50];
        public string[] spottingBudget = new string[50];//------------- NEW SPOTTING INFO ADDED;
        public string[] monthlyPerc = new string[12];
        public string[] monthlyBudPerc = new string[50];
        public string loadMonthlyBudgetYTD, unLoadMonthlyBudgetYTD, spottingMonthlyBudgetYTD, originalTotalVolumeYTD;//------------- NEW SPOTTING INFO ADDED;
        
        public string[] Page1TitleLine = new string[50];
        public string[] Page2TitleLine = new string[50];
        public string[] arWeeklyDate = new string[52];
        public string[] Left3TitleLine2 = new string[52];
        public string[] facilityId = new string[50];
        public string[] arTitles = new string[50];

        public string[,] PageLoadData = new string[52,50];
        public string[] RightLoadData = new string[52];
        public string[,] actualLoad = new string[52,50];
        public string[,] PageLoadBudget = new string[52,50];
        public string[] RightLoadBudget = new string[52];
        public string[,] budgetedLoad = new string[52,50];
        public string[] budgetedLoadYTD = new string[50];

        public string[,] PageUnloadBudget = new string[52,50];
        public string[] RightUnloadBudget = new string[52];
        public string[,] actualUnLoad = new string[52,50];
        public string[,] budgetedUnLoad = new string[52,50];
        public string[] RightUnLoadData = new string[52];
        public string[,] PageUnLoadData = new string[52,50];
        public string[] budgetedUnLoadYTD = new string[50];
        public string[,] PageSpottingBudget = new string[52,50];
        public string[] RightSpottingBudget = new string[52];
        public string[,] actualSpotting = new string[52,50];
        public string[,] budgetedSpotting = new string[52,50];
        public string[] RightSpottingData = new string[52];
        public string[,] PageSpottingData = new string[52,50];
        public string[] budgetedSpottingYTD = new string[50];//------------- NEW SPOTTING INFO ADDED;

        public string LeftLoadVariance;
        public string[] RightLoadVariance = new string[52];
        public string[,] PageLoadVariance = new string[52,50];
        public string[,] varianceLoad = new string[52,50];
        public string[] varianceLoadYTD = new string[50];
        public string LeftUnLoadVariance;
        public string[] RightUnLoadVariance = new string[52];
        public string[,] PageUnLoadVariance = new string[52,50];
        public string[,] varianceUnLoad = new string[52,50];
        public string[] varianceUnLoadYTD = new string[50];
        public string LeftSpottingVariance;
        public string[] RightSpottingVariance = new string[52];
        public string[,] PageSpottingVariance = new string[52,50];
        public string[,] varianceSpotting = new string[52,50];
        public string[] varianceSpottingYTD = new string[50];//------------- NEW SPOTTING INFO ADDED;

        public string LeftTotalVolume2;
        public string[] RightTotalVolume2 = new string[52];
        public string[,] PageTotalVolume2 = new string[52,50];
        public string[,] totalVolume = new string[52,50];
        public string[] totalVolumeYTD = new string[50];
        public string LeftTotalVolume1;
        public string[] RightTotalVolume1 = new string[52];
        public string[,] PageTotalVolume1 = new string[52,50];
        public string[,] budgetedVolume = new string[52,50];
        public string[] budgetedVolumeYTD = new string[50];
        public string LeftPayRollIRT;
        public string[] RightPayRollIRT = new string[52];
        public string[,] PagePayRollIRT = new string[52,50];
        public string[,] payRollIRT = new string[52,50];
        public string LeftPayRollTEMP;
        public string[] RightPayRollTEMP = new string[52];
        public string[,] PagePayRollTEMP = new string[52,50];
        public string[,] payRollTEMP = new string[52,50];
        public string LeftTotalPayRoll;
        public string[] RightTotalPayRoll = new string[52];
        public string[,] PageTotalPayRoll = new string[52,50];
        public string[,] payRollTotal = new string[52,50];
        public string[] payRollTotalYTD = new string[50];
        public string LeftPayRollHrsIRT;
        public string[] RightPayRollHrsIRT = new string[52];
        public string[,] PagePayRollHrsIRT = new string[52,50];
        public string[,] payRollHrsIRT = new string[52,50];
        public string LeftPayRollHrsTEMP;
        public string[] RightPayRollHrsTEMP = new string[52];
        public string[,] PagePayRollHrsTEMP = new string[52,50];
        public string[,] payRollHrsTEMP = new string[52,50];
        public string LeftTotalPayRollHrs;
        public string[] RightTotalPayRollHrs = new string[52];
        public string[,] PageTotalPayRollHrs = new string[52,50];
        public string[,] payRollHrsTotal = new string[52,50];
        public string[] payRollHrsYTD = new string[50];
        public string LeftOverTime;
        public string[] RightOverTime = new string[52];
        public string[,] PageOverTime = new string[52,50];
        public string[,] overTimeTotal = new string[52,50];
        public string LeftOTasPayroll;
        public string[] RightOTasPayroll = new string[52];
        public string[,] PageOTasPayroll = new string[52,50];
        public string[,] OTasPayrollTotal = new string[52,50];
        public string[] OTasPayrollYTD = new string[50];
        public string LeftEmployeesIRT;
        public string[] RightEmployeesIRT = new string[52];
        public string[,] PageEmployeesIRT = new string[52,50];
        public string[,] employeesIRTTotal = new string[52,50];
        public string LeftEmployeesTEMP;
        public string[] RightEmployeesTEMP = new string[52];
        public string[,] PageEmployeesTEMP = new string[52,50];
        public string[,] employeesTEMPTotal = new string[52,50];
        public string LeftEmployeesTotal;
        public string[] RightEmployeesTotal = new string[52];
        public string[,] PageEmployeesTotal = new string[52,50];
        public string[,] employeesTotal = new string[52,50];
        public string[] employeesTotalYTD = new string[50];
        public string LeftEmployeesUnits;
        public string[] RightEmployeesUnits = new string[52];
        public string[,] PageEmployeesUnits = new string[52,50];
        public string[,] employeesUnitsTotal = new string[52,50];
        public string LeftEmployeesAvgIRT;
        public string[] RightEmployeesAvgIRT = new string[52];
        public string[,] PageEmployeesAvgIRT = new string[52,50];
        public string[,] employeesAvgIRTTotal = new string[52,50];
        public string[] employeesAvgIRTYTD = new string[50];
        public string LeftEmployeesAvgTEMP;
        public string[] RightEmployeesAvgTEMP = new string[52];
        public string[,] PageEmployeesAvgTEMP = new string[52,50];
        public string[,] employeesAvgTEMPTotal = new string[52,50];
        public string[] employeesAvgTEMPYTD = new string[50];
        public string LeftMiscLaborRep;
        public string[] RightMiscLaborRep = new string[52];
        public string[,] PageMiscLaborRep = new string[52,50];
        public string[,] MiscLaborRepTotal = new string[52,50];
        public string LeftMiscLaborBud;
        public string[] RightMiscLaborBud = new string[52];
        public string[,] PageMiscLaborBud = new string[52,50];
        public string[,] MiscLaborBudTotal = new string[52,50];
        public string[,] actualMiscLaborBud = new string[52,50];
        public string LeftDiffBudPayRoll;
        public string[] RightDiffBudPayRoll = new string[52];
        public string[,] PageDiffBudPayRoll = new string[52,50];
        public string[] DiffBudPayRollTotal = new string[52];
        public string LeftDiffMiscBudPayRoll;
        public string[] RightDiffMiscBudPayRoll = new string[52];
        public string[,] PageDiffMiscBudPayRoll = new string[52,50];
        public string[] DiffMiscBudPayRollTotal = new string[52];
        public string LeftCPU;
        public string[] RightCPU = new string[52];
        public string[,] PageCPU = new string[52,50];
        public string[,] CPUTotal = new string[52,50];
        public string[] CPUYTD = new string[50];
        public string LeftBudgetedCPU;
        public string[] RightBudgetedCPU = new string[52];
        public string[,] PageBudgetedCPU = new string[52,50];
        public string[,] BudgetedCPUTotal = new string[52,50];
        public string[] BudgetedCPUYTD = new string[50];
        public string LeftCPUVariance;
        public string[] RightCPUVariance = new string[52];
        public string[,] PageCPUVariance = new string[52,50];
        public string[,] CPUVarianceTotal = new string[52,50];
        public string[] DiffCPUVarianceTotal = new string[52];
        public string[] CPUVarianceYTD = new string[50];
        public string LeftDiffFromBudget;
        public string[] RightDiffFromBudget = new string[52];
        public string[,] PageDiffFromBudget = new string[52,50];
        public string[] DiffFromBudgetTotal = new string[52];
        public string[] DiffFromBudgetYTD = new string[50];
        public string LeftOvertimeCost;
        public string[] RightOvertimeCost = new string[52];
        public string[,] PageOvertimeCost = new string[52,50];
        public string[,] OvertimeCostTotal = new string[52,50];
        public string[] OvertimeCostYTD = new string[50];
        public string LeftHolidays;
        public string[] RightHolidays = new string[52];
        public string[,] PageHolidays = new string[52,50];
        public string[,] HolidaysTotal = new string[52,50];
        public string LeftAvgHourlyPay;
        public string[] RightAvgHourlyPay = new string[52];
        public string[,] PageAvgHourlyPay = new string[52,50];
        public string[,] AvgHourlyPayTotal = new string[52,50];
        public string[] AvgHourlyPayYTD = new string[50];
        public string LeftBVTitle;
        public string[] RightBVTitle = new string[52];
        public string[,] PageBVTitle = new string[52,50];
        public string[,] BVTitleTotal = new string[52,50];

        public string LeftBVLoad;
        public string[] RightBVLoad = new string[52];
        public string[,] PageBVLoad = new string[52,50];
        public string[,] BVLoadTotal = new string[52,50];
        public string LeftBVUnLoad;
        public string[] RightBVUnLoad = new string[52];
        public string[,] PageBVUnLoad = new string[52,50];
        public string[,] BVUnLoadTotal = new string[52,50];
        public string LeftBVSpotting;
        public string[] RightBVSpotting = new string[52];
        public string[,] PageBVSpotting = new string[52,50];
        public string[,] BVSpottingTotal = new string[52,50];//------------- NEW SPOTTING INFO ADDED;
        public string LeftBVTotal;
        public string[] RightBVTotal = new string[52];
        public string[,] PageBVTotal = new string[52,50];
        public string[,] BVTotalTotal = new string[52,50];

        public string LeftBVLoadVar;
        public string[] RightBVLoadVar = new string[52];
        public string[,] PageBVLoadVar = new string[52,50];
        public string[,] BVLoadVarTotal = new string[52,50];
        public string LeftBVUnLoadVar;
        public string[] RightBVUnLoadVar = new string[52];
        public string[,] PageBVUnLoadVar = new string[52,50];
        public string[,] BVUnLoadVarTotal = new string[52,50];
        public string LeftBVSpottingVar;
        public string[] RightBVSpottingVar = new string[52];
        public string[,] PageBVSpottingVar = new string[52,50];
        public string[,] BVSpottingVarTotal = new string[52,50];//------------- NEW SPOTTING INFO ADDED;
        public string LeftBVTotalVar;
        public string[] RightBVTotalVar = new string[52];
        public string[,] PageBVTotalVar = new string[52,50];
        public string[,] BVTotalVarTotal = new string[52,50];

        public string LeftBVTotalPayroll;
        public string[] RightBVTotalPayroll = new string[52];
        public string[,] PageBVTotalPayroll = new string[52,50];
        public string[,] BVTotalPayrollTotal = new string[52,50];
        public string LeftBVBudgetedPayroll;
        public string[] RightBVBudgetedPayroll = new string[52];
        public string[,] PageBVBudgetedPayroll = new string[52,50];
        public string[,] BVBudgetedPayrollTotal = new string[52,50];
        public string LeftBVVolumesVariance;
        public string[] RightBVVolumesVariance = new string[52];
        public string[,] PageBVVolumesVariance = new string[52,50];
        public string[,] BVVolumesVarianceTotal = new string[52,50];
        public string LeftBVPayrollVariance;
        public string[] RightBVPayrollVariance = new string[52];
        public string[,] PageBVPayrollVariance = new string[52,50];
        public string[,] BVPayrollVarianceTotal = new string[52,50];

        public string LeftBVUnloadVolumes;
        public string[] RightBVUnloadVolumes = new string[52];
        public string[,] PageBVUnloadVolumes = new string[52,50];
        public string[,] BVUnloadVolumesTotal = new string[52,50];
        public string LeftBVLoadVolumes;
        public string[] RightBVLoadVolumes = new string[52];
        public string[,] PageBVLoadVolumes = new string[52,50];
        public string[,] BVLoadVolumesTotal = new string[52,50];
        public string LeftBVSpottingVolumes;
        public string[] RightBVSpottingVolumes = new string[52];
        public string[,] PageBVSpottingVolumes = new string[52,50];
        public string[,] BVSpottingVolumesTotal = new string[52,50];//------------- NEW SPOTTING INFO ADDED;
        public string LeftBVTotalVolumes;
        public string[] RightBVTotalVolumes = new string[52];
        public string[,] PageBVTotalVolumes = new string[52,50];
        public string[,] BVTotalVolumesTotal = new string[52,50];

        public double varianceUnloadTotal;
        public string varianceLoadTotal;
        public string varianceSpottingTotal;
        public string varianceTotal;//------------- NEW SPOTTING INFO ADDED;
        public string loadUnloadTotal; // should we make this loadUnloadSpottingTotal?;

        public string BVLoadTotals;
        public string BVVarLoadTotals;
        public string BVUnloadTotals;
        public string BVVarUnloadTotals;
        public string BVSpottingTotals;
        public string BVVarSpottingTotals;//------------- NEW SPOTTING INFO ADDED;

        public string BVUnloadVolumesTotals;
        public string BVLoadVolumesTotals;
        public string BVSpottingVolumesTotals;//------------- NEW SPOTTING INFO ADDED;

        public string originalMiscLaborBudYTD;
        public string originalMiscLaborRepYTD;

        public string[] ActualLoadYTD = new string[50];
        public string[] ActualUnLoadYTD = new string[50];
        public string[] ActualSpottingYTD = new string[50];//------------- NEW SPOTTING INFO ADDED;

        public string[] PayrollIRTYTD = new string[50];
        public string[] PayrollTEMPYTD = new string[50];
        public string[] payRollIRTHrsYTD = new string[50];
        public string[] payRollTEMPHrsYTD = new string[50];
        public string[] OverTimeYTD = new string[50];
        public string[] EmployeesIRTYTD = new string[50];
        public string[] EmployeesTEMPYTD = new string[50];
        public string[] EmployeesUnitsYTD = new string[50];
        public string[] MiscLaborRepYTD = new string[50];
        public string[] MiscLaborBudYTD = new string[50];
        public string[] DiffMiscBudPayRollYTD = new string[50];
        public string[] DiffBudPayRollYTD = new string[50];
        public string originalDiffMiscBudPayRollYTD;
        public string originalDiffBudPayRollYTD;
        public string originalOverTimeYTD;
        public string originalPayRollTotalHrsYTD;
        public string originalPayRollTotalYTD;
        public string LeftHoliday;
        public string[] RightHoliday = new string[52];
        public string[,] PageHoliday = new string[52,50];
        public string[,] HolidayTotal = new string[52,50];

        public int vbSunday = 1;
        public int vbMonday = 2;
        public int vbTuesday = 3;
        public int vbWednesday = 4;
        public int vbThursday = 5;
        public int vbFriday = 6;
        public int vbSaturday = 7; 

        public string Left3TitleLine = "";
        public string Right3TitleLine = "";
        public string Right3TitleLine2 = "";
        public string LeftLoadData = "";
        public string LeftUnloadData = "";
        public string LeftSpottingData = "";
        public string LeftUnloadBudget = "";
        public string LeftLoadBudget = "";
        public string LeftSpottingBudget = ""; //------------- NEW SPOTTING INFO ADDED;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin, User");


            selMonth = cInt(Request["selMonth"]);
            selYear = Request["selYear"];
            selRegion = Request["selRegion"];
            facMonth = MonthAbbr(selMonth);
            

            LeftLoadVariance = "";
            facilityCount = 0;
            pageCount = 0;
            pageSize = 14;   //How many Facilities across the page;

            // First get the First day AND Last day for( the weekly CPU range;
            // First Friday of the Month AND last Thursday of the Month;

            sFirstDay = FirstFriday(cDate(selMonth + "/1/" + selYear)).ToString("M/d/yyyy");
            sLastDay  = LastFriday(cDate(selMonth + "/1/" + selYear)).ToString("M/d/yyyy");

            //Set Weekly Date for( TitleLine AND Week Total;
            int cnt = 0;
            while (cnt < UBound(arWeeklyDate) && cDate(sFirstDay) <= cDate(sLastDay)){
                    arWeeklyDate[cnt] = sFirstDay;
                    sFirstDay = cStr(cDate(sFirstDay).AddDays(7).ToString("M/d/yyyy"));
                    weekIndex = cnt;
                    cnt = cnt + 1;
            }
            weekCount = cnt;//How many Weeks in Report;


            //Retrieving Titles && Facility IDs;
            DataReader rsTitles = new DataReader(" SELECT SUBSTRING(AlphaCode,0,3), id FROM Facility WHERE RegionId = " + selRegion + " ORDER BY AlphaCode ");
            rsTitles.Open();

            int facCnt = 0;
            while (rsTitles.Read() ){
                    arTitles[facCnt] = rsTitles.Fields(0);
                    facilityId[facCnt] = rsTitles.Fields(1);

                facCnt = facCnt + 1;
            } //End Loop
            facilityCount = facCnt;


            pageCount = Ceiling(cStr(SafeDiv(cStr(SafeDbl(cStr(facilityCount))), cStr(pageSize))));   //Number of Pages per Week based on Number Facilities divided by Number Allowed per Page;


            DataReader rsRegion = new DataReader(" SELECT RegionDescription FROM irgregion WHERE id = " + selRegion);
            rsRegion.Open();

            while (rsRegion.Read() ){
                    regionName = rsRegion.Fields(0);
            } //End Loop


    if (facilityCount != 0){
        string strSQL = "";
        //Retrieving Budgeted Load AND Unload [and Spotting] Figures for( the Month;

                for(int i=0; i < facilityCount;i++){
                        if (Trim(facilityId[i]) != ""){

                                strSQL +=  " SELECT IsNull(loadTotal,0)*(IsNull((SELECT MonthlyPercentage FROM MonthlyBudgetPerc WHERE ReportingMonth = '" + facMonth + "' AND ReportingYear = '" + selYear + "'),0)*.01), ";
                                strSQL +=  " IsNull(unloadTotal,0)*(IsNull((SELECT MonthlyPercentage FROM MonthlyBudgetPerc WHERE ReportingMonth = '" + facMonth + "' AND ReportingYear = '" + selYear + "'),0)*.01), ";
								strSQL +=  " IsNull(spottingTotal,0)*(IsNull((SELECT MonthlyPercentage FROM MonthlyBudgetPerc WHERE ReportingMonth = '" + facMonth + "' AND ReportingYear = '" + selYear + "'),0)*.01), "                                ;
                                strSQL +=  " Fac='" + arTitles[i] + "' ";
                                strSQL +=  " FROM FacilityAnnualBudget ";
                                strSQL +=  " WHERE ReportingYear = '" + Request["selYear"]  + "' AND facilityId = " + facilityId[i] + "  ";

                                if (i < facilityCount && Trim(facilityId[i]) != "")
                                {
                                        strSQL +=  " UNION ";
                                }
                       }
                }

                strSQL = Left(strSQL,Len(strSQL)-7) + " ORDER BY Fac ";

                DataReader rsLoadBudget = new DataReader(strSQL);
                rsLoadBudget.Open();
                int Z = 0;

                while (rsLoadBudget.Read() ){
                        loadBudget[Z] = rsLoadBudget.Fields(0);
                        unloadBudget[Z] = rsLoadBudget.Fields(1);
                        spottingBudget[Z] = rsLoadBudget.Fields(2);

                        Z = Z + 1;
                } //End Loop



            //Retrieving Budgeted Load AND Unload[and now spotting] Figures for( YTD;
            if (selMonth != 1){

                strSQL = "";
                string monthSQL = "";
                for( int c=1; c < cInt(selMonth-1); c++){
                        if (Trim(monthSQL) != ""){
                                monthSQL = monthSQL + ", ";
                        }
                        monthSQL = monthSQL + " '" + MonthAbbr(c) + "' ";
                }

                strSQL +=  "SELECT IsNull((SELECT SUM(BudgetValue) FROM FacilityMonthlyBudgetEntry ";
                strSQL +=  "WHERE ReportingYear = '" + Request["selYear"] + "' ";
                strSQL +=  "AND ReportingMonth IN (" + monthSQL + ") AND WorkType = 'load'),0) As Loaded, ";
                strSQL +=  "IsNull((SELECT SUM(BudgetValue) FROM FacilityMonthlyBudgetEntry ";
                strSQL +=  "WHERE ReportingYear = '" + Request["selYear"] + "' ";
                strSQL +=  "AND ReportingMonth IN (" + monthSQL + ") AND WorkType = 'unload'),0) As UnLoaded, ";
                strSQL +=  "IsNull((SELECT SUM(BudgetValue) FROM FacilityMonthlyBudgetEntry ";
                strSQL +=  "WHERE ReportingYear = '" + Request["selYear"] + "' ";
                strSQL +=  "AND ReportingMonth IN (" + monthSQL + ") AND WorkType = 'spotting'),0) As Spotting ";


				
                DataReader rsLoadMonthlyBudget= new DataReader(strSQL);
                rsLoadMonthlyBudget.Open();
                i = 1;
                while (rsLoadMonthlyBudget.Read()){
                        loadMonthlyBudgetYTD = rsLoadMonthlyBudget.Fields(0);
                        unLoadMonthlyBudgetYTD = rsLoadMonthlyBudget.Fields(1);
                        spottingMonthlyBudgetYTD = rsLoadMonthlyBudget.Fields(2);

                        i = i + 1;
                } //End Loop
       }
   }
}

public string MonthAbbr(int selMonth){

        switch( selMonth){
            case 1 : 
                return "jan";
                break;
            case 2 :          
                return  "feb";
                break;
            case 3 :          
                return  "mar";
                break;
            case 4 :          
                return  "apr";
                break;
            case 5 :          
                return  "may";
                break;
            case 6 :          
                return  "jun";
                break;
            case 7 :          
                return  "jul";
                break;
            case 8 :          
                return  "aug";
                break;
            case 9 :          
                return  "sep";
                break;
            case 10 :         
                return  "oct";
                break;
            case 11 :         
                return  "nov";
                break;
            case 12 :         
                return  "dec";
                break;
       }
       return "";
}


//Returns Last Thurs. of the Month;
public DateTime LastFriday(DateTime mydate){

        DateTime tempLastFriday;
        bool blnLastWeekofMonth = false;
        bool bWasPerformed = false;

        tempLastFriday = mydate;

        if (Month(mydate.AddDays(7)) != Month(mydate)){

                while( Month(tempLastFriday) == Month(mydate)){
                       if (Weekday(tempLastFriday) == vbThursday){
                                return mydate;  //Date minus 1 month;
                       }
                        bWasPerformed = true;
                        tempLastFriday = tempLastFriday.AddDays(1);
                } //End Loop

                if (bWasPerformed == false){
                        tempLastFriday = cDate(Month(mydate.AddMonths(1)) + "/1/" + Year(mydate));  //Add a month- date is past last Thurs. of month;
                        if (cInt(Month(tempLastFriday)) == 1){
                                tempLastFriday = tempLastFriday.AddYears(1);
                        }
                        blnLastWeekofMonth = true;
               }
        }else{
            tempLastFriday = mydate;
       }

        while( Weekday(tempLastFriday) != vbThursday) {
                tempLastFriday = tempLastFriday.AddDays(1);
        } //End Loop

        if (blnLastWeekofMonth == true){
                while( Month(tempLastFriday.AddDays(7)) == Month(mydate.AddMonths(1))){
                        tempLastFriday = tempLastFriday.AddDays(7);
                } //End Loop
        }else{
                while( Month(tempLastFriday.AddDays(7)) == Month(mydate)){
                        tempLastFriday = tempLastFriday.AddDays(7);
                } //End Loop
       }

       return tempLastFriday;
         
}


//First Friday after the Last Thursday of the Previous Year;
public DateTime BeginningYTD(DateTime mydate){

        DateTime tempBeginningYTD;
        bool blnLastWeekofYear = false;
        bool bWasPerformed = false;

        tempBeginningYTD = mydate;

        if (cDate(mydate) > cDate("12/24/" + Year(mydate)) && cDate(mydate) <= cDate("12/31/" + Year(mydate))){
                while ( cInt(Year(tempBeginningYTD)) < cInt(Year(mydate))){
                        if (Weekday(tempBeginningYTD) == vbThursday){
                                tempBeginningYTD = cDate("12/24/" + Year(mydate.AddYears(-1)));     //Date minus 1 year;
                        }
                        bWasPerformed = true;
                        tempBeginningYTD = tempBeginningYTD.AddDays(1);
                } //End Loop

                if (bWasPerformed == false ){
                        tempBeginningYTD = mydate;   //////Do !minus 1 year (ex. a date is 12/29/03 && the last Thursday is 12/23/03);
                        blnLastWeekofYear = true;
                }
        }else{
                tempBeginningYTD = cDate("12/24/" + Year(mydate.AddYears(-1)));   //Date minus 1 year;
       }

        while ( Weekday(tempBeginningYTD) != vbThursday){
                tempBeginningYTD = tempBeginningYTD.AddDays(1);
        } //End Loop

        if (blnLastWeekofYear == true){
                while ( Year(tempBeginningYTD.AddDays(7)) != (Year(mydate)+1)){
                       tempBeginningYTD = tempBeginningYTD.AddDays(7);
                } //End Loop
                tempBeginningYTD = tempBeginningYTD.AddDays(-6);
        }else{
                while( Year(tempBeginningYTD.AddDays(7)) != Year(mydate)){
                        tempBeginningYTD = tempBeginningYTD.AddDays(7);
                } //End Loop
                tempBeginningYTD = tempBeginningYTD.AddDays(1);
       }

    return tempBeginningYTD;
}


public int NumberOfWeeks(DateTime mydate) {

        int i, WeekCount,NumberOfWeeks;
        DateTime BeginningWeek;

        i = cInt(Day(BeginningYTD(mydate)));

        BeginningWeek = BeginningYTD(mydate);

        while (! (cInt(BeginningWeek.DayOfWeek) == vbFriday)){
                i = i + 1;
                BeginningWeek = cDate(Month(BeginningWeek) + "/" + i + "/" + Year(BeginningWeek));
        }

        WeekCount = 0;

        while ( cDate(BeginningWeek) <= cDate(mydate)){

                BeginningWeek = BeginningWeek.AddDays(7);
                WeekCount = WeekCount + 1;
        }

       return WeekCount;

}


public DateTime FirstFriday(DateTime mydate) {
    if (LastFriday(cDate(mydate)) >= cDate(mydate)){
            return LastFriday(mydate.AddMonths(-1)).AddDays(1);
    }else{
            return LastFriday(mydate).AddDays(1);
    }
}


public int Ceiling(string n) {

        int iTmp;
        bool bErr;
        double tmp;
        double f;

        try{ 
          tmp = cDbl(n);
        }catch (Exception ex){
            throw new Exception("Ceiling Function Input must be convertible to a sub-type of double");
        }

        f = Floor(cStr(tmp));
        if (f == tmp){
                return cInt(n);
        }

        return cInt(f + 1);
}

public int Floor(string n) {

        int iTmp;
        bool bErr;
        double tmp;

        try{
          tmp = cDbl(n);
        }catch(Exception ex){
             throw new Exception("Floor Function : Input must be convertible to a sub-type of double");
        }

        iTmp = cInt(Math.Round(tmp,0));

        if (iTmp > tmp){iTmp = iTmp - 1;}

        return cInt(iTmp);
}

public string FNum(string num,int dec){

        if (Trim(num) != ""){
          if(cDbl(num) == 0){
                return  "0";
          }else{
                return  FormatNumber(num, dec);
          }
         }else{
                return  "0";
         }
}

public string FNeg(string num,int dec){

  string tmpFNeg = "";
 
  if(cDbl(num) == 0){
     tmpFNeg = "0";
  }else{
     tmpFNeg = FormatNumber(num, dec);
  }

  if (Left(tmpFNeg,1) == "-"){
        return  "(" + cStr(Replace(tmpFNeg,"-","")) + ")";
  }

    return tmpFNeg;

}

public string FCur(string num, int dec){

        if (Trim(num) != ""){

          if(cDbl(num) == 0){
                if (dec == 0){
                        return  "$0";
                }else{
                        return "$0.00";
                }
          }else{
                if (Left(cStr(num),1) == "-"){
                        //FCur = "$(" + cStr(FormatNumber(Replace(num,"-",""), dec)) + ")";
						return "$(" + cStr(Replace(FormatNumber(num,dec),"-","")) + ")";
                }else{
                        return "$" + cStr(FormatNumber(Trim(num), dec));
               }
         }
        }else{
          if (dec == 0){
                return "$0";
          }else{
                return "$0.00";
     }
        }
}

public string FCurSP(string num)
{
    string FCurSP = "";
    if (!(num == null))
    {
        if (isNumeric(cStr(num)))
        {
            if (cDbl(num) != 0)
            {
                FCurSP = "$ " + cStr(FormatNumber(num, 2));
            }
        }
    }

    return FCurSP;

}

public double Sum(string[,] ar,int col){

  double total = 0;
  for(int z=0 ; z < (UBound(ar,1)-1); z++){
        if (InStr(0,ar[col,z],"-",0) != 0){
            total = total - SafeDbl(ar[col,z]);
        }else{
            total = total + SafeDbl(ar[col,z]);
        }
  }

  return total;
 
}

public int Count(string[,] ar, int col){

  int total = 0;

  for(int z = 0; z < (UBound(ar,2)-1); z++){
      if (Trim(ar[z,col]) != ""){
          total = total + 1;
      }
  }

  return total;

}

public string FTime(string x) {

        if (Trim(x) != ""){
                return Left(x,Len(x)-3);
        }else{
                return "";
       }
}


public string getUnitsHourstrSQL(string sFirstDate,string  sLastDate,string  sTask,string  sType){

          string strSQL = "";

          for( int i=0; i < facilityCount; i++){
                 if (Trim(facilityId[i]) != "") {

                                if(sType == "UNITS"){
                                        strSQL += " SELECT IsNULL(SUM(d.Units), 0) AS TotalUnits, Fac='" + arTitles[i] + "' ";
                                        strSQL +=  " FROM Facility f INNER JOIN FacilityProductionDetail d ON f.Id = d.FacilityID ";
                                        strSQL +=  "      INNER JOIN Tasks ON d.TaskId = Tasks.Id  ";
                                        strSQL +=  "          WHERE ";
                                }else if(sType == "PAY"){
                                       strSQL += " SELECT IsNULL(SUM(PayAmount),0)AS TotalPay, Fac='" + arTitles[i] + "' ";
                                       strSQL +=  " FROM Employee e, Facility f INNER JOIN EmployeeTaskWorked ON f.Id = EmployeeTaskWorked.FacilityID ";
                                       strSQL +=  "      INNER JOIN Tasks ON EmployeeTaskWorked.TaskID = Tasks.Id   ";
                                       strSQL +=  "      LEFT OUTER JOIN EmployeeTaskWorkedPay ON EmployeeTaskWorked.Id = EmployeeTaskWorkedPay.EmployeeTaskWorkedId ";
                                       strSQL +=  "              WHERE e.id = EmployeeTaskWorked.EmployeeId AND ";
                                   }else{
                                       strSQL += " SELECT IsNULL(SUM(HoursPaid),0) AS TotalHours, Fac='" + arTitles[i] + "' ";
                                       strSQL +=  " FROM Employee e, Facility f INNER JOIN EmployeeTaskWorked ON f.Id = EmployeeTaskWorked.FacilityID ";
                                       strSQL +=  "      INNER JOIN Tasks ON EmployeeTaskWorked.TaskID = Tasks.Id   ";
                                       strSQL +=  "      LEFT OUTER JOIN EmployeeTaskWorkedPay ON EmployeeTaskWorked.Id = EmployeeTaskWorkedPay.EmployeeTaskWorkedId ";
                                       strSQL +=  "              WHERE e.id = EmployeeTaskWorked.EmployeeId AND ";
                                   }

                                if(sTask == "ALL"){
                                   strSQL +=  "(1=1) ";
                                }else if(sTask == "SH"){
                                   strSQL +=  "(Tasks.TaskCode In ('SH', 'SU') ) ";
                                }else if(sTask == "RB"){
                                    strSQL +=  "(Tasks.Rebillable = 1) ";
                                }else if(sTask == "MISC"){
                                    strSQL +=  "(Tasks.TaskCode In ('RB', 'R1', 'R2') )  ";
                                }else{
                                    strSQL +=  "(Tasks.TaskCode = '" + sTask + "') ";
                                }
                                if(sType == "IRT"){
                                    strSQL +=  "AND (HireDate <= WorkDate AND HireDate Is NOT NULL)  ";
                                }else if(sType == "TEMP"){
                                     strSQL +=  "AND  (HireDate Is NULL OR HireDate > WorkDate)  ";
                                }

                                strSQL +=  "      AND f.ID = " + facilityId[i];
                                strSQL +=  "      AND WorkDate >= '" + cStr(sFirstDate) + "' AND WorkDate < '" + cStr(sLastDate) + "' ";

                                if (i < facilityCount && Trim(facilityId[i]) != ""){
                                   strSQL +=  " UNION ";
                                }
               }


          }

          strSQL = Left(strSQL, Len(strSQL) - 7) + " ORDER BY Fac ";

         return strSQL;

}


public string getUnitsHoursYTDSQL(string sFirstDate, string  sLastDate,string  sTask, string  sType){

          string strSQL = "";

            if(sType == "UNITS"){
                    strSQL = " SELECT IsNULL(SUM(d.Units), 0) AS TotalUnits  ";
                    strSQL +=  " FROM Facility f INNER JOIN FacilityProductionDetail d ON f.Id = d.FacilityID ";
                    strSQL +=  "      INNER JOIN Tasks ON d.TaskId = Tasks.Id  ";
                    strSQL +=  "          WHERE ";
            }else{
                if(sType == "PAY"){
                    strSQL = " SELECT IsNULL(SUM(PayAmount),0)AS TotalPay  ";
                    strSQL +=  " FROM Employee e, Facility f INNER JOIN EmployeeTaskWorked ON f.Id = EmployeeTaskWorked.FacilityID ";
                    strSQL +=  "      INNER JOIN Tasks ON EmployeeTaskWorked.TaskID = Tasks.Id   ";
                    strSQL +=  "      LEFT OUTER JOIN EmployeeTaskWorkedPay ON EmployeeTaskWorked.Id = EmployeeTaskWorkedPay.EmployeeTaskWorkedId ";
                    strSQL +=  "              WHERE e.id = EmployeeTaskWorked.EmployeeId AND ";
                }else{
                    strSQL = " SELECT IsNULL(SUM(HoursPaid),0) AS TotalHours  ";
                    strSQL +=  " FROM Employee e, Facility f INNER JOIN EmployeeTaskWorked ON f.Id = EmployeeTaskWorked.FacilityID ";
                    strSQL +=  "      INNER JOIN Tasks ON EmployeeTaskWorked.TaskID = Tasks.Id   ";
                    strSQL +=  "      LEFT OUTER JOIN EmployeeTaskWorkedPay ON EmployeeTaskWorked.Id = EmployeeTaskWorkedPay.EmployeeTaskWorkedId ";
                    strSQL +=  "              WHERE e.id = EmployeeTaskWorked.EmployeeId AND ";
                }
            }

            if(sTask == "ALL"){
                strSQL = strSQL + " (1=1) ";
            }else{
                if(sTask == "SH"){
                    strSQL = strSQL + " (Tasks.TaskCode In ('SH', 'SU') ) ";
                }else{
                    if(sTask == "RB"){
                        strSQL = strSQL + " (Tasks.Rebillable = 1) ";
                    }else{
                        if(sTask == "MISC"){
//                                             strSQL == strSQL + " (Tasks.TaskCode In ('TR', 'CL', 'RB', 'UA') )  ";
                            strSQL = strSQL + " (Tasks.TaskCode In ('RB', 'R1', 'R2') )  ";
                        }else{
                            strSQL = strSQL + " (Tasks.TaskCode = '" + sTask + "') ";
                        }
                     }
                }
            }
            if(sType == "IRT"){
                strSQL = strSQL + " AND (HireDate <= WorkDate AND HireDate Is NOT NULL)  ";
            }else{
                if(sType == "TEMP"){
                    strSQL = strSQL + " AND  (HireDate Is NULL OR HireDate > WorkDate)  ";
                }
            }

            strSQL = strSQL + "     AND WorkDate >= '" + cStr(BeginningYTD(cDate(sFirstDate))) + "' AND WorkDate < '" + cStr(sLastDate) + "' ";
            strSQL = strSQL + "     AND f.RegionId = "  + selRegion;

         return strSQL;

         //Response.Write(strSQL;
         //Response.End;

}


public string getMiscLaborBudYTDSQL(string sBeginDate,string  sLastDate,string sTask,string  sType,string  sMonth){

          string strSQL = "";
          string strMonth, strLastDate, strFirstDate;

          strLastDate = cStr(sBeginDate);
          strFirstDate = cStr(BeginningYTD(cDate(sBeginDate)));

          for( int i=0; i <  facilityCount; i ++){

                 if( Trim(facilityId[i]) != ""){

                                if(sType == "UNITS"){
                                        strSQL = " SELECT IsNULL(SUM(d.Units), 0) * (SELECT MiscellaneousCPU FROM FacilityAnnualBudget ";
                                        strSQL = strSQL + " WHERE ReportingYear = '" + Year(cDate(strFirstDate)) + "' AND facilityId = " + facilityId[i] + " ) ";
                                        strSQL = strSQL + " AS TotalVolume, Fac='" + arTitles[i] + "' ";
                                        strSQL = strSQL + "  FROM Facility f INNER JOIN FacilityProductionDetail d ON f.Id = d.FacilityID ";
                                        strSQL = strSQL + "       INNER JOIN Tasks ON d.TaskId = Tasks.Id  ";
                                        strSQL = strSQL + "           WHERE ";
                               }

                                if(sTask == "ALL"){
                                   strSQL = strSQL + " (1=1) ";
                                }else{
                                    if(sTask == "SH"){
                                       strSQL = strSQL + " (Tasks.TaskCode In ('SH', 'SU') ) ";
                                    }else{
                                        if(sTask == "RB"){
                                            strSQL = strSQL + " (Tasks.Rebillable = 1) ";
                                        }else{
                                            if(sTask == "MISC"){
                                               // strSQL = strSQL + " (Tasks.TaskCode In ('TR', 'CL', 'RB', 'UA') )  ";
                                               strSQL = strSQL + " (Tasks.TaskCode In ('RB', 'R1', 'R2') )  ";
                                            }else{
                                                strSQL = strSQL + " (Tasks.TaskCode = '" + sTask + "') ";
                                            }
                                        }
                                    }
                                }

                                if(sType == "IRT"){
                                   strSQL = strSQL + " AND (HireDate <= WorkDate AND HireDate Is NOT NULL)  ";
                                }else{
                                    if(sType == "TEMP"){
                                       strSQL = strSQL + " AND  (HireDate Is NULL OR HireDate > WorkDate)  ";
                                    }
                                }

                                strSQL = strSQL + "       AND WorkDate >= '" + cStr(strFirstDate) + "' AND WorkDate < '" + cStr(strLastDate) + "' ";
                                strSQL = strSQL + "       AND f.ID = " + facilityId[i];

                                if( i < facilityCount && Trim(facilityId[i]) != ""){
                                        strSQL = strSQL + "  UNION ";
                                }
               }
         }

         strSQL = Left(strSQL,Len(strSQL)-7) + " ORDER BY Fac ";
         return strSQL;

         //Response.Write((strSQL);
         //Response.End;

}


public string getSQL(string sFirstDate,string  sLastDate,string  sTask,string  sType){

          string strSQL = "";
          for(int i=0 ; i< facilityCount ;i++){
                 if(Trim(facilityId[i]) != ""){

                        if(sType == "PAYROLL"){
                           strSQL = strSQL + " SELECT IsNull(SUM(PayAmount),0) AS TotalPay, Fac='" + arTitles[i] + "' ";
                           strSQL = strSQL + "  FROM Employee e, Facility f INNER JOIN EmployeeTaskWorked ON f.Id = EmployeeTaskWorked.FacilityID ";
                           strSQL = strSQL + "       INNER JOIN Tasks ON EmployeeTaskWorked.TaskID = Tasks.Id   ";
                           strSQL = strSQL + "       LEFT OUTER JOIN EmployeeTaskWorkedPay ON EmployeeTaskWorked.Id = EmployeeTaskWorkedPay.EmployeeTaskWorkedId ";
                           strSQL = strSQL + "       WHERE e.id = EmployeeTaskWorked.EmployeeId";
                       }

                        if(sTask == "IRT"){
                           strSQL = strSQL + " AND (HireDate <= WorkDate AND HireDate Is NOT NULL)  ";
                        }else{
                            if(sTask == "TEMP"){
                               strSQL = strSQL + " AND  (HireDate Is NULL OR HireDate > WorkDate)  ";
                           }
                        }

                        strSQL = strSQL + "       AND f.ID = " + facilityId[i];
                        strSQL = strSQL + "       AND WorkDate >= '" + cStr(sFirstDate) + "' AND WorkDate < '" + cStr(sLastDate) + "' ";

                        if( i < facilityCount && Trim(facilityId[i]) != ""){
                                 strSQL = strSQL + " UNION ";
                       }
               }
         }

         strSQL = Left(strSQL,Len(strSQL)-7) + " ORDER BY Fac ";
         return strSQL;

         //Response.Write(strSQL;
         //Response.End;

}


public string getHolidaySQL(string sFirstDate,string  sLastDate){

        string strSQL = "";
        for(int i=0; i < facilityCount; i++){
                 if(Trim(facilityId[i]) != ""){

                        strSQL =  " SELECT Fac='" + arTitles[i] + "', IsNull(SUM(PayAmount),0) AS 'TotalAmount' ";
                        strSQL = strSQL + "         FROM EmployeeTaskWorkedPay INNER JOIN EmployeeTaskWorked ON EmployeeTaskWorkedId = EmployeeTaskWorked.Id ";
                        strSQL = strSQL + "                         INNER JOIN Employee ON EmployeeTaskWorked.EmployeeId = Employee.Id  ";
                        strSQL = strSQL + "         WHERE (HireDate <= WorkDate AND HireDate Is NOT NULL) AND (OtherTaskID = 1 AND TaskID=0) AND (EmployeeTaskWorked.PayrollStatus <> 'OPEN') ";
                        strSQL = strSQL + "                         AND (EmployeeTaskWorked.FacilityID = " + facilityId[i] + " ) ";
                        strSQL = strSQL + "                 AND WorkDate >= '" + cStr(sFirstDate) + "' AND WorkDate < '" + cStr(sLastDate) + "' ";

                        if(i < facilityCount && Trim(facilityId[i]) != ""){
                                 strSQL = strSQL + " UNION ";
                        }
               }
        }

        strSQL = Left(strSQL,Len(strSQL)-7) + " ORDER BY Fac ";
        return strSQL;

        //Response.Write(strSQL;
        //Response.End;

}


public string  getYTDSQL(string sFirstDate,string  sLastDate,string  sTask,string  sType){

          string strSQL = "";

                        if(sType == "PAYROLL"){
                           strSQL = strSQL + "SELECT SUM(PayAmount) AS TotalPay ";
                           strSQL = strSQL + "  FROM Employee e, Facility f INNER JOIN EmployeeTaskWorked ON f.Id = EmployeeTaskWorked.FacilityID ";
                           strSQL = strSQL + "       INNER JOIN Tasks ON EmployeeTaskWorked.TaskID = Tasks.Id   ";
                           strSQL = strSQL + "       LEFT OUTER JOIN EmployeeTaskWorkedPay ON EmployeeTaskWorked.Id = EmployeeTaskWorkedPay.EmployeeTaskWorkedId ";
                           strSQL = strSQL + "       WHERE e.id = EmployeeTaskWorked.EmployeeId";
                        }

                        if(sTask == "IRT"){
                           strSQL = strSQL + " AND (HireDate <= WorkDate AND HireDate Is NOT NULL)  ";
                        }else{
                           if(sTask == "TEMP"){
                               strSQL = strSQL + " AND  (HireDate Is NULL OR HireDate > WorkDate)  ";
                           }
                        }

                        strSQL = strSQL + "       AND WorkDate >= '" + cStr(BeginningYTD(cDate(sFirstDate))) + "' AND WorkDate < '" + cStr(sLastDate) + "' ";
                        strSQL = strSQL + "     AND f.RegionId = "  + selRegion;

         return strSQL;

         //Response.Write(strSQL;
         //Response.End;

}


public string  getEmployeeCountSQL(string sFirstDate,string  sLastDate,string  sTask,string  sType){

          string strSQL = "";
          string sFacilities, sEndWeek;
          strSQL = "";
          sEndWeek = cStr(sLastDate);

                        if(sType == "TOTAL"){
                                strSQL = " SELECT (SELECT COUNT(DISTINCT (Employee.Id)) AS TotalEmployee ";
                                strSQL = strSQL + "  FROM Employee, EmployeeTaskWorked WHERE Employee.FacilityId = facility.id AND";

                                if(sTask == "IRT"){
                                   strSQL = strSQL + " (HireDate <= WorkDate AND HireDate Is NOT NULL) AND  ";
                                }else{
                                    if(sTask == "TEMP"){
                                       strSQL = strSQL + " (HireDate Is NULL OR HireDate > WorkDate) AND ";
                                    }
                                }

                                strSQL = strSQL + "       WorkDate >= '" + cStr(sFirstDate) + "' AND WorkDate < '" + cStr(sLastDate) + "' ";
                                strSQL = strSQL + "     AND EmployeeTaskWorked.EmployeeId = employee.id )  ";
                                strSQL = strSQL + "     As TotalEmployee, facility.alphacode FROM facility   ";
                                strSQL = strSQL + "     WHERE RegionId = "  + selRegion;
                                strSQL = strSQL + " GROUP BY facility.alphacode, facility.id ORDER BY facility.alphacode, facility.id ";

                        }else{
                            if(sType == "YTD"){

                                sFirstDate = cStr(BeginningYTD(cDate(sFirstDate)));
                                for(int j=1; j < cInt(NumberOfWeeks(cDate(sEndWeek))); j++){
                                        strSQL = strSQL + "SELECT COUNT(DISTINCT (Employee.Id)) AS TotalEmployee, WeekOrder='" + j + "' ";
                                        strSQL = strSQL + "         FROM Employee, EmployeeTaskWorked INNER JOIN Facility ON EmployeeTaskWorked.FacilityID = Facility.Id WHERE  ";

                                        if(sTask == "IRT"){
                                           strSQL = strSQL + " (HireDate <= WorkDate AND HireDate Is NOT NULL) AND  ";
                                        }else{
                                            if(sTask == "TEMP"){
                                               strSQL = strSQL + " (HireDate Is NULL OR HireDate > WorkDate) AND ";
                                            }
                                        }
                                        strSQL = strSQL + "       WorkDate >= '" + cStr(sFirstDate) + "' AND WorkDate < '" + cStr(cDate(sFirstDate).AddDays(7)) + "' ";
                                        strSQL = strSQL + "     AND EmployeeTaskWorked.EmployeeId = employee.id   ";
                                        strSQL = strSQL + "     AND RegionId = "  + selRegion;
                                        strSQL = strSQL + " UNION ";

                                        sFirstDate = cStr(cDate(sFirstDate).AddDays(7));
                                }
                                strSQL = Left(strSQL,Len(strSQL)-7) + " ORDER BY WeekOrder";
                                sFirstDate = cStr(cDate(sFirstDate).AddDays(-7));

                            }
                        }

         return strSQL;

  //Response.Write(strSQL + "  -  <br>" + NumberOfWeeks(sEndWeek) + "    -  " + sEndWeek;
  //Response.End;

}


public string  getEmployeeSQL(string sFirstDate,string  sLastDate,string  sTask,string  sType){

          string strSQL = "";
          for(int i=0; i < facilityCount; i ++){
                 if(Trim(facilityId[i]) != ""){
                        if(sType =="UNITS"){
                        strSQL = " SELECT Units=( SELECT SUM(Units) FROM FacilityProductionDetail ";
                        strSQL = strSQL + "  WHERE facilityID = " + facilityId[i];
                                strSQL = strSQL + "       AND WorkDate >= '" + cStr(sFirstDate) + "' AND WorkDate < '" + cStr(sLastDate) + "' ) / ";

                        strSQL = strSQL + "  (SELECT Case COUNT(DISTINCT(EmployeeId)) WHEN 0 Then 1 Else COUNT(DISTINCT(EmployeeId)) End FROM EmployeeTaskWorked ";
                        strSQL = strSQL + "  WHERE facilityID = " + facilityId[i];
                                strSQL = strSQL + "       AND WorkDate >= '" + cStr(sFirstDate) + "' AND WorkDate < '" + cStr(sLastDate) + "') , ";
                                strSQL = strSQL + "           Fac='" + arTitles[i] + "' ";
                       }

                        if(i < facilityCount && Trim(facilityId[i]) != ""){
                                 strSQL = strSQL + " UNION ";
                       }

               }
         }

         strSQL = Left(strSQL,Len(strSQL)-7) + " ORDER BY Fac ";
         return strSQL;

  //Response.Write(strSQL;
  //Response.End;

}


public string  getEmployeeYTDSQL(string sFirstDate,string  sLastDate,string  sTask,string  sType){

          string strSQL = "";
          string sFacilities = "";

          for(int i=0; i < facilityCount; i++){
                 if(Trim(facilityId[i]) != ""){
                        sFacilities = sFacilities + " " + facilityId[i] + ", ";
                }
          }
          sFacilities = Left(sFacilities,Len(sFacilities)-2) + " ";

                        if(sType == "TOTAL"){
                                strSQL =  " SELECT COUNT(DISTINCT (et.EmployeeId)) AS TotalEmployee  ";
                                strSQL = strSQL + "  FROM EmployeeTaskWorked et, Employee e ";
                                strSQL = strSQL + "       WHERE et.EmployeeId = e.id AND ";

                                if(sTask == "IRT"){
                                   strSQL = strSQL + " (e.HireDate <= et.WorkDate AND e.HireDate Is NOT NULL) AND  ";
                                }else{
                                    if(sTask == "TEMP"){
                                       strSQL = strSQL + " (e.HireDate Is NULL OR e.HireDate > et.WorkDate) AND ";
                                    }
                                }

                                strSQL = strSQL + "       AND et.WorkDate >= '" + cStr(BeginningYTD(cDate(sFirstDate))) + "' AND et.WorkDate < '" + cStr(sLastDate) + "' ";
                                strSQL = strSQL + "       AND et.FacilityId IN (" + sFacilities + ") ";

                        }else{
                            if(sType =="UNITS"){
                            strSQL = strSQL + "SELECT Units=( SELECT SUM(Units) FROM FacilityProductionDetail ";
                            strSQL = strSQL + "  WHERE WorkDate >= '" + cStr(BeginningYTD(cDate(sFirstDate))) + "' AND WorkDate < '" + cStr(sLastDate) + "' ";
                            strSQL = strSQL + "       AND FacilityId IN (" + sFacilities + ") ) / ";

                            strSQL = strSQL + "  (SELECT Case COUNT(DISTINCT(EmployeeId)) WHEN 0 Then 1 ELSE COUNT(DISTINCT(EmployeeId)) End FROM EmployeeTaskWorked ";
                            strSQL = strSQL + "  WHERE WorkDate >= '" + cStr(BeginningYTD(cDate(sFirstDate))) + "' AND WorkDate < '" + cStr(sLastDate) + "'  AND FacilityId IN (" + sFacilities + ") ) ";

                           }
                        }

         return strSQL;

  //Response.Write(strSQL;
  //Response.End;

}


public string  getOvertimeSQL(string sFirstDate,string  sLastDate){

          string strSQL = "";
          for(int i=0; i < facilityCount; i++){
                 if(Trim(facilityId[i]) != ""){

                        strSQL = strSQL + " SELECT SUM(PayAmount) AS TotalPay, Fac='" + arTitles[i] + "' ";
                        strSQL = strSQL + "       FROM EmployeeTaskWorkedPay INNER JOIN EmployeeTaskWorked ON EmployeeTaskWorkedPay.EmployeeTaskWorkedId = EmployeeTaskWorked.Id ";
                        strSQL = strSQL + "       WHERE PayMultiplier <> 1 ";
                        strSQL = strSQL + "       AND facilityId = " + facilityId[i];
                        strSQL = strSQL + "       AND WorkDate >= '" + cStr(sFirstDate) + "' AND WorkDate < '" + cStr(sLastDate) + "' ";

                        if(i < facilityCount && Trim(facilityId[i]) != ""){
                                 strSQL = strSQL + " UNION ";
                       }
               }
         }

         strSQL = Left(strSQL,Len(strSQL)-7) + " ORDER BY Fac ";
         return strSQL;

  //Response.Write(strSQL;
  //Response.End;

}


public string  getOvertimeYTDSQL(string sFirstDate,string  sLastDate){

          string strSQL="";
          string sFacilities = "";

          for(int i=0; i < facilityCount; i++){
                 if(Trim(facilityId[i]) != ""){
                        sFacilities = sFacilities + " " + facilityId[i] + ", ";
                }
          }
          sFacilities = Left(sFacilities,Len(sFacilities)-2) + " ";

                        strSQL = "SELECT SUM(PayAmount) AS TotalPay  ";
                        strSQL = strSQL + "       FROM EmployeeTaskWorkedPay INNER JOIN EmployeeTaskWorked ON EmployeeTaskWorkedPay.EmployeeTaskWorkedId = EmployeeTaskWorked.Id ";
                        strSQL = strSQL + "       WHERE PayMultiplier <> 1 ";
                        strSQL = strSQL + "       AND WorkDate >= '" + cStr(BeginningYTD(cDate(sFirstDate))) + "' AND WorkDate < '" + cStr(sLastDate) + "' ";
                        strSQL = strSQL + "       AND EmployeeTaskWorked.facilityId IN (" + sFacilities + ") ";

        return strSQL;

  //Response.Write(strSQL;
  //Response.End;

}


public string  getBudgetCPUSQL(){

          string strSQL = "";
          for(int i=0; i < facilityCount; i++){
                 if(Trim(facilityId[i]) != ""){

                        strSQL += " SELECT  (SELECT     SUM(BudgetedCPU) AS BudgetedCPU FROM  FacilityAnnualBudgetTask Where facilityID = FacilityAnnualBudget.FacilityID And ReportingYear = FacilityAnnualBudget.ReportingYear ) as BudgetedCPU, MiscellaneousCPU, Fac='" + arTitles[i] + "' ";
                        strSQL = strSQL + "  FROM FacilityAnnualBudget ";
                        strSQL = strSQL + "       WHERE facilityId = " + facilityId[i] + " AND ReportingYear = '" + selYear + "' ";

                        if(i < facilityCount && Trim(facilityId[i]) != ""){
                                 strSQL = strSQL + " UNION ";
                       }
               }
         }

         strSQL = Left(strSQL,Len(strSQL)-7) + " ORDER BY Fac ";
         return strSQL;

}


public void CreateTitleRow(){

  int i = 0;
  int pageIndex = 0;
  int intEndofLine = 0;

   Page2TitleLine[pageIndex] = "";
   string stringBuffer = "";
   Left3TitleLine = "";

   //Set the static left lines;
   Left3TitleLine =    "<tr class=xl2430747 height=11 style='mso-height-source:userset;height:8.25pt'>";
   Left3TitleLine =    Left3TitleLine + "  <td height=11 class=xl2430747 width=276 style='height:8.25pt;width:207pt'></td>";
   Left3TitleLine =    Left3TitleLine + "  <td class=xl2430747 width=0></td>";

   //Set the Regional Facility titles;
   while (UBound(arTitles) > i){
         if (arTitles[i] != null && Trim(arTitles[i]) != ""){
                 stringBuffer =    stringBuffer + "<td class=xl2430747 width=60 style='width:45pt'>w</td>";
                 Page1TitleLine[pageIndex] = Page1TitleLine[pageIndex] + stringBuffer;
                 stringBuffer = "";
                 if((i + 1) % pageSize == 0){
                        pageIndex = pageIndex + 1;
                }
         }else{
         //Fill out the remaining columns to keep format;
                if(intEndofLine == 0){
                        intEndofLine = 1;
                }
        }
         if( intEndofLine == 1){
           while ( i % pageSize != 0){
                        Page1TitleLine[pageIndex] = Page1TitleLine[pageIndex] + "<td class=xl2430747 width=60 style='width:45pt'></td>";
                        intEndofLine = 2;
                        i = i + 1;
           }
        }
         i = i + 1;
   }

   i = 0;
   pageIndex = 0;
   intEndofLine = 0;

   //Set the static right columns;
   Right3TitleLine =    "<td class=xl2430747 width=10 style='width:8pt'></td>";
   Right3TitleLine =    Right3TitleLine +  "<td class=xl2430747 width=66 style='width:50pt'></td>";
   Right3TitleLine =    Right3TitleLine +  "<td class=xl2430747 width=58 style='width:44pt'></td>";
   Right3TitleLine =    Right3TitleLine +  "<td class=xl2430747 width=9 style='width:7pt'></td>";
   Right3TitleLine =    Right3TitleLine +  "<td class=xl2430747 width=73 style='width:55pt'></td>";
   Right3TitleLine =    Right3TitleLine +  "<td class=xl2430747 width=88 style='width:66pt'></td> </tr>";

   //Set the Weekly Dates;
   for( int j=0; j <= weekIndex; j ++){
           Left3TitleLine2[j] =    "<tr class=xl2630747 height=15 style='height:8pt'>";
           Left3TitleLine2[j] =    Left3TitleLine2[j] + "  <td height=15 class=xl2530747 style='height:8pt'><b>" + arWeeklyDate[j] + "</b></td>";
           Left3TitleLine2[j] =    Left3TitleLine2[j] + "  <td class=xl2430747 width=0></td>";
   }

   stringBuffer = "";

   while ( UBound(arTitles) > i){
         if(arTitles[i] != null && Trim(arTitles[i]) != ""){
         stringBuffer =   stringBuffer + "<td class=xl2630747><b>" + arTitles[i] + "</b></td>";
                 Page2TitleLine[pageIndex] = Page2TitleLine[pageIndex] + stringBuffer;
             stringBuffer = "";
                 if((i+1) % pageSize == 0){
                        pageIndex = pageIndex + 1;
                }
         }else{
         //Fill out the remaining columns to keep format;
                if(intEndofLine == 0){
                        intEndofLine = 1;
               }
        }
        if(intEndofLine == 1){
           while ( i % pageSize != 0){
                        Page2TitleLine[pageIndex] = Page2TitleLine[pageIndex] + "<td class=xl2430747 width=60 style='width:45pt'></td>";
                        intEndofLine = 2;
                        i = i + 1;
           }
        }
         i = i + 1;
   }
   i = 0;

   Right3TitleLine2 =    "<td class=xl2630747></td>";
   Right3TitleLine2 =    Right3TitleLine2 + "<td class=xl2630747><b>Totals</b></td>";
   Right3TitleLine2 =    Right3TitleLine2 + "<td class=xl2630747></td>";
   Right3TitleLine2 =    Right3TitleLine2 + "<td class=xl2630747></td>";
   Right3TitleLine2 =    Right3TitleLine2 + "<td class=xl2630747><b>YTD</b></td>";
   Right3TitleLine2 =    Right3TitleLine2 + "<td class=xl2630747></td> </tr>";

}


public void CreateLoadData(){

         LeftLoadData = "<tr height=15 style='height:8pt'>";
         LeftLoadData = LeftLoadData + "<td height=15 class=xl2730747 style='height:8pt'><b>Load</b></td>";
         LeftLoadData = LeftLoadData + "<td class=xl2830747></td>";


         // Get YTD Units for( LOAD;
         j = 0;
         if(selMonth != 1){
                DataReader rsLoadUnitsYTD = new DataReader(getUnitsHoursYTDSQL(arWeeklyDate[j], arWeeklyDate[j],"LO", "UNITS"));
                rsLoadUnitsYTD.Open();
                while (rsLoadUnitsYTD.Read()){
                        ActualLoadYTD[j] = rsLoadUnitsYTD.Fields(0);

                }

        }

         for( j=0; j <= weekIndex; j++){

                // Get Total Units for( LOAD;
                 DataReader rsLoadUnits = new DataReader(getUnitsHourstrSQL(arWeeklyDate[j], cStr(cDate(arWeeklyDate[j]).AddDays(7)),"LO", "UNITS"));

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int lastYTD = 0;
                 i = 0;

                 rsLoadUnits.Open();
                 while (rsLoadUnits.Read()){
                                stringBuffer = stringBuffer + "<td class=xl2830747 align=right>" + FNum(rsLoadUnits.Fields(0), 0) + "</td>";
                                PageLoadData[j,pageIndex] = PageLoadData[j,pageIndex] + stringBuffer;
                                actualLoad[j,i] = FNum(rsLoadUnits.Fields(0), 0);
                                stringBuffer = "";
                                if ( ((i+1) % pageSize) == 0 ){
                                        pageIndex = pageIndex + 1;
                               }
                                i = i + 1;

                 }

                 if (j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                }

                 ActualLoadYTD[j] = cStr(SafeDbl(ActualLoadYTD[lastYTD]) + SafeDbl(FNum(cStr(Sum(actualLoad,j)),0)));

                //Fill out remaining columns;
                 while ( i % pageSize != 0){
                        PageLoadData[j,pageIndex] = PageLoadData[j,pageIndex] + "<td class=xl2830747 >&nbsp;</td>";
                        //intEndofLine = 2;
                        i = i + 1;
                 }

                 RightLoadData[j] = "<td class=xl2830747></td>";
                 RightLoadData[j] = RightLoadData[j] + "<td class=xl2930747 align='right'>" + FNum(cStr(Sum(actualLoad,j)),0)  + "</td>";
                 RightLoadData[j] = RightLoadData[j] + "<td class=xl2830747></td>";
                 RightLoadData[j] = RightLoadData[j] + "<td class=xl2830747></td>";
                 RightLoadData[j] = RightLoadData[j] + "<td class=xl3030747 align='right'>" + FNum(ActualLoadYTD[j],0) + "</td>";
                 RightLoadData[j] = RightLoadData[j] + "<td class=xl2830747></td>";
                 RightLoadData[j] = RightLoadData[j] + "</tr>";

                 //PageLoadData[j,pageIndex] = PageLoadData[j,pageIndex] + "<tr><td colspan=//15//>&nbsp;</td></tr>";

         }



}


public void CreateLoadBudgeted(){

         LeftLoadBudget = "<tr height=15 style='height:8pt'>";
         LeftLoadBudget = LeftLoadBudget + "<td height=15 class=xl2830747 style='height:8pt'>Budgeted</td>";
         LeftLoadBudget = LeftLoadBudget + "<td class=xl3130747>&nbsp;</td>";

         // Get YTD Units for( LOAD;
         j = 0;
         if(selMonth != 1){
                budgetedLoadYTD[j] = loadMonthlyBudgetYTD;
         }

         // Calculate Total Units for( Budgeted;
         for( j=0; j <= weekIndex; j++){

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int lastYTD = 0;
                 int i = 0;
                 for(i=0; i < (UBound(actualLoad,1)-1); i ++){

                                if(loadBudget[i] != null && Trim(loadBudget[i]) != ""){
                                        stringBuffer = stringBuffer + "<td class=xl3130747 align=right>" + FNum(cStr(SafeDiv(cStr(SafeDbl(loadBudget[i])), cStr(SafeDbl(cStr(weekCount))))), 0) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl2830747 align=right></td>";
                               }
                                PageLoadBudget[j,pageIndex] = PageLoadBudget[j,pageIndex] + stringBuffer;
                                budgetedLoad[j,i] = FNum(cStr(SafeDiv(cStr(SafeDbl(loadBudget[i])), cStr(SafeDbl(cStr(weekCount))))), 0);
                                stringBuffer = "";
                                if ( ((i+1) % pageSize) == 0 ){
                                        pageIndex = pageIndex + 1;
                               }
                 }

                 if(j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                }
                 budgetedLoadYTD[j] = cStr(SafeDbl(budgetedLoadYTD[lastYTD]) + SafeDbl(FNum(cStr(Sum(budgetedLoad, j)), 0)));

                //Fill out remaining columns;
                 leftOverColumns = ((i) % pageSize);
                 for(int  y=0 ; y <  (leftOverColumns-2); y ++){
                        PageLoadBudget[j,pageIndex] = PageLoadBudget[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                 }

                 RightLoadBudget[j] = "<td class=xl2830747></td>";
                 RightLoadBudget[j] = RightLoadBudget[j] + "<td class=xl3230747 align='right'>" + FNum(cStr(Sum(budgetedLoad, j)), 0)  + "</td>";
                 RightLoadBudget[j] = RightLoadBudget[j] + "<td class=xl2830747></td>";
                 RightLoadBudget[j] = RightLoadBudget[j] + "<td class=xl2830747></td>";
                 RightLoadBudget[j] = RightLoadBudget[j] + "<td class=xl3330747 align='right'>" + FNum(budgetedLoadYTD[j],0) + "</td>";
                 RightLoadBudget[j] = RightLoadBudget[j] + "<td class=xl2830747></td>";
                 RightLoadBudget[j] = RightLoadBudget[j] + "</tr>";

         }

}


public void CreateLoadVariance(){

         LeftLoadVariance = "<tr height=15 style='height:8pt'>";
         LeftLoadVariance = LeftLoadVariance + "<td height=15 class=xl3430747 style='height:8pt'>Variance</td>";
         LeftLoadVariance = LeftLoadVariance + "<td class=xl3530747></td>";

         for(int j=0; j <= weekIndex; j++){

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;

                 for(i=0; i <(UBound(actualLoad,1)-1); i++){

                                if( (Trim(actualLoad[j,i]) != "" || Trim(budgetedLoad[j,i]) != "") && i < facilityCount){
                                        stringBuffer = stringBuffer + "<td class=xl3530747 align=right>" + FNeg(cStr(SafeDbl(actualLoad[j,i]) - SafeDbl(budgetedLoad[j,i])), 0) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl3530747 align=right></td>";
                               }
                                PageLoadVariance[j,pageIndex] = PageLoadVariance[j,pageIndex] + stringBuffer;
                                varianceLoad[j,i] = FNum(cStr(SafeDbl(actualLoad[j,i]) - SafeDbl(budgetedLoad[j,i])), 0);
                                stringBuffer = "";
                                if((i+1) % pageSize == 0){
                                        pageIndex = pageIndex + 1;
                               }
                 }

                 if(j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }

                 varianceLoadYTD[j] = cStr(SafeDbl(ActualLoadYTD[j]) - SafeDbl(budgetedLoadYTD[j]));

                //Fill out remaining columns;
                 leftOverColumns = ((i) % pageSize);
                 for(int y=0; y < (leftOverColumns - 2) ; y++){
                        PageLoadVariance[j,pageIndex] = PageLoadVariance[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                 }

                 RightLoadVariance[j] = "<td class=xl2830747></td>";
                 RightLoadVariance[j] = RightLoadVariance[j] + "<td class=xl2930747>" + FNeg(cStr(Sum(actualLoad,j) - Sum(budgetedLoad, j)),0)  + "</td>";
                 RightLoadVariance[j] = RightLoadVariance[j] + "<td class=xl2830747></td>";
                 RightLoadVariance[j] = RightLoadVariance[j] + "<td class=xl2830747></td>";
                 RightLoadVariance[j] = RightLoadVariance[j] + "<td class=xl3030747 align='right'>" + FNeg(varianceLoadYTD[j],0) + " </td>";
                 RightLoadVariance[j] = RightLoadVariance[j] + "<td class=xl2830747></td>";
                 RightLoadVariance[j] = RightLoadVariance[j] + "</tr>";

         }

}


public void CreateUnLoadData(){

         LeftUnloadData = "<tr height=15 style='height:8pt'>";
         LeftUnloadData = LeftUnloadData + "<td height=15 class=xl2730747 style='height:8pt'><b>Unload</b></td>";
         LeftUnloadData = LeftUnloadData + "<td class=xl2830747></td>";

         // Get Total Units for( UNLOAD;

         j = 0;
         if(selMonth != 1){

                 DataReader rsUnLoadUnitsYTD = new DataReader(getUnitsHoursYTDSQL(arWeeklyDate[j], arWeeklyDate[j],"UL", "UNITS"));
                 rsUnLoadUnitsYTD.Open();

                 while (rsUnLoadUnitsYTD.Read() ){
                      ActualUnLoadYTD[j] = rsUnLoadUnitsYTD.Fields(0);

                 }

        }

         for(j=0; j <= weekIndex; j++){

                 DataReader rsUnloadUnits = new DataReader(getUnitsHourstrSQL(arWeeklyDate[j], cStr(cDate(arWeeklyDate[j]).AddDays(7)),"UL", "UNITS"));
                 rsUnloadUnits.Open();

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD;

                 while (rsUnloadUnits.Read()){

                    stringBuffer = stringBuffer + "<td class=xl2830747 align=right>" + FNum(rsUnloadUnits.Fields(0), 0) + "</td>";
                    PageUnLoadData[j,pageIndex] = PageUnLoadData[j,pageIndex] + stringBuffer;
                    actualUnLoad[j,i] = FNum(rsUnloadUnits.Fields(0), 0);
                    stringBuffer = "";
                    if((i+1) % pageSize == 0){
                            pageIndex = pageIndex + 1;
                    }
                    i = i + 1;

                 }
                 

                 if(j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }

                 ActualUnLoadYTD[j] = cStr(SafeDbl(ActualUnLoadYTD[lastYTD]) + SafeDbl(FNum(cStr(Sum(actualUnLoad,j)),0)));

                //Fill out remaining columns;
                 while ( i % pageSize != 0){
                            PageUnLoadData[j,pageIndex] = PageUnLoadData[j,pageIndex] + "<td class=xl2830747 >&nbsp;</td>";
                            //intEndofLine = 2;
                            i = i + 1;
                 }

                 RightUnLoadData[j] = "<td class=xl2830747></td>";
                 RightUnLoadData[j] = RightUnLoadData[j] + "<td class=xl2930747 align='right'>" + FNum(cStr(Sum(actualUnLoad,j)),0)  + "</td>";
                 RightUnLoadData[j] = RightUnLoadData[j] + "<td class=xl2830747></td>";
                 RightUnLoadData[j] = RightUnLoadData[j] + "<td class=xl2830747></td>";
                 RightUnLoadData[j] = RightUnLoadData[j] + "<td class=xl3030747 align='right'>" + FNum(ActualUnLoadYTD[j],0) + " </td>";
                 RightUnLoadData[j] = RightUnLoadData[j] + "<td class=xl2830747></td>";
                 RightUnLoadData[j] = RightUnLoadData[j] + "</tr>";

         }



}


public void CreateUnLoadBudgeted(){

         LeftUnloadBudget = "<tr height=15 style='height:8pt'>";
         LeftUnloadBudget = LeftUnloadBudget + "<td height=15 class=xl2830747 style='height:8pt'>Budgeted</td>";
         LeftUnloadBudget = LeftUnloadBudget + "<td class=xl3130747>&nbsp;</td>";

         j = 0;
         if(selMonth != 1){
                budgetedUnLoadYTD[j] = unLoadMonthlyBudgetYTD;
         }

         // Calculate Total Units for( Budgeted;
         for( j=0; j <= weekIndex;j++){

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;

                 for(i=0; i < (UBound(actualLoad,1)-1); i++){
                                if(unloadBudget[i] != null && Trim(unloadBudget[i]) != ""){
                                        stringBuffer = stringBuffer + "<td class=xl3130747 align=right>" + FNum(cStr(SafeDiv(cStr(SafeDbl(unloadBudget[i])), cStr(SafeDbl(cStr(weekCount))))), 0) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl2830747 align=right></td>";
                                }
                                PageUnloadBudget[j,pageIndex] = PageUnloadBudget[j,pageIndex] + stringBuffer;
                                budgetedUnLoad[j,i] = FNum(cStr(SafeDiv(cStr(SafeDbl(unloadBudget[i])), cStr(SafeDbl(cStr(weekCount))))), 0);
                                stringBuffer = "";
                                if((i+1) % pageSize == 0){
                                        pageIndex = pageIndex + 1;
                               }
                 }

                 if(j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }
                 budgetedUnLoadYTD[j] = cStr(SafeDbl(budgetedUnLoadYTD[lastYTD]) + SafeDbl(cStr(FNum(cStr(Sum(budgetedUnLoad, j)), 0))));

                //Fill out remaining columns;
                 leftOverColumns = (i) % pageSize;
                 for (int y = 0; y < (leftOverColumns - 2); y++)
                 {
                     PageUnloadBudget[j, pageIndex] = PageUnloadBudget[j, pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                 }

                 RightUnloadBudget[j] = "<td class=xl2830747></td>";
                 RightUnloadBudget[j] = RightUnloadBudget[j] + "<td class=xl3230747 align='right'>" + FNum(cStr(Sum(budgetedUnLoad, j)), 0)  + "</td>";
                 RightUnloadBudget[j] = RightUnloadBudget[j] + "<td class=xl2830747></td>";
                 RightUnloadBudget[j] = RightUnloadBudget[j] + "<td class=xl2830747></td>";
                 RightUnloadBudget[j] = RightUnloadBudget[j] + "<td class=xl3330747 align='right'>" + FNum(budgetedUnLoadYTD[j],0) + " </td>";
                 RightUnloadBudget[j] = RightUnloadBudget[j] + "<td class=xl2830747></td>";
                 RightUnloadBudget[j] = RightUnloadBudget[j] + "</tr>";

         }

}


public void CreateUnLoadVariance(){

         LeftUnLoadVariance = "<tr height=15 style='height:8pt'>";
         LeftUnLoadVariance = LeftUnLoadVariance + "<td height=15 class=xl3430747 style='height:8pt'>Variance</td>";
         LeftUnLoadVariance = LeftUnLoadVariance + "<td class=xl3530747></td>";

         for( int j=0; j <= weekIndex;j++){

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD;

                 for( i=0; i <(UBound(actualLoad,1)-1);i++){
                                if(( Trim(actualUnLoad[j,i]) != "" || Trim(budgetedUnLoad[j,i]) != "") && ( i < facilityCount) ){
                                        stringBuffer = stringBuffer + "<td class=xl3530747 align=right>" + FNeg(cStr(SafeDbl(actualUnLoad[j,i]) - SafeDbl(budgetedUnLoad[j,i])), 0) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl3530747 align=right></td>";
                                }
                                PageUnLoadVariance[j,pageIndex] = PageUnLoadVariance[j,pageIndex] + stringBuffer;
                                varianceUnLoad[j,i] = FNum(cStr(SafeDbl(actualUnLoad[j,i]) - SafeDbl(budgetedUnLoad[j,i])), 0);
                                stringBuffer = "";
                                if((i+1) % pageSize == 0){
                                        pageIndex = pageIndex + 1;
                                }
                 }

                 if(j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }
                 varianceUnLoadYTD[j] = cStr((SafeDbl(ActualUnLoadYTD[j]) - SafeDbl(budgetedUnLoadYTD[j])));

                //Fill out remaining columns;
                 leftOverColumns = (i) % pageSize;
                 for( int y=0; y < (leftOverColumns-2); y++){
                        PageUnLoadVariance[j,pageIndex] = PageUnLoadVariance[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                 }

                 RightUnLoadVariance[j] = "<td class=xl2830747></td>";
                 RightUnLoadVariance[j] = RightUnLoadVariance[j] + "<td class=xl2930747>" + FNeg(cStr(Sum(actualUnLoad,j) - Sum(budgetedUnLoad,j)),0)  + "</td>";
                 RightUnLoadVariance[j] = RightUnLoadVariance[j] + "<td class=xl2830747></td>";
                 RightUnLoadVariance[j] = RightUnLoadVariance[j] + "<td class=xl2830747></td>";
                 RightUnLoadVariance[j] = RightUnLoadVariance[j] + "<td class=xl3030747 align='right'>" + FNeg(varianceUnLoadYTD[j],0) + "</td>";
                 RightUnLoadVariance[j] = RightUnLoadVariance[j] + "<td class=xl2830747></td>";
                 RightUnLoadVariance[j] = RightUnLoadVariance[j] + "</tr>";

         }

}
//***************************************************************************************************************************;
//*************************************** ADDED 5/16/2007 STEVE HICKS *******************************************************;
public void CreateSpottingData(){

		 LeftSpottingData = "<tr height=15 style='height:8pt'><td>&nbsp;</td></tr>"	;
         LeftSpottingData = LeftSpottingData + "<tr height=15 style='height:8pt'>";
         LeftSpottingData = LeftSpottingData + "<td height=15 class=xl2730747 style='height:8pt'><b>Spotting</b></td>";
         LeftSpottingData = LeftSpottingData + "<td class=xl2830747></td>";


         // Get YTD Units for( Spotting;
         int j = 0;
         if(selMonth != 1){

                 DataReader rsSpottingUnitsYTD = new DataReader(getUnitsHoursYTDSQL(arWeeklyDate[j], arWeeklyDate[j],"SP", "UNITS"));
                 rsSpottingUnitsYTD.Open();

                 while (rsSpottingUnitsYTD.Read()){
                     ActualSpottingYTD[j] = rsSpottingUnitsYTD.Fields(0);

                 }

        }

         for(j=0; j <= weekIndex; j++){

                 DataReader rsSpottingUnits = new DataReader(getUnitsHourstrSQL(arWeeklyDate[j], cStr(cDate(arWeeklyDate[j]).AddDays(7)),"SP", "UNITS"));
                 rsSpottingUnits.Open();

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD;

                 while ( rsSpottingUnits.Read()){

                                stringBuffer = stringBuffer + "<td class=xl2830747 align=right>" + FNum(rsSpottingUnits.Fields(0), 0) + "</td>";
                                PageSpottingData[j,pageIndex] = PageSpottingData[j,pageIndex] + stringBuffer;
                                actualSpotting[j,i] = FNum(rsSpottingUnits.Fields(0), 0);
                                stringBuffer = "";
                                if((i+1) % pageSize == 0){
                                        pageIndex = pageIndex + 1;
                                }
                                i = i + 1;

                 }

                 if(j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }

                 ActualSpottingYTD[j] = cStr(SafeDbl(ActualSpottingYTD[lastYTD]) + SafeDbl(FNum(cStr(Sum(actualSpotting,j)),0)));

                //Fill out remaining columns;
                 //leftOverColumns = (i) % pageSize;
                 //for( int y=0; y < (leftOverColumns-1);
                 //      PageSpottingData[j,pageIndex] = PageSpottingData[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                 //}
             while (i % pageSize != 0){
                        PageSpottingData[j,pageIndex] = PageSpottingData[j,pageIndex] + "<td class=xl2830747 >&nbsp;</td>";
                        //intEndofLine = 2;
                        i = i + 1;
             }

                 RightSpottingData[j] = "<td class=xl2830747></td>";
                 RightSpottingData[j] = RightSpottingData[j] + "<td class=xl2930747 align='right'>" + FNum(cStr(Sum(actualSpotting,j)),0)  + "</td>";
                 RightSpottingData[j] = RightSpottingData[j] + "<td class=xl2830747></td>";
                 RightSpottingData[j] = RightSpottingData[j] + "<td class=xl2830747></td>";
                 RightSpottingData[j] = RightSpottingData[j] + "<td class=xl3030747 align='right'>" + FNum(ActualSpottingYTD[j],0) + " </td>";
                 RightSpottingData[j] = RightSpottingData[j] + "<td class=xl2830747></td>";
                 RightSpottingData[j] = RightSpottingData[j] + "</tr>";

         }



}


public void CreateSpottingBudgeted(){

         LeftSpottingBudget = "<tr height=15 style='height:8pt'>";
         LeftSpottingBudget = LeftSpottingBudget + "<td height=15 class=xl2830747 style='height:8pt'>Budgeted</td>";
         LeftSpottingBudget = LeftSpottingBudget + "<td class=xl3130747>&nbsp;</td>";

         int j = 0;
         if(selMonth != 1){
                budgetedSpottingYTD[j] = spottingMonthlyBudgetYTD;
         }

         // Calculate Total Units for( Budgeted;
         for( j=0; j <= weekIndex;j++){

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;

                 for(i=0; i < (UBound(actualLoad,1)-1); i++){

                                if(spottingBudget[i] != null && Trim(spottingBudget[i]) != ""){
                                        stringBuffer = stringBuffer + "<td class=xl3130747 align=right>" + FNum(cStr(SafeDiv(cStr(SafeDbl(spottingBudget[i])), cStr(SafeDbl(cStr(weekCount))))), 0) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl2830747 align=right></td>";
                                }
                                PageSpottingBudget[j,pageIndex] = PageSpottingBudget[j,pageIndex] + stringBuffer;
                                budgetedSpotting[j,i] = FNum(cStr(SafeDiv(cStr(SafeDbl(spottingBudget[i])), cStr(SafeDbl(cStr(weekCount))))), 0);
                                stringBuffer = "";
                                if((i+1) % pageSize == 0){
                                        pageIndex = pageIndex + 1;
                                }
                 }

                 if(j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }
                 budgetedSpottingYTD[j] = cStr(SafeDbl(budgetedSpottingYTD[lastYTD]) + SafeDbl(cStr(FNum(cStr(Sum(budgetedSpotting, j)), 0))));

                //Fill out remaining columns;
                 leftOverColumns = (i) % pageSize;
                 for(int y=0; y < (leftOverColumns-2); y++){
                        PageSpottingBudget[j,pageIndex] = PageSpottingBudget[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                 }

                 RightSpottingBudget[j] = "<td class=xl2830747></td>";
                 RightSpottingBudget[j] = RightSpottingBudget[j] + "<td class=xl3230747 align='right'>" + FNum(cStr(Sum(budgetedSpotting, j)), 0)  + "</td>";
                 RightSpottingBudget[j] = RightSpottingBudget[j] + "<td class=xl2830747></td>";
                 RightSpottingBudget[j] = RightSpottingBudget[j] + "<td class=xl2830747></td>";
                 RightSpottingBudget[j] = RightSpottingBudget[j] + "<td class=xl3330747 align='right'>" + FNum(budgetedSpottingYTD[j],0) + " </td>";
                 RightSpottingBudget[j] = RightSpottingBudget[j] + "<td class=xl2830747></td>";
                 RightSpottingBudget[j] = RightSpottingBudget[j] + "</tr>";

         }

}


public void CreateSpottingVariance(){

         LeftSpottingVariance = "<tr height=15 style='height:8pt'>";
         LeftSpottingVariance = LeftSpottingVariance + "<td height=15 class=xl3430747 style='height:8pt'>Variance</td>";
         LeftSpottingVariance = LeftSpottingVariance + "<td class=xl3530747></td>";

         for( int j=0; j <= weekIndex; j++){

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD;

                 for(i=0; i < (UBound(actualLoad,1)-1); i ++){
                                if((Trim(actualSpotting[j,i]) != "" || Trim(budgetedSpotting[j,i]) != "") && i < facilityCount){
                                        stringBuffer = stringBuffer + "<td class=xl3530747 align=right>" + FNeg(cStr(SafeDbl(actualSpotting[j,i]) - SafeDbl(budgetedSpotting[j,i])), 0) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl3530747 align=right></td>";
                                }
                                PageSpottingVariance[j,pageIndex] = PageSpottingVariance[j,pageIndex] + stringBuffer;
                                varianceSpotting[j,i] = FNum(cStr(SafeDbl(actualSpotting[j,i]) - SafeDbl(budgetedSpotting[j,i])), 0);
                                stringBuffer = "";
                                if((i+1) % pageSize == 0){
                                        pageIndex = pageIndex + 1;
                                }
                 }

                 if(j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }
                 varianceSpottingYTD[j] = cStr(SafeDbl(ActualSpottingYTD[j]) - SafeDbl(budgetedSpottingYTD[j]));

                //Fill out remaining columns;
                 leftOverColumns = (i) % pageSize;
                 for(int y=0; y < (leftOverColumns-2); y++){
                        PageSpottingVariance[j,pageIndex] = PageSpottingVariance[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                 }

                 RightSpottingVariance[j] = "<td class=xl2830747></td>";
                 RightSpottingVariance[j] = RightSpottingVariance[j] + "<td class=xl2930747>" + FNeg(cStr(Sum(actualSpotting,j) - Sum(budgetedSpotting,j)),0)  + "</td>";
                 RightSpottingVariance[j] = RightSpottingVariance[j] + "<td class=xl2830747></td>";
                 RightSpottingVariance[j] = RightSpottingVariance[j] + "<td class=xl2830747></td>";
                 RightSpottingVariance[j] = RightSpottingVariance[j] + "<td class=xl3030747 align='right'>" + FNeg(varianceSpottingYTD[j],0) + "</td>";
                 RightSpottingVariance[j] = RightSpottingVariance[j] + "<td class=xl2830747></td>";
                 RightSpottingVariance[j] = RightSpottingVariance[j] + "</tr>";

         }

}

//***************************************************************************************************************************;
public void CreateTotalVolumes(){

         LeftTotalVolume1 = "<tr height=15 style='height:8pt'>";
         LeftTotalVolume1 = LeftTotalVolume1 + "<td height=15 class=xl2830747 style='height:8pt'></td>";
         LeftTotalVolume1 = LeftTotalVolume1 + "<td class=xl2830747></td>";

         LeftTotalVolume2 = "<tr height=15 style='height:8pt'>";
         LeftTotalVolume2 = LeftTotalVolume2 + "<td height=15 class=xl3430747 style='height:8pt'>Total Volumes</td>";
         LeftTotalVolume2 = LeftTotalVolume2 + "<td class=xl2930747></td>";

         j=0;
         originalTotalVolumeYTD = cStr(SafeDbl(cStr(FNum(ActualUnLoadYTD[j],0))) +  SafeDbl(cStr(FNum(ActualLoadYTD[j],0))) +  SafeDbl(cStr(FNum(ActualSpottingYTD[j],0))));
         totalVolumeYTD[j] = originalTotalVolumeYTD;

         for( j=0; j <= weekIndex;  j++){

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;

                 for (i=0; i <  (UBound(actualLoad,1)-1);i++){
                                PageTotalVolume1[j,pageIndex] = PageTotalVolume1[j,pageIndex] + "<td class=xl2830747></td>";
                                if ((actualUnLoad[j,i] != null && Trim(actualUnLoad[j,i]) != "") || (actualLoad[j,i] != null && Trim(actualLoad[j,i]) != "") || (actualSpotting[j,i] != null && Trim(actualSpotting[j,i]) != "")){
                                        stringBuffer = stringBuffer + "<td class=xl2930747 align=right>" + FNeg(cStr(SafeDbl(actualLoad[j,i]) + SafeDbl(actualUnLoad[j,i]) + SafeDbl(actualSpotting[j,i])), 0) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl2930747 align=right></td>";
                                }
                                PageTotalVolume2[j,pageIndex] = PageTotalVolume2[j,pageIndex] + stringBuffer;
                                totalVolume[j,i] = FNum(cStr(SafeDbl(actualLoad[j,i]) + SafeDbl(actualUnLoad[j,i]) + SafeDbl(actualSpotting[j,i])), 0);
                                budgetedVolume[j,i] = FNum(cStr(SafeDbl(budgetedLoad[j,i]) + SafeDbl(budgetedUnLoad[j,i]) + SafeDbl(budgetedSpotting[j,i])), 0);
                                stringBuffer = "";
                                if ( ((i+1) % pageSize) == 0 ){
                                        pageIndex = pageIndex + 1;
                               }
                 }

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                }
                 totalVolumeYTD[j] = cStr(SafeDbl(totalVolumeYTD[lastYTD]) + SafeDbl(cStr(FNum(cStr(Sum(totalVolume,j)),0))));

                //Fill out remaining columns;
                 leftOverColumns = (i) % pageSize;
                 for( int y=0; y < (leftOverColumns-2);y++){
                        PageTotalVolume1[j,pageIndex] = PageTotalVolume1[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                        PageTotalVolume2[j,pageIndex] = PageTotalVolume2[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                 }


                 RightTotalVolume1[j] = "<td class=xl2830747></td>";
                 RightTotalVolume1[j] = RightTotalVolume1[j] + "<td class=xl2930747></td>";
                 RightTotalVolume1[j] = RightTotalVolume1[j] + "<td class=xl2830747></td>";
                 RightTotalVolume1[j] = RightTotalVolume1[j] + "<td class=xl2830747>/- Budget</td>";
                 RightTotalVolume1[j] = RightTotalVolume1[j] + "<td class=xl2830747 align='right'></td>";
                 RightTotalVolume1[j] = RightTotalVolume1[j] + "<td class=xl2830747>/- Budget</td>";
                 RightTotalVolume1[j] = RightTotalVolume1[j] + "</tr>";

                 RightTotalVolume2[j] = "<td class=xl2830747></td>";
                 RightTotalVolume2[j] = RightTotalVolume2[j] + "<td class=xl2930747>" + FNeg(cStr(Sum(totalVolume,j)),0)  + "</td>";
                 RightTotalVolume2[j] = RightTotalVolume2[j] + "<td class=xl3030747></td>";
                 RightTotalVolume2[j] = RightTotalVolume2[j] + "<td class=xl2830747 align='center'>" + FNeg(cStr(SafeDbl(FNum(cStr(Sum(totalVolume,j)),0)) - (SafeDbl(FNum(cStr(Sum(budgetedUnLoad,j)),0)) + SafeDbl(FNum(cStr(Sum(budgetedLoad,j)),0)) + SafeDbl(FNum(cStr(Sum(budgetedSpotting,j)),0)))),0)   + "</td>";
                 RightTotalVolume2[j] = RightTotalVolume2[j] + "<td class=xl3030747 align='right'>" + FNum(totalVolumeYTD[j],0) + "</td>";
                 RightTotalVolume2[j] = RightTotalVolume2[j] + "<td class=xl2830747 align='left'>&nbsp;&nbsp;&nbsp;" + FNeg(cStr(SafeDbl(cStr(FNum(totalVolumeYTD[j],0))) - (SafeDbl(cStr(FNum(budgetedUnLoadYTD[j],0))) + SafeDbl(cStr(FNum(budgetedLoadYTD[j],0))) + SafeDbl(cStr(FNum(budgetedSpottingYTD[j],0))))),0) + "</td>";
                 RightTotalVolume2[j] = RightTotalVolume2[j] + "</tr>";

         }

}


public void CreatePayRollIRT(){

         LeftPayRollIRT = "<tr height=15 style='height:8pt'>";
         LeftPayRollIRT = LeftPayRollIRT + "<td height=15 class=xl2730747 style='height:8pt'>IRT</td>";
         LeftPayRollIRT = LeftPayRollIRT + "<td class=xl2830747></td>";


         // Get YTD PayRoll IRT;
         int j = 0;
         if (selMonth != 1){

                 DataReader rsPayRollIRTYTD = new DataReader(getYTDSQL(arWeeklyDate[j], arWeeklyDate[j],"IRT","PAYROLL"));
                 rsPayRollIRTYTD.Open();

                 while (rsPayRollIRTYTD.Read()){
                    PayrollIRTYTD[j] = rsPayRollIRTYTD.Fields(0);

                 }

         }

         for(  j=0; j <= weekIndex;  j++){

                 DataReader rsPayRollIRT = new DataReader(getSQL(arWeeklyDate[j], cStr(cDate(arWeeklyDate[j]).AddDays(7)) ,"IRT","PAYROLL"));
                 rsPayRollIRT.Open();

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;
                 while (rsPayRollIRT.Read()){
                                stringBuffer = stringBuffer + "<td class=xl2830747 align=right>" + FNum(rsPayRollIRT.Fields(0), 0) + "</td>";
                                PagePayRollIRT[j,pageIndex] = PagePayRollIRT[j,pageIndex] + stringBuffer;
                                payRollIRT[j,i] = FNum(rsPayRollIRT.Fields(0), 0);
                                stringBuffer = "";
                                if ( ((i+1) % pageSize) == 0 ){
                                        pageIndex = pageIndex + 1;
                               }
                                i = i + 1;

                 }

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }
                 PayrollIRTYTD[j] = cStr(SafeDbl(PayrollIRTYTD[lastYTD]) + SafeDbl(FNum(cStr(Sum(payRollIRT,j)),0 )));

                //Fill out remaining columns;
                 //leftOverColumns = (i) % pageSize;
                 //for( int y=0; y < (leftOverColumns-1);
                 //      PagePayRollIRT[j,pageIndex] = PagePayRollIRT[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                 //}
             while ( i % pageSize != 0){
                        PagePayRollIRT[j,pageIndex] = PagePayRollIRT[j,pageIndex] + "<td class=xl2830747 >&nbsp;</td>";
                        //intEndofLine = 2;
                        i = i + 1;
             }

                 RightPayRollIRT[j] = "<td class=xl2830747></td>";
                 RightPayRollIRT[j] = RightPayRollIRT[j] + "<td class=xl2930747 align='right'>" + FCur(cStr(Sum(payRollIRT,j)),0 ) + "</td>";
                 RightPayRollIRT[j] = RightPayRollIRT[j] + "<td class=xl2830747></td>";
                 RightPayRollIRT[j] = RightPayRollIRT[j] + "<td class=xl2830747></td>";
                 RightPayRollIRT[j] = RightPayRollIRT[j] + "<td class=xl3030747 align='right'>" + FCur(PayrollIRTYTD[j] ,0) + "</td>";
                 RightPayRollIRT[j] = RightPayRollIRT[j] + "<td class=xl2830747></td>";
                 RightPayRollIRT[j] = RightPayRollIRT[j] + "</tr>";

         }



}


public void CreatePayRollTEMP(){

         LeftPayRollTEMP = "<tr height=15 style='height:8pt'>";
         LeftPayRollTEMP = LeftPayRollTEMP + "<td height=15 class=xl2830747 style='height:8pt'>TEMP</td>";
         LeftPayRollTEMP = LeftPayRollTEMP + "<td class=xl3230747>&nbsp;</td>";

         // Get YTD PayRoll TEMP;
         int j = 0;
         if (selMonth != 1){

                 DataReader rsPayRollTEMPYTD = new DataReader(getYTDSQL(arWeeklyDate[j], arWeeklyDate[j],"TEMP","PAYROLL"));
                 rsPayRollTEMPYTD.Open();

                 while (rsPayRollTEMPYTD.Read()){
                    PayrollTEMPYTD[j] = rsPayRollTEMPYTD.Fields(0);

                 }

        }

         for( j=0; j <= weekIndex;  j++){

                 DataReader rsPayRollTEMP = new DataReader(getSQL(arWeeklyDate[j], cStr(cDate(arWeeklyDate[j]).AddDays(7)),"TEMP","PAYROLL"));
                 rsPayRollTEMP.Open();

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;
 
                 while (rsPayRollTEMP.Read()){

                    stringBuffer = stringBuffer + "<td class=xl3230747 align=right>" + FNum(rsPayRollTEMP.Fields(0), 0) + "</td>";
                    PagePayRollTEMP[j,pageIndex] = PagePayRollTEMP[j,pageIndex] + stringBuffer;
                    payRollTEMP[j,i] = FNum(rsPayRollTEMP.Fields(0), 0);
                    stringBuffer = "";
                    if ( ((i+1) % pageSize) == 0 ){
                            pageIndex = pageIndex + 1;
                    }
                    i = i + 1;

                 }

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }
                 PayrollTEMPYTD[j] = cStr(SafeDbl(PayrollTEMPYTD[lastYTD]) + SafeDbl(FNum(cStr(Sum(payRollTEMP,j)),0 )));

                //Fill out remaining columns;
                 //leftOverColumns = (i) % pageSize;
                 //for( int y=0; y < (leftOverColumns-1);
                 //      PagePayRollTEMP[j,pageIndex] = PagePayRollTEMP[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                 //}
             while (i % pageSize != 0){
                        PagePayRollTEMP[j,pageIndex] = PagePayRollTEMP[j,pageIndex] + "<td class=xl2830747 >&nbsp;</td>";
                        //intEndofLine = 2;
                        i = i + 1;
             }

                 RightPayRollTEMP[j] = "<td class=xl2830747></td>";
                 RightPayRollTEMP[j] = RightPayRollTEMP[j] + "<td class=xl3230747 align='right'>" + FCur(cStr(Sum(payRollTEMP,j)),0 ) + "</td>";
                 RightPayRollTEMP[j] = RightPayRollTEMP[j] + "<td class=xl2830747></td>";
                 RightPayRollTEMP[j] = RightPayRollTEMP[j] + "<td class=xl2830747></td>";
                 RightPayRollTEMP[j] = RightPayRollTEMP[j] + "<td class=xl3730747 align='right'>" + FCur(PayrollTEMPYTD[j],0) + "</td>";
                 RightPayRollTEMP[j] = RightPayRollTEMP[j] + "<td class=xl2830747></td>";
                 RightPayRollTEMP[j] = RightPayRollTEMP[j] + "</tr>";

         }



}


public void CreateTotalPayRoll(){

         LeftTotalPayRoll = "<tr height=15 style='height:8pt'>";
         LeftTotalPayRoll = LeftTotalPayRoll + "<td height=15 class=xl3430747 style='height:8pt'>Total Payroll</td>";
         LeftTotalPayRoll = LeftTotalPayRoll + "<td class=xl3530747></td>";

         //Get YTD TotalPayroll;
         j=0;
         payRollTotalYTD[j] = cStr(SafeDbl(FNum(PayrollTEMPYTD[j],0)) + SafeDbl(FNum(PayrollIRTYTD[j],0)));
         originalPayRollTotalYTD = payRollTotalYTD[j];

         for( j=0; j <= weekIndex;  j++){
                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;

                 for ( i=0; i <  (UBound(actualLoad,1)-1);i++){
                                if ((payRollTEMP[j,i] != null && Trim(payRollTEMP[j,i]) != "") || (payRollIRT[j,i] != null && Trim(payRollIRT[j,i]) != "")){
                                        stringBuffer = stringBuffer + "<td class=xl3530747 align=right>" + FCur(cStr(SafeDbl(payRollTEMP[j,i]) + SafeDbl(payRollIRT[j,i])), 0) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl3530747 align=right></td>";
                                }
                                PageTotalPayRoll[j,pageIndex] = PageTotalPayRoll[j,pageIndex] + stringBuffer;
                                payRollTotal[j,i] = FNum(cStr(SafeDbl(payRollTEMP[j,i]) + SafeDbl(payRollIRT[j,i])),0);
                                stringBuffer = "";
                                if ( ((i+1) % pageSize) == 0 ){
                                        pageIndex = pageIndex + 1;
                               }
                 }

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }
                 payRollTotalYTD[j] = cStr(SafeDbl(payRollTotalYTD[lastYTD]) + SafeDbl(FNum(cStr(Sum(payRollTotal,j)),0)));

                //Fill out remaining columns;
                 leftOverColumns = (i) % pageSize;
                 for( int y=0; y < (leftOverColumns-2); y++){
                        PageTotalPayRoll[j,pageIndex] = PageTotalPayRoll[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                 }

                 RightTotalPayRoll[j] = "<td class=xl2830747></td>";
                 RightTotalPayRoll[j] = RightTotalPayRoll[j] + "<td class=xl2930747>" + FCur(cStr(Sum(payRollTotal,j)),0)  + "</td>";
                 RightTotalPayRoll[j] = RightTotalPayRoll[j] + "<td class=xl2830747></td>";
                 RightTotalPayRoll[j] = RightTotalPayRoll[j] + "<td class=xl2830747></td>";
                 RightTotalPayRoll[j] = RightTotalPayRoll[j] + "<td class=xl3030747 align='right'>" + FCur(payRollTotalYTD[j],0) + "</td>";
                 RightTotalPayRoll[j] = RightTotalPayRoll[j] + "<td class=xl2830747></td>";
                 RightTotalPayRoll[j] = RightTotalPayRoll[j] + "</tr>";

         }

}


public void CreatePayRollHrsIRT(){

         LeftPayRollHrsIRT = "<tr height=15 style='height:8pt'>";
         LeftPayRollHrsIRT = LeftPayRollHrsIRT + "<td height=15 class=xl2730747 style='height:8pt'>IRT</td>";
         LeftPayRollHrsIRT = LeftPayRollHrsIRT + "<td class=xl2830747></td>";

         // Get YTD PayRoll IRT;
         int j = 0;
         if (selMonth != 1){

                 DataReader rsPayRollHrsIRTYTD = new DataReader(getUnitsHoursYTDSQL(arWeeklyDate[j], arWeeklyDate[j],"ALL", "IRT"));
                 rsPayRollHrsIRTYTD.Open();

                 while (rsPayRollHrsIRTYTD.Read()){
                        payRollIRTHrsYTD[j] = rsPayRollHrsIRTYTD.Fields(0);

                 }

         }

         for(j=0; j <= weekIndex;  j++){

                 DataReader rsPayRollHrsIRT = new DataReader(getUnitsHourstrSQL(arWeeklyDate[j], cStr(cDate(arWeeklyDate[j]).AddDays(7)) ,"ALL", "IRT"));
                 rsPayRollHrsIRT.Open();

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD;

                 while (rsPayRollHrsIRT.Read()){

                                stringBuffer = stringBuffer + "<td class=xl2830747 align=right>" + FNum(rsPayRollHrsIRT.Fields(0), 0) + "</td>";
                                PagePayRollHrsIRT[j,pageIndex] = PagePayRollHrsIRT[j,pageIndex] + stringBuffer;
                                payRollHrsIRT[j,i] = FNum(rsPayRollHrsIRT.Fields(0), 0);
                                stringBuffer = "";
                                if ( ((i+1) % pageSize) == 0 ){
                                        pageIndex = pageIndex + 1;
                               }
                                i = i + 1;

                 }

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }
                 payRollIRTHrsYTD[j] = cStr(SafeDbl(payRollIRTHrsYTD[lastYTD]) + SafeDbl(FNum(cStr(Sum(payRollHrsIRT,j)),0 )));

                //Fill out remaining columns;
                 //leftOverColumns = (i) % pageSize;
                 //for( int y=0; y < (leftOverColumns-1);
                 //      PagePayRollHrsIRT[j,pageIndex] = PagePayRollHrsIRT[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                 //}
             while ( i % pageSize != 0){
                        PagePayRollHrsIRT[j,pageIndex] = PagePayRollHrsIRT[j,pageIndex] + "<td class=xl2830747 >&nbsp;</td>";
                        //intEndofLine = 2;
                        i = i + 1;
             }

                 RightPayRollHrsIRT[j] = "<td class=xl2830747></td>";
                 RightPayRollHrsIRT[j] = RightPayRollHrsIRT[j] + "<td class=xl2930747 align='right'>" + FNum(cStr(Sum(payRollHrsIRT,j)),0 ) + "</td>";
                 RightPayRollHrsIRT[j] = RightPayRollHrsIRT[j] + "<td class=xl2830747></td>";
                 RightPayRollHrsIRT[j] = RightPayRollHrsIRT[j] + "<td class=xl2830747></td>";
                 RightPayRollHrsIRT[j] = RightPayRollHrsIRT[j] + "<td class=xl3030747 align='right'>" + FNum(payRollIRTHrsYTD[j],0)  + "</td>";
                 RightPayRollHrsIRT[j] = RightPayRollHrsIRT[j] + "<td class=xl2830747></td>";
                 RightPayRollHrsIRT[j] = RightPayRollHrsIRT[j] + "</tr>";

         }



}


public void CreatePayRollHrsTEMP(){

         LeftPayRollHrsTEMP = "<tr height=15 style='height:8pt'>";
         LeftPayRollHrsTEMP = LeftPayRollHrsTEMP + "<td height=15 class=xl2830747 style='height:8pt'>TEMP</td>";
         LeftPayRollHrsTEMP = LeftPayRollHrsTEMP + "<td class=xl3230747>&nbsp;</td>";

         // Get YTD PayRollHrs TEMP;
         int j = 0;
         if (selMonth != 1){

                 DataReader rsPayRollHrsTEMPYTD = new DataReader(getUnitsHoursYTDSQL(arWeeklyDate[j], arWeeklyDate[j],"ALL", "TEMP"));
                 rsPayRollHrsTEMPYTD.Open();

                 while (rsPayRollHrsTEMPYTD.Read()){
                    payRollTEMPHrsYTD[j] = rsPayRollHrsTEMPYTD.Fields(0);

                 }

        }

         for( j=0; j <= weekIndex;  j++){

                 DataReader rsPayRollHrsTEMP = new DataReader(getUnitsHourstrSQL(arWeeklyDate[j], cStr(cDate(arWeeklyDate[j]).AddDays(7)),"ALL", "TEMP"));
                 rsPayRollHrsTEMP.Open();

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;

                 while (rsPayRollHrsTEMP.Read()){

                    stringBuffer = stringBuffer + "<td class=xl3230747 align=right>" + FNum(rsPayRollHrsTEMP.Fields(0), 0) + "</td>";
                    PagePayRollHrsTEMP[j,pageIndex] = PagePayRollHrsTEMP[j,pageIndex] + stringBuffer;
                    payRollHrsTEMP[j,i] = FNum(rsPayRollHrsTEMP.Fields(0), 0);
                    stringBuffer = "";
                    if ( ((i+1) % pageSize) == 0 ){
                            pageIndex = pageIndex + 1;
                    }
                    i = i + 1;

                 }

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }
                 payRollTEMPHrsYTD[j] = cStr(SafeDbl(payRollTEMPHrsYTD[lastYTD]) + SafeDbl(FNum(cStr(Sum(payRollHrsTEMP,j)),0 )));

             //Fill out remaining columns;
             while (i % pageSize != 0 ){
                        PagePayRollHrsTEMP[j,pageIndex] = PagePayRollHrsTEMP[j,pageIndex] + "<td class=xl2830747 >&nbsp;</td>";
                        //intEndofLine = 2;
                        i = i + 1;
             }

                 RightPayRollHrsTEMP[j] = "<td class=xxl2830747></td>";
                 RightPayRollHrsTEMP[j] = RightPayRollHrsTEMP[j] + "<td class=xl3230747 align='right'>" + FNum(cStr(Sum(payRollHrsTEMP,j)),0 ) + "</td>";
                 RightPayRollHrsTEMP[j] = RightPayRollHrsTEMP[j] + "<td class=xl2830747></td>";
                 RightPayRollHrsTEMP[j] = RightPayRollHrsTEMP[j] + "<td class=xl2830747></td>";
                 RightPayRollHrsTEMP[j] = RightPayRollHrsTEMP[j] + "<td class=xl3730747 align='right'>" + FNum(payRollTEMPHrsYTD[j],0)  + "</td>";
                 RightPayRollHrsTEMP[j] = RightPayRollHrsTEMP[j] + "<td class=xl2830747></td>";
                 RightPayRollHrsTEMP[j] = RightPayRollHrsTEMP[j] + "</tr>";

         }



}


public void CreateTotalPayRollHrs(){

         LeftTotalPayRollHrs = "<tr height=15 style='height:8pt'>";
         LeftTotalPayRollHrs = LeftTotalPayRollHrs + "<td height=15 class=xl3430747 style='height:8pt'>Total Hours</td>";
         LeftTotalPayRollHrs = LeftTotalPayRollHrs + "<td class=xl3530747></td>";

         //Get YTD Total Payroll Hours;
         j=0;
         payRollHrsYTD[j] = cStr(SafeDbl(FNum(payRollTEMPHrsYTD[j],0)) + SafeDbl(FNum(payRollIRTHrsYTD[j],0)));
         originalPayRollTotalHrsYTD = payRollHrsYTD[j];

         for( j=0; j <= weekIndex;  j++){

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;


                 for (i=0; i <  (UBound(actualLoad,1)-1);i++){
                                if ((payRollHrsTEMP[j,i] != null && Trim(payRollHrsTEMP[j,i]) != "") || (payRollHrsIRT[j,i] != null && Trim(payRollHrsIRT[j,i]) != "")){
                                        stringBuffer = stringBuffer + "<td class=xl3530747 align=right>" + FNum(cStr(SafeDbl(payRollHrsTEMP[j,i]) + SafeDbl(payRollHrsIRT[j,i])), 0) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl3530747 align=right></td>";
                                }
                                PageTotalPayRollHrs[j,pageIndex] = PageTotalPayRollHrs[j,pageIndex] + stringBuffer;
                                payRollHrsTotal[j,i] = FNum(cStr(SafeDbl(payRollHrsTEMP[j,i]) + SafeDbl(payRollHrsIRT[j,i])), 0);
                                stringBuffer = "";
                                if ( ((i+1) % pageSize) == 0 ){
                                        pageIndex = pageIndex + 1;
                                }
                 }

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                }
                 payRollHrsYTD[j] = cStr(SafeDbl(payRollHrsYTD[lastYTD]) + SafeDbl( FNum(cStr(Sum(payRollHrsTotal,j)),0)));

                //Fill out remaining columns;
                 leftOverColumns = (i) % pageSize;
                 for( int y=0; y < (leftOverColumns-2);y++){
                        PageTotalPayRollHrs[j,pageIndex] = PageTotalPayRollHrs[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                 }

                 RightTotalPayRollHrs[j] = "<td class=xl2830747></td>";
                 RightTotalPayRollHrs[j] = RightTotalPayRollHrs[j] + "<td class=xl2930747>" + FNum(cStr(Sum(payRollHrsTotal,j)),0)  + "</td>";
                 RightTotalPayRollHrs[j] = RightTotalPayRollHrs[j] + "<td class=xl2830747></td>";
                 RightTotalPayRollHrs[j] = RightTotalPayRollHrs[j] + "<td class=xl2830747></td>";
                 RightTotalPayRollHrs[j] = RightTotalPayRollHrs[j] + "<td class=xl3030747 align='right'>" + FNum(payRollHrsYTD[j] ,0) + "</td>";
                 RightTotalPayRollHrs[j] = RightTotalPayRollHrs[j] + "<td class=xl2830747></td>";
                 RightTotalPayRollHrs[j] = RightTotalPayRollHrs[j] + "</tr>";

         }

}



public void CreateOverTime(){

         LeftOverTime = "<tr height=15 style='height:8pt'>";
         LeftOverTime = LeftOverTime + "<td height=15 class=xl4330747 style='height:8pt'><b>Overtime</b></td>";
         LeftOverTime = LeftOverTime + "<td class=xl4430747>&nbsp;</td>";

         // Get YTD OverTime;
         int j = 0;
         if (selMonth != 1){

                 DataReader rsOverTimeYTD = new DataReader(getOvertimeYTDSQL(arWeeklyDate[j], arWeeklyDate[j]));
                 rsOverTimeYTD.Open();

                 while (rsOverTimeYTD.Read()){
                    OverTimeYTD[j] = rsOverTimeYTD.Fields(0);

                 }
 
         }
         originalOverTimeYTD = cStr(SafeDbl(OverTimeYTD[j]));

         for( j=0; j <= weekIndex;  j++){

                 DataReader rsOverTime = new DataReader(getOvertimeSQL(arWeeklyDate[j], cStr(cDate(arWeeklyDate[j]).AddDays(7))));
                 rsOverTime.Open();

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;


                 while (rsOverTime.Read()){
                    stringBuffer = stringBuffer + "<td class=xl4430747 align=right>" + FCur(rsOverTime.Fields(0), 0) + "</td>";
                    PageOverTime[j,pageIndex] = PageOverTime[j,pageIndex] + stringBuffer;
                    overTimeTotal[j,i] = FNum(rsOverTime.Fields(0),0);
                    stringBuffer = "";
                    if ( ((i+1) % pageSize) == 0 ){
                            pageIndex = pageIndex + 1;
                    }
                    i = i + 1;

                 }

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }
                 OverTimeYTD[j] = cStr(SafeDbl(OverTimeYTD[lastYTD]) + SafeDbl(FNum(cStr(Sum(overTimeTotal,j)),0 )));

                //Fill out remaining columns;
             while (i % pageSize != 0){
                        PageOverTime[j,pageIndex] = PageOverTime[j,pageIndex] + "<td class=xl2830747 >&nbsp;</td>";
                        //intEndofLine = 2;
                        i = i + 1;
             }

                 RightOverTime[j] = "<td class=xl2830747></td>";
                 RightOverTime[j] = RightOverTime[j] + "<td class=xl3630747 align='right'>" + FCur(cStr(Sum(overTimeTotal,j)),0 ) + "</td>";
                 RightOverTime[j] = RightOverTime[j] + "<td class=xl2830747></td>";
                 RightOverTime[j] = RightOverTime[j] + "<td class=xl2830747></td>";
                 RightOverTime[j] = RightOverTime[j] + "<td class=xl3630747 align='right'>" + FCur(OverTimeYTD[j] ,0) + "</td>";
                 RightOverTime[j] = RightOverTime[j] + "<td class=xl2830747></td>";
                 RightOverTime[j] = RightOverTime[j] + "</tr>";

         }



}


public void CreateOTasPayroll(){

         LeftOTasPayroll = "<tr height=15 style='height:8pt'>";
         LeftOTasPayroll = LeftOTasPayroll + "<td height=15 class=xl4530747 style='height:8pt'>OT as % of Total Payroll</td>";
         LeftOTasPayroll = LeftOTasPayroll + "<td class=xl4630747>&nbsp;</td>";

         for( int j=0; j <= weekIndex;  j++){

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD =0; 

                 for (i=0; i <  (UBound(actualLoad,1)-1);i++){
                        if (Trim(payRollTotal[j,i]) != "" && Trim(overTimeTotal[j,i]) != ""){
                                stringBuffer = stringBuffer + "<td class=xl4630747 align=right>" + FPerc(SafeDiv(cStr(SafeDbl(overTimeTotal[j,i])), cStr(SafeDbl(payRollTotal[j,i])))) + "</td>";
                        }else{
                                stringBuffer = stringBuffer + "<td class=xl4730747 align=right></td>";
                       }
                        PageOTasPayroll[j,pageIndex] = PageOTasPayroll[j,pageIndex] + stringBuffer;
                        OTasPayrollTotal[j,i] = FNum(cStr(SafeDiv(overTimeTotal[j,i],payRollTotal[j,i])), 0);
                        stringBuffer = "";
                        if ( ((i+1) % pageSize) == 0 ){
                                pageIndex = pageIndex + 1;
                        }
                 }

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }
                 OTasPayrollYTD[j] = cStr(SafeDiv(cStr(SafeDbl(OverTimeYTD[j])), cStr(SafeDbl(payRollTotalYTD[j]))));

                //Fill out remaining columns;
                 leftOverColumns = (i) % pageSize;
                 for( int y=0; y < (leftOverColumns-2);y++){
                        PageOTasPayroll[j,pageIndex] = PageOTasPayroll[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                 }

                 RightOTasPayroll[j] = "<td class=xl4730747></td>";
                 RightOTasPayroll[j] = RightOTasPayroll[j] + "<td class=xl4630747 align='right'>" + FPerc(SafeDiv(cStr(SafeDbl(cStr(Sum(overTimeTotal,j)))), cStr(SafeDbl(cStr(Sum(payRollTotal,j))))))  + "</td>";
                 RightOTasPayroll[j] = RightOTasPayroll[j] + "<td class=xl4730747></td>";
                 RightOTasPayroll[j] = RightOTasPayroll[j] + "<td class=xl4730747></td>";
                 RightOTasPayroll[j] = RightOTasPayroll[j] + "<td class=xl4630747 align='right'>" + FPerc(cDbl(OTasPayrollYTD[j])) + "</td>";
                 RightOTasPayroll[j] = RightOTasPayroll[j] + "<td class=xl4730747></td>";
                 RightOTasPayroll[j] = RightOTasPayroll[j] + "</tr>";

         }

}


public void CreateEmployeesIRT(){

         LeftEmployeesIRT = "<tr height=15 style='height:8pt'>";
         LeftEmployeesIRT = LeftEmployeesIRT + "<td height=15 class=xl3930747 style='height:8pt'>IRT</td>";
         LeftEmployeesIRT = LeftEmployeesIRT + "<td class=xl4930747>&nbsp;</td>";

         // Get YTD Employees IRT;
         int j = 0;
         if (selMonth != 1){

                 DataReader rsEmployeesIRTYTD = new DataReader(getEmployeeCountSQL(arWeeklyDate[0], arWeeklyDate[0],"IRT", "YTD"));
                 rsEmployeesIRTYTD.Open();

                 int x = 0;
                 while (rsEmployeesIRTYTD.Read()){
                        if( x != 0){
                                EmployeesIRTYTD[0] = cStr(SafeDiv(cStr(SafeDbl(EmployeesIRTYTD[0]) + SafeDbl(rsEmployeesIRTYTD.Fields(0))), "2"));
                        }else{
                                EmployeesIRTYTD[0] = cStr(SafeDbl(rsEmployeesIRTYTD.Fields(0)));
                        }

                        x = x + 1;
                 }

         }

         for( j=0; j <= weekIndex;  j++){

                 DataReader rsEmployeesIRT = new DataReader(getEmployeeCountSQL(arWeeklyDate[j], cStr(cDate(arWeeklyDate[j]).AddDays(7)),"IRT", "TOTAL"));
                 rsEmployeesIRT.Open();

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;
                 int intDivider = 0;

                 while (rsEmployeesIRT.Read()){
                        stringBuffer = stringBuffer + "<td class=xl4930747 align=right>" + FNum(cStr(SafeDbl(rsEmployeesIRT.Fields(0))), 0) + "</td>";
                        PageEmployeesIRT[j,pageIndex] = PageEmployeesIRT[j,pageIndex] + stringBuffer;
                        employeesIRTTotal[j,i] = FNum(rsEmployeesIRT.Fields(0), 0);
                        stringBuffer = "";
                        if ( ((i+1) % pageSize) == 0 ){
                                pageIndex = pageIndex + 1;
                        }
                        i = i + 1;

                 }

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }

                 if (selMonth == 1 && j == 0){
                        intDivider = 1;
                 }else{
                        intDivider = 2;
                 }

                 EmployeesIRTYTD[j] = cStr((SafeDbl(EmployeesIRTYTD[lastYTD]) + SafeDbl(FNum(cStr(Sum(employeesIRTTotal,j)),0 ))) / intDivider);

                //Fill out remaining columns;
                 //leftOverColumns = (i) % pageSize;
                 //for( int y=0; y < (leftOverColumns-1);
                 //      PageEmployeesIRT[j,pageIndex] = PageEmployeesIRT[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                 //}
             while (i % pageSize != 0){
                        PageEmployeesIRT[j,pageIndex] = PageEmployeesIRT[j,pageIndex] + "<td class=xl2830747 >&nbsp;</td>";
                        //intEndofLine = 2;
                        i = i + 1;
             }

                 RightEmployeesIRT[j] = "<td class=xl5030747></td>";
                 RightEmployeesIRT[j] = RightEmployeesIRT[j] + "<td class=xl4030747 align='right'>" + FNum(cStr(Sum(employeesIRTTotal,j)),0 ) + "</td>";
                 RightEmployeesIRT[j] = RightEmployeesIRT[j] + "<td class=xl3930747></td>";
                 RightEmployeesIRT[j] = RightEmployeesIRT[j] + "<td class=xl3930747></td>";
                 RightEmployeesIRT[j] = RightEmployeesIRT[j] + "<td class=xl3030747 align='right'>" + FNum(EmployeesIRTYTD[j],0) + "</td>";
                 RightEmployeesIRT[j] = RightEmployeesIRT[j] + "<td class=xl3930747>average</td>";
                 RightEmployeesIRT[j] = RightEmployeesIRT[j] + "</tr>";

         }



}


public void CreateEmployeesTEMP(){

         LeftEmployeesTEMP = "<tr height=15 style='height:8pt'>";
         LeftEmployeesTEMP = LeftEmployeesTEMP + "<td height=15 class=xl3930747 style='height:8pt'>TEMP</td>";
         LeftEmployeesTEMP = LeftEmployeesTEMP + "<td class=xl5130747>&nbsp;</td>";

         // Get YTD Employees TEMP;
         j = 0;
         if(selMonth != 1){

                 DataReader rsEmployeesTEMPYTD = new DataReader(getEmployeeCountSQL(arWeeklyDate[0], arWeeklyDate[0],"TEMP", "YTD"));
                 rsEmployeesTEMPYTD.Open();

                 int x = 0;
                 while (! rsEmployeesTEMPYTD.EOF){
                      rsEmployeesTEMPYTD.Read();
                       if( x != 0){
                                EmployeesTEMPYTD[0] = cStr(SafeDiv(cStr(SafeDbl(EmployeesTEMPYTD[0]) + SafeDbl(rsEmployeesTEMPYTD.Fields(0))), "2"));
                        }else{
                                EmployeesTEMPYTD[0] = cStr(SafeDbl(rsEmployeesTEMPYTD.Fields(0)));
                       }

                        x = x + 1;
                 }

         }

         for( j=0; j <= weekIndex; j++){

                 DataReader rsEmployeesTEMP = new DataReader(getEmployeeCountSQL(arWeeklyDate[j], cStr(cDate(arWeeklyDate[j]).AddDays(7)),"TEMP", "TOTAL"));
                 rsEmployeesTEMP.Open();

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;
                 int intDivider = 0;

                 while (! rsEmployeesTEMP.EOF){
                       rsEmployeesTEMP.Read();
                        stringBuffer = stringBuffer + "<td class=xl5130747 align=right>&nbsp;" + FNum(cStr(SafeDbl(rsEmployeesTEMP.Fields(0))), 0) + "</td>";
                        PageEmployeesTEMP[j,pageIndex] = PageEmployeesTEMP[j,pageIndex] + stringBuffer;
                        employeesTEMPTotal[j,i] = FNum(rsEmployeesTEMP.Fields(0), 0);
                        stringBuffer = "";
                        if ( ((i+1) % pageSize) == 0 ){
                                pageIndex = pageIndex + 1;
                        }
                        i = i + 1;

                 }

                if ( j == 0){
                        lastYTD = 0;
                }else{
                        lastYTD = j-1;
                }

                if((selMonth == 1) && (j == 0)){
                        intDivider = 1;
                 }else{
                        intDivider = 2;
                }

                 EmployeesTEMPYTD[j] = cStr((SafeDbl(EmployeesTEMPYTD[lastYTD]) + SafeDbl(FNum(cStr(Sum(employeesTEMPTotal,j)),0 ))) / intDivider);

                //Fill out remaining columns;
            while ( i % pageSize != 0){
                        PageEmployeesTEMP[j,pageIndex] = PageEmployeesTEMP[j,pageIndex] + "<td class=xl2830747 >&nbsp;</td>";
                        //intEndofLine = 2;
                        i = i + 1;
             }

                 RightEmployeesTEMP[j] = "<td class=xxl2830747></td>";
                 RightEmployeesTEMP[j] = RightEmployeesTEMP[j] + "<td class=xl4130747 align='right'>" + FNum(cStr(Sum(employeesTEMPTotal,j)),0 ) + "</td>";
                 RightEmployeesTEMP[j] = RightEmployeesTEMP[j] + "<td class=xl3930747></td>";
                 RightEmployeesTEMP[j] = RightEmployeesTEMP[j] + "<td class=xl3930747></td>";
                 RightEmployeesTEMP[j] = RightEmployeesTEMP[j] + "<td class=xl3330747 align='right'>" + FNum(EmployeesTEMPYTD[j],0) + "</td>";
                 RightEmployeesTEMP[j] = RightEmployeesTEMP[j] + "<td class=xl3930747></td>";
                 RightEmployeesTEMP[j] = RightEmployeesTEMP[j] + "</tr>";

         }



}



public void CreateEmployeesTotal(){

         LeftEmployeesTotal = "<tr height=15 style='height:8pt'>";
         LeftEmployeesTotal = LeftEmployeesTotal + "<td height=15 class=xl5330747 style='height:8pt'>Total Number of Employees</td>";
         LeftEmployeesTotal = LeftEmployeesTotal + "<td class=xl4930747></td>";

         //Get YTD Employee Total;
         int j = 0;
         if(selMonth != 1){
                employeesTotalYTD[j] = cStr(SafeDbl(EmployeesTEMPYTD[j]) + SafeDbl(EmployeesIRTYTD[j]));
         }

         for(j=0; j <= weekIndex; j++){ 

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;

                 for( i=0; i < (UBound(actualLoad,1)-1); i++){
                                if(Trim(employeesTEMPTotal[j,i]) != "" || Trim(employeesIRTTotal[j,i]) != ""){
                                        stringBuffer = stringBuffer + "<td class=xl4930747 align=right>" + FNum(cStr(SafeDbl(employeesTEMPTotal[j,i]) + SafeDbl(employeesIRTTotal[j,i])), 0) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl4930747 align=right></td>";
                                }
                                PageEmployeesTotal[j,pageIndex] = PageEmployeesTotal[j,pageIndex] + stringBuffer;
                                employeesTotal[j,i] = FNum(cStr(SafeDbl(employeesTEMPTotal[j,i]) + SafeDbl(employeesIRTTotal[j,i])),0);
                                stringBuffer = "";
                                if((i+1) % pageSize == 0){
                                        pageIndex = pageIndex + 1;
                                }
                 }

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }

                 employeesTotalYTD[j] = cStr(SafeDbl(EmployeesTEMPYTD[j]) + SafeDbl(EmployeesIRTYTD[j]));

                //Fill out remaining columns;
                 leftOverColumns = (i) % pageSize;
                 for(int y=0; y < (leftOverColumns-2); y++){
                        PageEmployeesTotal[j,pageIndex] = PageEmployeesTotal[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                 }

                 RightEmployeesTotal[j] = "<td class=xl5330747></td>";
                 RightEmployeesTotal[j] = RightEmployeesTotal[j] + "<td class=xl4030747 align='right'>" + FNum(cStr(Sum(employeesTotal,j)),0)  + "</td>";
                 RightEmployeesTotal[j] = RightEmployeesTotal[j] + "<td class=xl5030747></td>";
                 RightEmployeesTotal[j] = RightEmployeesTotal[j] + "<td class=xl5030747></td>";
                 RightEmployeesTotal[j] = RightEmployeesTotal[j] + "<td class=xl3030747 align='right'>" + FNum(employeesTotalYTD[j],0) + "</td>";
                 RightEmployeesTotal[j] = RightEmployeesTotal[j] + "<td class=xl5030747></td>";
                 RightEmployeesTotal[j] = RightEmployeesTotal[j] + "</tr>";

         }

}


public void CreateEmployeesUnits(){

         LeftEmployeesUnits = "<tr height=15 style='height:8pt'>";
         LeftEmployeesUnits = LeftEmployeesUnits + "<td height=15 class=xl3430747 style='height:8pt'>Units/Employees</td>";
         LeftEmployeesUnits = LeftEmployeesUnits + "<td class=xl5430747>&nbsp;</td>";

         // Get YTD Employee Units;
         int j = 0;
         if(selMonth != 1){
                 DataReader rsEmployeesUnitsYTD = new DataReader(getEmployeeYTDSQL(arWeeklyDate[j], arWeeklyDate[j],"", "UNITS"));
                 rsEmployeesUnitsYTD.Open();

                 while (! rsEmployeesUnitsYTD.EOF){
                        rsEmployeesUnitsYTD.Read();
                        EmployeesUnitsYTD[j] = cStr(SafeDbl(rsEmployeesUnitsYTD.Fields(0)));

                 }

        }

         for(j=0; j <= weekIndex; j++){

                 DataReader rsEmployeesUnits = new DataReader(getEmployeeSQL(arWeeklyDate[j], cStr(cDate(arWeeklyDate[j]).AddDays(7)) ,"", "UNITS"));
                 rsEmployeesUnits.Open();

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;
                 int intDivider;

                 while (! rsEmployeesUnits.EOF){
                       rsEmployeesUnits.Read();
                        stringBuffer = stringBuffer + "<td class=xl5430747 align=right>" + FNum(rsEmployeesUnits.Fields(0), 2) + "</td>";
                        PageEmployeesUnits[j,pageIndex] = PageEmployeesUnits[j,pageIndex] + stringBuffer;
                        employeesUnitsTotal[j,i] = FNum(rsEmployeesUnits.Fields(0), 2);
                        stringBuffer = "";
                        if((i+1) % pageSize == 0){
                                pageIndex = pageIndex + 1;
                        }
                        i = i + 1;

                 }

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }

                 if((selMonth == 1) && (j == 0)){
                        intDivider = 1;
                 }else{
                        intDivider = 2;
                 }

                 EmployeesUnitsYTD[j] = cStr((SafeDbl(FNum(cStr(Ave(employeesUnitsTotal,j)),2)) + SafeDbl(EmployeesUnitsYTD[lastYTD])) / intDivider);

                //Fill out remaining columns;

             while(i % pageSize != 0 ){
                        PageEmployeesUnits[j,pageIndex] = PageEmployeesUnits[j,pageIndex] + "<td class=xl2830747 >&nbsp;</td>";
                        //intEndofLine = 2;
                        i = i + 1;
             }

                 RightEmployeesUnits[j] = "<td class=xl2830747></td>";
                 RightEmployeesUnits[j] = RightEmployeesUnits[j] + "<td class=xl5530747 align='right'>" + FNum(cStr(Ave(employeesUnitsTotal,j)),2 ) + "</td>";
                 RightEmployeesUnits[j] = RightEmployeesUnits[j] + "<td class=xl3930747></td>";
                 RightEmployeesUnits[j] = RightEmployeesUnits[j] + "<td class=xl3930747></td>";
                 RightEmployeesUnits[j] = RightEmployeesUnits[j] + "<td class=xl5630747 align='right'>" + FNum(EmployeesUnitsYTD[j],2) + "</td>";
                 RightEmployeesUnits[j] = RightEmployeesUnits[j] + "<td class=xl3930747></td>";
                 RightEmployeesUnits[j] = RightEmployeesUnits[j] + "</tr>";

         }



}


public void CreateEmployeesAvgIRT(){

         LeftEmployeesAvgIRT = "<tr height=15 style='height:8pt'>";
         LeftEmployeesAvgIRT = LeftEmployeesAvgIRT + "<td height=15 class=xl3430747 style='height:8pt'>Average Hours Per IRT Employee</td>";
         LeftEmployeesAvgIRT = LeftEmployeesAvgIRT + "<td class=xl5430747>&nbsp;</td>";

         //Get YTD Employee Average IRT;
         j = 0;
         if(selMonth != 1){
                employeesAvgIRTYTD[j] = cStr(SafeDiv(cStr(SafeDbl(payRollIRTHrsYTD[j])), cStr(SafeDbl(EmployeesIRTYTD[j]))));
         }

         for( j=0; j <= weekIndex;j ++){

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;
                 int intDivider = 0;

                 for(i=0; i < (UBound(actualLoad,1)-1); i++){
                        if(Trim(payRollHrsIRT[j,i]) != "" && Trim(employeesIRTTotal[j,i]) != ""){
                                stringBuffer = stringBuffer + "<td class=xl5430747 align=right>" + FNum(cStr(SafeDiv(cStr(SafeDbl(payRollHrsIRT[j,i])), cStr(SafeDbl(employeesIRTTotal[j,i])))),2) + "</td>";
                         }else{
                                stringBuffer = stringBuffer + "<td class=xl5430747 align=right></td>";
                        }
                        PageEmployeesAvgIRT[j,pageIndex] = PageEmployeesAvgIRT[j,pageIndex] + stringBuffer;
                        employeesAvgIRTTotal[j,i] = FNum(cStr(SafeDiv(cStr(SafeDbl(payRollHrsIRT[j,i])), cStr(SafeDbl(employeesIRTTotal[j,i])))),2);
                        stringBuffer = "";
                        if((i+1) % pageSize == 0){
                                pageIndex = pageIndex + 1;
                        }
                 }

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }
                 if ((selMonth == 1) && (j == 0)){
                        intDivider = 1;
                 }else{
                        intDivider = 2;
                 }
                 employeesAvgIRTYTD[j] = cStr((SafeDbl(FNum(cStr(Ave(employeesAvgIRTTotal,j)),2 )) + SafeDbl(employeesAvgIRTYTD[lastYTD])) / intDivider);

                //Fill out remaining columns;
                 leftOverColumns = (i) % pageSize;
                 for(int y=0; y < (leftOverColumns-2);y++){
                        PageEmployeesAvgIRT[j,pageIndex] = PageEmployeesAvgIRT[j,pageIndex] + "<td class=xl5430747 align=right>&nbsp;</td>";
                 }

                 RightEmployeesAvgIRT[j] = "<td class=xl5530747></td>";
                 RightEmployeesAvgIRT[j] = RightEmployeesAvgIRT[j] + "<td class=xl5530747 align='right'>" + FNum(cStr(Ave(employeesAvgIRTTotal,j)),2)  + "</td>";
                 RightEmployeesAvgIRT[j] = RightEmployeesAvgIRT[j] + "<td class=xl3930747></td>";
                 RightEmployeesAvgIRT[j] = RightEmployeesAvgIRT[j] + "<td class=xl3930747></td>";
                 RightEmployeesAvgIRT[j] = RightEmployeesAvgIRT[j] + "<td class=xl5630747 align='right'>" + FNum(employeesAvgIRTYTD[j],2) + "</td>";
                 RightEmployeesAvgIRT[j] = RightEmployeesAvgIRT[j] + "<td class=xl3930747></td>";
                 RightEmployeesAvgIRT[j] = RightEmployeesAvgIRT[j] + "</tr>";

         }

}


public void CreateEmployeesAvgTEMP(){

         LeftEmployeesAvgTEMP = "<tr height=15 style='height:8pt'>";
         LeftEmployeesAvgTEMP = LeftEmployeesAvgTEMP + "<td height=15 class=xl3430747 style='height:8pt'>Average Hours Per TEMP Employee</td>";
         LeftEmployeesAvgTEMP = LeftEmployeesAvgTEMP + "<td class=xl5430747>&nbsp;</td>";

         //Get YTD Employee Average TEMP;
         int j = 0;
         if(selMonth != 1){
                employeesAvgTEMPYTD[j] = FNum(cStr(SafeDiv(cStr(SafeDbl(payRollTEMPHrsYTD[j])), cStr(SafeDbl(EmployeesTEMPYTD[j])))),2);
         }

         for( j=0; j <= weekIndex;j ++){

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;
                 int intDivider = 0;

                 for( i=0; i < (UBound(actualLoad,1)-1);i ++){
                        if(Trim(payRollHrsTEMP[j,i]) != "" && Trim(employeesTEMPTotal[j,i]) != ""){
                                stringBuffer = stringBuffer + "<td class=xl5430747 align=right>" + FNum(cStr(SafeDiv(cStr(SafeDbl(payRollHrsTEMP[j,i])),cStr(SafeDbl(employeesTEMPTotal[j,i])))),2) + "</td>";
                        }else{
                                stringBuffer = stringBuffer + "<td class=xl5430747 align=right></td>";
                        }
                        PageEmployeesAvgTEMP[j,pageIndex] = PageEmployeesAvgTEMP[j,pageIndex] + stringBuffer;
                        employeesAvgTEMPTotal[j,i] = FNum(cStr(SafeDiv(cStr(SafeDbl(payRollHrsTEMP[j,i])), cStr(SafeDbl(employeesTEMPTotal[j,i])))),2);
                        stringBuffer = "";
                        if((i+1) % pageSize == 0){
                                pageIndex = pageIndex + 1;
                        }
                 }

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }

                 if((selMonth == 1) && (j == 0)){
                        intDivider = 1;
                 }else{
                        intDivider = 2;
                 }
                 employeesAvgTEMPYTD[j] = cStr((SafeDbl(FNum(cStr(Ave(employeesAvgTEMPTotal,j)),2 )) + SafeDbl(employeesAvgTEMPYTD[lastYTD])) / intDivider);

                //Fill out remaining columns;
                 leftOverColumns = (i) % pageSize;
                 for(int y=0; y < (leftOverColumns-2);y++){
                        PageEmployeesAvgTEMP[j,pageIndex] = PageEmployeesAvgTEMP[j,pageIndex] + "<td class=xl5430747 align=right>&nbsp;</td>";
                 }

                 RightEmployeesAvgTEMP[j] = "<td class=xl5530747></td>";
                 RightEmployeesAvgTEMP[j] = RightEmployeesAvgTEMP[j] + "<td class=xl5530747 align='right'>" + FNum(cStr(Ave(employeesAvgTEMPTotal,j)),2)  + "</td>";
                 RightEmployeesAvgTEMP[j] = RightEmployeesAvgTEMP[j] + "<td class=xl3930747></td>";
                 RightEmployeesAvgTEMP[j] = RightEmployeesAvgTEMP[j] + "<td class=xl3930747></td>";
                 RightEmployeesAvgTEMP[j] = RightEmployeesAvgTEMP[j] + "<td class=xl5630747 align='right'>" + FNum(employeesAvgTEMPYTD[j],2) + "</td>";
                 RightEmployeesAvgTEMP[j] = RightEmployeesAvgTEMP[j] + "<td class=xl3930747></td>";
                 RightEmployeesAvgTEMP[j] = RightEmployeesAvgTEMP[j] + "</tr>";

         }


}


public void CreateMiscLaborRep(){

         LeftMiscLaborRep = "<tr height=15 style='height:8pt'>";
         LeftMiscLaborRep = LeftMiscLaborRep + "<td height=15 class=xl3430747 style='height:8pt'><b>Reported Misc. Labor</b></td>";
         LeftMiscLaborRep = LeftMiscLaborRep + "<td class=xl5430747>&nbsp;</td>";


         // Get YTD Reported Misc. Labor;
         int j = 0;
         if (selMonth != 1){

                 DataReader rsMiscLaborRepYTD = new DataReader(getUnitsHoursYTDSQL(arWeeklyDate[j], arWeeklyDate[j],"MISC", "PAY"));
                 rsMiscLaborRepYTD.Open();

                 while (! rsMiscLaborRepYTD.EOF){
                        rsMiscLaborRepYTD.Read();
                        MiscLaborRepYTD[j] = rsMiscLaborRepYTD.Fields(0);

                 }

        }
         originalMiscLaborRepYTD = cStr(SafeDbl(MiscLaborRepYTD[j]));

         for(j=0; j <= weekIndex;  j++){

                 DataReader rsMiscLaborRep = new DataReader(getUnitsHourstrSQL(arWeeklyDate[j], cStr(cDate(arWeeklyDate[j]).AddDays(7)) ,"MISC", "PAY"));
                 rsMiscLaborRep.Open();

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;

                 while (! rsMiscLaborRep.EOF){
                       rsMiscLaborRep.Read();

                                if (Trim(rsMiscLaborRep.Fields(0)) != ""){
                                        stringBuffer = stringBuffer + "<td class=xl5430747 align=right>" + FCur(rsMiscLaborRep.Fields(0), 0) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl5430747 align=right></td>";
                                }

                                PageMiscLaborRep[j,pageIndex] = PageMiscLaborRep[j,pageIndex] + stringBuffer;
                                MiscLaborRepTotal[j,i] = FNum(rsMiscLaborRep.Fields(0), 0);
                                stringBuffer = "";
                                if ( ((i+1) % pageSize) == 0 ){
                                        pageIndex = pageIndex + 1;
                                }
                                i = i + 1;

                 }

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }
                 MiscLaborRepYTD[j] = cStr(SafeDbl(MiscLaborRepYTD[lastYTD]) + SafeDbl(FNum(cStr(Sum(MiscLaborRepTotal,j)),0)));

                    //Fill out remaining columns;
                 while (i % pageSize != 0){
                            PageMiscLaborRep[j,pageIndex] = PageMiscLaborRep[j,pageIndex] + "<td class=xl2830747 >&nbsp;</td>";
                            //intEndofLine = 2;
                            i = i + 1;
                 }

                 RightMiscLaborRep[j] = "<td class=xl2830747></td>";
                 RightMiscLaborRep[j] = RightMiscLaborRep[j] + "<td class=xl5530747 align='right'>" + FCur(cStr(Sum(MiscLaborRepTotal,j)),0) + "</td>";
                 RightMiscLaborRep[j] = RightMiscLaborRep[j] + "<td class=xl3930747></td>";
                 RightMiscLaborRep[j] = RightMiscLaborRep[j] + "<td class=xl3930747></td>";
                 RightMiscLaborRep[j] = RightMiscLaborRep[j] + "<td class=xl5630747 align='right'>" + FNum(MiscLaborRepYTD[j],0) + "</td>";
                 RightMiscLaborRep[j] = RightMiscLaborRep[j] + "<td class=xl3930747></td>";
                 RightMiscLaborRep[j] = RightMiscLaborRep[j] + "</tr>";

         }



}


public void CreateMiscLaborBud(){

         LeftMiscLaborBud = "<tr height=15 style='height:8pt'>";
         LeftMiscLaborBud = LeftMiscLaborBud + "<td height=15 class=xl3430747 style='height:8pt'>Budgeted Misc. Labor</td>";
         LeftMiscLaborBud = LeftMiscLaborBud + "<td class=xl3730747>&nbsp;</td>";

         // Get Misc. Budgeted Labor;

         int j = 0;
         if (selMonth != 1){

                 DataReader rsMonthlyBudPerc = new DataReader(getMiscLaborBudYTDSQL(arWeeklyDate[0], arWeeklyDate[j],"ALL", "UNITS",cStr(selMonth)));
                 rsMonthlyBudPerc.Open();

                 i = 1;
                 while (! rsMonthlyBudPerc.EOF){
                     rsMonthlyBudPerc.Read();
                     monthlyBudPerc[i] = FNum(cStr(SafeDbl(rsMonthlyBudPerc.Fields(0))),0);
                     i = i + 1;
                 }
 

                 for( j=0; j <UBound(monthlyBudPerc);j++){
                        MiscLaborBudYTD[0] = cStr(SafeDbl(MiscLaborBudYTD[0]) + SafeDbl(monthlyBudPerc[j]));
                 }
        }
         originalMiscLaborBudYTD = cStr(SafeDbl(MiscLaborBudYTD[0]));

         for( j=0; j <= weekIndex;  j++){

                 DataReader rsMiscLaborBud = new DataReader(getBudgetCPUSQL());
                 rsMiscLaborBud.Open();

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;

                 while (! rsMiscLaborBud.EOF){
                        rsMiscLaborBud.Read();
                        if (Trim(rsMiscLaborBud.Fields(0)) != ""){
                                stringBuffer = stringBuffer + "<td class=xl5430747 align=right>" + FCur(cStr(SafeDbl(rsMiscLaborBud.Fields(1)) * SafeDbl(totalVolume[j,i])),0) + "</td>";
                        }else{
                                stringBuffer = stringBuffer + "<td class=xl5430747 align=right></td>";
                        }
                        PageMiscLaborBud[j,pageIndex] = PageMiscLaborBud[j,pageIndex] + stringBuffer;
                        MiscLaborBudTotal[j,i] = FNum(cStr(SafeDbl(cStr(rsMiscLaborBud.Fields(1))) * cDbl(totalVolume[j,i])),0);
                        stringBuffer = "";
                        if ( ((i+1) % pageSize) == 0 ){
                                pageIndex = pageIndex + 1;
                       }
                        i = i + 1;

                 }

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }
                 MiscLaborBudYTD[j] = cStr(SafeDbl(MiscLaborBudYTD[lastYTD]) + SafeDbl(FNum(cStr(Sum(MiscLaborBudTotal,j)),0)));

                //Fill out remaining columns;

             while (i % pageSize != 0){
                        PageMiscLaborBud[j,pageIndex] = PageMiscLaborBud[j,pageIndex] + "<td class=xl2830747 >&nbsp;</td>";
                        //intEndofLine = 2;
                        i = i + 1;
             }

                 RightMiscLaborBud[j] = "<td class=xl2830747></td>";
                 RightMiscLaborBud[j] = RightMiscLaborBud[j] + "<td class=xl3730747 align='right'>" + FCur(cStr(Sum(MiscLaborBudTotal,j)),0 ) + "</td>";
                 RightMiscLaborBud[j] = RightMiscLaborBud[j] + "<td class=xl2830747></td>";
                 RightMiscLaborBud[j] = RightMiscLaborBud[j] + "<td class=xl2830747></td>";
                 RightMiscLaborBud[j] = RightMiscLaborBud[j] + "<td class=xl3730747 align='right'>" + FCur(cStr(SafeDbl(MiscLaborBudYTD[j])),0) + "</td>";
                 RightMiscLaborBud[j] = RightMiscLaborBud[j] + "<td class=xl2830747></td>";
                 RightMiscLaborBud[j] = RightMiscLaborBud[j] + "</tr>";

         }



}


public void CreateDiffBudPayRoll(){

         LeftDiffBudPayRoll = "<tr height=15 style='height:8pt'>";
         LeftDiffBudPayRoll = LeftDiffBudPayRoll + "<td height=15 class=xl3430747 style='height:8pt'>Diff. In Budgeted Payroll</td>";
         LeftDiffBudPayRoll = LeftDiffBudPayRoll + "<td class=xl5430747>&nbsp;</td>";

         //Get YTD DiffBudPayRoll;
         if (selMonth != 1){
                j = 0;
                DiffBudPayRollYTD[j] = cStr(SafeDbl(CPUVarianceYTD[j]) * SafeDbl(originalTotalVolumeYTD));
                originalDiffBudPayRollYTD = DiffBudPayRollYTD[j];
        }

         for( int j=0; j <= weekIndex;  j++){

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;

                 for (i=0; i <  (UBound(actualLoad,1)-1);i++){
                        if (Trim(totalVolume[j,i]) != "" && i < facilityCount){
                                stringBuffer = stringBuffer + "<td class=xl5430747 align=right>" + FCur(cStr(SafeDbl(cStr(cDbl(totalVolume[j,i]) * cDbl(CPUVarianceTotal[j,i])))),0) + "</td>";
                        }else{
                                stringBuffer = stringBuffer + "<td class=xl5430747 align=right></td>";
                        }
                        PageDiffBudPayRoll[j,pageIndex] = PageDiffBudPayRoll[j,pageIndex] + stringBuffer;
                        stringBuffer = "";
                        if ( ((i+1) % pageSize) == 0 ){
                                pageIndex = pageIndex + 1;
                        }
                 }

                 DiffBudPayRollTotal[j] = FNum(cStr(SafeDbl(DiffCPUVarianceTotal[j]) * SafeDbl(cStr(Sum(totalVolume, j)))),0);

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }
                 DiffBudPayRollYTD[j] = cStr(SafeDbl(DiffBudPayRollYTD[lastYTD]) + SafeDbl(DiffBudPayRollTotal[j]));

                //Fill out remaining columns;
                 leftOverColumns = (i) % pageSize;
                 for( int y=0; y < (leftOverColumns-2);y++){
                        PageDiffBudPayRoll[j,pageIndex] = PageDiffBudPayRoll[j,pageIndex] + "<td class=xl5430747 align=right>&nbsp;</td>";
                 }

                 RightDiffBudPayRoll[j] = "<td class=xl5530747></td>";
                 RightDiffBudPayRoll[j] = RightDiffBudPayRoll[j] + "<td class=xl5530747 align='right'>" + FCur(cStr(SafeDbl(DiffBudPayRollTotal[j])),0)  + "</td>";
                 RightDiffBudPayRoll[j] = RightDiffBudPayRoll[j] + "<td class=xl3930747></td>";
                 RightDiffBudPayRoll[j] = RightDiffBudPayRoll[j] + "<td class=xl3930747></td>";
                 RightDiffBudPayRoll[j] = RightDiffBudPayRoll[j] + "<td class=xl5630747 align='right'>" + FCur(DiffBudPayRollYTD[j],0) + "</td>";
                 RightDiffBudPayRoll[j] = RightDiffBudPayRoll[j] + "<td class=xl3930747></td>";
                 RightDiffBudPayRoll[j] = RightDiffBudPayRoll[j] + "</tr>";

         }

}


public void CreateDiffMiscBudPayRoll(){

         LeftDiffMiscBudPayRoll = "<tr height=15 style='height:8pt'>";
         LeftDiffMiscBudPayRoll = LeftDiffMiscBudPayRoll + "<td height=15 class=xl3430747 style='height:8pt'>Diff. In Budgeted Misc Payroll</td>";
         LeftDiffMiscBudPayRoll = LeftDiffMiscBudPayRoll + "<td class=xl5430747>&nbsp;</td>";

         //Get YTD DiffMiscBudPayRoll;
         if (selMonth != 1){
                j = 0;
                DiffMiscBudPayRollYTD[j] = cStr(SafeDbl(originalMiscLaborRepYTD) - SafeDbl(originalMiscLaborBudYTD));
                originalDiffMiscBudPayRollYTD = DiffMiscBudPayRollYTD[j];
         }

         for( int j=0; j <= weekIndex;  j++){

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;

                 for (i=0; i <  (UBound(actualLoad,1)-1);i++){
                        if (Trim(MiscLaborRepTotal[j,i]) != "" && Trim(MiscLaborBudTotal[j,i]) != ""){
                                stringBuffer = stringBuffer + "<td class=xl5430747 align=right>" + FCur(cStr(SafeDbl(MiscLaborRepTotal[j,i]) - SafeDbl(MiscLaborBudTotal[j,i])),0) + "</td>";
                        }else{
                                stringBuffer = stringBuffer + "<td class=xl5430747 align=right></td>";
                        }
                        PageDiffMiscBudPayRoll[j,pageIndex] = PageDiffMiscBudPayRoll[j,pageIndex] + stringBuffer;
                        stringBuffer = "";
                        if ( ((i+1) % pageSize) == 0 ){
                                pageIndex = pageIndex + 1;
                        }
                 }

                 DiffMiscBudPayRollTotal[j] = FNum(cStr(SafeDbl(cStr(Sum(MiscLaborRepTotal,j))) - SafeDbl(cStr(Sum(MiscLaborBudTotal,j)))),0);

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }
                 DiffMiscBudPayRollYTD[j] = cStr(SafeDbl(DiffMiscBudPayRollYTD[lastYTD]) + SafeDbl(FNum(DiffMiscBudPayRollTotal[j],0)));

                //Fill out remaining columns;
                 leftOverColumns = (i) % pageSize;
                 for( int y=0; y < (leftOverColumns-2);y++){
                        PageDiffMiscBudPayRoll[j,pageIndex] = PageDiffMiscBudPayRoll[j,pageIndex] + "<td class=xl5430747 align=right>&nbsp;</td>";
                 }

                 RightDiffMiscBudPayRoll[j] = "<td class=xl5530747></td>";
                 RightDiffMiscBudPayRoll[j] = RightDiffMiscBudPayRoll[j] + "<td class=xl5530747 align='right'>" + FCur(DiffMiscBudPayRollTotal[j],0)  + "</td>";
                 RightDiffMiscBudPayRoll[j] = RightDiffMiscBudPayRoll[j] + "<td class=xl3930747></td>";
                 RightDiffMiscBudPayRoll[j] = RightDiffMiscBudPayRoll[j] + "<td class=xl3930747></td>";
                 RightDiffMiscBudPayRoll[j] = RightDiffMiscBudPayRoll[j] + "<td class=xl5630747 align='right'>" +  FCur(DiffMiscBudPayRollYTD[j],0) + " </td>";
                 RightDiffMiscBudPayRoll[j] = RightDiffMiscBudPayRoll[j] + "<td class=xl3930747></td>";
                 RightDiffMiscBudPayRoll[j] = RightDiffMiscBudPayRoll[j] + "</tr>";

         }


}


public void CreateCPU(){

         LeftCPU = "<tr height=15 style='height:8pt'>";
         LeftCPU = LeftCPU + "<td height=15 class=xl2730747 style='height:8pt'><b>CPU</b></td>";
         LeftCPU = LeftCPU + "<td class=xl5730747>&nbsp;</td>";


         // Get YTD CPU;
         if (selMonth != 1){

                 int j = 0;
                 DataReader rsPay = new DataReader(getUnitsHoursYTDSQL(arWeeklyDate[j], arWeeklyDate[j],"ALL", "PAY"));
                 rsPay.Open();

                 DataReader rsUnits = new DataReader(getUnitsHoursYTDSQL(arWeeklyDate[j], arWeeklyDate[j],"ALL", "UNITS"));
                 rsUnits.Open();

                 while (! rsPay.EOF && !rsUnits.EOF){

                        rsPay.Read();
                        rsUnits.Read();
                        CPUYTD[j] = cStr(SafeDbl(FNum(cStr(SafeDiv(cStr(SafeDbl(rsPay.Fields(0))), cStr(SafeDbl(rsUnits.Fields(0))))),2)));

                 }

        }

         for(j=0; j <= weekIndex;  j++){

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0; 
                 int intDivider = 0;

                 for (i=0; i < (UBound(actualLoad,1)); i++){

                                if((Trim(actualLoad[j,i]) != "" || Trim(actualUnLoad[j,i]) != "" || Trim(actualSpotting[j,i]) != "") && (i < facilityCount)){
                                        stringBuffer = stringBuffer + "<td class=xl5730747 align=right>" + FCur(SafeDiv(FNum(cStr(SafeDbl(payRollTEMP[j,i]) + SafeDbl(payRollIRT[j,i])), 0), FNum(cStr(SafeDbl(actualLoad[j,i]) + SafeDbl(actualUnLoad[j,i]) + SafeDbl(actualSpotting[j,i])), 0)).ToString("F"),2) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl5730747 align=right></td>";
                                }
                                PageCPU[j,pageIndex] = PageCPU[j,pageIndex] + stringBuffer;
                                CPUTotal[j,i] = FNum(SafeDiv(FNum(cStr(SafeDbl(payRollTEMP[j,i]) + SafeDbl(payRollIRT[j,i])), 0), FNum(cStr(SafeDbl(actualLoad[j,i]) + SafeDbl(actualUnLoad[j,i]) + SafeDbl(actualSpotting[j,i])), 0)).ToString("F"),2);
                                stringBuffer = "";
                                
								if ( ((i+1) % pageSize) == 0 ){
                                        pageIndex = pageIndex + 1;
                                }
                 }

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }
                 if (selMonth == 1 && j == 0){
                        intDivider = 1;
                 }else{
                        intDivider = 2;
                 }
                 CPUYTD[j] = cStr((SafeDbl(FNum(cStr(Ave(CPUTotal,j)),2)) + SafeDbl(CPUYTD[lastYTD])) / intDivider);

                //Fill out remaining columns;
                 leftOverColumns = (i) % pageSize;
                 for( int y=0; y < (leftOverColumns-2);y++){
                        PageCPU[j,pageIndex] = PageCPU[j,pageIndex] + "<td class=xl2830747 >&nbsp;</td>";
                 }


                 RightCPU[j] = "<td class=xl2830747></td>";
                 RightCPU[j] = RightCPU[j] + "<td class=xl5730747 align='right'>" + FCur(cStr(Ave(CPUTotal,j)),2) + "</td>";
                 RightCPU[j] = RightCPU[j] + "<td class=xl2830747></td>";
                 RightCPU[j] = RightCPU[j] + "<td class=xl2830747></td>";
                 RightCPU[j] = RightCPU[j] + "<td class=xl5730747 align='right'>" + FCur(CPUYTD[j],2) + "</td>";
                 RightCPU[j] = RightCPU[j] + "<td class=xl2830747></td>";
                 RightCPU[j] = RightCPU[j] + "</tr>";

         }




}


public void CreateBudgetedCPU(){

         LeftBudgetedCPU = "<tr height=15 style='height:8pt'>";
         LeftBudgetedCPU = LeftBudgetedCPU + "<td height=15 class=xl2830747 style='height:8pt'>Budgeted CPU</td>";
         LeftBudgetedCPU = LeftBudgetedCPU + "<td class=xl5830747>&nbsp;</td>";

         //Get CPU;

         for( int j=0; j <= weekIndex;  j++){

                 DataReader rsBudgetedCPU = new DataReader(getBudgetCPUSQL());
                 rsBudgetedCPU.Open();

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;

                 while (! rsBudgetedCPU.EOF){
                        rsBudgetedCPU.Read();

                        if (Trim(rsBudgetedCPU.Fields(0)) != ""){
                                stringBuffer = stringBuffer + "<td class=xl5830747 align=right>" + FCur(rsBudgetedCPU.Fields(0),2) + "</td>";
                        }else{
                                stringBuffer = stringBuffer + "<td class=xl5830747 align=right></td>";
                        }
                        PageBudgetedCPU[j,pageIndex] = PageBudgetedCPU[j,pageIndex] + stringBuffer;
                        BudgetedCPUTotal[j,i] = FNum(rsBudgetedCPU.Fields(0), 2);
                        stringBuffer = "";
								
                        if (((i+1) % pageSize) == 0 ){
                                pageIndex = pageIndex + 1;
                        }
                        i = i + 1;

                 }

                 BudgetedCPUYTD[j] = cStr(SafeDbl(FNum(cStr(Ave(BudgetedCPUTotal,j)),2)));

                //Fill out remaining columns;

                 while (i % pageSize != 0 ){
                            PageBudgetedCPU[j,pageIndex] = PageBudgetedCPU[j,pageIndex] + "<td class=xl2830747 >&nbsp;</td>";
                            i = i + 1;
                 }

                 RightBudgetedCPU[j] = "<td class=xl2830747></td>";
                 RightBudgetedCPU[j] = RightBudgetedCPU[j] + "<td class=xl5830747 align='right'>" + FCur(cStr(Ave(BudgetedCPUTotal,j)),2) + "</td>";
                 RightBudgetedCPU[j] = RightBudgetedCPU[j] + "<td class=xl2830747></td>";
                 RightBudgetedCPU[j] = RightBudgetedCPU[j] + "<td class=xl2830747>(average)</td>";
                 RightBudgetedCPU[j] = RightBudgetedCPU[j] + "<td class=xl5830747 align='right'>" + FCur(BudgetedCPUYTD[j],2) + "</td>";
                 RightBudgetedCPU[j] = RightBudgetedCPU[j] + "<td class=xl2830747></td>";
                 RightBudgetedCPU[j] = RightBudgetedCPU[j] + "</tr>";
             
         }



}


public void CreateCPUVariance(){

         LeftCPUVariance = "<tr height=15 style='height:8pt'>";
         LeftCPUVariance = LeftCPUVariance + "<td height=15 class=xl4530747 style='height:8pt'>Variance</td>";
         LeftCPUVariance = LeftCPUVariance + "<td class=xl5930747></td>";

         for( int j=0; j <= weekIndex;  j++){
                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;

                 for (i=0; i <  (UBound(actualLoad,1)-1); i++){
                        if((Trim(CPUTotal[j,i]) != "" || Trim(BudgetedCPUTotal[j,i]) != "") && (i < facilityCount)){
                                stringBuffer = stringBuffer + "<td class=xl5930747 align=right>" + FCur(cStr(SafeDbl(CPUTotal[j,i]) - SafeDbl(BudgetedCPUTotal[j,i])), 2) + "</td>";
                        }else{
                                stringBuffer = stringBuffer + "<td class=xl5930747 align=right></td>";
                        }
                        PageCPUVariance[j,pageIndex] = PageCPUVariance[j,pageIndex] + stringBuffer;
								
						CPUVarianceTotal[j,i] = FNum(cStr(SafeDbl(CPUTotal[j,i]) - SafeDbl(BudgetedCPUTotal[j,i])), 2);
                        stringBuffer = "";
								
                        if (((i+1) % pageSize) == 0 ){
                                pageIndex = pageIndex + 1;
                        }
                 }

                 DiffCPUVarianceTotal[j] = FNum(cStr(cDbl(FNum(cStr(Ave(CPUTotal,j)),2)) - cDbl(FNum(cStr(Ave(BudgetedCPUTotal,j)),2))),2);

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                 }
                 CPUVarianceYTD[j] = cStr(SafeDbl(FNum(CPUYTD[j],2)) - SafeDbl(FNum(BudgetedCPUYTD[j],2)));

                //Fill out remaining columns;
                 leftOverColumns = (i) % pageSize;
                 for( int y=0; y < (leftOverColumns-2);y++){ 
                        PageCPUVariance[j,pageIndex] = PageCPUVariance[j,pageIndex] + "<td class=xl4730747 align=right>&nbsp;</td>";
                 }

                 RightCPUVariance[j] = "<td class=xl4730747></td>";
                 RightCPUVariance[j] = RightCPUVariance[j] + "<td class=xl5930747>" + FCur(DiffCPUVarianceTotal[j] ,2)  + "</td>";
                 RightCPUVariance[j] = RightCPUVariance[j] + "<td class=xl4730747></td>";
                 RightCPUVariance[j] = RightCPUVariance[j] + "<td class=xl4730747></td>";
                 RightCPUVariance[j] = RightCPUVariance[j] + "<td class=xl5930747 align='right'>" + FCur(CPUVarianceYTD[j],2) + "</td>";
                 RightCPUVariance[j] = RightCPUVariance[j] + "<td class=xl4730747></td>";
                 RightCPUVariance[j] = RightCPUVariance[j] + "</tr>";

         }

}


public void CreateDiffFromBudget(){

         LeftDiffFromBudget = "<tr height=15 style='height:8pt'>";
         LeftDiffFromBudget = LeftDiffFromBudget + "<td height=15 class=xl6030747 style='height:8pt'>Diff from Budget(!related to Misc Labor)</td>";
         LeftDiffFromBudget = LeftDiffFromBudget + "<td class=xl6130747>&nbsp;</td>";


         // Get Diff. from Budget(!related to Misc. Labor);
         int j = 0;
         if (selMonth != 1){
                DiffFromBudgetYTD[j] = cStr(SafeDiv(cStr(SafeDbl(originalDiffBudPayRollYTD) - SafeDbl(originalDiffMiscBudPayRollYTD)), cStr(SafeDbl(originalTotalVolumeYTD))));
         }

         for( j=0; j <= weekIndex;  j++){
                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;
                 int intDivider = 0;

                 for (i=0; i <  (UBound(actualLoad,1)-1); i++){
                                if ((Trim(totalVolume[j,i]) != "" && (i < facilityCount))){
                                        stringBuffer = stringBuffer + "<td class=xl5930747 align=right>" + FCur(cStr(SafeDiv(cStr(SafeDbl(cStr(cDbl(FNum(cStr(SafeDbl(totalVolume[j,i]) * SafeDbl(CPUVarianceTotal[j,i])),0)) - cDbl(FNum(cStr(SafeDbl(MiscLaborRepTotal[j,i]) - SafeDbl(MiscLaborBudTotal[j,i])),0))))), cStr(SafeDbl(totalVolume[j,i])))), 2) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl5930747 align=right></td>";
                               }
                                PageDiffFromBudget[j,pageIndex] = PageDiffFromBudget[j,pageIndex] + stringBuffer;
                                stringBuffer = "";
                                if ( ((i+1) % pageSize) == 0 ){
                                        pageIndex = pageIndex + 1;
                               }
                 }

                 DiffFromBudgetTotal[j] = FNum(cStr(SafeDiv(cStr(SafeDbl(DiffBudPayRollTotal[j]) - SafeDbl(DiffMiscBudPayRollTotal[j])), cStr(SafeDbl(cStr(Sum(totalVolume,j)))))), 2);

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                }
                 if (selMonth == 1 && j == 0){
                        intDivider = 1;
                 }else{
                        intDivider = 2;
                }
                 DiffFromBudgetYTD[j] = cStr((SafeDbl(FNum(DiffFromBudgetTotal[j] ,2)) + SafeDbl(DiffFromBudgetYTD[lastYTD])) / intDivider);

                //Fill out remaining columns;
                 leftOverColumns = (i) % pageSize;
                 for( int y=0; y < (leftOverColumns-2); y++){
                        PageDiffFromBudget[j,pageIndex] = PageDiffFromBudget[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                 }

                 RightDiffFromBudget[j] = "<td class=xl4730747></td>";
                 RightDiffFromBudget[j] = RightDiffFromBudget[j] + "<td class=xl6130747 align='right'>" + FCur(cStr(SafeDbl(DiffFromBudgetTotal[j])),2) + "</td>";
                 RightDiffFromBudget[j] = RightDiffFromBudget[j] + "<td class=xl4730747></td>";
                 RightDiffFromBudget[j] = RightDiffFromBudget[j] + "<td class=xl4730747></td>";
                 RightDiffFromBudget[j] = RightDiffFromBudget[j] + "<td class=xl6130747 align='right'>" + FCur(DiffFromBudgetYTD[j],2) + "</td>";
                 RightDiffFromBudget[j] = RightDiffFromBudget[j] + "<td class=xl4730747></td>";
                 RightDiffFromBudget[j] = RightDiffFromBudget[j] + "</tr>";

         }

}


public void CreateOvertimeCost(){

         LeftOvertimeCost = "<tr height=15 style='height:8pt'>";
         LeftOvertimeCost = LeftOvertimeCost + "<td height=15 class=xl6030747 style='height:8pt'>Overtime Cost per unit</td>";
         LeftOvertimeCost = LeftOvertimeCost + "<td class=xl6130747>&nbsp;</td>";


         // Get YTD Overtime Cost;
         j = 0;
         if (selMonth != 1){
                OvertimeCostYTD[j] = cStr(SafeDiv(cStr(SafeDbl(originalOverTimeYTD)), cStr(SafeDbl(originalTotalVolumeYTD))));
         }

         for( j=0; j <= weekIndex;  j++){
                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;
                 int intDivider = 0;

                 for (i=0; i <  (UBound(actualLoad,1)-1); i++){
                                if (Trim(totalVolume[j,i]) != "" && (i < facilityCount)){
                                        stringBuffer = stringBuffer + "<td class=xl5930747 align=right>" + FCur(SafeDiv(cStr(SafeDbl(overTimeTotal[j,i])), cStr(SafeDbl(totalVolume[j,i]))).ToString("F"), 2) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl5930747 align=right></td>";
                               }
                                PageOvertimeCost[j,pageIndex] = PageOvertimeCost[j,pageIndex] + stringBuffer;
                                OvertimeCostTotal[j,i] = FNum(SafeDiv(cStr(SafeDbl(overTimeTotal[j,i])), cStr(SafeDbl(totalVolume[j,i]))).ToString("F"), 2);
                                stringBuffer = "";
                                if ( ((i+1) % pageSize) == 0 ){
                                        pageIndex = pageIndex + 1;
                               }
                 }

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                }
                 if (selMonth == 1 && j == 0){
                        intDivider = 1;
                 }else{
                        intDivider = 2;
                }
                 OvertimeCostYTD[j] = FNum(cStr(SafeDiv(cStr(SafeDbl(OverTimeYTD[j])), cStr(SafeDbl(totalVolumeYTD[j])))),2);

                //Fill out remaining columns;
                 leftOverColumns = (i) % pageSize;
                 for( int y=0; y < (leftOverColumns-2);y++){
                        PageOvertimeCost[j,pageIndex] = PageOvertimeCost[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                 }

                 RightOvertimeCost[j] = "<td class=xl4730747></td>";
                 RightOvertimeCost[j] = RightOvertimeCost[j] + "<td class=xl6130747 align='right'>" + FCur(SafeDiv(cStr(SafeDbl(cStr(Sum(overTimeTotal,j)))), cStr(SafeDbl(cStr(Sum(totalVolume,j))))).ToString("F"),2) + "</td>";
                 RightOvertimeCost[j] = RightOvertimeCost[j] + "<td class=xl4730747></td>";
                 RightOvertimeCost[j] = RightOvertimeCost[j] + "<td class=xl4730747></td>";
                 RightOvertimeCost[j] = RightOvertimeCost[j] + "<td class=xl6130747 align='right'>" + FCur(OvertimeCostYTD[j],2) + "</td>";
                 RightOvertimeCost[j] = RightOvertimeCost[j] + "<td class=xl4730747></td>";
                 RightOvertimeCost[j] = RightOvertimeCost[j] + "</tr>";

         }

}


public void CreateHoliday(){

         LeftHoliday = "<tr height=15 style='height:8pt'>";
         LeftHoliday = LeftHoliday + "<td height=15 class=xl2830747 style='height:8pt' align='right'><font color='red'>Holiday</font></td>";
         LeftHoliday = LeftHoliday + "<td class=xl5830747>&nbsp;</td>";

         //Get CPU;

         for( int j=0; j <= weekIndex;  j++){

                 DataReader rsHoliday = new DataReader(getHolidaySQL(arWeeklyDate[j], cStr(cDate(arWeeklyDate[j]).AddDays(7))));
                 rsHoliday.Open();

                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;

                 while (! rsHoliday.EOF){
                        rsHoliday.Read();
                        if (Trim(rsHoliday.Fields(0)) != ""){
                                stringBuffer = stringBuffer + "<td class=xl5830747 align=right><font color='red'>" + FCur(SafeDiv(cStr(SafeDbl(rsHoliday.Fields(1))), FNum(cStr(SafeDbl(actualLoad[j,i]) + SafeDbl(actualUnLoad[j,i])), 0)).ToString("F"),2) + "</font></td>";
                        }else{
                                stringBuffer = stringBuffer + "<td class=xl5830747 align=right></td>";
                        }
                        PageHoliday[j,pageIndex] = PageHoliday[j,pageIndex] + stringBuffer;
                        HolidayTotal[j,i] = FCur(SafeDiv(cStr(SafeDbl(rsHoliday.Fields(1))), FNum(cStr(SafeDbl(actualLoad[j,i]) + SafeDbl(actualUnLoad[j,i])), 0)).ToString("F"),2);
                        stringBuffer = "";
                        if ( ((i+1) % pageSize) == 0 ){
                                pageIndex = pageIndex + 1;
                        }
                        i = i + 1;

                 }
 
                //Fill out remaining columns;

             while (i % pageSize != 0 ){
                        PageHoliday[j,pageIndex] = PageHoliday[j,pageIndex] + "<td class=xl2830747 >&nbsp;</td>";
                        //intEndofLine = 2;
                        i = i + 1;
             }

                 RightHoliday[j] = "<td class=xl2830747></td>";
                 RightHoliday[j] = RightHoliday[j] + "<td class=xl5830747 align='right'><font color='red'>" + FCur(cStr(Ave(HolidayTotal,j)),2) + "</font></td>";
                 RightHoliday[j] = RightHoliday[j] + "<td class=xl2830747></td>";
                 RightHoliday[j] = RightHoliday[j] + "<td class=xl2830747></td>";
                 RightHoliday[j] = RightHoliday[j] + "<td class=xl5830747 align='right'></td>";
                 RightHoliday[j] = RightHoliday[j] + "<td class=xl2830747></td>";
                 RightHoliday[j] = RightHoliday[j] + "</tr>";

         }



}


public void CreateAvgHourlyPay(){

         LeftAvgHourlyPay = "<tr height=15 style='height:8pt'>";
         LeftAvgHourlyPay = LeftAvgHourlyPay + "<td height=15 class=xl4330747 style='height:8pt'><b>Average Hourly Pay</b></td>";
         LeftAvgHourlyPay = LeftAvgHourlyPay + "<td class=xl5730747>&nbsp;</td>";

         // Get YTD Average Hourly Pay;
         j = 0;
         if (selMonth != 1){
                AvgHourlyPayYTD[j] = cStr(SafeDiv(cStr(SafeDbl(originalPayRollTotalYTD)), cStr(SafeDbl(originalPayRollTotalHrsYTD))));
         }

         for( j=0; j <= weekIndex;  j++){
                 stringBuffer = "";
                 pageIndex = 0;
                 int leftOverColumns = 0;
                 int i = 0;
                 int lastYTD = 0;
                 int intDivider = 0;

                 for (i=0; i <  (UBound(actualLoad,1)-1); i++){
                        if (Trim(payRollTotal[j,i]) != "" && Trim(payRollHrsTotal[j,i]) != "" && i < facilityCount){
                                stringBuffer = stringBuffer + "<td class=xl5730747 align=right>" + FCur(cStr(SafeDiv(cStr(SafeDbl(payRollTotal[j,i])), cStr(SafeDbl(payRollHrsTotal[j,i])))),2) + "</td>";
                        }else{
                                stringBuffer = stringBuffer + "<td class=xl5730747 align=right>&nbsp;</td>";
                        }
                        PageAvgHourlyPay[j,pageIndex] = PageAvgHourlyPay[j,pageIndex] + stringBuffer;
                        AvgHourlyPayTotal[j,i] = FNum(cStr(SafeDiv(payRollTotal[j,i],payRollHrsTotal[j,i])), 2);
                        stringBuffer = "";
                        if ( ((i+1) % pageSize) == 0 ){
                                pageIndex = pageIndex + 1;
                       }
                 }

                 if ( j == 0){
                        lastYTD = 0;
                 }else{
                        lastYTD = j-1;
                }
                 if (selMonth == 1 && j == 0){
                        intDivider = 1;
                 }else{
                        intDivider = 2;
                }
                 AvgHourlyPayYTD[j] = ((SafeDiv(cStr(SafeDbl(cStr(Sum(payRollTotal, j)))), cStr(SafeDbl(cStr(Sum(payRollHrsTotal, j))))) + SafeDbl(AvgHourlyPayYTD[lastYTD])) / intDivider).ToString("F");

                //Fill out remaining columns;
                 leftOverColumns = (i) % pageSize;
                 for( int y=0; y < (leftOverColumns-2);y++){ 
                        PageAvgHourlyPay[j,pageIndex] = PageAvgHourlyPay[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                 }

                 RightAvgHourlyPay[j] = "<td class=xl2830747></td>";
                 RightAvgHourlyPay[j] = RightAvgHourlyPay[j] + "<td class=xl5730747 align='right'>" + FCur(cStr(SafeDiv(cStr(SafeDbl(cStr(Sum(payRollTotal,j)))), cStr(SafeDbl(cStr(Sum(payRollHrsTotal,j)))))),2)  + "</td>";
                 RightAvgHourlyPay[j] = RightAvgHourlyPay[j] + "<td class=xl2830747></td>";
                 RightAvgHourlyPay[j] = RightAvgHourlyPay[j] + "<td class=xl2830747></td>";
                 RightAvgHourlyPay[j] = RightAvgHourlyPay[j] + "<td class=xl5730747 align='right'>" + FCur(AvgHourlyPayYTD[j],2) + "</td>";
                 RightAvgHourlyPay[j] = RightAvgHourlyPay[j] + "<td class=xl2830747></td>";
                 RightAvgHourlyPay[j] = RightAvgHourlyPay[j] + "</tr>";

         }

}


public void CreateBudgetedVolumes(){

                         //TITLE;
                         LeftBVTitle = "<tr height=15 style='height:8pt'>";
                         LeftBVTitle = LeftBVTitle + "<td height=15 class=xl2530747 width=276 style='height:8.25pt;width:207pt'>" + MonthName(selMonth) + "</td>";
                         LeftBVTitle = LeftBVTitle + "<td class=xl2430747>&nbsp;</td>";
                         string varianceTotalPayrollTotal = "";
                         string varianceBudgetedPayrollTotal = "";

                         for( int j=0; j <1;j++){
                                 stringBuffer = "";
                                 pageIndex = 0;
                                 int leftOverColumns = 0;
                                 int  i = 0;

                                 for (i=0; i <  (UBound(actualLoad,1)-1);i++){

                                        if (Trim(arTitles[i]) != "" && i < facilityCount){
                                                stringBuffer = stringBuffer + "<td class=xl2630747 align=center>" + arTitles[i] + "</td>";
                                        }else{
                                                stringBuffer = stringBuffer + "<td class=xl2430747 align=right>&nbsp;</td>";
                                        }
                                        PageBVTitle[j,pageIndex] = PageBVTitle[j,pageIndex] + stringBuffer;
                                        BVTitleTotal[j,i] = FNum(cStr(SafeDiv(payRollTotal[j,i],payRollHrsTotal[j,i])), 2);
                                        stringBuffer = "";
                                        if ( ((i+1) % pageSize) == 0 ){
                                                pageIndex = pageIndex + 1;
                                        }
                                 }

                                //Fill out remaining columns;
                                 leftOverColumns = ((i) % pageSize);
                                 for( int y=0; y < (leftOverColumns-2);y++){
                                        PageBVTitle[j,pageIndex] = PageBVTitle[j,pageIndex] + "<td class=xl2430747 align=right>&nbsp;</td>";
                                 }

                                 RightBVTitle[j] = "<td class=xl2630747></td>";
                                 RightBVTitle[j] = RightBVTitle[j] + "<td class=xl2630747 align='center'>Total</td>";
                                 RightBVTitle[j] = RightBVTitle[j] + "<td class=xl2630747></td>";
                                 RightBVTitle[j] = RightBVTitle[j] + "<td class=xl2630747></td>";
                                 RightBVTitle[j] = RightBVTitle[j] + "<td class=xl2630747 align='right'></td>";
                                 RightBVTitle[j] = RightBVTitle[j] + "<td class=xl2630747></td>";
                                 RightBVTitle[j] = RightBVTitle[j] + "</tr>";

                         }


                         //LOAD;
                         LeftBVLoad = "<tr height=15 style='height:8pt'>";
                         LeftBVLoad = LeftBVLoad + "<td height=15 class=xl2730747 align='left' style='height:8.25pt;width:207pt'>Load</td>";
                         LeftBVLoad = LeftBVLoad + "<td class=xl5730747>&nbsp;</td>";

                         for( int j=0; j < 1;j++){
                                 stringBuffer = "";
                                 pageIndex = 0;
                                 int leftOverColumns = 0;
                                int  i = 0;

                                 for (i=0; i <  (UBound(actualLoad,1)-1);i++){

                                        if (Trim(loadBudget[i]) != "" && i < facilityCount){
                                                stringBuffer = stringBuffer + "<td class=xl3430747 align=right>" + FNum(cStr(SafeDbl(loadBudget[i])),0) + "</td>";
                                        }else{
                                                stringBuffer = stringBuffer + "<td class=xl5730747 align=right>&nbsp;</td>";
                                        }

                                        PageBVLoad[j,pageIndex] = PageBVLoad[j,pageIndex] + stringBuffer;
                                        BVLoadTotal[j,i] = FNum(cStr(SafeDbl(loadBudget[i])),0);
                                        stringBuffer = "";
                                        if ( ((i+1) % pageSize) == 0 ){
                                                pageIndex = pageIndex + 1;
                                        }
                                 }

                                 BVLoadTotals = FNum(cStr(Sum(BVLoadTotal, j)),0);

                                //Fill out remaining columns;
                                 leftOverColumns = (i) % pageSize;
                                 for( int y=0; y < (leftOverColumns-2);y++){
                                        PageBVLoad[j,pageIndex] = PageBVLoad[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                                 }

                                 RightBVLoad[j] = "<td class=xl2830747></td>";
                                 RightBVLoad[j] = RightBVLoad[j] + "<td class=xl3430747 align='right'>" + FNum(cStr(Sum(BVLoadTotal, j)),0) + "</td>";
                                 RightBVLoad[j] = RightBVLoad[j] + "<td class=xl2830747></td>";
                                 RightBVLoad[j] = RightBVLoad[j] + "<td class=xl2830747></td>";
                                 RightBVLoad[j] = RightBVLoad[j] + "<td class=xl2830747 align='right'></td>";
                                 RightBVLoad[j] = RightBVLoad[j] + "<td class=xl2830747></td>";
                                 RightBVLoad[j] = RightBVLoad[j] + "</tr>";

                         }


                         //UNLOAD;
                         LeftBVUnLoad = "<tr height=15 style='height:8pt'>";
                         LeftBVUnLoad = LeftBVUnLoad + "<td height=15 class=xl2730747 align='left' style='height:8.25pt;width:207pt'>UnLoad</td>";
                         LeftBVUnLoad = LeftBVUnLoad + "<td class=xl5730747>&nbsp;</td>";

                         for( int j=0; j < 1;j++){
                                 stringBuffer = "";
                                 pageIndex = 0;
                                 int leftOverColumns = 0;
                                 int  i = 0;

                                 for (i=0; i <  (UBound(actualLoad,1)-1);i++){
                                        if (Trim(unloadBudget[i]) != "" && i < facilityCount){
                                                stringBuffer = stringBuffer + "<td class=xl3130747 align=right>" + FNum(cStr(SafeDbl(unloadBudget[i])),0) + "</td>";
                                        }else{
                                                stringBuffer = stringBuffer + "<td class=xl5730747 align=right>&nbsp;</td>";
                                        }
                                        PageBVUnLoad[j,pageIndex] = PageBVUnLoad[j,pageIndex] + stringBuffer;
                                        BVUnLoadTotal[j,i] = FNum(cStr(SafeDbl(unloadBudget[i])),0);
                                        stringBuffer = "";
                                        if ( ((i+1) % pageSize) == 0 ){
                                                pageIndex = pageIndex + 1;
                                        }
                                 }

                                 BVUnloadTotals = FNum(cStr(Sum(BVUnLoadTotal, j)),0);

                                //Fill out remaining columns;
                                 leftOverColumns = (i) % pageSize;
                                 for( int y=0; y < (leftOverColumns-2);y++){
                                        PageBVUnLoad[j,pageIndex] = PageBVUnLoad[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                                 }

                                 RightBVUnLoad[j] = "<td class=xl2830747></td>";
                                 RightBVUnLoad[j] = RightBVUnLoad[j] + "<td class=xl3130747 align='right'>" + FNum(cStr(Sum(BVUnLoadTotal, j)),0) + "</td>";
                                 RightBVUnLoad[j] = RightBVUnLoad[j] + "<td class=xl2830747></td>";
                                 RightBVUnLoad[j] = RightBVUnLoad[j] + "<td class=xl2830747></td>";
                                 RightBVUnLoad[j] = RightBVUnLoad[j] + "<td class=xl2830747 align='right'></td>";
                                 RightBVUnLoad[j] = RightBVUnLoad[j] + "<td class=xl2830747></td>";
                                 RightBVUnLoad[j] = RightBVUnLoad[j] + "</tr>";

                         }


                         //TOTAL;
                         LeftBVTotal = "<tr height=15 style='height:8pt'>";
                         LeftBVTotal = LeftBVTotal + "<td height=15 class=xl2630747 align='right' style='height:8.25pt;width:207pt'>Total</td>";
                         LeftBVTotal = LeftBVTotal + "<td class=xl5730747>&nbsp;</td>";

                         for( int j=0; j < 1; j++){
                                 stringBuffer = "";
                                 pageIndex = 0;
                                 int leftOverColumns = 0;
                                int  i = 0;

                                 for (i=0; i <  (UBound(actualLoad,1)-1);i++){
                                        if ( i < facilityCount){
                                                stringBuffer = stringBuffer + "<td class=xl3430747 align=right>" + FNum(cStr(SafeDbl(unloadBudget[i]) + SafeDbl(loadBudget[i])),0) + "</td>";
                                        }else{
                                                stringBuffer = stringBuffer + "<td class=xl5730747 align=right>&nbsp;</td>";
                                        }
                                        PageBVTotal[j,pageIndex] = PageBVTotal[j,pageIndex] + stringBuffer;
                                        BVTotalTotal[j,i] = FNum(cStr(SafeDbl(unloadBudget[i]) + SafeDbl(loadBudget[i])),0);
                                        stringBuffer = "";
                                        if ( ((i+1) % pageSize) == 0 ){
                                                pageIndex = pageIndex + 1;
                                        }
                                 }

                                 loadUnloadTotal = FNum(cStr(Sum(BVTotalTotal, j)),0);

                                //Fill out remaining columns;
                                 leftOverColumns = (i) % pageSize;
                                 for( int y=0; y < (leftOverColumns-2);y++){
                                        PageBVTotal[j,pageIndex] = PageBVTotal[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                                 }

                                 RightBVTotal[j] = "<td class=xl2830747></td>";
                                 RightBVTotal[j] = RightBVTotal[j] + "<td class=xl3430747 align='right'>" + FNeg(cStr(Sum(BVTotalTotal, j)),0) + "</td>";
                                 RightBVTotal[j] = RightBVTotal[j] + "<td class=xl2830747></td>";
                                 RightBVTotal[j] = RightBVTotal[j] + "<td class=xl2830747></td>";
                                 RightBVTotal[j] = RightBVTotal[j] + "<td class=xl2830747 align='right'></td>";
                                 RightBVTotal[j] = RightBVTotal[j] + "<td class=xl2830747></td>";
                                 RightBVTotal[j] = RightBVTotal[j] + "</tr>";

                         }

                 //UNLOAD VARIANCE;
                 LeftBVUnLoadVar = "<tr height=15 style='height:8pt'>";
                 LeftBVUnLoadVar = LeftBVUnLoadVar + "<td height=15 class=xl3430747 align='right' style='height:8.25pt;width:207pt'>UnLoad</td>";
                 LeftBVUnLoadVar = LeftBVUnLoadVar + "<td class=xl5730747>&nbsp;</td>";

                 for( int j=0; j < 1; j++){

                         stringBuffer = "";
                         pageIndex = 0;
                         int leftOverColumns = 0;
                         int i = 0;

                         for (i=0; i <  (UBound(actualLoad,1)-1);i++){
                                varianceUnloadTotal = 0;

                                for( int y=0; y < weekCount;y++){

                                        varianceUnloadTotal = varianceUnloadTotal + SafeDbl(varianceUnLoad[y,i]);
                                }

                                if(i < facilityCount){
                                        stringBuffer = stringBuffer + "<td class=xl3430747 align=right>" + FNeg(varianceUnloadTotal.ToString("F"),0) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl5730747 align=right>&nbsp;</td>";
                                }
                                PageBVUnLoadVar[j,pageIndex] = PageBVUnLoadVar[j,pageIndex] + stringBuffer;
                                BVUnLoadVarTotal[j,i] = FNeg(varianceUnloadTotal.ToString("F"),0);
                                stringBuffer = "";
                                if ( ((i+1) % pageSize) == 0 ){
                                        pageIndex = pageIndex + 1;
                               }
                         }

                         BVVarUnloadTotals = FNum(cStr(Sum(BVUnLoadVarTotal, j)),0);

                        //Fill out remaining columns;
                         leftOverColumns = (i) % pageSize;
                         for( int y=0; y < (leftOverColumns-2);y++){
                                PageBVUnLoadVar[j,pageIndex] = PageBVUnLoadVar[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                         }

                         RightBVUnLoadVar[j] = "<td class=xl2830747></td>";
                         RightBVUnLoadVar[j] = RightBVUnLoadVar[j] + "<td class=xl3430747 align='right'>" + FNeg(cStr(Sum(BVUnLoadVarTotal, j)),0) +  "</td>";
                         RightBVUnLoadVar[j] = RightBVUnLoadVar[j] + "<td class=xl2830747></td>";
                         RightBVUnLoadVar[j] = RightBVUnLoadVar[j] + "<td class=xl2830747></td>";
                         RightBVUnLoadVar[j] = RightBVUnLoadVar[j] + "<td class=xl2830747 align='right'></td>";
                         RightBVUnLoadVar[j] = RightBVUnLoadVar[j] + "<td class=xl2830747></td>";
                         RightBVUnLoadVar[j] = RightBVUnLoadVar[j] + "</tr>";

                 }


                 //LOAD VARIANCE;
                 LeftBVLoadVar = "<tr height=15 style='height:8pt'>";
                 LeftBVLoadVar = LeftBVLoadVar + "<td height=15 class=xl3430747 align='right' style='height:8.25pt;width:207pt'>Load</td>";
                 LeftBVLoadVar = LeftBVLoadVar + "<td class=xl5730747>&nbsp;</td>";

                 for( int j=0; j <1;j++){

                         stringBuffer = "";
                         pageIndex = 0;
                         int leftOverColumns = 0;
                         int i = 0;

                         for (i=0; i <  (UBound(actualLoad,1)-1);i++){
                                double varianceLoadTotal = 0;
                                for( int y=0; y < weekCount; y++){
                                        varianceLoadTotal = varianceLoadTotal + SafeDbl(varianceLoad[y,i]);
                            }
                                if(i < facilityCount){
                                        stringBuffer = stringBuffer + "<td class=xl3130747 align=right>" + FNeg(cStr(varianceLoadTotal),0) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl5730747 align=right>&nbsp;</td>";
                                }
                                PageBVLoadVar[j,pageIndex] = PageBVLoadVar[j,pageIndex] + stringBuffer;
                                BVLoadVarTotal[j,i] = FNeg(cStr(varianceLoadTotal),0);
                                stringBuffer = "";
                                if ( ((i+1) % pageSize) == 0 ){
                                        pageIndex = pageIndex + 1;
                                }
                         }

                         BVVarLoadTotals = FNum(cStr(Sum(BVLoadVarTotal, j)),0);

                        //Fill out remaining columns;
                         leftOverColumns = (i) % pageSize;
                         for( int y=0; y < (leftOverColumns-2);y++){
                                PageBVLoadVar[j,pageIndex] = PageBVLoadVar[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                         }

                         RightBVLoadVar[j] = "<td class=xl2830747></td>";
                         RightBVLoadVar[j] = RightBVLoadVar[j] + "<td class=xl3130747 align='right'>" + FNeg(cStr(Sum(BVLoadVarTotal, j)),0) +  "</td>";
                         RightBVLoadVar[j] = RightBVLoadVar[j] + "<td class=xl2830747></td>";
                         RightBVLoadVar[j] = RightBVLoadVar[j] + "<td class=xl2830747></td>";
                         RightBVLoadVar[j] = RightBVLoadVar[j] + "<td class=xl2830747 align='right'></td>";
                         RightBVLoadVar[j] = RightBVLoadVar[j] + "<td class=xl2830747></td>";
                         RightBVLoadVar[j] = RightBVLoadVar[j] + "</tr>";

                 }


                 //TOTAL VARIANCE;
                 LeftBVTotalVar = "<tr height=15 style='height:8pt'>";
                 LeftBVTotalVar = LeftBVTotalVar + "<td height=15 class=xl2630747 align='left' style='height:8.25pt;width:207pt'></td>";
                 LeftBVTotalVar = LeftBVTotalVar + "<td class=xl5730747>&nbsp;</td>";

                 for( int j=0; j <1;j++){

                         stringBuffer = "";
                         pageIndex = 0;
                         int leftOverColumns = 0;
                         int i = 0;

                         for (i=0; i <  (UBound(actualLoad,1)-1);i++){
                                double varianceLoadTotal = 0;
                                for( int y=0; y < weekCount;y++){
                                        varianceLoadTotal = varianceLoadTotal + SafeDbl(varianceLoad[y,i]);
                                }
                                if( i < facilityCount){
                                        stringBuffer = stringBuffer + "<td class=xl3430747 align=right>" + FNeg(cStr(SafeDbl(BVLoadVarTotal[j,i]) + SafeDbl(BVUnLoadVarTotal[j,i])),0) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl5730747 align=right>&nbsp;</td>";
                                }
                                PageBVTotalVar[j,pageIndex] = PageBVTotalVar[j,pageIndex] + stringBuffer;
                                BVTotalVarTotal[j,i] = cStr(SafeDbl(BVLoadVarTotal[j,i]) + SafeDbl(BVUnLoadVarTotal[j,i]));
                                stringBuffer = "";
                                if ( ((i+1) % pageSize) == 0 ){
                                        pageIndex = pageIndex + 1;
                                }
                         }

                         varianceTotal = FNum(cStr(Sum(BVTotalVarTotal, j)),0);

                        //Fill out remaining columns;
                         leftOverColumns = (i) % pageSize;
                         for( int y=0; y < (leftOverColumns-2);y++){
                                PageBVTotalVar[j,pageIndex] = PageBVTotalVar[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                         }

                         RightBVTotalVar[j] = "<td class=xl2830747></td>";
                         RightBVTotalVar[j] = RightBVTotalVar[j] + "<td class=xl3430747 align='right'>" + FNeg(cStr(SafeDbl(cStr(Sum(BVLoadVarTotal, j))) + SafeDbl(cStr(Sum(BVUnLoadVarTotal, j)))),0) + "</td>";   
                         RightBVTotalVar[j] = RightBVTotalVar[j] + "<td class=xl2830747></td>";
                         RightBVTotalVar[j] = RightBVTotalVar[j] + "<td class=xl2830747></td>";
                         RightBVTotalVar[j] = RightBVTotalVar[j] + "<td class=xl2830747 align='right'></td>";
                         RightBVTotalVar[j] = RightBVTotalVar[j] + "<td class=xl2830747></td>";
                         RightBVTotalVar[j] = RightBVTotalVar[j] + "</tr>";

                 }


                 //TOTAL PAYROLL;
                 LeftBVTotalPayroll = "<tr height=15 style='height:8pt'>";
                 LeftBVTotalPayroll = LeftBVTotalPayroll + "<td height=15 class=xl2730747 align='left' style='height:8.25pt;width:207pt'>Total Payroll</td>";
                 LeftBVTotalPayroll = LeftBVTotalPayroll + "<td class=xl5730747>&nbsp;</td>";

                 for( int j=0; j < 1; j++){
                         stringBuffer = "";
                         pageIndex = 0;
                         int leftOverColumns = 0;
                         int i = 0;

                         for (i=0; i <  (UBound(actualLoad,1)-1);i++){
                                double variancePayrollTotal = 0;
                                for( int y=0; y < weekCount;y++){
                                        variancePayrollTotal = variancePayrollTotal + SafeDbl(payRollTotal[y,i]);
                            }
                                if( i < facilityCount){
                                        stringBuffer = stringBuffer + "<td class=xl3430747 align=right>$ " + FNeg(cStr(SafeDbl(cStr(variancePayrollTotal))),0) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl5730747 align=right>&nbsp;</td>";
                                }
                                PageBVTotalPayroll[j,pageIndex] = PageBVTotalPayroll[j,pageIndex] + stringBuffer;
                                BVTotalPayrollTotal[j,i] = cStr(variancePayrollTotal);
                                stringBuffer = "";
                                if ( ((i+1) % pageSize) == 0 ){
                                        pageIndex = pageIndex + 1;
                                }
                         }

                         varianceTotalPayrollTotal = FNum(cStr(Sum(BVTotalPayrollTotal, j)),0);

                        //Fill out remaining columns;
                         leftOverColumns = (i) % pageSize;
                         for( int y=0; y < (leftOverColumns-2); y++){
                                PageBVTotalPayroll[j,pageIndex] = PageBVTotalPayroll[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                         }

                         RightBVTotalPayroll[j] = "<td class=xl2830747></td>";
                         RightBVTotalPayroll[j] = RightBVTotalPayroll[j] + "<td class=xl3430747 align='right'>" + FNeg(cStr(Sum(BVTotalPayrollTotal, j)),0) +  "</td>";
                         RightBVTotalPayroll[j] = RightBVTotalPayroll[j] + "<td class=xl2830747></td>";
                         RightBVTotalPayroll[j] = RightBVTotalPayroll[j] + "<td class=xl2830747></td>";
                         RightBVTotalPayroll[j] = RightBVTotalPayroll[j] + "<td class=xl2830747 align='right'></td>";
                         RightBVTotalPayroll[j] = RightBVTotalPayroll[j] + "<td class=xl2830747></td>";
                         RightBVTotalPayroll[j] = RightBVTotalPayroll[j] + "</tr>";

                 }


                 //BUDGETED PAYROLL;
                 LeftBVBudgetedPayroll = "<tr height=15 style='height:8pt'>";
                 LeftBVBudgetedPayroll = LeftBVBudgetedPayroll + "<td height=15 class=xl2730747 align='left' style='height:8.25pt;width:207pt'>Budgeted Payroll</td>";
                 LeftBVBudgetedPayroll = LeftBVBudgetedPayroll + "<td class=xl5730747>&nbsp;</td>";

                 for( int j=0; j <1;j++){

                         stringBuffer = "";
                         pageIndex = 0;
                         int leftOverColumns = 0;
                         int i = 0;

                         for (i=0; i <  (UBound(actualLoad,1)-1);i++){
                                if ( i < facilityCount){
                                        stringBuffer = stringBuffer + "<td class=xl3430747 align=right>$ " + FNeg(cStr((SafeDbl(BVTotalVarTotal[j,i]) + SafeDbl(BVTotalTotal[j,i])) * SafeDbl(BudgetedCPUTotal[j,i])),0) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl5730747 align=right>&nbsp;</td>";
                               }
                                PageBVBudgetedPayroll[j,pageIndex] = PageBVBudgetedPayroll[j,pageIndex] + stringBuffer;
                                BVBudgetedPayrollTotal[j,i] = FNum(cStr((SafeDbl(BVTotalVarTotal[j,i]) + SafeDbl(BVTotalTotal[j,i])) * SafeDbl(BudgetedCPUTotal[j,i])),0);
                                stringBuffer = "";
                                if ( ((i+1) % pageSize) == 0 ){
                                        pageIndex = pageIndex + 1;
                               }
                         }

                         varianceBudgetedPayrollTotal = FNum(cStr(Sum(BVBudgetedPayrollTotal, j)),0);

                        //Fill out remaining columns;
                         leftOverColumns = (i) % pageSize;
                         for( int y=0; y < (leftOverColumns-2);y++){
                                PageBVBudgetedPayroll[j,pageIndex] = PageBVBudgetedPayroll[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                         }

                         RightBVBudgetedPayroll[j] = "<td class=xl2830747></td>";
                         RightBVBudgetedPayroll[j] = RightBVBudgetedPayroll[j] + "<td class=xl3430747 align='right'>" + FNeg(cStr(Sum(BVBudgetedPayrollTotal, j)),0) +  "</td>";
                         RightBVBudgetedPayroll[j] = RightBVBudgetedPayroll[j] + "<td class=xl2830747></td>";
                         RightBVBudgetedPayroll[j] = RightBVBudgetedPayroll[j] + "<td class=xl2830747></td>";
                         RightBVBudgetedPayroll[j] = RightBVBudgetedPayroll[j] + "<td class=xl2830747 align='right'></td>";
                         RightBVBudgetedPayroll[j] = RightBVBudgetedPayroll[j] + "<td class=xl2830747></td>";
                         RightBVBudgetedPayroll[j] = RightBVBudgetedPayroll[j] + "</tr>";

                 }


                 //PERCENT VOLUMES VARIANCE;
                 LeftBVVolumesVariance = "<tr height=15 style='height:8pt'>";
                 LeftBVVolumesVariance = LeftBVVolumesVariance + "<td height=15 class=xl2730747 align='left' style='height:8.25pt;width:207pt'><b>% Volumes Variance</td>";
                 LeftBVVolumesVariance = LeftBVVolumesVariance + "<td class=xl5730747>&nbsp;</td>";

                 for( int j=0; j < 1; j++){

                         stringBuffer = "";
                         pageIndex = 0;
                         int leftOverColumns = 0;
                         int i = 0;

                         for (i=0; i <  (UBound(actualLoad,1)-1);i++){
                                if(i < facilityCount && SafeDbl(BVTotalTotal[j,i]) != 0){
                                        stringBuffer = stringBuffer + "<td class=xl3430747 align=right>" + FPerc((SafeDiv(cStr(SafeDbl(BVTotalVarTotal[j,i]) + SafeDbl(BVTotalTotal[j,i])),cStr(SafeDbl(BVTotalTotal[j,i])))-1)) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl5730747 align=right>&nbsp;</td>";
                               }
                                PageBVVolumesVariance[j,pageIndex] = PageBVVolumesVariance[j,pageIndex] + stringBuffer;
                                BVVolumesVarianceTotal[j,i] = FNum(cStr(SafeDiv(cStr(SafeDbl(BVTotalVarTotal[j,i]) + SafeDbl(BVTotalTotal[j,i])), cStr(SafeDbl(BVTotalTotal[j,i])-1))),0);
                                stringBuffer = "";
                                if ( ((i+1) % pageSize) == 0 ){
                                        pageIndex = pageIndex + 1;
                               }
                         }

                        //Fill out remaining columns;
                         leftOverColumns = (i) % pageSize;
                         for( int y=0; y < (leftOverColumns-2);y++){
                                PageBVVolumesVariance[j,pageIndex] = PageBVVolumesVariance[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                         }

                         RightBVVolumesVariance[j] = "<td class=xl2830747></td>";
                         RightBVVolumesVariance[j] = RightBVVolumesVariance[j] + "<td class=xl3430747 align='right'>"  + FPerc((SafeDiv(cStr(SafeDbl(loadUnloadTotal) + SafeDbl(varianceTotal)), cStr(SafeDbl(loadUnloadTotal))) -1)) +  "</td>";
                         RightBVVolumesVariance[j] = RightBVVolumesVariance[j] + "<td class=xl2830747></td>";
                         RightBVVolumesVariance[j] = RightBVVolumesVariance[j] + "<td class=xl2830747></td>";
                         RightBVVolumesVariance[j] = RightBVVolumesVariance[j] + "<td class=xl2830747 align='right'></td>";
                         RightBVVolumesVariance[j] = RightBVVolumesVariance[j] + "<td class=xl2830747></td>";
                         RightBVVolumesVariance[j] = RightBVVolumesVariance[j] + "</tr>";

                 }


                 //PERCENT PAYROLL VARIANCE;
                 LeftBVPayrollVariance = "<tr height=15 style='height:8pt'>";
                 LeftBVPayrollVariance = LeftBVPayrollVariance + "<td height=15 class=xl2730747 align='left' style='height:8.25pt;width:207pt'><b>% Payroll Variance</td>";
                 LeftBVPayrollVariance = LeftBVPayrollVariance + "<td class=xl5730747>&nbsp;</td>";

                 for( int j=0; j <1; j++){

                         stringBuffer = "";
                         pageIndex = 0;
                         int leftOverColumns = 0;
                         int i = 0;

                         for (i=0; i <  (UBound(actualLoad,1)-1);i++){
                                if(SafeDbl(BVBudgetedPayrollTotal[j,i]) != 0 && (i < facilityCount)){
                                        stringBuffer = stringBuffer + "<td class=xl3430747 align=right>" + FPerc((SafeDiv(cStr(SafeDbl(BVTotalPayrollTotal[j,i])), cStr(SafeDbl(BVBudgetedPayrollTotal[j,i])))-1)) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl5730747 align=right>&nbsp;</td>";
                                }
                                PageBVPayrollVariance[j,pageIndex] = PageBVPayrollVariance[j,pageIndex] + stringBuffer;
                                BVPayrollVarianceTotal[j,i] = FNum(cStr(SafeDiv(cStr(SafeDbl(BVTotalPayrollTotal[j,i])), cStr(SafeDbl(BVBudgetedPayrollTotal[j,i])))-1),0);
                                stringBuffer = "";
                                if ( ((i+1) % pageSize) == 0 ){
                                        pageIndex = pageIndex + 1;
                                }
                         }

                        //Fill out remaining columns;
                         leftOverColumns = ((i) % pageSize);
                         for( int y=0; y < (leftOverColumns-2);y++){
                                PageBVPayrollVariance[j,pageIndex] = PageBVPayrollVariance[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                         }

                         RightBVPayrollVariance[j] = "<td class=xl2830747></td>";
                         RightBVPayrollVariance[j] = RightBVPayrollVariance[j] + "<td class=xl3430747 align='right'>"  + FPerc(SafeDiv(cStr(SafeDbl(varianceTotalPayrollTotal)), cStr(SafeDbl(varianceBudgetedPayrollTotal)) )-1) + "</td>";
                         RightBVPayrollVariance[j] = RightBVPayrollVariance[j] + "<td class=xl2830747></td>";
                         RightBVPayrollVariance[j] = RightBVPayrollVariance[j] + "<td class=xl2830747></td>";
                         RightBVPayrollVariance[j] = RightBVPayrollVariance[j] + "<td class=xl2830747 align='right'></td>";
                         RightBVPayrollVariance[j] = RightBVPayrollVariance[j] + "<td class=xl2830747></td>";
                         RightBVPayrollVariance[j] = RightBVPayrollVariance[j] + "</tr>";

                 }


                 //UNLOAD VOLUMES;
                 LeftBVUnloadVolumes = "<tr height=15 style='height:8pt'>";
                 LeftBVUnloadVolumes = LeftBVUnloadVolumes + "<td height=15 class=xl3430747 align='right' style='height:8.25pt;width:207pt'>Unload</td>";
                 LeftBVUnloadVolumes = LeftBVUnloadVolumes + "<td class=xl5730747>&nbsp;</td>";

                 for( int j=0; j <1;j++){
                         stringBuffer = "";
                         pageIndex = 0;
                         int leftOverColumns = 0;
                         int i = 0;

                         for (i=0; i <  (UBound(actualLoad,1)-1);i++){
                                if(i < facilityCount){
                                        stringBuffer = stringBuffer + "<td class=xl3430747 align=right>" + FNum(cStr(SafeDbl(BVUnLoadTotal[j,i]) + SafeDbl(BVUnLoadVarTotal[j,i])) ,0) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl5730747 align=right>&nbsp;</td>";
                                }
                                PageBVUnloadVolumes[j,pageIndex] = PageBVUnloadVolumes[j,pageIndex] + stringBuffer;
                                BVUnloadVolumesTotal[j,i] = FNum(cStr(SafeDbl(BVUnLoadTotal[j,i]) + SafeDbl(BVUnLoadVarTotal[j,i])),0);
                                stringBuffer = "";
                                if ( ((i+1) % pageSize) == 0 ){
                                        pageIndex = pageIndex + 1;
                               }
                         }

                         BVUnloadVolumesTotals = FNum(cStr(SafeDbl(BVUnloadTotals) + SafeDbl(BVVarUnloadTotals)),0);

                        //Fill out remaining columns;
                         leftOverColumns = (i) % pageSize;
                         for( int y=0; y < (leftOverColumns-2);y++){
                                PageBVUnloadVolumes[j,pageIndex] = PageBVUnloadVolumes[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                         }

                         RightBVUnloadVolumes[j] = "<td class=xl2830747></td>";
                         RightBVUnloadVolumes[j] = RightBVUnloadVolumes[j] + "<td class=xl3430747 align='right'>"  + FNum(cStr(SafeDbl(BVUnloadTotals) + SafeDbl(BVVarUnloadTotals)),0) + "</td>";
                         RightBVUnloadVolumes[j] = RightBVUnloadVolumes[j] + "<td class=xl2830747></td>";
                         RightBVUnloadVolumes[j] = RightBVUnloadVolumes[j] + "<td class=xl2830747></td>";
                         RightBVUnloadVolumes[j] = RightBVUnloadVolumes[j] + "<td class=xl2830747 align='right'></td>";
                         RightBVUnloadVolumes[j] = RightBVUnloadVolumes[j] + "<td class=xl2830747></td>";
                         RightBVUnloadVolumes[j] = RightBVUnloadVolumes[j] + "</tr>";

                 }


                 //LOAD VOLUMES;
                 LeftBVLoadVolumes = "<tr height=15 style='height:8pt'>";
                 LeftBVLoadVolumes = LeftBVLoadVolumes + "<td height=15 class=xl3430747 align='right' style='height:8.25pt;width:207pt'>Load</td>";
                 LeftBVLoadVolumes = LeftBVLoadVolumes + "<td class=xl5730747>&nbsp;</td>";

                 for( int j=0; j <1;j++){

                         stringBuffer = "";
                         pageIndex = 0;
                         int leftOverColumns = 0;
                         int i = 0;

                         for (i=0; i <  (UBound(actualLoad,1)-1);i++){

                                if(i < facilityCount){
                                        stringBuffer = stringBuffer + "<td class=xl3130747 align=right>" + FNum(cStr(SafeDbl(BVLoadTotal[j,i]) + SafeDbl(BVLoadVarTotal[j,i])) ,0) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl5730747 align=right>&nbsp;</td>";
                               }
                                PageBVLoadVolumes[j,pageIndex] = PageBVLoadVolumes[j,pageIndex] + stringBuffer;
                                BVLoadVolumesTotal[j,i] = FNum(cStr(SafeDbl(BVLoadTotal[j,i]) + SafeDbl(BVLoadVarTotal[j,i])),0);
                                stringBuffer = "";
                                if ( ((i+1) % pageSize) == 0 ){
                                        pageIndex = pageIndex + 1;
                               }
                         }

                         BVLoadVolumesTotals = FNum(cStr(SafeDbl(BVLoadTotals) + SafeDbl(BVVarLoadTotals)),0);

                        //Fill out remaining columns;
                         leftOverColumns = (i) % pageSize;
                         for( int y=0; y < (leftOverColumns-2);y++){
                                PageBVLoadVolumes[j,pageIndex] = PageBVLoadVolumes[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                         }

                         RightBVLoadVolumes[j] = "<td class=xl2830747></td>";
                         RightBVLoadVolumes[j] = RightBVLoadVolumes[j] + "<td class=xl3130747 align='right'>"  + FNum(cStr(SafeDbl(BVLoadTotals) + SafeDbl(BVVarLoadTotals)),0) + "</td>";
                         RightBVLoadVolumes[j] = RightBVLoadVolumes[j] + "<td class=xl2830747></td>";
                         RightBVLoadVolumes[j] = RightBVLoadVolumes[j] + "<td class=xl2830747></td>";
                         RightBVLoadVolumes[j] = RightBVLoadVolumes[j] + "<td class=xl2830747 align='right'></td>";
                         RightBVLoadVolumes[j] = RightBVLoadVolumes[j] + "<td class=xl2830747></td>";
                         RightBVLoadVolumes[j] = RightBVLoadVolumes[j] + "</tr>";

                 }


                 //TOTAL VOLUMES;
                 LeftBVTotalVolumes = "<tr height=15 style='height:8pt'>";
                 LeftBVTotalVolumes = LeftBVTotalVolumes + "<td height=15 class=xl3430747 align='right' style='height:8.25pt;width:207pt'>Total</td>";
                 LeftBVTotalVolumes = LeftBVTotalVolumes + "<td class=xl5730747>&nbsp;</td>";

                 for( int j=0; j < 1;j++){

                         stringBuffer = "";
                         pageIndex = 0;
                         int leftOverColumns = 0;
                         int i = 0;

                         for (i=0; i <  (UBound(actualLoad,1)-1);i++){
                                if(i < facilityCount){
                                        stringBuffer = stringBuffer + "<td class=xl3430747 align=right>" + FNum(cStr(SafeDbl(BVLoadVolumesTotal[j,i]) + SafeDbl(BVUnloadVolumesTotal[j,i])) ,0) + "</td>";
                                }else{
                                        stringBuffer = stringBuffer + "<td class=xl5730747 align=right>&nbsp;</td>";
                                }
                                PageBVTotalVolumes[j,pageIndex] = PageBVTotalVolumes[j,pageIndex] + stringBuffer;
                                BVTotalVolumesTotal[j,i] = FNum(cStr(SafeDbl(BVLoadVolumesTotal[j,i]) + SafeDbl(BVUnloadVolumesTotal[j,i])),0);
                                stringBuffer = "";
                                if ( ((i+1) % pageSize) == 0 ){
                                        pageIndex = pageIndex + 1;
                                }
                         }

                        //Fill out remaining columns;
                         leftOverColumns = (i) % pageSize;
                         for( int y=0; y < (leftOverColumns-2); y++){
                                PageBVTotalVolumes[j,pageIndex] = PageBVTotalVolumes[j,pageIndex] + "<td class=xl2830747 align=right>&nbsp;</td>";
                         }

                         RightBVTotalVolumes[j] = "<td class=xl2830747></td>";
                         RightBVTotalVolumes[j] = RightBVTotalVolumes[j] + "<td class=xl3430747 align='right'>"  + FNum(cStr(SafeDbl(BVUnloadVolumesTotals) + SafeDbl(BVLoadVolumesTotals)),0) + "</td>";
                         RightBVTotalVolumes[j] = RightBVTotalVolumes[j] + "<td class=xl2830747></td>";
                         RightBVTotalVolumes[j] = RightBVTotalVolumes[j] + "<td class=xl2830747></td>";
                         RightBVTotalVolumes[j] = RightBVTotalVolumes[j] + "<td class=xl2830747 align='right'></td>";
                         RightBVTotalVolumes[j] = RightBVTotalVolumes[j] + "<td class=xl2830747></td>";
                         RightBVTotalVolumes[j] = RightBVTotalVolumes[j] + "</tr>";

                 }


         }

    }
}