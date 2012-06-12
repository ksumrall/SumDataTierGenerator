using System;
using System.Collections.Generic;
using System.Text;

namespace PriorityIt.DataTierGenerator.Generator {

    class AbstractBusinessEntityGenerator : GeneratorBase {

        #region private and protected member variables

        private string m_DdataMappingXml;

        #endregion

        #region internal structured members
        #endregion

        #region constructors / desturctors

        public AbstractBusinessEntityGenerator( ) {
        }

        public AbstractBusinessEntityGenerator( string rootNamespace )
            : base( rootNamespace ) {
        }

        public AbstractBusinessEntityGenerator( string rootNamespace, Table table )
            : base( rootNamespace, table ) {
        }

        ~AbstractBusinessEntityGenerator( ) {
        }

        #endregion

        #region public properties

        public string DdataMappingXml {
            get {
                return m_DdataMappingXml;
            }
            set {
                m_DdataMappingXml = value;
            }
        }

        public override string CLASS_NAME {
            get {
                if ( m_CLASS_NAME == "" ) {
                    m_CLASS_NAME = Utility.FormatPascal( Table.ProgrammaticAlias );
                    m_CLASS_NAME = "_" + m_CLASS_NAME + "BusinessObject";
                }

                return m_CLASS_NAME;
            }
        }
        
    #endregion

        #region event handlers / overrides

        public override string ToString( ) {

            base.CLASS_SUMMARY = "Base business class for #CLASS_NAME_PREFIX#BusinessEntity.";
            base.m_TARGET_NAMESPACE = base.ROOT_NAMESPACE + ".BLL.Entity.Abstract";
            base.IsAbstract = true;

            return base.ToString( );

        }

        protected override void OnGetUsingStatements( ) {
            base.OnGetUsingStatements( );

            AppendLine( );
            AppendLine( "using #ROOT_NAMESPACE#.DAL.Entity;" );
            AppendLine( "using #ROOT_NAMESPACE#.DAL.Gateway;" );
        }

        protected override void OnGetRegion_PrivateProtectedMemberVariables( ) {

            base.OnGetRegion_PrivateProtectedMemberVariables( );

            AppendLine( );
            AppendLine( "protected #CONCRETE_DATA_ENTITY_TYPE_NAME# m_#CONCRETE_DATA_ENTITY_TYPE_NAME#;" );
            AppendLine( "protected #CONCRETE_GATEWAY_TYPE_NAME# m_#CONCRETE_GATEWAY_TYPE_NAME#;" );

        }

        protected override void OnConstructorBody( ) {

            AppendLine( );
            IndentIncrement( );
            AppendLine( "m_#CONCRETE_GATEWAY_TYPE_NAME# = new #CONCRETE_GATEWAY_TYPE_NAME#();" );
            AppendLine( "m_#CONCRETE_DATA_ENTITY_TYPE_NAME# = new #CONCRETE_DATA_ENTITY_TYPE_NAME#();" );
            IndentDecrement( );
        }

        protected override void OnPostConstructor( ) {

            AppendLine( );
            AppendLine( "public #CLASS_NAME#( #CONCRETE_DATA_ENTITY_TYPE_NAME# #CLASS_VARIABLE_NAME_PREFIX#DataObject ){" );

            AppendLine( );
            IndentIncrement( );
            AppendLine( "m_#CONCRETE_GATEWAY_TYPE_NAME# = new #CONCRETE_GATEWAY_TYPE_NAME#();" );
            AppendLine( "m_#CONCRETE_DATA_ENTITY_TYPE_NAME# = #CLASS_VARIABLE_NAME_PREFIX#DataObject;" );
            IndentDecrement( );

            AppendLine( );
            AppendLine( "}" );

            if ( m_Table.PrimaryKey.Columns.Count > 0 ) {

                AppendLine( );
                AppendLine( "public #CLASS_NAME#( #PK_PARAMETER_LIST# ){" );

                AppendLine( );
                IndentIncrement( );
                AppendLine( "m_#CONCRETE_GATEWAY_TYPE_NAME# = new #CONCRETE_GATEWAY_TYPE_NAME#();" );
                AppendLine( "m_#CONCRETE_DATA_ENTITY_TYPE_NAME# =" );
                IndentIncrement( );
                AppendLine( "m_#CONCRETE_GATEWAY_TYPE_NAME#.SelectByPrimaryKey( #PK_ARGUMENT_LIST# );" );
                IndentDecrement ();

                AppendLine ("if (m_#CONCRETE_DATA_ENTITY_TYPE_NAME# == null) {");
                IndentIncrement ();
                AppendLine ("throw new InvalidOperationException();");
                IndentDecrement ();
                AppendLine ("}");

                IndentDecrement( );
                IndentDecrement( );

                AppendLine( );
                AppendLine( "}" );

            }

            base.OnPostConstructor( );

        }

