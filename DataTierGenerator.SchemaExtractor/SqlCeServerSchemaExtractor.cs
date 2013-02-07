using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Data.SqlServerCe;

using TotalSafety.DataTierGenerator.Common;

namespace TotalSafety.DataTierGenerator.SchemaExtractor
{
    class SqlCeServerSchemaExtractor : ISchemaExtractor
    {

        #region private / protected member variables

        private string m_ConnectionString = "";
        XmlDocument m_DataMappingXml;

        #endregion

        #region constructors / desturctors

        public SqlCeServerSchemaExtractor()
        {
            string dataMapping = Utility.GetResource(Assembly.GetExecutingAssembly(), "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.DataMapping.xml");

            m_DataMappingXml = new XmlDocument();
            m_DataMappingXml.LoadXml(dataMapping);
        }

        public SqlCeServerSchemaExtractor(string connectionString)
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
            DataTable tables = new DataTable();
            DataTable tableColumns = new DataTable();
            DataTable primaryKeys = new DataTable();

            string sqlQuery;

            XmlElement xmlEntitySchemas = xDoc.CreateElement("schemas");
            XmlElement xmlEntitySchema = xDoc.CreateElement("schema");
            xmlEntitySchema.Attributes.Append(GetXmlAttribute(xDoc, "name", "dbo"));

            xmlEntitySchemas.AppendChild(xmlEntitySchema);
            xDoc.AppendChild(xmlEntitySchemas);

            XmlElement xmlEntityTables = xDoc.CreateElement("tables");
            xmlEntitySchema.AppendChild(xmlEntityTables);

            sqlQuery = GetTheSchemaExtractorQuery();

            PopulateTheSchemaTables(sqlQuery, new DataTable[] { tables, tableColumns, primaryKeys });

            #region build the xml document from the resulting recordsets

