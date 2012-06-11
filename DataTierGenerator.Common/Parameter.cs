using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

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
    [XmlRoot(Namespace = "urn:dtg-project")]
    public class Parameter : ProjectSchema.Parameter
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

        private string m_DefaultValue;

        private string m_PropertyName;

        #endregion

        #region constructors

        public Parameter(){}

        public Parameter(XmlNode parameterNode)
        {
            /*
                <parameter name="@Beginning_Date" id="1" data_type="datetime" max_length="8" precision="23" scale="3" is_output="False" />
             * * */
            m_Name = parameterNode.Attributes["name"].Value;
            m_Description = parameterNode.Attributes["description"].Value;
            m_Type = parameterNode.Attributes["data_type"].Value;
            //m_ClrType = columnNode.Attributes[""].Value;
            //m_LanguageType = columnNode.Attributes[""].Value;
            //m_EnumeratedTypeName = columnNode.Attributes[""].Value;
            m_Length = parameterNode.Attributes["max_length"].Value;
            m_Precision = parameterNode.Attributes["precision"].Value;
            m_Scale = parameterNode.Attributes["scale"].Value;

            if (parameterNode.Attributes["default_definition"] != null)
            {
                m_DefaultValue = parameterNode.Attributes["default_definition"].Value;
            }

            m_PropertyName = m_Name;
            //m_PropertyName = System.Text.RegularExpressions.Regex.Replace(m_Name, "\\W", "_");
        }

        #endregion

        #region public properties

        /// <summary>
        /// Name of the column.
        /// </summary>
        //public string Name
        //{
        //    get
        //    {
        //        return m_Name;
        //    }
        //    set
        //    {
        //        m_Name = value;
        //    }
        //}

        //public string Description
        //{
        //    get
        //    {
        //        return m_Description;
        //    }
        //    set
        //    {
        //        m_Description = value;
        //    }
        //}

        /// <summary>
        /// Data type of the column.
        /// </summary>
        //public string Type
        //{
        //    get
        //    {
        //        return m_Type;
        //    }
        //    set
        //    {
        //        m_Type = value;
        //    }
        //}

        /// <summary>
        /// System type of the column.
        /// </summary>
        //public string ClrType
        //{
        //    get
        //    {
        //        return m_ClrType;
        //    }
        //    set
        //    {
        //        m_ClrType = value;
        //    }
        //}

        /// <summary>
        /// Language (c#, VB.NET, etc.) type of the column.
        /// </summary>
        //public string LanguageType
        //{
        //    get
        //    {
        //        return m_LanguageType;
        //    }
        //    set
        //    {
        //        m_LanguageType = value;
        //    }
        //}

        /// <summary>
        /// This is the enumerated string value for SqlDbType, OleDbType, OracleType...
        /// </summary>
        //public string EnumeratedTypeName
        //{
        //    get
        //    {
        //        return m_EnumeratedTypeName;
        //    }
        //    set
        //    {
        //        m_EnumeratedTypeName = value;
        //    }
        //}

        /// <summary>
        /// Length in bytes of the column.
        /// </summary>
        //public string Length
        //{
        //    get
        //    {
        //        return m_Length;
        //    }
        //    set
        //    {
        //        m_Length = value;
        //    }
        //}

        /// <summary>
        /// Precision of the column. Applicable to decimal, float, and numeric data types only.
        /// </summary>
        //public string Precision
        //{
        //    get
        //    {
        //        return m_Precision;
        //    }
        //    set
        //    {
        //        m_Precision = value;
        //    }
        //}

        /// <summary>
        /// Scale of the column. Applicable to decimal, and numeric data types only.
        /// </summary>
        //public string Scale
        //{
        //    get
        //    {
        //        return m_Scale;
        //    }
        //    set
        //    {
        //        m_Scale = value;
        //    }
        //}

        //public string DefaultValue
        //{
        //    get
        //    {
        //        return m_DefaultValue;
        //    }
        //    set
        //    {
        //        m_DefaultValue = value;
        //    }
        //}

        //public string PropertyName
        //{
        //    get
        //    {
        //        if (m_PropertyName == null || m_PropertyName == "")
        //        {
        //            m_PropertyName = System.Text.RegularExpressions.Regex.Replace(m_Name, "\\W", "_");

        //        }

        //        return m_PropertyName;
        //    }
        //    set
        //    {
        //        m_PropertyName = value;
        //    }
        //}

        #endregion

        #region comparison implementation

        public static int CompareByProgrammaticAlias(Parameter obj1, Parameter obj2)
        {
            return obj1.PropertyName.CompareTo(obj2.Name);
        }

        #endregion

    }
}