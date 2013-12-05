<%@ Page Language="C#" MasterPageFile="~/Payroll/Payroll.Master" AutoEventWireup="true" CodeBehind="CSVGenerate.aspx.cs" Inherits="InterrailPPRS.Payroll.CSVGenerate" %>



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
                  <%= ChangeFacilityLink(); %> 
                  <tr> 
                    <td width="8%">&nbsp;</td>
                    <td width="13%">&nbsp;</td>
                    <td width="79%">&nbsp;</td>
                  </tr>
                  <% if (CheckSecurity()) { %>
                  <tr> 
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
                    <td width="79%"><a href="Payroll.aspx">Daily 
                      Payroll</a></td>
                  </tr>
                  <%}%>
                  <% if (CheckSecurity()) { %>
                  <tr> 
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
                    <td width="79%"><a href="calc.aspx">Calculate</a></td>
                  </tr>
                  <%}%>

                  <% if (CheckSecurity()) { %>
                  <tr> 
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
                    <td width="79%"> <a href="ApprovePayrollData.aspx">Approval</a></td>
                  </tr>
                  <% } %>

                 <% if (CheckSuperAdminSecurity()) { %>
                  <tr> 
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
                    <td width="79%"> <a href="CSVSelect.aspx">Create ADP File</a></td>
                  </tr>
                  <% }%>

                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%">&nbsp;</td>
                    <td width="79%">&nbsp;</td>
                  </tr>
                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
                    <td width="79%"><a href="../Logout.aspx">Logout</a></td>
                  </tr>
                                  
                </table>
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="Main" runat="server">
    <td>
    <table width="90%" border="0" cellspacing="0" cellpadding="8">
                        <tr>
                          <td height="40"> <!-- #BeginEditable "MainBody" -->
                            <form name='frmPrint' method='post' action=''>
                              <table width="91%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                  <td colspan="4" class="pageTitle" align='center'>
                                    <div class="cellTopBottomBorder">CSV
                                      File Created </div>
                                  </td>
                                </tr>
                                <tr>
                                  <td width="52%" align='right'  >&nbsp; </td>
                                  <td width="17%" align='center' >&nbsp;</td>
                                  <td width="20%" align='center' >&nbsp;</td>
                                  <td width="11%" align='center'>&nbsp;</td>
                                </tr>
                                <tr>
                                <td colspan="4" align='center'>
                                  <%=sFileInfo%>
                                </td>
                                </tr>
                              </table>
                            </form>
                            <!-- #EndEditable --> </td>
                        </tr>

                          </table>
</td>
</asp:Content>
