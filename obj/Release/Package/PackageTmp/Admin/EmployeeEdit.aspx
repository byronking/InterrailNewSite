<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EmployeeEdit.aspx.cs" Inherits="InterrailPPRS.Admin.EmployeeEdit" %>
<%@ Register Src="~/UserControls/LeftNavMenu.ascx" TagName="LeftNavMenu" TagPrefix="uc" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>

<asp:Content ID="leftNavigation" ContentPlaceHolderID="Navigation" runat="server">
    <uc:LeftNavMenu id="leftNavMenu" runat="server" />
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="Main" runat="server">
    <form runat="server">
        <div align="center" class="pageTitle"><asp:Label ID="lblTitleLabel" runat="server" Text="Edit Existing Employee" /></div><p />
        <div>
            <asp:Label ID="lblValidationMessage" runat="server" ForeColor="Red" Visible="false" ClientIDMode="Static" />
            <asp:ValidationSummary ID="validateEmployee" runat="server" DisplayMode="BulletList" HeaderText="You must enter a value in the following fields: " />
        </div>
        <table border="0">
            <tr><td colspan="6">&nbsp;</td></tr>
            <tr valign="baseline">                
                <td align="right">
                    <asp:Label ID="lblFirstName" runat="server" CssClass="required" Text="First Name:" />
                </td>
                <td align="right">
                    <asp:TextBox ID="txtFirstName" runat="server" ClientIDMode="Static" />
                    <asp:RequiredFieldValidator ID="valFirstName" runat="server" ControlToValidate="txtFirstName" ErrorMessage="First name " Display="None" />
                </td>
                <td align="right" style="width:10%">
                    <asp:Label ID="lblMiddleInitial" runat="server" Text="MI:"  />
                </td>
                <td>
                    <asp:TextBox ID="txtMiddleInitial" runat="server" Width="15px" ClientIDMode="Static" />
                </td>
                <td align="right">
                    <asp:Label ID="lblLastName" runat="server" CssClass="required" Text="Last Name:" />
                </td>
                <td align="right">
                    <asp:TextBox ID="txtLastName" runat="server" ClientIDMode="Static" />
                    <asp:RequiredFieldValidator ID="valLastName" runat="server" ControlToValidate="txtLastName" ErrorMessage="Last name " Display="None" />
                </td>
            </tr>          
            <tr valign="baseline">
                <td align="right">
                    <asp:Label ID="lblSSN" runat="server" CssClass="required" Text="SSN:" />
                </td>
                <td align="right">
                    <asp:TextBox ID="txtSSN" runat="server" ClientIDMode="Static" />
                    <asp:RequiredFieldValidator ID="valSsn" runat="server" ControlToValidate="txtSSN" ErrorMessage="Social security number " Display="None" />
                </td>
                <td colspan="2">&nbsp</td>
                <td align="right">
                    <asp:Label ID="lblDOB" runat="server" CssClass="required" Text="DOB:" />
                </td>
                <td>
                    <asp:TextBox ID="txtDOB" runat="server" ClientIDMode="Static" />
                    <asp:RequiredFieldValidator ID="valDoB" runat="server" ControlToValidate="txtDOB" ErrorMessage="Date of birth " Display="None" />
                </td>
            </tr>
            <tr valign="baseline">
                <td align="right">
                    <asp:Label ID="lblEmployeePhone" runat="server" CssClass="required" Text="Phone:" />
                </td>
                <td>
                    <asp:TextBox ID="txtEmployeePhone" runat="server" ClientIDMode="Static" />
                    <asp:RequiredFieldValidator ID="valPhone" runat="server" ControlToValidate="txtEmployeePhone" ErrorMessage="Phone number " Display="None" />
                </td>
                <td colspan="4">&nbsp</td>
            </tr>
            <tr valign="baseline">
                <td align="right">
                    <asp:Label ID="lblAddress1" runat="server" CssClass="required" Text="Address 1:" />
                </td>
                <td>
                    <asp:TextBox ID="txtAddress1" runat="server" ClientIDMode="Static" />
                    <asp:RequiredFieldValidator ID="valAddress1" runat="server" ControlToValidate="txtAddress1" ErrorMessage="Address 1 " Display="None" />
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
                    <asp:TextBox ID="txtCity" runat="server" ClientIDMode="Static" />
                    <asp:RequiredFieldValidator ID="valCity" runat="server" ControlToValidate="txtCity" ErrorMessage="City " Display="None" />
                </td>
                <td align="right">
                    <asp:Label ID="lblState" runat="server" CssClass="required" Text="State:" />
                </td>
                <td>
                    <asp:TextBox ID="txtState" runat="server" Width="30px" ClientIDMode="Static" />
                    <asp:RequiredFieldValidator ID="valState" runat="server" ControlToValidate="txtState" ErrorMessage="State " Display="None" />
                </td>
                <td align="right">
                    <asp:Label ID="lblZipCode" runat="server" CssClass="required" Text="Zip Code:" />
                </td>
                <td>
                    <asp:TextBox ID="txtZipCode" runat="server" ClientIDMode="Static" />
                    <asp:RequiredFieldValidator ID="valZipCode" runat="server" ControlToValidate="txtZipCode" ErrorMessage="Zip code " Display="None" />
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
                    <asp:RadioButton ID="radioPermanent" Checked="true" ClientIDMode="Static" runat="server" GroupName="TempOrPerm" />                    
                </td>
                <td colspan="2">&nbsp</td>
                <td align="right">
                    <asp:Label ID="lblTemporary" runat="server" Text="Temporary:" />
                </td>
                <td>
                    <asp:RadioButton ID="radioTemporary" ClientIDMode="Static" runat="server" GroupName="TempOrPerm" />
                </td>
            </tr>
            <tr valign="baseline">                
                <td align="right">
                    <asp:Label ID="lblEmployeeNumber" CssClass="required" runat="server" Text="Employee Number:" />
                </td>
                <td>
                    <asp:TextBox ID="txtEmployeeNumber" runat="server" ClientIDMode="Static" />
                    <asp:RequiredFieldValidator ID="valEmployeeNumber" runat="server" ControlToValidate="txtEmployeeNumber" ErrorMessage="Employee number " Display="None" />
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
                    <asp:TextBox ID="txtHireDate" runat="server" ClientIDMode="Static" />
                    <asp:RequiredFieldValidator ID="valHireDate" runat="server" ControlToValidate="txtHireDate" ErrorMessage="Hire date " Display="None" />
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
                    <asp:Label ID="lblSource" runat="server" CssClass="required" Text="Hiring Source:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlEmploymentSource" ClientIDMode="Static" runat="server" Width="125px" />
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
                    <asp:Button ID="btnSaveEmployee" ClientIDMode="Static" runat="server" Text="Save" OnClick="btnSaveEmployee_Click" />
                </td>
                <td align="right">
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                </td>
            </tr>
            <tr><td colspan="6"><asp:Label ID="lblRecordSaved" runat="server" CssClass="required" Text="Employee saved" Visible="false" /></td></tr>
            <tr><td colspan="6">&nbsp;</td></tr>
            <tr><td colspan="6">&nbsp;</td></tr>
            <tr><td colspan="6">Fields in <span class="required">RED</span> are required.</td></tr>
        </table>
    </form>    
</asp:Content>