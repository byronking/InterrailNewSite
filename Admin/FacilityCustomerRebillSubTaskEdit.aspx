<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="FacilityCustomerRebillSubTaskEdit.aspx.cs" Inherits="InterrailPPRS.Admin.FacilityCustomerRebillSubTaskEdit" %>



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
  if (ValidText(frm.Description,  1, "Description") != true) {return false;}
  
    OriginalFormCheckSumValue = CheckStringForForm(document.form1);

  frm.action = "<%=MM_editAction%>";
  frm.submit();
}
function goCancel()
{
	try 
	  {
	    self.location = "FacilityCustomerRebillSubTask.aspx";
	  }
	catch(e)
	  {};
}

  var strContactName = new Array();
  var strContactAddress1 = new Array();
  var strContactAddress2 = new Array();
  var strContactAddress3 = new Array();
  
<%
while (! rsCustomer.EOF){

  rsCustomer.Read();

  int i = rsCustomer.Item("Id");
  Response.Write("   strContactName[" + i + "] = '" + Trim(rsCustomer.Item("ContactName")) + "';"  );
  Response.Write("   strContactAddress1[" + i + "] = '" + Trim(rsCustomer.Item("ContactAddress1")) + "';"  );
  Response.Write("   strContactAddress2[" + i + "] = '" + Trim(rsCustomer.Item("ContactAddress2")) + "';"  );
  Response.Write("   strContactAddress3[" + i + "] = '" + Trim(rsCustomer.Item("ContactAddress3")) + "';"  );
  

}

rsCustomer.Requery();

%>


function CustomerChange()
{
    frm = document.form1;
    i = frm.Customer[frm.Customer.selectedIndex].value;
	if (i > -1)
	  {
	    document.all["divContactName"].innerHTML = strContactName[i];
	    document.all["divAddress1"].innerHTML = strContactAddress1[i];
	    document.all["divAddress2"].innerHTML = strContactAddress2[i];
	    document.all["divAddress3"].innerHTML = strContactAddress3[i];
	  }
}

function bodyload()
{
  if ('<% =Request["ID"]; %>' == "0")
    { document.form1.checkbox.checked = true;}
  
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
        <%if ( Session["UserType"].ToString().IndexOf("Super") >=1 ) { %>
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
                          <td> <!-- #BeginEditable "MainBody" --> 
                            <form method="POST" ACTION="<%=MM_editAction%>" name="form1">
                              <table align="center">
                                <tr valign="baseline"> 
                                  <td nowrap align="right" colspan="2"> 
                                    <div align="center" class="pageTitle">Rebill 
                                      Task</div>
                                  </td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right" height="13">&nbsp;</td>
                                  <td height="13">&nbsp;</td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right" class="required" height="24">Description:</td>
                                  <td height="24"> 
                                    <input type="text" name="Description" value="<%= Trim((rsRebill.Item("Description"))); %>" size="30" maxlength="30" />
                                  </td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right" class="required">Task:</td>
                                  <td> 
                                    <select name="selTasks" style="width:170px">
                                      <%
                                        while (! rsTasks.EOF){
                                            rsTasks.Read();
                                        %>
                                      <option value="<%=(rsTasks.Item("Id"))%>" <% if (cStr(rsTasks.Item("Id")) = cStr(rsRebill.Item("TaskID"))) { Response.Write("SELECTED"); }else{ Response.Write(""); }%> ><%=(rsTasks.Item("TaskDescription")); %> (<%=Trim(rsTasks.Item("TaskCode"));%>)</option>
                                      <%
                                      
                                        }
                                        
                                        rsTasks.Requery();
                                        
                                    %>
                                    </select>
                                  </td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right" class="required">Customer:</td>
                                  <td> 
                                    <select name="Customer" style="width:170px" onchange="CustomerChange();">
                                      <%
                                        while (! rsCustomer.EOF){
                                            
                                            rsCustomer.Read();
    
                                        %>
                                      <option value="<%=(rsCustomer.Item("Id")); %>" <%if (cStr(rsCustomer.Item("Id")) == cStr((rsRebill.Item("FacilityCustomerId")))) { Response.Write("SELECTED"); }else{ Response.Write(""); }%> ><%=(rsCustomer.Item("CustomerName")); %></option>
                                      <%

                                          }
                                    rsCustomer.Requery();

                                    %>
                                    </select>
                                  </td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right">Contact:</td>
                                  <td> 
                                    <div id="divContactName" align="left"></div>
                                  </td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right">Address:</td>
                                  <td> 
                                    <div id="divAddress1" align="left"></div>
                                  </td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right">&nbsp;</td>
                                  <td> 
                                    <div id="divAddress2" align="left"></div>
                                  </td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right">&nbsp;</td>
                                  <td> 
                                    <div id="divAddress3" align="left"></div>
                                  </td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right">Active</td>
                                  <td> 
                                    <input <% if (rsRebill.Item("Active") != "0") { Response.Write("CHECKED"); }else{ Response.Write("");}%>  type="checkbox" name="checkbox" value="checkbox" />
                                  </td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right" width="149">&nbsp;</td>
                                  <td width="138">&nbsp;</td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right" width="149">Last Modified:</td>
                                  <td width="138"><%=(rsRebill.Item("LastModifiedOn")); %></td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right" width="149">By:</td>
                                  <td width="138"><%=(rsRebill.Item("LastModifiedBy")); %> </td>
                                </tr>
                                <tr valign="baseline"> 
                                  <td nowrap align="right" width="149">&nbsp;</td>
                                  <td width="138"> 
                                    <input type="button" value="Save" onclick="goValidate();" />
                                    <input type="button" name="btnCancel" value="Cancel" onclick="goCancel();" />
                                  </td>
                                </tr>
                              </table>
                              <input type="hidden" name="MM_update" value="true">
                              <input type="hidden" name="MM_recordId" value="<%= rsRebill.Item("Id"); %>" />
                              <input type="hidden" name="LastModifiedOn" value="<%=System.DateTime.Now.ToString()%>" />
                              <input type="hidden" name="LastModifiedBy" value="<%= Session["UserName"]; %>" />
                            </form>
                            <p>&nbsp;</p>
                            Fields in <span class="Required">RED</span> are required. 
                            <!-- #EndEditable --> </td>
                        </tr>
                      </table>
                      <p>&nbsp;</p>
</asp:Content>