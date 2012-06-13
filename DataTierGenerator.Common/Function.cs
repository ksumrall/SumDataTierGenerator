using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace TotalSafety.DataTierGenerator.Common
{
    public class FunctionList : List<Function>
    {

        public Function this[string schemaName, string name]
        {
            get
            {
                return FindView(schemaName, name);
            }
        }

        public Function FindView(string schemaName, string name)
        {

            foreach (Function obj in this)
            {
                if (obj.Schema == schemaName && obj.Name == name)
                {
                    return obj;
                }
            }

            return null;

        }

    }

    public partial class Function
    {

        #region private and protected member variables

        private ParameterList m_ParameterList;

        #endregion

        #region constructors / desturctors

        /// <summary>
        /// Default constructor.  All collections are initialized.
        /// </summary>
        public Function()
        {
            m_ParameterList = new ParameterList();

            Schema = "";
            Name = "";
            Description = "";
        }

        public Function(XmlNode functionNode)
        {

            Name = functionNode.Attributes["name"].Value;

            XmlNodeList list = functionNode.SelectNodes(".//parameters//parameter");

            List<Parameter> paramList = new List<Parameter>();
            foreach (XmlNode node in list)
            {
                paramList.Add(new Parameter(node));
            }
            Parameters = paramList.ToArray();
        }

        #endregion

        #region public methods

        public XmlNode GetNode(XmlDocument xmlDoc)
        {

            XmlNode node = xmlDoc.CreateElement(Name);

            return node;
        }

        #endregion

        #region comparison implementation

        public static int CompareByProgrammaticAlias(Function obj1, Function obj2)
        {
            return obj1.Name.CompareTo(obj2.Name);
        }

        #endregion

    }
}
