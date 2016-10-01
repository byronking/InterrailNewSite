<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EmploymentSourceEdit.aspx.cs" Inherits="InterrailPPRS.Admin.EmploymentSourceEdit" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script type="text/javascript">
<!--
    
    function bodyload() {
        if ('<%=System.Convert.ToString(Request["ID"])%>' == "0")
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
        if (ValidText(frm.SourceCode, 1, "Code") != true) { return false; }
        if (ValidText(frm.SourceName, 1, "Name") != true) { return false; }

        OriginalFormCheckSumValue = CheckStringForForm(document.form1);
        frm.action = "<%=MM_editAction%>";
        frm.submit();
    }

    function goCancel() {
        try {
            self.location = "EmploymentSource.aspx";
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
    <table width="90%" border="0" cellspacing="0" cellpadding="8">
        <tr> 
            <td>
			<!-- #BeginEditable "MainBody" --> 
            <form method="POST" action="<%=MM_editAction%>" name="form1">
                <table align="center">
                <tr valign="baseline"> 
                    <td nowrap align="right" colspan="2"> 
                    <div align="center"  class="pageTitle">Employment Source </div>
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="91" height="19">&nbsp;</td>
                    <td width="196" height="19">&nbsp;</td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="91" class="Required">Code:</td>
                    <td width="196"> 
                    <input type="text" name="SourceCode" value="<%=Trim(rs.Item("SourceCode"))%>" size="32">
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="91" class="Required">Name:</td>
                    <td width="196"> 
                    <input type="text" name="SourceName" value="<%=Trim(rs.Item("SourceName"))%>" size="32">
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="91" class="Required">Facility</td>
                    <td width="196"><%=Session["FacilityName"]%></td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="91">Active:</td>
                    <td width="196"> 
                    <input <%if (rs.Item("Active") != "0") { Response.Write("CHECKED");} else { Response.Write("");}%> type="checkbox" name="Active" value="1" >
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="91">&nbsp;</td>
                    <td width="196">&nbsp;</td>
                </tr>
                <tr valign="baseline" bgcolor="#FFFFFF"> 
                    <td nowrap align="right" width="91">Last Modified:</td>
                    <td width="196"><%= (rs.Item("LastModifiedOn")) %></td>
                </tr>
                <tr valign="baseline" bgcolor="#FFFFFF"> 
                    <td nowrap align="right" width="91">By:</td>
                    <td width="196"><%= (rs.Item("LastModifiedBy")) %></td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="91">&nbsp;</td>
                    <td width="196"> 
                    <input type="button" value="Save" onclick="goValidate();" />
                    <input type="button" name="btnCancel" value="Cancel" onclick="goCancel();" />
                    </td>
                </tr>
                </table>
                <input type="hidden" name="MM_update" value="true" />
                <input type="hidden" name="MM_recordId" value="<%= rs.Item("Id") %>" />
                <input type="hidden" name="LastModifiedOn" value="<%=System.DateTime.Now%>" size="30" />
                <input type="hidden" name="LastModifiedBy" value="<%=Session["UserName"]%>" size="30" />
                <input type="hidden" name="TheFacID" value="<%=Session["FacilityID"]%>" size="30" />
            </form>
            <p>&nbsp;</p>
            Fields in <span class="Required">RED</span> are required. 
            <!-- #EndEditable -->
            </td>
        </tr>
       
	    </table>
        <p>&nbsp;</p>
</asp:Content>