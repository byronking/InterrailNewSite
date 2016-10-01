<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="InterrailPPRS.Admin.Default" %>
<%@ Register Src="~/UserControls/LeftNavMenu.ascx" TagName="LeftNavMenu" TagPrefix="uc" %>
<script type="C#" runat="server">
    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);        
    }        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

</asp:Content>

<asp:Content ID="leftNavigation" ContentPlaceHolderID="Navigation" runat="server">
  <form runat="server" id="navForm">
      <uc:LeftNavMenu id="leftNavMenu" runat="server" />
   </form>
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="Main" runat="server">
    <table width="90%" border="0" cellspacing="0" cellpadding="8">
        <tr> 
            <td>
		    <!-- #BeginEditable "MainBody" --> 
            <p>&nbsp;</p>
            <p>&nbsp;</p>
            <p>Welcome - Administration Main Page!</p>
            <p>&nbsp;</p>
            <p>&nbsp;</p>
            <p>&nbsp;</p>
            <p>&nbsp;</p>
            <p>&nbsp;</p>
            <!-- #EndEditable -->
            </td>
        </tr>
       
    </table>
    <p>&nbsp;</p>
</asp:Content>