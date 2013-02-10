using System;
using System.Collections.Generic;
using System.Text;

using SumDataTierGenerator.Common;

namespace SumDataTierGenerator.CodeGenerationFactory
{

    public class UserTableGateway : GeneratorBase
    {

        private Table m_Table;
        private string m_PK_PARAMETER_TYPE_LIST;

        #region constructors / desturctors

        public UserTableGateway()
            : this(null)
        {
        }

        public UserTableGateway(string rootNamespace)
            : this(rootNamespace, null)
        {
        }

        public UserTableGateway(string rootNamespace, string providerType)
            : this(rootNamespace, providerType, null)
        {
        }

        public UserTableGateway(string rootNamespace, string providerType, Table table)
            : base(rootNamespace, providerType, table)
        {
            m_Table = table;
            string pkType = PK_PARAMETER_TYPE_LIST;

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
        }

        #endregion

        #region public properties

        /// <summary>
        /// pascal case table name with spaces as underline
        /// </summary>
        public override string CLASS_NAME
        {
            get
            {
                if (m_CLASS_NAME == "")
                {
                    m_CLASS_NAME = CLASS_NAME_PREFIX + "Gateway";
                }

                return m_CLASS_NAME;
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

        #endregion

        #region event handlers / overrides

        public override string ToString()
        {

            base.CLASS_SUMMARY = "Provides CRUD functionality for the #TABLE_NAME# table.";

            return base.ToString();
        }

        protected override void OnGetUsingStatements()
        {

            base.OnGetUsingStatements();

            AppendLine("using System.Collections.Generic;");
            AppendLine("using System.Data;");
            AppendLine("using System.Data.Common;");
            AppendLine("using System.Text;");

        }

        protected override void OnGetRegion_ConstructorsDestructors()
        {

            AppendLine();
            AppendLine("static #CLASS_NAME#(){");
            AppendLine("}");

            AppendLine();
            AppendLine("private #CLASS_NAME#(){");
            AppendLine("}");

        }

        #endregion

        #region CRUD methods

        protected virtual void OnCRUD_Update()
        {

            if (m_Table.PrimaryKey == null || m_Table.PrimaryKey.PkColumns.Length == 0)
            {
                return;
            }

            //List<Column> pkList = m_Table.PrimaryKey.Columns;
            int columnCount = m_Table.PrimaryKey.PkColumns.Length;

            AppendLine();
            AppendLine("public void Update( #CONCRETE_DATA_ENTITY_TYPE_NAME# dataObject ){");
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
            AppendLine("for( int index = 0; index < FieldDefinitions.Length; index++ ){");
            IndentIncrement();
            AppendLine();

            AppendLine("if( dataObject.Fields[index].IsDirty ){");
            IndentIncrement();
            AppendLine();

            AppendLine("fieldDefinitions.Add( FieldDefinitions[index] );");
            AppendLine("fields.Add( dataObject.Fields[index] );");

            AppendLine();
            IndentDecrement();
            AppendLine("}");

            AppendLine();
            IndentDecrement();
            AppendLine("}");

            AppendLine();
            AppendStartLine("query = GatewayHelper.BuildUpdateByPrimaryKeyQuery( ");
            AppendEndLine("SchemaName, FieldDefinitions, dataObject.Fields );");

            #endregion

            #region initialize the primary key

            AppendLine();
            AppendLine("// gather the primary key fields");
            AppendLine("for( int index = 0; index < FieldDefinitions.Length; index++ ){");
            IndentIncrement();
            AppendLine();

            AppendLine("if( FieldDefinitions[index].IsPrimaryKey ){");
            IndentIncrement();
            AppendLine();

            AppendLine("fieldDefinitions.Add( FieldDefinitions[index] );");
            AppendLine("fields.Add( dataObject.Fields[index] );");
            AppendLine("//pkFieldDefinitions.Add( FieldDefinitions[index] );");
            AppendLine("//pkFields.Add( dataObject.Fields[index] );");

            AppendLine();
            AppendLine("//if( !fieldDefinitions.Contains(FieldDefinitions[index]) ){");
            IndentIncrement();
            AppendLine();

            AppendLine("//fieldDefinitions.Add( FieldDefinitions[index] );");
            AppendLine("//fields.Add( dataObject.Fields[index] );");

            AppendLine();
            IndentDecrement();
            AppendLine("//}");

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
            AppendLine("//query += GatewayHelper.BuildSelectAllQuery( SchemaName, FieldDefinitions );");
            AppendLine("//query += Environment.NewLine;");
            AppendLine("//query += GatewayHelper.BuildWhereClause( pkFieldDefinitions.ToArray() );");

            AppendLine();
            IndentDecrement();
            AppendLine("//}");
            AppendLine();
            AppendLine("dataObjects = GetDataObjectsByQuery(query, fields );");
            AppendLine();
            AppendLine("if ( dataObjects.Count == 1 ) {");
            IndentIncrement();
            AppendLine("dataObject.IsDirty = false;");
            AppendLine();
            AppendLine("for ( int index = 0; index < FieldDefinitions.Length; index++ ) {");
            IndentIncrement();
            AppendLine("dataObject.Fields[index].Value = dataObjects[0].Fields[index].Value;");
            AppendLine("dataObject.Fields[index].IsDirty = false;");
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

        #endregion

        #region private implementation
        #endregion

    }

}
