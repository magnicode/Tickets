using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace EventTickets.SALES
{
    public partial class calc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            classes.Payment payment = new classes.Payment();
            DataTable dtPayment = payment.GetPayment();
            DataRow[] dr = dtPayment.Select("paymenttype='cash'");
            decimal amount = 0;
            if (dr.Length > 0)
            {
                for (int i = 0; i < dr.Length; i++)
                {
                    amount += Convert.ToDecimal(dr[i]["amount"].ToString());
                }
            }
            td_cash_receivable.InnerText = amount.ToString("#0.00");

        }
    }
}