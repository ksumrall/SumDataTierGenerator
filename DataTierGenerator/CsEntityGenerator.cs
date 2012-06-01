// ===============================================================================
//
// CsEntityGenerator.cs
//
// This file contains the implementations of the class CsEntityGenerator
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
using System.Reflection;
using System.Resources;
using System.Text;
using System.Xml;

namespace PriorityIt.DataTierGenerator {
    /// <summary>
    /// Summary description for CsEntityGenerator.
    /// </summary>
    public class CsEntityGenerator {

        #region private member variables

        const string TAB = "    ";
        const string TWO_TABS = TAB + TAB;
        const string THREE_TABS = TWO_TABS + TAB;
        const string FOUR_TABS = THREE_TABS + TAB;
        const string FIVE_TABS = FOUR_TABS + TAB;
        const string SIX_TABS = FIVE_TABS + TAB;
        const string SEVEN_TABS = SIX_TABS + TAB;

        #endregion

        #region constructors / destructors

        public CsEntityGenerator( ) {
            //
            // TODO: Add constructor logic here
            //
        }


        ~CsEntityGenerator( ) {
        }


        #endregion

        #region public properties
        #endregion

        #region public methods

        public void GenerateEntity( Table table, string targetNamespace, XmlDocument dataMappingXml, string path ) {

            string fileContents = Utility.GetResource( "PriorityIt.DataTierGenerator.Resource.Entity.txt" );
            string className = Utility.CleanWhitespace( Utility.FormatPascal( table.ClassName ) );
            string internalMemberVariables = "";
            string publicProperties = "";
            string variableName;

            #region build the internal member variables AND public properties

            //foreach ( Column column in table.Columns ) {

            //    variableName = column.Name.Replace( " ", "" );

            //    xPath =
            //        "/Databases/Database[@Name='MSSQL']/FromDatabase/Type[@Name='"
            //        + column.Type + "']/CSharp";
            //    node = dataMappingXml.SelectSingleNode( xPath );

            //    publicProperties +=
            //        Environment.NewLine + TWO_TABS + "/// <summary>" + Environment.NewLine
            //        + TWO_TABS + "/// " + column.Description + Environment.NewLine
            //        + TWO_TABS + "/// </summary>" + Environment.NewLine
            //        + TWO_TABS + "public " + column.SystemType + " " + column.PropertyName + "{" + Environment.NewLine
            //        + THREE_TABS + "get{" + Environment.NewLine
            //        + THREE_TABS + TAB + "object valueToReturn = (" + column.SystemType + ")((Field)fieldValues[(int)"
            //            + className + "FieldIndex." + column.PropertyName + "]).CurrentValue;" + Environment.NewLine
            //        + THREE_TABS + TAB + "if(valueToReturn == null) {" + Environment.NewLine
            //        + THREE_TABS + TWO_TABS + "valueToReturn = TypeDefaultValue.GetDefaultValue(typeof(" + column.SystemType + "));" + Environment.NewLine
            //        + THREE_TABS + TAB + "}" + Environment.NewLine
            //        + THREE_TABS + TAB + "return (" + column.SystemType + ")valueToReturn;" + Environment.NewLine
            //        + THREE_TABS + "}" + Environment.NewLine;

            //    if ( !column.IsComputed && !column.IsIdentity ) {
            //        publicProperties +=
            //            THREE_TABS + "set{" + Environment.NewLine
            //            + THREE_TABS + TAB + "((Field)fieldValues[(int)"
            //                + className + "FieldIndex." + column.PropertyName + "]).CurrentValue = value;" + Environment.NewLine
            //            + THREE_TABS + TAB + "m_IsDirty = true;" + Environment.NewLine
            //            + THREE_TABS + "}" + Environment.NewLine;
            //    }

            //    publicProperties +=
            //        TWO_TABS + "}" + Environment.NewLine;
            //}

            //if ( internalMemberVariables.Length > 0 ) {
            //    internalMemberVariables += Environment.NewLine;
            //}

            #endregion

            fileContents = fileContents.Replace( "#FIELD_COUNT#", table.Columns.Count.ToString( ) );
            if ( table.PrimaryKey != null && table.PrimaryKey.Columns.Count > 0 ) {
                fileContents = fileContents.Replace( "#PRIMARY_FIELD_COUNT#", table.PrimaryKey.Columns.Count.ToString() );
            }
            fileContents = fileContents.Replace( "#FIELD_INITIALIZATION#", Get_FIELD_INITIALIZATION( table ) );
            fileContents = fileContents.Replace( "#PRIMARY_FIELD_INITIALIZATION#", GetPrimaryKeyFieldInitializationString( table ) );
            fileContents = fileContents.Replace( "#PUBLIC_PROPERTIES#", Get_PUBLIC_PROPERTIES( table, className, dataMappingXml ) );

            fileContents = fileContents.Replace( "#ROOT_NAMESPACE#", targetNamespace );
            fileContents = fileContents.Replace( "#ENTITY_NAME#", className );

            // create the directory if it does not exist
            if ( !Directory.Exists( path ) ) {
                Directory.CreateDirectory( path );
            }

            StreamWriter streamWriter = new StreamWriter( path + className + "DataObject.cs" );
            streamWriter.Write( fileContents );
            streamWriter.Close( );

        }

