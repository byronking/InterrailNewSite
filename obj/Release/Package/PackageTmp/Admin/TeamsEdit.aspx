<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="TeamsEdit.aspx.cs" Inherits="InterrailPPRS.Admin.TeamsEdit" %>
<%@ Register Src="~/UserControls/LeftNavMenu.ascx" TagName="LeftNavMenu" TagPrefix="uc" %>


<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript">
    </script>
</asp:Content>

<asp:Content ID="leftNavigation" ContentPlaceHolderID="Navigation" runat="server">
      <uc:LeftNavMenu id="leftNavMenu" runat="server" />
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="Main" runat="server">
    <form runat="server">   
    <h2>View or edit teams</h2>

        <p><b>Facility: <asp:Label ID="lblFacility" runat="server" /></b></p>

        <div class="container">
            <div class="float-left">          
                <asp:Label ID="lblTeamName" runat="server" Text="Team Name:" ForeColor="Red" /> 
                <asp:TextBox ID="txtTeamName" runat="server" ClientIDMode="Static" />    
                Active: <asp:CheckBox ID="chkActive" runat="server" />  
            </div>
        </div>

        <div class="container">
            <div class="float_left down_20">
                <b>Available Employees</b><p />
                <asp:ListBox ID="listEmployees" runat="server" Width="165" Rows="10" SelectionMode="Multiple" /> 
            </div>
                               
            <div class="float_right down_20">
                <b>Team Members</b><p />
                <asp:ListBox ID="listTeamMembers" runat="server" Width="165" Rows="10" SelectionMode="Multiple" />
            </div>
        </div>

        <div class="container">
            <div class="float_left"><asp:Button ID="btnAddMembers" runat="server" Text ="Add members >>" ToolTip="Add team members" OnClick="btnAddMembers_Click" /></div>
            <div class="float_right"><asp:Button ID="btnRemoveRembers" runat="server" Text ="<< Remove members" ToolTip="Remove team members" OnClick="btnRemoveMembers_Click" /></div>    
        </div>      
        
        <div class="container down_20">
            <asp:Button ID="btnSave" runat="server" Text="Save" ClientIDMode="Static" OnClick="btnSave_Click" /> &nbsp; 
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" ClientIDMode="Static" OnClick="btnCancel_Click" />
        </div>  
            
        <div class="container">
            <div class="float_left down_20" style="clear:both;">Last Modified On: <asp:Label ID="lblLastModifiedOn" runat="server" /></div>
            <div class="float_right down_20">Last Modified By: <asp:Label ID="lblLastModifiedBy" runat="server" /></div>
        </div>

        <div class="container">
            <div class="float_left down_20" style="clear:both;">Fields in <asp:Label ID="lblRed" runat="server" ForeColor="Red" Text="RED" /> are required.</div>
        </div>
            
    </form> 
</asp:Content>
