<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SmartIndex.aspx.cs" Inherits="EventTickets.Sales.SmartIndex" %>
<asp:Content ID="C1" ContentPlaceHolderID="H" runat="server">
<script type="text/javascript">
    function OpenPrd(filter, SmartDest) {
        location.href = 'SmartSales.aspx?cid=' + SmartDest + "&filter=" + filter;        
    }
</script>
</asp:Content>
<asp:Content ID="C2" ContentPlaceHolderID="M" runat="server">
<div style="padding:5px 0px 0px 5px">
    <div style="font-size:20px;font-weight:bold;text-align:center;color:#000">Smart Destinations - Attractions</div>
    <asp:DataList ID="dC" runat="server" RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0" RepeatColumns="4" EnableViewState="false">
    <ItemTemplate>
        <div class="citem" onclick="OpenPrd('<%# Eval("filter").ToString()%>','<%# Eval("SmartDest").ToString()%>')">
            <div class="box-shadow cbox">
                <div class="cname"> <%# Eval("Name").ToString()%> </div>
                <div class="cimg"><img src="../images/<%# Eval("ImageUrl").ToString()%>" alt='<%# Eval("Name").ToString()%>' width="198" /></div>
            </div>
        </div>
    </ItemTemplate>
    </asp:DataList>
    </div>
</asp:Content>
