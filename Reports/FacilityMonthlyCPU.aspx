<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="FacilityMonthlyCPU.aspx.cs" Inherits="InterrailPPRS.Reports.FacilityMonthlyCPU" %>

<html>
<!-- #BeginTemplate "/Templates/admin.dwt" -->
<head>
<title>PPRS</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<link rel="stylesheet" href="../Styles/styles.css" type="text/css" />
<link rel="stylesheet" href="MonthlyCPU.css" type="text/css">
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
<table x:str border=0 cellpadding=0 cellspacing=0 width=930 style='border-collapse: collapse;'>
 <tr height=10 style='height:10pt'>
  <td height=10 class=xl1520928 width=50 style='height:10pt;width:38pt'></td>
  <td colspan=23 class=xl2222222 align='center'><br><br><B>Monthly FMS Report<br>Year of <%=selYear%><br><%=Session["FacilityName"]%></b>
  </td>
 </tr>

 <tr height=10 style='height:10pt'>
  <td height=10 class=xl1520928 width=50 style='height:10pt;width:38pt'></td>
  <td colspan=6 class=xl11820928 style='width:261pt'></td>
  <td colspan=2 class=xl11720928 width=116 style='width:88pt'>&nbsp;</td>
  <td colspan=2 class=xl11820928 width=98 style='width:74pt' ></td>
  <td style='width:38pt'></td>
  <td style='width:48pt'></td>
  <td style='width:35pt'></td>
  <td style='width:38pt'></td>
  <td style='width:33pt'></td>
  <td style='width:35pt'></td>
  <td style='width:40pt'></td>
  <td style='width:56pt'></td>
  <td style='width:40pt'></td>
  <td style='width:34pt'></td>
  <td style='width:39pt'></td>
  <td style='width:35pt'></td>
  <td style='width:45pt'></td>
  <td style='width:33pt'></td>
 </tr>

 <tr height=17 style='height:12.75pt'>
  <td height=17 class=xl3920928  style='height:12.75pt'>&nbsp;</td>
  <td colspan=3 class=xl11420928 style='border-right:.5pt solid black'>                   <b>Unloading    </b></td>
  <td colspan=3 class=xl11520928 style='border-right:.5pt solid black; border-left:none'> <b>Loading      </b></td>
  <td           class=xl2920928  style='border-top:none;border-left:none'>                <b>Total        </b></td>
  <td colspan=3 class=xl11420928 style='border-right:.5pt solid black;  border-left:none'><b>Shuttling    </b></td>
  <td colspan=5 class=xl10220928 style='border-right:.5pt solid black;  border-left:none'><b>Spotting     </b></td>
  <td colspan=4 class=xl11920928 style='border-right:.5pt solid black'>                   <b>Miscellaneous</b></td>
  <td colspan=4 class=xl9820928  style='border-right:.5pt solid black;  border-left:none'><b>Self-Audits  </b></td>
 </tr>
 <tr height=17 style='height:12.75pt'>
  <td height=17 class=xl3920928 style='height:12.75pt'>&nbsp;</td>
  <td class=xl3320928>Tot.</td>
  <td class=xl3320928>&nbsp;</td>
  <td class=xl3420928>&nbsp;</td>
  <td class=xl3720928>Tot.</td>
  <td class=xl7520928>&nbsp;</td>
  <td class=xl7620928>&nbsp;</td>
  <td class=xl3020928 style='border-left:none'><b>Units</b></td>
  <td class=xl3320928 style='border-left:none'>Tot.</td>
  <td class=xl3320928>&nbsp;</td>
  <td class=xl3420928>&nbsp;</td>
  <td class=xl3720928 style='border-left:none'>R/C</td>
  <td class=xl7520928>R/C</td>
  <td class=xl7520928>Down</td>
  <td class=xl7520928>Tot.</td>
  <td class=xl7620928 >R/C</td>
  <td class=xl3320928>&nbsp;</td>
  <td class=xl3320928>&nbsp;</td>
  <td class=xl3320928>&nbsp;</td>
  <td class=xl3420928>Non</td>
  <td class=xl7420928 style='border-left:none'># of</td>
  <td class=xl7620928>In-Bay</td>
  <td class=xl3720928 style='border-left:none'># of</td>
  <td class=xl7620928>Fac.</td>
 </tr>
 <tr class=xl2320928 height=17 style='height:12.75pt'>
  <td height=17 class=xl4020928 style='height:12.75pt'><b>Month</b></td>
  <td class=xl3520928>Hrs.</td>
  <td class=xl3520928>Units</td>
  <td class=xl3620928>UPM</td>
  <td class=xl4520928>Hrs.</td>
  <td class=xl7220928>Units</td>
  <td class=xl7320928>UPM</td>
  <td class=xl3120928 style='border-left:none'><b>/Man</b></td>
  <td class=xl3520928 style='border-left:none'>Hrs.</td>
  <td class=xl3520928>Units</td>
  <td class=xl3620928>UPM</td>

  <td class=xl4520928 style='border-left:none'>In</td>
  <td class=xl7220928>Out</td>
  <td class=xl7220928>Time</td>
  <td class=xl7220928>Hrs.</td>
  <td class=xl7320928 x:str="'/Hr.">/Hr.</td>

  <td class=xl3820928>Train</td>
  <td class=xl3520928>Clerk</td>
  <td class=xl3520928>Misc</td>
  <td class=xl3620928>Rev.</td>

  <td class=xl4520928 style='border-left:none'>In-Bay</td>
  <td class=xl7320928>Scores</td>
  <td class=xl4520928 style='border-left:none'>Fac.</td>
  <td class=xl7320928>Scores</td>
 </tr>

 <% WriteTopRows(); %>

 <% WriteTopTotal(); %>

