using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

using TotalSafety.DataTierGenerator.Common;

namespace TotalSafety.DataTierGenerator.SchemaExtractor
{
    public sealed class SchemaExtractor
    {

        #region private / protected member variables

        private string m_ConnectionString = "";
        private Table[] m_Tables;
        private View[] m_Views;
        private Procedure[] m_Procedures;
        //private Function[] m_Functions;

        XmlDocument m_DataMappingXml;

        //private string m_DataLayerOutputPath = "";
        //private string m_DataLayerNamespace = "";

        #endregion

        #region constructors / desturctors

        public SchemaExtractor()
        {

            //string dataMapping = Utility.GetResource("TotalSafety.DataTierGenerator.Resource.DataMapping.xml");

            //m_DataMappingXml = new XmlDocument();
            //m_DataMappingXml.LoadXml(dataMapping);

            m_Tables = new Table[0];
            m_Views = new View[0];
            m_Procedures = new Procedure[0];

        }

        public SchemaExtractor(string connectionString)
            : this()
        {

            m_ConnectionString = connectionString;

        }

        #endregion

        #region public properties

        public string ConnectionString
        {
            get
            {
                return m_ConnectionString;
            }
            set
            {
                m_ConnectionString = value;
            }
        }

        public Table[] Tables
        {
            get
            {
                return m_Tables;
            }
            set
            {
                m_Tables = value;
            }
        }

        public View[] Views
        {
            get
            {
                return m_Views;
            }
            set
            {
                m_Views = value;
            }
        }

        public Procedure[] Procedures
        {
            get
            {
                return m_Procedures;
            }
            set
            {
                m_Procedures = value;
            }
        }

        #endregion

        #region public methods

        public XmlDocument GetSchemaDefinition(string connectionString)
        {
            m_ConnectionString = connectionString;

            return GetSchemaDefinition();
        }

        public XmlDocument GetSchemaDefinition()
        {
            XmlDocument xDoc = new XmlDocument();
            DataTable schemas = new DataTable();
            DataTable tables = new DataTable();
            DataTable tableColumns = new DataTable();
            DataTable primaryKeys = new DataTable();
            DataTable foreignKeys = new DataTable();
            DataTable views = new DataTable();
            DataTable viewColumns = new DataTable();
            DataTable functions = new DataTable();
            DataTable procedures = new DataTable();
            DataTable parameters = new DataTable();

            StringBuilder sqlQuery = new StringBuilder();

            #region build the sql query

            sqlQuery.AppendLine(
                Utility.GetResource(Assembly.GetExecutingAssembly(),
                "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.ExtractDatabaseSchemas.sql"));

            sqlQuery.AppendLine(
                Utility.GetResource(Assembly.GetExecutingAssembly(),
                "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.ExtractDatabaseTables.sql"));

            sqlQuery.AppendLine(
                Utility.GetResource(Assembly.GetExecutingAssembly(),
                "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.ExtractDatabaseTableColumns.sql"));

            sqlQuery.AppendLine(
                Utility.GetResource(Assembly.GetExecutingAssembly(),
                "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.ExtractDatabasePrimaryKeys.sql"));

            sqlQuery.AppendLine(
                Utility.GetResource(Assembly.GetExecutingAssembly(),
                "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.ExtractDatabaseForeignKeys.sql"));

            sqlQuery.AppendLine(
                Utility.GetResource(Assembly.GetExecutingAssembly(),
                "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.ExtractDatabaseViews.sql"));

            sqlQuery.AppendLine(
                Utility.GetResource(Assembly.GetExecutingAssembly(),
                "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.ExtractDatabaseViewColumns.sql"));

            sqlQuery.AppendLine(
                Utility.GetResource(Assembly.GetExecutingAssembly(),
                "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.ExtractDatabaseFunctions.sql"));

            sqlQuery.AppendLine(
                Utility.GetResource(Assembly.GetExecutingAssembly(),
                "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.ExtractDatabaseProcedures.sql"));

            sqlQuery.AppendLine(
                Utility.GetResource(Assembly.GetExecutingAssembly(),
                "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.ExtractDatabaseParameters.sql"));

            #endregion

            #region get the definitions

            using (SqlConnection connection = new SqlConnection(m_ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery.ToString(), connection);
                SqlDataReader rdr = cmd.ExecuteReader();

                populateTablesFromReader(rdr, new DataTable[] { schemas, tables, tableColumns, primaryKeys, foreignKeys, views, viewColumns, functions, procedures, parameters });

            }

            #endregion

            #region build the xml document from the resulting recordsets
            #endregion

            #region get all the schemas needed for processing

            #endregion

            return xDoc;
        }

