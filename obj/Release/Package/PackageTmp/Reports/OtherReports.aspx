<%@ Page Language="C#" MasterPageFile="~/Reports/Reports.Master" AutoEventWireup="true" CodeBehind="OtherReports.aspx.cs" Inherits="InterrailPPRS.Reports.OtherReports" %>



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



    function goValidate(rptName) {

        if (rptName == "OriginReport") {
            var from = document.frmPrint.originfromDate;
            var to = document.frmPrint.origintoDate;
            var dateFrom = new Date(Date.parse(document.frmPrint.originfromDate.value));
            var dateTo = new Date(Date.parse(document.frmPrint.origintoDate.value));
        }
        else if (rptName == "ManufacturerSummary") {
            var from = document.frmPrint.manufacturerfromDate;
            var to = document.frmPrint.manufacturertoDate;
            var dateFrom = new Date(Date.parse(document.frmPrint.manufacturerfromDate.value));
            var dateTo = new Date(Date.parse(document.frmPrint.manufacturertoDate.value));
        }
        else if (rptName == "Termination") {
            var from = document.frmPrint.terminationfromDate;
            var to = document.frmPrint.terminationtoDate;
            var dateFrom = new Date(Date.parse(document.frmPrint.terminationfromDate.value));
            var dateTo = new Date(Date.parse(document.frmPrint.terminationtoDate.value));
        }
        else if (rptName == "CostPerUnit") {
            var from = document.frmPrint.costfromDate;
            var to = document.frmPrint.costtoDate;
            var dateFrom = new Date(Date.parse(document.frmPrint.costfromDate.value));
            var dateTo = new Date(Date.parse(document.frmPrint.costtoDate.value));
        }

        if (ValidDate(from, 1, "Date From") != true) { return false; }
        if (ValidDate(to, 1, "Date To") != true) { return false; }

        if (dateFrom > dateTo) {
            from.focus();
            from.select();
            return false;
        }

        if (rptName == "OriginReport") {
            // if (none are selected, select all....
            if (selectedFacilities() == 1) {
                document.frmPrint.selFacilities.value = "0"

                for (var i = 0; i < document.frmPrint.selOriginFacilities.length; i++) {
                    if (document.frmPrint.selOriginFacilities.options[i].selected == true) {
                        document.frmPrint.selFacilities.value = document.frmPrint.selFacilities.value + ", " + document.frmPrint.selOriginFacilities.options[i].value;
                    }
                }
            }

            sURL = "fromDate=" + document.frmPrint.originfromDate.value + "&toDate=" + document.frmPrint.origintoDate.value
            document.frmPrint.action = "GenerateOrigin.aspx?" + sURL;
        }
        else if (rptName == "ManufacturerSummary") {
            sURL = "fromDate=" + document.frmPrint.manufacturerfromDate.value + "&toDate=" + document.frmPrint.manufacturertoDate.value
            document.frmPrint.action = "GenerateManufacturerSummary.aspx?" + sURL;
        }
        else if (rptName == "Termination") {
            sURL = "fromDate=" + document.frmPrint.terminationfromDate.value + "&toDate=" + document.frmPrint.terminationtoDate.value
            document.frmPrint.action = "GenerateTermination.aspx?" + sURL;
        }
        else if (rptName == "CostPerUnit") {
            sURL = "fromDate=" + document.frmPrint.costfromDate.value + "&toDate=" + document.frmPrint.costtoDate.value
            document.frmPrint.action = "GenerateCostPerUnit.aspx?" + sURL;
        }

        document.frmPrint.submit();
    }


    function selectedFacilities() {
        for (var i = 0; i < document.frmPrint.selOriginFacilities.length; i++) {
            if (document.frmPrint.selOriginFacilities.options[i].selected == true) {
                return 1
            }
        }
        return 0
    }

    function goNoParams(rptName) {
        document.frmPrint.action = "OtherReports.aspx?RptType=" + rptName;
        document.frmPrint.submit();

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


                              <input type='hidden' name='selFacilities' value='<%=sFacilities%>'>

                              <!--  =========================== -->
                              <!--        Origin  Report        -->
                              <!--  =========================== -->

                              <table width="91%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                  <td colspan="4" class="pageTitle" align='center'>
                                    <div class="cellTopBottomBorder">Origin Report</div>
                                  </td>
                                </tr>
                                <tr>
                                  <td width="52%" align='right'  >&nbsp;</td>
                                  <td width="17%" align='center' >&nbsp;</td>
                                  <td width="17%" align='center' >&nbsp;</td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="52%" align='center'  ><font color='green'>Facilities*</font></td>
                                  <td colspan="2" align='center' ><font color='green'>Date
                                    Range</font></td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr>
                                  <td rowspan="2" align='right'><font color='green'>

                                    <select name="selOriginFacilities" size="4" multiple style="width:160px">
                                      <%
                                      while (! rsFac.EOF){ 
                                       rsFac.Read();
                                       %>
                                        <option value="<%=(rsFac.Item("Id"))%>" <%if (cStr(rsFac.Item("Id")) == cStr(Session["FacilityID"])) { Response.Write("SELECTED"); }else{ Response.Write(""); } %>><%=(rsFac.Item("Name"))%> (<%=(rsFac.Item("AlphaCode"))%>)</option>
                                       <%
                                      }
                                      %>
                                    </select>
                                    </font></td>
                                  <td colspan="2" align='center' class='cellTopBottomBorder'>
                                    <input type="text" name="originfromDate" size="10" maxlength="10" value='<%=Session["LastStartDate"]%>'>
                                    <input type="text" name="origintoDate" size="10" maxlength="10" value='<%=Session["LastEndDate"]%>'>
                                  </td>
                                  <td width="14%" align='center'>
                                    <input type="button" name="btnRange" value="Go" onClick="goValidate('OriginReport');">
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
                                <tr>
                                  <td colspan="4" align='right'  class="required">*
                                    Select one or more facilities. if (none are
                                    selected, the default facility is used.</td>
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
                                <tr>

                              </table>

                              <!--  =========================== -->
                              <!--  Manufacturer Summary Report -->
                              <!--  =========================== -->

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
                                  <td align='right'><font color='green'>Date Range: &nbsp;</font></td>
                                  <td colspan="2" align='center' class='cellTopBottomBorder'>
                                    <input type="text" name="manufacturerfromDate" size="10" maxlength="10" value='<%=Session["LastStartDate"]%>'>
                                    <input type="text" name="manufacturertoDate" size="10" maxlength="10" value='<%=Session["LastEndDate"]%>'>
                                  </td>
                                  <td width="14%" align='center'>
                                    <input type="button" name="btnRange" value="Go" onClick="goValidate('ManufacturerSummary');">
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
                              </table>


                              <!--  =========================== -->
                              <!--   Users By Facility  Report  -->
                              <!--  =========================== -->

                              <table width="91%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                  <td colspan="5" class="pageTitle" align='center'>
                                    <div class="cellTopBottomBorder">Users By Facility Report</div>
                                  </td>
                                </tr>
                                <tr>
                                  <td width="52%" align='center' valign="top">&nbsp;</td>
                                  <td colspan="2" align='center'>&nbsp;</td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="14%" align='center'>&nbsp;</td>
                                  <td colspan="4" align='center' class=''>
                                    <input type="button" name="btnRange" value="Generate Users By Facility Report" onClick="goNoParams('USERSBYFACILITY');">
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

                              </table>

                              <!--  =========================== -->
                              <!--       Termination Report     -->
                              <!--  =========================== -->

                              <table width="91%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                  <td colspan="4" class="pageTitle" align='center'>
                                    <div class="cellTopBottomBorder">Termination Report</div>
                                  </td>
                                </tr>
                                <tr>
                                  <td width="52%" align='right'  >&nbsp;</td>
                                  <td width="17%" align='center' >&nbsp;</td>
                                  <td width="17%" align='center' >&nbsp;</td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr>
                                  <td align='right'><font color='green'>Date Range: &nbsp;</font></td>
                                  <td colspan="2" align='center' class='cellTopBottomBorder'>
                                    <input type="text" name="terminationfromDate" size="10" maxlength="10" value='<%=Session["LastStartDate"]%>'>
                                    <input type="text" name="terminationtoDate" size="10" maxlength="10" value='<%=Session["LastEndDate"]%>'>
                                  </td>
                                  <td width="14%" align='center'>
                                    <input type="button" name="btnRange" value="Go" onClick="goValidate('Termination');">
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
                              </table>



                              <!--  =========================== -->
                              <!--  Temporary Employees Report  -->
                              <!--  =========================== -->

                              <table width="91%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                  <td colspan="5" class="pageTitle" align='center'>
                                    <div class="cellTopBottomBorder">Temporary Employees Report</div>
                                  </td>
                                </tr>
                                <tr>
                                  <td width="52%" align='center' valign="top">&nbsp;</td>
                                  <td colspan="2" align='center'>&nbsp;</td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="14%" align='center'>&nbsp;</td>
                                  <td colspan="4" align='center' class=''>
                                    <input type="button" name="btnRange" value="Generate Temporary Employees Report" onClick="goNoParams('TEMPORARY');">
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

                              </table>

                              <!--  =========================== -->
                              <!--      Cost Per Unit Report    -->
                              <!--  =========================== -->

                              <table width="91%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                  <td colspan="4" class="pageTitle" align='center'>
                                    <div class="cellTopBottomBorder">Cost Per Unit Report</div>
                                  </td>
                                </tr>
                                <tr>
                                  <td width="52%" align='right'  >&nbsp;</td>
                                  <td width="17%" align='center' >&nbsp;</td>
                                  <td width="17%" align='center' >&nbsp;</td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr>
                                  <td align='right'><font color='green'>Date Range: &nbsp;</font></td>
                                  <td colspan="2" align='center' class='cellTopBottomBorder'>
                                    <input type="text" name="costfromDate" size="10" maxlength="10" value='<%=Session["LastStartDate"]%>'>
                                    <input type="text" name="costtoDate" size="10" maxlength="10" value='<%=Session["LastEndDate"]%>'>
                                  </td>
                                  <td width="14%" align='center'>
                                    <input type="button" name="btnRange" value="Go" onClick="goValidate('CostPerUnit');">
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
                              </table>

                            </form>



                            <!-- #EndEditable -->
                          </td>
                        </tr>
                    </table>
</asp:Content>