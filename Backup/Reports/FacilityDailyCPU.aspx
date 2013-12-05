<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="FacilityDailyCPU.aspx.cs" Inherits="InterrailPPRS.Reports.FacilityDailyCPU" %>

<html>
<!-- #BeginTemplate "/Templates/admin.dwt" -->
<head>
<title>PPRS</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<link rel="stylesheet" href="../Styles/styles.css" type="text/css" />
<link rel="stylesheet" href="DailyCPU.css" type="text/css">
<script type="text/javascript" language="JavaScript" src="../Scripts/Validation.js"></script>

<script type="text/javascript" language="JavaScript">
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

<STYLE TYPE="text/css">
     DIV.breakhere {page-break-before: always}
     DIV.breakauto {page-break-before: auto}
</STYLE>


</head>
<script type="C#" runat="server">

    protected void OnLoad(System.EventArgs e ){
        base.OnLoad(e);
        
    }
        
</script>

<body bgcolor="#FFFFFF" text="#000000" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">


<table x:str border=0 cellpadding=0 cellspacing=0 width=930 style='border-collapse:
 collapse;table-layout:fixed;width:870pt'>
 <col width=40 style='mso-width-source:userset;mso-width-alt:2742;width:34pt'>
 <col width=67 span=2 style='mso-width-source:userset;mso-width-alt:2304;
 width:34pt'>
 <col width=60 style='mso-width-source:userset;mso-width-alt:2194;width:34pt'>
 <col width=59 style='mso-width-source:userset;mso-width-alt:2157;width:30pt'>
 <col width=58 style='mso-width-source:userset;mso-width-alt:2121;width:30pt'>
 <col width=63 span=2 style='mso-width-source:userset;mso-width-alt:2304;
 width:34pt'>
 <col width=60 style='mso-width-source:userset;mso-width-alt:2194;width:34pt'>
 <col width=50 style='mso-width-source:userset;mso-width-alt:1828;width:34pt'>
 <col width=58 span=2 style='mso-width-source:userset;mso-width-alt:2121;
 width:34pt'>
 <col width=63 span=2 style='mso-width-source:userset;mso-width-alt:2304;
 width:34pt'>
 <col width=60 style='mso-width-source:userset;mso-width-alt:2194;width:34pt'>
 <col width=63 span=2 style='mso-width-source:userset;mso-width-alt:2304;
 width:34pt'>
 <col width=60 span=2 style='mso-width-source:userset;mso-width-alt:2194;
 width:28pt'>
 <col width=50 span=2 style='mso-width-source:userset;mso-width-alt:1828;
 width:34pt'>
 <col width=64 span=3 style='width:34pt'>
 <tr height=17 style='height:1pt'>
  <td height=31 width=40 style='height:1pt;width:34pt'></td>
  <td width=63 style='width:47pt'></td>
  <td width=63 style='width:47pt'></td>
  <td width=60 style='width:45pt'></td>
  <td width=59 style='width:44pt'></td>
  <td width=58 style='width:44pt'></td>
  <td width=63 style='width:47pt'></td>
  <td width=63 style='width:47pt'></td>
  <td width=60 style='width:45pt'></td>
  <td width=50 style='width:38pt'></td>
  <td width=58 style='width:44pt'></td>

  <td class=xl30 colspan=11 align=left ></td>
  <td  style='mso-ignore:colspan'></td>
 </tr>
 <tr >
  <td class=xl2222222 colspan=24 align=left>
          <table border=0 width=900>
                <tr>
                        <td align=center class=xl2222222>
                                 <b>Daily FMS Report<br><%=MonthName(cInt(selMonth))%>, <%=selYear%><br><%=Session["FacilityName"]%></b>
                        </td>
                </tr>
          </table>
  </td>
 </tr>
 <tr height=31 style='height:1.25pt'>
  <td height=31 class=xl64 style='height:1.25pt'></td>
  <td class=xl52></td>
  <td class=xl52></td>
  <td class=xl29 colspan=18 style='mso-ignore:colspan'></td>
  <td class=xl52>&nbsp;</td>
  <td class=xl29>&nbsp;</td>
  <td class=xl29>&nbsp;</td>
  <td class=xl29>&nbsp;</td>
  <td class=xl28>&nbsp;</td>
  <td class=xl28>&nbsp;</td>
  <td class=xl29>&nbsp;</td>
  <td class=xl29>&nbsp;</td>
  <td class=xl29>&nbsp;</td>
  <td class=xl29>&nbsp;</td>
  <td class=xl29>&nbsp;</td>
  <td class=xl28>&nbsp;</td>
  <td class=xl52>&nbsp;</td>
  <td class=xl29>&nbsp;</td>
  <td class=xl64></td>
  <td colspan=3 style='mso-ignore:colspan'></td>
 </tr>
 <tr height=18 style='mso-height-source:userset;height:13.1pt'>
  <td height=18 class=xl34 style='height:13.1pt'>&nbsp;</td>
  <td colspan=5 class=xl106>UNLOADING</td>
  <td colspan=5 class=xl109>LOADING</td>
  <td class=xl78 style='border-top:none'>TOTAL</td>
  <td colspan=3 class=xl106 style='border-left:none'>SHUTTLING</td>
  <td colspan=6 class=xl109 style='border-right:2.0pt double black'>R/C
  SPOTTING</td>
  <td colspan=3 style='mso-ignore:colspan'></td>
 </tr>
 <tr height=18 style='mso-height-source:userset;height:13.1pt'>
  <td height=18 style='height:13.1pt'></td>
  <td class=xl43>TOTAL</td>
  <td class=xl24>#</td>
  <td class=xl24>#</td>
  <td class=xl24>&nbsp;</td>
  <td class=xl35>UNITS</td>
  <td class=xl37>TOTAL</td>
  <td class=xl27>#</td>
  <td class=xl27>#</td>
  <td class=xl27>&nbsp;</td>
  <td class=xl41>UNITS</td>
  <td class=xl79>UNITS/</td>
  <td class=xl43 style='border-left:none'>TOTAL</td>
  <td class=xl24>#</td>
  <td class=xl24>&nbsp;</td>
  <td class=xl37 x:str="SPOT ">SPOT </td>
  <td class=xl27>#R/C</td>
  <td class=xl27>#R/C</td>
  <td class=xl27>DOWN</td>
  <td class=xl27>HRS</td>
  <td class=xl45 style='border-top:none'>R/C's</td>
  <td colspan=3 style='mso-ignore:colspan'></td>
 </tr>
 <tr height=18 style='mso-height-source:userset;height:13.1pt'>
  <td height=18 class=xl104 style='height:13.1pt'>DAY</td>
  <td class=xl44>HRS</td>
  <td class=xl25>EMP</td>
  <td class=xl25>UNITS</td>
  <td class=xl25>UPM</td>
  <td class=xl36>/EMP</td>
  <td class=xl38>HRS</td>
  <td class=xl26>EMP</td>
  <td class=xl26>UNITS</td>
  <td class=xl26>UPM</td>
  <td class=xl42>/EMP</td>
  <td class=xl80>EMP</td>
  <td class=xl44 style='border-left:none'>HRS</td>
  <td class=xl25>UNITS</td>
  <td class=xl25>UPM</td>
  <td class=xl38>TIME</td>
  <td class=xl26>IN</td>
  <td class=xl26>OUT</td>
  <td class=xl26>TIME</td>
  <td class=xl26>WKED</td>
  <td class=xl46>/HR.</td>
  <td colspan=3 style='mso-ignore:colspan'></td>
 </tr>

 <%

   CreatePageOneData();

   WritePageOneData();

   WritePageOneTotals();

   %>

