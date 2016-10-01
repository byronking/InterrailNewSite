<%@ Page language="c#" Codebehind="AdHocReport.aspx.cs" AutoEventWireup="True" Inherits="InterrailPPRS.Reports.WebForm2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Report</title>
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
<link rel="stylesheet" href="../styles.css" type="text/css">
</HEAD>
	<body MS_POSITIONING="GridLayout">
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td bgcolor="#9999cc">
      <table width="760" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td class="company" width="272" bgcolor="#FFFFFF">
            <div align="center"></div>
          </td>
          <td width="488"><img src="../images/train.jpg" width="488" height="68"></td>
        </tr>
      </table>
    </td>
  </tr>
  <tr>
    <td>
      <table width="100%" height="32" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td width="100%" height="6" background="../images/topbevelbg.gif" bgcolor="#9999CC" ><img src="../images/topbevel.gif" width="760" height="6"></td>
        </tr>
        <tr>
          <td bgcolor="#FF3300">
            <table width="760" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="200" class="date" bgcolor="#FF3300" background="../images/dateblock.gif">
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
                <td width="45"><img src="../images/navgrad01.gif" width="45" height="20"></td>
                <td width="115"><a href="../Admin/Default.aspx"><img src="../images/nav01.gif" width="115" height="20" border="0"></a></td>
                <td width="91"><a href="../Production/Default.aspx"><img src="../images/nav02.gif" width="91" height="20" border="0"></a></td>
                <td width="68"><a href="../Payroll/Default.aspx"><img src="../images/nav03.gif" width="68" height="20" border="0"></a></td>
                <td width="83"><a href="../Rebilling/Default.aspx"><img src="../images/nav04a.gif" width="83" height="20" border="0"></a></td>
                <td width="83"><a href="Default.aspx"><img src="../images/nav05a.gif"  alt="Reports"  width="83" height="20" border="0"></a></td>
                <td width="180"><img src="../images/navgrad02.gif" width="59" height="20"></td>
              </tr>
            </table>
          </td>
        </tr>
        <tr>
          <td width="100%" height="6" background="../images/botbevelbg.gif" ><img src="../images/botbevel.gif" width="760" height="6"></td>
        </tr>
      </table>
    </td>
  </tr>
</table>
	
		<form id="WebForm2" method="post" runat="server">
		<asp:button id="btnShowAll"  runat="server" Text="Show All" onclick="btnShowAll_Click"></asp:button><br />
			<asp:datagrid id="dgMain"  runat="server" HorizontalAlign="Justify" PageSize="20" ShowFooter="True" AllowPaging="True" Height="227px" Width="841px" AllowSorting="True" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="4" GridLines="Vertical" ForeColor="Black" AutoGenerateColumns="False" Font-Size="Smaller">
				<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#CE5D5A"></SelectedItemStyle>
				<AlternatingItemStyle BackColor="White"></AlternatingItemStyle>
				<ItemStyle BackColor="#DEDFFF"></ItemStyle>
				<HeaderStyle Font-Bold="True" ForeColor="White" BorderStyle="None" BackColor="#6B696B"></HeaderStyle>
				<FooterStyle BackColor="#CCCC99"></FooterStyle>
				<PagerStyle HorizontalAlign="Left" ForeColor="Black" Position="TopAndBottom" BackColor="#F7F7DE" Mode="NumericPages"></PagerStyle>
			</asp:datagrid></form>
</body>
</HTML>
