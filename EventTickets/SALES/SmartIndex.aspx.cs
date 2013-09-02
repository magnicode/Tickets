using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace EventTickets.Sales
{
    public partial class SmartIndex : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int cid = EventFunctions.Functions.ToInt(Request.QueryString["cid"]);
            if (cid == 3)
            {
                DataTable DT = new DataTable();
                DT.Columns.Add(new DataColumn("filter"));
                DT.Columns.Add(new DataColumn("SmartDest"));
                DT.Columns.Add(new DataColumn("Name"));
                DT.Columns.Add(new DataColumn("ImageUrl"));
                DataRow DR = DT.NewRow();
                DR["filter"] = "atpt_cat_ride_isride";
                DR["SmartDest"] = cid.ToString();
                DR["Name"] = "Rides & Adventure";
                DR["ImageUrl"] = "smart_ride.jpg";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR["filter"] = "atpt_cat_zoo_iszoo";
                DR["SmartDest"] = cid.ToString();
                DR["Name"] = "Zoos & Aquariums";
                DR["ImageUrl"] = "smart_zoo.jpg";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR["filter"] = "atpt_cat_museum_ismuseum";
                DR["SmartDest"] = cid.ToString();
                DR["Name"] = "Museums & Historical";
                DR["ImageUrl"] = "smart_museum.jpg";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR["filter"] = "atpt_cat_rental_isrental";
                DR["SmartDest"] = cid.ToString();
                DR["Name"] = "Rental";
                DR["ImageUrl"] = "smart_rental.jpg";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR["filter"] = "atpt_cat_tour_istour";
                DR["SmartDest"] = cid.ToString();
                DR["Name"] = "Tour";
                DR["ImageUrl"] = "smart_tour.jpg";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR["filter"] = "atpt_cat_childfree_ischildfree";
                DR["SmartDest"] = cid.ToString();
                DR["Name"] = "Kids Free Entry";
                DR["ImageUrl"] = "smart_childfree.jpg";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR["filter"] = "atpt_cat_familyfriendly_isfamilyfriendly";
                DR["SmartDest"] = cid.ToString();
                DR["Name"] = "Family Friendly";
                DR["ImageUrl"] = "smart_familyfriendly.jpg";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR["filter"] = "atpt_cat_openlate_isopenlate";
                DR["SmartDest"] = cid.ToString();
                DR["Name"] = "Open Late";
                DR["ImageUrl"] = "smart_openlate.jpg";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR["filter"] = "atpt_cat_seasonalsmmer_isseasonalsummer";
                DR["SmartDest"] = cid.ToString();
                DR["Name"] = "Seasonal - Summer";
                DR["ImageUrl"] = "smart_seasonsumm.jpg";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR["filter"] = "atpt_cat_seasonalwinter_isseasonalwinter";
                DR["SmartDest"] = cid.ToString();
                DR["Name"] = "Seasonal - Winter";
                DR["ImageUrl"] = "smart_seasonalwinter.jpg";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR["filter"] = "atpt_cat_nochild_isnochild";
                DR["SmartDest"] = cid.ToString();
                DR["Name"] = "No Kids Allowed";
                DR["ImageUrl"] = "smart_nochild.jpg";
                DT.Rows.Add(DR);

                dC.DataSource = DT;
                dC.DataBind();
               
            }
        }
    }
}