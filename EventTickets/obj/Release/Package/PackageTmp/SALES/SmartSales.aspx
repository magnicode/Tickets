<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SmartSales.aspx.cs" Inherits="EventTickets.Sales.SmartSales" %>
<asp:Content ID="C1" ContentPlaceHolderID="H" runat="server">
</asp:Content>
<asp:Content ID="C2" ContentPlaceHolderID="M" runat="server">
<div id="SmartTitle" runat="server" style="font-size:20px;font-weight:bold;text-align:center;color:#000">Smart Destinations - Packages</div>
<div style="position:relative">
<asp:DataList ID="dC" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" CellPadding="0" CellSpacing="0" EnableViewState="false">
    <ItemTemplate>
        <div style="width:290px;padding:10px;cursor:pointer;" onclick="location.href='SmartSalesDetails.aspx?eid=<%# Eval("uIdPk").ToString()%>&cid=<%# Request.QueryString["cid"] %>'">
            <div class="box-shadow ebox">
            <div class="enmae"> <%# Eval("destinationName").ToString()%> </div>
            <div style="height:125px;overflow:hidden">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="edesc"> <%# Eval("attractionName").ToString()%> </td>
                    </tr>
                </table>
            </div>            
            </div>
        </div>
    </ItemTemplate>
    <ItemStyle VerticalAlign="Top" />
    </asp:DataList>
</div>
</asp:Content>
