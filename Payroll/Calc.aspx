<%@ Page Language="C#" MasterPageFile="~/Payroll/Payroll.Master" AutoEventWireup="true" CodeBehind="Calc.aspx.cs" Inherits="InterrailPPRS.Payroll.Calc" %>



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
                  <% =ChangeFacilityLink() %> 
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
                  <%}%>
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
    <table width="90%" border="0" cellspacing="0" cellpadding="8">
                        <tr> 
                          <td height="40"> <!-- #BeginEditable "MainBody" -->
                            <form name="form1" method="post" action="">
<%=StatusMessage%>                            
                              <p>Do Payroll Calc</p>
                              <p>
<input type="hidden" name="DoCalc" value="Yes" />
                              </p>
                              <p>Pick Pay Period
                                                          <select name="Payperiod">
                                                          <% =getPayPeriods(0,52,"") %>
                                                          </select>
                              </p>
                              <p>
                                <input type="submit" name="Submit" value="Submit" />
                              </p>
                            </form>
                            <!-- #EndEditable --> </td>
                        </tr>
       
                          </table>
</asp:Content>