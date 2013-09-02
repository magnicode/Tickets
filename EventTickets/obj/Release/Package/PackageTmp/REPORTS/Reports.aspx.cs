using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using EventFunctions;
using MYSQLDatabase;
using EventTickets.classes;

namespace HR_PAYROLL.Reports
{
    public partial class Reports : System.Web.UI.Page
    {
        ReportDocument doc = new ReportDocument();
        bool isNavigated = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            

            try
            {
                isNavigated = false;
                string UserId = EventFunctions.Functions.GetSessionUserId();
                if (UserId == "")
                    return;

                HideWindow();
                string sCmd = ("" + Request.QueryString["cmd"]).ToString();
                if ((Request.QueryString["ReportName"] != null) && (Request.QueryString["DataSource"] != null) && sCmd != "finished")
                {
                    //BL.Report repObj = new BL.Report();
                    string _reportname = null;
                    DataTable _datasource = null;
                    string _reportselectionformula = null;
                    bool _grouptree = false;
                    string _reporttitle = null;
                    string _printType = null;
                    DataTable[] _dt = new DataTable[10];
                    _reportname = Request.QueryString["ReportName"];
                    if (Request.QueryString["ReportSelectionFormula"] != null)
                    {
                        _reportselectionformula = Request.QueryString["ReportSelectionFormula"];
                        _reportselectionformula = _reportselectionformula.Replace("~", "'");
                    }
                    _grouptree = Convert.ToBoolean(Request.QueryString["GroupTree"]);
                    _reporttitle = Request.QueryString["ReportTitle"];
                    _printType = Request.QueryString["PrintType"];
                    /*repObj.DateParameter1 = Request.QueryString["DateParameter1"];
                    repObj.DateParameter2 = Request.QueryString["DateParameter2"];
                    repObj.DateParameter3 = Request.QueryString["DateParameter3"];
                    repObj.DateParameter4 = Request.QueryString["DateParameter4"];
                    repObj.DecimalParameter1 = Request.QueryString["DecimalParameter1"];
                    repObj.DecimalParameter2 = Request.QueryString["DecimalParameter2"];
                    repObj.StringParameter1 = "" + Request.QueryString["sP1"];
                    repObj.StringParameter2 = "" + Request.QueryString["sP2"];
                    repObj.StringParameter3 = "" + Request.QueryString["sP3"];
                    repObj.StringParameter4 = "" + Request.QueryString["sP4"];
                    repObj.StringParameter5 = "" + Request.QueryString["sP5"];
                    repObj.StringParameter6 = "" + Request.QueryString["sP6"];
                    repObj.BoolParameter1 = Request.QueryString["BoolParameter1"];
                    repObj.BoolParameter2 = Request.QueryString["BoolParameter2"];
                    if (("" + Request.QueryString["StringArray"]).ToString().Trim() != "")
                        repObj.StringArray = Request.QueryString["StringArray"].Trim().Split(',');
                    int iSubRptCnt = System.Common.Functions.ToInt(Request.QueryString["SubReports"]);
                    if (iSubRptCnt > 0)
                        _dt = repObj.LoadReportDatasourceArray(Request.QueryString["DataSource"]);
                    else
                        _datasource = repObj.LoadReportDatasource(Request.QueryString["DataSource"]);*/
                    int SalesId = Functions.ToInt(Request.QueryString["SalesId"]);
                    int iSubRptCnt = 0;
                    ParameterValues myparameterValues = new ParameterValues();
                    ParameterDiscreteValue myparamDiscreteValue = new ParameterDiscreteValue();

                    DBConnection DBConn = new DBConnection();
                    switch (_reportname)
                    {
                        case "Receipt":
                            string sql = "select count(*) as cnt from ProductDetail P, SalesDetails S where P.SalesDetailId = S.SalesDetailId and status = 3 and S.SalesMasterId = " + SalesId;
                            string returnCnt = DBConn.RetData(sql, CommandType.Text);

                            _datasource = DBConn.ExecuteDataSet("SELECT SalesMaster.SalesMasterId SalesId,DATE_FORMAT(SalesDate,'%m/%d/%Y') SalesDate,CustomerName,DATE_FORMAT(SalesDate,'%h:%i %p')  as `SalesTime`,concat(IFNULL((SELECT Name FROM ProductMaster WHERE ProductMaster.ProductId = SalesDetails.ProductId),''), IFNULL(SalesDetails.ProductName,''), IFNULL(concat(' (SD Order #: ',SDOrderNo,')'),'')) as Description,Count as Quantity,SalesAmount as Price,'' as PayType,0 as PayAmount, " + returnCnt + " as ReturnCount FROM SalesMaster,SalesDetails where SalesMaster.SalesMasterId = SalesDetails.SalesMasterId and SalesMaster.SalesMasterId= " + SalesId, CommandType.Text).Tables[0];
                            _reportname = "Receipt.rpt";
                            break;
                        case "Sales":
                            string sFrom = "" + Request.QueryString["fromd"];
                            string sTo = "" + Request.QueryString["tod"];
                            DateTime dt1;
                            sql = "SELECT '0' as type, 0 as SRID, SalesMasterId ID, DATE_FORMAT(SalesDate,'%m/%d/%Y %h:%i %p') SalesDate, SalesDate sdt, PurchaseAmount as Amount, GrandTotal GT, (SELECT Name FROM LocationMaster WHERE LocationMaster.LocationId = SalesMaster.LocationId) as Location, (SELECT UserName FROM UserMaster WHERE UserMaster.UserId = SalesMaster.ClerkId) as Clerk, CustomerName, (SELECT SUM(COUNT) FROM SalesDetails WHERE SalesMasterId = SalesMaster.SalesMasterId) ticket,  ";
                            if (sFrom.Trim() != "" && sTo.Trim() != "")
                            {
                                sql += "'" + sFrom + "' sFrom,'" + sTo + "' sTo FROM SalesMaster where 1=1";
                                dt1 = DateTime.Parse(sFrom);
                                sFrom = dt1.ToString("yyyy-MM-dd");
                                dt1 = DateTime.Parse(sTo);
                                sTo = dt1.ToString("yyyy-MM-dd");
                                sql += " and (SalesDate between '" + sFrom + " 00:00:01' and '" + sTo + " 23:59:59')";
                            }
                            else if (sFrom.Trim() != "")
                            {
                                sql += "'" + sFrom + "' sFrom,'' sTo FROM SalesMaster where 1=1";
                                dt1 = DateTime.Parse(sFrom);
                                sFrom = dt1.ToString("yyyy-MM-dd");
                                sql += " and SalesDate >= '" + sFrom + " 00:00:01'";
                            }
                            else if (sTo.Trim() != "")
                            {
                                sql += "'' sFrom,'" + sTo + "' sTo FROM SalesMaster where 1=1";
                                dt1 = DateTime.Parse(sTo);
                                sTo = dt1.ToString("yyyy-MM-dd");
                                sql += " and SalesDate <= '" + sTo + " 23:59:59'";
                            }
                            else
                                sql += "'' sFrom,'' sTo FROM SalesMaster where 1=1";
                            //sql += " order by sdt";

                            sql += " union SELECT '1' as type, SalesReturnMasterId SRID, (SELECT SalesMasterId from SalesDetails, SalesReturnDetails WHERE SalesDetails.SalesDetailId = SalesReturnDetails.SalesDetailId and SalesReturnDetails.SalesReturnMasterId = SalesReturnMaster.SalesReturnMasterId limit 1) as ID, DATE_FORMAT(SalesReturnDate,'%m/%d/%Y %h:%i %p') SalesDate, SalesReturnDate sdt, 0 as Amount, Amount GT, (SELECT Name FROM LocationMaster WHERE LocationMaster.LocationId = SalesReturnMaster.LocationId) as Location, (SELECT UserName FROM UserMaster WHERE UserMaster.UserId = SalesReturnMaster.ClerkId) as Clerk, '' as CustomerName, (SELECT SUM(Qty) FROM SalesReturnDetails WHERE SalesReturnMasterId = SalesReturnMaster.SalesReturnMasterId) ticket,  ";
                            if (sFrom.Trim() != "" && sTo.Trim() != "")
                            {
                                sql += "'" + sFrom + "' sFrom,'" + sTo + "' sTo FROM SalesReturnMaster where 1=1";
                                dt1 = DateTime.Parse(sFrom);
                                sFrom = dt1.ToString("yyyy-MM-dd");
                                dt1 = DateTime.Parse(sTo);
                                sTo = dt1.ToString("yyyy-MM-dd");
                                sql += " and (SalesReturnDate between '" + sFrom + " 00:00:01' and '" + sTo + " 23:59:59')";
                            }
                            else if (sFrom.Trim() != "")
                            {
                                sql += "'" + sFrom + "' sFrom,'' sTo FROM SalesReturnMaster where 1=1";
                                dt1 = DateTime.Parse(sFrom);
                                sFrom = dt1.ToString("yyyy-MM-dd");
                                sql += " and SalesReturnDate >= '" + sFrom + " 00:00:01'";
                            }
                            else if (sTo.Trim() != "")
                            {
                                sql += "'' sFrom,'" + sTo + "' sTo FROM SalesReturnMaster where 1=1";
                                dt1 = DateTime.Parse(sTo);
                                sTo = dt1.ToString("yyyy-MM-dd");
                                sql += " and SalesReturnDate <= '" + sTo + " 23:59:59'";
                            }
                            else
                                sql += "'' sFrom,'' sTo FROM SalesReturnMaster where 1=1";
                            sql += " order by type, sdt";

                            _datasource = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                            _reportname = "Sales.rpt";
                            break;
                        case "SalesDetails":
                            sFrom = "" + Request.QueryString["fromd"];
                            sTo = "" + Request.QueryString["tod"];
                            sql = @"SELECT '0' as type, 0 as SalesMasterId, SalesMaster.SalesMasterId SalesId,DATE_FORMAT(SalesDate,'%m/%d/%Y %h:%i %p') SalesDate,PurchaseAmount AS Amount,
                                    DATE_FORMAT(SalesDate,'%m/%d/%Y') SD,SalesDetails.SerialNo,
                                    (SELECT NAME FROM LocationMaster WHERE LocationMaster.LocationId = SalesMaster.LocationId) AS Location,
                                    (SELECT UserName FROM UserMaster WHERE UserMaster.UserId = SalesMaster.ClerkId) AS Clerk,CustomerName,
                                    concat(IFNULL((SELECT Name FROM ProductMaster WHERE ProductMaster.ProductId = SalesDetails.ProductId),''), IFNULL(SalesDetails.ProductName,'')) AS Description,
                                    COUNT AS Quantity,SalesAmount AS Price,
                                    (SELECT CASE WHEN PaymentType=1 THEN 'Cash' WHEN PaymentType=2 THEN 'Card' Else 'Vouher' END FROM PaymentDetails WHERE SalesMasterId = SalesMaster.SalesMasterId) paytype,
                                    (SELECT CardAuthorizationId FROM PaymentDetails WHERE SalesMasterId = SalesMaster.SalesMasterId) authcode,";
                            if (sFrom.Trim() != "" && sTo.Trim() != "")
                            {
                                sql += "'" + sFrom + "' sFrom,'" + sTo + "' sTo FROM SalesMaster,SalesDetails WHERE SalesMaster.SalesMasterId = SalesDetails.SalesMasterId ";
                                dt1 = DateTime.Parse(sFrom);
                                sFrom = dt1.ToString("yyyy-MM-dd");
                                dt1 = DateTime.Parse(sTo);
                                sTo = dt1.ToString("yyyy-MM-dd");
                                sql += " and (SalesMaster.SalesDate between '" + sFrom + " 00:00:01' and '" + sTo + " 23:59:59')";
                            }
                            else if (sFrom.Trim() != "")
                            {
                                sql += "'" + sFrom + "' sFrom,'' sTo FROM SalesMaster,SalesDetails WHERE SalesMaster.SalesMasterId = SalesDetails.SalesMasterId";
                                dt1 = DateTime.Parse(sFrom);
                                sFrom = dt1.ToString("yyyy-MM-dd");
                                sql += " and SalesMaster.SalesDate >= '" + sFrom + " 00:00:01'";
                            }
                            else if (sTo.Trim() != "")
                            {
                                sql += "'' sFrom,'" + sTo + "' sTo FROM SalesMaster,SalesDetails WHERE SalesMaster.SalesMasterId = SalesDetails.SalesMasterId";
                                dt1 = DateTime.Parse(sTo);
                                sTo = dt1.ToString("yyyy-MM-dd");
                                sql += " and SalesMaster.SalesDate <= '" + sTo + " 23:59:59'";
                            }
                            else
                                sql += "'' sFrom,'' sTo FROM SalesMaster,SalesDetails WHERE SalesMaster.SalesMasterId = SalesDetails.SalesMasterId";
                            //sql += " order by SalesMaster.SalesDate";


                            sql += @" union SELECT '1' as type, SalesReturnMaster.SalesReturnMasterId SalesMasterId, (select SalesMasterId from SalesDetails where SalesDetailId = SalesReturnDetails.SalesDetailId limit 1) as SalesId, DATE_FORMAT(SalesReturnDate,'%m/%d/%Y %h:%i %p') SalesDate, SalesReturnMaster.Amount AS Amount,
                                    DATE_FORMAT(SalesReturnDate,'%m/%d/%Y') SD, SalesReturnDetails.SerialNo,
                                    (SELECT NAME FROM LocationMaster WHERE LocationMaster.LocationId = SalesReturnMaster.LocationId) AS Location,
                                    (SELECT UserName FROM UserMaster WHERE UserMaster.UserId = SalesReturnMaster.ClerkId) AS Clerk, '' as CustomerName,
                                    (SELECT NAME FROM ProductMaster WHERE ProductMaster.ProductId = SalesReturnDetails.ProductId) AS Description,
                                    Qty AS Quantity, SalesReturnDetails.Amount AS Price,
                                    (SELECT CASE WHEN PaymentType=1 THEN 'Cash' WHEN PaymentType=2 THEN 'Card' Else 'Vouher' END FROM PaymentDetails WHERE SalesMasterId = SalesReturnMaster.SalesReturnMasterId) paytype,
                                    '' as authcode,";
                            if (sFrom.Trim() != "" && sTo.Trim() != "")
                            {
                                sql += "'" + sFrom + "' sFrom,'" + sTo + "' sTo FROM SalesReturnMaster, SalesReturnDetails WHERE SalesReturnMaster.SalesReturnMasterId = SalesReturnDetails.SalesReturnMasterId ";
                                dt1 = DateTime.Parse(sFrom);
                                sFrom = dt1.ToString("yyyy-MM-dd");
                                dt1 = DateTime.Parse(sTo);
                                sTo = dt1.ToString("yyyy-MM-dd");
                                sql += " and (SalesReturnMaster.SalesReturnDate between '" + sFrom + " 00:00:01' and '" + sTo + " 23:59:59')";
                            }
                            else if (sFrom.Trim() != "")
                            {
                                sql += "'" + sFrom + "' sFrom,'' sTo FROM SalesReturnMaster,SalesReturnDetails WHERE SalesReturnMaster.SalesReturnMasterId = SalesReturnDetails.SalesReturnMasterId";
                                dt1 = DateTime.Parse(sFrom);
                                sFrom = dt1.ToString("yyyy-MM-dd");
                                sql += " and SalesReturnMaster.SalesReturnDate >= '" + sFrom + " 00:00:01'";
                            }
                            else if (sTo.Trim() != "")
                            {
                                sql += "'' sFrom,'" + sTo + "' sTo FROM SalesReturnMaster,SalesReturnDetails WHERE SalesReturnMaster.SalesReturnMasterId = SalesReturnDetails.SalesReturnMasterId";
                                dt1 = DateTime.Parse(sTo);
                                sTo = dt1.ToString("yyyy-MM-dd");
                                sql += " and SalesReturnMaster.SalesReturnDate <= '" + sTo + " 23:59:59'";
                            }
                            else
                                sql += "'' sFrom,'' sTo FROM SalesReturnMaster,SalesReturnDetails WHERE SalesReturnMaster.SalesReturnMasterId = SalesReturnDetails.SalesReturnMasterId";
                            sql += " order by SalesDate";


                            _datasource = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                            _reportname = "SalesDetails.rpt";
                            break;
                        case "EventSoldCount":
                            sFrom = "" + Request.QueryString["fromd"];
                            sTo = "" + Request.QueryString["tod"];
                            sql = @"select '0' as type, L.Name as location_name, count(*) as product_count, P.Name as product_name, '" + sFrom + "' as from_date, '" + sTo + @"' as to_date from SalesMaster S, SalesDetails D, LocationMaster L, ProductMaster P, ProductDetail PD
                                            where S.LocationId = L.LocationId and S.SalesMasterId = D.SalesMasterId and D.ProductId = P.ProductId and P.ProductId = PD.ProductId and PD.SalesDetailId = D.SalesDetailId";

                            if (sFrom != "")
                                sql += " and S.SalesDate >= '" + Convert.ToDateTime(sFrom).ToString("yyyy-MM-dd") + " 00:00:00' ";
                            if (sTo != "")
                                sql += " and S.SalesDate <= '" + Convert.ToDateTime(sTo).ToString("yyyy-MM-dd") + @" 23:59:59'";
                            sql += " group by L.LocationId, L.Name, P.ProductId, P.Name ";

                            sql += @" union select '1' as type, L.Name as location_name, count(*) as product_count, P.Name as product_name, '" + sFrom + "' as from_date, '" + sTo + @"' as to_date from SalesReturnMaster S, SalesReturnDetails D, LocationMaster L, ProductMaster P, ProductDetail PD
                                            where S.LocationId = L.LocationId and S.SalesReturnMasterId = D.SalesReturnMasterId and D.ProductId = P.ProductId and P.ProductId = PD.ProductId and PD.SalesReturnDetailId = D.SalesReturnDetailId";

                            if (sFrom != "")
                                sql += " and S.SalesReturnDate >= '" + Convert.ToDateTime(sFrom).ToString("yyyy-MM-dd") + " 00:00:00' ";
                            if (sTo != "")
                                sql += " and S.SalesReturnDate <= '" + Convert.ToDateTime(sTo).ToString("yyyy-MM-dd") + @" 23:59:59'";
                            sql += " group by type, L.LocationId, L.Name, P.ProductId, P.Name order by type, location_name, product_name";
                             
                            _datasource = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                            _reportname = "EventSoldCount.rpt";
                            break;

                        case "CategorySummary":
                            string CategoryId = "" + Request.QueryString["category_id"];
                            string CategoryName = "" + Request.QueryString["category_name"];
                            sFrom = "" + Request.QueryString["fromd"];
                            sTo = "" + Request.QueryString["tod"];

                            sql = "select * from Category where CategoryId = " + CategoryId;
                            DataTable dt = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                            if (("" + dt.Rows[0]["IsPhysicalInventory"].ToString()).ToString() == "1")
                            {
                                sql = @"select '0' as type, '" + CategoryName + " Pass Summary by Date' as Title, '" + sFrom + "' as from_date, '" + sTo + @"' as to_date, S.SalesDate as purchase_date, P.Name as product_name, 1 as product_count, '' as voucher_no, PD.SerialNo as serial_no, D.SalesAmount as purchase_price
                                    from SalesMaster S, SalesDetails D, ProductMaster P, ProductDetail PD
                                    where S.SalesMasterId = D.SalesMasterId and D.ProductId = P.ProductId and P.ProductId = PD.ProductId and PD.SalesDetailId = D.SalesDetailId and P.CategoryId = " + CategoryId;
                                if (sFrom != "")
                                    sql += " and S.SalesDate >= '" + Convert.ToDateTime(sFrom).ToString("yyyy-MM-dd") + " 00:00:00' ";
                                if (sTo != "")
                                    sql += " and S.SalesDate <= '" + Convert.ToDateTime(sTo).ToString("yyyy-MM-dd") + @" 23:59:59'";

                                sql += @" union select '1' as type, '" + CategoryName + " Pass Summary by Date' as Title, '" + sFrom + "' as from_date, '" + sTo + @"' as to_date, S.SalesReturnDate as purchase_date, P.Name as product_name, 1 as product_count, '' as voucher_no, PD.SerialNo as serial_no, D.Amount as purchase_price
                                    from SalesReturnMaster S, SalesReturnDetails D, ProductMaster P, ProductDetail PD
                                    where S.SalesReturnMasterId = D.SalesReturnMasterId and D.ProductId = P.ProductId and P.ProductId = PD.ProductId and PD.SalesReturnDetailId = D.SalesReturnDetailId and P.CategoryId = " + CategoryId;
                                if (sFrom != "")
                                    sql += " and S.SalesReturnDate >= '" + Convert.ToDateTime(sFrom).ToString("yyyy-MM-dd") + " 00:00:00' ";
                                if (sTo != "")
                                    sql += " and S.SalesReturnDate <= '" + Convert.ToDateTime(sTo).ToString("yyyy-MM-dd") + @" 23:59:59'";
                                sql += " order by type, purchase_date, product_name, serial_no";

                            }
                            else
                            {
                                sql = @"select '0' as type, '" + CategoryName + " Pass Summary by Date' as Title, '" + sFrom + "' as from_date, '" + sTo + @"' as to_date, S.SalesDate as purchase_date, 
                                    concat(IFNULL((SELECT Name FROM ProductMaster WHERE ProductMaster.ProductId = D.ProductId),''), IFNULL(D.ProductName,'')) as product_name, 
                                    1 as product_count, '' as voucher_no, 
                                    case when IsSmartDest = 0 then (select SerialNo from ProductDetail where ProductId = D.ProductId and SalesDetailId = D.SalesDetailId) else '' as serial_no, D.SalesAmount as purchase_price
                                    from SalesMaster S, SalesDetails D, ProductDetail PD
                                    where S.SalesMasterId = D.SalesMasterId and D.ProductId = PD.ProductId and PD.SalesDetailId = D.SalesDetailId and P.CategoryId = " + CategoryId;
                                if (sFrom != "")
                                    sql += " and S.SalesDate >= '" + Convert.ToDateTime(sFrom).ToString("yyyy-MM-dd") + " 00:00:00' ";
                                if (sTo != "")
                                    sql += " and S.SalesDate <= '" + Convert.ToDateTime(sTo).ToString("yyyy-MM-dd") + @" 23:59:59'";

                                sql += @" union select '1' as type, '" + CategoryName + " Pass Summary by Date' as Title, '" + sFrom + "' as from_date, '" + sTo + @"' as to_date, S.SalesReturnDate as purchase_date, concat(IFNULL((SELECT Name FROM ProductMaster WHERE ProductMaster.ProductId = D.ProductId),''), IFNULL(D.ProductName,'')) as product_name, 1 as product_count, '' as voucher_no, PD.SerialNo as serial_no, D.Amount as purchase_price
                                    from SalesReturnMaster S, SalesReturnDetails D, ProductDetail PD
                                    where S.SalesReturnMasterId = D.SalesReturnMasterId and D.ProductId = P.ProductId and P.ProductId = PD.ProductId and PD.SalesReturnDetailId = D.SalesReturnDetailId and P.CategoryId = " + CategoryId;
                                if (sFrom != "")
                                    sql += " and S.SalesReturnDate >= '" + Convert.ToDateTime(sFrom).ToString("yyyy-MM-dd") + " 00:00:00' ";
                                if (sTo != "")
                                    sql += " and S.SalesReturnDate <= '" + Convert.ToDateTime(sTo).ToString("yyyy-MM-dd") + @" 23:59:59'";
                                sql += " order by purchase_date, product_name, serial_no";
                            }
                            _datasource = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                            _reportname = "CategorySummary.rpt";
                            break;

                        case "ClerkSummary":
                            string LocationId = "" + Request.QueryString["location_id"];
                            sFrom = "" + Request.QueryString["fromd"];
                            //sTo = "" + Request.QueryString["tod"];

                            sql = @"SELECT '0' as type, '" + sFrom + @"' as from_date, '' as to_date, L.Name as location_name, U.UserName as user_name,
                                    Sum(case when PaymentType = 1 then 1 else 0 end) as cash_count, Sum(case when PaymentType = 1 then P.Amount else 0 end) as cash_total,
                                    Sum(case when PaymentType = 2 then 1 else 0 end) as credit_count, Sum(case when PaymentType = 2 then P.Amount else 0 end) as credit_total,
                                    Sum(case when PaymentType = 3 then 1 else 0 end) as voucher_count, Sum(case when PaymentType = 3 then P.Amount else 0 end) as voucher_total
                                    FROM SalesMaster S, PaymentDetails P, LocationMaster L, UserMaster U
                                    WHERE S.SalesMasterId = P.SalesMasterId and P.TransType = 0 and S.LocationId = L.LocationId and S.ClerkId = U.UserId and S.LocationId = " + LocationId ;
                            if (sFrom != "")
                                sql += " and S.SalesDate between '" + Convert.ToDateTime(sFrom).ToString("yyyy-MM-dd") + " 00:00:00' and  '" + Convert.ToDateTime(sFrom).ToString("yyyy-MM-dd") + " 23:59:59'";
                            sql += " group by L.Name, U.UserName  ";
                            sql += " union ";
                            
                            sql += @"SELECT '1' as type, '" + sFrom + @"' as from_date, '' as to_date, L.Name as location_name, U.UserName as user_name,
                                    Sum(case when PaymentType = 1 then 1 else 0 end) as cash_count, Sum(case when PaymentType = 1 then P.Amount else 0 end) as cash_total,
                                    Sum(case when PaymentType = 2 then 1 else 0 end) as credit_count, Sum(case when PaymentType = 2 then P.Amount else 0 end) as credit_total,
                                    Sum(case when PaymentType = 3 then 1 else 0 end) as voucher_count, Sum(case when PaymentType = 3 then P.Amount else 0 end) as voucher_total
                                    FROM SalesReturnMaster S, PaymentDetails P, LocationMaster L, UserMaster U
                                    WHERE S.SalesReturnMasterId = P.SalesMasterId and P.TransType = 1 and S.LocationId = L.LocationId and S.ClerkId = U.UserId and S.LocationId = " + LocationId;
                            if (sFrom != "")
                                sql += " and S.SalesReturnDate between '" + Convert.ToDateTime(sFrom).ToString("yyyy-MM-dd") + " 00:00:00' and  '" + Convert.ToDateTime(sFrom).ToString("yyyy-MM-dd") + " 23:59:59'";
                            sql += " group by L.Name, U.UserName  ";
 
                            _datasource = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                            _reportname = "ClerkSummary.rpt";
                            break;
                        case "VoucherSummary":
                            CategoryId = "" + Request.QueryString["category_id"];
                            CategoryName = "" + Request.QueryString["category_name"];
                            sFrom = "" + Request.QueryString["fromd"];
                            sTo = "" + Request.QueryString["tod"];
                            if (sTo == "")
                                sTo = sFrom;
                            sql = @"select '"+ CategoryName +"' as Title, '"+ sFrom +"' FromDate, '"+ sTo +@"' ToDate, SM.SalesMasterId, SM.SalesDate as `Date`, PD.CardCheckNo as VoucherNumber, PM.Name, SD.SerialNo as SerialNumber from
                                    SalesMaster SM, PaymentDetails PD, ProductMaster PM, SalesDetails SD where
                                    SM.SalesMasterId = PD.SalesMasterId AND SD.SalesMasterId = SM.SalesMasterId AND
                                    SD.ProductId = PM.ProductId AND PD.PaymentType = 3 AND PM.CategoryId = " + CategoryId;
                            if (sFrom != "")
                                sql += " and SM.SalesDate  between '" + Convert.ToDateTime(sFrom).ToString("yyyy-MM-dd") + " 00:00:00' and '" + Convert.ToDateTime(sTo).ToString("yyyy-MM-dd") + @" 23:59:59'";
                             _datasource = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                             _reportname = "VoucherSummary.rpt";
                            break;

                        case "SDReturns":

                            sFrom = "" + Request.QueryString["fromd"];
                            sTo = "" + Request.QueryString["tod"];
                            sql = @"SELECT 'Smart Destination Returns' as title, '" + sFrom + @"' as from_date, '" + sTo + @"' as to_date, SD.SalesMasterId as sales_id, date_format(SR.SalesReturnDate,'%m/%d/%Y') as return_date, 
                                    SD.ProductName as product_name, SRD.qty, SRD.amount, UM.UserName as user_name, concat(' ',SDOrderNo) as SDOrderNo
                                    FROM SalesReturnMaster SR, SalesReturnDetails SRD, SalesMaster SM, SalesDetails SD, UserMaster UM
                                    WHERE SR.SalesReturnMasterId = SRD.SalesReturnMasterId and SRD.SalesDetailId = SD.SalesDetailId and SM.SalesMasterId = SD.SalesMasterId and SD.IsSmartDest = 1 and SRD.ProductId = SD.ProductId and SR.ClerkId = UM.UserId and SR.LocationId = " + Session["LocationId"].ToString();
                            if (sFrom != "")
                                sql += " and SR.SalesReturnDate >= '" + Convert.ToDateTime(sFrom).ToString("yyyy-MM-dd") + " 00:00:00' ";
                            if (sTo != "")
                                sql += " and SR.SalesReturnDate <= '" + Convert.ToDateTime(sTo).ToString("yyyy-MM-dd") + @" 23:59:59'";sql += "";
                            sql += " order by SR.SalesReturnDate";

                            _datasource = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                             _reportname = "SDReturns.rpt";

                            break;

                        case "UserLog":
                            sFrom = "" + Request.QueryString["fromd"];
                            sTo = "" + Request.QueryString["tod"];
                            sql = @"SELECT username as user_name, LoginDateTime AS login_date, LogoutDatetime AS logout_date, IP
                                    FROM UserLog, UserMaster
                                    WHERE UserLog.UserId = UserMaster.UserId ";
                            if (sFrom != "")
                                sql += " and LoginDateTime >= '" + Convert.ToDateTime(sFrom).ToString("yyyy-MM-dd") + " 00:00:00' ";
                            if (sTo != "")
                                sql += " and LoginDateTime <= '" + Convert.ToDateTime(sTo).ToString("yyyy-MM-dd") + @" 23:59:59'"; 
                            sql += " ORDER BY UserName, LoginDateTime";
                            _datasource = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                            _reportname = "UserLog.rpt";
                                     
                            break;

                        case "SalesReturn":
                            sFrom = "" + Request.QueryString["fromd"];
                            sTo = "" + Request.QueryString["tod"];
                            sql = " SELECT SalesReturnMasterId SRID, (SELECT SalesMasterId from SalesDetails, SalesReturnDetails WHERE SalesDetails.SalesDetailId = SalesReturnDetails.SalesDetailId and SalesReturnDetails.SalesReturnMasterId = SalesReturnMaster.SalesReturnMasterId limit 1) as ID, (select DATE_FORMAT(SalesDate,'%m/%d/%Y %h:%i %p') from SalesDetails, SalesReturnDetails, SalesMaster WHERE SalesMaster.`SalesMasterId` = SalesDetails.`SalesMasterId` AND SalesDetails.SalesDetailId = SalesReturnDetails.SalesDetailId and SalesReturnDetails.SalesReturnMasterId = SalesReturnMaster.SalesReturnMasterId limit 1) SalesDate, DATE_FORMAT(SalesReturnDate,'%m/%d/%Y %h:%i %p') SalesReturnDate, Amount, (SELECT Name FROM LocationMaster WHERE LocationMaster.LocationId = SalesReturnMaster.LocationId) as Location, (SELECT UserName FROM UserMaster WHERE UserMaster.UserId = SalesReturnMaster.ClerkId) as Clerk, '' as CustomerName, (SELECT SUM(Qty) FROM SalesReturnDetails WHERE SalesReturnMasterId = SalesReturnMaster.SalesReturnMasterId) ticket,  ";
                            if (sFrom.Trim() != "" && sTo.Trim() != "")
                            {
                                sql += "'" + sFrom + "' sFrom,'" + sTo + "' sTo FROM SalesReturnMaster where 1=1";
                                dt1 = DateTime.Parse(sFrom);
                                sFrom = dt1.ToString("yyyy-MM-dd");
                                dt1 = DateTime.Parse(sTo);
                                sTo = dt1.ToString("yyyy-MM-dd");
                                sql += " and (SalesReturnDate between '" + sFrom + " 00:00:01' and '" + sTo + " 23:59:59')";
                            }
                            else if (sFrom.Trim() != "")
                            {
                                sql += "'" + sFrom + "' sFrom,'' sTo FROM SalesReturnMaster where 1=1";
                                dt1 = DateTime.Parse(sFrom);
                                sFrom = dt1.ToString("yyyy-MM-dd");
                                sql += " and SalesReturnDate >= '" + sFrom + " 00:00:01'";
                            }
                            else if (sTo.Trim() != "")
                            {
                                sql += "'' sFrom,'" + sTo + "' sTo FROM SalesReturnMaster where 1=1";
                                dt1 = DateTime.Parse(sTo);
                                sTo = dt1.ToString("yyyy-MM-dd");
                                sql += " and SalesReturnDate <= '" + sTo + " 23:59:59'";
                            }
                            else
                                sql += "'' sFrom,'' sTo FROM SalesReturnMaster where 1=1";
                            sql += " order by SalesReturnDate";

                            _datasource = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                            _reportname = "SalesReturn.rpt";
                            break;

                        case "SalesReturnDetails":
                            sFrom = "" + Request.QueryString["fromd"];
                            sTo = "" + Request.QueryString["tod"];
                            sql = @" SELECT '1' as type, SalesReturnMaster.SalesReturnMasterId SalesMasterId, (select SalesMasterId from SalesDetails where SalesDetailId = SalesReturnDetails.SalesDetailId limit 1) as SalesId, DATE_FORMAT(SalesReturnDate,'%m/%d/%Y %h:%i %p') SalesReturnDate, SalesReturnMaster.Amount AS Amount,
                                    (select DATE_FORMAT(SalesDate,'%m/%d/%Y %h:%i %p') from SalesDetails, SalesReturnDetails, SalesMaster WHERE SalesMaster.`SalesMasterId` = SalesDetails.`SalesMasterId` AND SalesDetails.SalesDetailId = SalesReturnDetails.SalesDetailId and SalesReturnDetails.SalesReturnMasterId = SalesReturnMaster.SalesReturnMasterId limit 1) SalesDate, SalesReturnDetails.SerialNo,
                                    (SELECT NAME FROM LocationMaster WHERE LocationMaster.LocationId = SalesReturnMaster.LocationId) AS Location,
                                    (SELECT UserName FROM UserMaster WHERE UserMaster.UserId = SalesReturnMaster.ClerkId) AS Clerk, '' as CustomerName,
                                    (SELECT NAME FROM ProductMaster WHERE ProductMaster.ProductId = SalesReturnDetails.ProductId) AS Description,
                                    Qty AS Quantity, SalesReturnDetails.Amount AS Price,
                                    (SELECT CASE WHEN PaymentType=1 THEN 'Cash' WHEN PaymentType=2 THEN 'Card' Else 'Vouher' END FROM PaymentDetails WHERE SalesMasterId = SalesReturnMaster.SalesReturnMasterId) paytype,
                                    '' as authcode,";
                            if (sFrom.Trim() != "" && sTo.Trim() != "")
                            {
                                sql += "'" + sFrom + "' sFrom,'" + sTo + "' sTo FROM SalesReturnMaster, SalesReturnDetails WHERE SalesReturnMaster.SalesReturnMasterId = SalesReturnDetails.SalesReturnMasterId ";
                                dt1 = DateTime.Parse(sFrom);
                                sFrom = dt1.ToString("yyyy-MM-dd");
                                dt1 = DateTime.Parse(sTo);
                                sTo = dt1.ToString("yyyy-MM-dd");
                                sql += " and (SalesReturnMaster.SalesReturnDate between '" + sFrom + " 00:00:01' and '" + sTo + " 23:59:59')";
                            }
                            else if (sFrom.Trim() != "")
                            {
                                sql += "'" + sFrom + "' sFrom,'' sTo FROM SalesReturnMaster,SalesReturnDetails WHERE SalesReturnMaster.SalesReturnMasterId = SalesReturnDetails.SalesReturnMasterId";
                                dt1 = DateTime.Parse(sFrom);
                                sFrom = dt1.ToString("yyyy-MM-dd");
                                sql += " and SalesReturnMaster.SalesReturnDate >= '" + sFrom + " 00:00:01'";
                            }
                            else if (sTo.Trim() != "")
                            {
                                sql += "'' sFrom,'" + sTo + "' sTo FROM SalesReturnMaster,SalesReturnDetails WHERE SalesReturnMaster.SalesReturnMasterId = SalesReturnDetails.SalesReturnMasterId";
                                dt1 = DateTime.Parse(sTo);
                                sTo = dt1.ToString("yyyy-MM-dd");
                                sql += " and SalesReturnMaster.SalesReturnDate <= '" + sTo + " 23:59:59'";
                            }
                            else
                                sql += "'' sFrom,'' sTo FROM SalesReturnMaster,SalesReturnDetails WHERE SalesReturnMaster.SalesReturnMasterId = SalesReturnDetails.SalesReturnMasterId";
                            sql += " order by SalesDate";


                            _datasource = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                            _reportname = "SalesReturnDetails.rpt";
                            break;

                        case "Users":
                            Users objUsers = new Users();
                            _datasource = objUsers.GetUsers();
                            _reportname = "Users.rpt";
                            
                            break;
                    }
                    doc.Load(Server.MapPath("") + @"\" + _reportname);
                    if (_datasource != null)
                    {
                        doc.SetDataSource(_datasource);
                        if (_reportname == "Receipt.rpt")
                        {
                            string sql = "SELECT SalesDate, CustomerName,'' as `SalesTime`,CASE PaymentType WHEN 1 THEN 'Cash' WHEN 2 THEN CONCAT(CardType, ' ....',CardCheckNo) ELSE concat('Voucher No. ',CardCheckNo) END as PayType,case when CardAuthorizationId != 0 then CONCAT('Authorization:',CardAuthorizationId) else '' end as Authorization,Amount as PayAmount FROM SalesMaster,PaymentDetails  where SalesMaster.SalesMasterId = PaymentDetails.SalesMasterId and SalesMaster.SalesMasterId= " + SalesId;
                            _datasource = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                            doc.Subreports[0].SetDataSource(_datasource);

                            sql = @"SELECT concat(IFNULL((SELECT Name FROM ProductMaster WHERE ProductMaster.ProductId = SD.ProductId),''), IFNULL(SD.ProductName,'')) as Description, SUM(D.Qty) as Quantity, D.Amount as Price
                                    FROM SalesReturnDetails D, SalesDetails SD
                                    where D.SalesDetailId = SD.SalesDetailId and SD.SalesMasterId = " + SalesId + " GROUP by D.SalesDetailId";

                            _datasource = DBConn.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                            doc.Subreports[1].SetDataSource(_datasource);
                            if (Request.QueryString["Dup"] == "yes")
                                doc.Subreports[0].ReportDefinition.Sections["ReportFooterSection1"].SectionFormat.EnableSuppress = true;
                        }
                    }
                    
                    doc.SummaryInfo.ReportTitle = _reporttitle;
                    doc.RecordSelectionFormula = _reportselectionformula;

                    if (_reportname == "CategorySummary.rpt" || _reportname == "VoucherSummary.rpt")
                    { 
                      ((TextObject) doc.ReportDefinition.Sections["Section1"].ReportObjects["title"]).Text = "" + Request.QueryString["category_name"] + " Voucher Summary by Date";
                    }

                    CrViewer.HyperlinkTarget = "_blank";
                    //repObj.LoadParameterFields(doc, Request.QueryString["DataSource"]);
                    MaximizeWindow();
                    if (_printType == "DirectPrint")// Report.ReportOutPutEnum.DirectPrint.ToString())
                    {
                        CrViewer.SeparatePages = false;
                        //CrViewer.PrintMode = CrystalDecisions.Web.PrintMode.ActiveX;

                    }
                    /*else if (_printType == Report.ReportOutPutEnum.ExportToPDF.ToString())
                    {
                        MemoryStream oStream; // using System.IO
                        oStream = (MemoryStream)
                        doc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                        Response.Clear();
                        Response.Buffer = true;
                        if ("" + Request.QueryString["DataSource"] == "SIBREPORT")
                        {
                            string sFileName = "";
                            if ("" + ConfigurationManager.AppSettings["ExchId"] == "LULU")
                                sFileName = "LNF";
                            else
                                sFileName = "ANF";
                            sFileName = sFileName + DateTime.Parse(repObj.DateParameter1).ToString("ddMMyyyy") + repObj.StringParameter2; //objFgen.GetSIBFileName(tFun.Text);
                            sFileName = sFileName.Replace("/", "");
                            string fileName = Server.MapPath("files") + "\\" + sFileName;
                            Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);
                        }
                        Response.ContentType = "application/pdf";
                        Response.BinaryWrite(oStream.ToArray());
                        Response.End();
                    }
                    else if (_printType == Report.ReportOutPutEnum.ExportToExcel.ToString())
                    {
                        MemoryStream oStream; // using System.IO
                        oStream = (MemoryStream)
                        doc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.BinaryWrite(oStream.ToArray());
                        Response.End();
                    }
                    else if (_printType == Report.ReportOutPutEnum.ExportToDoc.ToString())
                    {
                        MemoryStream oStream; // using System.IO
                        oStream = (MemoryStream)
                        doc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.WordForWindows);
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "application/vnd.ms-word";
                        Response.BinaryWrite(oStream.ToArray());
                        Response.End();
                    }*/
                    CrViewer.ReportSource = doc;
                    CrViewer.DataBind();
                }
            }
            catch (Exception E)
            {
                Response.Write(E.ToString());
            }
        }
        private void CloseWindow()
        {
            Response.Write("<script type='text/javascript'> " +
                           "{alert('No Records Found!');window.close(); }" +
                           "</script>");

        }
        private void MaximizeWindow()
        {
            Response.Write("<script type='text/javascript'> " +
                          "{window.moveTo(0, 0); window.resizeTo(screen.width, screen.height); window.focus(); }" +
                          "</script>");
        }
        private void HideWindow()
        {
            Response.Write("<script type='text/javascript'> " +
                          "{window.moveTo(-1000, -1000); window.resizeTo(0,0); }" +
                          "</script>");
        }
        protected void Page_UnLoad(object sender, EventArgs e)
        {
            if (doc.IsLoaded)
                doc.Dispose();
            doc = null;
            this.Dispose();
            GC.Collect();
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            MemoryStream oStream; // using System.IO
            oStream = (MemoryStream)
            doc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(oStream.ToArray());
            Response.End();

        }

        protected void CrViewer_ReportRefresh(object source, CrystalDecisions.Web.ViewerEventArgs e)
        {
            CrViewer.RefreshReport();
        }

        protected void CrViewer_ViewZoom(object source, CrystalDecisions.Web.ZoomEventArgs e)
        {
            CrViewer.PageZoomFactor = 150;
        }

        protected void CrViewer_Navigate(object source, CrystalDecisions.Web.NavigateEventArgs e)
        {
            if (isNavigated) return;
            if (e.CurrentPageNumber != e.NewPageNumber)
            {
                isNavigated = true;
                CrViewer.ShowNthPage(e.NewPageNumber);
            }
        }
    }
}
