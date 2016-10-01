<%@ Page Language="C#" MasterPageFile="~/Rebilling/Rebilling.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="InterrailPPRS.Rebilling.Default" %>


<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script type="text/javascript" >

<!--

    function goNewDate() {
        var arDates = document.form1.selDateRange.value.split(",");
        document.form1.startDate.value = arDates[0];
        document.form1.action = "Default.aspx";
        //document.frmPrint.action = "/col.aspx";
        document.form1.submit();
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
        <asp:Label ID="lblDateRange" runat="server" Visible="true" ClientIDMode="Static" />
        <input type="hidden" id="dateRange" />
        
        <form runat="server">
            <div style="margin-bottom:-25px;">
                <h4>Rebilling approval status for:</h4>
                <asp:DropDownList ID="ddlDateRange" runat="server" OnSelectedIndexChanged="ddlDateRange_SelectedIndexChanged" AutoPostBack="true" /><p />
            </div>
             <% ShowApprovalStatus(); %>
            <!--
            <table width="90%" border="0" cellspacing="0" cellpadding="8">
                <tr>
                    <td><h3>Rebilling approval status for:</h3></td>
                    <td><asp:DropDownList ID="ddlDateRange2" runat="server" OnSelectedIndexChanged="ddlDateRange_SelectedIndexChanged" AutoPostBack="true" /></td>
                </tr>
                <tr> 
                    <td> <!-- #BeginEditable "MainBody" -->
                    <%-- ShowApprovalStatus(); --%>
                    <!-- #EndEditable --> 
                    <!--</td>
                </tr>       
	        </table> 
            -->
        </form>       
</asp:Content>