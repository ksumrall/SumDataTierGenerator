using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;

using TotalSafety.DataTierGenerator.Common;
using TotalSafety.DataTierGenerator.Factory;

namespace TotalSafety.DataTierGenerator
{
    public sealed class SchemaExtractor
    {

        #region private / protected member variables

        private string m_ConnectionString = "";
        private TableList m_TableList;
        private List<Object> m_ViewList;
        private List<Object> m_StoredProcedureList;

        XmlDocument m_DataMappingXml;

        //private string m_DataLayerOutputPath = "";
        //private string m_DataLayerNamespace = "";

        #endregion

        #region constructors / desturctors

        public SchemaExtractor() {

            string dataMapping = Utility.GetResource("TotalSafety.DataTierGenerator.Resource.DataMapping.xml");

            m_DataMappingXml = new XmlDocument();
            m_DataMappingXml.LoadXml(dataMapping);

            m_TableList = new TableList();
            m_ViewList = new List<object>();
            m_StoredProcedureList = new List<object>();

        }

        public SchemaExtractor(string connectionString)
            : this() {

            m_ConnectionString = connectionString;

        }

        #endregion

        #region public properties

        public string ConnectionString {
            get {
                return m_ConnectionString;
            }
            set {
                m_ConnectionString = value;
            }
        }

        public TableList TableList {
            get {
                return m_TableList;
            }
            set {
                m_TableList = value;
            }
        }

        public List<Object> ViewList {
            get {
                return m_ViewList;
            }
            set {
                m_ViewList = value;
            }
        }

        public List<Object> StoredProcedureList {
            get {
                return m_StoredProcedureList;
            }
            set {
                m_StoredProcedureList = value;
            }
        }

        //public string DatalayerOutputPath {
        //    get {
        //        return m_DataLayerOutputPath;
        //    }
        //    set {
        //        m_DataLayerOutputPath = value;
        //    }
        //}

        //public string BusinessLayerOutputPath {
        //    get {
        //        return m_BusinessLayerOutputPath;
        //    }
        //    set {
        //        m_BusinessLayerOutputPath = value;
        //    }
        //}

        //public string RootNamespace {
        //    get {
        //        return m_RootNamespace;
        //    }
        //    set {
        //        m_RootNamespace = value;
        //    }
        //}

        //public string BusinessLayerNamespace {
        //    get {
        //        return m_BusinessLayerNamespace;
        //    }
        //    set {
        //        m_BusinessLayerNamespace = value;
        //    }
        //}

        //public string DataLayerNamespace {
        //    get {
        //        return m_DataLayerNamespace;
        //    }
        //    set {
        //        m_DataLayerNamespace = value;
        //    }
        //}

        #endregion

        #region public methods

