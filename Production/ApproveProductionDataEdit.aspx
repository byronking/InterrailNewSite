<%@ Page Language="C#" MasterPageFile="~/Production/Production.Master" AutoEventWireup="true" CodeBehind="ApproveProductionDataEdit.aspx.cs" Inherits="InterrailPPRS.Production.ApproveProductionDataEdit" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script  type="text/javascript">
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

    function goApprove() {
        frm = document.form1;

        if (!confirm("Approved data cannot be modified.\nPress OK to Approve.")) {
            return false;
        }

        if ('<%=Trim(UCase(cStr(Session["UserType"])))%>' == 'USER') {
            frm.action = 'ApproveProductionDataEdit.aspx?Approval=FACILITY&ID=<%=Request["ID"]%>';
        }
        else {
            frm.action = 'ApproveProductionDataEdit.aspx?Approval=CORPORATE&ID=<%=Request["ID"]%>';
        }
        frm.submit();
    }

    function goOpen() {
        frm = document.form1;

        if (!confirm("Are you sure you want to set the status to OPEN?\nPress OK to set approval status to OPEN.")) {
            return false;
        }
        else {
            frm.action = 'ApproveProductionDataEdit.aspx?Approval=OPEN&ID=<%=Request["ID"]%>';
        }
        frm.submit();
    }

    function goBack() {
        frm = document.form1;
        //frm.action = "ApproveProductionData.aspx";
        //frm.submit();
        history.go(-1);
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
                              <table align="center" width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr> 
                                  <td colspan="4" class="pageTitle" align="center">Production Data</td>
                                </tr>
                                <tr> 
                                  <td colspan="4"> 
                                    <div align="center" class="pageTitle"><%=Session["FacilityName"]%></div>
                                  </td>
                                </tr>
                                <tr> 
                                  <td width="20%">&nbsp;</td><td width="30%">&nbsp;</td><td width="20%">&nbsp;</td><td width="30%">&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td width="20%">&nbsp;</td>
                                  <td align="right" width="30%">&nbsp;</td>
                                  <td align="right"  width="20%" class='lblColor'>Approval Status:&nbsp;&nbsp;</td>
                                  <td width="30%"> 
                                    <div  class="required"><B><%=sStatus%></B></div>
                                  </td>
                                </tr>
                                <tr> 
                                  <td width="20%">&nbsp;</td><td width="30%">&nbsp;</td><td width="20%">&nbsp;</td><td width="30%">&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td width="20%" valign="top" align="right" class='lblColor'> 
                                    Date:&nbsp;&nbsp;</td>
                                  <td width="30%"><%= rs.Item("WorkDate")%></td>
                                  <td width="20%" align="right" class='lblColor'>New/Used:&nbsp;&nbsp;</td>
                                  <td width="30%"><%=rs.Item("NewUsed")%></td>
                                </tr>
                                <tr> 
                                  <td width="20%" valign="top" align="right" class='lblColor'>Shift:&nbsp;&nbsp;</td>
                                  <td width="30%"><%= rs.Item("ShiftID")%></td>
                                  <td width="20%" align="right" class='lblColor'>Rail Car :&nbsp;&nbsp;</td>
                                  <td width="30%"><%=rs.Item("CarTypeDescription")%>&nbsp;-&nbsp;<%= rs.Item("RailCarNumber")%></td>
                                </tr>
                                <tr> 
                                  <td width="20%" valign="top" align="right" class='lblColor'>Customer:&nbsp;&nbsp;</td>
                                  <td width="30%"><%=rs.Item("CustomerName")%>&nbsp;(<%=rs.Item("CustomerCode")%>)</td>
                                  <td width="20%"align="right" class='lblColor'>Level:&nbsp;&nbsp;</td>
                                  <td width="30%"><%=rs.Item("LevelType")%></td>
                                </tr>
                                <tr> 
                                  <td width="20%" valign="top" align="right" class='lblColor'>Task:&nbsp;&nbsp;</td>
                                  <td width="30%"><%=rs.Item("TaskDescription")%>&nbsp;(<%=rs.Item("TaskCode")%>)</td>
                                  <td width="20%" align="right" class='lblColor'>Origin:&nbsp;&nbsp;</td>
                                  <td width="30%"><%=rs.Item("OriginName")%>&nbsp;(<%=rs.Item("OriginCode")%>)</td>
                                </tr>
                                <tr> 
                                  <td width="20%" valign="top" align="right" class='lblColor'>Units:&nbsp;&nbsp;</td>
                                  <td width="30%"><%= rs.Item("Units")%></td>
                                  <td width="20%" align="right" class='lblColor'>Manuf.:&nbsp;&nbsp;</td>
                                  <td width="30%"><%=rs.Item("ManufacturerName")%>&nbsp;(<%=rs.Item("ManufacturerCode")%>)</td>
                                </tr>
                                <tr> 
                                  <td width="20%">&nbsp;</td><td width="30%">&nbsp;</td><td width="20%">&nbsp;</td><td width="30%">&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td width="20%" valign="top" align="right" class='lblColor'>Notes:&nbsp;&nbsp;</td>
                                  <td colspan="3"> 
                                    <textarea rows="5" cols="45" readonly name="textarea"><%= rs.Item("Notes")%></textarea>
                                  </td>
                                </tr>
                                <tr> 
                                  <td width="20%">&nbsp;</td><td width="30%">&nbsp;</td><td width="20%">&nbsp;</td><td width="30%">&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td width="20%" align="right" class='lblColor'>Last Modified:&nbsp;&nbsp;</td>
                                  <td colspan="2"><%=rs.Item("LastModifiedOn")%></td>
                                  <td width="30%">&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td width="20%" align="right" class='lblColor'>By:&nbsp;&nbsp;</td>
                                  <td colspan="2"><%= rs.Item("LastModifiedBy") %></td>
                                  <td width="30%">&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td width="20%">&nbsp;</td><td width="30%">&nbsp;</td><td width="20%">&nbsp;</td><td width="30%">&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td width="20%">&nbsp;</td>

								  <td>&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td width="20%">&nbsp;</td>
                                  <td width="30%">&nbsp;</td>
                                  <td width="20%">&nbsp;</td>
                                  <td width="30%">&nbsp;</td>
                                </tr>
                                <tr> 
                                  <td width="20%">&nbsp;</td>
                                  <td colspan="3" align="center"> 
								 
                                    <input type="button" name="btnBack" value="Back" onClick="goBack();" />
                                  </td>
                                </tr>
                              </table>
                            </form>
                            <!-- #EndEditable -->
                          </td>
                        </tr>

                    </table>
</asp:Content>