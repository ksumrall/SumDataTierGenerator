using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using TotalSafety.DataTierGenerator.Common;

namespace TotalSafety.DataTierGenerator.CodeGenerationFactory
{

    public class GeneratorBase
    {

        #region private and protected member variables

        protected StringBuilder m_FileBuilder;

        protected int m_IndentLevel = 0;
        protected int m_SpacesPerTab = 4;
        protected bool m_IsAbstract = false;
        protected bool m_IsStatic = false;

        protected IView m_IView;
        private string m_COLUMN_COUNT;


        private string m_SELECT_COLUMN_LIST_ALL;

        // general substitutions
        protected string m_FILE_NAME;
        protected string m_NAMESPACE;
        protected string m_TABLE_NAME; // for use in queries
        protected string m_CLASS_NAME; // pascal case [_]tableName<DataObject | BusinessObject | Gateway>
        protected string m_SUBCLASS_NAME;
        protected string m_CLASS_SUMMARY;
        protected string m_CLASS_NAME_PREFIX; // pascal case table name with spaces as underline
        protected string m_CLASS_VARIABLE_NAME_PREFIX; // camel case table name with spaces as underline
        protected string m_ABSTRACT_BUSINESS_ENTITY_TYPE_NAME; // camel case table name with spaces as underline
        protected string m_CONCRETE_BUSINESS_ENTITY_TYPE_NAME; // camel case table name with spaces as underline
        protected string m_ABSTRACT_DATA_ENTITY_TYPE_NAME; // camel case table name with spaces as underline
        protected string m_CONCRETE_DATA_ENTITY_TYPE_NAME; // camel case table name with spaces as underline
        protected string m_ABSTRACT_GATEWAY_TYPE_NAME; // camel case table name with spaces as underline
        protected string m_CONCRETE_GATEWAY_TYPE_NAME; // camel case table name with spaces as underline

        #endregion

        #region internal structured members
        #endregion

        #region constructors / desturctors

        public GeneratorBase(string rootNamespace, IView iView)
        {

            m_FileBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(rootNamespace))
            {
                m_NAMESPACE = rootNamespace;
            }

            IView = iView;

        }

        ~GeneratorBase()
        {
        }

        #endregion

        #region public properties

        public int SpacesPerTab
        {
            get
            {
                return m_SpacesPerTab;
            }
            set
            {
                m_SpacesPerTab = value;
            }
        }

        public int IndentLevel
        {
            get
            {
                return m_IndentLevel;
            }
            set
            {
                m_IndentLevel = value;
            }
        }

        public string Indent
        {
            get
            {
                return "".PadLeft(m_SpacesPerTab * m_IndentLevel);
            }
        }

        public bool IsAbstract
        {
            get
            {
                return m_IsAbstract;
            }
            set
            {
                m_IsAbstract = value;
            }
        }

        public bool IsStatic
        {
            get
            {
                return m_IsAbstract;
            }
            set
            {
                m_IsAbstract = value;
            }
        }

        public virtual IView IView
        {
            get
            {
                return m_IView;
            }
            set
            {
                m_IView = value;

                // clear everything
                m_TABLE_NAME = "";
                m_CLASS_NAME = "";
                m_SUBCLASS_NAME = "";
                m_CLASS_NAME_PREFIX = "";
                m_CLASS_VARIABLE_NAME_PREFIX = "";

                m_COLUMN_COUNT = "";

                m_SELECT_COLUMN_LIST_ALL = "";

                m_ABSTRACT_DATA_ENTITY_TYPE_NAME = "";
                m_CONCRETE_DATA_ENTITY_TYPE_NAME = "";
                m_ABSTRACT_GATEWAY_TYPE_NAME = "";
                m_CONCRETE_GATEWAY_TYPE_NAME = "";

            }
        }

        public virtual string FILE_NAME
        {
            get
            {
                return m_FILE_NAME;
            }
        }

        public virtual string Namespace
        {
            get
            {
                return m_NAMESPACE;
            }
            set
            {
                m_NAMESPACE = value;
            }
        }

        /// <summary>
        /// for use in queries
        /// </summary>
        public string TABLE_NAME
        {
            get
            {
                if (m_TABLE_NAME == "")
                {
                    m_TABLE_NAME = IView.Name;
                }

                return m_TABLE_NAME;
            }
        }

