<%@ Page Language="C#" MasterPageFile="~/Reports/AdminReports.Master" AutoEventWireup="true" Codebehind="AdminReports.aspx.cs" Inherits="InterrailPPRS.Reports.AdminReport" %>

<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script type="text/javascript">
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

                 <%if ( CheckSecurity("Super, Admin, User") ) { %>
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
                    <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" /></td>
                    <td width="79%"><a href="../Logout.aspx">Logout</a></td>
                  </tr>

                </table>
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="Main" runat="server">

		<table width="90%" border="0" cellspacing="0" cellpadding="8">
			<tr>
				<td valign="top"><!-- #BeginEditable "MainBody" -->
					<table width="91%" border="0" cellspacing="0" cellpadding="0">
						<tr>
							<td colspan="4" class="pageTitle" align='center'>
								<div class="cellTopBottomBorder">Admin Reports</div>
							</td>
						</tr>
						<tr>
							<td width="52%" align='right' height="16">&nbsp;</td>
							<td width="17%" align='center' height="16">&nbsp;</td>
							<td width="17%" align='center' height="16">&nbsp;</td>
							<td width="14%" height="16">&nbsp;</td>
						</tr>
						<tr>
							<td colspan="4" align='right' height="16">
								<form id="Form1" method="post" runat="server">
									<table width="100%" border="0">
										<tr>
											<td>&nbsp;</td>
											<td></td>
											<td>&nbsp;</td>
											<td>&nbsp;</td>
											<td>&nbsp;</td>
											<td>&nbsp;</td>
											<td></td>
										</tr>
										<tr>
											<td><asp:dropdownlist ID="DBTableDropDown" runat="server" AutoPostBack="True" onselectedindexchanged="DBTableDropDown_Changed"></asp:dropdownlist></td>
											<td></td>
											<td>&nbsp;</td>
											<td colspan="2"><asp:label ID="Label2" runat="server" Width="201px">Fields to show on Report</asp:label></td>
											<td>&nbsp;</td>
											<td></td>
										</tr>
										<tr>
											<td rowspan="9">
                                                <asp:label ID="Label5" runat="server" Width="110px">Fields to Show, Match or Order</asp:label><br />
												<asp:listbox ID="lbFields" runat="server" Width="110px" Height="329px" AutoPostBack="True" onselectedindexchanged="lbFields_SelectedIndexChanged"></asp:listbox>
											</td>
											<td></td>
											<td><asp:button ID="bntAddSelect" runat="server" Text=">>" ToolTip="Add" onclick="bntAddSelect_Click"></asp:button>
												<asp:button ID="btnSelectAddAll" runat="server" Text="Add All" onclick="btnSelectAddAll_Click"></asp:button></td>
											<td colspan="2">
												<asp:listbox ID="lbSelect" runat="server" Width="220px" Height="123px"></asp:listbox></td>
											<td><asp:button ID="bntSelectUp" runat="server" Text="^" ToolTip="Move Up" onclick="bntSelectUp_Click"></asp:button>
												<asp:button ID="bntSelectDown" runat="server" Text="v" onclick="bntSelectDown_Click"></asp:button>
												<asp:button ID="bntSelectDelete" runat="server" Text="X" ToolTip="Delete" onclick="bntSelectDelete_Click"></asp:button></td>
											<td></td>
										</tr>
										<tr>
											<td>&nbsp;</td>
											<td></td>
											<td>&nbsp;</td>
											<td>&nbsp;</td>
											<td>&nbsp;</td>
											<td></td>
										</tr>
										<tr>
											<td>&nbsp;</td>
											<td></td>
											<td>&nbsp;</td>
											<td>&nbsp;</td>
											<td>&nbsp;</td>
											<td></td>
										</tr>
										<tr>
											<td>&nbsp;</td>
											<td></td>
											<td>&nbsp;</td>
											<td><asp:label ID="Label3" runat="server" Width="201px">Show Records that match</asp:label></td>
											<td>&nbsp;</td>
											<td></td>
										</tr>
										<tr>
											<td>&nbsp;</td>
											<td>
												<asp:dropdownlist ID="lbWhereOp" runat="server" AutoPostBack="True" onselectedindexchanged="lbWhereOp_SelectedIndexChanged"></asp:dropdownlist>
												<asp:textbox ID="tbWhereOne" Width="120px" runat="server"></asp:textbox>
												<asp:textbox ID="tbWhereTwo" Width="120px" runat="server"></asp:textbox></td>
											<td><asp:button ID="bntAddWhere" runat="server" Text=">>" ToolTip="Add" onclick="bntAddWhere_Click"></asp:button></td>
											<td><asp:listbox ID="lbWhere" runat="server" Width="200px" onselectedindexchanged="ListBox1_SelectedIndexChanged"></asp:listbox></td>
											<td><asp:button ID="btnWhereDelete" runat="server" Text="X" ToolTip="Delete" onclick="btnWhereDelete_Click"></asp:button></td>
											<td></td>
										</tr>
										<tr>
											<td>&nbsp;</td>
											<td></td>
											<td>&nbsp;</td>
											<td><asp:button ID="btnWhereAnd" runat="server" Text="And" onclick="btnWhereAnd_Click"></asp:button>
												<asp:button ID="btnWhereOr" runat="server" Text="Or" onclick="btnWhereOr_Click"></asp:button></td>
											<td>&nbsp;</td>
											<td></td>
										</tr>
										<tr>
											<td>&nbsp;</td>
											<td></td>
											<td colspan="2"><asp:label ID="Label4" runat="server" Width="201px">Order by</asp:label></td>
											<td>&nbsp;</td>
											<td>&nbsp;</td>
										</tr>
										<tr>
											<td>&nbsp;</td>
											<td><asp:button ID="btnOrderAdd" runat="server" Text=">>" ToolTip="Add" onclick="btnOrderAdd_Click"></asp:button></td>
											<td colspan="2">
												<asp:listbox ID="lbOrder" runat="server" Width="220px" Height="102px"></asp:listbox></td>
											<td><asp:button ID="btnOrderUp" runat="server" Text="^" ToolTip="Move Up" onclick="btnOrderUp_Click"></asp:button>
												<asp:button ID="btnOrderDown" runat="server" Text="v" ToolTip="Move Down" onclick="btnOrderDown_Click"></asp:button>
												<asp:button ID="btnOrderDelete" runat="server" Text="X" ToolTip="Delete" Height="29px" onclick="btnOrderDelete_Click"></asp:button></td>
											<td>&nbsp;</td>
										</tr>
										<tr>
											<td>&nbsp;</td>
											<td>&nbsp;</td>
											<td>&nbsp;</td>
											<td>&nbsp;</td>
											<td>&nbsp;</td>
											<td>&nbsp;</td>
										</tr>
										<tr>
											<td>&nbsp;</td>
											<td></td>
											<td>&nbsp;</td>
											<td>&nbsp;</td>
											<td>&nbsp;</td>
											<td>&nbsp;</td>
											<td></td>
										</tr>
										<tr>
											<td><asp:label ID="Label1" runat="server" Width="123px" Visible="False"></asp:label></td>
											<td></td>
											<td>&nbsp;</td>
											<td>&nbsp;</td>
											<td><asp:button ID="btnGenerate" runat="server" Text="Generate Report" onclick="btnGenerate_Click"></asp:button></td>
											<td>&nbsp;</td>
											<td></td>
										</tr>
									</table>
									<BR>
									<BR>
									&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
								</form>
							</td>
						</tr>
					</table>
					<!-- #EndEditable -->
				</td>
			</tr>
		</table>

</asp:Content>