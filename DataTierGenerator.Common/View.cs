using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace TotalSafety.DataTierGenerator.Common
{
    public class ViewList : List<View>
    {

        public View this[string schemaName, string name]
        {
            get
            {
                return FindView(schemaName, name);
            }
        }

        public View FindView(string schemaName, string name)
        {

            foreach (View view in this)
            {
                if (view.Schema == schemaName && view.Name == name)
                {
                    return view;
                }
            }

            return null;

        }

    }

    public partial class View : IView
    {

        #region private and protected member variables

        private string m_ClassName;

        #endregion

        #region constructors / desturctors

        /// <summary>
        /// Default constructor.  All collections are initialized.
        /// </summary>
        public View()
        {
            Schema = "";
            Name = "";
            Description = "";

            m_ClassName = "";
        }

        public View(XmlNode viewNode):this()
        {

            Name = viewNode.Attributes["name"].Value;

            XmlNodeList list = viewNode.SelectNodes(".//columns//column");

            List<Column> columnList = new List<Column>();
            foreach (XmlNode node in list)
            {
                columnList.Add(new Column(node));
            }
            Columns = columnList.ToArray();

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

        public static int CompareByProgrammaticAlias(View obj1, View obj2)
        {
            return obj1.Name.CompareTo(obj2.Name);
        }

        #endregion

    }
}
