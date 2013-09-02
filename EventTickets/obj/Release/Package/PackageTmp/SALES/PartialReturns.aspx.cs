using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MYSQLDatabase;
using System.Data;

namespace EventTickets.SALES
{
    public partial class PartialReturns : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DBConnection dbCon = new DBConnection();
                string sql = "Select SalesReturnMasterId as `Return ID`, date_format(SalesReturnDate, '%m/%d/%Y') as `Return Date`, Amount from SalesReturnMaster where status = 0 order by SalesReturnDate";
                DataTable dt = dbCon.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                gv.DataSource = dt;
                gv.DataBind();
            }
        }
    }
}