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

            string sqlQuery;

            XmlElement xmlEntitySchemas = xDoc.CreateElement("schemas");
            XmlElement xmlEntitySchema;

            sqlQuery = GetTheSchemaExtractorQuery();

            PopulateTheSchemaTables(sqlQuery, new DataTable[] { schemas, tables, tableColumns, primaryKeys, foreignKeys, views, viewColumns, functions, procedures, parameters });

            #region build the xml document from the resulting recordsets

            xDoc.AppendChild(xmlEntitySchemas);

            foreach (DataRow dr in schemas.Rows)
            {
                xmlEntitySchema = GetSchemaEntity(xDoc, dr);
                LoadTables(xmlEntitySchema["tables"], tables, tableColumns, primaryKeys, foreignKeys);
                LoadViews(xmlEntitySchema["views"], views, viewColumns);
                LoadFunctions(xmlEntitySchema["functions"], functions, parameters);
                LoadProcedures(xmlEntitySchema["procedures"], procedures, parameters);
                xmlEntitySchemas.AppendChild(xmlEntitySchema);
            }

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

        private string GetTheSchemaExtractorQuery()
        {
            StringBuilder sqlQuery = new StringBuilder();

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

            return sqlQuery.ToString();
        }

        private void PopulateTheSchemaTables(string schemaQuery, DataTable[] tables)
        {
            using (SqlConnection connection = new SqlConnection(m_ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(schemaQuery, connection);
                SqlDataReader rdr = cmd.ExecuteReader();

                foreach (DataTable tbl in tables)
                {
                    if (!rdr.IsClosed)
                    {
                        tbl.Load(rdr);
                    }
                }

            }
        }

        private XmlElement GetSchemaEntity(XmlDocument xDoc, DataRow schemaRow)
        {
            XmlElement xmlElement = xDoc.CreateElement("schema");

            xmlElement.Attributes.Append(GetXmlAttribute(xDoc, "name", schemaRow["schema_name"].ToString()));

            xmlElement.AppendChild(xDoc.CreateElement("tables"));
            xmlElement.AppendChild(xDoc.CreateElement("views"));
            xmlElement.AppendChild(xDoc.CreateElement("functions"));
            xmlElement.AppendChild(xDoc.CreateElement("procedures"));

            return xmlElement;
        }

        private void LoadTables(XmlElement tablesElement, DataTable tables, DataTable tableColumns, DataTable primaryKeys, DataTable foreignKeys)
        {
            XmlElement tableElement;
            XmlElement xmlElement;
            XmlElement columnElement;
            XmlDocument xDoc = tablesElement.OwnerDocument;
            string schemaName = tablesElement.ParentNode.Attributes["name"].Value;

            DataView tablesView = new DataView(tables, string.Format("schema_name = '{0}'", schemaName), "", DataViewRowState.CurrentRows);
            DataView tableColumnsView;
            DataView primaryKeyView;
            DataView foreignKeysView;
            string tableName;

            foreach (DataRowView drv in tablesView)
            {
                tableName = drv["table_name"].ToString();
                tableElement = xDoc.CreateElement("table");
                tableElement.Attributes.Append(GetXmlAttribute(xDoc, "name", tableName));
                tableElement.Attributes.Append(GetXmlAttribute(xDoc, "create_date", drv["create_date"].ToString()));
                tableElement.Attributes.Append(GetXmlAttribute(xDoc, "modify_date", drv["modify_date"].ToString()));

                #region get the table columns

                xmlElement = xDoc.CreateElement("columns");
                tableColumnsView = new DataView(tableColumns, string.Format("schema_name = '{0}' AND table_name = '{1}'", schemaName, tableName), "", DataViewRowState.CurrentRows);
                foreach (DataRowView drvC in tableColumnsView)
                {
                    columnElement = xDoc.CreateElement("column");
                    columnElement.Attributes.Append(GetXmlAttribute(xDoc, "name", drvC["column_name"].ToString()));
                    columnElement.Attributes.Append(GetXmlAttribute(xDoc, "id", drvC["column_id"].ToString()));
                    columnElement.Attributes.Append(GetXmlAttribute(xDoc, "data_type", drvC["data_type"].ToString()));
                    columnElement.Attributes.Append(GetXmlAttribute(xDoc, "max_length", drvC["max_length"].ToString()));
                    columnElement.Attributes.Append(GetXmlAttribute(xDoc, "precision", drvC["precision"].ToString()));
                    columnElement.Attributes.Append(GetXmlAttribute(xDoc, "scale", drvC["scale"].ToString()));
                    columnElement.Attributes.Append(GetXmlAttribute(xDoc, "is_nullable", drvC["is_nullable"].ToString()));
                    columnElement.Attributes.Append(GetXmlAttribute(xDoc, "is_rowguidcol", drvC["is_rowguidcol"].ToString()));
                    columnElement.Attributes.Append(GetXmlAttribute(xDoc, "is_identity", drvC["is_identity"].ToString()));
                    columnElement.Attributes.Append(GetXmlAttribute(xDoc, "description", drvC["description"].ToString()));
                    columnElement.Attributes.Append(GetXmlAttribute(xDoc, "default_definition", drvC["default_definition"].ToString()));

                    xmlElement.AppendChild(columnElement);
                }
                tableElement.AppendChild(xmlElement);

                #endregion

                #region get the primary key columns

                primaryKeyView = new DataView(primaryKeys, string.Format("schema_name = '{0}' AND table_name = '{1}'", schemaName, tableName), "", DataViewRowState.CurrentRows);
                if (primaryKeyView.Count > 0)
                {
                    xmlElement = xDoc.CreateElement("primary_key");
                    xmlElement.Attributes.Append(GetXmlAttribute(xDoc, "name", primaryKeyView[0]["index_name"].ToString()));
                    foreach (DataRowView drvP in primaryKeyView)
                    {
                        columnElement = xDoc.CreateElement("column");
                        columnElement.Attributes.Append(GetXmlAttribute(xDoc, "name", drvP["column_name"].ToString()));
                        columnElement.Attributes.Append(GetXmlAttribute(xDoc, "id", drvP["key_ordinal"].ToString()));

                        xmlElement.AppendChild(columnElement);
                    }
                    tableElement.AppendChild(xmlElement);
                }

                #endregion

                #region get the foreign key columns

                foreignKeysView = new DataView(foreignKeys, string.Format("schema_name = '{0}' AND table_name = '{1}'", schemaName, tableName), "", DataViewRowState.CurrentRows);
                if (foreignKeysView.Count > 0)
                {
                    XmlElement foreignKeysElement = xDoc.CreateElement("foreign_keys");
                    string prevName = "";
                    string foreignKeyName = "";

                    foreach (DataRowView drvF in foreignKeysView)
                    {
                        foreignKeyName = drvF["foreign_key_name"].ToString();

                        if (prevName != foreignKeyName)
                        {
                            xmlElement = xDoc.CreateElement("foreign_key");
                            xmlElement.Attributes.Append(GetXmlAttribute(xDoc, "name", foreignKeyName));
                            foreignKeysElement.AppendChild(xmlElement);
                            prevName = foreignKeyName;
                        }

                        columnElement = xDoc.CreateElement("column");
                        columnElement.Attributes.Append(GetXmlAttribute(xDoc, "constraint_column_name", drvF["constraint_column_name"].ToString()));
                        columnElement.Attributes.Append(GetXmlAttribute(xDoc, "constraint_column_id", drvF["constraint_column_id"].ToString()));
                        columnElement.Attributes.Append(GetXmlAttribute(xDoc, "referenced_object", drvF["referenced_object"].ToString()));
                        columnElement.Attributes.Append(GetXmlAttribute(xDoc, "referenced_column_name", drvF["referenced_column_name"].ToString()));

                        xmlElement.AppendChild(columnElement);
                    }
                    tableElement.AppendChild(foreignKeysElement);
                }

                #endregion

                tablesElement.AppendChild(tableElement);
            }
        }

        private void LoadViews(XmlElement containerElement, DataTable views, DataTable viewColumns)
        {
            string schemaName = containerElement.ParentNode.Attributes["name"].Value;

            XmlDocument xDoc = containerElement.OwnerDocument;

            DataView viewsView = new DataView(views, string.Format("schema_name = '{0}'", schemaName), "", DataViewRowState.CurrentRows);
            DataView viewColumnsView;

            XmlElement parentElement;
            XmlElement childContainerElement;
            XmlElement childElement;

            string viewName;

            foreach (DataRowView drv in viewsView)
            {
                viewName = drv["view_name"].ToString();
                parentElement = xDoc.CreateElement("view");
                parentElement.Attributes.Append(GetXmlAttribute(xDoc, "name", viewName));
                parentElement.Attributes.Append(GetXmlAttribute(xDoc, "create_date", drv["create_date"].ToString()));
                parentElement.Attributes.Append(GetXmlAttribute(xDoc, "modify_date", drv["modify_date"].ToString()));

                #region get the view columns

                childContainerElement = xDoc.CreateElement("columns");
                viewColumnsView = new DataView(viewColumns, string.Format("schema_name = '{0}' AND view_name = '{1}'", schemaName, viewName), "", DataViewRowState.CurrentRows);
                foreach (DataRowView drvC in viewColumnsView)
                {
                    childElement = xDoc.CreateElement("column");
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "name", drvC["column_name"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "id", drvC["column_id"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "data_type", drvC["data_type"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "max_length", drvC["max_length"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "precision", drvC["precision"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "scale", drvC["scale"].ToString()));

                    childContainerElement.AppendChild(childElement);
                }
                parentElement.AppendChild(childContainerElement);

                #endregion

                containerElement.AppendChild(parentElement);
            }

        }

        private void LoadFunctions(XmlElement containerElement, DataTable functions, DataTable parameters)
        {
            string schemaName = containerElement.ParentNode.Attributes["name"].Value;
            XmlDocument xDoc = containerElement.OwnerDocument;

            DataView functionsView = new DataView(functions, string.Format("schema_name = '{0}'", schemaName), "", DataViewRowState.CurrentRows);
            DataView parametersView;

            XmlElement parentElement;
            XmlElement childContainerElement;
            XmlElement childElement;

            string functionName;
            string parameterName;

            foreach (DataRowView drv in functionsView)
            {
                functionName = drv["function_name"].ToString();
                parentElement = xDoc.CreateElement("function");
                parentElement.Attributes.Append(GetXmlAttribute(xDoc, "name", functionName));
                parentElement.Attributes.Append(GetXmlAttribute(xDoc, "type", drv["type_desc"].ToString()));
                parentElement.Attributes.Append(GetXmlAttribute(xDoc, "create_date", drv["create_date"].ToString()));
                parentElement.Attributes.Append(GetXmlAttribute(xDoc, "modify_date", drv["modify_date"].ToString()));

                #region get the parameters

                childContainerElement = xDoc.CreateElement("parameters");

                parametersView = new DataView(parameters, string.Format("schema_name = '{0}' AND procedure_name = '{1}'", schemaName, functionName), "", DataViewRowState.CurrentRows);
                foreach (DataRowView drvP in parametersView)
                {
                    parameterName = drvP["parameter_name"].ToString();
                    if (string.IsNullOrEmpty(parameterName))
                    {
                        parameterName = "@ReturnValue";
                    }

                    childElement = xDoc.CreateElement("parameter");
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "name", parameterName));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "id", drvP["parameter_id"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "data_type", drvP["parameter_type"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "max_length", drvP["max_length"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "precision", drvP["precision"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "scale", drvP["scale"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "is_output", drvP["is_output"].ToString()));

                    childContainerElement.AppendChild(childElement);
                }
                parentElement.AppendChild(childContainerElement);

                #endregion

                containerElement.AppendChild(parentElement);
            }
        }

        private void LoadProcedures(XmlElement containerElement, DataTable procedures, DataTable parameters)
        {
            string schemaName = containerElement.ParentNode.Attributes["name"].Value;
            XmlDocument xDoc = containerElement.OwnerDocument;

            DataView proceduresView = new DataView(procedures, string.Format("schema_name = '{0}'", schemaName), "", DataViewRowState.CurrentRows);
            DataView parametersView;

            XmlElement parentElement;
            XmlElement childContainerElement;
            XmlElement childElement;

            string procedureName;
            string parameterName;

            foreach (DataRowView drv in proceduresView)
            {
                procedureName = drv["procedure_name"].ToString();
                parentElement = xDoc.CreateElement("procedure");
                parentElement.Attributes.Append(GetXmlAttribute(xDoc, "name", procedureName));
                parentElement.Attributes.Append(GetXmlAttribute(xDoc, "type", drv["type_desc"].ToString()));
                parentElement.Attributes.Append(GetXmlAttribute(xDoc, "create_date", drv["create_date"].ToString()));
                parentElement.Attributes.Append(GetXmlAttribute(xDoc, "modify_date", drv["modify_date"].ToString()));

                #region get the parameters

                childContainerElement = xDoc.CreateElement("parameters");

                parametersView = new DataView(parameters, string.Format("schema_name = '{0}' AND procedure_name = '{1}'", schemaName, procedureName), "", DataViewRowState.CurrentRows);
                foreach (DataRowView drvP in parametersView)
                {
                    parameterName = drvP["parameter_name"].ToString();
                    if (string.IsNullOrEmpty(parameterName))
                    {
                        parameterName = "@ReturnValue";
                    }

                    childElement = xDoc.CreateElement("parameter");
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "name", drvP["parameter_name"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "id", drvP["parameter_id"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "data_type", drvP["parameter_type"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "max_length", drvP["max_length"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "precision", drvP["precision"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "scale", drvP["scale"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "is_output", drvP["is_output"].ToString()));

                    childContainerElement.AppendChild(childElement);
                }
                parentElement.AppendChild(childContainerElement);

                #endregion

                containerElement.AppendChild(parentElement);
            }
        }

        private XmlAttribute GetXmlAttribute(XmlDocument xDoc, string name, string value)
        {
            XmlAttribute xmlAttribute;
            xmlAttribute = xDoc.CreateAttribute(name);
            xmlAttribute.Value = value;
            return xmlAttribute;
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