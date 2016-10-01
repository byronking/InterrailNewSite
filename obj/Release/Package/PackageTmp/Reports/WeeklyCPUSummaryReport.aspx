<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="WeeklyCPUSummaryReport.aspx.cs" Inherits="InterrailPPRS.Reports.WeeklyCPUSummaryReport" %>



<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>


<html>
<title>Inter-Rail Transport</title>
<meta http-equiv=Content-Type content="text/html; charset=windows-1252">
<head title="Weekly CPU Summary Report">
</head>
    <body bgcolor="#FFFFFF" text="#000000" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
        <form runat="server">
            <div style="margin-left:30px;">
                <h4 style="font-family:Arial;margin-top:10px;">Weekly CPU Summary Report</h4>
                <h5 style="font-family:Arial;margin-top:-15px;">Week ending Date: <%= selDateWeeklySummary.ToShortDateString() %></h5>
                <asp:GridView ID="grdWeeklyCpuSummary" runat="server" AlternatingRowStyle-BackColor="AliceBlue" GridLines="Both" CellPadding="5" ShowFooter="true" FooterStyle-Font-Names="Arial" FooterStyle-Font-Bold="true" FooterStyle-Font-Size="11px"
                    FooterStyle-HorizontalAlign="Center" RowStyle-Font-Size="11px" RowStyle-Font-Names="Arial" HeaderStyle-Font-Names="Arial" AutoGenerateColumns="false" RowStyle-Wrap="true"
					HeaderStyle-Font-Size="11px" OnRowDataBound="grdWeeklyCpuSummary_OnRowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Facility" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblFacility" runat="server" Text='<%#Eval("Facility") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblWeeklyTotals" runat="server" Text="Weekly Totals" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Pay" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblTotalPay" runat="server" Text='<%#Eval("TotalPay", "{0:C}") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblTotalPayValue" runat="server" />
                            </FooterTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Total Units" ItemStyle-Width="75px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblTotalUnits" runat="server" Text='<%#Eval("TotalUnits") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblTotalUnitsValue" runat="server" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cost / Unit" ItemStyle-Width="75px" ItemStyle-HorizontalAlign="Center" >
                            <ItemTemplate>
                                <asp:Label ID="lblCostPerUnit" runat="server" Text='<%#Eval("TotalCostPerUnit", "{0:C}") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblCostPerUnitValue" runat="server" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Variance" DataField="Variance" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:C}" />
                        <asp:BoundField HeaderText="Possible Determining Factors of Variance" DataField="PossibleFactorsForVariance" ItemStyle-Font-Size="12px" ItemStyle-Width="475" />
                    </Columns>
                </asp:GridView>
            </div>
        </form>
    </body>
</html>