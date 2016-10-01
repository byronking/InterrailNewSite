<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApproveRebillData.aspx.cs" MasterPageFile="~/Rebilling/Rebilling.Master" Inherits="InterrailPPRS.Rebilling.ApproveRebillData" %>
<%@ Register Src="~/UserControls/LeftNavMenu.ascx" TagName="LeftNavMenu" TagPrefix="uc" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

</asp:Content>

<asp:Content ID="leftNavigation" ContentPlaceHolderID="Navigation" runat="server">
    <uc:LeftNavMenu id="leftNavMenu" runat="server" />
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="Main" runat="server">
    <h2>Approve Rebill Data: <asp:Label ID="lblFacilityName" runat="server" /></h2>
    <form id="rebillDataForm" runat="server">
        <div>
            <h3>
                <asp:Label ID="lblApproveChecked" runat="server" ForeColor="Green" Text="Approve checked: " />
                <asp:Button ID="btnApproveChecked" runat="server" OnClick="btnApproveChecked_Click" Text="Go" />
            </h3>
        </div><p />
        <div>
            <h3>
                <asp:Label ID="lblApproveDateRange" runat="server" ForeColor="Green" Text="Approve all OPEN within this date range: " /><br />
                <asp:TextBox ID="txtStartDate" runat="server" ToolTip="Enter start date" Placeholder="Start date" ClientIDMode="Static" />&nbsp;
                <asp:TextBox ID="txtEndDate" runat="server" ToolTip="Enter end date" Placeholder="End date" ClientIDMode="Static" />
                <asp:Button ID="btnApproveDateRange" runat="server" OnClick="btnApproveDateRange_Click" Text="Go" ClientIDMode="Static" />
            </h3>
        </div><p />
        <div><h3><asp:Label ID="lblSaveMessage" runat="server" Text="Rebill data approval successful!" Visible="false" /></h3></div>
        <div>
            <asp:GridView runat="server" ID="grdRebillDataForApproval" AllowPaging="true" PageSize="25" OnPageIndexChanging="grdRebillDataForApproval_PageIndexChanging" AllowSorting="false" 
                OnSorting="grdRebillDataForApproval_Sorting" AlternatingRowStyle-BackColor="#DEDFFF" AutoGenerateColumns="false" GridLines="Horizontal" EmptyDataText="No results found!">
                <PagerSettings Position="Bottom" />
                <Columns>
                    <asp:HyperLinkField DataTextField="RebillDetailId" HeaderText="ID" DataNavigateUrlFields="RebillDetailId" DataNavigateUrlFormatString="ApproveRebillDataEdit.aspx?Id={0}" 
                        HeaderStyle-Width="100" ItemStyle-HorizontalAlign="Center" visible="false" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="lblSelectWorkDate" runat="server" ClientIDMode="Static" style="display:none;" Text='<%# DataBinder.Eval(Container.DataItem, "RebillDetailId") %>' />
                            <asp:CheckBox ID="chkSelectWorkDate" runat="server" ClientIDMode="Static" data-rebill-id='<%# DataBinder.Eval(Container.DataItem, "RebillDetailId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:HyperLinkField DataTextField="WorkDate"  HeaderText="Work Date" DataNavigateUrlFields="RebillDetailId" DataNavigateUrlFormatString="ApproveRebillDataEdit.aspx?Id={0}" 
                        DataTextFormatString="{0:d}" HeaderStyle-Width="100" ItemStyle-HorizontalAlign="Center" />                
                    <asp:BoundField DataField="RebillStatus" HeaderText="Status" HeaderStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="TaskCode" HeaderText="Task Code" HeaderStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" HeaderStyle-Width="200" ItemStyle-HorizontalAlign="Center" /> 
                    <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" HeaderStyle-Width="80" ItemStyle-HorizontalAlign="Center" />                   
                    <asp:BoundField DataField="TotalUnits" HeaderText="Units" HeaderStyle-Width="60" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="TotalHours" HeaderText="Hours" HeaderStyle-Width="60" ItemStyle-HorizontalAlign="Center" />
                </Columns>
            </asp:GridView>
        </div><p />        
    </form>
</asp:Content>