using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MYSQLDatabase;

using System.Net;
using System.Xml;
using Newtonsoft.Json;

namespace EventTickets.Sales
{
    public partial class Authorize : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            ((Button)Pay.FindControl("bCancel")).Enabled = false;
            ((Button)Pay.FindControl("bFinish")).Enabled = false;

            if ("" + Session["CartId"] == "") return;

            if (!IsPostBack)
            {
                string payment_type = "";
                string TransactionIds = "";
                classes.Payment payment = new classes.Payment();
                bool IsAuthorized = payment.AuthorizePayment();
                DataTable dt = payment.GetPayment();
                DataList dl = (DataList)Pay.FindControl("dlPayment");
                dl.DataSource = dt;
                dl.DataBind();
                string PrintURL = "";
                if (IsAuthorized)
                {
                    string sql = "";
                    DBConnection dbCon = new DBConnection();
                    int SalesMasterId = 0;
                    int SDOrderNo = 0;
                    try
                    {
                        dbCon.BeginTransaction();
                        sql = "select sum(productprice*quantity) + sum(ChildProductPrice*ChildQuantity) as productprice, sum(servicefee*quantity) as servicefee from ShoppingCart where CurrentCartId = " + Session["CartId"];
                        DataTable dtSum = dbCon.ExecuteTransactionDataSet(sql, CommandType.Text).Tables[0];
                        if (dtSum.Rows.Count > 0)
                        {
                            decimal productprice = EventFunctions.Functions.ToDecimal(dtSum.Rows[0]["productprice"].ToString());
                            decimal servicefee = EventFunctions.Functions.ToDecimal(dtSum.Rows[0]["servicefee"].ToString());

                            sql = @"insert into SalesMaster (CartId, SalesDate, PurchaseAmount, PurchaseTax, ServiceFee, GrandTotal, LocationId, ClerkId, CustomerName, CustomerAddress) values (" + 
                            Session["CartId"].ToString() + ", now()," + productprice.ToString() + ", 0, " + servicefee.ToString() + ", " + (productprice + servicefee).ToString() + ", " + Session["LocationId"] + ", '" + Session["userid"] + "', '" + dt.Rows[0]["cardname"].ToString().Replace("'","''") + "', '')";
                            SalesMasterId = dbCon.ExecuteTransactionGetID(sql, CommandType.Text);
                            
                            if (SalesMasterId > 0)
                            {
                                sql = "select C.productid, C.SmartProductCode, C.productprice,C.quantity,C.ProductName,C.SmartProductCode,1 as IsAdult, (select group_concat(serialNumber SEPARATOR ',') from ShoppingCartSerialNo S where S.CurrentCartId = C.CurrentCartId and S.ProductId = C.ProductId) as SNo,IsSmartDestination from ShoppingCart C where C.CurrentCartId = " + Session["CartId"];
                                sql += " Union select C.productid, C.SmartProductCode, C.ChildProductPrice as productprice,C.ChildQuantity as quantity,Concat(C.ProductName,' - Child'),C.SmartProductCode,0 as IsAdult, '' as SNo,IsSmartDestination from ShoppingCart C where C.ChildQuantity >0 and  C.CurrentCartId = " + Session["CartId"];

                                DataTable dtCart = dbCon.ExecuteTransactionDataSet(sql, CommandType.Text).Tables[0];

                                DataTable dtSmart = new DataTable();
                                dtSmart.Columns.Add(new DataColumn("SmartProductCode"));
                                dtSmart.Columns.Add(new DataColumn("SmartProductId"));
                                dtSmart.Columns.Add(new DataColumn("SmartAdultQty"));
                                dtSmart.Columns.Add(new DataColumn("SmartChildQty"));
                                for (int i = 0; i < dtCart.Rows.Count; i++)
                                {
                                    //Inserting SalesDetails 
                                    sql = "insert into SalesDetails (SalesMasterId, ProductId, SalesAmount, Count, SerialNo, LocationId,IsAdult,IsSmartDest,ProductName,ProductCode) values (" + SalesMasterId + ", " + dtCart.Rows[i]["productid"].ToString() + ", " + dtCart.Rows[i]["productprice"].ToString() + ", " + dtCart.Rows[i]["quantity"].ToString() + ", '" + dtCart.Rows[i]["SNo"].ToString() + "', " + Session["LocationId"] + "," + dtCart.Rows[i]["IsAdult"].ToString() + "," + dtCart.Rows[i]["IsSmartDestination"].ToString() + ",'" + dtCart.Rows[i]["ProductName"].ToString().Replace("'", "''") + "','" + dtCart.Rows[i]["SmartProductCode"].ToString().Replace("'", "''") + "')";
                                    int SalesDetailId = dbCon.ExecuteTransactionGetID(sql, CommandType.Text);

                                    if (dtCart.Rows[i]["IsSmartDestination"].ToString() == "0")
                                    {
                                        //Updating Status & SalesDetailId in ProductDetail table
                                        sql = @"update ProductDetail P Set P.SalesDetailId = " + SalesDetailId + @", P.Status = 1
                                                where ProductId = " + dtCart.Rows[i]["productid"].ToString() + " and LocationId = " + Session["LocationId"].ToString() + " and P.SerialNo in (" + dtCart.Rows[i]["SNo"].ToString() + ")";
                                        dbCon.ExecuteTransaction(sql, CommandType.Text);

                                        //Updating TicketCount in ProductMaster table
                                        sql = "update ProductMaster set TicketCount = TicketCount - " + dtCart.Rows[i]["quantity"].ToString() + " where ProductId = " + dtCart.Rows[i]["productid"].ToString();
                                        dbCon.ExecuteTransaction(sql, CommandType.Text);

                                        //updating TicketCount in ProductInventoryByLocation table
                                        sql = "update ProductInventoryByLocation set TicketCount = TicketCount - " + dtCart.Rows[i]["quantity"].ToString() + " where ProductId = " + dtCart.Rows[i]["productid"].ToString() + " and LocationId = " + Session["LocationId"].ToString();
                                        dbCon.ExecuteTransaction(sql, CommandType.Text);
                                    }
                                    else
                                    {

                                        DataRow drSmart = dtSmart.NewRow();
                                        drSmart["SmartProductCode"] = dtCart.Rows[i]["SmartProductCode"].ToString();
                                        drSmart["SmartProductId"] = dtCart.Rows[i]["ProductId"].ToString();
                                        if (dtCart.Rows[i]["IsAdult"].ToString() == "1")
                                        {
                                            drSmart["SmartAdultQty"] = dtCart.Rows[i]["Quantity"].ToString();
                                            drSmart["SmartChildQty"] = "0";
                                        }
                                        else
                                        {
                                            drSmart["SmartAdultQty"] = "0";
                                            drSmart["SmartChildQty"] = dtCart.Rows[i]["Quantity"].ToString();
                                        }
                                        dtSmart.Rows.Add(drSmart);
                                    }
                                }

                                
                                if (dtSmart.Rows.Count > 0)
                                {
                                    //Order Smart Destination Card Requet

                                    string skuOrderList = "";
                                    string quantityAdult = "";
                                    string quantityChild = "";

                                    
                                    for (int i = 0; i < dtSmart.Rows.Count; i++)
                                    {
                                        if (skuOrderList.IndexOf(dtSmart.Rows[i]["SmartProductCode"].ToString()) < 0)
                                            skuOrderList += (skuOrderList != "" ? "," : "") + dtSmart.Rows[i]["SmartProductCode"].ToString();
                                        if (dtSmart.Rows[i]["SmartAdultQty"].ToString() != "0")
                                            quantityAdult += (quantityAdult != "" ? "," : "") + dtSmart.Rows[i]["SmartAdultQty"].ToString();
                                        if (dtSmart.Rows[i]["SmartChildQty"].ToString() != "0")
                                            quantityChild += (quantityChild != "" ? "," : "") + dtSmart.Rows[i]["SmartChildQty"].ToString();
                                    }
                                    if (quantityAdult == "") quantityAdult = "0";
                                    if (quantityChild == "") quantityChild = "0";
                                    string SmartURL = string.Format("{0}/rest/secure/requestOrderMultipleAttr/skuOrderList/{1}/quantityAdult/{2}/quantityChild/{3}/email/{4}", System.Configuration.ConfigurationManager.AppSettings["smart_destination_url"], skuOrderList, quantityAdult, quantityChild, System.Configuration.ConfigurationManager.AppSettings["smart_destination_login"]);
                                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SmartURL);
                                    request.Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["smart_destination_login"], System.Configuration.ConfigurationManager.AppSettings["smart_destination_password"]);
                                    request.Method = "POST";
                                    HttpWebResponse response = null;
                                    try
                                    {
                                        request.Timeout = 240000; //extend to 4 minutes;
                                        response = (HttpWebResponse)request.GetResponse();
                                    }
                                    catch (Exception ee)
                                    {
                                        Response.Write(ee.Message);

                                    }
                                    if (response.StatusCode == HttpStatusCode.Created)
                                    {
                                        WebHeaderCollection whc = response.Headers;
                                        string location = whc["Location"];
                                        request.Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["smart_destination_login"], System.Configuration.ConfigurationManager.AppSettings["smart_destination_password"]);
                                        request = (HttpWebRequest)WebRequest.Create(location);
                                        System.IO.Stream responseStream = null;

                                        try
                                        {
                                            responseStream = request.GetResponse().GetResponseStream();
                                        }
                                        catch (Exception ex)
                                        {
                                            HttpWebRequest newrequest = (HttpWebRequest)WebRequest.Create(request.Address.AbsoluteUri);
                                            newrequest.Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["smart_destination_login"], System.Configuration.ConfigurationManager.AppSettings["smart_destination_password"]);
                                            responseStream = newrequest.GetResponse().GetResponseStream();
                                        }

                                        string jsonString = null;
                                        using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                                        {
                                            jsonString = reader.ReadToEnd();
                                            reader.Close();
                                            XmlDocument xd = new XmlDocument();
                                            xd = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonString, "skuCodeList");
                                            DataSet ds = new DataSet();
                                            ds.ReadXml(new XmlNodeReader(xd));
                                            PrintURL = ds.Tables[0].Rows[0]["printPassesUrl"].ToString();
                                            SDOrderNo = Convert.ToInt32(ds.Tables[0].Rows[0]["orderNumber"].ToString());
                                            sql = "Update SalesMaster set SDOrderNo = " + SDOrderNo + ", PrintURL = '" + PrintURL + "' where SalesMasterId = " + SalesMasterId;
                                            dbCon.ExecuteTransaction(sql, CommandType.Text);
                                        }
                                    }
                                }


