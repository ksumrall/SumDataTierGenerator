using System;
using System.Collections.Generic;
using System.Text;

using TotalSafety.DataTierGenerator.Common;

namespace TotalSafety.DataTierGenerator.CodeGenerationFactory
{

    public class GeneratedGateway : GeneratorBase {

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

        #region internal structured members
        #endregion

        #region constructors / desturctors

        public GeneratedGateway()
            : this( null ) {
        }

        public GeneratedGateway( string rootNamespace )
            : this( rootNamespace, null ) {
        }

        public GeneratedGateway( string rootNamespace, Table table )
            : base( rootNamespace, table ) {

            string pkType = base.PK_PARAMETER_TYPE_LIST;

            if( !string.IsNullOrEmpty( pkType ) ) {
                this.SUBCLASS_NAME = "IGateway<#CLASS_NAME_PREFIX#DataObject,"
                    + base.PK_PARAMETER_TYPE_LIST
                    + ">";
            } else {
                this.SUBCLASS_NAME = "IGateway<#CLASS_NAME_PREFIX#DataObject,int>";
            }

        }

        ~GeneratedGateway() {
        }

        #endregion

        #region public properties

        public override Table Table {
            get {
                return m_Table;
            }
            set {
                base.Table = value;

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
        public override string CLASS_NAME {
            get {
                if( String.IsNullOrEmpty( m_CLASS_NAME ) ) {
                    m_CLASS_NAME = Utility.FormatPascal( base.CONCRETE_GATEWAY_TYPE_NAME );
                }

                return m_CLASS_NAME;
            }
        }

        #endregion

        #region event handlers / overrides

        public override string ToString() {

            base.CLASS_SUMMARY = "Generated class that provides CRUD functionality for the #TABLE_NAME# table.";

            return base.ToString();
        }

        protected override void OnGetFileComments() {

            AppendLine( "// THIS IS AN AUTOMATICALLY GENERATED FILE." );
            AppendLine( "// It contains the implementations of the class #CLASS_NAME#." );
            AppendLine( "// Do not make modifications to this file since they will be overwritten" );
            AppendLine( "// when this file is regenerated. Instead, make your modifications" );
            AppendLine( "// to the file without the \'_Gateway\' extension." );
            AppendLine( "//" );

            base.OnGetFileComments();

        }

        protected override void OnGetUsingStatements() {
            base.OnGetUsingStatements();

            AppendLine( "using System.Collections.Generic;" );
            AppendLine( "using System.Data;" );
            AppendLine( "using System.Text;" );

        }

        protected override void OnPreGetClass() {

            List<Column> columns = m_Table.Columns;
            int columnCount = m_Table.Columns.Count;

            AppendLine();
            AppendLine( "public enum #CLASS_NAME_PREFIX#FieldIndex{" );

            this.IndentIncrement();

            for( int index = 0; index < columnCount; index++ ) {
                AppendStartLine( ( columns[index] ).PropertyName );
                if( index < columnCount - 1 ) {
                    Append( "," );
                }
                AppendEndLine( "" );
            }

            this.IndentDecrement();

            AppendLine( "};" );

            base.OnPreGetClass();
        }

        protected override void OnGetRegion_InternalStructuredMembers() {

            AddFieldDefinitionClassDefinition();

            base.OnGetRegion_InternalStructuredMembers();
        }

        protected override void OnGetRegion_PrivateProtectedMemberVariables() {

            List<Column> columns = m_Table.Columns;
            int columnCount = m_Table.Columns.Count;

            AppendLine();
            AppendLine( "private static readonly #CLASS_NAME# m_Instance = new #CLASS_NAME#();" );
            AppendLine( "private static FieldDefinitionGroup m_FieldDefinitions = new FieldDefinitionGroup();" );

        }

        //protected override void OnGetRegion_ConstructorsDesturctors() {

        //    //AppendLine();
        //    //AppendLine( "static #CLASS_NAME#(){" );
        //    //AppendLine( "}" );

        //}

        protected override void OnGetRegion_InterfaceImplementationMethods() {

            string pkType = base.PK_PARAMETER_TYPE_LIST;
            if( string.IsNullOrEmpty( pkType ) ) {
                pkType = "int";
            }

            AppendLine();
            AppendLine( "#region IGateway Members" );

            AppendLine();
            AppendLine( "#region properties" );

            #region IGateway<T>.FieldDefinitionArray

            AppendLine();
            AppendLine( "FieldDefinition[] IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.FieldDefinitionArray {" );
            IndentIncrement();
            AppendLine( "get {" );
            IndentIncrement();
            AppendLine( "return FieldDefinitionArray;" );
            IndentDecrement();
            AppendLine( "}" );
            IndentDecrement();
            AppendLine( "}" );

            #endregion

            #region IGateway<T>.DatabaseName

            AppendLine();
            AppendLine( "string IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.DatabaseName {" );
            IndentIncrement();
            AppendLine( "get {" );
            IndentIncrement();
            AppendLine( "return DatabaseName;" );
            IndentDecrement();
            AppendLine( "}" );
            IndentDecrement();
            AppendLine( "}" );

            #endregion

            #region IGateway<T>.SchemaName

            AppendLine();
            AppendLine( "string IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.SchemaName {" );
            IndentIncrement();
            AppendLine( "get {" );
            IndentIncrement();
            AppendLine( "return SchemaName;" );
            IndentDecrement();
            AppendLine( "}" );
            IndentDecrement();
            AppendLine( "}" );

            #endregion

            #region IGateway<T>.TableName

            AppendLine();
            AppendLine( "string IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.TableName {" );
            IndentIncrement();
            AppendLine( "get {" );
            IndentIncrement();
            AppendLine( "return TableName;" );
            IndentDecrement();
            AppendLine( "}" );
            IndentDecrement();
            AppendLine( "}" );

            #endregion

            #region IGateway<T>.SchemaTableName

            AppendLine();
            AppendLine( "string IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.SchemaTableName {" );
            IndentIncrement();
            AppendLine( "get {" );
            IndentIncrement();
            AppendLine( "return SchemaTableName;" );
            IndentDecrement();
            AppendLine( "}" );
            IndentDecrement();
            AppendLine( "}" );

            #endregion

            #region IGateway<T>.FullyQualifiedTableName

            AppendLine();
            AppendLine( "string IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.FullyQualifiedTableName {" );
            IndentIncrement();
            AppendLine( "get {" );
            IndentIncrement();
            AppendLine( "return FullyQualifiedTableName;" );
            IndentDecrement();
            AppendLine( "}" );
            IndentDecrement();
            AppendLine( "}" );

            #endregion

            AppendLine();
            AppendLine( "#endregion" );

            AppendLine();
            AppendLine( "#region methods" );

            #region IGateway<T>.SelectAll

            AppendLine();
            AppendLine( "List<#CLASS_NAME_PREFIX#DataObject> IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.SelectAll(){" );
            IndentIncrement();
            AppendLine( "return #CLASS_NAME#.SelectAll();" );
            IndentDecrement();
            AppendLine( "}" );

            #endregion

            #region IGateway<T>.SelectByPrimaryKey

            AppendLine();

            AppendLine( "#CLASS_NAME_PREFIX#DataObject IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.SelectByPrimaryKey( " + pkType + " id ){" );
            IndentIncrement();

            if( m_Table.PrimaryKey == null || m_Table.PrimaryKey.Columns.Count == 0 ) {
                AppendLine( "return null;" );
            } else {
                AppendLine( "return #CLASS_NAME#.SelectByPrimaryKey( id );" );
            }

            IndentDecrement();
            AppendLine( "}" );

            #endregion

            #region IGateway<T>.Insert

            AppendLine();
            AppendLine( "void IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.Insert( #CLASS_NAME_PREFIX#DataObject dataObject ){" );
            IndentIncrement();
            AppendLine( "#CLASS_NAME#.Insert( (#CONCRETE_DATA_ENTITY_TYPE_NAME#)dataObject );" );
            IndentDecrement();
            AppendLine( "}" );

            #endregion

            #region IGateway<T>.Update

            AppendLine();
            AppendLine( "void IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.Update( #CLASS_NAME_PREFIX#DataObject dataObject ){" );
            IndentIncrement();
            AppendLine( "#CLASS_NAME#.Update( (#CONCRETE_DATA_ENTITY_TYPE_NAME#)dataObject );" );
            IndentDecrement();
            AppendLine( "}" );

            #endregion

            #region IGateway<T>.Delete

            AppendLine();
            AppendLine( "void IGateway<#CLASS_NAME_PREFIX#DataObject," + pkType + ">.Delete( #CLASS_NAME_PREFIX#DataObject dataObject ){" );
            IndentIncrement();
            AppendLine( "#CLASS_NAME#.Delete( (#CONCRETE_DATA_ENTITY_TYPE_NAME#)dataObject );" );
            IndentDecrement();
            AppendLine( "}" );

            #endregion

            AppendLine();
            AppendLine( "#endregion" );

            AppendLine();
            AppendLine( "#endregion" );

        }

        protected override void OnGetRegion_PublicProperties() {

            AppendLine();
            AppendLine( "#region properties" );

            #region Instance

            AppendLine();
            AppendLine( "public static #CLASS_NAME# Instance {" );
            IndentIncrement();
            AppendLine( "get {" );
            IndentIncrement();
            AppendLine( "return m_Instance;" );
            IndentDecrement();
            AppendLine( "}" );
            IndentDecrement();
            AppendLine( "}" );

            #endregion

            #region static FieldDefinitionArray

            AppendLine();
            AppendLine( "public static FieldDefinition[] FieldDefinitionArray {" );
            IndentIncrement();
            AppendLine( "get {" );
            IndentIncrement();
            AppendLine( "return m_FieldDefinitions.FieldDefinitionArray;" );
            IndentDecrement();
            AppendLine( "}" );
            IndentDecrement();
            AppendLine( "}" );

            #endregion

            #region static FieldDefinitions

            AppendLine();
            AppendLine( "public static FieldDefinitionGroup FieldDefinitions {" );
            IndentIncrement();
            AppendLine( "get {" );
            IndentIncrement();
            AppendLine( "return m_FieldDefinitions;" );
            IndentDecrement();
            AppendLine( "}" );
            IndentDecrement();
            AppendLine( "}" );

            #endregion

            #region static DatabaseName

            AppendLine();
            AppendLine( "public static string DatabaseName {" );
            IndentIncrement();
            AppendLine( "get {" );
            IndentIncrement();
            AppendLine( "return \"" + m_Table.DatabaseName + "\";" );
            IndentDecrement();
            AppendLine( "}" );
            IndentDecrement();
            AppendLine( "}" );

            #endregion

            #region static SchemaName

            AppendLine();
            AppendLine( "public static string SchemaName {" );
            IndentIncrement();
            AppendLine( "get {" );
            IndentIncrement();
            AppendLine( "return \"" + m_Table.Schema + "\";" );
            IndentDecrement();
            AppendLine( "}" );
            IndentDecrement();
            AppendLine( "}" );

            #endregion

            #region static TableName

            AppendLine();
            AppendLine( "public static string TableName {" );
            IndentIncrement();
            AppendLine( "get {" );
            IndentIncrement();
            AppendLine( "return \"[" + m_Table.Name + "]\";" );
            IndentDecrement();
            AppendLine( "}" );
            IndentDecrement();
            AppendLine( "}" );

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
            AppendLine( "public static string FullyQualifiedTableName {" );
            IndentIncrement();
            AppendLine( "get {" );
            IndentIncrement();
            AppendLine( "return DatabaseName + \".\" + SchemaName + \".\" + TableName;" );
            IndentDecrement();
            AppendLine( "}" );
            IndentDecrement();
            AppendLine( "}" );

            #endregion

            AppendLine();
            AppendLine( "#endregion" );

        }

        protected override void OnGetRegion_PublicMethods() {

            AppendLine();
            AppendLine( "#region public methods" );

            OnCRUD_SelectAll();
            OnCRUD_SelectByPrimaryKey();
            OnCRUD_LoadByPrimaryKey();
            OnCRUD_Insert();
            OnCRUD_Update();
            OnCRUD_Delete();
            OnCRUD_DeleteByPrimaryKey();
            OnCRUD_DeleteByFieldsList();
            OnCRUD_DeleteByFieldsArray();

            AppendLine();
            AppendLine( "#endregion" );

        }

        protected override void OnGetRegion_PrivateMethods() {

            AppendLine();
            AppendLine( "#region private methods" );

            AddMethod_GetDataObjectsByQuery();
            AddMethod_GetDataObjectFromReader();
            AddMethod_LoadDataObjectByQuery();

            AppendLine();
            AppendLine( "#endregion" );

        }

        #endregion

        #region CRUD methods

        protected virtual void OnCRUD_SelectAll() {

            AppendLine();
            AppendLine( "public static List<#CONCRETE_DATA_ENTITY_TYPE_NAME#> SelectAll(){" );
            IndentIncrement();
            AppendLine();
            AppendLine( "string query;" );
            AppendLine();
            AppendLine( "query = GatewayHelper.BuildSelectAllQuery( SchemaTableName, FieldDefinitionArray );" );
            AppendLine();
            AppendLine( "return #CLASS_NAME#.GetDataObjectsByQuery( query, (FieldValue[])null );" );
            AppendLine();
            IndentDecrement();
            AppendLine( "}" );

        }

        protected virtual void OnCRUD_SelectByPrimaryKey() {

            if( m_Table.PrimaryKey == null || m_Table.PrimaryKey.Columns.Count == 0 ) {
                return;
            }

            List<Column> pkList = m_Table.PrimaryKey.Columns;
            int columnCount = m_Table.PrimaryKey.Columns.Count;

            AppendLine();
            AppendStartLine( "public static #CONCRETE_DATA_ENTITY_TYPE_NAME# SelectByPrimaryKey( " );
            Append( base.PK_PARAMETER_LIST );
            AppendEndLine( " ){" );
            IndentIncrement();

            #region variable declaration

            AppendLine();
            AppendLine( "string query;" );
            AppendLine( "List<FieldValue> fieldValueList = new List<FieldValue>( );" );
            AppendLine();
            AppendLine( "List<#CONCRETE_DATA_ENTITY_TYPE_NAME#> dataObjects;" );
            AppendLine( "#CONCRETE_DATA_ENTITY_TYPE_NAME# dataObject = null;" );
            AppendLine();

            #endregion

            #region initialization of the fielddefinitions and fieldvalues

            AppendLine();
            for( int index = 0; index < columnCount; index++ ) {
                AppendStartLine( "fieldValueList.Add(" );
                Append( " new FieldValue( m_FieldDefinitions." );
                Append( pkList[index].PropertyName );
                Append( ", " );
                Append( Utility.FormatCamel( pkList[index].PropertyName ) );
                AppendEndLine( " ) );" );
            }

            #endregion

            #region run query to get the object

            AppendLine();
            AppendLine( "query = GatewayHelper.BuildSelectByFieldsQuery( SchemaTableName, FieldDefinitionArray );" );
            AppendLine();
            AppendLine( "dataObjects = #CLASS_NAME#.GetDataObjectsByQuery(query, fieldValueList );" );
            AppendLine();
            AppendLine( "if ( dataObjects.Count == 1 ) {" );
            IndentIncrement();
            AppendLine( "dataObject = dataObjects[0];" );
            IndentDecrement();
            AppendLine( "}" );

            #endregion

            AppendLine();
            AppendLine( "return dataObject;" );
            IndentDecrement();
            AppendLine();
            AppendLine( "}" );

        }

        protected virtual void OnCRUD_LoadByPrimaryKey() {

            if( m_Table.PrimaryKey == null || m_Table.PrimaryKey.Columns.Count == 0 ) {
                return;
            }

            List<Column> pkList = m_Table.PrimaryKey.Columns;
            int columnCount = m_Table.PrimaryKey.Columns.Count;

            AppendLine();
            AppendLine( "internal static void LoadByPrimaryKey( #CONCRETE_DATA_ENTITY_TYPE_NAME# dataObject, " );
            Append( base.PK_PARAMETER_LIST );
            AppendEndLine( " ){" );
            IndentIncrement();
            AppendLine();
            AppendLine( "string query;" );
            AppendLine( "List<FieldValue> fieldValueList = new List<FieldValue>();" );
            AppendLine();
            #region initialization of the fielddefinitions and fieldvalues

            AppendLine();
            for( int index = 0; index < columnCount; index++ ) {
                AppendStartLine( "fieldValueList.Add(" );
                Append( " new FieldValue( m_FieldDefinitions." );
                Append( pkList[index].PropertyName );
                Append( ", " );
                Append( Utility.FormatCamel( pkList[index].PropertyName ) );
                AppendEndLine( " ) );" );
            }

            #endregion
            AppendLine();
            AppendLine( "query = GatewayHelper.BuildSelectByFieldsQuery( SchemaTableName, FieldDefinitionArray );" );
            AppendLine();
            AppendLine( "#CLASS_NAME#.LoadDataObjectByQuery(dataObject, query, fieldValueList );" );
            AppendLine();
            IndentDecrement();
            AppendLine( "}" );

        }

        protected virtual void OnCRUD_Insert() {

            AppendLine();
            AppendLine( "public static void Insert( #CONCRETE_DATA_ENTITY_TYPE_NAME# dataObject ){" );
            IndentIncrement();

            #region variable declaration

            AppendLine();
            AppendLine( "GatewayHelper.ParameterizedQuery parameterizedQuery;" );
            AppendLine( "List<FieldDefinition> fieldDefinitions = new List<FieldDefinition>();" );
            AppendLine();
            AppendLine( "List<#CONCRETE_DATA_ENTITY_TYPE_NAME#> dataObjects;" );

            #endregion

            #region run query to get the object

            AppendLine();
            AppendLine( "parameterizedQuery = GatewayHelper.BuildInsertQuery( SchemaTableName, (IFieldValues)dataObject );" );
            AppendLine();
            AppendLine( "dataObjects = #CLASS_NAME#.GetDataObjectsByQuery(parameterizedQuery.Query, parameterizedQuery.ParameterFieldValueList );" );
            AppendLine();
            AppendLine( "if ( dataObjects.Count == 1 ) {" );
            IndentIncrement();
            AppendLine( "dataObject.IsDirty = false;" );
            AppendLine();
            AppendLine( "for ( int index = 0; index < FieldDefinitionArray.Length; index++ ) {" );
            IndentIncrement();
            AppendLine( "((IFieldValues)dataObject).FieldValues[index].Value = ((IFieldValues)dataObjects[0]).FieldValues[index].Value;" );
            AppendLine( "((IFieldValues)dataObject).FieldValues[index].IsDirty = false;" );
            AppendLine( "dataObject.IsNew = false;" );
            IndentDecrement();
            AppendLine( "}" );
            IndentDecrement();
            AppendLine( "}" );

            #endregion

            IndentDecrement();
            AppendLine();
            AppendLine( "}" );
        }

        protected virtual void OnCRUD_Update() {
            Index pk = m_Table.PrimaryKey;

            AppendLine();
            AppendLine( "/// <summary>" );
            AppendLine( "/// This method updates the record from the underlying table where all the fields match." );
            AppendLine( "/// If there is more than one record matching the filter, they all get updated." );
            AppendLine( "/// </summary>" );
            AppendLine( "public static void Update( #CONCRETE_DATA_ENTITY_TYPE_NAME# dataObject ){" );
            IndentIncrement();

            #region variable declaration

            //AppendLine ();
            AppendLine( "StringBuilder query = new StringBuilder();" );
            //AppendLine ("List<FieldDefinition> fieldDefinitions = new List<FieldDefinition>();");
            //AppendLine ("List<FieldValue> fields = new List<FieldValue>();");
            //AppendLine ();
            //AppendLine ("List<FieldDefinition> pkFieldDefinitions = new List<FieldDefinition>();");
            //AppendLine ("List<FieldValue> pkFields = new List<FieldValue>();");
            //AppendLine ();
            AppendLine( "List<#CONCRETE_DATA_ENTITY_TYPE_NAME#> dataObjects;" );

            #endregion

            #region initialization of the fielddefinitions and fieldvalues

            //AppendLine ();
            //AppendLine ("// gather all the changed fields");
            //AppendLine( "for( int index = 0; index < FieldDefinitionArray.Length; index++ ){" );
            //IndentIncrement ();
            //AppendLine ();

            //AppendLine( "if( ((IFieldValues)dataObject).FieldValues[index].IsDirty ){" );
            //IndentIncrement ();
            //AppendLine ();

            //AppendLine( "fieldDefinitions.Add( FieldDefinitionArray[index] );" );
            //AppendLine ("fields.Add( ((IFieldValues)dataObject).FieldValues[index] );");

            //AppendLine ();
            //IndentDecrement ();
            //AppendLine ("}");

            //AppendLine ();
            //IndentDecrement ();
            //AppendLine ("}");

            AppendLine();
            AppendLine( "query.AppendLine( GatewayHelper.BuildUpdateByPrimaryKeyQuery( SchemaTableName, (IFieldValues)dataObject ) );" );

            AppendLine();
            AppendLine( "// get the select all clause" );
            AppendLine( "query.AppendLine( GatewayHelper.BuildSelectByPrimaryKeyQuery( SchemaTableName, (IFieldValues)dataObject ) );" );

            #endregion

            #region initialize the primary key

            //AppendLine ();
            //AppendLine ("// gather the primary key fields");
            //AppendLine ("for( int index = 0; index < FieldDefinitionArray.Length; index++ ){");
            //IndentIncrement ();
            //AppendLine ();

            //AppendLine( "if( FieldDefinitionArray[index].IsPrimaryKey ){" );
            //IndentIncrement ();
            //AppendLine ();

            //AppendLine( "fieldDefinitions.Add( FieldDefinitionArray[index] );" );
            //AppendLine ("fields.Add( ((IFieldValues)dataObject).FieldValues[index] );");

            //AppendLine ();
            //IndentDecrement ();
            //AppendLine ("}");

            //AppendLine ();
            //IndentDecrement ();
            //AppendLine ("}");

            #endregion

            #region run query to get the object

            AppendLine();
            AppendLine( "dataObjects = #CLASS_NAME#.GetDataObjectsByQuery( query.ToString(), ((IFieldValues)dataObject).FieldValues );" );
            AppendLine();
            AppendLine( "if ( dataObjects.Count == 1 ) {" );
            IndentIncrement();
            AppendLine( "dataObject.IsDirty = false;" );
            AppendLine();
            AppendLine( "for ( int index = 0; index < FieldDefinitionArray.Length; index++ ) {" );
            IndentIncrement();
            AppendLine( "((IFieldValues)dataObject).FieldValues[index].Value = ((IFieldValues)dataObjects[0]).FieldValues[index].Value;" );
            AppendLine( "((IFieldValues)dataObject).FieldValues[index].IsDirty = false;" );
            AppendLine( "dataObject.IsNew = false;" );
            IndentDecrement();
            AppendLine( "}" );
            IndentDecrement();
            AppendLine( "}" );

            #endregion

            IndentDecrement();
            AppendLine();
            AppendLine( "}" );
        }

        protected virtual void OnCRUD_UpdateByPrimaryKey() {

            if( m_Table.PrimaryKey == null || m_Table.PrimaryKey.Columns.Count == 0 ) {
                return;
            }

            List<Column> pkList = m_Table.PrimaryKey.Columns;
            int columnCount = m_Table.PrimaryKey.Columns.Count;

            AppendLine();
            AppendLine( "public static void Update( #CONCRETE_DATA_ENTITY_TYPE_NAME# dataObject ){" );
            IndentIncrement();

            #region variable declaration

            AppendLine();
            AppendLine( "string query;" );
            AppendLine( "List<FieldDefinition> fieldDefinitions = new List<FieldDefinition>();" );
            AppendLine( "List<FieldValue> fields = new List<FieldValue>();" );
            AppendLine();
            AppendLine( "List<FieldDefinition> pkFieldDefinitions = new List<FieldDefinition>();" );
            AppendLine( "List<FieldValue> pkFields = new List<FieldValue>();" );
            AppendLine();
            AppendLine( "List<#CONCRETE_DATA_ENTITY_TYPE_NAME#> dataObjects;" );

            #endregion

            #region initialization of the fielddefinitions and fieldvalues

            AppendLine();
            AppendLine( "// gather all the changed fields" );
            AppendLine( "for( int index = 0; index < FieldDefinitions.Length; index++ ){" );
            IndentIncrement();
            AppendLine();

            AppendLine( "if( ((IFieldValues)dataObject).FieldValues[index].IsDirty ){" );
            IndentIncrement();
            AppendLine();

            AppendLine( "fieldDefinitions.Add( FieldDefinitions[index] );" );
            AppendLine( "fields.Add( ((IFieldValues)dataObject).FieldValues[index] );" );

            AppendLine();
            IndentDecrement();
            AppendLine( "}" );

            AppendLine();
            IndentDecrement();
            AppendLine( "}" );

            AppendLine();
            AppendLine( "query = GatewayHelper.BuildUpdateByPrimaryKeyQuery( SchemaTableName, FieldDefinitionArray, ((IFieldValues)dataObject).FieldValues );" );

            #endregion

            #region initialize the primary key

            AppendLine();
            AppendLine( "// gather the primary key fields" );
            AppendLine( "for( int index = 0; index < FieldDefinitions.Length; index++ ){" );
            IndentIncrement();
            AppendLine();

            AppendLine( "if( FieldDefinitions[index].IsPrimaryKey ){" );
            IndentIncrement();
            AppendLine();

            AppendLine( "fieldDefinitions.Add( FieldDefinitions[index] );" );
            AppendLine( "fields.Add( ((IFieldValues)dataObject).FieldValues[index] );" );

            AppendLine();
            IndentDecrement();
            AppendLine( "}" );

            AppendLine();
            IndentDecrement();
            AppendLine( "}" );

            #endregion

            #region run query to get the object

            AppendLine();
            AppendLine( "//if( pkFields.Count > 0 ){" );
            IndentIncrement();
            AppendLine();

            AppendLine( "//query += Environment.NewLine;" );
            AppendLine( "//query += GatewayHelper.BuildSelectAllQuery( SchemaTableName, FieldDefinitions );" );
            AppendLine( "//query += Environment.NewLine;" );
            AppendLine( "//query += GatewayHelper.BuildWhereClause( pkFieldDefinitions.ToArray() );" );

            AppendLine();
            IndentDecrement();
            AppendLine( "//}" );
            AppendLine();
            AppendLine( "dataObjects = #CLASS_NAME#.GetDataObjectsByQuery(query, fields );" );
            AppendLine();
            AppendLine( "if ( dataObjects.Count == 1 ) {" );
            IndentIncrement();
            AppendLine( "dataObject.IsDirty = false;" );
            AppendLine();
            AppendLine( "for ( int index = 0; index < FieldDefinitions.Length; index++ ) {" );
            IndentIncrement();
            AppendLine( "((IFieldValues)dataObject).FieldValues[index].Value = ((IFieldValues)dataObjects[0]).FieldValues[index].Value;" );
            AppendLine( "((IFieldValues)dataObject).FieldValues[index].IsDirty = false;" );
            AppendLine( "dataObject.IsNew = false;" );
            IndentDecrement();
            AppendLine( "}" );
            IndentDecrement();
            AppendLine( "}" );

            #endregion

            IndentDecrement();
            AppendLine();
            AppendLine( "}" );
        }

        protected virtual void OnCRUD_Delete() {

            AppendLine();
            AppendLine( "/// <summary>" );
            AppendLine( "/// This method deletes the record from the underlying table where all the fields match." );
            AppendLine( "/// If there is more than one record matching the filter, they all get deleted." );
            AppendLine( "/// </summary>" );
            AppendLine( "public static void Delete( #CONCRETE_DATA_ENTITY_TYPE_NAME# dataObject ){" );
            IndentIncrement();

            AppendLine();
            AppendLine( "if ( dataObject.m_PrimaryKeyFieldValues.Length > 0 ) {" );
            IndentIncrement();
            AppendLine( "DeleteByFields(dataObject.m_PrimaryKeyFieldValues);" );
            IndentDecrement();
            AppendLine( "} else {" );
            IndentIncrement();
            AppendLine( "DeleteByFields(new List<FieldValue>(( (IFieldValues)dataObject ).FieldValues));" );
            IndentDecrement();
            AppendLine( "}" );
            AppendLine();

            IndentDecrement();
            AppendLine( "}" );

        }

        protected virtual void OnCRUD_DeleteByFieldsList() {

            AppendLine();
            AppendLine( "/// <summary>" );
            AppendLine( "/// This method deletes the record from the underlying table where all the fields match." );
            AppendLine( "/// If there is more than one record matching the filter, they all get deleted." );
            AppendLine( "/// </summary>" );
            AppendLine( "public static void DeleteByFields( List<FieldValue> fieldValueList ){" );

            AppendLine();
            IndentIncrement();
            AppendLine( "DeleteByFields(fieldValueList.ToArray());" );

            IndentDecrement();
            AppendLine( "}" );

        }

        protected virtual void OnCRUD_DeleteByFieldsArray() {

            AppendLine();
            AppendLine( "/// <summary>" );
            AppendLine( "/// This method deletes the record from the underlying table where all the fields match." );
            AppendLine( "/// If there is more than one record matching the filter, they all get deleted." );
            AppendLine( "/// </summary>" );
            AppendLine( "public static void DeleteByFields( FieldValue[] fieldValueArray ){" );

            #region variable declaration

            AppendLine();
            IndentIncrement();
            AppendLine( "StringBuilder deleteQuery = new StringBuilder();" );

            #endregion

            #region build the query

            AppendLine();
            AppendLine( "deleteQuery.AppendLine(GatewayHelper.BuildDeleteQuery(SchemaTableName));" );
            AppendLine( "deleteQuery.AppendLine(GatewayHelper.BuildWhereClause(fieldValueArray));" );

            #endregion

            #region run query

            AppendLine();
            AppendLine( "GatewayHelper.ExecuteNonQueryFromSql(deleteQuery.ToString(), fieldValueArray);" );

            #endregion

            IndentDecrement();
            AppendLine( "}" );

        }

        protected virtual void OnCRUD_DeleteByPrimaryKey() {

            if( m_Table.PrimaryKey == null || m_Table.PrimaryKey.Columns.Count == 0 ) {
                return;
            }

            List<Column> pkList = m_Table.PrimaryKey.Columns;
            int columnCount = m_Table.PrimaryKey.Columns.Count;

            #region variable declaration

            AppendLine();
            AppendStartLine( "public static void DeleteByPrimaryKey( " );
            Append( base.PK_PARAMETER_LIST );
            AppendEndLine( " ){" );
            IndentIncrement();
            AppendLine();
            AppendLine( "List<FieldValue> fieldValueList = new List<FieldValue>( );" );

            #endregion

            #region initialization of the fielddefinitions and fieldvalues

            AppendLine();
            for( int index = 0; index < columnCount; index++ ) {
                AppendStartLine( "fieldValueList.Add(" );
                Append( " new FieldValue( m_FieldDefinitions." );
                Append( pkList[index].PropertyName );
                Append( ", " );
                Append( Utility.FormatCamel( pkList[index].PropertyName ) );
                AppendEndLine( " ) );" );
            }

            #endregion

            #region run query

            AppendLine();
            AppendLine( "DeleteByFields(fieldValueList);" );
            AppendLine();

            #endregion

            IndentDecrement();
            AppendLine( "}" );

        }

        #endregion

        #region private implementation

        protected void AddMethod_GetDataObjectsByQuery() {

            AppendLine();
            AppendLine( "internal static List<#CONCRETE_DATA_ENTITY_TYPE_NAME#> GetDataObjectsByQuery(" );
            IndentIncrement();
            AppendLine( "string query, List<FieldValue> dirtyFieldValueList ) {" );
            AppendLine();
            AppendLine( "return GetDataObjectsByQuery( query, dirtyFieldValueList.ToArray() );" );

            AppendLine();
            IndentDecrement();
            AppendLine( "}" );

            AppendLine();
            AppendLine( "internal static List<#CONCRETE_DATA_ENTITY_TYPE_NAME#> GetDataObjectsByQuery(" );
            IndentIncrement();
            AppendLine( "string query, FieldValue[] dirtyFieldValues ) {" );
            AppendLine();
            AppendLine( "IDataReader dataReader = null;" );
            AppendLine();
            AppendLine( "#CONCRETE_DATA_ENTITY_TYPE_NAME# #CLASS_VARIABLE_NAME_PREFIX#DataObject;" );
            AppendLine( "List<#CONCRETE_DATA_ENTITY_TYPE_NAME#> dataObjects;" );
            AppendLine();
            AppendLine( "try {" );
            IndentIncrement();
            AppendLine();
            AppendLine( "dataObjects = new List<#CONCRETE_DATA_ENTITY_TYPE_NAME#>();" );
            AppendLine();
            AppendLine( "dataReader = GatewayHelper.ExecuteReaderFromSql( query, dirtyFieldValues );" );
            AppendLine();
            AppendLine( "while ( dataReader.Read( ) ) {" );
            IndentIncrement();
            AppendLine();
            AppendLine( "#CLASS_VARIABLE_NAME_PREFIX#DataObject = GetDataObjectFromReader( dataReader );" );
            AppendLine();
            AppendLine( "dataObjects.Add(#CLASS_VARIABLE_NAME_PREFIX#DataObject);" );
            AppendLine();
            IndentDecrement();
            AppendLine( "}" );
            IndentDecrement();
            AppendLine( "} finally {" );
            IndentIncrement();
            AppendLine( "if ( dataReader != null ) {" );
            IndentIncrement();
            AppendLine( "dataReader.Close( );" );
            IndentDecrement();
            AppendLine( "}" );
            IndentDecrement();
            AppendLine( "}" );
            AppendLine();
            AppendLine( "return dataObjects;" );

            IndentDecrement();
            AppendLine( "}" );

        }

        protected void AddMethod_GetDataObjectFromReader() {

            List<Column> columns = m_Table.Columns;
            int columnCount = m_Table.Columns.Count;

            #region get data object from reader

            AppendLine();
            AppendLine( "internal static #CONCRETE_DATA_ENTITY_TYPE_NAME# GetDataObjectFromReader( IDataReader dataReader ){" );

            IndentIncrement();
            AppendLine();
            AppendLine( "#CONCRETE_DATA_ENTITY_TYPE_NAME# dataObject;" );
            AppendLine( "dataObject = new #CONCRETE_DATA_ENTITY_TYPE_NAME#();" );
            AppendLine();
            AppendLine( "GetDataObjectFromReader( dataObject, dataReader );" );
            AppendLine();
            AppendLine( "return dataObject;" );
            IndentDecrement();
            AppendLine();
            AppendLine( "}" );

            #endregion

            #region get supplied data object from reader

            AppendLine();
            AppendLine( "internal static void GetDataObjectFromReader( #CONCRETE_DATA_ENTITY_TYPE_NAME# dataObject, IDataReader dataReader ){" );

            IndentIncrement();
            AppendLine();
            AppendLine( "FieldDefinition fieldDef;" );
            AppendLine();
            AppendLine( "dataObject.IsNew = false;;" );

            for( int index = 0; index < columnCount; index++ ) {

                AppendLine();
                AppendStartLine( "fieldDef = FieldDefinitionArray[( int )#CLASS_NAME_PREFIX#FieldIndex." );
                Append( ( (Column)columns[index] ).PropertyName );
                AppendEndLine( "];" );

                AppendLine( "if ( dataReader.GetSchemaTable( ).Select(\"ColumnName = \'\" + fieldDef.SourceColumnName + \"\'\" ).Length != 0 ) {" );

                IndentIncrement();
                AppendLine( "if( dataReader[fieldDef.SourceColumnName] != DBNull.Value ){" );

                IndentIncrement();
                AppendStartLine( "((IFieldValues)dataObject).FieldValues[( int )#CLASS_NAME_PREFIX#FieldIndex." );
                Append( ( (Column)columns[index] ).PropertyName );
                AppendEndLine( "].m_Value = " );

                IndentIncrement();
                AppendStartLine( "( " );
                Append( ( (Column)columns[index] ).LanguageType );
                AppendEndLine( " )dataReader[fieldDef.SourceColumnName];" );

                IndentDecrement();
                IndentDecrement();
                AppendLine( "}" );

                IndentDecrement();
                AppendLine( "}" );
            }


            IndentDecrement();
            AppendLine();
            AppendLine( "}" );

            #endregion

        }

        protected void AddMethod_LoadDataObjectByQuery() {

            AppendLine();
            AppendLine( "internal static void LoadDataObjectByQuery( #CONCRETE_DATA_ENTITY_TYPE_NAME# dataObject, string query, List<FieldValue> pkFieldValues ) {" );
            AppendLine();
            IndentIncrement();
            AppendLine( "IDataReader dataReader = null;" );
            AppendLine();
            AppendLine( "try {" );
            AppendLine();
            IndentIncrement();
            AppendLine( "dataReader = GatewayHelper.ExecuteReaderFromSql( query, pkFieldValues );" );
            AppendLine();
            AppendLine( "if( dataReader.Read() ) {" );
            IndentIncrement();
            AppendLine( "GetDataObjectFromReader( dataObject, dataReader );" );
            IndentDecrement();
            AppendLine( "}" );
            AppendLine();
            IndentDecrement();
            AppendLine( "} finally {" );
            IndentIncrement();
            AppendLine( "if( dataReader != null ) {" );
            IndentIncrement();
            AppendLine( "dataReader.Close();" );
            IndentDecrement();
            AppendLine( "}" );
            IndentDecrement();
            AppendLine( "}" );
            AppendLine();
            IndentDecrement();
            AppendLine( "}" );

        }

        private void AddFieldDefinitionClassDefinition() {

            AppendLine();
            AppendLine( "public class FieldDefinitionGroup {" );
            IndentIncrement();

            #region add the private member variable field definition array

            AppendLine();
            AppendLine( "private FieldDefinition[] m_FieldDefinitions = {" );
            IndentIncrement();

            // add fielddefinition objects
            for( int index = 0; index < m_Table.Columns.Count; index++ ) {
                AppendStartLine( Get_NewFieldDefinition( m_Table, index ) );
                if( index < m_Table.Columns.Count - 1 ) {
                    Append( "," );
                }
                AppendEndLine();
            }

            IndentDecrement();
            AppendLine( "};" );

            #endregion

            #region add a property for each field definition

            AppendLine();
            AppendLine( "public FieldDefinition[] FieldDefinitionArray {" );
            IndentIncrement();
            AppendLine( "get {" );
            IndentIncrement();
            AppendLine( "return m_FieldDefinitions;" );
            IndentDecrement();
            AppendLine( "}" );
            IndentDecrement();
            AppendLine( "}" );

            for( int index = 0; index < m_Table.Columns.Count; index++ ) {
                AppendLine();
                AppendLine( "public FieldDefinition " + m_Table.Columns[index].PropertyName + " {" );
                IndentIncrement();

                AppendLine( "get {" );
                IndentIncrement();

                AppendLine( "return m_FieldDefinitions[" + index.ToString() + "];" );

                IndentDecrement();
                AppendLine( "}" );

                IndentDecrement();
                AppendLine( "}" );
            }

            #endregion

            IndentDecrement();
            AppendLine( "}" );

        }

        #endregion

    }

}
