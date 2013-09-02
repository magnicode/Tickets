<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="credit.aspx.cs" Inherits="EventTickets.SALES.credit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Credit Card Information</title>
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
        function appendSpace() {

            $.ajax({
                type: "GET",
                url: '/ajax/ajax.aspx',
                data: 'Type=ParseCreditCardInfo&scan=' + encodeURI($("#tCardNo").val()) + '&id=' + guidGenerator(),
                success: function (msg) {

                    if (msg != "") {
                        var arr = msg.split("~");
                        if (arr.length == 4) {
                            $("#tCardNo").val(arr[0]);
                            $("#ExpMonth").val(arr[1]);
                            $("#ExpYear").val(arr[2]);
                            $("#tCardName").val(arr[3]);
                        }
                    }
                }
            });

            if(document.getElementById('tCardNo').value.substring(document.getElementById('tCardNo').value.length - 1) == " ")
                document.getElementById('tCardNo').value = document.getElementById('tCardNo').value.substring(0,document.getElementById('tCardNo').value.length - 1);
            else
                document.getElementById('tCardNo').value = document.getElementById('tCardNo').value + " ";
        }

        function guidGenerator() {
            var S4 = function () {
                return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
            };
            return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
        }
    </script>
</head>
<body onload="window.resizeTo(500, 300);">
    <form id="form1" runat="server" defaultbutton="OK">
    <div>
        <table width="460px">
            <tr>
                <td colspan="2" style="font-size:18px; font-weight:bold;">Swipe Credit Card</td>
            </tr>
            <tr>
                <td colspan="2" style="font-size:12px; color:Red"><asp:Label ID="ErrMsg" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td colspan="2" style="width:100%; padding:10px;">
                    <asp:TextBox ID="tCardNo" runat="server" Width="100%" AutoPostBack="false" 
                        ontextchanged="tCardNo_TextChanged" AutoCompleteType="None" onchange="appendSpace()"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="width:100%; padding:20px;">
                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td align="right" style="width:50%">
                                Expiration Month: <asp:DropDownList ID="ExpMonth" runat="server"></asp:DropDownList>
                            </td>
                            <td align="right">
                                Expiration Year: <asp:DropDownList ID="ExpYear" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr><td style="height:10px;"></td></tr>
                        <tr>
                            <td colspan="2" align="right">
                                Name on Card: <asp:TextBox ID="tCardName" runat="server" Width="72%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left"><asp:Button CssClass="btn" Font-Size="18px" Font-Bold="true" 
                        ID="Cancel" Text="Cancel" runat="server" onclick="Cancel_Click" /></td>
                <td align="right"><asp:Button CssClass="btn" Font-Size="18px" Font-Bold="true" ID="OK" Text="OK" runat="server" onclick="OK_Click" /></td>
            </tr>
        </table>
        
    </div>    
    </form>
</body>
<style type="text/css"> 
div{behavior: url(/Scripts/PIE.htc);}
.btn{behavior: url(/Scripts/PIE.htc);}
</style>
<script>    document.getElementById('tCardNo').focus(); </script>
</html>
