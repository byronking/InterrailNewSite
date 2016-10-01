<%@ Page Language="C#" MasterPageFile="~/Reports/Reports.Master" AutoEventWireup="true" CodeBehind="ProductionReports.aspx.cs" Inherits="InterrailPPRS.Reports.ProductionReports" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script language="JavaScript">
<!--
    function MM_reloadPage(init) {  //reloads the window if Nav4 resized
        if (init == true) with (navigator) {
            if ((appName == "Netscape") && (parseInt(appVersion) == 4)) {
                document.MM_pgW = innerWidth; document.MM_pgH = innerHeight; onresize = MM_reloadPage;
            } 
        }
        else if (innerWidth != document.MM_pgW || innerHeight != document.MM_pgH) location.reload();
    }
    MM_reloadPage(true);
    // -->

    function bodyload() {

        OriginalFormCheckSumValue = CheckStringForForm(document.form1);
    }

    function bodyunload() {
        if (CheckStringForForm(document.form1) != OriginalFormCheckSumValue) {
            event.returnValue = "You have not saved your changes.";
        }
    }


    function get_shifts(checkboxname) {

        var results = "";
        // Loop from zero to the one minus the number of checkbox button selections
        for (counter = 0; counter < checkboxname.length; counter++) {
            // if (a checkbox has been selected it will return true
            // (if (not it will return false)
            if (checkboxname[counter].checked) {
                if (!(results == "")) {
                    results = results + ", ";
                }
                results = results + checkboxname[counter].value;
            }
        }
        return (results);
    }

    function checkbox_checker(checkboxname) {
        // set var checkbox_choices to zero
        var checkbox_choices = 0;

        // Loop from zero to the one minus the number of checkbox button selections
        for (counter = 0; counter < checkboxname.length; counter++) {
            // if (a checkbox has been selected it will return true
            // (if (not it will return false)
            if (checkboxname[counter].checked)
            { return (true); }
        }
        return (false);
    }


    function goValidate(iRpt) {
        if (iRpt == 0) {
            var from = document.frmPrint.fromDate;
            var to = document.frmPrint.toDate;
            var dateFrom = new Date(Date.parse(document.frmPrint.fromDate.value));
            var dateTo = new Date(Date.parse(document.frmPrint.toDate.value));
            var shiftschecked = checkbox_checker(document.frmPrint.SelectedShiftsDaily);
        }
        if (iRpt == 1) {
            var from = document.frmPrint.fromDateCust;
            var to = document.frmPrint.toDateCust;
            var dateFrom = new Date(Date.parse(document.frmPrint.fromDateCust.value));
            var dateTo = new Date(Date.parse(document.frmPrint.toDateCust.value));
            var shiftschecked = checkbox_checker(document.frmPrint.SelectedShiftsDaily);
        }

        if (iRpt == 2) {
            var from = document.frmPrint.fromDateMan;
            var to = document.frmPrint.toDateMan;
            var dateFrom = new Date(Date.parse(document.frmPrint.fromDateMan.value));
            var dateTo = new Date(Date.parse(document.frmPrint.toDateMan.value));
            var shiftschecked = checkbox_checker(document.frmPrint.SelectedShiftsDaily);
        }

        if (iRpt == 9) {
            var from = document.frmPrint.fromDateRange;
            var to = document.frmPrint.toDateRange;
            var dateFrom = new Date(Date.parse(document.frmPrint.fromDateRange.value));
            var dateTo = new Date(Date.parse(document.frmPrint.toDateRange.value));
            var shiftschecked = checkbox_checker(document.frmPrint.SelectedShiftsProductionReporting);
        }
        if (iRpt == 10) {
            var from = document.frmPrint.fromCDateRange;
            var to = document.frmPrint.toCDateRange;
            var dateFrom = new Date(Date.parse(document.frmPrint.fromDateRange.value));
            var dateTo = new Date(Date.parse(document.frmPrint.toDateRange.value));
            var selCusIDandNAME = document.frmPrint.selCCustomerID.value;
            var shiftschecked = checkbox_checker(document.frmPrint.SelectedShiftsProductionReporting);

            if (selCusIDandNAME == "") {
                alert(" Select a Customer ");
                document.frmPrint.selCCustomerID.focus();
                return false;
            }
        }

        if (!shiftschecked) {
            alert(" Must Select at least one shift ");
            return false;
        }

        if (ValidDate(from, 1, "Date From") != true) { return false; }
        if (ValidDate(to, 1, "Date To") != true) { return false; }

        if (dateFrom > dateTo) {
            alert(" 'Date From' must be prior to 'Date To'.");
            from.focus();
            from.select();
            return false;
        }

        if (iRpt == 0) {
            document.frmPrint.action = "ProductionReportByTask.aspx?PrintPreview=1&ReportType=Daily&From=" + escape(from.value) + "&To=" + escape(to.value) + "&SelectedShifts=" + get_shifts(document.frmPrint.SelectedShiftsDaily);
        }
        if (iRpt == 1) {
            document.frmPrint.action = "ProductionReportByCustomer.aspx?PrintPreview=1&ReportType=Daily&From=" + escape(from.value) + "&To=" + escape(to.value) + "&SelectedShifts=" + get_shifts(document.frmPrint.SelectedShiftsDaily);
        }

        if (iRpt == 2) {
            document.frmPrint.action = "ProductionReportByManufacturer.aspx?PrintPreview=1&ReportType=Daily&From=" + escape(from.value) + "&To=" + escape(to.value) + "&SelectedShifts=" + get_shifts(document.frmPrint.SelectedShiftsDaily);
        }

        if (iRpt == 9) {
            document.frmPrint.action = "ProductionReportSummary.aspx?PrintPreview=1&From=" + escape(from.value) + "&To=" + escape(to.value) + "&SelectedShifts=" + get_shifts(document.frmPrint.SelectedShiftsProductionReporting);
        }
        if (iRpt == 10) {
            document.frmPrint.action = "ProductionReportSummarybyCustomer.aspx?PrintPreview=1&From=" + escape(from.value) + "&To=" + escape(to.value) + "&CustomerIDandName=" + selCusIDandNAME + "&SelectedShifts=" + get_shifts(document.frmPrint.SelectedShiftsProductionReporting);
        }

        document.frmPrint.submit();
    }

    function goMonthly(iRpt) {
        var shiftschecked = checkbox_checker(document.frmPrint.SelectedShiftsMonth);

        if (!shiftschecked) {
            alert(" Must Select at least one shift ");
            return false;
        }

        if (iRpt == 0) {
            document.frmPrint.action = "ProductionReportByTask.aspx?PrintPreview=1&ReportType=Monthly&Month=" + (document.frmPrint.selMonth.value) + "&Year=" + (document.frmPrint.selYear.value) + "&SelectedShifts=" + get_shifts(document.frmPrint.SelectedShiftsMonth);
            document.frmPrint.submit();
        }
        else if (iRpt == 1) {
            document.frmPrint.action = "ProductionReportByCustomer.aspx?PrintPreview=1&ReportType=Monthly&Month=" + (document.frmPrint.selMonthCust.value) + "&Year=" + (document.frmPrint.selYearCust.value) + "&SelectedShifts=" + get_shifts(document.frmPrint.SelectedShiftsMonth);
            document.frmPrint.submit();
        }
        else if (iRpt == 2) {
            document.frmPrint.action = "ProductionReportByManufacturer.aspx?PrintPreview=1&ReportType=Monthly&Month=" + (document.frmPrint.selMonthMan.value) + "&Year=" + (document.frmPrint.selYearMan.value) + "&SelectedShifts=" + get_shifts(document.frmPrint.SelectedShiftsMonth);
            document.frmPrint.submit();
        }
    }

    function goDetail() {
        //var arDates  = document.frmPrint.selDateRange.value.split(",");
        //document.frmPrint.fromDateDetail.value = arDates[0];
        //document.frmPrint.toDateDetail.value = arDates[1];

        var from = document.frmPrint.fromDateDetail;
        var to = document.frmPrint.toDateDetail;

        var shiftschecked = checkbox_checker(document.frmPrint.SelectedShiftsDetail);

        if (!shiftschecked) {
            alert(" Must Select at least one shift ");
            return false;
        }
        if (ValidDate(from, 1, "Date From") != true) { return false; }
        if (ValidDate(to, 1, "Date To") != true) { return false; }

        var dateFrom = new Date(Date.parse(document.frmPrint.fromDateDetail.value));
        var dateTo = new Date(Date.parse(document.frmPrint.toDateDetail.value));

        if (dateFrom > dateTo) {
            alert(" 'Date From' must be prior to 'Date To'.");
            from.focus();
            from.select();
            return false;
        }

        document.frmPrint.action = "ProductionDetailReport.aspx?PrintPreview=1" + "&SelectedShifts=" + get_shifts(document.frmPrint.SelectedShiftsDetail);
        //document.frmPrint.action = "/col.aspx";
        document.frmPrint.submit();

    }

