<%@ Page Language="C#" MasterPageFile="~/Production/Production.Master" AutoEventWireup="true" CodeBehind="DetailEdit.aspx.cs" Inherits="InterrailPPRS.Production.DetailEdit" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

<script type="text/javascript" >

  var TS = [];

  <%
    // Get PayrollStatus for last 90 days
    
      int iDay = 0;
      string[] pStatus = new string[90];
      string TheDate = "";
      DateTime today = DateTime.Now.AddDays(-90);
      string gotPayRollStatus = "";

      for( iDay=1; iDay <= 91; iDay ++){
        TheDate =  cStr(today.AddDays(iDay-1));
        gotPayRollStatus = getPayRollStatus(TheDate, cInt(Session["FacilityID"]));
   %>
        TS["<%=FormatTheDate(cDate(TheDate))%>"]="<%=gotPayRollStatus%>";
  <%  
      } 
  %>


  function showPayrollStatus()
  {
     var TheDate = formatTheDate(document.form1.WorkDate.value);

     //document.all["pStatus"].innerHTML = TS[TheDate];

     var tStatus = TS[TheDate];
     if (tStatus == undefined)
     {
        alert("Unknown Payroll Status!");
        document.all["pStatus"].innerHTML = "** UNKNOWN **";
     }
     else if (TS[TheDate] == "LOCKED")
     {
        document.all["pStatus"].innerHTML = "** LOCKED **";
        document.form1.WorkDate.focus();
        document.form1.WorkDate.select();


     }
     else
     {
        document.all["pStatus"].innerHTML = " ";
     }
  }


  function formatTheDate(theDate){
     var d, sd;
     d = new Date(theDate);
     sd = (d.getMonth() + 101).toString().substring(1,3)  + "/";
     sd += (d.getDate() + 100).toString().substring(1,3)  + "/";
     sd += d.getFullYear();
     return(sd);
  }


function MM_reloadPage(init) {  //reloads the window if Nav4 resized
  if (init==true) with (navigator) {if ((appName=="Netscape")&&(parseInt(appVersion)==4)) {
    document.MM_pgW=innerWidth; document.MM_pgH=innerHeight; onresize=MM_reloadPage; }}
  else if (innerWidth!=document.MM_pgW || innerHeight!=document.MM_pgH) location.reload();
}
MM_reloadPage(true);

var unsaveddata = '';
var displaydata = '';

function bodyload()
{

  showPayrollStatus();

  OriginalFormCheckSumValue = CheckStringForForm(document.form1);
  document.form1.selCarType.focus();
  document.form1.selCarType.select();


}

function bodyunload()
{
  if ( CheckStringForForm(document.form1) != OriginalFormCheckSumValue)
  {
    event.returnValue = "You have not saved your changes.\nYou will lose all Unsaved Information!";
  }
}

function formkeypress()
{
  if (event.keyCode == 13)
    {
         for (i = 0; i < form1.elements.length; i++)
             {
                   if (form1.elements(i).name == 'btnSave')
                   {
                     goValidate();
                         return true;
                   }
                   if (form1.elements(i).name == 'btnAdd')
                   {
                       goAdd();
                       event.keyCode = 0;
                         return false;
                   }

                   if ( (form1.elements(i).name == 'btnDelete') || (form1.elements(i).name == 'btnCancel')  )
                     {
                           form1.elements(i).onClick();
                           window.event.returnValue = false;
                           return;
                         }
                   if (form1.elements(i) == document.activeElement )
                      {
                          if (i == form1.elements.length - 1)
                               i = -1;
                          form1.elements(i+1).focus();
                          try
                            {
                                 form1.elements(i+1).select();
                                }
                          catch (localerror)
                            {}
                          window.event.returnValue = false;
                          return;
                          }
                 }
         }
}


