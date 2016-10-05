<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CustomerEdit.aspx.cs" Inherits="InterrailPPRS.Admin.CustomerEdit" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script type="text/javascript">

    function goCancel() {
        try {
            self.location = "Customer.aspx";
        }
        catch (e) { }
    }
</script>
</asp:Content>

<asp:Content ID="leftNavigation" ContentPlaceHolderID="Navigation" runat="server">
  <table width="81%" border="0"  valign="top">
        <% if (CheckSecurity("Super"))
           { %>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallBleuArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"><a href="Company.aspx">Companies</a></td>
        </tr>
        <%}%>
        <% if (CheckSecurity("Super, Admin"))
           { %>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallBleuArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"><a href="Facility.aspx">Facilities</a></td>
        </tr>
        <%}%>
        <% if (CheckSecurity("Super, Admin"))
           { %>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallBleuArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"><a href="Tasks.aspx">Tasks</a></td>
        </tr>
        <%}%>
        <% if (CheckSecurity("Super, Admin"))
           { %>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallBleuArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"><a href="User.aspx">Users</a></td>
        </tr>
        <%}%>
        <% if (CheckSecurity("Super, Admin"))
           { %>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallBleuArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"><a href="EmployeeTransfer.aspx">Transfer Employees</a></td>
        </tr>
        <%}%>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallBleuArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"><a href="MAS90Select.aspx">MAS 90</a></td>
        </tr>
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
        <% if (CheckSecurity("Super, Admin, User"))
           { %>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"><a href="RebillSubTask.aspx">Re-bill Tasks</a></td>
        </tr>
        <%}%>
        <% if (CheckSecurity("Super, Admin, User"))
           { %>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"><a href="EmploymentSource.aspx">Employment Sources</a></td>
        </tr>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"><a href="Employee.aspx">Employees</a></td>
        </tr>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"><a href="Teams.aspx">Teams</a></td>
        </tr>
        <tr>
        <td width="8%" height="18">&nbsp; </td>
        <td width="13%" height="18"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%" height="18"><a href="Customer.aspx">Customers</a></td>
        </tr>
        <%}%>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%">&nbsp;</td>
        <td width="79%">&nbsp;</td>
        </tr>
        <tr>
        <td width="8%">&nbsp;</td>
        <td width="13%"><img src="../Images/SmallRedArrow.gif" width="10" height="12" alt="" /></td>
        <td width="79%"><a href="../Logout.aspx">Logout</a></td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="Main" runat="server">
    <form id="Form1" runat="server"> 
        <table width="90%" border="0" cellspacing="0" cellpadding="8">
            <tr> 
                <td>
	                <!-- #BeginEditable "MainBody" -->            
                    <table align="center" width="249">
                        <tr valign="baseline"> 
                            <td nowrap align="right" colspan="2"> 
                            <div align="center"  class="pageTitle">Customer</div>
                            </td>
                        </tr>
                        <tr valign="baseline"> 
                            <td nowrap align="right" class="Required">&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr valign="baseline"> 
                            <td nowrap align="right">Select Carrier: </td>
                            <td> 
                                <asp:DropDownList id="ddlCarrierSelect" runat="server" OnSelectedIndexChanged="ddlCarrierSelect_SelectedIndexChanged" AutoPostBack="true" />
                            </td>
                        </tr>
                        <tr valign="baseline"> 
                            <td nowrap align="right">Code:</td>
                            <td> 
                                <asp:TextBox ID="txtCustomerCode" runat="server" Width="250" ClientIDMode="Static" />
                            </td>
                        </tr>
                        <tr valign="baseline"> 
                            <td nowrap align="right">Name:</td>
                            <td> 
                                <asp:TextBox ID="txtCustomerName" runat="server" Width="250" ClientIDMode="Static" />
                            </td>
                        </tr>
                        <tr valign="baseline"> 
                            <td nowrap align="right">Default Customer:</td>
                            <td>
                                <asp:CheckBox ID="chkDefaultCustomer" runat="server" />
                            </td>
                        </tr>
                        <tr valign="baseline"> 
                            <td nowrap align="right">Active:</td>
                            <td> 
                                <asp:CheckBox ID="chkActive" runat="server" />
                            </td>
                        </tr>
                        <tr valign="baseline"> 
                            <td>&nbsp;</td>
                            <td nowrap align="left" class="cellTopBottomBorder">Contact Information</td>
                        </tr>
                        <tr valign="baseline"> 
                            <td nowrap align="right">Contact Name:</td>
                            <td>
                                <asp:TextBox ID="txtContactName" ClientIDMode="Static" runat="server" Width="250" />
                            </td>
                        </tr>
                        <tr valign="baseline"> 
                            <td nowrap align="right">Contact Address:</td>
                            <td> 
                                <asp:TextBox ID="txtContactAddress1" runat="server" Width="250" />
                            </td>
                        </tr>
                        <tr valign="baseline"> 
                            <td nowrap align="right">&nbsp;</td>
                            <td> 
                                <asp:TextBox ID="txtContactAddress2" runat="server" Width="250" />
                            </td>
                        </tr>
                        <tr valign="baseline"> 
                            <td nowrap align="right">&nbsp;</td>
                            <td> 
                                <asp:TextBox ID="txtContactAddress3" runat="server" Width="250" />
                            </td>
                        </tr>
                        <tr valign="baseline"> 
                            <td nowrap align="right">&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr valign="baseline"> 
                            <td nowrap align="right" width="149">Last Modified:</td>
                            <td width="138"><asp:Label ID="lblLastModifiedOn" runat="server" Width="250" /></td>
                        </tr>
                        <tr valign="baseline"> 
                            <td nowrap align="right" width="149">By:</td>
                            <td width="138"><asp:Label ID="lblLastModifiedBy" runat="server" Width="250" /></td>
                        </tr>
                        <tr> 
                            <td nowrap align="right">&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr nowrap align="right">&nbsp;
                            <td> 
                                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" ClientIDMode="Static" />
                                <input type="button" name="btnCancel" value="Cancel" onclick="goCancel();" />
                            </td>
                        </tr>
                        <tr> 
                            <td nowrap align="right">&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>   
        </table>
    </form>

    <script src="../Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#btnSave").click(function (e) {
                if ($("#txtCustomerCode").val() == "") {
                    alert("Please enter a customer code");
                    $("#lblErrorMsg").show();
                    e.preventDefault();
                }

                if ($("#txtCustomerCode").val() == "") {
                    alert("Please enter a customer name");
                    $("#lblErrorMsg").show();
                    e.preventDefault();
                }

                if ($("#txtContactName").val() == "") {
                    alert("Please enter a contact name");
                    $("#lblErrorMsg").show();
                    e.preventDefault();
                }
            });
        });
    </script>
</asp:Content>