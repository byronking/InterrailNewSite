<%@ Page Language="C#" MasterPageFile="~/Rebilling/Rebilling.Master" AutoEventWireup="true" CodeBehind="RebillEntryDetail.aspx.cs" Inherits="InterrailPPRS.Rebilling.RebillEntryDetail" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script language="JavaScript">
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

    function checkAll(ck) {
        for (var i = 1; i <= document.frm.MaxRDOCount.value; i++)
            document.frm["rbRebill_" + i].checked = eval(ck);
        document.frm.rdoALL.checked = eval(ck);
    }

    function goSave() {

        document.frm.action = "RebillEntryDetail.aspx";
        //document.frm.action = "../col.aspx";
        document.frm.submit();

    }
    function goCancel() {

        document.frm.action = "RebillEntryDetail.aspx";
        document.frm.Mode.value = "Cancel";
        //document.frm.action = "../col.aspx";
        document.frm.submit();

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
            <form name='frm' method='post' action='' ID="frm">
            <table align="center"  border="0" cellspacing="0" cellpadding="0" width="100%" >
                <tr>
                <td align="default" colspan="8">
                    <div align="center" class="pageTitle"><b>Rebill Entry</b></div>
                </td>
                </tr>
                <tr>
                <td align="default" width="12%">&nbsp;</td>
                <td align="default" colspan="7">&nbsp;</td>
                </tr>
                <tr>
                <td align="default" width="12%">&nbsp;</td>
                <td align="default" colspan="7">&nbsp;</td>
                </tr>
                <tr>
                <td align="default" width="12%">&nbsp;</td>
                <td align="default" colspan="7">&nbsp;</td>
                </tr>
                <tr>
                <td align="default" class="cellTopBorder"  >&nbsp;</td>

                <td align="right"   class="cellTopBorder"ColSpan=7  >Check/Uncheck All <input type="checkbox" name="rdoALL" id="rdoALL" onclick="checkAll(this.checked)" /></td>
                </tr>
                <tr>
                <td align="default" width="12%" class="cellTopBottomBorder">
                    Work Date </td>
                <td align="default" width="8%" class="cellTopBottomBorder">Approval</td>
                <td align="left" width="12%" class="cellTopBottomBorder">Currently<br />Rebilled</td>
                <td align="left" width="21%" class="cellTopBottomBorder">
                    SubTask (Task)</td>
                <td align="left" width="21%" class="cellTopBottomBorder">Customer</td>

                <td align="left" width="8%" class="cellTopBottomBorder">Att.</td>
                <td align="right" width="8%" class="cellTopBottomBorder">Hours</td>
                <td align="right" width="8%" class="cellTopBottomBorder">&nbsp;</td>
                </tr>
                <%string rowColor;

                int Repeat1__index = 1;
                              
                while (! rs.EOF){
                rs.Read();
                if (Repeat1__index % 2 == 0) {
                    rowColor = "reportEvenLine";
                }else{
                    rowColor = "reportOddLine";
                }
                %>

                <tr class="<%=rowColor%>">
                <td align="default" width="12%"><%=(rs.Item("WorkDate"))%></td>
                <td align="default" width="8%"><%=(rs.Item("RebillStatus"))%></td>
                <td align="left" width="12%"><% if (rs.Item("Rebilled") == "True" ) {%><img src='../images/check.gif' /> <%}else{%>&nbsp; <%}%></td>
                <td align="left" width="21%"><%=(rs.Item("Description"))%> (<%=(rs.Item("TaskCode"))%>)</td>
                <td align="left" width="21%"><%=(rs.Item("CustomerName"))%> (<%=(rs.Item("CustomerCode"))%>)</td>

                    <%
                        rsAttachments = new DataReader("SELECT path, title FROM RebillAttachments WHERE RebillDetailId = "  + cStr(rs.Item("ID")) );
					    string strAttList = "";
					    int attcount = 0;
						    while(! rsAttachments.EOF){
                                rsAttachments.Read();  
							    attcount = attcount + 1;

							    if (attcount > 1){
							    strAttList = strAttList + "<BR>";
							    }
							    strAttList = strAttList + "<a href='../Rebilling/" + cStr(rsAttachments.Item("Path")) + "' target=_blank>" + cStr(rs.Item("ID")) + "</a>";
                            }
                                          
					    Response.Write( "    <td align='Left'   width='8%' >"  + strAttList + "</td>");

                        %>

                <td align="right" width="8%"><%=(rs.Item("TotalHours"))%> </td>
                <td align="right" width="8%">
                <input type="hidden" id="rbID_<%=Repeat1__index%>" name="rbID_<%=Repeat1__index%>" value="<%=(rs.Item("ID"))%>" />
                <input type="checkbox" id="rbRebill_<%=Repeat1__index%>" name="rbRebill_<%=Repeat1__index%>" <%if (rs.Item("Rebilled") == "True") { response.Write("CHECKED"); }%> />
                </td>
                </tr>
                <%
                    Repeat1__index = Repeat1__index+1;
                    Repeat1__numRows = Repeat1__numRows-1;
                    }
                %>
            </table>
            <table border="0" width="50%" align="center">
                <tr>
                <td width="23%" align="center">&nbsp;
                <input type="hidden" name="MaxRDOCount" id="MaxRDOCOunt" value="<%=Repeat1__index - 1%>" />
                <input type="hidden" name="Mode" id="Mode" value="Save" />
                </td>
                <td width="31%" align="center">&nbsp;
                <% if ( Repeat1__index > 1){ %>
                    <input type="button" name="btnDetail" value="Save" onClick="goSave();" ID="btnDetail">
                <% } %>
                    </td>
                <td width="23%" align="center">&nbsp;<input type="button" name="btnCancel" value="Cancel" onClick="goCancel();" ID="btnCancel"></td>
                <td width="23%" align="center">&nbsp;</td>
                </tr>
            </table>
                </form>
            <!-- #EndEditable -->
            </td>
        </tr>

	    </table>

</asp:Content>