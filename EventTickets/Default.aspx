<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="EventTickets._Default" %>

<asp:Content ID="H1" runat="server" ContentPlaceHolderID="H">
</asp:Content>
<asp:Content ID="B" runat="server" ContentPlaceHolderID="M">
   <asp:DataList ID="dC" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" CellPadding="0" CellSpacing="0" EnableViewState="false">
    <ItemTemplate>
        <div style="width:290px;padding:10px;cursor:pointer;" onclick="location.href='#SalesDetails.aspx?eid=<%# Eval("uIdPk").ToString()%>'">
            <div class="box-shadow ebox">
            <div class="enmae"> <%# Eval("destinationName").ToString()%> </div>
            <div style="height:125px;overflow:hidden">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="edesc"> <%# Eval("attractionName").ToString()%> </td>
                    </tr>
                </table>                
            </div>
            <div>
                Remaining: 
            </div>
            </div>
        </div>
    </ItemTemplate>
    <ItemStyle VerticalAlign="Top" />
    </asp:DataList>
</asp:Content>
