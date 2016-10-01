<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PayrollReports2.aspx.cs" MasterPageFile="~/Reports/Reports.Master" Inherits="InterrailPPRS.Reports.PayrollReports2" %>
<%@ Register Src="~/UserControls/LeftNavMenu.ascx" TagName="LeftNavMenu" TagPrefix="uc" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

</asp:Content>

<asp:Content ID="leftNavigation" ContentPlaceHolderID="Navigation" runat="server">
  <form runat="server" id="navForm">
      <uc:LeftNavMenu id="leftNavMenu" runat="server" />
   </form>
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="Main" runat="server">
    <hr />
    <h2>Payroll Report</h2> 
    <hr />

    <h3>Facilities</h3>
    <form runat="server" id="mainForm">
        <asp:DropDownList ID="ddlFacilities" runat="server" />
        <span class="red">* Select one or more facilities. if none are selected, the default facility is used.</span> 
    </form>
    
</asp:Content>