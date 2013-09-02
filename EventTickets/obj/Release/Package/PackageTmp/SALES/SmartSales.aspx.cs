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

namespace EventTickets.Sales
{
    public partial class SmartSales : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int cid = EventFunctions.Functions.ToInt(Request.QueryString["cid"]);
                string sfilter = "" + Request.QueryString["filter"];
                string sSmart = System.Configuration.ConfigurationManager.AppSettings["smart_destination_url"] + "/packages.json?dest=Nyc";
                string dTable = "packages";
                if (cid == 3)
                {
                    sSmart = System.Configuration.ConfigurationManager.AppSettings["smart_destination_url"] + "/attractionFilter.json?categoryCode=Nyc_Prod_Pt_Att&filters=" + sfilter;
                    dTable = "attractionList";
                    switch (sfilter)
                    {
                        //SmartTitle
                        case "atpt_cat_ride_isride":
                            SmartTitle.InnerText = "Rides & Adventure";
                            break;

                        case "atpt_cat_zoo_iszoo":
                            SmartTitle.InnerText = "Zoos & Aquariums";
                            break;

                        case "atpt_cat_museum_ismuseum":
                            SmartTitle.InnerText = "Museums & Historical";
                            break;

                        case "atpt_cat_rental_isrental":
                            SmartTitle.InnerText = "Rental";
                            break;

                        case "atpt_cat_tour_istour":
                            SmartTitle.InnerText = "Tour";
                            break;


                        case "atpt_cat_childfree_ischildfree":
                            SmartTitle.InnerText = "Kids Free Entry";
                            break;

                        case "atpt_cat_familyfriendly_isfamilyfriendly":
                            SmartTitle.InnerText = "Family Friendly";
                            break;

                        case "atpt_cat_openlate_isopenlate":
                            SmartTitle.InnerText = "Open Late";
                            break;

                        case "atpt_cat_seasonalsmmer_isseasonalsummer":
                            SmartTitle.InnerText = "Seasonal - Summer";
                            break;

                        case "atpt_cat_seasonalwinter_isseasonalwinter":
                            SmartTitle.InnerText = "Seasonal - Winter";
                            break;

                        case "atpt_cat_nochild_isnochild":
                            SmartTitle.InnerText = "No Kids Allowed";
                            break;
                    }
                }                
                string jsonString = new WebClient().DownloadString(sSmart);

                XmlDocument xd = new XmlDocument();
                //jsonString = "{ \"rootNode\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";
                xd = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonString, "uidPk");
                DataSet ds = new DataSet();
                ds.ReadXml(new XmlNodeReader(xd));
                DataTable DT = ds.Tables[dTable];
                if (cid == 4)
                {
                    DT.Columns["shortProductName"].ColumnName = "destinationName";
                    DT.Columns["sellCopyShort"].ColumnName = "attractionName";
                }
                dC.DataSource = DT;
                dC.DataBind();
            }
            catch (Exception)
            {
                                
            }
        }
    }
}