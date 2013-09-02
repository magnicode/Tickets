using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MYSQLDatabase;
using System.Data;
using System.Web.Security;

namespace EventTickets.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    string sql = "Select LocationId, Name from LocationMaster where status = 1 Order by Name";
                    DBConnection dbCon = new DBConnection();
                    DataSet dtUser = dbCon.ExecuteDataSet(sql, CommandType.Text);
                    Location.DataSource = dtUser.Tables[0];
                    Location.DataTextField = "Name";
                    Location.DataValueField = "LocationId";
                    Location.DataBind();

                    HttpCookie cookie = Request.Cookies["Location"];

                    if (cookie != null && cookie["LocationId"] != "")
                    {
                        Location.SelectedIndex = Location.Items.IndexOf(Location.Items.FindByValue(cookie["LocationId"]));
                    }
                }
                catch (Exception)
                {
                    
                   
                }
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            try
            {
                LoginErrMsg.Text = "";
                string sql = "Select * from UserMaster where UserName = '" + UserName.Text.Replace("'", "''") + "' and status = 1";
                DBConnection dbCon = new DBConnection();
                DataSet dtUser = dbCon.ExecuteDataSet(sql, CommandType.Text);

                if (dtUser.Tables[0].Rows.Count > 0)
                {
                    if (classes.Crypt.Decrypt(dtUser.Tables[0].Rows[0]["password"].ToString()) ==  Password.Text)
                    {
                        sql = "insert into UserLog (userid, logindatetime, ip) values (" + dtUser.Tables[0].Rows[0]["userid"].ToString() + ",now(),'" + Request.UserHostAddress + "') ";
                        dbCon.Execute(sql, CommandType.Text);

                        
                        Session.Timeout = 60;
                        Session["userid"] = dtUser.Tables[0].Rows[0]["userid"].ToString();
                        Session["username"] = UserName.Text;
                        Session["userfullname"] = dtUser.Tables[0].Rows[0]["firstname"].ToString() + dtUser.Tables[0].Rows[0]["lastname"].ToString();
                        Session["useremail"] = dtUser.Tables[0].Rows[0]["email"].ToString();
                        Session["UserRights"] = dtUser.Tables[0].Rows[0]["userright"].ToString();
                        Session["LocationId"] = Location.SelectedValue;

                        string UserPermissions = "0"; 
                        if(dtUser.Tables[0].Rows[0]["IndividualPermissions"].ToString()!="")
                            UserPermissions += "," + dtUser.Tables[0].Rows[0]["IndividualPermissions"].ToString();

                        if (("" + dtUser.Tables[0].Rows[0]["GroupPermissions"]).ToString() != "")
                        {
                            sql = "select Permissions from GroupPermissions where GroupId in (" + dtUser.Tables[0].Rows[0]["GroupPermissions"].ToString() + ")";
                            DataTable dtGrp = dbCon.ExecuteDataSet(sql, CommandType.Text).Tables[0];
                            if (dtGrp.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtGrp.Rows.Count; i++)
                                {
                                    UserPermissions += (UserPermissions != "" ? "," : "") + dtGrp.Rows[i]["Permissions"].ToString();
                                }
                            }
                        }
                        Session["UserPermissions"] = UserPermissions;

                        HttpCookie cookie = Request.Cookies["Location"];
                        if (cookie == null)
                            cookie = new HttpCookie("Location");
                        cookie["LocationId"] = Location.SelectedValue;
                        cookie["userid"] = dtUser.Tables[0].Rows[0]["userid"].ToString();
                        //cookie.Domain = Request.ServerVariables["HTTP_HOST"]; 
                        cookie.Expires = DateTime.Now.AddHours(1);
                        Response.Cookies.Add(cookie);

                        //----------------------------
                        FormsAuthenticationTicket tkt;
                        string cookiestr;
                        HttpCookie ck;
                        tkt = new FormsAuthenticationTicket(1, dtUser.Tables[0].Rows[0]["userid"].ToString(), DateTime.Now, DateTime.Now.AddMinutes(30), true, UserPermissions);
                        cookiestr = FormsAuthentication.Encrypt(tkt);
                        ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
                        ck.Expires = tkt.Expiration;
                        ck.Path = FormsAuthentication.FormsCookiePath;
                        Response.Cookies.Add(ck);

                        //------------------------------
                        
                        string strRedirect;
                        strRedirect = Request["ReturnUrl"];
                        if (strRedirect == null)
                            strRedirect = "../sales/index.aspx";
                        Response.Redirect(strRedirect, true);

	                     
                        /*
                        string sredir = "" + Request.QueryString["redirect"];
                        if (sredir == "")
                            Response.Redirect("../sales/index.aspx");
                        else
                            Response.Redirect(sredir);
                        */

                    }
                    else
                    {
                        LoginErrMsg.Text = "Invalid Username and/or Password";

                        sql = "insert into FailedLogin (logindatetime, username, password, ip) values (now(), '" + UserName.Text.Replace("'", "''") + "', '" + Password.Text.Replace("'", "''") + "', '" + Request.UserHostAddress + "') ";
                        dbCon.Execute(sql, CommandType.Text);
                        Response.Redirect("login.aspx", true);
                    }
                }
                else
                {
                    LoginErrMsg.Text = "Invalid Username and/or Password";
                    
                    sql = "insert into FailedLogin (logindatetime, username, password, ip) values (now(), '" + UserName.Text.Replace("'", "''") + "', '" + Password.Text.Replace("'", "''") + "', '" + Request.UserHostAddress + "') ";
                    dbCon.Execute(sql, CommandType.Text);

                }
                dbCon = null;
            }
            catch (Exception ee)
            {
            }
        }
    }
}
