using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Net;
using MYSQLDatabase;

namespace EventFunctions
{
    public class Functions
    {
        public Functions()
        {

        }
        public static string ConvertDate(string sDate)
        {
            //string[] langArray
            string sOut = "";
            try
            {
                string[] str = sDate.Split('/');
                sOut = str[1].ToString() + '/' + str[0].ToString() + '/' + str[2].ToString();

                return sOut;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static bool CheckDate(string sDate)
        {
            //string[] langArray
            try
            {
                bool bOut = false;
                string[] str = sDate.Split('/');
                if (int.Parse(str[0]) == 0 | int.Parse(str[2]) == 0)
                {
                    bOut = false;
                    return bOut; // TODO: might not be correct. Was : Exit Function
                }
                if (int.Parse(str[0]) > 31 | int.Parse(str[1]) > 12 | int.Parse(str[2]) < 1900)
                {
                    //Or CInt(str(2)) > Date.Now.Year
                    bOut = false;
                    return bOut; // TODO: might not be correct. Was : Exit Function
                }
                bOut = true;
                return bOut;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public static bool IsNumeric(object num)
        {
            try
            {
                Convert.ToDouble(num);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static int ToInt(object num)
        {
            try
            {
                return Convert.ToInt32(num);
            }
            catch
            {
                return 0;
            }
        }

        public static Double ToDouble(object num)
        {
            try
            {
                return Convert.ToDouble(num);
            }
            catch
            {
                return 0;
            }
        }

        public static Decimal ToDecimal(object num)
        {
            try
            {
                return decimal.Parse(num.ToString());
            }
            catch (Exception)
            {

                return 0;
            }
        }

        public static int Multiple(int intNum, int intNoTimes)
        {
            int intMultipleVal = 1;
            for (int i = 1; i <= intNoTimes; i++)
            {
                intMultipleVal *= intNum;
            }

            return intMultipleVal;

        }

        public static bool IsCheck(Object objDate)
        {
            string strDate = objDate.ToString();
            try
            {
                DateTime dt = DateTime.Parse(strDate);
                if (dt != DateTime.MinValue && dt != DateTime.MaxValue)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsDate(Object objDate)
        {
            string strDate = objDate.ToString();
            try
            {
                //DateTime dt = DateTime.Parse(strDate, new System.Globalization.CultureInfo("[en-NZ]")); // Convert.ToDateTime(objDate);//DateTime.ParseExact(strDate,"dd/MM/yyyy",null);
                DateTime dt = DateTime.ParseExact(strDate, "dd/MM/yyyy", null);
                if (dt != DateTime.MinValue && dt != DateTime.MaxValue)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static string ToDateDDMMYY(string dat)
        {
            DateTime dt = Convert.ToDateTime(dat);
            return dt.ToString("dd/mm/yy");
        }

        public static string ToDateDDMMMYYYY(string dat)
        {
            if (!IsDate(dat)) return dat;
            string[] strDt;
            strDt = dat.Split('/');
            if (strDt.Length < 3)
                strDt = dat.Split('-');
            if (ToInt(strDt[1]) > 12)
            {
                string strTmp = strDt[1];
                strDt[1] = strDt[0];
                strDt[0] = strTmp;
            }
            dat = strDt[0] + "/" + strDt[1] + "/" + strDt[2];

            try
            {
                DateTime dt = Convert.ToDateTime(dat);
                return dt.ToString("dd/MMM/yyyy");
            }
            catch (Exception)
            {
                if (ToInt(strDt[0]) > 12)
                {
                    string strTmp = strDt[0];
                    strDt[0] = strDt[1];
                    strDt[1] = strTmp;
                }
                dat = strDt[0] + "/" + strDt[1] + "/" + strDt[2];
                DateTime dt = Convert.ToDateTime(dat);
                return dt.ToString("dd/MMM/yyyy");
            }
        }

        public static string FormatDate(string sDateTime, string sFormat)
        {
            try
            {
                return Convert.ToDateTime(sDateTime).ToString(sFormat);
            }
            catch (Exception)
            {
                return sDateTime;
            }
        }

        public static char ToChar(object objChar)
        {
            try
            {
                return Convert.ToChar(objChar);
            }
            catch
            {
                return Convert.ToChar(' ');
            }
        }

        int Next(int Num)
        {
            Num = Num / 10;
            int nextr;
            nextr = Num % 10;
            return nextr;

        }

        public static string sha256(string str)
        {
            System.Security.Cryptography.SHA256 sha = new System.Security.Cryptography.SHA256Managed();
            return ByteArrayToStr(sha.ComputeHash(StrToByteArray(str)));
        }

        static byte[] StrToByteArray(string str)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(str);
        }

        static string ByteArrayToStr(byte[] dBytes)
        {
            //System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            //return enc.GetString(dBytes);
            // the above returns the text, below returns a string containing the hex values
            string str = "";
            for (int i = 0; i < dBytes.Length; i++)
                str += dBytes[i].ToString("x2").ToLower();
            return str;
        }        

        public static bool SendMail(string ToAddress, string Subject, string Content)
        {
            try
            {
                //System.Web.Mail.MailMessage Mail = new System.Web.Mail.MailMessage();
                //Mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserver"] = "email-smtp.us-east-1.amazonaws.com";// "http://iwant6percentmore.com";//"localhost";// ConfigurationSettings.AppSettings["SMTPHost"];
                //Mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"] = 2;

                //Mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverport"] = "465";// ConfigurationSettings.AppSettings["SMTPPort"];
                ////if (fSSL)
                ////    Mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpusessl"] = "true";

                //Mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"] = 1;
                //Mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendusername"] = "AKIAJ53FZXGET7GIPEJA";// ConfigurationSettings.AppSettings["SMTPUsername"]; 
                //Mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendpassword"] = "AmB3gLbyNsVOgqSKIdU7RnSz65MLWCix1rTHEUtRAoBp";// ConfigurationSettings.AppSettings["SMTPPassword"]; 

                //Mail.To = "heavenlal@gmail.com";// ToAddress;
                //Mail.From = "Sweepstakes@Iwant6percentmore.com";// ConfigurationSettings.AppSettings["SMTPUsername"];
                //Mail.Subject = Subject;
                //Mail.Body = Content;
                //Mail.BodyFormat = System.Web.Mail.MailFormat.Html;

                //System.Web.Mail.SmtpMail.SmtpServer = "email-smtp.us-east-1.amazonaws.com"; //ConfigurationSettings.AppSettings["SMTPHost"];
                //System.Web.Mail.SmtpMail.Send(Mail);

                /*MailMessage msgMail = new MailMessage();
                msgMail.To = ToAddress;
                msgMail.From = "admin@amexpromotion.net";
                msgMail.Subject = Subject;
                msgMail.BodyFormat = MailFormat.Html;
                msgMail.Body = Content;
                SmtpMail.Send(msgMail);
                msgMail = null;*/

                /*System.Net.Mail.MailAddress SendFrom = new System.Net.Mail.MailAddress("Sweepstakes@Iwant6percentmore.com");
                System.Net.Mail.MailAddress SendTo = new System.Net.Mail.MailAddress("heavenlal@gmail.com");//ToAddress
                System.Net.Mail.MailMessage MyMessage = new System.Net.Mail.MailMessage(SendFrom, SendTo);
                MyMessage.Subject = Subject;
                // MyMessage.Body = getData();

                MyMessage.Body = Content;
                MyMessage.Priority = System.Net.Mail.MailPriority.Normal;
                System.Net.Mail.SmtpClient emailClient = new System.Net.Mail.SmtpClient("email-smtp.us-east-1.amazonaws.com", 465);
                emailClient.Send(MyMessage);*/

                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
                //smtpClient.Host = "74.208.126.111";
                smtpClient.Host = "76.12.251.228";
                //smtpClient.Credentials = new System.Net.NetworkCredential("Sweepstakes@Iwant6percentmore.com", "AmB3gLbyNsVOgqSKIdU7RnSz65MLWCix1rTHEUtRAoBp");//Contest@iwant6percentmore.com
                smtpClient.Credentials = new System.Net.NetworkCredential("website@iwant6percentmore.com", "gytre993");//Contest@iwant6percentmore.com           
                //smtpClient.Port = 465;
                smtpClient.Port = 25;
                //System.Net.Mail.MailAddress fromAddress = new System.Net.Mail.MailAddress("Sweepstakes@Iwant6percentmore.com");
                System.Net.Mail.MailAddress fromAddress = new System.Net.Mail.MailAddress("website@iwant6percentmore.com");            
                System.Net.Mail.MailAddress SendTo = new System.Net.Mail.MailAddress(ToAddress);
                System.Net.Mail.MailMessage mmessage = new System.Net.Mail.MailMessage(fromAddress, SendTo);
                mmessage.Body = Content;
                mmessage.Subject = Subject;
                mmessage.IsBodyHtml = true;
                smtpClient.Send(mmessage);
                
                return true;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.ToString());
                return false;
            }
        }

        public static bool IsPostDate()
        {
            try
            {
                string spostdate = System.Configuration.ConfigurationManager.AppSettings["postdate"].ToString();
                if (spostdate != "")
                {
                    DateTime sPdate = Convert.ToDateTime(DateTime.ParseExact(spostdate, "MM/dd/yyyy HH:mm:ss.fff", null));
                    if (sPdate <= DateTime.Now || ("" + HttpContext.Current.Request.QueryString["mode"]).ToString() == "vp")
                    {
                        string url = (HttpContext.Current.Request.ServerVariables["URL"]).ToLower();
                        if (url.IndexOf("gallery.aspx") <= 0 && url.IndexOf("vote.aspx") <= 0 && url.IndexOf("login.aspx") <= 0 && url.IndexOf("ftshare.aspx") <= 0)
                        {
                            HttpContext.Current.Response.Redirect("gallery.aspx");
                            HttpContext.Current.Response.End();
                            return true;
                        }
                        return true;
                    }
                }
                return false;
            }
            catch (Exception E1)
            {
                return false;
            }
        }

        public static bool IsEndDate()
        {
            try
            {
                /*string svotefrom = System.Configuration.ConfigurationManager.AppSettings["votefrom"].ToString();
                string svoteto = System.Configuration.ConfigurationManager.AppSettings["voteto"].ToString();
                if (svotefrom != "" && svoteto != "")
                {
                    DateTime dvotefrom = Convert.ToDateTime(DateTime.ParseExact(svotefrom, "MM/dd/yyyy HH:mm:ss.fff", null));
                    DateTime dvoteto = Convert.ToDateTime(DateTime.ParseExact(svoteto, "MM/dd/yyyy HH:mm:ss.fff", null));
                    if (dvotefrom <= DateTime.Now && dvoteto >= DateTime.Now)
                    {
                        string url = (HttpContext.Current.Request.ServerVariables["URL"]).ToLower();
                        if (url.IndexOf("gallery.aspx") <= 0 && url.IndexOf("login.aspx") <= 0)
                        {
                            HttpContext.Current.Response.Redirect("gallery.aspx");
                            HttpContext.Current.Response.End();
                            return true;
                        }
                        return true;
                    }                   
                }*/
                string senddate = System.Configuration.ConfigurationManager.AppSettings["enddate"].ToString();
                if (senddate != "")
                {
                    DateTime sEdate = Convert.ToDateTime(DateTime.ParseExact(senddate, "MM/dd/yyyy HH:mm:ss.fff", null));
                    if (sEdate <= DateTime.Now || ("" + HttpContext.Current.Request.QueryString["mode"]).ToString() == "over")
                    {
                        string url = (HttpContext.Current.Request.ServerVariables["URL"]).ToLower();
                        if (url.IndexOf("over.aspx") <= 0 && url.IndexOf("login.aspx") <= 0)
                        {
                            HttpContext.Current.Response.Redirect("over.aspx");
                            HttpContext.Current.Response.End();
                            return true;
                        }
                        return true;
                    }
                }
                return false;
            }
            catch (Exception E1)
            {
                return false;
            }
        }

        public static bool IsEndDateVoting()
        {
            try
            {
                /*string svotefrom = System.Configuration.ConfigurationManager.AppSettings["votefrom"].ToString();
                string svoteto = System.Configuration.ConfigurationManager.AppSettings["voteto"].ToString();
                if (svotefrom != "" && svoteto != "")
                {
                    DateTime dvotefrom = Convert.ToDateTime(DateTime.ParseExact(svotefrom, "MM/dd/yyyy HH:mm:ss.fff", null));
                    DateTime dvoteto = Convert.ToDateTime(DateTime.ParseExact(svoteto, "MM/dd/yyyy HH:mm:ss.fff", null));
                    if (dvotefrom <= DateTime.Now && dvoteto >= DateTime.Now)
                    {
                        string url = (HttpContext.Current.Request.ServerVariables["URL"]).ToLower();
                        if (url.IndexOf("gallery.aspx") <= 0 && url.IndexOf("login.aspx") <= 0)
                        {
                            HttpContext.Current.Response.Redirect("gallery.aspx");
                            HttpContext.Current.Response.End();
                            return true;
                        }
                        return true;
                    }                   
                }*/
                string senddate = System.Configuration.ConfigurationManager.AppSettings["voteto"].ToString();
                if (senddate != "")
                {
                    DateTime sEdate = Convert.ToDateTime(DateTime.ParseExact(senddate, "MM/dd/yyyy HH:mm:ss.fff", null));
                    if (sEdate <= DateTime.Now || ("" + HttpContext.Current.Request.QueryString["mode"]).ToString() == "over")
                    {
                        string url = (HttpContext.Current.Request.ServerVariables["URL"]).ToLower();
                        if (url.IndexOf("over.aspx") <= 0 && url.IndexOf("login.aspx") <= 0)
                        {
                            HttpContext.Current.Response.Redirect("over.aspx");
                            HttpContext.Current.Response.End();
                            return true;
                        }
                        return true;
                    }
                }
                return false;
            }
            catch (Exception E1)
            {
                return false;
            }
        }

        public static string GetVotingDaysLeft()
        {
            try
            {
                string enddate = System.Configuration.ConfigurationManager.AppSettings["enddate"].ToString();
                if (enddate != "")
                {
                    DateTime sEdate = Convert.ToDateTime(DateTime.ParseExact(enddate, "MM/dd/yyyy HH:mm:ss.fff", null));
                    if (sEdate <= DateTime.Now)
                    {
                        return "0";
                    }
                    else
                        return sEdate.Subtract(DateTime.Now).Days.ToString();
                }
                return "0";
            }
            catch (Exception E1)
            {
                return "0";
            }
        }

        public static bool DomainValidate(string myemail)
        {
            string[] domains = new string[] { "AC", "AD", "AE", "AERO", "AF", "AG", "AI", "AL", "AM", "AN", "AO", "AQ", "AR", "ARPA", "AS", "ASIA", "AT", "AU", "AW", "AX", "AZ", "BA", "BB", "BD", "BE", "BF", "BG", "BH", "BI", "BIZ", "BJ", "BM", "BN", "BO", "BR", "BS", "BT", "BV", "BW", "BY", "BZ", "CA", "CAT", "CC", "CD", "CF", "CG", "CH", "CI", "CK", "CL", "CM", "CN", "CO", "COM", "COOP", "CR", "CU", "CV", "CX", "CY", "CZ", "DE", "DJ", "DK", "DM", "DO", "DZ", "EC", "EDU", "EE", "EG", "ER", "ES", "ET", "EU", "FI", "FJ", "FK", "FM", "FO", "FR", "GA", "GB", "GD", "GE", "GF", "GG", "GH", "GI", "GL", "GM", "GN", "GOV", "GP", "GQ", "GR", "GS", "GT", "GU", "GW", "GY", "HK", "HM", "HN", "HR", "HT", "HU", "ID", "IE", "IL", "IM", "IN", "INFO", "INT", "IO", "IQ", "IR", "IS", "IT", "JE", "JM", "JO", "JOBS", "JP", "KE", "KG", "KH", "KI", "KM", "KN", "KP", "KR", "KW", "KY", "KZ", "LA", "LB", "LC", "LI", "LK", "LR", "LS", "LT", "LU", "LV", "LY", "MA", "MC", "MD", "ME", "MG", "MH", "MIL", "MK", "ML", "MM", "MN", "MO", "MOBI", "MP", "MQ", "MR", "MS", "MT", "MU", "MUSEUM", "MV", "MW", "MX", "MY", "MZ", "NA", "NAME", "NC", "NE", "NET", "NF", "NG", "NI", "NL", "NO", "NP", "NR", "NU", "NZ", "OM", "ORG", "PA", "PE", "PF", "PG", "PH", "PK", "PL", "PM", "PN", "PR", "PRO", "PS", "PT", "PW", "PY", "QA", "RE", "RO", "RS", "RU", "RW", "SA", "SB", "SC", "SD", "SE", "SG", "SH", "SI", "SJ", "SK", "SL", "SM", "SN", "SO", "SR", "ST", "SU", "SV", "SY", "SZ", "TC", "TD", "TEL", "TF", "TG", "TH", "TJ", "TK", "TL", "TM", "TN", "TO", "TP", "TR", "TRAVEL", "TT", "TV", "TW", "TZ", "UA", "UG", "UK", "US", "UY", "UZ", "VA", "VC", "VE", "VG", "VI", "VN", "VU", "WF", "WS", "XXX", "YE", "YT", "ZA", "ZM", "ZW" };
            string[] emailarr = myemail.Split('.');
            string domain = emailarr[emailarr.Length - 1];
            domain = domain.ToUpper();
            try
            {
                if (Array.IndexOf(domains, domain) < 0)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                for (int i = 0; i < domains.Length; i++)
                {
                    if (domains[i] == domain)
                        return true;
                }
                return false;
            }
        }

        public static void GenerateThumbnails(double scaleFactor, int height, int width, System.IO.Stream sourcePath, string targetPath)
        {
            using (System.Drawing.Image  image = System.Drawing.Image.FromStream(sourcePath))
            {
                // can given width of image as we want
                int newWidth = width;//(int)(image.Width * scaleFactor);
                // can given height of image as we want
                int newHeight = height;//(int)(image.Height * scaleFactor);
                System.Drawing.Bitmap thumbnailImg = new System.Drawing.Bitmap(newWidth, newHeight);
                thumbnailImg.SetResolution(72, 72);//Newly added
                System.Drawing.Graphics thumbGraph = System.Drawing.Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                System.Drawing.Rectangle imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }
       
        public static string GetSessionUserId()
        {
            string rVal = "" + HttpContext.Current.Session["userid"];
            if (rVal == "")
                
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["Location"];
                if (cookie != null)
                {
                    rVal = "" + cookie["userid"];
                    HttpContext.Current.Session["userid"] = rVal;
                    if ("" + cookie["LocationId"] != "") HttpContext.Current.Session["LocationId"] = "" + cookie["LocationId"];
                }                
            }
            return rVal;
        }

        public static string GetLocationId()
        {
            string rVal = "";
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Location"];
            if (cookie != null)
            {
                rVal = "" + cookie["LocationId"];
            }
            return rVal;
        }

        public enum enumPageId
        { 
            Home=1,
            Sales,
            SalesReturn,
            Administration
            
        }

        public enum enumControlId
        {

            Home_Administration=1,
            Home_Sales=2,
            Home_SalesReturn=3,
            Home_Macys=4,

            Administration_SalesSummaryReport=5,
            Administration_SalesDetailsReport=6,
            Administration_EventSoldCountReport=7,
            Administration_CategorySummaryReport=8,
            Administration_ClerkSummaryReport=9,
            Administration_VoucherSummaryReport=10,
            Administration_SDReturnsReports=11,
            Administration_UserLogReport=12,
            Administration_SalesReturnSummaryReport=13,
            Administration_SalesReturnDetailsReport=14,
            Administration_MacysSurveryDataReport=15,
            Administration_StockManagement=16,
            Administration_AvailabilityCheck=17
        }

        public static bool HasRights(enumPageId PageId, enumControlId ControlId)
        {
            DBConnection dbCon = new DBConnection();

            string sql = "select UserRight from UserMaster where UserId = " + HttpContext.Current.Session["userid"];
            string UserRights = dbCon.RetData(sql,CommandType.Text);
            string[] arrUserRights = UserRights.Split(',');

            sql = "select UserRights from AccessRights where PageId = " + Convert.ToInt16(PageId) + " and ControlId = " + Convert.ToInt16(ControlId);
            string AccessRights = dbCon.RetData(sql, CommandType.Text);
            if (AccessRights == "")
            {
                return true;
            }
            else
            {
                string[] arrAccessRights = AccessRights.Split(',');
                for (int i = 0; i < arrUserRights.Length; i++)
                {
                    if (Array.IndexOf(arrAccessRights, arrUserRights[i]) >= 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
