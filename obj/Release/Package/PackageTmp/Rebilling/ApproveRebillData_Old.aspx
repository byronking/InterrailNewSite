<%@ Page Language="C#" MasterPageFile="~/Rebilling/Rebilling.Master" AutoEventWireup="true" CodeBehind="ApproveRebillData_Old.aspx.cs" Inherits="InterrailPPRS.Rebilling.ApproveRebillData_Old" %>

<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);        
    }        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
<script type="text/javascript" >
<!--
    function goApprove(sType) {
        //frm = document.form1;
        var frm = $("#aspnetForm");

        var from = $("#fromDate").val();
        var to = $("#toDate").val();

        var dateFrom = from;
        var dateTo = to;
        //alert('date from: ' + dateFrom);
        //alert('date to: ' + dateTo);

        if (dateFrom !== undefined || dateTo !== undefined)
        {
            if (dateFrom > dateTo) {
                alert(" 'Date From' must be prior to 'Date To'.");
                from.focus();
                from.select();
                return false;
            }
        }

        if ('<%=Trim(UCase(System.Convert.ToString(Session["UserType"])))%>' == 'USER') {
            frm.action = 'ApproveRebillDataEdit.aspx?Approval=FACILITY&Which=' + sType + "&From=" + escape(from + "&To=" + escape(to));
            //alert('frm.action1 : ' + frm.action);
        }
        else {
            frm.action = "ApproveRebillDataEdit.aspx?Approval=CORPORATE&Which=" + sType + "&From=" + escape(from + "&To=" + escape(to));
            //alert('frm.action2 : ' + frm.action);
        }

        if (!confirm("Approved data cannot be modified.\nPress OK to Approve.")) {
            return false;
        }

        frm.submit();
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
        <%if (CheckSecurity("Super, Admin, User")){ %>
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
                            <form name="form1" method="post" action="" runat="server" id="form1">
                              <table align="center"  border="0" cellspacing="0" cellpadding="0" width="100%" >
                                <tr>
                                  <td align="default" colspan="7">
                                    <div align="center" class="pageTitle">Approve
                                      Rebill Data </div>
                                  </td>
                                </tr>
                                <tr>
                                  <td align="default" colspan="8" >
                                    <div align="center" class="pageTitle"><%=Session["FacilityName"]%></div>
                                  </td>
                                </tr>
                                <tr>
                                  <td align="default" width="13%" >&nbsp;</td>
                                  <td align="default" width="4%" >&nbsp;</td>
                                  <td align="default" width="19%" >&nbsp;</td>
                                  <td align="default" width="20%" >&nbsp;</td>
                                  <td align="default" colspan="2" >&nbsp;</td>
                                  <td align="right" width="14%"   >&nbsp;</td>
                                  <td align="right" width="14%"   >&nbsp;</td>
                                </tr>
                                <tr>
                                  <td align="default" width="13%" >&nbsp;</td>
                                  <td align="default" colspan="7" >&nbsp;</td>
                                </tr>
                                <tr>
                                  <td align="center" colspan="6">
                                    <span style="margin-left:180px">
                                        <font color='green'><b>Approve ALL Open within the range: </b></font>
                                    </span>
                                  </td>
                                </tr>
                                <tr>
                                  <td align="right" width="13%" >&nbsp;</td>
                                  <td align="right" width="4%" >&nbsp;</td>
                                  <td align="right" colspan="2" >&nbsp;</td>
                                  <td align="center" width="15%" class='cellTopBottomBorder'>From</td>
                                  <td align="center" width="15%" class='cellTopBottomBorder'>To</td>
                                  <td align="left" width="15%" >&nbsp;</td>
                                  <td align="left" width="15%" >&nbsp;</td>
                                </tr>
                                <tr>
                                  <td align="left" colspan="4">
                                      <font color='green'><b>Approve checked:</b></font> <input type="button" name="btnChecked" value="Go" onClick="goApprove('CHECKED');">
                                  </td>
                                  <%--<td align="right" >&nbsp;</td>--%>
                                  <td align="center" width="15%" >
                                    <%--<input type="text" id="fromDate" size="10" maxlength="10">--%>
                                    <asp:TextBox runat="server" ID="txtFromDate" ClientIDMode="Static" MaxLength="10" />
                                  </td>
                                  <td align="center" width="15%">
                                    <%--<input type="text" id="toDate" size="10" maxlength="10">--%>
                                      <asp:TextBox runat="server" ID="txtToDate" ClientIDMode="Static" MaxLength="10" style="margin-left:5px" />
                                  </td>
                                  <td align="right" width="10%">
                                    <%--<input type="button" name="btnOpen" value="Go" onClick="goApprove('OPEN');">--%>
                                      <asp:Button ID="btnGo" runat="server" Text="Go Now" OnClick="btnGo_Click" style="margin-left:5px" /> 
                                  </td>
                                  <td align="left" width="15%" >&nbsp;</td>
                                </tr>
                                <tr>
                                  <td align="default" width="13%" >&nbsp;</td>
                                  <td align="default" width="4%" >&nbsp;</td>
                                  <td align="default" width="19%" >&nbsp;</td>
                                  <td align="default" width="20%" >&nbsp;</td>
                                  <td align="default" colspan="2" >&nbsp;</td>
                                  <td align="right" width="14%"   >&nbsp;</td>
                                  <td align="right" width="14%"   >&nbsp;</td>
                                </tr>
                                <tr>
                                  <td align="center" width="13%" class="cellTopBottomBorder">Work
                                    Date</td>
                                  <td align="center" colspan="2" class="cellTopBottomBorder">Approval</td>
                                  <td align="center" width="25%" class="cellTopBottomBorder">SubTask (Task) </td>
                                  <td align="center" class="cellTopBottomBorder" colspan="2">Customer</td>
                                  <td align="left" width="10%" class="cellTopBottomBorder">Units</td>
                                  <td align="center" width="14%"   class="cellTopBottomBorder">Hrs.</td>
                                </tr>
                                <%string  rowColor;%>
                                <%
                                while ((Repeat1__numRows != 0) &&(! rs.EOF)){
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
                                  <td align="default" width="13%"><a href="ApproveRebillDataEdit.aspx?Id=<%=rs.Item("Id")%>" ><%=cDate(rs.Item("WorkDate")).ToShortDateString()%></a></td>
                                  <td align="center" width="4%">
                                    <% if ( (UCase(Trim(rs.Item("RebillStatus"))) == "OPEN")  ||  ((Trim(UCase(System.Convert.ToString(Session["UserType"]))) != "USER") && (UCase(Trim(rs.Item("RebillStatus"))) != "CORPORATE")) ) { %>
                                    <input type="checkbox" name="cbxApprove" value="<%=rs.Item("ID")%>">
                                    <% }else{ %>
                                    &nbsp;
                                    <% } %>
                                  </td>
                                  <td align="Left" width="19%">
                                    <%=rs.Item("RebillStatus")%></td>
                                  <td align="center" width="25%"><%=rs.Item("Description")%> (<%=rs.Item("TaskCode")%>)</td>
                                  <td align="center" colspan="2"><%=rs.Item("CustomerName")%> (<%=rs.Item("CustomerCode")%>)</td>
                                  <% if (rs.Item("HoursOrUnits") == "H") { %>
                                     <td align="right" width="14%">&nbsp;</td>
                                  <% }else{ %>
                                     <td align="right" width="14%"><%=rs.Item("TotalUnits")%>&nbsp;</td>
                                  <% } %>
                                  <td align="center" width="14%"><%=rs.Item("TotalHours")%> </td>
                                </tr>
                                <%
                                  Repeat1__index=Repeat1__index+1;
                                  Repeat1__numRows=Repeat1__numRows-1;

                                  }
                                %>
                              </table>
                            </form>
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
                                  <% if ( MM_offset != 0 ){ %>
                                  <a href="<%=MM_movePrev%>">Previous</a>
                                  <% } // end MM_offset <> 0 %>
                                </td>
                                <td width="23%" align="center">
                                  <% if (! MM_atTotal ){%>
                                  <a href="<%=MM_moveNext%>">Next</a>
                                  <% } // end Not MM_atTotal %>
                                </td>
                                <td width="23%" align="center">
                                  <% if(! MM_atTotal ){ %>
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