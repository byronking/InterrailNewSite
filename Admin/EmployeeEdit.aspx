<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EmployeeEdit.aspx.cs" Inherits="InterrailPPRS.Admin.EmployeeEdit" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

<script type="text/javascript">
<!--

function bodyload()
{
  if ('<%=ErrMsg%>' != '')
  {
    alert('<%=ErrMsg%>');
    OriginalFormCheckSumValue = CheckStringForForm(document.form1);
        history.go(-1);
  }

  if ('<%=System.Convert.ToString(Request["ID"]) %>' == "0")
  {
    document.form1.Active.checked       = true;
    document.form1.EmpStatus[0].checked = true;
  }

  OriginalFormCheckSumValue = CheckStringForForm(document.form1);

  if ( document.form1.EmpStatus[0].checked)
  {
     doChangeRequired('pe');
  }
  else
  {
     doChangeRequired('te');
  }

}

function bodyunload()
{
  if ( CheckStringForForm(document.form1) != OriginalFormCheckSumValue)
  {
    event.returnValue = "You have not saved your changes.";
  }
}

function doChangeRequired(td)
{
  frm = document.form1;
  if (td == "pe")
  {
        <% if (System.Convert.ToString(Request["ID"]) != "0") { 
        
           Response.Write(" if ((frm.EmployeeNumber.value.length == 0) && ('" + Trim(rs.Item("EmployeeNumber")) + "'.length == 0)){");
           Response.Write(" frm.EmployeeNumber.value = '" + NewEmpNumber + "';" );
           Response.Write(" }else{ ");
           Response.Write("    frm.EmployeeNumber.value = '" + Trim(rs.Item("EmployeeNumber")) + "';");
           Response.Write(" }try{ ");
           Response.Write(" document.all['divempnum'].style.visibility  = 'visible';");
           Response.Write(" document.all['divtempnum'].style.visibility = 'hidden';");
           Response.Write("  } catch(e) {} ");

        }%>

        document.all['hd'].style.color="red";
        document.all['pn'].style.color="red";
        document.all['sd'].style.color="";
        document.all['tn'].style.color="";
        document.all['ts'].style.color="";
  }
  else
  {
        <% if (System.Convert.ToString(Request["ID"]) != "0") { 

           Response.Write("if ( (frm.TempNumber.value == '') && ('" + Trim(rs.Item("TempNumber")) + "' == '')) { ");
           Response.Write("  frm.TempNumber.value = '" + NewTempNumber + "';");
           Response.Write("}else{");
           Response.Write("  frm.TempNumber.value = '" + Trim(rs.Item("TempNumber")) + "'; ");
           Response.Write(" }try{ ");
           Response.Write("   document.all['divempnum'].style.visibility  = 'hidden';");
           Response.Write("   document.all['divtempnum'].style.visibility = 'visible';");
           Response.Write("  } catch(e) {} ");
        
       }%>

        document.all['sd'].style.color="red";
        document.all['tn'].style.color="red";
        document.all['ts'].style.color="red";
        document.all['hd'].style.color="";
        document.all['pn'].style.color="";

      <% if (rsEmploymentSource.EOF) { %>
           alert('There are no employment sources to select from.');
           frm.EmpStatus[0].checked = true;
           doChangeRequired('pe');
      <% } %>
  }

  if (document.form1.EmpStatus[1].checked == true)
  {
    document.form1.Salaried.checked = false;
  }

}
function goValidate()
{
  frm = document.form1;
  <% if ( (UCase(System.Convert.ToString(Session["UserType"])) == "SUPER") || (UCase(System.Convert.ToString(Session["UserType"])) == "ADMIN")) { %>
  {
    if ( frm.EmpStatus[0].checked)
      { if (ValidNumber(frm.EmployeeNumber, 1, "Employee Number")  != true) {return false;} }
        else
      { if (ValidNumber(frm.EmployeeNumber, 0, "Employee Number")  != true) {return false;} }

    if ( frm.EmpStatus[1].checked)
      { if (ValidNumber(frm.TempNumber,     1, "Temporary Number") != true) {return false;} }
        else
      { if (ValidNumber(frm.TempNumber,     0, "Temporary Number") != true) {return false;} }
  }
  <%}%>

  if (ValidText(frm.LastName,         1, "Last Name")        != true) {return false;}
  if (ValidText(frm.FirstName,        1, "First Name")       != true) {return false;}
  if (ValidSSN(frm.SSN,               1, "SSN")              != true) {return false;}
  if (ValidPhoneNumber(frm.EmpPhone,  1, "Phone")            != true) {return false;}
  if (ValidText(frm.Address1,         1, "Address")          != true) {return false;}
  if (ValidText(frm.City,             1, "City")             != true) {return false;}
  if (ValidText(frm.State,            1, "State")            != true) {return false;}
  if (ValidText(frm.Zip,              1, "Zip")              != true) {return false;}

  if (ValidDate(frm.BirthDate,        1, "BirthDate")        != true) {return false;}

  if ( frm.EmpStatus[0].checked)
  {
    if (ValidDate(frm.HireDate, 1, "Hire Date")        != true) {return false;}
  }
  else
  {
    if (ValidDate(frm.HireDate, 0, "Hire Date")        != true) {return false;}
  }

  if (ValidDate(frm.InactiveDate,     0, "Inactive Date")    != true) {return false;}
  if (ValidDate(frm.TerminationDate,  0, "Termination Date") != true) {return false;}


  if ( frm.EmpStatus[1].checked)
  {
     if (ValidDate(frm.TempStartDate, 1, "Temp. Start Date") != true) {return false;}
  }
  else
  {
     if (ValidDate(frm.TempStartDate, 0, "Temp. Start Date") != true) {return false;}
  }

    <% 
      
    if ((UCase(System.Convert.ToString(Session["UserType"])) == "SUPER") || (UCase(System.Convert.ToString(Session["UserType"])) == "ADMIN"))
    { 
    
    }  else { 
         
    if (System.Convert.ToString(Request["ID"]) == "0"){
        
    %>
     if ( frm.EmpStatus[1].checked){
            frm.EmployeeNumber.value = "";
            frm.TempNumber.value     = "<%=NewTempNumber%>";
     }else{
            frm.EmployeeNumber.value = "<%=NewEmpNumber%>";
            frm.TempNumber.value     = "";
     }

   <% }} %>


  OriginalFormCheckSumValue = CheckStringForForm(document.form1);

  frm.action = "<%=MM_editAction%>";
  //frm.action = "/col.aspx";
  frm.submit();
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
        <%if ( Session["UserType"].ToString().IndexOf("Super") >=1 ) { %>
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
    <form method="POST" action="<%=MM_editAction%>" name="form1">
        <table align="center" width="100%">
        <tr valign="baseline">
            <td nowrap align="right" colspan="6">
            <div align="center"  class="pageTitle">Employee</div>
            </td>
        </tr>
        <tr valign="baseline">
            <td nowrap align="left" colspan="6" class="required">&nbsp;</td>
        </tr>
        <tr valign="baseline">
            <td nowrap align="right" width="18%" height="26">
            <span  class="required">Last Name</span>:</td>
            <td width="27%" height="26">
            <input type="text" name="LastName" value="<%= Trim((rs.Item("LastName"))) %>" size="25" maxlength="30">
            </td>
            <td align="center" width="11%" height="26">
            MI:
            <input type="text" name="MiddleInitial" value="<%=(rs.Item("MiddleInitial"))%>" size="1" maxlength="1">
            </td>
            <td align="right" class="required" width="11%" height="26">First:</td>
            <td colspan="2" align="left" height="26">
            <input type="text" name="FirstName" value="<%= Trim((rs.Item("FirstName"))) %>" size="25" maxlength="30">
            </td>
        </tr>
        <tr valign="baseline">
            <td nowrap align="right" class="required"  width="18%" height="25">SSN:</td>
            <td width="27%" height="25" >
            <input type="text" name="SSN" value="<%= Trim((rs.Item("SSN"))) %>" size="25" maxlength="11">
            </td>
            <td align="right" width="11%" height="25">&nbsp;</td>
            <td align="right" width="11%" class="required"  height="25">DOB:</td>
            <td colspan="2" height="25" >
            <input type="text" name="BirthDate" value="<%= (rs.Item("BirthDate") == "")?"":cDate(Trim((rs.Item("BirthDate")))).ToShortDateString() %>" size="25" maxlength="10">
            </td>
        </tr>
        <tr valign="baseline">
            <td nowrap align="right" class="required" width="18%" height="26">Phone:</td>
            <td width="27%" height="26" >
            <input value="<%=(rs.Item("EmployeePhone"))%>" type="text" name="EmpPhone" size="25" maxlength="20">
            </td>
            <td align="right" width="11%" height="26">&nbsp;</td>
            <td align="right" width="11%" height="26">&nbsp;</td>
            <td colspan="2" height="26" >&nbsp; </td>
        </tr>
        <tr valign="baseline">
            <td nowrap align="right" class="required" width="18%" height="28">Address:</td>
            <td height="28" width="27%">
            <input type="text" name="Address1" value="<%= Trim((rs.Item("Address1"))) %>" size="25" maxlength="30">
            </td>
            <td align="center" height="28" width="11%" nowrap>&nbsp;</td>
            <td align="right" height="28" width="11%">Line
            2:</td>
            <td colspan="2" height="28">
            <input type="text" name="Address2" value="<%= Trim((rs.Item("Address2"))) %>" size="25" maxlength="30">
            </td>
        </tr>
        <tr valign="baseline">
            <td nowrap align="right" class="required" width="18%" height="28">City:</td>
            <td height="28" width="27%">
            <input type="text" name="City" value="<%=Trim(rs.Item("City"))%>" size="25" maxlength="30">
            </td>
            <td align="center" class="required" height="28" width="11%" nowrap>
            ST:
            <input type="text" name="State" value="<%= Trim((rs.Item("State"))) %>" size="2" maxlength="2">
            </td>
            <td align="right" class="required" height="28" width="11%">Zip:
            </td>
            <td colspan="2" height="28">
            <input type="text" name="Zip" value="<%= Trim((rs.Item("Zip"))) %>" size="25" maxlength="14">
            </td>
        </tr>
        <tr valign="baseline">
            <td nowrap align="right" width="18%">Emergency<br>
            Contact:</td>
            <td width="27%">
            <input type="text" name="EmergencyContact" value="<%= Trim((rs.Item("EmergencyContact"))) %>" size="25" maxlength="30">
            </td>
            <td width="11%" align="center">&nbsp;</td>
            <td width="11%" align="right">Phone:</td>
            <td colspan="2">
            <input type="text" name="ContactPhone" value="<%= Trim((rs.Item("ContactPhone"))) %>" size="25" maxlength="20">
            </td>
        </tr>
        <tr valign="baseline">
            <td nowrap align="right" width="18%">&nbsp;</td>
            <td width="27%" align="right">Active: </td>
            <td width="11%" align="left">
            <input <%if (rs.Item("Active") != "False") { Response.Write("CHECKED"); }else{ Response.Write("") ;} %> type="checkbox" name="Active" value="1" >
            </td>
            <td width="11%" align="center">&nbsp;</td>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr valign="baseline">
            <td nowrap align="right" width="18%">&nbsp;</td>
            <td width="27%" align="right">&nbsp;</td>
            <td width="11%" align="left">&nbsp;</td>
            <td width="11%" align="center">&nbsp;</td>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr valign="baseline">
            <td nowrap align="right" width="18%">&nbsp;</td>
            <td width="27%" align="left" class="cellTopBottomBorder">
            <input id="rdoPerm" <%  if (rs.Item("TempEmployee") != null && rs.Item("TempEmployee") == "False") { Response.Write("CHECKED"); }else{ Response.Write(""); }%>  type="radio" name="EmpStatus" value="0" onclick="doChangeRequired('pe');">
            <label for="rdoPerm">Permanent</label> </td>
            <td align="left" >&nbsp; </td>
            <td align="left" colspan="3" class="cellTopBottomBorder">
            <input id="rdoTemp" <%if (rs.Item("TempEmployee") != null && rs.Item("TempEmployee") != "False") { Response.Write("CHECKED"); }else{ Response.Write(""); }%> type="radio" name="EmpStatus" value="1"  onclick="doChangeRequired('te');">
            <label for="rdoTemp">Temporary</label> </td>
        </tr>
        <tr valign="baseline">
            <td nowrap align="right" width="18%" height="22" id="pn">Number:</td>
            <td width="27%" align="left" height="22">
            <%if ( (UCase(System.Convert.ToString(Session["UserType"])) == "SUPER") || (UCase(System.Convert.ToString(Session["UserType"])) == "ADMIN")) { %>
            <%if (System.Convert.ToString(Request["ID"]) == "0") { %>
                <input type="text" name="EmployeeNumber" value="<%=NewEmpNumber%>" size="25" maxlength="6" >
            <%}else{%>
                <input type="text" name="EmployeeNumber" value="<%=Trim(rs.Item("EmployeeNumber")) %>" size="25" maxlength="6" >
            <%}%>
            <%}else{%>
            <%if (System.Convert.ToString(Request["ID"]) == "0") {%>
            <input type="hidden" name="EmployeeNumber" value="<%=NewEmpNumber%>" size="25" maxlength="6" >
            <%=NewEmpNumber%>
            <%}else{%>
                <%if (rs.Item("TempEmployee") == "1") { %>
                <div id="divempnum" style="{visibility:hidden}"><%=NewEmpNumber%></div>
                <%}else{%>
                <div id="divempnum" style="{visibility:hidden}"><%=Trim(rs.Item("EmployeeNumber")) %></div>
                <%}%>
                <input type="hidden" name="EmployeeNumber" value="<%=Trim(rs.Item("EmployeeNumber"))%>" size="25" maxlength="6" >
                                   
            <%}}%>

            </td>
            <td align="right" height="22" id="tn">Number:
            </td>
            <td align="left" height="22" colspan="3">
            <% if ( (UCase(System.Convert.ToString(Session["UserType"])) == "SUPER") || (UCase(System.Convert.ToString(Session["UserType"])) == "ADMIN")) { %>
              <%if (System.Convert.ToString(Request["ID"]) == "0") {%>
               <input value="<%=NewTempNumber%>" type="text" name="TempNumber" size="25" maxlength="4" >
              <%}else{%>
               <input value="<%=Trim(rs.Item("TempNumber")) %>" type="text" name="TempNumber" size="25" maxlength="4" >
              <%}%>
            <%}else{%>
              <% if (System.Convert.ToString(Request["ID"]) == "0") {%>
               <input value="<%=NewTempNumber%>" type="hidden" name="TempNumber" size="25" maxlength="4" >
               <%=NewTempNumber%>
              <%}else{%>
                  <%if (rs.Item("TempEmployee") == "0") { %>
                     <div id="divtempnum" style="{visibility:hidden}"><%=NewTempNumber%></div>
                  <%}else{%>
                     <div id="divtempnum" style="{visibility:hidden}"><%=Trim(rs.Item("TempNumber")) %></div>
                  <%}%>
                   <input value="<%=Trim(rs.Item("TempNumber")) %>" type="hidden" name="TempNumber" size="25" maxlength="4" >
              <%}%>
            <%}%>
            </td>
        </tr>
        <tr valign="baseline">
            <td nowrap align="right" width="18%" height="22" id="hd">Hire
            Date: </td>
            <td width="27%" align="left" height="22">
            <input type="text" name="HireDate" value="<%  if(Trim(rs.Item("HireDate")).Length > 0){  Response.Write(cDate(Trim(rs.Item("HireDate"))).ToShortDateString()) ;}else{ Response.Write("");} %>" size="25" maxlength="14">
            </td>
            <td align="right" height="22" id="sd">Start:</td>
            <td colspan="3" align="left" height="22">
            <input type="text" name="TempStartDate" value="<% if(Trim(rs.Item("TempStartDate")).Length > 0) { Response.Write(cDate(Trim(rs.Item("TempStartDate"))).ToShortDateString());}else{ Response.Write("");} %>" size="10" maxlength="14">
            </td>
        </tr>
        <tr valign="baseline">
            <td nowrap align="right" width="18%" height="19" id="id">Inactive:
            </td>
            <td width="27%" align="left" height="19">
            <input type="text" name="InactiveDate" value="<% if(Trim(rs.Item("InactiveDate")).Length > 0) { Response.Write(cDate(Trim(rs.Item("InactiveDate"))).ToShortDateString()); }else{ Response.Write("");} %>" size="25" maxlength="14">
            </td>
            <td align="right" height="19" id="ts">Source:
            </td>
            <td colspan="3" align="left" height="19">
            <select name="select2">
                <%
                while (! rsEmploymentSource.EOF){
                    rsEmploymentSource.Read();
                %>
                <option value="<%=(rsEmploymentSource.Item("Id")) %>" <%if ((rsEmploymentSource.Item("Id")) == (rs.Item("EmploymentSourceId"))) { Response.Write("SELECTED"); }else{ Response.Write(""); } %>><%=rsEmploymentSource.Item("SourceName") %></option>
                <%
                                       
                }
                                        
                rsEmploymentSource.Requery();
                                        
                %>
            </select>
            </td>
        </tr>
        <tr valign="baseline">
            <td nowrap align="right" width="18%" height="22" id="td">Termination
            Date: </td>
            <td width="27%" align="right" height="22">
            <input type="text" name="TerminationDate" value="<% if(Trim(rs.Item("TerminationDate")).Length > 0){ Response.Write(cDate(Trim(rs.Item("TerminationDate"))).ToShortDateString()); }else{ Response.Write(""); } %>" size="25" maxlength="14">
            </td>
            <td align="right" height="22">&nbsp;</td>
            <td colspan="3" align="left" height="22">&nbsp; </td>
        </tr>
        <tr valign="baseline">
            <td nowrap align="right" width="18%">Salaried:
            </td>
            <td width="27%">
            <input <% if (rs.Item("Salaried") != null && rs.Item("Salaried") == "True") { Response.Write("CHECKED"); }else{ Response.Write(""); }%> type="checkbox" name="Salaried" value="checkbox">
            </td>
            <td width="11%" align="center">&nbsp;</td>
            <td colspan="3" align="center">&nbsp;</td>
        </tr>
        <tr valign="baseline">
            <td nowrap align="right" width="18%">&nbsp;</td>
            <td width="27%">&nbsp;</td>
            <td width="11%" align="center">&nbsp;</td>
            <td colspan="3" align="center">&nbsp;</td>
        </tr>
        <tr valign="baseline">
            <td nowrap align="right" width="18%">Last Modified:</td>
            <td colspan="4"><%= rs.Item("LastModifiedOn") %></td>
        </tr>
        <tr valign="baseline">
            <td nowrap align="right" width="18%">By:</td>
            <td colspan="4"><%= rs.Item("LastModifiedBy") %></td>
        </tr>
        <tr valign="baseline">
            <td nowrap align="right" width="18%">
            <input type="hidden" name="MM_update" value="true" />
            <input type="hidden" name="MM_recordId" value="<%= rs.Item("Id") %>" />
            <input type="hidden" name="LastModifiedOn" value="<%=System.DateTime.Now.ToString() %>" size="30" />
            <input type="hidden" name="LastModifiedBy" value="<%=Session["UserName"] %>" size="30" />
            <input type="hidden" name="ThisFacID" value="<%=Session["FacilityID"] %>" size="30" />
            </td>
            <td>&nbsp; </td>
            <td colspan="3">
            <input type="button" value="Save" onclick="goValidate();" name="button" />
            <input type="button" name="btnCancel" value="Cancel" onclick="goCancel();" />
            </td>
            <td>&nbsp;</td>
        </tr>
        </table>
    </form>
    <p>&nbsp;</p>
    Fields in <span class="Required">RED</span> are required.

    </td>
    </tr>
    </table>

</asp:Content>