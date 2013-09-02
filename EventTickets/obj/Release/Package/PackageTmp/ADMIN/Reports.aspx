<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="EventTickets.Admin.Reports" %>
<asp:Content ID="C1" ContentPlaceHolderID="H" runat="server">   
    <script src="../Scripts/jquery.datepick.js" type="text/javascript"></script>
    <link href="../Styles/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=tFromD.ClientID %>").datepick();
            $("#<%=tToD.ClientID %>").datepick();
        });

        function showsummary() {
            document.getElementById('hTitle').innerHTML = "Sales Summary Report";
            document.getElementById('hVal').value = "1";
            document.getElementById('trSSumm').style.display = '';
            document.getElementById('td_date_to').style.display = '';
            document.getElementById('td_category').style.display = 'none';
            document.getElementById('td_location').style.display = 'none';
            document.getElementById('lbldatefrom').innerText = "Date from";
        }

        function showdtls() {
            document.getElementById('hTitle').innerHTML = "Sales Details Report";
            document.getElementById('hVal').value = "2";
            document.getElementById('trSSumm').style.display = '';
            document.getElementById('td_category').style.display = 'none';
            document.getElementById('td_date_to').style.display = '';
            document.getElementById('td_location').style.display = 'none';
            document.getElementById('lbldatefrom').innerText = "Date from";
        }

        function showsales() {
            document.getElementById('trSSumm').style.display = 'none';
            document.getElementById('td_date_to').style.display = '';
            window.open('../Reports/Reports.aspx?ReportName=SalesDetails&DataSource=Sales');
        }

        function showmacy() {
            document.getElementById('trSSumm').style.display = 'none';
            document.getElementById('td_date_to').style.display = '';
            location.href = '../survey/macyfile.aspx';
        }

        function showeventsoldcount() {
            document.getElementById('hTitle').innerHTML = "Event Sold Count by Date Range";
            document.getElementById('hVal').value = "3";
            document.getElementById('trSSumm').style.display = '';
            document.getElementById('td_category').style.display = 'none';
            document.getElementById('td_date_to').style.display = '';
            document.getElementById('td_location').style.display = 'none';
            document.getElementById('lbldatefrom').innerText = "Date from";
        }
        
        function showcategorysummary() {
            document.getElementById('hTitle').innerHTML = "Category Summary by Date Range";
            document.getElementById('hVal').value = "4";
            document.getElementById('trSSumm').style.display = '';
            document.getElementById('td_category').style.display = '';
            document.getElementById('td_date_to').style.display = '';
            document.getElementById('td_location').style.display = 'none';
            document.getElementById('lbldatefrom').innerText = "Date from";
        }
        function showclerksummary() {
            document.getElementById('hTitle').innerHTML = "Clerk Summary by Location";
            document.getElementById('hVal').value = "5";
            document.getElementById('trSSumm').style.display = '';
            document.getElementById('td_category').style.display = 'none';
            document.getElementById('td_date_to').style.display = 'none';
            document.getElementById('td_location').style.display = '';
            document.getElementById('lbldatefrom').innerText = "Date";
        }
        
        function showvouchersummary(){
            document.getElementById('hTitle').innerHTML = "Voucher Summary by Category";
            document.getElementById('hVal').value = "6";
            document.getElementById('trSSumm').style.display = '';
            document.getElementById('td_category').style.display = '';
            document.getElementById('td_date_to').style.display = '';
            document.getElementById('td_location').style.display = 'none';
            document.getElementById('lbldatefrom').innerText = "Date from";
        }
        function showSDReturn() {

            document.getElementById('hTitle').innerHTML = "SD Returns by Date";
            document.getElementById('hVal').value = "7";
            document.getElementById('trSSumm').style.display = '';
            document.getElementById('td_category').style.display = 'none';
            document.getElementById('td_date_to').style.display = '';
            document.getElementById('td_location').style.display = 'none';
            document.getElementById('lbldatefrom').innerText = "Date from";

        }

        function showUserLog() {
            document.getElementById('hTitle').innerHTML = "User Log by Date";
            document.getElementById('hVal').value = "8";
            document.getElementById('trSSumm').style.display = '';
            document.getElementById('td_category').style.display = 'none';
            document.getElementById('td_date_to').style.display = '';
            document.getElementById('td_location').style.display = 'none';
            document.getElementById('lbldatefrom').innerText = "Date from";
        }

        function showReturnSummary() {
            document.getElementById('hTitle').innerHTML = "Sales Return Summary";
            document.getElementById('hVal').value = "9";
            document.getElementById('trSSumm').style.display = '';
            document.getElementById('td_category').style.display = 'none';
            document.getElementById('td_date_to').style.display = '';
            document.getElementById('td_location').style.display = 'none';
            document.getElementById('lbldatefrom').innerText = "Date from";
        }

        function showReturnDetails() {
            document.getElementById('hTitle').innerHTML = "Sales Return Details";
            document.getElementById('hVal').value = "10";
            document.getElementById('trSSumm').style.display = '';
            document.getElementById('td_category').style.display = 'none';
            document.getElementById('td_date_to').style.display = '';
            document.getElementById('td_location').style.display = 'none';
            document.getElementById('lbldatefrom').innerText = "Date from";
        }

        function showUsers() {
            window.open('../Reports/Reports.aspx?ReportName=Users&DataSource=Users');   
        }

        function showsummaryrep() {
            //alert('../Reports/Reports.aspx?ReportName=Sales&DataSource=Sales&fromd=' + document.getElementById('tFromD').value + '&tod=' + document.getElementById('tToD').value);       
            if (document.getElementById('hVal').value == "1")
                window.open('../Reports/Reports.aspx?ReportName=Sales&DataSource=Sales&fromd=' + document.getElementById('M_tFromD').value + '&tod=' + document.getElementById('M_tToD').value);
            else if (document.getElementById('hVal').value == "2")
                window.open('../Reports/Reports.aspx?ReportName=SalesDetails&DataSource=Sales&fromd=' + document.getElementById('M_tFromD').value + '&tod=' + document.getElementById('M_tToD').value);
            else if (document.getElementById('hVal').value == "3")
                window.open('../Reports/Reports.aspx?ReportName=EventSoldCount&DataSource=EventSoldCount&fromd=' + document.getElementById('M_tFromD').value + '&tod=' + document.getElementById('M_tToD').value);
            else if (document.getElementById('hVal').value == "4")
                window.open('../Reports/Reports.aspx?ReportName=CategorySummary&DataSource=CategorySummary&fromd=' + document.getElementById('M_tFromD').value + '&tod=' + document.getElementById('M_tToD').value + "&category_id=" + document.getElementById('M_dCategory').value + "&category_name=" + document.getElementById('M_dCategory').options[document.getElementById('M_dCategory').selectedIndex].text);
            else if (document.getElementById('hVal').value == "5")
                window.open('../Reports/Reports.aspx?ReportName=ClerkSummary&DataSource=ClerkSummary&fromd=' + document.getElementById('M_tFromD').value + "&location_id=" + document.getElementById('M_dLocation').value);
            else if (document.getElementById('hVal').value == "6")
                window.open('../Reports/Reports.aspx?ReportName=VoucherSummary&DataSource=VoucherSummary&fromd=' + document.getElementById('M_tFromD').value + '&tod=' + document.getElementById('M_tToD').value + "&category_id=" + document.getElementById('M_dCategory').value + "&category_name=" + document.getElementById('M_dCategory').options[document.getElementById('M_dCategory').selectedIndex].text);
            else if (document.getElementById('hVal').value == "7")
                window.open('../Reports/Reports.aspx?ReportName=SDReturns&DataSource=SDReturns&fromd=' + document.getElementById('M_tFromD').value + '&tod=' + document.getElementById('M_tToD').value);
            else if (document.getElementById('hVal').value == "8")
                window.open('../Reports/Reports.aspx?ReportName=UserLog&DataSource=UserLog&fromd=' + document.getElementById('M_tFromD').value + '&tod=' + document.getElementById('M_tToD').value);
            else if (document.getElementById('hVal').value == "9")
                window.open('../Reports/Reports.aspx?ReportName=SalesReturn&DataSource=SalesReturn&fromd=' + document.getElementById('M_tFromD').value + '&tod=' + document.getElementById('M_tToD').value);
            else if (document.getElementById('hVal').value == "10")
                window.open('../Reports/Reports.aspx?ReportName=SalesReturnDetails&DataSource=SalesReturnDetails&fromd=' + document.getElementById('M_tFromD').value + '&tod=' + document.getElementById('M_tToD').value);           

        }
        function showstockmanagement() {
            document.getElementById('trSSumm').style.display = 'none';
            document.getElementById('td_date_to').style.display = 'none';
            location.href = 'stockentry.aspx';
        }

        function showavailabilitycheck() {
            document.getElementById('trSSumm').style.display = 'none';
            document.getElementById('td_date_to').style.display = 'none';
            location.href = 'stock.aspx';
        
        }


    </script>
