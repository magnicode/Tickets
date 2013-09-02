using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MYSQLDatabase;
namespace EventTickets
{
    public partial class payment : System.Web.UI.UserControl
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            decimal AmountDue = EventFunctions.Functions.ToDecimal(Session["AmountDue"]);  
            lblAmtDue.Text = "$" + AmountDue.ToString("#0.00");
            FillPayments();
            FindTotal();
        }

        public void FindTotal()
        {
            DataTable dt = (DataTable)dlPayment.DataSource;

            decimal AmountDue = Convert.ToDecimal(lblAmtDue.Text.Replace("$", ""));
            decimal TotalPaid = 0;
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TotalPaid += Convert.ToDecimal(dt.Rows[i]["Amount"].ToString());
                }
            }
            lblAmtPaid.Text = "$" + TotalPaid.ToString("#0.00");
            lblAmtRem.Text = "$" + (AmountDue - TotalPaid).ToString("#0.00");
            Session["AmountRemaining"] = AmountDue - TotalPaid;
        }
        public void FillPayments()
        {
            try
            {
                //if (Session["Payment"] == null) return;

                DataTable dt = (DataTable)Session["Payment"];
                dlPayment.DataSource = dt;
                dlPayment.DataBind();
            }
            catch (Exception)
            {
                
                
            }
        }

        protected void bFinish_Click(object sender, EventArgs e) 
        {
            if (Convert.ToDecimal(lblAmtRem.Text.Replace("$","")) == 0)
            {
                /*
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

                if (amount > 0)
                {
                    string sScript = "<script>window.open('calc.aspx','calc', 'width=500,height=250'); </script>";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "calc", sScript);
                }
                else
                {
                    Response.Redirect("authorize.aspx");
                }
                */

                Response.Redirect("authorize.aspx");
                
            }
        }

        protected void dlPayment_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                HiddenField cId = ((HiddenField)e.Item.FindControl("cId"));
                classes.Payment payment = new classes.Payment();
                payment.DeletePayment(cId.Value);

                FillPayments();
                FindTotal();
                Response.Redirect("paymentoptions.aspx");
            }

            if (e.CommandName == "edit")
            {
                HiddenField cId = ((HiddenField)e.Item.FindControl("cId"));
                classes.Payment payment = new classes.Payment();
                DataTable dtPayment = payment.GetPayment();
                dtPayment.PrimaryKey = new DataColumn[] { dtPayment.Columns["Id"] };
                DataRow dr = dtPayment.Rows.Find(cId.Value);
                string sScript = "";
                if (dr != null)
                {
                    if (dr["PaymentType"].ToString() == "credit")
                    {
                        string from = "";
                        if (Request.ServerVariables["SCRIPT_NAME"].ToLower().IndexOf("authorize.aspx") >= 0)
                            from = "a";
                        else if (Request.ServerVariables["SCRIPT_NAME"].ToLower().IndexOf("tickets.aspx") >= 0)
                            from = "t";
                        //Response.Write("<script>window.open('credit.aspx?id=" + cId.Value + "&from=" + from + "','credit', 'width=500,height=300'); </script>");
                        sScript = "<script>window.open('credit.aspx?id=" + cId.Value + "&from=" + from + "','credit', 'width=500,height=300'); </script>";
                    }
                    else if (dr["PaymentType"].ToString() == "voucher")
                    {
                        string from = "";
                        if (Request.ServerVariables["SCRIPT_NAME"].ToLower().IndexOf("authorize.aspx") >= 0)
                            from = "a";
                        else if (Request.ServerVariables["SCRIPT_NAME"].ToLower().IndexOf("tickets.aspx") >= 0)
                            from = "t";
                        //Response.Write("<script>window.open('voucher.aspx?id=" + cId.Value + "&from=" + from + "','credit', 'width=500,height=300'); </script>");
                        sScript = "<script>window.open('voucher.aspx?id=" + cId.Value + "&from=" + from + "','credit', 'width=500,height=300'); </script>";
                    }
                
                }
                HttpContext.Current.Session["Payment"] = dtPayment;
                strScript.InnerHtml = sScript;
            }
        }

        protected void bCancel_Click(object sender, EventArgs e)
        {
            Session["CartId"] = "";
            Session["Payment"] = null;
            Session["AmountDue"] = "";
            Session["AmountRemaining"] = "";
            Session["SalesMasterId"] = "";

            Response.Redirect("../sales/index.aspx");
        }
    }
}