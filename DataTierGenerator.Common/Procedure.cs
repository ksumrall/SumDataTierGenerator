using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace SumDataTierGenerator.Common
{
    public class ProcedureList : List<Procedure>
    {

        public Procedure this[string schemaName, string name]
        {
            get
            {
                return FindView(schemaName, name);
            }
        }

        public Procedure FindView(string schemaName, string name)
        {

            foreach (Procedure obj in this)
            {
                if (obj.Schema == schemaName && obj.Name == name)
                {
                    return obj;
                }
            }

            return null;

        }

    }

    public partial class Procedure
    {

        #region private and protected member variables

        private string m_ClassName;

        #endregion

        #region constructors / desturctors

        /// <summary>
        /// Default constructor.  All collections are initialized.
        /// </summary>
        public Procedure()
        {
            Schema = "";
            Name = "";
            Description = "";

            m_ClassName = "";
        }

        public Procedure(XmlNode procedureNode)
            : this()
        {

            Name = procedureNode.Attributes["name"].Value;

            XmlNodeList list = procedureNode.SelectNodes(".//parameters//parameter");

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

        public static int CompareByProgrammaticAlias(Procedure obj1, Procedure obj2)
        {
            return obj1.Name.CompareTo(obj2.Name);
        }

        #endregion

    }
}
