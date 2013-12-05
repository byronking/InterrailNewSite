<%@ Page language="c#" Codebehind="ManageAttachments.aspx.cs" AutoEventWireup="True" Inherits="InterrailPPRS.Rebilling.ManageAttachments" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Rebill Attachments</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body text="#000000" bgColor="#ffffff" leftMargin="0" background="../images/bglines.gif"
		topMargin="0" marginheight="0" marginwidth="0">
		<table cellSpacing="0" cellPadding="0" width="100%" border="0">
			<tr>
				<td bgColor="#9999cc">
					<table cellSpacing="0" cellPadding="0" width="760" border="0">
						<tr>
							<td class="company" width="272" bgColor="#ffffff">
								<div align="center"><asp:label id="lbCompanyName" runat="server"></asp:label></div>
							</td>
							<td width="488"><IMG height="68" src="../images/train.jpg" width="488"></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td>
					<table height="32" cellSpacing="0" cellPadding="0" width="100%" border="0">
						<tr>
							<td width="100%" background="../images/topbevelbg.gif" bgColor="#9999cc" height="6"><IMG height="6" src="../images/topbevel.gif" width="760"></td>
						</tr>
						<tr>
							<td bgColor="#ff3300">
								<table cellSpacing="0" cellPadding="0" width="760" border="0">
									<tr>
										<td class="date" width="200" background="../images/dateblock.gif" bgColor="#ff3300">
											<div align="center">
												<script language="JavaScript1.2"><!--