</asp:Content>
<asp:Content ID="C2" ContentPlaceHolderID="M" runat="server">
<div style="padding:5px 0px 0px 5px">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td colspan="3" width="100%">
                
                <table cellpadding="0" cellspacing="0" width="100%" border="0">
                    <tr>
                        <td style="width:100%; height:30px; font-size:16px; font-weight:bold; border-bottom:1px solid black;" valign="bottom">
                            Sales Reports <input type="hidden" id="hVal" value="0" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <div runat="server" id="divSalesSummary" style="height:40px; padding:10px; float:left;"><input type="button" runat="server" id="bSalesSummary" style="width:220px;" value="Sales Summary" onclick="showsummary();" /></div>
                            <div runat="server" id="divSalesDetails" style="height:40px; padding:10px; float:left;"><input type="button" runat="server" id="bSalesDetails" style="width:220px;" value="Sales Details" onclick="showdtls();" /></div>
                            <div runat="server" id="divEventSoldCount" style="height:40px; padding:10px; float:left;"><input type="button" runat="server" id="bEventSoldCount" style="width:220px;" value="Event Sold Count" onclick="showeventsoldcount();" /></div>
                            <div runat="server" id="divCategorySummary" style="height:40px; padding:10px; float:left;"><input type="button" runat="server" id="bCategorySummary" style="width:220px;" value="Category Summary" onclick="showcategorysummary();" /></div>
                            <div runat="server" id="divClerkSummary" style="height:40px; padding:10px; float:left;"><input type="button" runat="server" id="bClerkSummary" style="width:220px;" value="Clerk Summary" onclick="showclerksummary();" /></div>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <div runat="server" id="divVoucherSummary" style="height:40px; padding:10px; float:left;"><input type="button" runat="server" id="bVoucherSummary" style="width:220px;" value="Voucher Summary" onclick="showvouchersummary();" /></div>
                            <div runat="server" id="divSDReturns" style="height:40px; padding:10px; float:left;"><input type="button" runat="server" id="bSDReturns" style="width:220px;" value="SD Returns" onclick="showSDReturn();" /></div>
                            <div runat="server" id="divUserLog" style="height:40px; padding:10px; float:left;"><input type="button" runat="server" id="bUserLog" style="width:220px;" value="User Log" onclick="showUserLog();" /></div>
                            <div runat="server" id="divSalesReturnSummary" style="height:40px; padding:10px; float:left;"><input type="button" runat="server" id="bSalesReturnSummary" style="width:220px;font-size:16px" value="Sales Return Summary" onclick="showReturnSummary();" /></div>
                            <div runat="server" id="divSalesReturnDetails" style="height:40px; padding:10px; float:left;"><input type="button" runat="server" id="bSalesReturnDetails" style="width:220px;" value="Sales Return Details" onclick="showReturnDetails();" /></div>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <div runat="server" id="divUsers" style="height:40px; padding:10px; float:left;"><input type="button" runat="server" id="bUsers" style="width:220px;" value="Users" onclick="showUsers();" /></div>
                        </td>
                    </tr>

                    <tr runat="server" id="tr_MacysReports">
                        <td style="height:50px; font-size:16px; font-weight:bold; border-bottom:1px solid black;" valign="bottom">
                            Macy's Reports
                        </td>
                    </tr>
                    <tr>
                        <td style="height:50px; padding-left:10px">
                            <div style="height:40px; padding:10px; float:left;"><input type="button"  runat="server" id="bMacysSurveyData" style="width:220px;" value="Macy's Survey Data" onclick="showmacy();" /></div>
                        </td>
                    </tr>
                    <tr runat="server" id="tr_InventoryManagement">
                        <td style="height:50px; font-size:16px; font-weight:bold; border-bottom:1px solid black;" valign="bottom">
                            Inventory Management
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div runat="server" id="divStockManagement" style="height:40px; padding:10px; float:left;"><input type="button" runat="server" id="bStockManagement" style="width:220px;" value="Stock Management" onclick="showstockmanagement();" /></div>
                            <div runat="server" id="divAvailabilityCheck" style="height:40px; padding:10px; float:left;"><input type="button" runat="server" id="bAvailabilityCheck" style="width:220px;" value="Availability Check" onclick="showavailabilitycheck();" /></div>
                        </td>
                    </tr>
            </table>
            </td>
        </tr>
        <tr id="trSSumm" style="display:none; height:50px; vertical-align:bottom;">
            <td colspan="3">
                <table width="100%">
                    <tr>
                        <td colspan="6" align="center">
                            <h3 id="hTitle">Sales Summary Report</h3>
                        </td>
                    </tr>
                    <tr style="height:5px">
                        <td colspan="6" valign="top"><div style="width:100%; height:1px; background-color:Red;" ></div></td>
                    </tr>
                    <tr>
                        <td id="td_category">Product Category: <asp:DropDownList ID="dCategory" runat="server"></asp:DropDownList> </td>
                        <td id="td_location">Location: <asp:DropDownList ID="dLocation" runat="server"></asp:DropDownList> </td>
                        <td id="td_date_from"><span id="lbldatefrom">Date From</span>: <asp:TextBox ID="tFromD" runat="server"></asp:TextBox></td>
                        <td id="td_date_to">Date To: <asp:TextBox ID="tToD" runat="server"></asp:TextBox></td>
                        <td><input type="button" value="Show Report" onclick="showsummaryrep();" /></td>
                        <td></td>
                    </tr>
                </table>
            </td>            
        </tr>
    </table>
</div>
</asp:Content>
