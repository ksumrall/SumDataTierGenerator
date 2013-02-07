using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Text;

namespace #ROOT_NAMESPACE#
{

    public class SqlCeDatabase
    {
        private SqlCeConnection m_Connection;
        public SqlCeDatabase(string connectionString)
        {
            m_Connection = new SqlCeConnection();
            m_Connection.ConnectionString = connectionString;
        }

        public void Open()
        {
            if (m_Connection.State == ConnectionState.Closed)
            {
                m_Connection.Open();
            }
        }

        public void Close()
        {
            if (m_Connection.State == ConnectionState.Open)
            {
                m_Connection.Close();
            }
        }

        public void Dispose()
        {
            m_Connection.Close();
            m_Connection.Dispose();
        }

        public SqlCeCommand GetSqlStringCommand(string query)
        {
            SqlCeCommand cmd = new SqlCeCommand(query, GatewayHelper.Connection);
            cmd.Connection = m_Connection;
            return cmd;
        }

        public SqlCeResultSet ExecuteDataSet(SqlCeCommand cmd)
        {
            return ExecuteDataSet(cmd, true);
        }

        public SqlCeResultSet ExecuteDataSet(SqlCeCommand cmd, bool dispose)
        {
            Open();
            SqlCeResultSet rs = cmd.ExecuteResultSet(ResultSetOptions.Scrollable);
            if (dispose)
            {
                Dispose();
            }

            return rs;
        }

        public int ExecuteNonQuery(SqlCeCommand cmd)
        {
            return ExecuteNonQuery(cmd, true);
        }

        public int ExecuteNonQuery(SqlCeCommand cmd, bool dispose)
        {
            Open();
            int rtnVal = cmd.ExecuteNonQuery();
            if (dispose)
            {
                Dispose();
            }

            return rtnVal;
        }

        public Object ExecuteScalar(SqlCeCommand cmd)
        {
            return ExecuteScalar(cmd, true);
        }

        public Object ExecuteScalar(SqlCeCommand cmd, bool dispose)
        {
            Open();
            Object obj = cmd.ExecuteScalar();
            if (dispose)
            {
                Dispose();
            }

            return obj;
        }

        public SqlCeDataReader ExecuteReader(SqlCeCommand cmd)
        {
            Open();
            SqlCeDataReader rdr =
                cmd.ExecuteReader(CommandBehavior.CloseConnection);

            return rdr;
        }
    }

    public static class GatewayHelper
    {

        #region private / protected member variables

        private static char m_FieldPrefix = '[';
        private static char m_FieldSuffix = ']';

        private static readonly GatewaySettings m_Settings = new GatewaySettings();
        private static SqlCeConnection m_Connection = new SqlCeConnection();

        #endregion

        #region internal structure definitions

        public struct ParameterizedQuery
        {

            public string Query;
            public List<FieldValue> ParameterFieldValueList;

        }

        public class PreExecutionEventArgs : EventArgs
        {
            //public SqlCeDatabase SqlCeDatabase;
            public DbCommand Command;
        }

        public class GatewaySettings
        {
            public string ConnectionString;
            public int CommandTimeout;
        }

        #endregion

        #region properties

        static public GatewaySettings Settings
        {
            get
            {
                return m_Settings;
            }
        }

        static internal SqlCeConnection Connection
        {
            get
            {
                return m_Connection;
            }
        }

        static public string ConnectionString 
        {
            get
            {
                return m_Settings.ConnectionString;
            }
            set
            {
                m_Settings.ConnectionString = value;
                m_Connection.ConnectionString = value;
            }
        }

        static public char FieldPrefixCharacter
        {
            get
            {
                return m_FieldPrefix;
            }
            set
            {
                m_FieldPrefix = value;
            }
        }

        static public char FieldSuffixCharacter
        {
            get
            {
                return m_FieldSuffix;
            }
            set
            {
                m_FieldSuffix = value;
            }
        }

        #endregion

        #region query building routines

        #region create primary key array

