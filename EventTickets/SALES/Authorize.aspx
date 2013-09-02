<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Authorize.aspx.cs" Inherits="EventTickets.Sales.Authorize" %>
<%@ Register Src="../payment.ascx" TagName="pay" TagPrefix="p" %>
<asp:Content ID="C1" ContentPlaceHolderID="H" runat="server"></asp:Content>
<asp:Content ID="C2" ContentPlaceHolderID="M" runat="server">

<style>
    .Amt
    {
        font-size:16px;
        color:Black;
        font-weight:bold;	
    }
</style>
 
<table style="padding:10px; width:100%; height:370px;">
<tr>
<td style="width:100%; height:100%;">
<table style="width:100%; height:100%; text-align:center; vertical-align:middle;" cellpadding="0"  cellspacing="0" border="0">
<tr>
    <td align="center" width="70%"  valign="top" style="height:100%; ">
        <div class="box-shadow" style="min-height:445px; border:1px solid grey;">
            <table style="text-align:center; vertical-align:top; width:100%; height:100%" cellpadding="0"  cellspacing="0" border="0" >
                <tr>
                    <td colspan="3" align="center" style="font-size:22px; background-color:#cccccc; font-weight:bold; vertical-align:top; color:Black; height:30px;">Authorization</td>
                </tr>
                <tr><td colspan="3" style="height:10px;"></td></tr>
                <tr>
                    <td colspan="3" align="left" style="height:335px; vertical-align:top; padding-left:10px;" valign="top">
                         <asp:Label ID="lblMsg" runat="server" Text="" Height="100%" Width="100%" Font-Size="18px"></asp:Label>
                    </td>
                </tr>
                <tr><td colspan="3" ></td></tr>
                <tr>
                    <td style="height:50px; vertical-align:bottom; padding:5px;" align="left">
                        <asp:Button runat="server" CssClass="btn" Text="Print Receipt" 
                            ID="PrintReceipt" Visible="false"  />&nbsp;&nbsp;&nbsp;<asp:Button runat="server" CssClass="btn" Text="Print Ticket PDF" 
                            ID="btnPrintPDF" Visible="false"  />
                    </td>
                    <td style="height:50px; vertical-align:bottom; padding:5px;" align="right">
                        <asp:Button runat="server" CssClass="btn"  Visible="false" 
                            Text="Payment failed. Back to payment page" ID="btnBackPayment" 
                            onclick="btnBackPayment_Click" />
                    </td>
                    <td style="height:50px; vertical-align:bottom; padding:5px;" align="right">
                        <asp:Button runat="server" CssClass="btn" Text="Proceed to New Sale" ID="btnProceed" onclick="bNewSales_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </td>
    <td style="width:20px;"></td>
    <td valign="top" style="height:100%">
       <p:Pay ID="Pay" runat="server" />
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
