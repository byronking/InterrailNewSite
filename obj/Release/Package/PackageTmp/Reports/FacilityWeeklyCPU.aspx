<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FacilityWeeklyCPU.aspx.cs" Inherits="InterrailPPRS.Reports.FacilityWeeklyCPU" %>

<html><!-- #BeginTemplate "/Templates/Reports.dwt" -->
<head>
<title>PPRS</title>
<meta http-equiv=Content-Type content="text/html; charset=windows-1252">
<link rel="stylesheet" href="../Styles/styles.css" type="text/css" />
<link rel="stylesheet" href="WeeklyCPU.css" type="text/css">
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
// -->
</script>
<!-- #EndEditable -->
</head>

<body bgcolor="#FFFFFF" text="#000000" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">


<script type="C#" runat="server">
    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);        
    }        
</script>

<%--    <form id="Form1" runat="server">
        <div style="margin-left:40px;">
            <h3 style="font-family:Arial;margin-top:20px;">Weekly CPU Report</h3>
                <h4 style="font-family:Arial;">Year: <%= selYear %></h4><p/>
                <h4 style="font-family:Arial;">Facility: <%= selYear %></h4><p/>
                <asp:GridView ID="grdWeeklyCpuSummary" runat="server" AlternatingRowStyle-BackColor="AliceBlue" GridLines="Both" CellPadding="5"
                    RowStyle-Font-Size="12px" RowStyle-Font-Names="Arial" HeaderStyle-Font-Names="Arial" AutoGenerateColumns="false" RowStyle-Wrap="true">
                    <Columns>
                        <asp:BoundField HeaderText="Week Of"  DataField="WeekOf" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText ="Total Pay" DataField="TotalPay" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataFormatString="${0:0.00}" />
                        <asp:BoundField HeaderText ="Total Units" DataField="TotalUnits" ItemStyle-Width="105px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText ="Cost / Unit" DataField="TotalCostPerUnit" ItemStyle-Width="105px" ItemStyle-HorizontalAlign="Center" DataFormatString="${0:0.00}" />
                        <asp:BoundField HeaderText ="Variance" DataField="Variance" ItemStyle-Width="110px" ItemStyle-HorizontalAlign="Center" DataFormatString="${0:0.00}" />
                        <asp:BoundField HeaderText ="Possible Determining Factors of Variance" DataField="PossibleFactorsForVariance" ItemStyle-Width="500px" />
                    </Columns>
                </asp:GridView>
        </div>
    </form>--%>

<table x:str border=0 cellpadding=0 cellspacing=0 width="950px" style='border-collapse:
 collapse;table-layout:fixed;width:650pt'>
 <col width=77 style='mso-width-source:userset;mso-width-alt:2816;width:47pt'>
 <col width=82 style='mso-width-source:userset;mso-width-alt:2998;width:47pt'>
 <col width=70 style='mso-width-source:userset;mso-width-alt:2560;width:53pt'>
 <col width=72 style='mso-width-source:userset;mso-width-alt:2633;width:35pt'>
 <col width=71 style='mso-width-source:userset;mso-width-alt:2596;width:45pt'>
 <col width=64 span=12 style='width:45pt'>
 <tr height=27 style='height:35.25pt'>
  <td height=27 class=xl24 width=77 style='height:35.25pt;width:58pt'></td>
  <td class=xl25 width=82 style='width:62pt'></td>
  <td colspan=9 class=xl2222222 align=center width=597 style='mso-ignore:colspan;
  width:295pt'><b>Weekly CPU Report<br>Year of <%=selYear%><br><%=Session["FacilityName"]%></b></td>
  <td width=64 style='width:48pt'></td>
  <td width=64 style='width:48pt'></td>
  <td width=64 style='width:48pt'></td>
  <td width=64 style='width:48pt'></td>
  <td width=64 style='width:48pt'></td>
  <td width=64 style='width:48pt'></td>
 </tr>
 <tr height=28 style='height:12.0pt'>
  <td height=28 colspan=9 class=xl25 style='height:12.0pt;mso-ignore:colspan'></td>
  <td colspan=2 class=xl26 style='mso-ignore:colspan'></td>
  <td colspan=6 style='mso-ignore:colspan'></td>
 </tr>

 <tr height=17 style='height:12.75pt'>
  <td colspan=2 height=17 class=xl69 style='height:12.75pt'>Budgeted CPU</td>
  <td class=xl66 x:num="0"><a name=BudgetedCPU><%=FCur(cStr(fBudgetedCPU),2)%></a></td>
  <td></td>
  <td class=xl51 align=right x:num></td>
  <td></td>
  <td colspan=5 class=xl71>Actual Cost Per Unit YTD</td>
  <td class=xl67><%=AverageCPU%></td>
  <td colspan=6 style='mso-ignore:colspan'></td>
 </tr>
 <tr height=18 style='height:13.5pt'>
  <td height=18 colspan=6 style='height:13.5pt;mso-ignore:colspan'></td>
  <td colspan=5 class=xl73>Variance of Budgeted VS. Actual CPU YTD</td>
  <td class=xl68 align=center ><%=VarianceYTD%></td>
  <td colspan=6 style='mso-ignore:colspan'></td>
 </tr>
 <tr height=17 style='height:12.75pt'>
  <td height=17 colspan=6 style='height:12.75pt;mso-ignore:colspan'></td>
  <td colspan=4 class=xl27 style='mso-ignore:colspan'></td>
  <td class=xl28></td>
  <td colspan=6 style='mso-ignore:colspan'></td>
 </tr>
 <tr class=xl51 height=24.75 style='height:25.75pt'>
  <td height=20.75 colspan=2 class=xl60 align=center style='height:25.75pt;
  mso-ignore:colspan;border-right:.5pt solid black'>Week of</td>
  <td class=xl2222223 style='border-left:none'>Total Pay</td>
  <td class=xl62 colspan="2" style='border-left:none'>Total Units</td>
  <td class=xl2222223 colspan="2" style='border-left:none'>Cost / Unit</td>
  <td class=xl62 style='border-left:none'>Variance</td>
  <td colspan=10 class="xl2222223" align=center style='mso-ignore:colspan;border-right:
  .5pt solid black' width="600px">Possible Determining Factors of Variance</td>
  <td colspan=5 class=xl51 style='mso-ignore:colspan'></td>
 </tr>


<% WritePageOneLines();%>

 <tr height=17 style='height:12.75pt'>
  <td height=17 colspan=2 style='height:12.75pt;mso-ignore:colspan'></td>
  <td><span style="mso-spacerun: yes"> </span></td>
  <td x:str="            "><span style="mso-spacerun: yes">            </span></td>
  <td colspan=2 style='mso-ignore:colspan'></td>
  <td><span style="mso-spacerun: yes"> </span></td>
  <td colspan=10 style='mso-ignore:colspan'></td>
 </tr>
 <tr height=17 style='height:12.75pt'>
  <td height=17 colspan=6 style='height:12.75pt;mso-ignore:colspan'></td>
  <td><span style="mso-spacerun: yes"> </span></td>
  <td colspan=10 style='mso-ignore:colspan'></td>
 </tr>
</table>
<!-- End Page One -->
</body>
<!-- #EndTemplate -->

</html>