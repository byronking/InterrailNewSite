<%@ Page Language="C#" MasterPageFile="~/Rebilling/Rebilling.Master" AutoEventWireup="true" CodeBehind="Profitability.aspx.cs" Inherits="InterrailPPRS.Rebilling.Profitability" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script language="JavaScript">


    function bodyload() {
        var frm = document.form1;
    }

    function goShowReport() {
        var from = document.form1.fromDate;
        var to = document.form1.toDate;

        if (ValidDate(from, 1, "Date From") != true) { return false; }
        if (ValidDate(to, 1, "Date To") != true) { return false; }

        var dateFrom = new Date(Date.parse(document.form1.fromDate.value));
        var dateTo = new Date(Date.parse(document.form1.toDate.value));

        if (dateFrom > dateTo) {
            alert(" 'Date From' must be prior to 'Date To'.");
            from.focus();
            from.select();
            return false;
        }

        if (form1.selFacilities.selectedIndex == -1) {
            document.form1.action = 'Profitability.aspx?PrintPreview=1&selFacilities=<%=Session["FacilityID"]%>'
        }
        else {
            document.form1.action = 'Profitability.aspx?PrintPreview=1';
        }

        //document.form1.action = "/col.aspx";
        document.form1.submit();

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
        <%if (CheckSecurity("Super, Admin, User")) { %>
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
            <td><!-- #BeginEditable "MainBody" -->
            <form name='form1' method='post' action='Profitability.aspx?PrintPreview=0'>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <% if ( Request["PrintPreview"] != null && cStr(Request["PrintPreview"]).Length > 0) { %>
                <tr>
                    <td colspan="4" class="pageTitle" align='center'>
                    <div class="cellTopBottomBorder">Re-billing
                        Profitability Analysis Report</div>
                    </td>
                </tr>
                <tr>
                    <td width="52%" align='right' height="16">&nbsp;</td>
                    <td width="17%" align='center' height="16">&nbsp;</td>
                    <td width="17%" align='center' height="16">&nbsp;</td>
                    <td width="14%" height="16">&nbsp;</td>
                </tr>
                <tr>
                    <td width="52%" align='right'  >&nbsp;</td>
                    <td width="17%" align='center' >&nbsp;</td>
                    <td width="17%" align='center' >&nbsp;</td>
                    <td width="14%" align='center'>&nbsp;</td>
                </tr>
                <tr>
                    <td width="52%" align='center'  >&nbsp;</td>
                    <td colspan="2" align='center' ><font color='green'>Date
                    Range</font></td>
                    <td width="14%" align='center'>&nbsp;</td>
                </tr>
                <tr>
                    <td width="52%" align='center'  ><font color="green">Facilities*</font></td>
                    <td align='center'  class='cellTopBottomBorder'>From</td>
                    <td align='center'  class='cellTopBottomBorder'>To</td>
                    <td width="14%" align='center'>&nbsp;</td>
                </tr>
                <tr>
                    <td width="52%" align='right'  >
                    <select name="selFacilities" size="10" multiple style="width:200px">
                        <%
                        while (! rsFac.EOF){
                            rsFac.Read();
                        %>
                        <%
                        arFac = Split(Request["selFacilities"], ",");
                        string sSelected = " ";
                        for(int i=LBound(arFac); i < UBound(arFac); i ++){
                        if (cInt(rsFac.Item("id")) == cInt(arFac[i]) ) {
                            sSelected = "SELECTED";
                        }
                        }
                        %>
                        <option value='<%=(rsFac.Item("Id"))%>' <%= sSelected %> ><%=(rsFac.Item("Name"))%> (<%=(rsFac.Item("AlphaCode"))%>)</option>
                        <%
                                          
                        }

                        rsFac.Requery();

                    %>

                    </select>
                    </td>
                    <td align='center' valign="top">
                    <input type="text" name="fromDate" size="10" maxlength="10" value='<%=sFrom%>'>
                    </td>
                    <td align='center' valign="top">
                    <input type="text" name="toDate" size="10" maxlength="10" value='<%=sTo%>'>
                    </td>
                    <td width="14%" align='center' valign="top">
                    <input type="button" name="btnDetail" value="Go" onClick="goShowReport();">
                    </td>
                </tr>
                <tr>
                    <td align='right'><font color='green'> </font></td>
                    <td colspan="3" align='center'>&nbsp; </td>
                </tr>
                <tr>
                    <td width="52%" align='center' valign="top"><font color='green'>
                    </font></td>
                    <td colspan="2" align='center'><font color='green'>
                    </font></td>
                    <td width="14%" align='center'>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3" align='left' class="required">&nbsp;*
                    Select one or more facilities. if (none are
                    selected, the default facility is used.</td>
                    <td width="14%" align='center'>&nbsp;</td>
                </tr>
                <tr>
                    <td align='right' class="required">&nbsp;</td>
                    <td align='right' class="required">&nbsp;</td>
                    <td align='right' class="required">&nbsp;</td>
                    <td width="14%" align='center'>&nbsp;</td>
                </tr>
                <% } %>
                <% if (sViewReport)
                    { %>
                <tr>
                    <td align='right' class="required" colspan="4" valign='top'>
                    <%  ShowProfitabilityReport(); %> </td>
                </tr>
                <% } %>
                </table>
            </form>
            <!-- #EndEditable -->
            </td>
        </tr>
    </table>
</asp:Content>