<%@ Page Title="Visitor Center Survey" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="macys.aspx.cs" Inherits="EventTickets.survey.macys" %>
<asp:Content ID="Content1" ContentPlaceHolderID="H" runat="server">
    <style type="text/css">
        .td18
        {
        	font-size:18px;
        	text-align:right;
        	padding-right:5px;
        }
		input[type="text"],select
        {
            font-size:16px;
            font-weight:normal;
            width:250px;
            border:1px solid #ccc;
        }
        input[type="text"]:focus,select:focus
        { 
            border:1px solid lime;
        }
        .btn:hover{border:1px solid lime;}
    </style>
    <script type="text/javascript">
		function ShowDI() {
            if (document.getElementById("M_dVisit").value == "1") {
                $("#M_dCnt").show(); $("#M_dState").hide();
            }
            else {
                $("#M_dState").show(); $("#M_dCnt").hide();
            }
        }        
		
        function checkval() {
            var e = document.getElementById("M_dPurp");
            var sPurp = e.options[e.selectedIndex].value;
            var sMsg = "";
            //if (sPurp == "")
                //sMsg = "Please select purpose. \r\n";
            e = document.getElementById("M_dNo");
            sPurp = e.options[e.selectedIndex].value;
            //if (sPurp == "")
                //sMsg += "Please select number of visitors. \r\n";
            e = document.getElementById("M_dStay");
            sPurp = e.options[e.selectedIndex].value;
            if (sPurp == ""){
                sMsg += "Please select Where are you staying. \r\n";
				document.getElementById("M_dStay").focus();
			}
            else if (sPurp == "3") {
                sPurp = document.getElementById('M_tHotel').value;
                if (sPurp == ""){
                    sMsg += "Please enter Hotel Name.";
					document.getElementById("M_tHotel").focus();
				}
            }
            if (sMsg != "") {
                alert(sMsg);
                return false;
            }
            return true;
        }
		function staychange() {
			if (!document.getElementById('M_dStay')) return;
            if (document.getElementById('M_dStay').value == 3) {
                document.getElementById('M_trHotel').style.display = '';                
            }
            else {
                document.getElementById('M_trHotel').style.display = 'none';                
            }
        }

        function hearchange() {
			if (!document.getElementById('M_dHear')) return;
            if (document.getElementById('M_dHear').value == 4 || document.getElementById('M_dHear').value == 5) {
                if (document.getElementById('M_dHear').value == 4)
                    document.getElementById('M_spnName').innerHTML = "Enter name of magazine";
                else
                    document.getElementById('M_spnName').innerHTML = "Enter name of website";
                document.getElementById('M_trHN').style.display = '';

            }
            else {
                document.getElementById('M_trHN').style.display = 'none';
            }
        }
		function ClearMsg() {
            //document.getElementById("hMsg").innerHTML = "";
        } 
    </script>
