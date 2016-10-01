<%@ Page Language="C#" MasterPageFile="~/Reports/Reports.Master" AutoEventWireup="true" CodeBehind="PayrollDetailReport2.aspx.cs" Inherits="InterrailPPRS.Reports.PayrollDetailReport2" %>
<%@ Register Src="~/UserControls/LeftNavMenu.ascx" TagName="LeftNavMenu" TagPrefix="uc" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

</asp:Content>

<asp:Content ID="leftNavigation" ContentPlaceHolderID="Navigation" runat="server">
    <uc:LeftNavMenu id="leftNavMenu" runat="server" />
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="Main" runat="server">
    <h3>Payroll Detail Report</h3>
</asp:Content>