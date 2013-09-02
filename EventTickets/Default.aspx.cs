using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Net;
using System.Xml;
using Newtonsoft.Json;


namespace EventTickets
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ("" + Session["userid"] != "")
            {
                Response.Redirect("/Sales/Index.aspx");
            }
            else
                Response.Redirect("/Account/Login.aspx");
            return;
            /* */
            //string jsonString = new WebClient().DownloadString("http://www.smartdestinations.com/packages.json?dest=Nyc");
            string jsonString = new WebClient().DownloadString("http://www.smartdestinations.com/attractionFilter.json?categoryCode=Nyc_Prod_Pt_Att&filters=");
            XmlDocument xd = new XmlDocument();
            //jsonString = "{ \"rootNode\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";
            xd = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonString,"uidPk");
            DataSet ds = new DataSet();
            ds.ReadXml(new XmlNodeReader(xd));
            DataTable DT = ds.Tables["attractionList"];//packages"];
            dC.DataSource = DT;
            dC.DataBind();
            //return ds;
            //HttpRuntimeSection section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
            // section.MaxRequestLength
        }        
    }   
}
