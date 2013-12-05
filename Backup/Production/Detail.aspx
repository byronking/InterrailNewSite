<%@ Page Language="C#" MasterPageFile="~/Production/Production.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="InterrailPPRS.Production.Detail" %>


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
                            <table align="center"  border="0" cellspacing="0" cellpadding="0" width="100%" >
                              <tr> 
                                <td align="default" colspan="5"> 
                                  <div align="center" class="pageTitle"><b>Production Detail</b></div>
                                </td>
                              </tr>
                              <tr> 
                                <td align="default" width="15%">&nbsp;</td>
                                <td align="default" width="15%">&nbsp;</td>
                                <td align="default" width="36%">&nbsp;</td>
                                <td align="default" width="15%">&nbsp;</td>
                                <td align="default" width="19%">&nbsp;</td>
                              </tr>
                              <tr> 
                                <td align="default" width="15%">&nbsp;</td>
                                <td align="default" colspan="4"> 
                                  <div align="right"><a href="DetailEdit.aspx?id=0&WDate=<%= System.DateTime.Now.ToShortDateString()%>">Add New Facility Production Detail</a></div>
                                </td>
                              </tr>
                              <tr> 
                                <td align="default" width="15%">&nbsp;</td>
                                <td align="default" width="15%">&nbsp;</td>
                                <td align="default" width="36%">&nbsp;</td>
                                <td align="default" width="15%">&nbsp;</td>
                                <td align="default" width="19%">&nbsp;</td>
                              </tr>
                              <tr> 
                                <td align="default" width="15%" class="cellTopBottomBorder"> Work Date </td>
                                <td align="left" colspan="4" class="cellTopBottomBorder"> Tasks Summary</td>
                              </tr>
                              <% 
                                  
                            string  rowColor;
                             
                            while ((Repeat1__numRows != 0) && (! rs.EOF)) {

                               rs.Read();  
                             
                              if (Repeat1__index % 2 == 0) {
                                rowColor = "reportEvenLine";
                              }else{	
                                rowColor = "reportOddLine";
                              }


                              rstID = "         SELECT MAX(FacilityProductionDetail.ID)As ID, WorkDate ";
                              rstID += "            FROM FacilityProductionDetail ";
                              rstID += "           WHERE 1 = 1 ";
                              rstID += "             AND FacilityProductionDetail.FacilityID = " + cStr(Session["FacilityID"]);
                              rstID += "             AND WorkDate = '" + cDate(rs.Item("WorkDate")).ToShortDateString() + "'";
                              rstID += "        GROUP BY WorkDate ";
                              rstID += "          UNION ";
                              rstID += "             SELECT 0 As ID, WorkDate  ";
                              rstID += "            FROM NoProductionData ";
                              rstID += "           WHERE 1 = 1 ";
                              rstID += "             AND NoProductionData.FacilityID = " + cStr(Session["FacilityID"]);
                              rstID += "             AND WorkDate = '" + cDate(rs.Item("WorkDate")).ToShortDateString() + "'";

                              rsID = new InterrailPPRS.Production.DataReader(rstID);
                              rsID.Open();
                              rsID.Read();  
                              string sID = rsID.Item("ID");


                              rstTasks  =  "       SELECT TaskCode, COUNT(*) AS NRec,SUM(Units) AS TU, WorkDate,              ";
                              rstTasks += "                 MAX(FacilityProductionDetail.ID)As ID                               ";
                              rstTasks += "            FROM FacilityProductionDetail                                            ";
                              rstTasks += "                 INNER JOIN Tasks                                                    ";
                              rstTasks += "                 ON FacilityProductionDetail.TaskId = Tasks.Id                       ";
                              rstTasks += "                 RIGHT OUTER JOIN FacilityTasks                                      ";
                              rstTasks += "                 ON FacilityProductionDetail.FacilityID = FacilityTasks.FacilityID   ";
                              rstTasks += "                 AND Tasks.Id = FacilityTasks.TaskId                                 ";
                              rstTasks += "           WHERE 1 = 1                                             ";
                              rstTasks += "             AND FacilityProductionDetail.FacilityID = " + cStr(Session["FacilityID"]);
                              rstTasks += "             AND FacilityProductionDetail.WorkDate = '" + cDate(rs.Item("WorkDate")).ToShortDateString() + "'"; 
                              rstTasks += "        GROUP BY WorkDate, TaskCode                                                  ";

                              rsTasks = new InterrailPPRS.Production.DataReader(rstTasks);
                              rsTasks.Open();
                              string sTasks = "";

                              while (! rsTasks.EOF){ 
                                rsTasks.Read();   
                                sTasks += rsTasks.Item("TaskCode") + ":&nbsp;&nbsp;" + rsTasks.Item("NRec") + "&nbsp;(" + rsTasks.Item("TU") + ")&nbsp;&nbsp;&nbsp;";
                              }
  
                              %>
                              <tr class="<%=rowColor%>"> 
                                <td align="default" width="15%"> 
                                  <a href="DetailEdit.aspx?Id=0&WDate=<%=cDate(rs.Item("WorkDate")).ToShortDateString()%>" ><%=cDate(rs.Item("WorkDate")).ToShortDateString()%></a> 
                                </td>
								  <!-- Always use ID=0 instead of sID to avoid opening approved records... -->
                                  <!-- a href="DetailEdit.aspx?Id=<%=sID %>&WDate=<%=cDate(rs.Item("WorkDate")).ToShortDateString()%>" ><%=cDate(rs.Item("WorkDate")).ToShortDateString()%></a> </td  -->
                                <td align="left" colspan=4><%=sTasks%></td>
                              </tr>
                              <% 
                                  Repeat1__index = Repeat1__index + 1;
                                  Repeat1__numRows = Repeat1__numRows - 1;

                                  }
                                %>
                            </table>
                            <table border="0" width="50%" align="center">
                              <tr> 
                                <td width="23%" align="center">&nbsp;</td>
                                <td width="31%" align="center">&nbsp;</td>
                                <td width="23%" align="center">&nbsp;</td>
                                <td width="23%" align="center">&nbsp;</td>
                              </tr>
                              <tr> 
                                <td width="23%" align="center"> 
                                  <% if( MM_offset != 0 ){ %>
                                  <a href="<%=MM_moveFirst%>">First</a> 
                                  <% } // end MM_offset <> 0 %>
                                </td>
                                <td width="31%" align="center"> 
                                  <% if( MM_offset != 0 ){ %>
                                  <a href="<%=MM_movePrev%>">Previous</a> 
                                  <% } // end MM_offset <> 0 %>
                                </td>
                                <td width="23%" align="center"> 
                                  <% if (! MM_atTotal ){ %>
                                  <a href="<%=MM_moveNext%>">Next</a> 
                                  <% } // end Not MM_atTotal %>
                                </td>
                                <td width="23%" align="center"> 
                                  <% if (! MM_atTotal ){ %>
                                  <a href="<%=MM_moveLast%>">Last</a> 
                                  <% } // end Not MM_atTotal %>
                                </td>
                              </tr>
                            </table>
                            <p>Records <%=(rs_first)%> to <%=(rs_last)%> of <%=(rs_total)%> </p>
                            <!-- #EndEditable -->
                          </td>
                        </tr>

                    </table>
</asp:Content>