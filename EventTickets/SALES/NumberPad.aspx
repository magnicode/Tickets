<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NumberPad.aspx.cs" Inherits="EventTickets.Sales.NumberPad" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head runat="server" id="h1">
<title>On Screen Number Pad Using JavaScript</title>
<script type="text/javascript">
    function number_write(x) {
        var text_box = document.getElementById("number");
        if (x == ".") {
            if (text_box.value.indexOf(".") > 0)
                return false;
        }
        if (x >= 0 && x <= 9) {
            if (isNaN(text_box.value))
                text_box.value = 0;
            if (text_box.value.indexOf(".") > 0)
                text_box.value = (text_box.value) + x;
            else
                text_box.value = (text_box.value * 10) + x;
        }
        else if(x == '.')
            text_box.value = (text_box.value) + x;
    }
    function number_clear() {
        document.getElementById("number").value = 0;
    }
    function number_c() {
        var text_box = document.getElementById("number");
        var num = text_box.value;
        var num1 = num % 10;
        num -= num1;
        num /= 10;
        text_box.value = num;
    }

    function ValidateQty() {
        if (document.getElementById("number").value == "" || document.getElementById("number").value == "0") {
            alert("Please enter quantity");
            return false;
        }
        if (isNaN(document.getElementById("number").value)) {
            alert("Please enter a valid quantity");
            return false;
        }
        <% if ("" + Request.QueryString["type"] == "MTA"){ %>
            window.opener.document.getElementById('<%= Request.QueryString["idtbox"] %>').value = document.getElementById("number").value;
            window.close();
            return false;
        <% } %>
        return true;
    }
</script>
<link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
<style type="text/css">
body   
{
    padding:0px;
    margin:0px;
    background-color:#f5f5f5;
    background-image:none;    
}
.main_panel
{
width:270px;
height:470px;
background-color:#f5f5f5;
border-top-right-radius:20px;
border-top-left-radius:20px;
border-bottom-right-radius:20px;
border-bottom-left-radius:20px;
padding:20px;
}
.number_button
{
width:70px;
height:70px;
margin:10px;
float:left;
background-color:#cccccc;
border-top-right-radius:20px;
border-top-left-radius:20px;
border-bottom-right-radius:20px;
border-bottom-left-radius:20px;
font-size:36px;
text-align:center;
cursor:pointer;
color:#000;
}
.number_button:hover
{
background-color:#66FF66;
}
.text_box
{
width:250px; 
height:30px;
font-size:24px;
text-align:right;
}
</style>
</head>
<body>
<form id="f" runat="server">
<div class="main_panel">
<br /> 
<center><input class="text_box" runat="server" type="text" id="number" name="num" /></center>
<br /><br />
<div class="number_button" onclick="number_write(7);">7</div>
<div class="number_button" onclick="number_write(8);">8</div>
<div class="number_button" onclick="number_write(9);">9</div>
<div class="number_button" onclick="number_write(4);">4</div>
<div class="number_button" onclick="number_write(5);">5</div>
<div class="number_button" onclick="number_write(6);">6</div>
<div class="number_button" onclick="number_write(1);">1</div>
<div class="number_button" onclick="number_write(2);">2</div>
<div class="number_button" onclick="number_write(3);">3</div>
<div class="number_button" onclick="number_clear();">Clr</div>
<div class="number_button" onclick="number_write(0);">0</div>
<% if(""+Request.QueryString["type"] != "paymentoptions") { %>
<div class="number_button" onclick="number_c();">C</div>
<% } else { %>
<div class="number_button" onclick="number_write('.');">.</div>
<% } %>
<div style="width:100%;text-align:right;padding-top:10px">
    <input type="button" onclick="window.close()" value="Cancel" /> 
    <asp:Button runat="server" ID="bOk" Text="OK" CssClass="btn"  OnClientClick="return ValidateQty()"
        onclick="bOk_Click" />
</div>
</div>
<div style="display:none" id="dmsg" runat="server"></div>
</form>
</body>
</html>