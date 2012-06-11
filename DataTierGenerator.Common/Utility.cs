using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TotalSafety.DataTierGenerator.Common
{
    public sealed class Utility
    {
        private Utility()
        {}

        /// <summary>
        /// Creates the specified subdirectory, if it doesn't exist.
        /// </summary>
        /// <param name="name">The name of the subdirectory to be created.</param>
        /// <param name="deleteIfExists">Indicates if the directory should be deleted if it exists.</param>
        public static void CreateSubDirectory(string name, bool deleteIfExists)
        {
            if (Directory.Exists(name)) {
                Directory.Delete(name, true);
            }

            Directory.CreateDirectory(name);
        }

        /// <summary>
        /// Retrieves the specified manifest resource stream from the executing assembly as a string.
        /// </summary>
        /// <param name="name">Name of the resource to retrieve.</param>
        /// <returns>The value of the specified manifest resource.</returns>
        public static string GetResource(string name)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return GetResource(assembly, name);
        }

        /// <summary>
        /// Retrieves the specified manifest resource stream from the executing assembly as a string.
        /// </summary>
        /// <param name="name">Name of the resource to retrieve.</param>
        /// <returns>The value of the specified manifest resource.</returns>
        public static string GetResource(Assembly assembly, string name)
        {
            using (StreamReader streamReader =
                new StreamReader(assembly.GetManifestResourceStream(name)))
            {

                return streamReader.ReadToEnd();

            }
        }

        /// <summary>
        /// Retrieves the specified manifest resource stream from the executing assembly as a string, replacing the specified old value with the specified new value.
        /// </summary>
        /// <param name="name">Name of the resource to retrieve.</param>
        /// <param name="oldValue">A string to be replaced.</param>
        /// <param name="newValue">A string to replace all occurrences of oldValue.</param>
        /// <returns>The value of the specified manifest resource, with all instances of oldValue replaced with newValue.</returns>
        public static string GetResource(string name, string oldValue, string newValue)
        {
            string returnValue = GetResource(name);
            return returnValue.Replace(oldValue, newValue);
        }

        public static void SaveResourceFile(string resourceFileName, string fullPathName)
        {

            Assembly assembly = Assembly.GetExecutingAssembly();
            SaveResourceFile(assembly, resourceFileName, fullPathName);

        }

        public static void SaveResourceFile(Assembly assembly, string resourceFileName, string fullPathName)
        {
            using (FileStream fileStream = File.Create(fullPathName))
            {

                byte[] byteArray = new byte[1024];

                BinaryReader binaryReader = new BinaryReader(assembly.GetManifestResourceStream(resourceFileName));
                BinaryWriter binaryWriter = new BinaryWriter(fileStream);

                do
                {
                    byteArray = binaryReader.ReadBytes(1024);
                    binaryWriter.Write(byteArray);
                    //fileStream.Write(byteArray, 0, byteArray.Length);
                } while (byteArray.Length == 1024);

                binaryWriter.Flush();

                binaryWriter.Close();
                binaryReader.Close();
            }
        }

        /// <summary>
        /// Returns the query that should be used for retrieving the list of tables for the specified database.
        /// </summary>
        /// <param name="databaseName">The database to be queried for.</param>
        /// <returns>The query that should be used for retrieving the list of tables for the specified database.</returns>
        public static string GetTableQuery(string databaseName)
        {
            //return GetResource("TotalSafety.DataTierGenerator.Resource.TableQuery.sql", "#DatabaseName#", databaseName);
            return GetResource("TotalSafety.DataTierGenerator.Resource.ExtractDatabaseTableSchema.sql");
        }

        /// <summary>
        /// Returns the query that should be used for retrieving the list of columns for the specified table.
        /// </summary>
        /// <param name="tableName">The table to be queried for.</param>
        /// <returns>The query that should be used for retrieving the list of columns for the specified table.</returns>
        public static string GetColumnQuery(string tableSchema, string tableName)
        {
            string returnValue = GetResource("TotalSafety.DataTierGenerator.Resource.ExtractDatabaseColumnSchema.sql");
            returnValue = returnValue.Replace("#TableSchema#", tableSchema);
            returnValue = returnValue.Replace("#TableName#", tableName);
            return returnValue;
        }

        /// <summary>
        /// Retrieves the specified manifest resource stream from the executing assembly as a string, replacing the specified old value with the specified new value.
        /// </summary>
        /// <param name="databaseName">Name of the resource to retrieve.</param>
        /// <param name="databaseName">The name of the database to be used.</param>
        /// <param name="grantLoginName">The name of the user to be used.</param>
        /// <returns>The queries that should be used to create the specified database login.</returns>
        public static string GetUserQueries(string databaseName, string grantLoginName)
        {
            string returnValue = GetResource( "TotalSafety.DataTierGenerator.Resource.User.sql" );
            returnValue = returnValue.Replace("#DatabaseName#", databaseName);
            returnValue = returnValue.Replace("#UserName#", grantLoginName);
            return returnValue;
        }

        /// <summary>
        /// Returns the query that should be used for retrieving the list of tables for the specified database.
        /// </summary>
        /// <param name="databaseName">The database to be queried for.</param>
        /// <returns>The query that should be used for retrieving the list of tables for the specified database.</returns>
        public static string Get(string databaseName)
        {
            return GetResource("TotalSafety.DataTierGenerator.TableQuery.sql", "#DatabaseName#", databaseName);
        }

        /// <summary>
        /// Retrieves the foreign key information for the specified table.
        /// </summary>
        /// <param name="connection">The SqlConnection to be used when querying for the table information.</param>
        /// <param name="tableName">Name of the table that foreign keys should be checked for.</param>
        /// <returns>DataReader containing the foreign key information for the specified table.</returns>
        public static DataTable GetForeignKeyList(SqlConnection connection, string tableSchema, string tableName)
        {
            SqlParameter parameter;

            using (SqlCommand command = new SqlCommand("sp_fkeys", connection)) {
                command.CommandType = CommandType.StoredProcedure;

                parameter = new SqlParameter("@pktable_name", SqlDbType.NVarChar, 128, ParameterDirection.Input, true, 0, 0, "pktable_name", DataRowVersion.Current, DBNull.Value);
                command.Parameters.Add(parameter);
                parameter = new SqlParameter("@pktable_owner", SqlDbType.NVarChar, 128, ParameterDirection.Input, true, 0, 0, "pktable_owner", DataRowVersion.Current, DBNull.Value);
                command.Parameters.Add(parameter);
                parameter = new SqlParameter("@pktable_qualifier", SqlDbType.NVarChar, 128, ParameterDirection.Input, true, 0, 0, "pktable_qualifier", DataRowVersion.Current, DBNull.Value);
                command.Parameters.Add(parameter);
                parameter = new SqlParameter("@fktable_name", SqlDbType.NVarChar, 128, ParameterDirection.Input, true, 0, 0, "fktable_name", DataRowVersion.Current, tableName);
                command.Parameters.Add(parameter);
                parameter = new SqlParameter("@fktable_owner", SqlDbType.NVarChar, 128, ParameterDirection.Input, true, 0, 0, "fktable_owner", DataRowVersion.Current, tableSchema);
                command.Parameters.Add(parameter);
                parameter = new SqlParameter("@fktable_qualifier", SqlDbType.NVarChar, 128, ParameterDirection.Input, true, 0, 0, "fktable_qualifier", DataRowVersion.Current, DBNull.Value);
                command.Parameters.Add(parameter);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                return dataTable;
            }
        }

        /// <summary>
        /// Retrieves the primary key information for the specified table.
        /// </summary>
        /// <param name="connection">The SqlConnection to be used when querying for the table information.</param>
        /// <param name="tableName">Name of the table that primary keys should be checked for.</param>
        /// <returns>DataReader containing the primary key information for the specified table.</returns>
        public static DataTable GetPrimaryKeyList(SqlConnection connection, string tableSchema, string tableName)
        {
            SqlParameter parameter;

            using (SqlCommand command = new SqlCommand("sp_pkeys", connection)) {
                command.CommandType = CommandType.StoredProcedure;

                parameter = new SqlParameter("@table_name", SqlDbType.NVarChar, 128, ParameterDirection.Input, false, 0, 0, "table_name", DataRowVersion.Current, tableName);
                command.Parameters.Add(parameter);
                parameter = new SqlParameter("@table_owner", SqlDbType.NVarChar, 128, ParameterDirection.Input, true, 0, 0, "table_owner", DataRowVersion.Current, tableSchema);
                command.Parameters.Add(parameter);
                parameter = new SqlParameter("@table_qualifier", SqlDbType.NVarChar, 128, ParameterDirection.Input, true, 0, 0, "table_qualifier", DataRowVersion.Current, DBNull.Value);
                command.Parameters.Add(parameter);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                return dataTable;
            }
        }

        /// <summary>
        /// Creates a string containing the parameter declaration for a stored procedure based on the parameters passed in.
        /// </summary>
        /// <param name="column">Object that stores the information for the column the parameter represents.</param>
        /// <returns>String containing parameter information of the specified column for a stored procedure.</returns>
        public static string CreateParameterString(Column column)
        {
            string columnName;
            string parameter;

            columnName = column.PropertyName;

            switch (column.DbType.ToLower()) {
                case "binary":
                    parameter = "@" + columnName + " " + column.DbType + "(" + column.Length + ")";
                    break;
                case "bigint":
                    parameter = "@" + columnName + " " + column.DbType;
                    break;
                case "bit":
                    parameter = "@" + columnName + " " + column.DbType;
                    break;
                case "char":
                    parameter = "@" + columnName + " " + column.DbType + "(" + column.Length + ")";
                    break;
                case "datetime":
                    parameter = "@" + columnName + " " + column.DbType;
                    break;
                case "decimal":
                    if (column.Scale.Length == 0)
                        parameter = "@" + columnName + " " + column.DbType + "(" + column.Precision + ")";
                    else
                        parameter = "@" + columnName + " " + column.DbType + "(" + column.Precision + ", " + column.Scale + ")";
                    break;
                case "float":
                    parameter = "@" + columnName + " " + column.DbType + "(" + column.Precision + ")";
                    break;
                case "image":
                    parameter = "@" + columnName + " " + column.DbType;
                    break;
                case "int":
                    parameter = "@" + columnName + " " + column.DbType;
                    break;
                case "money":
                    parameter = "@" + columnName + " " + column.DbType;
                    break;
                case "nchar":
                    parameter = "@" + columnName + " " + column.DbType + "(" + column.Length + ")";
                    break;
                case "ntext":
                    parameter = "@" + columnName + " " + column.DbType;
                    break;
                case "nvarchar":
                    parameter = "@" + columnName + " " + column.DbType + "(" + column.Length + ")";
                    break;
                case "numeric":
                    if (column.Scale.Length == 0)
                        parameter = "@" + columnName + " " + column.DbType + "(" + column.Precision + ")";
                    else
                        parameter = "@" + columnName + " " + column.DbType + "(" + column.Precision + ", " + column.Scale + ")";
                    break;
                case "real":
                    parameter = "@" + columnName + " " + column.DbType;
                    break;
                case "smalldatetime":
                    parameter = "@" + columnName + " " + column.DbType;
                    break;
                case "smallint":
                    parameter = "@" + columnName + " " + column.DbType;
                    break;
                case "smallmoney":
                    parameter = "@" + columnName + " " + column.DbType;
                    break;
                case "sql_variant":
                    parameter = "@" + columnName + " " + column.DbType;
                    break;
                case "sysname":
                    parameter = "@" + columnName + " " + column.DbType;
                    break;
                case "text":
                    parameter = "@" + columnName + " " + column.DbType;
                    break;
                case "timestamp":
                    parameter = "@" + columnName + " " + column.DbType;
                    break;
                case "tinyint":
                    parameter = "@" + columnName + " " + column.DbType;
                    break;
                case "varbinary":
                    parameter = "@" + columnName + " " + column.DbType + "(" + column.Length + ")";
                    break;
                case "varchar":
                    parameter = "@" + columnName + " " + column.DbType + "(" + column.Length + ")";
                    break;
                case "uniqueidentifier":
                    parameter = "@" + columnName + " " + column.DbType;
                    break;
                case "xml":
                    parameter = "@" + columnName + " " + column.DbType;
                    break;
                default: // Unknow data type
                    throw (new Exception("Invalid SQL Server data type specified: " + column.DbType));
            }

            // Return the new parameter string
            return parameter;
        }

        /// <summary>
        /// Creates a string for a method parameter representing the specified column.
        /// </summary>
        /// <param name="column">Object that stores the information for the column the parameter represents.</param>
        /// <returns>String containing parameter information of the specified column for a method call.</returns>
        public static string CreateMethodParameter(Column column)
        {
            string parameter;
            string columnName;

            // Format the column name
            columnName = column.PropertyName;
            columnName = FormatCamel(columnName);

            switch (column.DbType.ToLower())
            {
                case "binary":
                    parameter = "byte[] " + columnName;
                    break;
                case "bigint":
                    parameter = "Int64 " + columnName;
                    break;
                case "bit":
                    parameter = "bool " + columnName;
                    break;
                case "char":
                    parameter = "string " + columnName;
                    break;
                case "datetime":
                    parameter = "DateTime " + columnName;
                    break;
                case "decimal":
                    parameter = "decimal " + columnName;
                    break;
                case "float":
                    parameter = "double " + columnName;
                    break;
                case "image":
                    parameter = "byte[] " + columnName;
                    break;
                case "int":
                    parameter = "int " + columnName;
                    break;
                case "money":
                    parameter = "decimal " + columnName;
                    break;
                case "nchar":
                    parameter = "string " + columnName;
                    break;
                case "ntext":
                    parameter = "string " + columnName;
                    break;
                case "nvarchar":
                    parameter = "string " + columnName;
                    break;
                case "numeric":
                    parameter = "decimal " + columnName;
                    break;
                case "real":
                    parameter = "float " + columnName;
                    break;
                case "smalldatetime":
                    parameter = "DateTime " + columnName;
                    break;
                case "smallint":
                    parameter = "short " + columnName;
                    break;
                case "smallmoney":
                    parameter = "decimal " + columnName;
                    break;
                case "sql_variant":
                    parameter = "object " + columnName;
                    break;
                case "sysname":
                    parameter = "string " + columnName;
                    break;
                case "text":
                    parameter = "string " + columnName;
                    break;
                case "timestamp":
                    parameter = "DateTime " + columnName;
                    break;
                case "tinyint":
                    parameter = "byte " + columnName;
                    break;
                case "varbinary":
                    parameter = "byte[] " + columnName;
                    break;
                case "varchar":
                    parameter = "string " + columnName;
                    break;
                case "uniqueidentifier":
                    parameter = "Guid " + columnName;
                    break;
                case "xml":
                    parameter = "String " + columnName;
                    break;
                default: // Unknow data type
                    throw (new Exception("Invalid SQL Server data type specified: " + column.DbType));
            }

            // Return the new parameter string
            return parameter;
        }

        /// <summary>
        /// Formats a string in Camel case (the first letter is in lower case).
        /// </summary>
        /// <param name="original">A string to be formatted.</param>
        /// <returns>A string in Camel case.</returns>
        public static string FormatCamel(string original)
        {
            if (original.Length > 0) {
                return original.Substring(0, 1).ToLower() + original.Substring(1);
            } else {
                return String.Empty;
            }
        }

        /// <summary>
        /// Formats a string in Pascal case (the first letter is in upper case).
        /// </summary>
        /// <param name="original">A string to be formatted.</param>
        /// <returns>A string in Pascal case.</returns>
        public static string FormatPascal(string original)
        {
            if (original.Length > 0) {
                return original.Substring(0, 1).ToUpper() + original.Substring(1);
            } else {
                return String.Empty;
            }
        }

        /// <summary>
        /// Matches a SQL Server data type to a SqlClient.SqlDbType.
        /// </summary>
        /// <param name="column">Object that stores the information for the column the parameter represents.</param>
        /// <returns>A string representing a SqlClient.SqlDbType.</returns>
        public static string GetSqlDBType(Column column)
        {
            switch (column.DbType.ToLower())
            {
                case "binary":
                    return "Binary";
                case "bigint":
                    return "Int64";
                case "bit":
                    return "Boolean";
                case "char":
                    return "String";
                case "datetime":
                    return "DateTime";
                case "decimal":
                    return "Decimal";
                case "float":
                    return "Double";
                case "image":
                    return "Binary";
                case "int":
                    return "Int32";
                case "money":
                    return "Currency";
                case "nchar":
                    return "String";
                case "ntext":
                    return "String";
                case "nvarchar":
                    return "String";
                case "numeric":
                    return "Decimal";
                case "real":
                    return "Single";
                case "smalldatetime":
                    return "DateTime";
                case "smallint":
                    return "Int16";
                case "smallmoney":
                    return "Currency";
                case "sql_variant":
                    return "Object";
                case "sysname":
                    return "String";
                case "text":
                    return "String";
                case "timestamp":
                    return "DateTime";
                case "tinyint":
                    return "Byte";
                case "varbinary":
                    return "Binary";
                case "varchar":
                    return "String";
                case "uniqueidentifier":
                    return "Guid";
                case "xml":
                    return "String";
                default: // Unknow data type
                    throw (new Exception("Invalid SQL Server data type specified: " + column.DbType));
            }
        }


        /// <summary>
        /// Creates a string for a SqlParameter representing the specified column.
        /// </summary>
        /// <param name="column">Object that stores the information for the column the parameter represents.</param>
        /// <returns>String containing SqlParameter information of the specified column for a method call.</returns>
        public static string CreateSqlParameter(Column column)
        {
            string[] methodParameter;
            string columnDBType = GetSqlDBType(column);

            // Get an array of data types and variable names
            methodParameter = CreateMethodParameter(column).Split(' ');

            return "myCommand.AddInParameter(\"@" + column.PropertyName + "\", DbType." + columnDBType + ", " + methodParameter[1] + ")";
        }

        /// <summary>
        /// Cleans whitespace inside a string.
        /// </summary>
        /// <param name="original">A string to be cleaned.</param>
        /// <returns>A cleaned string.</returns>
        public static string CleanWhitespace(string original)
        {
            if (original.Length > 0) {
                return Regex.Replace(original, @"[\s]", "");
            } else {
                return String.Empty;
            }
        }
    }
}