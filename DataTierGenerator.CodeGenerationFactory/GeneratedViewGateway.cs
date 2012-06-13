using System;
using System.Collections.Generic;
using System.Text;

using TotalSafety.DataTierGenerator.Common;

namespace TotalSafety.DataTierGenerator.CodeGenerationFactory
{

    public class GeneratedViewGateway : GeneratorBase
    {

        #region private and protected member variables

        // table field substitutions
        private string m_COLUMN_INDEX_ENUMERATION;

        // primary key related substitutions
        private string m_PK_COLUMN_DEFINITIONS_INITIALIZATION;
        private string m_PK_COLUMN_VALUES_INITIALIZATION;


        private string m_COLUMN_DEFINITION_INITIALIZATION;
        private string m_PK_COLUMN_DEFINITION_INITIALIZATION;

        private string m_SELECT_BY_PRIMARY_KEY_METHOD;

        private string m_NEW_DATA_OBJECT_FROM_READER;

        #endregion

        #region constructors / desturctors

        public GeneratedViewGateway()
            : this(null)
        {
        }

        public GeneratedViewGateway(string rootNamespace)
            : this(rootNamespace, null)
        {
        }

        public GeneratedViewGateway(string rootNamespace, View view)
            : base(rootNamespace, view)
        {
            this.SUBCLASS_NAME = "IGateway<#CLASS_NAME_PREFIX#DataObject,int>";
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

        #endregion

        #region event handlers / overrides

        public override string ToString()
        {

            base.CLASS_SUMMARY = "Table Data Gateway class that provides CRUD functionality for the #TABLE_NAME# table.";

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

            AppendLine();
            AppendLine("private static readonly #CLASS_NAME# m_Instance = new #CLASS_NAME#();");
            AppendLine("private static FieldDefinitionGroup m_FieldDefinitions = new FieldDefinitionGroup();");

        }

        protected override void OnGetRegion_InternalStructuredMembers()
        {

            AddFieldDefinitionClassDefinition();

            base.OnGetRegion_InternalStructuredMembers();
        }

        protected override void OnGetRegion_PublicProperties()
        {

            AppendLine();
            AppendLine("#region properties");

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

            AppendLine();
            AppendLine("#endregion");

        }

        protected override void OnGetRegion_InterfaceImplementationMethods()
        {

            string pkType = "int";

            AppendLine();
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
            AppendLine("List<#CLASS_NAME_PREFIX#DataObject> IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.SelectAll(){");
            IndentIncrement();
            AppendLine("return #CLASS_NAME#.SelectAll();");
            IndentDecrement();
            AppendLine("}");

            #endregion

            AppendLine();
            AppendLine("#endregion");

            AppendLine();
            AppendLine("#endregion");

        }

        protected override void OnGetRegion_PublicMethods()
        {

            AppendLine();
            AppendLine("#region public methods");

            OnCRUD_SelectAll();

            AppendLine();
            AppendLine("#endregion");

        }

        protected override void OnGetRegion_PrivateMethods()
        {

            AppendLine();
            AppendLine("#region private methods");

            AddMethod_GetDataObjectsByQuery();
            AddMethod_GetDataObjectFromReader();
            AddMethod_LoadDataObjectByQuery();

            AppendLine();
            AppendLine("#endregion");

        }

        #endregion

        #region CRUD methods

        protected virtual void OnCRUD_SelectAll()
        {

            AppendLine();
            AppendLine("public static List<#CONCRETE_DATA_ENTITY_TYPE_NAME#> SelectAll(){");
            IndentIncrement();
            AppendLine();
            AppendLine("string query;");
            AppendLine();
            AppendLine("query = GatewayHelper.BuildSelectAllQuery( SchemaTableName, FieldDefinitionArray );");
            AppendLine();
            AppendLine("return #CLASS_NAME#.GetDataObjectsByQuery( query, (FieldValue[])null );");
            AppendLine();
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
            IndentDecrement();
            AppendLine("}");
            IndentDecrement();
            AppendLine("}");
            AppendLine();
            AppendLine("return dataObjects;");

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
