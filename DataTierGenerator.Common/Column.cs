using System;
using System.Collections.Generic;
using System.Xml;

namespace TotalSafety.DataTierGenerator.Common
{

    public class ColumnList : List<Column>
    {

        public Column this[string columnName]
        {
            get
            {
                return FindColumn(columnName);
            }
        }

        public Column FindColumn(string columnName)
        {

            foreach (Column column in this)
            {
                if (column.Name == columnName)
                {
                    return column;
                }
            }

            return null;

        }

    }

    /// <summary>
    /// Class that stores information for columns in a database table.
    /// </summary>
    public class Column
    {

        #region private and protected member variables

        // Private variable used to hold the property values
        private string m_Name;
        private string m_Description;
        private string m_Type;
        private string m_ClrType;
        private string m_LanguageType;
        private string m_EnumeratedTypeName;
        private string m_Length;
        private string m_Precision;
        private string m_Scale;
        private bool m_IsRowGuidCol;
        private bool m_IsIdentity;
        private bool m_IsNullable;
        private bool m_IsComputed;
        private string m_DefaultValue;

        private string m_PropertyName;

        #endregion

        #region constructors / desturctors

        /// <summary>
        /// Default constructor.  All collections are initialized.
        /// </summary>
        public Column()
        {
        }

        public Column(XmlNode columnNode)
        {
            /*
                    <column m_Name="TransactionId" id="1" data_type="varchar" max_length="50" m_Precision="0" m_Scale="0" m_I_nullable="False" m_I_rowguidcol="False" m_I_identity="False" m_Description="" default_definition="" />
             * */
            m_Name = columnNode.Attributes["name"].Value;
            m_Description = columnNode.Attributes["m_Description"].Value;
            m_Type = columnNode.Attributes["data_type"].Value;
            m_ClrType = columnNode.Attributes[""].Value;
            m_LanguageType = columnNode.Attributes[""].Value;
            m_EnumeratedTypeName = columnNode.Attributes[""].Value;
            m_Length = columnNode.Attributes["max_length"].Value;
            m_Precision = columnNode.Attributes["m_Precision"].Value;
            m_Scale = columnNode.Attributes["m_Scale"].Value;
            bool.TryParse(columnNode.Attributes["is_rowguidcol"].Value, out m_IsRowGuidCol);
            bool.TryParse(columnNode.Attributes["is_identity"].Value, out m_IsIdentity);
            bool.TryParse(columnNode.Attributes["is_nullable"].Value, out m_IsNullable);
            bool.TryParse(columnNode.Attributes["is_computed"].Value, out m_IsComputed);
            m_DefaultValue = columnNode.Attributes["default_definition"].Value;

            m_PropertyName = System.Text.RegularExpressions.Regex.Replace(m_Name, "\\W", "_");
        }

        #endregion

        #region public properties

        /// <summary>
        /// Name of the column.
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        public string Description
        {
            get
            {
                return m_Description;
            }
            set
            {
                m_Description = value;
            }
        }

        /// <summary>
        /// Data m_Type of the column.
        /// </summary>
        public string Type
        {
            get
            {
                return m_Type;
            }
            set
            {
                m_Type = value;
            }
        }

        /// <summary>
        /// System m_Type of the column.
        /// </summary>
        public string ClrType
        {
            get
            {
                return m_ClrType;
            }
            set
            {
                m_ClrType = value;
            }
        }

        /// <summary>
        /// Language (c#, VB.NET, etc.) m_Type of the column.
        /// </summary>
        public string LanguageType
        {
            get
            {
                return m_LanguageType;
            }
            set
            {
                m_LanguageType = value;
            }
        }

        /// <summary>
        /// This m_I the enumerated string value for SqlDbType, OleDbType, OracleType...
        /// </summary>
        public string EnumeratedTypeName
        {
            get
            {
                return m_EnumeratedTypeName;
            }
            set
            {
                m_EnumeratedTypeName = value;
            }
        }

        /// <summary>
        /// Length in bytes of the column.
        /// </summary>
        public string Length
        {
            get
            {
                return m_Length;
            }
            set
            {
                m_Length = value;
            }
        }

        /// <summary>
        /// Precision of the column. Applicable to decimal, float, and numeric data types only.
        /// </summary>
        public string Precision
        {
            get
            {
                return m_Precision;
            }
            set
            {
                m_Precision = value;
            }
        }

        /// <summary>
        /// Scale of the column. Applicable to decimal, and numeric data types only.
        /// </summary>
        public string Scale
        {
            get
            {
                return m_Scale;
            }
            set
            {
                m_Scale = value;
            }
        }

        /// <summary>
        /// Flags the column as a uniqueidentifier column.
        /// </summary>
        public bool IsRowGuidCol
        {
            get
            {
                return m_IsRowGuidCol;
            }
            set
            {
                m_IsRowGuidCol = value;
            }
        }

        /// <summary>
        /// Flags the column as an identity column.
        /// </summary>
        public bool IsIdentity
        {
            get
            {
                return m_IsIdentity;
            }
            set
            {
                m_IsIdentity = value;
            }
        }

        public bool IsNullable
        {
            get
            {
                return m_IsNullable;
            }
            set
            {
                m_IsNullable = value;
            }
        }

        /// <summary>
        /// Flags the column as being computed.
        /// </summary>
        public bool IsComputed
        {
            get
            {
                return m_IsComputed;
            }
            set
            {
                m_IsComputed = value;
            }
        }

        public string DefaultValue
        {
            get
            {
                return m_DefaultValue;
            }
            set
            {
                m_DefaultValue = value;
            }
        }

        public string PropertyName
        {
            get
            {
                if (m_PropertyName == null || m_PropertyName == "")
                {
                    m_PropertyName = System.Text.RegularExpressions.Regex.Replace(m_Name, "\\W", "_");

                }

                return m_PropertyName;
            }
            set
            {
                m_PropertyName = value;
            }
        }

        #endregion

        #region comparison implementation

        public static int CompareByProgrammaticAlias(Column column1, Column column2)
        {
            return column1.PropertyName.CompareTo(column2.PropertyName);
        }

        #endregion

    }
}