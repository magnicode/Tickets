<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StockEntry.aspx.cs" Inherits="EventTickets.Admin.StockEntry" %>
<asp:Content ID="C1" ContentPlaceHolderID="H" runat="server">
<script src="../Scripts/jquery.datepick.js" type="text/javascript"></script>
<link href="../Styles/jquery.datepick.css" rel="stylesheet" type="text/css" />
        
<script type="text/javascript">
    var i = 0;
    var iadded = 0;
    var items = new Array();
    var insideAjax = false;
    var isPhyStock = false;
    var activedeltab = "del";

    function GotoEnd(e) {
        if (e.shiftKey || e.ctrlKey || e.altKey) {
            if (e.preventDefault) e.preventDefault(); else e.returnValue = false;
        } else {
            var key = e.keyCode;
            /*if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105))) {
                if (e.preventDefault) e.preventDefault(); else e.returnValue = false;
                e.keycode = 0;
            }*/
            if ((key < 48 || key > 57) && key > 31 && (key < 96 || key > 105)) {
                //alert(key);
                if (e.preventDefault) e.preventDefault(); else e.returnValue = false;
                e.keycode = 0;
                return false;
            }
        }
        if (e.keyCode != 13) return;
        if ($.trim($("#M_tSerialNoS").val()) == "") {
            alert("Please enter starting serial number");
            $("#M_tSerialNoS").focus();
            return;
        }
        $("#M_tSerialNo").focus();
    }

    function AddSerial(e) {
        iadded = 0;
            if (e.shiftKey || e.ctrlKey || e.altKey) {
                if (e.preventDefault) e.preventDefault(); else e.returnValue = false;
            } else {
                var key = e.keyCode;
                /*if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105))) {
                    if (e.preventDefault) e.preventDefault(); else e.returnValue = false;
                    e.keycode = 0;
                }*/
                if ((key < 48 || key > 57) && key > 31 && (key < 96 || key > 105)) {
                    //alert(key);
                    if (e.preventDefault) e.preventDefault(); else e.returnValue = false;
                    e.keycode = 0; 
                    return false;                    
                }
            }
            if (e.keyCode != 13) return;
            if (insideAjax) return;
            if ($("#dProduct").val() == "" || $("#dProduct").val() == null) {
            alert("Please select product");
            return;
        }
        if ($.trim($("#M_tSerialNoS").val()) == "") {
            alert("Please enter starting serial number");
            return;
        }

        var startserial = parseInt($.trim($("#M_tSerialNoS").val()));
        var endserial = parseInt($.trim($("#M_tSerialNo").val()));
        if(endserial == 0 || isNaN(endserial))
            endserial = startserial;
        if (endserial == 0 || isNaN(endserial)) {
            alert("Please eneter numeric values");
            return
        }

        $("#wait").show();
        $.ajax({
            type: "GET",
            url: '/ajax/ajax.aspx',
            data: 'Type=checkDuplicate&Location=' + $("#M_dLocation").val() + '&Product=' + $("#dProduct").val() + '&Category=' + $("#M_dCategory").val() + "&startserial=" + startserial + "&endserial=" + endserial + '&tid=' + guidGenerator(),
            success: function (msg) {
                if (msg != "") {
                    $("#wait").hide();
                    if (msg == "1") {
                        for (var j = 0; j < items.length; j++) {
                            //if (items[j][1] == $("#dProduct").val() && items[j][0] == $("#M_tSerialNoS").val()) 
                            if (items[j][1] == $("#dProduct").val() && (items[j][0] >= startserial && items[j][0] <= endserial)) {
                                alert("Serial number alerady entered");
                                return;
                            }
                        }
                        for (var k = startserial; k <= endserial; k++) {
                            items[i] = new Array()
                            items[i][0] = k; //$("#M_tSerialNo").val();
                            items[i][1] = $("#dProduct").val();
                            items[i][2] = $("#M_dLocation").val();
                            items[i][3] = $("#dProduct option:selected").text();
                            items[i][4] = $("#M_dLocation option:selected").text();
                            i++;
                        }
                        var sAdded = '<table class="grid" width="100%" cellpadding="0" cellspacing="0"><tr><th>Serial Number</th><th>Location</th><th>Product</th></tr>';
                        for (var j = 0; j < items.length; j++) {

                            sAdded += "<tr><td>" + items[j][0] + "</td><td>" + items[j][4] + "</td><td>" + items[j][3] + "</td></tr>";
                        }
                        $("#StockAdded").html(sAdded);

                        $("#M_tSerialNo").val("");
                        //$("#M_tSerialNo").focus();
                        $("#M_tSerialNoS").val("");
                        $("#M_tSerialNoS").focus();
                    }
                    else {
                        alert(msg);
                    }
                }
            }
        });
        
        return false;
    }

    function ClearData() {
        items = new Array();
        i = 0;
        iadded = 0;
        $("#StockAdded").html('<table width="100%" class="grid" cellpadding="0" cellspacing="0"><tr><th>Serial Number</th><th>Location</th><th>Product</th></tr></table>');
    }

    function ShowProducts() {
        if ($("#M_dCategory").val() == "") {
            alert("Please select category");
            return;
        }
        $("#wait").show();
        insideAjax = true;
        $.ajax({
            type: "GET",
            url: '/ajax/ajax.aspx',
            data: 'Type=getProducts&Category=' + $("#M_dCategory").val() + '&tid=' + guidGenerator(),
            success: function (msg) {
                if (msg != "") {
                    $("#tdProducts").html(msg);
                    insideAjax = false;
                    $("#wait").hide();
                }
            }
        });
    }

    function ShowProductsnp() {
        if ($("#M_dCategorynp").val() == "") {
            alert("Please select category");
            return;
        }
        $("#wait").show();
        insideAjax = true;
        $.ajax({
            type: "GET",
            url: '/ajax/ajax.aspx',
            data: 'Type=getProducts&Category=' + $("#M_dCategorynp").val() + '&tid=' + guidGenerator(),
            success: function (msg) {
                if (msg != "") {
                    $("#tdProductsnp").html(msg.replace("ID='dProduct' onchange='LoadStock()'","ID='dProductnp' onchange='LoadStocknp()'"));
                    insideAjax = false;
                    $("#wait").hide();
                }
            }
        });
    }

    function SaveInventory() {
        if (isPhyStock == false) {
            if ($("#tNoCards").val() == "") {
                alert("Please enter Number of Cards");
                return;
            }
            if ($("#tExpDate").val() == "") {
                alert("Please enter Expiration Date");
                return;
            }
            $("#wait").show();
            $.ajax({
                type: "GET",
                url: '/ajax/ajax.aspx',
                data: 'Type=addNoPhyStock&Location=' + $("#M_dLocation").val() + '&Product=' + $("#dProduct").val() + '&Category=' + $("#M_dCategory").val() + '&Tickets=' + $("#tNoCards").val() + '&ExpDate=' + $("#tExpDate").val() + '&tid=' + guidGenerator(),
                success: function (msg) {
                    //if (msg != "") 
                    //{
                    iadded = 0;
                    LoadStock();
                    $("#wait").hide();
                    $("#tNoCards").val("");
                    $("#tExpDate").val("");
                    alert("Stock added successfully");
                    //}
                }
            });
            return;
        }

        if (items.length <= 0) return;
        $("#wait").show();
        $.ajax({
            type: "GET",
            url: '/ajax/ajax.aspx',
            data: 'Type=addStock&Location=' + items[iadded][2] + '&Product=' + items[iadded][1] + '&Serail=' + items[iadded][0] + '&tid=' + guidGenerator(),
            success: function (msg) {
                if (msg != "") {
                    //$("#tdProducts").html(msg);
                    iadded++;
                    if (iadded < items.length)
                        SaveInventory();
                    else {
                        iadded = 0;
                        LoadStock();
                        $("#wait").hide();
                        ClearData();
                        alert("Stock added successfully");
                    }
                }
            }
        });
    }

    function LoadStock() {
        $("#wait").show();
        $("#tFstart").val("");
        $("#tFend").val("");
        $.ajax({
            type: "GET",
            url: '/ajax/ajax.aspx',
            data: 'Type=loadStock&Location=' + $("#M_dLocation").val() + '&Product=' + $("#dProduct").val() + '&Category=' + $("#M_dCategory").val() + '&tid=' + guidGenerator(),
            success: function (msg) {
                if (msg != "") {
                    var msgarr = msg.split("~");
                    $("#dStock").html(msgarr[0]);
                    $("#stockcount").html("<b>Tickets in existing inventory</b> - Total: " + msgarr[1]);
                    //alert(msgarr[2]);
                    if (msgarr[2] == "0") {
                        $(".nophy").show();
                        $(".phy").hide();
                        $("#newPhyInv").hide();
                        isPhyStock = false;
                    }
                    else {
                        $(".phy").show();
                        $(".nophy").hide();
                        $("#newPhyInv").show();
                        isPhyStock = true;
                    }
                    $("#wait").hide();
                }
            }
        });
    }
    
    function RemoveSerail(stockid) {
        var n = $("input:checked").length;
        if (n <= 0) return;
        var ni = 0;
        if (activedeltab == "delnp") {
            if($("#dProductnp").val() == "" || $("#M_dCategorynp").val() == "" || $("#M_dLocationnp").val() == "")
            {
                alert("Please select category, product and location");
                return;
            }
            if (!confirm("Are you sure you want  to remove the selected stock?")) return;
            $("#wait").show();
            $('input:checked').each(function () {
                //alert(this.value);
                stockid = this.value;
                $.ajax({
                    type: "GET",
                    url: '/ajax/ajax.aspx',
                    data: 'Type=removeStocknp&Location=' + $("#M_dLocationnp").val() + '&Product=' + $("#dProductnp").val() + '&Category=' + $("#M_dCategorynp").val() + '&expdate=' + stockid + '&reason=' + $("#tReason").val() + '&tid=' + guidGenerator(),
                    success: function (msg) {
                        ni++;
                        if (ni == n) {
                            $("#wait").hide();
                            LoadStocknp();
                        }
                    }
                });
            });
            return;
        }
        if (!confirm("Are you sure you want  to remove the selected stock?")) return;
        $("#wait").show();
        $('input:checked').each(function () {
            //alert(this.value);
            stockid = this.value;
            $.ajax({
                type: "GET",
                url: '/ajax/ajax.aspx',
                data: 'Type=removeStock&stockid=' + stockid + '&reason=' + $("#tReason").val() + '&tid=' + guidGenerator(),
                success: function (msg) {
                    ni++;
                    if (ni == n) {
                        //LoadStock();
                        $("#wait").hide();
                        var startserial = parseInt($.trim($("#tFstart").val()));
                        var endserial = parseInt($.trim($("#tFend").val()));
                        if (endserial == 0 || isNaN(endserial))
                            endserial = startserial;
                        if (endserial == 0 || isNaN(endserial)) {
                            alert("Please eneter numeric values");
                            return
                        }
                        LoadStockBySerial(startserial, endserial);
                    }
                }
            });
        });
    }

    function GotoFEnd(e) {
        if (e.shiftKey || e.ctrlKey || e.altKey) {
            if (e.preventDefault) e.preventDefault(); else e.returnValue = false;
        } else {
            var key = e.keyCode;
            /*if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105))) {
            if (e.preventDefault) e.preventDefault(); else e.returnValue = false;
            e.keycode = 0;
            }*/
            if ((key < 48 || key > 57) && key > 31 && (key < 96 || key > 105)) {
                //alert(key);
                if (e.preventDefault) e.preventDefault(); else e.returnValue = false;
                e.keycode = 0;
                return false;
            }
        }
        if (e.keyCode != 13) return;
        if ($.trim($("#tFstart").val()) == "") {
            alert("Please enter starting serial number");
            $("#tFstart").focus();
            return;
        }
        $("#tFend").focus();
    }

    function SearchSerial(e) {
        if (e.shiftKey || e.ctrlKey || e.altKey) {
            if (e.preventDefault) e.preventDefault(); else e.returnValue = false;
        } else {
            var key = e.keyCode;
            if ((key < 48 || key > 57) && key > 31 && (key < 96 || key > 105)) {
                //alert(key);
                if (e.preventDefault) e.preventDefault(); else e.returnValue = false;
                e.keycode = 0;
                return false;
            }
        }
        if (e.keyCode != 13) return;
        if (insideAjax) return;
        if ($.trim($("#tFstart").val()) == "") {
            alert("Please enter starting serial number");
            return;
        }

        var startserial = parseInt($.trim($("#tFstart").val()));
        var endserial = parseInt($.trim($("#tFend").val()));
        if (endserial == 0 || isNaN(endserial))
            endserial = startserial;
        if (endserial == 0 || isNaN(endserial)) {
            alert("Please eneter numeric values");
            return
        }
        LoadStockBySerial(startserial, endserial);
    }

    function LoadStockBySerial(startserial, endserial) {
        $("#wait").show();
        $.ajax({
            type: "GET",
            url: '/ajax/ajax.aspx',
            data: 'Type=loadStockBySerial&startserial=' + startserial + '&endserial=' + endserial + '&tid=' + guidGenerator(),
            success: function (msg) {
                if (msg != "") {
                    var msgarr = msg.split("~");
                    $("#ddStock").html(msgarr[0]);
                    $("#dstockcount").html("<b>Delete inventory</b> - Total: " + msgarr[1]);

                    $("#ddStocknp").html("<table width='100%' class='grid' cellpadding='0' cellspacing='0'><tr><th>Tickets Count</th><th>Product</th><th>Expiry Date</th><th>Location</th></tr></table>");
                    $("#dstockcountnp").html("<b>Delete inventory</b>");

                    $("#wait").hide();
                }
            }
        });
    }

    function LoadStocknp() {       
        $("#wait").show();
        $.ajax({
            type: "GET",
            url: '/ajax/ajax.aspx',
            data: 'Type=loadStocknp&Location=' + $("#M_dLocationnp").val() + '&Product=' + $("#dProductnp").val() + '&Category=' + $("#M_dCategorynp").val() + '&tid=' + guidGenerator(),            
            success: function (msg) {
                if (msg != "") {
                    var msgarr = msg.split("~");
                    $("#ddStocknp").html(msgarr[0]);
                    $("#dstockcountnp").html("<b>Delete inventory</b> - Total: " + msgarr[1]);

                    $("#ddStock").html("<table width='100%' class='grid' cellpadding='0' cellspacing='0'><tr><th>Serial Number</th><th>Product</th><th>Category</th><th>Location</th></tr></table>");
                    $("#dstockcount").html("<b>Delete inventory</b>");

                    $("#wait").hide();
                }
            }
        });
    }

    function AdjustQty() {
        var n = $("input:checked").length;
        if (n <= 0) return;
        var ni = 0;
        if (activedeltab == "delnp") {
            if ($("#dProductnp").val() == "" || $("#M_dCategorynp").val() == "" || $("#M_dLocationnp").val() == "") {
                alert("Please select category, product and location");
                return;
            }
            if (!confirm("Are you sure you want  to adjust the selected stock?")) return;
            $("#wait").show();
            var newqty = 0;
            $('input:checked').each(function () {
                //alert(this.value);
                stockid = this.value;
                var tr = $(this).parent('td').parent('tr');
                newqty = 0;
                $("input[type=text]", tr).each(function () {
                    newqty = $(this).val();
                });
                $.ajax({
                    type: "GET",
                    url: '/ajax/ajax.aspx',
                    data: 'Type=adjustStocknp&Location=' + $("#M_dLocationnp").val() + '&Product=' + $("#dProductnp").val() + '&Category=' + $("#M_dCategorynp").val() + '&expdate=' + stockid + '&newqty=' + newqty + '&reason=' + $("#tReason").val() + '&tid=' + guidGenerator(),
                    success: function (msg) {
                        ni++;
                        if (ni == n) {
                            $("#wait").hide();
                            LoadStocknp();
                        }
                    }
                });
            });
            return;
        }
    }

    function SelectAll() {
        $('input:checkbox').each(function () {
            this.checked = true;
        });
    }

    function UnSelectAll() {
        $('input:checkbox').each(function () {
            this.checked = false;
        });
    }

    function guidGenerator() {
        var S4 = function () {
            return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
        };
        return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
    }
