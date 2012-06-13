using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace TotalSafety.DataTierGenerator.Common
{

    public class SchemaList : List<Schema>
    {

        public Schema this[string schemaName]
        {
            get
            {
                return FindSchema(schemaName);
            }
        }

        public Schema FindSchema(string schemaName)
        {

            foreach (Schema schema in this)
            {
                if (schema.Name == schemaName)
                {
                    return schema;
                }
            }

            return null;

        }

    }

    /// <summary>
    /// Class that stores information for tables in a database.
    /// </summary>
    public partial class Schema
    {

        #region private and protected member variables

        private string m_ClassName;

        private bool m_BuildClass;

        #endregion

        #region constructors / desturctors

        /// <summary>
        /// Default constructor.  All collections are initialized.
        /// </summary>
        public Schema()
        {
            Tables = new Table[0];
            Views = new View[0];

            m_ClassName = "";
        }

        public Schema(XmlNode tableNode)
        {

            XmlNode node;
            Name = tableNode.Attributes["name"].Value;

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

        public static int CompareByProgrammaticAlias(Schema schema1, Schema schema2)
        {
            return schema1.Name.CompareTo(schema2.Name);
        }

        #endregion

    }
}