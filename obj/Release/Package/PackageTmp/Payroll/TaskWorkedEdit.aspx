<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Payroll/Payroll.Master" CodeBehind="TaskWorkedEdit.aspx.cs" Inherits="InterrailPPRS.Payroll.TaskWorkedEdit" %>
<%@ Register TagPrefix="uc" TagName="LeftNavMenu" Src="~/UserControls/LeftNavMenu.ascx"%>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

</asp:Content>

<asp:Content ID="leftNavigation" ContentPlaceHolderID="Navigation" runat="server">
    <uc:LeftNavMenu runat="server" id="leftNavMenu" />
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="Main" runat="server">
    <form runat="server">
        <h2>Task Worked</h2>
        <p/>
        <table border="0">
            <tr>
                <td>Date:</td>
                <td><asp:Label ID="lblDate" runat="server" /></td>
            </tr>
            <tr>
                <td>Shift:</td>
                <td><asp:Label ID="lblShift" runat="server" /></td>
            </tr>
            <tr>
                <td>Employee:</td>
                <td><asp:DropDownList runat="server" ID="ddlEmployees" ClientIDMode="Static" /></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblTaskDesc" runat="server" Text="Task: " Visible="false" />
                    <asp:Label ID="lblTaskDropdown" runat="server" Text="Task: " Visible="false" />
                </td>
                <td>
                    <asp:Label ID="lblTask" runat="server" Visible="false" />
                    <asp:DropDownList ID="ddlTasks" runat="server" Visible="false" />
                </td>
            </tr>
            <tr>
                <td>Hours:</td>
                <td><asp:TextBox ID="txtHours" runat="server" /></td>
            </tr>
            <tr>
                <td>UPM:</td>
                <td><asp:TextBox ID="txtUpm" runat="server" Enabled="false" /></td>
            </tr>
            <tr>
                <td> Out of Town: </td>
                <td>
                    <asp:DropDownList ID="ddlOutOfTown" runat="server">
                        <asp:ListItem Text="No" Value="N" />            
                        <asp:ListItem Text="Out of town" Value="O" />
                        <asp:ListItem Text="Daily" Value="D" />
                    </asp:DropDownList>
                </td>
            </tr>            
            <tr>
                <td>Notes:</td>
                <td><asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Rows="8" Columns="20" /></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>            
            <tr>
                <td>&nbsp;</td>
                <td>                    
                    <div style="margin-left:30px">
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />&nbsp;
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" Visible="false" />&nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />&nbsp;
                    </div>
                </td>
            </tr>
            <tr><td colspan="2">&nbsp;</td></tr>
            <tr>
                <td colspan="2">This record is: <asp:Label ID="lblRecordStatus" runat="server" /></td>
            </tr>
            <tr>
                <td>Last Modified:</td>
                <td><asp:Label ID="lblLastModifiedOn" runat="server" /></td>
            </tr>
            <tr>
                <td>Last Modified By:</td>
                <td><asp:Label ID="lblLastModifiedBy" runat="server" /></td>
            </tr>
        </table>
    </form>    
</asp:Content>