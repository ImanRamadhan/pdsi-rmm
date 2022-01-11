using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System;

namespace Kairos.Library.DataAccessHelper
{
    /// <summary>
    /// Get data from data base
    /// </summary>
    public class DbTransaction
    {
        /// <summary>
        /// return sql connection string 
        /// </summary>
        public string SqlConnectionString;

        public DbTransaction(string sqlConnectionString)
        {
            SqlConnectionString = sqlConnectionString;
        }

        #region DbExecute
        /// <summary>
        /// Execute date base to from sql query, 
        /// with parameter query string and 
        /// if query string is storage procedure
        /// </summary>
        /// <param name="Query">The key defining the data.</param>
        /// <param name="IsStorageProcedure">The data.</param>
        public void DbExecute(string Query, bool IsStorageProcedure)
        {
            using (var sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                if (!IsStorageProcedure)
                    sqlConnection.Execute(Query);
                else
                    sqlConnection.Execute(Query, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Execute date base to from sql query, 
        /// with  filtering parameter query string and 
        /// if query string is storage procedure
        /// </summary>
        public void DbExecute(string Query, object Parameter, bool IsStorageProcedure)
        {
            using (var sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                if (!IsStorageProcedure)
                    sqlConnection.Execute(Query, Parameter);
                else
                    sqlConnection.Execute(Query, Parameter, commandType: CommandType.StoredProcedure);

            }
        }
        #endregion


        #region DbToString
        /// <summary>
        /// Return System.string from date base using sql query, 
        /// with parameter query string and 
        /// if query string is storage procedure
        /// </summary>
        public string DbToString(string Query, bool IsStorageProcedure)
        {
            using (var sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                if (!IsStorageProcedure)
                    return sqlConnection.Query<string>(Query).FirstOrDefault();
                else
                    return sqlConnection.Query<string>(Query, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        /// <summary>
        /// Return System.string from date base using sql query, 
        /// with filtering parameter query string and 
        /// if query string is storage procedure
        /// </summary>
        public string DbToString(string Query, object Parameter, bool IsStorageProcedure)
        {
            using (var sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                if (!IsStorageProcedure)
                    return sqlConnection.Query<string>(Query, Parameter).FirstOrDefault();
                else
                    return sqlConnection.Query<string>(Query, Parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        #endregion


        #region DbToInt
        /// <summary>
        /// Return System.int from date base using sql query, 
        /// with parameter query string and 
        /// if query string is storage procedure
        /// </summary>
        public int DbToInt(string Query, bool IsStorageProcedure)
        {
            using (var sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                if (!IsStorageProcedure)
                    return sqlConnection.Query<int>(Query).FirstOrDefault();
                else
                    return sqlConnection.Query<int>(Query, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        /// <summary>
        /// Return System.int from date base using sql query, 
        /// with filtering parameter query string and 
        /// if query string is storage procedure
        /// </summary>
        public int DbToInt(string Query, object Parameter, bool IsStorageProcedure)
        {
            using (var sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                if (!IsStorageProcedure)
                    return sqlConnection.Query<int>(Query, Parameter).FirstOrDefault();
                else
                    return sqlConnection.Query<int>(Query, Parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        #endregion


        #region DbToList
        /// <summary>
        /// Return System.Collection.List<> from date base using sql query, 
        /// with parameter query string and 
        /// if query string is storage procedure
        /// </summary>
        public List<T> DbToList<T>(string Query, bool IsStorageProcedure)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                if (!IsStorageProcedure)
                    return sqlConnection.Query<T>(Query).ToList();
                else
                    return sqlConnection.Query<T>(Query, commandType: CommandType.StoredProcedure).ToList();
            }
        }


        //// <summary>
        /// Return System.Collection.List<> from date base using sql query, 
        /// with filtering parameter query string and 
        /// if query string is storage procedure
        /// </summary>
        public List<T> DbToList<T>(string Query, object Parameter, bool IsStorageProcedure)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                if (!IsStorageProcedure)
                    return sqlConnection.Query<T>(Query, Parameter).ToList();
                else
                    return sqlConnection.Query<T>(Query, Parameter, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        #endregion

        #region DbToDataSet
        public DataSet DbToDataSet(string Query, object Parameter)
        {
            return ExecuteDataSet(Query, ObjetToSqlParameter(Parameter));
        }

        private DataSet ExecuteDataSet(string Query, params SqlParameter[] arrParam)
        {
            DataSet ds = new DataSet();

            // Open the connection 
            using (SqlConnection sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                // Define the command 
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = sqlConnection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = Query;

                    // Handle the parameters
                    if (arrParam != null)
                        foreach (SqlParameter param in arrParam)
                            cmd.Parameters.Add(param);

                    // Define the data adapter and fill the dataset
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(ds);
                }
            }
            return ds;
        }
        #endregion

        #region DbToDataTable
        // Summary:
        //     This methode is not required because list object more faster than System.Data.DataTabe
        //     Gets the Pertamina.IP2P.Classes data for the current application's
        //     return data from sql query.
        //
        // Returns:
        //     Use DbToList<> by support by Dapper is required
        //     Returns a System.Data.DataTable object that
        //     contains the contents of the Pertamina.IP2P.Classes object
        //     for the current application's default connection.
        //
        // Exceptions:
        //   Pertamina.IP2P.Classes.DbToDataTableErrorsException:
        //     Could not retrieve a Pertamina.IP2P.Classes.DbTransaction object
        //     with the application settings data.
        public DataTable DbToDataTable(string Query, bool IsStorageProcedure)
        {
            return ExecuteDataTable(IsStorageProcedure, Query, ObjetToSqlParameter(new { }));
        }

        // Summary:
        //     This methode is not required because list object more faster than System.Data.DataTabe
        //     Gets the Pertamina.IP2P.Classes data for the current application's
        //     return data from sql storage procedure.
        //
        // Returns:
        //     Use DbToList<> by support by Dapper is required
        //     Returns a System.Data.DataTable object that
        //     contains the contents of the Pertamina.IP2P.Classes object
        //     for the current application's default connection.
        //
        // Exceptions:
        //   Pertamina.IP2P.Classes.DbToDataTableErrorsException:
        //     Could not retrieve a Pertamina.IP2P.Classes.DbTransaction object
        //     with the application settings data.
        public DataTable DbToDataTable(string Query, object Parameter, bool IsStorageProcedure)
        {
            return ExecuteDataTable(IsStorageProcedure, Query, ObjetToSqlParameter(Parameter));
        }

        private SqlParameter[] ObjetToSqlParameter(object dataObject)
        {
            Type type = dataObject.GetType();
            PropertyInfo[] props = type.GetProperties();
            List<SqlParameter> paramList = new List<SqlParameter>();

            for (int i = 0; i < props.Length; i++)
            {

                if (props[i].PropertyType.IsValueType || props[i].PropertyType.Name == "String" || props[i].PropertyType.Name == "Object")
                {
                    object fieldValue = type.InvokeMember(props[i].Name, BindingFlags.GetProperty, null, dataObject, null);
                    SqlParameter sqlParameter = new SqlParameter("@" + props[i].Name, fieldValue);
                    paramList.Add(sqlParameter);
                }
            }
            return paramList.ToArray();
        }

        private DataTable ExecuteDataTable(bool IsStorageProcedure, string Query, params SqlParameter[] arrParam)
        {
            DataTable dt = new DataTable();

            // Open the connection 
            using (SqlConnection sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                // Define the command 
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = sqlConnection;
                    if (IsStorageProcedure)
                        cmd.CommandType = CommandType.StoredProcedure;
                    else
                        cmd.CommandType = CommandType.Text;

                    cmd.CommandText = Query;

                    //// Handle the parameters 
                    if (arrParam != null)
                    {
                        foreach (SqlParameter param in arrParam)
                            cmd.Parameters.Add(param);
                    }

                    //cmd.Parameters.Add("MenuId", 1);

                    // Define the data adapter and fill the dataset 
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }
        #endregion


        #region DbToDynamic
        /// <summary>
        /// Return System.Collection.dynamic from date base using sql query, 
        /// with parameter query string and 
        /// if query string is storage procedure
        /// </summary>
        public dynamic DbToDynamic(string Query, bool IsStorageProcedure)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                if (!IsStorageProcedure)
                    return sqlConnection.Query<dynamic>(Query).ToList();
                else
                    return sqlConnection.Query<dynamic>(Query, commandType: CommandType.StoredProcedure).ToList();
            }
        }


        //// <summary>
        /// Return System.Collection.dynamic from date base using sql query, 
        /// with filtering parameter query string and 
        /// if query string is storage procedure
        /// </summary>
        public dynamic DbToDynamic(string Query, object Parameter, bool IsStorageProcedure)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                if (!IsStorageProcedure)
                    return sqlConnection.Query<dynamic>(Query, Parameter).ToList();
                else
                    return sqlConnection.Query<dynamic>(Query, Parameter, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        #endregion


        #region 'BulkCopy'
        public void BulkCopy(DataTable dt, string TableName, int IDForeignKey)
        {
            try
            {
                SqlBulkCopy bulkCopy = null;
                bulkCopy = new SqlBulkCopy(SqlConnectionString);
                bulkCopy.DestinationTableName = TableName;

                foreach (var column in dt.Columns)
                    bulkCopy.ColumnMappings.Add(column.ToString(), column.ToString());

                bulkCopy.WriteToServer(dt);
            }
            catch
            {
                throw;
            }

        }
        #endregion
    }

}