</table>

<table x:str border=0 cellpadding=0 cellspacing=0 width=900 style='border-collapse: collapse;'>

 <tr height=17 style='height:12.75pt'>
  <td height=17 class=xl3920928   style='height:12.75pt'>&nbsp;</td>
  <td colspan=2 class=xl11520928  style='border-right:.5pt solid black; border-left:none'>  <b>Accidents           </b></td>
  <td colspan=6 class=xl11920928  style='border-right:.5pt solid black'>                    <b>Injuries            </b></td>
  <td colspan=10 class=xl11520928 style='border-right:.5pt solid black; border-left:none'>  <b>Budget/Cost per Unit</b></td>
  <td colspan=3 class=xl11920928   style='border-right:.5pt solid black; border-left:none'><b>Yard Air            </b></td>
  <td colspan=3 class=xl10420928  style='border-right:.5pt solid black'>                    <b>R/C Prepping        </b></td>
 </tr>
 <tr height=17 style='height:12.75pt'>
  <td height=17 class=xl8220928 style='height:12.75pt'>&nbsp;</td>
  <td class=xl7420928 style='border-left:none'># of</td>
  <td class=xl7620928>Per</td>
  <td class=xl3320928># of</td>
  <td class=xl3320928 x:str="Per ">Per </td>
  <td class=xl3320928>Lost</td>
  <td class=xl3320928>Per</td>
  <td class=xl3320928>1st</td>
  <td class=xl3420928>Per</td>
  <td class=xl7520928>&nbsp;</td>
  <td class=xl7520928>&nbsp;</td>
  <td class=xl7520928>&nbsp;</td>
  <td class=xl7520928>&nbsp;</td>
  <td class=xl7520928>&nbsp;</td>
  <td class=xl7520928>&nbsp;</td>
  <td class=xl7520928>&nbsp;</td>
  <td class=xl7520928>&nbsp;</td>
  <td class=xl7520928>&nbsp;</td>
  <td class=xl7620928>&nbsp;</td>
  <td class=xl3320928 style='border-left:none'><span style="mso-spacerun: yes"> </span></td>
  <td class=xl3320928>&nbsp;</td>
  <td class=xl3420928>R/C's</td>
  <td class=xl7520928>&nbsp;</td>
  <td class=xl7520928>&nbsp;</td>
  <td class=xl7620928>R/C's</td>
 </tr>
 <tr height=17 style='height:12.75pt'>
  <td height=17 class=xl4020928 style='height:12.75pt'><b>Month</b></td>
  <td class=xl4520928 style='border-left:none'>Acc.</td>
  <td class=xl8120928 x:num="10000">10,000</td>
  <td class=xl3520928>Report.</td>
  <td class=xl8020928 x:num="10000">10,000</td>
  <td class=xl3520928>Time</td>
  <td class=xl8020928 x:num="10000">10,000</td>
  <td class=xl3520928>Aid</td>
  <td class=xl3620928>10,000</td>
  <td colspan=2 class=xl7220928>Total Units</td>
  <td colspan=2 class=xl7220928>Reg. Labor</td>
  <td colspan=2 class=xl7220928>Overtime</td>
  <td colspan=2 class=xl7220928>Total Labor</td>
  <td colspan=2 class=xl7320928 style='border-right:.5pt solid black'>Cost/Unit</td>
  <td class=xl3520928 style='border-left:none'>R/C's</td>
  <td class=xl8020928>Hrs.</td>
  <td class=xl3620928 x:str="'/Hr.">/Hr.</td>
  <td class=xl7220928>R/C's</td>
  <td class=xl7220928>Hrs.</td>
  <td class=xl7320928 x:str="'/Hr.">/Hr.</td>
 </tr>

 <% WriteBottomRows(); %>

 <% WriteBottomTotal(); %>


</table>

</body>

</html>