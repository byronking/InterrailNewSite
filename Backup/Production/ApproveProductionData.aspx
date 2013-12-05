<%@ Page Language="C#" MasterPageFile="~/Production/Production.Master" AutoEventWireup="true" CodeBehind="ApproveProductionData.aspx.cs" Inherits="InterrailPPRS.Production.ApproveProductionData" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script  type="text/javascript">

var jsAlreadyIn = '<%=sNotIn%>';

<!--
function MM_reloadPage(init) {  //reloads the window if Nav4 resized
  if (init==true) with (navigator) {if ((appName=="Netscape")&&(parseInt(appVersion)==4)) {
    document.MM_pgW=innerWidth; document.MM_pgH=innerHeight; onresize=MM_reloadPage; }}
  else if (innerWidth!=document.MM_pgW || innerHeight!=document.MM_pgH) location.reload();
}
MM_reloadPage(true);

function bodyload()
{

  frm = document.form1;
  // Select corresponding Type/Status for this day (DayApprovalStatus)
  var iLen = frm.selAllDates.options.length;
  if (iLen == 0)
  {
     var newOption = new Option('<%=Date()%>', '<%=Date()%>', false, false); 
     frm.selAllDates.options[0] = newOption;
  }
  else
  {
    if ('<%=Request["forDate"]%>' == '') 
        {
          getData();
        }
  }
}

function goGenerate()
{
    var to   = document.form1.toDate;
        
        if (jsAlreadyIn != '')
        {
        if (jsAlreadyIn.indexOf("|"+to.value+"|") !=-1)
            {
              alert('There is already an entry for this day: ' + to.value+'.\nPlease select date from the list.');
              return false;
        }
    }
        
    var dateTo   = new Date(Date.parse(document.form1.toDate.value));
    var today    = new Date(Date.parse('<%=Date()%>'));

    if (ValidDate(to,   1, "Date To")    != true) {return false;}

    if (dateTo > today)
    {
      alert(" 'Date' must be less than or equal to today.");
      to.focus();
      to.select();
      return false;
    }

    document.form1.action = "ApproveProductionData.aspx?Generate=NEW&From="+escape(to.value)+"&To="+escape(to.value);
    //alert( document.form1.action);
    document.form1.submit();
}

function goApprove(sType)
{
  frm = document.form1;

  if (sType == "NOPROD")
  {
     var sWhich = "&Which="+sType;
  }
  else
  {
     var sWhich = "&Which="+sType;
  }

  var iSel = frm.selAllDates.selectedIndex;
  frm.DayApprovalStatus.selectedIndex = iSel;
  vDType = frm.DayApprovalStatus.options[iSel].text;
  
  if ('<%=Trim(UCase(cStr(Session["UserType"])))%>' == 'USER')
  {
        frm.action = "ApproveProductionData.aspx?Approval=FACILITY"+sWhich+"&DType="+vDType;
  }
  else
  {
        frm.action = "ApproveProductionData.aspx?Approval=CORPORATE"+sWhich+"&DType="+vDType;
  }
  
  if (!confirm("Approved data cannot be modified.\nPress OK to Approve."))
  {
        return false;
  }

  frm.submit();
}

