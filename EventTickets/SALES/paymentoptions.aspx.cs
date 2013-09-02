using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MYSQLDatabase;

namespace EventTickets.SALES
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             

                System.Web.UI.HtmlControls.HtmlInputButton btn = (System.Web.UI.HtmlControls.HtmlInputButton)(Master.FindControl("cart").FindControl("bPurcahse"));
                btn.Disabled = true;

                if (Session["Payment"] != null)
                {
                    DataTable dt = (DataTable)Session["Payment"];
                    if (dt.Rows.Count > 0)
                    {
                        bCash.Enabled = false;
                        bCredit.Enabled = false;
                        bVoucher.Enabled = false;
                    }
                    else
                    {
                        bCash.Enabled = true;
                        bCredit.Enabled = true;
                        bVoucher.Enabled = true;
                    }
                }
                else
                {
                    bCash.Enabled = true;
                    bCredit.Enabled = true;
                    bVoucher.Enabled = true;
                }
 
             
        }
    }
}