<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="TaskRatesEdit.aspx.cs" Inherits="InterrailPPRS.Admin.TaskRatesEdit" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script type="text/javascript">
<!--

function goValidate()
{
  frm = document.form1;
  <% if ((sPayType == "HOURS") || (sPayType == "EITHER")) {%>
  if (ValidPositiveNumber(frm.HoursPayRate,    1, "Hours Pay Rate")       != true) {return false;}
  if (frm.HoursPayRate.value > 100.0)
  {
     alert("Hours Pay Rate must not exceed 100");
   frm.HoursPayRate.focus();
     frm.HoursPayRate.select();
   return false;
  }
  <% } %>

  <% if ((sPayType == "UNITS") || (sPayType == "EITHER")) { %>
  if (ValidPositiveNumber(frm.UnitsPayRate,    1, "Units Pay Rate")       != true) {return false;}
  if (frm.UnitsPayRate.value > 100.0)
  {
     alert("Units Pay Rate must not exceed 100");
   frm.UnitsPayRate.focus();
     frm.UnitsPayRate.select();
   return false;
  }
  <% } %>

  if (ValidDate(frm.EffectiveDate,        1, "EffectiveDate")  != true) {return false;}
  if (ValidDate(frm.ExpirationDate,       0, "ExpirationDate") != true) {return false;}

     var dateFrom = new Date(Date.parse(frm.EffectiveDate.value));
     var dateTo   = new Date(Date.parse(frm.ExpirationDate.value));

     if (dateFrom > dateTo)
     {
       alert(" 'Effective Date ' must be prior to 'Expiration Date'.");
       frm.EffectiveDate.focus();
       frm.EffectiveDate.select();
       return false;
     }


  frm.action = "<%=MM_editAction%>";
  frm.submit();
}
function goCancel()
{
  if (confirm("Are you sure you want to cancel?"))
  self.location = "<%=MM_editRedirectUrl%>";
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
                <form method="POST" action="<%=MM_editAction%>" name="form1">
                    <table align="center">
                    <tr valign="baseline">
                        <td nowrap colspan="2">
                        <div align="center" class="pageTitle">Task
                            Rate</div>
                        </td>
                    </tr>
                    <tr valign="baseline">
                        <td nowrap align="right">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr valign="baseline">
                        <td nowrap align="right"  class="required">Task:</td>
                        <td>
                        <%
                            while (! rsTasks.EOF){
                                rsTasks.Read();
                                    if (cStr(rsTasks.Item("Id")) == cStr(Request["TID"])) {
                                    Response.Write(Trim(rsTasks.Item("TaskDescription")));
                                    }
                            }
                                      
                            rsTasks.Requery();

                            %>
                        </td>
                    </tr>
                    <tr valign="baseline">
                        <td nowrap align="right" class="required">Employee:</td>
                        <td> 
						<select name="select2">
                            <%
                            while (! rs.EOF){
    
                                rs.Read();
                            %>
                            <option value="<%=(rsEmp.Item("ID")); %>" <% if ((rsEmp.Item("ID")) == (rs.Item("EmployeeId"))) { Response.Write("SELECTED"); }else{ Response.Write(""); }%> ><%=(rsEmp.Item("LastName")); %></option>
                            <%

                            }
  
                            rs.Requery();

                        %>
                        </select>
                    </td>
                    </tr>
                    <!--
                    <tr valign="baseline">
                        <td nowrap align="right">FaciltityTaskID:</td>
                        <td>
                        <input type="text" name="FaciltityTaskID" value="<%=(rs.Item("FaciltityTaskID")); %>" size="32">
                        </td>
                    </tr>
                    <tr valign="baseline">
                        <td nowrap align="right">EmployeeID:</td>
                        <td>
                        <input type="text" name="EmployeeID" value="<%=(rs.Item("EmployeeID"))%>" size="32">
                        </td>
                    </tr>
                    -->
                    <% if (sPayType != "UNITS") {%>
                    <tr valign="baseline">
                        <td nowrap align="right" class="required">Hours
                        Pay Rate:</td>
                        <td>
                        <input type="text" name="HoursPayRate" value="<%=(rs.Item("HoursPayRate")); %>" size="32" maxlength="6">
                        </td>
                    </tr>
                    <% if (sPayType != "EITHER") {%>
                    <input type="hidden" name="UnitsPayRate" value="0.0" size="32" maxlength="6">
                    <% } %>
                    <% } %>
                    <% if (sPayType != "HOURS") {%>
                    <tr valign="baseline">
                        <td nowrap align="right" class="required">Units
                        Pay Rate:</td>
                        <td>
                        <input type="text" name="UnitsPayRate" value="<%=(rs.Item("UnitsPayRate")); %>" size="32" maxlength="6">
                        </td>
                    </tr>
                    <% if (sPayType != "EITHER") {%>
                    <input type="hidden" name="HoursPayRate" value="0.0" size="32" maxlength="6">
                    <% } %>
                    <% } %>
                    <tr valign="baseline">
                        <td nowrap align="right" class="required">Effective Date:</td>
                        <td>
                        <input type="text" name="EffectiveDate"
                    <%
                        if (rs.Item("EffectiveDate") != "") {
                        Response.Write ("value='" + rs.Item("EffectiveDate") + "' ");
                    }else{
                        Response.Write(" value='" + cStr(Date+1) + "' ");
                    }
                        %>
                    size="32"  Maxlength="10">
                        </td>
                    </tr>
                    <tr valign="baseline">
                        <td nowrap align="right">Expiration Date:</td>
                        <td>
                        <input type="text" name="ExpirationDate" value="<%=rs.Item("ExpirationDate"); %>" size="32"  Maxlength="10">
                        </td>
                    </tr> 
                    <tr valign="baseline">
                        <td nowrap align="right" height="15">&nbsp;</td>
                        <td height="15">&nbsp;</td>
                    </tr>
                    <tr valign="baseline">
                        <td nowrap align="right" width="149">Last Modified:</td>
                        <td width="138"><%= rs.Item("LastModifiedOn"); %></td>
                    </tr>
                    <tr valign="baseline">
                        <td nowrap align="right" width="149">By:</td>
                        <td width="138"><%= rs.Item("LastModifiedBy"); %> </td>
                    </tr>
                    <td nowrap align="right">&nbsp;</td>
                    <td>
                        <input type="button" value="Save" onclick="goValidate();">
                        <input type="button" name="btnCancel" value="Cancel" onclick="goCancel();">
                    </td>
                    </tr>
                    </table>
                    <table align="center" border="1">
                    </table>
                    <input type="hidden" name="MM_update" value="true">
                    <input type="hidden" name="MM_recordId" value="<%= rs.Item("ID"); %>">
                    <input type="hidden" name="LastModifiedOn" value="<%=System.DateTime.Now.ToString(); %>" size="32">
                    <input type="hidden" name="LastModifiedBy" value="<%=Session["UserName"]; %>" size="32">
                    <input type="hidden" name="selTaskID" value="<%=Request["TID"]; %>">
                </form> 
                <p>&nbsp;</p>
                Fields in <span class="Required">RED</span> are required.
                <!-- #EndEditable -->
                </td>
            </tr>

        </table>
            <p>&nbsp;</p>
</asp:Content>