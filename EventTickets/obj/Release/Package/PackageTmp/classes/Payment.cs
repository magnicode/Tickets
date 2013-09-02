using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Net;
using MYSQLDatabase;
using EventFunctions;

namespace EventTickets.classes
{
    public class Payment
    {
        public Guid Id { get; set; }
        public int CartId {get; set;}
        public string PaymentType { get; set; }
        public decimal Amount { get; set; }
        public string CardType { get; set; }
        public string CardNo { get; set; }
        public int CardExpMonth { get; set; }
        public int CardExpYear { get; set; }
        public string CardName { get; set; }
        //public string? ImagePath { get; set; }
        public int Status { get; set; }
        public long TransId { get; set; }

        public Payment()
        {
            this.CardType = "";
            this.CardNo = "";
            this.CardExpMonth = 0;
            this.CardExpYear = 0;
            this.CardName = "";
            this.TransId = 0;
        }

        public void SetPayment()
        {
            DataTable dtPayment = new DataTable();
            if(HttpContext.Current.Session["Payment"] == null)
            {
                dtPayment.Columns.Add(new DataColumn("Id", typeof(Guid)));
                dtPayment.Columns.Add(new DataColumn("CartId", typeof(int)));
                dtPayment.Columns.Add(new DataColumn("PaymentType",typeof(string)));
                dtPayment.Columns.Add(new DataColumn("Amount",typeof(decimal)));
                dtPayment.Columns.Add(new DataColumn("ImagePath", typeof(string)));
                dtPayment.Columns.Add(new DataColumn("CardType",typeof(string)));
                dtPayment.Columns.Add(new DataColumn("CardNo", typeof(string)));
                dtPayment.Columns.Add(new DataColumn("CardExpMonth", typeof(int)));
                dtPayment.Columns.Add(new DataColumn("CardExpYear", typeof(int)));
                dtPayment.Columns.Add(new DataColumn("CardName", typeof(string)));
                dtPayment.Columns.Add(new DataColumn("Description", typeof(string)));
                dtPayment.Columns.Add(new DataColumn("Status", typeof(int)));
                dtPayment.Columns.Add(new DataColumn("Response", typeof(string[])));

                HttpContext.Current.Session.Add("Payment", dtPayment);
            }
            dtPayment = (DataTable)HttpContext.Current.Session["Payment"];

            DataRow drPayment = dtPayment.NewRow();
            drPayment["Id"] = Guid.NewGuid();
            drPayment["CartId"] = this.CartId;
            drPayment["PaymentType"] = this.PaymentType;
            drPayment["Amount"] = this.Amount;
            drPayment["CardType"] = this.CardType;

            if(this.PaymentType == "cash")
            {
                drPayment["ImagePath"] = "../images/cash.png";
                drPayment["Description"] = "Cash Payment";
            }
            else if(this.PaymentType == "credit")
            {
                if (this.CardType == "American Express")
                    drPayment["ImagePath"] = "../images/amex.png";
                else if (this.CardType == "Visa")
                    drPayment["ImagePath"] = "../images/visa.png";
                else if (this.CardType == "Mastercard")
                    drPayment["ImagePath"] = "../images/mastercard.png";
                else if (this.CardType == "Discover")
                    drPayment["ImagePath"] = "../images/discover.png";
                else
                    drPayment["ImagePath"] = "";

                drPayment["Description"] = "Card Payment ...." + this.CardNo.Substring(this.CardNo.Length - 4);
            }
            else if(this.PaymentType == "voucher")
            {
                drPayment["ImagePath"] = "../images/voucher.png";
                drPayment["Description"] = "Voucher Payment";
            }


            
            drPayment["CardNo"] = this.CardNo;
            drPayment["CardExpMonth"] = this.CardExpMonth;
            drPayment["CardExpYear"] = this.CardExpYear;
            drPayment["CardName"] = this.CardName;
            //drPayment["Description"] = this.PaymentType == "cash"?"Cash Payment":"Card Payment ...." + this.CardNo.Substring(this.CardNo.Length-4);
            drPayment["Status"] = 1;

            dtPayment.Rows.Add(drPayment);

            HttpContext.Current.Session["Payment"] = dtPayment;
        }

        public DataTable GetPayment()
        {
            DataTable dtPayment = new DataTable();
            dtPayment = (DataTable)HttpContext.Current.Session["Payment"];
            return dtPayment;
        }