        internal static FieldDefinition[] CreatePrimaryKeysList(FieldDefinition[] fieldDefinitions)
        {

            List<FieldDefinition> pkList = new List<FieldDefinition>();

            // iterate through list of fieldDefainitions
            for (int index = 0; index < fieldDefinitions.Length; index++)
            {

                // Is this field a PrimaryKey
                if (fieldDefinitions[index].IsPrimaryKey)
                {

                    // incrment primaryKeyCount
                    pkList.Add(fieldDefinitions[index]);
                }

            }

            // return value
            return pkList.ToArray();

        }

        /// <summary>
        /// This method creates a list of FieldDefinition objects that are primary keys.
        /// </summary>
        /// <param name="fieldDefinitions"></param>
        /// <returns></returns>
        internal static FieldDefinition[] CreatePrimaryKeysList(FieldValue[] fieldValues)
        {

            List<FieldDefinition> pkList = new List<FieldDefinition>();

            // iterate through list of fieldDefainitions
            for (int index = 0; index < fieldValues.Length; index++)
            {

                // Is this field a PrimaryKey
                if (fieldValues[index].FieldDefinition.IsPrimaryKey)
                {

                    // incrment primaryKeyCount
                    pkList.Add(fieldValues[index].FieldDefinition);
                }

            }

            // return value
            return pkList.ToArray();

        }

        internal static FieldDefinition[] CreatePrimaryKeysList(List<FieldValue> fieldValueList)
        {

            List<FieldDefinition> pkList = new List<FieldDefinition>();

            // iterate through list of fieldDefainitions
            for (int index = 0; index < fieldValueList.Count; index++)
            {

                // Is this field a PrimaryKey
                if (fieldValueList[index].FieldDefinition.IsPrimaryKey)
                {

                    // incrment primaryKeyCount
                    pkList.Add(fieldValueList[index].FieldDefinition);
                }

            }

            // return value
            return pkList.ToArray();

        }

        #endregion

        #region select clause query builder methods

        internal static string BuildSelectAllQuery(string tableName, FieldDefinition[] fieldDefinitions)
        {
            return BuildPrefixedSelectAllQuery(tableName, null, fieldDefinitions);
        }

        internal static string BuildDistinctPrefixedSelectAllQuery(string fullyQualifiedTableName, string prefix, FieldDefinition[] fieldDefinitions)
        {
            StringBuilder query = new StringBuilder();

            query.Append("SELECT DISTINCT ");

            if (String.IsNullOrEmpty(prefix))
            {
                prefix = "";
            }
            else
            {
                prefix += ".";
            }

            for (int index = 0; index < fieldDefinitions.Length; index++)
            {

                query.Append(prefix + m_FieldPrefix
                    + fieldDefinitions[index].SourceColumnName
                    + m_FieldSuffix + ", ");

            }

            query.Remove(query.Length - 2, 2);
            query.AppendLine();

            query.AppendLine("FROM "
                + fullyQualifiedTableName);

            return query.ToString();
        }

        internal static string BuildPrefixedSelectAllQuery(string tableName, string prefix, FieldDefinition[] fieldDefinitions)
        {

            StringBuilder query = new StringBuilder();

            query.Append("SELECT ");

            if (String.IsNullOrEmpty(prefix))
            {
                prefix = "";
            }
            else
            {
                prefix += ".";
            }

            for (int index = 0; index < fieldDefinitions.Length; index++)
            {

                query.Append(prefix + m_FieldPrefix
                    + fieldDefinitions[index].SourceColumnName
                    + m_FieldSuffix + ", ");

            }

            query.Remove(query.Length - 2, 2);
            query.AppendLine();

            query.AppendLine("FROM "
                + tableName);

            return query.ToString();

        }

        internal static string BuildSelectAllQuery(string tableName, IFieldValues fieldValues)
        {

            StringBuilder query = new StringBuilder();

            query.Append("SELECT ");

            for (int index = 0; index < fieldValues.FieldValues.Length; index++)
            {

                query.Append(m_FieldPrefix
                    + fieldValues.FieldValues[index].FieldDefinition.SourceColumnName
                    + m_FieldSuffix + ", ");

            }

            query.Remove(query.Length - 2, 2);
            query.AppendLine();

            query.AppendLine("FROM "
                + tableName);

            return query.ToString();

        }

