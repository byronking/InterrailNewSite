<%@ Page Language="C#" MasterPageFile="~/Payroll/Payroll.Master" AutoEventWireup="true" CodeBehind="Payroll.aspx.cs" Inherits="InterrailPPRS.Payroll.Payroll1" %>



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
<table align="center" border="0" cellpadding="0" cellspacing="0" width="85%">
    <tr> 
    <td align="default" colspan="5"> 
        <div align="center" class="pageTitle">Daily 
        Payroll</div>
    </td>
    </tr>
    <tr> 
    <td align="default" colspan="2"><%=GetPayLinks(PWorkDate)%></td>
    <td align="right" width="24%">Shift : 
        <% if ( System.Convert.ToInt32(PShift)  > 1){ %>
        <a href="payroll.aspx?workdate=<%=PWorkDate%>&shift=<%= (System.Convert.ToInt32(PShift) + -1)%>">&lt;</a> 
        <% } %>
        <%=PShift%> 
        <% if ( System.Convert.ToInt32(PShift) < 4 ){ %>
        <a href="payroll.aspx?workdate=<%=PWorkDate%>&shift=<%= (System.Convert.ToInt32(PShift) + 1) %>">&gt;</a> 
        <% } %>
    </td>
    <td align="right" colspan="2"><a href="Payemployee.aspx?WorkDate=<%=PWorkDate%>&Shift=<%=PShift%>&NewTask=Yes">Add New</a></td>
    </tr>
    <tr> 
    <td align="default" width="15%">&nbsp;</td>
    <td align="default" colspan="4"> 
        <div align="right"></div>
    </td>
    </tr>
    <tr> 
    <td align="default" width="15%">&nbsp;</td>
    <td align="default" width="25%">&nbsp;</td>
    <td align="default" width="24%">&nbsp;</td>
    <td align="default" colspan="2">&nbsp;</td>
    </tr>

   <%
       
   string rowColor;

  //if(Repeat1__index % 2 == 0) {
 //   rowColor = "reportEvenLine";
  //}else{  
  //  rowColor = "reportOddLine";
  //}

  string TaskandUnit = GetTasksandUnit();
  string RebillTaks  = GetRebillTasks();
  string OtherTasks =  GetOtherTasks();
  string AdditionalTasks = GetAdditionalTasks();

  if ( TaskandUnit.Length > 0){ %>
     <tr> 
     <th align="default" width="15%"  class="cellTopBottomBorder">
        <div align="left">Task</div>
        </th>
       <th align="default" width="25%"  class="cellTopBottomBorder"> 
       <div align="left">Count</div>
           </th>
       <th align="default" width="24%"  class="cellTopBottomBorder"> 
       <div align="left">Units</div>
       </th>
       <th align="default" width="17%"  class="cellTopBottomBorder"> 
       <div align="left">People </div>
       </th>
       <th align="default" width="19%"  class="cellTopBottomBorder">Hours</th>
       </tr>

        <%=TaskandUnit%> 
        <% } %>
 
 
 
      <tr> 
      <th align="default" width="35%"  class="cellTopBottomBorder">
       <div align="left">Rebill Task <a href="../Rebilling/Rebilledit.aspx?ID=0&DefDate=<%=PWorkDate%>">Add New</a></div>
       </th>
      <th align="default" width="15%"  class="cellTopBottomBorder"> 
      <div align="left">Hours</div>
          </th>
      <th align="default" width="14%"  class="cellTopBottomBorder"> 
      <div align="left">&nbsp;</div>
      </th>
      <th align="default" width="17%"  class="cellTopBottomBorder"> 
      <div align="left">People </div>
      </th>
      <th align="default" width="19%"  class="cellTopBottomBorder">Hours</th>
      </tr>

       <%=RebillTaks%> 
 
 
 
 
 <%  if( AdditionalTasks.Length > 0){ %>
    <tr> 
     <th align="default" width="15%"  class="cellTopBottomBorder">
        <div align="left">Additional Task</div>
        </th>
       <th align="default" width="25%"  class="cellTopBottomBorder"> 
       <div align="left">&nbsp;</div>
           </th>
       <th align="default" width="24%"  class="cellTopBottomBorder"> 
       <div align="left">&nbsp;</div>
       </th>
       <th align="default" width="17%"  class="cellTopBottomBorder"> 
       <div align="left">People </div>
       </th>
       <th align="default" width="19%"  class="cellTopBottomBorder">Hours</th>
       </tr>
         <%=AdditionalTasks%> 
 <% } %>





 <%  if ( OtherTasks.Length > 0 ){ %>
    <tr> 
     <th align="default" width="15%"  class="cellTopBottomBorder">
        <div align="left">Other Task</div>
        </th>
       <th align="default" width="25%"  class="cellTopBottomBorder"> 
       <div align="left">&nbsp;</div>
           </th>
       <th align="default" width="24%"  class="cellTopBottomBorder"> 
       <div align="left">&nbsp;</div>
       </th>
       <th align="default" width="17%"  class="cellTopBottomBorder"> 
       <div align="left">People </div>
       </th>
       <th align="default" width="19%"  class="cellTopBottomBorder">Hours</th>
       </tr>
                                                          
    <%=OtherTasks%> 
 <% } %>
                                                          
        <tr> 
        <td align="default" width="15%"> </td>
        <td align="default" width="25%">&nbsp;</td>
        <td align="default" width="24%">&nbsp; </td>
        <td align="right" colspan="2"></td>
        </tr>
    </table>
    <p>&nbsp;</p>
</asp:Content> 