</asp:Content>
<asp:Content ID="C2" ContentPlaceHolderID="M" runat="server">
    <div style="width:100%; min-height:570px" align="center">
        <table cellpadding="0" cellspacing="0" width="90%" style="min-height:570px;padding:5px">
            <tr>
                <td style="width:25%;height:65px" align="left"><img src="../images/macys.jpg" height="41" width="150" alt="" /></td>
                <td align="center" style="width:50%; font-size:22px; font-family:Verdana; font-weight:bold;height:65px">
                    Visitor Center Survey
                </td>
                <td style="width:25%"></td>
            </tr>
            <tr id="trHd" runat="server">
                <td align="left"><span style="font-weight:bold; color:Red; text-align:left;">Form - 003 - Herald Square</span></td>
                <td>&nbsp;</td>
                <td align="right">
                    User: <asp:Label ID="lUser" runat="server"></asp:Label> 
                </td>
            </tr>
            <tr style="height:2px">
                <td colspan="3"><div style="width:100%; height:1px; background-color:Red;" ></div></td>
            </tr>
            <tr>
                <td colspan="3" valign="top">
                    <table cellpadding="5" cellspacing="0" width="100%" id="trMain" runat="server">
                        <tr>
                            <td class="td18">Visit Date:</td>
                            <td align="left"><asp:Label ID="lDt" runat="server" Font-Size="18px" ></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="td18">From where are you visiting?</td>
                            <td align="left">
                                <asp:DropDownList ID="dVisit" runat="server" onchange="ShowDI()">
                                    <asp:ListItem Value="1">International</asp:ListItem>
                                    <asp:ListItem Value="2">Domestic</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td align="left">                                
                                <asp:DropDownList ID="dCnt" runat="server">
                                </asp:DropDownList>
                                <asp:DropDownList ID="dState" runat="server">
                                </asp:DropDownList>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="td18">What is the purpose of your Trip?</td>
                            <td align="left">
                                <asp:DropDownList ID="dPurp" runat="server"  onfocus="ClearMsg()" >
                                    <asp:ListItem Value="">Please Select</asp:ListItem>
                                    <asp:ListItem Value="1">Business</asp:ListItem>
                                    <asp:ListItem Value="2">Leisure</asp:ListItem>
                                    <asp:ListItem Value="3">Both</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="td18">How many visitors in your party?</td>
                            <td align="left">
                                <asp:DropDownList ID="dNo" runat="server" >
                                    <asp:ListItem Value="" Selected="True">Please Select</asp:ListItem>
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="5">5</asp:ListItem>  
                                    <asp:ListItem Value="6">6</asp:ListItem>
                                    <asp:ListItem Value="7">7</asp:ListItem>
                                    <asp:ListItem Value="8">8</asp:ListItem>
                                    <asp:ListItem Value="9">9</asp:ListItem>
                                    <asp:ListItem Value="10">10</asp:ListItem>                        
                                    <asp:ListItem Value="11">11</asp:ListItem>
                                    <asp:ListItem Value="12">12</asp:ListItem>
                                    <asp:ListItem Value="13">13</asp:ListItem>
                                    <asp:ListItem Value="14">14</asp:ListItem>
                                    <asp:ListItem Value="15">15</asp:ListItem>
                                    <asp:ListItem Value="16">16</asp:ListItem>  
                                    <asp:ListItem Value="17">17</asp:ListItem>
                                    <asp:ListItem Value="18">18</asp:ListItem>
                                    <asp:ListItem Value="19">19</asp:ListItem>
                                    <asp:ListItem Value="20">20</asp:ListItem>
                                    <asp:ListItem Value="50">50</asp:ListItem>
                                    <asp:ListItem Value="100">100</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="td18">Where are you staying?</td>
                            <td align="left">
                                <select id="dStay" runat="server" onchange="staychange()" >
                                    <option Value="">Please Select</option>
                                    <option Value="1">Day Trip</option>
                                    <option Value="2">Family and Friends</option>
                                    <option Value="3">Hotel</option>
                                    <option Value="4">Other/Unknown</option>
                                </select>
                            </td>
                        </tr>
                        <tr id="trHotel" runat="server" style="display:none;">
                            <td></td>
                            <td align="left">
                                <div style="z-index:100">
                                    <asp:TextBox ID="tHotel1" Visible="false" runat="server"  Text="1"></asp:TextBox>
                                    <asp:DropDownList runat="server" ID="tHotel" ></asp:DropDownList>
                                </div>                    
                            </td>
                        </tr>
                        <tr>
                            <td class="td18">How did you hear or learn about the visitor savings pass?</td>
                            <td align="left">
                                <div style="z-index:-1">
                                 <!--option Value="1">Store Associate</option>
                                    <option Value="2">Hotel Concierge</option>
                                    <option Value="3">Word of Mouth</option>
                                    <option Value="4">Magazine</option>
                                    <option Value="5">Website</option-->
                                <select id="dHear" runat="server" onchange="hearchange()" >
                                    <option value="">Please Select</option>
                                   
                                    <option value="Associate Referral">Associate Referral</option>
                                    <option value="Explorer Pass">Explorer Pass</option>
                                    <option value="Grayline Sightseeing">Grayline Sightseeing</option>
                                    <option value="Hotel Concierge">Hotel Concierge</option>
                                    <option value="Internet">Internet</option>
                                    <option value="NY Pass">NY Pass</option>
                                    <option value="NYC Pocket Guide">NYC Pocket Guide</option>
                                    <option value="NYC Visitor Center">NYC Visitor Center</option>
                                    <option value="nyvisitguide.com">nyvisitguide.com</option>
                                    <option value="Official NYC Guide">Official NYC Guide</option>
                                    <option value="Times Square Visitor Center">Times Square Visitor Center</option>
                                    <option value="Tour Operator">Tour Operator</option>
                                </select></div>
                            </td>
                        </tr>
                        <tr id="trHN" runat="server" style="display:none">
                            <td id="spnName" class="td18"></td>
                            <td align="left"><asp:TextBox ID="tHN" runat="server" ></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="td18">What brand do you shop for at Macy's?</td>
                            <td align="left">
                                <asp:DropDownList ID="dBrand" runat="server" style="z-index:-1;" >
                                    <asp:ListItem Value="">Please Select</asp:ListItem>
                                    <asp:ListItem Value="Calvin Klein">Calvin Klein</asp:ListItem>
                                    <asp:ListItem Value="Coach">Coach</asp:ListItem>
                                    <asp:ListItem Value="Dooney Bourke">Dooney Bourke</asp:ListItem>
                                    <asp:ListItem Value="Levis">Levis</asp:ListItem>
                                    <asp:ListItem Value="Michael Kors">Michael Kors</asp:ListItem>
                                    <asp:ListItem Value="Ralph Lauren">Ralph Lauren</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="td18">
                                Please provide your email address to let us tell you about future exclusive sales and events, both online and in-store.
                            </td>
                            <td align="left">
                                <asp:TextBox ID="tMail" runat="server" Width="250px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" 
                                    ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                    ControlToValidate="tMail" ErrorMessage="Invalid Email Format" ForeColor="Red"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:50%" align="left">
                                <span style="font-size:9pt; text-align:left;">By providing your email address you confirm that you are over 18 and agree to receive emails from macys.com. You understand that macys.com's emails and US-based website use cookies or other tracking and you accept use of such cookies/tracking. More information is available at macys.com Notice of Privacy Practices.</span>
                            </td>
                            <td style="width:50%"></td>
                        </tr>
                        <tr>
                            <td align="left" style="padding-right:5px">
                                
                                </td>
                            <td align="left">
                                <asp:Button ID="bSubmit" runat="server" Text="Submit" onclick="bSubmit_Click" CssClass="btn" OnClientClick="return checkval();" />
                                <asp:Button ID="bClear" runat="server" Text="Clear" CssClass="btn" 
                                    onclick="bClear_Click" />
                             </td>
                        </tr>
                    </table>
                    <table cellpadding="4" cellspacing="0" width="100%" id="tblMsg" runat="server" visible="false">
                        <tr>
                            <td><h1 runat="server" id="hMsg">The form has been successfully saved!</h1></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table width="100%">
                                    <tr>
                                        <td width="40%" align="left">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td><asp:ImageButton ID="bEdit" runat="server" Visible="false" 
                                    ImageUrl="/images/left.gif" onclick="bEdit_Click" Height="40" Width="40" /></td><td>
                                    <asp:Button ID="bE" runat="server" Text="Edit previous entry" 
                                    CssClass="btn" onclick="bE_Click"  />
                                    </td>
                                            </tr>
                                        </table>
                                        
                                            </td>
                                        <td width="60%" align="left"><asp:Button ID="btnRet" runat="server" Text="Go to New Form" 
                                    onclick="btnRet_Click" CssClass="btn"  /></td>
                                    </tr>
                                </table>
                            
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            
        </table>
    </div>
    <script>
        hearchange();
        if (document.getElementById("dVisit")) document.getElementById("dVisit").focus();
        staychange();
        if (document.getElementById("btnRet")) document.getElementById("btnRet").focus();
   </script>
</asp:Content>
