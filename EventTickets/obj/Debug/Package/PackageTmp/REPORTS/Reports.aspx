<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="HR_PAYROLL.Reports.Reports" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<html>
<head id="Head1" runat="server">
<title>eVENTS Ticketing</title>
<link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body> 
<form id="form1" runat="server" >
<asp:Panel ID="pnlPage" runat="server" Width="100%">
<table style="width: 100%; height: 100%" cellpadding="0" cellspacing="0"><tr><td style="background-color: #e3e3e3;"><center>
<table>  
<tr><td style="background-color: #ffffff; font-size:12PX; ">
    <CR:CrystalReportViewer ID="CrViewer" runat="server" AutoDataBind="True" 
        PrintMode="ActiveX" HasCrystalLogo="False" 
        HasRefreshButton="True" HasSearchButton="False" 
        Height="50px" Width="350px" HasDrilldownTabs="False" 
        HasDrillUpButton="False" EnableDrillDown="False" 
        HasToggleGroupTreeButton="False" ToolPanelView="None" onnavigate="CrViewer_Navigate" onreportrefresh="CrViewer_ReportRefresh" onviewzoom="CrViewer_ViewZoom" 
       />
</td></tr>
</table> </center>
</td>
</tr>
</table> 
</asp:Panel>  
</form>
</body>
</html>
