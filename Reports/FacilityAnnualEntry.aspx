<%@ Page Language="C#" MasterPageFile="~/Reports/Reports.Master" AutoEventWireup="true" CodeBehind="FacilityAnnualEntry.aspx.cs" Inherits="InterrailPPRS.Reports.FacilityAnnualEntry" %>



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

    function goSave() {

        var frm = document.frmSave;

        if (ValidPositiveNumber(frm.jan, 0, " 'January' ") != true) { return false; }
        if (ValidPositiveNumber(frm.feb, 0, " 'February' ") != true) { return false; }
        if (ValidPositiveNumber(frm.mar, 0, " 'March' ") != true) { return false; }
        if (ValidPositiveNumber(frm.apr, 0, " 'April' ") != true) { return false; }
        if (ValidPositiveNumber(frm.may, 0, " 'May' ") != true) { return false; }
        if (ValidPositiveNumber(frm.jun, 0, " 'June' ") != true) { return false; }
        if (ValidPositiveNumber(frm.jul, 0, " 'July' ") != true) { return false; }
        if (ValidPositiveNumber(frm.aug, 0, " 'August' ") != true) { return false; }
        if (ValidPositiveNumber(frm.sep, 0, " 'September' ") != true) { return false; }
        if (ValidPositiveNumber(frm.oct, 0, " 'October' ") != true) { return false; }
        if (ValidPositiveNumber(frm.nov, 0, " 'November' ") != true) { return false; }
        if (ValidPositiveNumber(frm.dec, 0, " 'December' ") != true) { return false; }

        if (lessThanOneHundred() != true) { return false; }

        frm.action = "FacilityAnnualEntry.aspx?MODE=SAVE"
        frm.submit();

    }

    function lessThanOneHundred() {

        var frm = document.frmSave;
        var jan = parseFloat(frm.jan.value);
        var feb = parseFloat(frm.feb.value);
        var mar = parseFloat(frm.mar.value);
        var apr = parseFloat(frm.apr.value);
        var may = parseFloat(frm.may.value);
        var jun = parseFloat(frm.jun.value);
        var jul = parseFloat(frm.jul.value);
        var aug = parseFloat(frm.aug.value);
        var sep = parseFloat(frm.sep.value);
        var oct = parseFloat(frm.oct.value);
        var nov = parseFloat(frm.nov.value);
        var dec = parseFloat(frm.dec.value);
        var total = jan + feb + mar + apr + may + jun + jul + aug + sep + oct + nov + dec;

        if (total > 100) {
            alert("Combined total of all months must be equal or less than 100%");
            return false;
        } else {
            alert("Your combined total of all months equals " + total + "%");
            return true;
        }
    }

    function CheckValue(inputValue) {
        if (ValidPositiveNumber(inputValue, 0, "this input box") != true) { return false; }
    }

    function formkeypress() {
        if (event.keyCode == 13) {
            for (i = 0; i < frmSave.elements.length; i++) {
                if (frmSave.elements(i).name == 'btnSave') {
                    return true;
                }

                if ((frmSave.elements(i).name == 'btnBack')) {
                    frmSave.elements(i).onClick();
                    window.event.returnValue = false;
                    return;
                }

                if (frmSave.elements(i) == document.activeElement) {
                    if (i == frmSave.elements.length - 1)
                        i = -1;
                    while (frmSave.elements(i + 1).type == 'hidden') {
                        i++;
                    }

                    frmSave.elements(i + 1).focus();
                    try {
                        frmSave.elements(i + 1).select();
                    }
                    catch (localerror)
									{ }
                    window.event.returnValue = false;
                    return;
                }
            }
        }
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

                 <%if ( CheckSecurity("Super, Admin, User") ) { %>
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
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" /></td>
                    <td width="79%"><a href="../Logout.aspx">Logout</a></td>
                  </tr>

                </table>
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="Main" runat="server">
                     <table width="90%" border="0" cellspacing="0" cellpadding="8">
                        <tr>
                          <td valign="top"><!-- #BeginEditable "MainBody" -->
                            <form name='frmSave' method='post' action='FacilityMonitor.aspx' onkeypress="formkeypress();">
                                <table width="91%" border="0" cellspacing="0" cellpadding="0">
                                  <tr>
                                    <td colspan="4" class="pageTitle" align='center'>
                                      <div class="cellTopBorder">Facility Monitoring Annual Entry <br>Year of <%= selYear %></div>
                                    </td>
                                  </tr>
                                  <tr>
                                    <td colspan="4" class="pageTitle" align='center'>
                                      <div class="cellBottomBorder"> </div>
                                    </td>
                                  </tr>


                                  <tr><td width='100%' colspan='4'>&nbsp;</td></tr>

								  <tr><td width='110%' colspan='4'>
								  		<table border="0" cellspacing="0" cellpadding="0" width='100%'>

                                  			<% ShowMonthlyPercentage();%>

                                  		</table>
								  </td></tr>
                                  <tr><td width='100%' colspan='4' align='center'><input type='button' value='Save' onClick='goSave()' name='btnSave' ID="Button1">&nbsp;&nbsp;<input type='submit' value='Back' name='btnBack' ID="Submit1"></td></tr>

                                  <tr><td width='100%' colspan='4'>&nbsp;</td></tr>
										<table border="0" cellspacing="0" cellpadding="0" width='100%'>
											<% ShowFacilityBudget();%>
										</table>

                                  <tr><td width='100%' colspan='4'>&nbsp;</td></tr>

                                 <input type=hidden name=selYear value='<%=selYear%>'>
                                 <input type=hidden name=sFac    value='<%=sFac%>'>


                               	</table>
                            </form>
                            <!-- #EndEditable -->

</asp:Content>