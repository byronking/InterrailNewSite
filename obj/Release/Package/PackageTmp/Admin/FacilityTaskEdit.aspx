<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="FacilityTaskEdit.aspx.cs" Inherits="InterrailPPRS.Admin.FacilityTaskEdit" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script type="text/javascript">
<!--
    function goValidate() {
        frm = document.form1;
        if (ValidText(frm.TaskCode, 1, "Task Code") != true) { return false; }
        if (ValidText(frm.TaskDescription, 1, "Description") != true) { return false; }

        frm.action = "<%=MM_editAction%>";
        frm.submit();
    }
    function goCancel() {
        if (confirm("Are you sure you want to cancel?"))
            self.location = "FacilityTask.aspx";
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
                            <form ACTION="<%=MM_editAction%>" METHOD="POST" name="form1">
                              <table align="center">
                                <tr valign="baseline"> 
                                  <td nowrap align="right" colspan="2"> 
                                    <div align="center"  class="pageTitle">Task</div>
                                  </td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right" class="required">&nbsp;</td>
                                  <td>&nbsp;</td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right" class="required">Facility:</td>
                                  <td> <%= cStr(Session["FacilityName"]) %></td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right" class="required">Code:</td>
                                  <td> 
                                    <input type="text" name="TaskCode" value="<%=Trim(rs.Item("TaskCode")) %>" size="32" maxlength="10">
                                  </td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right" class="required">Description:</td>
                                  <td> 
                                    <input type="text" name="TaskDescription" value="<%=Trim(rs.Item("TaskDescription")) %>" size="32" maxlength="30">
                                  </td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right">Rebillable:</td>
                                  <td> 
                                    <input <%if (System.Convert.ToBoolean(rs.Item("RebillableTask"))) { Response.Write("CHECKED";) }else{ Response.Write(""); }%> type="checkbox" name="RebillableTask" value=1 >
                                  </td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right" class="required">Pay 
                                    Type:</td>
                                  <td> 
                                    <select name="select2">
                                      <option value="EITHER" <%if (Trim(rs.Item("PayType")) == "EITHER" ) { Response.Write("SELECTED"); }else{ Response.Write(""); }%> >Either</option>
                                      <option value="UNITS"  <%if (Trim(rs.Item("PayType")) == "UNITS" )  { Response.Write("SELECTED"); }else{ Response.Write(""); }%> >Units</option>
                                      <option value="HOURS"  <%if (Trim(rs.Item("PayType")) == "HOURS" )  { Response.Write("SELECTED"); }else{ Response.Write(""); }%> >Hours</option>
                                    </select>
                                  </td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right">GL Account Number:</td>
                                  <td> 
                                    <input type="text" name="GLAcctNumber" value="<%=Trim(rs.Item("GLAcctNumber")) %>" size="32">
                                  </td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right">Active:</td>
                                  <td> 
                                    <input <%if (System.Convert.ToBoolean(rs.Item("Active"))) { Response.Write("CHECKED"); }else{ Response.Write(""); }%> type="checkbox" name="Active" value=1 >
                                  </td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right">&nbsp;</td>
                                  <td>&nbsp;</td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right">Last Modified:</td>
                                  <td> <%=rs.Item("LastModifiedOn") %> </td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right">By:</td>
                                  <td> <%=rs.Item("LastModifiedBy") %> </td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right">&nbsp;</td>
                                  <td> 
                                    <input type="button" value="Save" onclick="goValidate();" />
                                    <input type="button" name="btnCancel" value="Cancel" onclick="goCancel();" />
                                  </td>
                                </tr>
                              </table>
                              <input type="hidden" name="MM_recordId" value="<%= rs.Item("Id") %>" />
                              <input type="hidden" name="LastModifiedOn" value="<%=System.DateTime.Now.ToString() %>" size="32" />
                              <input type="hidden" name="LastModifiedBy" value="<%=Session["UserName"] %>" size="32" />
                              <input type="hidden" name="MM_update" value="true" />
                              <input type="hidden" name="FacID" value="<%=Session["FacilityID"] %>" />
                            </form>
                            <p>&nbsp;</p>
                            Fields in <span class="Required">RED</span> are required.
                            <!-- #EndEditable -->
                           </td>
                        </tr>

                    </table>
                      <p>&nbsp;</p>
</asp:Content>