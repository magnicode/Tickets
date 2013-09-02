<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Cart.ascx.cs" Inherits="EventTickets.Cart.Cart" EnableViewState="false" %>
<script type="text/javascript">
    function ShowDel(id) {
     <% if((Request.ServerVariables["URL"]).ToLower().IndexOf("paymentoptions.aspx") <= 0 && (Request.ServerVariables["URL"]).ToLower().IndexOf("tickets.aspx") <= 0 ) { %>
        if ($("#" + id).is(":visible"))
            $("#" + id).hide();
        else
            $("#" + id).show();
      <% } %>
    }
</script>
<table cellpadding="0" cellspacing="0" width="100%" height="100%" style="border:1px solid #000;min-height:700px;">
    <tr>
        <td style="padding:5px 0px 10px 15px;height:10px;color:#000;background-color:transparent;font-size:18px" valign="top">
            <b>Current Cart</b>
        </td>
    </tr>
    <tr>
        <td id="CartTotal" style="min-height:60px; padding:0px 0px 5px 15px; color:#000;background-color:transparent" runat="server"></td>
    </tr>
    <tr>
        <td style="padding:5px 9px 0px 5px;" valign="top" height="100%">
        
        <div class="box-shadow"  style="padding:5px 0px 0px 5px;border:1px solid #ccc;width:98%;background-color:#fff;min-height:400px; overflow-y:auto;overflow-x:hidden;">
            <asp:DataList ID="dC" runat="server" RepeatDirection="Horizontal"
                RepeatColumns="1" CellPadding="0" CellSpacing="0" Width="100%" 
                onitemcommand="dC_ItemCommand">
            <ItemTemplate>
                <div style="width:98%;padding:0px 5px 5px 0px;font-size:12px;color:#000">
                <div class="box-shadow" style="background-color:#d3d3d3;padding:5px 0px 5px 5px;border:1px solid #000;line-height:20px;cursor:pointer" onclick="ShowDel('d_<%# Eval("CartId").ToString()%>')">
                <div style="font-weight:bold;">
                    <%# Eval("Name").ToString()%>
                </div>
                <div>
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="text-align:left" width="50%">
                                Quantity: <%# Eval("Quantity").ToString()%>
                            </td>
                            <td style="text-align:right">
                                Total: $<%# Eval("Total").ToString()%></td>
                        </tr>
                    </table>
                </div>
                </div>
                <div id="d_<%# Eval("CartId").ToString()%>" style="display:none;text-align:center;width:100%;margin-top:-20px;">
                    <asp:HiddenField runat="server" ID="cId" Value='<%# Eval("CartId").ToString()%>' />
                    <asp:LinkButton runat="server" ID="bRemove" CssClass="lbtn" Text="Remove" CommandName="del" />
                </div>
                </div>
            </ItemTemplate>
            </asp:DataList>
            </div>
            <div style="text-align:center;padding-top:5px;">
                <input type="button" runat="server" disabled="disabled" id="bPurcahse" value="Purchase" style="width:100%" onclick="location.href='/sales/tickets.aspx'" />
            </div>
             
        </td>
    </tr>    
</table>