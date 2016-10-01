<%@ Page Language="C#" MasterPageFile="~/Reports/Reports.Master" AutoEventWireup="true" CodeBehind="FacilityMonitor.aspx.cs" Inherits="InterrailPPRS.Reports.FacilityMonitor" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script type="text/javascript" >
<!--
    function MM_reloadPage(init) {  //reloads the window if Nav4 resized
        if (init == true) with (navigator) {
            if ((appName == "Netscape") && (parseInt(appVersion) == 4)) {
                document.MM_pgW = innerWidth; document.MM_pgH = innerHeight; onresize = MM_reloadPage;
            } 
        }
        else if (innerWidth != document.MM_pgW || innerHeight != document.MM_pgH) location.reload();
    }
    MM_reloadPage(true);
    // -->

    function validateData() {

        var dDate = document.frmPrint.selDate;
        if (ValidDate(dDate, 1, "Date") != true) { return false; }

        document.frmPrint.action = "FacilityDataEntry.aspx"
        document.frmPrint.submit();

    }

    function annualDataEntry() {
        document.frmPrint.action = "FacilityAnnualEntry.aspx"
        document.frmPrint.submit();
    }

    function regionalpayrollEntry() {
        if (document.frmPrint.selRegion2.value == '0') {
            alert("You must select a Region.");
        } else {
            document.frmPrint.action = "RegionFacilityHours.aspx?selRegion=" + document.frmPrint.selRegion2.value
            document.frmPrint.submit();
        }
    }

    function payrollEntry() {
        document.frmPrint.action = "FacilityHours.aspx"
        document.frmPrint.submit();
    }
    function weeklyPopUp() {
        popUps = window.open("FacilityWeeklyCPU.aspx?selYear=" + document.frmPrint.selYear.value, "", "toolbar=yes,location=no,menubar=yes,scrollbars=yes,resizable=yes,left=50,top=50,screenX=50,screenY=50");
        popUps2 = window.open("FacilityWeeklyCPU2.aspx?selYear=" + document.frmPrint.selYear.value, "", "toolbar=yes,location=no,menubar=yes,scrollbars=yes,resizable=yes,left=50,top=50,screenX=50,screenY=50");
    }

    function weeklyCPUSummaryReportPopUp() {
        var dDate = document.frmPrint.selDateWeeklySummary;
        if (ValidDate(dDate, 1, "Date") != true) { return false; }

        popUps = window.open("WeeklyCPUSummaryReport.aspx?selDate=" + document.frmPrint.selDateWeeklySummary.value, "", "toolbar=yes,location=no,menubar=yes,scrollbars=yes,resizable=yes,left=50, top=50,screenX=50,screenY=50");
    }

    function dailyPopUp() {
        popUps = window.open("FacilityDailyCPU.aspx?selYear2=" + document.frmPrint.selYear2.value + "&selMonth=" + document.frmPrint.selMonth.value, "", "toolbar=yes,location=no,menubar=yes,scrollbars=yes,resizable=yes,left=50, top=50,screenX=50,screenY=50");
    }

    function monthlyPopUp() {
        popUps = window.open("FacilityMonthlyCPU.aspx?selYear=" + document.frmPrint.selYear.value, "", "toolbar=yes,location=no,menubar=yes,scrollbars=yes,resizable=yes,left=50, top=50,screenX=50,screenY=50");
    }

    function regionalPopUp() {
        if (document.frmPrint.selRegion1.value == '0') {
            alert("You must select a Region.");
        } else {
            popUps = window.open("FacilityRegionalCPU.aspx?selYear=" + document.frmPrint.selYear3.value + "&selMonth=" + document.frmPrint.selMonth3.value + "&selRegion=" + document.frmPrint.selRegion1.value, "", "toolbar=yes,location=no,menubar=yes,scrollbars=yes,resizable=yes,left=50, top=50,screenX=50,screenY=50");
        }
    }

    function regionalMonthlyPopUp() {
        if (document.frmPrint.selRegion2.value == '0') {
            alert("You must select a Region.");
        } else {
            popUps = window.open("FacilityRegionalMonthlyCPU.aspx?selYear=" + document.frmPrint.selYear5.value + "&selRegion=" + document.frmPrint.selRegion2.value, "", "toolbar=yes,location=no,menubar=yes,scrollbars=yes,resizable=yes,left=50, top=50,screenX=50,screenY=50");
        }
    }

    function corporatePopUp() {
        popUps = window.open("FacilityCorporateMonthlyCPU.aspx?selYear=" + document.frmPrint.selYear6.value, "", "toolbar=yes,location=no,menubar=yes,scrollbars=yes,resizable=yes,left=50, top=50,screenX=50,screenY=50");
    }

    function corporateunitsPopUp() {

        var from = document.frmPrint.fromDateRange;
        var to = document.frmPrint.toDateRange;
        var dateFrom = new Date(Date.parse(document.frmPrint.fromDateRange.value));
        var dateTo = new Date(Date.parse(document.frmPrint.toDateRange.value));

        if (ValidDate(from, 1, "Date From") != true) { return false; }
        if (ValidDate(to, 1, "Date To") != true) { return false; }
        if (dateFrom > dateTo) {
            alert(" 'Date From' must be prior to 'Date To'.");
            from.focus();
            from.select();
            return false;
        }
        else {
            popUps = window.open("FacilityCorporateUnits.aspx?selFrom=" + document.frmPrint.fromDateRange.value + "&selTo=" + document.frmPrint.toDateRange.value, "", "toolbar=yes,location=no,menubar=yes,scrollbars=yes,resizable=yes,left=50, top=50,screenX=50,screenY=50");
        }
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
                  <%if ( CheckSecurity("Super, Admin, User") ) { %>
                    <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
                    <td width="79%"><a href="AdminReports.aspx">Admin Reports</a></td>
                  </tr>
                 <%} %>

                 <%if ( CheckSecurity("Super, Admin, User, Production") ) { %>

                                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
                    <td width="79%"><a href="ProductionReports.aspx">Production Reports</a></td>
                  </tr>
                 <%} %>

                 <%if ( CheckSecurity("Super, Admin, User") )
                   { %>
                                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
                    <td width="79%"><a href="PayRollReports.aspx">Payroll Reports</a></td>
                  </tr>
                                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
                    <td width="79%"><a href="RebillReports.aspx">Rebilling Reports</a></td>
                  </tr>
                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%">&nbsp;</td>
                    <td width="79%">&nbsp;</td>
                  </tr>
                                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
                    <td width="79%"><a href="FacilityMonitor.aspx">Facility Monitor</a></td>
                  </tr>

                                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%">&nbsp;</td>
                    <td width="79%">&nbsp;</td>
                  </tr>

                  <tr>
                    <td width="8%">&nbsp;</td>
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12"></td>
                    <td width="79%"><a href="OtherReports.aspx">Other Reports</a></td>

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
            <td valign="top"><!-- #BeginEditable "MainBody" -->
                <form name='frmPrint' method='post' action=''>

                    <table width="91%" border="0" cellspacing="0" cellpadding="0">
		            <tr>
			            <td colspan='4'>

                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td colspan="4" class="pageTitle" align='center'>
                        <div class="cellTopBottomBorder">Facility Monitor - Facilities</div>
                        </td>
                    </tr>
                    <tr>
                        <td width="42%" align='right'  >&nbsp; </td>
                        <td width="22%" align='center' >&nbsp;</td>
                        <td width="18%" align='center' >&nbsp;</td>
                        <td width="18%" align='center' >&nbsp;</td>
                    </tr>
                    <tr>
                        <td  width="82%" align='right' colspan="3">Date (mm/dd/yyyy):&nbsp;
                        <% if ( cStr(Session["LastStartDate"]) == "") { %>
                            <input maxlength='10' size='10' type='text' name='selDate' value='<%= Date() %>' />
                        <% }else{ %>
                            <input maxlength='10' size='10' type='text' name='selDate' value='<%= cStr(Session["LastStartDate"]) %>' />
                        <% } %>
                        &nbsp;&nbsp;
                        </td>
                        <td align='left'>
                        <input style="width:234px" type="button" name="btnGoDataEntry" value='<%= cStr(Session["FacilityName"]) %>&nbsp;- Data FMS Entry' onClick='validateData();' />&nbsp;&nbsp;
                        </td>
                    </tr>

                    </table>
			            </td>
		            </tr>

                    <tr>
                        <td width="42%" align='right'  >&nbsp; </td>
                        <td width="22%" align='center' >&nbsp;</td>
                        <td width="18%" align='center' >&nbsp;</td>
                        <td width="18%" align='center' >&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="30%" align='right' valign='top' rowspan=2>Mon/Year:&nbsp;
                        <select name="selMonth">
                            <%
                            for( int i=1; i <= 12; i ++){
                                if (i == (cDate(Date()).Month)) {
                                Response.Write("<option value=" + i + " Selected>" + MonthName(i) + "</option>");
                                }else{
                                Response.Write("<option value=" + i + ">" + MonthName(i) + "</option>");
                                }
                            }
                            %>
                        </select><select name="selYear2">
                            <option value="<%=cStr(cInt(Year(System.DateTime.Now)) - 4)%>"><%=cStr(cInt(Year(System.DateTime.Now))- 4)%></option>
                            <option value="<%=cStr(cInt(Year(System.DateTime.Now)) - 3)%>"><%=cStr(cInt(Year(System.DateTime.Now)) - 3)%></option>
                            <option value="<%=cStr(cInt(Year(System.DateTime.Now)) - 2)%>"><%=cStr(cInt(Year(System.DateTime.Now)) - 2)%></option>
                            <option value="<%=cStr(cInt(Year(System.DateTime.Now)) - 1)%>"><%=cStr(cInt(Year(System.DateTime.Now)) - 1)%></option>
                            <option value="<%=cStr(Year(System.DateTime.Now))%>" Selected><%=cStr(Year(System.DateTime.Now))%></option>
                        </select>
                        </td>
                        <td  width="70%" align='right'>
                        <input style="width:234px" type="button" name="btnGoDaily" value="<%=cStr(Session["FacilityName"]) %>&nbsp;- Daily FMS" onClick='javascript:dailyPopUp()'>
                        </td>
                    </tr>

                    <tr>
                        <td width="42%" align='right'  >&nbsp; </td>
                        <td width="22%" align='center' >&nbsp;</td>
                        <td width="18%" align='center' >&nbsp;</td>
                        <td width="18%" align='center'>&nbsp;</td>
                    </tr>

                    <tr>
                        <td width="30%" align='right' rowspan=2 valign='top'>Year:&nbsp;
                        <select name="selYear">
                            <option value="<%=cStr(cInt(Year(System.DateTime.Now))- 4)%>"><%=cStr(cInt(Year(System.DateTime.Now))- 4)%></option>
                            <option value="<%=cStr(cInt(Year(System.DateTime.Now))- 3)%>"><%=cStr(cInt(Year(System.DateTime.Now))- 3)%></option>
                            <option value="<%=cStr(cInt(Year(System.DateTime.Now))- 2)%>"><%=cStr(cInt(Year(System.DateTime.Now))- 2)%></option>
                            <option value="<%=cStr(cInt(Year(System.DateTime.Now))- 1)%>"><%=cStr(cInt(Year(System.DateTime.Now))- 1)%></option>
                            <option value="<%=cStr(Year(System.DateTime.Now))%>" Selected><%=cStr(Year(System.DateTime.Now))%></option>
                        </select>
                        </td>
                        <td  width="70%" align='right' valign='top'>
                        <input  style="width:234px" type="button" name="btnGoWeekly" value="<%=Session["FacilityName"]%>&nbsp;- Weekly CPU" onClick='javascript:weeklyPopUp()'>
                        </td>
                    </tr>
                    <tr>
                        <td width="42%" align='right'  >&nbsp; </td>
                        <td width="22%" align='center' >&nbsp;</td>
                        <td width="18%" align='center' >&nbsp;</td>
                        <td width="18%" align='center' >&nbsp;</td>
                    </tr>                                
                                
                    <tr>
                        <td  width="70%" align='right' colspan=3>
                            <input  style="width:234px" type="button" name="btnGoMonthly" value="<%=Session["FacilityName"]%>&nbsp;- Monthly FMS" onClick='javascript:monthlyPopUp()'>
                        </td>
                    </tr>
                    <tr>
                        <td width="42%" align='right'  >&nbsp; </td>
                        <td width="22%" align='center' >&nbsp;</td>
                        <td width="18%" align='center' >&nbsp;</td>
                        <td width="18%" align='center'>&nbsp;</td>
                    </tr>

                    <tr>
                        <td width="42%" align='right' height=10 >&nbsp; </td>
                        <td width="22%" align='center' height=10 >&nbsp;</td>
                        <td width="18%" align='center' height=10 >&nbsp;</td>
                        <td width="18%" align='center' height=10 >&nbsp;</td>
                    </tr>

                    <% 

		            if ((cStr(Session["UserType"]) == "User" && rsRegions.LastReadSuccess) || (! (cStr(Session["UserType"]) == "User")) ){  
                                   
                    %>

		            <tr>
			            <td colspan="4" class="pageTitle" align='center'>
				            <div class="cellTopBottomBorder">Facility Monitor - Regional</div>
			            </td>
		            </tr>

		            <tr>
			            <td width="42%" align='right' height='1'>&nbsp; </td>
			            <td width="22%" align='center' height='1'>&nbsp;</td>
			            <td width="18%" align='center' height='1'>&nbsp;</td>
			            <td width="18%" align='center' height='1'>&nbsp;</td>
		            </tr>

                    <tr>
                        <td width="30%" align='right' rowspan=2>Year:&nbsp;
      		            <select name="selYear5">
                            <option value="<%=cStr(cInt(Year(System.DateTime.Now))- 4)%>"><%=cStr(cInt(Year(System.DateTime.Now)) - 4)%></option>
                            <option value="<%=cStr(cInt(Year(System.DateTime.Now))- 3)%>"><%=cStr(cInt(Year(System.DateTime.Now)) - 3)%></option>
                            <option value="<%=cStr(cInt(Year(System.DateTime.Now))- 2)%>"><%=cStr(cInt(Year(System.DateTime.Now))- 2)%></option>
                            <option value="<%=cStr(cInt(Year(System.DateTime.Now))- 1)%>"><%=cStr(cInt(Year(System.DateTime.Now)) - 1)%></option>
                            <option value="<%=cStr(Year(System.DateTime.Now))%>" Selected><%=cStr(Year(System.DateTime.Now))%></option>
                        </select>
                        </td>
                        <td width="70%" align='right'>
                            <input style="width:234px" type="button" name="btnGoRegionalMonthly" value="Regional - Monthly FMS" onClick='javascript:regionalMonthlyPopUp()' />
                        </td>
                    </tr>
                    <tr>
                        <td width="42%" align='right' height='6'></td>
                        <td width="22%" align='center' height='6'></td>
                        <td width="18%" align='center' height='6'></td>
                        <td width="18%" align='center' height='6'></td>
                    </tr>

                    <tr>
                        <td align='left' width="30%">Region:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <select name="selRegion2">
                            <option value="0" selected>-----Choose Region-----</option>
					            <%

					            while(! rsRegions.EOF){
                                    rsRegions.Read();
						            Response.Write("<option value='" + rsRegions.Fields(0) + "'>" + Trim(rsRegions.Fields(1)) + "</option>");
					            }

					            %>
                        </select>
                        </td>                                  
                        <td width="22%" align='center' >&nbsp;
                        <input style="width:234px" type="button" name="btnGoRegionalPayrollEntry" value="Regional - Facility Daily Payroll Entry" onClick='javascript:regionalpayrollEntry()' id="bntRegionalPayrollEntry" />
			            </td>
                    </tr>

                                

                    <tr>
                        <td width="42%" align='right' height=10 >&nbsp; </td>
                        <td width="22%" align='center' height=10 >&nbsp;</td>
                        <td width="18%" align='center' height=10 >&nbsp;</td>
                        <td width="18%" align='center' height=10 >&nbsp;</td>
                    </tr>

            <%  } %>


                <% if ((! (cStr(Session["UserType"]) == "User")) || (rsRegions.RecordCount > 0) ) { %>


                    <tr>
                        <td width='100%' colspan='4'>

			            <table width="100%" border="0" cellspacing="0" cellpadding="0">
				            <tr>
					            <td colspan="4" class="pageTitle" align='center'>
					            <div class="cellTopBottomBorder">Facility Monitor - Corporate</div>
					            </td>
				            </tr>
				            <tr>
					            <td width="42%" align='right'  >&nbsp; </td>
					            <td width="22%" align='center' >&nbsp;</td>
					            <td width="18%" align='center' >&nbsp;</td>
					            <td width="18%" align='center' >&nbsp;</td>
				            </tr>

                <% if (! (cStr(Session["UserType"]) == "User")) { %>

				            <tr>
					            <td  width="82%" align='right' colspan=3>Year:&nbsp;
					            <select name="selYear4">
						            <option value="<%=cStr(cInt(Year(System.DateTime.Now)) - 4)%>"><%=cStr(cInt(Year(System.DateTime.Now)) - 4)%></option>
						            <option value="<%=cStr(cInt(Year(System.DateTime.Now)) - 3)%>"><%=cStr(cInt(Year(System.DateTime.Now)) - 3)%></option>
						            <option value="<%=cStr(cInt(Year(System.DateTime.Now)) - 2)%>"><%=cStr(cInt(Year(System.DateTime.Now)) - 2)%></option>
						            <option value="<%=cStr(cInt(Year(System.DateTime.Now)) - 1)%>"><%=cStr(cInt(Year(System.DateTime.Now)) - 1)%></option>
						            <option value="<%=cStr(Year(System.DateTime.Now))%>" Selected><%=cStr(Year(System.DateTime.Now))%></option>
					            </select>
					            &nbsp;&nbsp;
					            </td>
					            <td  align='left'>
					            <input style="width:234px" type="button" name="btnGoAnnualEntry" value="Annual Data Entry" onClick="annualDataEntry();" />&nbsp;&nbsp;
					            </td>
				            </tr>

				            <tr>
					            <td width="42%" align='right'  >&nbsp; </td>
					            <td width="22%" align='center' >&nbsp;</td>
					            <td width="18%" align='center' >&nbsp;</td>
					            <td width="18%" align='center'>&nbsp;</td>
				            </tr>

				            </table>

                        </td>
                    </tr>

                    <tr>
                        <td width="30%" align='right' valign='top' rowspan=2>Year:&nbsp;
                        <select name="selYear6">
                            <option value="<%=cStr(cInt(Year(System.DateTime.Now)) - 4)%>"><%=cStr(cInt(Year(System.DateTime.Now)) - 4)%></option>
                            <option value="<%=cStr(cInt(Year(System.DateTime.Now)) - 3)%>"><%=cStr(cInt(Year(System.DateTime.Now)) - 3)%></option>
                            <option value="<%=cStr(cInt(Year(System.DateTime.Now)) - 2)%>"><%=cStr(cInt(Year(System.DateTime.Now)) - 2)%></option>
                            <option value="<%=cStr(cInt(Year(System.DateTime.Now)) - 1)%>"><%=cStr(cInt(Year(System.DateTime.Now)) - 1)%></option>
                            <option value="<%=cStr(Year(System.DateTime.Now))%>" Selected><%=cStr(Year(System.DateTime.Now))%></option>
                        </select>
                        </td>
                        <td  width="70%" align='right' valign='bottom'>
                        <input  style="width:234px" type="button" name="btnGoCorporate" value="Corporate FMS" onClick='javascript:corporatePopUp()'>
                        </td>
                    </tr>

            <% } %>
            <% if (rsRegions.RecordCount > 0 ){ %>

            <tr>
                        <td width="42%" align='right' height='15'>&nbsp; </td>
                        <td width="22%" align='center' height='15'>&nbsp;</td>
                        <td width="18%" align='center' height='15'>&nbsp;</td>
                        <td width="18%" align='center' height='15'>&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="30%" align='right' rowspan=2>Mon/Year:&nbsp;
                        <select name="selMonth3">
                            <%
                            for(int i=1; i <= 12; i ++){
                                if (i == (cDate(Date()).Month)) {
                                Response.Write("<option value=" + i + " Selected>" + MonthName(i) + "</option>");
                                }else{
                                Response.Write("<option value=" + i + ">" + MonthName(i) + "</option>");
                                }
                            }
                            %>
                        </select><select name="selYear3">
                            <option value="<%=cStr(cInt(Year(System.DateTime.Now)) - 4)%>"><%=cStr(cInt(Year(System.DateTime.Now))- 4)%></option>
                            <option value="<%=cStr(cInt(Year(System.DateTime.Now)) - 3)%>"><%=cStr(cInt(Year(System.DateTime.Now)) - 3)%></option>
                            <option value="<%=cStr(cInt(Year(System.DateTime.Now)) - 2)%>"><%=cStr(cInt(Year(System.DateTime.Now)) - 2)%></option>
                            <option value="<%=cStr(cInt(Year(System.DateTime.Now)) - 1)%>"><%=cStr(cInt(Year(System.DateTime.Now)) - 1)%></option>
                            <option value="<%=cStr(Year(System.DateTime.Now))%>" Selected><%=cStr(Year(System.DateTime.Now))%></option>
                        </select>
                        </td>
                        <td width="70%" align='right'>
                        <input style="width:234px" type="button" name="btnGoRegional" value="Corporate Regional CPU" onClick='javascript:regionalPopUp()'>
                        </td>
                    </tr>
                    <tr>
                        <td width="42%" align='right' height='1' ></td>
                        <td width="22%" align='center' height='1'></td>
                        <td width="18%" align='center' height='1'></td>
                        <td width="18%" align='center' height='1'></td>
                    </tr>

		            <tr>
                        <td align='left' width="30%">Region:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <select name="selRegion1">
                            <option value="0" selected>-----Choose Region-----</option>
					            <%
					            rsRegions.Requery();
					            while (! rsRegions.EOF){
                                    rsRegions.Read();
						            Response.Write("<option value='" + rsRegions.Fields(0) + "'>" + Trim(rsRegions.Fields(1)) + "</option>");
					            }

					            %>
                        </select>
                        </td>
                        <td width="70%" align='right'>
                        </td>
                    </tr>


            <% } %>
            <% } %>
                <% if (! (cStr(Session["UserType"]) == "User")){ %>
		            <tr class='ReportOddLine'>
		            <td>
		            <table width="100%"  border="0" cellspacing="0" cellpadding="0">
			            <tr>
			            <td width="50%" align='center' class='cellTopBottomBorder'>From</td>
                        <td width="50%" align='center' class='cellTopBottomBorder'>To</td>
			            </tr>
			            </table>
			            </td>
                    </tr>
		            <tr width="100%">
		            <td>
		            <table width="100%"  border="0" cellspacing="0" cellpadding="0">
		            <tr>
			            <td width="50%" align='center'>
			            <% if ( cStr((Session["LastStartDate"])) == "") { %>
                            <input maxlength='10' size='10' type='text' name='fromDateRange' value='<%= Date() %>' >
                        <% }else{ %>
                            <input maxlength='10' size='10' type='text' name='fromDateRange' value='<%= Session["LastStartDate"] %>' >
                        <% } %>
			            </td>

                        <td width="50%" align='center'>
			            <input type="text" name="toDateRange" size="10" maxlength="10" value=''>
			            </td>
			            </tr>
			            </table>
			            </td>
                        <td>
				            <input style="width:234px" type="button" name="btnGoUnits" value="Corporate Total Units" onClick='javascript:corporateunitsPopUp()'>
			            </td>

		            </tr>


 		            <tr>
                        <td width="42%" align='right' height='15'>&nbsp; </td>
                        <td width="22%" align='center' height='15'>&nbsp;</td>
                        <td width="18%" align='center' height='15'>&nbsp;</td>
                        <td width="18%" align='center' height='15'>&nbsp;</td>
                    </tr>

		            <tr >
		            <td>
		            </td>
                    </tr>

		            <tr width="100%">
		            <td>
		            <table width="100%"  border="0" cellspacing="0" cellpadding="0" id="Table2">
		            <tr>
			            <td width="50%" align='center'>
			            </td>

                        <td width="50%" align='center'>
			            </td>
			            </tr>
			            </table>
			            </td>
                        <td>
				            <input style="width:234px" type="button" name="btnGoPayrollEntry" value="Corporate - Facility Daily Payroll Entry" onClick='javascript:payrollEntry()' ID="bntPayrollEntry">
			            </td>

		            </tr>
		            <tr>
                        <td width="42%" align='right'  >&nbsp; </td>
                        <td width="22%" align='center' >&nbsp;</td>
                        <td width="18%" align='center' >&nbsp;</td>
                        <td width="18%" align='center' >&nbsp;</td>
                    </tr>
		            <tr>
			            <td  width="82%" align='right' >Date (mm/dd/yyyy):&nbsp;
                        <% if (cStr(Session["LastStartDate"]) == "") { %>
                            <input maxlength='10' size='10' type='text' name='selDateWeeklySummary' value='<%= Date() %>' id="Text1" />
                        <% }else{ %>
                            <input maxlength='10' size='10' type='text' name='selDateWeeklySummary' value='<%= Session["LastStartDate"] %>' id="Text2" />
                        <% } %>
                        &nbsp;&nbsp;
                        </td>
			            <td  width="70%" align='right' valign='top'>                                    
                        <input  style="width:234px" type="button" name="btnGoWeeklyCPUSummary" value="Weekly CPU Summary Report" onClick='javascript:weeklyCPUSummaryReportPopUp()' ID="btnGoWeeklyCPUSummary">
                        </td>
                    </tr>



            <% } %>

 		            <tr>
                        <td width="42%" align='right' height='15'>&nbsp; </td>
                        <td width="22%" align='center' height='15'>&nbsp;</td>
                        <td width="18%" align='center' height='15'>&nbsp;</td>
                        <td width="18%" align='center' height='15'>&nbsp;</td>
                    </tr>

                    </table>

            </form>

            <!-- #EndEditable --> </td>
        </tr>
    </table>
</asp:Content>