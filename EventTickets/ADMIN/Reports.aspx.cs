using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EventFunctions;
using EventTickets.classes;

namespace EventTickets.Admin
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MYSQLDatabase.DBConnection dbcon = new MYSQLDatabase.DBConnection();
                DataTable dt = dbcon.ExecuteDataSet("Select CategoryId, Name from Category where status = 1 order by Name", CommandType.Text).Tables[0];
                dCategory.DataSource = dt;
                dCategory.DataTextField = "Name";
                dCategory.DataValueField = "CategoryId";
                dCategory.DataBind();

                dt = dbcon.ExecuteDataSet("Select LocationId, Name from LocationMaster where status = 1 order by Name", CommandType.Text).Tables[0];
                dLocation.DataSource = dt;
                dLocation.DataTextField = "Name";
                dLocation.DataValueField = "LocationId";
                dLocation.DataBind();

                dt = null;
                dbcon = null;

                /*
                divSalesSummary.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_SalesSummaryReport);
                bSalesSummary.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_SalesSummaryReport);
                divSalesDetails.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_SalesDetailsReport);
                bSalesDetails.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_SalesDetailsReport);
                divEventSoldCount.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_EventSoldCountReport);
                bEventSoldCount.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_EventSoldCountReport);
                divCategorySummary.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_CategorySummaryReport);
                bCategorySummary.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_CategorySummaryReport);
                divClerkSummary.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_ClerkSummaryReport);
                bClerkSummary.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_ClerkSummaryReport);
                divVoucherSummary.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_VoucherSummaryReport);
                bVoucherSummary.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_VoucherSummaryReport);
                divSDReturns.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_SDReturnsReports);
                bSDReturns.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_SDReturnsReports);
                divUserLog.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_UserLogReport);
                bUserLog.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_UserLogReport);
                divSalesReturnSummary.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_SalesReturnSummaryReport);
                bSalesReturnSummary.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_SalesReturnSummaryReport);
                divSalesReturnDetails.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_SalesReturnDetailsReport);
                bSalesReturnDetails.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_SalesReturnDetailsReport);

                tr_MacysReports.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_MacysSurveryDataReport);
                bMacysSurveyData.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_MacysSurveryDataReport);

                tr_InventoryManagement.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_StockManagement) || Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_AvailabilityCheck);
                divStockManagement.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_StockManagement);
                bStockManagement.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_StockManagement);
                divAvailabilityCheck.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_AvailabilityCheck);
                bAvailabilityCheck.Visible = Functions.HasRights(Functions.enumPageId.Administration, Functions.enumControlId.Administration_AvailabilityCheck);
                */

                divSalesSummary.Visible = Users.HasRights(Users.enumRoles.BasicSalesReports);
                bSalesSummary.Visible = Users.HasRights(Users.enumRoles.BasicSalesReports);

                divSalesDetails.Visible = Users.HasRights(Users.enumRoles.BasicSalesReports);
                bSalesDetails.Visible = Users.HasRights(Users.enumRoles.BasicSalesReports);

                divEventSoldCount.Visible = Users.HasRights(Users.enumRoles.BasicSalesReports);
                bEventSoldCount.Visible = Users.HasRights(Users.enumRoles.BasicSalesReports);

                divCategorySummary.Visible = Users.HasRights(Users.enumRoles.BasicSalesReports);
                bCategorySummary.Visible = Users.HasRights(Users.enumRoles.BasicSalesReports);

                divClerkSummary.Visible = Users.HasRights(Users.enumRoles.BasicClerkReports) || Users.HasRights(Users.enumRoles.BasicSalesReports);
                bClerkSummary.Visible = Users.HasRights(Users.enumRoles.BasicClerkReports) || Users.HasRights(Users.enumRoles.BasicSalesReports);

                divVoucherSummary.Visible = Users.HasRights(Users.enumRoles.BasicManagementReports);
                bVoucherSummary.Visible = Users.HasRights(Users.enumRoles.BasicManagementReports);

                divSDReturns.Visible = Users.HasRights(Users.enumRoles.BasicManagementReports);
                bSDReturns.Visible = Users.HasRights(Users.enumRoles.BasicManagementReports);

                divUserLog.Visible = Users.HasRights(Users.enumRoles.BasicManagementReports);
                bUserLog.Visible = Users.HasRights(Users.enumRoles.BasicManagementReports);

                divSalesReturnSummary.Visible = Users.HasRights(Users.enumRoles.BasicManagementReports);
                bSalesReturnSummary.Visible = Users.HasRights(Users.enumRoles.BasicManagementReports);

                divSalesReturnDetails.Visible = Users.HasRights(Users.enumRoles.BasicManagementReports);
                bSalesReturnDetails.Visible = Users.HasRights(Users.enumRoles.BasicManagementReports);

                tr_MacysReports.Visible = Users.HasRights(Users.enumRoles.MacysReports);
                bMacysSurveyData.Visible = Users.HasRights(Users.enumRoles.MacysReports);

                tr_InventoryManagement.Visible = Users.HasRights(Users.enumRoles.InventoryManagement);
                divStockManagement.Visible = Users.HasRights(Users.enumRoles.InventoryManagement);
                bStockManagement.Visible = Users.HasRights(Users.enumRoles.InventoryManagement);
                divAvailabilityCheck.Visible = Users.HasRights(Users.enumRoles.InventoryManagement);
                bAvailabilityCheck.Visible = Users.HasRights(Users.enumRoles.InventoryManagement);

            }
        }
    }
}