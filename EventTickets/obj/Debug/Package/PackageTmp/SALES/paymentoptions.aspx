<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="paymentoptions.aspx.cs" Inherits="EventTickets.SALES.WebForm1" %>
<%@ Register Src="../payment.ascx" TagName="pay" TagPrefix="p" %>
<asp:Content ID="Content1" ContentPlaceHolderID="H" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="M" runat="server">
<style>
    .Amt
    {
        font-size:16px;
        color:Black;
        font-weight:bold;	
    }
</style>
<script type="text/javascript">
    function OpenNumpad(paymenttype) {

        if (paymenttype == "cash") {

            $.ajax({
                type: "GET",
                url: '/ajax/ajax.aspx',
                data: 'Type=AddCashPayment&tid=' + guidGenerator(),
                success: function (msg) {
                    location.href = "paymentoptions.aspx";
                }
            });
        }
        else if (paymenttype == "credit") {
            var win = window.open('credit.aspx', 'npad', 'width=500,height=300,statusbar=no');
            win.focus();
        }
        else if (paymenttype == "voucher") {
            var win = window.open('voucher.aspx', 'npad', 'width=500,height=300,statusbar=no');
            win.focus();
        }

    /*
        $.ajax({
            type: "GET",
            url: '/ajax/ajax.aspx',
            data: 'Type=ValidatePayment&paymenttype=' + paymenttype,
            success: function (msg) {
                if (msg == "") {
                    var win = window.open("NumberPad.aspx?type=paymentoptions&paymenttype=" + paymenttype + "&BalanceAmt=" + document.getElementById('M_payment_lblAmtRem').innerText, 'npad', 'width=312,height=530,statusbar=no');
                    win.focus();  
                }
                else if (msg == "1") {
                    alert("Payment option already added.");  
                }
                else if (msg == "2") {
                    alert("No remaining amount.");  
                }
            }
        });
        */
        return false;
    }
    function guidGenerator() {
        var S4 = function () {
            return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
        };
        return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
    }
</script>
<table style="padding:10px; width:100%; height:370px;">
<tr>
<td style="width:100%; height:100%;">
<table style="width:100%; height:100%; text-align:center; vertical-align:middle;" cellpadding="0"  cellspacing="0" border="0">
<tr>
    <td align="center" width="70%"  valign="top" style="height:100%; ">
        <div class="box-shadow" style="min-height:445px; border:1px solid grey;">
            <table style="text-align:center; vertical-align:top; width:100%; height:100%" cellpadding="0"  cellspacing="0" border="0" >
                <tr>
                    <td align="center" width="70%"  style="font-size:22px; background-color:#cccccc; font-weight:bold; vertical-align:top; color:Black; height:30px;">Payment Options</td>
                </tr>
                <tr><td style="height:20px;"></td></tr>
                <tr>
                    <td style="height:75px;">
                        <asp:Button ID="bCash"  Text="Cash" runat="server" Font-Bold="true" 
                            Font-Size="50px" ForeColor="#317c31" Width="375px" Height="70px" 
                            OnClientClick="return OpenNumpad('cash');" />
                    </td>
                </tr>
                <tr><td style="height:20px;"></td></tr>
                <tr>
                    <td style="height:75px;">
                        <asp:Button ID="bCredit" Text="Credit" runat="server" Font-Bold="true" 
                            Font-Size="50px" ForeColor="#902154"  Width="375px" Height="70px" 
                            OnClientClick="return OpenNumpad('credit');" />
                    </td>
                </tr>
                <tr><td style="height:20px;"></td></tr>
                <tr>
                    <td style="height:75px;">
                        <asp:Button ID="bVoucher" Text="Voucher" runat="server" Font-Bold="true" 
                            Font-Size="50px" ForeColor="#902154"  Width="375px" Height="70px" 
                            OnClientClick="return OpenNumpad('voucher');" />
                    </td>
                </tr>
                <tr><td></td></tr>
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
<style type="text/css"> 
    div{behavior: url(/Scripts/PIE.htc);}
    .btn{behavior: url(/Scripts/PIE.htc);}
</style> 

</asp:Content>