        internal static string BuildSelectByPrimaryKeyQuery(string tableName, IFieldValues fieldValues)
        {

            string selectClause = BuildSelectAllQuery(tableName, fieldValues);
            string whereClause = BuildWhereClause(fieldValues.PrimaryKeyFieldValues);

            return selectClause + Environment.NewLine + whereClause;

        }

        internal static string BuildSelectAllWithOrderByFieldQuery(string tableName, FieldDefinition[] fieldDefinitions, FieldDefinition orderByField, bool ascending)
        {

            // Get root of BuildSelectAllQuery
            string query = BuildSelectAllQuery(tableName, fieldDefinitions);

            // create StringBuilder
            StringBuilder sb = new StringBuilder(query);

            // now append Order By Field
            sb.Append(" Order By ");

            // append FieldDefintion.Name
            sb.Append(orderByField.Name);

            // get direction
            string direction = " asc";

            // if descending
            if (!ascending)
            {
                direction = " desc";
            }

            // append direction
            sb.AppendLine(direction);

            // return value
            return sb.ToString();

        }

        internal static string BuildSelectByFieldsQuery(string tableName, FieldDefinition[] fieldDefinitions)
        {

            // create PrimaryKeyDefinitions
            FieldDefinition[] pkFieldDefinitions = CreatePrimaryKeysList(fieldDefinitions);

            return BuildSelectAllQuery(tableName, fieldDefinitions)
                    + BuildWhereClause(pkFieldDefinitions);

        }

        internal static string BuildSelectByFieldsQuery(string tableName, FieldDefinition[] selectFieldDefinitions, List<FieldValue> whereFieldValueList)
        {

            return BuildSelectAllQuery(tableName, selectFieldDefinitions)
                    + BuildWhereClause(whereFieldValueList);

        }

        #endregion

        #region where clause query builder methods

        internal static string BuildWhereClause(List<FieldValue> fieldValueList)
        {

            return BuildWhereClause(fieldValueList.ToArray());

        }

        internal static string BuildWhereClause(FieldValue[] fieldValueArray)
        {

            string whereClause = "";

            foreach (FieldValue fieldValue in fieldValueArray)
            {
                if (fieldValue.Value == null)
                {
                    whereClause += m_FieldPrefix + fieldValue.FieldDefinition.SourceColumnName + m_FieldSuffix
                        + " IS NULL And ";
                }
                else
                {
                    whereClause += m_FieldPrefix + fieldValue.FieldDefinition.SourceColumnName + m_FieldSuffix
                        + " = @" + fieldValue.FieldDefinition.Name + " And ";
                }
            }

            if (whereClause.Length > 0)
            {
                whereClause = "WHERE " + whereClause.Substring(0, whereClause.Length - 5);
            }

            return whereClause;

        }

        internal static string BuildWhereClause(FieldDefinition[] fieldDefinitions)
        {

            string whereClause = "WHERE ";

            for (int index = 0; index < fieldDefinitions.Length; index++)
            {

                // add this fieldDefinition to where clause.
                whereClause += m_FieldPrefix + fieldDefinitions[index].SourceColumnName + m_FieldSuffix
                    + " = @" + fieldDefinitions[index].Name + " And ";

            }

            if (whereClause.Length > 7)
            {
                whereClause = whereClause.Substring(0, whereClause.Length - 5);
            }
            else
            {
                whereClause = "";
            }

            return whereClause;

        }

        #endregion

        internal static string BuildOrderByClause(FieldDefinition[] fieldDefinitions)
        {

            string orderByClause = "";

            for (int index = 0; index < fieldDefinitions.Length; index++)
            {

                // add this fieldDefinition to where clause.
                orderByClause +=
                    m_FieldPrefix + fieldDefinitions[index].SourceColumnName + m_FieldSuffix + ", ";

            }

            if (orderByClause.Length > 0)
            {
                orderByClause =
                    "ORDER BY " + orderByClause.Substring(0, orderByClause.Length - 2);
            }

            return orderByClause;

        }

        #region insert query builder methods

