<%@ Page Language="C#" MasterPageFile="~/Payroll/Payroll.Master" AutoEventWireup="true" CodeBehind="CalcWait.aspx.cs" Inherits="InterrailPPRS.Payroll.CalcWait" %>



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
               
                  <tr> 
                    <td width="8%">&nbsp;</td>
                    <td width="13%">&nbsp;</td>
                    <td width="79%">&nbsp;</td>
                  </tr>
                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%">&nbsp;</td>
                    <td width="79%">&nbsp;</td>
                  </tr>
                                  
                </table>
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="Main" runat="server">
 <table width="90%" border="0" cellspacing="0" cellpadding="8">
    <tr> 
        <td height="40"> <!-- #BeginEditable "MainBody" -->
        <p align="center">
        Payroll is being calculated... Please wait. 
        <img src="../images/aniyin.gif">
        </p>
        <!-- #EndEditable --> </td>
    </tr>
       
</table>
</asp:Content>