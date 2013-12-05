<%@ Page Language="C#" MasterPageFile="~/Rebilling/Rebilling.Master" AutoEventWireup="true" CodeBehind="SampleForm.aspx.cs" Inherits="InterrailPPRS.Rebilling.SampleForm" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

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

</asp:Content>