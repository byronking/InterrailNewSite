<%@ Page Language="C#" MasterPageFile="~/Reports/Reports.Master" AutoEventWireup="true" CodeBehind="FacilityAnnualEntryDetail.aspx.cs" Inherits="InterrailPPRS.Reports.FacilityAnnualEntryDetail" %>

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
        frm.action = "FacilityAnnualEntryDetail.aspx?MODE=SAVE"
        frm.submit();
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
            <form id="frmSave" name="frmSave" method="post" action='FacilityAnnualEntry.aspx?selYear4=<%=selYear%>' onkeypress="formkeypress();">
                <table width="91%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                    <td colspan="4" class="pageTitle" align='center'>
                        <div class="cellTopBorder">Facility Monitoring Annual Entry<br><%=facilityname%> <br>Year of <%= selYear %></div>
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

                            <% ShowFacilityLoad(); %>

                        </table>
				    </td></tr>

                    <tr><td width='100%' colspan='4'>&nbsp;</td></tr>
					    <table border="0" cellspacing="0" cellpadding="0" width='100%'>
						    <% ShowFacilityBudgetTask(); %>
					    </table>

                    <tr><td width='100%' colspan='4'>&nbsp;</td></tr>

                    <input type=hidden name=selYear value='<%=selYear%>'>
                    <input type=hidden name=FID    value='<%=sFac%>'>
                    <input type=hidden name=FNAME    value='<%=facilityname%>' >
                    <tr><td width='100%' colspan='4' align='center'>
                        <%--<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click1" />--%>
                        <input type="button" value="Save" onclick="goSave()" name="btnSave" id="btnSave" />&nbsp;&nbsp;
                        <input type="submit" value="Back" name="btnBack" id="btnBack" /></td></tr>

                </table>
            </form>
<!-- #EndEditable -->
    </asp:Content>