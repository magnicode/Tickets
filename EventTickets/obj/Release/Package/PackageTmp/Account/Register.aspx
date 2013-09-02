<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Register.aspx.cs" Inherits="EventTickets.Account.Register" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="H">
<script>
    function validate() {
        if (document.getElementById('M_tUsername').value == "") {
            alert('Please enter username');
            document.getElementById('M_tUsername').focus();
            return false;
        }
        if (document.getElementById('M_tPassword').value == "") {
            alert('Please enter password');
            document.getElementById('M_tPassword').focus();
            return false;
        }
        if (document.getElementById('M_tConfirmPassword').value == "") {
            alert('Please confirm your password');
            document.getElementById('M_tConfirmPassword').focus();
            return false;
        }
        if (document.getElementById('M_tPassword').value != document.getElementById('M_tConfirmPassword').value) {
            alert('Mismatch in passwords');
            document.getElementById('M_tConfirmPassword').focus();
            return false;
        }

        if (document.getElementById('M_tFirstName').value == "") {
            alert('Please enter first name');
            document.getElementById('M_tFirstName').focus();
            return false;
        }
        if (document.getElementById('M_tLastName').value == "") {
            alert('Please enter last name');
            document.getElementById('M_tLastName').focus();
            return false;
        }
        /*
        if (document.getElementById('M_tPhone').value == "") {
            alert('Please enter phone');
            document.getElementById('M_tPhone').focus();
            return false;
        }
        if (document.getElementById('M_tEmail').value == "") {
            alert('Please enter email');
            document.getElementById('M_tEmail').focus();
            return false;
        }
        */
    }

    function CheckAvailability() {

        $.ajax({
            type: "GET",
            url: '/ajax/ajax.aspx',
            data: 'Type=GetUserDetails&username=' + $("#M_tUsername").val() + '&id=' + guidGenerator(),
            contentType: "application/json; charset=utf-8",
            dataType: "xml",
            success: function (msg) {

                if (msg != null) {
                    var xml = $(msg);

                    $('#M_tUserId').val(xml.find("Table").find("UserId").text());
                    $('#M_tFirstName').val(xml.find("Table").find("FirstName").text());
                    $('#M_tLastName').val(xml.find("Table").find("LastName").text());

                     

                } else {
                    $('#M_tUserId').val("");
                    $('#M_tFirstName').val("");
                    $('#M_tLastName').val("");
                }

            }
        });
    }

    function guidGenerator() {
        var S4 = function () {
            return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
        };
        return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
    }
 

</script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server"  ContentPlaceHolderID="M">
    <table style="padding:10px; width:100%; height:370px;" border="0">
        <tr>
        <td style="width:100%; height:100%;">
        <table style="width:100%; height:100%; text-align:center; vertical-align:middle;" cellpadding="0"  cellspacing="0" border="0">
        <tr>
            <td align="center" width="100%"  valign="top" style="height:100%; ">
                <div class="box-shadow" style="min-height:445px; border:1px solid grey;">
                    <table style="text-align:center; vertical-align:top; width:100%; height:100%" cellpadding="2"  cellspacing="2" border="0" >
                        <tr>
                            <td colspan="2" align="center" style="font-size:22px; background-color:#cccccc; font-weight:bold; vertical-align:top; color:Black; height:30px;">User Creation</td>
                        </tr>
                        <tr><td colspan="2" style="height:20px;"><div id="div_msg" style="display:none; color:Red; border:1px solid red; background-color:InfoBackground">Username not available</div></td></tr>
                        <tr>
                            <td style="width:20%" align="right">
                                User Name:&nbsp;        
                            </td>
                            <td align="left">
                                <asp:TextBox AutoPostBack="true" runat="server" ID="tUsername" width="200px" 
                                    ontextchanged="tUsername_TextChanged" ></asp:TextBox>
                                <asp:HiddenField ID="hUserId" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td  align="right">
                                Password:&nbsp;
                            </td>
                            <td align="left">
                                <input type="password" runat="server" id="tPassword" style="width:200px;"  />
                            </td>
                        </tr>
                        <tr>
                            <td  align="right">
                                Confirm Password:&nbsp; 
                            </td>
                            <td align="left">
                                <input type="password" runat="server" id="tConfirmPassword" style="width:200px;"  />
                            </td>
                        </tr>

                        <tr>
                            <td  align="right">
                                First Name:&nbsp;        
                            </td>
                            <td align="left">
                                <input type="text" runat="server" id="tFirstName" style="width:200px;" />
                            </td>
                        </tr>
                         
                        <tr>
                            <td  align="right">
                                Last Name:&nbsp; 
                            </td>
                            <td align="left">
                                <input type="text" runat="server" id="tLastName" style="width:200px;" />
                            </td>
                        </tr>
                        
                        <tr style="display:none;">
                            <td  align="right">
                                Address 1:&nbsp; 
                            </td>
                            <td align="left">
                                <input type="text" runat="server" id="tAddress1" style="width:400px;" />
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td  align="right">
                                Address 2:&nbsp; 
                            </td>
                            <td align="left">
                                <input type="text" runat="server" id="tAddress2" style="width:400px;" />
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td  align="right">
                                City:&nbsp; 
                            </td>
                            <td align="left">
                                <input type="text" runat="server" id="tCity" style="width:200px;" />
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td  align="right">
                                State:&nbsp; 
                            </td>
                            <td align="left">
                                <input type="text" runat="server" id="tState" style="width:200px;" />
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td  align="right">
                                Zip:&nbsp; 
                            </td>
                            <td align="left">
                                <input type="text" runat="server" id="tZip" style="width:200px;" />
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td  align="right">
                                Phone:&nbsp; 
                            </td>
                            <td align="left">
                                <input type="text" runat="server" id="tPhone" style="width:200px;" />
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td  align="right">
                                Email:&nbsp; 
                            </td>
                            <td align="left">
                                <input type="text" runat="server" id="tEmail" style="width:200px;" />
                            </td>
                        </tr>
                        <tr><td colspan="2" style="height:10px;"></td></tr>
                        <tr>
                            <td colspan="2" style="width:100%">
                                <table width="100%" border="0">
                                    <tr>
                                        <td style="width:20%" align="right" valign="top">
                                            Individual Permissions:&nbsp;
                                        </td>
                                        <td style="width:20%" align="left" valign="top">
                                            <asp:CheckBoxList ID="lstIndPermissions" runat="server">
                                            </asp:CheckBoxList>
                                        </td>
                                        <td></td>
                                        <td style="width:20%" align="right" valign="top">
                                            Group Permissions:&nbsp;
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:CheckBoxList ID="lstGrpPermissions" runat="server">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr><td colspan="2" style="height:10px;"></td></tr>
                        <tr>
                            <td  align="right">
                                &nbsp; 
                            </td>
                            <td align="left">
                                <asp:Button CssClass="btn" Font-Size="18px" Font-Bold="true" ID="btnSubmit"  OnClientClick="return validate();"
                                    Text="Submit" runat="server" onclick="btnSubmit_Click" />
                            </td>
                        </tr>
                        <tr><td colspan="2" style="height:10px;"></td></tr>
                    </table>
                </div>
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
