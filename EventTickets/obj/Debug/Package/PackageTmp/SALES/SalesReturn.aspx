<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalesReturn.aspx.cs" Inherits="EventTickets.SALES.SalesReturn" %>
<asp:Content ID="C1" ContentPlaceHolderID="H" runat="server">
<script>
    function FillSalesDetails() {
        if ($("#M_tSalesId").val() != "") {

            $.ajax({
                type: "GET",
                url: '/ajax/ajax.aspx',
                data: 'Type=getSalesMasterDetails&SalesId=' + $("#M_tSalesId").val() + '&id=' + guidGenerator(),
                success: function (msg) {
                    if (msg == "") {
                        $("#td_sales_master").html("No data found!");
                        $("#td_sales").html("");
                        $("#td_payment").html("");
                        $("#td_return").hide();
                        //$("#tr_checkall").hide();
                        $("#td_print").hide();
                        $("#td_sales_serialno").hide();
                        return false;
                    }
                    if (msg != "") {

                        if (msg == "0") {
                            $("#td_sales_master").html("Already returned");
                            //$("#td_sales").html("");
                            //$("#td_payment").html("");
                            $("#td_return").hide();
                            //$("#tr_checkall").hide();
                            //$("#td_print").hide();
                            return false;
                        }

                        $("#td_return").show();
                        //$("#tr_checkall").show();
                        $("#td_print").show();
                        $("#td_sales_master").html(msg);
                        $("#td_sales_serialno").show();

                        $.ajax({
                            type: "GET",
                            url: '/ajax/ajax.aspx',
                            data: 'Type=getSalesDetails&Searchby=SalesId&SalesId=' + $("#M_tSalesId").val() + '&id=' + guidGenerator(),
                            success: function (msg) {
                                if (msg != "") {
                                    $("#td_sales").html(msg);
                                    
                                    $.ajax({
                                        type: "GET",
                                        url: '/ajax/ajax.aspx',
                                        data: 'Type=getPaymentDetails&Searchby=SalesId&SalesId=' + $("#M_tSalesId").val() + '&id=' + guidGenerator(),
                                        success: function (msg) {
                                            if (msg != "") {
                                                $("#td_payment").html(msg);
                                                document.getElementById('td_return').style.display = "";
                                                //SelectAll();
                                            }
                                            findTotal();
                                        }
                                    });

                                }
                                //document.getElementById('tr_checkall').style.display = '';
                                document.getElementById('td_print').style.display = '';
                                var tbl = document.getElementById('tbl_phystock');
                                if (tbl)
                                    document.getElementById('td_sales_serialno').style.display = '';
                                else
                                    document.getElementById('td_sales_serialno').style.display = 'none';
                            }
                        });


                    }
                }
            });

        }
        else if ($("#M_tSNo").val() != "") {

        $.ajax({
            type: "GET",
            url: '/ajax/ajax.aspx',
            data: 'Type=getSalesDetails&Searchby=SerialNo&SerialNo=' + $("#M_tSNo").val() + '&id=' + guidGenerator(),
            success: function (msg) {
                if (msg != "") {
                    if (msg == "M") {
                        $.ajax({
                            type: "GET",
                            url: '/ajax/ajax.aspx',
                            data: 'Type=getSalesMaster&SerialNo=' + $("#M_tSNo").val() + '&id=' + guidGenerator(),
                            success: function (msg) {
                                if (msg != "") {
                                    newwindow = window.open('', 'Sales', 'width=800,height=500');
                                    style = "<style>table.grid th {border:solid 1px #d3d3d3;font-size: 11px;font-weight: bold;text-align: left;color: #000000;position:relative;	cursor: default;padding:4px;height:25px;background-color:#9ab1c7;} table.grid td{background:#efefef;text-align:left;height:18px;border:solid 0px #d3d3d3;padding:1px; } table.grid{	background-color:#f0f0f0;border:solid 0 px #ffcfcc;}</style>";

                                    newwindow.document.write(style + msg);
                                    return;
                                }
                            }
                        });
                    }
                    else if (parseInt(msg) > 0) {

                        document.getElementById('M_tSalesId').value = msg;
                        document.getElementById('bSearch').click();
                        return false;
                    }
                    else {
                        $("#td_sales").html(msg);
                        $.ajax({
                            type: "GET",
                            url: '/ajax/ajax.aspx',
                            data: 'Type=getPaymentDetails&Searchby=SerialNo&SerialNo=' + $("#M_tSNo").val() + '&id=' + guidGenerator(),
                            success: function (msg) {
                                if (msg != "") {
                                    $("#td_payment").html(msg);
                                    document.getElementById('td_return').style.display = "";
                                    //SelectAll();
                                    //document.getElementById('tr_checkall').style.display = '';

                                    document.getElementById('td_print').style.display = '';
                                    var tbl = document.getElementById('tbl_phystock');
                                    if (tbl)
                                        document.getElementById('td_sales_serialno').style.display = '';
                                    else
                                        document.getElementById('td_sales_serialno').style.display = 'none';
                                }
                            }
                        });
                    }
                }
                else {
                    $("#td_sales_master").html("No data found!");
                    $("#td_sales").html("");
                    $("#td_payment").html("");
                    $("#td_return").hide();
                    //$("#tr_checkall").hide();
                    $("#td_print").hide();
                    $("#td_sales_serialno").hide();
                    return false;
                }
            }
        });
        }
    }

    function guidGenerator() {
        var S4 = function () {
            return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
        };
        return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
    }
    function SelectAll() {
        $('input:checkbox[disabled!=disabled]').each(function () {
            this.checked = true;
        });
        findTotal();
    }
    function UnSelectAll() {
        $('input:checkbox').each(function () {
            this.checked = false;
        });
        findTotal();
    }
    function findTotal() {
        total = 0.00;
        cash_amt = 0;
        credit_amt = 0;
        voucher_amt = 0;
        return_cash = 0;
        return_credit = 0;
        return_voucher = 0;
        balance = 0;
        try
        {
            cash_amt = parseFloat(document.getElementById('td_sales_amt_Cash').innerText);
        }
        catch (err)
        {
        }
        try {
            credit_amt = parseFloat(document.getElementById('td_sales_amt_Credit').innerText);
        }
        catch (err) {
        }
        try {
            voucher_amt = parseFloat(document.getElementById('td_sales_amt_Voucher').innerText);
        }
        catch (err) {

        }
        
        $('input[type=checkbox]').each(function () {
            if (this.checked) {
                amt = parseFloat(document.getElementById('td_amt_' + $(this).val()).innerText)
                 
                total += amt;
                if (cash_amt > 0) {
                    return_cash += amt;
                }
                else if (credit_amt > 0) {
                    return_credit += amt;
                }
                else if (voucher_amt > 0) {
                    return_voucher += amt;
                }
            }
        });

        var table = document.getElementById("tbl_cards");
        if (table) 
        {
            for (var i = 1; i < table.rows.length - 1; i++) 
            {
                row = table.rows[i]; 
                var SalesDetailId = row.cells[0].getElementsByTagName('input')[0].value;
                var price = parseFloat(row.cells[2].innerText);
                if (document.getElementById('t_qty_return_' + SalesDetailId + '_' + i)) 
                {
                    var qty = parseFloat(document.getElementById('t_qty_return_' + SalesDetailId + '_' + i).value);
                    if (qty > 0) 
                    {
                        amt = price * qty;
                        total += amt;
                        if (cash_amt > 0) 
                        {
                            return_cash += amt;
                        }
                        else if (credit_amt > 0) 
                        {
                            return_credit += amt;
                        }
                        else if (voucher_amt > 0) {
                            return_voucher += amt;
                        }
                    }
                }
            }
        }
         
        if (cash_amt > 0)
            document.getElementById('return_amt_Cash').value = return_cash.toFixed(2);
        if (credit_amt > 0)
            document.getElementById('return_amt_Credit').value = return_credit.toFixed(2);
        if (voucher_amt > 0)
            document.getElementById('return_amt_Voucher').value = return_voucher.toFixed(2);
        document.getElementById('amt_return').innerHTML = total.toFixed(2);
    }

    function Print() {
        if ($("#M_tSalesId").val() != "") {
            window.open("/Reports/Reports.aspx?ReportName=Receipt&DataSource=Receipt&SalesId=" + $("#M_tSalesId").val(), "Receipt", "statusbar=no");
            window.open("/Reports/Reports.aspx?ReportName=Receipt&DataSource=Receipt&Dup=yes&SalesId=" + $("#M_tSalesId").val(), "ReceiptCopy", "statusbar=no");
            return false;
        }
    }
    function submitReturn() {
        SalesMasterId = $("#hSalesMasterId").val();
        ProductDetailId = ($('input:checked').map(function () { return this.value; }).get().join(','));
        strSalesDetailId = "0";
        strExpiryDate = "''";
        strReturnQty = "0";

        var table = document.getElementById("tbl_cards");
        if (table) {
            for (var i = 1; i < table.rows.length - 1; i++) {
                row = table.rows[i];
                var SalesDetailId = row.cells[0].getElementsByTagName('input')[0].value;
                var ExpiryDate = row.cells[4].innerText;
                if (document.getElementById('t_qty_return_' + SalesDetailId + '_' + i)) {
                    var ReturnQty = parseFloat(document.getElementById('t_qty_return_' + SalesDetailId + '_' + i).value);
                    if (ReturnQty > 0) {
                        
                        strSalesDetailId += "," + SalesDetailId;
                        strExpiryDate += ",'" + ExpiryDate + "'";
                        strReturnQty += "," + ReturnQty;
                    }
                }
            }
        }

         
        Cash = 0;
        Credit = 0;
        Voucher = 0;
        try {
            Cash = parseFloat(document.getElementById('return_amt_Cash').value);
        }
        catch (err) {
        }
        try {
            Credit = parseFloat(document.getElementById('return_amt_Credit').value);
        }
        catch (err) {
        }
        try {
            Voucher = parseFloat(document.getElementById('return_amt_Voucher').value);
        }
        catch (err) {
        }

        if (Cash + Credit + Voucher <= 0) {
            alert("Return amount should be greater than zero");
            return;
        }

        $.ajax({
            type: "GET",
            url: '/ajax/ajax.aspx',
            data: 'Type=submitReturn&SalesMasterId=' + SalesMasterId + '&ProductDetailId=' + ProductDetailId + '&SalesDetailId=' + strSalesDetailId + '&ExpiryDate=' + strExpiryDate + '&ReturnQty=' + strReturnQty + '&Cash=' + Cash + '&Credit=' + Credit + '&Voucher=' + Voucher + '&id=' + guidGenerator(),
            success: function (msg) {
                if (msg == "0") {
                    alert("Error while processing sales return!");
                }
                else if (msg == "-1") {
                    alert("Credit card refund cannot process");
                }
                else if (msg == "-2") {
                    alert("Credit card payment information not found");
                }
                else {
                    alert("Sales return details saved successfully");
                    FillSalesDetails();
                }
            }
        });

    }

    function ValidateQty(SalesDetailId) {
        var balanceQty = parseInt($("#hQty_"+SalesDetailId).val());
        var returnQty = parseInt($("#t_qty_return_"+SalesDetailId).val());
        if (returnQty > balanceQty) {
            alert("Quantity return should not exceed " + balanceQty);
            $("#t_qty_return_" + SalesDetailId).val(balanceQty);
            findTotal();
        }
        else
            findTotal();
    }

    function CheckSNo() {

        SalesMasterId = $("#hSalesMasterId").val();
        SerialNo = document.getElementById('M_tSerialNo').value;


        $.ajax({
            type: "GET",
            url: '/ajax/ajax.aspx',
            data: 'Type=CheckSNo&SalesMasterId=' + SalesMasterId + '&SerialNo=' + SerialNo + '&id=' + guidGenerator(),
            success: function (msg) {
                if (msg == "") {
                    alert("Invalid Serial No.");
                }
                else {

                    document.getElementById('tr_' + msg).style.color = 'green';
                    document.getElementById('chk_' + msg).checked = true;
                    document.getElementById('td_status_' + msg).innerText = "Marked to be returned";
                    document.getElementById('td_status_' + msg).style.color = 'green';
                    findTotal();
                    document.getElementById('M_tSerialNo').value = "";
                }
            }
        });
    }

    function BlockKeys(keyCode)
    {
        if (!(keyCode == 13 || keyCode == 8 || keyCode == 9 || keyCode == 16 || keyCode == 17 || keyCode == 18 || keyCode == 37 || keyCode == 38 || keyCode == 39 || keyCode == 40 || keyCode == 45 || keyCode == 46)) {
            //keyCode = 0;
            //return false;
        }
    }