            LoadTables(xmlEntityTables, tables, tableColumns, primaryKeys);

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
                "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.SqlCeExtractDatabaseTables.sql"));

            sqlQuery.AppendLine(
                Utility.GetResource(Assembly.GetExecutingAssembly(),
                "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.SqlCeExtractDatabaseTableColumns.sql"));

            sqlQuery.AppendLine(
                Utility.GetResource(Assembly.GetExecutingAssembly(),
                "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.SqlCeExtractDatabasePrimaryKeys.sql"));

            //sqlQuery.AppendLine(
            //    Utility.GetResource(Assembly.GetExecutingAssembly(),
            //    "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.SqlCeExtractDatabaseForeignKeys.sql"));

            //sqlQuery.AppendLine(
            //    Utility.GetResource(Assembly.GetExecutingAssembly(),
            //    "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.SqlCeExtractDatabaseViews.sql"));

            //sqlQuery.AppendLine(
            //    Utility.GetResource(Assembly.GetExecutingAssembly(),
            //    "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.SqlCeExtractDatabaseViewColumns.sql"));

            //sqlQuery.AppendLine(
            //    Utility.GetResource(Assembly.GetExecutingAssembly(),
            //    "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.SqlCeExtractDatabaseFunctions.sql"));

            //sqlQuery.AppendLine(
            //    Utility.GetResource(Assembly.GetExecutingAssembly(),
            //    "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.SqlCeExtractDatabaseProcedures.sql"));

            //sqlQuery.AppendLine(
            //    Utility.GetResource(Assembly.GetExecutingAssembly(),
            //    "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.SqlCeExtractDatabaseParameters.sql"));

            return sqlQuery.ToString();
        }

        private void PopulateTheSchemaTables(string schemaQuery, DataTable[] tables)
        {
            string sqlQuery;

            using (SqlCeConnection connection = new SqlCeConnection(m_ConnectionString))
            {
                connection.Open();
                SqlCeDataAdapter adapter = new SqlCeDataAdapter();

                sqlQuery =
                     Utility.GetResource(Assembly.GetExecutingAssembly(),
                     "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.SqlCeExtractDatabaseTables.sql");

                DataTable tblTables = new DataTable("Tables");
                adapter.SelectCommand = new SqlCeCommand(sqlQuery, connection);
                adapter.Fill(tables[0]);

                sqlQuery =
                    Utility.GetResource(Assembly.GetExecutingAssembly(),
                    "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.SqlCeExtractDatabaseTableColumns.sql");

                DataTable tblColumns = new DataTable("Columns");
                adapter.SelectCommand.CommandText = sqlQuery;
                adapter.Fill(tables[1]);

                sqlQuery =
                    Utility.GetResource(Assembly.GetExecutingAssembly(),
                    "TotalSafety.DataTierGenerator.SchemaExtractor.EmbeddedResources.SqlCeExtractDatabasePrimaryKeys.sql");

                DataTable tblKeys = new DataTable("Keys");
                adapter.SelectCommand.CommandText = sqlQuery;
                adapter.Fill(tables[2]);

                adapter.Dispose();
                connection.Close();
            }

        }

        private void LoadTables(XmlElement tablesElement, DataTable tables, DataTable tableColumns, DataTable primaryKeys)
        {
            XmlDocument xDoc = tablesElement.OwnerDocument;
            XmlElement xmlElement;
            XmlElement tableElement;
            XmlElement columnElement;

            DataView tablesView = new DataView(tables);
            DataView tableColumnsView;
            DataView primaryKeyView;
            string tableName;

            foreach (DataRowView drv in tablesView)
            {
                tableName = drv["TABLE_NAME"].ToString();
                tableElement = xDoc.CreateElement("table");
                tableElement.Attributes.Append(GetXmlAttribute(xDoc, "name", tableName));
                tableElement.Attributes.Append(GetXmlAttribute(xDoc, "create_date", drv["DATE_CREATED"].ToString()));
                tableElement.Attributes.Append(GetXmlAttribute(xDoc, "modify_date", drv["DATE_MODIFIED"].ToString()));

                #region get the table columns

                xmlElement = xDoc.CreateElement("columns");
                tableColumnsView = new DataView(tableColumns, string.Format("TABLE_NAME = '{0}'", tableName), "", DataViewRowState.CurrentRows);
                LoadColumns(xmlElement, tableColumnsView);
                tableElement.AppendChild(xmlElement);

                #endregion

                #region get the primary key columns

                primaryKeyView = new DataView(primaryKeys, string.Format("TABLE_NAME = '{0}'", tableName), "", DataViewRowState.CurrentRows);
                if (primaryKeyView.Count > 0)
                {
                    xmlElement = xDoc.CreateElement("primary_key");
                    xmlElement.Attributes.Append(GetXmlAttribute(xDoc, "name", primaryKeyView[0]["INDEX_NAME"].ToString()));
                    foreach (DataRowView drvP in primaryKeyView)
                    {
                        columnElement = xDoc.CreateElement("pk_column");
                        columnElement.Attributes.Append(GetXmlAttribute(xDoc, "column_name", drvP["COLUMN_NAME"].ToString()));
                        columnElement.Attributes.Append(GetXmlAttribute(xDoc, "key_ordinal", drvP["ORDINAL_POSITION"].ToString()));

                        xmlElement.AppendChild(columnElement);
                    }
                    tableElement.AppendChild(xmlElement);
                }

                #endregion

                tablesElement.AppendChild(tableElement);
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
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "name", drvC["COLUMN_NAME"].ToString()));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "ordinal_position", drvC["ORDINAL_POSITION"].ToString()));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "column_hasdefault", drvC["COLUMN_HASDEFAULT"].ToString()));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "column_default", drvC["COLUMN_DEFAULT"].ToString()));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "data_type", drvC["DATA_TYPE"].ToString()));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "max_length", drvC["CHARACTER_MAXIMUM_LENGTH"].ToString()));
                //columnElement.Attributes.Append(GetXmlAttribute(xDoc, "character_octet_length", drvC["CHARACTER_OCTET_LENGTH"].ToString()));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "precision", drvC["NUMERIC_PRECISION"].ToString()));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "scale", drvC["NUMERIC_SCALE"].ToString()));
                //columnElement.Attributes.Append(GetXmlAttribute(xDoc, "datetime_precision", drvC["DATETIME_PRECISION"].ToString()));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "is_nullable", (drvC["IS_NULLABLE"].ToString() == "YES" ? "true" : "false")));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "autoinc_increment", drvC["AUTOINC_INCREMENT"].ToString()));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "description", ""));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "is_rowguidcol", "false"));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "is_identity", ((drvC["AUTOINC_INCREMENT"] is System.DBNull) ? "false" : "true")));
                columnElement.Attributes.Append(GetXmlAttribute(xDoc, "is_computed", "false"));

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
