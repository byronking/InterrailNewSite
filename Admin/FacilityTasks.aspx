<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="FacilityTasks.aspx.cs" Inherits="InterrailPPRS.Admin.FacilityTasks" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script type="text/javascript">
<!--

    function bodyload() {
        OriginalFormCheckSumValue = CheckStringForForm(document.form1);
    }

    function bodyunload() {
        if (CheckStringForForm(document.form1) != OriginalFormCheckSumValue) {
            event.returnValue = "You have not saved your changes.";
        }
    }


    function goValidate() {
        frm = document.form1;

        OriginalFormCheckSumValue = CheckStringForForm(document.form1);

        frm.action = "<%=MM_editAction%>";
        frm.submit();
    }

    function goCancel() {
        try {
            self.location = "Facility.aspx";
        }
        catch (e) { }
    }

// -->
</script>
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
						    <%if (rs.EOF) {%>
							   No tasks available.   Please create tasks first. 
							<%}else{%>
                            <form name="form1" method="POST" action="<%=MM_editAction%>">
                              <table bborder="0" cellspacing="0" cellpadding="0" width="97%">
                                <tr> 
                                  <td colspan="2"> 
                                    <div align="center" class="pageTitle">Facility 
                                      Tasks For: &nbsp;<%=Request["FName"] %></div>
                                  </td>
                                </tr>
                                <tr> 
                                  <td width="20%">&nbsp;</td>
                                  <td width="80%">&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td width="20%">&nbsp;</td>
                                  <td width="80%"   class="cellTopBottomBorder">Task 
                                    Description - (Task Code)</td>
                                </tr>
                                <tr> 
                                  <td width="20%">&nbsp;</td>
                                  <td width="80%">&nbsp;</td>
                                </tr>
                                <% 
                                string rowColor = "";  
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
                                <tr> 
                                  <td width="20%" align="right"> 
                                    <input <%if ( rs.Item("TaskId") == rs.Item("Id")) { Response.Write("CHECKED"); }else{ Response.Write(""); }%> type="checkbox" name="checkbox" value="<%=rs.Item("Id") %>" >
                                  </td>
                                  <td width="80%" class="<%=rowColor%>" align="left"><%=(rs.Item("TaskDescription")) %> - (<%=Trim(rs.Item("TaskCode")) %>)</td>
                                </tr>
                                <% 
                                  Repeat1__index = Repeat1__index + 1;
                                  Repeat1__numRows = Repeat1__numRows - 1;
                                  }
                                %>
                                <%
                                    rs.Requery();
                                    rs.Read();
                                    
                                    %>
                                <tr> 
                                  <td width="20%">&nbsp;</td>
                                  <td width="80%">&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td width="20%" align="right">Last Modified: 
                                  </td>
                                  <td width="80%"> <%=rs.Item("LastModifiedOn") %></td>
                                </tr>
                                <tr> 
                                  <td width="20%" align="right">By: </td>
                                  <td width="80%"> <%=rs.Item("LastModifiedBy") %></td>
                                </tr>
                                <tr> 
                                  <td width="20%">&nbsp;</td>
                                  <td width="80%">&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td width="20%"> 
                                    <input type="hidden" name="MM_update" value="true">
                                    <input type="hidden" name="MM_recordId" value="<%=Request["ID"] %>">
                                  </td>
                                  <td width="80%"> 
                                    <input type="button" name="Submit" value="Save" onclick="goValidate();">
                                    <input type="button" name="btnCancel" value="Cancel" onclick="goCancel();">
                                  </td>
                                </tr>
                              </table>
                            </form>
							<%}%>
                            <!-- #EndEditable -->
                           </td>
                        </tr>
       
	                  </table>
                      <p>&nbsp;</p>
</asp:Content>