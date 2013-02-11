using System;
using System.Collections.Generic;
using System.Text;

using SumDataTierGenerator.Common;

namespace SumDataTierGenerator.CodeGenerationFactory
{

    public class GeneratedTableGateway : GeneratorBase
    {

        #region private and protected member variables

        private Table m_Table;

        // table field substitutions
        private string m_COLUMN_INDEX_ENUMERATION;

        // primary key related substitutions
        private string m_PK_COLUMN_COUNT;
        private string m_PK_ARGUMENT_LIST;
        private string m_PK_PARAMETER_LIST;
        private string m_PK_PARAMETER_TYPE_LIST;
        private string m_PK_WHERE_FILTER;

        private string m_PK_COLUMN_DEFINITIONS_INITIALIZATION;
        private string m_PK_COLUMN_VALUES_INITIALIZATION;


        private string m_COLUMN_DEFINITION_INITIALIZATION;
        private string m_PK_COLUMN_DEFINITION_INITIALIZATION;

        private string m_SELECT_BY_PRIMARY_KEY_METHOD;

        private string m_NEW_DATA_OBJECT_FROM_READER;

        #endregion

        #region constructors / desturctors

        public GeneratedTableGateway()
            : this(null)
        {
        }

        public GeneratedTableGateway(string rootNamespace)
            : this(rootNamespace, null)
        {
        }

        public GeneratedTableGateway(string rootNamespace, string providerType)
            : this(rootNamespace, providerType, null)
        {
        }

        public GeneratedTableGateway(string rootNamespace, string providerType, Table table)
            : base(rootNamespace, providerType, table)
        {

            m_Table = table;
            string pkType = PK_PARAMETER_TYPE_LIST;

            // TODO: Remove following code if removing IGateway is the right choice
            /*
            if (!string.IsNullOrEmpty(pkType))
            {
                this.SUBCLASS_NAME = "IGateway<#CLASS_NAME_PREFIX#DataObject,"
                    + PK_PARAMETER_TYPE_LIST
                    + ">";
            }
            else
            {
                this.SUBCLASS_NAME = "IGateway<#CLASS_NAME_PREFIX#DataObject,int>";
            }
            */

        }

        #endregion

        #region public properties

        /// <summary>
        /// returns the name of the file that is gnerated
        /// </summary>
        public override string FILE_NAME
        {
            get
            {
                if (String.IsNullOrEmpty(m_FILE_NAME))
                {
                    m_FILE_NAME = CLASS_NAME + "_Generated.cs";
                }

                return m_FILE_NAME;
            }
        }

        public override IView IView
        {
            get
            {
                return m_IView;
            }
            set
            {
                base.IView = value;

                m_COLUMN_INDEX_ENUMERATION = "";
                m_COLUMN_DEFINITION_INITIALIZATION = "";

                // primary key related substitutions
                m_PK_COLUMN_COUNT = "";
                m_PK_ARGUMENT_LIST = "";
                m_PK_PARAMETER_LIST = "";
                m_PK_WHERE_FILTER = "";

                m_PK_COLUMN_VALUES_INITIALIZATION = "";
                m_PK_COLUMN_DEFINITIONS_INITIALIZATION = "";
                m_PK_COLUMN_DEFINITION_INITIALIZATION = "";

                m_SELECT_BY_PRIMARY_KEY_METHOD = "";

                m_NEW_DATA_OBJECT_FROM_READER = "";
            }
        }

        /// <summary>
        /// pascal case table name with spaces as underline
        /// </summary>
        public override string CLASS_NAME
        {
            get
            {
                if (String.IsNullOrEmpty(m_CLASS_NAME))
                {
                    m_CLASS_NAME = Utility.FormatPascal(base.CONCRETE_GATEWAY_TYPE_NAME);
                }

                return m_CLASS_NAME;
            }
        }

        /// <summary>
        /// number of columns making up the primary key
        /// </summary>
        public string PK_COLUMN_COUNT
        {
            get
            {
                if (m_PK_COLUMN_COUNT == "")
                {
                    if (m_Table.PrimaryKey != null && m_Table.PrimaryKey.PkColumns.Length > 0)
                    {
                        m_PK_COLUMN_COUNT = m_Table.PrimaryKey.PkColumns.Length.ToString();
                    }
                }

                return m_PK_COLUMN_COUNT;
            }
        }

        /// <summary>
        /// number of columns making up the primary key
        /// </summary>
        public string PK_ARGUMENT_LIST
        {
            get
            {
                if (m_PK_ARGUMENT_LIST == "")
                {

                    if (m_Table.PrimaryKey != null && m_Table.PrimaryKey.PkColumns.Length > 0)
                    {
                        PkColumn[] pkList = m_Table.PrimaryKey.PkColumns;
                        int columnCount = m_Table.PrimaryKey.PkColumns.Length;

                        for (int index = 0; index < columnCount; index++)
                        {
                            m_PK_ARGUMENT_LIST += Utility.FormatCamel(m_Table.GetPkColumn(pkList[index].ColumnName).PropertyName);
                            if (index < columnCount - 1)
                            {
                                m_PK_ARGUMENT_LIST += ",";
                            }
                        }
                    }
                }

                return m_PK_ARGUMENT_LIST;
            }
        }