        public void LoadSchema()
        {

            string query;

            DataTable tableAndColumnSchema;
            DataTable tablePrimaryKeySchema;
            DataTable tableForeignKeySchema;
            DataTable viewSchema;
            DataTable storedProcedureSchema;

            SqlDataAdapter dataAdapter;

            if (string.IsNullOrEmpty(m_ConnectionString))
            {
                throw new Exception("The connection string is not set. The connection string needs to be set in order to communicate with the database.");
            }

            #region get all the schemas needed for processing

            using (SqlConnection connection = new SqlConnection(m_ConnectionString))
            {

                connection.Open();

                // Table and Column Schema
                tableAndColumnSchema = new DataTable();
                query =
                    Utility.GetResource(Assembly.GetExecutingAssembly(), "ExtractDatabaseTableAndColumnSchema.sql");
                dataAdapter =
                    new SqlDataAdapter(query, connection);
                dataAdapter.Fill(tableAndColumnSchema);

                // Primary Key Schema
                tablePrimaryKeySchema = new DataTable();
                query =
                    Utility.GetResource(Assembly.GetExecutingAssembly(), "ExtractDatabasePrimaryKeySchema.sql");
                dataAdapter.SelectCommand.CommandText = query;
                dataAdapter.Fill(tablePrimaryKeySchema);

                // Foreign Key Schema
                tableForeignKeySchema = new DataTable();
                query =
                    Utility.GetResource(Assembly.GetExecutingAssembly(), "ExtractDatabaseForeignKeySchema.sql");
                dataAdapter.SelectCommand.CommandText = query;
                dataAdapter.Fill(tableForeignKeySchema);

                //viewSchema = new DataTable();
                //dataAdapter =
                //    new SqlDataAdapter(Utility.GetTableQuery(connection.Database), connection);
                //dataAdapter.Fill(viewSchema);

                //storedProcedureSchema = new DataTable();
                //dataAdapter =
                //    new SqlDataAdapter(Utility.GetTableQuery(connection.Database), connection);
                //dataAdapter.Fill(storedProcedureSchema);

                connection.Close();

            }

            #endregion

            BuildTableList(tableAndColumnSchema, tablePrimaryKeySchema, tableForeignKeySchema);

            //ExtractViewSchema(databaseViewSchema);

            //ExtractStoredProcedureSchema(databaseStoredProcedureSchema);

        }

        #endregion

        #region private implementation

        private void populateTablesFromReader(SqlDataReader rdr, DataTable[] tables)
        {
            if (rdr == null)
            {
                throw new ArgumentNullException("rdr");
            }

            foreach (DataTable tbl in tables)
            {
                if (!rdr.IsClosed)
                {
                    tbl.Load(rdr);
                }
            }
        }






        // anything below this line should be deleted when the xml version is ready 96/12/2012

        private void BuildTableList(DataTable tableAndColumnSchema
            , DataTable tablePrimaryKeySchema, DataTable tableForeignKeySchema)
        {

            string curSchemaName = "";
            string curTableName = "";

            string prevSchemaName = "";
            string prevTableName = "";

            DataView tableColumnView;
            DataView primaryKeyView;
            DataView foreignKeyView;

            Table table;

            System.Data.SqlClient.SqlConnectionStringBuilder bldr;

            bldr = new SqlConnectionStringBuilder(m_ConnectionString);

            // add a new table to the table list for each table schema row in the datatable
            foreach (DataRow dataRow in tableAndColumnSchema.Rows)
            {

                curSchemaName = (string)dataRow["SchemaName"];
                curTableName = (string)dataRow["TableName"];

                if (!curSchemaName.Equals(prevSchemaName) || !curTableName.Equals(prevTableName))
                {

                    prevSchemaName = curSchemaName;
                    prevTableName = curTableName;

                    tableColumnView = tableAndColumnSchema.DefaultView;
                    tableColumnView.RowFilter =
                        "SchemaName=\'" + curSchemaName + "\' And TableName=\'" + curTableName + "\'";

                    primaryKeyView = tablePrimaryKeySchema.DefaultView;
                    primaryKeyView.RowFilter =
                        "SchemaName=\'" + curSchemaName + "\' And TableName=\'" + curTableName + "\'";

                    table = CreateTable(bldr.DataSource, tableColumnView);

                    if (primaryKeyView.Count > 0)
                    {
                        AddPrimaryKey(table, primaryKeyView);
                    }

                    //m_Tables.Add(table);
                }

            }

            prevSchemaName = "";
            prevTableName = "";

            foreach (Table curTable in m_Tables)
            {

                curSchemaName = curTable.Schema;
                curTableName = curTable.Name;

                if (!curSchemaName.Equals(prevSchemaName) || !curTableName.Equals(prevTableName))
                {

                    prevSchemaName = curSchemaName;
                    prevTableName = curTableName;

                    foreignKeyView = tableForeignKeySchema.DefaultView;
                    foreignKeyView.RowFilter =
                        "SchemaName=\'" + curSchemaName + "\' And TableName=\'" + curTableName + "\'";

                    if (foreignKeyView.Count > 0)
                    {
                        AddForeignKeyList(curTable, foreignKeyView);
                    }

                }

            }

        }

        private void ExtractViewSchema()
        {
        }

        private void ExtractStoredProcedureSchema()
        {
        }