function updateUnsaved()
{
  var arLines = displaydata.split('|');
  var s = "<table bgcolor='yellow' border='0' width='100%'><tr><td align='center' colspan=7>Unsaved Information</td></tr>";
  var lastRailCar = '';

  for (var i = 0; i < arLines.length; i++)
  {
    var lc = "";
    if ((i % 2) == 0)
        {
      lc = "reportEvenLine";
         }
     else
         {
      lc = "reportOddLine";
         }
lc = ""
//alert(arLines[i]);

    var arLine  = arLines[i].split('~');
    if ( arLine[5] + "-" + arLine[6] == lastRailCar)
          {
           lc = "reportVeryOddLine";
          }
        s = s + "<tr class='" + lc + "'><td width='11%'><div  class='smallerText'>" + arLine[0] + "</div></td>";
        s = s + "<td width='12%'><div  class='smallerText'>" + arLine[2] + "</div></td>";
        s = s + "<td  width='10%' align='center'><div  class='smallerText'>" + arLine[3] + "</div></td>";
        s = s + "<td ><div  class='smallerText'>" + arLine[5] + "-" + arLine[6] + "</div></td>";
        s = s + "<td width='6%' align='center'><div  class='smallerText'>" + arLine[4] + "</div></td>";
        s = s + "<td width='15%' align='right'><div  class='smallerText'>" + arLine[7] + "</div></td>";
        s = s + "<td width='15%'><div  class='smallerText'>" + arLine[1] + "</div></td>";
        s = s + "</tr>";

    lastRailCar = arLine[5] + "-" + arLine[6];
  }
  s = s + '</table>';

  document.all["divUnsaved"].innerHTML = s;

}

function getStrInPren(str)
{
    var start = str.indexOf('(');
    var stop  = str.indexOf(')');

    if (start < stop)
      {
       return  str.slice(start+1,stop);
      }
    else
      {
       return '';
      }

}

var nUnsaved      = 0;
var nTotalUnsaved = 0;
var nMaxUnsaved   = 20;

function goAdd()
{
   var s = "";
 frm = document.form1;
  if (ValidDate(frm.WorkDate,                1, "Work Date")       != true) {return false;}
  if (ValidPositiveNumber(frm.RailCarNumber, 1, "Rail Car Number") != true) {return false;}
  if (frm.RailCarNumber.value.length < 6)
  {
     alert('Rail Car Number must a 6-digit number.');
         frm.RailCarNumber.focus();
         return false;
  }

  if (ValidPositiveNumber(frm.Units,     1, "Units")               != true) {return false;}

  var notes = frm.Notes.value;
  if (notes.length > 250)
  {
     alert('Notes must not exceed 250 characters.');
         frm.Notes.focus();
         return false;
  }

   s = s + frm.WorkDate.value;
   s = s + '~' +  frm.selShift.value;
   s = s + '~' +  frm.selCustomer.value;
   s = s + '~' +  frm.selTask.value;
   s = s + '~' +  frm.selOrigin.value;
   s = s + '~' +  frm.selManufacturer.value;
   s = s + '~' +  frm.selNewUsed.value;
   s = s + '~' +  frm.selLevel.value;
   s = s + '~' +  frm.selCarType.value;
   s = s + '~' +  frm.RailCarNumber.value;
   s = s + '~' +  frm.Units.value;
   s = s + '~' +  frm.Notes.value;


   if (unsaveddata.length > 0 )
   {
     unsaveddata = s + '|' + unsaveddata;
   }
   else
   {
     unsaveddata = s;
   }

   s =            getStrInPren(frm.selTask.options[frm.selTask.selectedIndex].text);
   s = s + '~' +  getStrInPren(frm.selOrigin.options[frm.selOrigin.selectedIndex].text);
   s = s + '~' +  getStrInPren(frm.selManufacturer.options[frm.selManufacturer.selectedIndex].text);
   s = s + '~' +  frm.selNewUsed.options[frm.selNewUsed.selectedIndex].text.substr(0,1);
   s = s + '~' +  frm.selLevel.options[frm.selLevel.selectedIndex].text.substr(0,1);
   s = s + '~' +  frm.selCarType.options[frm.selCarType.selectedIndex].text;
   s = s + '~' +  frm.RailCarNumber.value;
   s = s + '~' +  frm.Units.value;

   if (displaydata.length > 0)
   {
     displaydata = s + '|' + displaydata;
   }
   else
   {
     displaydata = s ;
   }

   updateUnsaved();

   nUnsaved = nUnsaved + 1;
   nTotalUnsaved = nTotalUnsaved + 1;

   document.all["divUnsavedCount"].innerHTML = '&nbsp;(' + nTotalUnsaved + ' unsaved entries)';

   if (nUnsaved >= nMaxUnsaved)
   {
      if (confirm('You have ' + nTotalUnsaved + ' unsaved entries.\n\nPress OK to save now OR Cancel to save later.'))
          {
                goSaveAll();
            nUnsaved      = 0;
        nTotalUnsaved = 0;
        document.all["divUnsavedCount"].innerHTML = '';
          }
          else
          {
            nUnsaved = 0;
            alert('Unsaved Entries: ' + nTotalUnsaved + '.\n\nPlease save frequently to avoid losing data.');
          }
   }

   document.form1.selCarType.focus();
 //  document.form1.selCarType.select();


/*
WorkDate
selShift
selCustomer
selTask
selOrigin
selManufacturer
selNewUsed
selLevel
selCarType
RailCarNumber
Units
Notes
MM_recordId
LastModifiedOn
LastModifiedBy
FacilityID
WDate
 */
}