        /// <summary>
        /// number of columns making up the primary key
        /// </summary>
        public string PK_PARAMETER_LIST
        {
            get
            {
                if (m_PK_PARAMETER_LIST == "")
                {

                    if (m_Table.PrimaryKey != null && m_Table.PrimaryKey.PkColumns.Length > 0)
                    {
                        PkColumn[] pkList = m_Table.PrimaryKey.PkColumns;
                        int columnCount = m_Table.PrimaryKey.PkColumns.Length;
                        Column pkColumn;

                        for (int index = 0; index < columnCount; index++)
                        {
                            pkColumn = m_Table.GetPkColumn(pkList[index].ColumnName);
                            m_PK_PARAMETER_LIST += pkColumn.LanguageType + " "
                            + Utility.FormatCamel(pkColumn.PropertyName);
                            if (index < columnCount - 1)
                            {
                                m_PK_PARAMETER_LIST += ",";
                            }
                        }
                    }
                }

                return m_PK_PARAMETER_LIST;
            }
        }

        public string PK_PARAMETER_TYPE_LIST
        {
            get
            {
                if (string.IsNullOrEmpty(m_PK_PARAMETER_TYPE_LIST))
                {

                    if (m_Table.PrimaryKey != null && m_Table.PrimaryKey.PkColumns.Length > 0)
                    {
                        PkColumn[] pkList = m_Table.PrimaryKey.PkColumns;
                        int columnCount = m_Table.PrimaryKey.PkColumns.Length;

                        for (int index = 0; index < columnCount; index++)
                        {
                            m_PK_PARAMETER_TYPE_LIST += m_Table.GetPkColumn(pkList[index].ColumnName).LanguageType;
                            if (index < columnCount - 1)
                            {
                                m_PK_PARAMETER_TYPE_LIST += ",";
                            }
                        }
                    }
                }

                return m_PK_PARAMETER_TYPE_LIST;
            }
        }

        public string PK_WHERE_FILTER
        {
            get
            {
                if (m_PK_WHERE_FILTER == "")
                {

                    if (m_Table.PrimaryKey != null && m_Table.PrimaryKey.PkColumns.Length > 0)
                    {

                        PkColumn[] pkList = m_Table.PrimaryKey.PkColumns;
                        int columnCount = m_Table.PrimaryKey.PkColumns.Length;
                        Column pkColumn;

                        for (int index = 0; index < columnCount; index++)
                        {
                            pkColumn = m_Table.GetPkColumn(pkList[index].ColumnName);
                            m_PK_WHERE_FILTER += "[" + pkColumn.Name + "] = @"
                            + pkColumn.PropertyName;
                            if (index < columnCount - 1)
                            {
                                m_PK_WHERE_FILTER += " AND ";
                            }
                        }
                    }
                }

                return m_PK_WHERE_FILTER;
            }
        }

        #endregion

        #region event handlers / overrides

        public override string ToString()
        {

            base.CLASS_SUMMARY = "Table Data Gateway class that provides CRUD functionality for the #TABLE_NAME# table.";

            m_FileBuilder.Replace("#PK_PARAMETER_LIST#", PK_PARAMETER_LIST);

            return base.ToString();
        }

        protected override void OnGetFileComments()
        {

            AppendLine("// THIS IS AN AUTOMATICALLY GENERATED FILE.");
            AppendLine("// It contains the implementations of the class #CLASS_NAME#.");
            AppendLine("// Do not make modifications to this file since they will be overwritten");
            AppendLine("// when this file is regenerated. Instead, make your modifications");
            AppendLine("// to the file without the \'_Generated\' extension.");
            AppendLine("//");

            base.OnGetFileComments();

        }

        protected override void OnGetUsingStatements()
        {
            base.OnGetUsingStatements();

            AppendLine("using System.Collections.Generic;");
            AppendLine("using System.Data;");
            AppendLine("using System.Data.SqlClient;");
            AppendLine("using System.Text;");

        }

        protected override void OnPreGetClass()
        {

            Column[] columns = m_IView.Columns;
            int columnCount = m_IView.Columns.Length;

            AppendLine();
            AppendLine("public enum #CLASS_NAME_PREFIX#FieldIndex{");

            this.IndentIncrement();

            for (int index = 0; index < columnCount; index++)
            {
                AppendStartLine((columns[index]).PropertyName);
                if (index < columnCount - 1)
                {
                    Append(",");
                }
                AppendEndLine("");
            }

            this.IndentDecrement();

            AppendLine("};");

            base.OnPreGetClass();
        }

        protected override void OnGetRegion_PrivateProtectedMemberVariables()
        {

            Column[] columns = m_IView.Columns;
            int columnCount = m_IView.Columns.Length;

            AddRegionHeader("private and protected member variables");

            AppendLine("private static readonly #CLASS_NAME# m_Instance = new #CLASS_NAME#();");
            AppendLine("private static FieldDefinitionGroup m_FieldDefinitions = new FieldDefinitionGroup();");

            AddRegionFooter();

        }

        protected override void OnGetRegion_InternalStructuredMembers()
        {

            AddRegionHeader("internal structured members");

            AddFieldDefinitionClassDefinition();

            base.OnGetRegion_InternalStructuredMembers();

            AddRegionFooter();

        }

