<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SmartSalesDetails.aspx.cs" Inherits="EventTickets.Sales.SmartSalesDetails" %>
<asp:Content ID="C1" ContentPlaceHolderID="H" runat="server">
<script type="text/javascript">
    function ValidateQty() {
        if (document.getElementById("M_adultcnt").value == "0" && document.getElementById("M_childcnt").value == "0") {
            alert("Please select number of adult/child");
            return false;
        }
    }

    function OpenNumpad(tp) {
        var win = window.open("NumberPad.aspx?cid="+ <%= Request.QueryString["cid"] %> +"&qtyaddtype=" + tp + "&pid=" + $("#M_UidPk").val() + "&productCode=" + $("#M_productCode").val() + "&productname=" + $("#M_productname").val() + "&priceadult=" + $("#M_priceadult").val() + "&pricechild=" + $("#M_pricechild").val(), 'npad', 'width=312,height=530');
        win.focus();
    }
</script>
</asp:Content>
<asp:Content ID="C2" ContentPlaceHolderID="M" runat="server">
<div style="height:100%;min-height:600px;background-color:#f5f5f5"  class="box-shadow">
  <div style="padding:10px;height:100%;" >
    <table cellpadding="0" cellspacing="0" width="100%" height="100%" style="min-height:350px">
      <tr>
        <td id="aCtop" style="height:85%;min-height:350px" valign="top"><table cellpadding="0" cellspacing="0" height="100%">
            <tr>
              <td colspan="2" style="font-size:18px;font-weight:bold;text-align:center;color:#000"><asp:Label ID="lName" runat="server"></asp:Label></td>
            </tr>
            <tr>
              <td id="tImgPrice" runat="server" valign="top" style="font-size:16px;line-height:25px;color:#000;padding-right:20px;width:130px"></td>
              <td id="tDesc" runat="server" valign="top" style="background:#fff;width:100%;height:100%;min-height:300px"></td>
            </tr>
          </table></td>
      </tr>
      <tr><td style="height:50px;"></td></tr>
      <tr>
        <td colspan="2" valign="bottom" style="height:15%;padding-bottom:20px" align="right">
            <table cellpadding="0" cellspacing="0" align="right">
                <tr>
                    <td style="padding-left:5px">
                    <input type="button" id="pAdult" runat="server" disabled="disabled" value="Purchase Adult" OnClick="OpenNumpad('A')" />
                    </td>
                    <td style="padding-left:5px">
                    <input type="hidden" id="UidPk" runat="server" />
                    <input type="hidden" id="productCode" runat="server" />
                    <input type="hidden" id="productname" runat="server" />
                    <input type="hidden" id="priceadult" runat="server" />
                    <input type="hidden" id="pricechild" runat="server" />
                    <input type="button" id="pChild" runat="server" disabled="disabled" value="Purchase Child"  OnClick="OpenNumpad('C')" />
                    </td>
                </tr>
            </table>            
        </td>
      </tr>
    </table>
  </div>
  </div>
</asp:Content>
