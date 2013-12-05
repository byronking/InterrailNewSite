<%@ Page Language="C#" MasterPageFile="~/Payroll/Payroll.Master" AutoEventWireup="true" CodeBehind="ApprovePayrollDataEdit.aspx.cs" Inherits="InterrailPPRS.Payroll.ApprovePayrollDataEdit" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

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
        <form name="form1" method="post" action="">
            <table align="center" width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr> 
                <td colspan="4" class="pageTitle" align="center">Payroll 
                Data</td>
            </tr>
            <tr> 
                <td colspan="4"> 
                <div align="center" class="pageTitle"><%=Session["FacilityName"]%></div>
                </td>
            </tr>
            <tr> 
                <td width="20%">&nbsp;</td>
                <td width="30%">&nbsp;</td>
                <td width="20%">&nbsp;</td>
                <td width="30%">&nbsp;</td>
            </tr>
            <tr> 
                <td width="20%">&nbsp;</td>
                <td align="right" width="30%">&nbsp;</td>
                <td align="right"  width="20%" class='lblColor'>Approval 
                Status:&nbsp;&nbsp;</td>
                <td width="30%"> 
                <div  class="required"><B><%=sStatus%></B></div>
                </td>
            </tr>
            <tr> 
                <td width="20%">&nbsp;</td>
                <td width="30%">&nbsp;</td>
                <td width="20%">&nbsp;</td>
                <td width="30%">&nbsp;</td>
            </tr>
            <tr> 
                <td width="20%" valign="top" align="right" class='lblColor'> 
                Date:&nbsp;&nbsp;</td>
                <td width="30%"><%= rs.Item("WorkDate"); %></td>
                <td width="20%" align="right" class='lblColor'>&nbsp;</td>
                <td width="30%">&nbsp;</td>
            </tr>
            <tr> 
                <td width="20%" valign="top" align="right" class='lblColor'>Shift:&nbsp;&nbsp;</td>
                <td width="30%"><%= rs.Item("ShiftID"); %></td>
                <td width="20%" align="right" class='lblColor'>&nbsp;</td>
                <td width="30%">&nbsp;</td>
            </tr>
            <tr> 
                <td width="20%" valign="top" align="right" class='lblColor'>Customer:&nbsp;&nbsp;</td>
                <td width="30%"><%=rs.Item("EmpName");%></td>
                <td width="20%"align="right" class='lblColor'>&nbsp;</td>
                <td width="30%">&nbsp;</td>
            </tr>
            <tr> 
                <td width="20%" valign="top" align="right" class='lblColor'>Task:&nbsp;&nbsp;</td>
                <td width="30%"><%=rs.Item("TaskDescription") %>&nbsp;(<%=rs.Item("TaskCode"); %>)</td>
                <td width="20%" align="right" class='lblColor'>&nbsp;</td>
                <td width="30%">&nbsp;</td>
            </tr>
            <tr> 
                <td width="20%" valign="top" align="right" class='lblColor'>Units:&nbsp;&nbsp;</td>
                <td width="30%"><%= rs.Item("UPM"); %></td>
                <td width="20%" align="right" class='lblColor'>&nbsp;</td>
                <td width="30%">&nbsp;</td>
            </tr>
            <tr> 
                <td width="20%" valign="top" align="right" class='lblColor'> 
                Hours:&nbsp;&nbsp;
                </td>
                <td width="30%"><%= rs.Item("HoursWorked"); %></td>
                <td width="20%">&nbsp;</td>
                <td width="30%">&nbsp;</td>
            </tr>
            <tr> 
                <td width="20%">&nbsp;</td>
                <td width="30%">&nbsp;</td>
                <td width="20%">&nbsp;</td>
                <td width="30%">&nbsp;</td>
            </tr>
            <tr> 
                <td width="20%" valign="top" align="right" class='lblColor'>Notes:&nbsp;&nbsp;</td>
                <td colspan="3"> 
                <textarea rows="5" cols="45" readonly name="textarea"><%= rs.Item("Notes"); %></textarea>
                </td>
            </tr>
            <tr> 
                <td width="20%">&nbsp;</td>
                <td width="30%">&nbsp;</td>
                <td width="20%">&nbsp;</td>
                <td width="30%">&nbsp;</td>
            </tr>
            <tr> 
                <td width="20%" align="right" class='lblColor'>Last 
                Modified:&nbsp;&nbsp;</td>
                <td colspan="2"><%=rs.Item("LastModifiedOn"); %></td>
                <td width="30%">&nbsp;</td>
            </tr>
            <tr> 
                <td width="20%" align="right" class='lblColor'>By:&nbsp;&nbsp;</td>
                <td colspan="2"><%=rs.Item("LastModifiedBy"); %></td>
                <td width="30%">&nbsp;</td>
            </tr>
            <tr> 
                <td width="20%">&nbsp;</td> 
                <td width="30%">&nbsp;</td>
                <td width="20%">&nbsp;</td>
                <td width="30%">&nbsp;</td>
            </tr>
            <tr> 
                <td width="20%">&nbsp;</td>
                <td colspan="3" align="Left"> 
                <% sWDate = "&WDate=" + rs["WorkDate"]; %>
                <% if ( (sStatus == "OPEN")  || ((sStatus == "CORPORATE") && (Trim(UCase(System.Convert.ToString(Session["UserType"]))) == "SUPER") ) ||
						((sStatus == "FACILITY")  && (Trim(UCase(System.Convert.ToString(Session["UserType"]))) == "ADMIN") ) ) { 
						%>
                <a href="../Payroll/TaskWorkedEdit.aspx?ID=<% =cStr(System.Convert.ToString(Request["ID"])) + sReturnTo + cStr(System.Convert.ToString(Request["ID"])) %>" >Edit 
                Task Worked</a> 
                <% } %>
                </td>
            </tr>
            <tr> 
                <td width="20%">&nbsp;</td>
                <td width="30%">&nbsp;</td>
                <td width="20%">&nbsp;</td>
                <td width="30%">&nbsp;</td>
            </tr>
            <tr> 
                <td width="20%">&nbsp;</td>
                <td colspan="3" align="center"> 
                <% if (sStatus == "OPEN") { %>
                <input type="button" name="btnApprove" value="Approve" onclick="goApprove();" />
                <% } %>
                <% if ((sStatus == "FACILITY") && (Trim(UCase(System.Convert.ToString(Session["UserType"]))) != "USER") ) { %>
                <input type="button" name="btnOpen"    value="Open"    onclick="goOpen();" />
                <input type="button" name="btnApprove" value="Approve" onclick="goApprove();" />
                <% } %>
                <% if ((sStatus == "CORPORATE") && (Trim(UCase(System.Convert.ToString(Session["UserType"]))) == "SUPER") ) { %>
                <input type="button" name="btnOpen"    value="Open"    onclick="goOpen();"   />
                <% } %>
                <input type="button" name="btnBack" value="Back" onclick="goBack();" />
                </td>
            </tr>
            </table>
        </form>
        <!-- #EndEditable --> </td>
    </tr>
       
	</table>
</asp:Content>
