<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PartialReturns.aspx.cs" Inherits="EventTickets.SALES.PartialReturns" %>
<asp:Content ID="C1" ContentPlaceHolderID="H" runat="server">
</asp:Content>
<asp:Content ID="C2" ContentPlaceHolderID="M" runat="server">
<table width="100%" cellpadding="2" cellspacing="0">
    <tr>
        <td>
            <asp:GridView ID="gv" runat="server" AutoGenerateColumns="true">
            </asp:GridView>
        </td>
    </tr>
    <tr>
        <td align="right">
            <input type="button" id="btnProcess" value="Process" class="btn" />
        </td>
    </tr>
</table>
</asp:Content>