                                //Inserting Payment details
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    string transid = "0";
                                    if (dt.Rows[i]["PaymentType"].ToString() == "credit" && dt.Rows[i]["Response"] != null)
                                    {
                                        string[] ResponseArray = (string[])dt.Rows[i]["Response"];
                                        transid = ResponseArray[6];
                                        TransactionIds += (TransactionIds.Length > 0 ? "," : "") + transid;
                                    }
                                    string CardNo = "";
                                    //string CardName = "";
                                    if (dt.Rows[i]["PaymentType"].ToString() == "credit" )
                                    {
                                        CardNo = dt.Rows[i]["cardno"].ToString().Substring(dt.Rows[i]["cardno"].ToString().Length - 4);
                                        //CardName = dt.Rows[i]["cardname"].ToString();
                                    }
                                    else if (dt.Rows[i]["PaymentType"].ToString() == "voucher")
                                    {
                                        CardNo = dt.Rows[i]["cardno"].ToString();
                                    }
                                    
                                    if (dt.Rows[i]["PaymentType"].ToString() == "cash")
                                        payment_type = "1";
                                    else if (dt.Rows[i]["PaymentType"].ToString() == "credit")
                                        payment_type = "2";
                                    else if (dt.Rows[i]["PaymentType"].ToString() == "voucher")
                                        payment_type = "3";

