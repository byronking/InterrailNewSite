<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CustomerEdit.aspx.cs" Inherits="InterrailPPRS.Admin.CustomerEdit" %>



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

    function setCustomer() {
        frm = document.form1;
        if (frm.selCarrier.selectedIndex != 0) {
            frm.CustomerCode.value = frm.selCarrier.options[frm.selCarrier.selectedIndex].value;
            frm.CustomerName.value = frm.selCarrier.options[frm.selCarrier.selectedIndex].text;
        }
    }

    function goValidate() {
        frm = document.form1;
        if (ValidText(frm.CustomerCode, 1, "Code") != true) { return false; }
        if (ValidText(frm.CustomerName, 1, "Name") != true) { return false; }

        OriginalFormCheckSumValue = CheckStringForForm(document.form1);

        frm.action = "<%=MM_editAction%>";
        frm.submit();
    }

    function goCancel() {
        try {
            self.location = "Customer.aspx";
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
            <table align="center" width="249">
            <tr valign="baseline"> 
                <td nowrap align="right" colspan="2"> 
                <div align="center"  class="pageTitle">Customer</div>
                </td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right" class="Required">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right">Select Carrier: </td>
                <td> 
                <select name="selCarrier" onChange='setCustomer()';>
                    <% while (!rsRC.EOF){ 
                        rsRC.Read();
                    %>
                    <option value="<% = rsRC.Item("RailCarrierCode") %>"><% =rsRC.Item("RailCarrierName")%></option>
                    <%
                    }
                                      
                    rsRC.Requery();

                %>

                </select>
                </td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right" class="required">Code:</td>
                <td> 
                <input type="text" name="CustomerCode" value="<%=Trim(rs.Item("CustomerCode"))%>" size="30" maxlength="10">
                </td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right" class="required">Name:</td>
                <td> 
                <input type="text" name="CustomerName" value="<%=Trim(rs.Item("CustomerName"))%>" size="30" maxlength="30">
                </td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right">Default Customer</td>
                <td> 
                <input <%if (rs.Item("DefaultCustomer") != "") { Response.Write("CHECKED"); }else{ Response.Write(""); }%> type="checkbox" name="defaultcustomer" value="checkbox" />
                </td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right">Active:</td>
                <td> 
                <input <%if (rs.Item("Active") != "0" ) { Response.Write("CHECKED"); }else{ Response.Write(""); } %> type="checkbox" name="Active" value="1" />
                </td>
            </tr>
            <tr valign="baseline"> 
                <td>&nbsp;</td>
                <td nowrap align="left"   class="cellTopBottomBorder">Contact Information</td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right">Contact Name</td>
                <td> 
                <input value="<%=(rs.Item("ContactName"))%>" type="text" name="contactname" size="30" maxlength="250">
                </td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right">Contact Address</td>
                <td> 
                <input value="<%=(rs.Item("ContactAddress1"))%>" type="text" name="address1" size="30" maxlength="250">
                </td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right">&nbsp;</td>
                <td> 
                <input value="<%=(rs.Item("ContactAddress2"))%>" type="text" name="address2" size="30" maxlength="250">
                </td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right">&nbsp;</td>
                <td> 
                <input value="<%=(rs.Item("ContactAddress3"))%>" type="text" name="address3" size="30" maxlength="250">
                </td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right" width="149">Last Modified:</td>
                <td width="138"><%= (rs.Item("LastModifiedOn")) %></td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right" width="149">By:</td>
                <td width="138"><%= (rs.Item("LastModifiedBy")) %> </td>
            </tr>
            <tr> 
                <td nowrap align="right">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <td nowrap align="right">&nbsp;</td>
            <td> 
                <input type="button" value="Save" onclick="goValidate();" />
                <input type="button" name="btnCancel" value="Cancel" onclick="goCancel();" />
            </td>
            </tr>
    </table>
    <input type="hidden" name="MM_update" value="true" />
    <input type="hidden" name="MM_recordId" value="<%= rs.Item("Id") %>" />
    <input type="hidden" name="LastModifiedOn" value="<%=System.DateTime.Now%>" size="32" />
    <input type="hidden" name="LastModifiedBy" value="<%=Session["UserName"]%>" size="32" />
    <input type="hidden" name="ThisFacID" value="<%=Session["FacilityID"]%>" size="32" />
</form>
<p>&nbsp;</p>
Fields in <span class="Required">RED</span> are required

</td>
</tr>
</table>

</asp:Content>