        protected override void OnGetRegion_PublicProperties()
        {

            AddRegionHeader("properties");

            #region Instance

            AppendLine();
            AppendLine("public static #CLASS_NAME# Instance {");
            IndentIncrement();
            AppendLine("get {");
            IndentIncrement();
            AppendLine("return m_Instance;");
            IndentDecrement();
            AppendLine("}");
            IndentDecrement();
            AppendLine("}");

            #endregion

            #region static FieldDefinitionArray

            AppendLine();
            AppendLine("public static FieldDefinition[] FieldDefinitionArray {");
            IndentIncrement();
            AppendLine("get {");
            IndentIncrement();
            AppendLine("return m_FieldDefinitions.FieldDefinitionArray;");
            IndentDecrement();
            AppendLine("}");
            IndentDecrement();
            AppendLine("}");

            #endregion

            #region static FieldDefinitions

            AppendLine();
            AppendLine("public static FieldDefinitionGroup FieldDefinitions {");
            IndentIncrement();
            AppendLine("get {");
            IndentIncrement();
            AppendLine("return m_FieldDefinitions;");
            IndentDecrement();
            AppendLine("}");
            IndentDecrement();
            AppendLine("}");

            #endregion

            #region static DatabaseName

            AppendLine();
            AppendLine("public static string DatabaseName {");
            IndentIncrement();
            AppendLine("get {");
            IndentIncrement();
            AppendLine("return \"" + m_IView.DatabaseName + "\";");
            IndentDecrement();
            AppendLine("}");
            IndentDecrement();
            AppendLine("}");

            #endregion

            #region static SchemaName

            AppendLine();
            AppendLine("public static string SchemaName {");
            IndentIncrement();
            AppendLine("get {");
            IndentIncrement();
            AppendLine("return \"" + m_IView.Schema + "\";");
            IndentDecrement();
            AppendLine("}");
            IndentDecrement();
            AppendLine("}");

            #endregion

            #region static TableName

            AppendLine();
            AppendLine("public static string TableName {");
            IndentIncrement();
            AppendLine("get {");
            IndentIncrement();
            AppendLine("return \"[" + m_IView.Name + "]\";");
            IndentDecrement();
            AppendLine("}");
            IndentDecrement();
            AppendLine("}");

            #endregion

            #region static SchemaTableName

            AppendLine();
            AppendLine("public static string SchemaTableName {");
            IndentIncrement();
            AppendLine("get {");
            IndentIncrement();
            AppendLine("return SchemaName + \".\" + TableName;");
            IndentDecrement();
            AppendLine("}");
            IndentDecrement();
            AppendLine("}");

            #endregion

            #region static FullyQualifiedTableName

            AppendLine();
            AppendLine("public static string FullyQualifiedTableName {");
            IndentIncrement();
            AppendLine("get {");
            IndentIncrement();
            AppendLine("return DatabaseName + \".\" + SchemaName + \".\" + TableName;");
            IndentDecrement();
            AppendLine("}");
            IndentDecrement();
            AppendLine("}");

            #endregion

            AddRegionFooter();

        }

        protected override void OnGetRegion_InterfaceImplementationMethods()
        {

            string pkType = PK_PARAMETER_TYPE_LIST;
            if (string.IsNullOrEmpty(pkType))
            {
                pkType = "int";
            }

            AppendLine("#region IGateway Members");

            AppendLine();
            AppendLine("#region properties");

            #region IGateway<T>.FieldDefinitionArray

            AppendLine();
            AppendLine("FieldDefinition[] IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.FieldDefinitionArray {");
            IndentIncrement();
            AppendLine("get {");
            IndentIncrement();
            AppendLine("return FieldDefinitionArray;");
            IndentDecrement();
            AppendLine("}");
            IndentDecrement();
            AppendLine("}");

            #endregion

            #region IGateway<T>.DatabaseName

            AppendLine();
            AppendLine("string IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.DatabaseName {");
            IndentIncrement();
            AppendLine("get {");
            IndentIncrement();
            AppendLine("return DatabaseName;");
            IndentDecrement();
            AppendLine("}");
            IndentDecrement();
            AppendLine("}");

            #endregion

            #region IGateway<T>.SchemaName

            AppendLine();
            AppendLine("string IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.SchemaName {");
            IndentIncrement();
            AppendLine("get {");
            IndentIncrement();
            AppendLine("return SchemaName;");
            IndentDecrement();
            AppendLine("}");
            IndentDecrement();
            AppendLine("}");

            #endregion

            #region IGateway<T>.TableName

            AppendLine();
            AppendLine("string IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.TableName {");
            IndentIncrement();
            AppendLine("get {");
            IndentIncrement();
            AppendLine("return TableName;");
            IndentDecrement();
            AppendLine("}");
            IndentDecrement();
            AppendLine("}");

            #endregion

            #region IGateway<T>.SchemaTableName

            AppendLine();
            AppendLine("string IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.SchemaTableName {");
            IndentIncrement();
            AppendLine("get {");
            IndentIncrement();
            AppendLine("return SchemaTableName;");
            IndentDecrement();
            AppendLine("}");
            IndentDecrement();
            AppendLine("}");

            #endregion

            #region IGateway<T>.FullyQualifiedTableName

            AppendLine();
            AppendLine("string IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.FullyQualifiedTableName {");
            IndentIncrement();
            AppendLine("get {");
            IndentIncrement();
            AppendLine("return FullyQualifiedTableName;");
            IndentDecrement();
            AppendLine("}");
            IndentDecrement();
            AppendLine("}");

            #endregion

            AppendLine();
            AppendLine("#endregion");

            AppendLine();
            AppendLine("#region methods");

            #region IGateway<T>.SelectAll

            AppendLine();
            AppendLine("List<#CLASS_NAME_PREFIX#DataObject> IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.SelectAll(string connectionStringName){");
            IndentIncrement();
            AppendLine("return #CLASS_NAME#.SelectAll(connectionStringName);");
            IndentDecrement();
            AppendLine("}");

            #endregion

            #region IGateway<T>.SelectByPrimaryKey

            AppendLine();

            AppendLine("#CLASS_NAME_PREFIX#DataObject IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.SelectByPrimaryKey( " + pkType + " id ){");
            IndentIncrement();

            if (m_Table.PrimaryKey == null || m_Table.PrimaryKey.PkColumns.Length == 0)
            {
                AppendLine("return null;");
            }
            else
            {
                AppendLine("return #CLASS_NAME#.SelectByPrimaryKey( id );");
            }

            IndentDecrement();
            AppendLine("}");

            #endregion

            #region IGateway<T>.Insert

            AppendLine();
            AppendLine("void IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.Insert( string connectionStringName, #CONCRETE_DATA_ENTITY_TYPE_NAME# dataObject ){");
            IndentIncrement();
            AppendLine("#CLASS_NAME#.Insert( connectionStringName, dataObject );");
            IndentDecrement();
            AppendLine("}");

            #endregion

            #region IGateway<T>.Update

            AppendLine();
            AppendLine("void IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.Update( #CONCRETE_DATA_ENTITY_TYPE_NAME# dataObject ){");
            IndentIncrement();
            AppendLine("#CLASS_NAME#.Update( dataObject );");
            IndentDecrement();
            AppendLine("}");

            #endregion

            #region IGateway<T>.Delete

            AppendLine();
            AppendLine("void IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.Delete( #CONCRETE_DATA_ENTITY_TYPE_NAME# dataObject ){");
            IndentIncrement();
            AppendLine("#CLASS_NAME#.Delete( dataObject );");
            IndentDecrement();
            AppendLine("}");

            #endregion

            AppendLine();
            AppendLine("#endregion");

            AppendLine();
            AppendLine("#endregion");
            AppendLine();

        }

