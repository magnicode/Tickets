<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Stock.aspx.cs" Inherits="EventTickets.Admin.Stock" %>
<asp:Content ID="C1" ContentPlaceHolderID="H" runat="server">
<script type="text/javascript">
    var i = 0;
    var iadded = 0;
    var items = new Array();
    var insideAjax = false;    

    function ShowProducts() {
        if ($("#M_dCategory").val() == "") {
            alert("Please select category");
            return;
        }
        insideAjax = true;
        $.ajax({
            type: "GET",
            url: '/ajax/ajax.aspx',
            data: 'Type=getProducts&Category=' + $("#M_dCategory").val(),
            success: function (msg) {
                if (msg != "") {
                    $("#tdProducts").html(msg);
                    insideAjax = false;
                    LoadStock();
                }
            }
        });
    }   

    function LoadStock() {
        $.ajax({
            type: "GET",
            url: '/ajax/ajax.aspx',
            data: 'Type=getStock&Location=' + $("#M_dLocation").val() + '&Product=' + $("#dProduct").val() + '&Category=' + $("#M_dCategory").val() + '&id=' + guidGenerator(),
            success: function (msg) {
                if (msg != "") {
                    $("#dStock").html(msg);
                }
            }
        });
    }

    function guidGenerator() {
        var S4 = function () {
            return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
        };
        return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
    }
</script>
</asp:Content>
<asp:Content ID="C2" ContentPlaceHolderID="M" runat="server">
<div style="height:100%;min-height:600px;background-color:#f5f5f5"  class="box-shadow">
  <div style="padding:10px;height:100%;" >
    <table cellpadding="0" cellspacing="0" width="100%" height="100%" style="min-height:350px">
      <tr>
        <td id="aCtop" style="height:85%;min-height:350px" valign="top"><table cellpadding="0" cellspacing="0" height="100%" width="100%">
            <tr>
              <td colspan="2" style="font-size:20px;font-weight:bold;text-align:center;color:#000" valign="top" height="10">Ticket Numbers Available in Inventory</td>
            </tr>
            <tr>
              <td colspan="2" style="background-color:#ccc" valign="top"><table cellpadding="0" cellspacing="0" width="100%">
                  <tr>
                    <td colspan="2" style="font-size:18px;font-weight:bold;text-align:center;color:#000" valign="top" height="10">Stock</td>
                  </tr>
                  <tr>
                    <td align="left" valign="top" width="100%"><table cellpadding="2" cellspacing="4" width="100%">
                        <tr>
                          <td>Category: </td>
                          <td><asp:DropDownList runat="server" ID="dCategory" width="200"
                            onchange="ShowProducts()"></asp:DropDownList></td>
                          <td>Product: </td>
                          <td id="tdProducts"><select ID="dProduct" style="width:200px">
                            </select></td>
                        </tr>
                        <tr>
                          <td>Location: </td>
                          <td><asp:DropDownList runat="server" ID="dLocation" onchange="LoadStock()" width="200"></asp:DropDownList></td>
                        </tr>                        
                        <tr>
                          <td colspan="4"><div id="dStock" style="max-height:300px;overflow:auto;text-align:left">
                        <table width='100%' class='grid' cellpadding='0' cellspacing='0'>
                          <tr>
                            <th>Category</th>
                            <th>Product</th>
                            <th>Location</th>
                            <th>Serial Number</th>                            
                          </tr>
                        </table>
                      </div></td>
                        </tr>
                      </table></td>                    
                  </tr>
                </table></td>
            </tr>
          </table></td>
      </tr>      
    </table>
  </div>
</div>
</asp:Content>
