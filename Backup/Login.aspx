<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="InterrailPPRS.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<!-- #BeginTemplate "/Templates/NoSecurity.dwt" --> 
<head>
<!-- #BeginEditable "Head" --> 
<title>PPRS</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<link rel="stylesheet" href="Styles/styles.css" type="text/css" />
<script type="text/javascript" language="JavaScript">
<!--
    function bodyload() {
        document.frmLogin.edtUserName.focus();
    }


    function reloadPage(init) {  //reloads the window if Nav4 resized
        if (init == true) with (navigator) {
            if ((appName == "Netscape") && (parseInt(appVersion) == 4)) {
                document.pgW = innerWidth; document.pgH = innerHeight; onresize = reloadPage;
            } 
        }
        else if (innerWidth != document.pgW || innerHeight != document.pgH) location.reload();
    }
    reloadPage(true);
// -->
</script>
<!-- #EndEditable --> 
</head>
<body bgcolor="#FFFFFF" text="#000000" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0" background="images/bglines.gif"  onload="try{bodyload()}catch(e){};" onbeforeunload="try{bodyunload()}catch(e){};">
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr> 
    <td bgcolor="#9999cc"> 
      <table width="760" border="0" cellspacing="0" cellpadding="0">
        <tr> 
          <td class="company" width="272" bgcolor="#FFFFFF"> 
            <div align="center"><%=Session["CompanyName"]%></div>
          </td>
          <td width="488"><img src="images/train.jpg" width="488" height="68"></td>
        </tr>
      </table>
    </td>
  </tr>
  <tr> 
    <td> 
      <table width="100%" height="32" border="0" cellspacing="0" cellpadding="0">
        <tr> 
          <td width="100%" height="6" background="images/topbevelbg.gif" bgcolor="#9999CC" ><img src="images/topbevel.gif" width="760" height="6" alt="" /></td>
        </tr>
        <tr> 
          <td bgcolor="#FF3300"> 
            <table width="760" border="0" cellspacing="0" cellpadding="0">
              <tr> 
                <td width="200" class="date" bgcolor="#FF3300" background="images/dateblock.gif"> 
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
                <td width="45"><img src="images/navgrad01.gif" width="45" height="20" alt="" /></td>
                <td width="115"><a href="Admin/Default.aspx"><img src="images/nav01.gif" width="115" height="20" border="0" alt="" /></a></td>
                <td width="91"><a href="Production/Default.aspx"><img src="images/nav02.gif" width="91" height="20" border="0" alt="" /></a></td>
                <td width="68"><a href="Payroll/Default.aspx"><img src="images/nav03.gif" width="68" height="20" border="0" alt="" /></a></td>
                <td width="83"><a href="Rebilling/Default.aspx"><img src="images/nav04a.gif" width="83" height="20" border="0" alt="" /></a></td>
                <td width="83"><a href="Reports/Default.aspx"><img src="images/nav05a.gif"  alt="Reports"  width="83" height="20" border="0" /></a></td>
                <td width="180"><img src="images/navgrad02.gif" width="59" height="20" alt="" /></td>
              </tr>
            </table>
          </td>
        </tr>
        <tr> 
          <td width="100%" height="6" background="images/botbevelbg.gif" ><img src="images/botbevel.gif" width="760" height="6"  alt="" /></td>
        </tr>
      </table>
    </td>
  </tr>
</table>
<!--- hi there! --->
<table width="580" height="100%" border="0" cellspacing="0" cellpadding="0">
  <tr> 
    <td height="14" align="left" valign="top"><img src="images/subnavtop.gif" width="180" height="14" alt="" /></td>
  </tr>

   <tr> 
     <td height="100%" align="left" valign="top"> 
        <table width="760" height="100%" border="0" cellspacing="0" cellpadding="0">
          <tr> 
            <td width="156" height="100%" background="images/subnavbg.gif" align="left" valign="top" > 
              <table width="156" border="0" cellspacing="0" cellpadding="0">
                <tr> 
                  <td><img src="images/subnav00.gif" width="156" height="22" alt="" /></td>
                </tr>
                <tr> 
                  <td height="220" background="Images/Subnav.gif"><!-- #BeginEditable "LeftNav" --><!-- #EndEditable --></td>
                </tr>
                <tr> 
                  <td> <!-- #BeginEditable "Additional%20Left%20Nav" --><!-- #EndEditable --> 
                    <p>&nbsp;</p>
                    <p>&nbsp;</p>
                    <p align="center"><img src="images/g1440.gif" width="76" height="23" alt="" /></p>
                  </td>
                </tr>
              </table>
            </td>
            <td align="left" valign="top" height="100%"> 
              <table width="580" border="0" cellspacing="0" cellpadding="0">
                <tr> 
                  <td><img src="images/spacer.gif" width="12" height="1"  alt="" /></td>
                  <td><img src="Images/moduletopblank.gif" width="580" height="56"  alt="" /></td>
                </tr>
                <tr> 
                  <td></td>
                  <td> 
                    <table width="580" height="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr> 
                        <td width="2" align="left" valign="top" background="images/leftborder.gif"><img src="images/leftborder.gif" width="2" height="10" alt="" /></td>
                        <td width="547" bgcolor="#FFFFFF" align="center"> 
                          <table width="90%" border="0" cellspacing="0" cellpadding="8">
                            <tr> 
                              <td> <!-- #BeginEditable "MainBody" --> 
                                <form name="frmLogin" runat="server">
                                  <table width="80%" border="0">
                                    <tr> 
                                      <td colspan="2"> 
                                        <% if (Request["why"] == "noaccess")
                                           {   %>
                                        <p align="center">You are not authorized to access that section.</p>
                                        <% }
                                           else
                                           {
                                               if (Request["why"] == "nofacility")
                                               {   %>
                                        <p align="center">User does not have an active default facility.</p>
                                        <%}
                                               else
                                        { %>
                                        <p align="center">Enter valid User Name and Password.</p>
                                        <% }
                                           }%>
                                      </td>
                                    </tr>
                                    <tr> 
                                      <td>&nbsp;</td>
                                      <td>&nbsp;</td>
                                    </tr>
                                    <tr> 
                                      <td> 
                                        <div align="right">User Name: </div>
                                      </td>
                                      <td align="left"> 
                                        <input type="text" style="width:150px;" name="edtUserName" value="<%=Session["UserName"]%>" />
                                      </td>
                                    </tr>
                                    <tr> 
                                      <td> 
                                        <div align="right">Password:</div>
                                      </td>
                                      <td align="left"> 
                                        <input type="password" style="width:150px;" name="edtPassword" />
                                      </td>
                                    </tr>
                                    <tr> 
                                      <td>&nbsp;</td>
                                      <td> 
                                        <asp:Button ID="Logon" runat="server" Text="Log On" onclick="Logon_Click" />
                                      </td>
                                    </tr>
                                  </table>
                                </form>
                                <!-- #EndEditable --> </td>
                            </tr>
                          </table>
                          <p>&nbsp;</p>
                        </td>
                        <td width="31" align="right" valign="top" background="images/rightborder.gif"> 
                          <img src="images/modulecorner.gif" width="31" height="38" alt="" /> 
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
                <tr> 
                  <td></td>
                  <td><img src="images/modulebottom.gif" width="580" height="20" alt="" /></td>
                </tr>
              </table>
            </td>
          </tr>
        </table>
        </td>
   </tr>

</table>

</body>
<!-- #EndTemplate --> 
</html>
