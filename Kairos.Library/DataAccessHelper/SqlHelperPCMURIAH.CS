using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kairos.Library.DataAccessHelper
{
    public class SqlHelperPCMURIAH
    {
        SqlConnection conn;
        //string strConn = ConfigurationManager.ConnectionStrings["DB_GAAS"].ConnectionString;
        private string strConn;

        public SqlHelperPCMURIAH(string _strConn)
        {
            strConn = _strConn;
        }

        public DataTable GetData(string sp, Hashtable hash)
        {
            DataTable dt = new DataTable("Hasil");

            if (conn == null)
                conn = new SqlConnection(strConn);
            try
            {
                conn.Open();
                SqlDataAdapter adp = new SqlDataAdapter(sp, conn);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                foreach (DictionaryEntry entry in hash)
                {
                    string[] temp = entry.Key.ToString().Split('-');
                    if (temp[1].ToLower() == "char")
                    {
                        adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.Char, int.Parse(temp[2])).Value = entry.Value.ToString();
                    }
                    else if (temp[1].ToLower() == "nchar")
                    {
                        adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.NChar, int.Parse(temp[2])).Value = entry.Value.ToString();
                    }
                    else if (temp[1].ToLower() == "varchar")
                    {
                        if (temp[2].ToLower() == "max")
                        {
                            adp.SelectCommand.Parameters.AddWithValue(temp[0], entry.Value.ToString());
                        }
                        else
                        {
                            adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.VarChar, int.Parse(temp[2])).Value = entry.Value.ToString();
                        }
                    }
                    else if (temp[1].ToLower() == "nvarchar")
                    {
                        if (temp[2].ToLower() == "max")
                        {
                            adp.SelectCommand.Parameters.AddWithValue(temp[0], entry.Value.ToString());
                        }
                        else
                        {
                            adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.NVarChar, int.Parse(temp[2])).Value = entry.Value.ToString();
                        }
                    }
                    else if (temp[1].ToLower() == "int")
                    {
                        adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.Int).Value = int.Parse(entry.Value.ToString());
                    }
                    else if (temp[1].ToLower() == "decimal")
                    {
                        if (entry.Value == DBNull.Value)
                            adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.Decimal).Value = DBNull.Value;
                        else
                            adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.Decimal).Value = decimal.Parse(entry.Value.ToString());
                    }
                    else if (temp[1].ToLower() == "date")
                    {
                        adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.Date).Value = DateTime.Parse(entry.Value.ToString());
                    }
                    else if (temp[1].ToLower() == "datetime")
                    {
                        if (entry.Value == DBNull.Value)
                            adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.DateTime).Value = DBNull.Value;
                        else
                            adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.DateTime).Value = DateTime.Parse(entry.Value.ToString());
                    }
                    else if (temp[1].ToLower() == "bit")
                    {
                        adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.Bit).Value = entry.Value.ToString();
                    }
                    else if (temp[1].ToLower() == "smalldatetime")
                    {
                        adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.SmallDateTime).Value = DateTime.Parse(entry.Value.ToString());
                    }
                    else if (temp[1].ToLower() == "time")
                    {
                        adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.Time).Value = entry.Value.ToString();
                    }
                    else if (temp[1].ToLower() == "table")
                    {
                        adp.SelectCommand.Parameters.Add(temp[0].ToString(), SqlDbType.Structured).Value = entry.Value;
                    }
                    else if (temp[1].ToLower() == "binary")
                    {
                        adp.SelectCommand.Parameters.Add(temp[0].ToString(), SqlDbType.Binary, int.Parse(temp[2])).Value = entry.Value;
                    }
                    else if (temp[1].ToLower() == "uniqueidentifier")
                    {
                        adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.UniqueIdentifier).Value = entry.Value.ToString();
                    }
                }

                adp.Fill(dt);
            }
            catch (Exception error)
            {
                throw error;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }

            return dt;
        }

        public void ExecNonQuery(string sp, Hashtable hash)
        {
            if (conn == null)
                conn = new SqlConnection(strConn);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sp, conn) { CommandType = CommandType.StoredProcedure };
                foreach (DictionaryEntry entry in hash)
                {
                    string[] temp = entry.Key.ToString().Split('-');
                    if (temp[1].ToLower() == "char")
                    {
                        cmd.Parameters.Add(temp[0], SqlDbType.Char, int.Parse(temp[2])).Value = entry.Value.ToString();
                    }
                    else if (temp[1].ToLower() == "nchar")
                    {
                        cmd.Parameters.Add(temp[0], SqlDbType.NChar, int.Parse(temp[2])).Value = entry.Value.ToString();
                    }
                    else if (temp[1].ToLower() == "table")
                    {
                        cmd.Parameters.AddWithValue(temp[0].ToString(), entry.Value);
                    }
                    else if (temp[1].ToLower() == "varchar")
                    {
                        if (temp[2].ToLower() == "max")
                        {
                            cmd.Parameters.AddWithValue(temp[0], entry.Value.ToString());
                        }
                        else
                        {
                            cmd.Parameters.Add(temp[0], SqlDbType.VarChar, int.Parse(temp[2])).Value = entry.Value.ToString();
                        }
                    }
                    else if (temp[1].ToLower() == "nvarchar")
                    {
                        if (temp[2].ToLower() == "max")
                        {
                            cmd.Parameters.AddWithValue(temp[0], entry.Value.ToString());
                        }
                        else
                        {
                            cmd.Parameters.Add(temp[0], SqlDbType.NVarChar, int.Parse(temp[2])).Value = entry.Value.ToString();
                        }
                    }
                    else if (temp[1].ToLower() == "int")
                    {
                        cmd.Parameters.Add(temp[0], SqlDbType.Int).Value = int.Parse(entry.Value.ToString());
                    }
                    else if (temp[1].ToLower() == "decimal")
                    {
                        if (entry.Value == DBNull.Value)
                            cmd.Parameters.Add(temp[0], SqlDbType.Decimal).Value = DBNull.Value;
                        else
                            cmd.Parameters.Add(temp[0], SqlDbType.Decimal).Value = decimal.Parse(entry.Value.ToString());
                    }
                    else if (temp[1].ToLower() == "date")
                    {
                        cmd.Parameters.Add(temp[0], SqlDbType.Date).Value = DateTime.Parse(entry.Value.ToString());
                    }
                    else if (temp[1].ToLower() == "datetime")
                    {
                        if (entry.Value == DBNull.Value)
                            cmd.Parameters.Add(temp[0], SqlDbType.DateTime).Value = DBNull.Value;
                        else
                            cmd.Parameters.Add(temp[0], SqlDbType.DateTime).Value = DateTime.Parse(entry.Value.ToString());
                    }
                    else if (temp[1].ToLower() == "datetimenull")
                    {
                        cmd.Parameters.Add(temp[0], SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else if (temp[1].ToLower() == "bit")
                    {
                        cmd.Parameters.Add(temp[0], SqlDbType.Bit).Value = entry.Value.ToString();
                    }
                    else if (temp[1].ToLower() == "smalldatetime")
                    {
                        cmd.Parameters.Add(temp[0], SqlDbType.SmallDateTime).Value = entry.Value.ToString();
                    }
                    else if (temp[1].ToLower() == "time")
                    {
                        cmd.Parameters.Add(temp[0], SqlDbType.Time).Value = entry.Value.ToString();
                    }
                    else if (temp[1].ToLower() == "binary")
                    {
                        cmd.Parameters.Add(temp[0].ToString(), SqlDbType.Binary, int.Parse(temp[2])).Value = entry.Value;
                    }
                    else if (temp[1].ToLower() == "uniqueidentifier")
                    {
                        cmd.Parameters.Add(temp[0], SqlDbType.UniqueIdentifier).Value = entry.Value.ToString();
                    }

                }
                cmd.ExecuteNonQuery();
            }
            catch (Exception error)
            {
                throw error;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public DataSet ExecuteBulkQ(string proc, Hashtable hashtab)
        {
            DataSet data = new DataSet("hasil");
            if (conn == null)
            {
                conn = new SqlConnection(strConn);
            }
            try
            {
                conn.Open();
                SqlDataAdapter adp = new SqlDataAdapter(proc, conn);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;

                foreach (DictionaryEntry entry in hashtab)
                {
                    string[] temp = entry.Key.ToString().Split('-');
                    if (temp[1].ToLower() == "char")
                    {
                        adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.Char, int.Parse(temp[2])).Value = entry.Value.ToString();
                    }
                    else if (temp[1].ToLower() == "nchar")
                    {
                        adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.NChar, int.Parse(temp[2])).Value = entry.Value.ToString();
                    }
                    else if (temp[1].ToLower() == "varchar")
                    {
                        if (temp[2].ToLower() == "max")
                        {
                            adp.SelectCommand.Parameters.AddWithValue(temp[0], entry.Value.ToString());
                        }
                        else
                        {
                            adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.VarChar, int.Parse(temp[2])).Value = entry.Value.ToString();
                        }
                    }
                    else if (temp[1].ToLower() == "nvarchar")
                    {
                        adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.NVarChar, int.Parse(temp[2])).Value = entry.Value.ToString();
                    }
                    else if (temp[1].ToLower() == "int")
                    {
                        adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.Int).Value = int.Parse(entry.Value.ToString());
                    }
                    else if (temp[1].ToLower() == "decimal")
                    {
                        adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.Decimal).Value = decimal.Parse(entry.Value.ToString());
                    }
                    else if (temp[1].ToLower() == "date")
                    {
                        adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.Date).Value = DateTime.Parse(entry.Value.ToString());
                    }
                    else if (temp[1].ToLower() == "datetime")
                    {
                        adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.DateTime).Value = DateTime.Parse(entry.Value.ToString());
                    }
                    else if (temp[1].ToLower() == "bit")
                    {
                        adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.Bit).Value = entry.Value.ToString();
                    }
                    else if (temp[1].ToLower() == "smalldatetime")
                    {
                        adp.SelectCommand.Parameters.Add(temp[0], SqlDbType.SmallDateTime).Value = DateTime.Parse(entry.Value.ToString());
                    }
                    else if (temp[1].ToLower() == "binary")
                    {
                        adp.SelectCommand.Parameters.Add(temp[0].ToString(), SqlDbType.Binary, int.Parse(temp[2])).Value = entry.Value;
                    }
                }
                adp.Fill(data);
            }
            catch (Exception error)
            {
                throw error;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return data;
        }

        public string ExecScalar(string sp, Hashtable hash)
        {
            if (conn == null)
                conn = new SqlConnection(strConn);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sp, conn) { CommandType = CommandType.StoredProcedure };
                foreach (DictionaryEntry entry in hash)
                {
                    cmd.Parameters.AddWithValue(entry.Key.ToString(), entry.Value);
                }
                object ret = cmd.ExecuteScalar();

                if (ret == null)
                {
                    return ret.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception error)
            {
                throw error;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public void BulkData(DataTable DTable, string TableName)
        {
            if (conn == null)
            { conn = new SqlConnection(strConn); }
            try
            {
                SqlBulkCopy bulkInsert = new SqlBulkCopy(conn);
                conn.Open();
                bulkInsert.DestinationTableName = TableName;
                bulkInsert.WriteToServer(DTable);
            }
            catch (Exception error)
            { throw error; }
            finally
            {
                if (conn.State == ConnectionState.Open)
                { conn.Close(); }
            }
        }
    }
}
