<%@ Page Language="C#" MasterPageFile="~/Rebilling/Rebilling.Master" AutoEventWireup="true" CodeBehind="ApproveRebillDataEdit.aspx.cs" Inherits="InterrailPPRS.Rebilling.ApproveRebillDataEdit" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script language="JavaScript">

    function goApprove() {
        frm = document.form1;

        if (!confirm("Approved data cannot be modified.\nPress OK to Approve.")) {
            return false;
        }

        if ('<%=Trim(UCase(System.Convert.ToString(Session["UserType"])))%>' == 'USER') {
            frm.action = 'ApproveRebillDataEdit.aspx?Approval=FACILITY&ID=<%=Request["ID"]%>';
        }
        else {
            frm.action = 'ApproveRebillDataEdit.aspx?Approval=CORPORATE&ID=<%=Request["ID"]%>';
        }
        frm.submit();
    }

    function goOpen() {
        frm = document.form1;

        if (!confirm("Are you sure you want to set the status to OPEN?\nPress OK to set approval status to OPEN.")) {
            return false;
        }
        else {
            frm.action = 'ApproveRebillDataEdit.aspx?Approval=OPEN&ID=<%=Request["ID"]%>';
        }
        frm.submit();
    }

    function goBack() {
        frm = document.form1;
        //frm.action = "ApproveRebillData.aspx";
        //frm.submit();
        history.go(-1);
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
        <%if (CheckSecurity("Super, Admin, User")) { %>
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
                            <form name="form1" method="post" action="">
                              <table align="center" width="80%" border="0" cellspacing="1" cellpadding="1">
                                <tr>
                                  <td colspan="4" class="pageTitle" align="center">Rebill
                                    Data</td>
                                </tr>
                                <tr>
                                  <td colspan="4">
                                    <div align="center" class="pageTitle"><%=Session["FacilityName"]%></div>
                                  </td>
                                </tr>
                                <tr>
                                  <td width="20%">&nbsp;</td>
                                  <td colspan="2">&nbsp;</td>
                                  <td width="10%">&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="20%">&nbsp;</td>
                                  <td colspan="2" align="right">Rebill Status:</td>
                                  <td width="10%">
                                    <div  class="required"><B><%=sStatus%></B></div>
                                  </td>
                                </tr>
                                <tr>
                                  <td width="20%">&nbsp;</td>
                                  <td width="20%">&nbsp;</td>
                                  <td width="10%">&nbsp;</td>
                                  <td width="10%">&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="20%" valign="top" align="right">Work
                                    Date: </td>
                                  <td width="20%"><%= Replace(rs.Item("WorkDate"),"12:00:00 AM","")%></td>
                                  <td width="10%" valign="top" align="right">&nbsp;</td>
                                  <td width="10%">&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="20%" valign="top" align="right">Shift:
                                  </td>
                                  <td width="20%"><%= rs.Item("ShiftID")%></td>
                                  <td width="10%">&nbsp;</td>
                                  <td width="10%">&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="20%" valign="top" align="right">Hours:
                                  </td>
                                  <td width="20%"><%= rs.Item("TotalHours")%></td>
                                  <td width="10%">&nbsp;</td>
                                  <td width="10%">&nbsp;</td>
                                </tr>
                                <% if(rs.Item("HoursOrUnits") == "U") { %>
                                <tr>
                                  <td width="20%" valign="top" align="right">Units:
                                  </td>
                                  <td width="20%"><%= rs.Item("TotalUnits")%></td>
                                  <td width="10%">&nbsp;</td>
                                  <td width="10%">&nbsp;</td>
                                </tr>
                                <%  } %>
                                <tr>
                                  <td width="20%" valign="top" align="right">Task:
                                  </td>
                                  <td colspan="2"><%=rs.Item("TaskDescription")%>&nbsp;(<%=rs.Item("TaskCode")%>)</td>
                                  <td>&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="20%" valign="top" align="right">SubTask:
                                  </td>
                                  <td colspan="2"><%=rs.Item("Description")%></td>
                                  <td>&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="20%" align="right">Customer:</td>
                                  <td colspan="2"><%=rs.Item("CustomerName")%> (<%=rs.Item("CustomerCode")%>)</td>
                                  <td width="10%">&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="20%">&nbsp;</td>
                                  <td colspan="2">&nbsp;</td>
                                  <td width="10%">&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="20%" align="right"><nobr>Material Costs:</nobr></td>
                                  <td colspan="2">$<%=rs.Item("MaterialCosts")%></td>
                                  <td width="10%">&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="20%" align="right"><nobr>Invoice Number:</nobr></td>
                                  <td colspan="2"><%=rs.Item("InvoiceNumber")%></td>
                                  <td width="10%">&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="20%" align="right">Vendors:</td>
                                  <td colspan="2"><%=rs.Item("Vendors")%></td>
                                  <td width="10%">&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="20%">&nbsp;</td>
                                  <td colspan="2">&nbsp;</td>
                                  <td width="10%">&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="20%" valign="top" align="right">Employees:
                                  </td>
                                  <td class="cellTopBottomBorder" colspan="2">Name</td>
                                  <td class="cellTopBottomBorder" align="right">Hours</td>
                                </tr>
                                <%
                                  string rowColor;
                                  int iRow = 0;
                                  
                                  while (! rsETW.EOF){
                                     rsETW.Read();
                                    if (iRow % 2 == 0) {
                                       rowColor = "reportOddLine";
                                    }else{
                                       rowColor = "reportEvenLine";
                                     }
                                    %>
                                <tr>
                                  <td width="20%">&nbsp;</td>
                                  <td colspan="2" class="<%=rowColor%>"><%=(rsETW.Item("LastName"))%>, <%=(rsETW.Item("FirstName"))%> (<%=(rsETW.Item("EmpNum"))%>)</td>
                                  <td class="<%=rowColor%>" align="right"><%=(rsETW.Item("HoursWorked"))%></td>
                                </tr>
                                <%
                                      iRow=iRow+1;
                                  }
                                %>
                                <tr>
                                  <td width="20%" valign="top" align="right">&nbsp;</td>
                                  <td>&nbsp;</td>
                                  <td>&nbsp;</td>
                                  <td>&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="20%" valign="top" align="right">Notes:
                                  </td>
                                  <td colspan="3">
                                    <textarea rows="5" cols="45" readonly name="textarea"><%= rs.Item("WorkDescription")%></textarea>
                                  </td>
                                </tr>
                                <tr>
                                  <td width="20%" align="right">&nbsp;</td>
                                  <td colspan="2">&nbsp;</td>
                                  <td width="10%">&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="20%" align="right">Last Modified:
                                  </td>
                                  <td colspan="2"><%=rs.Item("LastModifiedOn")%></td>
                                  <td width="10%">&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="20%" align="right">By: </td>
                                  <td colspan="2"><%=rs.Item("LastModifiedBy")%></td>
                                  <td width="10%">&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="20%">&nbsp;</td>
                                  <td colspan="3" align="Left">&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="20%">&nbsp;</td>
                                  <td colspan="3" align="Left">
                                    <% if ( (sStatus == "OPEN")  ||  ((sStatus == "CORPORATE") && (Trim(UCase(System.Convert.ToString(Session["UserType"]))) == "SUPER") ) ||  ((sStatus == "FACILITY")  && (Trim(UCase(System.Convert.ToString(Session["UserType"]))) == "ADMIN") ) ) { %>
                                    <a href="RebillEdit.aspx?ID=<%=cStr(Request["ID"]) + sReturnTo + cStr(Request["ID"])%>" >Edit Rebill Data</a>
                                    <%  } %>
                                  </td>
                                </tr>
                                <tr>
                                  <td width="20%">&nbsp;</td>
                                  <td width="20%">&nbsp;</td>
                                  <td width="10%">&nbsp;</td>
                                  <td width="10%">&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="20%">&nbsp;</td>
                                  <td colspan="3" align="center">
                                    <% if (sStatus == "OPEN") { %>
                                    <input type="button" name="btnApprove" value="Approve" onClick="goApprove();">
                                    <%  } %>
                                    <% if ((sStatus == "FACILITY") && (Trim(UCase(System.Convert.ToString(Session["UserType"]))) != "USER") ) { %>
                                    <input type="button" name="btnOpen"    value="Open"    onClick="goOpen();"   >
                                    <input type="button" name="btnApprove" value="Approve" onClick="goApprove();">
                                    <%  } %>
                                    <% if ((sStatus == "CORPORATE") && (Trim(UCase(System.Convert.ToString(Session["UserType"]))) == "SUPER") ) { %>
                                    <input type="button" name="btnOpen"    value="Open"    onClick="goOpen();"   >
                                    <%  } %>
                                    <input type="button" name="btnBack" value="Back" onClick="goBack();">
                                  </td>
                                </tr>
                              </table>
                              </form>
                                                <!-- BXM -->
                                                <% if (System.Convert.ToString(Request["ID"]) != "0") {%>                                                
                                                <table align="center"  border="0" cellspacing="0" cellpadding="0" width="100%">
                                                  <%
                                                      
                                                  int iiRow=0;


                                                    while (! rsAttach.EOF){

                                                       rsAttach.Read();
                                                        
                                                    %>
                                                  <%
                                                    iiRow=iiRow+1;
                                                  if (Repeat2__index % 2 == 0) {
                                                    rowColor = "reportEvenLine";
                                                  }else{
                                                    rowColor = "reportOddLine";
                                                   }
                                                  %>
                                                  <% if ( (iiRow==1) ) { %>
                                                  <tr  class="rowColor">
                                                    <td align="default" width="80%" class="cellTopBottomBorder">Documents</td>

                                                  </tr>
                                                  <% }%>
                                                  <tr class="rowColor">
                                                    <td align="default" width="80%">
                                                      <a href="<%=cStr(rsAttach.Item("Path"))%>" target=_blank><%=cStr(rsAttach.Item("Title"))%>&nbsp;</a></td>
                                                  </tr>
                                                  <%
                                                      
                                                  Repeat2__index = Repeat2__index + 1;

                                                }
                                                %>
                            </table>
                             <%  } %>
                              
                            <!-- #EndEditable -->
                          </td>
                        </tr>

                          </table>
</asp:Content>