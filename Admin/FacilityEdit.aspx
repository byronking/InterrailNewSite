<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="FacilityEdit.aspx.cs" Inherits="InterrailPPRS.Admin.FacilityEdit" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script type="text/javascript">
<!--
  
    function bodyload() {
        if ('<%=Request["ID"]%>' == "0")
        { document.form1.checkbox.checked = true; }


        OriginalFormCheckSumValue = CheckStringForForm(document.form1);
    }

    function bodyunload() {
        if (CheckStringForForm(document.form1) != OriginalFormCheckSumValue) {
            event.returnValue = "You have not saved your changes.";
        }
    }

    function goValidate() {
        frm = document.form1;

        if (ValidPositiveNumber(frm.FacilityNumber, 1, "Facility Number") != true) { return false; }
        if (ValidText(frm.AlphaCode, 1, "Alpha Code") != true) { return false; }
        if (ValidText(frm.Name, 1, "Facility Name") != true) { return false; }

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
                            <% if (rsTasks.EOF) { %>
                            No tasks of type UNITS or EITHER available. Each facility
                            must be assigned a default task.<br>
                            Please create a new task first.
                            <% }else{ %>
                            <form method="POST" action="<%=MM_editAction%>" name="form1">
                              <table align="center">
                                <tr valign="baseline">
                                  <td nowrap align="right" colspan="2">
                                    <div align="center" class="pageTitle">Facility</div>
                                  </td>
                                </tr>
                                <tr valign="baseline">
                                  <td nowrap align="right">&nbsp;</td>
                                  <td>&nbsp;</td>
                                </tr>
                                <tr valign="baseline">
                                  <td nowrap align="right" class="required">Facility Number:</td>
                                  <td>
                                    <input type="text" name="FacilityNumber" value="<%=Trim(rsFac.Item("FacilityNumber"))%>" size="32">
                                  </td>
                                </tr>
                                <tr valign="baseline">
                                  <td nowrap align="right" class="required">Alpha Code:</td>
                                  <td>
                                    <input type="text" name="AlphaCode" value="<%=Trim(rsFac.Item("AlphaCode"))%>" size="32" maxlength="10">
                                  </td>
                                </tr>
                                <tr valign="baseline">
                                  <td nowrap align="right" class="required">Name:</td>
                                  <td>
                                    <input type="text" name="Name" value="<%=Trim(rsFac.Item("Name"))%>" size="32" maxlength="30">
                                  </td>
                                </tr>
                                <tr valign="baseline">
                                  <td nowrap align="right" rowspan="3">Address:</td>
                                  <td>
                                    <input value="<%=(rsFac.Item("Address1"))%>" type="text" name="Address1"  size="32" maxlength="250">
                                  </td>
                                </tr>
                                <tr valign="baseline">
                                  <td>
                                    <input value="<%=(rsFac.Item("Address2"))%>" type="text" name="Address2" size="32" maxlength="250">
                                  </td>
                                </tr>
                                <tr valign="baseline">
                                  <td>
                                    <input value="<%=(rsFac.Item("Address3"))%>" type="text" name="Address3" size="32" maxlength="250">
                                  </td>
                                </tr>

                                <tr valign="baseline">
                                  <td nowrap align="right" class='required'>Default Task:</td>
                                  <td>
                                    <select name="select" style="width:180px">
                                      <%
                                        while (! rsTasks.EOF){
                                            rsTasks.Read();
                                        %>
                                      <option value="<%=rsTasks.Item("Id") %>" <% if (cStr(rsTasks.Item("Id")) == cStr(rsFac.Item("DefaultTaskID"))) { Response.Write("SELECTED"); }else{ Response.Write(""); } %> ><%=rsTasks.Item("TaskDescription") %></option>
                                      <%
                                     }
                                      rsTasks.Requery();

                                    %>
                                    </select>
                                  </td>
                                </tr>
                                <tr valign="baseline">
                                  <td nowrap align="right">Default Shift ID:</td>
                                  <td>
                                    <input type="text" name="DefaultShiftID" value="<%=Trim(rsFac.Item("DefaultShiftID")) %>" size="32" maxlength="10" />
                                  </td>
                                </tr>
                                <tr valign="baseline">
                                  <td nowrap align="right" class="required">Region:</td>
                                  <td>
                                    <select name="Region" style="width:180px">
                                      <%
                                        while (! rsRegion.EOF){
                                            rsRegion.Read();
                                        %>
                                      <option value="<%=(rsRegion.Item("ID"))%>" <%if (cStr(rsRegion.Item("ID")) == cStr(rsFac.Item("RegionID"))) { Response.Write("SELECTED"); }else{ Response.Write(""); }%> ><%=(rsRegion.Item("RegionDescription")) %></option>
                                      <%
                                          }

                                      rsRegion.Requery();

                                    %>
                                    </select>
                                  </td>
                                </tr>
                                <tr valign="baseline">
                                  <td nowrap align="right" class="required">Overtime Calc Basis:</td>
                                  <td>
                                    <select name="OTCalcBasis" style="width:180px">
                                      <%
                                        while (! rsOTBasis.EOF){
                                            rsOTBasis.Read();
                                        %>
                                      <option value="<%=(rsOTBasis.Item("OvertimeCalcBasis"))%>" <%if (cStr(rsOTBasis.Item("OvertimeCalcBasis")) == cStr(rsFac.Item("OvertimeCalcBasis"))) { Response.Write("SELECTED"); }else{ Response.Write(""); }%> ><%=(rsOTBasis.Item("Description")) %></option>
                                      <%
                                          }
                                      rsOTBasis.Requery();

                                    %>
                                    </select>
                                  </td>
                                </tr>
                                <tr valign="baseline">
                                  <td nowrap align="right">GL Cost Center:</td>
                                  <td>
                                    <input type="text" name="GLCostCenter" value="<%=Trim(rsFac.Item("GLCostCenter")) %>" size="32" maxlength="3">
                                  </td>
                                </tr>
                                <tr valign="baseline">
                                  <td nowrap align="right" class="required">Company:</td>
                                  <td>
                                    <select name="Company" style="width:180px">
                                      <%
                                        while (! rsCompany.EOF){
                                            rsCompany.Read();
                                        %>
                                      <option value="<%=(rsCompany.Item("Id")) %>" <%if ((rsCompany.Item("Id")) == (rsFac.Item("IRGCompanyId"))) { Response.Write("SELECTED"); }else{ Response.Write(""); }%> ><%=(rsCompany.Item("CompanyName")) %></option>
                                      <%
                                          }

                                      rsCompany.Requery();

                                    %>
                                    </select>
                                  </td>
                                </tr>
                                <tr valign="baseline">
                                  <td nowrap align="right">Active</td>
                                  <td>
                                    <input <% if ( rsFac.Item("Active") != "0" ) { Response.Write("CHECKED"); }else{ Response.Write(""); }%>  type="checkbox" name="checkbox" value="checkbox" >
                                  </td>
                                </tr>
                                <tr valign="baseline">
                                  <td nowrap align="right" width="149">&nbsp;</td>
                                  <td width="138">&nbsp;</td>
                                </tr>
                                <tr valign="baseline">
                                  <td nowrap align="right" width="149">Last Modified:</td>
                                  <td width="138"><%= (rsFac.Item("LastModifiedOn")) %></td>
                                </tr>
                                <tr valign="baseline">
                                  <td nowrap align="right" width="149">By:</td>
                                  <td width="138"><%= (rsFac.Item("LastModifiedBy")) %> </td>
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
                              <input type="hidden" name="MM_recordId" value="<%= rsFac.Item("Id") %>">
                              <input type="hidden" name="LastModifiedOn" value="<%=System.DateTime.Now.ToString() %>">
                              <input type="hidden" name="LastModifiedBy" value="<%= Session["UserName"] %>">
                         <p>&nbsp;</p>
                            Fields in <span class="Required">RED</span> are required.
                            <%}%>
                            <!-- #EndEditable -->
                           </td>
                        </tr>

                          </table>
</asp:Content>