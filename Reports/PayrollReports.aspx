<%@ Page Language="C#" MasterPageFile="~/Reports/Reports.Master" AutoEventWireup="true" CodeBehind="PayrollReports.aspx.cs" Inherits="InterrailPPRS.Reports.PayrollReports" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

<script type="text/javascript">
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

    function setDates() {
        var arDates = document.frmPrint.selDateRange.value.split(",");
        document.frmPrint.fromDateDetail.value = arDates[0];
        document.frmPrint.toDateDetail.value = arDates[1];
        alert(arDates[0]);
    }

    function goDetail(et) {
        var from = document.frmPrint.fromDateDetail;
        var to = document.frmPrint.toDateDetail;

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

        if (et == 'pe') {
            document.frmPrint.action = "PayrollDetailReport.aspx?PrintPreview=1&Type=Perm";
        }
        else {
            if (et == 'te') {
                document.frmPrint.action = "PayrollDetailReport.aspx?PrintPreview=1&Type=Temp";
            }
            else {
                document.frmPrint.action = "PayrollDetailReport.aspx?PrintPreview=1&Type=All";
            }
        }
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
                                    <div class="cellTopBottomBorder">Payroll Report</div>
                                  </td>
                                </tr>
                                <tr> 
                                  <td width="42%" align='right'  >&nbsp; </td>
                                  <td width="22%" align='center' >&nbsp;</td>
                                  <td width="18%" align='center' >&nbsp;</td>
                                  <td width="18%" align='center'>&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td width="42%" align='center'  ><font color='green'>Facilities*</font></td>
                                  <td colspan="2" align='center' ><font color="green">Date Range </font></td>
                                  <td width="18%" align='center'>&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td align='right' width="42%"><font color='green'> 
                                    <select name="selFacilities" size="8" multiple style="width:160px">
                                        <%
                                        while (! rsFac.EOF){
                                            rsFac.Read();
                                        %>
                                        <option value="<%=rsFac.Item("Id")%>" <% if (cStr(rsFac.Item("Id")) == cStr(Session["FacilityID"])){ Response.Write("SELECTED");}else{ Response.Write("");} %>> <%=rsFac.Item("Name")%> ( <%=rsFac.Item("AlphaCode")%> )</option>
                                        <%

                                        }
                                          rsFac.Requery();
                                       %>
                                    </select>
                                    </font></td>
                                  <td colspan="2" align='center'  valign='top'> 

                                    <input type="text" name="fromDateDetail" size="10" maxlength="10" value="<% =cStr(Session["LastStartDate"]) %>" />
                                    <input type="text" name="toDateDetail"   size="10" maxlength="10" value="<% =cStr(Session["LastEndDate"]) %>" />
                                    <br>
                                    <input type="checkbox" name="summary">
                                    Summary </td>
                                  <td align='center' valign='top' width="18%"> 
                                    <input type="button" name="btnDetail" value="Permanent" onClick="goDetail('pe');" />
                                    <input type="button" name="btnTemp"   value="Temporary" onClick="goDetail('te');" />
                                    <input type="button" name="btnAll"    value="      All     "  onClick="goDetail('all');" />
                                  </td>
                                </tr>
                                <tr> 
                                  <td width="42%" align='center' valign="top">&nbsp;</td>
                                  <td colspan="2" align='center'>&nbsp; </td>
                                  <td width="18%" align='center'>&nbsp; </td>
                                </tr>
                                <tr> 
                                  <td colspan="3" align='right'  class="required">* 
                                    Select one or more facilities. if none are selected, the default facility is used.</td>
                                  <td width="18%" align='center'>&nbsp;</td>
                                </tr>
                              </table>
                            </form>
                            <!-- #EndEditable -->
                          </td>
                        </tr>
                    </table>
</asp:Content>