        public void ExtractSchema() {

            string query;

            DataTable tableAndColumnSchema;
            DataTable tablePrimaryKeySchema;
            DataTable tableForeignKeySchema;
            DataTable viewSchema;
            DataTable storedProcedureSchema;

            SqlDataAdapter dataAdapter;

            if (string.IsNullOrEmpty(m_ConnectionString)) {
                throw new Exception("The connection string is not set. The connection string needs to be set in order to communicate with the database.");
            }

            #region get all the schemas needed for processing

            using (SqlConnection connection = new SqlConnection(m_ConnectionString)) {

                connection.Open();

                // Table and Column Schema
                tableAndColumnSchema = new DataTable();
                query =
                    Utility.GetResource("ExtractDatabaseTableAndColumnSchema.sql");
                dataAdapter =
                    new SqlDataAdapter(query, connection);
                dataAdapter.Fill(tableAndColumnSchema);

                // Primary Key Schema
                tablePrimaryKeySchema = new DataTable();
                query =
                    Utility.GetResource("ExtractDatabasePrimaryKeySchema.sql");
                dataAdapter.SelectCommand.CommandText = query;
                dataAdapter.Fill(tablePrimaryKeySchema);

                // Foreign Key Schema
                tableForeignKeySchema = new DataTable();
                query =
                    Utility.GetResource("ExtractDatabaseForeignKeySchema.sql");
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

        public void ExtractSchema(string connectionString) {

            m_ConnectionString = connectionString;

            ExtractSchema();
        }

        #region commented code, saved as a reference for generating the DAL. When the new DAL generation is coded, delete this.

        //public XmlDocument Generate()
        //{
        //    StreamWriter streamWriter;
        //    string fileContents;
        //    string directoryPath;
        //    string fullFileName;

        //    XmlDocument xmlDoc = GenerateProjectXmlFile( );

        //    List<Table> tableList = new List<Table>( );

        //    //tableList = GetTableSchemas( ConnectionString );

        //    // Generate the necessary C# code for each table
        //    if (tableList.Count > 0) {

        //        CsEntityGenerator entityGenerator = new CsEntityGenerator();

        //        #region ensure all the directories are created before creating files

        //        directoryPath = m_DataLayerOutputPath;
        //        // create the directory if it does not exist
        //        if ( !Directory.Exists( directoryPath ) ) {
        //            Directory.CreateDirectory( directoryPath );
        //        }

        //        directoryPath = m_DataLayerOutputPath + "Common\\";
        //        // create the directory if it does not exist
        //        if ( !Directory.Exists( directoryPath ) ) {
        //            Directory.CreateDirectory( directoryPath );
        //        }

        //        //directoryPath = m_DataLayerOutputPath + "Entity\\";
        //        //// create the directory if it does not exist
        //        //if ( !Directory.Exists( directoryPath ) ) {
        //        //    Directory.CreateDirectory( directoryPath );
        //        //}

        //        //directoryPath = m_DataLayerOutputPath + "Entity\\Abstract\\";
        //        //// create the directory if it does not exist
        //        //if ( !Directory.Exists( directoryPath ) ) {
        //        //    Directory.CreateDirectory( directoryPath );
        //        //}

        //        //directoryPath = m_BusinessLayerOutputPath + "Entity\\";
        //        //// create the directory if it does not exist
        //        //if ( !Directory.Exists( directoryPath ) ) {
        //        //    Directory.CreateDirectory( directoryPath );
        //        //}

        //        //directoryPath = m_BusinessLayerOutputPath + "Entity\\Abstract\\";
        //        //// create the directory if it does not exist
        //        //if ( !Directory.Exists( directoryPath ) ) {
        //        //    Directory.CreateDirectory( directoryPath );
        //        //}

        //        #endregion

        //        #region create the common data layer files

        //        // create FieldDefinition
        //        fileContents = Utility.GetResource( "TotalSafety.DataTierGenerator.Resource.FieldDefinition.cs" );
        //        streamWriter = new StreamWriter( m_DataLayerOutputPath + "Common\\FieldDefinition.cs" );
        //        //fileContents = fileContents.Replace( "#ROOT_NAMESPACE#", RootNamespace );
        //        streamWriter.Write( fileContents );
        //        streamWriter.Close( );

        //        // create the TypeDefaultValue
        //        fileContents = Utility.GetResource( "TotalSafety.DataTierGenerator.Resource.TypeDefaultValue.cs" );
        //        streamWriter = new StreamWriter( m_DataLayerOutputPath + "Common\\TypeDefaultValue.cs" );
        //        //fileContents = fileContents.Replace( "#ROOT_NAMESPACE#", RootNamespace );
        //        streamWriter.Write( fileContents );
        //        streamWriter.Close( );

        //        // create the TypeDefaultValue
        //        fileContents = Utility.GetResource( "TotalSafety.DataTierGenerator.Resource.GatewayHelper.cs" );
        //        streamWriter = new StreamWriter( m_DataLayerOutputPath + "Common\\GatewayHelper.cs" );
        //        //fileContents = fileContents.Replace( "#ROOT_NAMESPACE#", RootNamespace );
        //        streamWriter.Write( fileContents );
        //        streamWriter.Close( );

        //        #endregion

        //        // Create everything we need
        //        foreach (Table table in tableList) {

        //            #region create the Abstract Gateway for the table

        //            AbstractGatewayGenerator abstractGatewayGenerator =
        //                new AbstractGatewayGenerator( DataLayerNamespace, table );

        //            directoryPath = m_DataLayerOutputPath;
        //            fullFileName = directoryPath + abstractGatewayGenerator.CLASS_NAME + "_Generated.cs";

        //            fileContents = abstractGatewayGenerator.ToString( );
        //            File.WriteAllText(fullFileName, fileContents);

        //            #endregion

        //            #region create the Concrete Gateway for the table

        //            ConcreteGatewayGenerator concreteGatewayGenerator =
        //                new ConcreteGatewayGenerator(DataLayerNamespace, table);

        //            directoryPath = m_DataLayerOutputPath;
        //            fullFileName = directoryPath + concreteGatewayGenerator.CLASS_NAME + ".cs";

        //            if ( !File.Exists( fullFileName ) ) {
        //                fileContents = concreteGatewayGenerator.ToString( );
        //                File.WriteAllText( fullFileName, fileContents );
        //            }

        //            #endregion

        //            // create the Abstract Data Entity for the table
        //            entityGenerator.GenerateAbstractEntity(table, DataLayerNamespace, m_DataMappingXml, m_DataLayerOutputPath);
        //            // create the Concrete Data Entity for the table
        //            //entityGenerator.GenerateEntity(table, RootNamespace, m_DataLayerOutputPath + "Entity\\");

        //            #region create the Abstract Business Entity for the table

        //            //AbstractBusinessEntityGenerator abstractBusinessEntityGenerator =
        //            //    new AbstractBusinessEntityGenerator( RootNamespace, table );

        //            //directoryPath = m_BusinessLayerOutputPath + "Entity\\Abstract\\";
        //            //fullFileName = directoryPath + abstractBusinessEntityGenerator.CLASS_NAME + ".cs";

        //            //fileContents = abstractBusinessEntityGenerator.ToString( );
        //            //File.WriteAllText( fullFileName, fileContents );

        //            #endregion

        //            #region create the Concrete Business Entity for the table

        //            //ConcreteBusinessEntityGenerator concreteBusinessEntityGenerator =
        //            //    new ConcreteBusinessEntityGenerator( RootNamespace, table );

        //            //directoryPath = m_BusinessLayerOutputPath + "Entity\\";
        //            //fullFileName = directoryPath + concreteBusinessEntityGenerator.CLASS_NAME + ".cs";

        //            //if ( !File.Exists( fullFileName ) ) {
        //            //    fileContents = concreteBusinessEntityGenerator.ToString( );
        //            //    File.WriteAllText( fullFileName, fileContents );
        //            //}

        //            #endregion

        //        }
        //    }

        //    return xmlDoc;
        //}

        #endregion

        #endregion

        #region private implementation

        private void BuildTableList(DataTable tableAndColumnSchema
            , DataTable tablePrimaryKeySchema, DataTable tableForeignKeySchema) {

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
            foreach (DataRow dataRow in tableAndColumnSchema.Rows) {

                curSchemaName = (string)dataRow["SchemaName"];
                curTableName = (string)dataRow["TableName"];

                if (!curSchemaName.Equals(prevSchemaName) || !curTableName.Equals(prevTableName)) {

                    prevSchemaName = curSchemaName;
                    prevTableName = curTableName;

                    tableColumnView = tableAndColumnSchema.DefaultView;
                    tableColumnView.RowFilter = 
                        "SchemaName=\'" + curSchemaName + "\' And TableName=\'" + curTableName + "\'";

                    primaryKeyView = tablePrimaryKeySchema.DefaultView;
                    primaryKeyView.RowFilter =
                        "SchemaName=\'" + curSchemaName + "\' And TableName=\'" + curTableName + "\'";

                    table = CreateTable(bldr.DataSource, tableColumnView);

                    if (primaryKeyView.Count > 0) {
                        AddPrimaryKey(table, primaryKeyView);
                    }

                    m_TableList.Add(table);
                }
                
            }

            prevSchemaName = "";
            prevTableName = "";

            foreach (Table curTable in m_TableList) {

                curSchemaName = curTable.Schema;
                curTableName = curTable.Name;

                if (!curSchemaName.Equals(prevSchemaName) || !curTableName.Equals(prevTableName)) {

                    prevSchemaName = curSchemaName;
                    prevTableName = curTableName;

                    foreignKeyView = tableForeignKeySchema.DefaultView;
                    foreignKeyView.RowFilter =
                        "SchemaName=\'" + curSchemaName + "\' And TableName=\'" + curTableName + "\'";

                    if (foreignKeyView.Count > 0) {
                        AddForeignKeyList(curTable, foreignKeyView);
                    }

                }

            }

        }

        private void ExtractViewSchema() {
        }

        private void ExtractStoredProcedureSchema() {
        }

        private Table CreateTable(string dataSource, DataView tableColumnView) {

            Table table = new Table();

            table.DatabaseName = dataSource;
            table.Schema = (string)tableColumnView[0]["SchemaName"];
            table.Name = (string)tableColumnView[0]["TableName"];

            if (tableColumnView[0]["TableDescription"] != DBNull.Value) {
                table.Description = (string)tableColumnView[0]["TableDescription"];
            }

            foreach (DataRowView drv in tableColumnView) {
                table.Columns.Add(CreateColumn(drv));
            }

            return table;

        }

        private Column CreateColumn(DataRowView tableColumnRowView) {

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
            if (tableColumnRowView["ColumnDescription"] != DBNull.Value) {
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
            if (tableColumnRowView["DefaultValue"] != DBNull.Value) {
                column.DefaultValue = (string)tableColumnRowView["DefaultValue"];
            } else {
                column.DefaultValue = "";
            }

            return column;
        
        }

        private void AddPrimaryKey(Table table, DataView primaryKeyView) {

            Index pkIndex;

            pkIndex = new Index();

            pkIndex.Name = (string)primaryKeyView[0]["IndexName"];
            pkIndex.ParentTable = table;

            primaryKeyView.Sort = "Ordinal";

            foreach (DataRowView drv in primaryKeyView) {

                foreach (Column column in table.Columns) {

                    if (column.Name.Equals((string)drv["ColumnName"])) {
                        pkIndex.Columns.Add(column);
                    }

                }

            }

            table.PrimaryKey = pkIndex;

        }

        private void AddForeignKeyList(Table table, DataView foreignKeyView) {

            ForeignKeyIndex fkIndex;

            if (foreignKeyView != null && foreignKeyView.Count > 0) {

                #region create the first index from the first foreign key record

                fkIndex = CreateForeignKeyIndex(table, foreignKeyView[0]);

                table.ForeignKeys.Add(fkIndex);

                #endregion

                // cycle through the remaining records either creating an new index or
                // adding additional columns to the current index
                for (int index = 1; index < foreignKeyView.Count; index++) {

                    if (fkIndex.Name != (string)foreignKeyView[index]["IndexName"]) {

                        fkIndex = CreateForeignKeyIndex(table, foreignKeyView[index]);

                        table.ForeignKeys.Add(fkIndex);
                    
                    } else {
                        fkIndex.Columns.Add(table.Columns[(string)foreignKeyView[index]["ColumnName"]]);
                    }

                }

            }

        }

        private ForeignKeyIndex CreateForeignKeyIndex(Table table, DataRowView foreignKeyRowView) {

            ForeignKeyIndex fkIndex;

            fkIndex = new ForeignKeyIndex();

            fkIndex.Name = (string)foreignKeyRowView["IndexName"];;
            fkIndex.ParentTable = table;
            fkIndex.ReferencedTable =
                m_TableList.FindTable((string)foreignKeyRowView["ReferencedSchemaName"]
                , (string)foreignKeyRowView["ReferencedTableName"]);
            fkIndex.ReferencedPrimaryKey = fkIndex.ReferencedTable.PrimaryKey;

            fkIndex.Columns.Add(table.Columns[(string)foreignKeyRowView["ColumnName"]]);

            return fkIndex;

        }

        #endregion

    }
}