        protected override void OnGetRegion_PublicMethods()
        {

            AddRegionHeader("public methods");

            OnCRUD_SelectAll();
            OnCRUD_SelectByPrimaryKey();
            OnCRUD_LoadByPrimaryKey();
            OnCRUD_Insert();
            OnCRUD_Update();
            //OnCRUD_UpdateByPrimaryKey();
            OnCRUD_Delete();
            OnCRUD_DeleteByPrimaryKey();
            OnCRUD_DeleteByFieldsList();
            OnCRUD_DeleteByFieldsArray();

            AddRegionFooter();

        }

        protected override void OnGetRegion_PrivateMethods()
        {

            AddRegionHeader("private methods");

            AddMethod_GetDataObjectsByQuery();
            AddMethod_GetDataObjectsFromReader();
            AddMethod_GetDataObjectFromReader();
            AddMethod_LoadDataObjectByQuery();

            AddRegionFooter();

        }

        #endregion

        #region CRUD methods

        protected virtual void OnCRUD_SelectAll()
        {

            AppendLine();
            AppendLine("public static List<#CONCRETE_DATA_ENTITY_TYPE_NAME#> SelectAll(){");
            IndentIncrement();
            AppendLine();

            AppendLine("IDataReader dataReader = GatewayHelper.ExecuteReaderFromStoredProcedure(\"usp_zzz_autogen_crud__#TABLE_NAME#_SelectAll\");");
            AppendLine("return GetDataObjectsFromReader(dataReader);");

            AppendLine();
            IndentDecrement();
            AppendLine("}");

        }

        protected virtual void OnCRUD_SelectByPrimaryKey()
        {

            if (m_Table.PrimaryKey == null || m_Table.PrimaryKey.PkColumns.Length == 0)
            {
                return;
            }

            PkColumn[] pkList = m_Table.PrimaryKey.PkColumns;
            int columnCount = m_Table.PrimaryKey.PkColumns.Length;

            AppendLine();
            AppendStartLine("public static #CONCRETE_DATA_ENTITY_TYPE_NAME# SelectByPrimaryKey( ");
            Append(PK_PARAMETER_LIST);
            AppendEndLine(" ){");
            IndentIncrement();

            #region variable declaration

            AppendLine();
            AppendLine("#CONCRETE_DATA_ENTITY_TYPE_NAME# dataObject = null;");
            AppendLine("List<FieldValue> fieldValueList = new List<FieldValue>( );");
            AppendLine();

            #endregion

            #region initialization of the fielddefinitions and fieldvalues

            for (int index = 0; index < columnCount; index++)
            {
                AppendStartLine("fieldValueList.Add(");
                Append(" new FieldValue( m_FieldDefinitions.");
                Append(m_Table.GetPkColumn(pkList[index].ColumnName).PropertyName);
                Append(", ");
                Append(Utility.FormatCamel(m_Table.GetPkColumn(pkList[index].ColumnName).PropertyName));
                AppendEndLine(" ) );");
            }

            #endregion

            #region run query to get the object

            AppendLine();
            AppendLine("IDataReader dataReader = GatewayHelper.ExecuteReaderFromStoredProcedure(\"usp_zzz_autogen_crud__#TABLE_NAME#_SelectByPk\", fieldValueList);");
            AppendLine();
            AppendLine("if(dataReader.Read()){");
            IndentIncrement();
            AppendLine("dataObject = GetDataObjectFromReader(dataReader);");
            IndentDecrement();
            AppendLine("}");

            #endregion

            AppendLine();
            AppendLine("return dataObject;");
            IndentDecrement();
            AppendLine();
            AppendLine("}");

        }

