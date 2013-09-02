<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalesDetails.aspx.cs" Inherits="EventTickets.Sales.SalesDetails" %>
<asp:Content ID="C1" ContentPlaceHolderID="H" runat="server">
  <script type="text/javascript">
      function OpenNumpad(pid) {
          var win = window.open("NumberPad.aspx?pid=" + pid, 'npad', 'width=312,height=530');
          win.focus();
      }
</script>
</asp:Content>

<asp:Content ID="c2" ContentPlaceHolderID="M" runat="server" EnableViewState="false">
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
            <input type="button" ID="bAddToCart" value="Add to Cart" OnClick="OpenNumpad('<%= Request.QueryString["eid"] %>')" />
        </td>
      </tr>
    </table>
  </div>
  </div> 
  <script type="text/javascript">
    if ($.browser.msie){
        $("#aCtop").css('height', "75%");
    }
    //$("#bAddToCart").corner();
  </script> 
</asp:Content>