<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="FacilityWeeklyCPU2.aspx.cs" Inherits="InterrailPPRS.Reports.FacilityWeeklyCPU2" %>

<html> <!-- #BeginTemplate "/Templates/Reports.dwt" -->

<title>PPRS</title>
<meta http-equiv=Content-Type content="text/html; charset=windows-1252">
<link rel="stylesheet" href="../Styles/styles.css" type="text/css" />
<link rel="stylesheet" href="WeeklyCPU2.css" type="text/css">
<script language="JavaScript">
<!--
    function MM_reloadPage(init) {  //reloads the window if Nav4 resized
        if (init == true) with (navigator) {
            if ((appName == "Netscape") && (parseIt(appVersion) == 4)) {
                document.MM_pgW = innerWidth; document.MM_pgH = innerHeight; onresize = MM_reloadPage;
            } 
        }
        else if (innerWidth != document.MM_pgW || innerHeight != document.MM_pgH) location.reload();
    }
    MM_reloadPage(true);
// -->
</script>
<!-- #EndEditable -->

<STYLE TYPE="text/css">
     P.breakhere {page-break-before: always}
     DIV.breakauto {page-break-before: auto}
</STYLE>

</head>

<body bgcolor="#FFFFFF" text="#000000" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>



<table x:str border=0 cellpadding=0 cellspacing=0 width=3730 style='border-collapse:
 collapse;table-layout:fixed;width:1200pt'>
 <tr height=27 style='height:35.25pt'>
  <td colspan=9 class=xl25 align=left >
          <table border=0 width=950>
                <tr>
                        <td align=center class=xl2222222>
                                 <b>Weekly CPU Report - By Task<br>Year of <%=selYear%><br><%=Session["FacilityName"]%></b>
                        </td>
                </tr>
          </table>
  </td>
 </tr>
</table>

<table x:str border=0 cellpadding=0 cellspacing=0 width=3730 style='border-collapse:
 collapse;table-layout:fixed;width:1900pt'>
 <col width=79 style='mso-width-source:userset;mso-width-alt:2889;width:40pt'>
 <col width=111 style='mso-width-source:userset;mso-width-alt:4059;width:45pt'>
 <col width=97 style='mso-width-source:userset;mso-width-alt:3547;width:48pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=110 style='mso-width-source:userset;mso-width-alt:4022;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=64 span=2 style='width:48pt'>
 <tr height=17 style='height:12.75pt'>
  <td height=17 colspan=43 style='height:12.75pt;mso-ignore:colspan'></td>
 </tr>

<DIV CLASS="breakauto">
<!-- start average line -->
<% CreatePage2Data(); %>
<% WritePage2Ave(); %>
<!-- end average line -->

<!-- Start Title  Row -->
<% WritePage2TitleRow(); %>
<!-- End Title  Row -->

<!-- Start Page Data -->
<% WritePage2Data(); %>

<tr height=17 style='height:6.75pt'>
    <td height=17 colspan=43 style='height:6.75pt;mso-ignore:colspan'></td>
</tr>

<!-- Start Page Totals -->
<% WritePage2Totals(); %>

</table>
</DIV>
<br>
<br>
<br>

<p class="breakhere">
<!-- Write 2nd set of pages if more than 8 categories -->
<table x:str border=0 cellpadding=0 cellspacing=0 width=3730 style='border-collapse:
 collapse;table-layout:fixed;width:1200pt'>
 <tr height=27 style='height:35.25pt'>
  <td colspan=9 class=xl25 align=left >
          <table border=0 width=950>
                <tr>
                        <td align=center class=xl2222222>
                                 <b>Weekly CPU Report - By Task<br>Year of <%=selYear%><br><%=Session["FacilityName"]%></b>
                        </td>
                </tr>
          </table>
  </td>
 </tr>
</table>

<table x:str border=0 cellpadding=0 cellspacing=0 width=3730 style='border-collapse:
 collapse;table-layout:fixed;width:1900pt'>
 <col width=79 style='mso-width-source:userset;mso-width-alt:2889;width:40pt'>
 <col width=111 style='mso-width-source:userset;mso-width-alt:4059;width:45pt'>
 <col width=97 style='mso-width-source:userset;mso-width-alt:3547;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=110 style='mso-width-source:userset;mso-width-alt:4022;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=101 style='mso-width-source:userset;mso-width-alt:3693;width:41pt'>
 <col width=73 style='mso-width-source:userset;mso-width-alt:2669;width:41pt'>
 <col width=64 span=2 style='width:48pt'>
 <tr height=17 style='height:12.75pt'>
  <td height=17 colspan=43 style='height:12.75pt;mso-ignore:colspan'></td>
 </tr>

<% WritePage3Ave();%>
<!-- end average line -->

<!-- Start Title  Row -->
<% WritePage3TitleRow();%>
<!-- End Title  Row -->

<!-- Start Page Data -->
<% WritePage3Data();%>

<tr height=17 style='height:12.75pt'>
  <td height=17 colspan=43 style='height:12.75pt;mso-ignore:colspan'></td>
 </tr>

<!-- Start Page Totals -->
<% WritePage3Totals();%>

</table>
</p>

</body>
</html>