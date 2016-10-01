﻿<%@ Page Language="C#" MasterPageFile="~/Reports/Reports.Master" AutoEventWireup="true" CodeBehind="PayrollDetailReport.aspx.cs" Inherits="InterrailPPRS.Reports.PayrollDetailReport" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

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
            <%
            if ( iOpen > 0 ) {

                %>
                    You must <a href="../Payroll/calc.aspx">calculate</a> payroll for the following facilities and work dates before running reports.
                <%
                                    
                %>
                <p />
                <form runat="server">
                    <asp:GridView ID="grdOpenFacilities" runat="server" AllowPaging="true" PageSize="15" OnPageIndexChanging="grdOpenFacilities_PageIndexChanging" 
                        AlternatingRowStyle-BackColor="#DEDFFF" AutoGenerateColumns="false" EmptyDataText="No results found!">
                        <PagerSettings Position="Bottom"/>
                        <Columns>
                            <asp:BoundField DataField="FacilityName" HeaderText="Facility Name" HeaderStyle-Width="200" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="WorkDate" HeaderText="Work Date" HeaderStyle-Width="150" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:d}" />
                        </Columns>
                    </asp:GridView>
                </form>                              

                <%

            }else{
                if (RType == "All" || RType == "Perm"){
                    try
                    {
                        ShowDetailReport();
                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.ToString());
                    }
                }
                            
                if (RType == "All" || RType == "Temp" ){
                    try
                    {
                        ShowTempDetailReport();
                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.ToString());
                    }
                }
            }
            %>
                <form name="form1" method="post" action="../Reports/PayrollDetailReport.aspx?PrintPreview=0&Type=<%=RType%>">
                    <input type="hidden" name="summary"          value='<% if (isSummary) { Response.Write ("on"); }   %>'    />
                    <input type="hidden" name="selFacilities"    value='<%= sselFacilities %>'    />
                    <input type="hidden" name="fromDateDetail"   value='<%= sfromDateDetail %>'   />
                    <input type="hidden" name="toDateDetail"     value='<%= stoDateDetail %>'     />
                </form>
                <!-- #EndEditable -->
            </td>
        </tr>
    </table>
</asp:Content>