        #endregion

        #region private implementation

        private string Get_FIELD_INITIALIZATION( Table table ) {

            StringBuilder fields = new StringBuilder( );
            int columnCount = table.Columns.Count;

            for ( int index = 0; index < columnCount; index++ ) {
                fields.Append( THREE_TABS );
                fields.Append( "Fields[" );
                fields.Append( index.ToString() );
                fields.Append( "] = new FieldValue( #ENTITY_NAME#Gateway.Instance.FieldDefinitionArray[" );
                fields.Append( index.ToString( ) );
                fields.Append( "] );" );
                if ( index < columnCount ) {
                    fields.Append( Environment.NewLine );
                }
            }

            return fields.ToString();
        }

        private string GetPrimaryKeyFieldInitializationString( Table table ) {
            return "";
        }

        private string Get_PUBLIC_PROPERTIES( Table table, string className, XmlDocument dataMappingXml ) {

            StringBuilder properties = new StringBuilder( );
            List<Column> columns = table.Columns;
            int columnCount = table.Columns.Count;

            string xPath;
            XmlNode node;

            for ( int index = 0; index < columnCount; index++ ) {

                xPath =
                    "/Databases/Database[@Name='MSSQL']/FromDatabase/Type[@Name='"
                    + (( Column )columns[index]).Type + "']/CSharp";
                node = dataMappingXml.SelectSingleNode( xPath );

                properties.Append( GetPublicPropertyString( ( Column )columns[index], className ) );
                if ( index < columnCount - 1 ) {
                    properties.Append( Environment.NewLine );
                }
            }

            if ( properties.Length > 0 ) {
                properties.Insert( 0, Environment.NewLine );
                properties.Append( Environment.NewLine );
            }
            return properties.ToString( );
        }