</script>
</asp:Content>
<asp:Content ID="C2" ContentPlaceHolderID="M" runat="server">
     
    <table style="padding:10px; width:100%; height:370px;">
        <tr>
            <td style="width:100%; height:100%;"> 
                <table style="width:100%; height:100%; text-align:center; vertical-align:middle;" cellpadding="0"  cellspacing="0" border="0">
                    <tr>
                        <td align="left" width="100%"  valign="top" style="height:100%; ">
                            <div class="box-shadow" style="min-height:445px; border:1px solid grey; ">
                                <table style="text-align:left; vertical-align:top; width:100%; height:100%" cellpadding="5"  cellspacing="0" border="0" >
                                    <tr>
                                        <td colspan="2" align="center" width="100%"  style="font-size:22px; background-color:#cccccc; font-weight:bold; vertical-align:top; color:Black; height:30px;">Sales Return</td>
                                    </tr>
                                    
                                    <tr>
                                        <td style="height:60px; padding:10px; width:70%">
                                            <table width="100%" style="height:100%; border:1px solid grey;">
                                                <tr>
                                                    <td style="height:60px; width:70%;">
                                                        Enter Transaction ID: <asp:TextBox ID="tSalesId" runat="server" onkeydown="Javascript: {if (event.keyCode==13) FillSalesDetails(); document.getElementById('M_tSNo').value='';}"></asp:TextBox> 
                                                        OR Scan in Serial #: <asp:TextBox ID="tSNo" runat="server" onkeydown="Javascript: {if (event.keyCode==13) FillSalesDetails(); document.getElementById('M_tSalesId').value='';}"></asp:TextBox> 
                                                        &nbsp;&nbsp;&nbsp;<input type="button" ID="bSearch" value="Search" Class="btn"  onclick="FillSalesDetails();" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                                   
                                    </tr>
                                    <tr>
                                        <td colspan="2" id="td_sales_master"></td>
                                    </tr>
                                    <!--
                                    <tr id="tr_checkall" style="display:none;">
                                        <td colspan="2" style="height:20px;">
                                            <a href="javascript:void(0)" onclick="SelectAll()">Select All</a> <a href="javascript:void(0)" onclick="UnSelectAll()">Unselect All</a>
                                        </td>
                                    </tr>
                                    -->
                                    <tr>
                                        <td colspan="2" id="td_sales_serialno" style="display:none; font-size:16px; font-weight:bold; color:Maroon;" oncopy="return false" onpaste="return false" oncut="return false" onkeydown="return BlockKeys(event.keyCode);" >Scan in the ticket serial number to be returned: <input type="text" id="tSerialNo" runat="server" onkeydown="Javascript: {if (event.keyCode==13) CheckSNo(); }" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" id="td_sales">
                                                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="td_payment" style="width:50%">
                                                                            
                                        </td>
                                        <td id="td_return" style="display:none;">
                                            <table width="100%">
                                                <tr>
                                                    <td align="right">Amount Return: </td>
                                                    <td id="amt_return">0.00</td><td style="width:50%;" align="right">
                                                        <input type="button" ID="bReturn" value="Return" Class="btn" onclick="submitReturn();" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="right" id="td_print" style="display:none; text-align:right;">
                                            <input type="button" runat="server" class="btn" value="Print Receipt" id="PrintReceipt" onclick="Print();"  />
                                        </td>
                                    </tr>
                                    <tr><td></td></tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:Button ID="btnDisableEnter" runat="server" Text="" OnClientClick="return false;" style="display:none;"/> 
     
    <style type="text/css"> 
        div{behavior: url(/Scripts/PIE.htc);}
        .btn{behavior: url(/Scripts/PIE.htc);}
    </style>
    
</asp:Content>
