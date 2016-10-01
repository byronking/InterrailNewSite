<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FacilityRegionalCPU.aspx.cs" Inherits="InterrailPPRS.Reports.FacilityRegionalCPU" %>

<html><!-- #BeginTemplate "/Templates/Reports.dwt" -->

<!-- #BeginEditable "Head" -->
<title>PPRS</title>
<meta http-equiv=Content-Type content="text/html; charset=windows-1252">
<link rel="stylesheet" href="../Styles/styles.css" type="text/css" />
<link rel="stylesheet" href="FacilityRegionalCPU.css" type="text/css">
<script language="JavaScript">
<!--
    function MM_reloadPage(init) {  //reloads the window if Nav4 resized
        if (init == true) with (navigator) {
            if ((appName == "Netscape") && (parseIt(appVersion) == 4)) {
                document.MM_pgW = innerWidth; document.MM_pgH = innerHeight; onresize = MM_reloadPage;
            } 
        }
        else if (innerWidth != document.MM_pgW || innerHeight != document.MM_pgH) location.reload();
    }
    MM_reloadPage(true);
// -->
</script>
<!-- #EndEditable -->

<STYLE TYPE="text/css">
     DIV.breakhere {page-break-before: always}
     DIV.breakauto {page-break-before: auto}
</STYLE>
</head>
<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>
<body bgcolor="#FFFFFF" text="#000000" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">






<table x:str border=0 cellpadding=0 cellspacing=0 style='border-collapse:
 collapse;width=100%'>
 <tr height=27 style='height:35.25pt'>
  <td colspan=9 class=xl25 align=center >
          <table border=0 width=100%>
                <tr>
                        <td align=center class=xl2222222>
                                 <B>Facility Monitoring System<br>Year of <%=selYear%><br><%=regionName%>&nbsp;Region</b>
                                 <br>
                        </td>
                </tr>
          </table>
  </td>
 </tr>
</table>

