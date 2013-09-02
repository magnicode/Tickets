<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="payment.ascx.cs" Inherits="EventTickets.payment" %>
<script type="text/javascript">
    function ShowDelPayment(id, Status) {
        if (Status == '1') {
            if ($("#d_" + id).is(":visible")) {
                $("#d_" + id).hide(); $("#de_" + id).hide();
            } else {
                $("#d_" + id).show(); $("#de_" + id).show();
            }
        }
    }
    function Validate() {
        if (document.getElementById('M_payment_lblAmtRem').innerText != "$0.00") {
            
            alert('To finish, the amount remaining has to be zero');
            return false;
        }
        return true;
    }
</script>
<table cellpadding="0"  cellspacing="0" width="100%"  style="height:100%" border="0">
    <tr>
        <td valign="top" style="height:75%;">
            <div class="box-shadow" style="width:100%; height:100%; border:1px solid grey;">
                <table width="100%" style="height:100%" cellpadding="0"  cellspacing="0">
                    <tr>
                        <td align="center" style="font-size:22px; background-color:#cccccc; font-weight:bold; vertical-align:top; color:Black; height:30px;">Payments</td>
                    </tr>
                    <tr>
                        <td valign="top" style="padding:3px;">
                            <asp:DataList ID="dlPayment" runat="server" ShowHeader="false" ShowFooter="false" Width="100%" onitemcommand="dlPayment_ItemCommand">
                                <ItemTemplate >
                                    <div class="box-shadow" style="background-color:#d3d3d3;padding:3px 0px 3px 3px;border:1px solid #000;line-height:20px;cursor:pointer" onclick="ShowDelPayment('<%# Eval("Id").ToString()%>','<%# Eval("Status").ToString()%>')">
                                    <table width="100%" border="0" style="border:0px solid black; background-color:#d3d3d3" cellpadding="2">
                                        <tr>
                                            <td style="width:50px;"><img src="<%# Eval("ImagePath") %>" style="height:40px; width:auto;"></td>
                                            <td align="Left">
                                                <table width="100%" border="0">
                                                    <tr>
                                                        <td><asp:Label ID="lblPmtType" CssClass="Amt" style="width:100%" runat="server" Text='<%# Eval("Description") %>'></asp:Label></td>
                                                    </tr>
                                                    
                                                    <tr style='<%# Eval("PaymentType")!="credit"?"display:none;":"display:" %>'>
                                                        <td align="left">Name: <asp:Label ID="lblCardName" CssClass="Amt"  style="font-size:12px; width:100%" runat="server" Text='<%# Eval("CardName") %>'></asp:Label></td>
                                                    </tr>
                                                     
                                                    <tr>
                                                        <td align="right" style="color:Black;">
                                                            Amount: <asp:Label ID="lblAmt" CssClass="Amt" style="width:100%" runat="server" Text='<%# "$"+Eval("Amount") %>'></asp:Label>        
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    </div>
                                    
                                    <% if (Request.ServerVariables["SCRIPT_NAME"].ToLower().IndexOf("authorize.aspx") < 0)
                                       { %>
                                    <asp:HiddenField runat="server" ID="cId" Value='<%# Eval("Id").ToString()%>' />
                                    <div id="de_<%# Eval("Id").ToString()%>" style="display:none;text-align:center;width:50%; margin-top:-10px; float:left;">
                                        <asp:LinkButton runat="server" ID="bEdit" CssClass="lbtn" Text="Edit" CommandName="edit" />
                                    </div>
                                    <div id="d_<%# Eval("Id").ToString()%>" style="display:none;text-align:center;width:50%;margin-top:-10px; float:right;">
                                        <asp:LinkButton runat="server" ID="bRemove" CssClass="lbtn" Text="Remove" CommandName="del" OnClientClick="return confirm('Are you sure you want to remove?');" />
                                    </div>
                                    <% } %>
                                    <div style="height:10px;"></div>
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td style="height:10px; border:0px solid black;" align="right">
        </td>
    </tr>
    <tr>
        <td style="border:0px solid black;">
            <div class="box-shadow" style="width:100%; height:100%; border:1px solid grey;">
            <table width="100%" border="0">
                <tr>
                    <td align="right" class="Amt" style="width:70%;">Amount Due: </td><td align="right"><asp:Label ID="lblAmtDue" Text="$0.00" runat="server" Width="100%" class="Amt"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right" class="Amt">Amount Paid: </td><td align="right" style="border-bottom:2px solid black;"><asp:Label ID="lblAmtPaid" Text="$0.00" runat="server" Width="100%" class="Amt" style="text-align:right;"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right" class="Amt">Amount Remaining: </td><td align="right"><asp:Label ID="lblAmtRem" Text="$0.00" runat="server" Width="100%" class="Amt"></asp:Label></td>
                </tr>
                <tr>
                    <td style="height:10px;"></td><td></td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="bCancel" runat="server" Text="Cancel" class="btn" Width="100px" 
                            Height="35px" onclick="bCancel_Click" OnClientClick="return confirm('Click OK to remove everything from the cart and start over.');" />
                        &nbsp;
                        <asp:Button ID="bFinish" runat="server" Text="Finish" class="btn" Width="100px" 
                            Height="35px" onclick="bFinish_Click" OnClientClick="return Validate();" />
                    </td>
                </tr>
                <tr>
                    <td style="height:10px;"></td><td></td>
                </tr>
            </table>
            </div>
        </td>
    </tr>
</table>
<div id="strScript" runat="server" style="display:none;"></div>