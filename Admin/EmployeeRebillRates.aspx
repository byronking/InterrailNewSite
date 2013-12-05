<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EmployeeRebillRates.aspx.cs" Inherits="InterrailPPRS.Admin.EmployeeRebillRates" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

<script type="text/javascript">

function bodyload()
{
  OriginalFormCheckSumValue = CheckStringForForm(document.form1);
}

function bodyunload()
{
  if ( CheckStringForForm(document.form1) != OriginalFormCheckSumValue)
  {
    event.returnValue = "You have not saved your changes.";
  }
}

function showHideRate()
{
   var frm = document.form1;

   var selTask = frm.newTask.options[frm.newTask.selectedIndex].value;
   if (selTask == 0)
   {
     frm.newUR.value     = "";
         frm.newUR.disabled  = true;
     frm.newHR.value     = "";
         frm.newHR.disabled  = true;
     frm.newEFD.value    = "";
     frm.newEFD.disabled = true;
   }
   else
   {
         frm.newUR.disabled  = false;
         frm.newHR.disabled  = false;
     frm.newEFD.disabled = false;
   }

   frm.newRateType.options[frm.newTask.selectedIndex].selected = true;
   var rateType = frm.newRateType.options[frm.newRateType.selectedIndex].text;

   if (rateType == "HOURS")
   {
          document.all["Type"].innerHTML  = "Hourly Rate";
          frm.newUR.value     = "0";
          frm.newUR.disabled  = true;
          frm.newHR.disabled  = false;
      frm.newEFD.disabled = false;
          frm.newHR.focus();
   }

   if (rateType == "UNITS")
   {
          document.all["Type"].innerHTML  = "Units Rate";
          frm.newHR.value     = "0";
          frm.newHR.disabled  = true;
          frm.newUR.disabled  = false;
      frm.newEFD.disabled = false;
          frm.newUR.focus();
   }

   if (rateType == "EITHER")
   {
      if (frm.newTask.selectedIndex == 0)
          {
            document.all["Type"].innerHTML  = "";
      }
          else
          {
            document.all["Type"].innerHTML  = "Units/Hourly";
            frm.newHR.disabled  = false;
            frm.newUR.disabled  = false;
            frm.newEFD.disabled = false;
            frm.newUR.focus();
          }
   }
}

function goAdd()
{
  frm = document.form1;

  var pType = frm.newRateType.options[frm.newRateType.selectedIndex].text;
  frm.newPType.value = pType;
  var selTask = frm.newTask.options[frm.newTask.selectedIndex].value;

  if (selTask != 0)
  {
         if ( (pType == "UNITS") || (pType == "EITHER") )
         {
       if (ValidPositiveNumber(document.all['newUR'], 1, "Units Pay Rate") != true) {return false;}
       if ( (document.all['newUR'].value > 100.0) || (document.all['newUR'].value < 0.0) )
       {
         alert("Units Pay Rate must be greater than or equal to 0.0 but less than 100.0");
         document.all['newUR'].focus();
         document.all['newUR'].select();
         return false;
       }
         }

         if ( (pType == "HOURS") || (pType == "EITHER") )
         {
       if (ValidPositiveNumber(document.all['newHR'], 1, "Hourly Pay Rate") != true) {return false;}
       if ( (document.all['newHR'].value > 100.0) || (document.all['newHR'].value < 0.0) )
       {
         alert("Hourly Pay Rate must be greater than or equal to 0.0 but less than 100.0");
         document.all['newHR'].focus();
         document.all['newHR'].select();
         return false;
       }
         }

     if (ValidDate(document.all['newEFD'], 1, "Effective Date")  != true) {return false;}
  }

  if (goValidate())
  {

    OriginalFormCheckSumValue = CheckStringForForm(document.form1);

    frm.action = "EmployeeRebillRates.aspx?Action=ADDNEWRATE";
   // frm.action = "/col.aspx?Action=ADDNEWRATE";
    frm.submit();
  }
}

