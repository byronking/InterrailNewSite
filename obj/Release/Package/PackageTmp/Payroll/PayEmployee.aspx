<%@ Page Title="" Language="C#" MasterPageFile="~/Payroll/Payroll.Master" AutoEventWireup="true" CodeBehind="PayEmployee.aspx.cs" Inherits="InterrailPPRS.Payroll.PayEmployee" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e )
    {
        base.OnLoad(e);
    }

</script>

<asp:Content ID="headerScripts" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript" language="javascript">
    function bodyload()
    {
        LoadMembers();  
        setCustomerTask();
        updateUPM();
  
        OriginalFormCheckSumValue = CheckStringForForm(document.form1);
    }

    function setCustomerTask()
    {
      frm = document.form1;

      var tin   = frm.select;
      var iend = ((tin.options.length)-1);

      document.all['zTask'].innerHTML     = "";
      document.all['zCustomer'].innerHTML = "";
  
      for (i = iend; i > -1; i--) 
      {  
        if (tin.options[i].selected)
            {
              frm.allTasks.options[i].selected = true;
              frm.allCustomers.options[i].selected = true;
              document.all['zTask'].innerHTML     = frm.allTasks.options[i].text;
              document.all['zCustomer'].innerHTML = frm.allCustomers.options[i].text;
            }      
      }
    }

    function bodyunload()
    {
    }

    function updateUPM()
    {   
       var upm;
       var h;
       var nc;
   
       h = 0.0;
   
       upm = 0.0;
       try
          {
           h =  ((document.form1.select8.options.length) + parseFloat(document.form1.CurrentEmpCount.value));
           document.all("divEMP").innerHTML = h.toFixed(0);
           h =  (parseFloat(document.form1.Hours.value));
               if (isNaN(h))
                  { 
                      h = 0.0;
                      }
                    h = h  * (document.form1.select8.options.length);
                    h = h   + parseFloat(document.form1.CurrentLaborHours.value);
           document.all("divTotalHours").innerHTML = h.toFixed(3);
               document.form1.TotalLaborHours.value = h.toFixed(3);
          }
       catch (locale)
         {
           document.all("divEMP").innerHTML = "0";
         }
       try
         {
            upm = <%=strUnitCount%> / (parseFloat(document.form1.TotalLaborHours.value) ) ;
           if (!(isNaN(upm)) &&  isFinite(upm) )
                {
                 document.all("divUPM").innerHTML = upm.toFixed(3);
             document.form1.UPM.value = upm.toFixed(3);
                     }
               else
                {
             document.all("divUPM").innerHTML = "?";
             document.form1.UPM.value = "0";
                    }
             }
       catch (locale)
         {
          document.all("divUPM").innerHTML = "?";
          document.form1.UPM.value = "0";
             }
    }

    function goOutOfTown()
    {
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

    function goValidate(submittype)
    {
      if (submittype =='UPDATE')
      {
          frm = document.form1;
      
          if (ValidPositiveNumber(frm.Hours, 1, "Hours")   != true) {return false;}


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


          //OriginalFormCheckSumValue = CheckStringForForm(document.form1);

        frm.ActionType.value = "UPDATE";
        frm.action = "Payemployee.aspx";
      
          frm.submit();
      }
      if (submittype == 'RECALC')
         {
          updateUPM();
          frm = document.form1;
      
          if (ValidPositiveNumber(frm.Hours, 1, "Hours")   != true) {return false;}
     
              frm.ActionType.value = "RECALC";
              frm.action = "Payemployee.aspx";
          frm.submit();
     
         }
     
    }

    function goCancel()
    {
      try
      {

         <% if ( Request["ReturnTo"] != "") { %>
                self.location = '<%=Request["ReturnTo"]%>';
         <% }else{ %>
               self.location = "Payroll.aspx?WorkDate=<%=PWorkDate%>&Shift=<%=PShift%>";
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
      updateUPM();
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
      updateUPM();
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
     updateUPM();
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
        <td width="79%"> <a href="CSVSelect.aspx">Create CSV File</a></td>
        </tr>
        <%}%>

        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%">&nbsp;</td>
        <td width="79%">&nbsp;</td>
        </tr>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"  alt="" /></td>
        <td width="79%"><a href="../Logout.aspx">Logout</a></td>
        </tr>

    </table>
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="Main" runat="server">
    <table width="90%" border="0" cellspacing="0" cellpadding="8">
    <tr> 
        <td height="40"> <!-- #BeginEditable "MainBody" --> 
 
        <form method="POST" action="" name="form1">
            <table align="center" border="0" cellpadding="0" cellspacing="0" width="85%">
            <tr> 
                <td align="default" colspan="7"> 
                <div align="center" class="pageTitle">Employee Task Worked</div>
                </td>
            </tr>
            <tr> 
                <td align="default" colspan="7"> 
                  <table width="95%" border="0">
                    <tr> 
                    <td class="cellTopBottomBorder">#</td>
                    <td class="cellTopBottomBorder">Employee</td>
                    <td class="cellTopBottomBorder">Rate</td>
                    <td class="cellTopBottomBorder">Hours</td>
                    <td class="cellTopBottomBorder">UPM</td>
                    <td class="cellTopBottomBorder">Comments</td>
                    </tr>
                    <%
                        string rowColor;
                        int rsEmpindex = 0;
                        double CurrentLaborHours = 0; 
                        string strWorkedList = "";
                                          
                        if( Request["NewTask"] != "Yes" ){
                            while (! rsEmpWorked.EOF) {
                              rsEmpWorked.Read();
                                if (rsEmpindex == 0 ){
                                    strWorkedList = cStr(rsEmpWorked.Item("ID"));
                                }else{
                                    strWorkedList = strWorkedList + ", " + cStr(rsEmpWorked.Item("ID"));
                                }
  
                                if (rsEmpindex % 2 == 0) {
                                  rowColor = "reportEvenLine";
                                }else{  
                                  rowColor = "reportOddLine";
                                }
                                                  
                        %>
                        <tr class="<%=rowColor%>"> 
                    <td><a href="taskworkedEdit.aspx?id=<% =rsEmpWorked.Item("ID") + "&returnto=" + strBackTo %>" > 
                        <%=rsEmpWorked.Item("ENum")%> </a> </td>
                    <td><% = rsEmpWorked.Item("LastName") + ", " + rsEmpWorked.Item("FirstName").Trim() %></td>
                    <td><%=cStr(getUnitRate(rsEmpWorked.Item("TaskID"),PWorkDate,rsEmpWorked.Item("EmployeeId")))%></td>
                    <td><%=rsEmpWorked.Item("HoursWorked")%></td>
                    <td><%=rsEmpWorked.Item("UPM")%></td>
                    <td>
                    <% 
                        if (rsEmpWorked.Item("Notes") != null && rsEmpWorked.Item("Notes").Length > 30)
                       { %>
                    <%= rsEmpWorked.Item("Notes").Substring(0, 30) %>
                    <% }else{ %>
                    <% = rsEmpWorked.Item("Notes") %>
                    <% } %>
                    </td>
                    <% 
                                            
                        CurrentLaborHours = CurrentLaborHours + cDbl(rsEmpWorked.Item("HoursWorked") );
                        rsEmpindex = rsEmpindex + 1;

                        } //End While
                        }
                    %>

                    </tr>
                    <tr> 
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    </tr>
                </table>
                </td>
            </tr>
            <tr> 
                <td align="default" colspan="7">
                <input type="hidden" name="WorkDate" value="<%=PWorkDate%>" />
                                                    <% if ( Request["NewTask"] != null &&  Request["NewTask"].ToString() == "Yes" ){ %>
                <% //<input type="hidden" name="NewTask" value="Yes"> %>
                                                    <% }else{ %>
                <input type="hidden" name="Shift" value="<%=PShift%>" />
                <input type="hidden" name="Task" value="<%=PTask%>" />
                                                    <% } %>
                <input type="hidden" name="OtherTask" value="<%=PisOtherTask%>" />
                <input type="hidden" name="ActionType" value="" />
                <input type="hidden" name="CurrentEmpCount" value="<%=rsEmpindex%>" />
                <input type="hidden" name="CurrentRecordIDs" value="<%=strWorkedList%>" />
                <input type="hidden" name="TotalLaborHours" value="0" />
                <input type="hidden" name="CurrentLaborHours" value="<%=cStr(CurrentLaborHours)%>" />
                <input type="hidden" name="LastModifiedOn" value="<%=System.DateTime.Now%>" />
                <input type="hidden" name="LastModifiedBy" value="<%=Session["UserName"].ToString()%>" />
                <input type="hidden" name="ReturnTo" value="<%=strBackTo%>" />
                                                                         
                </td>
            </tr>
            <tr> 
                <td align="default" colspan="7">&nbsp;</td>
            </tr>
            <tr> 
                <td align="default" colspan="7"> 
                <div align="right"> </div>
                </td>
            </tr>
                </table>
                <% 
                    string strStatus;
                    string strDivBot;
                                                            
                    strStatus = getPayRollStatus(PWorkDate, System.Convert.ToInt32(Session["FacilityID"]));
                    Response.Write( strStatus);
                    if (strStatus == "OPEN" || strStatus == "PAYROLL" ){
                      strDivBot = "<div name=\"bottom\" id=\"bottom\" >";
                    }else{
                      strDivBot =             "<table  align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"85%\"> ";
                      strDivBot =  strDivBot + "<tr><td>This pay period is not open </td></tr></table> ";
                      strDivBot = strDivBot + "<div name=\"bottom\" id=\"bottom\" style=\"visibility: hidden; \"> ";
                    }
                %>
                <%=strDivBot%>
                <table  align="center" border="0" cellpadding="0" cellspacing="0" width="85%">
                                                                
            <tr> 
                <td align="default" colspan="6" class="reportTotalLine">Add Employees to Task Worked</td>
                <td align="default"> 
                    </td>
            </tr>
            <tr valign="baseline"> 
                <td align="center" colspan="6"> 
                <div align="center"></div>
                <table width="95%" border="0">
                    <tr> 
                    <td class="cellTopDoubleBottomBorder">Date</td>
                    <td class="cellTopDoubleBottomBorder">Shift</td>
                    <td class="cellTopDoubleBottomBorder">Task</td>
                    <td class="cellTopDoubleBottomBorder">Hours</td>
                    <td class="cellTopDoubleBottomBorder">Total Hours</td>
                    <td class="cellTopDoubleBottomBorder">Units</td>
                    <td class="cellTopDoubleBottomBorder">#Emp</td>
                    <td class="cellTopDoubleBottomBorder">UPM</td>
                    </tr>
                    <tr> 
                    <td><%=PWorkDate%></td>
                    <% if (Request["NewTask"] == "Yes"){ %>
                        <td><select name="Shift">
                                <option value="1">1</option>
                                <option value="2">2</option>
                                <option value="3">3</option>
                                <option value="4">4</option> </select>
                                </td>
                                <td>
                                <select name="Task">
                                <%=sTaskOptionList%>
                                </select>
                                </td>                                                                 
                            <% }else{ %>
                    <td><%=PShift%></td>
                    <td><%=strTaskCode%></td>
                    <% } %>
                    <td> 
                        <input type="text" name="Hours" size="8" onchange="updateUPM();" />
                                                                </td>
                    <td> <div id="divTotalHours">0</div>
                    </td>
                    <td> <%=strUnitCount%> 
                        <input type="hidden" name="Units" value="<%=strUnitCount%>" />
                    </td>
                    <td> 
                        <div id="divEMP">0</div>
                    </td>
                    <td> 
                        <div id="divUPM">0</div>
                        <input type="hidden" name="UPM" value="0" />
                    </td>
                    </tr>
                                                                
            <tr> 
                <td align="right" colspan="6" >
                <%//<input type="button" name="ReCalc" value="Update UPM for Current Records" onclick="goValidate('RECALC');"> %>
                </td>
                <td align="default"> 
                    </td>
            </tr>
                                      
                    <tr> 
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    </tr>
                </table>
                </td>
            </tr>
            <tr valign="baseline"> 
                <td align="right" width="45">&nbsp;</td>
                <td colspan="2" align="right">&nbsp;</td>
                <td colspan="3">&nbsp;</td>
            </tr>
            <tr valign="baseline"> 
                <td align="right" width="45">&nbsp;</td>
                <td colspan="2" align="right"> Add Team:</td>
                <td colspan="3"> 
                <select name="select9"  style="width:170px" onchange="LoadMembers();">
                    <%
                        while (!rsTeams.EOF)
                        {
                            rsTeams.Read();
                    %>
                        <option value="<%=rsTeams.Item("TeamMembers") %>"><%=rsTeams.Item("TeamName")%></option>
                    <%  }  %>

                </select>
                </td>
            </tr>
            <tr valign="baseline"> 
                <td nowrap align="right" width="45">&nbsp;</td>
                <td width="178" class="cellTopBottomBorder">Available 
                Employees</td>
                <td width="34">&nbsp;</td>
                <td class="cellTopBottomBorder" colspan="3"> 
                <div>Selected Employees</div>
                </td>
            </tr>
            <tr valign="baseline"> 
                <td align="left" width="45"><nobr><nobr></td>
                <td rowspan="6" width="178"> 
                <select name="select7" multiple size="6" style="width:170px" title="Available Employees">
                    <option value="  "> </option>
                </select>
                <input type="button" name="bntOutOfTown" value="Add Out of Town Employees" onclick="goOutOfTown();">
                </td>
                <td width="34">&nbsp;</td>
                <td rowspan="6" colspan="3"> 
                <select name="select8" multiple size="6" style="width:170px" title="Team Members" onchange="updateUPM();">
                    <option value=" "> </option>
                </select>
                <input type="button" name="Submit" value="Add Selected Employees" onclick="goValidate('UPDATE');" />
                </td>
            </tr>
            <tr valign="baseline"> 
                <td align="right" width="45">&nbsp; </td>
                <td width="34" align="center"> 
                <input type="button" name="btnAdd" value="&gt;&gt;" onclick="goAddRemove('ADD');" />
                </td>
            </tr>
            <tr valign="baseline"> 
                <td align="right" width="45">&nbsp;</td>
                <td width="34" align="center"> 
                <input type="button" name="btnRemove" value="&lt;&lt;" onclick="goAddRemove('REMOVE');" />
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
            <input type="hidden" name="TeamMembers" value="0" size="32" />
            <div id="Layer1" style="position:absolute; width:1px; height:1px; z-index:1; visibility: hidden; left: 278px; top: 907px; overflow: hidden"> 
                <select name="selectAll" multiple size="2">
                <%
                while(rsEmp.Read()) {
                %>
                <option value="<%=rsEmp.Item("Id")%>"><%=rsEmp.Item("LastName").Trim() %>, <%= rsEmp.Item("FirstName").Trim()%> (<%= rsEmp.Item("EmployeeNumber").Trim() %>)
                <% if(rsEmp.Item("FacilityId").Trim() == Session["FacilityID"].ToString().Trim()){
                        Response.Write("");
                   }else{
                       Response.Write(" + "); 
                   }
                 %>
          
                    </option>
                    <% } %>
                  </select>
            </div>
            </table>
                                        
        </form>                                                     
                                                        
        <p>&nbsp;</p>
        <!-- #EndEditable --> </td>
    </tr>
       
        </table>
</asp:Content>
