<%@ Page Language="C#" MasterPageFile="~/Common/Common.Master" AutoEventWireup="true" CodeBehind="SetDefaultFacility.aspx.cs" Inherits="InterrailPPRS.Common.SetDefaultFacility" %>


<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script type="text/javascript">
<!--
    function goAccept() {
        frm = document.forms[0];
        frm.method = "post";
        frm.FacilityID.value = frm.select[frm.select.selectedIndex].value;
        frm.FacilityName.value = frm.select.options[frm.select.selectedIndex].text;
        frm.action = "SetDefaultFacility.aspx?goTo=<%=sFrom%>";
        frm.submit();
    }
    function goCancel() {
        self.location = "<%=sFrom%>";
    }

    function bodyload() {
        document.form1.select.focus();
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
            <td> <!-- #BeginEditable "MainBody" -->
            <p>&nbsp;</p>
                <table width="75%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="55%">&nbsp;</td>
                    <td width="55%">
                    <div align="center" class="pageTitle">Change  Facility</div>
                    </td>
                    <td width="45%">&nbsp;</td>
                </tr>
                <tr>
                    <td width="55%">&nbsp;</td>
                    <td width="55%">&nbsp;</td>
                    <td width="45%">&nbsp;</td>
                </tr>
                <tr>
                    <td width="55%">&nbsp;</td>
                    <td width="55%">
                    <select name="select" style="width:200px">
                        <%
                        while (! rs.EOF){
    
                            rs.Read();
                        %>
                        <option value="<%=rs.Item("Id")%>" <%if (System.Convert.ToString(rs.Item("Name")) == System.Convert.ToString(Session["FacilityName"])) { Response.Write("SELECTED"); }else{  Response.Write(""); }%> ><%=rs.Item("Name")%></option>
                        <%
                            }
                        rs.Requery();

                    %>
                    </select>
                    </td>
                    <td width="45%">&nbsp;</td>
                </tr>
                <tr>
                    <td width="55%">&nbsp;</td>
                    <td width="55%">&nbsp;</td>
                    <td width="45%">&nbsp;</td>
                </tr>
                <tr>
                    <td width="55%">&nbsp;</td>
                    <td width="55%" align="center">
                    <input type="button" name="btnAccept" value="Accept" onClick="goAccept();">
                    <input type="button" name="btnCancel" value="Cancel" onClick="goCancel();">
                    </td>
                    <td width="45%">&nbsp;</td>
                </tr>
                <tr>
                    <td width="55%">
                    <input type="hidden" name="FacilityID"   value="<%=Session["FacilityID"]%>">
                    <input type="hidden" name="FacilityName" value="<%=Session["FacilityName"]%>">
                    </td>
                    <td width="55%">&nbsp;</td>
                    <td width="45%">&nbsp;</td>
                </tr>
                </table>
                <p>&nbsp; </p>
                <p>&nbsp; </p>
            <p>&nbsp; </p>
            <p>
            </p>
            <!-- #EndEditable -->
            </td>
        </tr>

    </table>

</asp:Content> 