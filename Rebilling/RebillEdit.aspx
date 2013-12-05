<%@ Page Language="C#" MasterPageFile="~/Rebilling/Rebilling.Master" AutoEventWireup="true" CodeBehind="RebillEdit.aspx.cs" Inherits="InterrailPPRS.Rebilling.RebillEdit" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script language="JavaScript">
<!--

function bodyload()
{
  <%if (rsETW.EOF) {%>
  LoadMembers();
  <%}%>

  setCustomerTask();

  OriginalFormCheckSumValue = CheckStringForForm(document.form1);
}

function bodyunload()
{
  if ( CheckStringForForm(document.form1) != OriginalFormCheckSumValue)
  {
    event.returnValue = "You have not saved your changes.";
  }
}

function goOutOfTown()
{
  //  var theList=window.open("/common/OutOfTown.aspx");
   var theList=showModalDialog("../common/OutOfTown.aspx",0,"dialogWidth:700px;dialogHeight:600px;unadorned:yes;status:no;resizable:no;help:no;dialogTop:50px;dialogLeft:50px");
   if ((theList) && (theList != ''))
         {
       var arList = theList.split('|');
           for (var i = 0; i < arList.length; i++)
              {
                    arItem = arList[i].split('~');
                    var id = arItem[1];
            var val = '*' + arItem[0];
                        var found = false;
                        for (var j = 0; j < document.form1.select8.options.length; j++)
                           {
                             if (document.form1.select8.options[j].value == id)
                                    {
                                          found = true;
                                        }
                           }
                        if (!found)
                          {
               var newOption = new Option(val, id, false, false);
               document.form1.select8.options[document.form1.select8.options.length] = newOption;
               document.form1.select8.options[(document.form1.select8.options.length) - 1].selected = true;
                          }
                }
        }
}

function goDelete()
{
  if (confirm('Are you sure you want to delete this rebill record?\nPress OK to confirm the delete.'))
  {
    frm = document.form1;
    frm.action = "<%=MM_editAction%>";
    frm.action = frm.action + '&Delete=YES';
    frm.submit();
  }
}

function goValidate()
{
  frm = document.form1;
  if (ValidDate(frm.WorkDate,             1, "Work Date")    != true) {return false;}

  <%if (rsETW.EOF) {%>
  if (ValidPositiveNumber(frm.TotalHours, 1, "Total Hours")   != true) {return false;}
  <%}%>

  if (ValidPositiveNumber(frm.TotalUnits, 1, "Units")   != true) {return false;}

  if (ValidPositiveNumber(frm.MaterialCosts, 0, "Material Costs")   != true) {return false;}

  var notes = frm.WorkDesc.value;
  if (notes.length > 250)
  {
     alert('Notes must not exceed 250 characters.');
         frm.WorkDesc.focus();
         return false;
  }

  <%if (rsETW.EOF) {%>
  var from = frm.select8;
  var iend = ((from.options.length)-1);

  frm.TeamMembers.value = "0";

  for (i = iend; i > -1; i--)
  {
        from.options[i].selected = true;
        if (i == iend)
        {
          frm.TeamMembers.value = from.options[i].value
        }
        else
        {
          frm.TeamMembers.value = frm.TeamMembers.value + ", " + from.options[i].value
        }
  }

  if (frm.TeamMembers.value == "0")
  {
        if (!confirm('No employees selected.\nNo "Employee Task Worked" records will be created.\nPress OK to save anyway.'))
        {
          return false;
        }
  }

  <%}%>

  OriginalFormCheckSumValue = CheckStringForForm(document.form1);

  <%if (rsETW.EOF) {%>
      frm.action = "<%=MM_editAction%>";
        //frm.action = "/col.aspx";
  <%}else{%>
      frm.action = "<%=MM_editAction%>";
        //frm.action = "/col.aspx";
  <%}%>

  frm.submit();
}

function goCancel()
{
  try
  {

     <% if (Request["ReturnTo"] != null && Request["ReturnTo"] != "") { %>
        self.location = '<%=Request["ReturnTo"]%>';
     <% }else{ %>
        self.location = "RebillDetail.aspx";
     <% } %>
  }
  catch(e) {}
}