</script>
</asp:Content>

<asp:Content ID="leftNavigation" ContentPlaceHolderID="Navigation" runat="server">
      <table width="81%" border="0"  valign="top">
                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%">&nbsp;</td>
                    <td width="79%">&nbsp;</td>
                  </tr>
                  <%= ChangeFacilityLink() %>
                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%">&nbsp;</td>
                    <td width="79%">&nbsp;</td>
                  </tr>
                  <%if ( CheckSecurity("Super, Admin, User") ) { %>
                    <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
                    <td width="79%"><a href="AdminReports.aspx">Admin Reports</a></td>
                  </tr>
                 <%} %>

                 <%if ( CheckSecurity("Super, Admin, User, Production") ) { %>

                                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
                    <td width="79%"><a href="ProductionReports.aspx">Production Reports</a></td>
                  </tr>
                 <%} %>

                 <%if ( CheckSecurity("Super, Admin, User") )
                   { %>
                                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
                    <td width="79%"><a href="PayRollReports.aspx">Payroll Reports</a></td>
                  </tr>
                                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
                    <td width="79%"><a href="RebillReports.aspx">Rebilling Reports</a></td>
                  </tr>
                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%">&nbsp;</td>
                    <td width="79%">&nbsp;</td>
                  </tr>
                                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
                    <td width="79%"><a href="FacilityMonitor.aspx">Facility Monitor</a></td>
                  </tr>

                                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%">&nbsp;</td>
                    <td width="79%">&nbsp;</td>
                  </tr>

                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
                    <td width="79%"><a href="OtherReports.aspx">Other Reports</a></td>

                  </tr>

                  <%}%>

                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%">&nbsp;</td>
                    <td width="79%">&nbsp;</td>
                  </tr>
                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
                    <td width="79%"><a href="../Logout.aspx">Logout</a></td>
                  </tr>

                </table>
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="Main" runat="server">
                      <table width="90%" border="0" cellspacing="0" cellpadding="8">
                        <tr>
                          <td valign="top"><!-- #BeginEditable "MainBody" --> 
                            <form name='frmPrint' method='post' action=''>
                              <table width="91%" border="0" cellspacing="0" cellpadding="0">
                                <tr> 
                                  <td colspan="4" class="pageTitle" align='center'> 
                                    <div class="cellTopBottomBorder">Summary Production 
                                      Reports</div>
                                  </td>
                                </tr>
                                <tr> 
                                  <td width="52%" align='right' height="16">&nbsp;</td>
                                  <td width="17%" align='center' height="16">&nbsp;</td>
                                  <td width="17%" align='center' height="16">&nbsp;</td>
                                  <td width="14%" height="16">&nbsp;</td>
                                </tr>
                                <tr class='ReportOddLine'> 
                                  <td width="52%" align='right'><font color='green'><b>Daily 
                                    Production Reports: </b></font></td>
                                  <td width="17%" align='center' class='cellTopBottomBorder'>From</td>
                                  <td width="17%" align='center' class='cellTopBottomBorder'>To</td>
                                  <td width="14%">&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td colspan=3 align='right'>Shifts:<%=GetShiftCheckBoxes("SelectedShiftsDaily")%></td>
                                  <td width="14%">&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td width="52%" align='right'><font color='green'> 
                                    By Task:&nbsp;</font></td>
                                  <td width="17%" align='left'> 
                                    <input type="text" name="fromDate" size="10" maxlength="10" value='<%=Session["LastStartDate"]%>'>
                                  </td>
                                  <td width="17%" align='left'> 
                                    <input type="text" name="toDate" size="10" maxlength="10" value='<%=Session["LastEndDate"]%>'>
                                  </td>
                                  <td width="14%" align='center'> 
                                    <input type="button" name="btnDaily" value="Go" onClick="goValidate(0);">
                                  </td>
                                </tr>
                                <tr> 
                                  <td width="52%" align='right'><font color='green'> 
                                    By Customer:&nbsp;</font></td>
                                  <td width="17%" align='left'> 
                                    <input type="text" name="fromDateCust" size="10" maxlength="10" value='<%=Session["LastStartDate"]%>'>
                                  </td>
                                  <td width="17%" align='left'> 
                                    <input type="text" name="toDateCust" size="10" maxlength="10" value='<%=Session["LastEndDate"]%>'>
                                  </td>
                                  <td width="14%" align='center'> 
                                    <input type="button" name="btnDailyCust" value="Go" onClick="goValidate(1);">
                                  </td>
                                </tr>
                                <tr> 
                                  <td width="52%" align='right'><font color='green'> 
                                    By Manufacturer:&nbsp;</font></td>
                                  <td width="17%" align='left'> 
                                    <input type="text" name="fromDateMan" size="10" maxlength="10" value='<%=Session["LastStartDate"]%>'>
                                  </td>
                                  <td width="17%" align='left'> 
                                    <input type="text" name="toDateMan" size="10" maxlength="10" value='<%=Session["LastEndDate"]%>'>
                                  </td>
                                  <td width="14%" align='center'> 
                                    <input type="button" name="btnDailyMan" value="Go" onClick="goValidate(2);">
                                  </td>
                                </tr>
                                <tr> 
                                  <td width="52%">&nbsp;</td>
                                  <td width="17%" align='center'>&nbsp;</td>
                                  <td width="17%" align='center'>&nbsp;</td>
                                  <td width="14%">&nbsp;</td>
                                </tr>
                                <tr class='ReportOddLine'> 
                                  <td width="52%" align='right'><font color='green'><b>Monthly 
                                    Production Reports: </b></font></td>
                                  <td width="17%" align='center' class='cellTopBottomBorder'>Month</td>
                                  <td width="17%" align='center' class='cellTopBottomBorder'>Year</td>
                                  <td width="14%">&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td colspan=3 align='right'>Shifts:<%=GetShiftCheckBoxes("SelectedShiftsMonth")%></td>
                                  <td width="14%">&nbsp;</td>
                                </tr>
                                <tr> 
                                
                                  <td width="52%" align='right'><font color='green'> 
                                    By Task:&nbsp;</font></td>
                                  <td width="17%" align='left'> 
                                    <select name="selMonth">
                                      <option value="1"   <%if (System.DateTime.Now.Month == 1)  { Response.Write("selected");} %> >January</option>
                                      <option value="2"   <%if (System.DateTime.Now.Month == 2)  { Response.Write("selected");} %> >February</option>
                                      <option value="3"   <%if (System.DateTime.Now.Month == 3)  { Response.Write("selected");} %> >March</option>
                                      <option value="4"   <%if (System.DateTime.Now.Month == 4)  { Response.Write("selected");} %> >April</option>
                                      <option value="5"   <%if (System.DateTime.Now.Month == 5)  { Response.Write("selected");} %> >May</option>
                                      <option value="6"   <%if (System.DateTime.Now.Month == 6)  { Response.Write("selected");} %> >June</option>
                                      <option value="7"   <%if (System.DateTime.Now.Month == 7)  { Response.Write("selected");} %> >July</option>
                                      <option value="8"   <%if (System.DateTime.Now.Month == 8)  { Response.Write("selected");} %> >August</option>
                                      <option value="9"   <%if (System.DateTime.Now.Month == 9)  { Response.Write("selected");} %> >September</option>
                                      <option value="10"  <%if (System.DateTime.Now.Month == 10)  { Response.Write("selected");} %> >October</option>
                                      <option value="11"  <%if (System.DateTime.Now.Month == 11)  { Response.Write("selected");} %> >November</option>
                                      <option value="12"  <%if (System.DateTime.Now.Month == 12)  { Response.Write("selected");} %> >December</option>
                                    </select>
                                  </td>
                                  <td width="17%" align='left'> 
                                    <select name="selYear">
                                      <option value="<%=cStr(System.DateTime.Now.Year- 4)%>"><%=cStr(System.DateTime.Now.Year- 4)%></option>
                                      <option value="<%=cStr(System.DateTime.Now.Year- 3)%>"><%=cStr(System.DateTime.Now.Year- 3)%></option>
                                      <option value="<%=cStr(System.DateTime.Now.Year- 2)%>"><%=cStr(System.DateTime.Now.Year- 2)%></option>
                                      <option value="<%=cStr(System.DateTime.Now.Year- 1)%>"><%=cStr(System.DateTime.Now.Year- 1)%></option>
                                      <option value="<%=cStr(System.DateTime.Now.Year) %>" Selected><%=cStr(System.DateTime.Now.Year)%></option>
                                    </select>
                                  </td>
                                  <td width="14%" align='center'> 
                                    <input type="button" name="btnMonthly" value="Go" onClick="goMonthly(0);">
                                  </td>
                                </tr>
                                <tr> 
                                  <td width="52%" align='right'><font color='green'>By Customer:&nbsp;</font></td>
                                  <td width="17%" align='left'> 
                                    <select name="selMonthCust">
                                      <option value="1"   <%if (System.DateTime.Now.Month == 1){ Response.Write("selected");} %> >January</option>
                                      <option value="2"   <%if (System.DateTime.Now.Month == 2){ Response.Write("selected");}  %> >February</option>
                                      <option value="3"   <%if (System.DateTime.Now.Month == 3){ Response.Write("selected");}  %> >March</option>
                                      <option value="4"   <%if (System.DateTime.Now.Month == 4){ Response.Write("selected");}  %> >April</option>
                                      <option value="5"   <%if (System.DateTime.Now.Month == 5){ Response.Write("selected");}  %> >May</option>
                                      <option value="6"   <%if (System.DateTime.Now.Month == 6){ Response.Write("selected");}  %> >June</option>
                                      <option value="7"   <%if (System.DateTime.Now.Month == 7){ Response.Write("selected");}  %> >July</option>
                                      <option value="8"   <%if (System.DateTime.Now.Month == 8){ Response.Write("selected");}  %> >August</option>
                                      <option value="9"   <%if (System.DateTime.Now.Month == 9){ Response.Write("selected");}  %> >September</option>
                                      <option value="10"  <%if (System.DateTime.Now.Month == 10){ Response.Write("selected");}  %> >October</option>
                                      <option value="11"  <%if (System.DateTime.Now.Month == 11){ Response.Write("selected");}  %> >November</option>
                                      <option value="12"  <%if (System.DateTime.Now.Month == 12){ Response.Write("selected");}  %> >December</option>
                                    </select>
                                  </td>
                                  <td width="17%" align='left'> 
                                    <select name="selYearCust">
                                      <option value="<%=cStr(System.DateTime.Now.Year - 4)%>"><%=cStr(System.DateTime.Now.Year - 4)%></option>
                                      <option value="<%=cStr(System.DateTime.Now.Year - 3)%>"><%=cStr(System.DateTime.Now.Year- 3)%></option>
                                      <option value="<%=cStr(System.DateTime.Now.Year - 2)%>"><%=cStr(System.DateTime.Now.Year- 2)%></option>
                                      <option value="<%=cStr(System.DateTime.Now.Year - 1)%>"><%=cStr(System.DateTime.Now.Year- 1)%></option>
                                      <option value="<%=cStr(System.DateTime.Now.Year)%>" Selected><%=FormatDate(cStr(System.DateTime.Now))%></option>
                                    </select>
                                  </td>
                                  <td width="14%" align='center'> 
                                    <input type="button" name="btnMonthly2" value="Go" onClick="goMonthly(1);" />
                                  </td>
                                </tr>
                                <tr> 
                                  <td width="52%" align='right'><font color='green'>By 
                                    Manufacturer:&nbsp;</font></td>
                                  <td width="17%" align='left'> 
                                    <select name="selMonthMan">
                                      <option value="1"   <%if (System.DateTime.Now.Month == 1) { Response.Write("selected"); }%> >January</option>
                                      <option value="2"   <%if (System.DateTime.Now.Month == 2) { Response.Write("selected"); }%> >February</option>
                                      <option value="3"   <%if (System.DateTime.Now.Month == 3) { Response.Write("selected"); }%> >March</option>
                                      <option value="4"   <%if (System.DateTime.Now.Month == 4) { Response.Write("selected"); }%> >April</option>
                                      <option value="5"   <%if (System.DateTime.Now.Month == 5) { Response.Write("selected"); }%> >May</option>
                                      <option value="6"   <%if (System.DateTime.Now.Month == 6) { Response.Write("selected"); }%> >June</option>
                                      <option value="7"   <%if (System.DateTime.Now.Month == 7) { Response.Write("selected"); }%> >July</option>
                                      <option value="8"   <%if (System.DateTime.Now.Month == 8) { Response.Write("selected"); }%> >August</option>
                                      <option value="9"   <%if (System.DateTime.Now.Month == 9) { Response.Write("selected"); }%> >September</option>
                                      <option value="10"  <%if (System.DateTime.Now.Month == 10) { Response.Write("selected"); } %> >October</option>
                                      <option value="11"  <%if (System.DateTime.Now.Month == 11) { Response.Write("selected"); } %> >November</option>
                                      <option value="12"  <%if (System.DateTime.Now.Month == 12) { Response.Write("selected"); } %> >December</option>
                                    </select>
                                  </td>
                                  <td width="17%" align='left'> 
                                    <select name="selYearMan">
                                      <option value="<%=cStr(System.DateTime.Now.Year- 4)%>"><%=cStr(System.DateTime.Now.Year- 4)%></option>
                                      <option value="<%=cStr(System.DateTime.Now.Year- 3)%>"><%=cStr(System.DateTime.Now.Year- 3)%></option>
                                      <option value="<%=cStr(System.DateTime.Now.Year- 2)%>"><%=cStr(System.DateTime.Now.Year- 2)%></option>
                                      <option value="<%=cStr(System.DateTime.Now.Year- 1)%>"><%=cStr(System.DateTime.Now.Year- 1)%></option>
                                      <option value="<%=cStr(System.DateTime.Now.Year)%>" Selected><%=cStr(System.DateTime.Now.Year)%></option>
                                    </select>
                                  </td>
                                  <td width="14%" align='center'> 
                                    <input type="button" name="btnMonthly3" value="Go" onClick="goMonthly(2);">
                                  </td>
                                </tr>
                                <tr> 
                                  <td width="52%" align='right'>&nbsp;</td>
                                  <td width="17%" align='left'>&nbsp;</td>
                                  <td width="17%" align='left'>&nbsp;</td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <td colspan="4" class="pageTitle" align='center'> 
                                  <div class="cellTopBottomBorder">Production 
                                    Reporting </div>
                                </td>
                                <tr> 
                                  <td width="52%" align='right'>&nbsp;</td>
                                  <td width="17%" align='left'>&nbsp;</td>
                                  <td width="17%" align='left'>&nbsp;</td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr class='ReportOddLine'> 
                                  <td width="52%" align='right'>&nbsp;</td>
                                  <td width="17%" align='center' class='cellTopBottomBorder'>From</td>
                                  <td width="17%" align='center' class='cellTopBottomBorder'>To</td>
                                  <td width="14%">&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td width="52%" align='right'><font color='green'> 
                                    <b>Summary For Date Range: </b>&nbsp;</font></td>
                                  <td width="17%" align='left'> 
                                    <input type="text" name="fromDateRange" size="10" maxlength="10" value='<%=Session["LastStartDate"]%>'>
                                  </td>
                                  <td width="17%" align='left'> 
                                    <input type="text" name="toDateRange" size="10" maxlength="10" value='<%=Session["LastEndDate"]%>'>
                                  </td>
                                  <td width="14%" align='center'> 
                                    <input type="button" name="btnRange" value="Go" onClick="goValidate(9);">
                                  </td>
                                </tr>
                                <tr> 
                                  <td width="52%" align='right'><font color='green'> 
                                    <b>Client Summary For Date Range: </b>&nbsp;</font>
                                    <select name="selCCustomerID" size="1" style="width:220px">
                                      <%
                                        while (! rsFacCustomers.EOF){
                                            rsFacCustomers.Read();
                                        %>
                                     <option value="<%=(rsFacCustomers.Item("Id")) + "|" + (rsFacCustomers.Item("CustomerName"))%>" ><%=(rsFacCustomers.Item("CustomerName"))%> (<%=(rsFacCustomers.Item("CustomerCode"))%>)</option>
                                      <%

                                        }
                                      rsFacCustomers.Requery();

                                    %>
                                    </select>
                                    </td>
                                  <td width="17%" align='left'> 
                                    <input type="text" name="fromCDateRange" size="10" maxlength="10" value='<%=Session["LastStartDate"]%>'>
                                  </td>
                                  <td width="17%" align='left'> 
                                    <input type="text" name="toCDateRange" size="10" maxlength="10" value='<%=Session["LastEndDate"]%>'>
                                  </td>
                                  <td width="14%" align='center'> 
                                    <input type="button" name="btnCRange" value="Go" onClick="goValidate(10);">
                                  </td>
                                </tr>
                                <tr> 
                                  <td colspan="3" align='right'  valign='top'>&nbsp; 
                                    Shifts: <%=GetShiftCheckBoxes("SelectedShiftsProductionReporting")%>
                                  </td>
                                  <td width="14%" align='center'>&nbsp; </td>
                                </tr>
                                <tr> 
                                  <td width="52%" align='right'>&nbsp;</td>
                                  <td width="17%" align='left'>&nbsp;</td>
                                  <td width="17%" align='left'>&nbsp;</td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td colspan="4" class="pageTitle" align='center'> 
                                    <div class="cellTopBottomBorder">Detail Production 
                                      Report</div>
                                  </td>
                                </tr>
                                <tr> 
                                  <td width="52%" align='right'  >&nbsp;</td>
                                  <td width="17%" align='center' >&nbsp;</td>
                                  <td width="17%" align='center' >&nbsp;</td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td width="52%" align='center'  ><font color='green'>Facilities**</font></td>
                                  <td colspan="2" align='center' ><font color='green'>Date 
                                    Range</font></td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td rowspan="2" align='right'><font color='green'> 
                                    <select name="selFacilities" size="4" multiple style="width:160px">
                                      <%
                                        while (! rsFac.EOF){
                                            rsFac.Read();
                                        %>
                                        <option value="<%=(rsFac.Item("Id"))%>" <%if (cStr(rsFac.Item("Id")) == cStr(Session["FacilityID"])) { Response.Write("SELECTED");} else { Response.Write("");} %>><%=(rsFac.Item("Name"))%> (<%=(rsFac.Item("AlphaCode"))%>)</option>
                                      <%
                                          }

                                      rsFac.Requery();

                                    %>
                                    </select>
                                    </font></td>
                                  <td colspan="2" align='center' class='cellTopBottomBorder'> 
                                    <!--
                                    <select name="selDateRange">
                                      <%=getPayPeriods(0,52,"")%>
                                    </select>
                                                                        -->
                                    <input type="text" name="fromDateDetail" size="10" maxlength="10" value='<%=Session["LastStartDate"]%>' />
                                    <input type="text" name="toDateDetail" size="10" maxlength="10" value='<%=Session["LastEndDate"]%>' />
                                  </td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td colspan="2" align='center'  valign='top'>&nbsp; 
                                  </td>
                                  <td width="14%" align='center'>&nbsp; </td>
                                </tr>
                                <tr> 
                                  <td colspan="3" align='right'  valign='top'>&nbsp; 
                                    Shifts: <%=GetShiftCheckBoxes("SelectedShiftsDetail")%>
                                  </td>
                                  <td width="14%" align='center'>&nbsp; </td>
                                </tr>
                                <tr> 
                                  <td width="52%" align='center' valign="top">&nbsp;</td>
                                  <td colspan="2" align='center'>&nbsp;</td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td width="52%" align='center' valign="top"><font color='green'>Tasks*&nbsp;</font></td>
                                  <td colspan="2" align='center'><font color="green">Customers*</font></td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td width="52%" align='right' valign="top"><font color='green'> 
                                    <select name="selTasks" size="4" multiple style="width:160px">
                                      <%
                                        while (! rsTask.EOF){
                                            rsTask.Read();
                                        %>
                                      <option value="<%=(rsTask.Item("Id"))%>" ><%=(rsTask.Item("TaskDescription"))%> (<%=Trim(rsTask.Item("TaskCode"))%>)</option>
                                      <%
                                          }
                                          

                                      rsTask.Requery();

                                    %>
                                    </select>
                                    </font></td>
                                  <td colspan="2" align='left'> 
                                    <select name="selCustomers" size="4" multiple style="width:160px">
                                      <%
                                    while (! rsCustomers.EOF){
                                        rsCustomers.Read();
                                    %>
                                      <option value="<%=(rsCustomers.Item("Id"))%>" ><%=(rsCustomers.Item("CustomerName"))%> (<%=(rsCustomers.Item("CustomerCode"))%>)</option>
                                      <%
                                          }
                                          

                                      rsCustomers.Requery();

                                    %>
                                  </select>
                                  </td>
                                  <td width="14%" align='center'> 
                                    <input type="button" name="btnDetail" value="Go" onClick="goDetail();">
                                  </td>
                                </tr>
                                <tr> 
                                  <td width="52%" align='right' valign="top">&nbsp;</td>
                                  <td colspan="2" align='left'>&nbsp; </td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td width="52%" align='center' valign="top"><font color="green">Origin</font><font color="green">s*</font></td>
                                  <td colspan="2" align='center'><font color="green">Manufacturers*</font></td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td width="52%" align='right' valign="top"><font color='green'> 
                                    <select name="selOrigins" size="4" multiple style="width:160px">
                                      <%
                                    while (! rsOrigin.EOF){
                                        rsOrigin.Read();
                                    %>
                                      <% if (Trim(rsOrigin.Item("OriginName")) != "NONE") { %>
                                      <option value="<%=(rsOrigin.Item("ID"))%>" ><%=(rsOrigin.Item("OriginName"))%> (<%=(rsOrigin.Item("OriginCode"))%>)</option>
                                      <% } %>
                                      <%
                                      }
                                      rsOrigin.Requery();
 
                                    %>
                                    </select>
                                    </font></td>
                                  <td colspan="2" align='left'> 
                                    <select name="selManufacturers" size="4" multiple style="width:160px">
                                      <%
                                        while (! rsMan.EOF){
                                            rsMan.Read();
                                        %>
                                      <%if (Trim(rsMan.Item("ManufacturerName")) != "NONE") { %>
                                      <option value="<%=(rsMan.Item("ID"))%>" ><%=(rsMan.Item("ManufacturerName"))%> (<%=(rsMan.Item("ManufacturerCode"))%>)</option>
                                      <% } %>
                                      <%
                                      }

                                      rsMan.Requery();

  
                                    %>
                                    </select>
                                  </td>
                                  <td width="14%" align='center'>&nbsp; </td>
                                </tr>
                                <tr> 
                                  <td width="52%" align='right' valign="top">&nbsp;</td>
                                  <td colspan="2" align='left'>&nbsp;</td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td colspan="3" align='left' class="required">&nbsp;* 
                                    Select one or more. if (none are selected, ALL is assumed</td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td colspan="3" align='left'  class="required">** 
                                    Select one or more facilities. if (none are selected, the default facility is used.</td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                              </table>
                            </form>
                            <!-- #EndEditable -->
                          </td>
                        </tr>
                    </table>
</asp:Content>