        internal static ParameterizedQuery BuildInsertQuery(string tableName
            , IFieldValues fieldValues)
        {

            string insertQuery;
            string fields = " ( ";
            string values = " VALUES ( ";
            ParameterizedQuery parameterizedQuery;
            List<FieldValue> insertFieldList = new List<FieldValue>();
            List<FieldDefinition> pkDefinitions = new List<FieldDefinition>();

            #region build the insert portion of the query

            for (int index = 0; index < fieldValues.FieldValues.Length; index++)
            {

                if (!fieldValues.FieldValues[index].FieldDefinition.IsReadOnly)
                {
                    // skip fields that are null and have a default value
                    if (!(fieldValues.FieldValues[index].Value == null && fieldValues.FieldValues[index].FieldDefinition.HasDefault))
                    {
                        fields += m_FieldPrefix + fieldValues.FieldValues[index].FieldDefinition.SourceColumnName + m_FieldSuffix + ", ";
                        values += "@" + fieldValues.FieldValues[index].FieldDefinition.Name + ", ";
                        insertFieldList.Add(fieldValues.FieldValues[index]);
                    }

                }

            }

            fields = fields.Substring(0, fields.Length - 2);
            values = values.Substring(0, values.Length - 2);

            fields += " )";
            values += " )";

            insertQuery = "INSERT INTO "
                + tableName + Environment.NewLine
                + fields + Environment.NewLine
                + values;

            #endregion

            parameterizedQuery.Query = insertQuery;
            parameterizedQuery.ParameterFieldValueList = insertFieldList;

            return parameterizedQuery;

        }

        #endregion

        #region update query builder methods

        internal static string BuildUpdatePortion(string tableName, FieldValue[] fieldValues)
        {

            StringBuilder query = new StringBuilder();
            StringBuilder fields = new StringBuilder();

            #region add each dirty field

            if (fieldValues.Length > 0)
            {

                for (int index = 0; index < fieldValues.Length; index++)
                {
                    if (fieldValues[index].IsDirty)
                    {
                        fields.Append(m_FieldPrefix + fieldValues[index].FieldDefinition.SourceColumnName + m_FieldSuffix + " = @");
                        fields.AppendLine(fieldValues[index].FieldDefinition.Name + ",");
                    }
                }

            }

            #endregion

            if (fields.Length > 0)
            {

                // remove the last comma
                fields.Remove(fields.Length - 3, 3);

                query.Append("UPDATE ");
                query.Append(tableName);
                query.AppendLine(" SET ");

                query.AppendLine(fields.ToString());

            }

            return query.ToString();

        }

        internal static string BuildUpdateByPrimaryKeyQuery
            (string tableName, IFieldValues fieldValues)
        {

            StringBuilder query = new StringBuilder();

            if (fieldValues.PrimaryKeyFieldValues.Length == 0)
            {
                throw new ApplicationException("The Primary Key is missing on table '" + tableName + "'.");
            }

            // attach all the little pieces of the query
            query.Append(BuildUpdatePortion(tableName, fieldValues.FieldValues));
            query.AppendLine(BuildWhereClause(fieldValues.PrimaryKeyFieldValues));

            return query.ToString();

        }

        #endregion

        #region delete query builder methods

        internal static string BuildDeleteQuery(string tableName)
        {

            string query = "DELETE FROM " + tableName;

            return query;

        }

        internal static string BuildDeleteByPrimaryKeyQuery(string tableName, FieldDefinition[] fieldDefinitions)
        {

            FieldDefinition[] pkFieldDefinitionList = CreatePrimaryKeysList(fieldDefinitions);

            return BuildDeleteByFieldsQuery(tableName, pkFieldDefinitionList);

        }

        internal static string BuildDeleteByFieldsQuery(string tableName, FieldDefinition[] fieldDefinitions)
        {

            StringBuilder deleteQuery = new StringBuilder();

            deleteQuery.AppendLine(BuildDeleteQuery(tableName));

            // add the where condition
            if (fieldDefinitions != null && fieldDefinitions.Length > 0)
            {
                deleteQuery.AppendLine(BuildWhereClause(fieldDefinitions));
            }

            return deleteQuery.ToString();

        }

