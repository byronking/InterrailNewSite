<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Company.aspx.cs" Inherits="InterrailPPRS.Admin.Company" %>



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
                            <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                              <tr> 
                                <td align="default" colspan="2"> 
                                  <div align="center" class="pageTitle">Companies</div>
                                </td>
                              </tr>
                              <tr> 
                                <td align="default" width="26%">&nbsp;</td>
                                <td align="default" width="74%">&nbsp;</td>
                              </tr>
                              <tr> 
                                <td align="default" width="26%">&nbsp;</td>
                                <td align="default" width="74%"> 
                                  <div align="right"><a href="CompanyEdit.aspx?ID=0">Add 
                                    New Company</a></div>
                                </td>
                              </tr>
                              <tr> 
                                <td align="default" width="26%">&nbsp;</td>
                                <td align="default" width="74%">&nbsp;</td>
                              </tr>
                              <tr bordercolor="#CCCCCC"> 
                                <th align="default" width="26%" class="cellTopBottomBorder"> 
                                  <div align="left"> ID </div>
                                </th>
                                <th align="default" width="74%" class="cellTopBottomBorder"> 
                                  <div align="left"> Name </div>
                                </th>
                              </tr>
                            <%
                            
                             while ((Repeat1__numRows != 0) && ( rsCompany.Read())) {
                            %>
                                                          <%
                              if (Repeat1__index % 2 == 0) {
                                rowColor = "reportEvenLine";
                              }else{	
                                rowColor = "reportOddLine";
                              }
                              %>
                              <tr class="<%=rowColor%>"> 
                                <td align="left" width="26%"> 
                                  <a href="CompanyEdit.aspx?<%= MM_keepBoth + MM_joinChar(MM_keepBoth) + "Id=" + rsCompany.Item("Id").ToString() %>"><%=rsCompany.Item("CompanyID")%></a></td>
                                <td align="left" width="74%"> 
                                  <a href="CompanyEdit.aspx?<%= MM_keepBoth + MM_joinChar(MM_keepBoth) + "Id=" + rsCompany.Item("Id").ToString() %>"><%=rsCompany.Item("CompanyName") %></a></td>
                              </tr>
                              <% 
                              Repeat1__index = Repeat1__index + 1;
                              Repeat1__numRows = Repeat1__numRows - 1 ;

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
                                  <% if ( MM_offset != 0 ){ %>
                                  <a href="<%=MM_moveFirst%>">First</a> 
                                  <% } // end MM_offset <> 0 %>
                                </td>
                                <td width="31%" align="center"> 
                                  <% if( MM_offset !=  0 ){ %>
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
                            <p>Records <%=(rsCompany_first)%> to <%=(rsCompany_last)%> of <%=(rsCompany_total)%> </p>
                            <!-- #EndEditable -->
                           </td>
                        </tr>
       
	                  </table>
</asp:Content>