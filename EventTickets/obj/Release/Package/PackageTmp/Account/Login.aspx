<%@ Page Title="Log In" Language="C#"   AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="EventTickets.Account.Login" %>

 <html>
 <head>
 <title>eVENTS Ticketing</title>
 <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
 <script>
     function validate() {
         if (document.getElementById('UserName').value == "" || document.getElementById('Password').value == "") {
             alert('User Name and Password cannot be empty');
             return false;
         }
         return true;
     }
 </script>
 
 </head>
 <body style="background-image:url(../images/background.jpg); background-repeat:no-repeat; text-align:center; vertical-align:middle; height:100%;" >
    <form runat="server" style="vertical-align:middle;">
     
            <table style="width:100%; height:100%; vertical-align:middle; text-align:center;">
                <tr>
                    <td align="center">
                    
            <span class="failureNotification" style="text-align:left; height:30px;">
                <asp:Label ID="LoginErrMsg" runat="server" ForeColor="Red" Font-Italic="true" Font-Size="13px"></asp:Label>
            </span>
            
            <div class="box-shadow" style="vertical-align:middle; text-align:center; width:300px; height:150px;" >
                 
                    <table cellpadding="2" cellspacing="2" class="table-box1">
                        <tr><td style="height:10px;"></td></tr>
                        <tr>
                            <td align="right"><asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" Width="80px" Font-Size="12px" ForeColor="Black">User Name:</asp:Label></td>
                            <td>
                                <asp:TextBox ID="UserName" TabIndex="0" runat="server" CssClass="textEntry" Width="150px" Height="25px" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right"><asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" Width="80px" Font-Size="12px" ForeColor="Black">Password:</asp:Label></td>
                            <td>
                                <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password" Width="150px" Height="25px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="LocationLabel" runat="server" AssociatedControlID="Location" Width="80px" Font-Size="12px" ForeColor="Black">Location:</asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="Location" runat="server" CssClass="textEntry" Width="150px" Height="25px"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td></td><td align="left"><asp:Button ID="LoginButton" CssClass="btn" runat="server" Text="Login" Height="30px" Width="100px" Font-Bold="true" Font-Size="16px" onclick="LoginButton_Click" OnClientClick="return validate();" /></td>
                        </tr>
                    </table>
                 
            </div>
                        </td>
                </tr>
            </table> 

    </form>
</body>
<script>
    document.getElementById("UserName").focus();
</script>
<style type="text/css"> 
div{behavior: url(/Scripts/PIE.htc);}
.btn{behavior: url(/Scripts/PIE.htc);}
</style> 
</html>
 