<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Rebilling/Rebilling.Master" CodeBehind="RebillDetail.aspx.cs" Inherits="InterrailPPRS.Rebilling.RebillDetail2" %>
<%@ Register Src="~/UserControls/LeftNavMenu.ascx" TagName="LeftNavMenu" TagPrefix="uc" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

</asp:Content>

<asp:Content ID="leftNavigation" ContentPlaceHolderID="Navigation" runat="server">
      <uc:LeftNavMenu id="leftNavMenu" runat="server" />
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="Main" runat="server">
    <h2>Rebill Detail</h2><p />
    <div>
        <a href="RebillEntry.aspx">Rebill Entry</a><br />
        <a href="RebillEdit.aspx?id=0">Add New Rebill Detail</a><p />
    </div>
    <div>
        <form runat="server">
            <asp:GridView runat="server" ID="grdRebillData" AllowPaging="true" PageSize="25" OnPageIndexChanging="grdRebillData_PageIndexChanging" AllowSorting="false"
                AlternatingRowStyle-BackColor="#DEDFFF" AutoGenerateColumns="false" GridLines="Horizontal" EmptyDataText="No results found!" RowStyle-Height="40">
                <PagerSettings Position="Bottom" />
                <Columns>
                    <asp:HyperLinkField DataTextField="WorkDate" HeaderText="Work Date" DataNavigateUrlFields="RebillDetailId" 
                        DataNavigateUrlFormatString="~\Rebilling\ApproveRebillDataEdit.aspx?Id={0}" HeaderStyle-Width="100" 
                        ItemStyle-HorizontalAlign="Center" DataTextFormatString="{0:MM/dd/yy}" />
                    <asp:BoundField DataField="RebillStatus" HeaderText="Approval" HeaderStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="TaskCode" HeaderText="Task" HeaderStyle-Width="60" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer" HeaderStyle-Width="260" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" HeaderStyle-Width="60" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="TotalHours" HeaderText="Hours" HeaderStyle-Width="60" ItemStyle-HorizontalAlign="Center" />
                </Columns>
            </asp:GridView>
        </form>        
    </div><p />
</asp:Content>