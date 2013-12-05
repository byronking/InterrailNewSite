<%@ Page Language="C#" MasterPageFile="~/Payroll/Payroll.Master" AutoEventWireup="true" CodeBehind="TaskWorkedEdit.aspx.cs" Inherits="InterrailPPRS.Payroll.TaskWorkedEdit" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script type="text/javascript" >
<!--

function bodyload()
{
        
 OriginalFormCheckSumValue = CheckStringForForm(document.form1);
}

function bodyunload()
{
  if ( CheckStringForForm(document.form1) != OriginalFormCheckSumValue)
  {
    event.returnValue = "You have not saved your changes.";
  }
}

function goValidate()
{
  frm = document.form1;
  if (ValidDate(frm.dateworked,          1, "Date Worked") != true) {return false;}
  if (ValidPositiveNumber(frm.txtHours,  1, "Hours")       != true) {return false;}

<% if(! Rebill ){ %>
  var taskID = frm.selTask.value;
  var arTasks = taskID.split(",");
  frm.TaskID.value      = arTasks[0];
  frm.OtherTaskID.value = arTasks[1];
<% } %>

  
  OriginalFormCheckSumValue = CheckStringForForm(document.form1);

  frm.action = "TaskWorkedEdit.aspx";
  frm.submit();
}

function goCancel()
{
  try
  {
<%
    string cancelUrl;
    if (Request["ReturnTo"] != "") {
      cancelUrl = Request["ReturnTo"];
    }else{
      cancelUrl = "TaskWorkedEdit.aspx";
    }

%>
    self.location = "<%=cancelUrl%>";
  }
  catch(e) {}
}

function goDelete()
{
  if (confirm('Are you sure you want to delete this record?\nPress OK to confirm the delete.'))
  {
    frm = document.form1;

    <% if (! Rebill ){ %>
      var taskID = frm.selTask.value;
      var arTasks = taskID.split(",");
      frm.TaskID.value      = arTasks[0];
      frm.OtherTaskID.value = arTasks[1];
      var theTaskID = arTasks[0];
      var theRebillDetailID = '';
    <% } %>
    <% if (Rebill){ %>
      var theTaskID = '<%=rbTaskID%>';
      var theRebillDetailID = '<%=Request["RebillDetailID"]%>';
    <% } %>

    OriginalFormCheckSumValue = CheckStringForForm(document.form1);

    frm.action = 'TaskWorkedEdit.aspx?ID=<%=Request["ID"]%>&Delete=YES&dateworked=<%=(rs.Item("WorkDate"))%>&TaskID=' + theTaskID + '&selShift=<%=rs.Item("ShiftID")%>&RebillDetailID=' + theRebillDetailID ;

    frm.submit();
  }
}

// -->
</script>
</asp:Content>

