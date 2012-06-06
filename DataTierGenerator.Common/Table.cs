using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace TotalSafety.DataTierGenerator.Common
{

    public class TableList : List<Table>
    {

        public Table this[string schemaName, string tableName]
        {
            get
            {
                return FindTable(schemaName, tableName);
            }
        }

        public Table FindTable(string schemaName, string tableName)
        {

            foreach (Table table in this)
            {
                if (table.Schema == schemaName && table.Name == tableName)
                {
                    return table;
                }
            }

            return null;

        }

    }

    /// <summary>
    /// Class that stores information for tables in a database.
    /// </summary>
    public class Table
    {

        #region private and protected member variables

        private string m_DatabaseName;
        private string m_Schema;
        private string m_Name;
        private string m_Description;
        private ColumnList m_ColumnList;
        private Index m_PrimaryKey;
        private List<Index> m_ForeignKeyList;

        private string m_ClassName;

        private bool m_BuildClass;

        #endregion

        #region constructors / desturctors

        /// <summary>
        /// Default constructor.  All collections are initialized.
        /// </summary>
        public Table()
        {
            m_ColumnList = new ColumnList();
            m_PrimaryKey = null;
            m_ForeignKeyList = new List<Index>();

            m_Schema = "";
            m_Name = "";
            m_Description = "";

            m_ClassName = "";
        }

        public Table(XmlNode tableNode)
        {

            XmlNode node;

            node = tableNode.SelectSingleNode("./DatabaseName");
            if (node != null)
                m_DatabaseName = node.Value;

            node = tableNode.SelectSingleNode("./Schema");
            if (node != null)
                m_Schema = node.Value;

            node = tableNode.SelectSingleNode("./Name");
            if (node != null)
                m_Name = node.Value;

            node = tableNode.SelectSingleNode("./Description");
            if (node != null)
                m_Description = node.Value;

        }

        #endregion

        #region public database related properties

        /// <summary>
        /// Contains the list of Column instances that define the table.
        /// </summary>
        public ColumnList Columns
        {
            get
            {
                return m_ColumnList;
            }
        }

        /// <summary>
        /// DatabaseName of the table.
        /// </summary>
        public string DatabaseName
        {
            get
            {
                return m_DatabaseName;
            }
            set
            {
                m_DatabaseName = value;
            }
        }

        /// <summary>
        /// Schema of the table.
        /// </summary>
        public string Schema
        {
            get
            {
                return m_Schema;
            }
            set
            {
                m_Schema = value;
            }
        }

        /// <summary>
        /// Name of the table.
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

                if (string.IsNullOrEmpty(m_ClassName))
                {
                    m_ClassName = m_Name;
                }
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
        /// Contains the list of primary key Column instances that define the table.
        /// </summary>
        public Index PrimaryKey
        {
            get
            {
                return m_PrimaryKey;
            }
            set
            {
                m_PrimaryKey = value;
            }
        }

        /// <summary>
        /// Contains the list of Column instances that define the table.  The Hashtable returned 
        /// is keyed on the foreign key name, and the value associated with the key is an 
        /// ArrayList of Column instances that compose the foreign key.
        /// </summary>
        [XmlIgnore]
        public List<Index> ForeignKeys
        {
            get
            {
                return m_ForeignKeyList;
            }
        }

        #endregion

        #region public project related properties

        public bool BuildClass
        {
            get { return m_BuildClass; }
            set { m_BuildClass = value; }
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

        public static int CompareByProgrammaticAlias(Table table1, Table table2)
        {
            return table1.Name.CompareTo(table2.Name);
        }

        #endregion

    }
}