<% if (facilityCount != 0 ){ %>

        <!-- Create title Row -->
        <% CreateTitleRow(); %>


        <!-- Create Load Data -->
        <% CreateLoadData(); %>

        <!-- Create Load Budget -->
        <% CreateLoadBudgeted(); %>

        <!-- Create Load Variance -->
        <% CreateLoadVariance(); %>

        <!-- Create UnLoad Data -->
        <% CreateUnLoadData(); %>

        <!-- Create UnLoad Budget -->
        <% CreateUnLoadBudgeted(); %>

        <!-- Create UnLoad Variance -->
        <% CreateUnLoadVariance(); %>
        
        
        <!-- Create Spotting Data -->
        <% CreateSpottingData(); %>

		
        <!-- Create Spotting Budget -->
        <% CreateSpottingBudgeted(); %>

        <!-- Create Spotting Variance -->
        <% CreateSpottingVariance(); %>
        


        

        <!-- Create Total Volumes -->
        <% CreateTotalVolumes(); %>



        <!-- Create PayRoll IRT -->
        <% CreatePayRollIRT(); %>

        <!-- Create PayRoll TEMP -->
        <% CreatePayRollTEMP(); %>

        <!-- Create Total PayRoll -->
        <% CreateTotalPayRoll(); %>

        <!-- Create PayRoll Hours IRT -->
        <% CreatePayRollHrsIRT(); %>

        <!-- Create PayRoll Hours IRT -->
        <% CreatePayRollHrsTEMP(); %>

        <!-- Create Total PayRoll Hours -->
        <% CreateTotalPayRollHrs(); %>

        <!-- Create OverTime -->
        <% CreateOverTime(); %>

        <!-- Create OTasPayroll -->
        <% CreateOTasPayroll(); %>



        <!-- Create Employees IRT -->
        <% CreateEmployeesIRT(); %>

        <!-- Create Employees TEMP -->
        <% CreateEmployeesTEMP(); %>

        <!-- Create Total Number of Employees -->
        <% CreateEmployeesTotal(); %>

        <!-- Create Units/Employees -->
        <% CreateEmployeesUnits(); %>

        <!-- Create Average Hrs per IRT Employees -->
        <% CreateEmployeesAvgIRT(); %>

        <!-- Create Average Hrs per TEMP Employees -->
        <% CreateEmployeesAvgTEMP(); %>

        <!-- Create Reported Miscellaneous Labor -->
        <% CreateMiscLaborRep(); %>

        <!-- Create Budgeted Miscellaneous Labor -->
        <% CreateMiscLaborBud(); %>

        <!-- Create Difference In Budgeted Misc. Labor -->
        <% CreateDiffMiscBudPayRoll(); %>



        <!-- Create CPU -->
        <% CreateCPU(); %>

        <!-- Create Budgeted CPU -->
        <% CreateBudgetedCPU(); %>

        <!-- Create CPU Variance -->
        <% CreateCPUVariance();%>

        <!-- Create Difference In Budgeted Labor - There is an array in here that is calculated from an array in CreateCPUVariance-->
        <% CreateDiffBudPayRoll(); %>

        <!-- Create Difference From Budget(Not Related to Misc Labor) -->
        <% CreateDiffFromBudget(); %>

        <!-- Create Overtime Cost -->
        <% CreateOvertimeCost(); %>

        <!-- Create Holidays -->
        <% CreateHoliday(); %>

        <!-- Create Average Hourly Pay -->
        <% CreateAvgHourlyPay();%>


        <!-- Create Budgeted Volumes -->
        <% CreateBudgetedVolumes();%>



<%

        for(int j=0 ; j <= weekIndex; j++){
                for(int i=0; i < (pageCount); i++){
%>
                <table x:str border=0 cellpadding=0 cellspacing=0 class=xl2830747
                 style='border-collapse:collapse;table-layout:fixed;width:421pt' align='center'>
                 <col class=xl2830747 width=110 style='mso-width-source:userset;mso-width-alt:
                 10093;width:110pt'>
                 <col class=xl2830747 width=0 style='display:none;mso-width-source:userset;
                 mso-width-alt:3291'>
                 <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                 2194;width:29pt'>
                 <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                 2230;width:29pt'>
                 <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                 365;width:29pt'>
                 <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                 365;width:29pt'>
                 <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                 365;width:29pt'>
                 <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                 365;width:29pt'>
                 <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                 365;width:29pt'>
                 <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                 365;width:29pt'>
                 <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                 365;width:29pt'>
                 <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                 365;width:29pt'>
                 <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                 365;width:29pt'>
                 <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                 365;width:29pt'>
                 <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                 365;width:29pt'>
                 <col class=xl2830747 width=31 style='mso-width-source:userset;mso-width-alt:
                 365;width:29pt'>
                 <col class=xl2830747 width=46 style='mso-width-source:userset;mso-width-alt:
                 2413;width:29pt'>
                 <col class=xl2830747 width=73 style='mso-width-source:userset;mso-width-alt:
                 2121;width:29pt'>
                 <col class=xl2830747 width=9 style='mso-width-source:userset;mso-width-alt:
                 329;width:1pt'>
                 <col class=xl2830747 width=73 style='mso-width-source:userset;mso-width-alt:
                 2669;width:27pt'>
                 <col class=xl2830747 width=88 style='mso-width-source:userset;mso-width-alt:
                 3218;width:36pt'>

<%
                                //Title Lines
                                Response.Write(Left3TitleLine + Page1TitleLine[i] + Right3TitleLine);
                                Response.Write(Left3TitleLine2[j] + Page2TitleLine[i] + Right3TitleLine2 );

                                //Load Data
                                Response.Write(LeftLoadData + PageLoadData[j,i] + RightLoadData[j]);

                                //Load Budgeted
                                Response.Write(LeftLoadBudget + PageLoadBudget[j,i] + RightLoadBudget[j]);

                                //Load Variance
                                Response.Write(LeftLoadVariance + PageLoadVariance[j,i] + RightLoadVariance[j]);
                                Response.Write("<tr><td colspan=12 style='height:3.75pt'>&nbsp;</td></tr>");



                                //UnLoad Data
                                Response.Write(LeftUnloadData + PageUnLoadData[j, i] + RightUnLoadData[j]);

                                //UnLoad Budgeted
                                Response.Write(LeftUnloadBudget + PageUnloadBudget[j, i] + RightUnloadBudget[j]);

                                //UnLoad Variance
                                Response.Write(LeftUnLoadVariance + PageUnLoadVariance[j,i] + RightUnLoadVariance[j]);

                   
                   
                   
                                //Spotting Data
                                Response.Write(LeftSpottingData + PageSpottingData[j,i] + RightSpottingData[j]);

                                //Spotting Budgeted
                                Response.Write(LeftSpottingBudget + PageSpottingBudget[j,i] + RightSpottingBudget[j]);

                                //Spotting Variance
                                Response.Write(LeftSpottingVariance + PageSpottingVariance[j,i] + RightSpottingVariance[j]);                   
                   
                   
                   
                   
                                //Total Volumes
                                Response.Write(LeftTotalVolume1 + PageTotalVolume1[j,i] + RightTotalVolume1[j]);
                                Response.Write(LeftTotalVolume2 + PageTotalVolume2[j,i] + RightTotalVolume2[j]);



                                //PayRoll IRT
                                Response.Write("<tr><td colspan=20 align='left' height='15' class=xl2730747><b>Payroll</b></td></tr>");
                                Response.Write(LeftPayRollIRT + PagePayRollIRT[j,i] + RightPayRollIRT[j]);

                                //PayRoll TEMP
                                Response.Write(LeftPayRollTEMP + PagePayRollTEMP[j,i] + RightPayRollTEMP[j]);

                                //PayRoll Total
                                Response.Write(LeftTotalPayRoll + PageTotalPayRoll[j,i] + RightTotalPayRoll[j]);
                                Response.Write("<tr><td colspan=12 style='height:3.75pt'>&nbsp;</td></tr>");



                                //PayRoll Hours IRT
                                Response.Write("<tr><td colspan=20 align='left' height='15' class=xl2730747><b>Payroll Hours</b></td></tr>");
                                Response.Write(LeftPayRollHrsIRT + PagePayRollHrsIRT[j,i] + RightPayRollHrsIRT[j]);

                                //PayRoll Hours TEMP
                                Response.Write(LeftPayRollHrsTEMP + PagePayRollHrsTEMP[j,i] + RightPayRollHrsTEMP[j]);

                                //PayRoll Total Hours
                                Response.Write(LeftTotalPayRollHrs + PageTotalPayRollHrs[j,i] + RightTotalPayRollHrs[j]);
                                Response.Write("<tr><td colspan=12 style='height:1.5pt'>&nbsp;</td></tr>");

                                //PayRoll OverTime
                                Response.Write(LeftOverTime + PageOverTime[j,i] + RightOverTime[j]);

                                //PayRoll OTasPayroll
                                Response.Write(LeftOTasPayroll + PageOTasPayroll[j,i] + RightOTasPayroll[j]);

                                //Employees IRT
                                Response.Write("<tr><td colspan=20 align='left' height='15' class=xl2730747><b>Employees</b></td></tr>");
                                Response.Write(LeftEmployeesIRT + PageEmployeesIRT[j,i] + RightEmployeesIRT[j]);

                                //Employees TEMP
                                Response.Write(LeftEmployeesTEMP + PageEmployeesTEMP[j,i] + RightEmployeesTEMP[j]);

                                //Employees Total
                                Response.Write(LeftEmployeesTotal + PageEmployeesTotal[j,i] + RightEmployeesTotal[j]);

                                //Employees Units
                                Response.Write(LeftEmployeesUnits + PageEmployeesUnits[j,i] + RightEmployeesUnits[j]);

                                //'Average Hrs per IRT Employees
                                Response.Write(LeftEmployeesAvgIRT + PageEmployeesAvgIRT[j,i] + RightEmployeesAvgIRT[j]);

                                //Average Hrs per TEMP Employees
                                Response.Write(LeftEmployeesAvgTEMP + PageEmployeesAvgTEMP[j,i] + RightEmployeesAvgTEMP[j]);
                                Response.Write("<tr><td colspan=20 style='height:4.5pt'>&nbsp;</td></tr>");

                                //Reported Misc Labor
                                Response.Write(LeftMiscLaborRep + PageMiscLaborRep[j,i] + RightMiscLaborRep[j]);

                                //Budgeted Misc Labor
                                Response.Write(LeftMiscLaborBud + PageMiscLaborBud[j,i] + RightMiscLaborBud[j]);

                                //Difference In Budgeted Labor
                                Response.Write(LeftDiffBudPayRoll + PageDiffBudPayRoll[j,i] + RightDiffBudPayRoll[j]);

                                //Difference In Budgeted Misc. Labor
                                Response.Write(LeftDiffMiscBudPayRoll + PageDiffMiscBudPayRoll[j,i] + RightDiffMiscBudPayRoll[j]);
                                Response.Write("<tr><td colspan=20 style='height:4.5pt'>&nbsp;</td></tr>");



                                //CPU
                                Response.Write(LeftCPU + PageCPU[j,i] + RightCPU[j]);

                                //Budgeted CPU
                                Response.Write(LeftBudgetedCPU + PageBudgetedCPU[j,i] + RightBudgetedCPU[j]);

                                //Variance CPU
                                Response.Write(LeftCPUVariance + PageCPUVariance[j,i] + RightCPUVariance[j]);
                                Response.Write("<tr><td colspan=20 style='height:1.5pt'>&nbsp;</td></tr>");

                                //Difference From Budget (not related to Misc Labor)
                                Response.Write(LeftDiffFromBudget + PageDiffFromBudget[j,i] + RightDiffFromBudget[j]);

                                //Overtime Cost
                                Response.Write(LeftOvertimeCost + PageOvertimeCost[j,i] + RightOvertimeCost[j]);

                                //Holidays
                                if (Sum(HolidayTotal,j) != 0 ){
                                        Response.Write(LeftHoliday + PageHoliday[j,i] + RightHoliday[j]);
                                }

                                //Average Hourly Pay
                                Response.Write(LeftAvgHourlyPay + PageAvgHourlyPay[j,i] + RightAvgHourlyPay[j]);


                                Response.Write("<tr><td colspan=20 style='height:15.75pt'>&nbsp;</td></tr>");
                                Response.Write("<tr><td colspan=20 style='height:3.75pt'>&nbsp;</td></tr>");

                                Response.Write("</table>");
                                Response.Write("<DIV CLASS='breakhere'></div>");

                                } //Next
                } //Next

        //Start of Code for bottom Volumes Section of Report
        for(int i=0; i <(pageCount); i++){
        
%>

                        <table x:str border=0 cellpadding=0 cellspacing=0 class=xl2830747
                         style='border-collapse:collapse;table-layout:fixed;width:421pt' align='left'>
                         <col class=xl2830747 width=110 style='mso-width-source:userset;mso-width-alt:
                         10093;width:110pt'>
                         <col class=xl2830747 width=0 style='display:none;mso-width-source:userset;
                         mso-width-alt:3291'>
                         <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                         2194;width:29pt'>
                         <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                         2230;width:29pt'>
                         <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                         365;width:29pt'>
                         <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                         365;width:29pt'>
                         <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                         365;width:29pt'>
                         <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                         365;width:29pt'>
                         <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                         365;width:29pt'>
                         <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                         365;width:29pt'>
                         <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                         365;width:29pt'>
                         <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                         365;width:29pt'>
                         <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                         365;width:29pt'>
                         <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                         365;width:29pt'>
                         <col class=xl2830747 width=50 style='mso-width-source:userset;mso-width-alt:
                         365;width:29pt'>
                         <col class=xl2830747 width=31 style='mso-width-source:userset;mso-width-alt:
                         365;width:29pt'>
                         <col class=xl2830747 width=46 style='mso-width-source:userset;mso-width-alt:
                         2413;width:29pt'>
                         <col class=xl2830747 width=73 style='mso-width-source:userset;mso-width-alt:
                         2121;width:29pt'>
                         <col class=xl2830747 width=9 style='mso-width-source:userset;mso-width-alt:
                         329;width:1pt'>
                         <col class=xl2830747 width=73 style='mso-width-source:userset;mso-width-alt:
                         2669;width:27pt'>
                         <col class=xl2830747 width=88 style='mso-width-source:userset;mso-width-alt:
                         3218;width:36pt'>

        <%
                        //Budgeted Volumes
                        
                        Response.Write("<tr><td colspan=30 align='left' height='15' class=xl2730747><b>Budgeted Volumes</b></td></tr>");
                        Response.Write(LeftBVTitle + PageBVTitle[0,i] + RightBVTitle[i]);
                        Response.Write(LeftBVLoad + PageBVLoad[0,i] + RightBVLoad[i]);
                        Response.Write(LeftBVUnLoad + PageBVUnLoad[0,i] + RightBVUnLoad[i]);
                        Response.Write(LeftBVTotal + PageBVTotal[0, i] + RightBVTotal[i]);

                        Response.Write("<tr><td colspan=30 align='left' height='15' class=xl2730747>Weeks in period</td></tr>");
                        Response.Write("<tr><td colspan=30 align='left' height='15' class=xl2730747><b>Actual Variance</b></td></tr>");

                        Response.Write(LeftBVUnLoadVar + PageBVUnLoadVar[0, i] + RightBVUnLoadVar[i]);
                        Response.Write(LeftBVLoadVar + PageBVLoadVar[0, i] + RightBVLoadVar[i]);
                        Response.Write(LeftBVTotalVar + PageBVTotalVar[0, i] + RightBVTotalVar[i]);

                        Response.Write("<tr><td colspan=30 align='left' height='15' class=xl2730747>&nbsp;</td></tr>");
                        Response.Write(LeftBVTotalPayroll + PageBVTotalPayroll[0, i] + RightBVTotalPayroll[i]);
                        Response.Write(LeftBVBudgetedPayroll + PageBVBudgetedPayroll[0, i] + RightBVBudgetedPayroll[i]);

                        Response.Write("<tr><td colspan=30 align='left' height='15' class=xl2730747>&nbsp;</td></tr>");
                        Response.Write(LeftBVVolumesVariance + PageBVVolumesVariance[0, i] + RightBVVolumesVariance[i]);
                        Response.Write(LeftBVPayrollVariance + PageBVPayrollVariance[0, i] + RightBVPayrollVariance[i]);

                        Response.Write("<tr><td colspan=30 align='left' height='15' class=xl2730747><b>Total Volumes</b></td></tr>");
                        Response.Write(LeftBVUnloadVolumes + PageBVUnloadVolumes[0, i] + RightBVUnloadVolumes[i]);
                        Response.Write(LeftBVLoadVolumes + PageBVLoadVolumes[0, i] + RightBVLoadVolumes[i]);
                        Response.Write(LeftBVTotalVolumes + PageBVTotalVolumes[0, i] + RightBVTotalVolumes[i]);
                        Response.Write("</table>");

                        if ((i % 2) == 0 ){
                                Response.Write("<DIV CLASS='breakhere'></div>");
                        }

                 }


%>

</table>

<% }else{ %>

                <br><br><br>
                <p align='center'>There are no Facilities in this Region.</p>

<% } %>

</body>
<!-- #EndTemplate -->

</html>