        protected virtual void OnCRUD_LoadByPrimaryKey()
        {

            if (m_Table.PrimaryKey == null || m_Table.PrimaryKey.PkColumns.Length == 0)
            {
                return;
            }

            PkColumn[] pkList = m_Table.PrimaryKey.PkColumns;
            int columnCount = m_Table.PrimaryKey.PkColumns.Length;

            AppendLine();
            AppendStartLine("internal static void LoadByPrimaryKey( #CONCRETE_DATA_ENTITY_TYPE_NAME# dataObject, ");
            Append(PK_PARAMETER_LIST);
            AppendEndLine(" ){");
            IndentIncrement();
            AppendLine();
            AppendLine("string query;");
            AppendLine("List<FieldValue> fieldValueList = new List<FieldValue>();");

            #region initialization of the fielddefinitions and fieldvalues

            AppendLine();
            for (int index = 0; index < columnCount; index++)
            {
                AppendStartLine("fieldValueList.Add(");
                Append(" new FieldValue( m_FieldDefinitions.");
                Append(m_Table.GetPkColumn(pkList[index].ColumnName).PropertyName);
                Append(", ");
                Append(Utility.FormatCamel(m_Table.GetPkColumn(pkList[index].ColumnName).PropertyName));
                AppendEndLine(" ) );");
            }

            #endregion

            AppendLine();

            AppendLine();
            AppendLine("IDataReader dataReader = GatewayHelper.ExecuteReaderFromStoredProcedure(\"usp_zzz_autogen_crud__#TABLE_NAME#_SelectByPk\", fieldValueList);");
            AppendLine();
            AppendLine("if(dataReader.Read()){");
            IndentIncrement();
            AppendLine("GetDataObjectFromReader(dataObject, dataReader);");
            IndentDecrement();
            AppendLine("}");

            IndentDecrement();
            AppendLine("}");

        }

        protected virtual void OnCRUD_Insert()
        {

            AppendLine();
            AppendLine("public static #CONCRETE_DATA_ENTITY_TYPE_NAME# Insert( string connectionStringName, #CONCRETE_DATA_ENTITY_TYPE_NAME# dataObject ){");
            IndentIncrement();

            #region variable declaration

            AppendLine();
            AppendLine("#CONCRETE_DATA_ENTITY_TYPE_NAME# newDataObject = null;");

            #endregion

            #region run query to get the object

            AppendLine();
            AppendLine("IDataReader dataReader = GatewayHelper.ExecuteReaderFromStoredProcedure(\"usp_zzz_autogen_crud__#TABLE_NAME#_Insert\", ((IFieldValues)dataObject).FieldValues);");
            AppendLine();
            AppendLine("if(dataReader.Read()){");
            IndentIncrement();
            AppendLine("newDataObject = GetDataObjectFromReader(dataReader);");
            IndentDecrement();
            AppendLine("}");
            AppendLine();
            AppendLine("return newDataObject;");

            #endregion

            IndentDecrement();
            AppendLine();
            AppendLine("}");
        }

        protected virtual void OnCRUD_Update()
        {
            AppendLine();
            AppendLine("/// <summary>");
            AppendLine("/// This method updates the record from the underlying table where all the fields match.");
            AppendLine("/// If there is more than one record matching the filter, they all get updated.");
            AppendLine("/// </summary>");
            AppendLine("public static #CONCRETE_DATA_ENTITY_TYPE_NAME# Update( #CONCRETE_DATA_ENTITY_TYPE_NAME# dataObject ){");
            IndentIncrement();

            #region variable declaration

            AppendLine();
            AppendLine("#CONCRETE_DATA_ENTITY_TYPE_NAME# newDataObject = null;");

            #endregion

            #region run query to get the object

            AppendLine();
            AppendLine("IDataReader dataReader = GatewayHelper.ExecuteReaderFromStoredProcedure(\"usp_zzz_autogen_crud__#TABLE_NAME#_Update\", ((IFieldValues)dataObject).FieldValues);");
            AppendLine();
            AppendLine("if(dataReader.Read()){");
            IndentIncrement();
            AppendLine("newDataObject = GetDataObjectFromReader(dataReader);");
            IndentDecrement();
            AppendLine("}");
            AppendLine();
            AppendLine("return newDataObject;");

            #endregion

            IndentDecrement();
            AppendLine();
            AppendLine("}");
        }

