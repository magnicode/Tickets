<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="calc.aspx.cs" Inherits="EventTickets.SALES.calc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.8.0/jquery.min.js"></script>
    <style type="text/css">
        body   
        {
            padding:0px;
            margin:0px;
            background-color:#fff;
            background-image:none;
            color:#000;    
            
        }
    </style>
    <script>
        function setBalance() {
            var cash_receivable = parseFloat(document.getElementById('td_cash_receivable').innerText);
            var cash_received = parseFloat(document.getElementById('txt_cash_received').value);
            document.getElementById('td_cash_balance').innerText = cash_received - cash_receivable;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="padding:5px; height:250px">
    <table width="100%"   cellpadding="2" cellspacing="2" style="border:1px solid black; height:240px">
        <tr><td colspan="2"></td></tr>
        <tr>
            <td style="width:50%;font-size:14px; text-align:right;" >Cash Receivable: </td>
            <td runat="server" id="td_cash_receivable" style="font-size:16px; font-weight:bold; text-align:left;">$0.00</td>
        </tr>
        <tr>
            <td style = "font-size:14px;  text-align:right;">Cash Received: </td>
            <td><input type="text" runat="server" id="txt_cash_received" value="" style="font-size:14px; font-weight:bold; text-align:left; width:80px;" onchange="setBalance();" /></td>
        </tr>
        <tr>
            <td style = "font-size:14px;  text-align:right;">Balance to Pay: </td>
            <td runat="server" id="td_cash_balance" style="font-size:40px; font-weight:bold; color:Maroon; text-align:left;">$0.00</td>
        </tr>
        <tr><td colspan="2"></td></tr>
    </table>
    </div>
    </form>
</body>
</html>

<style type="text/css"> 
    div{behavior: url(/Scripts/PIE.htc);}
    .btn{behavior: url(/Scripts/PIE.htc);}
</style> 