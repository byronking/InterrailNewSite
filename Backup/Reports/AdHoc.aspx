<%@ Page language="c#" Codebehind="AdHoc.aspx.cs" AutoEventWireup="True" Inherits="InterrailPPRS.Reports.WebForm1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Report Query</title>
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
			<asp:Label id="Label3" style="Z-INDEX: 106; LEFT: 392px; POSITION: absolute; TOP: 186px" runat="server" Width="201px">Show Records that match</asp:Label>
			<asp:Label id="Label5" style="Z-INDEX: 132; LEFT: 18px; POSITION: absolute; TOP: 36px" runat="server" Width="201px">Fields to Show, Match or Order</asp:Label>
			<asp:Label id="Label4" style="Z-INDEX: 130; LEFT: 339px; POSITION: absolute; TOP: 330px" runat="server" Width="201px">Order by</asp:Label>
			<asp:Button id="btnOrderAdd" style="Z-INDEX: 128; LEFT: 277px; POSITION: absolute; TOP: 352px" runat="server" Text=">>" ToolTip="Add" onclick="btnOrderAdd_Click"></asp:Button>
			<asp:Button id="btnOrderDelete" style="Z-INDEX: 127; LEFT: 723px; POSITION: absolute; TOP: 410px" runat="server" Text="X" ToolTip="Delete" Height="29px" onclick="btnOrderDelete_Click"></asp:Button>
			<asp:Button id="btnOrderUp" style="Z-INDEX: 126; LEFT: 722px; POSITION: absolute; TOP: 351px" runat="server" Text="^" ToolTip="Move Up" onclick="btnOrderUp_Click"></asp:Button>
			<asp:Button id="btnOrderDown" style="Z-INDEX: 125; LEFT: 723px; POSITION: absolute; TOP: 382px" runat="server" Text="v" ToolTip="Move Down" onclick="btnOrderDown_Click"></asp:Button>
			<asp:Button id="Button4" style="Z-INDEX: 114; LEFT: 685px; POSITION: absolute; TOP: 46px" runat="server" Text="^" ToolTip="Move Up"></asp:Button>
			<asp:Button id="Button2" style="Z-INDEX: 116; LEFT: 686px; POSITION: absolute; TOP: 81px" runat="server" Text="v"></asp:Button>
			<asp:Button id="Button1" style="Z-INDEX: 119; LEFT: 686px; POSITION: absolute; TOP: 111px" runat="server" Text="X" ToolTip="Delete"></asp:Button>
			<asp:ListBox id="lbOrder" style="Z-INDEX: 124; LEFT: 336px; POSITION: absolute; TOP: 356px" runat="server" Width="378px" Height="102px"></asp:ListBox>
			<asp:Button id="btnSelectAddAll" style="Z-INDEX: 123; LEFT: 254px; POSITION: absolute; TOP: 87px" runat="server" Text="Add All" onclick="btnSelectAddAll_Click"></asp:Button>
			<asp:Button id="Button3" style="Z-INDEX: 118; LEFT: 686px; POSITION: absolute; TOP: 81px" runat="server" Text="v" ToolTip="Move Down"></asp:Button>
			<asp:Button id="btnWhereDelete" style="Z-INDEX: 121; LEFT: 728px; POSITION: absolute; TOP: 214px" runat="server" Text="X" ToolTip="Delete" onclick="btnWhereDelete_Click"></asp:Button>
			<asp:dropdownlist id="DBTableDropDown" runat="server" AutoPostBack="True" style="Z-INDEX: 133; LEFT: 17px; POSITION: absolute; TOP: 8px" onselectedindexchanged="DBTableDropDown_Changed"></asp:dropdownlist>
			<asp:Label id="Label2" style="Z-INDEX: 102; LEFT: 376px; POSITION: absolute; TOP: 13px" runat="server" Width="201px">Fields to show on Report</asp:Label><BR>
			<BR>
			<asp:ListBox id="lbFields" runat="server" Width="203px" Height="329px" AutoPostBack="True" onselectedindexchanged="lbFields_SelectedIndexChanged"></asp:ListBox>
			<asp:ListBox id="lbSelect" style="Z-INDEX: 100; LEFT: 372px; POSITION: absolute; TOP: 39px" runat="server" Width="304px" Height="123px"></asp:ListBox>
			<asp:Label id="Label1" style="Z-INDEX: 101; LEFT: 5px; POSITION: absolute; TOP: 447px" runat="server" Width="123px" Visible="False"></asp:Label>
			<asp:ListBox id="lbWhere" style="Z-INDEX: 103; LEFT: 433px; POSITION: absolute; TOP: 209px" runat="server" Width="286px" onselectedindexchanged="ListBox1_SelectedIndexChanged"></asp:ListBox>
			<HR style="Z-INDEX: 104; LEFT: 230px; WIDTH: 56.68%; POSITION: absolute; TOP: 179px; HEIGHT: 1px" width="56.68%" SIZE="1">
			<HR style="Z-INDEX: 105; LEFT: 233px; WIDTH: 51.41%; POSITION: absolute; TOP: 323px; HEIGHT: 1px" width="51.41%" SIZE="1">
			&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
			<asp:TextBox id="tbWhereOne" style="Z-INDEX: 107; LEFT: 235px; POSITION: absolute; TOP: 219px" runat="server"></asp:TextBox>
			<asp:TextBox id="tbWhereTwo" style="Z-INDEX: 108; LEFT: 235px; POSITION: absolute; TOP: 247px" runat="server"></asp:TextBox>
			<asp:DropDownList id="lbWhereOp" style="Z-INDEX: 109; LEFT: 235px; POSITION: absolute; TOP: 190px" runat="server" AutoPostBack="True" onselectedindexchanged="lbWhereOp_SelectedIndexChanged"></asp:DropDownList>
			<asp:Button id="btnWhereAnd" style="Z-INDEX: 110; LEFT: 438px; POSITION: absolute; TOP: 288px" runat="server" Text="And" onclick="btnWhereAnd_Click"></asp:Button>
			<asp:Button id="btnWhereOr" style="Z-INDEX: 111; LEFT: 480px; POSITION: absolute; TOP: 289px" runat="server" Text="Or" onclick="btnWhereOr_Click"></asp:Button>
			<asp:Button id="bntAddWhere" style="Z-INDEX: 112; LEFT: 399px; POSITION: absolute; TOP: 217px" runat="server" Text=">>" ToolTip="Add" onclick="bntAddWhere_Click"></asp:Button>
			<asp:Button id="bntAddSelect" style="Z-INDEX: 113; LEFT: 271px; POSITION: absolute; TOP: 56px" runat="server" Text=">>" ToolTip="Add" onclick="bntAddSelect_Click"></asp:Button>
			<asp:Button id="bntSelectUp" style="Z-INDEX: 115; LEFT: 685px; POSITION: absolute; TOP: 46px" runat="server" Text="^" ToolTip="Move Up" onclick="bntSelectUp_Click"></asp:Button>
			<asp:Button id="bntSelectDown" style="Z-INDEX: 117; LEFT: 686px; POSITION: absolute; TOP: 81px" runat="server" Text="v" onclick="bntSelectDown_Click"></asp:Button>
			<asp:Button id="bntSelectDelete" style="Z-INDEX: 120; LEFT: 686px; POSITION: absolute; TOP: 111px" runat="server" Text="X" ToolTip="Delete" onclick="bntSelectDelete_Click"></asp:Button>
			<asp:Button id="btnGenerate" style="Z-INDEX: 122; LEFT: 284px; POSITION: absolute; TOP: 482px" runat="server" Text="Generate Report" onclick="btnGenerate_Click"></asp:Button>
			<HR style="Z-INDEX: 129; LEFT: 7px; WIDTH: 100.67%; POSITION: absolute; TOP: 473px; HEIGHT: 1px" width="100.67%" SIZE="1">
		</form>
	</body>
</HTML>
