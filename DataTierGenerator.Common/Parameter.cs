using System.Collections.Generic;

namespace TotalSafety.DataTierGenerator.Common
{

    public class ParameterList : List<Parameter>
    {

        public Parameter this[string name]
        {
            get
            {
                return FindParameter(name);
            }
        }

        public Parameter FindParameter(string name)
        {

            foreach (Parameter parameter in this)
            {
                if (parameter.Name == name)
                {
                    return parameter;
                }
            }

            return null;

        }

    }

    /// <summary>
    /// Class that stores information for columns in a database table.
    /// </summary>
    public class Parameter
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

        public static int CompareByProgrammaticAlias(Parameter obj1, Parameter obj2)
        {
            return obj1.PropertyName.CompareTo(obj2.Name);
        }

        #endregion

    }
}