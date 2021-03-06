using System;
using System.Collections.Generic;
using System.Text;

using SumDataTierGenerator.Common;

namespace SumDataTierGenerator.CodeGenerationFactory
{

    public class GeneratedTableDataObject : GeneratorBase
    {

        private Table m_Table;

        private string m_PK_COLUMN_COUNT;
        private string m_PK_ARGUMENT_LIST;
        private string m_PK_PARAMETER_LIST;

        #region constructors / desturctors

        public GeneratedTableDataObject()
            : this(null)
        {
        }

        public GeneratedTableDataObject(string rootNamespace)
            : this(rootNamespace, null)
        {
        }

        public GeneratedTableDataObject(string rootNamespace, string providerType)
            : this(rootNamespace, providerType, null)
        {
        }

        public GeneratedTableDataObject(string rootNamespace, string providerType, Table table)
            : base(rootNamespace, providerType, table)
        {
            m_Table = table;
            this.SUBCLASS_NAME = "I#CLASS_NAME#, IFieldValues";
        }

        #endregion

        #region public properties

        /// <summary>
        /// 
        /// </summary>
        public override string CLASS_NAME
        {
            get
            {
                if (m_CLASS_NAME == "")
                {
                    m_CLASS_NAME = Utility.FormatPascal(base.CONCRETE_DATA_ENTITY_TYPE_NAME);
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
                if (string.IsNullOrEmpty(m_PK_COLUMN_COUNT))
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
                if (string.IsNullOrEmpty(m_PK_ARGUMENT_LIST))
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
                if (string.IsNullOrEmpty(m_PK_PARAMETER_LIST))
                {

                    if (m_Table.PrimaryKey != null && m_Table.PrimaryKey.PkColumns.Length > 0)
                    {
                        PkColumn[] pkList = m_Table.PrimaryKey.PkColumns;
                        int columnCount = m_Table.PrimaryKey.PkColumns.Length;
                        Column pkColumn;

                        for (int index = 0; index < columnCount; index++)
                        {
                            pkColumn = m_Table.GetPkColumn(pkList[index].ColumnName);
                            m_PK_PARAMETER_LIST += pkColumn.LanguageType + " " + Utility.FormatCamel(pkColumn.PropertyName);
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

        #endregion

        #region event handlers / overrides

        public override string ToString()
        {

            base.CLASS_SUMMARY = "Provide object implementation for the represented row in the #TABLE_NAME# table";

            return base.ToString();

        }

        protected override void OnGetFileComments()
        {

            AppendLine("// THIS IS AN AUTOMATICALLY GENERATED FILE!");
            AppendLine("// It contains the implementations of the class #CLASS_NAME#.");
            AppendLine("// Do not make modifications to this file since they will be overwritten");
            AppendLine("// when this file is regenerated.");
            AppendLine("//");

            base.OnGetFileComments();

        }

        protected override void OnGetUsingStatements()
        {
            base.OnGetUsingStatements();

            AppendLine("using System.Collections.Generic;");

        }

        protected override void OnPreGetClass()
        {

            AddClassInterfaceDefinition();

            base.OnPreGetClass();

        }

        protected override void OnGetRegion_PrivateProtectedMemberVariables()
        {

            base.OnGetRegion_PrivateProtectedMemberVariables();

            AppendLine();
            AppendLine("internal bool m_IsNew;");
            AppendLine("internal bool m_IsDirty;");
            AppendLine();
            AppendLine("internal FieldValue[] m_FieldValues = new FieldValue[" + base.COLUMN_COUNT + "];");
            if (m_Table.PrimaryKey != null && m_Table.PrimaryKey.PkColumns.Length > 0)
            {
                AppendLine("internal FieldValue[] m_PrimaryKeyFieldValues = new FieldValue[" + PK_COLUMN_COUNT + "];");
            }
            else
            {
                AppendLine("internal FieldValue[] m_PrimaryKeyFieldValues = new FieldValue[0];");
            }

        }

        protected override void OnGetRegion_ConstructorsDestructors()
        {

            int columnCount;

            #region build empty constructor

            AppendLine();
            AppendLine("public #CLASS_NAME#(){");

            AppendLine();
            IndentIncrement();
            AppendLine("m_IsNew = true;");
            AppendLine("m_IsDirty = false;");

            AppendLine();
            // build the regular field values
            columnCount = m_IView.Columns.Length;
            for (int index = 0; index < columnCount; index++)
            {
                AppendStartLine("m_FieldValues[");
                Append(index.ToString());
                Append("] = new FieldValue( #CONCRETE_GATEWAY_TYPE_NAME#.FieldDefinitionArray[");
                Append(index.ToString());
                AppendEndLine("] );");
            }

            if (m_Table.PrimaryKey != null && m_Table.PrimaryKey.PkColumns.Length > 0)
            {
                AppendLine();
                // build the primary key field values
                columnCount = m_Table.PrimaryKey.PkColumns.Length;
                for (int index = 0; index < columnCount; index++)
                {
                    AppendStartLine("m_PrimaryKeyFieldValues[");
                    Append(index.ToString());
                    AppendEndLine("] = m_FieldValues[( int )" + CLASS_NAME_PREFIX + "FieldIndex."
                        + m_Table.GetPkColumn(m_Table.PrimaryKey.PkColumns[index].ColumnName).PropertyName + "];");
                }
            }

            IndentDecrement();

            AppendLine();
            AppendLine("}");

            #endregion

            #region build the primary key constructor

            if (!string.IsNullOrEmpty(PK_PARAMETER_LIST))
            {

                AppendLine();
                AppendStartLine("public #CLASS_NAME#( ");
                Append(PK_PARAMETER_LIST);
                AppendEndLine(" ) : this(){");
                IndentIncrement();

                AppendLine("#CONCRETE_GATEWAY_TYPE_NAME#.LoadByPrimaryKey(this, " + PK_ARGUMENT_LIST + ");");

                IndentDecrement();
                AppendLine();
                AppendLine("}");

            }

            #endregion

        }

        protected override void OnGetRegion_PublicProperties()
        {

            #region IDataObject implementation

            AppendLine();
            AppendLine("#region IDataObject");

            // IsNew
            AppendLine();
            AppendLine("public bool IsNew {");
            IndentIncrement();

            AppendLine("get {");
            IndentIncrement();
            AppendLine("return m_IsNew;");
            IndentDecrement();
            AppendLine("}");

            AppendLine("internal set {");
            IndentIncrement();
            AppendLine("m_IsNew = value;");
            IndentDecrement();
            AppendLine("}");

            IndentDecrement();
            AppendLine("}");

            // IsDirty
            AppendLine();
            AppendLine("public bool IsDirty {");
            IndentIncrement();

            AppendLine("get {");
            IndentIncrement();
            AppendLine("return m_IsDirty;");
            IndentDecrement();
            AppendLine("}");

            AppendLine("internal set {");
            IndentIncrement();
            AppendLine("m_IsDirty = value;");
            IndentDecrement();
            AppendLine("}");

            IndentDecrement();
            AppendLine("}");

            // IGateway
            /*
            AppendLine();
            AppendLine("public #CLASS_NAME_PREFIX#Gateway Gateway {");
            IndentIncrement();

            AppendLine("get {");
            IndentIncrement();
            AppendLine("return " + CLASS_NAME_PREFIX + "Gateway.Instance;");
            IndentDecrement();
            AppendLine("}");

            IndentDecrement();
            AppendLine("}");
            */

            AppendLine();
            AppendLine("#endregion");

            #endregion

            #region IFieldValues implementation

            AppendLine();
            AppendLine("#region IFieldValues");

            AppendLine();
            AppendLine("FieldValue[] IFieldValues.FieldValues {");
            IndentIncrement();

            AppendLine("get {");
            IndentIncrement();
            AppendLine("return m_FieldValues;");
            IndentDecrement();
            AppendLine("}");

            IndentDecrement();
            AppendLine("}");

            AppendLine();
            AppendLine("FieldValue[] IFieldValues.PrimaryKeyFieldValues {");
            IndentIncrement();

            AppendLine("get {");
            IndentIncrement();
            AppendLine("return m_PrimaryKeyFieldValues;");
            IndentDecrement();
            AppendLine("}");

            IndentDecrement();
            AppendLine("}");

            AppendLine();
            AppendLine("FieldValue[] IFieldValues.DirtyFieldValues {");
            IndentIncrement();

            AppendLine("get {");
            IndentIncrement();
            AppendLine("List<FieldValue> dirtyFieldValues = new List<FieldValue>();");
            AppendLine();
            AppendLine("foreach( FieldValue fieldValue in m_FieldValues ){");
            IndentIncrement();
            AppendLine("if( fieldValue.IsDirty ){");
            IndentIncrement();
            AppendLine("dirtyFieldValues.Add(fieldValue);");
            IndentDecrement();
            AppendLine("}");
            IndentDecrement();
            AppendLine("}");
            AppendLine();
            AppendLine("return dirtyFieldValues.ToArray();");
            IndentDecrement();
            AppendLine("}");

            IndentDecrement();
            AppendLine("}");

            AppendLine();
            AppendLine("#endregion");

            #endregion

            #region IClassType implementation

            AppendLine();
            AppendLine("#region I#CLASS_NAME#");

            AddDataObjectPropertyInterfaceImplementation();

            AppendLine();
            AppendLine("#endregion");

            #endregion

            base.OnGetRegion_PublicProperties();
        }

        protected override void OnGetRegion_PublicMethods()
        {

            #region IDataObject implementation
            /*
            AppendLine();
            AppendLine("public void Save() {");

            AppendLine("\ttry {");
            AppendLine("\t\tif (IsNew) {");
            AppendLine("\t\t\tvar obj = #CONCRETE_GATEWAY_TYPE_NAME#.Insert (this);");
            AppendLine("\t\t\tforeach (FieldValue fieldValue in m_FieldValues)");
            AppendLine("\t\t\t{");
            AppendLine("\t\t\t\tfieldValue.Value = obj.m_FieldValues[fieldValue.FieldDefinition.FieldIndex].Value;");
            AppendLine("\t\t\t\tfieldValue.IsDirty = false;");
            AppendLine("\t\t\t}");
            AppendLine("\t\t\tm_IsNew = false;");
            AppendLine("\t\t\tm_IsDirty = false;");
            AppendLine("\t\t} else if (IsDirty) {");
            AppendLine("\t\t\t#CONCRETE_GATEWAY_TYPE_NAME#.Update (this);");
            AppendLine("\t\t\tforeach (FieldValue fieldValue in m_FieldValues)");
            AppendLine("\t\t\t{");
            AppendLine("\t\t\t\tfieldValue.IsDirty = false;");
            AppendLine("\t\t\t}");
            AppendLine("\t\t\tm_IsDirty = false;");
            AppendLine("\t\t}");

            switch (m_ProviderType)
            {
                case "Microsoft SQL Server (SqlClient)":
                    AppendLine("\t} catch (System.Data.SqlClient.SqlException sqex) {");
                    break;

                case "Microsoft SQL Server Compact 3.5 (SqlCeClient)":
                    AppendLine("\t} catch (System.Data.SqlServerCe.SqlCeException sqex) {");
                    break;
            }

            AppendLine("\t\tthrow new System.Data.DataException (sqex.Message);");
            AppendLine("\t}");
            AppendLine("}");

            AppendLine();
            AppendLine("public void Delete() {");
            AppendLine("\t#CONCRETE_GATEWAY_TYPE_NAME#.Delete (this);");
            AppendLine("}");
            */
            #endregion

            AppendLine();
            AppendLine("public bool IsPropertyDirty(#CLASS_NAME_PREFIX#FieldIndex fieldIndex) {");
            AppendLine("\treturn m_FieldValues[(int)fieldIndex].IsDirty;");
            AppendLine("}");

        }

        #endregion

        #region private implementation

        private void AddClassInterfaceDefinition()
        {

            AppendLine();
            AppendLine("public interface I#CLASS_NAME# : IDataObject{");

            IndentIncrement();

            foreach (Column column in m_IView.Columns)
            {

                AppendLine();

                if (column.IsNullable
                    && (column.LanguageType.ToLower() != "string" && column.LanguageType.ToLower() != "byte[]"))
                {

                    AppendLine("Nullable<" + column.LanguageType + "> " + column.PropertyName + "{");
                }
                else
                {
                    AppendLine("" + column.LanguageType + " " + column.PropertyName + "{");
                }

                IndentIncrement();
                AppendLine("get;");

                if (!column.IsComputed && !column.IsIdentity)
                {
                    AppendLine("set;");
                }

                IndentDecrement();
                AppendLine("}");
            }

            IndentDecrement();

            AppendLine("}");
        }

        private void AddDataObjectPropertyInterfaceImplementation()
        {

            bool isNullableType;

            foreach (Column column in m_IView.Columns)
            {

                AppendLine();

                if (column.IsNullable
                    && (column.LanguageType.ToLower() != "string" && column.LanguageType.ToLower() != "byte[]"))
                {

                    isNullableType = true;
                    AppendLine("public Nullable<" + column.LanguageType + "> " + column.PropertyName + "{");

                }
                else
                {
                    isNullableType = false;
                    AppendLine("public " + column.LanguageType + " " + column.PropertyName + "{");
                }

                IndentIncrement();

                #region get implementation

                AppendLine("get{");
                IndentIncrement();

                if (isNullableType)
                {
                    AppendLine("if(m_FieldValues[( int )" + CLASS_NAME_PREFIX + "FieldIndex." + column.PropertyName + "].Value == null){");
                    AppendLine("\treturn new Nullable<" + column.LanguageType + ">();");
                    AppendLine("}else{");
                    AppendLine("\treturn (Nullable<" + column.LanguageType + ">)m_FieldValues[( int )" + CLASS_NAME_PREFIX + "FieldIndex." + ((Column)column).PropertyName + "].Value;");
                    AppendLine("}");
                }
                else
                {
                    AppendLine("return (" + column.LanguageType + ")m_FieldValues[( int )" + CLASS_NAME_PREFIX + "FieldIndex." + ((Column)column).PropertyName + "].Value;");
                }

                IndentDecrement();
                AppendLine("}");

                #endregion

                if (!column.IsComputed && !column.IsIdentity)
                {
                    #region set implementation

                    AppendLine("set{");
                    IndentIncrement();

                    if (isNullableType)
                    {
                        #region add the setter for a nullable column

                        AppendLine("if( m_FieldValues[( int )" + CLASS_NAME_PREFIX + "FieldIndex." + ((Column)column).PropertyName + "].Value == null ){");
                        AppendLine("\tif( value.HasValue ) {");
                        AppendLine("\t\tm_FieldValues[( int )" + CLASS_NAME_PREFIX + "FieldIndex." + ((Column)column).PropertyName + "].Value = value;");
                        AppendLine("\t\t\tm_IsDirty = true;");
                        AppendLine("\t}");
                        AppendLine("} else {");
                        AppendLine("\tif( !value.HasValue ) {");
                        AppendLine("\t\tm_FieldValues[( int )" + CLASS_NAME_PREFIX + "FieldIndex." + ((Column)column).PropertyName + "].Value = null;");
                        AppendLine("\t\tm_IsDirty = true;");
                        AppendLine("\t} else {");
                        AppendLine("\t\tif( ((Nullable<" + column.LanguageType + ">)m_FieldValues[( int )" + CLASS_NAME_PREFIX + "FieldIndex." + ((Column)column).PropertyName + "].Value).Value != value.Value ) {");
                        AppendLine("\t\t\tm_FieldValues[( int )" + CLASS_NAME_PREFIX + "FieldIndex." + ((Column)column).PropertyName + "].Value = value;");
                        AppendLine("\t\t\tm_IsDirty = true;");
                        AppendLine("\t\t}");
                        AppendLine("\t}");
                        AppendLine("}");

                        #endregion
                    }
                    else
                    {
                        #region add the setter for a non-nullable column

                        AppendLine("if( " + "m_FieldValues[( int )" + CLASS_NAME_PREFIX + "FieldIndex." + ((Column)column).PropertyName + "].Value == null");
                        AppendLine("\t|| ( " + column.LanguageType + " )m_FieldValues[( int )" + CLASS_NAME_PREFIX + "FieldIndex." + ((Column)column).PropertyName + "].Value != value ){");
                        AppendLine("\tm_FieldValues[( int )" + CLASS_NAME_PREFIX + "FieldIndex." + ((Column)column).PropertyName + "].Value = value;");
                        AppendLine("\tm_IsDirty = true;");
                        AppendLine("}");

                        #endregion
                    }

                    IndentDecrement();
                    AppendLine("}");

                    #endregion
                }
                else if (column.IsIdentity)
                {
                    AppendLine("set{");
                    IndentIncrement();

                    AppendLine("throw new Exception(\"Access violation. Unable to set the value of the identity field.\");");

                    IndentDecrement();
                    AppendLine("}");
                }

                IndentDecrement();
                AppendLine("}");

            }

        }

        #endregion

    }

}
