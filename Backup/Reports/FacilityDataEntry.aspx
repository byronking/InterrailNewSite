<%@ Page Language="C#" MasterPageFile="~/Reports/Reports.Master" AutoEventWireup="true" CodeBehind="FacilityDataEntry.aspx.cs" Inherits="InterrailPPRS.Reports.FacilityDataEntry" %>



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

        // SPOTTING
        //
        if (IsValidTime(frm.SPOTTING__SPOTTIME.value) != true) { return false; }
        if (ValidPositiveInteger(frm.SPOTTING__RC_IN, 1, " 'RailCars In' ") != true) { return false; }
        if (ValidPositiveInteger(frm.SPOTTING__RC_OUT, 1, " 'RailCars Out' ") != true) { return false; }
        if (ValidPositiveNumber(frm.SPOTTING__DOWN_TIME, 1, " 'Down Time' ") != true) { return false; }

        // KPI's
        //
        if (ValidPositiveInteger(frm.KPI__ACCIDENTS, 0, " 'Accidents' ") != true) { return false; }
        if (ValidPositiveInteger(frm.KPI__REP_INJURIES, 0, " 'Reportable Injuries' ") != true) { return false; }
        if (ValidPositiveNumber(frm.KPI__LOST_TIME, 0, " 'Lost Time' ") != true) { return false; }
        if (ValidPositiveInteger(frm.KPI__NON_MEDICAL, 0, " '1st Aid Injuries' ") != true) { return false; }

        // SELF AUDITS
        //
        if (ValidPositiveNumber(frm.SELF_AUDITS__BAY, 0, " 'Bay' ") != true) { return false; }
        if (ValidPositiveNumber(frm.SELF_AUDITS__FACILITY, 0, " 'Facility' ") != true) { return false; }

        // DETERMINING_FACTOR
        //
        if (MaximumLength(frm.FACTORS__DETERMINING_FACTOR.value, 250) != true) { return false; }

        frm.action = "FacilityDataEntry.aspx?MODE=SAVE"
        frm.submit(); 1


    }

    function CheckChars(count) {
        var MaxAllowed = 250
        var LengthCheck = count.length;

        if (LengthCheck > MaxAllowed){
            alert("DETERMINING FACTOR must only be " + MaxAllowed + " characters long.");
       }


        document.getElementById("divCharCount").innerHTML = "Character count :" + count.length + " of " + MaxAllowed + " allowed."; 


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
                            <form name='frmSave' method='post' action='FacilityMonitor.aspx' onkeypress="formkeypress();">
                                <table width="91%" border="0" cellspacing="0" cellpadding="0">
                                  <tr>
                                    <td colspan="4" class="pageTitle" align='center'>
                                      <div class="cellTopBorder">Facility Monitoring Data Entry - <%= Session["FacilityName"] %> </div>
                                    </td>
                                  </tr>
                                  <tr>
                                    <td colspan="4" class="pageTitle" align='center'>
                                      <div class="cellBottomBorder"><%= MonthName(cInt(selMonth)) + " " + selDay + ", " + selYear %> </div>
                                    </td>
                                  </tr>

                                  <tr><td width='100%' colspan='4'>&nbsp;</td></tr>


                                  <% ShowSpotting();%>

                                  <tr><td width='100%' colspan='4'>&nbsp;</td></tr>

                                  <% ShowKPIs(); %>


                                  <tr><td width='100%' colspan='4'>&nbsp;</td></tr>

                                  <% ShowSelfAudits(); %>



                                  <tr><td width='100%' colspan='4'>&nbsp;</td></tr>

                                  <% ShowDetermingFactors(); %>


                                  <tr><td width='100%' colspan='4'>&nbsp;</td></tr>


                                 <input type=hidden name=selDate value='<%=selDate%>'>
                                    <td width="22%" align='center' ><input type=hidden name=sFac    value='<%=sFac%>'>
                                  </tr>

                                  <tr><td width='100%' colspan='4' align='center'><input type='button' value='Save' onClick='goSave();' name='btnSave'>&nbsp;&nbsp;<input type='submit' value='Back' name='btnBack'></td></tr>

                                </table>
                            </form>
                            <!-- #EndEditable -->
                          </td>
                        </tr>
                      </table>
</asp:Content>