        protected virtual void OnCRUD_UpdateByPrimaryKey()
        {

            if (m_Table.PrimaryKey == null || m_Table.PrimaryKey.PkColumns.Length == 0)
            {
                return;
            }

            PkColumn[] pkList = m_Table.PrimaryKey.PkColumns;
            int columnCount = m_Table.PrimaryKey.PkColumns.Length;

            AppendLine();
            AppendLine("public static void UpdateByPrimaryKey( #CONCRETE_DATA_ENTITY_TYPE_NAME# dataObject ){");
            IndentIncrement();

            #region variable declaration

            AppendLine();
            AppendLine("string query;");
            AppendLine("List<FieldDefinition> fieldDefinitions = new List<FieldDefinition>();");
            AppendLine("List<FieldValue> fields = new List<FieldValue>();");
            AppendLine();
            AppendLine("List<FieldDefinition> pkFieldDefinitions = new List<FieldDefinition>();");
            AppendLine("List<FieldValue> pkFields = new List<FieldValue>();");
            AppendLine();
            AppendLine("List<#CONCRETE_DATA_ENTITY_TYPE_NAME#> dataObjects;");

            #endregion

            #region initialization of the fielddefinitions and fieldvalues

            AppendLine();
            AppendLine("// gather all the changed fields");
            AppendLine("for( int index = 0; index < FieldDefinitions.FieldDefinitionArray.Length; index++ ){");
            IndentIncrement();
            AppendLine();

            AppendLine("if( ((IFieldValues)dataObject).FieldValues[index].IsDirty ){");
            IndentIncrement();
            AppendLine();

            AppendLine("fieldDefinitions.Add( FieldDefinitions.FieldDefinitionArray[index] );");
            AppendLine("fields.Add( ((IFieldValues)dataObject).FieldValues[index] );");

            AppendLine();
            IndentDecrement();
            AppendLine("}");

            AppendLine();
            IndentDecrement();
            AppendLine("}");

            AppendLine();
            switch (m_ProviderType)
            {
                case "Microsoft SQL Server (SqlClient)":
                    AppendLine("query = GatewayHelper.BuildUpdateByPrimaryKeyQuery( SchemaTableName, (IFieldValues)dataObject );");
                    break;

                case "Microsoft SQL Server Compact 3.5 (SqlCeClient)":
                    AppendLine("query = GatewayHelper.BuildUpdateByPrimaryKeyQuery( TableName, (IFieldValues)dataObject );");
                    break;
            }

            #endregion

            #region initialize the primary key

            AppendLine();
            AppendLine("// gather the primary key fields");
            AppendLine("for( int index = 0; index < FieldDefinitions.FieldDefinitionArray.Length; index++ ){");
            IndentIncrement();
            AppendLine();

            AppendLine("if( FieldDefinitions.FieldDefinitionArray[index].IsPrimaryKey ){");
            IndentIncrement();
            AppendLine();

            AppendLine("fieldDefinitions.Add( FieldDefinitions.FieldDefinitionArray[index] );");
            AppendLine("fields.Add( ((IFieldValues)dataObject).FieldValues[index] );");

            AppendLine();
            IndentDecrement();
            AppendLine("}");

            AppendLine();
            IndentDecrement();
            AppendLine("}");

            #endregion

            #region run query to get the object

            AppendLine();
            AppendLine("//if( pkFields.Count > 0 ){");
            IndentIncrement();
            AppendLine();

            AppendLine("//query += Environment.NewLine;");

            switch (m_ProviderType)
            {
                case "Microsoft SQL Server (SqlClient)":
                    AppendLine("//query += GatewayHelper.BuildSelectAllQuery( SchemaTableName, FieldDefinitions );");
                    break;

                case "Microsoft SQL Server Compact 3.5 (SqlCeClient)":
                    AppendLine("//query += GatewayHelper.BuildSelectAllQuery( TableName, FieldDefinitions );");
                    break;
            }

            AppendLine("//query += Environment.NewLine;");
            AppendLine("//query += GatewayHelper.BuildWhereClause( pkFieldDefinitions.ToArray() );");

            AppendLine();
            IndentDecrement();
            AppendLine("//}");
            AppendLine();
            AppendLine("dataObjects = #CLASS_NAME#.GetDataObjectsByQuery(query, fields );");
            AppendLine();
            AppendLine("if ( dataObjects.Count == 1 ) {");
            IndentIncrement();
            AppendLine("dataObject.IsDirty = false;");
            AppendLine();
            AppendLine("for ( int index = 0; index < FieldDefinitions.FieldDefinitionArray.Length; index++ ) {");
            IndentIncrement();
            AppendLine("((IFieldValues)dataObject).FieldValues[index].Value = ((IFieldValues)dataObjects[0]).FieldValues[index].Value;");
            AppendLine("((IFieldValues)dataObject).FieldValues[index].IsDirty = false;");
            AppendLine("dataObject.IsNew = false;");
            IndentDecrement();
            AppendLine("}");
            IndentDecrement();
            AppendLine("}");

            #endregion

            IndentDecrement();
            AppendLine();
            AppendLine("}");
        }

        protected virtual void OnCRUD_Delete()
        {

            AppendLine();
            AppendLine("/// <summary>");
            AppendLine("/// This method deletes the record from the underlying table where all the fields match.");
            AppendLine("/// If there is more than one record matching the filter, they all get deleted.");
            AppendLine("/// </summary>");
            AppendLine("public static void Delete( #CONCRETE_DATA_ENTITY_TYPE_NAME# dataObject ){");
            IndentIncrement();

            AppendLine();
            AppendLine("if ( dataObject.m_PrimaryKeyFieldValues.Length > 0 ) {");
            IndentIncrement();
            AppendLine("DeleteByFields(dataObject.m_PrimaryKeyFieldValues);");
            IndentDecrement();
            AppendLine("} else {");
            IndentIncrement();
            AppendLine("DeleteByFields(new List<FieldValue>(( (IFieldValues)dataObject ).FieldValues));");
            IndentDecrement();
            AppendLine("}");
            AppendLine();

            IndentDecrement();
            AppendLine("}");

        }

        protected virtual void OnCRUD_DeleteByPrimaryKey()
        {

            if (m_Table.PrimaryKey == null || m_Table.PrimaryKey.PkColumns.Length == 0)
            {
                return;
            }

            PkColumn[] pkList = m_Table.PrimaryKey.PkColumns;
            int columnCount = m_Table.PrimaryKey.PkColumns.Length;

            #region variable declaration

            AppendLine();
            AppendStartLine("public static void DeleteByPrimaryKey( ");
            Append(PK_PARAMETER_LIST);
            AppendEndLine(" ){");
            IndentIncrement();
            AppendLine();
            AppendLine("List<FieldValue> fieldValueList = new List<FieldValue>( );");

            #endregion

            #region initialization of the fielddefinitions and fieldvalues

            AppendLine();
            for (int index = 0; index < columnCount; index++)
            {
                AppendStartLine("fieldValueList.Add(");
                Append(" new FieldValue( m_FieldDefinitions.");
                Append(m_Table.GetPkColumn(pkList[index].ColumnName).PropertyName);
                Append(", ");
                Append(Utility.FormatCamel(m_Table.GetPkColumn(pkList[index].ColumnName).PropertyName));
                AppendEndLine(" ) );");
            }

            #endregion

            #region run query

            AppendLine();
            AppendLine("DeleteByFields(fieldValueList);");
            AppendLine();

            #endregion

            IndentDecrement();
            AppendLine("}");

        }

