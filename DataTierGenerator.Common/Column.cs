using System.Collections.Generic;

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
        private string name;
        private string description;
        private string type;
        private string clrType;
        private string languageType;
        private string enumeratedTypeName;
        private string length;
        private string precision;
        private string scale;
        private bool isRowGuidCol;
        private bool isIdentity;
        private bool isNullable;
        private bool isComputed;
        private string defaultValue;

        private string m_PropertyName;

        #endregion

        #region public properties

        /// <summary>
        /// Name of the column.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        /// <summary>
        /// Data type of the column.
        /// </summary>
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        /// <summary>
        /// System type of the column.
        /// </summary>
        public string ClrType
        {
            get
            {
                return clrType;
            }
            set
            {
                clrType = value;
            }
        }

        /// <summary>
        /// Language (c#, VB.NET, etc.) type of the column.
        /// </summary>
        public string LanguageType
        {
            get
            {
                return languageType;
            }
            set
            {
                languageType = value;
            }
        }

        /// <summary>
        /// This is the enumerated string value for SqlDbType, OleDbType, OracleType...
        /// </summary>
        public string EnumeratedTypeName
        {
            get
            {
                return enumeratedTypeName;
            }
            set
            {
                enumeratedTypeName = value;
            }
        }

        /// <summary>
        /// Length in bytes of the column.
        /// </summary>
        public string Length
        {
            get
            {
                return length;
            }
            set
            {
                length = value;
            }
        }

        /// <summary>
        /// Precision of the column. Applicable to decimal, float, and numeric data types only.
        /// </summary>
        public string Precision
        {
            get
            {
                return precision;
            }
            set
            {
                precision = value;
            }
        }

        /// <summary>
        /// Scale of the column. Applicable to decimal, and numeric data types only.
        /// </summary>
        public string Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }

        /// <summary>
        /// Flags the column as a uniqueidentifier column.
        /// </summary>
        public bool IsRowGuidCol
        {
            get
            {
                return isRowGuidCol;
            }
            set
            {
                isRowGuidCol = value;
            }
        }

        /// <summary>
        /// Flags the column as an identity column.
        /// </summary>
        public bool IsIdentity
        {
            get
            {
                return isIdentity;
            }
            set
            {
                isIdentity = value;
            }
        }

        public bool IsNullable
        {
            get
            {
                return isNullable;
            }
            set
            {
                isNullable = value;
            }
        }

        /// <summary>
        /// Flags the column as being computed.
        /// </summary>
        public bool IsComputed
        {
            get
            {
                return isComputed;
            }
            set
            {
                isComputed = value;
            }
        }

        public string DefaultValue
        {
            get
            {
                return defaultValue;
            }
            set
            {
                defaultValue = value;
            }
        }

        public string PropertyName
        {
            get
            {
                if (m_PropertyName == null || m_PropertyName == "")
                {
                    m_PropertyName = System.Text.RegularExpressions.Regex.Replace(name, "\\W", "_");

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