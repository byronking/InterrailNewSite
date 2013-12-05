<%@ Page Language="C#" MasterPageFile="~/Payroll/Payroll.Master" AutoEventWireup="true" CodeBehind="CSVSelect.aspx.cs" Inherits="InterrailPPRS.Payroll.CSVSelect" %>



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
            <form name='frmPrint' method='post' action=''>
                <table width="91%" border="0" cellspacing="0" cellpadding="0">
                <tr> 
                    <td colspan="4" class="pageTitle" align='center'> 
                    <div class="cellTopBottomBorder">Create CSV 
                        File </div>
                    </td>
                </tr>
                <% if ( Request["NotLocked"] != null && System.Convert.ToString(Request["NotLocked"]).ToLower() == "true" ){ %>
                <tr> 
                    <td colspan="4" class="pageTitle" align='center'> 
                    <div class="cellTopBottomBorder">
                    Facilities not locked:<p>
                    <%=Request["Fac"]%>
                    </p>
                    </div>
                    </td>
                </tr>
                <% } %>
                <tr> 
                    <td width="52%" align='right'  >&nbsp; </td>
                    <td width="17%" align='center' >&nbsp;</td>
                    <td width="20%" align='center' >&nbsp;</td>
                    <td width="11%" align='center'>&nbsp;</td>
                </tr>
                <tr> 
                    <td width="52%" align='center'  ><font color='green'>Facilities*</font></td>
                    <td colspan="2" align='center' ><font color="green">Pay 
                    Period </font></td>
                    <td width="11%" align='center'>&nbsp;</td>
                </tr>
                <tr> 
                    <td align='right' width="52%"><font color='green'> 
                    <select name="selFacilities" size="4" multiple style="width:160px">
                        <%
                        while (rsFac.Read()){
                        %>
                        <option value="<% =rsFac.Item("Id")%>" 
                        <% if ( System.Convert.ToString(rsFac.Item("Id")) == System.Convert.ToString(Session["FacilityID"]) ) {  Response.Write("SELECTED"); }else{ Response.Write("");}%>>
                        <%=(rsFac.Item("Name"))%> (<%=(rsFac.Item("AlphaCode"))%>)
                        </option>
                        <%
                        }
                        
                        rsFac.Requery();
                        

                        %>
                    </select>

                    </font></td>
                    <td colspan="2" align='center'  valign='top'> 
                    <select name="selDateRange">
                        <%=getPayPeriods(0,52,"")%> 
                    </select>
                    <input type="hidden" name="fromDateDetail" value='<%=System.DateTime.Now.ToShortDateString()%>' />
                    <input type="hidden" name="toDateDetail"  value='<%=System.DateTime.Now.ToShortDateString()%>' />
                    </td>
                    <td align='center' valign='top'> 
                    <input type="button" name="btnServer" value="Save On Server" onclick="goDetail('server');" />
                    <input type="button" name="btnGive"   value="Give to me" onclick="goDetail('give');" />
                    <input type="hidden" name="savetype"  value='server' />
                    </td>
                </tr>
                <tr> 
                    <td width="52%" align='center' valign="top">&nbsp;</td>
                    <td colspan="2" align='center'>&nbsp;</td>
                    <td width="11%" align='center'>&nbsp;</td>
                </tr>
                <tr> 
                    <td colspan="3" align='right'  class="required">* 
                    Select one or more facilities. if (none are 
                    selected, the default facility is used.</td>
                    <td width="11%" align='center'>&nbsp;</td>
                </tr>
                </table>
            </form>
            <!-- #EndEditable --> </td>
        </tr>
       
	    </table>
</asp:Content>