        protected virtual void OnCRUD_DeleteByFieldsList()
        {

            AppendLine();
            AppendLine("/// <summary>");
            AppendLine("/// This method deletes the record from the underlying table where all the fields match.");
            AppendLine("/// If there is more than one record matching the filter, they all get deleted.");
            AppendLine("/// </summary>");
            AppendLine("public static void DeleteByFields( List<FieldValue> fieldValueList ){");

            AppendLine();
            IndentIncrement();
            AppendLine("DeleteByFields(fieldValueList.ToArray());");

            IndentDecrement();
            AppendLine("}");

        }

        protected virtual void OnCRUD_DeleteByFieldsArray()
        {

            AppendLine();
            AppendLine("/// <summary>");
            AppendLine("/// This method deletes the record from the underlying table where all the fields match.");
            AppendLine("/// If there is more than one record matching the filter, they all get deleted.");
            AppendLine("/// </summary>");
            AppendLine("public static void DeleteByFields( FieldValue[] fieldValueArray ){");

            #region variable declaration

            AppendLine();
            IndentIncrement();
            AppendLine("StringBuilder deleteQuery = new StringBuilder();");

            #endregion

            #region build the query

            AppendLine();
            switch (m_ProviderType)
            {
                case "Microsoft SQL Server (SqlClient)":
                    AppendLine("deleteQuery.AppendLine(GatewayHelper.BuildDeleteQuery(SchemaTableName));");
                    break;

                case "Microsoft SQL Server Compact 3.5 (SqlCeClient)":
                    AppendLine("deleteQuery.AppendLine(GatewayHelper.BuildDeleteQuery(TableName));");
                    break;
            }
            AppendLine("deleteQuery.AppendLine(GatewayHelper.BuildWhereClause(fieldValueArray));");

            #endregion

            #region run query

            AppendLine();
            AppendLine("GatewayHelper.ExecuteNonQueryFromSql(deleteQuery.ToString(), fieldValueArray);");

            #endregion

            IndentDecrement();
            AppendLine("}");

        }

        #endregion

        #region private implementation

        protected void AddMethod_GetDataObjectsByQuery()
        {

            AppendLine();
            AppendLine("internal static List<#CONCRETE_DATA_ENTITY_TYPE_NAME#> GetDataObjectsByQuery(");
            IndentIncrement();
            AppendLine("string query, List<FieldValue> dirtyFieldValueList ) {");
            AppendLine();
            AppendLine("return GetDataObjectsByQuery( query, dirtyFieldValueList.ToArray() );");

            AppendLine();
            IndentDecrement();
            AppendLine("}");

            AppendLine();
            AppendLine("internal static List<#CONCRETE_DATA_ENTITY_TYPE_NAME#> GetDataObjectsByQuery(");
            IndentIncrement();
            AppendLine("string query, FieldValue[] dirtyFieldValues ) {");
            AppendLine();
            AppendLine("IDataReader dataReader = null;");
            AppendLine();
            AppendLine("#CONCRETE_DATA_ENTITY_TYPE_NAME# #CLASS_VARIABLE_NAME_PREFIX#DataObject;");
            AppendLine("List<#CONCRETE_DATA_ENTITY_TYPE_NAME#> dataObjects;");
            AppendLine();
            AppendLine("try {");
            IndentIncrement();
            AppendLine();
            AppendLine("dataObjects = new List<#CONCRETE_DATA_ENTITY_TYPE_NAME#>();");
            AppendLine();
            AppendLine("dataReader = GatewayHelper.ExecuteReaderFromSql( query, dirtyFieldValues );");
            AppendLine();
            AppendLine("while ( dataReader.Read( ) ) {");
            IndentIncrement();
            AppendLine();
            AppendLine("#CLASS_VARIABLE_NAME_PREFIX#DataObject = GetDataObjectFromReader( dataReader );");
            AppendLine();
            AppendLine("dataObjects.Add(#CLASS_VARIABLE_NAME_PREFIX#DataObject);");
            AppendLine();
            IndentDecrement();
            AppendLine("}");
            IndentDecrement();
            AppendLine("} finally {");
            IndentIncrement();
            AppendLine("if ( dataReader != null ) {");
            IndentIncrement();
            AppendLine("dataReader.Close( );");
            AppendLine("dataReader.Dispose( );");
            IndentDecrement();
            AppendLine("}");
            IndentDecrement();
            AppendLine("}");
            AppendLine();
            AppendLine("return dataObjects;");

            IndentDecrement();
            AppendLine("}");

        }

        protected void AddMethod_GetDataObjectsFromReader()
        {
            AppendLine();
            AppendLine("internal static List<#CONCRETE_DATA_ENTITY_TYPE_NAME#> GetDataObjectsFromReader(IDataReader dataReader) {");
            IndentIncrement();
            AppendLine();
            AppendLine("List<#CONCRETE_DATA_ENTITY_TYPE_NAME#> dataObjectList = new List<#CONCRETE_DATA_ENTITY_TYPE_NAME#>();");
            AppendLine();
            AppendLine("while (dataReader.Read())");
            AppendLine("{");
            IndentIncrement();
            AppendLine("dataObjectList.Add(GetDataObjectFromReader(dataReader));");
            AppendLine("}");
            AppendLine();
            AppendLine("return dataObjectList;");
            IndentDecrement();
            AppendLine("}");
        }

