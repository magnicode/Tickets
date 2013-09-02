<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Tickets.aspx.cs" Inherits="EventTickets.Sales.Tickets" %>
<%@ Register Src="../payment.ascx" TagName="pay" TagPrefix="p" %>
<asp:Content ID="C1" ContentPlaceHolderID="H" runat="server">
<script type="text/javascript">
    function AddSerial() {
        $("#sInput").hide();
        $("#LimitMsg").html("");
        $("#wait").show();
        $.ajax({
            type: "GET",
            url: '/ajax/ajax.aspx',
            data: 'Type=addTicket&SerialNo=' + $("#tSerialNo").val() + '&ProductId=' + $("#M_hPid").val() + "&tid=" + guidGenerator(),
            success: function (msg) {
                if (msg != "") {
                    //alert(msg);
                    $("#wait").hide();
                    var marr = msg.split("~");
                    if (marr.length > 1) {
                        $("#M_sErialEnterd").html(marr[0]);
                        $("#M_tRemaining").html(marr[1] + " Remaining");
                        if (parseInt(marr[1]) == 0) {
                            $("#sInput").html("<br>You have entered all the serial numbers for this ticket type.<br><br><span style='color:#005928;font-weight:bold'>Please proceed to the next step</span>");
                            document.getElementById('M_bNext').disabled = false;
                            $("#M_bNext").removeClass("aspNetDisabled");
                            $("#M_bNext").addClass("btn");
                        }
                    }
                    else if (msg == "limit Reached") {
                        $("#sInput").html("<br>You have entered all the serial numbers for this ticket type.<br><br><span style='color:#005928;font-weight:bold'>Please proceed to the next step</span>");
                        document.getElementById('M_bNext').disabled = false;
                        document.getElementById('M_bNext').style.className = "btn";
                    }
                    else
                        alert(msg);
                    $("#sInput").show();
                    $("#tSerialNo").val("");
                    $("#tSerialNo").focus();
                }
            }
        });
        return false;
    }

    function AddCards() {



        $("#dCards").hide();
        $("#LimitMsg").html("");
        $("#wait").show();

        var table = document.getElementById("M_dlCards");

        for (var i = 1, row; row = table.rows[i]; i++) {
            var tbl = row.cells[0].getElementsByTagName('table');
            var Qty = tbl[0].getElementsByTagName('td')[2].getElementsByTagName('input')[0].value;
            if (Qty != "") {
                    
            }
        }


        for (var i = 1, row; row = table.rows[i]; i++) {

            var tbl = row.cells[0].getElementsByTagName('table');
            var ExpDate = tbl[0].getElementsByTagName('td')[0].innerText;
            var Qty = tbl[0].getElementsByTagName('td')[2].getElementsByTagName('input')[0].value;
            if (Qty != "") {
                $.ajax({
                    type: "GET",
                    url: '/ajax/ajax.aspx',
                    data: 'Type=addCards&Qty=' + Qty + '&ExpDate=' + ExpDate + '&ProductId=' + $("#M_hPid").val() + "&tid=" + guidGenerator(),
                    success: function (msg) {
                        if (msg != "") {
                            //alert(msg);
                            $("#wait").hide();
                            var marr = msg.split("~");
                            if (marr.length > 1) {
                                $("#M_dCardsAdded").html(marr[0]);
                                $("#M_tNumCardsRemain").html(marr[1] + " Remaining");
                                if (parseInt(marr[1]) == 0) {
                                    $("#dCards").html("<br>You have entered all the serial numbers for this ticket type.<br><br><span style='color:#005928;font-weight:bold'>Please proceed to the next step</span>");
                                    document.getElementById('M_bNext').disabled = false;
                                    $("#M_bNext").removeClass("aspNetDisabled");
                                    $("#M_bNext").addClass("btn");
                                }
                            }
                            else if (msg == "limit Reached") {
                                $("#dCards").html("<br>You have entered all the serial numbers for this ticket type.<br><br><span style='color:#005928;font-weight:bold'>Please proceed to the next step</span>");
                                document.getElementById('M_bNext').disabled = false;
                                document.getElementById('M_bNext').style.className = "btn";
                            }
                            else
                                alert(msg);
                            $("#dCards").show();
                            //$("#tSerialNo").val("");
                            //$("#tSerialNo").focus();
                        }
                    }
                });
            }
        }
         
        
        
        return false;
    }


    function guidGenerator() {
        var S4 = function () {
            return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
        };
        return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
    }

    function OpenNumpad(idtbox) {
        var win = window.open("NumberPad.aspx?idtbox=" + idtbox +"&type=MTA", 'npad', 'width=312,height=530');
        win.focus();
    }
</script>
<style>
    .Amt
    {
        font-size:16px;
        color:Black;
        font-weight:bold;	
    }
</style>
</asp:Content>
<asp:Content ID="C2" ContentPlaceHolderID="M" runat="server">

