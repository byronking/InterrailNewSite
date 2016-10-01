<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="InterrailPPRS.Admin.User" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script src="../Scripts/jquery-1.9.1.min.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="leftNavigation" ContentPlaceHolderID="Navigation" runat="server">
  <table width="81%" border="0"  valign="top">
        <% if (CheckSecurity("Super"))
           { %>
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

    <form id="allUsersForm" runat="server">
        <h2>InterRail Users</h2>
        <div style="float:right;"><asp:HyperLink ID="linkAddNewUser" runat="server" Text="Add New User" NavigateUrl="~\Admin\UserEdit.aspx?Id=0" /></div><p />
        <div><asp:TextBox ID="txtSearch" runat="server" />&nbsp;<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" /></div><p />
        <div>
            <asp:GridView runat="server" ID="grdAllUsers" AllowPaging="true" PageSize="15" OnPageIndexChanging="grdAllUsers_PageIndexChanging"
                AllowSorting="false" OnSorting="grdAllUsers_Sorting" AlternatingRowStyle-BackColor="#DEDFFF" 
                AutoGenerateColumns="false" GridLines="Horizontal" EmptyDataText="No results found!">
                <PagerSettings Position="Bottom" />
                <Columns>
                    <asp:HyperLinkField DataTextField="UserId" HeaderText="User ID" DataNavigateUrlFields="Id" 
                        DataNavigateUrlFormatString="~\Admin\UserEdit.aspx?Id={0}" HeaderStyle-Width="100" 
                        ItemStyle-HorizontalAlign="Center" SortExpression="UserId" />
                    <asp:BoundField DataField="UserName" HeaderText="First Name" HeaderStyle-Width="100" 
                        ItemStyle-HorizontalAlign="Center" SortExpression="UserName" />
                    <asp:BoundField DataField="UserLongName" HeaderText="Full Name" HeaderStyle-Width="100" 
                        ItemStyle-HorizontalAlign="Center" SortExpression="UserLongName" />
                    <asp:HyperLinkField DataTextField="Id" DataNavigateUrlFields="Id" 
                        DataNavigateUrlFormatString="~\Admin\UserFacilityAccess.aspx?Id={0}" 
                        DataTextFormatString="Facility Access" HeaderStyle-Width="98" ItemStyle-HorizontalAlign="Center" />
                    <asp:HyperLinkField DataTextField="Id" DataNavigateUrlFields="Id" 
                        DataNavigateUrlFormatString="~\Admin\UserFacilityMonitor.aspx?Id={0}" 
                        DataTextFormatString="Facility Monitoring" HeaderStyle-Width="112" 
                        ItemStyle-HorizontalAlign="Center" />
                </Columns>
            </asp:GridView>
        </div><p />
        <div><asp:Label ID="lblRecordCount" runat="server" /> </div>
    </form>
    <p>&nbsp;</p>
</asp:Content>