</table>


<DIV CLASS='breakhere'>

<table x:str border=0 cellpadding=0 cellspacing=0 width=870 style='border-collapse:
 collapse;table-layout:fixed;width:870pt'>
 <col width=75 style='mso-width-source:userset;mso-width-alt:2742;width:34pt'>
 <col width=63 span=2 style='mso-width-source:userset;mso-width-alt:2304;
 width:30pt'>
 <col width=60 style='mso-width-source:userset;mso-width-alt:2194;width:27pt'>
 <col width=50 style='mso-width-source:userset;mso-width-alt:1828;width:27pt'>
 <col width=58 style='mso-width-source:userset;mso-width-alt:2121;width:27pt'>
 <col width=63 span=2 style='mso-width-source:userset;mso-width-alt:2304;
 width:27pt'>
 <col width=60 style='mso-width-source:userset;mso-width-alt:2194;width:27pt'>
 <col width=68 style='mso-width-source:userset;mso-width-alt:1828;width:31pt'>
 <col width=68 style='mso-width-source:userset;mso-width-alt:2121;width:31pt'>
 <col width=68 style='mso-width-source:userset;mso-width-alt:2304;width:29pt'>
 <col width=82 style='mso-width-source:userset;mso-width-alt:2998;width:45pt'>
 <col width=69 style='mso-width-source:userset;mso-width-alt:2523;width:40pt'>
 <col width=79 style='mso-width-source:userset;mso-width-alt:2889;width:45pt'>
 <col width=63 style='mso-width-source:userset;mso-width-alt:2304;width:29pt'>
 <col width=60 span=2 style='mso-width-source:userset;mso-width-alt:2194;
 width:32pt'>
 <col width=50 span=2 style='mso-width-source:userset;mso-width-alt:1828;
 width:25pt'>
 <col width=64 span=3 style='width:30pt'>
 <tr height=31 style='height:13.25pt'>
  <td height=31 width=75 style='height:13.25pt;width:34pt'></td>
  <td width=63 style='width:47pt'></td>
  <td width=63 style='width:47pt'></td>
  <td width=60 style='width:45pt'></td>
  <td width=50 style='width:38pt'></td>
  <td width=58 style='width:44pt'></td>
  <td width=63 style='width:47pt'></td>
  <td width=63 style='width:47pt'></td>
  <td width=60 style='width:45pt'></td>
  <td width=50 style='width:38pt'></td>
  <td width=58 style='width:44pt'></td>
 </tr>
 <tr >
  <td class=xl2222222 colspan=24 align=left>
          <table border=0 width=900>
                <tr>
                        <td align=center class=xl2222222>
                                 <b>Daily CPU Report<br><%=MonthName(cInt(selMonth))%>, <%=selYear%><br><%=Session["FacilityName"]%></b>
                        </td>
                </tr>
          </table>
  </td>
 </tr>
 <tr height=31 style='height:1.25pt'>
  <td height=31 class=xl64 style='height:1.25pt' ></td>
  <td class=xl52></td>
  <td class=xl52></td>
  <td class=xl29 colspan=10 ></td>
  <td class=xl29>&nbsp;</td>
  <td class=xl29>&nbsp;</td>
  <td class=xl29>&nbsp;</td>
  <td class=xl28>&nbsp;</td>
  <td class=xl52>&nbsp;</td>
  <td class=xl29>&nbsp;</td>
  <td class=xl64></td>
 </tr>
 <tr height=18 style='mso-height-source:userset;height:13.1pt'>
  <td height=18 class=xl34 style='height:13.1pt'>&nbsp;</td>
  <td colspan=4 class=xl106 style='border-right:2.0pt double black'>MISC. HOURS</td>
  <td colspan=6 class=xl109 style='border-right:2.0pt double black;border-left:
  none'>KPI's</td>
  <td colspan=5 class=xl106 style='border-right:2.0pt double black;border-left:
  none'>BUDGET</td>
  <td colspan=3 class=xl109 style='border-left:none'>YARD AIR</td>
  <td colspan=3 class=xl106 style='border-right:2.0pt double black'>R/C
  PREPPING</td>
 </tr>
 <tr height=18 style='mso-height-source:userset;height:13.1pt'>
  <td height=18 style='height:13.1pt'></td>
  <td class=xl43>&nbsp;</td>
  <td colspan=2 class=xl24 style='mso-ignore:colspan'>&nbsp;</td>
  <td class=xl24>NON-</td>
  <td class=xl57 style='border-top:none'>&nbsp;</td>
  <td class=xl56 style='border-top:none'>REP.</td>
  <td class=xl27>LOST</td>
  <td class=xl27>1ST</td>
  <td colspan=2 class=xl113 style='border-right:2.0pt double black'>SELF AUDIT</td>
  <td class=xl43 style='border-left:none'>TOTAL</td>
  <td class=xl24>REG</td>
  <td class=xl24>OT.</td>
  <td class=xl24>TOTAL</td>
  <td class=xl24>COST</td>
  <td class=xl37 x:str="YARD ">YARD </td>
  <td class=xl27>&nbsp;</td>
  <td class=xl27>R/C's</td>
  <td class=xl61 style='border-top:none'>&nbsp;</td>
  <td class=xl24>&nbsp;</td>
  <td class=xl62 style='border-top:none'>R/C's</td>
 </tr>
 <tr height=18 style='mso-height-source:userset;height:13.1pt'>
  <td height=18 class=xl104 style='height:13.1pt'>DAY</td>
  <td class=xl44>TRAIN.</td>
  <td class=xl25>CLERK</td>
  <td class=xl25>MISC</td>
  <td class=xl25>REV.</td>
  <td class=xl58>ACC.</td>
  <td class=xl26>HRS</td>
  <td class=xl26>TIME</td>
  <td class=xl26>AID</td>
  <td class=xl60 style='border-top:none'>BAY</td>
  <td class=xl59 style='border-top:none;border-left:none'>FAC.</td>
  <td class=xl44 style='border-left:none'>UNITS</td>
  <td class=xl25>LABOR</td>
  <td class=xl25>LABOR</td>
  <td class=xl25>LABOR</td>
  <td class=xl25>/UNIT</td>
  <td class=xl38>R/C's</td>
  <td class=xl26>HRS.</td>
  <td class=xl26>/HR.</td>
  <td class=xl44>R/C's</td>
  <td class=xl25>HRS.</td>
  <td class=xl63>/HR.</td>
 </tr>



<%
  CreatePageTwoData();
  WritePageTwoData();
  WritePageTwoTotals();
%>


</table>

</body>

</html>