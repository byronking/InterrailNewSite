<%@ Page Language="C#" MasterPageFile="~/Rebilling/Rebilling.Master" AutoEventWireup="true" CodeBehind="ReconcileRebilling.aspx.cs" Inherits="InterrailPPRS.Rebilling.ReconcileRebilling" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script language="JavaScript">


function bodyload()
{
  var frm = document.frm;

  <% if(!sViewReconcile) { %>
     // First time, set defaults
    var iend = ((frm.selSubTasks.options.length)-1);
    for (i = iend; i > -1; i--)
    {
      frm.selSubTasks.options[i].selected = true;
    }
  <% }else{ %>
    var sSubTasks = '<%=sSubTasks%>';
  var arSubTasks  = sSubTasks.split(",");
    var arLen       = arSubTasks.length;

    var iend = ((frm.selSubTasks.options.length));

    for (var j=0; j < iend; j++)
    {
    var subTaskID  = frm.selSubTasks.options[j].value;
      for (var i=0; i < arLen; i++)
      {
      var inID = arSubTasks[i].replace(" ","");
      var ID = subTaskID.replace(" ","");
      if (inID == ID)
      {
          frm.selSubTasks.options[j].selected = true;
       }
      }
    }
  <% } %>
}

function goReconcile()
{
  var from = document.frm.fromDate;
  var to   = document.frm.toDate;

  if (ValidDate(from, 1, "Date From")  != true) {return false;}
  if (ValidDate(to,   1, "Date To")    != true) {return false;}

  var dateFrom = new Date(Date.parse(document.frm.fromDate.value));
  var dateTo   = new Date(Date.parse(document.frm.toDate.value));

  if (dateFrom > dateTo)
  {
    alert(" 'Date From' must be prior to 'Date To'.");
    from.focus();
    from.select();
    return false;
  }

  document.frm.action = "ReconcileRebilling.aspx";
  //document.frm.action = "/col.aspx";
  document.frm.submit();

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
                            <% if (rsSubTask.EOF)
                               {%>
                            No subtasks available for the facility.&nbsp;&nbsp;Please
                            assign subtasks first.
                            <% }
                               else
                               { %>
                            <form name='frm' method='post' action=''>
                              <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                  <td colspan="4" class="pageTitle" align='center'>
                                    <div class="cellTopBottomBorder">Reconcile
                                      Re-billing</div>
                                  </td>
                                </tr>
                                <tr>
                                  <td width="52%" align='right' height="16">&nbsp;</td>
                                  <td width="17%" align='center' height="16">&nbsp;</td>
                                  <td width="17%" align='center' height="16">&nbsp;</td>
                                  <td width="14%" height="16">&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="52%" align='right'  >&nbsp;</td>
                                  <td width="17%" align='center' >&nbsp;</td>
                                  <td width="17%" align='center' >&nbsp;</td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="52%" align='center'  ><font color="green">Rebill
                                    SubTasks*</font></td>
                                  <td colspan="2" align='center' ><font color='green'>Date
                                    Range</font></td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr>
                                  <td rowspan="2" align='right'><font color='green'>
                                    <select name="selSubTasks" size="4" multiple style="width:220px">
                                      <%
                                   while (!rsSubTask.EOF)
                                   {
                                       rsSubTask.Read();
                                    %>
                                      <option value="<%=(rsSubTask.Item("Id"))%>" ><font color="green"><%=(rsSubTask.Item("Description"))%> (<%=Trim(rsSubTask.Item("TaskCode"))%>) - <%=(rsSubTask.Item("CustomerName"))%></font></option>
                                      <%
                                   }


                                   rsSubTask.Requery();

                                    %>
                                    </select>
                                    </font></td>
                                  <td width="17%" align='center' class='cellTopBottomBorder'>From</td>
                                  <td width="17%" align='center' class='cellTopBottomBorder'>To</td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr>
                                  <td width="17%" align='center'>
                                    <input type="text" name="fromDate" size="10" maxlength="10" value='<%=sFrom%>'>
                                  </td>
                                  <td width="17%" align='left'>
                                    <input type="text" name="toDate" size="10" maxlength="10" value='<%=sTo%>'>
                                  </td>
                                  <td width="14%" align='center'>
                                    <input type="button" name="btnDetail" value="Go" onClick="goReconcile();">
                                  </td>
                                </tr>
                                <tr>
                                  <td width="52%" align='center' valign="top">&nbsp;</td>
                                  <td colspan="2" align='center'>&nbsp;</td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr>
                                  <td colspan="3" align='left' class="required">&nbsp;*
                                    Select one or more. if (none are selected,
                                    ALL is assumed</td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <tr>
                                  <td align='right' class="required">&nbsp;</td>
                                  <td align='right' class="required">&nbsp;</td>
                                  <td align='right' class="required">&nbsp;</td>
                                  <td width="14%" align='center'>&nbsp;</td>
                                </tr>
                                <% if (sViewReconcile)
                                   { %>
                                <tr>
                                  <td align='right' class="required" colspan="4">
                                    <% ShowReconcileReport(); %> </td>
                                </tr>
                                <% } %>
                              </table>
                            </form>
                            <% } %>
                            <!-- #EndEditable --> 
                          </td>
                        </tr>
       
	                  </table>
</asp:Content>