function goSave()
{
  frm = document.form1;
  var selTask = frm.newTask.options[frm.newTask.selectedIndex].value;

  if (selTask != 0)
  {
    if (goAdd() != true) {return false;}
  }

  if (goValidate())
  {
    OriginalFormCheckSumValue = CheckStringForForm(document.form1);

    frm.action = "EmployeeRebillRates.aspx?Action=UPDATE";
    frm.submit();
  }
}

function goEmployeeRebillRates()
{

  frm = document.frmSelect;
  var empID = frm.selEmployee.options[frm.selEmployee.selectedIndex].value;
  var shiftID = frm.selShiftID.options[frm.selShiftID.selectedIndex].value;
  var iend = frm.selEmployee.options.length-1;

  for (i = 0; i < iend; i++)
  {
    if (frm.selEmployee.options[i].value == '<%=Request["ID"]%>')
        {
            frm.selEmployee.options.selectedIndex = i;
        }
  }

  self.location = "EmployeeRebillRates.aspx?ID="+empID+"&ShiftID="+shiftID;
}

function goValidate()
{
  frm = document.form1;
  var nRec = frm.NRec.value;

  for (var j=0; j < nRec; j++)
  {

         <%if (InStr(0, "Super", System.Convert.ToString(Session["UserType"]), 1) >=1) { %>
            if (document.all['DELETE_'+(j+1)].checked)
         <% }else{ %>
            if  (1==2)
         <% } %>
         {
            // skip, do not validate
         }
         else
         {
           var pType = document.all['PT_'+(j+1)].value;
           if ( (pType == "UNITS") || (pType == "EITHER") )
           {
              if (ValidPositiveNumber(document.all['UR_'+(j+1)], 1, "Units Pay Rate") != true) {return false;}
              if ( (document.all['UR_'+(j+1)].value > 100.0) || (document.all['UR_'+(j+1)].value < 0.0) )
              {
                   alert("Units Pay Rate must be greater than or equal to 0.0 but less than 100.0");
                   document.all['UR_'+(j+1)].focus();
                   document.all['UR_'+(j+1)].select();
                   return false;
              }
            }

           if ( (pType == "HOURS") || (pType == "EITHER") )
           {
                 if (ValidPositiveNumber(document.all['HR_'+(j+1)], 1, "Hourly Pay Rate") != true) {return false;}
                 if ((document.all['HR_'+(j+1)].value > 100.0) || (document.all['HR_'+(j+1)].value < 0.0) )
                 {
                   alert("Hourly Pay Rate must be greater than or equal to  0.0 but less than 100.0");
                   document.all['HR_'+(j+1)].focus();
                   document.all['HR_'+(j+1)].select();
                   return false;
                 }
           }

            if (ValidDate(document.all['EFD_'+(j+1)], 1, "Effective Date")  != true) {return false;}
         }
  }
  return true;
}

function goCancel()
{
  try
  {
    self.location = "Employee.aspx";
  }
  catch(e) {}
}

