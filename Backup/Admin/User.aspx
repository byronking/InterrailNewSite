<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="InterrailPPRS.Admin.User" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

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
        <table width="100%" border="0" cellspacing="0" cellpadding="8">
        <tr>
            <td>
			<!-- #BeginEditable "MainBody" -->
            <p align="right">&nbsp;</p>
            <table align="center" border="0" cellspacing="0" cellpadding="0" width="98%">
                <tr>
                <td align="default" colspan="5">
                    <div align="center" class="pageTitle"><b>User</b></div>
                </td>
                </tr>
                <tr>
                <td align="default" width="15%">&nbsp;</td>
                <td align="default" width="15%">&nbsp;</td>
                <td align="default" width="34%">&nbsp;</td>
                <td align="default" width="26%" colspan="2">&nbsp;</td>
                </tr>
                <tr>
                <td align="default" width="15%">&nbsp;</td>
                <td align="default" width="15%">&nbsp;</td>
                <td align="default" width="34%">&nbsp;</td>
                <td align="default" width="26%" colspan="2">
                    <div align="right"><a href="UserEdit.aspx?id=0">Add
                    New User</a></div>
                </td>
                </tr>
                <tr>
                <td align="default" width="15%">&nbsp;</td>
                <td align="default" width="15%">&nbsp;</td>
                <td align="default" width="34%">&nbsp;</td>
                <td align="default" width="26%" colspan="2">&nbsp;</td>
                </tr>
                <tr  bordercolor="#CCCCCC">
                <th align="default" width="10%" height="13"  class="cellTopBottomBorder">
                    <div align="left"><b> User ID </b></div>
                </th>
                <th align="default" width="15%" height="13" class="cellTopBottomBorder">
                    <div align="left"><b> Name </b></div>
                </th>
                <th align="default" width="34%" height="13" class="cellTopBottomBorder">
                    <div align="left"><b> Long Name </b></div>
                </th>
                <th align="default" height="13" class="cellTopBottomBorder" colspan="2">
                    <div align="left">&nbsp;</div>
                </th>
                </tr>
                <%
				string  rowColor;
				%>
                <%
                while ((Repeat1__numRows != 0) && (! rs.EOF)){
                    rs.Read();
                %>
                                <%
                if((Repeat1__index % 2) == 0) {
                rowColor = "reportEvenLine";
                }else{
                    rowColor = "reportOddLine";
                }
                %>
                <tr class="<%=rowColor%>">
                <td align="left">
                    <a href="UserEdit.aspx?<%= MM_keepBoth + MM_joinChar(MM_keepBoth) + "Id=" + rs.Item("Id") %>"><%=(rs.Item("UserID"))%></a> </td>
                <td align="left" width="10%">
                    <%=(rs.Item("UserName"))%> </td>
                <td align="left">
                    <%=(rs.Item("UserLongName"))%> </td>
                <td align="left" width="20%" >
                    <% if( Trim(rs.Item("UserType")) == "User" || Trim(rs.Item("UserType")) == "Production") { %>
                    <a href="UserFacilityAccess.aspx?Id=<%=rs.Item("Id")%>">Facility
                    Access</a>
                    <% }  %>
                </td>
                <td align="left" width="20%" >
                    <% if ( Trim(rs.Item("UserType")) == "User" ) { %>
                    <a href="UserFacilityMonitor.aspx?Id=<%=rs.Item("Id")%>">Facility
                    Monitoring</a>
                    <% }  %>
                </td>
                </tr>
                <%
                Repeat1__index=Repeat1__index+1;
                Repeat1__numRows=Repeat1__numRows-1;
                }
                %>
            </table>
            <table border="0" width="50%" align="center">
                <tr>
                <td width="23%" align="center">&nbsp;</td>
                <td width="31%" align="center">&nbsp;</td>
                <td width="23%" align="center">&nbsp;</td>
                <td width="23%" align="center">&nbsp;</td>
                </tr>
                <tr>
                <td width="23%" align="center">
                    <% if ( MM_offset != 0) { %>
                    <a href="<%=MM_moveFirst%>">First</a>
                    <% } // end MM_offset <> 0 %>
                </td>
                <td width="31%" align="center">
                    <% if ( MM_offset != 0) { %>
                    <a href="<%=MM_movePrev%>">Previous</a>
                    <% } // end MM_offset <> 0 %>
                </td>
                <td width="23%" align="center">
                    <% if(! MM_atTotal ){ %>
                    <a href="<%=MM_moveNext%>">Next</a>
                    <% } //' end Not MM_atTotal %>
                </td>
                <td width="23%" align="center">
                    <% if(! MM_atTotal ){ %>
                    <a href="<%=MM_moveLast%>">Last</a>
                    <% } //' end Not MM_atTotal %>
                </td>
                </tr>
            </table>
            <p>Records <%=(rs_first)%> to <%=(rs_last)%> of <%=(rs_total)%> </p>
            <!-- #EndEditable -->
            </td>
        </tr>

	    </table>
        <p>&nbsp;</p>
</asp:Content>