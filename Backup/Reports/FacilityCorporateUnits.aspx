<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FacilityCorporateUnits.aspx.cs" Inherits="InterrailPPRS.Reports.FacilityCorporateUnits" %>

<html>
<!-- #BeginTemplate "/Templates/admin.dwt" -->
<head>
<title>PPRS</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<link rel="stylesheet" href="../Styles/styles.css" type="text/css" />

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

<table x:str border=0 cellpadding=0 cellspacing=0 style='border-collapse: collapse;width=100%'>
 <tr height=27 style='height:35. 60pt'>
  <td colspan=9 class=xl25 align=center >
          <table border=0 width=100%>
                <tr>
                        <td align=center class=xl2222222>
                                 <B>Facility Monitoring System<br>Corporate Unit Totals<br><%=selFrom%>&nbsp;to&nbsp;<%=selTo%></b>
                                 <br>&nbsp;
                        </td>
                </tr>
          </table>
  </td>
 </tr>
</table>


		
        <!-- Create Unit Data -->
        <%CreateUnitsData();%>
		
		

</body>

</html>