        protected override void OnGetRegion_PublicProperties( ) {

            StringBuilder properties = new StringBuilder( );
            List<Column> columns = m_Table.Columns;
            int columnCount = m_Table.Columns.Count;

            base.OnGetRegion_PublicProperties( );

            #region add the concrete_gateway_type property

            AppendLine( );
            AppendLine( "protected #CONCRETE_GATEWAY_TYPE_NAME# Gateway{" );
            IndentIncrement( );

            AppendLine( "get{" );
            IndentIncrement( );

            AppendLine( "if( m_#CONCRETE_GATEWAY_TYPE_NAME# == null ){" );
            IndentIncrement( );

            AppendLine( "m_#CONCRETE_GATEWAY_TYPE_NAME# = new #CONCRETE_GATEWAY_TYPE_NAME#();" );

            IndentDecrement( );
            AppendLine( "}" );

            AppendLine( "return m_#CONCRETE_GATEWAY_TYPE_NAME#;" );

            IndentDecrement( );
            AppendLine( "}" );

            IndentDecrement( );
            AppendLine( "}" );
            
            #endregion

            for ( int index = 0; index < columnCount; index++ ) {

                AppendLine( );
                AppendLine( "/// <summary>" );
                AppendStartLine( "/// " );
                AppendEndLine( columns[index].Description );
                AppendLine( "/// </summary>" );
                AppendStartLine( "public " );
                if ( columns[index].IsNullable
                    && ( columns[index].LanguageType.ToLower( ) != "string" && columns[index].LanguageType.ToLower( ) != "byte[]" ) ) {
                    Append( "Nullable<" );
                    Append( columns[index].LanguageType );
                    Append( ">" );
                } else {
                    Append( columns[index].LanguageType );
                }

                Append( " " );
                Append( columns[index].ProgrammaticAlias );
                AppendEndLine( "{" );
                IndentIncrement( );
                AppendLine( "get{" );
                IndentIncrement( );
                AppendStartLine( "return m_#CONCRETE_DATA_ENTITY_TYPE_NAME#." );
                Append( columns[index].ProgrammaticAlias );
                AppendEndLine( ";" );
                IndentDecrement( );
                AppendLine( "}" );

                if ( !columns[index].IsComputed && !columns[index].IsIdentity ) {
                    AppendLine( "set{" );
                    IndentIncrement();
                    AppendStartLine( "m_#CONCRETE_DATA_ENTITY_TYPE_NAME#." );
                    Append( columns[index].ProgrammaticAlias );
                    AppendEndLine( " = value;" );

                    IndentDecrement( );
                    AppendLine( "}" );
                }

                IndentDecrement( );
                AppendLine( "}" );

            }

        }

        protected override void OnGetRegion_PublicMethods( ) {

            #region create the save method

            AppendLine( );
            AppendLine( "public void Save(){" );
            AppendLine( );
            IndentIncrement( );

            AppendLine("try {");
            IndentIncrement();

            AppendLine( "if( m_#CONCRETE_DATA_ENTITY_TYPE_NAME#.IsNew ){" );
            IndentIncrement( );

            AppendLine();
            AppendLine( "OnInsert();" );
            AppendLine();

            IndentDecrement( );
            AppendLine( "}else{" );
            IndentIncrement( );

            AppendLine( );
            AppendLine( "OnUpdate();" );
            AppendLine( );

            IndentDecrement( );
            AppendLine( "}" );

            IndentDecrement();
            AppendLine("}");
            
            AppendLine("catch(System.Data.SqlClient.SqlException sqex) {");
            IndentIncrement();
            AppendLine("throw new System.Data.DataException(sqex.Message);");
            IndentDecrement();
            AppendLine("}");

            IndentDecrement( );
            AppendLine( "}" );

            #endregion

            #region create the delete method

            AppendLine( );
            AppendLine( "public void Delete(){" );
            AppendLine( );
            IndentIncrement( );

            AppendLine( "OnDelete();" );

            IndentDecrement( );
            AppendLine( "}" );

            #endregion

            #region create the OnInsert method

            AppendLine( );
            AppendLine( "protected virtual void OnInsert(){" );
            AppendLine( );
            IndentIncrement( );

            AppendLine( "Gateway.Insert( m_#CONCRETE_DATA_ENTITY_TYPE_NAME# );" );

            IndentDecrement( );
            AppendLine( );
            AppendLine( "}" );

            #endregion

            #region create the OnUpdate method

            AppendLine( );
            AppendLine( "protected virtual void OnUpdate(){" );
            AppendLine( );
            IndentIncrement( );

            AppendLine( "Gateway.Update( m_#CONCRETE_DATA_ENTITY_TYPE_NAME# );" );

            IndentDecrement( );
            AppendLine( "}" );

            #endregion

            #region create the OnDelete method

            AppendLine( );
            AppendLine( "protected virtual void OnDelete(){" );
            AppendLine( );
            IndentIncrement( );

            AppendLine( "Gateway.Delete( m_#CONCRETE_DATA_ENTITY_TYPE_NAME# );" );

            IndentDecrement( );
            AppendLine( "}" );

            #endregion

        }

        #endregion

        #region public methods
        #endregion

        #region private implementation
        #endregion

    }

}
