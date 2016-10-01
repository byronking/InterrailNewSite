<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="TasksEdit.aspx.cs" Inherits="InterrailPPRS.Admin.TasksEdit" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script type="text/javascript">
<!--

function bodyload()
{
  if ('<%=Request["ID"]%>' == "0")
    { document.form1.Active.checked = true;}
  <%if (rs.Item("Rebillable") != "") {%>
    setPayType('hoursonly')
  <%}else{%>
    setPayType('all')
  <%}%>
  OriginalFormCheckSumValue = CheckStringForForm(document.form1);
}

function bodyunload()
{
  if ( CheckStringForForm(document.form1) != OriginalFormCheckSumValue)
  {
    event.returnValue = "You have not saved your changes.";
  }
}

function doChangeRebillable()
{
  if (document.form1.rebillable.checked)
  {
    setPayType('hoursonly');
  }
  else
  {
	setPayType('all');
  }
}

function setPayType(type)
{
  var payType = document.form1.select2;
  if (type == 'hoursonly')
  {
	payType.options.length = 0;
	payType.options[0] = new Option('Hours', 'HOURS', false, false);
    document.all['rebilldiv'].style.visibility = 'visible';
    document.all['rebilldiv'].innerHTML  = '(Hours)';
    document.all['paytypediv'].style.visibility = 'hidden';
  }
  else
  {
	payType.options[0] = new Option('Either', 'EITHER', false, false);
	<%if (Trim(rs.Item("PayType")) == "EITHER") {%>
	  payType.options[0].selected = true;
    <%}%>
	payType.options[1] = new Option('Units',  'UNITS',  false, false);
	<%if (Trim(rs.Item("PayType")) == "UNITS") {%>
	  payType.options[1].selected = true;
    <%}%>
	payType.options[2] = new Option('Hours',  'HOURS',  false, false);
	<%if (Trim(rs.Item("PayType")) == "HOURS") {%>
	  payType.options[2].selected = true;
    <%}%>
	
    document.all['paytypediv'].style.visibility = 'visible';
    document.all['rebilldiv'].innerHTML  = 'Pay Type:';
    document.all['rebilldiv'].style.visibility = 'visible';
  }
}

function goValidate()
{
  frm = document.form1;
  if (ValidText(frm.TaskCode,                       1, "Task Code")            != true) {return false;}
  if (ValidText(frm.TaskDescription,                1, "Description")          != true) {return false;}

  OriginalFormCheckSumValue = CheckStringForForm(document.form1);

  frm.action = "<%=MM_editAction%>";
  frm.submit();
}
function goCancel()
{
  try
  {
    self.location = "Tasks.aspx";
  }
  catch(e) {}
}
// -->
</script>
</asp:Content>

<asp:Content ID="leftNavigation" ContentPlaceHolderID="Navigation" runat="server">
  <table width="81%" border="0"  valign="top">
        <% if ( Session["UserType"].ToString().IndexOf("Super") >=1) { %>
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
            <form ACTION="<%=MM_editAction%>" METHOD="POST" name="form1">
                <table align="center">
                <tr valign="baseline"> 
                    <td nowrap align="right" colspan="2"> 
                    <div align="center"  class="pageTitle">Task</div>
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" class="required">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <% if (Request["ID"] == "0") { %>
                <% } %>
                <tr valign="baseline"> 
                    <td nowrap align="right" class="required">Code:</td>
                    <td> 
                    <input type="text" name="TaskCode" value="<%=Trim(rs.Item("TaskCode")) %>" size="32" maxlength="10">
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" class="required">Description:</td>
                    <td> 
                    <input type="text" name="TaskDescription" value="<%=Trim(rs.Item("TaskDescription")) %>" size="32" maxlength="30">
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" class="required">GL 
                    Account Number:</td>
                    <td> 
                    <input type="text" name="GLAcctNumber" value="<%=Trim(rs.Item("GLAcctNumber"))%>" size="32" maxlength="4">
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" class="required">Rebillable:</td>
                    <td> 
                    <input <%if ( rs.Item("Rebillable") == "True" ) { Response.Write("CHECKED"); }else{ Response.Write(""); }%> type="checkbox" name="rebillable" value="checkbox" onclick="doChangeRebillable();">
                    </td>
                </tr>
                <tr valign="top"> 
                    <td nowrap align="right">
                    <div id="rebilldiv" style="{visibility: visible;}">Pay 
                        Type:</div>
                    </td>
                    <td  valign="bottom"> 
                    <div id="paytypediv" style="{visibility: hidden;}"> 
                        <select name="select2">
                        <option value=""> 
                        <option> 
                        </select>
                    </div>
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right">Active:</td>
                    <td> 
                    <input <%if (rs.Item("Active") != "0") { Response.Write("CHECKED"); }else{ Response.Write(""); }%> type="checkbox" name="Active" value="1" />
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right">Last Modified:</td>
                    <td> <%=rs.Item("LastModifiedOn") %> </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right">By:</td>
                    <td> <%=rs.Item("LastModifiedBy") %> </td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right">&nbsp;</td>
                    <td> 
                    <input type="button" value="Save" onclick="goValidate();" />
                    <input type="button" name="btnCancel" value="Cancel" onclick="goCancel();" />
                    </td>
                </tr>
                </table>
                <input type="hidden" name="MM_recordId" value="<%= rs.Item("Id") %>" />
                <input type="hidden" name="LastModifiedOn" value="<%=System.DateTime.Now.ToString() %>" size="32" />
                <input type="hidden" name="LastModifiedBy" value="<%=Session["UserName"] %>" size="32" />
                <input type="hidden" name="MM_update" value="true">
            </form>
            <p>&nbsp;</p>
            Fields in <span class="Required">RED</span> are required. 
            <!-- #EndEditable -->
            </td>
        </tr>
       
	    </table>
        <p>&nbsp;</p>
</asp:Content>