                                    sql = @"insert into PaymentDetails (SalesMasterId, PaymentType, Amount, CardCheckNo, CardName, CardAdd1, CardAdd2, CardCity, CardState, CardZip, ExpiryMonth, ExpiryYear, CardType, CardAuthorizationId) values 
                                        (" + SalesMasterId + ", " + payment_type + ", " + dt.Rows[i]["amount"].ToString() + ", '" + CardNo + "',  '" + dt.Rows[i]["cardname"].ToString() + @"', 
                                        '', '', '', '', '', 0, 0, '" + dt.Rows[i]["CardType"].ToString() + "', " + transid + ")";

                                    int PaymentId = dbCon.ExecuteTransactionGetID(sql, CommandType.Text);

                                    if (dt.Rows[i]["PaymentType"].ToString() == "credit")
                                    {
                                        sql = "update PaymentResponse set PaymentId = " + PaymentId + " where CartId = " + Session["CartId"] + " and GuId = '" + dt.Rows[i]["Id"].ToString() + "'";
                                        dbCon.ExecuteTransaction(sql, CommandType.Text);
                                    }
                                }
                            }
                        }

                        lblMsg.Text = "<p style='padding-bottom:0; margin-bottom:0; font-size:16px; font-weight:bold;'>Payment succefully authorized.</p>";
                        
                        /*
                        string payment_description = "";
                        if (payment_type == "1")
                            payment_description = "Cash payment received";
                        else if (payment_type == "2")
                            payment_description = "CC payment authorization (#" + TransactionIds + ")";
                        else if (payment_type == "3")
                            payment_description = "Voucher payment received, " + dt.Rows[0]["CardNo"].ToString();
                        lblMsg.Text += "<p style='padding:0; margin:0; font-size:13px; font-weight:normal; padding-left:20px;'>" + payment_description + "</p>";
                        */

                        string payment_description = "";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["paymenttype"].ToString() == "cash")
                                payment_description = "Cash payment received";
                            else if (dt.Rows[i]["paymenttype"].ToString() == "credit")
                                payment_description = "CC payment authorization (#" + TransactionIds + ")";
                            else if (dt.Rows[i]["paymenttype"].ToString() == "voucher")
                                payment_description = "Voucher payment received, " + dt.Rows[i]["CardNo"].ToString();
                            lblMsg.Text += "<p style='padding:0; margin:0; font-size:13px; font-weight:normal; padding-left:20px;'>" + payment_description + " for $" + Convert.ToDecimal(dt.Rows[i]["amount"].ToString()).ToString("#0.00") + "</p>";
                        }

                        
                        lblMsg.Text += "<p style='padding:0; padding-top:5px; margin:0; font-size:13px; font-weight:bold; padding-left:20px;'>eVents transaction ID: " + SalesMasterId + "</p>";
                        lblMsg.Text += "<p style='padding-bottom:0; margin-bottom:0; font-size:16px; font-weight:bold;'>Ticket inventory adjusted.</p>";

                        sql = "select P.name, S.serialNumber from ProductMaster P, ShoppingCartSerialNo S where P.ProductId = S.ProductId and IsPhysicalInventory = 0 and S.CurrentCartId = " + Session["CartId"];
                        DataTable dtSNo = dbCon.ExecuteTransactionDataSet(sql, CommandType.Text).Tables[0];
                        for (int i = 0; i < dtSNo.Rows.Count; i++)
                        {
                            lblMsg.Text += "<p style='padding:0; margin:0; font-size:13px; font-weight:normal; padding-left:20px;'>" + dtSNo.Rows[i]["name"].ToString() + " s/n " + dtSNo.Rows[i]["serialNumber"].ToString() + "<span style='color:green; '><i> marked as sold</i></span></p>";
                        }
                        sql = "select P.name, DATE_FORMAT(S.expirydate,'%m/%d/%Y') as expirydate, Count(*) as Qty from ProductMaster P, ShoppingCartSerialNo S where P.ProductId = S.ProductId and IsPhysicalInventory = 1 and S.CurrentCartId = " + Session["CartId"] + " group by P.name, S.expirydate";
                        dtSNo = dbCon.ExecuteTransactionDataSet(sql, CommandType.Text).Tables[0];
                        for (int i = 0; i < dtSNo.Rows.Count; i++)
                        {
                            lblMsg.Text += "<p style='padding:0; margin:0; font-size:13px; font-weight:normal; padding-left:20px;'>" + dtSNo.Rows[i]["name"].ToString() + " Expiry date: " + dtSNo.Rows[i]["expirydate"].ToString() + ", Qty: " + dtSNo.Rows[i]["Qty"].ToString() + "<span style='color:green; '><i> marked as sold</i></span></p>";
                        }


                        dtSNo = null;

                        lblMsg.Text += "<p style='font-size:18px; font-weight:bold; color:green;'>Please give tickets to the  customer.</p>";

                        PrintReceipt.Visible = true;
                        btnProceed.Visible = true;
                        btnBackPayment.Visible = false;
                        dbCon.CommitTransaction();

                        string sScript = "window.open('/Reports/Reports.aspx?ReportName=Receipt&DataSource=Receipt&SalesId=" + SalesMasterId + "', 'Receipt', 'statusbar=no'); window.open('/Reports/Reports.aspx?ReportName=Receipt&DataSource=Receipt&Dup=yes&SalesId=" + SalesMasterId + "', 'ReceiptCopy', 'statusbar=no'); return false; ";
                        PrintReceipt.Attributes.Add("onclick", sScript);

                        if (PrintURL != "")
                        {
                            sScript = "window.open('Viewpdf.aspx?filepath=" + Server.UrlEncode(PrintURL) + "', 'SDReceipt', 'status=no,resizable=yes,menubar=yes'); return false; ";
                            btnPrintPDF.Attributes.Add("onclick", sScript);
                            btnPrintPDF.Visible = true;
                        }
                        Session["CartId"] = "";
                        Session["Payment"] = null;
                        Session["AmountDue"] = "";
                        Session["AmountRemaining"] = "";
                        Session["SalesMasterId"] = "";

                        //string sScript = "<script>window.open('/Reports/Reports.aspx?ReportName=Receipt&DataSource=Receipt&SalesId=" + SalesMasterId + "', 'Receipt', 'statusbar=no');</script>";
                        //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "popupScript", sScript);
                        //Response.Write("<script>window.open('/Reports/Reports.aspx?ReportName=Receipt&DataSource=Receipt&SalesId=" + SalesMasterId + "', 'Receipt', 'statusbar=no');</script>");
                    }
                    catch (Exception)
                    {
                        dbCon.RollBackTransaction();
                    }
                    dbCon = null;
                }
                else
                {
                    lblMsg.Text = "Payment could not be authorized.<br /><br />";
                    PrintReceipt.Visible = false;
                    btnProceed.Visible = false;
                    btnBackPayment.Visible = true;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["Status"].ToString() == "1")
                        {
                            string ErrorResponse = "";
                            if (dt.Rows[i]["Response"] != null)
                            {
                                string[] ResponseArray = (string[])dt.Rows[i]["Response"];
                                ErrorResponse = ResponseArray[3];
                            }
                            lblMsg.Text += "<br />" + dt.Rows[i]["CardType"] + " " + dt.Rows[i]["Description"] + ": " + ErrorResponse;
                        }
                    }
                    //lblMsg.Text += "<br /><br /><span style='font-weight:bold; font-size:16px;'>Click on the payment to edit</span>";
                }
            }
            
            
            //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "popupScript", sScript);

        }

        protected void bNewSales_Click(object sender, EventArgs e)
        {
            Session["CartId"] = "";
            Session["Payment"] = null;
            Session["AmountDue"] = "";
            Session["AmountRemaining"] = "";
            Session["SalesMasterId"] = "";

            Response.Redirect("../sales/index.aspx");
        }

        protected void btnBackPayment_Click(object sender, EventArgs e)
        {
            Session["Payment"] = null;
            Session["AmountRemaining"] = Session["AmountDue"];
            Response.Redirect("paymentoptions.aspx");
        }

         
    }
}