function goDelete()
{
  if (confirm('Are you sure you want to delete this production record?\nPress OK to confirm the delete.'))
  {
    frm = document.form1;

    frm.action = "DetailEdit.aspx?ID=<%=Request["ID"]%>";

        if ('<%=Request["ReturnTo"]%>' == '')
        {
          var sReturnTo = '&ReturnTo=%2FPPRS%2FProduction%2FDetailEdit%2Easpx%3FID%3D0%26WDate%3D<%=Request["Wdate"]%>'
        }
        else
        {
          var sReturnTo = '';
        }

    frm.action = frm.action + '&Delete=YES' + sReturnTo;
    frm.submit();
  }
}

function goSaveAll()
{
  if (unsaveddata.length < 2)
    {
       alert('Nothing to save');
       return false;
    }

  frm = document.formSaveAll;
  OriginalFormCheckSumValue = CheckStringForForm(document.form1);

            nUnsaved      = 0;
        nTotalUnsaved = 0;

  frm.Data.value = unsaveddata;
  frm.action = "DetailEdit.aspx?ID=0&WDate="+document.form1.WorkDate.value;
  //frm.action = "/col.aspx?ID=0&WDate="+document.form1.WorkDate.value;
  frm.submit();

}


function goValidate()
{
  frm = document.form1;
  if (ValidDate(frm.WorkDate,                1, "Work Date")       != true) {return false;}
  if (ValidPositiveNumber(frm.RailCarNumber, 1, "Rail Car Number") != true) {return false;}
  if (frm.RailCarNumber.value.length < 6)
  {
     alert('Rail Car Number must a 6-digit number.');
         frm.RailCarNumber.focus();
         return false;
  }

  if (ValidPositiveNumber(frm.Units,     1, "Units")               != true) {return false;}

  var notes = frm.Notes.value;
  if (notes.length > 250)
  {
     alert('Notes must not exceed 250 characters.');
         frm.Notes.focus();
         return false;
  }

  OriginalFormCheckSumValue = CheckStringForForm(document.form1);
  frm.action = "DetailEdit.aspx?ID=<%=Request["ID"]%>&WDate="+frm.WorkDate.value;
  frm.submit();
}

