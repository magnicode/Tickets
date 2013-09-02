<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalesReturnNew.aspx.cs" Inherits="EventTickets.SALES.SalesReturnNew" %>
<asp:Content ID="C1" ContentPlaceHolderID="H" runat="server">
<!-- Mimic Internet Explorer 7 -->
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" >
<script src="../Scripts/jquery.datepick.js" type="text/javascript"></script>
<link href="../Styles/jquery.datepick.css" rel="stylesheet" type="text/css" />
<script>
    function FillSalesDetails() {
        if ($("#M_tSalesId").val() != "") {

            $.ajax({
                type: "GET",
                url: '/ajax/ajax.aspx',
                data: 'Type=getSalesMasterDetails&SalesId=' + $("#M_tSalesId").val() + '&id=' + guidGenerator(),
                success: function (msg) {
                    if (msg == "") {

                        /////////

                        $.ajax({
                            type: "GET",
                            url: '/ajax/ajax.aspx',
                            data: 'Type=getSalesDetailsNew&Searchby=SerialNo&SerialNo=' + $("#M_tSalesId").val() + '&id=' + guidGenerator(),
                            success: function (msg) {
                                if (msg != "") {
                                    if (msg == "M") {
                                        $.ajax({
                                            type: "GET",
                                            url: '/ajax/ajax.aspx',
                                            data: 'Type=getSalesMaster&SerialNo=' + $("#M_tSalesId").val() + '&id=' + guidGenerator(),
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
                                     
                                }
                                else {
                                    $("#td_sales_master").html("No data found!");
                                    $("#td_sales").html("");
                                    $("#td_payment").html("");
                                    $("#td_return").hide();
                                    $("#td_print").hide();
                                    $("#td_sales_serialno").hide();
                                    return false;
                                }
                            }
                        });

                        /////////

                        $("#td_sales_master").html("No data found!");
                        $("#td_sales").html("");
                        $("#td_payment").html("");
                        $("#td_return").hide();
                        $("#td_print").hide();
                        $("#div_scan_now").hide();
                        $("#td_sales_serialno").hide();
                        return false;
                    }
                    if (msg != "") {

                        if (msg == "0") {
                            $("#hStatus").val("0");
                            return false;
                        }

                        $("#td_return").show();
                        $("#td_print").show();
                        $("#td_sales_master").html(msg);
                        $.ajax({
                            type: "GET",
                            url: '/ajax/ajax.aspx',
                            data: 'Type=getSalesDetailsNew&Searchby=SalesId&SalesId=' + $("#M_tSalesId").val() + '&id=' + guidGenerator(),
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
                                            }
                                            findTotal();
                                            $(".datepick").datepick(
                                                { minDate: "-1Y",
                                                    onSelect: function () {
                                                        CheckExpDate(this,0);
                                                    }
                                                }
                                            );
                                        }
                                    });

                                    $(document).ready(function () {
                                    });
                                }
                                document.getElementById('td_print').style.display = '';
                            }
                        });
                    }
                }
            });

        }

        else if ($("#M_tSNo").val() != "") {

        
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
    function findTotal(chk, type, rowIndex) {

        
        if (!chk) return;

        if (type == 0 && chk.checked) {
            alert("Please fill in the expiry date of the ticket to be returned");
            chk.checked = false;
            return false;
        }
        else if (type == 0 && chk.checked == false) {
            chk.parentNode.parentNode.style.color = 'black';
            chk.checked = false;
            document.getElementById('txt_expdate_' + rowIndex).value = "";
            document.getElementById('td_status_' + rowIndex).innerText = "";
            document.getElementById('td_status_' + rowIndex).style.color = 'black';
            document.getElementById('td_amount_returned_' + rowIndex).innerText = "";
        }
        else if (type == 1 && chk.checked) {
            alert("Please scan in the serial number of the ticket to be returned");
            chk.checked = false;
            return false;
        }
        else if (type == 1 && chk.checked == false) {
            chk.parentNode.parentNode.style.color = 'black';
            chk.checked = false;
            document.getElementById('td_status_' + rowIndex).innerText = "";
            document.getElementById('td_status_' + rowIndex).style.color = 'black';
            document.getElementById('td_amount_returned_' + rowIndex).innerText = "";
        }
        else if (type == 2 && chk.checked) {
            if (!confirm("Have you verified that this ticket is unused?")) {
                chk.checked = false;
                return false;
            } else {
                chk.parentNode.parentNode.style.color = 'green';
                chk.checked = true;
                document.getElementById('td_status_' + rowIndex).innerText = "Marked to be returned";
                document.getElementById('td_status_' + rowIndex).style.color = 'green';
                document.getElementById('td_amount_returned_' + rowIndex).innerText = document.getElementById('td_amt_' + rowIndex).innerText;
            }
        } else if (type == 2 && chk.checked == false) {
            chk.parentNode.parentNode.style.color = 'black';
            chk.checked = false;
            document.getElementById('td_status_' + rowIndex).innerText = "";
            document.getElementById('td_status_' + rowIndex).style.color = 'black';
            document.getElementById('td_amount_returned_' + rowIndex).innerText = "";
        }

        document.getElementById('amt_return').innerHTML = "0.00";
        document.getElementById('td_total_amount_returned').innerHTML = "0.00";

        total = 0.00;
        cash_amt = 0;
        credit_amt = 0;
        voucher_amt = 0;
        return_cash = 0;
        return_credit = 0;
        return_voucher = 0;
        balance = 0;
        try {
            if(document.getElementById('td_sales_amt_Cash').innerText!="")
                cash_amt = parseFloat(document.getElementById('td_sales_amt_Cash').innerText);
        }
        catch (err)
        {
        }
        try {
            if (document.getElementById('td_sales_amt_Credit').innerText!="")
                credit_amt = parseFloat(document.getElementById('td_sales_amt_Credit').innerText);
        }
        catch (err) {
        }
        try {
            if(document.getElementById('td_sales_amt_Voucher').innerText!="")
                voucher_amt = parseFloat(document.getElementById('td_sales_amt_Voucher').innerText);
        }
        catch (err) {

        }

        
        $('input[type=checkbox]').each(function () {
            if (this.checked == true && this.disabled == false) {

                i = this.parentNode.getElementsByTagName('input')[0].value;
                amt = parseFloat(document.getElementById('td_amt_' + i).innerText)

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

        if (cash_amt > 0)
            document.getElementById('return_amt_Cash').value = return_cash.toFixed(2);
        if (credit_amt > 0)
            document.getElementById('return_amt_Credit').value = return_credit.toFixed(2);
        if (voucher_amt > 0)
            document.getElementById('return_amt_Voucher').value = return_voucher.toFixed(2);
        document.getElementById('amt_return').innerHTML = total.toFixed(2);
        document.getElementById('td_total_amount_returned').innerHTML = total.toFixed(2);
    }

    function getTotal() {
        total = 0.00;
        Cash = 0;
        Credit = 0;
        Voucher = 0;
        try {
            if (document.getElementById('return_amt_Cash').value != "")
                Cash = parseFloat(document.getElementById('return_amt_Cash').value);
        }
        catch (err) {
        }
        try {
            if (document.getElementById('return_amt_Credit').value != "")
                Credit = parseFloat(document.getElementById('return_amt_Credit').value);
        }
        catch (err) {
        }
        try {
            if (document.getElementById('return_amt_Voucher').value != "")
                Voucher = parseFloat(document.getElementById('return_amt_Voucher').value);
        }
        catch (err) {
        }
         
        total = Cash + Credit + Voucher;
        document.getElementById('amt_return').innerHTML = total.toFixed(2);
    }

    function Print() {
        if ($("#M_tSalesId").val() != "") {
            window.open("/Reports/Reports.aspx?ReportName=Receipt&DataSource=Receipt&SalesId=" + $("#M_tSalesId").val(), "Receipt", "statusbar=no");
            window.open("/Reports/Reports.aspx?ReportName=Receipt&DataSource=Receipt&Dup=yes&SalesId=" + $("#M_tSalesId").val(), "ReceiptCopy", "statusbar=no");
            return false;
        }
    }

    function PrintPDF() {
        if ($("#M_tSalesId").val() != "") {
            window.open("Viewpdf.aspx?SalesId=" + $("#M_tSalesId").val(), "SDReceipt", "status=no,resizable=yes,menubar=yes");
            return false;
        }
    }

    function submitReturn() {

        if ($("#span_status").html() != "") {
            alert("This transaction is already returned");
            return false;
        }
        
        /*
        var table = document.getElementById("tbl_phystock");
        if (table) {
            for (var i = 1; i < table.rows.length - 1; i++) {
                if(getElementById('chk_'+(i-1)).checked)
                {
                    
                }
            }
        }
        */

        
        SalesMasterId = $("#hSalesMasterId").val();
        strId = "";
        strExpiryDate = "";
        strType = "";
        SDCount = 0;
        var table = document.getElementById("tbl_phystock");
        if (table) {
            for (var i = 1; i < table.rows.length - 1; i++) {
                row = table.rows[i];
                chk = document.getElementById('chk_' + (i - 1));
                type = document.getElementById('product_type_' + (i - 1)).value;

                if (chk && chk.checked) {
                    if (type == 0) {
                        var ProductDetailId = document.getElementById('ProductDetailId_' + (i - 1)).value;
                        strId += (strId != "" ? "," : "") + ProductDetailId;
                        //strExpiryDate += (strExpiryDate != "" ? ",'" : "'") + ExpiryDate + "'";
                    }
                    else if (type == 1) {
                        var ProductDetailId = document.getElementById('ProductDetailId_' + (i - 1)).value;
                        strId += (strId != "" ? "," : "") + ProductDetailId;
                    }
                    else 
                    {
                        var SalesDetailId = document.getElementById('SalesDetailId_' + (i - 1)).value;
                        strId += (strId != "" ? "," : "") + SalesDetailId;
                        SDCount++;
                    }
                    strType += (strType != "" ? "," : "") + type;
                }
            }
        }

        if (SDCount > 0 && SDCount < parseInt(document.getElementById('SDCount').value)) {
            alert("Partial return of Smart Destination tickets are not allowed");
            return false;
        }

        
        Cash = 0;
        Credit = 0;
        Voucher = 0;
        try {
            if(document.getElementById('return_amt_Cash').value !="")
                Cash = parseFloat(document.getElementById('return_amt_Cash').value);
        }
        catch (err) {
        }
        try {
            if (document.getElementById('return_amt_Credit').value != "")
                Credit = parseFloat(document.getElementById('return_amt_Credit').value);
        }
        catch (err) {
        }
        try {
            if (document.getElementById('return_amt_Voucher').value != "")
                Voucher = parseFloat(document.getElementById('return_amt_Voucher').value);
        }
        catch (err) {
        }

        if (document.getElementById('td_total_amount_returned').innerText=="" ||  parseFloat(document.getElementById('td_total_amount_returned').innerText) <= 0) {
            alert("You have not selected any tickets to be returned");
            return;
        }

        if (parseFloat(document.getElementById('td_total_amount_returned').innerText) != parseFloat(document.getElementById('amt_return').innerText)) {
            alert("The return amount should match with the amound to be returned");
            return;
        }

        if (!confirm("Make sure you have received all tickets back and all tickets are unused before continuing with this refund.  \nDo you wish to continue with the refund?")) return false;

        $.ajax({
            type: "GET",
            url: '/ajax/ajax.aspx',
            data: 'Type=submitReturnNew&ProductType=' + strType + '&SalesMasterId=' + SalesMasterId + '&DetailId=' + strId + '&ExpiryDate=' + strExpiryDate + '&Cash=' + Cash + '&Credit=' + Credit + '&Voucher=' + Voucher + '&id=' + guidGenerator(),
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

                    var table = document.getElementById("tbl_phystock");
                    if (table) {
                        for (var i = 1; i < table.rows.length - 1; i++) {
                            row = table.rows[i];
                            if (msg == row.cells[0].getElementsByTagName('input')[2].value) {
                                document.getElementById('tr_' + (i - 1)).style.color = 'green';
                                document.getElementById('chk_' + (i - 1)).checked = true;
                                document.getElementById('td_status_' + (i - 1)).innerText = "Marked to be returned";
                                document.getElementById('td_status_' + (i - 1)).style.color = 'green';
                                findTotal(document.getElementById('chk_' + (i - 1)), -1, i - 1);
                                document.getElementById('M_tSerialNo').value = "";
                                document.getElementById('td_amount_returned_' + (i - 1)).innerText = document.getElementById('td_amt_' + (i - 1)).innerText;
                            }  
                        }
                    }


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

    function CheckExpDate(txt, rowIndex) {
        rowIndex = txt.parentNode.parentNode.rowIndex-1;
        if (txt.value == txt.parentNode.parentNode.cells[2].innerText) {
            document.getElementById('tr_' + (rowIndex)).style.color = 'green';
            document.getElementById('chk_' + (rowIndex)).checked = true;
            document.getElementById('td_status_' + (rowIndex)).innerText = "Marked to be returned";
            document.getElementById('td_status_' + (rowIndex)).style.color = 'green';
            findTotal(document.getElementById('chk_' + (rowIndex)), -1, rowIndex);
            document.getElementById('td_amount_returned_' + (rowIndex)).innerText = document.getElementById('td_amt_' + (rowIndex)).innerText;
            

        } else {
            document.getElementById('tr_' + (rowIndex)).style.color = 'black';
            document.getElementById('chk_' + (rowIndex)).checked = false;
            document.getElementById('td_status_' + (rowIndex)).innerText = "";
            document.getElementById('td_amount_returned_' + (rowIndex)).innerText = "";
            findTotal(document.getElementById('chk_' + (rowIndex)), -1, rowIndex);
        }
    }
 
</script>
</asp:Content>
<asp:Content ID="C2" ContentPlaceHolderID="M" runat="server">
    <div style="position:relative"> 
    <table style="padding:10px; width:100%; height:370px;">
        <tr>
            <td style="width:100%; height:100%;"> 
                <table style="width:100%; height:100%; text-align:center; vertical-align:middle;" cellpadding="0"  cellspacing="0" border="0">
                    <tr>
                        <td align="left" width="100%"  valign="top" style="height:100%; ">
                            <div class="box-shadow" style="min-height:445px; border:1px solid grey; ">
                                <table style="text-align:left; vertical-align:top; width:100%; height:100%" cellpadding="5"  cellspacing="0" border="0" >
                                    <tr>
                                        <td colspan="2" align="center" width="100%"  style="font-size:22px; background-color:#cccccc; font-weight:bold; vertical-align:top; color:Black; height:30px;">Sales Return New</td>
                                    </tr>
                                    <tr>
                                        <td style="height:80px; padding:10px; max-width:360px;" >
                                            <fieldset  style="height:80px;box-shadow: 0px 0px 0px #000; background-color:InfoBackground;z-index:0; margin:0; padding:0"><legend style="font-weight:bold; color:Black;">Find the Transaction Id to be returned: </legend>
                                            <table width="100%" style="height:60px;" border="0">
                                                <tr>
                                                    <td style="text-align:right; width:110px; vertical-align:middle;">
                                                        Enter Transaction ID: 
                                                        <b>OR</b> Scan in Serial #: 
                                                    </td>
                                                    <td style="width:100px">
                                                        <asp:TextBox ID="tSalesId" style="width:80px" runat="server" onkeydown="Javascript: {if (event.keyCode==13) FillSalesDetails(); }"></asp:TextBox> 
                                                    </td>
                                                    <td style="width:80px" >
                                                        <input type="button" ID="bSearch" value="Search" Class="btn"  onclick="FillSalesDetails();" />
                                                    </td>
                                                </tr>
                                            </table>
                                            </fieldset>
                                        </td>
                                        <td  style="height:80px; padding:10px; min-width:500px;;">
                                            <table width="100%" border="0">
                                                <tr>
                                                    <td valign="middle" align="right" style="text-align:center;vertical-align:middle;" >
                                                        <div id="div_scan_now" style="display:none;">
                                                            <input type="button" ID="bScan" value="Scan ticket now" Class="btn" style="color:Green; font-size:22px; width:250px; height:50px;"  onclick="document.getElementById('td_sales_serialno').style.display=''; document.getElementById('div_scan_now').style.display='none'; document.getElementById('M_tSerialNo').focus();" />
                                                        </div>
                                                        <div id="td_sales_serialno" style="display:none; height:25px; font-size:16px; font-weight:bold; color:Maroon; text-align:right;" oncopy="return false" onpaste="return false" oncut="return false" onkeydown="return BlockKeys(event.keyCode);" >Scan in the ticket serial number to be returned: <input type="text" id="tSerialNo" onblur="document.getElementById('td_sales_serialno').style.display='none'; document.getElementById('div_scan_now').style.display='';" runat="server" onkeydown="Javascript: {if (event.keyCode==13) CheckSNo(); }" /></div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" id="td_sales_master" style="font-size:16px;"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" id="td_sales">
                                                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"  id="td_return" style="display:none;">
                                            <table width="100%" border="0">
                                                <tr>
                                                    <td id="td_print" style="display:none; width:150px;">
                                                        <div><input type="button" runat="server" class="btn" style="width:140px; font-size:14px;" value="Print Receipt" id="PrintReceipt" onclick="Print();"  /></div>
                                                        <br />
                                                        <div id="div_printpdf" style="display:none; float:left;"><input type="button" runat="server" class="btn" style="width:140px; font-size:14px;" value="Print Ticket PDF" id="PrintPDF" onclick="PrintPDF();"  /></div>
                                                    </td>
                                                    <td>
                                                        <table width="100%" style="border:1px solid black;">
                                                            <tr><td colspan="2" style="font-size:16px; font-weight:bold;">Payment Details</td></tr>
                                                            <tr>
                                                                <td id="td_payment" style="width:60%">
                                                                            
                                                                </td>
                                                                <td style="width:20%;" align="right">
                                                                    <input type="button" id="bReturn" style="color:Maroon;" value="Process Return" class="btn" onclick="return submitReturn();" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
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
    <input type="hidden" id="hStatus" value="" /> 
    <style type="text/css"> 
        div{behavior: url(/Scripts/PIE.htc);}
        .btn{behavior: url(/Scripts/PIE.htc);}
        fieldset{behavior: url(/Scripts/PIE.htc);}
    </style>
    </div>
</asp:Content>