// -->
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
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
            <form name="frmSelect" method="post" action="">
            <tr>
                <td align="default" width="15%" >Go to: <a href="EmployeeRates.aspx?ID=<%=Request.QueryString["ID"]%>&Shift=<%=sDefShiftID%>">Task Rates</a></td>
                <td align="default" >&nbsp;</td>
                <td align="default" colspan="3" >&nbsp;</td>
            </tr>
            <tr>
                <td align="default" class="cellBottomBorder">
                Rebill Task Rates for:</td>
                <td align="default" colspan="4" class="cellBottomBorder">
                <table  width="90%" border="0" cellspacing="0" cellpadding="8">
                <tr>
                <td>
                <select name="selEmployee" onChange='goEmployeeRebillRates();'>
                    <%
                    while (! rsEmployee.EOF){
                        rsEmployee.Read();
                    %>
                        <option value="<%=rsEmployee.Item("Id") %>" <% if (cStr(rsEmployee.Item("Id")) == cStr(Request["ID"])) { Response.Write("SELECTED"); } else { Response.Write(""); }%>><%=rsEmployee.Item("LastName") %>, <%=rsEmployee.Item("FirstName") %> (<%=rsEmployee.Item("EmpNum") %> <% if (rsEmployee.Item("FromAssociatedFacility") == "1" ){ Response.Write("+"); } %> &nbsp; <%=(rsEmployee.Item("SalariedEmployee")) %> &nbsp;  <%=(rsEmployee.Item("EmpStatus")) %></option>
                    <%
                    }
                    
                    rsEmployee.Requery();
 
                    %>
                    </select>
                    </td>
                    <td>
                    Shift:
                            <select name="selShiftID" style="width:65px"   onChange='goEmployeeRebillRates();'>
                    <%
                    while (! rsShifts.EOF){
                        rsShifts.Read();
                    %>
                            <option value="<%=(rsShifts.Item("ID"))%>" <%if (Trim(rsShifts.Item("ID")) == Trim(sDefShiftID)) { Response.Write("SELECTED"); } else { Response.Write(""); }%>><%=rsShifts.Item("Shift")%></option>
                    <%
                    }
                    
                    rsShifts.Requery();

                    %>
                    </select>
                </td>
                </tr>
                </table>
                </td>
            </tr>
            <tr>
                <td align="default" width="15%" >&nbsp;</td>
                <td align="default" >&nbsp;</td>
                <td align="default" colspan="3" >&nbsp;</td>
            </tr>
            </form>
            <form name="form1" method="post" action="">
            <tr>
                <td align="default" width="15%" >&nbsp;</td>
                <td align="default" >&nbsp; </td>
                <td align="default" colspan="3" >
                <input type="button" name="btnSave" value="Save" onclick="goSave();" />
                <input type="button" name="btnCancel" value="Cancel" onclick="goCancel();" />
                </td>
            </tr>
            <tr>
                <td align="default" width="15%" >&nbsp;</td>
                <td align="default" width="15%" >&nbsp;</td>
                <td align="default" width="15%" >&nbsp;</td>
                <td align="default" width="20%" >&nbsp;</td>
                <td align="default" width="15%" >&nbsp;</td>
            </tr>
            <tr>
                <td align="default" width="15%" class="cellTopBottomBorder">Task
                Code</td>
                <td align="default" width="15%" class="cellTopBottomBorder">Units
                Rate</td>
                <td align="default" width="15%" class="cellTopBottomBorder">Hourly
                Rate</td>
                <td align="default" width="20%" class="cellTopBottomBorder">
                Effective Date</td>
                <td align="default" width="15%" class="cellTopBottomBorder">Delete?</td>
            </tr>
            <tr class="reportOddLine">
                <td align="default" width="15%" >
                <select name="newTask"  onChange="showHideRate();" style="width:155;">
                    <%
                    while (! rsTasks.EOF){
                        rsTasks.Read();
                    %>
                        <option value="<%=rsTasks.Item("Id")%>"><%=Trim(rsTasks.Item("TaskCode")) + " - " + Trim(rsTasks.Item("Description"))%></option>
                        <%
                    }

                    rsTasks.Requery();

                    %>
                </select>
                </td>
                <td align="default" width="15%" > $
                <input type="text" name="newUR" value="" size="5" maxlength="5" disabled="true">
                </td>
                <td align="default" width="15%" > $
                <input type="text" name="newHR" value="" size="5" maxlength="5" disabled="true">
                </td>
                <td align="default" width="20%" >
                <input type="text" name="newEFD" value="" size="10" maxlength="10" disabled="true">
                </td>
                <td align="right" width="15%">New</td>
            </tr>
            <tr>
                <td align="default" width="15%" >
                <div id="Type" class="required">&nbsp;</div>
                <input type="hidden" name="newPType" value="">
                <div id="Rates" style="position:absolute; width:1px; height:1px; z-index:1; visibility: hidden; left: 278px; top: 907px; overflow: hidden">
                    <select name="newRateType" >
                    <%
                    while (!rsTasks.EOF){
                        rsTasks.Read();
                    %>
                        <option value="<%=(rsTasks.Item("Id"))%>"><%=Trim(rsTasks.Item("PayType"))%></option>
                        <%
                    }

                    rsTasks.Requery();

                    %>
                    </select>
                </div>
                </td>
                <td align="default" width="15%" >&nbsp;</td>
                <td align="default" width="15%" >&nbsp;</td>
                <td align="default" width="20%" >&nbsp;</td>
                <td align="default" width="15%" >&nbsp;</td>
            </tr>
            <% int NRec = 0;  %>
            <% int RecID = 0;  %>
            <% string rowColor; %>
            <%
                while ((Repeat1__numRows != 0) && (! rs.EOF)){
                    rs.Read();
                %>
                    <% NRec = NRec + 1; %>
                    <% ID = Trim(cStr(rs.Item("ID"))); %>
                    <% RecID = NRec; %>
                    <%
                if (Repeat1__index % 2 == 0) {
                  rowColor = "reportOddLine";
                }else{
                  rowColor = "reportEvenLine";
                }
                %>
            <tr class="<%=rowColor%>">
                <td align="default" width="15%"><%=rs.Item("TaskCode") + " - " + Trim(rs.Item("Description")) %>
                <input value="<%=(rs.Item("ID"))%>" type="hidden" name="RecID_<%=RecID%>">
                <input value="<%=(rs.Item("TaskID"))%>" type="hidden" name="TID_<%=RecID%>">
                <input value="<%=Trim(rs.Item("PayType"))%>" type="hidden" name="PT_<%=RecID%>">
                </td>
                <td align="default" width="15%">
                <%if (InStr(0, "Super", System.Convert.ToString(Session["UserType"]), 1) >=1) { %>
                    <input value="<%=(FormatNumber((rs.Item("UnitsPayRate")), 2, -2, -2, -2)) %>" type="text" name="UR_<%=RecID%>" maxlength="5" size="5">
                <% }else{ %>
                    $<%=(FormatNumber((rs.Item("UnitsPayRate")), 2, -2, -2, -2)) %>
                    <input value="<%=(FormatNumber((rs.Item("UnitsPayRate")), 2, -2, -2, -2)) %>" type="hidden" name="UR_<%=RecID%>" maxlength="5" size="5">
                <% } %>
                </td>
                <td align="default" width="15%">
                <%if (InStr(0, "Super", System.Convert.ToString(Session["UserType"]), 1) >=1) { %>
                    <input value="<%=(FormatNumber((rs.Item("HoursPayRate")), 2, -2, -2, -2)) %>" type="text" name="HR_<%=RecID%>" maxlength="5" size="5">
                <% }else{ %>
                    $<%=(FormatNumber((rs.Item("HoursPayRate")), 2, -2, -2, -2))%>
                    <input value="<%=(FormatNumber((rs.Item("HoursPayRate")), 2, -2, -2, -2)) %>" type="hidden" name="HR_<%=RecID%>" maxlength="5" size="5">
                <% } %>
                </td>
                <td align="default" width="20%">
                <%if (InStr(0, "Super", System.Convert.ToString(Session["UserType"]), 1) >=1) { %>
                    <input value="<%=DateTime.Parse(rs.Item("EffectiveDate")).ToString("MM/dd/yyyy") %>" type="text" name="EFD_<%=RecID%>" size="10" maxlength="10" />
                <% }else{ %>
                    <%=(rs.Item("EffectiveDate"))%>
                    <input value="<%=DateTime.Parse(rs.Item("EffectiveDate")).ToString("MM/dd/yyyy") %>" type="hidden" name="EFD_<%=RecID%>" size="10" maxlength="10" />
                <% } %>
                </td>
                <td align="default" width="15%">
                <%if (InStr(0, "Super", System.Convert.ToString(Session["UserType"]), 1) >=1) { %>
                    <input  type="checkbox" name="DELETE_<%=RecID%>" />
                <% } %>
                </td>
            </tr>
                <%
                Repeat1__index = Repeat1__index + 1;
                Repeat1__numRows = Repeat1__numRows - 1;
                }
                %>
                <tr>
                <td align="default" width="15%">
                <input type="hidden" name="MM_update" value="true" />
                <input type="hidden" name="ShiftID" value="<%=sDefShiftID%>" />
                <input type="hidden" name="MM_recordId" value="<%=Request["ID"]%>" />
                </td>
                <td align="default" colspan="4">
                <input value="<%=NRec%>" type="hidden" name="NRec">
                </td>
            </tr>
            </form>
        </table>

       </td>
    </tr>
  </table>
</asp:Content>