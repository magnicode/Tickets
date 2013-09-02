using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MYSQLDatabase;

namespace EventTickets.CLASSES
{
    public class survey
    {
        public string _FN;
        public string _LN;
        public string _City;
        public string _Email;
        public string _State;
        public string _Country;
        public int _ForB;
        public int _ForL;
        public int _ForBoth;
        public string _Total;
        public string _WhereStay;
        public string _Hotel;
        public string _About;
        public string _Heard;
        public int SurveyId;
        public string _Brand;



        public DataTable getCountry()
        {
            string sql = "SELECT Id,NAME FROM Countries WHERE UPPER(NAME) = 'UNITED STATES' UNION ALL SELECT Id,NAME FROM Countries";
            DBConnection db = new DBConnection();
            DataTable dt = db.ExecuteDataSet(sql, CommandType.Text).Tables[0];
            db = null;
            return dt;
        }

        public DataTable getStates()
        {
            string sql = "SELECT state_name FROM State WHERE Country = 'US' order by state_name ";
            DBConnection db = new DBConnection();
            DataTable dt = db.ExecuteDataSet(sql, CommandType.Text).Tables[0];
            db = null;
            return dt;
        }

        public DataTable getHotels()
        {
            string sql = "SELECT NAME FROM MacyHotels order by NAME";
            DBConnection db = new DBConnection();
            DataTable dt = db.ExecuteDataSet(sql, CommandType.Text).Tables[0];
            db = null;
            return dt;
        }

        public int saveSurvey()
        {
            int bOut = 0;

            DBConnection db = new DBConnection();
            try
            {
                string sql = @"insert into MacyQueries (VisitDate,FirstName,LastName,City,Email,State,Country,ForBusiness,ForLiesure,ForBoth,PeopleInParty,WhereStaying,Hotel,HearAboutPass,HeardFrom,Brand,clerkid)
                            values ('" + DateTime.Now.ToString("yyyy-M-d") + "','" + _FN + "','" + _LN + "','" + _City + "','" + _Email.Replace("'", "''") + "','" + _State + "','" + _Country + "'," + _ForB + "," + _ForL + "," + _ForBoth + ",'" + _Total + "','" + _WhereStay.Replace("'", "''") + "','" + _Hotel.Replace("'", "''") + "','" + _About.Replace("'", "''") + "','" + _Heard + "','" + _Brand.Replace("'", "''") + "',"+ EventFunctions.Functions.GetSessionUserId()  +");select last_insert_id();";
                bOut = EventFunctions.Functions.ToInt(db.RetData(sql, CommandType.Text));                
            }
            catch (Exception)
            { }
            db = null;
            return bOut;
        }

        public DataTable getHotel()
        {
            DBConnection db = new DBConnection();
            DataTable dt = new DataTable();
            try
            {
                dt = db.ExecuteDataSet("SELECT DISTINCT hotel FROM MacyQueries where hotel != ''", CommandType.Text).Tables[0];
            }
            catch (Exception)
            {}
            db = null;
            return dt;
        }

        public DataTable getQueries()
        {
            string sql = @"SELECT QueryId,DATE_FORMAT(VisitDate,'%m/%d/%Y') VisitDate,FirstName,LastName,City,Email,State,(SELECT NAME FROM Countries WHERE Id=MacyQueries.Country) Country,ForBusiness,ForLiesure,ForBoth,PeopleInParty,WhereStaying,Hotel,HearAboutPass,HeardFrom
                        FROM MacyQueries";
            DBConnection db = new DBConnection();
            DataTable dt = new DataTable();
            try
            {
                dt = db.ExecuteDataSet(sql, CommandType.Text).Tables[0];
            }
            catch (Exception)
            { }
            db = null;
            return dt;
        }

        public DataTable getSurveyDetails()
        {
            string sql = "select * from MacyQueries where QueryId = " + SurveyId;
            DBConnection db = new DBConnection();
            DataTable dt = new DataTable();
            try
            {
                dt = db.ExecuteDataSet(sql, CommandType.Text).Tables[0];
            }
            catch (Exception)
            { }
            db = null;
            return dt;
        }

        public int updateSurvey()
        {
            DBConnection db = new DBConnection();
            try
            {
                string sql = @"update MacyQueries set FirstName = '" + _FN + @"',
                                LastName='" + _LN + "',City='" + _City + "',Email='" + _Email.Replace("'", "''") + @"',
                                State='" + _State + "',Country='" + _Country + @"',
                                ForBusiness=" + _ForB + ",ForLiesure=" + _ForL + ",ForBoth=" + _ForBoth + @",
                                PeopleInParty='" + _Total + "',WhereStaying='" + _WhereStay.Replace("'", "''") + @"',
                                Hotel='" + _Hotel.Replace("'", "''") + "',HearAboutPass='" + _About.Replace("'", "''") + @"',
                                HeardFrom='" + _Heard + "',Brand = '" + _Brand.Replace("'", "''") + "' where QueryId = " + SurveyId;
                db.Execute(sql, CommandType.Text);
            }
            catch (Exception)
            { }
            db = null;
            return SurveyId;
        }
    }
}