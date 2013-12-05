<%@ Page Language="C#" MasterPageFile="~/Payroll/Payroll.Master" AutoEventWireup="true" CodeBehind="TaskWorked.aspx.cs" Inherits="InterrailPPRS.Payroll.TaskWorked" %>



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
        <% = ChangeFacilityLink() %>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%">&nbsp;</td>
        <td width="79%">&nbsp;</td>
        </tr>
        <% 
        if (CheckSecurity()) { %>
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
<table width="90%" border="0" cellspacing="0" cellpadding="8">
    <tr> 
        <td height="40"> <!-- #BeginEditable "MainBody" --> 
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="85%">
            <tr> 
            <td align="default" colspan="4"> 
                <div align="center" class="pageTitle">Tasks 
                Worked </div>
            </td>
            </tr>
            <tr> 
            <td align="default" width="23%">&nbsp;</td>
            <td align="default" width="37%">&nbsp;</td>
            <td align="default" width="40%">&nbsp;</td>
            <td align="default" width="40%">&nbsp;</td>
            </tr>
            <tr> 
            <td align="default" width="23%">&nbsp;</td>
            <td align="default" colspan="3"> 
                <div align="right"></div>
                <div align="right"><a href="TaskWorkedEdit.aspx?ID=0">Add New Task Worked</a></div>
            </td>
            </tr>
            <tr> 
            <td align="default" width="23%">&nbsp;</td>
            <td align="default" width="37%">&nbsp;</td>
            <td align="default" width="40%">&nbsp;</td>
            <td align="default" width="40%">&nbsp;</td>
            </tr>
            <tr> 
            <th align="default" width="23%"  class="cellTopBottomBorder"> 
                <div align="left">Date</div>
            </th>
            <th align="default" width="37%"  class="cellTopBottomBorder"> 
                <div align="left"> Name</div>
            </th>
            <th align="default" width="40%"  class="cellTopBottomBorder"> 
                <div align="left">Task</div>
            </th>
            <th align="default" width="40%"  class="cellTopBottomBorder"> 
                <div align="right">Hours</div>
            </th>
            </tr>
            <%string rowColor;%>
            <% 
            while ((Repeat1__numRows != 0) && (! rs.EOF)) {
                rs.Read();
            %>
                                            <%
                if (Repeat1__index % 2 == 0) {
                rowColor = "reportEvenLine";
                }else{	
                rowColor = "reportOddLine";
                }
                %>
            <tr  class="<%=rowColor%>"> 
            <td align="default" width="23%"> 
                <a href="../Payroll/TaskworkedEdit.aspx?Id=<%=rs.Item("Id") %>" ><%=(rs.Item("WorkDate"))%></a> </td>
            <td align="default" width="37%"><%=(rs.Item("FullName"))%></td>
            <td align="default" width="40%"> 
                <%=(rs.Item("TaskDescription"))%></td>
            <td align="right" width="40%"><%=(rs.Item("HoursWorked"))%></td>
            </tr>
            <% 
            Repeat1__index = Repeat1__index + 1;
            Repeat1__numRows = Repeat1__numRows - 1;
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
                <% if( MM_offset != 0 ){ %>
                <a href="<%=MM_moveFirst%>">First</a> 
                <% } // end MM_offset <> 0 %>
            </td>
            <td width="31%" align="center"> 
                <% if( MM_offset != 0 ){ %>
                <a href="<%=MM_movePrev%>">Previous</a> 
                <% } //end MM_offset <> 0 %>
            </td>
            <td width="23%" align="center"> 
                <% if(! MM_atTotal ){ %>
                <a href="<%=MM_moveNext%>">Next</a> 
                <% } // end Not MM_atTotal %>
            </td>
            <td width="23%" align="center"> 
                <% if(! MM_atTotal ){ %>
                <a href="<%=MM_moveLast%>">Last</a> 
                <% } //MM_atTotal %>
            </td>
            </tr>
        </table>
        <p>Records <%=(rs_first)%> to <%=(rs_last)%> of <%=(rs_total)%> </p>
        <!-- #EndEditable --> </td>
    </tr>
       
	</table>
</asp:Content> 