<asp:Content ID="leftNavigation" ContentPlaceHolderID="Navigation" runat="server">
<table width="81%" border="0"  valign="top">
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
                  <% if (CheckSecurity()) { %>
                  <tr> 
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
                    <td width="79%"><a href="Payroll.aspx">Daily 
                      Payroll</a></td>
                  </tr>
                  <%}%>
                  <% if (CheckSecurity()) { %>
                  <tr> 
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
                    <td width="79%"><a href="calc.aspx">Calculate</a></td>
                  </tr>
                  <%}%>
                  <% if (CheckSecurity()) { %>
                  <tr> 
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
                    <td width="79%"> <a href="ApprovePayrollData.aspx">Approval</a></td>
                  </tr>
                  <%}%>
                 <% if (CheckSuperAdminSecurity()) { %>
                  <tr> 
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
                    <td width="79%"> <a href="CSVSelect.aspx">Create ADP File</a></td>
                  </tr>
                  <% }%>

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
        <td height="40"> <!-- #BeginEditable "MainBody" --> 
        <form method="POST" action="<%=MM_editAction%>" name="form1">
            <table align="center" bgcolor="#FFFFFF">
            <tr valign="baseline"> 
                <td nowrap align="right" colspan="2"> 
                <div align="center" class="pageTitle">Task Worked </div>
                </td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right" width="68">&nbsp;</td>
                <td width="258">&nbsp;</td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right" width="68" class="required" height="8">Date:</td>
                <td width="258" height="8"> 
                <% if (Rebill ){%>
                    <%=Request["WorkDate"]%>
                    <input type="hidden" name="dateworked" value="<%=Request["WorkDate"]%>" />
                <% }else{ %>
                    <% if (Request["ID"] != "0") { %>
                    <input value="<%=rs.GetDateTime("WorkDate").ToString("M/d/yyyy")%>" type="text" name="dateworked" size=10 maxlength=10>
                    <% } else {%>
                    <input value="<%=DateTime.Today.ToString("M/d/yyyy")%>" type="text" name="dateworked" size=10 maxlength=10>
                    <% }%>
                <% } %>
                </td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right" width="68" class="required">Shift:</td>
                <td width="258"> 
                <% if ( Rebill ){ %>
                    <%=Request["Shift"]%>
                    <input type="hidden" name="selShift" value="<%=Request["Shift"]%>">
                <% }else{ %>
                                   
                <select name="selShift" style="width:65px">
                    <%
                    while (rsShifts.Read()){
                    %>
                    <option value="<%=rsShifts.Item("ID")%>" <%if ( Trim(rsShifts.Item("ID")) == Trim(rs.Item("ShiftID")) ) { Response.Write("SELECTED"); }else{ Response.Write("");}%>><% =rsShifts.Item("Shift") %></option>
                    <%
                        }

                    rsShifts.Requery();

                %>
                </select>
                <% } %>
                </td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right" width="68" class="required">Employee:</td>
                <td width="258"> 
                <select name="selEmp" style="width:170px">
                    <%
                    while (rsEmp.Read()){
                        %>
                        <option value="<%=rsEmp.Item("Id")%>" <% if (cStr(rsEmp.Item("Id")) == cStr(rs.Item("EmployeeId"))) { Response.Write("SELECTED"); }else{ Response.Write(""); }%> ><%=rsEmp.Item("LastName")%>, <%=rsEmp.Item("FirstName")%> (<% if ( Len(rsEmp.Item("EmployeeNumber")) > 0 ){ Response.Write(rsEmp.Item("EmployeeNumber")); }else{ Response.Write(rsEmp.Item("TempNumber")); } %>)</option>
                        <%
                    }
                    rsEmp.Requery();

                    %>
                </select>
                </td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right" width="68" class="required">Task:</td>
                <td width="258"> 
                <% if (Rebill ){ %>
                    <%=rbDesc%>
                <input type="hidden" name="selTask" value="<%=rbTaskID%>" />
                <input type="hidden" name="TaskID"  value="<%=rbTaskID%>" />
                <input type="hidden" name="SubTaskID" value="<%=rbSubTaskID%>" />
                <input type="hidden" name="OtherTaskID" value="0" />
                <input type="hidden" name="RebillDetailID" value="<%=Request["RebillDetailID"]%>" />
                <% }else{ %>
                    <% if (Request["ID"] == "0") { %>
                        <select name="selTask" style="width:170px">
                    <%
                    while (rsTaskList.Read()){
                    %>
                    <option value="<%=rsTaskList.Item("TaskID") %>,<%=rsTaskList.Item("OtherTaskID") %>" <% if ( Trim(cStr(rsTaskList.Item("TaskID"))) + "," + Trim(cStr(rsTaskList.Item("OtherTaskID"))) == Trim(cStr(rs.Item("TaskID"))) + "," + Trim(cStr(rs.Item("OtherTaskID")))) { Response.Write("SELECTED"); }else{ Response.Write(""); }%> ><%=rsTaskList.Item("TaskDescription")%></option>
                    <%
                    }

                    rsTaskList.Requery();

                %>
                </select>
                    <%} else {%>
                    <%
                    while(rsTaskList.Read()){
                    if ( Trim(cStr(rsTaskList.Item("TaskID"))) + "," + Trim(cStr(rsTaskList.Item("OtherTaskID"))) == Trim(cStr(rs.Item("TaskID")))  + "," + Trim(cStr(rs.Item("OtherTaskID"))))  { 
                            %>
                            <%=rsTaskList.Item("TaskDescription")%>
                            <input type="hidden" name="selTask"      value="<%=rsTaskList.Item("TaskID")%>,<%=rsTaskList.Item("OtherTaskID")%>" />
                            <%
                    }      
                    }


                        rsTaskList.Requery();

                    %>
                                      
                    <%}%>
                <input type="hidden" name="TaskID" value="" />
                <input type="hidden" name="OtherTaskID" value="" />
                <input type="hidden" name="RebillDetailID" value="0" />
                <% } %> 
                </td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right" width="68" height="24" class="required">Hours</td>
                <td width="258" height="24"> 
                <input value="<%= rs.Item("HoursWorked")%>" type="text" name="txtHours" size=10 maxlength=10>
                </td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right" width="68" height="29">UPM</td>
                <td width="258" height="29"><%=rs.Item("UPM")%> </td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right" valign="top">Out Of 
                Town </td>
                <td nowrap align="left"> 
                <select name="selOutofTown">
                    <option value="N" <%if (rs.Item("OutofTownType") == "N") { Response.Write("SELECTED") ;}else{ Response.Write("") ;}%>>No</option>
                    <option value="O" <%if (rs.Item("OutofTownType") == "O") { Response.Write("SELECTED") ;}else{ Response.Write("") ;}%>>Out 
                    Of Town</option>
                    <option value="D" <%if (rs.Item("OutofTownType") == "D") { Response.Write("SELECTED") ;}else{ Response.Write("") ;}%>>Daily</option>
                </select>
                </td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right" valign="top">Notes 
                </td>
                <td nowrap align="left"> 
                <textarea name="txtnotes" cols="40" rows="5"><%=rs.Item("Notes")%></textarea>
                </td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right" width="68">&nbsp;</td>
                <td width="258">&nbsp;</td>
            </tr>
            <% if ( rs.Item("RebillDetailID") != "0" ) { %>
            <tr valign="baseline"> 
                <td nowrap align="right" width="68">&nbsp;</td>
                <td width="258"><a href="../rebilling/rebilledit.aspx?id=<%=rs.Item("RebillDetailID")%>">Go 
                to Rebill Detail</a></td>
            </tr>
            <% }  %>
            <tr> 
                <td nowrap align="right" width="68">&nbsp;</td>
                <td width="258">&nbsp;</td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right" width="68">Last Modified:</td>
                <td width="258"><%= rs.Item("LastModifiedOn") %></td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right" width="68">By:</td>
                <td width="258"><%= rs.Item("LastModifiedBy") %> </td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right" width="68">&nbsp;</td>
                <td width="258"> 
                <%
                    if (Request["ID"] != "0")  { 
                        strStatus = getPayRollStatus(cStr(rs.Item("WorkDate")), cStr(Session["FacilityID"]));
                    }else{
                        strStatus = "OPEN";
                    }
                    if ((Request["ID"] != "0") && ((strStatus == "OPEN") || (strStatus == "PAYROLL"))) { %>
                <input type="button" value="Delete" onClick="goDelete();" name="btnDelete">
                <% }%>
                <% if ((Request["ID"] == "0") || ((strStatus == "OPEN") || (strStatus == "PAYROLL")) ) { %>
                <input type="button" value="Save" onClick="goValidate();">
                <% }%>
                <input type="button" name="btnCancel" value="Cancel" onClick="goCancel();">
                </td>
                </tr>
                <tr>
                <% if (strStatus == "PAYROLL" ) { 
                    strStatus = "OPEN";
                }
                %>
                <td nowrap align="right" width="68">This record is <%=strStatus%></td>
            </tr>
            </table>
            <input type="hidden" name="MM_update" value="true">
            <input type="hidden" name="MM_recordId" value="<%= rs.Item("Id") %>">
            <input type="hidden" name="LastModifiedOn" value="<%= System.DateTime.Now %>">
            <input type="hidden" name="LastModifiedBy" value="<%= Session["UserName"] %>">
            <input type="hidden" name="hFacilityID" value="<%= Session["FacilityID"] %>">
            <% if ( Rebill ) { %>
            <input type="hidden" name="ReturnTo" value="<%=rbReturnTo%>">
            <input type="hidden" name="Type" value="Rebill">
            <% }else{ %>
            <input type="hidden" name="ReturnTo" value="<%=Request["ReturnTo"]%>">
            <% } %>
        </form>
        <p>&nbsp;</p>
        Fields in <span class="Required">RED</span> are required. 
        <!-- #EndEditable --> </td>
    </tr>
       
    </table>
</asp:Content>