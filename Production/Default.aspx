<%@ Page Language="C#" MasterPageFile="~/Production/Production.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="InterrailPPRS.Production.Default" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script language="JavaScript">

    function goNewDate() {
        var arDates = document.form1.selDateRange.value.split(",");
        document.form1.startDate.value = arDates[0];

        document.form1.action = "Default.aspx";
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
                  <%if( CheckSecurity("Super, Admin, User, Production")) { %>
                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
                    <td width="79%"><a href="Detail.aspx">Detail Maintenance</a></td>
                  </tr>
                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
                    <td width="79%"><a href="ApproveProductionData.aspx">Approve Production Data</a></td>
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
        <td><!-- #BeginEditable "MainBody" --> 
        <% ShowApprovalStatus(); %>
        <!-- #EndEditable -->
        </td>
    </tr>

</table>
</asp:Content>