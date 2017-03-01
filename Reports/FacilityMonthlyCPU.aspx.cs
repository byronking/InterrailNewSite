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
    public partial class FacilityMonthlyCPU : PageBase
    {

        public DataReader rsPay, rsUnits, rsCPU;
        public DataReader rsUnLoadHours, rsUnLoadUnits, rsLoadHours, rsLoadUnits, rsUM, rsShuttlingHours, rsShuttlingUnits;
        public DataReader rsSpottingHours, rsSpotting, rsTrainingHours, rsClericalHours, rsRBHours, rsUAHours, rsSelfAudit, rsKPI, rsBudgetCPU, rsATHours, rsATUnits, rsPrepHours, rsPrepUnits, rsTotalUnits;
        public string[] arrTotalAccidents = new string[12];
        public string[] arrTotalInjuryRep = new string[12];
        public string[] arrTotalLostTime = new string[12];
        public string[] arrTotalNonMed = new string[12];
        public string UnLoadHours, UnLoadUnits, LoadHours, LoadUnits, sUM, sUM1, sUM2, sUM3, shuttlingHours, shuttlingUnits;
        public string spottingHours, spotting1, spotting2, spotting3, clericalHours, trainingHours, RBHours, UAHours, selfAudit;
        public string KPI1, KPI2, KPI3, KPI4, budgetCPU, ATHours, ATUnits, prepHours, prepUnits, totalUnits;

        public double unLoadTotalHours = 0.0;
        public double unLoadTotalUnits = 0.0;

        public double LoadTotalHours = 0.0;
        public double LoadTotalUnits = 0.0;

        public double UMTotals = 0.0;
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

        public double RCs_In = 0.0;
        public double RCs_Out = 0.0;
        public double DownTime = 0.0;
        public double SpottingTotalHours = 0.0;
        public double Total_RC_Hr = 0.0;

        public double SA_Bay = 0.0;
        public double SA_Fac = 0.0;
        public double Total_SA_Bay = 0.0;
        public double Total_SA_Fac = 0.0;
        public double Total_SC_Bay = 0.0;
        public double Total_SC_Fac = 0.0;

        public double Total_Accidents = 0.0;
        public double Total_Injuries = 0.0;
        public double Total_LostTime = 0.0;
        public double Total_NonMed = 0.0;

        public string selYear = "";
        public string sFac = "";

        public bool bUnLoadHours, bUnLoadUnits, bLoadHours, bLoadUnits, bUM, bShuttlingHours, bShuttlingUnits;
        public bool bSpottingHours, bSpotting, bTrainingHours, bClericalHours, bRBHours, bUAHours, bSelfAudit, bKPI, bBudgetCPU, bATHours, bATUnits, bPrepHours, bPrepUnits, bTotalUnits;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin, User");
            
            selYear = Request["selYear"];
            sFac = cStr(Session["FacilityID"]);


            //;
            // Get Total Hours for( UNLOAD;
            //;
            rsUnLoadHours = new DataReader(getSQL(sFac, selYear, "UL", "HOURS"));
            rsUnLoadHours.Open();
            bUnLoadHours = rsUnLoadHours.Read();

            //;
            // Get Total Units for( UNLOAD;
            //;
            rsUnLoadUnits = new DataReader(getSQL(sFac, selYear, "UL", "UNITS"));
            rsUnLoadUnits.Open();
            bUnLoadUnits = rsUnLoadUnits.Read();

            //;
            // Get Total Hours for( LOAD;
            //;
            rsLoadHours = new DataReader(getSQL(sFac, selYear, "LO", "HOURS"));
            rsLoadHours.Open();
            bLoadHours = rsLoadHours.Read();

            //;
            // Get Total Units for( LOAD;
            //;
            rsLoadUnits = new DataReader(getSQL(sFac, selYear, "LO", "UNITS"));
            rsLoadUnits.Open();
            bLoadUnits = rsLoadUnits.Read();


            //;
            // Get Total Units/Man;
            //;
            rsUM = new DataReader(getUMSQL(sFac, selYear));
            rsUM.Open();
            bUM = rsUM.Read();

            //;
            // Get Total Hours for( SHUTTLING;
            //;
            rsShuttlingHours = new DataReader(getSQL(sFac, selYear, "SH", "HOURS"));
            rsShuttlingHours.Open();
            bShuttlingHours = rsShuttlingHours.Read();


            //;
            // Get Total Units for( SHUTTLING;
            //;
            rsShuttlingUnits = new DataReader(getSQL(sFac, selYear, "SH", "UNITS"));
            rsShuttlingUnits.Open();
            bShuttlingUnits = rsShuttlingUnits.Read();


            //;
            // Get Total Hours for( SPOTTING;
            //;
            rsSpottingHours = new DataReader(getSQL(sFac, selYear, "SP", "HOURS"));
            rsSpottingHours.Open();
            bSpottingHours = rsSpottingHours.Read();

            //;
            //;
            // Get SPOTTING RC//s IN/OUT AND Down Time;
            rsSpotting = new DataReader(getSpotSQL(sFac, selYear));
            rsSpotting.Open();
            bSpotting = rsSpotting.Read();


            //;
            // Get Total Hours for( Training;
            //;
            rsTrainingHours = new DataReader(getSQL(sFac, selYear, "TR", "HOURS"));
            rsTrainingHours.Open();
            bTrainingHours = rsTrainingHours.Read();

            //;
            // Get Total Hours for( Clerical;
            //;
            rsClericalHours = new DataReader(getSQL(sFac, selYear, "CL", "HOURS"));
            rsClericalHours.Open();
            bClericalHours = rsClericalHours.Read();

            //;
            // Get Total Hours for( Misc. (Rebilling);
            //;
            rsRBHours = new DataReader(getSQL(sFac, selYear, "RB", "HOURS"));
            rsRBHours.Open();
            bRBHours = rsRBHours.Read();

            //;
            // Get Total Hours for( UA;
            //;
            rsUAHours = new DataReader(getSQL(sFac, selYear, "UA", "HOURS"));
            rsUAHours.Open();
            bUAHours = rsUAHours.Read();

            //;
            //;
            // Get Self Audit;
            rsSelfAudit = new DataReader(getSelfAuditSQL(sFac, selYear));
            rsSelfAudit.Open();
            bSelfAudit = rsSelfAudit.Read();

            //;
            //;
            // Get KPIs;
            rsKPI = new DataReader(getKPISQL(sFac, selYear));
            rsKPI.Open();
            bKPI = rsKPI.Read();


            //;
            // Get Budget/Cost Per Unit;
            //;
            rsBudgetCPU = new DataReader(getBCPUSQL(sFac, selYear));
            rsBudgetCPU.Open();
            bBudgetCPU = rsBudgetCPU.Read(); 


            //;
            // Get Total Hours for( Air Test;
            //;
            rsATHours = new DataReader(getSQL(sFac, selYear, "AT", "HOURS"));
            rsATHours.Open();
            bATHours = rsATHours.Read();


            // Get Total Units for( Air Test;
            //;
            rsATUnits = new DataReader(getSQL(sFac, selYear, "AT", "UNITS"));
            rsATUnits.Open();
            bATUnits = rsATUnits.Read();


            // Get Total Hours for( Prepping;
            //;
            rsPrepHours = new DataReader(getSQL(sFac, selYear, "RP", "HOURS"));
            rsPrepHours.Open();
            bPrepHours = rsPrepHours.Read();


             // Get Total Units for( Prepping;
             //;
             rsPrepUnits = new DataReader(getSQL(sFac, selYear, "RP", "UNITS"));
             rsPrepUnits.Open();
             bPrepUnits = rsPrepUnits.Read();


             // Get Total Units;
             //;
             rsTotalUnits = new DataReader(getSQL(sFac, selYear, "ALL", "UNITS"));
             rsTotalUnits.Open();
             bTotalUnits = rsTotalUnits.Read();

        }


        public string getSQL(string sFac, string sYear, string sTask, string sPayOrUnits)
        {

            string strSQL = "";

            if (sPayOrUnits == "UNITS")
            {
                strSQL = "SELECT IsNULL(SUM(d.Units), 0) AS TotalUnits, MONTH(WorkDate) As Month ";
                strSQL += " FROM Facility f INNER JOIN FacilityProductionDetail d ON f.Id = d.FacilityID ";
                strSQL += "      INNER JOIN Tasks ON d.TaskId = Tasks.Id  ";
            }
            else
            {
                strSQL = "SELECT IsNULL(SUM(HoursPaid),0) AS TotalHours, MONTH(WorkDate) As Month ";
                strSQL += " FROM Facility f INNER JOIN EmployeeTaskWorked ON f.Id = EmployeeTaskWorked.FacilityID ";
                strSQL += "      INNER JOIN Tasks ON EmployeeTaskWorked.TaskID = Tasks.Id   ";
                strSQL += "      LEFT OUTER JOIN EmployeeTaskWorkedPay ON EmployeeTaskWorked.Id = EmployeeTaskWorkedPay.EmployeeTaskWorkedId ";
            }

            if (sTask == "ALL")
            {
                strSQL += "WHERE (Tasks.Id IN (1,2) ) ";
            }
            else
            {
                if (sTask == "SH")
                {
                    strSQL += "WHERE (Tasks.TaskCode In ('SH', 'SU') ) ";
                }
                else
                {
                    if (sTask == "RB")
                    {
                        strSQL += "WHERE (Tasks.Rebillable = 1) ";
                    }
                    else
                    {
                        strSQL += "WHERE (Tasks.TaskCode = '" + sTask + "') ";
                    }
                }
            }

            strSQL += "      AND f.ID = " + sFac;
            strSQL += "      AND WorkDate BETWEEN '" + "1/1/" + cStr(sYear) + "' AND '12/31/" + cStr(sYear) + "' ";
            strSQL += "GROUP BY Month(WorkDate) ";
            strSQL += "ORDER BY Month ";

            return strSQL;

        }

        public string getUMSQL(string sFac, string sYear)
        {

            string strSQL = "";
            string sWhere = "";

            for (int I = 1; I <= 12; I++)
            {
                sWhere = " Where Datepart(yyyy,WorkDate)=" + sYear + " AND DatePart(m,Workdate)=" + cStr(I) + " AND FacilityID=" + sFac + " AND TaskID IN (1,2)) ";

                strSQL += " SELECT TotalUnits=IsNULL((Select Sum(Units) From FacilityProductionDetail " + sWhere + ",0), ";
                strSQL += " TotalMen=IsNULL((Select Count(Distinct EmployeeID) From EmployeeTaskWorked " + sWhere + ",0), ";
                strSQL += " TotalDays=IsNULL((Select Count(Distinct Datepart(d,WorkDate)) From EMployeeTaskWorked " + sWhere + ",0), Mon=" + cStr(I);

                if (I < 12)
                {
                    strSQL += " UNION ";
                }
            }

            strSQL += "ORDER BY Mon ";
            return strSQL;

        }

        public string getBCPUSQL(string sFac, string sYear)
        {

            string strSQL = "";
            string  sWhereUnits, sWherePay;

            for (int I = 1; I <= 12; I++)
            {

                sWhereUnits = "  Datepart(yyyy,WorkDate)=" + sYear + " AND DatePart(m,Workdate)=" + cStr(I) + " AND FacilityID=" + sFac + " AND TaskID IN (1,2)) ";
                sWherePay = "  Datepart(yyyy,WorkDate)=" + sYear + " AND DatePart(m,Workdate)=" + cStr(I) + " AND FacilityID=" + sFac + " ) ";

                strSQL += " SELECT TotalUnits=IsNULL((Select Sum(Units) From FacilityProductionDetail WHERE " + sWhereUnits + ",0), ";
                strSQL += "  TotalRegHours=IsNULL((Select Sum(PayAmount) ";
                strSQL += "  FROM EmployeeTaskWorkedPay INNER JOIN EmployeeTaskWorked ON EmployeeTaskWorkedPay.EmployeeTaskWorkedId = EmployeeTaskWorked.Id ";
                strSQL += "  WHERE PayMultiplier=1 AND " + sWherePay + ",0), ";
                strSQL += "  TotalOTHours=IsNULL((Select Sum(PayAmount) ";
                strSQL += "  FROM EmployeeTaskWorkedPay INNER JOIN EmployeeTaskWorked ON EmployeeTaskWorkedPay.EmployeeTaskWorkedId = EmployeeTaskWorked.Id ";
                strSQL += "  WHERE PayMultiplier <> 1 AND " + sWherePay + ",0),  Mon=" + cStr(I);

                if (I < 12)
                {
                    strSQL += " UNION ";
                }
            }

            strSQL += "ORDER BY Mon ";
            return strSQL;

        }

        public string getSpotSQL(string sFac, string sYear)
        {

            string strSQL;

            strSQL = " SELECT  IsNULL(SUM(CASE FieldName WHEN 'RC_IN'    Then Cast(DataValue as float) Else 0 END), 0) AS RCs_IN,     ";
            strSQL += "       IsNULL(SUM(CASE FieldName WHEN 'RC_OUT'   Then Cast(DataValue as float) Else 0 END), 0) AS RCs_OUT,    ";
            strSQL += "       IsNULL(SUM(CASE FieldName WHEN 'DOWN_TIME' Then Cast(DataValue as float) Else 0 END), 0) AS DOWN_TIME,  ";
            strSQL += "               MONTH(WorkDate) As Month ";
            strSQL += " FROM FacilityMonitoringDataEntry ";
            strSQL += " WHERE FacilityID = " + sFac;
            strSQL += "      AND WorkDate BETWEEN '" + "1/1/" + cStr(sYear) + "' AND '12/31/" + cStr(sYear) + "' ";
            strSQL += " GROUP BY MONTH(WorkDate) ";
            strSQL += " ORDER BY Month ";

            return strSQL;
        }

        public string getSelfAuditSQL(string sFac, string sYear)
        {
            string strSQL = "";

            for (int I = 1; I <= 12; I++)
            {
                strSQL += " SELECT  (SELECT IsNULL(COUNT(DataValue),0) FROM FacilityMonitoringDataEntry ";
                strSQL += "                        WHERE FacilityID = " + sFac + " AND DatePart(m,WorkDate) = " + cStr(I) + " AND DatePart(yyyy,WorkDate) = ";
                strSQL += "                        " + cStr(sYear) + " AND DataValue <> '' AND DataValue <> '0' AND DataValue IS NOT NULL AND FieldName = 'BAY') AS IN_BAY,     ";
                strSQL += "                (SELECT ISNULL(COUNT(DataValue),0) FROM FacilityMonitoringDataEntry ";
                strSQL += "                        WHERE FacilityID = " + sFac + " AND DatePart(m,WorkDate) = " + cStr(I) + " AND DatePart(yyyy,WorkDate) = ";
                strSQL += "                        " + cStr(sYear) + " AND DataValue <> '' AND DataValue <> '0' AND DataValue IS NOT NULL AND FieldName = 'FACILITY') AS FAC,     ";
                strSQL += "                (SELECT IsNULL(AVG(Cast(DataValue as decimal(6,2))),0) FROM FacilityMonitoringDataEntry ";
                strSQL += "                        WHERE FacilityID = " + sFac + " AND DatePart(m,WorkDate) = " + cStr(I) + " AND DatePart(yyyy,WorkDate) = ";
                strSQL += "                        " + cStr(sYear) + " AND DataValue <> '' AND FieldName = 'BAY') AS IN_BAY_AVG,     ";
                strSQL += "                (SELECT IsNULL(AVG(Cast(DataValue as decimal(6,2))),0) FROM FacilityMonitoringDataEntry ";
                strSQL += "                        WHERE FacilityID = " + sFac + " AND DatePart(m,WorkDate) = " + cStr(I) + " AND DatePart(yyyy,WorkDate) = ";
                strSQL += "                        " + cStr(sYear) + " AND DataValue <> '' AND FieldName = 'FACILITY') AS FAC_AVG, ";
                strSQL += "                        IsNULL(SUM(CASE FieldName WHEN 'BAY'   Then Cast(DataValue as float) Else 0 END), 0) AS TOTAL_BAY,    ";
                strSQL += "                        IsNULL(SUM(CASE FieldName WHEN 'FACILITY'   Then Cast(DataValue as float) Else 0 END), 0) AS TOTAL_FAC,    ";
                strSQL += "       Mon=" + cStr(I);
                strSQL += " FROM FacilityMonitoringDataEntry ";
                strSQL += " WHERE FacilityID = " + sFac;
                strSQL += "  AND DatePart(m,WorkDate) = " + cStr(I) + " AND  DatePart(yyyy,WorkDate) = " + cStr(sYear);

                if (I < 12)
                {
                    strSQL += " UNION ";
                }
            }

            strSQL += "ORDER BY Mon ";

            return strSQL;
        }

        public string getKPISQL(string sFac, string sYear)
        {

            string strSQL;

            strSQL = " SELECT  IsNULL(SUM(CASE FieldName WHEN 'ACCIDENTS'   Then Cast(DataValue as float) Else 0 END), 0) AS ACC,     ";
            strSQL += "       IsNULL(SUM(CASE FieldName WHEN 'REP_INJURIES' Then Cast(DataValue as float) Else 0 END), 0) AS INJURIES,";
            strSQL += "       IsNULL(SUM(CASE FieldName WHEN 'LOST_TIME'   Then Cast(DataValue as float) Else 0 END), 0) AS LOSTTIME,";
            strSQL += "       IsNULL(SUM(CASE FieldName WHEN 'NON_MEDICAL' Then Cast(DataValue as float) Else 0 END), 0) AS NONMED,  ";
            strSQL += "       MONTH(WorkDate) As Month ";
            strSQL += " FROM FacilityMonitoringDataEntry ";
            strSQL += " WHERE FacilityID = " + sFac;
            strSQL += "      AND WorkDate BETWEEN '" + "1/1/" + cStr(sYear) + "' AND '12/31/" + cStr(sYear) + "' ";
            strSQL += " GROUP BY Month(WorkDate) ";
            strSQL += " ORDER BY Month ";

            return strSQL;
        }

        //Returns Last Day of Month;
        public DateTime LastDayOfMonth(DateTime mydate){

                DateTime LastDayOfMonth = mydate;
                while (LastDayOfMonth.Month ==  mydate.Month){
                        LastDayOfMonth = LastDayOfMonth.AddDays(1);
                }

                return LastDayOfMonth.AddDays(-1);
            }

        public void WriteTopRows(){

              string rc = "";

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
                if (bUnLoadUnits ){
                        if (cInt(rsUnLoadUnits.Fields(1)) == i ){
                                UnLoadUnits = cStr(rsUnLoadUnits.Fields(0));
                                bUnLoadUnits = rsUnLoadUnits.Read();
                        }else{
                                UnLoadUnits = "";
                        }
                    }else{
                        UnLoadUnits = "";
                   }

                if (bUnLoadHours ){
                        if (cInt(rsUnLoadHours.Fields(1)) == i ){
                                UnLoadHours = cStr(rsUnLoadHours.Fields(0));
                                bUnLoadHours = rsUnLoadHours.Read();

                        }else{
                                UnLoadHours = "";
                        }
                    }else{
                        UnLoadHours = "";
                   }

                if (UnLoadHours != string.Empty)
                {
                    UnLoadHours = SafeDbl(UnLoadHours).ToString("#.##");
                }
                else
                {
                    UnLoadHours = "0";
                }
                Response.Write("  <td class=xl5520928 style='border-top:none;border-left:none' align='right'>" + UnLoadHours + "</td>");
                Response.Write("  <td class=xl5620928 style='border-top:none;border-left:none' align='right'>" + cStr(SafeDbl(UnLoadUnits)) + "</td>");
                  if(SafeDbl(UnLoadHours) != 0 && SafeDbl(UnLoadUnits) != 0){
                    var unloadUnits = SafeDiv(cStr(SafeDbl(cStr(UnLoadUnits))), cStr(SafeDbl(cStr(UnLoadHours))));
                    Response.Write("  <td class=xl4620928 style='border-top:none;border-left:none' align='right'>" + unloadUnits.ToString("#.##") + "</td>");
                }
                else{
                    Response.Write ("  <td class=xl4620928 style='border-top:none;border-left:none' >0</td>");
                 }

                  unLoadTotalHours = unLoadTotalHours + SafeDbl(UnLoadHours);
                  unLoadTotalUnits = unLoadTotalUnits + SafeDbl(UnLoadUnits);



                //;
                // Loading;
                //;
                if (bLoadUnits ){
                            if (rsLoadUnits.Fields(1) == cStr(i) ){
                                    LoadUnits = cStr(rsLoadUnits.Fields(0));
                                    bLoadUnits = rsLoadUnits.Read();

                            }else{
                                    LoadUnits = "";
                           }
                    }else{
                            LoadUnits = "";
                   }

                if (bLoadHours ){
                            if (rsLoadHours.Fields(1) ==  cStr(i) ){
                                    LoadHours = cStr(rsLoadHours.Fields(0));
                                    bLoadHours = rsLoadHours.Read();

                            }else{
                                    LoadHours = "";
                           }
                    }else{
                            LoadHours = "";
                   }

                if (LoadHours != string.Empty)
                {
                    LoadHours = SafeDbl(LoadHours).ToString("#.##");
                }
                else
                {
                    LoadHours = "0";
                }
                Response.Write("  <td class=xl5520928 style='border-top:none;border-left:none' align='right'>" + LoadHours + "</td>");

                if (LoadUnits != string.Empty)
                {
                    LoadUnits = SafeDbl(LoadUnits).ToString("#.##");
                }
                else
                {
                    LoadUnits = "0";
                }
                Response.Write ("  <td class=xl5620928 style='border-top:none;border-left:none' align='right'>" + LoadUnits + "</td>");
                if(SafeDbl(cStr(SafeDbl(cStr(LoadHours)))) != 0 && SafeDbl(cStr(SafeDbl(cStr(LoadUnits)))) != 0){
                    var loadUnits = SafeDiv(cStr(SafeDbl(cStr(LoadUnits))), cStr(SafeDbl(cStr(LoadHours))));
                    Response.Write ("  <td class=xl4620928 style='border-top:none;border-left:none' align='right'>" +  loadUnits.ToString("#.##")  + "</td>");
                }else{
                    Response.Write ("  <td class=xl4620928 style='border-top:none;border-left:none' >0</td>");
               }

                LoadTotalHours = SafeDbl(cStr(LoadTotalHours)) + SafeDbl(LoadHours);
                LoadTotalUnits = SafeDbl(cStr(LoadTotalUnits)) + SafeDbl(LoadUnits);



                //;
                // Total Units / Man;
                //;
                if (bUM ){
                            if (cInt(rsUM.Fields(3)) == i ){
                                    sUM = "";
                                    sUM1 = cStr(rsUM.Fields(0));
                                    sUM2 = cStr(rsUM.Fields(1));
                                    sUM3 = cStr(rsUM.Fields(2));

                                    bUM = rsUM.Read();

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
                    sUM = cStr(SafeDbl(cStr(sUM1)) / SafeDbl(cStr(sUM2))  / SafeDbl(cStr(sUM3)));
                    UMTotals = SafeDbl(cStr(UMTotals)) + SafeDbl(sUM);
                    nUMTotals = nUMTotals + 1;
                    Response.Write ("  <td class=xl5620928 style='border-top:none;border-left:none' >" +  cStr(SafeDbl(sUM).ToString("#.##")) + "</td>");
                }else{
                    Response.Write ("  <td class=xl5620928 style='border-top:none;border-left:none' >0</td>");
               }



                //;
                // Shuttling;
                //;
                if (bShuttlingUnits ){
                            if (cInt(rsShuttlingUnits.Fields(1)) == i ){
                                    shuttlingUnits = cStr(rsShuttlingUnits.Fields(0));
                                    bShuttlingUnits = rsShuttlingUnits.Read();

                            }else{
                                    shuttlingUnits = "";
                            }
                    }else{
                            shuttlingUnits = "";
                   }

                if (bShuttlingHours ){
                            if (cInt(rsShuttlingHours.Fields(1))  == i ){
                                    shuttlingHours = cStr(rsShuttlingHours.Fields(0));
                                    bShuttlingHours = rsShuttlingHours.Read();

                            }else{
                                    shuttlingHours = "";
                           }
                    }else{
                            shuttlingHours = "";
                   }

                if (shuttlingHours != string.Empty)
                {
                    shuttlingHours = SafeDbl(shuttlingHours).ToString("#.##");
                }
                else
                {
                    shuttlingHours = "0";
                }
                Response.Write ("  <td class=xl4620928 style='border-top:none;border-left:none' align='right'>" + shuttlingHours + "</td>");
                Response.Write ("  <td class=xl5720928 style='border-top:none;border-left:none' align='right'>" +  cStr(SafeDbl(shuttlingUnits)) + "</td>");

                if(SafeDbl(shuttlingHours) != 0 && SafeDbl(shuttlingUnits) != 0){
                    var shuttlingUnitsDiv = SafeDiv(cStr(SafeDbl(cStr(shuttlingUnits))), cStr(SafeDbl(cStr(shuttlingHours))));
                   Response.Write ("  <td class=xl4620928 style='border-top:none;border-left:none' align='right'>" + shuttlingUnitsDiv.ToString("#.##") + "</td>");
                }else{
                   Response.Write ("  <td class=xl4620928 style='border-top:none;border-left:none' align='right'>0</td>");
               }

                ShuttlingTotalHours = SafeDbl(cStr(ShuttlingTotalHours)) + SafeDbl(shuttlingHours);
                ShuttlingTotalUnits = SafeDbl(cStr(ShuttlingTotalUnits)) + SafeDbl(shuttlingUnits);

                //;
                // Spotting;
                //;
                if (bSpotting ){
                            if (cInt(rsSpotting.Fields(3)) == i ){
                                    spotting1 = cStr(rsSpotting.Fields(0));
                                    spotting2 = cStr(rsSpotting.Fields(1));
                                    spotting3 = cStr(rsSpotting.Fields(2));
                                    bSpotting = rsSpotting.Read();

                            }else{
                                    spotting1 = "";
                                    spotting2 = "";
                                    spotting3 = "0";
                           }
                    }else{
                            spotting1 = "";
                            spotting2 = "";
                            spotting3 = "0";
                   }
                
                if (spotting1 != string.Empty && spotting1 != "0")
                {
                    spotting1 = SafeDbl(spotting1).ToString("#.##");
                }
                else
                {
                    spotting1 = "0";
                }
                Response.Write ("  <td class=xl5620928 style='border-top:none;border-left:none' >" +  spotting1 + "</td>");

                if (spotting2 != string.Empty && spotting2 != "0")
                {
                    spotting2 = SafeDbl(spotting2).ToString("#.##");
                }
                else
                {
                    spotting2 = "0";
                }
                Response.Write ("  <td class=xl5520928 style='border-top:none;border-left:none' >" +  spotting2 + "</td>");

                // if statement setting to "0" above
                Response.Write ("  <td class=xl5520928 style='border-top:none;border-left:none' >" +  spotting3 + "</td>");

                RCs_In   = RCs_In + SafeDbl(spotting1);
                RCs_Out  = RCs_Out + SafeDbl(spotting2);
                DownTime = DownTime + SafeDbl(spotting3);

                //;
                // Spotting Hours;
                //;
                if (bSpottingHours ){
                            if (cInt(rsSpottingHours.Fields(1)) == i ){
                                    spottingHours = cStr(rsSpottingHours.Fields(0));
                                    bSpottingHours = rsSpottingHours.Read();

                            }else{
                                    spottingHours = "";
                           }
                    }else{
                            spottingHours = "";
                   }

                Response.Write ("  <td class=xl5820928 style='border-top:none;border-left:none' align='right'>" +  FNum(cStr(SafeDbl(spottingHours)), 2) + "</td>");

                SpottingTotalHours = SafeDbl(cStr(SpottingTotalHours)) + SafeDbl(spottingHours);

                if(SafeDbl(spottingHours) != 0){
                      double RC_Hr = SafeDiv(cStr(SafeDbl(spotting1) + SafeDbl(spotting1)) , cStr(SafeDbl(spottingHours)));
                      Response.Write ("  <td class=xl4620928 style='border-top:none;border-left:none' >" +  FNum(cStr(RC_Hr), 2) + "</td>");
                }else{
                      Response.Write ("  <td class=xl4620928 style='border-top:none;border-left:none' >0</td>");
               }



                //;
                // Training Hours;
                //;
                if (bTrainingHours ){
                            if (cInt(rsTrainingHours.Fields(1)) == i ){
                                    trainingHours = cStr(rsTrainingHours.Fields(0));
                                    bTrainingHours = rsTrainingHours.Read();

                            }else{
                                    trainingHours = "0";
                           }
                    }else{
                            trainingHours = "0";
                   }

                Response.Write ("  <td class=xl5820928 style='border-top:none;border-left:none'>" + trainingHours + "</td>");
                TrainingTotalHours = SafeDbl(cStr(TrainingTotalHours)) + SafeDbl(trainingHours);

                //;
                // Clerical Hours;
                //;
                if (bClericalHours ){
                            if (cInt(rsClericalHours.Fields(1)) == i ){
                                    clericalHours = cStr(rsClericalHours.Fields(0));
                                    bClericalHours = rsClericalHours.Read();

                            }else{
                                    clericalHours = "0";
                           }
                    }else{
                            clericalHours = "0";
                   }

                Response.Write ("  <td class=xl5620928 style='border-top:none;border-left:none'>" + clericalHours + "</td>");
                ClericalTotalHours = SafeDbl(cStr(ClericalTotalHours)) + SafeDbl(clericalHours);

                //;
                    // Misc. (Rebilling) Hours;
                //;
                if (bRBHours ){
                            if (cInt(rsRBHours.Fields(1)) == i ){
                                    RBHours = cStr(rsRBHours.Fields(0));
                                    bRBHours = rsRBHours.Read();

                            }else{
                                    RBHours = "0";
                           }
                    }else{
                            RBHours = "0";
                   }

                Response.Write ("  <td class=xl5520928 style='border-top:none;border-left:none'>" + RBHours + "</td>");
                RBTotalHours = SafeDbl(cStr(RBTotalHours)) + SafeDbl(RBHours);

                //;
                    // UA Hours;
                //;
                if (bUAHours ){
                            if (cInt(rsUAHours.Fields(1)) == i ){
                                    UAHours = cStr(rsUAHours.Fields(0));
                                    bUAHours = rsUAHours.Read();

                            }else{
                                    UAHours = "0";
                           }
                    }else{
                            UAHours = "0";
                   }

                Response.Write ("  <td class=xl5520928 style='border-top:none;border-left:none'>" + UAHours + "</td>");
                UATotalHours = SafeDbl(cStr(UATotalHours)) + SafeDbl(UAHours);

                //;
                // Self Audits;
                //;
                if (bSelfAudit)
                {
                    var rsSelfAuditFields0 = "0";
                    if (rsSelfAudit.Fields(0) != "0")
                    {
                        rsSelfAuditFields0 = SafeDbl(rsSelfAudit.Fields(0)).ToString("#.##");
                    }
                    Response.Write ("  <td class=xl5820928 style='border-top:none;border-left:none' >" + rsSelfAuditFields0 + "&nbsp;</td>");

                    Total_SA_Bay = SafeDbl(cStr(Total_SA_Bay)) + SafeDbl(rsSelfAudit.Fields(0));
                    Total_SC_Bay = SafeDbl(cStr(Total_SC_Bay)) + SafeDbl(rsSelfAudit.Fields(4));

                    var rsSelfAuditFields2 = "0";
                    if (Convert.ToDouble(rsSelfAudit.Fields(2)) > 0)
                    {
                        rsSelfAuditFields2 = SafeDbl(rsSelfAudit.Fields(2)).ToString("#.##");
                    }
                    Response.Write ("  <td class=xl5820928 style='border-top:none;border-left:none' >" + rsSelfAuditFields2 + "&nbsp;</td>");

                    var rsSelfAuditFields1 = "0";
                    if (rsSelfAudit.Fields(1) != "0")
                    {
                        rsSelfAuditFields1 = SafeDbl(rsSelfAudit.Fields(1)).ToString("#.##");
                    }
                    Response.Write ("  <td class=xl6720928 style='border-top:none;border-left:none' >" + rsSelfAuditFields1 + "&nbsp;</td>");

                    Total_SA_Fac = SafeDbl(cStr(Total_SA_Fac)) + SafeDbl(rsSelfAudit.Fields(1));
                    Total_SC_Fac = SafeDbl(cStr(Total_SC_Fac)) + SafeDbl(rsSelfAudit.Fields(5));
                    var rsSelfAuditFields3 = "0";
                    if (Convert.ToDouble(rsSelfAudit.Fields(3)) > 0)
                    {
                        rsSelfAuditFields3 = SafeDbl(rsSelfAudit.Fields(3)).ToString("#.##");
                    }
                    Response.Write ("  <td class=xl5620928 style='border-top:none;border-left:none' >" + rsSelfAuditFields3 + "&nbsp;</td>");

                    bSelfAudit = rsSelfAudit.Read();
               }

                Response.Write ("</tr>");
              }
        }

        /// <summary>
        /// This writes the totals row for the top grid.
        /// </summary>
        public void WriteTopTotal()
        {
            Response.Write("<tr height=18 style='height:13.5pt'>");
            Response.Write("  <td height=18 class=xl4120928 style='height:13.5pt;border-top:none;border-right:.5pt solid black;'><b>Total</b></td>");
            //;
            // UnLoading;
            //;
            Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + unLoadTotalHours.ToString("#.##") + "</td>");
            Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + cStr(unLoadTotalUnits) + "</td>");
            if (SafeDbl(cStr(unLoadTotalHours)) != 0 && SafeDbl(cStr(unLoadTotalUnits)) != 0)
            {
                var unloadTotals = SafeDiv(cStr(unLoadTotalUnits), cStr(unLoadTotalHours));
                Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;' >" + unloadTotals.ToString("#.##") + "</td>");
            }
            else
            {
                Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;'>&nbsp;</td>");
            }
            //;
            // Loading;
            //;
            var loadTotalHours = "0";
            if (LoadTotalHours > 0)
            {
                loadTotalHours = LoadTotalHours.ToString("#.##");
            }
            Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + loadTotalHours + "</td>");
            Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + cStr(LoadTotalUnits) + "</td>");
            if (SafeDbl(cStr(LoadTotalHours)) != 0 && SafeDbl(cStr(LoadTotalUnits)) != 0)
            {
                var loadTotals = SafeDiv(cStr(LoadTotalUnits), cStr(LoadTotalHours));
                Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;' >" + loadTotals.ToString("#.##") + "</td>");
            }
            else
            {
                Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;'>0</td>");
            }
            //;
            // Total Units / Man;
            //;
            if (SafeDbl(cStr(nUMTotals)) != 0)
            {
                UMTotals = SafeDbl(cStr(UMTotals)) / SafeDbl(cStr(nUMTotals));
                Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;' >" + UMTotals.ToString("#.##") + "</td>");
            }
            else
            {
                Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;'>0</td>");
            }

            //;
            // Shuttling;
            //;
            var shuttlingTotalHours = "0";
            if (ShuttlingTotalHours > 0)
            {
                shuttlingTotalHours = ShuttlingTotalHours.ToString("#.##");
            }
            Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;' align='right'>" + shuttlingTotalHours + "</td>");

            var shuttlingTotalUnits = "0";
            if (ShuttlingTotalUnits > 0)
            {
                shuttlingTotalUnits = ShuttlingTotalUnits.ToString("#.##");
            }
            Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;' align='right'>" + shuttlingTotalUnits + "</td>");
            if (SafeDbl(cStr(ShuttlingTotalUnits)) != 0 && SafeDbl(cStr(ShuttlingTotalHours)) != 0)
            {
                Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;' align='right'>" + FNum(cStr(SafeDiv(cStr(ShuttlingTotalUnits), cStr(ShuttlingTotalHours))), 2) + "</td>");
            }
            else
            {
                Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;'>0</td>");
            }
            //;
            // Spotting;
            //;
            var rCsIn = "0";
            if (RCs_In > 0)
            {
                rCsIn = RCs_In.ToString("0");
            }
            Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + rCsIn + "</td>");

            var rCsOut = "0";
            if (RCs_Out > 0)
            {
                rCsOut = RCs_Out.ToString("0");
            }
            Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + rCsOut + "</td>");

            var downTime = "0";
            if(DownTime > 0)
            {
                downTime = DownTime.ToString("#.##");
            }
            Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + downTime + "</td>");

            var spottingTotalHours = "0";
            if (SpottingTotalHours > 0)
            {
                spottingTotalHours = SpottingTotalHours.ToString("#.##");
            }
            Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + spottingTotalHours + "</td>");

            if (SpottingTotalHours > 0)
            {
                Total_RC_Hr = (RCs_In + RCs_Out) / SpottingTotalHours;
                Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + Total_RC_Hr.ToString("#.##") + "</td>");
            }
            else
            {
                Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;'>0</td>");
            }
            //;
            // Miscellaneous (Training, Clerical, RB, UA);
            //;
            Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + TrainingTotalHours.ToString("#.##") + "</td>");
            var clericalTotalHours = "0";
            if(ClericalTotalHours > 0)
            {
                clericalTotalHours = ClericalTotalHours.ToString("#.##");
            }
            Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + clericalTotalHours + "</td>");
            Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + RBTotalHours.ToString("#.##") + "</td>");

            var uATotalHours = "0";
            if (UATotalHours > 0)
            {
                uATotalHours = UATotalHours.ToString("#.##");
            }
            Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + uATotalHours + "</td>");
            //;
            // Sealf-Audits;
            //;
            var totalSABay = "0";
            if (Total_SA_Bay > 0)
            {
                totalSABay = Total_SA_Bay.ToString("#");
            }
            Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + totalSABay + "</td>");
            Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + FNum(cStr(SafeDiv(FNum(cStr(Total_SC_Bay), 2), cStr(Total_SA_Bay))), 2).Replace("$", "") + "</td>");

            var totalSAFac = "0";
            if (Total_SA_Fac > 0)
            {
                totalSAFac = Total_SA_Fac.ToString("#");
            }
            Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + totalSAFac + "</td>");
            Response.Write("  <td class=xl9120928 style='border-right:.5pt solid black;'>" + FNum(cStr(SafeDiv(FNum(cStr(Total_SC_Fac), 2), cStr(Total_SA_Fac))), 2).Replace("$", "") + "</td>");
            Response.Write("</tr>");
            //;
            Response.Write("<tr height=18 style='height:13.5pt'>");
            Response.Write("  <td height=18 class=xl9120928 style='height:13.5pt;border-right:none;border-top:none'>&nbsp;</td>");
            Response.Write("  <td class=xl9120928 colspan=23 style='border-right:none' >&nbsp;</td>");
            Response.Write("  <td class=xl1520928 style='border-right:none' >&nbsp;</td>");
            Response.Write("</tr>");
        }

        public void WriteBottomRows(){

              string rc = "";

              for( int i=1 ; i <= 12; i ++){
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
                if ( bKPI) {
                            if (rsKPI.Fields(4) == cStr(i) ){
                                    KPI1 = cStr(rsKPI.Fields(0));
                                    KPI2 = cStr(rsKPI.Fields(1));
                                    KPI3 = cStr(rsKPI.Fields(2));
                                    KPI4 = cStr(rsKPI.Fields(3));

                                    bKPI = rsKPI.Read();

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

   if( bTotalUnits){
                if(rsTotalUnits.Fields(1) == cStr(i)){
                        totalUnits = cStr(rsTotalUnits.Fields(0));
                        bTotalUnits = rsTotalUnits.Read();
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


    //;
    // Budget / Cost Per Unit;
    //;
    double TotalLabor = 0.0;
    string CPU = "";
    if(bBudgetCPU){
      Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none' colspan=2 align='right'>" + rsBudgetCPU.Fields(0) + "</td>");
      Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none' colspan=2 align='right'>" +  FCur(rsBudgetCPU.Fields(1), 2) + "</td>");
      Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none' colspan=2 align='right'>" +  FCur(rsBudgetCPU.Fields(2), 2) + "</td>");

      BudgetTotalUnits = SafeDbl(cStr(BudgetTotalUnits)) + SafeDbl(rsBudgetCPU.Fields(0));
      BudgetTotalHours = SafeDbl(cStr(BudgetTotalHours)) + SafeDbl(rsBudgetCPU.Fields(1));
      BudgetTotalOTHrs = SafeDbl(cStr(BudgetTotalOTHrs)) + SafeDbl(rsBudgetCPU.Fields(2));
      TotalLabor = SafeDbl(rsBudgetCPU.Fields(1)) + SafeDbl(rsBudgetCPU.Fields(2));
      if(SafeDbl(rsBudgetCPU.Fields(0)) != 0 && TotalLabor != 0){
        CPU = cStr(SafeDiv(cStr(TotalLabor), cStr(SafeDbl(rsBudgetCPU.Fields(0)))));
        CPU = FCur(CPU, 2);
      }else{
        CPU = "&nbsp;";
     }

      bBudgetCPU = rsBudgetCPU.Read();

   }

    Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none' colspan=2 align='right'>" +  cStr(TotalLabor) + "</td>");
    Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none' colspan=2 align='right'>" +  CPU + "</td>");


    //;
    // Air Test;
    //;
    if(bATUnits){
                if(rsATUnits.Fields(1) == cStr(i)){
                        ATUnits = cStr(rsATUnits.Fields(0));
                        bATUnits = rsATUnits.Read();

                }else{
                        ATUnits = "";
               }
        }else{
                ATUnits = "";
       }

    if(bATHours){
                if(rsATHours.Fields(1) == cStr(i)){
                        ATHours = cStr(rsATHours.Fields(0));
                        bATHours = rsATHours.Read();

                }else{
                        ATHours = "";
               }
        }else{
                ATHours = "";
       }

    Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none; text-align:right'>" + FNum(cStr(SafeDbl(ATUnits)), 0) + "</td>");
    Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none; text-align:right'>" + FNum(cStr(SafeDbl(ATHours)), 2) + "</td>");

    ATTotalUnits = SafeDbl(cStr(ATTotalUnits)) + SafeDbl(ATUnits);
    ATTotalHours = SafeDbl(cStr(ATTotalHours)) + SafeDbl(ATHours);

    if(SafeDbl(ATUnits) != 0 && SafeDbl(ATHours) != 0){
       Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none; text-align:right' >" + FNum(cStr(SafeDiv(cStr(SafeDbl(cStr(ATUnits))) , cStr(SafeDbl(cStr(ATHours))))), 2) + "</td>");
    }else{
       Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none; text-align:right' >&nbsp;</td>");
   }



    // Prepping;
    //;
    //  oCPU.Fields(0).Value = rsPrepUnits.Fields(0).Value;
    //  oCPU.Fields(1).Value = rsPrepHours.Fields(0).Value;
    //;
    string PrepUnits,PrepHours;
    if(bPrepUnits){
                if(rsPrepUnits.Fields(1) == cStr(i)){
                        PrepUnits = cStr(rsPrepUnits.Fields(0));

                        bPrepUnits = rsPrepUnits.Read();

                }else{
                        PrepUnits = "";
               }
        }else{
                PrepUnits = "";
       }

    if(bPrepHours){
                if(rsPrepHours.Fields(1) == cStr(i)){
                        PrepHours = cStr(rsPrepHours.Fields(0));
                        bPrepHours = rsPrepHours.Read();

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

    if(SafeDbl(PrepUnits) != 0 && SafeDbl(PrepHours) != 0){
        Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none; text-align:right' >" + FNum(cStr(SafeDiv(cStr(SafeDbl(PrepUnits)), cStr(SafeDbl(PrepHours)))), 2) + "</td>");
    }else{
        Response.Write("  <td class=xl11020928 style='border-right:.5pt solid black; border-left:none; text-align:right' >&nbsp;</td>");
    }

    Response.Write("</tr>");

  }

}

        public void WriteBottomTotal(){

          Response.Write("<tr height=18 style='height:13.5pt'>");
          Response.Write("  <td height=18 class=xl4120928 style='height:13.5pt;border-top:none;border-right:.5pt solid black;'><b>Total</b></td>");
          Response.Write("  <td class=xl9120928 >" + FNum(cStr(Total_Accidents), 0) + "</td>");
           //Response.Write("  <td class=xl9120928 >" + FNum(Ave(arrTotalAccidents),2) + "</td>");
          Response.Write("  <td class=xl9120928 >" + FNum(cStr(SafeDiv(FNum(cStr(Total_Accidents), 0),(FNum(cStr(BudgetTotalUnits), 0)))*10000),2) + "</td>");
  
          Response.Write("  <td class=xl9120928 >" + FNum(cStr(Total_Injuries), 0) + "</td>");
          //Response.Write("  <td class=xl9120928 >" +  FNum(Ave(arrTotalInjuryRep),2) + "</td>";
          Response.Write("  <td class=xl9120928 >" + FNum(cStr(SafeDiv(FNum(cStr(Total_Injuries), 0),(FNum(cStr(BudgetTotalUnits), 0)))*10000),2) + "</td>");
	
          Response.Write("  <td class=xl9120928 >" + FNum(cStr(Total_LostTime), 2) + "</td>");
          //Response.Write("  <td class=xl9120928 >" +  FNum(Ave(arrTotalLostTime),2) + "</td>";
          Response.Write("  <td class=xl9120928 >" + FNum(cStr(SafeDiv(FNum(cStr(Total_LostTime), 2),(FNum(cStr(BudgetTotalUnits), 0)))*10000),2) + "</td>");
  
          Response.Write("  <td class=xl9120928 >" + FNum(cStr(Total_NonMed), 0) + "</td>");
          //Response.Write("  <td class=xl9120928 >" +  FNum(Ave(arrTotalNonMed),2) + "</td>";
          Response.Write("  <td class=xl9120928 >" + FNum(cStr(SafeDiv(FNum(cStr(Total_NonMed), 0),(FNum(cStr(BudgetTotalUnits), 0)))*10000),2) + "</td>");

          Response.Write("  <td class=xl9120928 colspan=2 >" + cStr(BudgetTotalUnits) + "</td>");
          Response.Write("  <td class=xl9120928 colspan=2 >" + FCur(cStr(BudgetTotalHours), 2) + "</td>");
          Response.Write("  <td class=xl9120928 colspan=2 >" + FCur(cStr(BudgetTotalOTHrs), 2) + "</td>");
          double BudgetTotalLabor = BudgetTotalHours+BudgetTotalOTHrs;
          Response.Write("  <td class=xl9120928 colspan=2 >" + FCur(cStr(BudgetTotalLabor), 2) + "</td>");
          if(BudgetTotalUnits != 0 && BudgetTotalLabor != 0){
            Response.Write("  <td class=xl9120928 colspan=2 >" + FCur(cStr(BudgetTotalLabor/BudgetTotalUnits), 2) + "</td>");
          }else{
            Response.Write("  <td class=xl9120928 colspan=2 >&nbsp;</td>");
         }

          //;
          // Air Test;
          //;
          Response.Write("  <td class=xl9120928           >" + FNum(cStr(ATTotalUnits), 0) + "</td>");
          Response.Write("  <td class=xl9120928           >" + FNum(cStr(ATTotalHours), 2) + "</td>");
          if(ATTotalUnits != 0 && ATTotalHours != 0){
            Response.Write("  <td class=xl9120928>" + FNum(cStr(ATTotalUnits/ATTotalHours), 2) + "</td>");
          }else{
            Response.Write("  <td class=xl9120928>&nbsp;</td>");
         }

          //;
          // Prepping;
          //;
          Response.Write("  <td class=xl9120928           >" + FNum(cStr(PrepTotalUnits), 0) + "</td>");
          Response.Write("  <td class=xl9120928           >" + FNum(cStr(PrepTotalHours), 2) + "</td>");
          if(PrepTotalUnits != 0 && PrepTotalHours != 0){
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