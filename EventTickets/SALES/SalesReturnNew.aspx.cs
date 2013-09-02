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
    public partial class SalesReturnNew : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            this.Form.DefaultButton = btnDisableEnter.UniqueID;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            Response.Write(User.Identity.IsAuthenticated);
            Response.Write(User.Identity.AuthenticationType);
            Response.Write(User.IsInRole("1"));
            */   
        }
        protected void bReturn_Click(object sender, EventArgs e)
        {
            int salesId = Convert.ToInt16(tSalesId.Text);

        }
    }
}