<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Rebilling/Rebilling2.Master" CodeBehind="Default2.aspx.cs" Inherits="InterrailPPRS.Rebilling.Default2" %>
<%@ Register Src="~/UserControls/LeftNavMenu.ascx" TagName="LeftNavMenu" TagPrefix="uc" %>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>

<asp:Content ID="leftNavigation" ContentPlaceHolderID="Navigation" runat="server">
    <uc:LeftNavMenu id="leftNavMenu" runat="server" />
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="Main" runat="server">
    <form runat="server">
        <h3>Rebilling approval status for pay period: <asp:Label ID="lblDateRange" runat="server" /></h3>
        <div style="float:left;">
            Select a pay period start date:<p />
            <asp:Calendar ID="calStartDate" runat="server" OnSelectionChanged="calStartDate_SelectionChanged" />            
        </div>

        <div style="float:right;margin-right:180px;">
            <span>Pay period end date:</span><p />
            <asp:TextBox ID="txtEndDate" runat="server" Enabled="false" Width="55px" /><p /> 
        </div>

        <div style="float:right;width:475px;margin-top:20px">
            <asp:Button ID="btnGetRebillingData" runat="server" Text="Get rebilling data" />
        </div>
    </form>         
</asp:Content>