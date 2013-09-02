using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MYSQLDatabase;
using System.Text.RegularExpressions;
using EventFunctions;

namespace EventTickets.SALES
{
    public partial class credit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                ExpMonth.Items.Add(new ListItem("", ""));
                ExpYear.Items.Add(new ListItem("", ""));

                for (int i = 1; i <= 12; i++)
                {
                    ExpMonth.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }

                for (int i = DateTime.Now.Year; i <= DateTime.Now.Year + 20; i++)
                {
                    ExpYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }

                if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    classes.Payment objPayment = new classes.Payment();
                    DataTable dtPayment = objPayment.GetPayment();
                    dtPayment.PrimaryKey = new DataColumn[] { dtPayment.Columns["Id"] };
                    DataRow dr = dtPayment.Rows.Find(Request.QueryString["Id"]);
                    if (dr != null)
                    {
                        tCardNo.Text = dr["CardNo"].ToString();
                        ExpMonth.SelectedIndex = ExpMonth.Items.IndexOf(ExpMonth.Items.FindByValue(dr["CardExpMonth"].ToString()));
                        ExpYear.SelectedIndex = ExpYear.Items.IndexOf(ExpYear.Items.FindByValue(dr["CardExpYear"].ToString()));
                        tCardName.Text = dr["CardName"].ToString();

                        //dr.Delete();
                    }
                    HttpContext.Current.Session["Payment"] = dtPayment;
                }

            }
            else
            {
                ErrMsg.Text = "";
            }
        }

        protected void OK_Click(object sender, EventArgs e)
        {
            DBConnection DBConn = new DBConnection();

            string CardType = CheckCardType(this.tCardNo.Text.Trim());
            string CardNo = tCardNo.Text.Trim();
            int CardExpMonth = 0;
            if (ExpMonth.SelectedIndex > 0) CardExpMonth = Convert.ToInt16(ExpMonth.SelectedValue);
            int CardExpYear = 0;
            if(ExpYear.SelectedIndex>0) CardExpYear = Convert.ToInt16(ExpYear.SelectedValue);
            string CardName = tCardName.Text.Trim().Replace("'", "''");
            decimal amount = Convert.ToDecimal(Request.QueryString["amount"]);  // Convert.ToDecimal(Session["AmountDue"].ToString()); // single payment

            if (CardType == "Unknown" || CardNo == "") 
            {
                ErrMsg.Text = "Invalid card number";
                return;
            }
            if (CardExpMonth == 0)
            {
                ErrMsg.Text = "Please select card expiry month";
                return;
            }
            if (CardExpYear == 0)
            {
                ErrMsg.Text = "Please select card expiry year";
                return;
            }
            if (CardExpYear == DateTime.Now.Year && CardExpMonth < DateTime.Now.Month)
            {
                ErrMsg.Text = "Card expired.  Please check expiry month and year";
                return;
            }
            if (CardName == "")
            {
                ErrMsg.Text = "Please enter card name";
                return;
            }
            

            /*
            string sql = @"insert into ShoppingCartPayment (CartId, PaymentType, Amount, SellDate, CardType, CardNo, CardExpMonth, CardExpYear, CardName) 
                        values (" + Session["CartId"] + ", 'card', " + amount.ToString() + ", now(), '" + CardType + "', '" + CardNo + "', " + CardExpMonth.ToString() + ", " + CardExpYear.ToString() + ", '" + CardName + "')";
            DBConn.Execute(sql, CommandType.Text);
            DBConn = null;
            */

            classes.Payment payment = new classes.Payment();

            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                DataTable dtPayment = payment.GetPayment();
                dtPayment.PrimaryKey = new DataColumn[] { dtPayment.Columns["Id"] };
                DataRow dr = dtPayment.Rows.Find(Request.QueryString["Id"]);
                dr.Delete();
            }

            payment.CartId = Functions.ToInt(Session["CartId"].ToString());
            payment.PaymentType = "credit";
            payment.Amount = amount;
            payment.CardType = CardType;
            payment.CardNo = CardNo;
            payment.CardExpMonth = CardExpMonth;
            payment.CardExpYear = CardExpYear;
            payment.CardName = CardName;
            payment.SetPayment();

            if (!String.IsNullOrEmpty(Request.QueryString["from"]))
            {
                if (Request.QueryString["from"] == "a")
                    Response.Write("<script>window.opener.location.href='Authorize.aspx'; window.close();</script>");
                else if (Request.QueryString["from"] == "t")
                    Response.Write("<script>window.opener.location.href='Tickets.aspx'; window.close();</script>");
            }
            else
            {
                Response.Write("<script>window.opener.location.href='paymentoptionsNew.aspx'; window.close();</script>");
            }
            return;
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Write("<script>window.close();</script>"); return;
        }

        private void ParseRaw(string scan)
        {
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
                    this.tCardNo.Text = strAccountNum;
                    this.ExpMonth.SelectedIndex = this.ExpMonth.Items.IndexOf(this.ExpMonth.Items.FindByValue(ExpMonth.ToString()));
                    this.ExpYear.SelectedIndex = this.ExpYear.Items.IndexOf(this.ExpYear.Items.FindByValue("20"+ExpYear.ToString()));
                    this.tCardName.Text = Name;
                    //this.RawData = scan;
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }//parse                     
        }//Parse Card


        private string CheckCardType(string number)
        {
            //string number = Crypto.DecryptStringAES(_number, _salt); //Create just to test. 

            string cardtype = "";

            if (Regex.IsMatch(number, @"^4") && number.Length == 16)
            {
                cardtype = "Visa";
                return cardtype;
            }

            //American Express
            if (Regex.IsMatch(number, @"^3[47]") && number.Length == 15)
            {
                cardtype = "American Express";
                return cardtype;
            }

            //MasterCard
            if (Regex.IsMatch(number, @"^5[1-5]") && number.Length == 16)
            {
                cardtype = "Mastercard";
                return cardtype;
            }

            //Discover
            if (Regex.IsMatch(number, @"^6011") && number.Length == 16)
            {
                cardtype = "Discover";
                return cardtype;
            }

            //Diners Club
            if (Regex.IsMatch(number, @"^2[014|149]") && number.Length == 15)
            {
                cardtype = "DinersClub";
                return cardtype;
            }

            if (Regex.IsMatch(number, @"^3[068][0-5].") && number.Length == 14)
            {
                cardtype = "DinersClub";
                return cardtype;
            }

            if (Regex.IsMatch(number, @"^3") && number.Length == 16)
            {
                cardtype = "JCB";
                return cardtype;
            }

            if (Regex.IsMatch(number, @"^[2131|1800]") && number.Length == 15)
            {
                cardtype = "JCB";
                return cardtype;
            }

            cardtype = "Unknown";
            return cardtype;
        }

        protected void tCardNo_TextChanged(object sender, EventArgs e)
        {
            ParseRaw(tCardNo.Text.Trim());
            //if (tCardNo.Text.Trim() != "" && ExpMonth.SelectedIndex > 0 && ExpYear.SelectedIndex > 0 && tCardName.Text.Trim() != "")
                //OK_Click(null, null);
        }
    }
}