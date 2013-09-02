using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MYSQLDatabase;

namespace EventTickets.classes
{
    public class Users
    {
        public enum enumRoles
        { 
            EnterOrders = 1,
            ApproveRefunds,
            EnterRefunds,
            BasicClerkReports,
            BasicSalesReports,
            BasicManagementReports,
            ProfitAndLossReport,
            MacysReports,
            InventoryManagement,
            UserCreation

        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string IndPermissions { get; set; }
        public string GrpPermissions { get; set; }
        public int Status { get; set; }

        public DataTable GetIndividualPermissions()
        {
            DBConnection dbCon = new DBConnection();
            string sql = "select IndivPermissionId, IndivPermissionName from IndividualPermissions order by IndivPermissionName";
            DataTable dt = dbCon.ExecuteDataSet(sql, CommandType.Text).Tables[0];
            return dt;
        }

        public DataTable GetGroupPermissions()
        {
            DBConnection dbCon = new DBConnection();
            string sql = "select GroupId, GroupName from GroupPermissions order by GroupName";
            DataTable dt = dbCon.ExecuteDataSet(sql, CommandType.Text).Tables[0];
            return dt;
        }

        public static bool HasRights(enumRoles Roles)
        {
            string[] arrUserPermissions = HttpContext.Current.Session["UserPermissions"].ToString().Split(',');
            if (Array.IndexOf(arrUserPermissions, Convert.ToInt16(Roles).ToString()) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public int SaveUser()
        {
            DBConnection dbCon = new DBConnection();
            try
            {
                int Id = 0;
                dbCon.BeginTransaction();
                string sql = "";

                
                if (this.UserId==0)
                {
                    sql = @" Insert into UserMaster 
                        (username, password, firstname, lastname, address1, address2, city, state, zip, phone, email, GroupPermissions, IndividualPermissions, status,UserRight) 
                        values 
                        ('" + this.Username.Replace("'", "''") + "', '" + Crypt.Encrypt(this.Password.Replace("'", "''")) + "', '" + this.FirstName.Replace("'", "''") + "', '" + this.LastName.Replace("'", "''") + "', '" + this.Address1.Replace("'", "''") + "', '" + this.Address2.Replace("'", "''") + "', '" + this.City.Replace("'", "''") + "', '" + this.State.Replace("'", "''") + "', '" + this.Zip.Replace("'", "''") + "', '" + this.Phone.Replace("'", "''") + "', '" + this.Email.Replace("'", "''") + "', '" + this.GrpPermissions.Replace("'", "''") + "', '" + this.IndPermissions.Replace("'", "''") + "',1,'1')";
                    Id = dbCon.ExecuteTransactionGetID(sql, CommandType.Text);
                    dbCon.CommitTransaction();
                    return Id;
                }
                else
                {
                    sql = @"update UserMaster set password = '" + Crypt.Encrypt(this.Password.Replace("'", "''")) + "', firstname = '" + this.FirstName.Replace("'", "''") + "', lastname = '" + this.LastName.Replace("'", "''") + "', address1 = '" + this.Address1.Replace("'", "''") + "', address2 = '" + this.Address2.Replace("'", "''") + "', city = '" + this.City.Replace("'", "''") + "', state = '" + this.State.Replace("'", "''") + "', zip = '" + this.Zip.Replace("'", "''") + "', phone = '" + this.Phone.Replace("'", "''") + "', email = '" + this.Phone.Replace("'", "''") + "', GroupPermissions = '" + this.GrpPermissions.Replace("'", "''") + "', IndividualPermissions = '" + this.IndPermissions.Replace("'", "''") + "' where UserId = " + this.UserId;
                    dbCon.ExecuteTransaction(sql, CommandType.Text);
                    dbCon.CommitTransaction();
                    return 0;
                }

            }
            catch (Exception ex)
            {
                dbCon.RollBackTransaction();
                return -1;
            }
        }

        public static bool IsUsernameExists(string Username)
        { 
            DBConnection dbCon = new DBConnection();
            string sql = "select count(1) from UserMaster where username = '" + Username.Replace("'", "''").Trim() + "'";
            if (Convert.ToInt16(dbCon.RetData(sql, CommandType.Text)) == 0)
                return false;
            else
                return true;
        }

        public DataTable GetUser(string Username)
        {
            DBConnection dbCon = new DBConnection();
            string sql = "select * from UserMaster where username = '" + Username.Replace("'", "''").Trim() + "'";
            DataTable dt = dbCon.ExecuteDataSet(sql, CommandType.Text).Tables[0];
            dbCon = null;
            return dt;
        }
        public DataTable GetUsers()
        {
            DBConnection dbCon = new DBConnection();
            string sql = "select Username, concat(FirstName, ' ', LastName) as Name, IndividualPermissions, GroupPermissions from UserMaster order by Username ";
            DataTable dt = dbCon.ExecuteDataSet(sql, CommandType.Text).Tables[0];
            //, (select group_concat(GroupName separator ',') from GroupPermissions where GroupId in UserMaster.GroupPermissions) as Grp
            DataTable dtUsers = new DataTable();
            dtUsers.Columns.Add(new DataColumn("Username"));
            dtUsers.Columns.Add(new DataColumn("Name"));
            dtUsers.Columns.Add(new DataColumn("IndPermissions"));
            dtUsers.Columns.Add(new DataColumn("GrpPermissions"));

            string IndPermissions = "";
            string GrpPermissions = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IndPermissions = "";
                    GrpPermissions = "";

                    if (dt.Rows[i]["IndividualPermissions"].ToString() != "")
                    {
                        sql = "select group_concat(IndivPermissionName separator ', ') from IndividualPermissions where IndivPermissionId in (" + dt.Rows[i]["IndividualPermissions"].ToString() + ") order by IndivPermissionName ";
                        IndPermissions = dbCon.RetData(sql, CommandType.Text);
                    }

                    if (dt.Rows[i]["GroupPermissions"].ToString() != "")
                    {
                        sql = "select group_concat(GroupName separator ', ') from GroupPermissions where GroupId in (" + dt.Rows[i]["GroupPermissions"].ToString() + ") order by GroupName ";
                        GrpPermissions = dbCon.RetData(sql, CommandType.Text);
                    }

                    DataRow dr = dtUsers.NewRow();
                    dr["Username"] = dt.Rows[i]["Username"].ToString();
                    dr["Name"] = dt.Rows[i]["Name"].ToString();
                    dr["IndPermissions"] = IndPermissions;
                    dr["GrpPermissions"] = GrpPermissions;
                    dtUsers.Rows.Add(dr);
                }
            }
            
            dbCon = null;
            return dtUsers;
        }
    }
}