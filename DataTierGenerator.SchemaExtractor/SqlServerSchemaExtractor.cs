using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Xml;

using TotalSafety.DataTierGenerator.Common;

namespace TotalSafety.DataTierGenerator.SchemaExtractor
{
    class SqlServerSchemaExtractor : ISchemaExtractor
    {

        #region private / protected member variables

        private string m_ConnectionString = "";
        XmlDocument m_DataMappingXml;

        #endregion

        #region constructors / desturctors

        public SqlServerSchemaExtractor()
        {
            string dataMapping = Utility.GetResource(Assembly.GetExecutingAssembly(), "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.DataMapping.xml");

            m_DataMappingXml = new XmlDocument();
            m_DataMappingXml.LoadXml(dataMapping);
        }

        public SqlServerSchemaExtractor(string connectionString)
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

        #endregion

        #region public methods

        XmlDocument ISchemaExtractor.GetSchemaDefinition()
        {
            return GetSchemaDefinition();
        }

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

            return xDoc;
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

            xmlElement.Attributes.Append(GetXmlAttribute(xDoc, "name", schemaRow["name"].ToString()));

            xmlElement.AppendChild(xDoc.CreateElement("tables"));
            xmlElement.AppendChild(xDoc.CreateElement("views"));
            xmlElement.AppendChild(xDoc.CreateElement("functions"));
            xmlElement.AppendChild(xDoc.CreateElement("procedures"));

            return xmlElement;
        }

