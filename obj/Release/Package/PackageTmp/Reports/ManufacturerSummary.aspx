<%@ Page Language="C#" MasterPageFile="~/Reports/Reports.Master" AutoEventWireup="true" CodeBehind="ManufacturerSummary.aspx.cs" Inherits="InterrailPPRS.Reports.ManufacturerSummary" %>



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



    function goValidate() {
        var from = document.frmPrint.fromDate;
        var to = document.frmPrint.toDate;

        var dateFrom = new Date(Date.parse(document.frmPrint.fromDate.value));
        var dateTo = new Date(Date.parse(document.frmPrint.toDate.value));

        if (ValidDate(from, 1, "Date From") != true) { return false; }
        if (ValidDate(to, 1, "Date To") != true) { return false; }

        if (dateFrom > dateTo) {
            alert(" 'Date From' must be prior to 'Date To'.");
            from.focus();
            from.select();
            return false;
        }

        document.frmPrint.action = "GenerateManufacturerSummary.aspx";
        document.frmPrint.submit();
    }


    function selectedFacilities() {
        for (var i = 0; i < document.frmPrint.selFacilities.length; i++) {
            if (document.frmPrint.selFacilities.options[i].selected == true) {
                return 1
            }
        }
        return 0
    }


// -->
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
                                    <div class="cellTopBottomBorder">Manufacturer Summary Report</div>
                                  </td>
                                </tr>
                                <tr>
                                  <td width="52%" align='right'  >&nbsp;</td>
                                  <td width="17%" align='center' >&nbsp;</td>
                                  <td width="17%" align='center' >&nbsp;</td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>

                                <tr>

                                  <%
                                     string sFacilities = "0";
                                     while(! rsFac.EOF){
                                       rsFac.Read();  
                                       sFacilities = sFacilities + ", " + cStr(rsFac.Item("Id"));
                                     }
                                  %>
                                  <input type='hidden' name='seFacilities' value='<%=sFacilities%>'>
                                  <td align='right'><font color='green'>Date Range: &nbsp;</font></td>
                                  <td colspan="2" align='center' class='cellTopBottomBorder'>
                                    
                                    <input type="text" name="fromDate" size="10" maxlength="10" value='<%=System.DateTime.Now%>'>
                                    <input type="text" name="toDate" size="10" maxlength="10" value='<%=System.DateTime.Now%>'>
                                  </td>
                                  <td width="14%" align='center'>
                                    <input type="button" name="btnRange" value="Go" onClick="goValidate();">
                                  </td>

                                </tr>
                                <tr>
                                  <td colspan="2" align='center'  valign='top'>&nbsp;
                                  </td>
                                  <td width="14%" align='center'>&nbsp; </td>
                                </tr>
                                <tr>
                                  <td width="52%" align='center' valign="top">&nbsp;</td>
                                  <td colspan="2" align='center'>&nbsp;</td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="52%" align='right' valign="top">&nbsp;</td>
                                  <td colspan="2" align='left'>&nbsp;</td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <!--
                                <tr>
                                  <td colspan="4" align='right'  class="required">*
                                    Select one or more facilities. if (none are
                                    selected, the default facility is used.</td>
                                </tr>
                                -->
                              </table>
                            </form>





                            <!-- #EndEditable -->
                          </td>
                        </tr>
                    </table>
</asp:Content>