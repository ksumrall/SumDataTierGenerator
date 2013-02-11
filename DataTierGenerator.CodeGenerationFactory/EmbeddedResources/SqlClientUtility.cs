using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Transactions;

namespace #ROOT_NAMESPACE#
{
    /// <summary>
    /// Provides helper functions for working with the <b>System.Data.SqlClient</b> namespace.
    /// </summary>
    public static class SqlClientUtility
    {
        /// <summary>
        /// Executes the stored procedure with the specified parameters.
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string to use.</param>
        /// <param name="commandType">The command type.</param>
        /// <param name="commandText">The stored procedure to execute.</param>
        /// <param name="parameters">The parameters of the stored procedure.</param>
        public static void ExecuteNonQuery(string connectionStringName, CommandType commandType, string commandText, params SqlParameter[] parameters)
        {
            if (Transaction.Current == null)
            {
                string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = CreateCommand(connection, commandType, commandText, parameters))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                SqlConnection connection = GetTransactionSqlConnection(connectionStringName);
                using (SqlCommand command = CreateCommand(connection, commandType, commandText, parameters))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Executes the stored procedure with the specified parameters and returns the result as a <see cref="SqlDataReader"/>.
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string to use.</param>
        /// <param name="commandType">The command type.</param>
        /// <param name="commandText">The stored procedure to execute.</param>
        /// <param name="parameters">The parameters of the stored procedure.</param>
        /// <returns>A <see cref="SqlDataReader"/> containing the results of the stored procedure execution.</returns>
        public static SqlDataReader ExecuteReader(string connectionStringName, CommandType commandType, string commandText, params SqlParameter[] parameters)
        {
            if (Transaction.Current == null)
            {
                string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
                SqlConnection connection = new SqlConnection(connectionString);

                using (SqlCommand command = CreateCommand(connection, commandType, commandText, parameters))
                {
                    return command.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            else
            {
                SqlConnection connection = GetTransactionSqlConnection(connectionStringName);
                using (SqlCommand command = CreateCommand(connection, commandType, commandText, parameters))
                {
                    return command.ExecuteReader();
                }
            }
        }

        /// <summary>
        /// Executes the stored procedure with the specified parameters, and returns the first column of the first row in the result set returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string to use.</param>
        /// <param name="commandType">The command type.</param>
        /// <param name="commandText">The stored procedure to execute.</param>
        /// <param name="parameters">The parameters of the stored procedure.</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public static object ExecuteScalar(string connectionStringName, CommandType commandType, string commandText, params SqlParameter[] parameters)
        {
            if (Transaction.Current == null)
            {
                string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
                SqlConnection connection = new SqlConnection(connectionString);

                using (SqlCommand command = CreateCommand(connection, commandType, commandText, parameters))
                {
                    return command.ExecuteScalar();
                }
            }
            else
            {
                SqlConnection connection = GetTransactionSqlConnection(connectionStringName);
                using (SqlCommand command = CreateCommand(connection, commandType, commandText, parameters))
                {
                    return command.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Executes the stored procedure and returns the result as a <see cref="DataSet"/>.
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string to use.</param>
        /// <param name="commandType">The command type.</param>
        /// <param name="commandText">The stored procedure to execute.</param>
        /// <param name="parameters">The parameters of the stored procedure.</param>
        /// <returns>A <see cref="DataSet"/> containing the results of the stored procedure execution.</returns>
        public static DataSet ExecuteDataSet(string connectionStringName, CommandType commandType, string commandText, params SqlParameter[] parameters)
        {
            if (Transaction.Current == null)
            {
                string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = CreateCommand(connection, commandType, commandText, parameters))
                    {
                        return CreateDataSet(command);
                    }
                }
            }
            else
            {
                SqlConnection connection = GetTransactionSqlConnection(connectionStringName);
                using (SqlCommand command = CreateCommand(connection, commandType, commandText, parameters))
                {
                    return CreateDataSet(command);
                }
            }
        }

        /// <summary>
        /// Executes the stored procedure and returns the result as a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string to use.</param>
        /// <param name="commandType">The command type.</param>
        /// <param name="commandText">The stored procedure to execute.</param>
        /// <param name="parameters">The parameters of the stored procedure.</param>
        /// <returns>A <see cref="DataTable"/> containing the results of the stored procedure execution.</returns>
        public static DataTable ExecuteDataTable(string connectionStringName, CommandType commandType, string commandText, params SqlParameter[] parameters)
        {
            if (Transaction.Current == null)
            {
                string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = CreateCommand(connection, commandType, commandText, parameters))
                    {
                        return CreateDataTable(command);
                    }
                }
            }
            else
            {
                SqlConnection connection = GetTransactionSqlConnection(connectionStringName);
                using (SqlCommand command = CreateCommand(connection, commandType, commandText, parameters))
                {
                    return CreateDataTable(command);
                }
            }
        }

        /// <summary>
        /// Gets the SqlConnection to be used as part of the current transaction.
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string to use.</param>
        /// <returns>The SqlConnection associated with the current transaction.</returns>
        /// <remarks>If no SqlConnection exists, one will be created.</remarks>
        private static SqlConnection GetTransactionSqlConnection(string connectionStringName)
        {
            LocalDataStoreSlot connectionDictionarySlot = Thread.GetNamedDataSlot("ConnectionDictionary");
            Dictionary<string, SqlConnection> connectionDictionary = (Dictionary<string, SqlConnection>)Thread.GetData(connectionDictionarySlot);

            if (connectionDictionary == null)
            {
                connectionDictionary = new Dictionary<string, SqlConnection>();
                Thread.SetData(connectionDictionarySlot, connectionDictionary);
            }

            SqlConnection connection = null;

            if (connectionDictionary.ContainsKey(connectionStringName))
            {
                connection = connectionDictionary[connectionStringName];
            }
            else
            {
                string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
                connection = new SqlConnection(connectionString);
                connectionDictionary.Add(connectionStringName, connection);

                Transaction.Current.TransactionCompleted += new TransactionCompletedEventHandler(Current_TransactionCompleted);
            }

            return connection;
        }

        /// <summary>
        /// Event handler for when any transaction that have been enlisted are completed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The TransactionEventArgs that contains the event data.</param>
        /// <remarks>This event handler will attempt to close any participating SqlConnections in the transaction.</remarks>
        private static void Current_TransactionCompleted(object sender, TransactionEventArgs e)
        {
            LocalDataStoreSlot connectionDictionarySlot = Thread.GetNamedDataSlot("ConnectionDictionary");
            Dictionary<string, SqlConnection> connectionDictionary = (Dictionary<string, SqlConnection>)Thread.GetData(connectionDictionarySlot);
            if (connectionDictionary != null)
            {
                foreach (SqlConnection connection in connectionDictionary.Values)
                {
                    if (connection != null && connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }

                connectionDictionary.Clear();
            }

            Thread.FreeNamedDataSlot("ConnectionDictionary");
        }

        public static string ExecuteJson(string connectionStringName, CommandType commandType, string commandText, params SqlParameter[] parameters)
        {
            int count = 0;
            StringBuilder stringBuilder = new StringBuilder();
            SqlDataReader dataReader = ExecuteReader(connectionStringName, commandType, commandText, parameters);
            while (dataReader.Read())
            {
                stringBuilder.Append("{");
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    if (dataReader.GetFieldType(i) == typeof(DateTime))
                    {
                        stringBuilder.AppendFormat("\"{0}\":\"{1:u}\"", dataReader.GetName(i), dataReader.GetValue(i));
                    }
                    else
                    {
                        stringBuilder.AppendFormat("\"{0}\":\"{1}\"", dataReader.GetName(i), dataReader.GetValue(i));
                    }
                    if (i < (dataReader.FieldCount - 1))
                    {
                        stringBuilder.Append(",");
                    }
                }
                stringBuilder.Append("},");
                count++;
            }
            if (stringBuilder.Length > 1)
            {
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }
            if (count == 1)
            {
                return stringBuilder.ToString();
            }
            if (count > 1)
            {
                return ("[" + stringBuilder.ToString() + "]");
            }
            return null;
        }

        #region Utility functions

        /// <summary>
        /// Converts the specified value to <code>DBNull.Value</code> if it is <code>null</code>.
        /// </summary>
        /// <param name="value">The value that should be checked for <code>null</code>.</param>
        /// <returns>The original value if it is not null, otherwise <code>DBNull.Value</code>.</returns>
        private static object CheckValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            else
            {
                return value;
            }
        }

        #region CreateCommand

        /// <summary>
        /// Creates, initializes, and returns a <see cref="SqlCommand"/> instance.
        /// </summary>
        /// <param name="connection">The <see cref="SqlConnection"/> the <see cref="SqlCommand"/> should be executed on.</param>
        /// <param name="commandType">The command type.</param>
        /// <param name="commandText">The name of the stored procedure to execute.</param>
        /// <returns>An initialized <see cref="SqlCommand"/> instance.</returns>
        private static SqlCommand CreateCommand(SqlConnection connection, CommandType commandType, string commandText)
        {
            if (connection != null && connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = commandText;
            command.CommandType = commandType;
            return command;
        }

        /// <summary>
        /// Creates, initializes, and returns a <see cref="SqlCommand"/> instance.
        /// </summary>
        /// <param name="connection">The <see cref="SqlConnection"/> the <see cref="SqlCommand"/> should be executed on.</param>
        /// <param name="commandType">The command type.</param>
        /// <param name="commandText">The name of the stored procedure to execute.</param>
        /// <param name="parameters">The parameters of the stored procedure.</param>
        /// <returns>An initialized <see cref="SqlCommand"/> instance.</returns>
        private static SqlCommand CreateCommand(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] parameters)
        {
            if (connection != null && connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = commandText;
            command.CommandType = commandType;

            if (parameters != null)
            {
                // Append each parameter to the command
                foreach (SqlParameter parameter in parameters)
                {
                    parameter.Value = CheckValue(parameter.Value);
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }

        #endregion CreateCommand

        private static DataSet CreateDataSet(SqlCommand command)
        {
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
            {
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                return dataSet;
            }
        }

        private static DataTable CreateDataTable(SqlCommand command)
        {
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
            {
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        #endregion

        #region Exception functions

        /// <summary>
        /// Determines if the specified exception is the result of a foreign key violation.
        /// </summary>
        /// <param name="e">The exception to check.</param>
        /// <returns><code>true</code> if the exception is a foreign key violation, otherwise <code>false</code>.</returns>
        public static bool IsForeignKeyContraintException(Exception e)
        {
            SqlException sqlex = e as SqlException;
            if (sqlex != null && sqlex.Number == 547)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines if the specified exception is the result of a unique constraint violation.
        /// </summary>
        /// <param name="e">The exception to check.</param>
        /// <returns><code>true</code> if the exception is a unique constraint violation, otherwise <code>false</code>.</returns>
        public static bool IsUniqueConstraintException(Exception e)
        {
            SqlException sqlex = e as SqlException;
            if (sqlex != null && (sqlex.Number == 2627 || sqlex.Number == 2601))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region DataSet and DataTable functions

        /// <summary>
        /// Returns the first (and only) DataTable found in the DataSet.
        /// </summary>
        /// <param name="dataSet">The DataSet to retrieve the DataTable from.</param>
        /// <returns>The first (and only) DataTable found in the DataSet, otherwise null.</returns>
        public static DataTable GetDataTable(DataSet dataSet)
        {
            if (dataSet != null && dataSet.Tables.Count == 1)
            {
                return dataSet.Tables[0];
            }

            return null;
        }

        /// <summary>
        /// Returns the first (and only) DataRow found in the DataTable.
        /// </summary>
        /// <param name="dataTable">The DataTable to retrieve the DataRow from.</param>
        /// <returns>The first (and only) DataRow found in the DataTable, otherwise null.</returns>
        public static DataRow GetDataRow(DataTable dataTable)
        {
            if (dataTable != null && dataTable.Rows.Count == 1)
            {
                return dataTable.Rows[0];
            }

            return null;
        }

        /// <summary>
        /// Returns the first (and only) DataRow found in the DataSet.
        /// </summary>
        /// <param name="dataSet">The DataSet to retrieve the DataRow from.</param>
        /// <returns>The first (and only) DataRow found in the DataSet, otherwise null.</returns>
        public static DataRow GetDataRow(DataSet dataSet)
        {
            DataTable dataTable = GetDataTable(dataSet);
            return GetDataRow(dataTable);
        }

        #endregion

        #region Field functions

        /// <summary>
        /// Attempts to extract the requested column value from a DataRow.
        /// </summary>
        /// <param name="dataRow">The DataRow to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise null.</returns>
        public static bool GetBoolean(DataRow dataRow, string columnName, bool valueIfNull)
        {
            object value = GetObject(dataRow, columnName, null);
            if (value != null)
            {
                if (value is bool)
                {
                    return (bool)value;
                }
                else if (value is byte)
                {
                    byte byteValue = (byte)value;
                    if (byteValue == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return Boolean.Parse(value.ToString());
                }
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a SqlDataReader.
        /// </summary>
        /// <param name="dataReader">The SqlDataReader to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise null.</returns>
        public static bool GetBoolean(SqlDataReader dataReader, string columnName, bool valueIfNull)
        {
            object value = GetObject(dataReader, columnName, valueIfNull);
            if (value != null)
            {
                if (value is bool)
                {
                    return (bool)value;
                }
                else if (value is byte)
                {
                    byte byteValue = (byte)value;
                    if (byteValue == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return Boolean.Parse(value.ToString());
                }
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a DataRow.
        /// </summary>
        /// <param name="dataRow">The DataRow to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise the value specified by <code>valueIfNull</code>.</returns>
        public static byte GetByte(DataRow dataRow, string columnName, byte valueIfNull)
        {
            object value = GetObject(dataRow, columnName, null);
            if (value != null)
            {
                if (value is byte)
                {
                    return (byte)value;
                }
                else
                {
                    return Byte.Parse(value.ToString());
                }
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a SqlDataReader.
        /// </summary>
        /// <param name="dataReader">The SqlDataReader to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise the value specified by <code>valueIfNull</code>.</returns>
        public static byte GetByte(SqlDataReader dataReader, string columnName, byte valueIfNull)
        {
            object value = GetObject(dataReader, columnName, null);
            if (value != null)
            {
                if (value is byte)
                {
                    return (byte)value;
                }
                else
                {
                    return Byte.Parse(value.ToString());
                }
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a DataRow.
        /// </summary>
        /// <param name="dataRow">The DataRow to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise the value specified by <code>valueIfNull</code>.</returns>
        public static byte[] GetBytes(DataRow dataRow, string columnName, byte[] valueIfNull)
        {
            object value = GetObject(dataRow, columnName, null);
            if (value != null && value is byte[])
            {
                return (byte[])value;
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a SqlDataReader.
        /// </summary>
        /// <param name="dataReader">The SqlDataReader to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise the value specified by <code>valueIfNull</code>.</returns>
        public static byte[] GetBytes(SqlDataReader dataReader, string columnName, byte[] valueIfNull)
        {
            object value = GetObject(dataReader, columnName, null);
            if (value != null && value is byte[])
            {
                return (byte[])value;
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a DataRow.
        /// </summary>
        /// <param name="dataRow">The DataRow to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise null.</returns>
        public static DateTime GetDateTime(DataRow dataRow, string columnName, DateTime valueIfNull)
        {
            object value = GetObject(dataRow, columnName, null);
            if (value != null)
            {
                if (value is DateTime)
                {
                    return (DateTime)value;
                }
                else
                {
                    return DateTime.Parse(value.ToString());
                }
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a SqlDataReader.
        /// </summary>
        /// <param name="dataReader">The SqlDataReader to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise null.</returns>
        public static DateTime GetDateTime(SqlDataReader dataReader, string columnName, DateTime valueIfNull)
        {
            object value = GetObject(dataReader, columnName, null);
            if (value != null)
            {
                if (value is DateTime)
                {
                    return (DateTime)value;
                }
                else
                {
                    return DateTime.Parse(value.ToString());
                }
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a DataRow.
        /// </summary>
        /// <param name="dataRow">The DataRow to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise null.</returns>
        public static decimal GetDecimal(DataRow dataRow, string columnName, decimal valueIfNull)
        {
            object value = GetObject(dataRow, columnName, null);
            if (value != null)
            {
                if (value is decimal)
                {
                    return (decimal)value;
                }
                else
                {
                    return Decimal.Parse(value.ToString());
                }
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a SqlDataReader.
        /// </summary>
        /// <param name="dataReader">The SqlDataReader to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise null.</returns>
        public static decimal GetDecimal(SqlDataReader dataReader, string columnName, decimal valueIfNull)
        {
            object value = GetObject(dataReader, columnName, null);
            if (value != null)
            {
                if (value is decimal)
                {
                    return (decimal)value;
                }
                else
                {
                    return Decimal.Parse(value.ToString());
                }
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a DataRow.
        /// </summary>
        /// <param name="dataRow">The DataRow to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise null.</returns>
        public static double GetDouble(DataRow dataRow, string columnName, double valueIfNull)
        {
            object value = GetObject(dataRow, columnName, null);
            if (value != null)
            {
                if (value is double)
                {
                    return (double)value;
                }
                else
                {
                    return Double.Parse(value.ToString());
                }
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a SqlDataReader.
        /// </summary>
        /// <param name="dataReader">The SqlDataReader to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise null.</returns>
        public static double GetDouble(SqlDataReader dataReader, string columnName, double valueIfNull)
        {
            object value = GetObject(dataReader, columnName, null);
            if (value != null)
            {
                if (value is double)
                {
                    return (double)value;
                }
                else
                {
                    return Double.Parse(value.ToString());
                }
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a DataRow.
        /// </summary>
        /// <param name="dataRow">The DataRow to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise null.</returns>
        public static Guid GetGuid(DataRow dataRow, string columnName, Guid valueIfNull)
        {
            object value = GetObject(dataRow, columnName, null);
            if (value != null)
            {
                if (value is Guid)
                {
                    return (Guid)value;
                }
                else
                {
                    return new Guid(value.ToString());
                }
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a SqlDataReader.
        /// </summary>
        /// <param name="dataReader">The SqlDataReader to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise null.</returns>
        public static Guid GetGuid(SqlDataReader dataReader, string columnName, Guid valueIfNull)
        {
            object value = GetObject(dataReader, columnName, null);
            if (value != null)
            {
                if (value is Guid)
                {
                    return (Guid)value;
                }
                else
                {
                    return new Guid(value.ToString());
                }
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a DataRow.
        /// </summary>
        /// <param name="dataRow">The DataRow to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise null.</returns>
        public static float GetSingle(DataRow dataRow, string columnName, float valueIfNull)
        {
            object value = GetObject(dataRow, columnName, null);
            if (value != null)
            {
                if (value is float)
                {
                    return (float)value;
                }
                else
                {
                    return Single.Parse(value.ToString());
                }
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a SqlDataReader.
        /// </summary>
        /// <param name="dataReader">The SqlDataReader to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise null.</returns>
        public static float GetSingle(SqlDataReader dataReader, string columnName, float valueIfNull)
        {
            object value = GetObject(dataReader, columnName, null);
            if (value != null)
            {
                if (value is float)
                {
                    return (float)value;
                }
                else
                {
                    return Single.Parse(value.ToString());
                }
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a DataRow.
        /// </summary>
        /// <param name="dataRow">The DataRow to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise null.</returns>
        public static short GetInt16(DataRow dataRow, string columnName, short valueIfNull)
        {
            object value = GetObject(dataRow, columnName, null);
            if (value != null)
            {
                if (value is short)
                {
                    return (short)value;
                }
                else
                {
                    return Int16.Parse(value.ToString());
                }
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a SqlDataReader.
        /// </summary>
        /// <param name="dataReader">The SqlDataReader to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise null.</returns>
        public static short GetInt16(SqlDataReader dataReader, string columnName, short valueIfNull)
        {
            object value = GetObject(dataReader, columnName, null);
            if (value != null)
            {
                if (value is short)
                {
                    return (short)value;
                }
                else
                {
                    return Int16.Parse(value.ToString());
                }
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a DataRow.
        /// </summary>
        /// <param name="dataRow">The DataRow to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise null.</returns>
        public static int GetInt32(DataRow dataRow, string columnName, int valueIfNull)
        {
            object value = GetObject(dataRow, columnName, null);
            if (value != null)
            {
                if (value is int)
                {
                    return (int)value;
                }
                else
                {
                    return Int32.Parse(value.ToString());
                }
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a SqlDataReader.
        /// </summary>
        /// <param name="dataReader">The SqlDataReader to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise null.</returns>
        public static int GetInt32(SqlDataReader dataReader, string columnName, int valueIfNull)
        {
            object value = GetObject(dataReader, columnName, null);
            if (value != null)
            {
                if (value is int)
                {
                    return (int)value;
                }
                else
                {
                    return Int32.Parse(value.ToString());
                }
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a DataRow.
        /// </summary>
        /// <param name="dataRow">The DataRow to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise null.</returns>
        public static long GetInt64(DataRow dataRow, string columnName, long valueIfNull)
        {
            object value = GetObject(dataRow, columnName, null);
            if (value != null)
            {
                if (value is long)
                {
                    return (long)value;
                }
                else
                {
                    return Int64.Parse(value.ToString());
                }
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a SqlDataReader.
        /// </summary>
        /// <param name="dataReader">The SqlDataReader to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise null.</returns>
        public static long GetInt64(SqlDataReader dataReader, string columnName, long valueIfNull)
        {
            object value = GetObject(dataReader, columnName, null);
            if (value != null)
            {
                if (value is long)
                {
                    return (long)value;
                }
                else
                {
                    return Int64.Parse(value.ToString());
                }
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a DataRow.
        /// </summary>
        /// <param name="dataRow">The DataRow to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise null.</returns>
        public static object GetObject(DataRow dataRow, string columnName, object valueIfNull)
        {
            object value = dataRow[columnName];
            if (value != null && value != DBNull.Value)
            {
                return value;
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a SqlDataReader.
        /// </summary>
        /// <param name="dataReader">The SqlDataReader to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise null.</returns>
        public static object GetObject(SqlDataReader dataReader, string columnName, object valueIfNull)
        {
            object value = dataReader[columnName];
            if (value != null && value != DBNull.Value)
            {
                return value;
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a DataRow.
        /// </summary>
        /// <param name="dataRow">The DataRow to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise null.</returns>
        public static string GetString(DataRow dataRow, string columnName, string valueIfNull)
        {
            object value = GetObject(dataRow, columnName, null);
            if (value != null)
            {
                if (value is string)
                {
                    return (string)value;
                }
                else
                {
                    return value.ToString();
                }
            }
            else
            {
                return valueIfNull;
            }
        }

        /// <summary>
        /// Attempts to extract the requested column value from a SqlDataReader.
        /// </summary>
        /// <param name="dataReader">The SqlDataReader to extract the column value from.</param>
        /// <param name="columnName">The name of the column to extract the value from.</param>
        /// <param name="valueIfNull">The value to return if the requested column value is null or DBNull.Value.</param>
        /// <returns>The value contained in the requested column if the value is not null or DBNull.Value, otherwise null.</returns>
        public static string GetString(SqlDataReader dataReader, string columnName, string valueIfNull)
        {
            object value = GetObject(dataReader, columnName, null);
            if (value != null)
            {
                if (value is string)
                {
                    return (string)value;
                }
                else
                {
                    return value.ToString();
                }
            }
            else
            {
                return valueIfNull;
            }
        }

        #endregion
    }
}