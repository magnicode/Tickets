<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="macyfile.aspx.cs" Inherits="EventTickets.survey.macyfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="H" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="M" runat="server">
    <div style="width:100%;" align="center">
        <table cellpadding="0" cellspacing="0" width="90%">
            <tr>
                <td align="center">
                    <h2>Visitor Center Survey File Download (CSV)</h2>
                </td>
            </tr>
            <tr>
                <td><div style="width:100%; height:1px; background-color:Red;" ></div></td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td><asp:Button ID="btnExport" runat="server" Text="Export to CSV" 
                        onclick="btnExport_Click" CssClass="btn" /></td>
            </tr>
        </table>
    </div>
</asp:Content>
