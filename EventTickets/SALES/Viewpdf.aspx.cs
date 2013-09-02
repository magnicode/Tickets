using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using MYSQLDatabase;
using System.Data;

namespace EventTickets.SALES
{
    public partial class Viewpdf : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //"https://stg.smartdestinations.com/checkout/pt-get-ticket.ep?addGuideBook=false&mobile=false&orderNumber=OBYIu0D7wRw1jcdh9uRZ7A%3D%3D";

            string path = "";
            if ("" + Request.QueryString["SalesId"] != "")
            {
                DBConnection dbCon = new DBConnection();
                string sql = "select PrintUrl from SalesMaster where SalesMasterId = " + Request.QueryString["SalesId"];
                path = dbCon.RetData(sql, CommandType.Text);
                dbCon = null;
            }
            else
            {
                path = Request.QueryString["filepath"];
            }
            System.Net.WebClient client = new System.Net.WebClient();
            Byte[] buffer = client.DownloadData(path);

            if (buffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", buffer.Length.ToString());
                Response.BinaryWrite(buffer);
                Response.End();
            }
        }
    }
}