        /// <summary>
        /// number of columns in the table
        /// </summary>
        public string COLUMN_COUNT
        {
            get
            {
                if (m_COLUMN_COUNT == "")
                {
                    m_COLUMN_COUNT = IView.Columns.Length.ToString();
                }

                return m_COLUMN_COUNT;
            }
        }

        public string SELECT_COLUMN_LIST_ALL
        {
            get
            {
                if (m_SELECT_COLUMN_LIST_ALL == "")
                {
                    m_SELECT_COLUMN_LIST_ALL = Get_SELECT_COLUMN_LIST_ALL();
                }

                return m_SELECT_COLUMN_LIST_ALL;
            }
        }

        /// <summary>
        /// pascal case [_]tableName<DataObject | BusinessObject | Gateway>
        /// </summary>
        public virtual string CLASS_NAME
        {
            get
            {
                if (m_CLASS_NAME == "")
                {
                    m_CLASS_NAME = CLASS_NAME_PREFIX;
                }

                return m_CLASS_NAME;
            }
        }

        /// <summary>
        /// pascal case subclass name
        /// </summary>
        public virtual string SUBCLASS_NAME
        {
            get
            {
                return m_SUBCLASS_NAME;
            }
            set
            {
                m_SUBCLASS_NAME = value;
            }
        }

        /// <summary>
        /// pascal case subclass name
        /// </summary>
        public virtual string CLASS_SUMMARY
        {
            get
            {
                return m_CLASS_SUMMARY;
            }
            set
            {
                m_CLASS_SUMMARY = value;
            }
        }

        /// <summary>
        /// pascal case table name with spaces as underline
        /// </summary>
        public virtual string CLASS_NAME_PREFIX
        {
            get
            {
                if (m_CLASS_NAME_PREFIX == "")
                {
                    m_CLASS_NAME_PREFIX = Utility.FormatPascal(IView.Name.Replace(" ", "_"));
                }

                return m_CLASS_NAME_PREFIX;
            }
        }

        /// <summary>
        /// camel case table name with spaces as underline
        /// </summary>
        public string CLASS_VARIABLE_NAME_PREFIX
        {
            get
            {
                if (m_CLASS_VARIABLE_NAME_PREFIX == "")
                {
                    m_CLASS_VARIABLE_NAME_PREFIX = CLASS_NAME;
                }

                return m_CLASS_VARIABLE_NAME_PREFIX;
            }
        }

        /// <summary>
        /// pascal case table name with spaces as underline
        /// word 'DataObject' as a suffix
        /// </summary>
        public string CONCRETE_DATA_ENTITY_TYPE_NAME
        {
            get
            {
                if (m_CONCRETE_DATA_ENTITY_TYPE_NAME == "")
                {
                    m_CONCRETE_DATA_ENTITY_TYPE_NAME =
                        CLASS_NAME_PREFIX + "DataObject";
                }

                return m_CONCRETE_DATA_ENTITY_TYPE_NAME;
            }
        }

        /// <summary>
        /// pascal case table name with spaces as underline
        /// word 'Gateway' as a suffix
        /// </summary>
        public string CONCRETE_GATEWAY_TYPE_NAME
        {
            get
            {
                if (m_CONCRETE_GATEWAY_TYPE_NAME == "")
                {
                    m_CONCRETE_GATEWAY_TYPE_NAME =
                        CLASS_NAME_PREFIX + "Gateway";

                }

                return m_CONCRETE_GATEWAY_TYPE_NAME;
            }
        }

        #endregion

        #region public methods

        public void IndentIncrement()
        {
            m_IndentLevel++;
        }

        public void IndentIncrement(int level)
        {
            m_IndentLevel += level;
        }

        public void IndentDecrement()
        {
            if (m_IndentLevel > 0)
            {
                m_IndentLevel--;
            }
        }

        public void IndentDecrement(int level)
        {
            m_IndentLevel -= level;
            if (m_IndentLevel < 0)
            {
                m_IndentLevel = 0;
            }
        }

