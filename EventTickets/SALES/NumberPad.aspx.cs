using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EventFunctions;
using MYSQLDatabase;
using System.Data;

namespace EventTickets.Sales
{
    public partial class NumberPad : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ("" + Session["userid"] == "")
                Response.Write("<script>window.close()</script>");

            if ("" + Request.QueryString["type"] == "MTA") return;
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["BalanceAmt"])) number.Value = Request.QueryString["BalanceAmt"].Replace("$", "");
            }
        }

        protected void bOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["type"] == "paymentoptions")
                {   //payment options

                    if (Request.QueryString["paymenttype"] == "cash")
                    {
                        /*
                        DBConnection DBConn = new DBConnection();
                        string sql = "insert into ShoppingCartPayment (CartId, PaymentType, Amount, SellDate) values (" + Session["CartId"] + ", '" + Request.QueryString["paymenttype"] + "', " + number.Value + ", now())";
                        DBConn.Execute(sql, CommandType.Text);
                        DBConn = null;
                        */

                        classes.Payment payment = new classes.Payment();
                        payment.CartId = Functions.ToInt(Session["CartId"].ToString());
                        payment.PaymentType = "cash";
                        payment.Amount = Functions.ToDecimal(number.Value);
                        payment.SetPayment();

                        Response.Write("<script>window.opener.location.href='paymentoptionsNew.aspx'; window.close();</script>");
                    }
                    else if (Request.QueryString["paymenttype"] == "credit")
                    {
                        Response.Redirect("credit.aspx?amount=" + number.Value);
                        //Response.Write("<script>window.open('credit.aspx?amount=" + Functions.ToInt(number.Value) + "' ,'credit', 'width=450,height=250'); window.close();</script>");
                    }
                    else if (Request.QueryString["paymenttype"] == "voucher")
                    {
                        Response.Redirect("voucher.aspx?amount=" + number.Value);
                        //Response.Write("<script>window.open('credit.aspx?amount=" + Functions.ToInt(number.Value) + "' ,'credit', 'width=450,height=250'); window.close();</script>");
                    }
                    return;
                }
                else if (Request.QueryString["qtyaddtype"] == "A")//Adding Adult Qty
                {
                    int pid = Functions.ToInt(Request.QueryString["pid"]);
                    int cid = Functions.ToInt(Request.QueryString["cid"]);
                    if (pid <= 0) { Response.Write("<script>window.close()</script>"); return; }

                    if (Functions.ToInt(number.Value) > 0)
                    {
                        decimal priceadult = Functions.ToDecimal(Request.QueryString["priceadult"]);
                        decimal pricechild = Functions.ToDecimal(Request.QueryString["pricechild"]);
                        string productname = "" + Request.QueryString["productname"];
                        string productCode = "" + Request.QueryString["productCode"];
                        DBConnection DBConn = new DBConnection();
                        if (Functions.ToInt(Session["CartId"]) > 0)
                        {
                            DataTable DTE = DBConn.ExecuteDataSet("SELECT * FROM ShoppingCart WHERE ProductId = " + pid.ToString() + " AND CurrentCartId = " + Session["CartId"], CommandType.Text).Tables[0];
                            if (DTE.Rows.Count > 0)
                                DBConn.Execute("UPDATE ShoppingCart SET Quantity = Quantity + " + Functions.ToInt(number.Value) + " WHERE ProductId = " + pid.ToString() + " AND CurrentCartId = " + Session["CartId"], CommandType.Text);
                            else
                                DBConn.RetData("INSERT INTO ShoppingCart (SalesDate,ProductId,ProductPrice,ServiceFee,ClerkId,CurrentCartId,Quantity,ChildProductPrice,ChildQuantity,ProductName,IsSmartDestination,SmartProductCode) VALUES (NOW()," + pid + "," + priceadult + ",0," + Session["userid"] + "," + Session["CartId"] + "," + Functions.ToInt(number.Value) + "," + pricechild + ",0,'" + productname.Replace("'", "''") + "',1,'" + productCode.Replace("'", "''") + "');select last_insert_id();", CommandType.Text);
                        }
                        else
                        {
                            string sCurrentCartId = DBConn.RetData("INSERT INTO ShoppingCart (SalesDate,ProductId,ProductPrice,ServiceFee,ClerkId,Quantity,ChildProductPrice,ChildQuantity,ProductName,IsSmartDestination,SmartProductCode) VALUES (NOW()," + pid + "," + priceadult + ",0," + Session["userid"] + "," + Functions.ToInt(number.Value) + "," + pricechild + ",0,'" + productname.Replace("'", "''") + "',1,'" + productCode.Replace("'", "''") + "');select last_insert_id();", CommandType.Text);
                            Session["CartId"] = sCurrentCartId;
                            DBConn.Execute("UPDATE ShoppingCart SET CurrentCartId = " + sCurrentCartId + " WHERE CartId = " + sCurrentCartId, CommandType.Text);
                        }
                        DBConn = null;
                        Response.Write("<script>window.opener.location.href='SmartSalesDetails.aspx?cid=" + cid + "&eid="+ pid +"'; window.close();</script>"); return;                        
                    }                    
                }
                else if (Request.QueryString["qtyaddtype"] == "C")//Adding Child Qty
                {
                    int pid = Functions.ToInt(Request.QueryString["pid"]);
                    int cid = Functions.ToInt(Request.QueryString["cid"]);
                    if (pid <= 0) { Response.Write("<script>window.close()</script>"); return; }

                    if (Functions.ToInt(number.Value) > 0)
                    {
                        decimal pricechild = Functions.ToDecimal(Request.QueryString["pricechild"]);
                        decimal priceadult = Functions.ToDecimal(Request.QueryString["priceadult"]);
                        string productname = "" + Request.QueryString["productname"];
                        string productCode = "" + Request.QueryString["productCode"];
                        DBConnection DBConn = new DBConnection();
                        if (Functions.ToInt(Session["CartId"]) > 0)
                        {
                            DataTable DTE = DBConn.ExecuteDataSet("SELECT * FROM ShoppingCart WHERE ProductId = " + pid.ToString() + " AND CurrentCartId = " + Session["CartId"], CommandType.Text).Tables[0];
                            if (DTE.Rows.Count > 0)
                                DBConn.Execute("UPDATE ShoppingCart SET ChildQuantity = ChildQuantity + " + Functions.ToInt(number.Value) + " WHERE ProductId = " + pid.ToString() + " AND CurrentCartId = " + Session["CartId"], CommandType.Text);
                            else
                                DBConn.RetData("INSERT INTO ShoppingCart (SalesDate,ProductId,ProductPrice,ServiceFee,ClerkId,CurrentCartId,Quantity,ChildProductPrice,ChildQuantity,ProductName,IsSmartDestination,SmartProductCode) VALUES (NOW()," + pid + "," + priceadult + ",0," + Session["userid"] + "," + Session["CartId"] + ",0," + pricechild + "," + Functions.ToInt(number.Value) + ",'" + productname.Replace("'", "''") + "',1,'" + productCode.Replace("'", "''") + "');select last_insert_id();", CommandType.Text);
                        }
                        else
                        {
                            string sCurrentCartId = DBConn.RetData("INSERT INTO ShoppingCart (SalesDate,ProductId,ProductPrice,ServiceFee,ClerkId,Quantity,ChildProductPrice,ChildQuantity,ProductName,IsSmartDestination,SmartProductCode) VALUES (NOW()," + pid + "," + priceadult + ",0," + Session["userid"] + ",0," + pricechild + "," + Functions.ToInt(number.Value) + ",'" + productname.Replace("'", "''") + "',1,'" + productCode.Replace("'", "''") + "');select last_insert_id();", CommandType.Text);
                            Session["CartId"] = sCurrentCartId;
                            DBConn.Execute("UPDATE ShoppingCart SET CurrentCartId = " + sCurrentCartId + " WHERE CartId = " + sCurrentCartId, CommandType.Text);
                        }
                        DBConn = null;
                        Response.Write("<script>window.opener.location.href='SmartSalesDetails.aspx?cid=" + cid + "&eid=" + pid + "'; window.close();</script>"); return;
                    } 
                }
                else
                {   // add to cart
                    int pid = Functions.ToInt(Request.QueryString["pid"]);
                    if (pid <= 0) { Response.Write("<script>window.close()</script>"); return; }
                    dmsg.InnerHtml = "";
                    DBConnection DBConn = new DBConnection();
                    string sLocLocationId = Functions.ToInt(Functions.GetLocationId()).ToString();
                    int qtyRemain = Functions.ToInt(DBConn.RetData("SELECT TicketCount FROM ProductInventoryByLocation WHERE ProductInventoryByLocation.ProductId = " + pid.ToString() + " AND LocationId =  " + sLocLocationId, CommandType.Text));
                    qtyRemain -= Functions.ToInt(DBConn.RetData("SELECT Quantity from ShoppingCart where ProductId = " + pid.ToString() + " and CurrentCartId = " + Functions.ToInt(Session["CartId"]), CommandType.Text));
                    if (Functions.ToInt(number.Value) > qtyRemain)
                    {
                        dmsg.InnerHtml = "<script>alert('Only " + qtyRemain + " exists in inventory. Please enter another quantity');</script>"; return;                        
                    }
                    string sql = "SELECT ProductMaster .*,TicketCount as Remaining FROM ProductMaster  WHERE ProductId = " + pid.ToString() + "";// AND Status = 1 ";
                    DataTable DT = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                    if (DT.Rows.Count <= 0)
                    {
                        Response.Write("<script>window.close()</script>");
                        return;
                    }

                    if (Functions.ToInt(Session["CartId"]) > 0)
                    {
                        DataTable DTE = DBConn.ExecuteDataSet("SELECT * FROM ShoppingCart WHERE ProductId = " + pid.ToString() + " AND CurrentCartId = " + Session["CartId"], CommandType.Text).Tables[0];
                        if (DTE.Rows.Count > 0)
                            DBConn.Execute("UPDATE ShoppingCart SET Quantity = Quantity + " + Functions.ToInt(number.Value) + " WHERE ProductId = " + pid.ToString() + " AND CurrentCartId = " + Session["CartId"], CommandType.Text);
                        else
                            DBConn.RetData("INSERT INTO ShoppingCart (SalesDate,ProductId,ProductPrice,ServiceFee,ClerkId,CurrentCartId,Quantity) VALUES (NOW()," + pid + "," + DT.Rows[0]["Price"].ToString() + "," + Functions.ToDecimal(DT.Rows[0]["ServiceFee"].ToString()) + "," + Session["userid"] + "," + Session["CartId"] + "," + Functions.ToInt(number.Value) + ");select last_insert_id();", CommandType.Text);
                    }
                    else
                    {
                        string sCurrentCartId = DBConn.RetData("INSERT INTO ShoppingCart (SalesDate,ProductId,ProductPrice,ServiceFee,ClerkId,Quantity) VALUES (NOW()," + pid + "," + DT.Rows[0]["Price"].ToString() + "," + Functions.ToDecimal(DT.Rows[0]["ServiceFee"].ToString()) + "," + Session["userid"] + "," + Functions.ToInt(number.Value) + ");select last_insert_id();", CommandType.Text);
                        Session["CartId"] = sCurrentCartId;
                        DBConn.Execute("UPDATE ShoppingCart SET CurrentCartId = " + sCurrentCartId + " WHERE CartId = " + sCurrentCartId, CommandType.Text);
                    }
                    DBConn = null;
                    Response.Write("<script>window.opener.location.href='Sales.aspx?cid=" + DT.Rows[0]["CategoryId"].ToString() + "'; window.close();</script>"); return;
                }
            }
            catch (Exception)
            {

            }
        }
    }
}