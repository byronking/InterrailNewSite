<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FastCalc.aspx.cs" Inherits="InterrailPPRS.Payroll.FastCalc" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FastCalc</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="FastCalc" method="post" runat="server">
			<asp:Label id="Label1" style="Z-INDEX: 101; LEFT: 268px; POSITION: absolute; TOP: 92px" runat="server" Width="342px" Height="43px" Font-Names="Arial Black" Font-Size="Large">Payroll Calculation</asp:Label>
			<asp:Label id="Label2" style="Z-INDEX: 102; LEFT: 276px; POSITION: absolute; TOP: 144px" runat="server" Width="334px" Height="48px">Starting......</asp:Label>
			<asp:HyperLink id="HyperLink1" style="Z-INDEX: 103; LEFT: 289px; POSITION: absolute; TOP: 361px" runat="server" Height="29px" Width="252px" NavigateUrl="calc.aspx">Return</asp:HyperLink>
			<asp:Literal id="Literal1" runat="server"></asp:Literal>
		</form>
	</body>
</HTML>