</script>
<script type="text/javascript">
    function ShowTab(tab) {
        switch (tab) {
            case "add":
                $("#dadd").show();
                $("#ddelete").hide();
                $("#bdel").css('border-color', "#000");
                $("#badd").css('border-color', "lime");
                $("#dbtnadd").show();
                $("#dbtndelete").hide();
                break;
            case "del":
                $("#dadd").hide();
                $("#ddelete").show();
                $("#bdel").css('border-color', "lime");
                $("#badd").css('border-color', "#000");
                $("#dbtnadd").hide();
                $("#dbtndelete").show();
                break;
        }
    }    

    $(document).ready(function () {
        $("#tExpDate").datepick();        
    });
</script>

<style>
ul.tabs {
    margin: 0;
    padding: 0;
    float: left;
    list-style: none;
    height: 32px;
    border-bottom: 1px solid #999;
    border-left: 1px solid #999;
    width: 100%;
    font-weight:bold;
}
ul.tabs li {
     
    float: left;
    margin: 0;
    padding: 0;
    height: 31px;
    line-height: 31px;
    border: 1px solid #999;
    border-left: none;
    margin-bottom: -1px;
    background: #F0F0F0;
    overflow: hidden;
    position: relative;
}
ul.tabs li a {
    text-decoration: none;
    color: #000;
    display: block;
    font-size: 1.2em;
    padding: 0 20px;
    border: 1px solid #fff;
    outline: none;
}
ul.tabs li a:hover {
    background: #ccc;
}   
html ul.tabs li.active, html ul.tabs li.active a:hover  {
    background: #ccc;
    border-bottom: 1px solid #ccc;
}
.nophy
{
    display:none;
}
</style>
</asp:Content>
<asp:Content ID="C2" ContentPlaceHolderID="M" runat="server">
    <div style="height:100%;min-height:600px;background-color:#f5f5f5"  class="box-shadow">
  <div style="padding:10px;height:100%;" >
    <table cellpadding="0" cellspacing="0" width="100%" height="100%" style="min-height:475px">
      <tr>
        <td id="aCtop" style="height:85%;min-height:350px" valign="top"><table cellpadding="0" cellspacing="0" height="100%" width="100%">
            <tr>
              <td colspan="2" style="font-size:20px;font-weight:bold;text-align:left;color:#000" valign="top" height="10">Stock Management</td>
            </tr>
            <tr>
                <td colspan="2" height="30" style="padding-top:5px">
                    <!--table cellpadding="0" cellspacing="0">
                    <tr><td><input type="button" id="badd" style="border:1px solid lime;" onclick="ShowTab('add')" value="Add Inventory" /></td><td><input type="button" id="bdel" onclick="ShowTab('del')" value="Delete Inventory" /></td></tr>
                    </table-->
                    <ul class="tabs" style="border-left:0px">
                        <li style="border-left:0px;border-top:0px;border-top:0px;width:10px"></li>
                        <li id="lifirst"><a href="#dadd">Add Inventory</a></li>
                        <li><a href="#ddelete">Delete Inventory(With s/n)</a></li>
                        <li><a href="#ddeletenp" onclick="LoadStocknp();">Delete Inventory(Without s/n)</a></li>  
                      </ul>

                </td>
            </tr>
            <tr>
              <td colspan="2" style="background-color:#ccc;min-height:400px" valign="top">
                <div id="dadd" style="width:100%" class="tab_content">
                <table cellpadding="0" cellspacing="0" width="100%">                 
                  <tr>
                    <td align="left" valign="top" width="400"><table cellpadding="2" cellspacing="4" width="100%">
                        <tr>
                          <td>Category: </td>
                          <td><asp:DropDownList runat="server" ID="dCategory" width="200"
                            onchange="ShowProducts()"></asp:DropDownList></td>
                        </tr>
                        <tr>
                          <td>Product: </td>
                          <td id="tdProducts"><select id="dProduct" style="width:200px">
                            </select></td>
                        </tr>
                        <tr>
                          <td>Location: </td>
                          <td><asp:DropDownList runat="server" ID="dLocation" onchange="LoadStock()" width="200"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="nophy">Number of Cards: </td><td class="nophy"><input type="text" id="tNoCards" /></td>
                        </tr>
                        <tr>
                            <td class="nophy">Expiration Date: </td><td class="nophy"><input type="text" id="tExpDate" /></td>
                        </tr>
                        <tr>
                          <td width="90" class="phy">Serial Number: </td>
                          <td class="phy"><div style="width:35px;float:left">Start:</div><div style="float:left"><asp:TextBox runat="server" ID="tSerialNoS" Width="160" MaxLength="100" onkeydown="Javascript:GotoEnd(event)"></asp:TextBox></div></td>
                        </tr>
                        <tr>
                          <td width="90" class="phy">&nbsp;</td>
                          <td class="phy"><div style="width:35px;float:left">End:</div><div style="float:left"><asp:TextBox runat="server" ID="tSerialNo" Width="160" MaxLength="100" onkeydown="Javascript:AddSerial(event)"></asp:TextBox></div></td>
                        </tr>
                        <tr>
                          <td colspan="2" id="newPhyInv">
                          <div>
                            <b>Pending tickets to add to inventory</b>
                          </div>
                          <div id="StockAdded" style="max-height:230px;overflow:auto">
                              <table class="grid" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                  <th>Serial Number</th>
                                  <th>Location</th>
                                  <th>Product</th>
                                </tr>
                              </table>
                            </div></td>
                        </tr>
                      </table></td>
                    <td valign="top" align="left" style="padding-left:10px"><div id="stockcount"><b>Tickets in existing inventory</b></div>
                      
                      <div id="dStock" style="width:100%;max-height:360px;overflow:auto;text-align:left">
                        <table width='100%' class='grid' cellpadding='0' cellspacing='0'>
                          <tr>
                            <th>Serial Number</th>
                            <th>Product</th>
                            <th>Category</th>
                            <th>Location</th>
                          </tr>
                        </table>
                      </div>                      
                      </td>
                  </tr>
                </table>
                </div>
                <div id="ddelete" style="width:100%;display:none;padding-left:10px;" class="tab_content">
                    <div id="dstockcount"><b>Delete inventory</b></div>
                      <div style="text-align:right;width:99%;height:30px">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td align="left">
                                    <a href="javascript:void(0)" onclick="SelectAll()">Select All</a> <a href="javascript:void(0)" onclick="UnSelectAll()">Unselect All</a>
                                </td>
                                <td align="right">
                                <table cellpadding="2" cellspacing="2" align="right">
                            <tr>
                                <td>Search by:  </td>
                                <td>Serial # from: </td><td><input ID="tFstart" Width="160" MaxLength="100" onkeydown="Javascript:GotoFEnd(event)" /></td>
                                <td>to: </td><td><input ID="tFend" Width="160" MaxLength="100" onkeydown="Javascript:SearchSerial(event)"/></td>
                            </tr>
                        </table>
                                </td>
                            </tr>
                        </table>                       
                      </div>
                      <div id="ddStock" style="width:99%;max-height:360px;overflow:auto;text-align:left">
                        <table width='100%' class='grid' cellpadding='0' cellspacing='0'>
                          <tr>
                            <th>Serial Number</th>
                            <th>Product</th>
                            <th>Category</th>
                            <th>Location</th>
                          </tr>
                        </table>
                      </div>
                </div>

                <div id="ddeletenp" style="width:100%;display:none;padding-left:10px;" class="tab_content">
                    <div id="dstockcountnp"><b>Delete inventory</b></div>
                      <div style="text-align:right;width:99%;height:30px">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td align="left">
                                    <a href="javascript:void(0)" onclick="SelectAll()">Select All</a> <a href="javascript:void(0)" onclick="UnSelectAll()">Unselect All</a>
                                </td>
                                <td align="right">
                                <table cellpadding="2" cellspacing="2" align="right">
                            
                            <tr>
                          <td>Category: </td>
                          <td><asp:DropDownList runat="server" ID="dCategorynp" width="200"
                            onchange="ShowProductsnp()"></asp:DropDownList></td>
                          <td>Product: </td>
                          <td id="tdProductsnp"><select style="width:200px">
                            </select></td>
                          <td>Location: </td>
                          <td><asp:DropDownList runat="server" ID="dLocationnp" onchange="LoadStocknp()" width="200"></asp:DropDownList></td>
                        </tr>

                        </table>
                                </td>
                            </tr>
                        </table>                       
                      </div>
                      <div id="ddStocknp" style="width:99%;max-height:360px;overflow:auto;text-align:left">
                        <table width='100%' class='grid' cellpadding='0' cellspacing='0'>
                          <tr>
                            <th>Tickets Count</th>
                            <th>Product</th>
                            <th>Expiry Date</th>
                            <th>Location</th>
                          </tr>
                        </table>
                      </div>
                </div>

              </td>
            </tr>
          </table></td>
      </tr>
      <tr>
        <td valign="bottom" style="height:15%;" align="left">
        <div id="dbtnadd">
        <table cellpadding="0" cellspacing="0">
                <tr>
                    <td width="400"><input type="button" id="bClear" onClick="ClearData()" value="Clear"/>
          <input type="button" id="bAddToStock" onClick="SaveInventory()" value="Submit to Inventory"/></td>                    
                </tr>
            </table>
        </div>
        <div id="dbtndelete" style="display:none">
        <table cellpadding="0" cellspacing="0">
                <tr>
                    <td width="400">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td> Reason for removal(optional): <br /><input type="text" id="tReason" maxlength="200" style="width:200px" /></td>
                            <td  style="padding-left:10px">
                    <input type='button' value='Remove Selected' onclick='RemoveSerail()'/></td>
                            <td id="AdjustStock" style="display:none;padding-left:10px">
                            <input type='button' value='Adjust Quantity' onclick='AdjustQty()'/>
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
<script type="text/javascript">
    $(document).ready(function () {

        //Default Action
        $(".tab_content").hide(); //Hide all content
        //$("ul.tabs li:first").addClass("active").show(); //Activate first tab
        $("#lifirst").addClass("active").show();
        $(".tab_content:first").show(); //Show first tab content       
        //On Click Event
        $("ul.tabs li").click(function () {
            $("ul.tabs li").removeClass("active"); //Remove any "active" class
            $(this).addClass("active"); //Add "active" class to selected tab
            $(".tab_content").hide(); //Hide all tab content
            var activeTab = $(this).find("a").attr("href"); //Find the rel attribute value to identify the active tab + content
            $(activeTab).show(); // fadeIn(); //Fade in the active content
            if (activeTab == "#dadd") {
                $("#dbtnadd").show();
                $("#dbtndelete").hide();
            }
            else {
                activedeltab = "del";
                $("#dbtnadd").hide();
                $("#dbtndelete").show();
            }
            $("#AdjustStock").hide();
            if (activeTab == "#ddeletenp") {
                activedeltab = "delnp";
                $("#AdjustStock").show(); 
            }
            return false;
        });

    });
</script>
</asp:Content>
