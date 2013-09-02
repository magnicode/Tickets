using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EventFunctions;

namespace EventTickets.survey
{
    public partial class macys : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            this.Form.DefaultButton = bSubmit.UniqueID;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lUser.Text = "" + Session["username"];
                lDt.Text = DateTime.Now.ToString("M/d/yyyy");
                CLASSES.survey obj = new CLASSES.survey();
                tHotel.DataSource = obj.getHotels();
                tHotel.DataTextField = "Name";
                tHotel.DataValueField = "Name";
                tHotel.DataBind();
                tHotel.Items.Insert(0, new ListItem("Please Select", ""));

                dCnt.DataSource = obj.getCountry();
                dCnt.DataTextField = "Name";
                dCnt.DataValueField = "Id";
                dCnt.DataBind();

                dState.DataSource = obj.getStates();
                dState.DataTextField = "state_name";
                dState.DataValueField = "state_name";
                dState.DataBind(); 
				obj = null;
            }
            if (dVisit.SelectedItem.Value == "1")
            {
                dState.Style.Add("display", "none");
                dCnt.Style.Add("display", "");
            }
            else
            {
                dState.Style.Add("display", "");
                dCnt.Style.Add("display", "none");
        }
        
            if (dStay.Value == "3")
                trHotel.Style.Remove("display");
            else
                trHotel.Style.Add("display", "none");
        }

        protected void bSubmit_Click(object sender, EventArgs e)
        {           
            CLASSES.survey obj = new CLASSES.survey();
            obj._FN = "";
            obj._LN = "";
            obj._City = "";
            obj._Email = tMail.Text.Replace("'", "''");
            if (dVisit.SelectedItem.Value == "2")
                obj._State = dState.SelectedItem.Value;
            else
            obj._State = "";
            obj._Country = dCnt.SelectedValue;
            obj._ForB = obj._ForL = obj._ForBoth = 0;
            if (dPurp.SelectedValue == "1")
                obj._ForB = 1;
            else if (dPurp.SelectedValue == "2")
                obj._ForL = 1;
            else if (dPurp.SelectedValue == "3")
                obj._ForBoth = 1;
            obj._Total = dNo.SelectedValue;
            obj._WhereStay = dStay.Value;
            if (dStay.Value == "3")
            	obj._Hotel = tHotel.Text.Replace("'", "''");
            else
                obj._Hotel = "";
            obj._About = dHear.Value;
            obj._Heard = tHN.Text.Replace("'", "''");
            obj._Brand = dBrand.SelectedItem.Value;
            int iSurveyId =0;
            if (EventFunctions.Functions.ToInt(ViewState["SurveyId"]) > 0)
            {
                obj.SurveyId = EventFunctions.Functions.ToInt(ViewState["SurveyId"]);
                iSurveyId = obj.updateSurvey();
            }
            else
                iSurveyId = obj.saveSurvey();
            if (iSurveyId > 0)
            {
                trMain.Visible = false;
                //trHd.Visible = false;
                tblMsg.Visible = true;
                tMail.Text = "";
                dCnt.ClearSelection();
                dPurp.ClearSelection();
                dNo.ClearSelection();
                dBrand.ClearSelection();
                
                dCnt.SelectedIndex = 0;
                dPurp.SelectedIndex = 0;
                dNo.SelectedIndex = 0;
                dBrand.SelectedIndex = 0;
                dHear.SelectedIndex = 0;
                dStay.SelectedIndex = 0;                
                tHotel.Text = "";
                tHN.Text = "";
                hMsg.InnerHtml = "The form has been successfully saved!";
                ViewState["SurveyId"] = iSurveyId;
                bEdit.Visible = true;
            }
            else
            {
                trMain.Visible = false;
                tblMsg.Visible = true;
                hMsg.InnerHtml = "Sorry!!! Please try again.";
            }
            obj = null;
        }

        protected void bClear_Click(object sender, EventArgs e)
        {
            //Response.Redirect("Survey.aspx");
            tMail.Text = "";
            dCnt.ClearSelection();
            dPurp.ClearSelection();
            dNo.ClearSelection();
            dBrand.ClearSelection();

            dCnt.SelectedIndex = 0;
            dPurp.SelectedIndex = 0;
            dNo.SelectedIndex = 0;
            dBrand.SelectedIndex = 0;
            dHear.SelectedIndex = 0;
            dStay.SelectedIndex = 0;
            tHotel.Text = "";
            tHN.Text = "";
            ViewState["SurveyId"] = "";
        }

        protected void btnRet_Click(object sender, EventArgs e)
        {
            //Response.Redirect("Survey.aspx");
            tMail.Text = "";
            dCnt.ClearSelection();
            dPurp.ClearSelection();
            dNo.ClearSelection();
            dBrand.ClearSelection();

            dCnt.SelectedIndex = 0;
            dPurp.SelectedIndex = 0;
            dNo.SelectedIndex = 0;
            dBrand.SelectedIndex = 0;
            dHear.SelectedIndex = 0;
            dStay.SelectedIndex = 0;
            tHotel.Text = "";
            tHN.Text = "";
            trMain.Visible = true;
            tblMsg.Visible = false;
            ViewState["SurveyId"] = "";
        }

        protected void bEdit_Click(object sender, ImageClickEventArgs e)
        {
            if (EventFunctions.Functions.ToInt(ViewState["SurveyId"]) > 0)
            {
                CLASSES.survey objSurvey = new CLASSES.survey();
                objSurvey.SurveyId = EventFunctions.Functions.ToInt(ViewState["SurveyId"]);
                DataTable DT = objSurvey.getSurveyDetails();
                if (DT.Rows.Count > 0)
                {
                    dCnt.SelectedIndex = dCnt.Items.IndexOf(dCnt.Items.FindByValue("" + DT.Rows[0]["Country"]));
                    dState.SelectedIndex = dState.Items.IndexOf(dState.Items.FindByValue("" + DT.Rows[0]["State"]));
                    if ("" + DT.Rows[0]["State"] != "")
                        dVisit.SelectedValue = "2";
                    if (Functions.ToInt(DT.Rows[0]["ForBusiness"]) == 1)
                        dPurp.SelectedIndex = 1;
                    if (Functions.ToInt(DT.Rows[0]["ForLiesure"]) == 1)
                        dPurp.SelectedIndex = 2;
                    if (Functions.ToInt(DT.Rows[0]["ForBoth"]) == 1)
                        dPurp.SelectedIndex = 3;
                    dNo.SelectedIndex = dNo.Items.IndexOf(dNo.Items.FindByValue("" + DT.Rows[0]["PeopleInParty"]));
                    dStay.Value = "" + DT.Rows[0]["WhereStaying"];
                    tHotel.SelectedIndex = tHotel.Items.IndexOf(tHotel.Items.FindByValue("" + DT.Rows[0]["Hotel"]));
                    dHear.Value = "" + DT.Rows[0]["HearAboutPass"];
                    tMail.Text = "" + DT.Rows[0]["Email"];
                    dBrand.SelectedIndex = dBrand.Items.IndexOf(dBrand.Items.FindByValue("" + DT.Rows[0]["Brand"]));
                    trMain.Visible = true;
                    tblMsg.Visible = false;
                }
                objSurvey = null;
            }
        }

        protected void bE_Click(object sender, EventArgs e)
        {
            bEdit_Click(null, null);
        }

        protected void lLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            HttpCookie cookie = Request.Cookies["Location"];
            if (cookie != null)
            {
                cookie["userid"] = "";
                Response.Cookies.Remove("userid");
                Response.Cookies.Set(cookie);
            }
            Response.Redirect("/Account/Login.aspx?redirect=/survey/Survey.aspx");
        }
    }
}