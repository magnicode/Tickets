<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="voucher.aspx.cs" Inherits="EventTickets.SALES.voucher" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Voucher Information</title>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
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
     
</head>
<body onload="window.resizeTo(500, 300);">
    <form id="form1" runat="server" defaultbutton="OK">
    <div>
        <table width="100%" cellpadding="5">
            <tr>
                <td colspan="2" style="font-size:18px; font-weight:bold;">Voucher Information</td>
            </tr>
            <tr>
                <td style="width:40%; text-align:right; vertical-align:top:" valign="top">Events to Voucher: </td>
                <td>
                    <asp:DataList ID="dC" runat="server" RepeatDirection="Horizontal" RepeatColumns="1" CellPadding="0" CellSpacing="0" Width="100%" >
                        <ItemTemplate>
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td style="width:70%; font-weight:bold;">
                                        <%# Eval("Name").ToString()%>, Qty: <%# Eval("Quantity").ToString()%>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; height:30px;">Voucher Value: </td>
                <td id="td_voucher_value" runat="server" style="text-align:left;font-weight:bold;">
                                        
                </td>
            </tr>
            <tr>
                <td colspan="2" style="font-size:12px; color:Red"><asp:Label ID="ErrMsg" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td style="width:30%; text-align:right; height:40px;">Enter Voucher Number: </td>
                <td style="padding:10px;">
                    <asp:TextBox ID="tVoucherNo" runat="server" Width="100%" AutoCompleteType="None" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left"><asp:Button CssClass="btn" Font-Size="18px" Font-Bold="true" 
                        ID="Cancel" Text="Cancel" runat="server" onclick="Cancel_Click"  /></td>
                <td align="right"><asp:Button CssClass="btn" Font-Size="18px" Font-Bold="true" 
                        ID="OK" Text="OK" runat="server" onclick="OK_Click" /></td>
            </tr>
        </table>
    </div>    
    </form>
</body>
</html>

<style type="text/css"> 
div{behavior: url(/Scripts/PIE.htc);}
.btn{behavior: url(/Scripts/PIE.htc);}
</style>
<script>    document.getElementById('tVoucherNo').focus(); </script>