<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="UserEdit.aspx.cs" Inherits="InterrailPPRS.Admin.UserEdit" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script type="text/javascript">
<!--

    function bodyload() {
        OriginalFormCheckSumValue = CheckStringForForm(document.form1);
    }

    function bodyunload() {
        if (CheckStringForForm(document.form1) != OriginalFormCheckSumValue) {
            event.returnValue = "You have not saved your changes.";
        }
    }

    function goValidate() {
        frm = document.form1;
        if (ValidText(frm.UserID, 1, "User ID") != true) { return false; }
        if (ValidText(frm.UserName, 1, "User Name") != true) { return false; }
        if (ValidText(frm.Password, 1, "Password") != true) { return false; }

        OriginalFormCheckSumValue = CheckStringForForm(document.form1);

        frm.action = "<%=MM_editAction%>";
        frm.submit();
    }
    function goCancel() {
        try {
            self.location = "User.aspx";
        }
        catch (e) { }
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
        <table width="95%" border="0" cellspacing="0" cellpadding="8">
        <tr> 
            <td>
			<!-- #BeginEditable "MainBody" --> 
            <form method="POST" action="<%=MM_editAction%>" name="form1">
                <table align="center">
                <tr valign="baseline"> 
                    <td nowrap colspan="2"> 
                    <div align="center" class="pageTitle">User</div>
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right"><span class="Required">User 
                    ID</span>:</td>
                    <td> 
                    <input type="text" name="UserID" value="<%=Trim(rs.Item("UserID")) %>" size="32">
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right"><span class="Required">User 
                    Name:</span></td>
                    <td> 
                    <input type="text" name="UserName" value="<%=Trim(rs.Item("UserName")) %>" size="32">
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" class="Required">Password:</td>
                    <td> 
                    <input type="text" name="Password" value="<%=Trim(rs.Item("Password")) %>" size="32">
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right">User Type:</td>
                    <td> 
                    <select name="select">
                        <%
                        while (! rsUserTypes.EOF){
                            rsUserTypes.Read();
                        %>
                        <option value="<%=(rsUserTypes.Item("Type")) %>" <%if (Trim(cStr(rsUserTypes.Item("Type"))) == Trim(cStr(rs.Item("UserType"))) ) { Response.Write("SELECTED"); }else{ Response.Write(""); }%> ><%= rsUserTypes.Item("Type") %></option>
                        <%
                            }
                        rsUserTypes.Requery();

                        %>
                    </select>
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right">User Long Name:</td>
                    <td> 
                    <input type="text" name="UserLongName" value="<%=Trim(rs.Item("UserLongName"))%>" size="32">
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="149">Last Modified:</td>
                    <td width="138"><%= rs.Item("LastModifiedOn") %></td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="149">By:</td>
                    <td width="138"><%= rs.Item("LastModifiedBy") %> </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right">&nbsp;</td>
                    <td> 
                    <input type="button" value="Save" onclick="goValidate();">
                    <input type="button" name="btnCancel" value="Cancel" onclick="goCancel();">
                    </td>
                </tr>
                </table>
                <input type="hidden" name="LastModifiedBy" value="<%= Session["UserName"] %>">
                <input type="hidden" name="MM_update" value="true">
                <input type="hidden" name="MM_recordId" value="<%=rs.Item("Id")%>">
                <input type="hidden" name="LastModifiedOn" value="<%=System.DateTime.Now.ToString() %>">
            </form>
            <p> 
                <% if (Request["id"] != "0") { %>
            </p>
            <form name="frmDelete" method="POST" action="<%=MM_editAction%>">
                <div align="center"> 
                <input type="submit" name="Submit" value="Delete This User">
                <input type="hidden" name="MM_delete" value="true">
                <input type="hidden" name="MM_recordId" value="<%= rsDelete.Item("Id") %>">
                </div>
            </form>
            <% } %>
            <p>&nbsp;</p>
            Fields in <span class="Required">RED</span> are required. 
            <!-- #EndEditable -->
            </td>
        </tr>
       
	    </table>
        <p>&nbsp;</p>
</asp:Content>