<%@ Page Language="C#" MasterPageFile="~/Common/Common.Master" AutoEventWireup="true" CodeBehind="OutOfTown.aspx.cs" Inherits="InterrailPPRS.Common.OutOfTown" %>


<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script type="text/javascript">
<!--

    function bodyload() {

        LoadMembers();

        OriginalFormCheckSumValue = CheckStringForForm(document.form1);
    }


    function bodyunload() {
    }


    function goValidate(submittype) {
        if (submittype == 'ADD') {
            frm = document.form1;

            var s = '';

            var from = frm.select8;
            var iend = ((from.options.length) - 1);


            for (i = iend; i > -1; i--) {
                from.options[i].selected = true;
                if (i == iend) {
                    s = from.options[i].value
                }
                else {
                    s = s + "|" + from.options[i].value
                }
            }



            window.returnValue = s;
            window.close();
        }
        if (submittype == 'CANCEL') {
            window.returnValue = '';
            window.close();

        }

    }

    function goAddRemove(direction) {
        var frm = document.form1;
        if (direction == "ADD") {
            var from = frm.select7;
            var to = frm.select8;
        }
        else {
            var from = frm.select8;
            var to = frm.select7;
        }

        var iend = ((from.options.length) - 1);

        for (i = iend; i > -1; i--) {
            if (from.options[i].selected) {
                var id = from.options[i].value;
                var val = from.options[i].text;
                var newOption = new Option(val, id, false, false);
                to.options[to.options.length] = newOption;
                to.options[(to.options.length) - 1].selected = true;
                from.options[i] = null;
            }
        }
    }

    function clearLists() {
        var frm = document.form1;
        var from = frm.select7;
        var iend = ((from.options.length) - 1);
        var i;
        for (i = iend; i > -1; i--) {
            from.options[i] = null;
        }

        var from = frm.select8;
        var iend = ((from.options.length) - 1);

        for (i = iend; i > -1; i--) {
            from.options[i] = null;
        }

    }

    function LoadMembers() {
        var frm = document.form1;

        var allEmp = frm.selectAll;
        var toAvailable = frm.select7;
        var toMembers = frm.select8;



        // Clear Available and Team Members Lists
        clearLists();

        var allLen = allEmp.options.length;

        // Clear list of selected employees
        for (var j = 0; j < allLen; j++) {
            allEmp.options[j].selected = false;
        }


        // if employee is in the array AND not already in list, move to Team Members
        // else move to list of available employees

        for (var j = 0; j < allLen; j++) {
            var allID = allEmp.options[j].value;
            var allVal = allEmp.options[j].text;
            var newOption = new Option(allVal, allID, false, false);

            bNotIn = true;
            var toLen = toMembers.options.length;
            for (var k = 0; k < toLen; k++) {
                if (toMembers.options[k].value == allID) { bNotIn = false; }
            }

            if ((allEmp.options[j].selected) && (bNotIn)) {
                toMembers.options[toMembers.options.length] = newOption;
            }
            else {
                if (bNotIn) { toAvailable.options[toAvailable.options.length] = newOption; }
            }
        }
    }



// -->
</script>
</asp:Content>

<asp:Content  ID="leftNavigation" ContentPlaceHolderID="Navigation" runat="server">

        <table width="81%" border="0"  valign="top">
            <tr>
            <td width="8%">&nbsp;</td>
            <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
            <td width="79%"><a href="../Admin/Default.aspx">Administrative</a></td>
            </tr>
            <tr>
            <td width="8%">&nbsp;</td>
            <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
            <td width="79%"><a href="../Production/Default.aspx">Production</a></td>
            </tr>
            <tr>
            <td width="8%">&nbsp;</td>
            <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
            <td width="79%"><a href="../Payroll/Default.aspx">Payroll</a></td>
            </tr>
            <tr>
            <td width="8%">&nbsp;</td>
            <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
            <td width="79%"><a href="../Reports/Default.aspx">Reports</a></td>
            </tr>

            <tr>
            <td width="8%">&nbsp;</td>
            <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
            <td width="79%"><a href="../Rebilling/Default.aspx">Re-Billing</a></td>
            </tr>

            <tr>
            <td width="8%">&nbsp;</td>
            <td width="13%">&nbsp;</td>
            <td width="79%">&nbsp;</td>
            </tr>
            <tr>
            <td width="8%">&nbsp;</td>
            <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
            <td width="79%"><a href="../Logout.aspx">Logout</a></td>
            </tr>

        </table>

