<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EmployeeTransfer.aspx.cs" Inherits="InterrailPPRS.Admin.EmployeeTransfer" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script type="text/javascript">

    function goTransfer() {
        var frm = document.form1;
        var emp = frm.selEmployees.options[frm.selEmployees.selectedIndex].text;
        var fac = frm.selFacilities.options[frm.selFacilities.selectedIndex].text;
        var msg = "Transfer " + emp + " to " + fac + "?";
        if (confirm(msg)) {
            frm.action = "EmployeeTransfer.aspx?MODE=TRANSFER&EMPNAME=" + emp + "&FACNAME=" + fac;
            frm.submit();
        }
    }

    function goCancel() {
        try {
            self.location = "Default.aspx";
        }
        catch (e)
    { };
    }

    function bodyload() {

        document.all["TransferMessage"].innerHTML = "<%=sMessage%>";

    }


    // -->
</script>
</asp:Content>

<asp:Content ID="leftNavigation" ContentPlaceHolderID="Navigation" runat="server">
  <table width="81%" border="0"  valign="top">
        <% if (CheckSecurity("Super"))
           { %>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallBleuArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"><a href="Company.aspx">Companies</a></td>
        </tr>
        <%}%>
        <% if (CheckSecurity("Super, Admin"))
           { %>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallBleuArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"><a href="Facility.aspx">Facilities</a></td>
        </tr>
        <%}%>
        <% if (CheckSecurity("Super, Admin"))
           { %>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallBleuArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"><a href="Tasks.aspx">Tasks</a></td>
        </tr>
        <%}%>
        <% if (CheckSecurity("Super, Admin"))
           { %>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallBleuArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"><a href="User.aspx">Users</a></td>
        </tr>
        <%}%>
        <% if (CheckSecurity("Super, Admin"))
           { %>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallBleuArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"><a href="EmployeeTransfer.aspx">Transfer Employees</a></td>
        </tr>
        <%}%>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallBleuArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"><a href="MAS90Select.aspx">MAS 90</a></td>
        </tr>
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
        <% if (CheckSecurity("Super, Admin, User"))
           { %>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"><a href="RebillSubTask.aspx">Re-bill Tasks</a></td>
        </tr>
        <%}%>
        <% if (CheckSecurity("Super, Admin, User"))
           { %>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"><a href="EmploymentSource.aspx">Employment Sources</a></td>
        </tr>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"><a href="Employee.aspx">Employees</a></td>
        </tr>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"><a href="Teams.aspx">Teams</a></td>
        </tr>
        <tr>
        <td width="8%" height="18">&nbsp; </td>
        <td width="13%" height="18"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%" height="18"><a href="Customer.aspx">Customers</a></td>
        </tr>
        <%}%>
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
            <td>
			<!-- #BeginEditable "MainBody" -->
            <% if ((rsEmployees.EOF) || (rsFac.EOF)) { %>
                No facilities and/or employees available.
            <% }else{ %>
            <form name="form1" method="post">
                <table align="center" width="420">
                <tr valign="baseline">
                    <td nowrap align="right" colspan="3">
                    <div align="center" class="pageTitle">Transfer Employee </div>
                    </td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" height="13" width="149">&nbsp;</td>
                    <td height="13" width="67">&nbsp;</td>
                    <td height="13" width="147">&nbsp;</td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="center" height="13" width="149">Employee to Transfer</td>
                    <td height="13" width="67">&nbsp;</td>
                    <td height="13" width="147">
                    <div align="center">New Facility</div>
                    </td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" width="149">
                    <select name="selEmployees" style="width:150px">
                        <%
                        while (! rsEmployees.EOF){
                            rsEmployees.Read();
                        %>
                        <option value="<%=(rsEmployees.Item("ID"))%>"><%=(rsEmployees.Item("LastName")) + ", " + (rsEmployees.Item("FirstName"))%></option>
                        <%

                        }
                        rsEmployees.Requery();

                    %>
                    </select>
                    </td>
                    <td width="67">
                    <input type="button" value="Transfer To" onclick="goTransfer();" name="button">
                    </td>
                    <td width="147">
                    <select name="selFacilities"  style="width:150px">
                        <%
                        while (! rsFac.EOF){
                            rsFac.Read();
                        %>
                        <option value="<%=(rsFac.Item("ID"))%>"><%=(rsFac.Item("Name"))%></option>
                        <%
                        }
                                        
                        rsFac.Requery();
                                        
                        %>
                    </select>
                    </td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" width="149">&nbsp;</td>
                    <td width="67">&nbsp;</td>
                    <td width="147">&nbsp;</td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" colspan="3"><div id="TransferMessage">&nbsp;</div></td>
                </tr>
                </table>
                <input type="hidden" name="LastModifiedOn" value="<%= System.DateTime.Now %>" />
                <input type="hidden" name="LastModifiedBy" value="<%=Session["UserName"]%>" />
            </form>
            <% } %>
            <!-- #EndEditable -->
            </td>
        </tr>
       
	    </table>
</asp:Content>