        private Table CreateTable(string dataSource, DataView tableColumnView)
        {

            Table table = new Table();

            table.DatabaseName = dataSource;
            table.Schema = (string)tableColumnView[0]["SchemaName"];
            table.Name = (string)tableColumnView[0]["TableName"];

            if (tableColumnView[0]["TableDescription"] != DBNull.Value)
            {
                table.Description = (string)tableColumnView[0]["TableDescription"];
            }

            foreach (DataRowView drv in tableColumnView)
            {
                table.Columns.Add(CreateColumn(drv));
            }

            return table;

        }

        private Column CreateColumn(DataRowView tableColumnRowView)
        {

            Column column = new Column();

            string xPath;
            XmlNode node;

            column.Name = tableColumnRowView["ColumnName"].ToString();
            column.Precision = tableColumnRowView["NumericPrecision"].ToString();
            column.Scale = tableColumnRowView["NumericScale"].ToString();

            column.Type = tableColumnRowView["SystemType"].ToString();

            // get the enumerated type name
            xPath =
                "/Databases/Database[@Name='MSSQL']/FromDatabase/Type[@Name='"
                + column.Type + "']/System.Data.SqlDbType";
            node = m_DataMappingXml.SelectSingleNode(xPath);
            column.EnumeratedTypeName = node.Attributes["Value"].Value;

            // get the ClrType
            xPath =
                "/Databases/Database[@Name='MSSQL']/FromDatabase/Type[@Name='"
                + column.Type + "']/System";
            node = m_DataMappingXml.SelectSingleNode(xPath);
            column.ClrType = node.Attributes["Value"].Value;

            // get the Language Type
            xPath =
                "/Databases/Database[@Name='MSSQL']/FromDatabase/Type[@Name='"
                + column.Type + "']/CSharp";
            node = m_DataMappingXml.SelectSingleNode(xPath);
            column.LanguageType = node.Attributes["Value"].Value;

            // Determine the column's extended name
            if (tableColumnRowView["ColumnDescription"] != DBNull.Value)
            {
                column.Description = tableColumnRowView["ColumnDescription"].ToString();
            }

            // Determine the column's length
            column.Length = tableColumnRowView["Length"].ToString();

            // Is the column a RowGuidCol column?
            column.IsRowGuidCol = (bool)tableColumnRowView["IsRowGuid"];

            // Is the column a Nullable column?
            column.IsNullable = (bool)tableColumnRowView["IsNullable"];

            // Is the column an Identity column?
            column.IsIdentity = (bool)tableColumnRowView["IsIdentity"];

            // Is tableColumnRowView column a computed column?
            column.IsComputed = (bool)tableColumnRowView["IsComputed"];

            // does tableColumnRowView have a default value?
            if (tableColumnRowView["DefaultValue"] != DBNull.Value)
            {
                column.DefaultValue = (string)tableColumnRowView["DefaultValue"];
            }
            else
            {
                column.DefaultValue = "";
            }

            return column;

        }

        private void AddPrimaryKey(Table table, DataView primaryKeyView)
        {

            Index pkIndex;

            pkIndex = new Index();

            pkIndex.Name = (string)primaryKeyView[0]["IndexName"];
            pkIndex.ParentTable = table;

            primaryKeyView.Sort = "Ordinal";

            foreach (DataRowView drv in primaryKeyView)
            {

                foreach (Column column in table.Columns)
                {

                    if (column.Name.Equals((string)drv["ColumnName"]))
                    {
                        pkIndex.Columns.Add(column);
                    }

                }

            }

            table.PrimaryKey = pkIndex;

        }

        private void AddForeignKeyList(Table table, DataView foreignKeyView)
        {

            ForeignKeyIndex fkIndex;

            if (foreignKeyView != null && foreignKeyView.Count > 0)
            {

                #region create the first index from the first foreign key record

                fkIndex = CreateForeignKeyIndex(table, foreignKeyView[0]);

                table.ForeignKeys.Add(fkIndex);

                #endregion

                // cycle through the remaining records either creating an new index or
                // adding additional columns to the current index
                for (int index = 1; index < foreignKeyView.Count; index++)
                {

                    if (fkIndex.Name != (string)foreignKeyView[index]["IndexName"])
                    {

                        fkIndex = CreateForeignKeyIndex(table, foreignKeyView[index]);

                        table.ForeignKeys.Add(fkIndex);

                    }
                    else
                    {
                        fkIndex.Columns.Add(table.Columns[(string)foreignKeyView[index]["ColumnName"]]);
                    }

                }

            }

        }

        private ForeignKeyIndex CreateForeignKeyIndex(Table table, DataRowView foreignKeyRowView)
        {

            ForeignKeyIndex fkIndex;

            fkIndex = new ForeignKeyIndex();

            //fkIndex.Name = (string)foreignKeyRowView["IndexName"];;
            //fkIndex.ParentTable = table;
            //fkIndex.ReferencedTable =
            //    m_Tables.FindTable((string)foreignKeyRowView["ReferencedSchemaName"]
            //    , (string)foreignKeyRowView["ReferencedTableName"]);
            //fkIndex.ReferencedPrimaryKey = fkIndex.ReferencedTable.PrimaryKey;

            //fkIndex.Columns.Add(table.Columns[(string)foreignKeyRowView["ColumnName"]]);

            return fkIndex;

        }

        #endregion

    }
}