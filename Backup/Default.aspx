<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Default.aspx.cs" Inherits="InterrailPPRS.Default" %>




<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

</asp:Content>

<asp:Content  ID="leftNavigation" ContentPlaceHolderID="Navigation" runat="server">
<!--- hi there! --->
        <table width="81%" border="0"  valign="top">
            <tr>
            <td width="8%">&nbsp;</td>
            <td width="13%"><img src="./Images/SmallRedArrow.gif" width="10" height="12"></td>
            <td width="79%"><a href="Admin/Default.aspx">Administrative</a></td>
            </tr>
            <tr>
            <td width="8%">&nbsp;</td>
            <td width="13%"><img src="./Images/SmallRedArrow.gif" width="10" height="12"></td>
            <td width="79%"><a href="Production/Default.aspx">Production</a></td>
            </tr>
            <tr>
            <td width="8%">&nbsp;</td>
            <td width="13%"><img src="./Images/SmallRedArrow.gif" width="10" height="12"></td>
            <td width="79%"><a href="Payroll/Default.aspx">Payroll</a></td>
            </tr>
            <tr>
            <td width="8%">&nbsp;</td>
            <td width="13%"><img src="./Images/SmallRedArrow.gif" width="10" height="12"></td>
            <td width="79%"><a href="Reports/Default.aspx">Reports</a></td>
            </tr>

            <tr>
            <td width="8%">&nbsp;</td>
            <td width="13%"><img src="./Images/SmallRedArrow.gif" width="10" height="12"></td>
            <td width="79%"><a href="Rebilling/Default.aspx">Re-Billing</a></td>
            </tr>

            <tr>
            <td width="8%">&nbsp;</td>
            <td width="13%">&nbsp;</td>
            <td width="79%">&nbsp;</td>
            </tr>
            <tr>
            <td width="8%">&nbsp;</td>
            <td width="13%"><img src="./Images/SmallRedArrow.gif" width="10" height="12"></td>
            <td width="79%"><a href="./Logout.aspx">Logout</a></td>
            </tr>

        </table>

</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="bodycontent" runat="server">

    <table width="90%" border="0" cellspacing="0" cellpadding="8">
        <tr>
            <td>&nbsp; </td>
        </tr>
    </table>
        <p>&nbsp;</p>
        <p>&nbsp;</p>
        <p>&nbsp;</p>
        <p>&nbsp;</p>
        <p>&nbsp;</p>
        <p>&nbsp;</p>
        <p>&nbsp;</p>
        <p>&nbsp;</p>

</asp:Content> 