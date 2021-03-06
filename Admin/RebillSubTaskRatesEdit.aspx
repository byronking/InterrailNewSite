﻿<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="RebillSubTaskRatesEdit.aspx.cs" Inherits="InterrailPPRS.Admin.RebillSubTaskRatesEdit" %>



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
  var sRate = frm.Rate.value;
  while (sRate.charAt(0) == '$' || sRate.charAt(0) == ' ') {
    sRate = sRate.substr(1);
  }
  frm.hdnRate.value = sRate;
  if (ValidNumber(frm.hdnRate,               1, "Rate")                   != true) {return false;}
  if (ValidText(frm.RateType,             1, "Rate Type Description")  != true) {return false;}
  if (ValidDate(frm.EffectiveDate,        1, "EffectiveDate")          != true) {return false;}
  if (ValidDate(frm.ExpirationDate,       0, "ExpirationDate")         != true) {return false;}
  if (frm.ExpirationDate.value != '') {
    d1 = new Date(frm.EffectiveDate.value);
    d2 = new Date(frm.ExpirationDate.value);
    if (d2 < d1) {
      alert('ExpirationDate cannot be less than EffectiveDate');
      return false;
    }
  }

  OriginalFormCheckSumValue = CheckStringForForm(document.form1);

  frm.action = "<%=MM_editAction%>";
  frm.submit();
}
function goCancel()
{
  try
   {
     self.location = "RebillSubTaskRates.aspx?Id=<%=Request["RBTask"]%>";
   }
  catch(e)
   {};
}

function bodyload()
{
  OriginalFormCheckSumValue = CheckStringForForm(document.form1);
  CustomerChange();
}

function bodyunload()
{
  if ( CheckStringForForm(document.form1) != OriginalFormCheckSumValue)
      {
        event.returnValue = "You have not saved your changes.";
      }
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
                    <td nowrap align="right" colspan="2">
                    <div align="center" class="pageTitle">Rebill Task Rate</div>
                    </td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" class="required">Effective Date:</td>
                    <td>
                    <input type="text" name="EffectiveDate" value="<%= Trim((rsRebill.Item("EffectiveDate"))) %>" size="32">
                    </td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right">Rebill Type:</td>

                    <input type=hidden name=txtHoursOrUnits value="<%=sHoursOrUnits%>">
                    <% if (sHoursOrUnits == "H") { %>
                        <td>Hourly</td>
                    <% }else{ %>
                        <td>Units</td>
                    <% } %>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" class="required">Rate:</td>
                    <td>
                    <input type="hidden" name="hdnRate" />
                    <input type="text" name="Rate" value="<%= FormatNumber((rsRebill.Item("RebillRate")), 2) %>" size="32" maxlength="10">
                    </td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" class="required">Rate Type Description:</td>
                    <td>
                    <input type="text" name="RateType" value="<%= Trim((rsRebill.Item("RebillType"))) %>" size="32" maxlength="50">
                    </td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right">Expiration Date: </td>
                    <td>
                    <input type="text" name="ExpirationDate" value="<%= Trim((rsRebill.Item("ExpirationDate"))) %>" size="32" maxlength="30">
                    </td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" width="149">&nbsp;</td>
                    <td width="138">&nbsp;</td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" width="149">Last Modified:</td>
                    <td width="138"><%=(rsRebill.Item("LastModifiedOn"))%></td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" width="149" height="15">By:</td>
                    <td width="138" height="15"><%=(rsRebill.Item("LastModifiedBy"))%> </td>
                </tr>
                <tr valign="baseline">
                    <td nowrap align="right" width="149">&nbsp;</td>
                    <td width="138">
                    <input type="button" value="Save" onclick="goValidate();">
                    <input type="button" name="btnCancel" value="Cancel" onclick="goCancel();">
                    </td>
                </tr>
                </table>
                <input type="hidden" name="MM_update" value="true">
                <input type="hidden" name="MM_recordId" value="<%= rsRebill.Item("Id") %>">
                <input type="hidden" name="LastModifiedOn" value="<%=System.DateTime.Now.ToString()%>">
                <input type="hidden" name="LastModifiedBy" value="<%= Session["UserName"] %>">
                <%if (Request["ID"] == "0") { %>
                <input type="hidden" name="RebillSubTaskId" value="<%=Request["RBTask"]%>">
                <% }else{ %>
                <input type="hidden" name="RebillSubTaskId" value="<%= rsRebill.Item("RebillSubTasksId") %>">
                <% } %>
            </form>
            <p>&nbsp;</p>
            Fields in <span class="Required">RED</span> are required.
            <!-- #EndEditable -->
            </td>
        </tr>
       
	    </table>
        <p>&nbsp;</p>
</asp:Content>