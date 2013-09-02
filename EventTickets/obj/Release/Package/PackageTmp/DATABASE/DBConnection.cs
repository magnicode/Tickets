using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Web;

namespace MYSQLDatabase
{
    public class DBConnection
    {
        public MySqlConnection Con;
        public MySqlCommand Cmd;
        public MySqlTransaction Trans;

        public string configPath;
         
        public DBConnection()
        {
            try
            {
                Con = new MySqlConnection();
                Cmd = new MySqlCommand();
                string sConStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ToString();
                configPath = sConStr;
                Con.ConnectionString = sConStr;                
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        ~DBConnection()
        {
            Con = null;
            Trans = null;
            Cmd = null;
            configPath = null;
        }       

        public void BeginTransaction()
        {
            try
            {
                this.OpenConnection();
                this.Trans = this.Con.BeginTransaction(IsolationLevel.ReadCommitted);

            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public int ExecuteTransaction(string CmdText, CommandType CmdType)
        {
            int i = 0;
            try
            {
                this.Cmd.CommandText = CmdText;
                this.Cmd.CommandType = CmdType;
                this.Cmd.Connection = this.Con;
                this.Cmd.Transaction = this.Trans;
                this.Cmd.CommandTimeout = 30;
                i = this.Cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
            }
            return i;
        }

        public int ExecuteTransactionGetID(string CmdText, CommandType CmdType)
        {
            int i = 0;
            try
            {
                this.Cmd.CommandText = CmdText + "; select last_insert_id();";
                this.Cmd.CommandType = CmdType;
                this.Cmd.Connection = this.Con;
                this.Cmd.Transaction = this.Trans;
                this.Cmd.CommandTimeout = 30;
                i = Convert.ToInt16(this.Cmd.ExecuteScalar());
            }
            catch (Exception e)
            {
                throw (e);
            }
            return i;
        }

        public void RollBackTransaction()
        {
            try
            {
                this.Trans.Rollback();
                this.CloseConnection();
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public void CommitTransaction()
        {
            try
            {
                this.Trans.Commit();
                this.CloseConnection();
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public void OpenConnection()
        {
            try
            {
                if (Con.State == ConnectionState.Open) Con.Close();
                Con.Open();
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (Con.State == ConnectionState.Open) Con.Close();
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public int Execute(string CmdText, CommandType CmdType)
        {
            int i = 0;
            try
            {
                this.Cmd.CommandText = CmdText;
                this.Cmd.CommandType = CmdType;
                this.Cmd.Connection = this.Con;
                this.OpenConnection();
                this.Cmd.CommandTimeout = 30;
                i = this.Cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
            catch (Exception e)
            {
                throw (e);
            }
            return i;
        }

        public int ExecuteContinuee(string CmdText, CommandType CmdType)
        {
            int i = 0;
            try
            {
                this.Cmd.CommandText = CmdText;
                this.Cmd.CommandType = CmdType;
                this.Cmd.Connection = this.Con;
                this.Cmd.CommandTimeout = 30;
                i = this.Cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
            }
            return i;
        }

        public string RetDataContinue(string CmdText, CommandType CmdType)
        {
            try
            {
                this.Cmd.CommandText = CmdText;
                this.Cmd.CommandType = CmdType;
                this.Cmd.Connection = this.Con;
                this.Cmd.CommandTimeout = 30;
                string strVal = "";
                strVal = "" + Cmd.ExecuteScalar();
                return strVal;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }
        public IDataReader ExecuteReader(string CmdText, CommandType CmdType)
        {
            try
            {
                MySqlDataReader dr;
                this.Cmd.CommandText = CmdText;
                this.Cmd.CommandType = CmdType;
                this.Cmd.Connection = this.Con;
                this.OpenConnection();
                this.Cmd.CommandTimeout = 300;
                dr = this.Cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return dr;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public IDataReader ExecuteTransactionReader(string CmdText, CommandType CmdType)
        {
            try
            {
                MySqlDataReader dr;
                this.Cmd.CommandText = CmdText;
                this.Cmd.CommandType = CmdType;
                this.Cmd.Transaction = this.Trans;
                this.Cmd.Connection = this.Con;
                this.Cmd.CommandTimeout = 300;
                dr = this.Cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return dr;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }
        
        public DataSet ExecuteDataSet(string CmdText, CommandType CmdType)
        {
            try
            {
                MySqlDataAdapter Adaptergrid;
                /*if (CmdType == CommandType.Text)
                {
                    Adaptergrid = new MySqlDataAdapter(CmdText, Con);
                    Adaptergrid.SelectCommand.CommandType = CmdType;
                }
                else*/
                {
                    Adaptergrid = new MySqlDataAdapter();
                    this.Cmd.CommandText = CmdText;
                    this.Cmd.CommandType = CmdType;
                    this.Cmd.Connection = this.Con;
                    Adaptergrid.SelectCommand = this.Cmd;
                }
                DataSet ds = new DataSet();

                this.OpenConnection();
                Adaptergrid.Fill(ds);
                this.CloseConnection();
                return ds;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public DataSet ExecuteContinueDataSet(string CmdText, CommandType CmdType)
        {
            try
            {
                MySqlDataAdapter Adaptergrid;
                if (CmdType == CommandType.Text)
                {
                    Adaptergrid = new MySqlDataAdapter(CmdText, Con);
                    Adaptergrid.SelectCommand.CommandType = CmdType;
                }
                else
                {
                    Adaptergrid = new MySqlDataAdapter();
                    this.Cmd.CommandText = CmdText;
                    this.Cmd.CommandType = CmdType;
                    this.Cmd.Connection = this.Con;
                    Adaptergrid.SelectCommand = this.Cmd;
                }
                DataSet ds = new DataSet();

                Adaptergrid.Fill(ds);
                return ds;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public DataSet ExecuteTransactionDataSet(string CmdText, CommandType CmdType)
        {
            try
            {
                MySqlDataAdapter Adaptergrid;
                if (CmdType == CommandType.Text)
                {
                    Adaptergrid = new MySqlDataAdapter(CmdText, this.Con);
                    Adaptergrid.SelectCommand.Transaction = this.Trans;
                    Adaptergrid.SelectCommand.CommandType = CmdType;
                }
                else
                {
                    Adaptergrid = new MySqlDataAdapter();
                    this.Cmd.CommandText = CmdText;
                    this.Cmd.CommandType = CmdType;
                    this.Cmd.Transaction = this.Trans;
                    this.Cmd.Connection = this.Con;
                    Adaptergrid.SelectCommand = this.Cmd;
                }
                DataSet ds = new DataSet();
                
                Adaptergrid.Fill(ds);                
                return ds;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public DataSet ExecutePagedDataSet(string CmdText, int intStart, int intMaxRec, string strTable, CommandType CmdType)
        {
            try
            {
                MySqlDataAdapter Adaptergrid;
                if (CmdType == CommandType.Text)
                {
                    Adaptergrid = new MySqlDataAdapter(CmdText, Con);
                    Adaptergrid.SelectCommand.CommandType = CmdType;
                }
                else
                {
                    Adaptergrid = new MySqlDataAdapter();
                    this.Cmd.CommandText = CmdText;
                    this.Cmd.CommandType = CmdType;
                    this.Cmd.Connection = this.Con;
                    Adaptergrid.SelectCommand = this.Cmd;
                }
                this.OpenConnection();
                DataSet ds = new DataSet();
                Adaptergrid.Fill(ds, intStart, intMaxRec, strTable);
                this.CloseConnection();
                return ds;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public string RetData(string CmdText, CommandType CmdType)
        {
            try
            {
                MySqlConnection ConRetData = new MySqlConnection();
                MySqlCommand Cmd = new MySqlCommand();
                ConRetData.ConnectionString = configPath;

                Cmd.CommandText = CmdText;
                Cmd.CommandType = CmdType;
                Cmd.Connection = ConRetData;

                string strVal = "";
                ConRetData.Open();
                strVal = "" + Cmd.ExecuteScalar();
                ConRetData.Close();
                ConRetData = null;
                return strVal;
            }
            catch (Exception e)
            {                
                throw (e);
            }
        }
        public bool isExits(string CmdText, CommandType CmdType)
        {
            try
            {
                MySqlConnection ConRetData = new MySqlConnection();
                MySqlCommand Cmd = new MySqlCommand();
                ConRetData.ConnectionString = configPath;

                Cmd.CommandText = CmdText;
                Cmd.CommandType = CmdType;
                Cmd.Connection = ConRetData;

                string strVal = "";
                ConRetData.Open();
                strVal = "" + Cmd.ExecuteScalar();
                ConRetData.Close();
                ConRetData = null;
                if (!string.IsNullOrEmpty(strVal))
                    return (true);
                else
                    return (false);
            }
            catch (Exception e)
            {
                throw (e);
                return (false);
            }
        }

        public void SetParameter(string strParmName,string strParamType,int intDirection,int intSize, string strValue)
        {
            try
            {

                MySqlDbType OType = MySqlDbType.VarChar;

                switch (strParamType.ToLower())
                {
                    case "blob":
                        OType = MySqlDbType.Binary;
                        break;
                    case "byte":
                        OType = MySqlDbType.Bit;
                        break;
                    case "char":
                        OType = MySqlDbType.VarChar;
                        break;
                    case "datetime":
                        OType = MySqlDbType.DateTime;
                        break;
                    case "double":
                        OType = MySqlDbType.Float;
                        break;
                    case "float":
                        OType = MySqlDbType.Float;
                        break;
                    case "int16":
                        OType = MySqlDbType.Int16;
                        break;
                    case "int":
                        OType = MySqlDbType.Int32;
                        break;
                    case "number":
                        OType = MySqlDbType.Int64;
                        break;
                    case "nvarchar":
                        OType = MySqlDbType.VarChar;
                        break;
                    case "varchar":
                        OType = MySqlDbType.VarChar;
                        break;
                }
                if (intDirection == 1)
                {
                    if (intSize > 0)
                    {
                        if (OType == MySqlDbType.Binary)
                            this.Cmd.Parameters.Add(strParmName, OType, intSize).Value = Convert.FromBase64String(strValue);
                        else
                            this.Cmd.Parameters.Add(strParmName, OType, intSize).Value = strValue;
                    }
                    else
                        this.Cmd.Parameters.Add(strParmName, OType).Value = strValue;
                }
                else
                {
                    if (intSize > 0)
                        this.Cmd.Parameters.Add(strParmName, OType,intSize);
                    else
                        this.Cmd.Parameters.Add(strParmName, OType);
                    this.Cmd.Parameters[strParmName].Direction = ParameterDirection.Output;
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public string GetParameter(string strParmName)
        {
            try
            {
                return this.Cmd.Parameters[strParmName].Value.ToString();
            }
            catch (Exception e)
            {
                
                throw(e);
            }
            
        }

        public void RemoveParameter(int intParIndex)
        {
            try
            {
                this.Cmd.Parameters.RemoveAt(intParIndex);
                //this.Cmd.Parameters.Remove(strParmName);                               
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public void ClearParameters()
        {
            try
            {
                this.Cmd.Parameters.Clear();
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public DataSet ExecutePagedDataSet(string CmdText, int intStart, int intMaxRec, string strTable, CommandType CmdType, out int totalRecords)
        {
            try
            {
                MySqlDataAdapter Adaptergrid;
                if (CmdType == CommandType.Text)
                {
                    Adaptergrid = new MySqlDataAdapter(CmdText, Con);
                    Adaptergrid.SelectCommand.CommandType = CmdType;
                }
                else
                {
                    Adaptergrid = new MySqlDataAdapter();
                    this.Cmd.CommandText = CmdText;
                    this.Cmd.CommandType = CmdType;
                    this.Cmd.Connection = this.Con;
                    Adaptergrid.SelectCommand = this.Cmd;
                }
                this.OpenConnection();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                Adaptergrid.Fill(ds, intStart, intMaxRec, strTable);
                Adaptergrid.Fill(dt);
                totalRecords = dt.Rows.Count;

                this.CloseConnection();
                return ds;
            }
            catch (Exception e)
            {
                totalRecords = 0;
                throw (e);
            }
        }               
    }
}