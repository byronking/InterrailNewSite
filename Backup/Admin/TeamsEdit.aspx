<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="TeamsEdit.aspx.cs" Inherits="InterrailPPRS.Admin.TeamsEdit" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script type="text/javascript">
<!--
   
    function bodyload() {
        if ('<%=Request["ID"]%>' == "0")
        { document.form1.Active.checked = true; }

        OriginalFormCheckSumValue = CheckStringForForm(document.form1);
    }

    function bodyunload() {
        if (CheckStringForForm(document.form1) != OriginalFormCheckSumValue) {
            event.returnValue = "You have not saved your changes.";
        }
    }

    function goValidate() {
        frm = document.form1;

        if (ValidText(frm.TeamName, 1, "Team Name") != true) { return false; }

        var from = frm.select2;
        var iend = ((from.options.length) - 1);

        for (i = iend; i > -1; i--) {
            from.options[i].selected = true;
            if (i == iend) {
                frm.TeamMembers.value = from.options[i].value
            }
            else {
                frm.TeamMembers.value = frm.TeamMembers.value + ", " + from.options[i].value
            }
        }

        OriginalFormCheckSumValue = CheckStringForForm(document.form1);

        frm.action = "<%=MM_editAction%>";
        frm.submit();
    }

    function goCancel() {
        try {
            self.location = "<%=MM_editRedirectUrl%>";
        }
        catch (e) { }
    }

    function goAddRemove(direction) {
        var frm = document.form1;
        if (direction == "ADD") {
            var from = frm.select;
            var to = frm.select2;
        }
        else {
            var from = frm.select2;
            var to = frm.select;
        }

        var iend = ((from.options.length) - 1);

        for (i = iend; i > -1; i--) {
            if (from.options[i].selected) {
                var id = from.options[i].value;
                var val = from.options[i].text;
                var newOption = new Option(val, id, false, false);
                to.options[to.options.length] = newOption;
                to.options[(to.options.length) - 1].selected = true;
                from.options[i] = null;
            }
        }
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
            <% if ((rsEmp.EOF) && (rsMembers.EOF)) { %>
            No employee/sources available for the facility.<br>
            Please assign employees to employment sources first.
            <% }else{ %>
            <form method="POST" action="<%=MM_editAction%>" name="form1">
                <table align="center" width="100%">
                <tr valign="baseline">
                    <td nowrap align="right" colspan="4">
                    <div align="center" class="pageTitle">Team
                        Setup</div>
                    </td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" width="71">&nbsp;</td>
                    <td width="165">&nbsp;</td>
                    <td width="34">&nbsp;</td>
                    <td width="186">&nbsp;</td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" width="71">&nbsp;</td>
                    <td width="165">&nbsp;</td>
                    <td width="34">&nbsp;</td>
                    <td width="186">&nbsp;</td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" width="71"  class="required">Facility:</td>
                    <td width="165"><%=Session["FacilityName"] %></td>
                    <td width="34">&nbsp;</td>
                    <td width="186"></td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" width="71"   class="required">Team
                    Name:</td>
                    <td width="165">
                    <input type="text" name="TeamName" value="<%=Trim(rs.Item("TeamName")) %>" size="30" maxlength="30">
                    </td>
                    <td align="right" width="165">Active: </td>
                    <td width="186">
                    <input <%if (rs.Item("Active") != "0") { Response.Write("CHECKED"); }else{ Response.Write(""); }%> type="checkbox" name="Active" value=1 >
                    </td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" width="71">&nbsp;</td>
                    <td width="165">&nbsp;</td>
                    <td width="34">&nbsp;</td>
                    <td width="186">&nbsp;</td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" width="71">&nbsp;</td>
                    <td width="165" class="cellTopBottomBorder">Available
                    Employees</td>
                    <td width="34">&nbsp;</td>
                    <td width="186" class="cellTopBottomBorder">Team
                    Members</td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" width="71">&nbsp;</td>
                    <td rowspan="5" width="165">
                    <select name="select" multiple size="6" style="width:170px" title="Available Employees">
                        <%
                        while (! rsEmp.EOF){
                            rsEmp.Read();
                        %>
                        <option value="<%=(rsEmp.Item("Id"))%>"><%=Trim(rsEmp.Item("LastName")) %>, <%=Trim(rsEmp.Item("FirstName")) %> (<%=Trim(rsEmp.Item("EmployeeNumber")) %>)</option>
                        <%
                        }
                        
                        rsEmp.Requery();

                        %>
                    </select>
                    </td>
                    <td width="34">&nbsp;</td>
                    <td width="186" rowspan="5">
                    <select name="select2" multiple size="6" style="width:170px" title="Team Members">
                        <%
                        while (! rsMembers.EOF){
                            rsMembers.Read();
                        %>
                        <option value="<%=(rsMembers.Item("Id")) %>"><%=(rsMembers.Item("LastName")) %>, <%=Trim(rsMembers.Item("FirstName")) %> (<%=Trim(rsMembers.Item("EmployeeNumber")) %>)</option>
                        <%
                            }
                    rsMembers.Requery();

                    %>
                    </select>
                    </td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" width="71">&nbsp;</td>
                    <td width="34" align="center">
                    <input type="button" name="btnAdd" value=">>" onclick="goAddRemove('ADD');">
                    </td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" width="71">&nbsp;</td>
                    <td width="34" align="center">
                    <input type="button" name="btnRemove" value="<<" onclick="goAddRemove('REMOVE');">
                    </td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" width="71">&nbsp;</td>
                    <td width="34">&nbsp; </td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" width="71">&nbsp;</td>
                    <td width="34">&nbsp;</td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" width="71">Last Modified:</td>
                    <td width="165"> <%=(rs.Item("LastModifiedOn"))%> </td>
                    <td nowrap align="right" width="34">&nbsp;</td>
                    <td width="186">&nbsp; </td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" width="71">By:</td>
                    <td width="165"><%=(rs.Item("LastModifiedBy"))%></td>
                    <td width="34">&nbsp; </td>
                    <td width="186">&nbsp; </td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" width="71">&nbsp;</td>
                    <td colspan="2" align="right">
                    <input type="button" value="Save" onclick="goValidate();" name="button">
                    <input type="button" name="btnCancel" value="Cancel" onclick="goCancel();">
                    </td>
                    <td width="186">&nbsp; </td>
                </tr>
                </table>
                <input type="hidden" name="MM_update" value="true">
                <input type="hidden" name="MM_recordId" value="<%= rs.Item("ID") %>">
                <input type="hidden" name="TheFacID" value="<%= Session["FacilityID"] %>">
                <input type="hidden" name="LastModifiedOn" value="<%=System.DateTime.Now.ToString()%>" size="32">
                <input type="hidden" name="LastModifiedBy" value="<%=Session["UserName"]%>" size="32">
                <input type="hidden" name="TeamMembers" value="" size="32">
            </form>
            Fields in <span class="Required">RED</span> are required.
            <% } %>
            <p>&nbsp;</p>
            <!-- #EndEditable -->
            </td>
        </tr>
       
	    </table>
        <p>&nbsp;</p>
</asp:Content>