        private string GetPublicPropertyString( Column column, string className ) {

            StringBuilder property = new StringBuilder( );

            property.Append( TWO_TABS + "/// <summary>" + Environment.NewLine );
            property.Append( TWO_TABS + "/// " + column.Description + Environment.NewLine);
            property.Append( TWO_TABS + "/// </summary>" + Environment.NewLine);

            if( column.IsNullable
                && ( column.LanguageType.ToLower( ) != "string" && column.LanguageType.ToLower( ) != "byte[]" ) ) {
                property.Append( TWO_TABS + "public Nullable<" + column.LanguageType + "> " + column.PropertyName + "{" + Environment.NewLine );
                property.AppendLine (THREE_TABS + "get{");
                property.Append (FOUR_TABS);
                property.AppendLine ("if(Fields[( int )" + className + "FieldIndex." + ((Column)column).PropertyName + "].Value == null)");
                property.Append (FIVE_TABS);
                property.AppendLine ("return new Nullable<" + column.LanguageType + ">();");
                property.Append (FOUR_TABS);
                property.AppendLine ("else");
                property.Append (FIVE_TABS);
                property.AppendLine ("return new Nullable<" + column.LanguageType + ">((" + column.LanguageType + ")Fields[( int )" + className + "FieldIndex." + ((Column)column).PropertyName + "].Value);");
                property.AppendLine (THREE_TABS + "}");
            }else{
                property.Append( TWO_TABS + "public " + column.LanguageType + " " + column.PropertyName + "{" + Environment.NewLine );
                
                // add the getter
                property.Append (THREE_TABS + "get{" + Environment.NewLine);
                property.Append (FOUR_TABS);
                property.Append ("return (");
                property.Append (column.LanguageType);
                property.Append (")Fields[( int )");
                property.Append (className);
                property.Append ("FieldIndex.");
                property.Append (((Column)column).PropertyName);
                property.Append ("].Value;");
                property.Append (Environment.NewLine);
                property.Append (THREE_TABS + "}" + Environment.NewLine);
            }           

            // add the setter if not an autogenerated identity column
            if ( !column.IsComputed && !column.IsIdentity ) {
                property.Append( THREE_TABS );
                property.Append( "set{" );
                property.Append( Environment.NewLine);

                if ( column.IsNullable
                    && ( column.LanguageType.ToLower( ) != "string" && column.LanguageType.ToLower( ) != "byte[]" ) ) {

                    #region add the setter for a nullable column

                    property.Append( FOUR_TABS );
                    property.Append( "if( " );
                    property.Append( "Fields[( int )" );
                    property.Append( className );
                    property.Append( "FieldIndex." );
                    property.Append( ( ( Column )column ).PropertyName );
                    property.Append( "].Value == null ){" );
                    property.Append( Environment.NewLine );

                    //            // orig value is null. check if it is being set to a value
                    //            if ( value.HasValue ) {
                    property.Append( FIVE_TABS );
                    property.Append( "if( value.HasValue ) {" );
                    property.Append( Environment.NewLine );
                    
                    //                Fields[( int )PaymentScheduleFieldIndex.PaymentTypeId].Value = value;
                    //                m_IsDirty = true;
                    property.Append( SIX_TABS );
                    property.Append( "Fields[( int )" );
                    property.Append( className );
                    property.Append( "FieldIndex." );
                    property.Append( ( ( Column )column ).PropertyName );
                    property.Append( "].Value = value;" );
                    property.Append( Environment.NewLine );

                    property.Append( SIX_TABS );
                    property.Append( "m_IsDirty = true;" );
                    property.Append( Environment.NewLine );

                    //            }
                    //        } else {
                    property.Append( FIVE_TABS );
                    property.Append( "}" );
                    property.Append( Environment.NewLine );
                    property.Append( FOUR_TABS );
                    property.Append( "} else {" );
                    property.Append( Environment.NewLine );

                    //            // original value is not null. check if it is being set to null
                    //            if ( !value.HasValue ) {
                    property.Append( FIVE_TABS );
                    property.Append( "if( !value.HasValue ) {" );
                    property.Append( Environment.NewLine );

                    //                Fields[( int )PaymentScheduleFieldIndex.PaymentTypeId].Value = null;
                    //                m_IsDirty = true;
                    property.Append( SIX_TABS );
                    property.Append( "Fields[( int )" );
                    property.Append( className );
                    property.Append( "FieldIndex." );
                    property.Append( ( ( Column )column ).PropertyName );
                    property.Append( "].Value = null;" );
                    property.Append( Environment.NewLine );

                    property.Append( SIX_TABS );
                    property.Append( "m_IsDirty = true;" );
                    property.Append( Environment.NewLine );

                    //            } else {
                    property.Append( FIVE_TABS );
                    property.Append( "} else {" );
                    property.Append( Environment.NewLine );

                    //                // check if the original value is different from the new value
                    //                if ( ((Nullable<int>)Fields[( int )PaymentScheduleFieldIndex.PaymentTypeId].Value).Value != value.Value ) {
                    property.Append( SIX_TABS );
                    property.Append( "if( ((Nullable<" );
                    property.Append( column.LanguageType );
                    property.Append( ">)Fields[( int )" );
                    property.Append( className );
                    property.Append( "FieldIndex." );
                    property.Append( ( ( Column )column ).PropertyName );
                    property.Append( "].Value).Value != value.Value ) {" );
                    property.Append( Environment.NewLine );

                    //                    Fields[( int )PaymentScheduleFieldIndex.PaymentTypeId].Value = null;
                    //                    m_IsDirty = true;
                    property.Append( SEVEN_TABS );
                    property.Append( "Fields[( int )" );
                    property.Append( className );
                    property.Append( "FieldIndex." );
                    property.Append( ( ( Column )column ).PropertyName );
                    property.Append( "].Value = value;" );
                    property.Append( Environment.NewLine );

                    property.Append( SEVEN_TABS );
                    property.Append( "m_IsDirty = true;" );
                    property.Append( Environment.NewLine );

                    //                }
                    property.Append( SIX_TABS );
                    property.Append( "}" );
                    property.Append( Environment.NewLine );

                    //            }
                    property.Append( FIVE_TABS );
                    property.Append( "}" );
                    property.Append( Environment.NewLine );

                    //        }
                    property.Append( FOUR_TABS );
                    property.Append( "}" );
                    property.Append( Environment.NewLine );

                    #endregion

                } else {

                    #region add the setter for a non-nullable column

                    property.Append( FOUR_TABS );
                    property.Append( "if( " );
                    property.Append( "Fields[( int )" );
                    property.Append( className );
                    property.Append( "FieldIndex." );
                    property.Append( ( ( Column )column ).PropertyName );
                    property.Append( "].Value == null" );
                    property.Append( Environment.NewLine );
                    property.Append( FIVE_TABS );
                    property.Append( "|| ( " );
                    property.Append( column.LanguageType );
                    property.Append( " )Fields[( int )" );
                    property.Append( className );
                    property.Append( "FieldIndex." );
                    property.Append( ( ( Column )column ).PropertyName );
                    property.Append( "].Value != value ){" );
                    property.Append( Environment.NewLine );

                    property.Append( FIVE_TABS );
                    property.Append( "Fields[( int )" );
                    property.Append( className );
                    property.Append( "FieldIndex." );
                    property.Append( ( ( Column )column ).PropertyName );
                    property.Append( "].Value = value;" );
                    property.Append( Environment.NewLine );

                    property.Append( FIVE_TABS );
                    property.Append( "m_IsDirty = true;" );
                    property.Append( Environment.NewLine );

                    property.Append( FOUR_TABS );
                    property.Append( "}" );
                    property.Append( Environment.NewLine );

                    //public DateTime OpenDate{
                    //    get{
                    //        return (DateTime)Fields[( int )PaymentScheduleFieldIndex.OpenDate].Value;
                    //    }
                    //    set{
                    //        if ( Fields[( int )PaymentScheduleFieldIndex.OpenDate].Value == null ) {
                    //            Fields[( int )PaymentScheduleFieldIndex.PaymentTypeId].Value = value;
                    //            m_IsDirty = true;
                    //        } else if ( ( DateTime )Fields[( int )PaymentScheduleFieldIndex.OpenDate].Value != value ) {
                    //            Fields[( int )PaymentScheduleFieldIndex.PaymentTypeId].Value = value;
                    //            m_IsDirty = true;
                    //        }
                    //    }
                    //}
                    #endregion

                }

                property.Append( THREE_TABS );
                property.Append( "}" );
                property.Append( Environment.NewLine );
            }

            property.Append( TWO_TABS + "}" + Environment.NewLine);

            return property.ToString( );
        }

        #endregion
    }
}
