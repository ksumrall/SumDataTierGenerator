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
    public partial class Parameter
    {

        #region private and protected member variables

        // Private variable used to hold the property values
        #endregion

        #region constructors

        public Parameter(){}

        public Parameter(XmlNode parameterNode)
        {
            /*
                <parameter name="@Beginning_Date" id="1" data_type="datetime" max_length="8" precision="23" scale="3" is_output="False" />
             * * */
            Name = parameterNode.Attributes["name"].Value;
            //Description = parameterNode.Attributes["description"].Value;
            DbType = parameterNode.Attributes["data_type"].Value;
            ClrType = parameterNode.Attributes["ClrType"].Value;
            LanguageType = parameterNode.Attributes["LanguageType"].Value;
            EnumeratedTypeName = parameterNode.Attributes["EnumeratedTypeName"].Value;
            Length = parameterNode.Attributes["max_length"].Value;
            Precision = parameterNode.Attributes["precision"].Value;
            Scale = parameterNode.Attributes["scale"].Value;

            if (parameterNode.Attributes["default_definition"] != null)
            {
                DefaultValue = parameterNode.Attributes["default_definition"].Value;
            }

            PropertyName = Name;
            //m_PropertyName = System.Text.RegularExpressions.Regex.Replace(m_Name, "\\W", "_");
        }

        #endregion

        #region public properties

        #endregion

        #region comparison implementation

        public static int CompareByProgrammaticAlias(Parameter obj1, Parameter obj2)
        {
            return obj1.PropertyName.CompareTo(obj2.Name);
        }

        #endregion

    }
}