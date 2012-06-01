// ===============================================================================
//
// CsGatewayGenerator.cs
//
// This file contains the implementations of the class CsGatewayGenerator
//
// ===============================================================================
// Release history
// ACTION   DATE        AUTHOR              NOTES
//
// ===============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PriorityIt.DataTierGenerator
{
	/// <summary>
	/// Summary description for CsGatewayGenerator.
	/// </summary>
	public class CsGatewayGenerator
	{
        #region private member variables

        const string TAB = "    ";
        const string TWO_TABS = TAB + TAB;
        const string THREE_TABS = TWO_TABS + TAB;
        const string FOUR_TABS = THREE_TABS + TAB;
        const string FIVE_TABS = FOUR_TABS + TAB;
        const string SIX_TABS = FIVE_TABS + TAB;

        #endregion

        #region constructors / destructors

        public CsGatewayGenerator()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        ~CsGatewayGenerator()
        {
        }


        #endregion

        #region public properties
        #endregion

        #region public methods

        //public void GenerateAbstractGateway(Table table, string targetNamespace, string path){

        //    string crudFile;
        //    string fileContents = Utility.GetResource("PriorityIt.DataTierGenerator.Resource.AbstractGateway.txt");

        //    AbstractGatewayGenerator sub = new AbstractGatewayGenerator( targetNamespace, table );

        //    string className = Utility.CleanWhitespace( Utility.FormatPascal( table.ProgrammaticAlias ) );

        //    fileContents = sub.ToString( );
        //    /*


        //    sub.IndentIncrement( );

        //    fileContents = fileContents.Replace( "#COLUMN_INDEX_ENUMERATION#", sub.COLUMN_INDEX_ENUMERATION );
        //    fileContents = fileContents.Replace( "#COLUMN_DEFINITION_INITIALIZATION#", sub.COLUMN_DEFINITION_INITIALIZATION );
        //    fileContents = fileContents.Replace( "#SELECT_COLUMN_LIST_ALL#", sub.SELECT_COLUMN_LIST_ALL );
        //    fileContents = fileContents.Replace( "#NEW_DATA_OBJECT_FROM_READER#", sub.NEW_DATA_OBJECT_FROM_READER );

        //    fileContents = fileContents.Replace( "#CRUD_METHODS#", sub.CRUD_METHODS );

        //    #region fill in the CRUD operations

        //    if ( table.PrimaryKeys.Count > 0 ) {

        //        crudFile = Utility.GetResource( "PriorityIt.DataTierGenerator.Resource.CRUD_SelectByPrimaryKey.txt" );

        //        fileContents = fileContents.Replace( "#PK_COLUMN_DEFINITION_INITIALIZATION#", sub.PK_COLUMN_DEFINITION_INITIALIZATION );
        //        fileContents = fileContents.Replace( "#SELECT_BY_PK_METHOD#", crudFile.ToString( ) );

        //        crudFile = Utility.GetResource( "PriorityIt.DataTierGenerator.Resource.CRUD_SelectByPrimaryKey.txt" );

        //        crudFile = crudFile.Replace( "#CLASS_NAME#", sub.CLASS_NAME );
        //        crudFile = crudFile.Replace( "#PK_PARAMETER_LIST#", Get_PRIMARY_KEY_PARAMETER_LIST( table ) );
        //        crudFile = crudFile.Replace( "#PK_COLUMN_COUNT#", sub.PK_COLUMN_COUNT );
        //        crudFile = crudFile.Replace( "#PK_COLUMN_DEFINITIONS_INITIALIZATION#", Get_PRIMARY_KEY_FIELD_DEFINITIONS_INITIALIZATION( table, className ) );
        //        crudFile = crudFile.Replace( "#PK_COLUMN_VALUES_INITIALIZATION#", Get_PRIMARY_KEY_FIELD_VALUES_INITIALIZATION( table ) );
        //        crudFile = crudFile.Replace( "#TABLE_NAME#", "[" + sub.TABLE_NAME + "]" );
        //        crudFile = crudFile.Replace( "#PK_WHERE_SELECTION#", Get_PRIMARY_KEY_WHERE_SELECTION( table ) );

        //    } else {
        //        fileContents = fileContents.Replace( "#PK_COLUMN_DEFINITION_INITIALIZATION#", "" );
        //        fileContents = fileContents.Replace( "#SELECT_BY_PK_METHOD#", "" );
        //    }

        //    #endregion

        //    sub.IndentDecrement( );

        //    #region single word substitutions

        //    fileContents = fileContents.Replace( "#NAMESPACE#", sub.NAMESPACE);
        //    fileContents = fileContents.Replace( "#CLASS_NAME#", sub.CLASS_NAME );
        //    fileContents = fileContents.Replace( "#CLASS_VARIABLE_NAME#", sub.CLASS_VARIABLE_NAME );
        //    fileContents = fileContents.Replace( "#TABLE_NAME#", sub.TABLE_NAME );

        //    #endregion
        //    */

        //    // create the directory if it does not exist
        //    if( !Directory.Exists(path) ){
        //        Directory.CreateDirectory(path);
        //    }

        //    StreamWriter streamWriter = new StreamWriter(path + "_" + className + "Gateway.cs");
        //    streamWriter.Write(fileContents);
        //    streamWriter.Close();

        //}

        public void GenerateGateway(Table table, string targetNamespace, string path){
        
            string fileContents = Utility.GetResource("PriorityIt.DataTierGenerator.Resource.Gateway.txt");
            string className = Utility.CleanWhitespace(Utility.FormatPascal(table.ProgrammaticAlias));

            fileContents = fileContents.Replace( "#ROOT_NAMESPACE#", targetNamespace );
            fileContents = fileContents.Replace("#ENTITY_NAME#", className);

            // create the directory if it does not exist
            if( !Directory.Exists(path) ){
                Directory.CreateDirectory(path);
            }

            if( !File.Exists(path + className + "Gateway.cs") ){
                StreamWriter streamWriter = new StreamWriter(path + className + "Gateway.cs");
                streamWriter.Write(fileContents);
                streamWriter.Close();
            }

        }

        #endregion

        #region private implementation

        //private string Get_INTERNAL_FIELD_ENUMERATION( Table table ) {

        //    StringBuilder columnList = new StringBuilder( );
        //    ArrayList columns = table.Columns;
        //    int columnCount = table.Columns.Count;

        //    for ( int index = 0; index < columnCount; index++ ) {
        //        columnList.Append( TWO_TABS );
        //        columnList.Append( ( ( Column )columns[index] ).ProgrammaticAlias );
        //        if ( index < columnCount - 1 ) {
        //            columnList.Append( "," + Environment.NewLine );
        //        }
        //    }

        //    return columnList.ToString( );
        //}

        //private string GetQueryColumnListAll( Table table ) {

        //    StringBuilder columnList = new StringBuilder( );
        //    List<Column> columns = table.Columns;
        //    int columnCount = table.Columns.Count;

        //    for ( int index = 0; index < columnCount; index++ ) {
        //        columnList.Append( "[" );
        //        columnList.Append( ( ( Column )columns[index] ).Name );
        //        columnList.Append( "]" );
        //        if ( index < columnCount - 1 ) {
        //            columnList.Append( ", " );
        //        }
        //    }

        //    return columnList.ToString( );
        //}

        //private string Get_NEW_DATA_OBJECT_FROM_READER( string className, Table table ) {

        //    StringBuilder newDataObjectString = new StringBuilder( );
        //    List<Column> columns = table.Columns;
        //    int columnCount = table.Columns.Count;

        //    newDataObjectString.Append( Environment.NewLine );

        //    for ( int index = 0; index < columnCount; index++ ) {

        //        newDataObjectString.Append( THREE_TABS );
        //        newDataObjectString.Append( "fieldDef = " );
        //        newDataObjectString.Append( className );
        //        newDataObjectString.Append( "Gateway.FieldDefinitions[( int )" );
        //        newDataObjectString.Append( className );
        //        newDataObjectString.Append( "FieldIndex." );
        //        newDataObjectString.Append( ( ( Column )columns[index] ).ProgrammaticAlias );
        //        newDataObjectString.Append( "];" );
        //        newDataObjectString.Append( Environment.NewLine );

        //        newDataObjectString.Append( THREE_TABS );
        //        newDataObjectString.Append( "if ( dataReader.GetSchemaTable( ).Select(\"ColumnName = \'\" + fieldDef.SourceColumnName + \"\'\" ).Length != 0 ) {" );
        //        newDataObjectString.Append( Environment.NewLine );

        //        newDataObjectString.Append( FOUR_TABS );
        //        newDataObjectString.Append( "if( dataReader[fieldDef.SourceColumnName] != DBNull.Value ){" );
        //        newDataObjectString.Append( Environment.NewLine );

        //        newDataObjectString.Append( FIVE_TABS );
        //        newDataObjectString.Append( "dataObject.Fields[( int )" );
        //        newDataObjectString.Append( className );
        //        newDataObjectString.Append( "FieldIndex." );
        //        newDataObjectString.Append( ( ( Column )columns[index] ).ProgrammaticAlias );
        //        newDataObjectString.Append( "].Value = " );
        //        newDataObjectString.Append( Environment.NewLine );

        //        newDataObjectString.Append( SIX_TABS );
        //        newDataObjectString.Append( "( " );
        //        newDataObjectString.Append( ( ( Column )columns[index] ).SystemType );
        //        newDataObjectString.Append( " )dataReader[fieldDef.SourceColumnName];" );
        //        newDataObjectString.Append( Environment.NewLine );

        //        newDataObjectString.Append( FOUR_TABS );
        //        newDataObjectString.Append( "}" );
        //        newDataObjectString.Append( Environment.NewLine );

        //        newDataObjectString.Append( THREE_TABS );
        //        newDataObjectString.Append( "}" );
        //        newDataObjectString.Append( Environment.NewLine );
        //    }

        //    return newDataObjectString.ToString();

        //}

        //private string Get_FIELD_DEFINITION_INITIALIZATION( Table table ) {

        //    StringBuilder fields = new StringBuilder( );
        //    List<Column> columns = table.Columns;
        //    int columnCount = table.Columns.Count;

        //    for ( int index = 0; index < columnCount; index++ ) {
        //        fields.Append( GetSingle_FieldDefinitionInitialization( table, table.Columns, index ) );
        //        if ( index < columnCount - 1 ) {
        //            fields.Append( "," );
        //            fields.Append( Environment.NewLine );
        //        }
        //    }

        //    return fields.ToString( );

        //}

        //private string GetSingle_FieldDefinitionInitialization( Table table, List<Column> columns, int index ) {

        //    StringBuilder field = new StringBuilder( );
        //    Column column = ( Column )columns[index];

        //    field.Append( THREE_TABS );
        //    field.Append( "new FieldDefinition(\"" );
        //    field.Append( column.ProgrammaticAlias );
        //    field.Append( "\", \"" );
        //    field.Append( column.EnumeratedTypeName );
        //    field.Append( "\", typeof(" );
        //    field.Append( column.SystemType );
        //    field.Append( "), \"" );
        //    field.Append( table.Schema );
        //    field.Append( "\", \"" );
        //    field.Append( table.Name );
        //    field.Append( "\", \"" );
        //    field.Append( column.Name );
        //    field.Append( "\", \"" );
        //    field.Append( column.Type );
        //    field.Append( "\", " );
        //    if ( column.Length.Length == 0 ) {
        //        field.Append( "0" );
        //    } else {
        //        field.Append( column.Length.ToString() );
        //    }
        //    field.Append( ", " );
        //    if ( column.Scale.Length == 0 ) {
        //        field.Append( "0" );
        //    } else {
        //        field.Append( column.Scale );
        //    }
        //    field.Append( ", " );
        //    if ( column.Precision.Length == 0 ) {
        //        field.Append( "0" );
        //    } else {
        //        field.Append( column.Precision );
        //    }
        //    field.Append( ", " );
        //    if ( column.IsIdentity ) {
        //        field.Append( "true" );
        //    } else {
        //        field.Append( "false" );
        //    }
        //    field.Append( ", " );
        //    if ( column.IsNullable ) {
        //        field.Append( "true" );
        //    } else {
        //        field.Append( "false" );
        //    }
        //    field.Append( ", " );
        //    field.Append( index.ToString( ) );
        //    field.Append( ")" );

        //    return field.ToString( );
        
        //}

        //private string Get_PRIMARY_KEY_FIELD_DEFINITION_INITIALIZATION( Table table ){

        //    StringBuilder sb = new StringBuilder( );
        //    List<Column> columns = table.PrimaryKeys;
        //    int columnCount = table.PrimaryKeys.Count;

        //    sb.Append( Environment.NewLine );
        //    sb.Append( TWO_TABS );
        //    sb.Append( "internal FieldDefinition[] primaryKey = {" );
        //    sb.Append( Environment.NewLine );

        //    for ( int index = 0; index < columnCount; index++ ) {
        //        sb.Append( GetSingle_FieldDefinitionInitialization( table, table.PrimaryKeys, index ) );
        //        if ( index < columnCount - 1 ) {
        //            sb.Append( "," );
        //        }
        //        sb.Append( Environment.NewLine );
        //    }

        //    sb.Append( TWO_TABS );
        //    sb.Append( "};" );
        //    sb.Append( Environment.NewLine );

        //    return sb.ToString();
        //}

        private string Get_PRIMARY_KEY_PARAMETER_LIST( Table table ) {

            StringBuilder primaryKeyParameterList = new StringBuilder( );
            List<Column> columns = table.PrimaryKeys;
            int columnCount = table.PrimaryKeys.Count;

            for ( int index = 0; index < columnCount; index++ ) {
                primaryKeyParameterList.Append( ( ( Column )columns[index] ).SystemType );
                primaryKeyParameterList.Append( " " );
                primaryKeyParameterList.Append( Utility.FormatCamel(( ( Column )columns[index] ).ProgrammaticAlias) );
                if ( index < columnCount - 1 ) {
                    primaryKeyParameterList.Append( ", " );
                }
            }

            return primaryKeyParameterList.ToString( );

        }

        private string Get_PRIMARY_KEY_FIELD_DEFINITIONS_INITIALIZATION( Table table, string className ) {

            StringBuilder sb = new StringBuilder( );
            List<Column> columns = table.PrimaryKeys;
            int columnCount = table.PrimaryKeys.Count;

            for ( int index = 0; index < columnCount; index++ ) {
                sb.Append( "pkFieldDefinitions[" );
                sb.Append( index.ToString( ) );
                sb.Append( "] = " );
                sb.Append( className );
                sb.Append( "Gateway.FieldDefinitions[( int )" );
                sb.Append( className );
                sb.Append( "FieldIndex.Client];" );
                if ( index < columnCount - 1 ) {
                    sb.Append( Environment.NewLine );
                }
            }

            return "";
        }

        private string Get_PRIMARY_KEY_FIELD_VALUES_INITIALIZATION( Table table ) {
            return "";
        }

        private string Get_PRIMARY_KEY_WHERE_SELECTION( Table table ) {
            return "";
        }

        #endregion
    }
}