        protected string Get_NewFieldDefinition(IView iView, int index)
        {

            StringBuilder newFieldDefinition = new StringBuilder();
            Column column = iView.Columns[index];
            string isReadOnly = "false";

            newFieldDefinition.Append("new FieldDefinition(\"");
            newFieldDefinition.Append(column.PropertyName);
            newFieldDefinition.Append("\", \"");
            newFieldDefinition.Append(column.EnumeratedTypeName);
            newFieldDefinition.Append("\", typeof(");
            newFieldDefinition.Append(column.LanguageType);
            newFieldDefinition.Append("), \"");
            newFieldDefinition.Append(m_IView.Schema);
            newFieldDefinition.Append("\", \"");
            newFieldDefinition.Append(m_IView.Name);
            newFieldDefinition.Append("\", \"");
            newFieldDefinition.Append(column.Name);
            newFieldDefinition.Append("\", \"");
            newFieldDefinition.Append(column.DbType);
            newFieldDefinition.Append("\", ");
            if (column.Length.Length == 0)
            {
                newFieldDefinition.Append("0");
            }
            else
            {
                newFieldDefinition.Append(column.Length);
            }
            newFieldDefinition.Append(", ");
            if (column.Scale.Length == 0)
            {
                newFieldDefinition.Append("0");
            }
            else
            {
                newFieldDefinition.Append(column.Scale);
            }
            newFieldDefinition.Append(", ");
            if (column.Precision.Length == 0)
            {
                newFieldDefinition.Append("0");
            }
            else
            {
                newFieldDefinition.Append(column.Precision);
            }
            newFieldDefinition.Append(", ");
            if (iView.GetType() == typeof(Table))
            {
                if (((Table)iView).PrimaryKey != null && ((IList)((Table)iView).PrimaryKey.PkColumns).Contains(column))
                {
                    newFieldDefinition.Append("true");
                }
                else
                {
                    newFieldDefinition.Append("false");
                }
            }
            else
            {
                newFieldDefinition.Append("false");
            }
            newFieldDefinition.Append(", ");
            if (column.IsIdentity)
            {
                newFieldDefinition.Append("true");
                isReadOnly = "true";
            }
            else
            {
                newFieldDefinition.Append("false");
            }
            newFieldDefinition.Append(", ");
            if (column.IsNullable)
            {
                newFieldDefinition.Append("true");
            }
            else
            {
                newFieldDefinition.Append("false");
            }
            if (column.IsComputed)
            {
                isReadOnly = "true";
            }
            if (column.IsRowGuid && column.DefaultValue != null)
            {
                isReadOnly = "true";
            }

            newFieldDefinition.Append(", ");
            newFieldDefinition.Append(isReadOnly);
            newFieldDefinition.Append(", ");
            if (column.DefaultValue != null)
            {
                newFieldDefinition.Append("true, ");
            }
            else
            {
                newFieldDefinition.Append("false, ");
            }
            newFieldDefinition.Append(index.ToString());
            newFieldDefinition.Append(")");

            return newFieldDefinition.ToString();

        }

        public override string ToString()
        {

            m_IndentLevel = 0;

            #region add file comments

            AppendLine("// ===============================================================================");
            AppendLine("//");
            AppendLine("// #FILE_NAME#");
            AppendLine("//");
            AppendLine("// This file contains the implementations of the class #CLASS_NAME#");
            AppendLine("//");

            OnGetFileComments();

            AppendLine("//");
            AppendLine("// ===============================================================================");
            AppendLine("// Release history");
            AppendLine("// ACTION   DATE        AUTHOR              NOTES");
            AppendLine("// Created  " + DateTime.Now.ToShortDateString());
            AppendLine("// ===============================================================================");

            #endregion

            #region add using statements

            AppendLine();
            AppendLine("using System;");
            OnGetUsingStatements();

            #endregion

            #region add the namespace and class body

            AppendLine();
            AppendLine("namespace #NAMESPACE# {");

            IndentIncrement();
            OnPreGetClass();
            OnGetClass();
            OnPostGetClass();
            IndentDecrement();

            AppendLine();
            AppendLine("}");

            #endregion

            m_FileBuilder.Replace("#FILE_NAME#", FILE_NAME);
            m_FileBuilder.Replace("#NAMESPACE#", Namespace);
            m_FileBuilder.Replace("#TABLE_NAME#", TABLE_NAME);
            m_FileBuilder.Replace("#CLASS_NAME#", CLASS_NAME);
            m_FileBuilder.Replace("#CLASS_NAME_PREFIX#", CLASS_NAME_PREFIX);
            m_FileBuilder.Replace("#CLASS_VARIABLE_NAME_PREFIX#", CLASS_VARIABLE_NAME_PREFIX);
            m_FileBuilder.Replace("#CONCRETE_DATA_ENTITY_TYPE_NAME#", CONCRETE_DATA_ENTITY_TYPE_NAME);
            m_FileBuilder.Replace("#CONCRETE_GATEWAY_TYPE_NAME#", CONCRETE_GATEWAY_TYPE_NAME);
            m_FileBuilder.Replace("#COLUMN_COUNT#", COLUMN_COUNT);

            return m_FileBuilder.ToString();
        }