</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="bodycontent" runat="server">

        <table width="90%" border="0" cellspacing="0" cellpadding="8">
        <tr> 
            <td height="40"> 
                <table  align="center" border="0" cellpadding="0" cellspacing="0" width="85%">
                <tr> 
                    <td align="default" colspan="6" class="pageTitle"> 
                    <div align="center">Select Out of Town Employees</div>
                    </td>
                    <td align="default"> </td>
                </tr>
                <tr valign="baseline"> 
                    <td align="center" colspan="6"> 
                    <div align="center"></div>
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td align="right" width="45">&nbsp;</td>
                    <td colspan="2" align="right">&nbsp;</td>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr valign="baseline"> 
                    <td align="right" width="45">&nbsp;</td>
                    <td colspan="2" align="right">&nbsp;</td>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr valign="baseline"> 
                    <td nowrap align="right" width="45">&nbsp;</td>
                    <td width="178" class="cellTopBottomBorder">Available Employees</td>
                    <td width="34">&nbsp;</td>
                    <td class="cellTopBottomBorder" colspan="3"> 
                    <div>Selected Employees</div>
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td align="left" width="45" height="91"><nobr><nobr></td>
                    <td rowspan="7" width="178"> 
                    <select name="select7" multiple size="20" style="width:170px" title="Available Employees">
                        <option value="  "> </option>
                    </select>
                    </td>
                    <td width="34" height="91">&nbsp;</td>
                    <td rowspan="7" colspan="3"> 
                    <p> 
                        <select name="select8" multiple size="18" style="width:170px" title="Team Members" >
                        <option value=" "> </option>
                        </select>
                        <input type="button" name="Submit" value="Add Selected Employees" onClick="goValidate('ADD');">
                    </p>
                    <p> 
                        <input type="button" name="bntCancel" value="Cancel" onClick="goValidate('CANCEL');">
                    </p>
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td align="right" width="45">&nbsp; </td>
                    <td width="34" align="center"> 
                    <input type="button" name="btnAdd" value="&gt;&gt;" onClick="goAddRemove('ADD');">
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td align="right" width="45">&nbsp;</td>
                    <td width="34" align="center"> 
                    <input type="button" name="btnRemove" value="&lt;&lt;" onClick="goAddRemove('REMOVE');" >
                    </td>
                </tr>
                <tr valign="baseline"> 
                    <td align="right" width="45">&nbsp;</td>
                    <td width="34">&nbsp;</td>
                </tr>
                <tr valign="baseline"> 
                    <td align="right" width="45">&nbsp;</td>
                    <td width="34">&nbsp;</td>
                </tr>
                <tr valign="baseline"> 
                    <td align="right" width="45">&nbsp;</td>
                    <td width="34">&nbsp;</td>
                </tr>
                <tr valign="baseline"> 
                    <td align="right" width="45">&nbsp;</td>
                    <td width="34">&nbsp;</td>
                </tr>
                <div id="Layer1" style="position:absolute; width:1px; height:1px; z-index:1; visibility: hidden; left: 278px; top: 907px; overflow: hidden"> 
                    <select name="selectAll" multiple size=2>
                    <%
                    while (! rs.EOF){
                        rs.Read();
                    %>
                    <option value="<%=Trim(rs.Item("LastName"))%>, <%=Trim(rs.Item("FirstName"))%> (<%=Trim(rs.Item("EmployeeNumber"))%>)~<%=(rs.Item("Id"))%>">
                    <%=Trim(rs.Item("LastName"))%>, <%=Trim(rs.Item("FirstName"))%> (<%=Trim(rs.Item("EmployeeNumber"))%>)
                    </option>
                    <%

                    }

                        rs.Requery();

                    %>
                    </select>
                </div>
                </table>
            <p>&nbsp;</p>
            </td>
        </tr>
        </table>  

</asp:Content> 