function goAddRemove(direction)
{
  var frm = document.form1;
  if (direction == "ADD")
  {
    var from = frm.select7;
    var to   = frm.select8;
  }
  else
  {
    var from = frm.select8;
    var to   = frm.select7;
  }

  var iend = ((from.options.length)-1);

  for (i = iend; i > -1; i--)
  {
    if (from.options[i].selected)
        {
          var id = from.options[i].value;
          var val = from.options[i].text;
          var newOption = new Option(val, id, false, false);
      to.options[to.options.length] = newOption;
          to.options[(to.options.length) - 1].selected = true;
          from.options[i] = null;
        }
  }
}

function clearLists()
{
  var frm = document.form1;
  var from = frm.select7;
  var iend = ((from.options.length)-1);

  for (i = iend; i > -1; i--)
  {
    from.options[i] = null;
  }

  var from = frm.select8;
  var iend = ((from.options.length)-1);

  if (frm.select9.selectedIndex == 0)
  {
    frm.TeamMembers.value = "0";
    for (i = iend; i > -1; i--)
    {
      from.options[i] = null;
    }
  }
}

function LoadMembers()
{
  var frm = document.form1;

  var allEmp      = frm.selectAll;
  var toAvailable = frm.select7;
  var toMembers   = frm.select8;
  var from        = frm.select9;

  var teamMembers = from.options[from.selectedIndex].value;
  var arMembers   = teamMembers.split(",");

  // Clear Available and Team Members Lists
  clearLists();

  var arLen       = arMembers.length;
  var allLen      = allEmp.options.length;

  // Clear list of selected employees
  for (var j=0; j < allLen; j++)
  {
    allEmp.options[j].selected = false;
  }

  // Set as selected the Team Members
  for (var j=0; j < allLen; j++)
  {
        var allID  = allEmp.options[j].value;
        var allVal = allEmp.options[j].text;
    var newOption = new Option(allVal, allID, false, false);

    for (var i=0; i < arLen; i++)
    {
          var memID = arMembers[i].replace(" ","");
          var empID = allID.replace(" ","");
          if (memID == empID)
          {
        allEmp.options[j].selected = true;
          }
    }
  }

  // if employee is in the array AND not already in list, move to Team Members
  // else move to list of available employees

  for (var j=0; j < allLen; j++)
  {
        var allID  = allEmp.options[j].value;
        var allVal = allEmp.options[j].text;
    var newOption = new Option(allVal, allID, false, false);

           bNotIn = true;
       var toLen = toMembers.options.length;
           for (var k=0; k < toLen; k++)
           {
             if (toMembers.options[k].value == allID) { bNotIn = false; }
           }

    if ( (allEmp.options[j].selected) && (bNotIn) )
        {
       toMembers.options[toMembers.options.length] = newOption;
        }
        else
        {
          if (bNotIn) { toAvailable.options[toAvailable.options.length] = newOption; }
        }
  }
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
                            <% if (rsTask.EOF) { %>
                            No Rebill subtasks available for the facility.&nbsp;Please
                            add subtasks to the facility first.
                            <% }else{ %>
                            <form method="POST" action="<%=MM_editAction%>" name="form1">
                              <table align="center" width="98%" border="0">
                                <tr valign="baseline">
                                  <td align="right" colspan="4">
                                    <table border="0" cellspacing="0" cellpadding="0" width="100%" >
                                      <tr>
                                        <td align="default" colspan="4">
                                          <div align="center" class="pageTitle"><b>Latest
                                            'Open' Rebill Detail Records</b></div>
                                        </td>
                                      </tr>
                                      <tr>
                                        <td align="default" colspan="4">&nbsp;</td>
                                      </tr>
                                      <tr>
                                        <td colspan=3 valign="top"  width="40%">
                                          <table border="0" cellspacing="0" cellpadding="0" width="100%" >
                                            <%string rowColor;%>
                                            <%
                                                
                                            int iRow = 0;
                                            int iiRow = 0;
                                                                          %>
                                            <%
                                            while ((Repeat1__numRows != 0) && (!rsLast.EOF)){
                                               rsLast.Read();  
                                               iRow = iRow+1;
                                            if (iRow == 1) {
                                            %>
                                            <tr>
                                              <td align="default" width="10%">&nbsp;</td>
                                              <td align="left" width="40%" class="cellTopBottomBorder">Work
                                                Date </td>
                                              <td align="center" width="40%"  class="cellTopBottomBorder">SubTask</td>
                                            </tr>
                                            <% } %>
                                            <% if (rsLast.Item("ID") == Request["ID"] ) { %>
                                            <tr class="reportOddLine">
                                              <%}else{%>
                                            <tr>
                                              <%}%>
                                              <td align="default" width="10%">
                                                <%if (rsLast.Item("ID") == Request["ID"] ) { %>
                                                <img src="../images/check.gif">
                                                <%}else{%>
                                                &nbsp;
                                                <%}%>
                                              </td>
                                              <td align="left" width="40%">
                                                <a href="RebillEdit.aspx?Id=<%=rsLast.Item("Id") %>"><%=Trim(Replace(rsLast.Item("WorkDate"), "12:00:00 AM", ""))%></a> </td>
                                              <td align="center" width="40%"><%=(rsLast.Item("Description"))%></td>
                                            </tr>
                                            <%
                                              Repeat1__index=Repeat1__index+1;
                                              Repeat1__numRows=Repeat1__numRows-1;
  
                                              }
                                            %>
                                          </table>
                                        </td>
                                        <td width="60%" valign="top" class="reportOddLine">
                                          <table  border="0" cellspacing="0" cellpadding="0" width="100%" >
                                            <tr>
                                              <td  class="cellTopBottomBorder" align="center">Employee
                                                Task Worked</td>
                                            </tr>
                                            <tr>
                                              <td align="default" valign="top" width="100%">
                                                <%if ( rsETW.LastReadSuccess) { %>
                                                <%if (rsETW.Item("RebillDetailID") == cStr(Request["ID"])) {%>
                                                <!-- BXM -->
                                                <table align="center"  border="0" cellspacing="0" cellpadding="0" width="100%">
                                                  <%
                                                while ((Repeat2__numRows != 0) && ( rsETW.LastReadSuccess)){
                                                    
                                                  

                                                  iiRow=iiRow+1;
                                                    
                                                  if (Repeat2__index % 2 == 0) {
                                                    rowColor = "reportEvenLine";
                                                  }else{
                                                    rowColor = "reportOddLine";
                                                  }
                                                    
                                                  if ( (iiRow == 1) ) { %>

                                                  <tr  class="reportOddLine">
                                                    <td align="default" width="80%" class="cellTopBottomBorder">Name</td>
                                                    <td align="center" width="20%" class="cellTopBottomBorder">Hours</td>
                                                  </tr>
                                                  <% } %>
                                                  <tr class="reportOddLine">
                                                    
                                                    <td align="default" width="80%">
                                                      <a href="../PayRoll/TaskWorkedEdit.aspx?Type=Rebill&RebillDetailID=<%=cStr(rsETW.Item("RebillDetailID"))%>&WorkDate=<%=DateTime.Parse(cStr(rsETW.Item("WorkDate"))).ToString("MM/dd/yyyy")%>&Shift=<%=cStr(rsETW.Item("ShiftID"))%>&ID=<%=cStr(rsETW.Item("ID")) + sReturnTo + cStr(Request["ID"])%>" >
                                                      <%=(rsETW.Item("LastName"))%>, <%=(rsETW.Item("FirstName"))%></a></td>
                                                    <td align="center" width="20%">
                                                      <%=(rsETW.Item("HoursWorked"))%> </td>
                                                  </tr>
                                                  <%
                                                  Repeat2__index=Repeat2__index+1;
                                                  Repeat2__numRows=Repeat2__numRows-1;
                                                  rsETW.Read();
                                                  }
                                                %>
                                                  <%rsETW.Requery(); rsETW.Read();%>
                                                </table>
                                                <!-- BXM End -->
                                                <%}%>
                                                <%} else{%>
                                                None.
                                                <%}%>
                                              </td>
                                            </tr>
                                          </table>
                                        </td>
                                      </tr>
                                    </table>
                                    <!-- BXM End -->
                                  </td>
                                </tr>
                                <tr valign="baseline">
                                  <td align="right" colspan="4">
                                    <div align="right"><a href="RebillEdit.aspx?id=0">Add
                                      New Rebill Detail</a></div>
                                  </td>
                                </tr>
                                <!-- New End here...-->
                                <tr valign="baseline">
                                  <td align="right" colspan="4" class="cellTopBottomBorder">
                                    <div align="center"  class="pageTitle">Rebill
                                      Detail</div>
                                  </td>
                                </tr>
                                <tr valign="baseline">
                                  <td align="right" width="55">
                                    <div   class="required">Work Date:</div>
                                  </td>
                                  <td width="178">
                                    <% string DefDate = ""; %>
                                    <% if ( isDate(cStr(Request["DefDate"]) ) ) {
                                        DefDate = Request["DefDate"];
                                       }else{
                                        DefDate = System.DateTime.Now.ToShortDateString();
                                       }
                                  %>
                                    <input type="text" name="WorkDate" value='<% if (Request["ID"] != "0") { Response.Write (Trim(Replace(rs.Item("WorkDate"),"12:00:00 AM","")));} else { Response.Write(DefDate); } %>' size="10" maxlength="10">
                                  </td>
                                  <td width="49" align="right" class="required">SubTask:</td>
                                  <td width="178">
                                    <select name="select" style="width:170px" onChange="setCustomerTask();">
                                      <%
                                         string aRebillTypes = "";

                                        while (! rsTask.EOF){
                                            rsTask.Read();
                                           if (aRebillTypes == "") {
                                              aRebillTypes = rsTask.Item("HoursOrUnits");
                                           }else{
                                              aRebillTypes = aRebillTypes + "|" +  rsTask.Item("HoursOrUnits");
                                           }
                                            %>
                                      <%
                                            string sDefTaskID = "";
                                            if (Request["ID"] == "0" && (! rsDef.EOF) ) {
                                                    sDefTaskID = (rsDef.Item("RebillSubTasksId"));
                                            }else{
                                                    sDefTaskID = (rs.Item("RebillSubTasksId"));
                                            }

                                            %>
                                      <option value="<%=(rsTask.Item("ID"))%>" <%if (rsTask.Item("ID") == sDefTaskID) { Response.Write("SELECTED");} else { Response.Write("");}%> ><%=(rsTask.Item("Description"))%></option>
                                      <%
                                          }
                                          

                                      rsTask.Requery();

                                    %>
                                    </select>
                                    <input type="hidden" name="aRebillTypes" value='<%= aRebillTypes %>'>

                                  </td>
                                </tr>
                                <tr valign="baseline">
                                  <td align="right" width="55" class="required" height="21">Shift:</td>
                                  <td width="178" align="left" height="21">
                                    <%
                                   string sDefShiftID = "";
                                    if (Request["ID"] == "0" && (!rsDef.EOF) ) {
                                            sDefShiftID = cStr(rsDef.Item("ShiftId"));
                                    }else{
                                            sDefShiftID = rs.Item("ShiftId");
                                    }
                                    %>
                                    <select name="selShift" style="width:65px">
                                      <%
                                     while (! rsShifts.EOF){
                                            rsShifts.Read();
                                        %>
                                      <option value="<%=(rsShifts.Item("ID"))%>" <%if (Trim(rsShifts.Item("ID")) == Trim(sDefShiftID)){ Response.Write("SELECTED");} else {Response.Write("");}%>><%=(rsShifts.Item("Shift"))%></option>
                                      <%
                                    }
  
                                      rsShifts.Requery();

                                    %>
                                    </select>
                                  </td>
                                  <td width="49" align="right" height="21">Task:</td>
                                  <td height="21" width="178">
                                    <div id="zTask">&nbsp;</div>
                                  </td>
                                </tr>


                                <tr>
                                  <td align="right" width="55" class="required">Hours:</td>
                                  <td width="178">
                                    <input type="text" name="TotalHours" value="<%=(rs.Item("TotalHours"))%>" size="10" maxlength="15">
                                  </td>
                                  <td width="49" align="right">Customer:</td>
                                  <td height="19" width="178">
                                    <div id="zCustomer">&nbsp;</div>
                                  </td>
                                </tr>

                                <!-- if (subtask is rebill type is Units, then get Total Units -->

                                <% nUnits = rs.Item("TotalUnits"); %>

                                <tr>
                                  <td align="right" width="55" class="required"><div id="divTotalUnitslbl" style="{visibility:hidden}">Units:</div></td>
                                  <td width="178">
                                     <div id="divTotalUnits" style="{visibility:hidden}">
                                      <input type="text" name="TotalUnits" value="<%= nUnits %>" size="10" maxlength="15">
                                     </div>
                                  <td width="49" align="right">&nbsp;</td>
                                  <td height="19" width="178">&nbsp;</td>
                                </tr>

                                <tr>
                                  <td align="right" width="55">Invoice Number:</td>
                                  <td width="178">
                                    <input type="text" name="InvoiceNumber" value="<%=(rs.Item("InvoiceNumber"))%>" size="20" maxlength="20">
                                  </td>
                                  <td width="49" align="right">&nbsp;</td>
                                  <td height="19" width="178">
                                    &nbsp;
                                  </td>
                                </tr>
                                <tr>
                                  <td align="right" width="55">Material Costs:</td>
                                  <td width="178">
                                    <input type="text" name="MaterialCosts" value="<%=(rs.Item("MaterialCosts"))%>" size="20" maxlength="15">
                                  </td>
                                  <td width="49" align="right">Vendors:&nbsp;</td>
                                  <td height="19" width="178">
                                    <input type="text" name="Vendors" value="<%=(rs.Item("Vendors"))%>" style="width:170px" maxlength="50">
                                  </td>
                                </tr>
                                <tr><td colspan=4>&nbsp;</td></tr>

                                <%if (rsETW.EOF) { %>
                                <tr valign="baseline">
                                  <td align="right" width="55">&nbsp;</td>
                                  <td colspan="2" align="right"> Add Team:</td>
                                  <td width="178">
                                    <select name="select9"  style="width:170px" onChange="LoadMembers();">
                                      <%
                                      while (! rsTeams.EOF){
                                        rsTeams.Read();
                                      %>
                                      <option value="<%=(rsTeams.Item("TeamMembers"))%>"><%=(rsTeams.Item("TeamName"))%></option>
                                      <%
                                          }
                                          

                                      rsTeams.Requery();

                                    %>
                                    </select>
                                  </td>
                                </tr>
                                <tr valign="baseline">
                                  <td nowrap align="right" width="55">&nbsp;</td>
                                  <td width="178" class="cellTopBottomBorder">Available Employees</td>
                                  <td width="49">&nbsp;</td>
                                  <td width="178" class="cellTopBottomBorder">
                                    <div>Selected Employees</div>
                                  </td>
                                </tr>
                                <tr valign="baseline">
                                  <td align="left" width="55">&nbsp;</td>
                                  <td rowspan="4" width="178">
                                    <select name="select7" multiple size="6" style="width:170px" title="Available Employees">
                                      <option value="  "> </option>
                                    </select>
                                  </td>
                                  <td width="49">&nbsp;</td>
                                  <td rowspan="4" width="178">
                                    <select name="select8" multiple size="6" style="width:170px" title="Team Members">
                                      <option value=" "> </option>
                                    </select>
                                  </td>
                                </tr>
                                <tr valign="baseline">
                                  <td align="right" width="55">&nbsp; </td>
                                  <td width="49" align="center">
                                    <input type="button" name="btnAdd" value=">>" onClick="goAddRemove('ADD');">
                                  </td>
                                </tr>
                                <tr valign="baseline">
                                  <td align="right" width="55">&nbsp;</td>
                                  <td width="49" align="center">
                                    <input type="button" name="btnRemove" value="<<" onClick="goAddRemove('REMOVE');" >
                                  </td>
                                </tr>
                                <tr valign="baseline">
                                  <td align="right" width="55">&nbsp;</td>
                                  <td width="49">&nbsp;</td>
                                </tr>
                                <tr valign="baseline">
                                  <td align="right" valign='top'>&nbsp;</td>
                                  <td>
                                    <input type="button" name="bntOutOfTown" value="Add Out of Town Employees" onClick="goOutOfTown();">
                                  </td>
                                  <td>&nbsp;</td>
                                  <td>&nbsp;</td>
                                </tr>
                                <% } %>
                                
                                <tr valign="baseline">
                                  <td align="right" valign='top'>Rebilled: </td>
                                  <td colspan="3">
                                    <input <%if (rs.Item("Rebilled") != null && cStr(rs.Item("Rebilled")).Length > 0) { Response.Write("CHECKED") ;} else {Response.Write(""); }%> type="checkbox" name="Rebilled" value=1 ID="Checkbox2">
                                  </td>
                                </tr>
                                
                                <tr valign="baseline">
                                  <td align="right" valign='top'>Desc.: </td>
                                  <td colspan="3">
                                    <textarea name="WorkDesc" rows="3" cols="40" style="width:400px"><%=(rs.Item("WorkDescription"))%></textarea>
                                  </td>
                                </tr>
                                <tr valign="baseline">
                                  <td align="right" width="55">&nbsp;</td>
                                  <td width="178">&nbsp;</td>
                                  <td width="49" align="right">&nbsp;</td>
                                  <td width="178" align="left" >&nbsp;</td>
                                </tr>
                                <tr valign="baseline">
                                  <td align="right" width="55">Last Modified:</td>
                                  <td width="178"> <%=(rs.Item("LastModifiedOn"))%> </td>
                                  <td width="49" align="right">By:</td>
                                  <td width="178" align="left" ><%=(rs.Item("LastModifiedBy"))%></td>
                                </tr>
                                <tr valign="baseline">
                                  <td nowrap align="right" width="55"></td>
                                  <td colspan="3" align="center">
                                    <% if ((Request["ID"] != "0") && (UCase(Trim(rs.Item("RebillStatus"))) == "OPEN")) { %>
                                    <input type="button" value="Delete" onClick="goDelete();" name="btnDelete">
                                    <% } %>
                                    <input type="button" value="Save" onClick="goValidate();" name="button">
                                    <input type="button" name="btnCancel" value="Cancel" onClick="goCancel();">
                                  </td>
                                </tr>
                              </table>
                              <input type="hidden" name="MM_upd" value="true">
                              <input type="hidden" name="MM_rec" value="<%= rs.Item("Id") %>">
                              <input type="hidden" name="LastModifiedOn" value="<%=System.DateTime.Now%>" size="32">
                              <input type="hidden" name="LastModifiedBy" value="<%=Session["UserName"]%>" size="32">
                              <input type="hidden" name="FacilityID" value="<%=Session["FacilityID"]%>" size="32">
                              <input type="hidden" name="TeamMembers" value="0" size="32">
                              <div id="Layer1" style="position:absolute; width:1px; height:1px; z-index:1; visibility: hidden; left: 278px; top: 907px; overflow: hidden">
                                <select name="allTasks">
                                  <%
                                while (! rsTask.EOF){
                                    rsTask.Read();
                                %>
                                  <option value="<%=(rsTask.Item("TaskID"))%>"  ><%=(rsTask.Item("TaskDescription"))%>(<%=(rsTask.Item("TaskCode"))%>)</option>
                                  <%
                                 }
                                  rsTask.Requery();

                                %>
                                </select>
                                <select name="allCustomers">
                                  <%
                                while (! rsTask.EOF){
                                    rsTask.Read();
                                %>
                                <option value="<%=(rsTask.Item("FacilityCustomerId"))%>"  ><%=(rsTask.Item("CustomerName"))%> (<%=(rsTask.Item("CustomerCode"))%>)</option>
                                  <%
                                }
                                      
                                  rsTask.Requery();

                                %>
                                </select>
                                <select name="selectAll" multiple size=2>
                                  <%
                                while (! rsEmp.EOF){
                                    rsEmp.Read();
                                  %>
                                  <option value="<%=(rsEmp.Item("Id"))%>"><%=Trim(rsEmp.Item("LastName"))%>, <%=Trim(rsEmp.Item("FirstName"))%> (<%=Trim(rsEmp.Item("EmployeeNumber"))%>)<% if(Trim(rsEmp.Item("FacilityId")) == (Session["FacilityID"] + "")) { %> "" <% }else{ %> " + " <% } %></option>
                                  <%
                                      }
                                      

                                  rsEmp.Requery();

                                %>
                                </select>
                              </div>
                              <input type="hidden" name="MM_update" value="true">
                              <input type="hidden" name="MM_recordId" value="<%= rs.Item("Id") %>">
                            </form>
                                                <!-- BXM -->
                                            <% if ( Request["ID"] != "0" ){ %>                                                
                                                <table align="center"  border="0" cellspacing="0" cellpadding="0" width="100%">
                                                  <%
                                                  iiRow=0;


                                                while  ( rsAttach != null && ! rsAttach.EOF){
                                                    rsAttach.Read();

                                               iiRow=iiRow+1;
                                              if (Repeat2__index % 2 == 0) {
                                                rowColor = "reportEvenLine";
                                              }else{
                                                rowColor = "reportOddLine";
                                              }
                                              %>
                                                  <%if ( iiRow==1 ) { %>
                                                  <tr  class="rowColor">
                                                    <td align="default" width="80%" class="cellTopBottomBorder">Documents</td>

                                                  </tr>
                                                  <%}%>
                                                  <tr class="rowColor">
                                                    <td align="default" width="80%">
                                                      <a href="<%=cStr(rsAttach.Item("Path"))%>" target=_blank><%=cStr(rsAttach.Item("Title"))%>&nbsp;</a></td>
                                                  </tr>
                                                  <%
                                              Repeat2__index=Repeat2__index+1;
                                              }
                                            %>
                            
                            <tr>
                            <td>
                            <form action="ManageAttachments.aspx" method=post>
                            <input type="hidden" name="connectionstring" value="<%=HttpContext.Current.Session["dbPath"].ToString()%>">
                            <input type="hidden" name="RebillId" value="<%=rs.Item("Id")%>">
                            <input type="hidden" name="CompanyName" value="<%=Session["CompanyName"]%>">
                            <input type=submit value="Add/Remove Documents">
                            </form>
                            </td>
                            </tr>
                            </table>
                             <% } else { %>
                            <table align="center"  border="0" cellspacing="0" cellpadding="0" width="100%">
                             <tr >
                             <td align="default" width="80%" class="cellTopBottomBorder">You must Save Rebill Detail before you can add documents.</td>
                            </td>
                            </tr>
                            </table>
                           <% } %>
                            Fields in <span class="Required">RED</span> are required.
                            <% } %>
                            <!-- #EndEditable -->
                          </td>
                        </tr>

                          </table>

                          <script language="JavaScript">
                              function setCustomerTask() {
                                  frm = document.form1;

                                  var tin = frm.select;
                                  var iend = ((tin.options.length) - 1);

                                  document.all['zTask'].innerHTML = "";
                                  document.all['zCustomer'].innerHTML = "";

                                  var aRebillTypes = frm.aRebillTypes.value.split("|");

                                  for (i = iend; i > -1; i--) {
                                      if (tin.options[i].selected) {
                                          frm.allTasks.options[i].selected = true;
                                          frm.allCustomers.options[i].selected = true;
                                          document.all['zTask'].innerHTML = frm.allTasks.options[i].text;
                                          document.all['zCustomer'].innerHTML = frm.allCustomers.options[i].text;
                                          if (aRebillTypes[i] == "U") {
                                              document.all['divTotalUnitslbl'].style.visibility = "visible";
                                              document.all['divTotalUnits'].style.visibility = "visible";
                                              frm.TotalUnits.value = "<%= nUnits %>";
                                          }
                                          else {
                                              document.all['divTotalUnitslbl'].style.visibility = "hidden";
                                              document.all['divTotalUnits'].style.visibility = "hidden";
                                              frm.TotalUnits.value = 0;
                                          }
                                      }

                                  }
                              }
                         </script>

</asp:Content>