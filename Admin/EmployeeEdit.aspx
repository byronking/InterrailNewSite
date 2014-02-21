<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EmployeeEdit.aspx.cs" Inherits="InterrailPPRS.Admin.EmployeeEdit" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>

<asp:Content ID="leftNavigation" ContentPlaceHolderID="Navigation" runat="server">
    <table width="81%" border="0"  valign="top">
        <%if ( Session["UserType"].ToString().IndexOf("Super") >=1 ) { %>
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
    <form runat="server">
        <div align="center" class="pageTitle"><asp:Label ID="lblTitleLabel" runat="server" Text="Edit Existing Employee" /></div><p />
        <table border="0">
            <tr><td colspan="6">&nbsp;</td></tr>
            <tr valign="baseline">                
                <td align="right">
                    <asp:Label ID="lblFirstName" runat="server" CssClass="required" Text="First Name:" />
                </td>
                <td align="right">
                    <asp:TextBox ID="txtFirstName" runat="server" />
                </td>
                <td align="right" style="width:10%">
                    <asp:Label ID="lblMiddleInitial" runat="server" Text="MI:"  />
                </td>
                <td>
                    <asp:TextBox ID="txtMiddleInitial" runat="server" Width="15px" />
                </td>
                <td align="right">
                    <asp:Label ID="lblLastName" runat="server" CssClass="required" Text="Last Name:" />
                </td>
                <td align="right">
                    <asp:TextBox ID="txtLastName" runat="server" />
                </td>
            </tr>          
            <tr valign="baseline">
                <td align="right">
                    <asp:Label ID="lblSSN" runat="server" CssClass="required" Text="SSN:" />
                </td>
                <td align="right">
                    <asp:TextBox ID="txtSSN" runat="server" />
                </td>
                <td colspan="2">&nbsp</td>
                <td align="right">
                    <asp:Label ID="lblDOB" runat="server" CssClass="required" Text="DOB:" />
                </td>
                <td>
                    <asp:TextBox ID="txtDOB" runat="server" />
                </td>
            </tr>
            <tr valign="baseline">
                <td align="right">
                    <asp:Label ID="lblEmployeePhone" runat="server" CssClass="required" Text="Phone:" />
                </td>
                <td>
                    <asp:TextBox ID="txtEmployeePhone" runat="server" />
                </td>
                <td colspan="4">&nbsp</td>
            </tr>
            <tr valign="baseline">
                <td align="right">
                    <asp:Label ID="lblAddress1" runat="server" CssClass="required" Text="Adress 1:" />
                </td>
                <td>
                    <asp:TextBox ID="txtAddress1" runat="server" />
                </td>
                <td colspan="2">&nbsp</td>
                <td align="right">
                    <asp:Label ID="lblAddress2" runat="server" Text="Address 2:" />
                </td>
                <td>
                    <asp:TextBox ID="txtAddress2" runat="server" />
                </td>                
            </tr> 
            <tr valign="baseline">
                <td align="right">
                    <asp:Label ID="lblCity" runat="server" CssClass="required" Text="City:" />
                </td>
                <td>
                    <asp:TextBox ID="txtCity" runat="server" />
                </td>
                <td align="right">
                    <asp:Label ID="lblState" runat="server" CssClass="required" Text="State:" />
                </td>
                <td>
                    <asp:TextBox ID="txtState" runat="server" Width="30px" />
                </td>
                <td align="right">
                    <asp:Label ID="lblZipCode" runat="server" CssClass="required" Text="Zip Code:" />
                </td>
                <td>
                    <asp:TextBox ID="txtZipCode" runat="server" />
                </td>
            </tr>
            <tr valign="baseline">
                <td align="right">
                    <asp:Label ID="lblEmergencyContact" runat="server" Text="Emergency Contact:" />
                </td>
                <td>
                    <asp:TextBox ID="txtEmergencyContact" runat="server" />
                </td>
                <td colspan="2">&nbsp</td>
                <td align="right">
                    <asp:Label ID="lblEmergencyContactPhone" runat="server" Text="Contact Phone:" />
                </td>
                <td>
                    <asp:TextBox ID="txtEmergencyContactPhone" runat="server" />
                </td>
            </tr>            
            <tr valign="baseline">                
                <td align="right">
                    <asp:Label ID="lblPermanent" runat="server" Text="Permanent:" />
                </td>
                <td>
                    <asp:RadioButton ID="radioPermanent" runat="server" GroupName="TempOrPerm" />                    
                </td>
                <td colspan="2">&nbsp</td>
                <td align="right">
                    <asp:Label ID="lblTemporary" runat="server" Text="Temporary:" />
                </td>
                <td>
                    <asp:RadioButton ID="radioTemporary" runat="server" GroupName="TempOrPerm" />
                </td>
            </tr>
            <tr valign="baseline">                
                <td align="right">
                    <asp:Label ID="lblEmployeeNumber" CssClass="required" runat="server" Text="Employee Number:" />
                </td>
                <td>
                    <asp:TextBox ID="txtEmployeeNumber" runat="server" />
                </td>
                <td colspan="2">&nbsp</td>
                <td align="right">
                    <asp:Label ID="lblTemporaryNumber" runat="server" Text="Temporary Number:" />
                </td>
                <td>
                    <asp:TextBox ID="txtTemporaryNumber" runat="server" />
                </td>
            </tr>
            <tr valign="baseline">
                <td align="right">
                    <asp:Label ID="lblHireDate" CssClass="required" runat="server" Text="Hire Date:" />
                </td>
                <td>
                    <asp:TextBox ID="txtHireDate" runat="server" />
                </td>
                <td colspan="2">&nbsp</td>
                <td align="right">
                    <asp:Label ID="lblStartDate" runat="server" Text="Start Date:" />
                </td>
                <td>
                    <asp:TextBox ID="txtStartDate" runat="server" />
                </td>
            </tr>
            <tr valign="baseline">
                <td align="right">
                    <asp:Label ID="lblInactiveDate" runat="server" Text="Inactive Date:" />
                </td>
                <td>
                    <asp:TextBox ID="txtInactiveDate" runat="server" />
                </td>
                <td colspan="2">&nbsp</td>
                <td align="right">
                    <asp:Label ID="lblSource" runat="server" Text="Hiring Source:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlEmploymentSource" runat="server" Width="125px" />
                </td>
            </tr>
            <tr valign="baseline">
                <td align="right">
                    <asp:Label ID="lblTerminationDate" runat="server" Text="Termination Date:" />
                </td>
                <td>
                    <asp:TextBox ID="txtTerminationDate" runat="server" />
                </td>
            </tr>
            <tr valign="baseline">
                <td align="right">
                    <asp:Label ID="lblSalaried" runat="server" Text="Salaried:" />
                </td>
                <td>
                    <asp:CheckBox ID="chkBoxSalaried" runat="server" />
                </td>
            </tr>
            <tr valign="baseline">
                <td align="right">
                    <asp:Label ID="lblActive" runat="server" Text="Active:" />
                </td>
                <td>
                    <asp:CheckBox ID="chkBoxActive" runat="server" />
                </td>
            </tr>
            <tr><td colspan="6">&nbsp;</td></tr>
            <tr><td colspan="6">&nbsp;</td></tr>
            <tr valign="baseline">
                <td colspan="2">
                    <asp:Label ID="lblLastModified" runat="server" Text="Last Modified:" />
                    <asp:Label ID="lblLastModifiedDate" runat="server" />
                </td>                
            </tr>
            <tr valign="baseline">
                <td colspan="3">
                    <asp:Label ID="lblModifiedBy" runat="server" Text="Modified By:" />
                    <asp:Label ID="lblModifiedByUser" runat="server" />
                </td>
            </tr>
            <tr><td colspan="6">&nbsp;</td></tr>
            <tr><td colspan="6">&nbsp;</td></tr>
            <tr valign="baseline">                
                <td>&nbsp</td>
                <td align="right">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                </td>
                <td align="right">
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                </td>
            </tr>
            <tr><td colspan="6"><asp:Label ID="lblRecordSaved" runat="server" CssClass="required" Text="Employee Saved" Visible="false" /></td></tr>
            <tr><td colspan="6">&nbsp;</td></tr>
            <tr><td colspan="6">&nbsp;</td></tr>
            <tr><td colspan="6">Fields in <span class="required">RED</span> are required.</td></tr>
        </table>
        
        <%--Next employee number: --%>
    </form>    
</asp:Content>