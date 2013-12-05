<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="MAS90Select.aspx.cs" Inherits="InterrailPPRS.Admin.MAS90Select" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script type="text/javascript">

     function bodyload() {

        OriginalFormCheckSumValue = CheckStringForForm(document.form1);
    }

    function bodyunload() {
        if (CheckStringForForm(document.form1) != OriginalFormCheckSumValue) {
            event.returnValue = "You have not saved your changes.";
        }
    }

    function goDetail(et) {

        if (et == 'server') {
            document.frmPrint.savetype.value = "Server";
        }
        else {
            document.frmPrint.savetype.value = "Give";
        }
        document.frmPrint.action = "MAS90Generate.aspx";
        document.frmPrint.submit();
    }

    function CheckFile() {
        return true;
    }

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
            <form name='frmPrint' method='post' action=''>
                <table width="91%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td colspan="4" class="pageTitle" align='center'>
                    <div class="cellTopBottomBorder">Create MAS 90 CSV File </div>
                    </td>
                </tr>
                <tr>
                    <td width="52%" align='right'  >&nbsp; </td>
                    <td width="17%" align='center' >&nbsp;</td>
                    <td width="20%" align='center' >&nbsp;</td>
                    <td width="11%" align='center'>&nbsp;</td>
                </tr>
                <tr>
                    <td width="52%" align='center'  >
                    <select name="Company" >

                    <%
                      

                    while (!rsComp.EOF)
                    {
                        rsComp.Read();
                    %>
                        <option value="<%=(rsComp.Item("Id"))%>"  ><%=(rsComp.Item("CompanyName"))%></option>
                    <%
                    }
                    %>
                        </select>
                    </td>
                    <td colspan="2" align='center' >&nbsp;</td>
                    <td width="11%" align='center'>&nbsp;</td>
                </tr>
                <tr>
                    <td width="52%"> Start Date:
                    <input type="text" name="startdate">
                    </td>
                    <td colspan="2">
                    <p>
                        <input type="radio" name="Type" value="payrollirg">
                        Payroll - IRG</p>
                    </td>
                    <td align='center' valign='top'>
                    <input type="button" name="btnServer" value="Save On Server" onclick="goDetail('server');">
                    <input type="hidden" name="savetype" size="10" maxlength="10" value='server'>
                    </td>
                </tr>
                <tr>
                    <td width="52%" valign="top"> End Date:
                    <input type="text" name="enddate">
                    </td>
                    <td colspan="2">
                    <input type="radio" name="Type" value="payrolltemp">
                    Payroll - Temp</td>
                    <td width="11%" align='center'>
                    <input type="button" name="btnGive"   value="Give to me" onclick="goDetail('give');">
                    </td>
                </tr>
                <tr>
                    <td width="52%">
                    </td>
                    <td colspan="2">
                    <p>
                        <input type="radio" name="Type" value="production" checked>
                        Production </p>
                    </td>
                    <td align='center' valign='top'>
                    </td>
                </tr>
                <tr>
                    <td width="52%" align='center' valign="top">&nbsp;</td>
                    <td colspan="2" align='center'>&nbsp;</td>
                    <td width="11%" align='center'>&nbsp;</td>
                </tr>
            </table>
            </form>

            <form method="post" ENCTYPE="multipart/form-data" action="ADPCompare.aspx" name="frmADP" onSubmit="return CheckFile();">
                <table width="91%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td colspan="3" class="pageTitle" align='center'>
                    <div class="cellTopBottomBorder">Compare Active Employees to ADP</div>
                    </td>
                </tr>
                <tr>
                    <td width="70%" align='right'  >&nbsp; </td>
                    <td width="20%" align='center' >&nbsp;</td>
                    <td width="10%" align='center'>&nbsp;</td>
                </tr>
                <tr>
                    <td width="95%" colspan=2 align='right'  ><input type="FILE" name="PictureName" size="50" maxlength="250"></td>
                    <td width="05%" align='center'><input type="submit" value="Go"></td>
                </tr>
                <tr>
                    <td width="70%" align='right'  >&nbsp; </td>
                    <td width="20%" align='center' >&nbsp;</td>
                    <td width="10%" align='center'>&nbsp;</td>
                </tr>
                </table>
            </form>

            <!-- #EndEditable -->
            </td>
        </tr>

        </table>
</asp:Content>