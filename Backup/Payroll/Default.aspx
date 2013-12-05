<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Payroll/Payroll.Master" CodeBehind="Default.aspx.cs" Inherits="InterrailPPRS.Payroll.Default" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>

<asp:Content  ID="leftNavigation" ContentPlaceHolderID="Navigation" runat="server">
    <table width="81%" border="0"  valign="top">
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%">&nbsp;</td>
        <td width="79%">&nbsp;</td>
        </tr>
        <% = ChangeFacilityLink()%>
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
        <% } %>
        <%if (CheckSecurity()) { %>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"><a href="calc.aspx">Calculate</a></td>
        </tr>
        <%}%>

        <%if(CheckSecurity()){ %>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"> <a href="ApprovePayrollData.aspx">Approval</a></td>
        </tr>
        <%}%>
        <%if(CheckSuperAdminSecurity()){ %>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"> <a href="CSVSelect.aspx">Create ADP File</a></td>
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

        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
            <td width="20%">&nbsp;</td>
            <td width="20%">&nbsp;</td>
            <td width="20%">&nbsp;</td>
            <td width="20%">&nbsp;</td>
            <td width="20%">&nbsp;</td>
            </tr>
            <tr class='pageTitle'>
            <td colspan="5" align='center'  valign='top'>Pay
                Period: &nbsp;<% =sPayRange %> </td>
            </tr>
            <tr>
            <td width="20%">&nbsp;</td>
            <td width="20%">&nbsp;</td>
            <td width="20%">&nbsp;</td>
            <td width="20%">&nbsp;</td>
            <td width="20%">&nbsp;</td>
            </tr>
               <%
                if (!rsPreEmp.HasRows) {
                  Response.Write("<tr> <td width='20%'>&nbsp;</td>");
                  Response.Write("<td colspan='4' align='left'  valign='top'>&nbsp;No new hires previous period.</td>");
                  Response.Write("</tr>");
                }else{
                  Response.Write("<tr>");
                  Response.Write("<td colspan='5' align='left'  valign='top'>New hires previous pay period:</td>");
                  Response.Write("</tr>");
                  Response.Write( "<tr> ");
                  Response.Write( "<td width='20%' class='cellTopBottomBorder'>Number</td>");
                  Response.Write( "<td colspan='2' class='cellTopBottomBorder'>Name</td>");
                  Response.Write( "<td width='20%' class='cellTopBottomBorder'>Hire Date</td>");
                  Response.Write( "<td width='20%' class='cellTopBottomBorder'>&nbsp;</td>");
                  Response.Write( "</tr>");
                  while(rsPreEmp.Read()){
                    Response.Write( "<tr> ");
                    Response.Write( "<td width='20%'><a href='../Admin/EmployeeEdit.aspx?ID=" + rsPreEmp["ID"] + "'>" + rsPreEmp["EmpNum"] + "</a></td>");
                    Response.Write( "<td colspan='2'><a href='../Admin/EmployeeEdit.aspx?ID=" + rsPreEmp["ID"] + "'>" + rsPreEmp["LastName"] + ", " + rsPreEmp["FirstName"] + "</a></td>");
                    Response.Write("<td width='20%'>" + ((DateTime)rsPreEmp["HireDate"]).ToString("MM/dd/yyyy") + "</td>");
                    Response.Write( "<td width='20%'>&nbsp;</td>");
                    Response.Write( "</tr>");
                  }
                }

                if (!rsNewEmp.HasRows) {
                  Response.Write( "<tr> <td width='20%'>&nbsp;</td>");
                  Response.Write( "<td colspan='4' align='left'  valign='top'>&nbsp;No new hires this pay period.</td>");
                  Response.Write( "</tr>");
                }else{
                  Response.Write( "<tr>");
                  Response.Write( "<td colspan='5' align='left'  valign='top'>New hires this pay period:</td>");
                  Response.Write( "</tr>");
                  Response.Write( "<tr> ");
                  Response.Write( "<td width='20%' class='cellTopBottomBorder'>Number</td>");
                  Response.Write( "<td colspan='2' class='cellTopBottomBorder'>Name</td>");
                  Response.Write( "<td width='20%' class='cellTopBottomBorder'>Hire Date</td>");
                  Response.Write( "<td width='20%' class='cellTopBottomBorder'>&nbsp;</td>");
                  Response.Write( "</tr>");
                  while(rsNewEmp.Read()){
                    Response.Write( "<tr> ");
                    Response.Write( "<td width='20%'><a href='../Admin/EmployeeEdit.aspx?ID=" + rsNewEmp["ID"] + "'>" + rsNewEmp["EmpNum"] + "</a></td>");
                    Response.Write("<td colspan='2'><a href='../Admin/EmployeeEdit.aspx?ID=" + rsNewEmp["ID"] + "'>" + rsNewEmp["LastName"] + ", " + rsNewEmp["FirstName"] + "</a></td>");
                    Response.Write( "<td width='20%'>" + ((DateTime)rsNewEmp["HireDate"]).ToString("MM/dd/yyyy") + "</td>");
                    Response.Write( "<td width='20%'>&nbsp;</td>");
                    Response.Write( "</tr>");
                   }
                }
              %>
        </table>

</asp:Content> 