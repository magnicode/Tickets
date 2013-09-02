<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Sales.aspx.cs" Inherits="EventTickets.Sales.Sales" %>
<asp:Content ID="C1" ContentPlaceHolderID="H" runat="server">
</asp:Content>
<asp:Content ID="C2" ContentPlaceHolderID="M" runat="server">
<asp:DataList ID="dC" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" CellPadding="0" CellSpacing="0" EnableViewState="false">
    <ItemTemplate>
        <div style="width:290px;padding:10px;cursor:pointer;" onclick="location.href='SalesDetails.aspx?eid=<%# Eval("ProductId").ToString()%>'">
            <div class="box-shadow ebox">
            <div class="enmae"> <%# Eval("Name").ToString()%> </div>
            <div style="height:125px;overflow:hidden">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top" width="100"><div style="width:100px;height:80px;overflow:hidden"><img src="../images/<%# Eval("ImageUrl").ToString()%>" width="100%" /></div></td>
                        <td "edesc"><%# Eval("ShortDescription").ToString()%></td>
                    </tr>
                </table>                
            </div>
            <div>
                Remaining: <%# Eval("Remaining").ToString()%>
            </div>
            </div>
        </div>
    </ItemTemplate>
    <ItemStyle VerticalAlign="Top" />
    </asp:DataList>
</asp:Content>
