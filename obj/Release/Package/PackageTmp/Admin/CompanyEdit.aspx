<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CompanyEdit.aspx.cs" Inherits="InterrailPPRS.Admin.CompanyEdit" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

<script type="text/javascript">
<!--

    function bodyload() {
        if ('<%=Request["ID"].ToString()%>' == "0")
        { document.form1.checkbox.checked = true; }

        OriginalFormCheckSumValue = CheckStringForForm(document.form1);
    }

    function bodyunload() {
        if (CheckStringForForm(document.form1) != OriginalFormCheckSumValue) {
            event.returnValue = "You have not saved your changes.";
        }
    }

    function goValidate() {
        frm = document.form1;
        if (ValidText(frm.CompanyID, 1, "ID") != true) { return false; }
        if (ValidText(frm.CompanyName, 1, "Name") != true) { return false; }
        if (ValidText(frm.PayrollCompanyCode, 1, "Payroll Code") != true) { return false; }
        if (ValidPositiveNumber(frm.OutOfTownRate, 0, "Out Of Town Rate") != true) { return false; }
        if (frm.OutOfTownRate.value > 100.0) {
            alert("Out Of Town Rate must not exceed 100");
            frm.OutOfTownRate.focus();
            frm.OutOfTownRate.select();
            return false;
        }
        if (ValidPositiveNumber(frm.OutOfTownHoursPerDay, 0, "Hours Per Day") != true) { return false; }
        if (frm.OutOfTownHoursPerDay.value > 24.0) {
            alert("Hours Per Day must not exceed 24");
            frm.OutOfTownHoursPerDay.focus();
            frm.OutOfTownHoursPerDay.select();
            return false;
        }

        OriginalFormCheckSumValue = CheckStringForForm(document.form1);

        frm.action = "<%=MM_editAction%>";
        frm.submit();
    }
    function goCancel() {
        try {
            self.location = "Company.aspx";
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
                <table align="center" bgcolor="#FFFFFF">
                <tr valign="baseline"> 
                    <td nowrap align="right" colspan="2"> 
                    <div align="center" class="pageTitle">Company</div>
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="149">&nbsp;</td>
                    <td width="138">&nbsp;</td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="149" class="required">ID:</td>
                    <td width="138"> 
                    <input type="text" name="CompanyID" value="<%=Trim(rs.Item("CompanyID")) %>" size="32" maxlength="10">
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="149" class="required">Name:</td>
                    <td width="138"> 
                    <input type="text" name="CompanyName" value="<%=Trim(rs.Item("CompanyName")) %>" size="32" maxlength="30">
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="149" class="required">Payroll 
                    Company Code:</td>
                    <td width="138"> 
                    <input type="text" name="PayrollCompanyCode" value="<%=Trim(rs.Item("PayrollCompanyCode")) %>" size="32" maxlength="10" />
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="149">Path for 
                    Company Logo</td>
                    <td width="138"> 
                    <input value="<%= rs.Item("LogoPath") %>" type="text" name="LogoPath"  size="32" maxlength="250" />
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="149">Pay Period:</td>
                    <td width="138"> 
                    <select name="select">
                        <%
                            while (!rsPayPeriod.EOF)
                            {
                            rsPayPeriod.Read();
                        %>
                        <option value="<%= rsPayPeriod.Item("Id").ToString() %>" 
                            <% if ( rsPayPeriod.Item("Id").ToString() == rs.Item("PayPeriodID").ToString() ){ 
                                Response.Write("SELECTED");  
                                }else{
                                Response.Write("");
                                } 
                            %> >
                            <% = rsPayPeriod.Item("Description").ToString() %>
                        </option>
                        <%

                        }

                            rsPayPeriod.Requery();
                                        
                        %>
                    </select>
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="149">Out Of 
                    Town Rate:</td>
                    <td width="138"> 
                    <input type="text" name="OutOfTownRate" value="<%=Trim(rs.Item("OutOfTownRate").ToString()) %>" size="32" maxlength="5" />
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="149">Out Of 
                    Town Hours Per Day:</td>
                    <td width="138"> 
                    <input type="text" name="OutOfTownHoursPerDay" value="<%=Trim(rs.Item("OutOfTownHoursPerDay").ToString()) %>" size="32" maxlength="5" />
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="149">Active:</td>
                    <td width="138"> 
                    <input <%if ( rs.Item("Active").ToString().Length > 0) { 
                    Response.Write("CHECKED");
                    } else {
                        Response.Write("");
                    }%> type="checkbox" name="checkbox" value="checkbox" />
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="149">&nbsp;</td>
                    <td width="138">&nbsp;</td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="149">Last Modified:</td>
                    <td width="138"><%= rs.Item("LastModifiedOn").ToString() %></td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="149">By:</td>
                    <td width="138"><%= rs.Item("LastModifiedBy").ToString() %> </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="149">&nbsp;</td>
                    <td width="138"> 
                    <input type="button" value="Save" onclick="goValidate();" />
                    <input type="button" name="btnCancel" value="Cancel" onclick="goCancel();" />
                    </td>
                </tr>
                </table>
                <input type="hidden" name="MM_update" value="true" />
                <input type="hidden" name="MM_recordId" value="<%= rs.Item("Id").ToString() %>" />
                <input type="hidden" name="LastModifiedOn" value="<%= System.DateTime.Now %>" />
                <input type="hidden" name="LastModifiedBy" value="<%= Session["UserName"] %>" />
            </form>
            <p>&nbsp;</p>
            Fields in <span class="Required">RED</span> are required. 
            <!-- #EndEditable -->

        </td>
     </tr>
    </table>
</asp:Content>