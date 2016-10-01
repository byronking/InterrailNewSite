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
    public partial class FacilityWeeklyCPU : PageBase
    {
        public DataReader rs, rsPay, rsUnits, rsCPU, rsFacTasks, rsTaskPay;
        public string selYear, sFirstDay, sLastDay, sFac;
        public string[,] arRowHeader = new string[53, 4];
        public string rc;

        public string Page2Data = "";
        public string PageOneLines = "";
        public string VarianceYTD = "";
        public string AverageCPU ="";
        public double fBudgetedCPU = 0.0;

        public int vbSunday = 1;
        public int vbMonday = 2;
        public int vbTuesday = 3;
        public int vbWednesday = 4;
        public int vbThursday = 5;
        public int vbFriday = 6;
        public int vbSaturday = 7; 

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            GrantAccess("Super, Admin, User");

            selYear = Request["selYear"];
            sFac    = cStr(Session["FacilityID"]);

            int i = 0;
            //;
            // First get the First day && Last day for( the weekly CPU range;
            // First Friday of the year && last Thursday following Last Friday of the year;
            //;

            sFirstDay = cStr(BeginningYTD("1/15/" + selYear));
            sLastDay  = cStr(LastFriday("12/1/" + selYear));

            string strSQL = " SELECT IsNull(SUM(BudgetedCPU), 0) AS BudgetedCPU FROM  FacilityAnnualBudgetTask Where facilityID = " + sFac + " And ReportingYear = '" + selYear + "' ";
            rsCPU = new DataReader(strSQL);
            rsCPU.Open();

            if (rsCPU.Read())
            {
                fBudgetedCPU = cDbl(rsCPU.Item("BudgetedCPU")) + 0.0;
            }
            else
            {
                fBudgetedCPU = 0.0;
            }

            //;
            // Get Total Units for( Load && Unload Tasks;
            //;
            rsUnits = new DataReader(getSQL(sFac, sFirstDay, sLastDay, "'LO', 'UL', 'SP'", "UNITS")); // Including the SP units for facilities that do not load (LO) or unload (UL).
            //rsUnits = new DataReader(getSQL(sFac, sFirstDay, sLastDay, "'LO', 'UL'", "UNITS"));
            rsUnits.Open();

            bool bHaveData = false;

            while (rsUnits.Read())
            {
                if (rsUnits.Item("TotalUnits") == "" || cInt(rsUnits.Item("TotalUnits")) > 0)                
                {
                    bHaveData = true;
                }
            } //End Loop

            if (bHaveData)
            {
                rsUnits.Requery();
                rsUnits.Read();
            }
            else
            {
                rsUnits  = new DataReader(getSQL(sFac, sFirstDay, sLastDay, "'SP'", "UNITS"));
                rsUnits.Open();
                rsUnits.Read();
            }

            //;
            // Get all OTHER tasks (exclude LO && UL) for( this facility;
            //;
            strSQL =   " Select TASK=RTrim(TaskCode) + ' - ' + RTrim(TaskDescription) ";
            strSQL +=  " FROM FacilityTasks INNER JOIN Tasks ON TaskId = Tasks.Id ";
            strSQL +=  " WHERE TaskCode NOT IN ('LO', 'UL') ";
            strSQL +=  " AND FacilityID = " + sFac;
            strSQL +=  " Order By TaskCode ";

            rsCPU = new DataReader(strSQL);
            rsCPU.Open();
              
            //string[] arFac = new string[18];
            //for(i=0 ; i < UBound(arFac); i++){
            //for (i = 0; i < arFac.Count; i++)
            //{
            //  arFac[i] = "";
            //}

            //if (bHaveData){
            //  arFac[0] = "UL - Unload";
            //  arFac[1] = "LO - Load";
            //  i=2;
            //}else{
            //  i=0;
            //}

            //while (rsCPU.Read() ){
            //    arFac[i] = rsCPU.Item("Task");
            //    i=i+1;
            //} //End Loop

            var arFac = new List<string>();

            while (rsCPU.Read())
            {
                arFac.Add(rsCPU.Item("Task"));
            }

            int nFacTasks = i;

            // Get Total Pay for( ALL Tasks;
            rsPay = new DataReader(getSQL(sFac, sFirstDay, sLastDay, "ALL", "PAY"));
            rsPay.Open();
            rsPay.Read();

            //;
            //Get Pay for( all tasks;
            string[,] arData = new string[52, (nFacTasks+1)*2];
            string[] arFactors = new string[52];
            int nFacTasksWithValues = nFacTasks;

            string[] arFacWithValues = new string[19];

            for( i=0; i < UBound(arFacWithValues); i++)
            {
                arFacWithValues[i] = "";
            }

            for(i =  0 ; i <= 51; i++)
            {
                for(int j = 0; j < nFacTasks + 1; j++)
                {
                    arData[i,j] = "";
                }
            }

            int k=-1;
            int l=1;

            for(i=0; i < nFacTasks;i++)
            {
                rsTaskPay = new DataReader(getSQL(sFac, sFirstDay, sLastDay, "'" + Left(arFac[i],2) + "'", "PAY"));
                rsTaskPay.Open();

                //;
                // Add TotalPay -;
                //;

                bHaveData = false;

                while (rsTaskPay.Read() )
                {
                  if ( rsTaskPay.Item("TotalPay") == "" || cDbl(rsTaskPay.Item("TotalPay")) > 0)
                  {
                      bHaveData = true;
                  }
                } //End Loop

                int datarow = 0;
                if (bHaveData)
                {
                    rsTaskPay.Requery();
                    while (rsTaskPay.Read())
                    {
                        arData[datarow, l-1] = rsTaskPay.Item("TotalPay");
                        arRowHeader[datarow, 0] = FormatTheDate(cDate(rsTaskPay.Fields(1)).AddDays(6));
                        datarow = datarow + 1;
                    } //End Loop

                    k = k + 1;
                    arFacWithValues[k] = arFac[i];
                    l=l+1;
                }
            }

            CreatePageOneLines();
        }
        
        //Returns Last Thurs. of the Month;
        public DateTime LastFriday(string mydate){

                string tempLastFriday;
                bool blnLastWeekofMonth = false;
                string LastFridayReturn = "";
 
                tempLastFriday = mydate; //DateAdd("d", -Day(mydate) + 1, mydate);
        
                if (Month(cDate(mydate).AddDays(7)) != Month(cDate(mydate))){
                        while( Month(cDate(tempLastFriday)) == Month(cDate(mydate))){
                                if (Weekday(cDate(tempLastFriday)) == vbThursday){
                                        LastFridayReturn = cStr(cDate(mydate));  //Date minus 1 month;
                                }
                                tempLastFriday = cStr(cDate(tempLastFriday).AddDays(1));
                        } //End Loop

                        if (Trim(LastFridayReturn) == ""){
                            LastFridayReturn = Month(cDate(mydate).AddMonths(1)) + "/1/" + Year(cDate(mydate)); //Add a month- date is past last Thurs. of month;
                                if (Month(cDate(LastFridayReturn)) == "1"){
                                        LastFridayReturn =   cStr(cDate(LastFridayReturn).AddYears(1)); 
                                }
                                blnLastWeekofMonth = true;
                        }
                }else{
                        LastFridayReturn =  mydate;
                }

                while( Weekday(cDate(LastFridayReturn)) != vbThursday){
                        LastFridayReturn =  cStr(cDate(LastFridayReturn).AddDays(1));
                } //End Loop

                if (blnLastWeekofMonth == true){
                        while( Month(cDate(LastFridayReturn).AddDays(7)) == Month(cDate(mydate).AddMonths(1))){
                                LastFridayReturn =  cStr(cDate(LastFridayReturn).AddDays(7));
                        } //End Loop
                }else{
                        while( Month(cDate(LastFridayReturn).AddDays(7)) == Month(cDate(mydate))){
                                LastFridayReturn = cStr(cDate(LastFridayReturn).AddDays(7));
                        } //End Loop
                }

                return cDate(LastFridayReturn);
        }

        public DateTime FirstFriday(string mydate){

                if (LastFriday(mydate) >= cDate(mydate)){
                        return LastFriday(cStr(cDate(mydate).AddMonths(-1))).AddDays(1);
                }else{
                        return LastFriday(mydate).AddDays(1);
                }
        }

        //First Friday after the Last Thursday of the Previous Year;
        public DateTime BeginningYTD(string mydate){

                string tempBeginningYTD;
                bool blnLastWeekofYear = false;
                string BeginningYTDReturn = "";
                tempBeginningYTD = mydate;

                if (cDate(mydate) > cDate("12/24/" + Year(cDate(mydate))) && cDate(mydate) <= cDate("12/31/" + Year(cDate(mydate)))){
                        while ( cInt(Year(cDate(tempBeginningYTD))) < cInt(Year(cDate(mydate)))){
                                if (Weekday(cDate(tempBeginningYTD)) == vbThursday){
                                        BeginningYTDReturn = "12/24/" + Year(cDate(mydate).AddYears(-1));     //Date minus 1 year;
                                }
                                tempBeginningYTD = cStr(cDate(tempBeginningYTD).AddDays(1));
                        } //End Loop
                        if (Trim(BeginningYTDReturn) == ""){
                                BeginningYTDReturn = mydate;   //////Do !minus 1 year (ex. a date is 12/29/03 && the last Thursday is 12/23/03);
                                blnLastWeekofYear = true;
                        }
                }else{
                        BeginningYTDReturn = "12/24/" + Year(cDate(mydate).AddYears(-1));  //Date minus 1 year;
                }

                while( Weekday(cDate(BeginningYTDReturn)) != vbThursday)
                {
                    BeginningYTDReturn = cStr(cDate(BeginningYTDReturn).AddDays(1));
                } //End Loop

                if (blnLastWeekofYear == true){
                        while( Year(cDate(BeginningYTDReturn).AddDays(7)) != (Year(cDate(mydate))+1)){
                                BeginningYTDReturn = cStr(cDate(BeginningYTDReturn).AddDays(7));
                        } //End Loop
                        BeginningYTDReturn = cStr(cDate(BeginningYTDReturn).AddDays(-6));
                }else{
                        while( Year(cDate(BeginningYTDReturn).AddDays(7)) != Year(cDate(mydate))){
                                BeginningYTDReturn = cStr(cDate(BeginningYTDReturn).AddDays(7));
                        } //End Loop
                        BeginningYTDReturn = cStr(cDate(BeginningYTDReturn).AddDays(1));
                }

                return cDate(BeginningYTDReturn);
        }

        public string getSQL(string sFac, string sFirstDay, string sLastDay, string sTask, string sPayOrUnits)
        {
            string strSQL = "";
            string sStartDate, sEndDate;

            sStartDate = sFirstDay;
            sEndDate = cStr(cDate(sFirstDay).AddDays(6));

            while(cDate(sEndDate) <= cDate(sLastDay))
            {
                if(sPayOrUnits == "UNITS")
                {
                    //TotalUnits becomes TotalUnits1 && New Sub Select becomes TotalUnits;
                    //strSQL += " SELECT SUM(d.Units) AS TotalUnits, StartDate=Convert(SmallDateTime,'" + sStartDate + "' ,101)";
                    strSQL += " SELECT SUM(d.Units) AS TotalUnits, StartDate=Convert(SmallDateTime,'" + sStartDate + "')";
                    strSQL += " FROM Facility f INNER JOIN FacilityProductionDetail d ON f.Id = d.FacilityID ";
                    strSQL += " INNER JOIN Tasks ON d.TaskId = Tasks.Id  ";
                }
                else
                {
                    //strSQL += " SELECT SUM(PayAmount) AS TotalPay, StartDate=Convert(SmallDateTime,'" + sStartDate + "' ,101),";
                    strSQL += " SELECT SUM(PayAmount) AS TotalPay, StartDate=Convert(SmallDateTime,'" + sStartDate + "'),";
                    strSQL +=  " (SELECT DataValue ";
                    strSQL +=  "  FROM FacilityMonitoringDataEntry ";
                    strSQL +=  " WHERE WorkDate = '" + cStr(sEndDate) + "'";
                    strSQL +=  "   AND FacilityID = " + sFac;
                    strSQL +=  "   AND FieldName = 'DETERMINING_FACTOR') As DeterminingFactor ";
                    strSQL +=  " FROM Facility f INNER JOIN EmployeeTaskWorked ON f.Id = EmployeeTaskWorked.FacilityID ";
                    strSQL +=  "      INNER JOIN Tasks ON EmployeeTaskWorked.TaskID = Tasks.Id   ";
                    strSQL +=  "      LEFT OUTER JOIN EmployeeTaskWorkedPay ON EmployeeTaskWorked.Id = EmployeeTaskWorkedPay.EmployeeTaskWorkedId ";
                }

                if (sTask == "ALL")
                {
                    strSQL += " WHERE (1=1) ";
                }
                else
                {
                    strSQL += " WHERE (Tasks.TaskCode IN (" + sTask + ") ) ";
                }

                strSQL +=  " AND f.ID = " + sFac;
                strSQL +=  " AND (WorkDate Between '" + sStartDate + "' AND '" + sEndDate + "') ";
    
                sStartDate =  cStr(cDate(sEndDate).AddDays(1));
                sEndDate   = cStr(cDate(sStartDate).AddDays(6));

                if (cDate(sEndDate) <= cDate(sLastDay))
                {
                    strSQL +=  " UNION ";
                }
            } //End Loop

            strSQL +=  "ORDER BY StartDate ";

            return strSQL;    
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

        public void WritePageOneLines()
        {
            Response.Write(PageOneLines);
        }

        public void CreatePageOneLines()
        {
            double SumTRCostPerUnit = 0;
            double SumTRPay = 0;
            double SumTRUnits = 0;
            double RowsWithData = 0;

            string TRstartdate = "";
            string TRendDate  = "";
            string TRPay = "";
            string TRUnits = "";
            string TRCostPerUnit = "";
            string TRVariance = "";
            string TRDetermingFactor = "";

            for(int i=1; i <= 52; i++)
            {
                if(i % 2 == 0)
                {
                    rc = "reportEvenLine";
                }
                else
                {
                    rc = "reportOddLine";
                }

                if (rsPay.LastReadSuccess)
                {
                    TRstartdate = FormatTheDate(cDate(rsPay.Fields(1)));
                    TRendDate = FormatTheDate(cDate(rsPay.Fields(1)).AddDays(6));
                    TRPay = rsPay.Fields(0);
                    arRowHeader[i, 0] = TRendDate;
                    arRowHeader[i, 1] = TRPay;
                    TRDetermingFactor = rsPay.Item("DeterminingFactor");
                    rsPay.Read();
                }

                if (rsUnits.LastReadSuccess)
                {
                    TRUnits = rsUnits.Fields(0);
                    arRowHeader[i, 2] = TRUnits;
                    rsUnits.Read();
                }

                TRCostPerUnit = "";
                TRVariance = "";
                if((TRPay != null) && (TRUnits != null))
                {

                    if (isNumeric(TRUnits) && cDbl(TRUnits) > 0) 
                    {
                        TRCostPerUnit = cStr(cDbl(TRPay) / cDbl(TRUnits));
                        TRVariance = cStr(cDbl(fBudgetedCPU) - cDbl(TRCostPerUnit));
                        RowsWithData = RowsWithData + 1;
                        SumTRCostPerUnit = SumTRCostPerUnit + cDbl(TRCostPerUnit);
                        SumTRPay = SumTRPay + cDbl(TRPay);
                        SumTRUnits = SumTRUnits + cDbl(TRUnits);
                    }
                }

                if(TRPay != "")
                {
                    TRPay = FCur(TRPay ,2);
                }
                else
                {
                    TRPay = "";
                }

                if(isNumeric(TRCostPerUnit))
                {
                    TRCostPerUnit = FCur(TRCostPerUnit ,2);
                }

                if(isNumeric(TRVariance))
                {
                    TRVariance = FCur(TRVariance ,2);
                }

                if(isNumeric(TRUnits))
                {
                    TRUnits = cStr(TRUnits);
                }

                PageOneLines = PageOneLines + "<tr class='" + rc + "'>";
                PageOneLines = PageOneLines + "  <td class=xl57 valign='top'>" + TRstartdate + "</td>";
                PageOneLines = PageOneLines + "<td class=xl57 style='border-left:none' valign='top'>" + TRendDate + "</td>";
                PageOneLines = PageOneLines + "<td class=xl43 style='border-left:none' align='right' valign='top'>";
                PageOneLines = PageOneLines +  TRPay;
                PageOneLines = PageOneLines + "</td>";
                PageOneLines = PageOneLines + "<td class=xl56 colspan=2 style='border-left:none' align='right' valign='top'>" + TRUnits + "</td>";
                PageOneLines = PageOneLines + "<td class=xl50 colspan=2 style='border-left:none' align='right' valign='top'>" + TRCostPerUnit;
                PageOneLines = PageOneLines + "</td>";
                PageOneLines = PageOneLines + "<td class=xl58 style='border-left:none' align='right' valign='top'>" + TRVariance + "</td>";
                PageOneLines = PageOneLines + "<td colspan=10 class='xl75' style='border-left:none' align='left' valign='top'>&nbsp;" + Trim(TRDetermingFactor) + "</td>";
                PageOneLines = PageOneLines + "<td colspan=5 style='mso-ignore:colspan' valign='top' bgcolor='white'></td>";
                PageOneLines = PageOneLines + "<td class=xl44 valign='top' bgcolor='white'></td>";
                PageOneLines = PageOneLines + "</tr>";

            } //Next

            VarianceYTD = "";
            AverageCPU ="";

            if (isNumeric(cStr(RowsWithData)))
            {
                if (RowsWithData > 0)
                {
                    AverageCPU = FCur(cStr(SumTRPay / SumTRUnits), 2);
                    VarianceYTD = FCur(cStr(SafeDbl(cStr(fBudgetedCPU)) - SafeDbl(AverageCPU)), 2);
                }
            }
        }
    }
}