function goCancel()
{
  try
  {

     <% if (Request["ReturnTo"] != null && Request["ReturnTo"] != "") { %>
            self.location = '<%=Request["ReturnTo"]%>';
         <% }else{ %>
        self.location = "Detail.aspx";
         <% } %>
  }
  catch(e) {}
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
                  <%if( CheckSecurity("Super, Admin, User, Production")) { %>
                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
                    <td width="79%"><a href="Detail.aspx">Detail Maintenance</a></td>
                  </tr>
                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
                    <td width="79%"><a href="ApproveProductionData.aspx">Approve Production Data</a></td>
                  </tr>
                <%}%>

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
                <td><!-- #BeginEditable "MainBody" -->
                <% if ((rsTask.EOF) ||(rsCustomer.EOF)) { %>
                No facility tasks and/or customers available for the
                facility.<br>
                Please assign tasks and/or customers to the facility
                first.
                <% }else{ %>
                <form method="POST" name="formSaveAll">
                <input type="hidden" name="SaveType" value="SaveAll">
                <input type="hidden" name="Data" value="">
                </form>
                <form method="POST" action="<%=MM_editAction%>" name="form1"  onkeypress="formkeypress();">
                    <table align="center" width="100%" border="0" cellpadding='0' cellspacing='0'>
                    <tr valign="baseline">
                        <td align="left"  colspan='3'>
                        <table width="100%">
                            <tr>
                            <td width="100%" align='left' class='smallerText'>
                                <%=ShowTotals()%> </td>
                            </tr>
                        </table>
                        </td>
                    </tr>
                    <tr valign="baseline">
                        <td align="right" colspan="3"> <a href="DetailEdit.aspx?id=0&WDate=<%=Request["WDate"]%>">Add
                        New</a> </td>
                    </tr>
                    <!-- New End here...-->
                    <tr valign="baseline">
                        <td align="right" class="cellTopBottomBorder" colspan="3">
                        <div align="center"  class="pageTitle">Production Detail</div>
                        </td>
                    </tr>

                                                    <!-- BXM Start -->
                                                    <tr>
                                                    <td colspan='2' width='196' valign='top'>
                        <table width='100%'>
                            <tr valign="baseline">
                            <td align="right" width="41">
                                <div   class="required"> Date:</div>
                            </td>
                            <td width="155">
                                <input type="text" name="WorkDate" value='<% if (Request["ID"] != "0") { Response.Write( DateTime.Parse(rs.Item("WorkDate")).ToString("MM/dd/yyyy")); }else{ Response.Write(cStr(Request["WDate"])); } %>' size="10" maxlength="10" onblur='showPayrollStatus();'><b><div name="pStatus" id="pStatus" class="required"' >** UNKNOWN **</div></b>
                            </td>
                            <tr valign="baseline">
                            <td align="right" width="41" class="required">Shift:</td>
                            <td width="155">
                                <%
                                if ((Request["ID"] == "0") && ( rsDef.LastReadSuccess) ) {
                                        sDefShiftID = cStr(rsDef.Item("ShiftId"));
                                }else{
                                    if (rs.LastReadSuccess)
                                    {
                                        sDefShiftID = (rs.Item("ShiftId"));
                                    }
                                }
                                %>
                                <select name="selShift" style="width:65px">
                                <%
                                while(rsShifts.Read()){
                                    
                                %>
                                <option value="<%=(rsShifts.Item("ID"))%>" <% if (Trim(rsShifts.Item("ID")) == Trim(sDefShiftID)) { Response.Write("SELECTED"); }else{ Response.Write("");}%>><%=(rsShifts.Item("Shift"))%></option>
                                <%
  
                                }
  
                                    rsShifts.Requery();

                                %>
                                </select>
                            </td>
                            </tr>
                            <tr valign="baseline">
                            <td align="right" width="41" class="required">Cust.:</td>
                            <td width="155">
                                <select name="selCustomer" style="width:150px">
                                <%
                                while(rsCustomer.Read()){
                                    
                                %>
                                    <%
                                if (Request["ID"] == "0") {
                                        sDefCustID = cStr(FacilityCustID);
                                }else{
                                    if (rs.LastReadSuccess)
                                    {
                                        sDefCustID = Trim(rs.Item("FacilityCustomerId"));
                                    }
                                }
                                %>
                                <option value="<%=(rsCustomer.Item("Id"))%>" <% if (Trim(rsCustomer.Item("Id")) == sDefCustID) { Response.Write("SELECTED"); }else{ Response.Write("");} %>><%=Trim(rsCustomer.Item("CustomerName"))%> (<%=Trim(rsCustomer.Item("CustomerCode"))%>)</option>
                                <%
                                    }
                                                
                                              

                                    rsCustomer.Requery();

                                %>
                                </select>
                            </td>
                            </tr>
                            <tr valign="baseline">
                            <td align="right" width="41" class="required">Task:</td>
                            <td width="155">
                                <select name="selTask" style="width:150px">
                                <%
                                while(rsTask.Read()){
                                    
                                %>
                                <%
                                    if (Request["ID"] == "0"  ) {
                                            sDefTaskID = cStr(TaskID);
                                    }else{
                                        if (rs.LastReadSuccess)
                                        {
                                            sDefTaskID = Trim(cStr(rs.Item("TaskId")));
                                        }
                                    }
                                                                %>
                                <option value="<%=(rsTask.Item("Id"))%>" <%if (Trim(rsTask.Item("Id")) == sDefTaskID ) { Response.Write("SELECTED") ; }else{ Response.Write("");} %> ><%=(rsTask.Item("TaskDescription"))%> (<%=(rsTask.Item("TaskCode"))%>)</option>
                                <%
                                        }
                                    

                                        rsTask.Requery();

                                    %>
                                </select>
                            </td>
                            </tr>
                            <tr valign="baseline">
                            <td align="right" width="41">&nbsp;</td>
                            <td width="155">&nbsp; </td>
                            </tr>
                            <tr valign="baseline">
                            <td align="right" width="41" class="required">N/U:</td>
                            <td width="155">
                                <%
                                    if (Request["ID"] == "0" &&  rsDef.LastReadSuccess ) {
                                        sDefNU = cStr(rsDef.Item("NewUsed"));
                                    }else{
                                        if (rs.LastReadSuccess)
                                        {
                                            sDefNU = (rs.Item("NewUsed"));
                                        }
                                    }
                                %>
                                <select name="selNewUsed" style="width:65px">
                                <option value="-" <%if (sDefNU == "-") { Response.Write("SELECTED"); }else{ Response.Write("");} %> >N/A</option>
                                <option value="N" <%if (sDefNU == "N") { Response.Write("SELECTED"); }else{ Response.Write("");} %> >New</option>
                                <option value="U" <%if (sDefNU == "U") { Response.Write("SELECTED"); }else{ Response.Write("");} %> >Used</option>
                                </select>
                            </td>
                            </tr>
                            <tr valign="baseline">
                            <td align="right" width="41">Origin:</td>
                            <td width="155">
                                <select name="selOrigin" style="width:150px">
                                <%
                                while(rsOrigin.Read()){
                                   
                                %>
                                <%
                                                                if (Request["ID"] == "0" && rsDef.LastReadSuccess ) {
                                                                        sOriginID = cStr(rsDef.Item("OriginId"));
                                                                }else{
                                                                    if (rs.LastReadSuccess)
                                                                    {
                                                                        sOriginID = (rs.Item("OriginId"));
                                                                    }
                                                                }
                                                                %>
                                <option value="<%=(rsOrigin.Item("ID"))%>" <%  if (Trim(rsOrigin.Item("ID")) == Trim(sOriginID)) { Response.Write("SELECTED") ; }else{ Response.Write(""); }%> ><%=(rsOrigin.Item("OriginName"))%> (<%=(rsOrigin.Item("OriginCode"))%>)</option>
                                <%

                                    }

                                rsOrigin.Requery();

                            %>
                                    </select>
                            </td>
                            </tr>
                            <tr valign="baseline">
                            <td align="right" width="41" class="required">RailCar:</td>
                            <td width="155">
                                <select name="selCarType" style="width:65px">
                                <%
                            while(rsRailCarType.Read()){
                               
                            %>
                                <%
                                    if (Request["ID"] == "0" &&  rsDef.LastReadSuccess ) {
                                            sCarTypeID = cStr(rsDef.Item("CarTypeID"));
                                    }else{
                                        if (rs.LastReadSuccess)
                                        {
                                            sCarTypeID = (rs.Item("CarTypeID"));
                                        }
                                    }
                                    %>
                                    <option value="<%=(rsRailCarType.Item("ID"))%>" <%if (Trim(rsRailCarType.Item("ID")) == Trim(sCarTypeID)) { Response.Write("SELECTED") ; }else{ Response.Write(""); }%>><%=(rsRailCarType.Item("CarTypeDescription"))%></option>
                                   <%
                                    }
                                                

                                    rsRailCarType.Requery();

                                %>
                                </select>
                                -
                                <input type="text" name="RailCarNumber" value="<% if (rs.LastReadSuccess){ Response.Write(rs.Item("RailCarNumber"));  } %>" size="6" maxlength="6" />
                                 
                            </td>
                            </tr>
                            <tr valign="baseline">
                            <td align="right" width="41" class="required">Level:</td>
                            <td width="155">
                                <%
                                    if (Request["ID"] == "0" &&  rsDef.LastReadSuccess ) {
                                        sDefLevel = cStr(rsDef.Item("LevelType"));
                                    }else{
                                        if (rs.LastReadSuccess)
                                        {
                                            sDefLevel = (rs.Item("LevelType"));
                                        }
                                    }
                                %>
                                <select name="selLevel" style="width:65px">
                                <option value="-" <%if (sDefLevel == "-") { Response.Write("SELECTED"); }else{ Response.Write("");} %> >N/A</option>
                                <option value="B" <%if (sDefLevel == "B") { Response.Write("SELECTED"); }else{ Response.Write("");} %> >Bi-Level</option>
                                <option value="T" <%if (sDefLevel == "T") { Response.Write("SELECTED"); }else{ Response.Write("");} %> >Tri-Level</option>
                                </select>
                            </td>
                            </tr>
                            <tr valign="baseline">
                            <td align="right" width="41" >Manuf.:</td>
                            <td width="155">
                                <select name="selManufacturer" style="width:150px">
                                <%
                                while(rsMan.Read()){
                                    
                                %>
                                <%
                                if (Request["ID"] == "0" && ( rsDef.LastReadSuccess) ) {
                                        sManID = cStr(rsDef.Item("ManufacturerID"));
                                }else{
                                    if (rs.LastReadSuccess)
                                    {
                                        sManID = (rs.Item("ManufacturerID"));
                                    }
                                }
                                %>
                                <option value="<%=(rsMan.Item("ID"))%>" <% if (Trim(rsMan.Item("ID")) == Trim(sManID)) { Response.Write("SELECTED") ; }else{ Response.Write("");}%>><%=(rsMan.Item("ManufacturerName"))%> (<%=(rsMan.Item("ManufacturerCode"))%>)</option>
                                <%
                                    }
                                                

                                rsMan.Requery();

                            %>
                                </select>
                            </td>
                            </tr>
                            <tr valign="baseline">
                            <td align="right" width="41" class="required">Units:</td>
                            <td width="155">
                                <input type="text" name="Units" value="<% if (rs.LastReadSuccess) { Response.Write(rs.Item("Units")); }%>" size="10" maxlength="10">
                            </td>
                            </tr>
                            <tr valign="baseline">
                            <td align="right" width="41">&nbsp;</td>
                            <td width="155">&nbsp;</td>
                            </tr>
                            <tr valign="baseline">
                            <td align="center" colspan="2">
                                <% if (!rs.LastReadSuccess)
                                   {     %>
                                <input type="button" value="Add" onClick="goAdd();" name="btnAdd" />
                                <input type="button" value="Save All Added" onClick="goSaveAll();" name="btnSaveAll" />
                                <% }else{ %>
                                <input type="button" value="Save" onClick="goValidate();" name="btnSave" />
                                <%} %>
                                <input type="button" name="btnCancel" value="Cancel" onClick="goCancel();" />
                                <% if ((cStr(Request["ID"]) != "0") && (UCase(Trim(rs.Item("ApprovalStatus"))) == "OPEN")) { %>
                                <input type="button" value="Delete" onClick="goDelete();" name="btnDelete">
                                <% } %>
                            </td>
                            </tr>
                            <tr valign="baseline">
                            <td align="right" width="41">&nbsp;</td>
                            <td width="155">&nbsp;</td>
                            </tr>
                            <tr valign="baseline">
                            <td align="right" width="41">Notes:</td>
                            <td width="155">&nbsp;</td>
                            </tr>
                            <tr valign="baseline">
                            <td align="left" valign='top' colspan="2" class="required">
                                <textarea name="Notes" rows="3" cols="40" style="width:210px"><% if (!rs.EOF) { Response.Write(rs.Item("Notes")); }  %></textarea>
                            </td>
                            </tr>
                            <tr valign="baseline">
                            <td align="right" width="41">&nbsp;</td>
                            <td width="155">&nbsp;</td>
                            </tr>
                            <tr valign="baseline">
                            <td align="right" width="41">Last Modified:</td>
                            <td width="155"><% if (rs.LastReadSuccess) { Response.Write(DateTime.Parse(rs.Item("LastModifiedOn")).ToString("MM/dd/yyyy")); }  %> </td>
                            </tr>
                            <tr valign="baseline">
                            <td align="right" width="41">By:</td>
                            <td width="155"><% if (rs.LastReadSuccess) { Response.Write(rs.Item("LastModifiedBy")); }  %> </td>
                            </tr>
                            <tr valign="baseline">
                            <td nowrap align="right" colspan="2">&nbsp;
                            </td>
                            </tr>
                        </table>
                    </td>
                        <td align="left" valign="top" width="249" >
                        <table width="100%" border="0">
                            <tr>
                            <td align="center" class='lblColor'><b>Daily Production <%=Request["WDate"]%><br><div id='divUnsavedCount' class='required'></div></b></td>
                            </tr>
                            <tr>
                            <td class="smallerText"> <%=ShowDetailForTheDay()%> </td>
                            </tr>
                        </table>
                        </td>
                    </tr>

                <!-- BXM End -->

            </table>
            <input type="hidden" name="MM_update" value="true" />
            <input type="hidden" name="MM_recordId" value="<% if (rs.LastReadSuccess) { Response.Write(rs.Item("Id"));} %>" />
            <input type="hidden" name="LastModifiedOn" value="<%=System.DateTime.Now.ToString()%>" size="32" />
            <input type="hidden" name="LastModifiedBy" value="<%=Session["UserName"]%>" size="32" />
            <input type="hidden" name="FacilityID" value="<%=Session["FacilityID"]%>" size="32" />
            <input type="hidden" name="WDate" value="<%=Request["WDate"]%>" size="32" />
                </form>
                Fields in <span class="Required">RED</span> are required.
                <% } %>
                <!-- #EndEditable -->
                </td>
            </tr>

        </table>
</asp:Content>