function getData()
{
  frm = document.form1;
  // Select corresponding Type/Status for this day (DayApprovalStatus)
  var iSel = frm.selAllDates.selectedIndex;
  frm.DayApprovalStatus.selectedIndex = iSel;
  
  vDType = frm.DayApprovalStatus.options[iSel].text;
  vDStatus = frm.DayApprovalStatus.options[iSel].value;
  
  //alert("Type = " + vDType + "   Status = " + vDStatus);
  
  frm.forDate.value = frm.selAllDates.value;
  var forDate = frm.forDate;

  if (ValidDate(forDate, 1, "Date (Select Date)")  != true) {return false;}

  frm.action = "ApproveProductionData.aspx?forDate="+frm.forDate.value+"&DType="+vDType;
  //alert(frm.action);
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
            <form name="form1" method="post" action="">
                <table align="center"  border="0" cellspacing="0" cellpadding="0" width="100%" >
                <tr> 
                    <td align="default" colspan="5"> 
                    <div align="center" class="pageTitle">Approve 
                        Production Data </div>
                    </td>
                </tr>
                <tr> 
                    <td align="default" colspan="5" > 
                    <div align="center" class="pageTitle"><%=Session["FacilityName"]%></div>
                    </td>
                </tr>
                <tr> 
                    <td align="left" colspan="2" >&nbsp;</td>
                    <td align="default" width="10%" >&nbsp;</td>
                    <td align="default" width="10%" >&nbsp;</td>
                    <td align="right" width="60%"   >&nbsp;</td>
                </tr>
                <tr> 
                    <td align="left" colspan="2" >&nbsp;</td>
                    <td align="default" width="10%" >&nbsp;</td>
                    <td align="default" width="10%" >&nbsp;</td>
                    <td align="right" width="60%"   >&nbsp;</td>
                </tr>
                <tr> 
                    <td align='right' valign='middle'>&nbsp;  </td>
                    <td align='right' valign='middle'>&nbsp;</td>
                    <td align='right' valign='middle'><input type="button" name="btnGo" value='Add' onClick="goGenerate();">&nbsp;</td>
                    <td align='right' valign='middle'>&nbsp;<input type="text" name="toDate" size="10" maxlength="10" value='<%=System.DateTime.Now.ToShortDateString()%>'></td>
                    <td align='left' valign='middle'>&nbsp;to "No Production Data".&nbsp;</td>
                </tr>
                <tr> 
                    <td align="left" colspan="2" >&nbsp; </td>
                    <td align="default" width="10%" >&nbsp; </td>
                    <td align="default" width="10%" >&nbsp;</td>
                    <td align="right" width="60%"   >&nbsp;</td>
                </tr>
                <tr> 
                    <td align="left" colspan='5' > 
                    <div id="Layer1" style="position:absolute; width:1px; height:1px; z-index:1; visibility: hidden; left: 278px; top: 907px; overflow: hidden" > 
                        <select name="DayApprovalStatus">
                        <%
                            while (!rsAllDates.EOF)
                            {
                                rsAllDates.Read();
                        %>
                            <option value="<%=(rsAllDates.Item("DStatus"))%>"><%=(rsAllDates.Item("DType"))%></option>
                        <%
                            }
                        rsAllDates.Requery();

                    %>
                        </select>
                    </div>
                    </td>
                </tr>
                <tr> 
                    <td align="left" colspan="2" > 
                    <input type="hidden" name="forDate" size="10" maxlength="10" value='<%=rs__PDate%>'>
                    &nbsp;&nbsp; 
                    <select name="selAllDates" onChange='getData();'>
                        <%
                        while (! rsAllDates.EOF){
                            rsAllDates.Read();
                        %>
                        <option value="<%=(rsAllDates.Item("WDate"))%>" <%if (cStr(cDate(rsAllDates.Item("WorkDate"))) == cStr(cDate(rs__PDate))) { Response.Write("SELECTED"); }else{ Response.Write("");} %> ><%=(rsAllDates.Item("WDate"))%></option>
                        <%
                        }

                        rsAllDates.Requery();

                    %>
                    </select>
                    </td>
                    <td align="right" colspan="3" ><font color='green'> 
                    </font><font color='green'> <b>&nbsp;</b></font><font color='green'> 
                    </font><font color='green'> </font><font color='green'> 
                    </font><font color='green'> 
                    <% if (ApproveAll) { %>
                        <input type="button" name="btnAll" value="Approve ALL" onClick="goApprove('ALL');" />
                    <% }else{ %>
                    <% } %>
                    <% if (sDType != "NOPROD") { %>
                        <input type="button" name="btnChecked" value="Approve Checked Only" onClick="goApprove('CHECKED');" />
                    <% } %>
                    </font></td>
                </tr>
                <tr> 
                    <td align="default" width="10%" >&nbsp;</td>
                    <td align="default" width="10%" >&nbsp;</td>
                    <td align="default" width="10%" >&nbsp;</td>
                    <td align="default" width="10%" >&nbsp;</td>
                    <td align="right"   width="60%" >&nbsp;</td>
                </tr>
                <% if (sDType == "NOPROD") { %>
                <tr class='ReportOddLine'> 
                    <td align="default" colspan="5" class="cellTopBottomBorder">Approval Status&nbsp;&nbsp;-&nbsp;&nbsp;<i>"No Production Data"</i>&nbsp;&nbsp;for this day.</td>
                </tr>
                <% }else{ %>
                <tr> 
                    <td align="default" colspan="2" class="cellTopBottomBorder">Approval Status </td>
                    <td align="right" class="cellTopBottomBorder" width="20%">Task &nbsp;</td>
                    <td align="default" class="cellTopBottomBorder">Items (Units)</td>
                    <td align="default" class="cellTopBottomBorder">&nbsp;</td>
                </tr>
                <% } %>
                <%string  rowColor = "";
                if (rs.EOF) {
                %>
                <tr> 
                    <td align="Left" colspan="5">No entries for 
                    <%= rs__PDate %>.&nbsp;&nbsp;Please select a different 
                    date <b>OR</b> add it to &quot;No Production 
                    Data&quot; days.</td>
                </tr>
                <%
                }

                if (sDType == "PROD") {                                                               

                    while ((Repeat1__numRows != 0) && (! rs.EOF)) {
                                    
                    rs.Read();

                    if (Repeat1__index % 2 == 0) {
                        rowColor = "reportEvenLine";
                    }else{ 
                        rowColor = "reportOddLine";
                    }
                                  

                    sDuplicates =  "      SELECT Distinct FacilityProductionDetail.TaskID, COUNT(*) AS NRec       ";
                    sDuplicates += "               FROM FacilityProductionDetail                                    ";
                    sDuplicates +=  "               INNER JOIN Tasks ON FacilityProductionDetail.TaskId = Tasks.Id   ";
                    sDuplicates +=  "               RIGHT OUTER JOIN FacilityTasks ON                                ";
                    sDuplicates +=  "               FacilityProductionDetail.FacilityID = FacilityTasks.FacilityID   ";
                    sDuplicates +=  "               AND Tasks.Id = FacilityTasks.TaskId                              ";
                    sDuplicates +=  "         WHERE FacilityProductionDetail.FacilityID = " + cStr(rs__PFacilityID);
                    sDuplicates +=  "               AND FacilityProductionDetail.WorkDate = '" + cStr(rs__PDate) + "'";
                    sDuplicates +=  "               AND FacilityProductionDetail.TaskID = " + rs.Item("TaskID");    
                    sDuplicates +=  "      GROUP BY TaskCode, FacilityProductionDetail.TaskID, CarTypeId, RailCarNumber  ";
                    sDuplicates +=  "      ORDER BY NRec Desc                                                        ";
                                      
                    rst = new InterrailPPRS.Production.DataReader(sDuplicates);
                    rst.Open();
                    rst.Read();    
                    int Dup = cInt(rst.Item("NRec"));

                %>
                <tr class="<%=rowColor%>"> 
                    <td align="right" width="10%"> 
                    <% if ( (UCase(Trim(rs.Item("ApprovalStatus"))) == "OPEN")  ||  ((Trim(UCase(cStr(Session["UserType"]))) != "USER") && (UCase(Trim(rs.Item("ApprovalStatus"))) != "CORPORATE")) ) { %>
                    <% if (Dup == 1) { %>
                    <input type="checkbox" name="cbxApprove" value="<%=(rs.Item("TaskID"))%>">
                    <% }else{ %>
                    <div class="required"><b>??&nbsp;</b></div>
                    <% } %>
                    <% }else{ %>
                    &nbsp; 
                    <% } %>
                    </td>
                    <td align="default" width="10%"><%=(rs.Item("ApprovalStatus"))%></td>
                    <td align="right" width="10%"><a href="DetailEdit.aspx?Id=0&WDate=<%=rs__PDate%>" ><%=(rs.Item("TaskCode"))%></a>: &nbsp;</td>
                    <td align="default"  width="10%"><%=(rs.Item("NRec"))%> (<%=(rs.Item("TU"))%>) </td>
                    <td align="default"  width="60%"> 
                    <% if (Dup > 1) {%>
                    <div class="required">Duplicate Car Number/Task, cannot be approved.</div>
                    <% } %>
                    <% if ( ((Trim(UCase(cStr(Session["UserType"])))) != "USER") && (UCase(Trim(rs.Item("ApprovalStatus"))) != "OPEN")) { %>
                    <a href='ApproveProductionData.aspx?OpenTask=<%=rs.Item("TaskID")%>&WDate=<%=rs__PDate%>&DType=PROD'>Set Status to OPEN</a> 
                    <% }else{ %>
                    &nbsp; 
                    <% } %>
                    </td>
                </tr>
                <% 
                    Repeat1__index = Repeat1__index+1;
                    Repeat1__numRows = Repeat1__numRows-1;

                    } //Wend
                %>
                <% } %>
                <% if (sDType == "NOPROD") {
                    rs.Requery();
                    rs.Read();
                %>
                <tr class="<%=rowColor%>"> 
                    <td align="left"  colspan='2'> <%=(rs.Item("ApprovalStatus"))%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                    </td>
                    <td  align="right"  colspan='1'> 
                    <% if ( (UCase(Trim(rs.Item("ApprovalStatus"))) == "OPEN")  ||  ((Trim(UCase(cStr(Session["UserType"]))) != "USER") && (UCase(Trim(rs.Item("ApprovalStatus"))) != "CORPORATE")) ) { %>
                        <a href='javascript:goApprove("NOPROD");'>Approve</a> 
                    <% }else{ %>
                        &nbsp; 
                    <%} %>
                    </td>
                    <td  align="right"  colspan='2'> 
                    <% if ( ((Trim(UCase(cStr(Session["UserType"])))) != "USER") && (UCase(Trim(rs.Item("ApprovalStatus"))) != "OPEN")) { %>
                        <a href='ApproveProductionData.aspx?OpenNoProdData=Y&forDate=<%=rs__PDate%>'>Set Status to OPEN</a> 
                    <% }else{ %>
                        &nbsp; 
                    <% } %>
                    </td>
                </tr>
                <% } %>
                </table>
                <font color='green'> </font> 
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