        internal static string BuildDeleteByFieldsQuery(string tableName, List<FieldValue> fieldValueList)
        {

            StringBuilder deleteQuery = new StringBuilder();

            deleteQuery.AppendLine(BuildDeleteQuery(tableName));

            // add the where condition
            if (fieldValueList != null && fieldValueList.Count > 0)
            {
                deleteQuery.AppendLine(BuildWhereClause(fieldValueList));
            }

            return deleteQuery.ToString();

        }

        #endregion

        #endregion

        #region query execution routines

        #region SqlCeResultSet

        internal static SqlCeResultSet ExecuteDataSetFromSql(string query)
        {

            return ExecuteDataSetFromSql(query, (FieldValue[])null);

        }

        internal static SqlCeResultSet ExecuteDataSetFromSql(string query, List<FieldValue> fieldValueList)
        {

            return ExecuteDataSetFromSql(query, fieldValueList.ToArray());

        }

        internal static SqlCeResultSet ExecuteDataSetFromSql(string query, FieldValue[] fieldValueArray)
        {

            SqlCeDatabase db;
            SqlCeResultSet dataSet = null;

            db = GetDatabase();

            dataSet = ExecuteDataSetFromSql(db, query, fieldValueArray);

            db.Close();
            db.Dispose();

            return dataSet;

        }

        internal static SqlCeResultSet ExecuteDataSetFromSql(SqlCeDatabase database, string query, FieldValue[] fieldValueArray)
        {

            SqlCeCommand dbCommand;
            SqlCeResultSet dataSet = null;

            dbCommand = database.GetSqlStringCommand(query);

            if (dbCommand != null)
            {
                try
                {

                    SetCommandProperties(dbCommand);

                    if (fieldValueArray != null && fieldValueArray.Length > 0)
                    {
                        dbCommand.Parameters.AddRange(GetParameters(fieldValueArray));
                    }

                    dataSet = database.ExecuteDataSet(dbCommand);

                }
                finally
                {
                    dbCommand.Dispose();
                }
            }

            return dataSet;

        }

        #endregion

        #region ExecuteNonQuery

        internal static int ExecuteNonQueryFromSql(string query)
        {

            return ExecuteNonQueryFromSql(query, (FieldValue[])null);
        }

        internal static int ExecuteNonQueryFromSql(string query, List<FieldValue> fieldValueList)
        {

            return ExecuteNonQueryFromSql(query, fieldValueList.ToArray());

        }

        internal static int ExecuteNonQueryFromSql(string query, FieldValue[] fieldValueArray)
        {

            SqlCeDatabase db;
            int rtnValue = -1;

            db = GetDatabase();

            rtnValue = ExecuteNonQueryFromSql(db, query, fieldValueArray);

            db.Close();
            db.Dispose();

            return rtnValue;

        }

        internal static int ExecuteNonQueryFromSql(SqlCeDatabase database, string query, FieldValue[] fieldValueArray)
        {
            SqlCeCommand dbCommand;
            int rtnValue = -1;

            dbCommand = database.GetSqlStringCommand(query);

            if (dbCommand != null)
            {
                try
                {

                    SetCommandProperties(dbCommand);

                    if (fieldValueArray != null && fieldValueArray.Length > 0)
                    {
                        dbCommand.Parameters.AddRange(GetParameters(fieldValueArray));
                    }

                    rtnValue = database.ExecuteNonQuery(dbCommand, false);

                }
                finally
                {
                    dbCommand.Dispose();
                }
            }

            return rtnValue;
        }

        #endregion

        #region ExecuteScalar

        internal static object ExecuteScalarFromSql(string query)
        {

            return ExecuteScalarFromSql(query, (FieldValue[])null);

        }

        internal static object ExecuteScalarFromSql(string query, List<FieldValue> fieldValueList)
        {

            return ExecuteScalarFromSql(query, fieldValueList.ToArray());

        }

        internal static object ExecuteScalarFromSql(string query, FieldValue[] fieldValueArray)
        {

            SqlCeDatabase db;
            Object obj = null;

            db = GetDatabase();

            obj = ExecuteScalarFromSql(db, query, fieldValueArray);

            db.Close();
            db.Dispose();

