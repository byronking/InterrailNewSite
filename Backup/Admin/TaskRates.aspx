<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="TaskRates.aspx.cs" Inherits="InterrailPPRS.Admin.TaskRates" %>



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
            <table align="center" border="0" cellpadding="0" cellspacing="0" width="520">
                <tr> 
                <td align="default" colspan="5"> 
                    <div align="center" class="pageTitle"><%=Request["Task"]%> Rates </div>
                </td>
                </tr>
                <tr> 
                <td align="default" width="25%">&nbsp;</td>
                <td align="default" width="25%">&nbsp;</td>
                <td align="default" width="10%">&nbsp;</td>
                <td align="default" width="15%">&nbsp;</td>
                <td align="default" width="15%">&nbsp;</td>
                </tr>
                <tr> 
                <td align="default" width="12%">&nbsp;</td>
                <td align="default" width="24%">&nbsp;</td>
                <td align="default" width="19%">&nbsp;</td>
                <td align="default" colspan="2"> 
                    <div align="right"><a href="TaskRatesEdit.aspx?ID=0&TID=<%= Request["ID"] %>&Task=<%= Request["Task"] %>" >Add 
                    New Task Rate</a></div>
                </td>
                </tr>
                <tr> 
                <td align="default" width="12%">&nbsp;</td>
                <td align="default" width="24%">&nbsp;</td>
                <td align="default" width="19%">&nbsp;</td>
                <td align="default" width="15%">&nbsp;</td>
                <td align="default" width="19%">&nbsp;</td>
                </tr>
                <tr> 
                <td align="default" width="12%" class="cellTopBottomBorder"> 
                    Last Name </td>
                <td align="default" width="24%" class="cellTopBottomBorder"> 
                    First Name </td>
                <td align="default" width="19%" class="cellTopBottomBorder">Number</td>
                <td align="default" width="15%" class="cellTopBottomBorder"> 
                    Effective</td>
                <td align="default" width="19%" class="cellTopBottomBorder"> 
                    Expiration</td>
                </tr>
                <%string rowColor; %>
                <% 
                while ((Repeat1__numRows != 0) && (! rs.EOF)){ 
                %>
                                                <%
                    if ((Repeat1__index % 2) == 0) {
                    rowColor = "reportEvenLine";
                    }else{	
                    rowColor = "reportOddLine" ;
                    }
                    %>
                <tr  class="<%=rowColor%>"> 
                <td align="default" width="12%"> 
                    <a href="TaskRatesEdit.aspx?<%= MM_keepForm + MM_joinChar(MM_keepForm) + "ID=" + rs.Item("ID") + "&TID=" + Request["ID"] + "&Task=" + Request["Task"] %>  "><%=(rs.Item("LastName"))%></a> </td>
                <td align="default" width="24%"> 
                    <a href="TaskRatesEdit.aspx?<%= MM_keepForm + MM_joinChar(MM_keepForm) + "ID=" + rs.Item("ID") + "&TID=" + Request["ID"] + "&Task=" + Request["Task"] %>  "><%=(rs.Item("FirstName"))%></a> </td>
                <td align="default" width="19%"> 
                    <a href="TaskRatesEdit.aspx?<%= MM_keepForm + MM_joinChar(MM_keepForm) + "ID=" + rs.Item("ID") + "&TID=" + Request["ID"] + "&Task=" + Request["Task"] %>  "><%=(rs.Item("EmployeeNumber"))%></a> </td>
                <td align="default" width="15%"> 
                    <%=(rs.Item("EffectiveDate"))%> </td>
                <td align="default" width="19%"> 
                    <%=(rs.Item("ExpirationDate"))%> </td>
                </tr>
                <% 
                Repeat1__index = Repeat1__index + 1;
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
                    <% if(! MM_atTotal ){ %>
                    <a href="<%=MM_moveNext%>">Next</a> 
                    <% } // end Not MM_atTotal %>
                </td>
                <td width="23%" align="center"> 
                    <% if(! MM_atTotal ){ %>
                    <a href="<%=MM_moveLast%>">Last</a> 
                    <% }// end Not MM_atTotal %>
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