var months=new Array(13);
months[1]="JANUARY";
months[2]="FEBRUARY";
months[3]="MARCH";
months[4]="APRIL";
months[5]="MAY";
months[6]="JUNE";
months[7]="JULY";
months[8]="AUGUST";
months[9]="SEPTEMBER";
months[10]="OCTOBER";
months[11]="NOVEMBER";
months[12]="DECEMBER";
var time=new Date();
var lmonth=months[time.getMonth() + 1];
var date=time.getDate();
var year=time.getYear();
if (year < 2000)    // Y2K Fix, Isaac Powell
year = year + 1900; // http://onyx.idbsu.edu/~ipowell
document.write(lmonth + " ");
document.write(date + ", " + year);
// -->
												</script>
											</div>
										</td>
										<td width="45"><IMG height="20" src="../images/navgrad01.gif" width="45"></td>
										<td width="115"><A href="../Admin/Default.aspx"><IMG height="20" src="../images/nav01.gif" width="115" border="0"></A></td>
										<td width="91"><A href="../Production/Default.aspx"><IMG height="20" src="../images/nav02.gif" width="91" border="0"></A></td>
										<td width="68"><A href="../Payroll/Default.aspx"><IMG height="20" src="../images/nav03.gif" width="68" border="0"></A></td>
										<td width="83"><A href="Default.aspx"><IMG height="20" src="../images/nav04a.gif" width="83" border="0"></A></td>
										<td width="83"><A href="../Reports/Default.aspx"><IMG height="20" alt="Reports" src="../images/nav05a.gif" width="83" border="0"></A></td>
										<td width="180"><IMG height="20" src="../images/navgrad02.gif" width="59"></td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td width="100%" background="../images/botbevelbg.gif" height="6"><IMG height="6" src="../images/botbevel.gif" width="760"></td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<table height="14" cellSpacing="0" cellPadding="0" width="580" border="0">
			<tr>
				<td vAlign="top" align="left" height="14"><IMG height="14" src="../images/subnavtop.gif" width="180"></td>
			</tr>
		</table>
		<tr>
			<td valign="top" align="left" height="100%">
				<table height="100%" cellSpacing="0" cellPadding="0" width="760" border="0">
					<tr>
						<td vAlign="top" align="left" width="156" background="../images/subnavbg.gif" height="100%">
							<table cellSpacing="0" cellPadding="0" width="156" border="0">
								<tr>
									<td><IMG height="22" src="../images/subnav00.gif" width="156"></td>
								</tr>
								<tr>
									<td vAlign="top" background="../Images/Subnav.gif" height="220">
										<p>&nbsp;</p>
										<table width="81%" border="0" valign="top">
											<tr>
												<td width="8%">&nbsp;</td>
												<td width="13%">&nbsp;</td>
												<td width="79%">&nbsp;</td>
											</tr>
											<tr>
												<td width="8%">&nbsp;</td>
												<td width="13%">&nbsp;</td>
												<td width="79%">&nbsp;</td>
											</tr>
											<tr>
												<td width="8%">&nbsp;
												</td>
												<td width="13%"><IMG height="12" src="../Images/SmallRedArrow.gif" width="10"></td>
												<td width="79%"><A href="RebillDetail.aspx">Rebill Detail</A></td>
											</tr>
											<tr>
												<td width="8%">&nbsp;</td>
												<td width="13%"><IMG height="12" src="../Images/SmallRedArrow.gif" width="10"></td>
												<td width="79%"><A href="ReconcileRebilling.aspx">Reconcile Rebilling</A></td>
											</tr>
											<tr>
												<td width="8%">&nbsp;</td>
												<td width="13%"><IMG height="12" src="../Images/SmallRedArrow.gif" width="10"></td>
												<td width="79%"><A href="RebillingInvoices.aspx">Generate Invoices</A></td>
											</tr>
											<tr>
												<td width="8%">&nbsp;</td>
												<td width="13%"><IMG height="12" src="../Images/SmallRedArrow.gif" width="10"></td>
												<td width="79%"><A href="ApproveRebillData.aspx">Approve Rebill Data</A></td>
											</tr>
											<tr>
												<td width="8%">&nbsp;</td>
												<td width="13%">&nbsp;</td>
												<td width="79%">&nbsp;</td>
											</tr>
											<tr>
												<td width="8%">&nbsp;</td>
												<td width="13%"><IMG height="12" src="../Images/SmallRedArrow.gif" width="10"></td>
												<td width="79%"><A href="../Logout.aspx">Logout</A></td>
											</tr>
										</table>
										<!-- #BeginEditable "LeftNav" --> <!-- #EndEditable --></td>
								</tr>
								<tr>
									<td> <!-- #BeginEditable "Additional%20Left%20Nav" --> <!-- #EndEditable -->
										<p>&nbsp;</p>
										<p>&nbsp;</p>
										<p align="center"><IMG height="23" src="../images/g1440.gif" width="76"></p>
									</td>
								</tr>
							</table>
						</td>
						<td vAlign="top" align="left" height="100%">
							<table cellSpacing="0" cellPadding="0" width="580" border="0">
								<tr>
									<td><IMG height="1" src="../images/spacer.gif" width="12"></td>
									<td><IMG height="56" src="../Images/moduletoprebilling.gif" width="580"></td>
								</tr>
								<tr>
									<td></td>
									<td>
										<table height="100%" cellSpacing="0" cellPadding="0" width="580" border="0">
											<tr>
												<td vAlign="top" align="left" width="2" background="../images/leftborder.gif"><IMG height="10" src="../images/leftborder.gif" width="2"></td>
												<td align="center" width="547" bgColor="#ffffff"><table cellSpacing="0" cellPadding="8" width="90%" border="0">
														<tr>
															<td> <!-- #BeginEditable "MainBody" -->
																<form id="Form1" method="post" encType="multipart/form-data" runat="server">
																	<table>
																		<tr>
																			<td><asp:hyperlink id="hlBack" runat="server">Back to Rebill Edit</asp:hyperlink></td>
																		</tr>
																		<tr>
																			<td><asp:label id="Label4" runat="server">Add New Document</asp:label></td>
																		</tr>
																		<tr>
																			<td><asp:label id="Label2" runat="server">Title</asp:label></td>
																			<td><asp:textbox id="tbTitle" runat="server" Width="387px"></asp:textbox></td>
																		</tr>
																		<tr>
																			<td><asp:label id="Label3" runat="server">Document File</asp:label></td>
																			<td><INPUT style="WIDTH: 385px; HEIGHT: 22px" type="file" size="45" name="Filename"></td>
																		</tr>
																		<tr>
																			<td><asp:button id="bnAdd" runat="server" Text="Add Document" onclick="bnAdd_Click"></asp:button>&nbsp;</td>
																			<td><asp:label id="lbMessage" runat="server" Width="335px" ForeColor="Red"></asp:label></td>
																		</tr>
																		<tr>
																			<HR width="100%" SIZE="1">
																		</tr>
																		<tr>
																			<td colSpan="2"><asp:label id="lbNoRecords" runat="server">No Documents Attached</asp:label><asp:datagrid id="DataGrid1" runat="server" Width="517px" AutoGenerateColumns="False" GridLines="Horizontal"
																					CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
																					<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
																					<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
																					<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
																					<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
																					<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
																					<Columns>
																						<asp:BoundColumn Visible="False" DataField="ID" ReadOnly="True"></asp:BoundColumn>
																						<asp:TemplateColumn HeaderText="Title">
																							<ItemTemplate>
																								<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Title") %>' ID="Label1">
																								</asp:Label>
																							</ItemTemplate>
																							<EditItemTemplate>
																								<asp:TextBox ID="edit_Title" runat="server" Columns="10" Text='<%# DataBinder.Eval(Container, "DataItem.Title") %>'>
																								</asp:TextBox>
																							</EditItemTemplate>
																						</asp:TemplateColumn>
																						<asp:EditCommandColumn ButtonType="LinkButton" UpdateText="Update" CancelText="Cancel" EditText="Edit"></asp:EditCommandColumn>
																						<asp:TemplateColumn>
																							<ItemTemplate>
																								<asp:LinkButton CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ID") %>' CommandName="Delete" Text="Delete" ID="btnDel" Runat="server" />
																							</ItemTemplate>
																						</asp:TemplateColumn>
																						<asp:TemplateColumn>
																							<ItemTemplate>
																								<asp:HyperLink NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Path") %>' runat="server" ID="hlView" Target=_blank>View</asp:HyperLink>
																							</ItemTemplate>
																						</asp:TemplateColumn>
																					</Columns>
																					<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
																				</asp:datagrid></td>
																		</tr>
																	</table>
																</form>
																<!-- #EndEditable --></td>
														</tr>
													</table>
													<p>&nbsp;</p>
												</td>
												<td vAlign="top" align="right" width="31" background="../images/rightborder.gif"><IMG height="38" src="../images/modulecorner.gif" width="31"></td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td></td>
									<td><IMG height="20" src="../images/modulebottom.gif" width="580"></td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
	</body>
</HTML>
