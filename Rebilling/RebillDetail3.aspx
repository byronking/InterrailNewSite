<%@ Page Language="C#" MasterPageFile="~/Rebilling/Rebilling.Master" AutoEventWireup="true" CodeBehind="RebillDetail3.aspx.cs" Inherits="InterrailPPRS.Rebilling.RebillDetail" %>



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
        <%if (InStr(0, "Super, Admin, User", System.Convert.ToString(Session["UserType"]), 1) >= 0)
        { %>
        <tr>
        <td width="8%">&nbsp; </td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
        <td width="79%"><a href="RebillDetail.aspx">Rebill Detail</a></td>
        </tr>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
        <td width="79%"><a href="ReconcileRebilling.aspx">Reconcile Rebilling</a></td>
        </tr>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
        <td width="79%"><a href="RebillingInvoices.aspx">Generate Invoices</a></td>
        </tr>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
        <td width="79%"><a href="ApproveRebillData.aspx">Approve Rebill Data</a></td>
        </tr>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
        <td width="79%"><a href="Profitability.aspx?PrintPreview=1&selFacilities=<%=Session["FacilityID"]%>">Profitability Report</a></td>
        </tr>
        <% } %>
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

<asp:Content ID="mainContent" ContentPlaceHolderID="Main" runat="server">
    <table width="90%" border="0" cellspacing="0" cellpadding="8">
        <tr> 
            <td> <!-- #BeginEditable "MainBody" --> 
            <table align="center"  border="0" cellspacing="0" cellpadding="0" width="100%" >
                <tr> 
                <td align="default" colspan="6"> 
                    <div align="center" class="pageTitle"><b>Rebill 
                    Detail</b></div>
                </td>
                </tr>
                <tr> 
                <td align="default" width="16%">&nbsp;</td>
                <td align="default" width="20%">&nbsp;</td>
                <td align="default" width="10%">&nbsp;</td>
                <td align="default" width="21%">&nbsp;</td>
                <td align="default" width="21%">&nbsp;</td>
                <td align="default" width="14%">&nbsp;</td>
                </tr>
                <tr> 
                <td align="default" width="16%">
                    <%if (CheckSecurity("Super, Admin")) { %>
                                <a href="RebillEntry.aspx">Rebill Entry</a>
                    <% } %>              
                </td>
                <td align="default" colspan="5"> 
                    <div align="right"><a href="RebillEdit.aspx?id=0">Add New Rebill Detail</a></div>
                </td>
                </tr>
                <tr> 
                <td align="default" width="16%">&nbsp;</td>
                <td align="default" width="20%">&nbsp;</td>
                <td align="default" width="10%">&nbsp;</td>
                <td align="default" width="21%">&nbsp;</td>
                <td align="default" width="21%">&nbsp;</td>
                <td align="default" width="14%">&nbsp;</td>
                </tr>
                <tr> 
                <td align="left" width="16%" class="cellTopBottomBorder">Work Date </td>
                <td align="left" width="20%" class="cellTopBottomBorder">Approval</td>
                <td align="left" width="10%" class="cellTopBottomBorder">Rebilled</td>
                <td align="left" width="21%" class="cellTopBottomBorder">SubTask (Task)</td>
                <td align="left" width="15%" class="cellTopBottomBorder">Customer</td>
                <td align="right" width="14%" class="cellTopBottomBorder">Hours 
                </td>
                </tr>
                <%string rowColor; %>
                <%
                while ((Repeat1__numRows != 0) && (! rs.EOF)){
                    rs.Read();
                %>
                                                <%
                    if (Repeat1__index % 2 == 0) {
                    rowColor = "reportEvenLine";
                    }else{
                    rowColor = "reportOddLine";
                    }
  
                    %>
                <tr class="<%=rowColor%>"> 
                <td align="left" width="16%"> 
                    <% if (UCase(Trim(rs.Item("RebillStatus"))) != "OPEN") { %>
                    <a href="ApproveRebillDataEdit.aspx?<%= MM_keepForm + MM_joinChar(MM_keepForm) + "Id=" + rs.Item("Id") %>"><%= Replace(rs.Item("WorkDate"),"12:00:00 AM","")%></a> 
                    <% }else{ %>
                    <a href="RebillEdit.aspx?<%= MM_keepForm + MM_joinChar(MM_keepForm) + "Id=" + rs.Item("Id") %>"><%= Replace(rs.Item("WorkDate"), "12:00:00 AM", "")%></a> 
                    <%} %>
                </td>
                <td align="left" width="20%"><%= rs.Item("RebillStatus")%></td>
                <td align="left" width="10%"><% if (rs.Item("Rebilled") == "True" ){ %><img src='../images/check.gif' /> <% } else { %>&nbsp; <% }%></td>
                <td align="left" width="21%"><%= rs.Item("Description") %> (<%= rs.Item("TaskCode")%>)</td>
                <td align="left" width="21%"><%= rs.Item("CustomerName") %> (<%= rs.Item("CustomerCode")%>)</td>
                <td align="right" width="14%"><%= rs.Item("TotalHours")%> </td>
                </tr>
                <%
                Repeat1__index = Repeat1__index+1;
                Repeat1__numRows = Repeat1__numRows-1;
                
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
                    <% } // end MM_offset <> 0 %>
                </td>
                <td width="23%" align="center"> 
                    <% if (! MM_atTotal ){ %>
                    <a href="<%=MM_moveNext%>">Next</a> 
                    <% } // end Not MM_atTotal %>
                </td>
                <td width="23%" align="center"> 
                    <% if (! MM_atTotal ){ %>
                    <a href="<%=MM_moveLast%>">Last</a> 
                    <% } // end Not MM_atTotal %>
                </td>
                </tr>
            </table>
            <p>Records <%=(rs_first)%> to <%=(rs_last)%> of <%=(rs_total)%> </p>
            <!-- #EndEditable --> 
            </td>
        </tr>
       
	 </table>

</asp:Content>