            return obj;

        }

        internal static object ExecuteScalarFromSql(SqlCeDatabase database, string query, FieldValue[] fieldValueArray)
        {

            SqlCeCommand dbCommand;
            Object obj = null;

            dbCommand = database.GetSqlStringCommand(query);

            if (dbCommand != null)
            {
                try
                {

                    SetCommandProperties(dbCommand);

                    if (fieldValueArray != null && fieldValueArray.Length > 0)
                    {
                        dbCommand.Parameters.AddRange(GetParameters(fieldValueArray));
                    }

                    obj = database.ExecuteScalar(dbCommand);
                    if (obj is DBNull)
                    {
                        obj = null;
                    }

                }
                finally
                {
                    dbCommand.Dispose();
                }
            }

            return obj;

        }

        #endregion

        #region ExecuteReader

        internal static IDataReader ExecuteReaderFromSql(string query)
        {

            return ExecuteReaderFromSql(query, (FieldValue[])null);

        }

        internal static IDataReader ExecuteReaderFromSql(string query, List<FieldValue> fieldValueList)
        {

            return ExecuteReaderFromSql(query, fieldValueList.ToArray());

        }

        internal static IDataReader ExecuteReaderFromSql(string query, FieldValue[] fieldValueArray)
        {

            SqlCeDatabase db;

            db = GetDatabase();
            return ExecuteReaderFromSql(db, query, fieldValueArray);

        }

        internal static IDataReader ExecuteReaderFromSql(SqlCeDatabase database, string query, FieldValue[] fieldValueArray)
        {

            SqlCeCommand dbCommand;
            IDataReader rdr = null;

            dbCommand = database.GetSqlStringCommand(query);

            if (dbCommand != null)
            {
                try
                {

                    SetCommandProperties(dbCommand);

                    if (fieldValueArray != null && fieldValueArray.Length > 0)
                    {
                        dbCommand.Parameters.AddRange(GetParameters(fieldValueArray));
                    }

                    rdr = database.ExecuteReader(dbCommand);

                }
                finally
                {
                    dbCommand.Dispose();
                }
            }

            return rdr;

        }

        #endregion

        #endregion

        #region private implementation

        private static SqlCeDatabase GetDatabase()
        {

            SqlCeDatabase db = null;

            if (!string.IsNullOrEmpty(m_Settings.ConnectionString))
            {
                db = new SqlCeDatabase(m_Settings.ConnectionString);
            }
            else
            {
                //db = DatabaseFactory.CreateDatabase();
            }

            return db;
        }

        private static void SetCommandProperties(DbCommand dbCommand)
        {

            if (m_Settings.CommandTimeout > 0)
            {
                dbCommand.CommandTimeout = m_Settings.CommandTimeout;
            }
        }

        private static SqlCeParameter[] GetParameters(List<FieldValue> fieldValueList)
        {

            return GetParameters(fieldValueList.ToArray());

        }

        private static SqlCeParameter[] GetParameters(FieldValue[] fieldValueArray)
        {

            List<SqlCeParameter> parameterList = new List<SqlCeParameter>();
            SqlCeParameter parameter;

            for (int index = 0; index < fieldValueArray.Length; index++)
            {

                parameter = new SqlCeParameter();

                parameter.Direction = ParameterDirection.Input;
                parameter.ParameterName = "@" + fieldValueArray[index].FieldDefinition.Name;
                parameter.SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType)
                    , fieldValueArray[index].FieldDefinition.DataTypeName, true);

                parameter.Size = fieldValueArray[index].FieldDefinition.SourceColumnMaxLength;
                parameter.Precision = fieldValueArray[index].FieldDefinition.SourceColumnPrecision;
                parameter.Scale = fieldValueArray[index].FieldDefinition.SourceColumnScale;

                if (fieldValueArray[index].Value == null)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = fieldValueArray[index].Value;
                    if (parameter.SqlDbType == SqlDbType.Text)
                    {
                        parameter.Size = fieldValueArray[index].Value.ToString().Length;
                    }
                }

                parameterList.Add(parameter);

            }

            return parameterList.ToArray();

        }

        #endregion

    }


}