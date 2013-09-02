<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="EventTickets.Sales.Index" %>
<asp:Content ID="C1" ContentPlaceHolderID="H" runat="server">
<script type="text/javascript">
    function OpenPrd(pid, SmartDest) {
        if (SmartDest == "3")
            location.href = 'SmartIndex.aspx?cid=' + SmartDest;
        else if(SmartDest == "4")
            location.href = 'SmartSales.aspx?cid=' + SmartDest;
        else
            location.href = 'Sales.aspx?cid=' + pid;
    }
</script>
</asp:Content>
<asp:Content ID="C2" ContentPlaceHolderID="M" runat="server">
    <div style="padding:5px 0px 0px 5px">
    <asp:DataList ID="dC" runat="server" RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0" RepeatColumns="4" EnableViewState="false">
    <ItemTemplate>
        <div class="citem" onclick="OpenPrd('<%# Eval("CategoryId").ToString()%>','<%# Eval("IsPhysicalInventory").ToString()%>')">
            <div class="box-shadow cbox">
                <div class="cname"> <%# Eval("Name").ToString()%> </div>
                <div class="cimg"><img src="../images/<%# Eval("ImageUrl").ToString()%>" width="198" /></div>
            </div>
        </div>
    </ItemTemplate>
    </asp:DataList>
    </div>
</asp:Content>