        private void LoadTables(XmlElement tablesElement, DataTable tables, DataTable tableColumns, DataTable primaryKeys, DataTable foreignKeys)
        {
            string xPath;
            XmlNode node;

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
                LoadColumns(xmlElement, tableColumnsView);
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
                        columnElement = xDoc.CreateElement("pk_column");
                        columnElement.Attributes.Append(GetXmlAttribute(xDoc, "column_name", drvP["column_name"].ToString()));
                        columnElement.Attributes.Append(GetXmlAttribute(xDoc, "key_ordinal", drvP["key_ordinal"].ToString()));

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

                        columnElement = xDoc.CreateElement("fk_column");
                        columnElement.Attributes.Append(GetXmlAttribute(xDoc, "constraint_column_name", drvF["constraint_column_name"].ToString()));
                        columnElement.Attributes.Append(GetXmlAttribute(xDoc, "constraint_column_id", drvF["constraint_column_id"].ToString()));
                        columnElement.Attributes.Append(GetXmlAttribute(xDoc, "referenced_table", drvF["referenced_object"].ToString()));
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
            string xPath;
            XmlNode node;
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
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "is_nullable", drvC["is_nullable"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "is_computed", drvC["is_computed"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "is_rowguidcol", drvC["is_rowguidcol"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "is_identity", drvC["is_identity"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "description", drvC["description"].ToString()));

                    // get the enumerated type name
                    xPath =
                        "/Databases/Database[@Name='MSSQL']/FromDatabase/Type[@Name='"
                        + drvC["data_type"].ToString() + "']/System.Data.SqlDbType";
                    node = m_DataMappingXml.SelectSingleNode(xPath);
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "EnumeratedTypeName", node.Attributes["Value"].Value));

                    // get the ClrType
                    xPath =
                        "/Databases/Database[@Name='MSSQL']/FromDatabase/Type[@Name='"
                        + drvC["data_type"].ToString() + "']/System";
                    node = m_DataMappingXml.SelectSingleNode(xPath);
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "ClrType", node.Attributes["Value"].Value));

                    // get the Language Type
                    xPath =
                        "/Databases/Database[@Name='MSSQL']/FromDatabase/Type[@Name='"
                        + drvC["data_type"].ToString() + "']/CSharp";
                    node = m_DataMappingXml.SelectSingleNode(xPath);
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "LanguageType", node.Attributes["Value"].Value));

                    childContainerElement.AppendChild(childElement);
                }
                parentElement.AppendChild(childContainerElement);

                #endregion

                containerElement.AppendChild(parentElement);
            }

        }

        private void LoadColumns(XmlElement containerElement, DataView columnsView)
        {
            string xPath;
            XmlNode node;
            XmlElement columnElement;
            XmlDocument xDoc = containerElement.OwnerDocument;

            foreach (DataRowView drvC in columnsView)
            {
                columnElement = xDoc.CreateElement("column");
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "name", drvC["column_name"].ToString()));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "id", drvC["column_id"].ToString()));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "data_type", drvC["data_type"].ToString()));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "max_length", drvC["max_length"].ToString()));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "precision", drvC["precision"].ToString()));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "scale", drvC["scale"].ToString()));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "is_nullable", drvC["is_nullable"].ToString()));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "is_computed", drvC["is_computed"].ToString()));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "is_rowguidcol", drvC["is_rowguidcol"].ToString()));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "is_identity", drvC["is_identity"].ToString()));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "description", drvC["description"].ToString()));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "default_definition", drvC["default_definition"].ToString()));
                
                // get the enumerated type name
                xPath =
                    "/Databases/Database[@Name='MSSQL']/FromDatabase/Type[@Name='"
                    + drvC["data_type"].ToString() + "']/System.Data.SqlDbType";
                node = m_DataMappingXml.SelectSingleNode(xPath);
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "EnumeratedTypeName", node.Attributes["Value"].Value));

                // get the ClrType
                xPath =
                    "/Databases/Database[@Name='MSSQL']/FromDatabase/Type[@Name='"
                    + drvC["data_type"].ToString() + "']/System";
                node = m_DataMappingXml.SelectSingleNode(xPath);
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "ClrType", node.Attributes["Value"].Value));

                // get the Language Type
                xPath =
                    "/Databases/Database[@Name='MSSQL']/FromDatabase/Type[@Name='"
                    + drvC["data_type"].ToString() + "']/CSharp";
                node = m_DataMappingXml.SelectSingleNode(xPath);
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "LanguageType", node.Attributes["Value"].Value));

                containerElement.AppendChild(columnElement);
            }
        }

        private void LoadFunctions(XmlElement containerElement, DataTable functions, DataTable parameters)
        {
            string xPath;
            XmlNode node;
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
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "description", drvP["description"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "data_type", drvP["data_type"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "max_length", drvP["max_length"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "precision", drvP["precision"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "scale", drvP["scale"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "is_output", drvP["is_output"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "default_definition", drvP["default_definition"].ToString()));

                    // get the enumerated type name
                    xPath =
                        "/Databases/Database[@Name='MSSQL']/FromDatabase/Type[@Name='"
                        + drvP["data_type"].ToString() + "']/System.Data.SqlDbType";
                    node = m_DataMappingXml.SelectSingleNode(xPath);
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "EnumeratedTypeName", node.Attributes["Value"].Value));

                    // get the ClrType
                    xPath =
                        "/Databases/Database[@Name='MSSQL']/FromDatabase/Type[@Name='"
                        + drvP["data_type"].ToString() + "']/System";
                    node = m_DataMappingXml.SelectSingleNode(xPath);
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "ClrType", node.Attributes["Value"].Value));

                    // get the Language Type
                    xPath =
                        "/Databases/Database[@Name='MSSQL']/FromDatabase/Type[@Name='"
                        + drvP["data_type"].ToString() + "']/CSharp";
                    node = m_DataMappingXml.SelectSingleNode(xPath);
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "LanguageType", node.Attributes["Value"].Value));

                    childContainerElement.AppendChild(childElement);
                }
                parentElement.AppendChild(childContainerElement);

                #endregion

                containerElement.AppendChild(parentElement);
            }
        }

        private void LoadProcedures(XmlElement containerElement, DataTable procedures, DataTable parameters)
        {
            string xPath;
            XmlNode node;
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
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "description", drvP["description"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "data_type", drvP["data_type"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "max_length", drvP["max_length"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "precision", drvP["precision"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "scale", drvP["scale"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "is_output", drvP["is_output"].ToString()));
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "default_definition", drvP["default_definition"].ToString()));

                    // get the enumerated type name
                    xPath =
                        "/Databases/Database[@Name='MSSQL']/FromDatabase/Type[@Name='"
                        + drvP["data_type"].ToString() + "']/System.Data.SqlDbType";
                    node = m_DataMappingXml.SelectSingleNode(xPath);
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "EnumeratedTypeName", node.Attributes["Value"].Value));

                    // get the ClrType
                    xPath =
                        "/Databases/Database[@Name='MSSQL']/FromDatabase/Type[@Name='"
                        + drvP["data_type"].ToString() + "']/System";
                    node = m_DataMappingXml.SelectSingleNode(xPath);
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "ClrType", node.Attributes["Value"].Value));

                    // get the Language Type
                    xPath =
                        "/Databases/Database[@Name='MSSQL']/FromDatabase/Type[@Name='"
                        + drvP["data_type"].ToString() + "']/CSharp";
                    node = m_DataMappingXml.SelectSingleNode(xPath);
                    childElement.Attributes.Append(GetXmlAttribute(xDoc, "LanguageType", node.Attributes["Value"].Value));

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

        #endregion

    }
}
