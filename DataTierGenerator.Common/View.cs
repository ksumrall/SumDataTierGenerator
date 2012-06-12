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

    public partial class View
    {

        #region private and protected member variables

        private string m_DatabaseName;
        private string m_Schema;
        private string m_Name;
        private string m_Description;
        private ColumnList m_ColumnList;

        private string m_ClassName;

        private bool m_BuildClass;

        #endregion

        #region constructors / desturctors

        /// <summary>
        /// Default constructor.  All collections are initialized.
        /// </summary>
        public View()
        {
            m_ColumnList = new ColumnList();

            m_Schema = "";
            m_Name = "";
            m_Description = "";

            m_ClassName = "";
        }

        public View(XmlNode viewNode):this()
        {

            XmlNode tempNode;

            Name = viewNode.Attributes["name"].Value;

            //tempNode = node.SelectSingleNode("./DatabaseName");
            //if (tempNode != null)
            //    m_DatabaseName = node.Value;

            //tempNode = node.SelectSingleNode("./Schema");
            //if (tempNode != null)
            //    m_Schema = node.Value;

            //tempNode = node.SelectSingleNode("./Name");
            //if (tempNode != null)
            //    m_Name = node.Value;

            //tempNode = node.SelectSingleNode("./Description");
            //if (tempNode != null)
            //    m_Description = node.Value;

            XmlNodeList list = viewNode.SelectNodes(".//columns//column");

            List<Column> columnList = new List<Column>();
            foreach (XmlNode node in list)
            {
                columnList.Add(new Column(node));
            }
            Columns = columnList.ToArray();

        }

        #endregion

        #region public database related properties

        /// <summary>
        /// Contains the list of Column instances that define the view.
        /// </summary>
        //public ColumnList Columns
        //{
        //    get
        //    {
        //        return m_ColumnList;
        //    }
        //}

        /// <summary>
        /// DatabaseName of the view.
        /// </summary>
        //public string DatabaseName
        //{
        //    get
        //    {
        //        return m_DatabaseName;
        //    }
        //    set
        //    {
        //        m_DatabaseName = value;
        //    }
        //}

        /// <summary>
        /// Schema of the view.
        /// </summary>
        //public string Schema
        //{
        //    get
        //    {
        //        return m_Schema;
        //    }
        //    set
        //    {
        //        m_Schema = value;
        //    }
        //}

        /// <summary>
        /// Name of the view.
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

        //        if (string.IsNullOrEmpty(m_ClassName))
        //        {
        //            m_ClassName = m_Name;
        //        }
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

        #endregion

        #region public project related properties

        public bool BuildClass
        {
            get { return m_BuildClass; }
            set { m_BuildClass = value; }
        }

        #endregion

        #region event handlers / overrides

        #endregion

        #region public methods

        public XmlNode GetNode(XmlDocument xmlDoc)
        {

            XmlNode node = xmlDoc.CreateElement(Name);

            return node;
        }

        #endregion

        #region private implementation

        #endregion

        #region comparison implementation

        public static int CompareByProgrammaticAlias(View obj1, View obj2)
        {
            return obj1.Name.CompareTo(obj2.Name);
        }

        #endregion

    }
}