        #endregion

        #region protected methods

        protected void AppendLine()
        {

            m_FileBuilder.AppendLine();

        }

        protected void AppendLine(string value)
        {

            value = Regex.Replace(value, "(?<=^\t*)\t", "".PadLeft(m_SpacesPerTab));
            m_FileBuilder.Append(Indent);
            m_FileBuilder.AppendLine(value);

        }

        protected void AppendStartLine(string value)
        {

            m_FileBuilder.Append(Indent);
            m_FileBuilder.Append(value);

        }

        protected void AppendEndLine()
        {

            m_FileBuilder.AppendLine();

        }

        protected void AppendEndLine(string value)
        {

            m_FileBuilder.Append(value);
            AppendEndLine();

        }

        protected void Append(string value)
        {

            m_FileBuilder.Append(value);

        }

        protected string Get_SELECT_COLUMN_LIST_ALL()
        {

            StringBuilder columnList = new StringBuilder();
            Column[] columns = m_IView.Columns;
            int columnCount = m_IView.Columns.Length;

            for (int index = 0; index < columnCount; index++)
            {
                columnList.Append("[");
                columnList.Append((columns[index]).Name);
                columnList.Append("]");
                if (index < columnCount - 1)
                {
                    columnList.Append(", ");
                }
            }

            return columnList.ToString();
        }

        #endregion

        #region virtual methods

        protected virtual void OnGetFileComments() { }
        protected virtual void OnGetUsingStatements() { }
        #region OnGetClass related methods

        protected virtual void OnPreGetClass() { }
        protected virtual void OnGetClass()
        {

            #region create the class definition

            AppendLine();
            AppendLine("/// <summary>");
            AppendStartLine("/// ");
            AppendEndLine(m_CLASS_SUMMARY);
            AppendLine("/// </summary>");
            AppendStartLine("public ");
            if (IsAbstract)
            {
                Append("abstract ");
            }
            else if (IsStatic)
            {
                Append("static ");
            }
            Append("partial class #CLASS_NAME#");
            if (m_SUBCLASS_NAME != null && m_SUBCLASS_NAME != "")
            {
                Append(" : " + m_SUBCLASS_NAME);
            }

            #endregion

            AppendEndLine(" {");

            #region get the class body

            IndentIncrement();

            OnGetRegion_PrivateProtectedMemberVariables();
            OnGetRegion_InternalStructuredMembers();
            OnGetRegion_ConstructorsDestructors();
            OnGetRegion_PublicProperties();
            OnGetRegion_EventHandlersAndOverrides();
            OnGetRegion_InterfaceImplementationMethods();
            OnGetRegion_PublicMethods();
            OnGetRegion_ProtectedMethods();
            OnGetRegion_PrivateMethods();

            IndentDecrement();

            #endregion

            AppendLine();
            AppendLine("}");

        }
        protected virtual void OnPostGetClass() { }

        #endregion
        protected virtual void OnGetRegion_PrivateProtectedMemberVariables() { }
        protected virtual void OnGetRegion_InternalStructuredMembers() { }
        protected virtual void OnGetRegion_ConstructorsDestructors() { }
        protected virtual void OnGetRegion_PublicProperties() { }
        protected virtual void OnGetRegion_EventHandlersAndOverrides() { }
        protected virtual void OnGetRegion_InterfaceImplementationMethods() { }
        protected virtual void OnGetRegion_PublicMethods() { }
        protected virtual void OnGetRegion_ProtectedMethods() { }
        protected virtual void OnGetRegion_PrivateMethods() { }

        #endregion

        #region private implementation

        protected void AddRegionHeader(string comment)
        {
            AppendLine("#region " + comment);
            AppendLine();
        }
        protected void AddRegionFooter()
        {
            AppendLine();
            AppendLine("#endregion");
            AppendLine();
        }

        #endregion

    }

}
