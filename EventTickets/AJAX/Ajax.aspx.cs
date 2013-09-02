using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EventFunctions;
using MYSQLDatabase;
using System.Data;

namespace EventTickets.Ajax
{
    public partial class Ajax : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sType = "" + Request.QueryString["Type"];
            DBConnection DBConn;
            DataTable DT;
            switch (sType)
            {
                case "addTicket":
                    string SerialNo = "" + Request.QueryString["SerialNo"];
                    string ProductId = "" + Request.QueryString["ProductId"];
                    if (Functions.ToInt(Session["CartId"]) > 0 && Functions.ToInt(ProductId) > 0 && SerialNo.Trim() != "")
                    {
                        DBConn = new DBConnection();
                        int iRemain = Functions.ToInt(DBConn.RetData("SELECT COUNT(1) FROM ShoppingCartSerialNo WHERE CurrentCartId = " + Functions.ToInt(Session["CartId"]) + " AND ProductId = " + Functions.ToInt(ProductId), CommandType.Text));
                        int iTotal = Functions.ToInt(DBConn.RetData("SELECT Quantity FROM ShoppingCart WHERE CurrentCartId = " + Functions.ToInt(Session["CartId"]) + " AND ProductId = " + Functions.ToInt(ProductId), CommandType.Text));
                        bool isValid = false;
                        bool isExcess = false;
                        //Response.Write(iTotal +"<br>");
                        //Response.Write(iRemain + "<br>");
                        //Response.Write(ProductId + "<br>");
                        if (iTotal - iRemain > 0)
                        {
                            if (Functions.ToInt(DBConn.RetData("SELECT ProductDetailId FROM ProductDetail WHERE ProductId = " + ProductId + " AND SerialNo = '" + SerialNo.Replace("'", "''") + "' AND Status in (0,3) ", CommandType.Text)) > 0)
                            {
                                //int iRAff = Functions.ToInt(DBConn.Execute("UPDATE ProductDetail SET Status = 1 WHERE ProductId = " + ProductId + " AND SerialNo = '" + SerialNo.Replace("'", "''") + "' AND Status = 0 ", CommandType.Text));
                                string sSno = DBConn.RetData("SELECT SerialNumber FROM ShoppingCartSerialNo WHERE CurrentCartId = " + Session["CartId"] + " AND ProductId = " + ProductId + " AND  SerialNumber = '" + SerialNo.Replace("'", "''") + "' ", CommandType.Text);
                                //if (iRAff > 0)
                                if (sSno == "")
                                {
                                    DBConn.Execute("INSERT INTO ShoppingCartSerialNo (CurrentCartId, ProductId, SerialNumber) VALUES (" + Session["CartId"] + "," + ProductId + ",'" + SerialNo.Replace("'", "''") + "')", CommandType.Text);
                                    iRemain++;
                                    isValid = true;
                                }
                            }
                        }
                        if (iTotal - iRemain <= 0)
                        {
                            DBConn.Execute("UPDATE ShoppingCart SET IsSerialNoEntered = 1 WHERE CurrentCartId = " + Session["CartId"] + " AND ProductId = " + ProductId, CommandType.Text);
                            isExcess = true;
                        }
                        if (isValid)
                        {
                            DT = DBConn.ExecuteDataSet("SELECT * FROM ShoppingCartSerialNo WHERE CurrentCartId = " + Session["CartId"] + " AND ProductId = " + ProductId, CommandType.Text).Tables[0];
                            Response.Write("<table>");
                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                Response.Write("<tr><td>" + DT.Rows[i]["SerialNumber"] + "</td><td style='padding-left:5px;color:#005928'>Verified in inventory</td></tr>");
                            }
                            Response.Write("</table>");
                            Response.Write("~");
                            Response.Write(iTotal - iRemain);
                        }
                        else if (isExcess)
                            Response.Write("limit Reached");
                        else
                            Response.Write("Invalid Serial Number");
                    }
                    Response.End();
                    break;

                case "addCards":


                    string ExpDate = Convert.ToDateTime("" + Request.QueryString["ExpDate"]).ToString("yyyy-MM-dd");
                    int Qty = Convert.ToInt16(Request.QueryString["Qty"]);
                    ProductId = "" + Request.QueryString["ProductId"];
                    SerialNo = "";