<table style="padding:10px; width:100%; height:370px;">
<tr>
<td style="width:100%; height:100%;">
<table style="width:100%; height:100%; text-align:center; vertical-align:middle;" cellpadding="0"  cellspacing="0" border="0" >
<tr>
    <td align="center" width="70%"  valign="top" style="height:100%;">
        <div style="height:100%;">
    <div style="font-size:20px;font-weight:bold;text-align:center;color:#000" valign="top" height="30">eVENTS Sales Finish</div>
    <div  class="box-shadow" style="height:80%;border:1px solid #ccc;padding:10px;min-height:350px">
    <table cellpadding="0" cellspacing="0" width="100%" style="min-height:350px">     
      <tr>
        <td align="center" style="font-weight:bold" valign="top" height="20">
            <asp:Label ID="lPname" runat="server"></asp:Label>
        </td>
      </tr>
      <tr>
        <td valign="top" align"left">
            <input type="hidden" id="hPid" runat="server"  />
            <div id="dphy" runat="server">
            <table cellpadding="0" cellspacing="0" width="100%" align="left">
                <tr>
                    <td valign="top" width="230" align="left">
                        <div id="tSerails" runat="server"></div>
                        <div id="tRemaining" runat="server"></div>
                        <div id="sInput">
                          Serial Number:<br />
                          &nbsp;&nbsp;&nbsp;&nbsp;<input type="text" id="tSerialNo" style="width:200px" onkeydown="Javascript: if (event.keyCode==13) AddSerial()" tabindex="0" />
                        </div>
                    </td>
                    <td style="padding-left:10px" valign="top" align="left">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td>Serial Numbers:</td>
                            </tr>
                            <tr>
                                <td width="100%">
                                   <div id="sErialEnterd" runat="server" style="border:1px solid #00a;min-width:300px;min-height:300px">                        
                                    </div>
                                </td>
                            </tr>
                        </table>                                                
                    </td>
                </tr>
            </table>
            </div>
            <div id="dnophy" runat="server" style="display:none">
             
            <table cellpadding="0" cellspacing="0" width="100%" align="left" >
                <tr>
                    <td valign="top" width="230" align="left" valign="top">
                        <div id="tNumCards" runat="server"></div>
                        <div id="tNumCardsRemain" runat="server"></div>
                        <div id="dCards" style="padding-left:10px;padding-top:10px">
                            <asp:DataList ID="dlCards" runat="server" Width="100%" ShowHeader="true" >
                                <HeaderTemplate>
                                    <div><b>Cards in inventory</b></div>
                                    <table width="100%" cellpadding="2" border="1">
                                        <tr>
                                            <td style="width:35%;">Expiry Date</td>
                                            <td style="width:35%; text-align:right;">Avilable Qty</td>
                                            <td style="text-align:right;">Qty to use</td>
                                        </tr>
                                    </table>    
                                </HeaderTemplate>
                                <ItemTemplate >
                                    <table width="100%" cellpadding="2" border="1">
                                        <tr>
                                            <td id="td_expdate" style="width:35%;"><%# Eval("expirydate") %></td>
                                            <td style="width:35%; text-align:right;"><%# Eval("avilable_qty") %></td>
                                            <td style="text-align:right;"><input type="text" style="width:90%;" runat="server" id="tQty" value="" onclick="OpenNumpad(this.id)"  /></td>
                                        </tr>
                                    </table>    
                                </ItemTemplate>
                            </asp:DataList>
                            <input type="button" value="Add Cards" onclick="Javascript: AddCards()" />
                        </div>
                    </td>
                    <td style="padding-left:10px" valign="top" align="left">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td width="100%">
                                   <div id="dCardsAdded" runat="server" style="border:1px solid #00a;min-width:300px;min-height:300px">                        
                                    </div>
                                </td>
                            </tr>
                        </table>                                                
                    </td>
                </tr>
            </table>    
            </div>
        </td>
      </tr>
    </table>
    </div>
  </div>
  <div style="text-align:right;padding-top:40px">
  <table cellpadding="0" cellspacing="0" style="width:100%">
    <tr>
        <td align="left" id="LimitMsg">&nbsp;</td>
        <td align="right"><asp:Button runat="server" ID="bNext" Text="Next" CssClass="btn" 
          onclick="bNext_Click" /></td>
    </tr>
  </table>    
  </div>
    </td>
    <td style="width:20px;"></td>
    <td valign="top" style="height:100%">
       <p:Pay ID="payment" runat="server" />
    </td>
</tr>
</table>
</td>
</tr>
</table>
<asp:Button ID="btnDisableEnter" runat="server" Text="" OnClientClick="return false;" style="display:none;"/>
<div style="width:100%;height:100%;display:none;background-color: rgb(136, 136, 136);position:absolute;top:0;left:0;opacity: 0.3;filter:alpha(opacity=30); " id="wait">
    <table align="center" style="height:100%" height="100%">
        <tr>
            <td height="100%" valign="middle">
                <table>
                    <tr>
                        <td style="opacity: 1;filter:alpha(opacity=100);width:220px;height:100px" align="center">
                            <div style="background:#fff;position:absolute;width:220px;height:100px">
                            <img src="../images/wait.gif" />
                            Please wait ...
                            </div>
                        </td>
                    </tr>
                </table>                
            </td>
        </tr>
    </table>
</div>
<script>
    try {
        document.getElementById("tSerialNo").focus();
    } catch (e) {

    } 
</script>
</asp:Content>