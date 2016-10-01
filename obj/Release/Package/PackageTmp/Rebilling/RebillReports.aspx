<%@ Page Language="C#" MasterPageFile="~/Rebilling/Rebilling.Master" AutoEventWireup="true" CodeBehind="RebillReports.aspx.cs" Inherits="InterrailPPRS.Rebilling.RebillReports" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script language="JavaScript">

    function bodyload() {

        OriginalFormCheckSumValue = CheckStringForForm(document.form1);
    }

    function bodyunload() {
        if (CheckStringForForm(document.form1) != OriginalFormCheckSumValue) {
            event.returnValue = "You have not saved your changes.";
        }
    }

    function goDetail() {
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

        document.frmPrint.action = "RebillDetailReport.aspx?PrintPreview=1";
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
        <%if (CheckSecurity("Super, Admin, User")){ %>
        <tr>
        <td width="8%">&nbsp; </td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
        <td width="79%"><a href="RebillDetail.aspx">Rebill Detail</a></td>
        </tr>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
        <td width="79%"><a href="ReconcileRebilling.aspx">Reconcile Rebilling</a></td>
        </tr>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
        <td width="79%"><a href="RebillingInvoices.aspx">Generate Invoices</a></td>
        </tr>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
        <td width="79%"><a href="ApproveRebillData.aspx">Approve Rebill Data</a></td>
        </tr>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
        <td width="79%"><a href="Profitability.aspx?PrintPreview=1&selFacilities=<%=Session["FacilityID"]%>">Profitability Report</a></td>
        </tr>
        <% } %>
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
            <td> <!-- #BeginEditable "MainBody" -->
            <form name='frmPrint' method='post' action=''>
                <table width="91%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td colspan="4" class="pageTitle" align='center'>
                    <div class="cellTopBottomBorder">Re-billing
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
                    <td width="52%" align='right'  >
                    <input type="radio" id="rdoSummary" name="rptType" value="Summary" checked>
                    <label for=rdoSummary>Summary Report</label></td>
                    <td width="17%" align='center' >&nbsp;</td>
                    <td colspan="2" align='left' >
                    <input type="radio" id="rdoDetail" name="rptType" value="Detail">
                    <label for=rdoDetail>Detail Report</label></td>
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
                        <option value="<%=(rsFac.Item("Id"))%>" <%if (cStr(rsFac.Item("Id")) == cStr(Session["FacilityID"])) { Response.Write("SELECTED"); }else{ Response.Write(""); }%>><%=(rsFac.Item("Name"))%> (<%=(rsFac.Item("AlphaCode"))%>)</option>
                        <%
                            }
                        rsFac.Requery();

                    %>
                    </select>
                    </font></td>
                    <td width="17%" align='center' class='cellTopBottomBorder'>From</td>
                    <td width="17%" align='center' class='cellTopBottomBorder'>To</td>
                    <td width="14%" align='center'>&nbsp;</td>
                </tr>
                <tr>
                    <td width="17%" align='center'>
                    <input type="text" name="fromDateDetail" size="10" maxlength="10" value='<%=System.DateTime.Now.ToShortDateString()%>'>
                    </td>
                    <td width="17%" align='left'>
                    <input type="text" name="toDateDetail" size="10" maxlength="10" value='<%=System.DateTime.Now.ToShortDateString()%>'>
                    </td>
                    <td width="14%" align='center'>&nbsp; </td>
                </tr>
                <tr>
                    <td width="52%" align='center' valign="top">&nbsp;</td>
                    <td colspan="2" align='center'>&nbsp;</td>
                    <td width="14%" align='center'>&nbsp;</td>
                </tr>
                <tr>
                    <td width="52%" align='center' valign="top"><font color='green'>SubTasks*&nbsp;</font></td>
                    <td colspan="2" align='center'><font color="green"></font></td>
                    <td width="14%" align='center'>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3" width="52%" align='right' valign="top"><font color='green'>
                    <select name="selSubTasks" size="8" multiple style="width:360px">
                        <%
                    while (!rsSubTask.EOF){
                        rsSubTask.Read();
                    %>
                        <option value="<%=(rsSubTask.Item("Id"))%>" ><%=(rsSubTask.Item("Description"))%> (<%=Trim(rsSubTask.Item("TaskCode"))%> in <%=Trim(rsSubTask.Item("Name"))%>)</option>
                        <%
                            }
                                          

                        rsSubTask.Requery();

                    %>
                    </select>
                    </font></td>
                    <td width="14%" align='center'>&nbsp;</td>
                    </tr>
                    <tr>
                    <td width="52%" align='center' valign="top">&nbsp;</td>
                    <td colspan="2" align='center'>&nbsp;</td>
                    <td width="14%" align='center'>&nbsp;</td>
                </tr>
                <tr>
                    <td width="52%" align='center' valign="top"><font color='green'>Customers*&nbsp;</font></td>
                    <td colspan="2" align='center'><font color="green"></font></td>
                    <td width="14%" align='center'>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2" align='center'>
                    <select name="selCustomers" size="4" multiple style="width:260px">
                        <%
                    while (! rsCustomers.EOF){
                        rsCustomers.Read();
                    %>
 
                        <option value="<%=(rsCustomers.Item("Id"))%>" ><%=(rsCustomers.Item("AlphaCode"))%> <%=(rsCustomers.Item("CustomerCode"))%> - <%=(rsCustomers.Item("ContactName"))%></option>
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
                    <td colspan="3" align='right' class="required">&nbsp;*
                    Select one or more. if (none are selected,
                    ALL is assumed</td>
                    <td width="14%" align='center'>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3" align='right'  class="required">**
                    Select one or more facilities. if (none are
                    selected, the default facility is used.</td>
                    <td width="14%" align='center'>&nbsp;</td>
                </tr>
                </table>
            </form>
            <!-- #EndEditable --> </td>
        </tr>
        </table>
</asp:Content>