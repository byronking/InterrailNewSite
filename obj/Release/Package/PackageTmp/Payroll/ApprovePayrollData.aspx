<%@ Page Language="C#" MasterPageFile="~/Payroll/Payroll.Master" AutoEventWireup="true" CodeBehind="ApprovePayrollData.aspx.cs" Inherits="InterrailPPRS.Payroll.ApprovePayrollData" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script type="text/javascript" >

    function goNewDate() {
        document.form1.PayPeriod.value = document.form1.selDateRange.value;

        document.form1.action = "ApprovePayrollData.aspx";
        //document.form1.action = "/col.aspx";
        document.form1.submit();

    }


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
                    <td width="79%"> <a href="CSVSelect.aspx">Create CSV File</a></td>
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
    <td>

    <table align="center"  border="0" cellspacing="0" cellpadding="0" width="100%" >
        <tr>
        <td align="default" colspan="8">
            <div align="center" class="pageTitle"> Approve Payroll Data for <%=cStr(rsEmp__PSTART) + " - " + cStr(rsEmp__PEND)%> </div>
        </td>
        </tr>

        <form name='form1' action=''>
        <tr class='pageTitle'>
        <td colspan='10' align='center'  valign='top'>
        <select name='selDateRange' onChange='goNewDate();'><%=getPayPeriods(0,52,rsEmp__PSTART)%></select>
        <input type='hidden' name='PayPeriod' />
        </td>
        </tr>
        </form>

    </table>

</td>
</tr>

<tr>
<td>
<%= GetLines()%>
</td>
       
    </table>
</asp:Content>