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
using MYSQLDatabase;
using EventFunctions;

namespace EventTickets.Sales
{
    public partial class SmartSalesDetails : System.Web.UI.Page
    {
        int cid, eid;
        protected void Page_Load(object sender, EventArgs e)
        {
            cid = EventFunctions.Functions.ToInt(Request.QueryString["cid"]);
            eid = EventFunctions.Functions.ToInt(Request.QueryString["eid"]);
            if (!IsPostBack)
            {             
                string sSmart;
                //if (cid == 4) return;//have to check for package api for individual item
                pAdult.Disabled = true;
                pChild.Disabled = true;
                if (cid == 4)//Package
                {
                    sSmart = System.Configuration.ConfigurationManager.AppSettings["smart_destination_url"] + "/packages.json?dest=Nyc&pids=" + eid;
                    string jsonString = new WebClient().DownloadString(sSmart);

                    XmlDocument xd = new XmlDocument();
                    //jsonString = "{ \"rootNode\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";
                    xd = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonString, "uidPk");
                    DataSet ds = new DataSet();
                    ds.ReadXml(new XmlNodeReader(xd));
                    DataTable DT = ds.Tables["packages"];
                    DataRow[] DR = DT.Select("uidPk='" + eid + "'");
                    if (DR.Length > 0)
                    {
                        lName.Text = DR[0]["shortProductName"].ToString();
                        tDesc.InnerHtml = "<div style='height:300px;overflow:auto;padding:20px;padding-top:10px;'>" + DR[0]["sellCopyShort"].ToString() + "<br/><br/>" + DR[0]["sellCopy"].ToString() + "</div>";
                        tImgPrice.InnerHtml = "<img src='' width='134' height='0' /><br>Adult Price: $" + EventFunctions.Functions.ToDecimal(DR[0]["adultSellPrice"].ToString()).ToString("0.00") + "<br>Child Price: $" + EventFunctions.Functions.ToDecimal(DR[0]["childSellPrice"].ToString()).ToString("0.00");
                        UidPk.Value = eid.ToString();
                        productname.Value = DR[0]["shortProductName"].ToString();
                        productCode.Value = DR[0]["code"].ToString();
                        pAdult.Disabled = false;
                        pChild.Disabled = false;
                        pricechild.Value = DR[0]["childSellPrice"].ToString();
                        priceadult.Value = DR[0]["adultSellPrice"].ToString();
                    }                                        
                }
                else
                {
                    sSmart = System.Configuration.ConfigurationManager.AppSettings["smart_destination_url"] + "/attractions.json?pids=" + eid;

                    string jsonString = new WebClient().DownloadString(sSmart);
                    XmlDocument xd = new XmlDocument();
                    //jsonString = "{ \"ProductDetail\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";
                    jsonString = "{ \"ProductDetail\": " + jsonString.Trim() + "}";
                    xd = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonString, "ProductDetail");
                    DataSet ds = new DataSet();
                    ds.ReadXml(new XmlNodeReader(xd));
                    DataTable DTAttrib = ds.Tables["attributeValues"];
                    DataTable DPrd = ds.Tables["ProductDetail"];
                    if (DTAttrib != null)
                    {
                        if (DTAttrib.Rows.Count > 0)
                        {
                            lName.Text = DPrd.Rows[0]["attractionName"].ToString();
                            productCode.Value = DPrd.Rows[0]["productCode"].ToString();
                            tDesc.InnerHtml = "<div style='height:300px;overflow:auto; padding:20px; padding-top:10px;'>" + DTAttrib.Rows[0]["ptAttractionSellCopy"].ToString() + "<br/><br/>" + DTAttrib.Rows[0]["ptAttractionSeoBody"].ToString() + "<br/><br/>" + DTAttrib.Rows[0]["ptAttractionSeoBodyExtended"].ToString() + "</div>";
                            tImgPrice.InnerHtml = "<img src='' width='134' height='0' /><br>Adult Price: $" + EventFunctions.Functions.ToDecimal(DPrd.Rows[0]["adultRetailPrice"].ToString()).ToString("0.00") + "<br>Child Price: $" + EventFunctions.Functions.ToDecimal(DPrd.Rows[0]["childRetailPrice"].ToString()).ToString("0.00");
                            pAdult.Disabled = false;
                            pChild.Disabled = false;
                            UidPk.Value = eid.ToString();
                            productname.Value = DPrd.Rows[0]["attractionName"].ToString();
                            pricechild.Value = DPrd.Rows[0]["childRetailPrice"].ToString();
                            priceadult.Value = DPrd.Rows[0]["adultRetailPrice"].ToString();
                        }
                    }
                }                
            }
        }        
    }
}