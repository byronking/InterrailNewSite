<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master" CodeBehind="Facility.aspx.cs" Inherits="InterrailPPRS.Admin.Facility2" %>
<%@ Register Src="~/UserControls/LeftNavMenu.ascx" TagName="LeftNavMenu" TagPrefix="uc" %>
<script type="C#" runat="server">
    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);        
    }        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

</asp:Content>

<asp:Content ID="leftNavigation" ContentPlaceHolderID="Navigation" runat="server">
      <uc:LeftNavMenu id="leftNavMenu" runat="server" />
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="Main" runat="server">
    <form id="allFacilitiesForm" runat="server">
        <h2>Interrail Facilities</h2>
        <div style="float:right;"><asp:HyperLink ID="linkAddNewFacility" runat="server" Text="Add New Facility" NavigateUrl="~\Admin\FacilityEdit.aspx?Id=0" /></div><p />
        <div>
            <asp:TextBox ID="txtSearch" runat="server" />&nbsp;
            <asp:Button ID="btnSearchFacilities" runat="server" Text="Search" OnClick="btnSearchFacilities_Click" />
            <asp:Button ID="btnReset" runat="server" Text="Reset" />
        </div><p />
        <div>
            <asp:GridView runat="server" ID="grdFacilities" AllowPaging="true" PageSize="15" OnPageIndexChanging="grdFacilities_PageIndexChanging" AllowSorting="false" OnSorting="grdFacilities_Sorting" 
                AlternatingRowStyle-BackColor="#DEDFFF" AutoGenerateColumns="false" GridLines="Horizontal" EmptyDataText="No results found!">
                <PagerSettings Position="Bottom" />
                <Columns>
                    <asp:HyperLinkField DataTextField="FacilityNumber" HeaderText="Number" DataNavigateUrlFields="FacilityId" DataNavigateUrlFormatString="~\Admin\FacilityEdit.aspx?Id={0}" HeaderStyle-Width="100" 
                        ItemStyle-HorizontalAlign="Center" SortExpression="Number" />
                    <asp:BoundField DataField="AlphaCode" HeaderText="Facility Code" HeaderStyle-Width="100" ItemStyle-HorizontalAlign="Center" SortExpression="AlphaCode" />
                    <asp:BoundField DataField="FacilityName" HeaderText="Facility Name" HeaderStyle-Width="160" ItemStyle-HorizontalAlign="Center" SortExpression="FacilityName" />
                    <asp:HyperLinkField DataTextField="FacilityId" DataNavigateUrlFields="FacilityId" DataNavigateUrlFormatString="~\Admin\FacilityTasks.aspx?Id={0}" DataTextFormatString="Facility Tasks" 
                        HeaderStyle-Width="116" ItemStyle-HorizontalAlign="Center" /> 
                </Columns>
            </asp:GridView>
        </div><p />
        <div><asp:Label ID="lblRecordCount" runat="server" /> </div>
    </form>
    <p>&nbsp;</p>
</asp:Content>