                    if (Functions.ToInt(Session["CartId"]) > 0 && Functions.ToInt(ProductId) > 0 && ExpDate.Trim() != "")
                    {
                        DBConn = new DBConnection();
                        int iRemain = Functions.ToInt(DBConn.RetData("SELECT COUNT(1) FROM ShoppingCartSerialNo WHERE CurrentCartId = " + Functions.ToInt(Session["CartId"]) + " AND ProductId = " + Functions.ToInt(ProductId), CommandType.Text));
                        int iTotal = Functions.ToInt(DBConn.RetData("SELECT Quantity FROM ShoppingCart WHERE CurrentCartId = " + Functions.ToInt(Session["CartId"]) + " AND ProductId = " + Functions.ToInt(ProductId), CommandType.Text));
                        bool isValid = false;
                        bool isExcess = false;
                        //Response.Write(iTotal +"<br>");
                        //Response.Write(iRemain + "<br>");
                        //Response.Write(ProductId + "<br>");
                        if (iTotal - iRemain > 0)
                        {

                            DataTable dtSNo = DBConn.ExecuteDataSet("Select * from ProductDetail where LocationId = " + Session["LocationId"] + " and ProductId = " + ProductId + " and expirydate = '" + ExpDate + "' and status in (0,3) order by SerialNo Limit " + Qty, CommandType.Text).Tables[0];
                            for (int i = 0; i < dtSNo.Rows.Count; i++)
                            {
                                if (Functions.ToInt(DBConn.RetData("SELECT ProductDetailId FROM ProductDetail WHERE ProductId = " + ProductId + " AND SerialNo = '" + dtSNo.Rows[i]["SerialNo"].ToString() + "' AND Status in (0,3) ", CommandType.Text)) > 0)
                                {
                                    //int iRAff = Functions.ToInt(DBConn.Execute("UPDATE ProductDetail SET Status = 1 WHERE ProductId = " + ProductId + " AND SerialNo = '" + SerialNo.Replace("'", "''") + "' AND Status = 0 ", CommandType.Text));
                                    string sSno = DBConn.RetData("SELECT SerialNumber FROM ShoppingCartSerialNo WHERE CurrentCartId = " + Session["CartId"] + " AND ProductId = " + ProductId + " AND  SerialNumber = '" + dtSNo.Rows[i]["SerialNo"].ToString() + "' and expirydate = '" + ExpDate + "'", CommandType.Text);
                                    //if (iRAff > 0)
                                    if (sSno == "")
                                    {
                                        DBConn.Execute("INSERT INTO ShoppingCartSerialNo (CurrentCartId, ProductId, SerialNumber,ExpiryDate) VALUES (" + Session["CartId"] + "," + ProductId + ",'" + dtSNo.Rows[i]["SerialNo"].ToString() + "', '" + ExpDate + "')", CommandType.Text);
                                        iRemain++;
                                        isValid = true;
                                    }
                                }

                            }
                        }
                        if (iTotal - iRemain <= 0)
                        {
                            DBConn.Execute("UPDATE ShoppingCart SET IsSerialNoEntered = 1 WHERE CurrentCartId = " + Session["CartId"] + " AND ProductId = " + ProductId, CommandType.Text);
                            isExcess = true;
                        }
                        if (isValid)
                        {
                            DT = DBConn.ExecuteDataSet("SELECT DATE_FORMAT(expirydate,'%m/%d/%Y') as expirydate, Count(*) as Qty FROM ShoppingCartSerialNo WHERE CurrentCartId = " + Session["CartId"] + " AND ProductId = " + ProductId + " group by ExpiryDate", CommandType.Text).Tables[0];
                            Response.Write("<table width='100%' cellpadding='2' border='0' cellspacing='1' bgcolor='#DDD'><tr style='color:#FFF; background-color:#999999;'><td>Expiry Date</td><td>Quantity</td><td>Status</td></tr>");

                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                Response.Write("<tr style='background-color:#FFF'><td>" + DT.Rows[i]["expirydate"].ToString() + "</td><td style='text-align:left'>" + DT.Rows[i]["Qty"].ToString() + "</td><td style='padding-left:5px;color:#005928'>Verified in inventory</td></tr>");
                            }
                            Response.Write("</table>");
                            Response.Write("~");
                            Response.Write(iTotal - iRemain);
                        }
                        else if (isExcess)
                            Response.Write("limit Reached");
                        else
                            Response.Write("Invalid Serial Number");
                    }
                    Response.End();

                    break;

                case "ValidatePayment":

                    if (Request.QueryString["paymenttype"] == "cash")
                    {
                        classes.Payment payment = new classes.Payment();
                        DataTable dtPayment = payment.GetPayment();
                        if (dtPayment != null)
                        {
                            DataRow[] dr = dtPayment.Select("PaymentType='cash'");
                            if (dr.Length > 0)
                            {
                                Response.Write("1");
                                Response.End();
                            }
                        }
                    }

                    if (Functions.ToDecimal(Session["AmountRemaining"]) == 0)
                        Response.Write("2");
                    else
                        Response.Write("");

                    Response.End();
                    break;

                case "AddCashPayment":

                    classes.Payment objPayment = new classes.Payment();
                    objPayment.CartId = Functions.ToInt(Session["CartId"].ToString());
                    objPayment.PaymentType = "cash";
                    objPayment.Amount = Functions.ToDecimal(Session["AmountDue"].ToString());
                    objPayment.SetPayment();

                    Response.End();
                    break;

                case "getProducts":
                    Response.Write("<select ID='dProduct' onchange='LoadStock()' style='width:200px'>");
                    Response.Write("<option value=''><--Select Product-->");
                    string Category = "" + Request.QueryString["Category"];
                    DBConn = new DBConnection();
                    string sql = "SELECT ProductId,Name FROM ProductMaster WHERE CategoryId = " + EventFunctions.Functions.ToInt(Category) + " order by Name";

                    DT = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write("<option value='" + DT.Rows[i]["ProductId"] + "'>" + DT.Rows[i]["Name"]);
                    }
                    DBConn = null;
                    Response.Write("</select>");
                    Response.End();
                    break;
                case "checkDuplicate":
                    string Location = Request.QueryString["Location"];
                    string Product = Request.QueryString["Product"];
                    string startserial = Request.QueryString["startserial"];
                    string endserial = Request.QueryString["endserial"];
                    string sSql = "";
                    if (Location != "" && Product != "" && startserial != "" && endserial != "")
                    {
                        DBConn = new DBConnection();
                        try
                        {
                            sSql = "SELECT SerialNo FROM ProductDetail WHERE ProductId =" + Product + " and Status = 0 and SerialNo between " + startserial + " and " + endserial;
                            DT = DBConn.ExecuteDataSet(sSql, CommandType.Text).Tables[0];
                            if (DT.Rows.Count > 0)
                            {
                                Response.Write("The Following Duplicates serial# were found, We cannot proceed\n");
                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    Response.Write(DT.Rows[i]["SerialNo"].ToString());
                                    if (i < DT.Rows.Count - 1)
                                        Response.Write(", ");
                                }
                            }
                            else
                                Response.Write("1");
                        }
                        catch (Exception)
                        {
                            Response.Write("1");
                        }
                        DBConn = null;
                    }
                    Response.End();
                    break;
                case "addStock":
                    Location = Request.QueryString["Location"];
                    Product = Request.QueryString["Product"];
                    string Serail = Request.QueryString["Serail"];
                    string sUserId = Functions.GetSessionUserId();
                    if (Location != "" && Product != "" && Serail != "")
                    {
                        DBConn = new DBConnection();
                        try
                        {
                            DBConn.BeginTransaction();
                            DT = DBConn.ExecuteTransactionDataSet("SELECT SerialNo FROM ProductDetail WHERE ProductId =" + Product + " AND SerialNo = '" + Serail.Replace("'", "''") + "' AND Status = 0", CommandType.Text).Tables[0];
                            if (DT.Rows.Count == 0)
                            {
                                DBConn.ExecuteTransaction("insert into ProductDetail (ProductId,SerialNo,UserId,Status,LocationId,PurchaseDate) values (" + Product + ",'" + Serail.Replace("'", "''") + "'," + sUserId + ",0," + Location + ", now())", CommandType.Text);
                                DBConn.ExecuteTransaction("UPDATE ProductInventoryByLocation SET TicketCount = TicketCount + 1 WHERE ProductId = " + Product + " AND LocationId = " + Location, CommandType.Text);
                                DBConn.ExecuteTransaction("UPDATE ProductMaster SET TicketCount = TicketCount + 1 WHERE ProductId = " + Product, CommandType.Text);
                            }
                            DBConn.CommitTransaction();
                        }
                        catch (Exception)
                        {
                            DBConn.RollBackTransaction();
                        }
                        DBConn = null;
                    }
                    Response.Write("1");
                    break;

                case "addNoPhyStock":
                    Location = Request.QueryString["Location"];
                    Product = Request.QueryString["Product"];
                    Category = Request.QueryString["Category"];
                    int Tickets = Functions.ToInt(Request.QueryString["Tickets"]);
                    ExpDate = Request.QueryString["ExpDate"];
                    sUserId = Functions.GetSessionUserId();
                    if (Location != "" && Product != "" && Tickets > 0 && ExpDate != "")
                    {
                        DBConn = new DBConnection();
                        try
                        {
                            DBConn.BeginTransaction();
                            for (int i = 0; i < Tickets; i++)
                            {
                                int iPdtId = DBConn.ExecuteTransactionGetID("insert into ProductDetail (ProductId,UserId,Status,LocationId,PurchaseDate,ExpiryDate) values (" + Product + "," + sUserId + ",0," + Location + ", now(),'" + Convert.ToDateTime(ExpDate).ToString("yyyy-MM-dd") + "')", CommandType.Text);
                                DBConn.ExecuteTransaction("UPDATE ProductDetail SET SerialNo = '" + iPdtId + "' WHERE ProductDetailId = " + iPdtId, CommandType.Text);
                                DBConn.ExecuteTransaction("UPDATE ProductInventoryByLocation SET TicketCount = TicketCount + 1 WHERE ProductId = " + Product + " AND LocationId = " + Location, CommandType.Text);
                                DBConn.ExecuteTransaction("UPDATE ProductMaster SET TicketCount = TicketCount + 1 WHERE ProductId = " + Product, CommandType.Text);
                            }
                            DBConn.CommitTransaction();
                        }
                        catch (Exception)
                        {
                            DBConn.RollBackTransaction();
                        }
                        DBConn = null;
                    }
                    break;

                case "loadStock":
                    Location = Request.QueryString["Location"];
                    Product = Request.QueryString["Product"];
                    Category = Request.QueryString["Category"];
                    DBConn = new DBConnection();
                    int IsPhysicalInventory = Functions.ToInt(DBConn.RetData("SELECT IsPhysicalInventory from Category where CategoryId = " + Functions.ToInt(Category), CommandType.Text));
                    if (IsPhysicalInventory == 0)
                    {
                        sSql = "SELECT count(SerialNo) as SerialNo,(SELECT Name from LocationMaster WHERE LocationMaster.LocationId = ProductDetail.LocationId) AS Location,(select Name from ProductMaster where ProductMaster.ProductId = ProductDetail.ProductId) as Name,ExpiryDate as Category FROM ProductDetail WHERE Status = 0 and ProductId = " + Functions.ToInt(Product);
                        if (Location != "")
                            sSql += " AND LocationId = " + Location;
                        sSql += " GROUP BY LocationId, ProductId, ExpiryDate, Status ORDER BY ExpiryDate ";
                        Response.Write("<table width='100%' class='grid' cellpadding='0' cellspacing='0'><tr><th>Tickets Count</th><th>Product</th><th>Expiry Date</th><th>Location</th></tr>");
                        DT = DBConn.ExecuteDataSet(sSql, CommandType.Text).Tables[0];
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write("<tr><td style='width:60px; text-align:right; padding-right:30px'>&nbsp;" + DT.Rows[i]["SerialNo"] + "</td><td>" + DT.Rows[i]["Name"] + "</td><td>" + Convert.ToDateTime(DT.Rows[i]["Category"].ToString()).ToString("MMMM dd, yyyy") + "</td><td>" + DT.Rows[i]["Location"] + "</td></tr>");//<a href='javascript:void(0)' onclick='RemoveSerail(" + DT.Rows[i]["ProductDetailId"] + ")'>X</a>
                        }
                    }
                    else
                    {
                        sSql = "SELECT ProductDetailId,SerialNo,(SELECT Name from LocationMaster WHERE LocationMaster.LocationId = ProductDetail.LocationId) AS Location,(select Name from ProductMaster where ProductMaster.ProductId = ProductDetail.ProductId) as Name,(Select Name from Category where Category.CategoryId = (select CategoryId from ProductMaster where ProductMaster.ProductId = ProductDetail.ProductId)) as Category FROM ProductDetail WHERE Status = 0 and ProductId = " + Functions.ToInt(Product);
                        if (Location != "")
                            sSql += " AND LocationId = " + Location;
                        sSql += " ORDER BY SerialNo ";
                        Response.Write("<table width='100%' class='grid' cellpadding='0' cellspacing='0'><tr><th>Serial Number</th><th>Product</th><th>Category</th><th>Location</th></tr>");
                        DT = DBConn.ExecuteDataSet(sSql, CommandType.Text).Tables[0];
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write("<tr><td >&nbsp;" + DT.Rows[i]["SerialNo"] + "</td><td>" + DT.Rows[i]["Name"] + "</td><td>" + DT.Rows[i]["Category"] + "</td><td>" + DT.Rows[i]["Location"] + "</td></tr>");//<a href='javascript:void(0)' onclick='RemoveSerail(" + DT.Rows[i]["ProductDetailId"] + ")'>X</a>
                        }
                    }

                    //if (DT.Rows.Count > 0)
                    //Response.Write("<tr><td colspan='3'><input type='button' value='Remove Selected' onclick='RemoveSerail()'/></td></tr>");                   
                    Response.Write("<table>~" + DT.Rows.Count + "~" + IsPhysicalInventory);
                    DBConn = null;
                    break;
                case "loadStockBySerial":
                    startserial = Request.QueryString["startserial"];
                    endserial = Request.QueryString["endserial"];
                    sSql = "SELECT ProductDetailId,SerialNo,(SELECT Name from LocationMaster WHERE LocationMaster.LocationId = ProductDetail.LocationId) AS Location, ProductMaster.Name as Name,Category.Name as Category FROM ProductDetail,ProductMaster,Category WHERE ProductMaster.ProductId = ProductDetail.ProductId and Category.CategoryId = ProductMaster.CategoryId  and ProductDetail.Status = 0 and Category.IsPhysicalInventory = 1 and SerialNo between " + startserial + " and " + endserial;
                    DBConn = new DBConnection();
                    DT = DBConn.ExecuteDataSet(sSql, CommandType.Text).Tables[0];
                    Response.Write("<table width='100%' class='grid' cellpadding='0' cellspacing='0'><tr><th>Serial Number</th><th>Product</th><th>Category</th><th>Location</th></tr>");
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write("<tr><td><input type='checkbox' value='" + DT.Rows[i]["ProductDetailId"] + "'/>&nbsp;" + DT.Rows[i]["SerialNo"] + "</td><td>" + DT.Rows[i]["Name"] + "</td><td>" + DT.Rows[i]["Category"] + "</td><td>" + DT.Rows[i]["Location"] + "</td></tr>");//<a href='javascript:void(0)' onclick='RemoveSerail(" + DT.Rows[i]["ProductDetailId"] + ")'>X</a>
                    }
                    //if (DT.Rows.Count > 0)
                    //Response.Write("<tr><td colspan='3'><input type='button' value='Remove Selected' onclick='RemoveSerail()'/></td></tr>");                   
                    Response.Write("<table>~" + DT.Rows.Count);
                    DBConn = null;
                    break;
                case "loadStocknp":
                    Location = Request.QueryString["Location"];
                    Product = Request.QueryString["Product"];
                    Category = Request.QueryString["Category"];
                    sSql = "SELECT count(SerialNo) as SerialNo,(SELECT Name from LocationMaster WHERE LocationMaster.LocationId = ProductDetail.LocationId) AS Location,(select Name from ProductMaster where ProductMaster.ProductId = ProductDetail.ProductId) as Name,ExpiryDate FROM ProductDetail WHERE Status = 0 and ProductId = " + Functions.ToInt(Product);
                    if (Location != "")
                        sSql += " AND LocationId = " + Location;
                    sSql += " GROUP BY LocationId, ProductId, ExpiryDate, Status ORDER BY ExpiryDate ";
                    DBConn = new DBConnection();
                    DT = DBConn.ExecuteDataSet(sSql, CommandType.Text).Tables[0];
                    Response.Write("<table width='100%' class='grid' cellpadding='0' cellspacing='0'><tr><th>Tickets Count</th><th>New Quantity</th><th>Product</th><th>Expiry Date</th><th>Location</th></tr>");
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write("<tr><td><input type='checkbox' value='" + DT.Rows[i]["ExpiryDate"] + "'/>&nbsp;" + DT.Rows[i]["SerialNo"] + "</td><td><input type='text' width='40' style='width:40px' maxlength='5'/></td><td>" + DT.Rows[i]["Name"] + "</td><td>" + DT.Rows[i]["ExpiryDate"] + "</td><td>" + DT.Rows[i]["Location"] + "</td></tr>");
                    }
                    Response.Write("<table>~" + DT.Rows.Count);
                    DBConn = null;
                    break;
                case "getStock":
                    Location = Request.QueryString["Location"];
                    Product = Request.QueryString["Product"];
                    Category = Request.QueryString["Category"];
                    DBConn = new DBConnection();
                    IsPhysicalInventory = Functions.ToInt(DBConn.RetData("SELECT IsPhysicalInventory from Category where CategoryId = " + Functions.ToInt(Category), CommandType.Text));
                    if (IsPhysicalInventory == 0)
                    {
                        sSql = "SELECT count(SerialNo) as SerialNo,(SELECT Name from LocationMaster WHERE LocationMaster.LocationId = ProductDetail.LocationId) AS Location,(select Name from ProductMaster where ProductMaster.ProductId = ProductDetail.ProductId) as Name,ExpiryDate as Category FROM ProductDetail WHERE Status = 0 and ProductId = " + Functions.ToInt(Product);
                        if (Location != "")
                            sSql += " AND LocationId = " + Location;
                        sSql += " GROUP BY LocationId, ProductId, ExpiryDate, Status ORDER BY ExpiryDate ";
                        Response.Write("<table width='100%' class='grid' cellpadding='0' cellspacing='0'><tr><th>Tickets Count</th><th>Product</th><th>Expiry Date</th><th>Location</th></tr>");
                        DT = DBConn.ExecuteDataSet(sSql, CommandType.Text).Tables[0];
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write("<tr><td style='width:60px; text-align:right; padding-right:30px'>&nbsp;" + DT.Rows[i]["SerialNo"] + "</td><td>" + DT.Rows[i]["Name"] + "</td><td>" + Convert.ToDateTime(DT.Rows[i]["Category"].ToString()).ToString("MMMM dd, yyyy") + "</td><td>" + DT.Rows[i]["Location"] + "</td></tr>");//<a href='javascript:void(0)' onclick='RemoveSerail(" + DT.Rows[i]["ProductDetailId"] + ")'>X</a>
                        }
                    }
                    else
                    {
                        sSql = "SELECT SerialNo,(Select Name From Category where Category.CategoryId = (Select CategoryId from ProductMaster where ProductMaster.ProductId = ProductDetail.ProductId)) as Category,(SELECT Name from LocationMaster WHERE LocationMaster.LocationId = ProductDetail.LocationId) AS Location,(select Name from ProductMaster where ProductMaster.ProductId = ProductDetail.ProductId) as Name FROM ProductDetail WHERE Status = 0 ";
                        if (Product != "")
                            sSql += " and ProductId = " + Product;
                        else if (Category != "")
                            sSql += " and ProductDetail.ProductId in (select ProductId from ProductMaster where CategoryId = " + Category + ")";
                        if (Location != "")
                            sSql += " AND LocationId = " + Location;
                        DT = DBConn.ExecuteDataSet(sSql, CommandType.Text).Tables[0];
                        Response.Write("<table width='100%' class='grid' cellpadding='0' cellspacing='0'><tr><th>Category</th><th>Product</th><th>Location</th><th>Serial Number</th></tr>");
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write("<tr><td>" + DT.Rows[i]["Category"] + "</td><td>" + DT.Rows[i]["Name"] + "</td><td>" + DT.Rows[i]["Location"] + "</td><td>" + DT.Rows[i]["SerialNo"] + "</td></tr>");
                        }
                    }
                    Response.Write("<table>");
                    DBConn = null;
                    break;
                case "removeStock":
                    int stockid = Functions.ToInt(Request.QueryString["stockid"]);
                    string reason = "" + Request.QueryString["reason"];
                    if (stockid > 0)
                    {
                        DBConn = new DBConnection();
                        try
                        {
                            DBConn.BeginTransaction();
                            DT = DBConn.ExecuteTransactionDataSet("SELECT SerialNo,ProductId,LocationId FROM ProductDetail WHERE ProductDetailId =" + stockid + " AND Status = 0", CommandType.Text).Tables[0];
                            if (DT.Rows.Count > 0)
                            {
                                DBConn.ExecuteTransaction("UPDATE ProductDetail SET Status = 2,RemovalReason = '" + reason.Replace("'", "''") + "' WHERE ProductDetailId = " + stockid + " AND Status = 0", CommandType.Text);
                                DBConn.ExecuteTransaction("UPDATE ProductInventoryByLocation SET TicketCount = TicketCount - 1 WHERE ProductId = " + DT.Rows[0]["ProductId"] + " AND LocationId = " + DT.Rows[0]["LocationId"], CommandType.Text);
                                DBConn.ExecuteTransaction("UPDATE ProductMaster SET TicketCount = TicketCount - 1 WHERE ProductId = " + DT.Rows[0]["ProductId"], CommandType.Text);
                            }
                            DBConn.CommitTransaction();
                        }
                        catch (Exception)
                        {
                            DBConn.RollBackTransaction();
                        }

                        DBConn = null;
                    }
                    break;
                case "removeStocknp":
                    Location = Request.QueryString["Location"];
                    Product = Request.QueryString["Product"];
                    Category = Request.QueryString["Category"];
                    ExpDate = Request.QueryString["ExpDate"];
                    sUserId = Functions.GetSessionUserId();
                    reason = "" + Request.QueryString["reason"];
                    if (Location != "" && Product != "" && Category != "" && ExpDate != "")
                    {
                        DBConn = new DBConnection();
                        try
                        {
                            DBConn.BeginTransaction();
                            int iRowCount = DBConn.ExecuteTransaction("UPDATE ProductDetail SET Status = 2,RemovalReason = '" + reason.Replace("'", "''") + "' WHERE ExpiryDate = '" + Convert.ToDateTime(ExpDate).ToString("yyyy-MM-dd") + "' AND Status = 0 and ProductId = " + Product + " and LocationId = " + Location, CommandType.Text);
                            DBConn.ExecuteTransaction("UPDATE ProductInventoryByLocation SET TicketCount = TicketCount - " + iRowCount + " WHERE ProductId = " + Product + " AND LocationId = " + Location, CommandType.Text);
                            DBConn.ExecuteTransaction("UPDATE ProductMaster SET TicketCount = TicketCount -  " + iRowCount + " WHERE ProductId = " + Product, CommandType.Text);
                            DBConn.CommitTransaction();
                        }
                        catch (Exception)
                        {
                            DBConn.RollBackTransaction();
                        }

                        DBConn = null;
                    }
                    break;
                case "adjustStocknp":
                    Location = Request.QueryString["Location"];
                    Product = Request.QueryString["Product"];
                    Category = Request.QueryString["Category"];
                    ExpDate = Request.QueryString["ExpDate"];
                    sUserId = Functions.GetSessionUserId();
                    reason = "" + Request.QueryString["reason"];
                    int newqty = Functions.ToInt(Request.QueryString["newqty"]);
                    if (Location != "" && Product != "" && Category != "" && ExpDate != "" && newqty > 0)
                    {
                        DBConn = new DBConnection();
                        try
                        {
                            DBConn.BeginTransaction();
                            int iRowCount = DBConn.ExecuteTransaction("UPDATE ProductDetail SET Status = 2,RemovalReason = '" + reason.Replace("'", "''") + "' WHERE ExpiryDate = '" + Convert.ToDateTime(ExpDate).ToString("yyyy-MM-dd") + "' AND Status = 0 and ProductId = " + Product + " and LocationId = " + Location, CommandType.Text);
                            DBConn.ExecuteTransaction("UPDATE ProductInventoryByLocation SET TicketCount = TicketCount - " + iRowCount + " WHERE ProductId = " + Product + " AND LocationId = " + Location, CommandType.Text);
                            DBConn.ExecuteTransaction("UPDATE ProductMaster SET TicketCount = TicketCount -  " + iRowCount + " WHERE ProductId = " + Product, CommandType.Text);
                            for (int i = 0; i < newqty; i++)
                            {
                                int iPdtId = DBConn.ExecuteTransactionGetID("insert into ProductDetail (ProductId,UserId,Status,LocationId,PurchaseDate,ExpiryDate) values (" + Product + "," + sUserId + ",0," + Location + ", now(),'" + Convert.ToDateTime(ExpDate).ToString("yyyy-MM-dd") + "')", CommandType.Text);
                                DBConn.ExecuteTransaction("UPDATE ProductDetail SET SerialNo = '" + iPdtId + "' WHERE ProductDetailId = " + iPdtId, CommandType.Text);
                                DBConn.ExecuteTransaction("UPDATE ProductInventoryByLocation SET TicketCount = TicketCount + 1 WHERE ProductId = " + Product + " AND LocationId = " + Location, CommandType.Text);
                                DBConn.ExecuteTransaction("UPDATE ProductMaster SET TicketCount = TicketCount + 1 WHERE ProductId = " + Product, CommandType.Text);
                            }
                            DBConn.CommitTransaction();
                        }
                        catch (Exception)
                        {
                            DBConn.RollBackTransaction();
                        }

                        DBConn = null;
                    }
                    break;
                case "getSalesMasterDetails":
                    DBConn = new DBConnection();
                    DataTable dt;
                    if (Request.QueryString["SalesId"] != "")
                    {
                        bool IsReturned = false;
                        sql = "select IFNULL(sum(Count),0) as SalesCount from  SalesDetails S where S.SalesMasterId = " + Request.QueryString["SalesId"];
                        int SalesCount = Convert.ToInt32(DBConn.RetData(sql, CommandType.Text));

                        sql = "select IFNULL(sum(R.Qty),0) as SalesReturnCount from SalesReturnDetails R, SalesDetails S where S.SalesDetailId = R.SalesDetailId and S.SalesMasterId = " + Request.QueryString["SalesId"];
                        int SalesReturnCount = Convert.ToInt32(DBConn.RetData(sql, CommandType.Text));

                        if (SalesCount - SalesReturnCount == 0)
                            IsReturned = true;

                        sql = @"select S.*, (Select Username from UserMaster where UserId = S.ClerkId) as UserName from SalesMaster S where S.SalesMasterId = " + Request.QueryString["SalesId"];
                        dt = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                        if (dt.Rows.Count > 0)
                        {

                            TimeSpan dateDiff = DateTime.Now - Convert.ToDateTime(dt.Rows[0]["SalesDate"].ToString());
                            string strDateDiff = "";
                            if (dateDiff.TotalHours < 4)
                                strDateDiff = (dateDiff.Hours > 0 ? dateDiff.Hours.ToString() + (dateDiff.Hours > 1 ? " Hours" : " Hour") : "") + (dateDiff.Minutes > 0 ? dateDiff.Minutes.ToString() + (dateDiff.Minutes > 1 ? " Minutes" : " Minute") : "");
                            else if (dateDiff.TotalDays < 2)
                                strDateDiff = (dateDiff.Days > 0 ? dateDiff.Days.ToString() + " Day " : "") + (dateDiff.Hours > 0 ? dateDiff.Hours.ToString() + (dateDiff.Hours > 1 ? " Hours" : " Hour") : "");
                            else
                                strDateDiff = dateDiff.Days.ToString() + " Days";
                            strDateDiff += " ago";

                            Response.Write("<div><input type='hidden' id='hSalesMasterId' value='" + dt.Rows[0]["SalesMasterId"].ToString() + "' />Trans. Id: <b>" + dt.Rows[0]["SalesMasterId"].ToString() + "</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Amount: <b>" + dt.Rows[0]["GrandTotal"].ToString() + "</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Clerk: <b>" + dt.Rows[0]["UserName"].ToString() + "</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Sale Date: <b>" + Convert.ToDateTime(dt.Rows[0]["SalesDate"].ToString()).ToString("MM/dd/yy hh:mm tt") + "</b> (<font style='color:blue;'><b><i>" + strDateDiff + "</i></b></font>)<span id='span_status'>" + (IsReturned ? "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Status: <span style='color:red'><b>Returned</b></span>" : "") + "</span></div>");
                        }
                        else
                        {
                            Response.Write("");
                        }

                        DBConn = null;
                        Response.End();
                    }
                    break;

                case "getSalesDetails":

                    DBConn = new DBConnection();
                    int SalesId = 0;
                    if (Request.QueryString["Searchby"] != "SalesId")
                    {
                        sql = @"select distinct D.SalesMasterId from SalesDetails D where FIND_IN_SET('" + Request.QueryString["SerialNo"] + "',D.SerialNo)";
                        dt = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows.Count > 1)
                            {
                                Response.Write("M");
                                DBConn = null;
                                Response.End();
                            }
                            else
                            {
                                SalesId = Convert.ToInt16(dt.Rows[0]["SalesMasterId"].ToString());
                                Response.Write(SalesId.ToString());
                                DBConn = null;
                                Response.End();
                            }
                        }
                        else
                        {
                            Response.Write("");
                            DBConn = null;
                            Response.End();
                        }
                    }
                    else
                    {
                        SalesId = Convert.ToInt16(Request.QueryString["SalesId"]);
                    }


                    sql = @"select distinct S.SalesMasterId, S.SalesDate, C.Name as category_name, P.Name as product_name, PD.SerialNo, D.SalesAmount, PD.ProductDetailId, PD.Status, 
                            (select PaymentType from PaymentDetails P where P.SalesMasterId = S.SalesMasterId and TransType=1) as PaymentType, D.ProductId 
                            from SalesMaster S, SalesDetails D, ProductMaster P, ProductDetail PD, Category C
                            where S.SalesMasterId = D.SalesMasterId and D.ProductId = P.ProductId and P.ProductId = PD.ProductId and C.IsPhysicalInventory = 1 
                            and PD.SalesDetailId = D.SalesDetailId and P.CategoryId = C.CategoryId and S.SalesMasterId = " + SalesId.ToString();

                    decimal total = 0;
                    string ProductIds = "";
                    dt = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        /* implement on next phase 
                        Response.Write("<table width='100%' id='tbl_phystock' class='grid' cellpadding='0' cellspacing='0'><tr><th style='display:none;'></th><th style='width:40%'>Product</th><th style='width:15%'>Serial #</th><th style='width:15%'>Price</th><th style='width:30%'>Status</th></tr>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            Response.Write("<tr id='tr_" + dt.Rows[i]["ProductDetailId"].ToString() + "'><td style='display:none;'><input type='checkbox' id='chk_" + dt.Rows[i]["ProductDetailId"].ToString() + "'  runat='server' value='" + dt.Rows[i]["ProductDetailId"].ToString() + "' onclick='findTotal()' " + (dt.Rows[i]["Status"].ToString() == "3" ? "disabled='disabled'" : (dt.Rows[i]["PaymentType"].ToString() == "3" ? " checked='true' disabled='disabled'" : "")) + " /></td><td>" + dt.Rows[i]["category_name"].ToString() + ", " + dt.Rows[i]["product_name"].ToString() + "</td><td>" + dt.Rows[i]["SerialNo"].ToString() + "</td><td align='right' style='text-align:right' id='td_amt_" + dt.Rows[i]["ProductDetailId"].ToString() + "'>" + dt.Rows[i]["SalesAmount"].ToString() + "</td><td id='td_status_" + dt.Rows[i]["ProductDetailId"].ToString() + "' style='color:red;'>" + (dt.Rows[i]["Status"].ToString() == "3" ? "Returned" : "") + "</td></tr>");
                            total += Convert.ToDecimal(dt.Rows[i]["SalesAmount"].ToString());
                            ProductIds += dt.Rows[i]["ProductId"] + (i < dt.Rows.Count - 1 ? "," : "");
                        }
                        Response.Write("<tr><td colspan='2' style='text-align:right; font-weight:bold;'>Total: </td><td style='text-align:right; font-weight:bold;'>" + total.ToString() + "</td><td></td></tr>");
                        Response.Write("</table>");
                        */

                        Response.Write("<table width='100%' id='tbl_phystock' class='grid' cellpadding='0' cellspacing='0'><tr><th style='display:none;'></th><th style='width:40%'>Product</th><th style='width:15%'>Serial #</th><th style='width:15%'>Price</th><th style='width:30%'>Status</th></tr>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            Response.Write("<tr id='tr_" + dt.Rows[i]["ProductDetailId"].ToString() + "'><td style='display:none;'><input type='checkbox' id='chk_" + dt.Rows[i]["ProductDetailId"].ToString() + "'  runat='server' value='" + dt.Rows[i]["ProductDetailId"].ToString() + "' onclick='findTotal()' " + (dt.Rows[i]["Status"].ToString() != "3" ? "checked='true' " : " disabled='disabled'") + " /></td><td>" + dt.Rows[i]["category_name"].ToString() + ", " + dt.Rows[i]["product_name"].ToString() + "</td><td>" + dt.Rows[i]["SerialNo"].ToString() + "</td><td align='right' style='text-align:right' id='td_amt_" + dt.Rows[i]["ProductDetailId"].ToString() + "'>" + dt.Rows[i]["SalesAmount"].ToString() + "</td><td id='td_status_" + dt.Rows[i]["ProductDetailId"].ToString() + "' style='color:red;'>" + (dt.Rows[i]["Status"].ToString() == "3" ? "Returned" : "") + "</td></tr>");
                            total += Convert.ToDecimal(dt.Rows[i]["SalesAmount"].ToString());
                            ProductIds += dt.Rows[i]["ProductId"] + (i < dt.Rows.Count - 1 ? "," : "");
                        }
                        Response.Write("<tr><td colspan='2' style='text-align:right; font-weight:bold;'>Total: </td><td style='text-align:right; font-weight:bold;'>" + total.ToString() + "</td><td></td></tr>");
                        Response.Write("</table>");

                    }
                    bool HasSD = false;
                    if (ProductIds == "") ProductIds = "0";
                    //date_format(now(), '%m/%d/%Y')
                    sql = @"select S.SalesMasterId, D.SalesDetailId, D.ProductId, IsSmartDest, S.SDOrderNo, 
                            case when IsSmartDest = 1 then 'Smart Destinations' else (select Name from Category where CategoryId = (select CategoryId from ProductMaster where ProductId = D.ProductId)) end as category_name, 
                            Concat(IFNULL((select Name from ProductMaster where ProductId = D.ProductId),''), IFNULL(D.ProductName,'')) as product_name, 
                            case when IsSmartDest = 1 then '' else (select date_format(ExpiryDate, '%m/%d/%Y') from ProductDetail where ProductId = D.ProductId and SalesDetailId = D.SalesDetailId limit 1) end as ExpiryDate, 
                            D.SalesAmount, sum(D.count) as SaleQty, 
                            ifnull((select sum(Qty) from SalesReturnDetails where SalesDetailId = D.SalesDetailId and ProductId = D.ProductId),0) as ReturnQty, 
                            (select PaymentType from PaymentDetails P where P.SalesMasterId = S.SalesMasterId and TransType=0) as PaymentType 
                            from SalesMaster S, SalesDetails D  
                            where S.SalesMasterId = D.SalesMasterId and S.SalesMasterId = " + SalesId.ToString() + @" and D.ProductId not in (" + ProductIds + @")
                            group by S.SalesMasterId, D.SalesDetailId, D.ProductId, ExpiryDate";

                    dt = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                    string strQty = "";
                    if (dt.Rows.Count > 0)
                    {
                        total = 0;
                        Response.Write("<br />");

                        //Response.Write("<table width='100%' class='grid' cellpadding='0' cellspacing='0' id='tbl_cards'><tr><th style='width:40%'>Product</th><th style='width:15%'>Sales Qty</th><th style='width:15%'>Price</th><th style='width:10%'>Status</th><th style='width:10%'>Exp. date</th><th style='width:10%'>Qty to Return</th></tr>");
                        Response.Write("<table width='100%' class='grid' cellpadding='0' cellspacing='0' id='tbl_cards'><tr><th style='width:40%'>Product</th><th style='width:15%'>Sales Qty</th><th style='width:15%'>Price</th><th style='width:10%'>Status</th><th style='width:20%'>Exp. date</th><th style='width:0%; display:none;'>Qty to Return</th></tr>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            /* implement on next phase 
                            if (dt.Rows[i]["IsSmartDest"].ToString() == "1" || dt.Rows[i]["PaymentType"].ToString() == "3")
                                strQty = " value = '" + (Convert.ToInt16(dt.Rows[i]["SaleQty"].ToString()) - Convert.ToInt16(dt.Rows[i]["ReturnQty"].ToString())).ToString() + "' readonly";
                            else if (Convert.ToInt16(dt.Rows[i]["ReturnQty"].ToString()) == Convert.ToInt16(dt.Rows[i]["SaleQty"].ToString()))
                                strQty = " value = '' readonly ";
                            else
                                strQty = " value = ''";

                            Response.Write(@"<tr>
                                            <td>
                                                <input type='hidden' runat='server' name='SalesDetailId' value='" + dt.Rows[i]["SalesDetailId"].ToString() + @"' />
                                                <input type='hidden' id='hQty_" + dt.Rows[i]["SalesDetailId"].ToString() + "_" + (i + 1) + "' runat='server' value='" + (Convert.ToInt16(dt.Rows[i]["SaleQty"].ToString()) - Convert.ToInt16(dt.Rows[i]["ReturnQty"].ToString())) + @"' />" +
                                                                                dt.Rows[i]["category_name"].ToString() + ", " + dt.Rows[i]["product_name"].ToString() + (dt.Rows[i]["IsSmartDest"].ToString() == "1"?" (Order No.: "+dt.Rows[i]["ProductId"].ToString()+")":"") + @"
                                            </td>
                                            <td id='td_qty_" + dt.Rows[i]["SalesDetailId"].ToString() + "_" + (i + 1) + "'>" + dt.Rows[i]["SaleQty"].ToString() + @"</td>
                                            <td align='right' style='text-align:right' id='td_amt_" + dt.Rows[i]["SalesDetailId"].ToString() + "_" + (i + 1) + "'>" + dt.Rows[i]["SalesAmount"].ToString() + @"</td>
                                            <td style='color:red;'>" + (Convert.ToInt16(dt.Rows[i]["ReturnQty"].ToString()) > 0 ? dt.Rows[i]["ReturnQty"].ToString() + " Returned" : "") + @"</td>
                                            <td id='td_expdate_" + dt.Rows[i]["SalesDetailId"].ToString() + "_" + (i + 1) + "'>" + dt.Rows[i]["ExpiryDate"].ToString() + @"</td>
                                            <td><input type='text' onchange=ValidateQty('" + dt.Rows[i]["SalesDetailId"].ToString() + "_" + (i + 1) + "') name='qty_return[]' id='t_qty_return_" + dt.Rows[i]["SalesDetailId"].ToString() + "_" + (i + 1) + "' style='width:95%' " + strQty + @" ></td>
                                        </tr>");
                            total += Convert.ToDecimal(dt.Rows[i]["SalesAmount"].ToString()) * Convert.ToInt16(dt.Rows[i]["SaleQty"].ToString());
                            */

                            if (dt.Rows[i]["IsSmartDest"].ToString() == "1" && !HasSD)
                                HasSD = true;

                            strQty = " value = '" + (Convert.ToInt16(dt.Rows[i]["SaleQty"].ToString()) - Convert.ToInt16(dt.Rows[i]["ReturnQty"].ToString())).ToString() + "' readonly";


                            Response.Write(@"<tr>
                                            <td>
                                                <input type='hidden' runat='server' name='SalesDetailId' value='" + dt.Rows[i]["SalesDetailId"].ToString() + @"' />
                                                <input type='hidden' id='hQty_" + dt.Rows[i]["SalesDetailId"].ToString() + "_" + (i + 1) + "' runat='server' value='" + (Convert.ToInt16(dt.Rows[i]["SaleQty"].ToString()) - Convert.ToInt16(dt.Rows[i]["ReturnQty"].ToString())) + @"' />" +
                                                                                dt.Rows[i]["category_name"].ToString() + ", " + dt.Rows[i]["product_name"].ToString() + (dt.Rows[i]["IsSmartDest"].ToString() == "1" ? " (Order No.: " + dt.Rows[i]["SDOrderNo"].ToString() + ")" : "") + @"
                                            </td>
                                            <td id='td_qty_" + dt.Rows[i]["SalesDetailId"].ToString() + "_" + (i + 1) + "'>" + dt.Rows[i]["SaleQty"].ToString() + @"</td>
                                            <td align='right' style='text-align:right' id='td_amt_" + dt.Rows[i]["SalesDetailId"].ToString() + "_" + (i + 1) + "'>" + dt.Rows[i]["SalesAmount"].ToString() + @"</td>
                                            <td style='color:red;'>" + (Convert.ToInt16(dt.Rows[i]["ReturnQty"].ToString()) > 0 ? dt.Rows[i]["ReturnQty"].ToString() + " Returned" : "") + @"</td>
                                            <td id='td_expdate_" + dt.Rows[i]["SalesDetailId"].ToString() + "_" + (i + 1) + "'>" + dt.Rows[i]["ExpiryDate"].ToString() + @"</td>
                                            <td style='display:none;'><input type='text' onchange=ValidateQty('" + dt.Rows[i]["SalesDetailId"].ToString() + "_" + (i + 1) + "') name='qty_return[]' id='t_qty_return_" + dt.Rows[i]["SalesDetailId"].ToString() + "_" + (i + 1) + "' style='width:95%' " + strQty + @" ></td>
                                        </tr>");
                            total += Convert.ToDecimal(dt.Rows[i]["SalesAmount"].ToString()) * Convert.ToInt16(dt.Rows[i]["SaleQty"].ToString());
                        }

                        Response.Write("<tr><td colspan='2' style='text-align:right; font-weight:bold;'>Total: </td><td style='text-align:right; font-weight:bold;'>" + total.ToString() + "</td><td></td><td></td></tr>");
                        Response.Write("</table>");
                        if (HasSD)
                            Response.Write("<script>document.getElementById('div_printpdf').style.display = '';</script>");
                        else
                            Response.Write("<script>document.getElementById('div_printpdf').style.display = 'none';</script>");
                    }

                    DBConn = null;
                    Response.End();
                    break;

                case "getPaymentDetails":

                    DBConn = new DBConnection();
                    sql = @"select distinct P.* from PaymentDetails P where P.SalesMasterId = " + Request.QueryString["SalesId"] + " and TransType=0";
                    dt = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                     
                    if (dt.Rows.Count > 0)
                    {
                        Response.Write("<table width='100%' class='grid' cellpadding='0' cellspacing='0'><tr><th width='40%'>Payment Type</th><th width='20%'>Purchase Amount</th><th width='20%'>Amount already Returned</th><th width='20%'><b>Current Return Amount</b></th></tr>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            /*   implement on next phase 
                            if (dt.Rows[i]["PaymentType"].ToString() == "1")
                                Response.Write("<tr><td runat='server' id='td_Cash'>Cash</td><td runat='server' align='right' style='text-align:right' id='td_sales_amt_Cash'>" + dt.Rows[i]["Amount"].ToString() + "</td><td><input type='text' runat='server' id='return_amt_Cash' value='' readonly /></td></tr>");
                            else if (dt.Rows[i]["PaymentType"].ToString() == "2")
                                Response.Write("<tr><td runat='server' id='td_Credit'>Credit</td><td runat='server' align='right' style='text-align:right' id='td_sales_amt_Credit'>" + dt.Rows[i]["Amount"].ToString() + "</td><td><input type='text' runat='server' id='return_amt_Credit' value='' readonly /></td></tr>");
                            else if (dt.Rows[i]["PaymentType"].ToString() == "3")
                                Response.Write("<tr><td runat='server' id='td_Voucher'>Voucher</td><td runat='server' align='right' style='text-align:right' id='td_sales_amt_Voucher'>" + dt.Rows[i]["Amount"].ToString() + "</td><td><input type='text' runat='server' id='return_amt_Voucher' value='' readonly /></td></tr>");
                            */

                            sql = @"select sum(amount) from PaymentDetails P where TransType=1 and PaymentType=" + dt.Rows[i]["PaymentType"].ToString() + " and SalesMasterId in (select distinct SalesReturnMasterId from SalesDetails S, SalesReturnDetails R where S.SalesDetailId = R.SalesDetailId and S.SalesMasterId = " + Request.QueryString["SalesId"] + ")";
                            string already_returned = DBConn.RetData(sql, CommandType.Text);

                            if (dt.Rows[i]["PaymentType"].ToString() == "1")
                                Response.Write("<tr><td runat='server' id='td_Cash'>Cash</td><td runat='server' align='right' style='text-align:right' id='td_sales_amt_Cash'>" + dt.Rows[i]["Amount"].ToString() + "</td><td style='text-align:right'>" + already_returned + "</td><td style='display:; width:50px;'><input type='text' runat='server' id='return_amt_Cash' value='' style='width:100px; text-align:right;' onchange='getTotal();' /></td></tr>");
                            else if (dt.Rows[i]["PaymentType"].ToString() == "2")
                                Response.Write("<tr><td runat='server' id='td_Credit'>Credit</td><td runat='server' align='right' style='text-align:right' id='td_sales_amt_Credit'>" + dt.Rows[i]["Amount"].ToString() + "</td><td style='text-align:right'>" + already_returned + "</td><td style='display:; width:50px;'><input type='text' runat='server' id='return_amt_Credit' value='' style='width:100px; text-align:right;' onchange='getTotal();' /></td></tr>");
                            else if (dt.Rows[i]["PaymentType"].ToString() == "3")
                                Response.Write("<tr><td runat='server' id='td_Voucher'>Voucher</td><td runat='server' align='right' style='text-align:right' id='td_sales_amt_Voucher'>" + dt.Rows[i]["Amount"].ToString() + "</td><td style='text-align:right'>" + already_returned + "</td><td style='display:; width:50px;'><input type='text' runat='server' id='return_amt_Voucher' value='' style='width:100px; text-align:right;' onchange='getTotal();'  /></td></tr>");

                        }
                        Response.Write("<tr><td colspan='3' style='background-color:white; border:none; text-align:right; font-size:16px; font-weight:bold;'>Total Returned:</td><td style='background-color:white; border:none; text-align:right; font-size:16px; font-weight:bold;' id='amt_return'>0.00</td></tr>");
                        Response.Write("</table>");
                    }
                    else
                    {
                        Response.Write("No data found.");
                    }
                    DBConn = null;
                    Response.End();
                    break;

                case "getSalesMaster":

                    DBConn = new DBConnection();

                    sql = @"select distinct S.SalesMasterId, S.SalesDate, S.GrandTotal, U.username, D.SalesAmount, C.Name as category_name, P.Name as product_name, D.serialNo, D.count
                            from SalesMaster S, SalesDetails D, UserMaster U, ProductMaster P, Category C
                            where S.SalesMasterId = D.SalesMasterId and S.ClerkId = U.UserId and D.ProductId = P.ProductId 
                            and P.CategoryId = C.CategoryId and S.SalesMasterId in 
                            (select distinct D.SalesMasterId from SalesDetails D where FIND_IN_SET('" + Request.QueryString["SerialNo"] + @"',D.SerialNo)) 
                            order by S.SalesMasterId";

                    dt = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        Response.Write("<p>More than one transaction is found matching the serial number.<br />Please select the transaction that matches the refund request from the list below.</p>");
                        Response.Write("<table width='100%' class='grid' cellpadding='0' cellspacing='0' style='font-family:arial; font-size:12px;'><tr><th>Trans Id</th><th>Sales Date</th><th>Clerk</th><th style='width:100px'>Category</th><th style='width:100px'>Product</th><th style='width:100px'>Serial #</th><th style='width:50px'>Quantity</th><th style='width:50px'>Amount</th></tr>");

                        string[] cols = { "SalesMasterId", "SalesDate", "UserName" };
                        DataTable dtSales = dt.DefaultView.ToTable(true, cols);

                        for (int i = 0; i < dtSales.Rows.Count; i++)
                        {
                            string strrow = "<tr><td>" + dtSales.Rows[i]["SalesMasterId"].ToString() + "<br><a href='javascript: void(0);' onclick=\"window.opener.document.getElementById('M_tSalesId').value='" + dtSales.Rows[i]["SalesMasterId"].ToString() + "'; window.opener.document.getElementById('bSearch').click(); window.close(); \">Select</a></td><td>" + dtSales.Rows[i]["SalesDate"].ToString() + "</td><td >" + dtSales.Rows[i]["UserName"].ToString() + "</td><td colspan='5'><table width='100%' cellpadding='0' cellspacing='0' style='font-family:arial; font-size:12px; border:none;'>";
                            DataRow[] dr = dt.Select("SalesMasterId=" + dtSales.Rows[i]["SalesMasterId"].ToString());
                            for (int j = 0; j < dr.Length; j++)
                            {
                                strrow += "<tr><td style='width:100px'>" + dr[j]["category_name"].ToString() + "</td><td style='width:100px'>" + dr[j]["product_name"].ToString() + "</td><td style='width:100px'>" + dr[j]["serialNo"].ToString() + "</td><td style='text-align:right; width:50px'>" + dr[j]["count"].ToString() + "</td><td style='text-align:right; width:50px'>" + dr[j]["SalesAmount"].ToString() + "</td></tr>";
                            }
                            strrow += "</table></td></tr><tr><td colspan='9'  style='height:2px;'><hr></td></tr>";
                            Response.Write(strrow);
                        }
                        Response.Write("</table>");

                    }
                    else
                    {
                        Response.Write("No data found.");
                    }
                    DBConn = null;
                    Response.End();
                    break;

                case "submitReturn":
                    DBConn = new DBConnection();
                    try
                    {
                        int SalesMasterId = Convert.ToInt16(Request.QueryString["SalesMasterId"]);
                        string ProductDetailId = Request.QueryString["ProductDetailId"];
                        decimal Cash = Convert.ToDecimal(Request.QueryString["Cash"]);
                        decimal Credit = Convert.ToDecimal(Request.QueryString["Credit"]);
                        decimal Voucher = Convert.ToDecimal(Request.QueryString["Voucher"]);
                        string[] arrProductDetailId = ProductDetailId.Split(',');

                        string[] arrSalesDetailId = Request.QueryString["SalesDetailId"].Split(',');
                        string[] arrExpiryDate = Request.QueryString["ExpiryDate"].Split(',');
                        string[] arrReturnQty = Request.QueryString["ReturnQty"].Split(',');

                        if (Cash + Credit + Voucher > 0)
                        {
                            DBConn.BeginTransaction();
                            sql = "insert into SalesReturnMaster (SalesReturnDate, Amount, LocationId, ClerkId, Status) values (now(), " + (Cash + Credit + Voucher).ToString() + ", " + Session["LocationId"] + ", " + Session["UserId"] + ", 1)";
                            int SalesReturnMasterId = DBConn.ExecuteTransactionGetID(sql, CommandType.Text);

                            if (arrProductDetailId.Length > 0 && ProductDetailId != "")
                            {
                                sql = "select ProductId, SalesDetailId, (select SalesAmount from SalesDetails where SalesDetailId = ProductDetail.SalesDetailId and ProductId = ProductDetail.ProductId and FIND_IN_SET(ProductDetail.SerialNo,SalesDetails.SerialNo)) as Amount, Count(*) as Qty, group_concat(serialNo SEPARATOR ',') as SerialNo,  group_concat(ProductDetailId SEPARATOR ',') as ProductDetailId from ProductDetail where ProductDetailId in (" + ProductDetailId + ") group by ProductId";
                                dt = DBConn.ExecuteTransactionDataSet(sql, CommandType.Text).Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        sql = "insert into SalesReturnDetails (SalesReturnMasterId, SalesDetailId, ProductId,SerialNo,Amount,LocationId,Qty) values (" + SalesReturnMasterId + ", " + dt.Rows[i]["SalesDetailId"].ToString() + ", " + dt.Rows[i]["ProductId"].ToString() + ", '" + dt.Rows[i]["SerialNo"].ToString() + "', " + dt.Rows[i]["Amount"].ToString() + ", " + Session["LocationId"] + ", " + dt.Rows[i]["Qty"].ToString() + ") ";
                                        int SalesReturnDetailId = DBConn.ExecuteTransactionGetID(sql, CommandType.Text);

                                        arrProductDetailId = dt.Rows[i]["ProductDetailId"].ToString().Split(',');
                                        for (int j = 0; j < arrProductDetailId.Length; j++)
                                        {
                                            sql = "update ProductDetail set Status = 3, SalesReturnDetailId = " + SalesReturnDetailId + "  where ProductDetailId = " + arrProductDetailId[j];
                                            DBConn.ExecuteTransaction(sql, CommandType.Text);

                                            sql = "update ProductMaster set TicketCount = TicketCount + 1 where ProductId = " + dt.Rows[i]["ProductId"].ToString();
                                            DBConn.ExecuteTransaction(sql, CommandType.Text);

                                            sql = "update ProductInventoryByLocation set TicketCount = TicketCount + 1 where ProductId = " + dt.Rows[i]["ProductId"].ToString() + " and LocationId = " + Session["LocationId"].ToString();
                                            DBConn.ExecuteTransaction(sql, CommandType.Text);
                                        }
                                    }
                                }
                            }

                            if (arrSalesDetailId.Length > 1 && Request.QueryString["SalesDetailId"] != "")
                            {
                                for (int i = 1; i < arrSalesDetailId.Length; i++)
                                {
                                    sql = "select ProductId, IsSmartDest, Count, SalesAmount from SalesDetails where SalesDetailId = " + arrSalesDetailId[i];
                                    DataTable dtSD = DBConn.ExecuteTransactionDataSet(sql, CommandType.Text).Tables[0];
                                    if (dtSD.Rows[0]["IsSmartDest"].ToString() == "0")
                                    {
                                        sql = "select ProductId, ExpiryDate, (select SalesAmount from SalesDetails where SalesDetailId = ProductDetail.SalesDetailId and ProductId = ProductDetail.ProductId and FIND_IN_SET(ProductDetail.SerialNo,SalesDetails.SerialNo)) as Amount, Count(*) as Qty, group_concat(serialNo SEPARATOR ',') as SerialNo,  group_concat(ProductDetailId SEPARATOR ',') as ProductDetailId from ProductDetail where SalesDetailId = " + arrSalesDetailId[i] + " and ExpiryDate = '" + Convert.ToDateTime(arrExpiryDate[i].Replace("'", "")).ToString("yyyy-MM-dd") + "' and status = 1 group by ProductId, ExpiryDate";
                                        dt = DBConn.ExecuteTransactionDataSet(sql, CommandType.Text).Tables[0];
                                        if (dt.Rows.Count > 0)
                                        {
                                            sql = "insert into SalesReturnDetails (SalesReturnMasterId, SalesDetailId, ProductId,SerialNo,Amount,LocationId,Qty,ExpiryDate) values (" + SalesReturnMasterId + ", " + arrSalesDetailId[i] + ", " + dt.Rows[0]["ProductId"].ToString() + ", '', " + dt.Rows[0]["Amount"].ToString() + ", " + Session["LocationId"] + ", " + arrReturnQty[i] + ",'" + Convert.ToDateTime(arrExpiryDate[i].Replace("'", "")).ToString("yyyy-MM-dd") + "') ";
                                            int SalesReturnDetailId = DBConn.ExecuteTransactionGetID(sql, CommandType.Text);

                                            sql = "select ProductDetailId, SerialNo from ProductDetail where ProductDetailId in (" + dt.Rows[0]["ProductDetailId"].ToString() + ") and status = 1 order by ProductDetailId limit " + arrReturnQty[i];
                                            DataTable dtSNo = DBConn.ExecuteTransactionDataSet(sql, CommandType.Text).Tables[0];
                                            for (int j = 0; j < dtSNo.Rows.Count; j++)
                                            {
                                                sql = "update ProductDetail set Status = 3, SalesReturnDetailId = " + SalesReturnDetailId + "  where ProductDetailId = " + dtSNo.Rows[j]["ProductDetailId"].ToString();
                                                DBConn.ExecuteTransaction(sql, CommandType.Text);

                                                sql = "update ProductMaster set TicketCount = TicketCount + 1 where ProductId = " + dt.Rows[0]["ProductId"].ToString();
                                                DBConn.ExecuteTransaction(sql, CommandType.Text);

                                                sql = "update ProductInventoryByLocation set TicketCount = TicketCount + 1 where ProductId = " + dt.Rows[0]["ProductId"].ToString() + " and LocationId = " + Session["LocationId"].ToString();
                                                DBConn.ExecuteTransaction(sql, CommandType.Text);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        sql = "insert into SalesReturnDetails (SalesReturnMasterId, SalesDetailId, ProductId,SerialNo,Amount,LocationId,Qty) values (" + SalesReturnMasterId + ", " + arrSalesDetailId[i] + ", " + dtSD.Rows[0]["ProductId"].ToString() + ", '', " + dtSD.Rows[0]["SalesAmount"].ToString() + ", " + Session["LocationId"] + ", " + arrReturnQty[i] + ")";
                                        int SalesReturnDetailId = DBConn.ExecuteTransactionGetID(sql, CommandType.Text);

                                    }
                                }
                            }

                            int PaymentType;
                            if (Cash > 0)
                                PaymentType = 1;
                            else if (Credit > 0)
                                PaymentType = 2;
                            else
                                PaymentType = 3;

                            sql = @"insert into PaymentDetails (SalesMasterId, PaymentType, Amount, CardCheckNo, CardName, CardAdd1, CardAdd2, CardCity, CardState, CardZip, ExpiryMonth, ExpiryYear, CardType, CardAuthorizationId, TransType) values 
                                    (" + SalesReturnMasterId + ", " + PaymentType + ", " + (Cash + Credit + Voucher).ToString() + ", '',  '', '', '', '', '', '', 0, 0, '', '0',1)";
                            int PaymentId = DBConn.ExecuteTransactionGetID(sql, CommandType.Text);

                            int ErrResponse = SalesReturnMasterId;

                            if (PaymentType == 2)   //credit card refund
                            {
                                sql = "select CardAuthorizationId, CardCheckNo, Amount from PaymentDetails where SalesMasterId = " + SalesMasterId;
                                DataTable dtPay = DBConn.ExecuteTransactionDataSet(sql, CommandType.Text).Tables[0];
                                if (dtPay.Rows.Count > 0)
                                {
                                    objPayment = new classes.Payment();
                                    //trying to refund assuming its a settled transaction
                                    bool IsRefund = objPayment.RefundPayment(dtPay.Rows[0]["CardAuthorizationId"].ToString(), dtPay.Rows[0]["CardCheckNo"].ToString(), Credit, PaymentId);

                                    if (!IsRefund)
                                    {
                                        if (Credit < Convert.ToDecimal(dtPay.Rows[0]["Amount"].ToString()))
                                        {
                                            //Partial refunds
                                            sql = "Update SalesReturnMaster set Status = 0 where SalesReturnMasterId = " + SalesReturnMasterId;
                                            DBConn.ExecuteTransaction(sql, CommandType.Text);
                                            ErrResponse = -3;
                                        }
                                        else
                                        {
                                            //Void transaction in case of full refund
                                            IsRefund = objPayment.VoidPayment(dtPay.Rows[0]["CardAuthorizationId"].ToString(), dtPay.Rows[0]["CardCheckNo"].ToString(), Credit, PaymentId);
                                            if (!IsRefund)
                                            {
                                                ErrResponse = -1;
                                            }
                                            else
                                            {
                                                sql = "update PaymentDetails set CardAuthorizationId = " + objPayment.TransId + " where PaymentId = " + PaymentId;
                                                DBConn.ExecuteTransaction(sql, CommandType.Text);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ErrResponse = -2;
                                }
                            }

                            if (ErrResponse == SalesReturnMasterId || ErrResponse == -3)
                            {
                                DBConn.CommitTransaction();
                                DBConn = null;
                                Response.Write(ErrResponse.ToString());
                            }
                            else
                            {
                                DBConn.RollBackTransaction();
                                DBConn = null;
                                Response.Write(ErrResponse.ToString());
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        DBConn.RollBackTransaction();
                        DBConn = null;
                        Response.Write("0");
                    }
                    break;


                case "CheckSNo":
                    DBConn = new DBConnection();
                    SalesId = int.Parse(Request.QueryString["SalesMasterId"]);
                    SerialNo = Request.QueryString["SerialNo"];

                    sql = "select P.ProductDetailId as cnt from ProductDetail P, SalesDetails S where P.SalesDetailId = S.SalesDetailId and status = 1 and S.SalesMasterId = " + SalesId + " and P.SerialNo = '" + SerialNo + "'";
                    Response.Write(DBConn.RetData(sql, CommandType.Text));
                    break;

                case "ParseCreditCardInfo":

                    string scan = Request.QueryString["scan"].Trim();
                    bool sentinels = false;
                    int expTotal = 0;
                    string[] tracks = scan.Split(new char[] { '\n' });
                    if (scan.StartsWith("%"))
                        sentinels = true;
                    foreach (string line in tracks)
                    {
                        if ((!line.StartsWith("B") && !sentinels) ||
                           (line[1] != 'B' && sentinels)) //credit card line.
                            continue;
                        int indexOfCarrot = scan.IndexOf('^');
                        int startIndex;
                        if (sentinels)
                            startIndex = 2;
                        else
                            startIndex = 1;
                        string strAccountNum = scan.Substring(startIndex, (indexOfCarrot - startIndex));

                        int indexOfLastCarrot = scan.LastIndexOf('^');
                        string Name = scan.Substring((indexOfCarrot + 1), (indexOfLastCarrot - indexOfCarrot - 1));
                        Name = Name.Trim();
                        string strExp = scan.Substring((indexOfLastCarrot + 1), 4);

                        try
                        {
                            expTotal = Int32.Parse(strExp);
                            int ExpMonth = expTotal % 100;
                            int ExpYear = expTotal / 100;

                            //Set to new Card
                            //this.tCardNo.Text = strAccountNum;
                            //this.ExpMonth.SelectedIndex = this.ExpMonth.Items.IndexOf(this.ExpMonth.Items.FindByValue(ExpMonth.ToString()));
                            //this.ExpYear.SelectedIndex = this.ExpYear.Items.IndexOf(this.ExpYear.Items.FindByValue("20" + ExpYear.ToString()));
                            //this.tCardName.Text = Name;
                            //this.RawData = scan;

                            Response.Write(strAccountNum + "~" + ExpMonth.ToString() + "~" + "20" + ExpYear.ToString() + "~" + Name);

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }//parse                     

                    break;


                case "getSalesDetailsNew":

                    DBConn = new DBConnection();
                    SalesId = 0;
                    if (Request.QueryString["Searchby"] != "SalesId")
                    {
                        sql = @"select distinct D.SalesMasterId from SalesDetails D where FIND_IN_SET('" + Request.QueryString["SerialNo"] + "',D.SerialNo)";
                        dt = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows.Count > 1)
                            {
                                Response.Write("M");
                                DBConn = null;
                                Response.End();
                            }
                            else
                            {
                                SalesId = Convert.ToInt16(dt.Rows[0]["SalesMasterId"].ToString());
                                Response.Write(SalesId.ToString());
                                DBConn = null;
                                Response.End();
                            }
                        }
                        else
                        {
                            Response.Write("");
                            DBConn = null;
                            Response.End();
                        }
                    }
                    else
                    {
                        SalesId = Convert.ToInt16(Request.QueryString["SalesId"]);
                    }

                    sql = "select sum(Count) from SalesDetails where SalesMasterId = " + SalesId;
                    int cnt = Convert.ToInt32(DBConn.RetData(sql, CommandType.Text)) + 2;

                    total = 0;
                    Response.Write("<table width='100%' id='tbl_phystock' class='grid' cellpadding='0' cellspacing='0'><tr><th style='display:;'></th><th>Product</th><th style='width:10%'>Serial # / Expiry Date / Order No.</th><th style='width:10%'>Amount Paid</th><th style='width:10px; background-color:black;' rowspan='" + cnt.ToString() + "'></th><th style='width:10%'>Exp. Date</th><th style='width:10%'>Amount Returned</th><th style='width:15%'>Status</th></tr>");

                    sql = @"select distinct C.IsPhysicalInventory, S.SalesMasterId, S.SalesDate, C.Name as category_name, P.Name as product_name, PD.SerialNo, D.SalesAmount, PD.ProductDetailId, PD.Status, 
                            (select PaymentType from PaymentDetails P where P.SalesMasterId = S.SalesMasterId and TransType=1) as PaymentType, D.ProductId, date_format(PD.ExpiryDate, '%m/%d/%Y') as ExpiryDate, S.SDOrderNo, 1 as Qty
                            from SalesMaster S, SalesDetails D, ProductMaster P, ProductDetail PD, Category C
                            where S.SalesMasterId = D.SalesMasterId and D.ProductId = P.ProductId and P.ProductId = PD.ProductId and D.IsSmartDest = 0 
                            and PD.SalesDetailId = D.SalesDetailId and P.CategoryId = C.CategoryId and S.SalesMasterId = " + SalesId.ToString();
                    
                    bool HasScanableTickets = false;

                    int rowIndex = 0;
                    dt = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            /*
                            if (dt.Rows[i]["IsSmartDest"].ToString() == "1" || dt.Rows[i]["PaymentType"].ToString() == "3")
                                strQty = " value = '" + (Convert.ToInt16(dt.Rows[i]["SaleQty"].ToString()) - Convert.ToInt16(dt.Rows[i]["ReturnQty"].ToString())).ToString() + "' readonly";
                            else if (Convert.ToInt16(dt.Rows[i]["ReturnQty"].ToString()) == Convert.ToInt16(dt.Rows[i]["SaleQty"].ToString()))
                                strQty = " value = '' readonly ";
                            else
                                strQty = " value = ''";
                            */

                            Response.Write(
                            @"<tr id='tr_" + rowIndex.ToString() + @"'>
                                <td style='display:;'><input type='hidden' id='slno' value='" + rowIndex.ToString() + "'><input type='hidden' id='product_type_" + rowIndex.ToString() + "' value='" + dt.Rows[i]["IsPhysicalInventory"].ToString() + "'><input type='hidden' id='ProductDetailId_" + rowIndex.ToString() + "' value='" + dt.Rows[i]["ProductDetailId"].ToString() + "'><input type='checkbox' id='chk_" + rowIndex.ToString() + "' value='" + dt.Rows[i]["ProductDetailId"].ToString() + "' onclick='findTotal(this," + dt.Rows[i]["IsPhysicalInventory"].ToString() + "," + rowIndex.ToString() + ")' " + (dt.Rows[i]["status"].ToString() == "3" ? " disabled='disabled' " : " ") + @" /></td>
                                <td>" + dt.Rows[i]["category_name"].ToString() + ", " + dt.Rows[i]["product_name"].ToString() + @"</td>
                                <td>" + (dt.Rows[i]["IsPhysicalInventory"].ToString() == "1" ? dt.Rows[i]["SerialNo"].ToString() : dt.Rows[i]["ExpiryDate"].ToString()) + @"</td>
                                <td align='right' style='text-align:right' id='td_amt_" + rowIndex.ToString() + "'>" + dt.Rows[i]["SalesAmount"].ToString() + @"</td>
                                <td>" + (dt.Rows[i]["IsPhysicalInventory"].ToString() == "0" && dt.Rows[i]["status"].ToString() == "1" ? "<input type='text' runat='server' id = 'txt_expdate_" + rowIndex.ToString() + "' class='datepick' style='width:150px;' onblur='CheckExpDate(this," + rowIndex.ToString() + ");' onchange='CheckExpDate(this," + rowIndex.ToString() + ");' />" : "") + @"</td>
                                <td id='td_amount_returned_" + rowIndex.ToString() + @"' align='right' style='text-align:right'></td>
                                <td id='td_status_" + rowIndex.ToString() + "' style='color:red;'>" + (dt.Rows[i]["Status"].ToString() == "3" ? "Returned" : "") + @"</td>
                             </tr>");
                            total += Convert.ToDecimal(dt.Rows[i]["SalesAmount"].ToString());
                            rowIndex++;
                            if (dt.Rows[i]["IsPhysicalInventory"].ToString() == "1" && HasScanableTickets == false)
                                HasScanableTickets = true;
                        }
                    }

                    HasSD = false;
                    sql = @" select distinct 2 as IsPhysicalInventory, S.SalesMasterId, D.SalesDetailId, S.SalesDate, 'Smart Destinations' as category_name, D.ProductName as product_name, '' as SerialNo, D.SalesAmount, 0 as ProductDetailId, 
                             (select PaymentType from PaymentDetails P where P.SalesMasterId = S.SalesMasterId and TransType=1) as PaymentType, D.ProductId, 0 as Status, '' as ExpiryDate, S.SDOrderNo, D.Count as Qty,
                             (select count(*) from SalesReturnDetails where SalesDetailId = D.SalesDetailId and ProductId = D.ProductId) as ReturnQty    
                             from SalesMaster S, SalesDetails D  
                             where S.SalesMasterId = D.SalesMasterId and D.IsSmartDest = 1 and S.SalesMasterId = " + SalesId.ToString();
                    dt = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            for (int j = 1; j <= Convert.ToInt32(dt.Rows[i]["Qty"].ToString()); j++)
                            {
                                Response.Write(
                                @"<tr id='tr_" + rowIndex.ToString() + @"'>
                                <td style='display:;'><input type='hidden' id='slno' value='" + rowIndex.ToString() + "'><input type='hidden' id='product_type_" + rowIndex.ToString() + "' value='" + dt.Rows[i]["IsPhysicalInventory"].ToString() + "'><input type='hidden' id='SalesDetailId_" + rowIndex.ToString() + "' value='" + dt.Rows[i]["SalesDetailId"].ToString() + "'><input type='checkbox' id='chk_" + rowIndex.ToString() + "' value='" + dt.Rows[i]["SalesDetailId"].ToString() + "' onclick='findTotal(this,2," + rowIndex.ToString() + ")' " + (dt.Rows[i]["ReturnQty"].ToString() == dt.Rows[i]["Qty"].ToString() ? " disabled='disabled' " : " ") + @" /></td>
                                <td>" + dt.Rows[i]["category_name"].ToString() + ", " + dt.Rows[i]["product_name"].ToString() + @"</td>
                                <td>" + dt.Rows[i]["SDOrderNo"].ToString() + @"</td>
                                <td align='right' style='text-align:right' id='td_amt_" + rowIndex.ToString() + "'>" + dt.Rows[i]["SalesAmount"].ToString() + @"</td>
                                <td></td>
                                <td id='td_amount_returned_" + rowIndex.ToString() + @"' align='right' style='text-align:right'></td>
                                <td id='td_status_" + rowIndex.ToString() + "' style='color:red;'>" + (dt.Rows[i]["ReturnQty"].ToString() == dt.Rows[i]["Qty"].ToString() ? "Returned" : "") + @"</td>
                                </tr>");
                                total += Convert.ToDecimal(dt.Rows[i]["SalesAmount"].ToString());
                                rowIndex++;
                            }
                        }

                        HasSD = true;
                    }
                    else
                    {
                        HasSD = false;
                    }
                    Response.Write("<tr><td colspan='3' style='text-align:right; font-weight:bold;'>Total: </td><td style='text-align:right; font-weight:bold;'>" + total.ToString() + "</td></td><td></td><td id='td_total_amount_returned' style='text-align:right; font-weight:bold;'>0.00</td><td></td></tr>");
                    Response.Write("</table>");

                    if (HasSD)
                    {
                        Response.Write("<input type='hidden' id='SDCount' value='" + dt.Rows.Count + "' />");
                        Response.Write("<script>document.getElementById('div_printpdf').style.display = '';</script>");
                    }
                    else
                        Response.Write("<script>document.getElementById('div_printpdf').style.display = 'none';</script>");
                    if (HasScanableTickets)
                    {
                        Response.Write("<script>document.getElementById('div_scan_now').style.display = '';</script>");
                        Response.Write("<script>document.getElementById('td_sales_serialno').style.display = 'none';</script>");
                    }
                    else
                    {
                        Response.Write("<script>document.getElementById('div_scan_now').style.display = 'none';</script>");
                        Response.Write("<script>document.getElementById('td_sales_serialno').style.display = 'none';</script>");
                    }

                    DBConn = null;
                    Response.End();
                    break;

                case "submitReturnNew":
                    DBConn = new DBConnection();
                    try
                    {
                        int SalesMasterId = Convert.ToInt16(Request.QueryString["SalesMasterId"]);
                        decimal Cash = Convert.ToDecimal(Request.QueryString["Cash"]);
                        decimal Credit = Convert.ToDecimal(Request.QueryString["Credit"]);
                        decimal Voucher = Convert.ToDecimal(Request.QueryString["Voucher"]);
                        string[] arrId = Request.QueryString["DetailId"].Split(',');
                        string[] arrType = Request.QueryString["ProductType"].Split(',');
                        //string[] arrExpiryDate = Request.QueryString["ExpiryDate"].Split(',');
                        int intSalesDetailId = 0;
                        int intProductId = 0;
                        decimal decAmount = 0;
                        string strSerialNo = "";

                        if (Cash + Credit + Voucher > 0)
                        {
                            DBConn.BeginTransaction();
                            sql = "insert into SalesReturnMaster (SalesReturnDate, Amount, LocationId, ClerkId, Status) values (now(), " + (Cash + Credit + Voucher).ToString() + ", " + Session["LocationId"] + ", " + Session["UserId"] + ", 1)";
                            int SalesReturnMasterId = DBConn.ExecuteTransactionGetID(sql, CommandType.Text);

                            if (arrId.Length > 0 && Request.QueryString["Id"] != "")
                            {
                                for (int i = 0; i < arrId.Length; i++)
                                {
                                    if (arrType[i] == "0" || arrType[i] == "1")
                                    {
                                        sql = "select ProductId, SalesDetailId, (select SalesAmount from SalesDetails where SalesDetailId = ProductDetail.SalesDetailId and ProductId = ProductDetail.ProductId and FIND_IN_SET(ProductDetail.SerialNo,SalesDetails.SerialNo)) as Amount, SerialNo from ProductDetail where ProductDetailId = " + arrId[i] + " and status = 1";
                                        dt = DBConn.ExecuteTransactionDataSet(sql, CommandType.Text).Tables[0];
                                        if (dt.Rows.Count > 0)
                                        {
                                            intProductId = Convert.ToInt32(dt.Rows[0]["ProductId"].ToString());
                                            intSalesDetailId = Convert.ToInt32(dt.Rows[0]["SalesDetailId"].ToString());
                                            decAmount = Convert.ToDecimal(dt.Rows[0]["Amount"].ToString());
                                            strSerialNo = dt.Rows[0]["SerialNo"].ToString();

                                            sql = "insert into SalesReturnDetails (SalesReturnMasterId, SalesDetailId, ProductId,SerialNo,Amount,LocationId,Qty) values (" + SalesReturnMasterId + ", " + intSalesDetailId + ", " + intProductId + ", '" + strSerialNo + "', " + decAmount + ", " + Session["LocationId"] + ", 1) ";
                                            int SalesReturnDetailId = DBConn.ExecuteTransactionGetID(sql, CommandType.Text);

                                            sql = "update ProductDetail set Status = 3, SalesReturnDetailId = " + SalesReturnDetailId + "  where ProductDetailId = " + arrId[i];
                                            DBConn.ExecuteTransaction(sql, CommandType.Text);

                                            sql = "update ProductMaster set TicketCount = TicketCount + 1 where ProductId = " + intProductId;
                                            DBConn.ExecuteTransaction(sql, CommandType.Text);

                                            sql = "update ProductInventoryByLocation set TicketCount = TicketCount + 1 where ProductId = " + intProductId + " and LocationId = " + Session["LocationId"].ToString();
                                            DBConn.ExecuteTransaction(sql, CommandType.Text);
                                        }
                                    }
                                    else
                                    {
                                        sql = "select ProductId, SalesDetailId, SalesAmount from SalesDetails where SalesDetailId = " + arrId[i] + "";
                                        dt = DBConn.ExecuteTransactionDataSet(sql, CommandType.Text).Tables[0];
                                        if (dt.Rows.Count > 0)
                                        {
                                            intProductId = Convert.ToInt32(dt.Rows[0]["ProductId"].ToString());
                                            intSalesDetailId = Convert.ToInt32(dt.Rows[0]["SalesDetailId"].ToString());
                                            decAmount = Convert.ToDecimal(dt.Rows[0]["SalesAmount"].ToString());
                                            strSerialNo = "";

                                            sql = "insert into SalesReturnDetails (SalesReturnMasterId, SalesDetailId, ProductId,SerialNo,Amount,LocationId,Qty) values (" + SalesReturnMasterId + ", " + intSalesDetailId + ", " + intProductId + ", '" + strSerialNo + "', " + decAmount + ", " + Session["LocationId"] + ", 1) ";
                                            int SalesReturnDetailId = DBConn.ExecuteTransactionGetID(sql, CommandType.Text);
                                        }
                                    }
                                }
                            }

                            // Payment part

                            int PaymentType;
                            if (Cash > 0)
                                PaymentType = 1;
                            else if (Credit > 0)
                                PaymentType = 2;
                            else
                                PaymentType = 3;

                            sql = @"insert into PaymentDetails (SalesMasterId, PaymentType, Amount, CardCheckNo, CardName, CardAdd1, CardAdd2, CardCity, CardState, CardZip, ExpiryMonth, ExpiryYear, CardType, CardAuthorizationId, TransType) values 
                                    (" + SalesReturnMasterId + ", " + PaymentType + ", " + (Cash + Credit + Voucher).ToString() + ", '',  '', '', '', '', '', '', 0, 0, '', '0',1)";
                            int PaymentId = DBConn.ExecuteTransactionGetID(sql, CommandType.Text);

                            int ErrResponse = SalesReturnMasterId;

                            if (PaymentType == 2)   //credit card refund
                            {
                                sql = "select CardAuthorizationId, CardCheckNo, Amount from PaymentDetails where SalesMasterId = " + SalesMasterId;
                                DataTable dtPay = DBConn.ExecuteTransactionDataSet(sql, CommandType.Text).Tables[0];
                                if (dtPay.Rows.Count > 0)
                                {
                                    objPayment = new classes.Payment();
                                    //trying to refund assuming its a settled transaction
                                    bool IsRefund = objPayment.RefundPayment(dtPay.Rows[0]["CardAuthorizationId"].ToString(), dtPay.Rows[0]["CardCheckNo"].ToString(), Credit, PaymentId);

                                    if (!IsRefund)
                                    {
                                        if (Credit < Convert.ToDecimal(dtPay.Rows[0]["Amount"].ToString()))
                                        {
                                            //Partial refunds
                                            sql = "Update SalesReturnMaster set Status = 0 where SalesReturnMasterId = " + SalesReturnMasterId;
                                            DBConn.ExecuteTransaction(sql, CommandType.Text);
                                            ErrResponse = -3;
                                        }
                                        else
                                        {
                                            //Void transaction in case of full refund
                                            IsRefund = objPayment.VoidPayment(dtPay.Rows[0]["CardAuthorizationId"].ToString(), dtPay.Rows[0]["CardCheckNo"].ToString(), Credit, PaymentId);
                                            if (!IsRefund)
                                            {
                                                ErrResponse = -1;
                                            }
                                            else
                                            {
                                                sql = "update PaymentDetails set CardAuthorizationId = " + objPayment.TransId + " where PaymentId = " + PaymentId;
                                                DBConn.ExecuteTransaction(sql, CommandType.Text);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ErrResponse = -2;
                                }
                            }

                            if (ErrResponse == SalesReturnMasterId || ErrResponse == -3)
                            {
                                DBConn.CommitTransaction();
                                DBConn = null;
                                Response.Write(ErrResponse.ToString());
                            }
                            else
                            {
                                DBConn.RollBackTransaction();
                                DBConn = null;
                                Response.Write(ErrResponse.ToString());
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        DBConn.RollBackTransaction();
                        DBConn = null;
                        Response.Write("0");
                    }
                    break;

               /* case "GetUserDetails":
                    DBConn = new DBConnection();

                    string Username = Request.QueryString["username"];
                    if (classes.Users.IsUsernameExists(Username))
                    {
                        classes.Users objUsers = new classes.Users();
                        DataSet ds = objUsers.GetUser(Username);
                        Response.Write(ds.GetXml());
                    }
                    else
                    {
                        Response.Write("");
                    }
                    break; */
            }
            Response.End();
        }
    }
}