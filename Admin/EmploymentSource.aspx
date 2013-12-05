<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EmploymentSource.aspx.cs" Inherits="InterrailPPRS.Admin.EmploymentSource" %>



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
        <%= ChangeFacilityLink()%>
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
            <table align="center"   cellpadding="0" cellspacing="0" width="100%">
                <tr> 
                <td align="default" colspan="2"> 
                    <div align="center" class="pageTitle">Employment Source </div>
                </td>
                </tr>
                <tr> 
                <td align="default" width="20%">&nbsp;</td>
                <td align="default" width="80%">&nbsp;</td>
                </tr>
                <tr> 
                <td align="default" width="20%">&nbsp;</td>
                <td align="default" width="80%"> 
                    <div align="right"><a href="EmploymentSourceEdit.aspx?ID=0">Add New Source</a></div>
                </td>
                </tr>
                <tr> 
                <td align="default" width="20%">&nbsp;</td>
                <td align="default" width="80%">&nbsp;</td>
                </tr>
                <tr> 
                <td align="left" width="20%" class="cellTopBottomBorder"> 
                    Code </td>
                <td align="left" width="80%" class="cellTopBottomBorder"> 
                    Name </td>
                </tr>
                <%string rowColor;%>
                <% 
                while ((Repeat1__numRows != 0) && (! rs.EOF)){
                    rs.Read();
                %>
                <% 
                    if ((Repeat1__index % 2) == 0) {
                    rowColor = "reportEvenLine";
                    }else{	
                    rowColor = "reportOddLine";
                    }
                    %>
                <tr class="<%=rowColor%>"> 
                <td align="left" width="20%"> 
                    <a href="EmploymentSourceEdit.aspx?<%= MM_keepForm + MM_joinChar(MM_keepForm) + "Id=" + rs.Item("Id") %>"><%=(rs.Item("SourceCode"))%></a> </td>
                <td align="left" width="80%"> 
                    <a href="EmploymentSourceEdit.aspx?<%= MM_keepForm + MM_joinChar(MM_keepForm) + "Id=" + rs.Item("Id") %>"><%=(rs.Item("SourceName"))%></a> </td>
                </tr>
                <% 
                Repeat1__index  = Repeat1__index + 1;
                Repeat1__numRows = Repeat1__numRows - 1;
                              
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
                    <% if( MM_offset != 0 ){ %>
                    <a href="<%=MM_moveFirst%>">First</a> 
                    <% } // end MM_offset <> 0 %>
                </td>
                <td width="31%" align="center"> 
                    <% if( MM_offset != 0 ){ %>
                    <a href="<%=MM_movePrev%>">Previous</a> 
                    <% } // end MM_offset <> 0 %>
                </td>
                <td width="23%" align="center"> 
                    <% if( ! MM_atTotal ){ %>
                    <a href="<%=MM_moveNext%>">Next</a> 
                    <% } // end Not MM_atTotal %>
                </td>
                <td width="23%" align="center"> 
                    <% if(! MM_atTotal ){ %>
                    <a href="<%=MM_moveLast%>">Last</a> 
                    <% } // end Not MM_atTotal %>
                </td>
                </tr>
            </table>
            <p>Records <%=(rs_first)%> to <%=(rs_last)%> of <%=(rs_total)%> </p>
            <!-- #EndEditable -->
            </td>
        </tr>
       
	    </table>

</asp:Content>