        public void DeletePayment(string Id)
        {
            DataTable dtPayment = new DataTable();
            dtPayment = (DataTable)HttpContext.Current.Session["Payment"];
            dtPayment.PrimaryKey = new DataColumn[] { dtPayment.Columns["Id"] };
            DataRow dr = dtPayment.Rows.Find(Id);
            dr.Delete();
            HttpContext.Current.Session["Payment"] = dtPayment;
        }

        public bool AuthorizePayment()
        {
            bool IsAuthorized = true;
            DataTable dtPayment = new DataTable();
            dtPayment = (DataTable)HttpContext.Current.Session["Payment"];
            for (int i = 0; i < dtPayment.Rows.Count; i++)
            {
                if (dtPayment.Rows[i]["Status"].ToString() == "1" )
                {
                    if (dtPayment.Rows[i]["PaymentType"].ToString() == "cash" || dtPayment.Rows[i]["PaymentType"].ToString() == "voucher")
                    {
                        dtPayment.Rows[i]["Status"] = "0";
                    }
                    else
                    {
                        string PostUrl = System.Configuration.ConfigurationManager.AppSettings["authorize_net_post_url"];
                        string Login = System.Configuration.ConfigurationManager.AppSettings["authorize_net_x_login"];
                        string TranKey = System.Configuration.ConfigurationManager.AppSettings["authorize_net_x_tran_key"];
                        string CardNum = dtPayment.Rows[i]["CardNo"].ToString();
                        string ExpDate = (EventFunctions.Functions.ToInt(dtPayment.Rows[i]["CardExpMonth"].ToString())).ToString("0#") + dtPayment.Rows[i]["CardExpYear"].ToString().Substring(2);
                        decimal Amount = EventFunctions.Functions.ToDecimal(dtPayment.Rows[i]["Amount"].ToString());
                        string Name = dtPayment.Rows[i]["CardName"].ToString();
                        string FirstName = "";
                        string LastName = "";
                        if (Name.IndexOf(" ") > 0)
                        {
                            FirstName = Name.Substring(0, Name.IndexOf(" "));
                            LastName = Name.Substring(Name.IndexOf(" ") + 1);
                        }
                        else
                        {
                            FirstName = Name;
                        }

                        Dictionary<string, string> PostValues = new Dictionary<string, string>();
                        PostValues.Add("x_login", Login);
                        PostValues.Add("x_tran_key", TranKey);
                        PostValues.Add("x_delim_data", "TRUE");
                        PostValues.Add("x_delim_char", "|");
                        PostValues.Add("x_relay_response", "FALSE");

                        PostValues.Add("x_type", "AUTH_CAPTURE");
                        PostValues.Add("x_method", "CC");
                        PostValues.Add("x_card_num", CardNum);
                        PostValues.Add("x_exp_date", ExpDate);

                        PostValues.Add("x_amount", Amount.ToString());
                        PostValues.Add("x_description", "Event Tickets");

                        PostValues.Add("x_first_name", FirstName);
                        PostValues.Add("x_last_name", LastName);
                        PostValues.Add("x_address", "");
                        PostValues.Add("x_state", "");
                        PostValues.Add("x_zip", "");

                        String PostString = "";
                        foreach (KeyValuePair<string, string> PostValue in PostValues)
                        {
                            PostString += PostValue.Key + "=" + HttpUtility.UrlEncode(PostValue.Value) + "&";
                        }
                        PostString = PostString.TrimEnd('&');


                        HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(PostUrl);
                        objRequest.Method = "POST";
                        objRequest.ContentLength = PostString.Length;
                        objRequest.ContentType = "application/x-www-form-urlencoded";

                        System.IO.StreamWriter myWriter = null;
                        myWriter = new System.IO.StreamWriter(objRequest.GetRequestStream());
                        myWriter.Write(PostString);
                        myWriter.Close();

                        HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();

                        String PostResponse;
                        using (System.IO.StreamReader ResponseStream = new System.IO.StreamReader(objResponse.GetResponseStream()))
                        {
                            PostResponse = ResponseStream.ReadToEnd();
                            ResponseStream.Close();
                        }

                        string[] ResponseArray = PostResponse.Split('|');
                        if (ResponseArray[0] == "1")
                        {
                            dtPayment.Rows[i]["Status"] = "0";
                        }
                        else
                        {
                            IsAuthorized = false;
                        }
                        dtPayment.Rows[i]["Response"] = ResponseArray;

                        DBConnection dbCon = new DBConnection();
                        try
                        {
                            string sql = @"insert into PaymentResponse 
                                        (
                                            GuId, CartId, PaymentId, paid_date, x_response_code, x_response_subcode, x_response_reason_code, x_response_reason_text, x_auth_code, x_avs_code, x_trans_id, x_invoice_num, 
                                            x_description, x_amount, x_method, x_type, x_account_number, x_card_type, x_split_tender_id, x_prepaid_requested_amount, x_prepaid_balance_on_card, 
                                            x_cust_id, x_first_name, x_last_name, x_company, x_address, x_city, x_state, x_zip, x_country, x_phone, x_fax, x_email, x_tax, x_duty, 
                                            x_freight, x_tax_exempt, x_po_num, x_MD5_Hash, x_cvv2_resp_code, x_cavv_response, user_deleted
                                        ) 
                                        values 
                                        (
                                            '" + dtPayment.Rows[i]["Id"].ToString() + "', " + HttpContext.Current.Session["CartId"].ToString() + ", 0, now(), " + ResponseArray[0] + ", " + ResponseArray[1] + ", " + ResponseArray[2] + ", '" + ResponseArray[3] + "', '" + ResponseArray[4] + "', '" + ResponseArray[5] + "', " + ResponseArray[6] + ", '" + ResponseArray[7] + "', '" +
                                                ResponseArray[8] + "', " + ResponseArray[9] + ", '" + ResponseArray[10] + "', '" + ResponseArray[11] + "', '" + ResponseArray[50] + "', '" + ResponseArray[51] + "', '" + ResponseArray[52] + "', " +  Functions.ToDecimal(ResponseArray[53]) + ", " + Functions.ToDecimal(ResponseArray[54]) + ", '" +
                                                ResponseArray[12] + "', '" + ResponseArray[13] + "', '" + ResponseArray[14] + "', '" + ResponseArray[15] + "', '" + ResponseArray[16] + "', '" + ResponseArray[17] + "', '" + ResponseArray[18] + "', '" + ResponseArray[19] + "', '" + ResponseArray[20] + "', '" + ResponseArray[21] + "', '" + ResponseArray[22] + "', '" + ResponseArray[23] + "', " + Functions.ToDecimal(ResponseArray[32]) + ", " + Functions.ToDecimal(ResponseArray[33]) + ", " +
                                                Functions.ToDecimal(ResponseArray[34]) + ", '" + ResponseArray[35] + "', '" + ResponseArray[36] + "', '" + ResponseArray[37] + "', '" + ResponseArray[38] + "', '" + ResponseArray[39] + "', 0" +
                                            ")";

                            dbCon.Execute(sql, CommandType.Text);
                            dbCon = null;

                            this.TransId = Convert.ToInt64(ResponseArray[6]);
                        }
                        catch (Exception e)
                        {
                            dbCon = null;
                        }
                    }
                     
                }
            
            }

            HttpContext.Current.Session["Payment"] = dtPayment;
            return IsAuthorized;
        }


        public bool RefundPayment(string TransactionId, string CardNum, decimal Amount, int PaymentId)
        {
            bool IsAuthorized = false;

            try
            {
                string PostUrl = System.Configuration.ConfigurationManager.AppSettings["authorize_net_post_url"];
                string Login = System.Configuration.ConfigurationManager.AppSettings["authorize_net_x_login"];
                string TranKey = System.Configuration.ConfigurationManager.AppSettings["authorize_net_x_tran_key"];

                Dictionary<string, string> PostValues = new Dictionary<string, string>();
                PostValues.Add("x_login", Login);
                PostValues.Add("x_tran_key", TranKey);
                PostValues.Add("x_delim_data", "TRUE");
                PostValues.Add("x_delim_char", "|");
                PostValues.Add("x_relay_response", "FALSE");
                PostValues.Add("x_trans_id", TransactionId);
                PostValues.Add("x_type", "CREDIT");
                PostValues.Add("x_method", "CC");
                PostValues.Add("x_card_num", CardNum);
                PostValues.Add("x_amount", Amount.ToString());
                PostValues.Add("x_description", "Event Tickets Refund");

                String PostString = "";
                foreach (KeyValuePair<string, string> PostValue in PostValues)
                {
                    PostString += PostValue.Key + "=" + HttpUtility.UrlEncode(PostValue.Value) + "&";
                }
                PostString = PostString.TrimEnd('&');


                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(PostUrl);
                objRequest.Method = "POST";
                objRequest.ContentLength = PostString.Length;
                objRequest.ContentType = "application/x-www-form-urlencoded";

                System.IO.StreamWriter myWriter = null;
                myWriter = new System.IO.StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(PostString);
                myWriter.Close();

                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();

                String PostResponse;
                using (System.IO.StreamReader ResponseStream = new System.IO.StreamReader(objResponse.GetResponseStream()))
                {
                    PostResponse = ResponseStream.ReadToEnd();
                    ResponseStream.Close();
                }

                string[] ResponseArray = PostResponse.Split('|');
                

                DBConnection dbCon = new DBConnection();
                try
                {
                    string sql = @"insert into PaymentResponse 
                                        (
                                            GuId, CartId, PaymentId, paid_date, x_response_code, x_response_subcode, x_response_reason_code, x_response_reason_text, x_auth_code, x_avs_code, x_trans_id, x_invoice_num, 
                                            x_description, x_amount, x_method, x_type, x_account_number, x_card_type, x_split_tender_id, x_prepaid_requested_amount, x_prepaid_balance_on_card, 
                                            x_cust_id, x_first_name, x_last_name, x_company, x_address, x_city, x_state, x_zip, x_country, x_phone, x_fax, x_email, x_tax, x_duty, 
                                            x_freight, x_tax_exempt, x_po_num, x_MD5_Hash, x_cvv2_resp_code, x_cavv_response, user_deleted
                                        ) 
                                        values 
                                        (
                                            '" + Guid.NewGuid() + "', 0, " + PaymentId + ", now(), " + ResponseArray[0] + ", " + ResponseArray[1] + ", " + ResponseArray[2] + ", '" + ResponseArray[3] + "', '" + ResponseArray[4] + "', '" + ResponseArray[5] + "', " + ResponseArray[6] + ", '" + ResponseArray[7] + "', '" +
                                        ResponseArray[8] + "', " + ResponseArray[9] + ", '" + ResponseArray[10] + "', '" + ResponseArray[11] + "', '" + ResponseArray[50] + "', '" + ResponseArray[51] + "', '" + ResponseArray[52] + "', " + Functions.ToDecimal(ResponseArray[53]) + ", " + Functions.ToDecimal(ResponseArray[54]) + ", '" +
                                        ResponseArray[12] + "', '" + ResponseArray[13] + "', '" + ResponseArray[14] + "', '" + ResponseArray[15] + "', '" + ResponseArray[16] + "', '" + ResponseArray[17] + "', '" + ResponseArray[18] + "', '" + ResponseArray[19] + "', '" + ResponseArray[20] + "', '" + ResponseArray[21] + "', '" + ResponseArray[22] + "', '" + ResponseArray[23] + "', " + Functions.ToDecimal(ResponseArray[32]) + ", " + Functions.ToDecimal(ResponseArray[33]) + ", " +
                                        Functions.ToDecimal(ResponseArray[34]) + ", '" + ResponseArray[35] + "', '" + ResponseArray[36] + "', '" + ResponseArray[37] + "', '" + ResponseArray[38] + "', '" + ResponseArray[39] + "', 0" +
                                    ")";

                    dbCon.Execute(sql, CommandType.Text);
                    dbCon = null;

                    this.TransId = Convert.ToInt64(ResponseArray[6]);
                    if (ResponseArray[0] == "1")
                    {
                        IsAuthorized = true;
                    }
                    else
                    {
                        IsAuthorized = false;
                    }
                }
                catch (Exception e)
                {
                    dbCon = null;
                }
            }
            catch (Exception ex)
            {
                 
            }
            return IsAuthorized;
        }

        public bool VoidPayment(string TransactionId, string CardNum, decimal Amount, int PaymentId)
        {
            bool IsAuthorized = false;

            try
            {
                string PostUrl = System.Configuration.ConfigurationManager.AppSettings["authorize_net_post_url"];
                string Login = System.Configuration.ConfigurationManager.AppSettings["authorize_net_x_login"];
                string TranKey = System.Configuration.ConfigurationManager.AppSettings["authorize_net_x_tran_key"];

                Dictionary<string, string> PostValues = new Dictionary<string, string>();
                PostValues.Add("x_login", Login);
                PostValues.Add("x_tran_key", TranKey);
                PostValues.Add("x_delim_data", "TRUE");
                PostValues.Add("x_delim_char", "|");
                PostValues.Add("x_relay_response", "FALSE");
                PostValues.Add("x_trans_id", TransactionId);
                PostValues.Add("x_type", "VOID");
                PostValues.Add("x_method", "CC");
                PostValues.Add("x_card_num", CardNum);
                PostValues.Add("x_amount", Amount.ToString());
                PostValues.Add("x_description", "Event Tickets Refund");

                String PostString = "";
                foreach (KeyValuePair<string, string> PostValue in PostValues)
                {
                    PostString += PostValue.Key + "=" + HttpUtility.UrlEncode(PostValue.Value) + "&";
                }
                PostString = PostString.TrimEnd('&');


                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(PostUrl);
                objRequest.Method = "POST";
                objRequest.ContentLength = PostString.Length;
                objRequest.ContentType = "application/x-www-form-urlencoded";

                System.IO.StreamWriter myWriter = null;
                myWriter = new System.IO.StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(PostString);
                myWriter.Close();

                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();

                String PostResponse;
                using (System.IO.StreamReader ResponseStream = new System.IO.StreamReader(objResponse.GetResponseStream()))
                {
                    PostResponse = ResponseStream.ReadToEnd();
                    ResponseStream.Close();
                }

                string[] ResponseArray = PostResponse.Split('|');


                DBConnection dbCon = new DBConnection();
                try
                {
                    string sql = @"insert into PaymentResponse 
                                        (
                                            GuId, CartId, PaymentId, paid_date, x_response_code, x_response_subcode, x_response_reason_code, x_response_reason_text, x_auth_code, x_avs_code, x_trans_id, x_invoice_num, 
                                            x_description, x_amount, x_method, x_type, x_account_number, x_card_type, x_split_tender_id, x_prepaid_requested_amount, x_prepaid_balance_on_card, 
                                            x_cust_id, x_first_name, x_last_name, x_company, x_address, x_city, x_state, x_zip, x_country, x_phone, x_fax, x_email, x_tax, x_duty, 
                                            x_freight, x_tax_exempt, x_po_num, x_MD5_Hash, x_cvv2_resp_code, x_cavv_response, user_deleted
                                        ) 
                                        values 
                                        (
                                            '" + Guid.NewGuid() + "', 0, " + PaymentId + ", now(), " + ResponseArray[0] + ", " + ResponseArray[1] + ", " + ResponseArray[2] + ", '" + ResponseArray[3] + "', '" + ResponseArray[4] + "', '" + ResponseArray[5] + "', " + ResponseArray[6] + ", '" + ResponseArray[7] + "', '" +
                                        ResponseArray[8] + "', " + ResponseArray[9] + ", '" + ResponseArray[10] + "', '" + ResponseArray[11] + "', '" + ResponseArray[50] + "', '" + ResponseArray[51] + "', '" + ResponseArray[52] + "', " + Functions.ToDecimal(ResponseArray[53]) + ", " + Functions.ToDecimal(ResponseArray[54]) + ", '" +
                                        ResponseArray[12] + "', '" + ResponseArray[13] + "', '" + ResponseArray[14] + "', '" + ResponseArray[15] + "', '" + ResponseArray[16] + "', '" + ResponseArray[17] + "', '" + ResponseArray[18] + "', '" + ResponseArray[19] + "', '" + ResponseArray[20] + "', '" + ResponseArray[21] + "', '" + ResponseArray[22] + "', '" + ResponseArray[23] + "', " + Functions.ToDecimal(ResponseArray[32]) + ", " + Functions.ToDecimal(ResponseArray[33]) + ", " +
                                        Functions.ToDecimal(ResponseArray[34]) + ", '" + ResponseArray[35] + "', '" + ResponseArray[36] + "', '" + ResponseArray[37] + "', '" + ResponseArray[38] + "', '" + ResponseArray[39] + "', 0" +
                                    ")";

                    dbCon.Execute(sql, CommandType.Text);
                    dbCon = null;

                    this.TransId = Convert.ToInt64(ResponseArray[6]);
                    if (ResponseArray[0] == "1")
                    {
                        IsAuthorized = true;
                    }
                    else
                    {
                        IsAuthorized = false;
                    }
                }
                catch (Exception e)
                {
                    dbCon = null;
                }
            }
            catch (Exception ex)
            {

            }
            return IsAuthorized;
        }
    }
}