<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Employee.aspx.cs" Inherits="InterrailPPRS.Admin.Employee" %>



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
  <table width="90%" border="0" cellspacing="0" cellpadding="8">
  <tr> 
    <td>
	<!-- #BeginEditable "MainBody" --> 
    <table align="center"  cellpadding="0" cellspacing="0" width="100%">
        <tr> 
        <td align="default" colspan="5"> 
            <div align="center" class="pageTitle">Employee</div>
        </td>
        </tr>
        <tr> 
        <td align="default" width="22%">&nbsp;</td>
        <td align="default" width="19%">&nbsp;</td>
        <td align="default" width="31%">&nbsp;</td>
        <td align="default" width="3%">&nbsp;</td>
        <td align="default" width="25%">&nbsp;</td>
        </tr>
        <tr> 
        <td align="default" colspan="3"> 
            <div ><form name='search' method='post' action='employee.aspx'>Last Name or SSN: <input type=text name='search' value='<%=Request["search"]%>'><input type=submit value='Search' /></form></div>
        </td>
        <td align="default" colspan="2"> 
            <div ><form name='search' method='post' action='employee.aspx'><input type=hidden name='search' value='' /><input type=submit value='Show All' /></form></div>
        </td>
        </tr>
        <tr> 
        <td align="default" width="22%">&nbsp;</td>
        <td align="default" width="19%"> </td>
        <td align="default" width="31%"> </td>
        <td align="default" width="3%"></td>
        <td align="default" width="25%"> 
            <div align="right"><a href="EmployeeEdit.aspx?ID=0">Add 
            New Employee</a></div>
        </td>
        </tr>
        <tr> 
        <td align="default" width="22%">&nbsp;</td>
        <td align="default" width="19%">&nbsp;</td>
        <td align="default" width="31%">&nbsp;</td>
        <td align="default" width="3%">&nbsp;</td>
        <td align="default" width="25%">&nbsp;</td>
        </tr>
        <tr> 
        <td align="left" width="22%" class="cellTopBottomBorder"> 
            Number </td>
        <td align="left" width="19%" class="cellTopBottomBorder"> 
            Last Name </td>
        <td align="left" width="31%" class="cellTopBottomBorder"> 
            First Name </td>
        <td align="left" width="3%" class="cellTopBottomBorder">Temp</td>
        <td align="left" width="25%" class="cellTopBottomBorder">&nbsp;</td>
        </tr>
        <% 
        
            string  rowColor;
         
        while ((Repeat2__numRows != 0) && (! rs.EOF)){
            rs.Read();
        %>
                                        <% 
            if (Repeat2__index % 2 == 0) {
               rowColor = "reportEvenLine";
            }else{  
               rowColor = "reportOddLine";
            }
        %>
        <tr class="<%=rowColor%>"> 
        <td align="left" width="22%"> 
			<%if (rs.Item("FromAssociatedFacility") == "1") { %>
            <%if (System.Convert.ToBoolean(rs.Item("TempEmployee"))) {%>
            <%=(rs.Item("TempNumber"))%> + 
            <%}else{%>
            <%=(rs.Item("EmployeeNumber"))%> +
            <%}%>
            <%}else{%>
            <%if (System.Convert.ToBoolean(rs.Item("TempEmployee"))) {%>
            <a href="EmployeeEdit.aspx?<%= MM_keepBoth + MM_joinChar(MM_keepBoth) + "Id=" + rs.Item("Id") %>"><%=(rs.Item("TempNumber"))%></a> 
            <%}else{%>
            <a href="EmployeeEdit.aspx?<%= MM_keepBoth + MM_joinChar(MM_keepBoth) + "Id=" + rs.Item("Id") %>"><%=(rs.Item("EmployeeNumber"))%></a> 
            <%}%>
            <%}%>
        </td>
        <td align="left" width="19%"> 
            <%=(rs.Item("LastName"))%></td>
        <td align="left" width="31%"><%=(rs.Item("FirstName"))%></td>
        <td align="center" width="3%">
            <%if (System.Convert.ToBoolean(rs.Item("TempEmployee"))) { %>
                <img src="../images/check.gif">
            <% }else{ %>
                &nbsp;
            <% } %>
        </td>
        <td align="right" width="25%"><a href="EmployeeRates.aspx?<%= MM_keepBoth + MM_joinChar(MM_keepBoth) + "Id=" + rs.Item("Id") %>">Task 
            Rates</a></td>
        </tr>
        <% 
        Repeat2__index=Repeat2__index + 1;
        Repeat2__numRows=Repeat2__numRows - 1;

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
            <% if( MM_offset != 0){ %>
            <a href="<%=MM_moveFirst%>">First</a> 
            <% } // end MM_offset <> 0 %>
        </td>
        <td width="31%" align="center"> 
            <% if( MM_offset != 0){ %>
            <a href="<%=MM_movePrev%>">Previous</a> 
            <% } // end MM_offset <> 0 %>
        </td>
        <td width="23%" align="center"> 
            <% if( ! MM_atTotal){ %>
            <a href="<%=MM_moveNext%>">Next</a> 
            <% } // end Not MM_atTotal %>
        </td>
        <td width="23%" align="center"> 
            <% if(! MM_atTotal){ %>
            <a href="<%=MM_moveLast%>">Last</a> 
            <% } // end Not MM_atTotal %>
        </td>
        </tr>
    </table>
    <p>Records <%=(rs_first)%> to <%=(rs_last)%> of <%=(rs_total)%> </p>

    </td>
    </tr>
    </table>
</asp:Content>