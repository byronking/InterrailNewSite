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
    public partial class FacilityWeeklyCPU2 : PageBase
    {

        public DataReader rs,rsTaskPay;
        public DataReader rsPay, rsUnits, rsCPU, rsFacTasks;
        public string selYear, sFirstDay, sLastDay,sFac;
        public string[,] arRowHeader = new string[53, 4];
        public string strColor;
        public string rc;
        public string pageSetNumber, stringBuffer;
        public int nFacTasks;
        public string[,] arData;
        public string[] arFactors;
        public string[] arFac = new string[19];
        public DataReader rsTBCPU;
        public string[] TaskBudgetedCPU = new string[200];
        public string[] TaskBudgetedCPUTaskCODE = new string[200];
        public string MaxTaskBudgetedCPU, strSQL;

        public string Left3Data = "";
        public string Page2Data = "";
        public string Page3Data = "";
        public string Page4Data = "";
        public string Page5Data = "";

        public string Left3Ave = "";
        public string Page3Ave = "";
        public string Page4Ave = "";
        public string Page5Ave = "";

        public string Left3Tot = "";
        public string Page3Tot = "";
        public string Page4Tot = "";
        public string Page5Tot = "";

        public string Left3TitleLine = "";
        public string Page3TitleLine = "";
        public string Page4TitleLine = "";
        public string Page5TitleLine = "";

        public string PageOneLines = "";
        public string VarianceYTD = "";
        public string AverageCPU = "";
        public double BudgetedCPU = 0.0;
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
            //;
            // First get the First day && Last day for( the weekly CPU range;
            // First Friday of the year && last Thursday following Last Friday of the year;
            //;
            sFirstDay = cStr(BeginningYTD("1/15/" + selYear));
            sLastDay  = cStr(LastFriday("12/1/" + selYear));
  
            strSQL = " SELECT     Tasks.TaskCode, isNull(FacilityAnnualBudgetTask.BudgetedCPU, '') as BudgetedCPU  ";
            strSQL += "FROM         FacilityAnnualBudgetTask INNER JOIN ";
            strSQL += "                    Tasks ON FacilityAnnualBudgetTask.TaskID = Tasks.Id ";
            strSQL += " WHERE     (FacilityAnnualBudgetTask.FacilityId = " + sFac + ") AND (FacilityAnnualBudgetTask.ReportingYear = " + selYear + ") ";

            rsTBCPU = new DataReader(strSQL);
            rsTBCPU.Open();
              
              int i = 0;
              while (rsTBCPU.Read()){
                 TaskBudgetedCPU[i] = rsTBCPU.Item("BudgetedCPU") + "";
                 TaskBudgetedCPUTaskCODE[i] = rsTBCPU.Item ("TaskCode") + "";
                 i = i + 1;
              }

              MaxTaskBudgetedCPU = cStr(i - 1);

              strSQL = " Select IsNull(BudgetedCPU, 0) AS BudgetedCPU From FacilityAnnualBudget Where facilityID = " + sFac;
              rsCPU = new DataReader(strSQL);
              rsCPU.Open();

              if (rsCPU.Read() ){
                 fBudgetedCPU = cDbl(rsCPU.Item("BudgetedCPU"));
              }else{
                 fBudgetedCPU = 0.0;
             }


              //;
              // Get Total Units for( Load && Unload Tasks;
              //;
              rsUnits = new DataReader(getSQL(sFac, sFirstDay, sLastDay, "'LO', 'UL'", "UNITS"));
              rsUnits.Open();
              
              bool bHaveData = false;

              while (rsUnits.Read() ){
                 
                 if ( rsUnits.Item("TotalUnits") == "" || cInt(rsUnits.Item("TotalUnits")) > 0){
                     bHaveData = true;
                 }

              } //End Loop

              if (bHaveData){
                rsUnits.Requery();
                rsUnits.Read();
              }else{
                rsUnits = new DataReader(getSQL(sFac, sFirstDay, sLastDay, "ALL", "UNITcDbl(S"));
                rsUnits.Open();
                rsUnits.Read();
              }

              //;
              // Get all OTHER tasks (exclude LO && UL) for( this facility;
              //;
              strSQL =   " Select TASK=RTrim(TaskCode) + ' - ' + RTrim(TaskDescription) ";
              strSQL +=  "  FROM FacilityTasks INNER JOIN Tasks ON TaskId = Tasks.Id ";
              strSQL +=  " Where TaskCode NOT IN ('LO', 'UL') ";
              strSQL +=  "   AND FacilityID = " + sFac;
              strSQL +=  " Order By TaskCode ";
              rsCPU = new DataReader(strSQL);
              rsCPU.Open();

              for( i=0; i < UBound(arFac); i++){
                arFac[i] = "";
              }

              if (bHaveData){
                arFac[0] = "UL - Unload";
                arFac[1] = "LO - Load";
                i=2;
              }else{
                i=0;
              }

              while (rsCPU.Read() ){
                
                arFac[i] = rsCPU.Item("Task");
                i=i+1;
              } //End Loop

              nFacTasks = i;



              //;
              // Get Total Pay for( ALL Tasks;
              //;
              rsPay = new DataReader(getSQL(sFac, sFirstDay, sLastDay, "ALL", "PAY"));
              rsPay.Open();
              rsPay.Read();

              arData = new string[52, (nFacTasks+1)*2];
              arFactors = new string[52];


              double nFacTasksWithValues = nFacTasks;

              string[] arFacWithValues = new string[19];
              for(i=0 ; i < UBound(arFacWithValues); i++){
                arFacWithValues[i] = "";
              }

              for( i =  0; i <= 51; i++){
                 for( int j = 0 ; j < nFacTasks+1;j++){
                    arData[i,j] = "";
                 }
              }


              int k=-1;
              int l=1;

              for(i=0 ; i < nFacTasks; i++){

                rsTaskPay = new DataReader(getSQL(sFac, sFirstDay, sLastDay, "'" + Left(arFac[i], 2) + "'", "PAY"));
                rsTaskPay.Open();
                // Add TotalPay -;
   
                bHaveData = false;


                int datarow = 0;
                  while (rsTaskPay.Read()){
                    
                    arData[datarow, l-1] = rsTaskPay.Item("TotalPay");
                    arRowHeader[datarow, 0] = FormatTheDate(cDate(rsTaskPay.Fields(1)).AddDays(6));
                    datarow = datarow + 1;

                  } //End Loop
                  k = k + 1;
                  arFacWithValues[k] = arFac[i];
                  l=l+1;



              }

              CreatePageOneLines();
    }


       // Task Headings;

        public string getSQL(string sFac,string  sFirstDay,string  sLastDay,string  sTask,string  sPayOrUnits){

      string strSQL = "";
      string sStartDate, sEndDate;

      sStartDate = sFirstDay;
      sEndDate   =  cStr(cDate(sFirstDay).AddDays(6));

       while (cDate(sEndDate) <= cDate(sLastDay)){

        if(sPayOrUnits == "UNITS"){
           strSQL += " SELECT SUM(d.Units) AS TotalUnits, StartDate=Convert(SmallDateTime,'" + sStartDate + "' ,101)";
           strSQL +=  " FROM Facility f INNER JOIN FacilityProductionDetail d ON f.Id = d.FacilityID ";
           strSQL +=  "      INNER JOIN Tasks ON d.TaskId = Tasks.Id  ";
        }else{
           strSQL += " SELECT SUM(PayAmount) AS TotalPay, StartDate=Convert(SmallDateTime,'" + sStartDate + "' ,101),";
           strSQL +=  "(SELECT DataValue ";
               strSQL +=  "  FROM FacilityMonitoringDataEntry ";
               strSQL +=  " WHERE WorkDate = '" + cStr(sEndDate) + "'";
               strSQL +=  "   AND FacilityID = " + sFac;
               strSQL +=  "   AND FieldName = 'DETERMINING_FACTOR') As DeterminingFactor ";
           strSQL +=  " FROM Facility f INNER JOIN EmployeeTaskWorked ON f.Id = EmployeeTaskWorked.FacilityID ";
           strSQL +=  "      INNER JOIN Tasks ON EmployeeTaskWorked.TaskID = Tasks.Id   ";
           strSQL +=  "      LEFT OUTER JOIN EmployeeTaskWorkedPay ON EmployeeTaskWorked.Id = EmployeeTaskWorkedPay.EmployeeTaskWorkedId ";
       }

        if(sTask == "ALL"){
           strSQL +=  "WHERE (1=1) ";
        }else{
           strSQL +=  "WHERE (Tasks.TaskCode IN (" + sTask + ") ) ";
       }
        strSQL +=  "      AND f.ID = " + sFac;
        strSQL +=  "      AND (WorkDate Between '" + sStartDate + "' And '" + sEndDate + "') ";

        sStartDate = cStr(cDate(sEndDate).AddDays(1));
        sEndDate   = cStr(cDate(sStartDate).AddDays(6));

        if (cDate(sEndDate) <= cDate(sLastDay)){
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

        public void WritePageOneLines(){
              Response.Write(PageOneLines);
        }

        public void  CreatePageOneLines(){

          double SumTRCostPerUnit = 0;
          int RowsWithData = 0;

          for( int i=1; i <= 52; i++){

             string TRstartdate = "";
             string TRendDate  = "";
             string TRPay = "";
             string TRUnits = "";
             string TRCostPerUnit = "";
             string TRVariance = "";
             string TRDetermingFactor = "";

             if (rsPay.LastReadSuccess){
                 TRstartdate = FormatTheDate(cDate(rsPay.Fields(1)));
                 TRendDate = FormatTheDate(cDate(rsPay.Fields(1)).AddDays(6));
                 TRPay = rsPay.Fields(0);
                 arRowHeader[i, 0] = TRendDate;
                 arRowHeader[i, 1] = TRPay;
                 TRDetermingFactor = rsPay.Item("DeterminingFactor");
                 rsPay.Read();

            }
             if (rsUnits.LastReadSuccess){
                 TRUnits = rsUnits.Fields(0);
                 arRowHeader[i, 2] = TRUnits;
                 rsUnits.Read();

            }

             if((TRPay != "") && (TRUnits!= "")){
                 if(cDbl(TRUnits) > 0){
                    TRCostPerUnit = cStr(cDbl(TRPay) / cDbl(TRUnits));
                    TRVariance = cStr(cDbl(fBudgetedCPU) - cDbl(TRCostPerUnit));
                    RowsWithData = RowsWithData + 1;
                    SumTRCostPerUnit = SumTRCostPerUnit + cDbl(TRCostPerUnit);
                }
            }

             if (isNumeric(TRPay)){
                TRPay = FCur(TRPay ,2);
             }else{
                TRPay = "";
            }

             if(isNumeric(TRCostPerUnit)){
                TRCostPerUnit = FCur(TRCostPerUnit ,2);
            }

             if(isNumeric(TRVariance)){
                TRVariance = FCur(TRVariance ,2);
            }

             if(isNumeric(TRUnits)){
                TRUnits = cStr(TRUnits);
            }


              PageOneLines = PageOneLines + "<tr >";
              PageOneLines = PageOneLines + "  <td class=xl57 valign='top'>" + TRstartdate + "</td>";
              PageOneLines = PageOneLines + "<td class=xl57 style='border-left:none' valign='top'>" + TRendDate + "</td>";
              PageOneLines = PageOneLines + "<td class=xl43 style='border-left:none' align='right' valign='top'>";
              PageOneLines = PageOneLines +  TRPay;
              PageOneLines = PageOneLines + "</td>";
              PageOneLines = PageOneLines + "<td class=xl56 style='border-left:none' align='right' valign='top'>" + TRUnits + "</td>";
              PageOneLines = PageOneLines + "<td class=xl50 style='border-left:none' align='right' valign='top'>" + TRCostPerUnit;
              PageOneLines = PageOneLines + "</td>";
              PageOneLines = PageOneLines + "<td class=xl58 style='border-left:none' align='right' valign='top'>" + TRVariance + "</td>";
              PageOneLines = PageOneLines + "<td colspan=5 class=xl75 style='border-left:none' align='left' valign='top'>&nbsp;" + Trim(TRDetermingFactor) + "</td>";
              PageOneLines = PageOneLines + "<td colspan=5 style='mso-ignore:colspan' valign='top'></td>";
              PageOneLines = PageOneLines + "<td class=xl44 valign='top'></td>";
              PageOneLines = PageOneLines + "</tr>";

          } //Next

           VarianceYTD = "";
           AverageCPU ="";

             if (RowsWithData > 0){
                AverageCPU = FCur(cStr(SumTRCostPerUnit / RowsWithData), 2);
                VarianceYTD = FCur(cStr(fBudgetedCPU - SafeDbl(AverageCPU)), 2);
            }

        }

        public void CreatePage2Data(){

            string rc = "";
            double sumrow = 0.0;

           for(int  i = 0; i<= 51;i++){
              if(i % 2 == 0){
                rc = "reportEvenLine";
              }else{
                rc = "reportOddLine";
             }
              sumrow = 0.0;
              for( int j = 0;j<= 19;j++){
                if (j < nFacTasks){

                      sumrow = sumrow + SafeDbl(arData[i,j]);
               }
              }//Next

              arRowHeader[i+1,3] = cStr(sumrow);

              Left3Data =    "<tr height=17 style='height:12.75pt' class='" + rc + "'>";
              Left3Data =    Left3Data + "<td height=17 class=xl53 style='height:12.75pt'>" + arRowHeader[i+1,0] + "</td>";
              Left3Data =    Left3Data + "<td class=xl43 s   tyle='border-left:none' align='right'>" + FCurSP(cStr(sumrow)) + "</td>";
              Left3Data =    Left3Data + "<td class=xl50 style='border-left:none' align='right'>" + FCurSP(cStr(SafeDbl(arRowHeader[i+1,1]) - sumrow)) + "</td>";
                  stringBuffer = stringBuffer + Left3Data;

              for( int j = 0 ; j <= 5;j++){
                if (j < nFacTasks){
                      stringBuffer = stringBuffer + "  <td class=xl43 style='border-left:none' align='right'>" + FCurSP(cStr(SafeDbl(arData[i,j]))) + "</td>";
                      if (SafeDbl(arRowHeader[i+1,2]) > 0){
                         stringBuffer =    stringBuffer + "  <td class=xl43 style='border-left:none' align='right'>" + FCurSP( cStr(SafeDbl(arData[i,j]) / SafeDbl(arRowHeader[i+1,2]))) + "</td>";
                      }else{
                         stringBuffer =    stringBuffer + "  <td class=xl43 style='border-left:none'>&nbsp;</td>";
                     }
               }
              } //Next

              for(int j = 6; j <= 11;j++){
                if( j == 6) {
                        Page3Data = Page3Data + "<tr height=17 style='height:12.75pt' class='" + rc + "'> <td height=17 class=xl53 style='height:12.75pt'>" + arRowHeader[i+1,0] + "</td>";
               }
                if (j < nFacTasks){
                      Page3Data = Page3Data + "  <td class=xl43 style='border-left:none' align='right'>" + FCurSP(cStr(SafeDbl(arData[i,j]))) + "</td>";
                      if (SafeDbl(arRowHeader[i+1,2]) > 0){
                         Page3Data =    Page3Data + "  <td class=xl43 style='border-left:none' align='right'>" + FCurSP(cStr(SafeDbl(arData[i,j])/SafeDbl(arRowHeader[i+1,2]))) + "</td>";
                      }else{
                         Page3Data =    Page3Data + "  <td class=xl43 style='border-left:none'>&nbsp;</td>";
                     }
               }
                if (j == 11){
                        Page3Data = Page3Data + "</tr>";
               }
              } //Next

              for( int j = 12; j <= 19;j++){
                if ( j == 12) {
                        Page4Data = Page4Data + "<tr height=17 style='height:12.75pt' class='" + rc + "'>  <td height=17 class=xl53 style='height:12.75pt'>" + arRowHeader[i+1,0] + "</td>";
               }
                if (j < nFacTasks) {
                      Page4Data =  Page4Data + Left3Data +  "  <td class=xl43 style='border-left:none' align='right'>" + FCurSP(cStr(SafeDbl(arData[i,j]))) + "</td>";
                      if (SafeDbl(arRowHeader[i+1,2]) > 0){
                         Page4Data =    Page4Data + "  <td class=xl43 style='border-left:none' align='right'>" + FCurSP(cStr(SafeDbl(arData[i,j]) / SafeDbl(arRowHeader[i+1,2]))) + "</td>";
                      }else{
                         Page4Data =    Page4Data + "  <td class=xl43 style='border-left:none'>&nbsp;</td>";
                     }
               }
                if (j == 19){
                        Page4Data = Page4Data + "</tr>";
               }
              } //Next

              Left3Data = "";

           } //Next

           Page2Data =    stringBuffer +  "</tr>";
           //Page3Data =    Page3Data +  "</tr>";
           //Page4Data =    Page4Data +  "</tr>";

        }

        public void WritePage2Ave(){

              string Page2Ave = "";
              string stringBuffer = "";

              Page2Ave =    Page2Ave +   "<tr class=xl45 height=17 style='height:12.75pt;border:1'>";
              Page2Ave =    Page2Ave +   "<td height=17 class=xl2222226 style='height:12.75pt'><b>&nbsp;</b></td>";
              Left3Ave = Page2Ave;

              double localAve = 0;
              double localcount = 0;
              double locallast = 0;

              for(int i = 1; i<= 52; i++){
                locallast = SafeDbl(arRowHeader[i,3]);
                localAve = localAve + locallast;
                if (locallast != 0){
                   localcount = localcount + 1;
                }
              } //Next

              if (localcount > 0){
                 localAve = localAve / localcount;
              }

              Page2Ave =    Page2Ave +  "<td class=xl2222225 align='right' bgcolor='#99CCFF'>&nbsp;</td>";

              localAve = 0;
              localcount = 0;

              for( int i = 1; i <= 52; i++){
                locallast = SafeDbl(arRowHeader[i,1]) - SafeDbl(arRowHeader[i,3]);
                localAve = localAve + locallast;
                if (locallast != 0){
                   localcount = localcount + 1;
               }
              }//Next
              if (localcount > 0){
                 localAve = localAve / localcount;
             }

              Page2Ave =    Page2Ave +  "<td class=xl2222225 align='right' bgcolor='#FFFF99'>&nbsp;</td>";


              for( int j = 0 ; j <= 19; j++){
                 if ( j < nFacTasks){

                    localAve = 0;
                    localcount = 0;
                    locallast = 0;

                    for( int i = 1; i <= 52;i++){
                      locallast = SafeDbl(arData[i-1,j]);
                      localAve = localAve + locallast;
                      if (locallast != 0){
                         localcount = localcount + 1;
                      }
                    } //Next

                    if (localcount > 0){
                       localAve = localAve / localcount;
                    }
                    if ( j % 2 == 0){
                            strColor = "#99CCFF";
                    }else{
                        strColor = "#FFFF99";
                    }


                    string strTaskCodeBCPU = Left(arFac[j],2);
            
                    string strBCPU = "";
            
                    for( int k = 0; k <= cInt(MaxTaskBudgetedCPU); k++){
                        if ( Trim(cStr(TaskBudgetedCPUTaskCODE[k] + "")) == strTaskCodeBCPU){
                            strBCPU = TaskBudgetedCPU[k];
                        }
                    } //Next

                    if ( strBCPU == "0"){
                       strBCPU = "";
                    }else{
                       strBCPU = "$ " + strBCPU;
                    } 
            
           
                    stringBuffer =    stringBuffer +  "<td class=xl2222225 align='right' bgcolor='" + strColor + "'>" + FCurSP(cStr(localAve)) + "</td>";
                    localAve = 0;
                    localcount = 0;

                    for(int i = 1; i <= 52; i++){
                      locallast = 0;
                      if (SafeDbl(arRowHeader[i,2]) != 0){
                          locallast = SafeDbl(arData[i-1,j])/SafeDbl(arRowHeader[i,2]);
                      }
                      localAve = localAve + locallast;
                      if (locallast != 0){
                         localcount = localcount + 1;
                      }
                    }//Next

                    if (localcount > 0){
                       localAve = localAve / localcount;
                    }
            
            
                    stringBuffer =    stringBuffer +  "<td class=xl2222225 align='right' bgcolor='" + strColor + "'>" + strBCPU + "</td>";
            
                }

                 if (j == 5){
                        Page2Ave = Page2Ave + stringBuffer;
                        stringBuffer = "";
                 }else{
                     if (j == 11){
                        Page3Ave = stringBuffer;
                        stringBuffer = "";
                    }else{
                         if (j == 19){
                            Page4Ave = stringBuffer;
                            stringBuffer = "";
                         }
                    }
                 }

                 } //Next

              Page2Ave =    Page2Ave +   "</tr>";
              Page3Ave =    Left3Ave + Page3Ave +   "</tr>";
              Page4Ave =    Left3Ave + Page4Ave +   "</tr>";

              Response.Write(Page2Ave);

        }

        public void WritePage3Ave(){
                Response.Write( Page3Ave);
        }

        public void WritePage4Ave(){
                Response.Write( Page4Ave);
        }

        public void WritePage2Totals(){

              string Page2Tot = "";
              string stringBuffer = "";

              Page2Tot =    Page2Tot +   "<tr class=xl51 height=17 style='height:12.75pt'>";
              Page2Tot =    Page2Tot +   "<td height=17 class=xl52 style='height:12.75pt;border-bottom:none'><b>Total</b></td>";
              Left3Tot = Page2Tot;

              double localsum = 0;
              double locallast = 0;

              for( int i = 1;i <= 52; i++){
                locallast = SafeDbl(arRowHeader[i,3]);
                localsum = localsum + locallast;
              } //Next

              Page2Tot =    Page2Tot +   "<td class=xl52 align='center'><b>" + FCurSP(cStr(localsum)) + "</b></td>";

              localsum = 0;
              for(int i = 1; i <= 52;i++){
                locallast =  SafeDbl(arRowHeader[i,1]) - SafeDbl(arRowHeader[i,3]);
                localsum = localsum + locallast;
              } //Netx
              Page2Tot = Page2Tot +   "<td class=xl52 align='center'><b>" + FCurSP(cStr(localsum)) + "</b></td>";
              //Left3Tot = Page2Tot;


                for( int j = 0; j <= 19; j++){
                   if ( j < nFacTasks){
                      localsum = 0;
                      for( int i = 1; i <= 52; i++ ){
                        locallast =  SafeDbl(arData[i-1,j]);
                        localsum = localsum + locallast;
                      } //Next
                      stringBuffer =    stringBuffer +   "<td class=xl52 align='center'><b>" + FCurSP(cStr(localsum)) + "</b></td>";

                      localsum = 0;
                      double localcount = 0;

                      for( int i = 1; i <= 52;i++){
                        if (SafeDbl(arRowHeader[i,2]) > 0){
                            locallast =  SafeDbl(arData[i-1,j])/SafeDbl(arRowHeader[i,2]);
                            //locallast =  SafeDbl(arData(i-1,j));
                            localcount = localcount + 1;
                         }else{
                            locallast = 0;
                        }
                        localsum = localsum + locallast;
                      } //Next
              
                      //stringBuffer =    stringBuffer +   "<td class=xl52 align=//center//><b>" + fCurSP(localsum/localcount) + "</b></td>";
                      if(localsum != 0 && localcount != 0){
                        stringBuffer =    stringBuffer +   "<td class=xl52 align='center'><b>" + FCurSP(cStr(localsum/localcount)) + "</b></td>";
                      }else{
                        stringBuffer =    stringBuffer +   "<td class=xl52 align='center'><b>" + FCurSP(cStr(0)) + "</b></td>";
                      }
              
                   }else{
                      stringBuffer =    stringBuffer +   " <td class=xl52></td>";
                      stringBuffer =    stringBuffer +   " <td class=xl52></td>";
                   }
                           if( j == 5){
                                        Page2Tot = Page2Tot + stringBuffer;
                                        stringBuffer = "";
                           }else{
                               if (j == 11){
                                        Page3Tot = stringBuffer;
                                        stringBuffer = "";
                                }else{
                                   if (j == 19){
                                        Page4Tot = stringBuffer;
                                        stringBuffer = "";
                                   }
                               }
                           }
                }//Next

              Page2Tot =    Page2Tot +   " </tr>";
              Page3Tot =    Left3Tot + Page3Tot +   " </tr>";
              Page4Tot =    Left3Tot + Page4Tot +   " </tr>";

              Response.Write(Page2Tot);

        } //End Function

        public void WritePage3Totals(){
                Response.Write(Page3Tot);
        }

        public void WritePage4Totals(){
                Response.Write(Page4Tot);
        }

        public void WritePage2Data(){
           Response.Write(Page2Data);
        }

        public void WritePage3Data(){
           Response.Write(Page3Data);
        }

        public void WritePage4Data(){
           Response.Write(Page4Data);
        }

        public void WritePage2TitleRow(){

           string Page2TitleLine = "";
           string stringBuffer = "";
           string Left3TitleLine2 = "";

           Page2TitleLine =    Page2TitleLine + "<tr height=17 style='height:22.75pt'>";
           Page2TitleLine =    Page2TitleLine + "  <td height=17 class=xl30 style='height:22.75pt' bgcolor='#FFFF99'><b>Week Ending</b></td>";
           Page2TitleLine =    Page2TitleLine + "  <td class=xl30 bgcolor='#99CCFF'><b>Total Pay</b></td>";
           Page2TitleLine =    Page2TitleLine + "  <td class=xl30 style='border-left:none' bgcolor='#FFFF99'><b>Pay Difference From 1st Page</b></td>";
           Left3TitleLine =        "<tr height=17 style='height:22.75pt'>  <td height=17 class=xl30 style='height:22.75pt' bgcolor='#FFFF99'><b>Week Ending</b></td>";

           for( int i = 0; i <= 19; i++){

             if (i % 2 == 0){
                strColor = "#99CCFF";
             }else{
                        strColor = "#FFFF99";
             }

             if(i < nFacTasks){
                stringBuffer =    stringBuffer + "<td class=xl47 style='border-left:none' bgcolor='" + strColor + "'><b>Total Pay</b></td>";
                stringBuffer =    stringBuffer + "<td class=xl48 style='border-left:none' bgcolor='" + strColor + "'><b>Cost p/Unit</b></td>";
             }
                 if(i == 5){
                        Page2TitleLine = Page2TitleLine + stringBuffer;
                        stringBuffer = "";
                 }else{
                   if (i == 11){
                        Page3TitleLine = stringBuffer;
                        stringBuffer = "";
                   }else{
                      if (i == 19){
                        Page4TitleLine = stringBuffer;
                        stringBuffer = "";
                      }
                   } 
                }

            }//Next

           Page2TitleLine =    Page2TitleLine + "  </tr>";
           Page3TitleLine =    Page3TitleLine + "  </tr>";
           Page4TitleLine =    Page4TitleLine + "  </tr>";
           stringBuffer = "";

           Left3TitleLine2 =    Left3TitleLine2 +  "<tr height=17 style='height:12.75pt'>";
           Left3TitleLine2 =    Left3TitleLine2 +  "<td height=17 class=xl54 style='height:12.75pt;border-top:none' bgcolor='#FFFF99'></td>";
           Left3TitleLine2 =    Left3TitleLine2 +  "<td class=xl30 style='border-top:none' bgcolor='#99CCFF'></td>";
           Left3TitleLine2 =    Left3TitleLine2 +  "<td class=xl30 style='border-left:none;border-top:none' bgcolor='#FFFF99'></td>";

           for( int i = 0; i <= 19; i++){

                 if (i % 2 == 0){
                  strColor = "#99CCFF";
                 }else{
                  strColor = "#FFFF99";
                 }

                 if (i < nFacTasks){
                   stringBuffer =    stringBuffer + "<td colspan=2 class=xl79 style='border-right:.5pt solid black;border-left:none' bgcolor='" + strColor + "'><b>" + arFac[i] + "</b></td>";
                 }

                 if (i == 5){
                        Page2TitleLine = Page2TitleLine + Left3TitleLine2 + stringBuffer;
                        stringBuffer = "";
                 }else{
                    if (i == 11){
                        Page3TitleLine = Page3TitleLine + "<tr height=17 style='height:12.75pt'> <td height=17 class=xl30 style='height:12.75pt;border-top:none' bgcolor='#FFFF99'></td>"  + stringBuffer;
                        stringBuffer = "";
                    }else{
                       if (i == 19){
                        Page4TitleLine = Page4TitleLine + "<tr height=17 style='height:12.75pt'> <td height=17 class=xl30 style='height:12.75pt;border-top:none' bgcolor='#FFFF99'></td>" + stringBuffer;
                        stringBuffer = "";
                       }
                    }
                }

            } //Next

           Page2TitleLine =    Page2TitleLine + "  </tr>";
           Page3TitleLine =    "<tr height=17 style='height:22.75pt'> <td height=17 class=xl30 style='height:12.75pt;border-top:none' bgcolor='#FFFF99'><b>Week Ending</b></td>" + Page3TitleLine + "  </tr>";
           Page4TitleLine =    "<tr height=17 style='height:22.75pt'> <td height=17 class=xl30 style='height:12.75pt;border-top:none' bgcolor='#FFFF99'><b>Week Ending</b></td>" + Page4TitleLine + "  </tr>";

           Response.Write(Page2TitleLine);

        }

        public void WritePage3TitleRow(){
                Response.Write(Page3TitleLine);
        }

        public void WritePage4TitleRow(){
                Response.Write(Page4TitleLine);
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

                while( Weekday(cDate(BeginningYTDReturn)) != vbThursday){
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

    }
}