        protected void AddMethod_GetDataObjectFromReader()
        {

            Column[] columns = m_IView.Columns;
            int columnCount = m_IView.Columns.Length;

            #region get data object from reader

            AppendLine();
            AppendLine("internal static #CONCRETE_DATA_ENTITY_TYPE_NAME# GetDataObjectFromReader( IDataReader dataReader ){");

            IndentIncrement();
            AppendLine();
            AppendLine("#CONCRETE_DATA_ENTITY_TYPE_NAME# dataObject;");
            AppendLine("dataObject = new #CONCRETE_DATA_ENTITY_TYPE_NAME#();");
            AppendLine();
            AppendLine("GetDataObjectFromReader( dataObject, dataReader );");
            AppendLine();
            AppendLine("return dataObject;");
            IndentDecrement();
            AppendLine();
            AppendLine("}");

            #endregion

            #region get supplied data object from reader

            AppendLine();
            AppendLine("internal static void GetDataObjectFromReader( #CONCRETE_DATA_ENTITY_TYPE_NAME# dataObject, IDataReader dataReader ){");

            IndentIncrement();
            AppendLine();
            AppendLine("FieldDefinition fieldDef;");
            AppendLine();
            AppendLine("dataObject.IsNew = false;;");

            for (int index = 0; index < columnCount; index++)
            {

                AppendLine();
                AppendStartLine("fieldDef = FieldDefinitionArray[( int )#CLASS_NAME_PREFIX#FieldIndex.");
                Append(((Column)columns[index]).PropertyName);
                AppendEndLine("];");

                AppendLine("if ( dataReader.GetSchemaTable( ).Select(\"ColumnName = \'\" + fieldDef.SourceColumnName + \"\'\" ).Length != 0 ) {");

                IndentIncrement();
                AppendLine("if( dataReader[fieldDef.SourceColumnName] != DBNull.Value ){");

                IndentIncrement();
                AppendStartLine("((IFieldValues)dataObject).FieldValues[( int )#CLASS_NAME_PREFIX#FieldIndex.");
                Append(((Column)columns[index]).PropertyName);
                AppendEndLine("].m_Value = ");

                IndentIncrement();
                AppendStartLine("( ");
                Append(((Column)columns[index]).LanguageType);
                AppendEndLine(" )dataReader[fieldDef.SourceColumnName];");

                IndentDecrement();
                IndentDecrement();
                AppendLine("}");

                IndentDecrement();
                AppendLine("}");
            }


            IndentDecrement();
            AppendLine();
            AppendLine("}");

            #endregion

        }

        protected void AddMethod_LoadDataObjectByQuery()
        {

            AppendLine();
            AppendLine("internal static void LoadDataObjectByQuery( #CONCRETE_DATA_ENTITY_TYPE_NAME# dataObject, string query, List<FieldValue> pkFieldValues ) {");
            AppendLine();
            IndentIncrement();
            AppendLine("IDataReader dataReader = null;");
            AppendLine();
            AppendLine("try {");
            AppendLine();
            IndentIncrement();
            AppendLine("dataReader = GatewayHelper.ExecuteReaderFromSql( query, pkFieldValues );");
            AppendLine();
            AppendLine("if( dataReader.Read() ) {");
            IndentIncrement();
            AppendLine("GetDataObjectFromReader( dataObject, dataReader );");
            IndentDecrement();
            AppendLine("}");
            AppendLine();
            IndentDecrement();
            AppendLine("} finally {");
            IndentIncrement();
            AppendLine("if( dataReader != null ) {");
            IndentIncrement();
            AppendLine("dataReader.Close();");
            IndentDecrement();
            AppendLine("}");
            IndentDecrement();
            AppendLine("}");
            AppendLine();
            IndentDecrement();
            AppendLine("}");

        }

        private void AddFieldDefinitionClassDefinition()
        {

            AppendLine();
            AppendLine("public class FieldDefinitionGroup {");
            IndentIncrement();

            #region add the private member variable field definition array

            AppendLine();
            AppendLine("private FieldDefinition[] m_FieldDefinitions = {");
            IndentIncrement();

            // add fielddefinition objects
            for (int index = 0; index < m_IView.Columns.Length; index++)
            {
                AppendStartLine(Get_NewFieldDefinition(m_IView, index));
                if (index < m_IView.Columns.Length - 1)
                {
                    Append(",");
                }
                AppendEndLine();
            }

            IndentDecrement();
            AppendLine("};");

            #endregion

            #region add a property for each field definition

            AppendLine();
            AppendLine("public FieldDefinition[] FieldDefinitionArray {");
            IndentIncrement();
            AppendLine("get {");
            IndentIncrement();
            AppendLine("return m_FieldDefinitions;");
            IndentDecrement();
            AppendLine("}");
            IndentDecrement();
            AppendLine("}");

            for (int index = 0; index < m_IView.Columns.Length; index++)
            {
                AppendLine();
                AppendLine("public FieldDefinition " + m_IView.Columns[index].PropertyName + " {");
                IndentIncrement();

                AppendLine("get {");
                IndentIncrement();

                AppendLine("return m_FieldDefinitions[" + index.ToString() + "];");

                IndentDecrement();
                AppendLine("}");

                IndentDecrement();
                AppendLine("}");
            }

            #endregion

            IndentDecrement();
            AppendLine("}");

        }

        #endregion

    }

}
