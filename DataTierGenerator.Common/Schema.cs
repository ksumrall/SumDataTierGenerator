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
    public class Schema : ProjectSchema.Schema
    {

        #region private and protected member variables

        //private string m_DatabaseName;
        //private string m_Schema;
        //private string m_Name;
        //private string m_Description;

        //private TableList m_TableList;
        //private ViewList m_ViewList;
        //private FunctionList m_FunctionList;
        //private ProcedureList m_ProcedureList;

        private string m_ClassName;

        private bool m_BuildClass;

        #endregion

        #region constructors / desturctors

        /// <summary>
        /// Default constructor.  All collections are initialized.
        /// </summary>
        public Schema()
        {
            Tables = new ProjectSchema.Table[0];
            Views = new ProjectSchema.View[0];

            //m_TableList = new TableList();
            //m_ViewList = new ViewList();
            //m_FunctionList = new FunctionList();
            //m_ProcedureList = new ProcedureList();

            //m_Name = "";
            //m_Description = "";

            m_ClassName = "";
        }

        public Schema(XmlNode tableNode)
        {

            XmlNode node;
            Name = tableNode.Attributes["name"].Value;

            //node = tableNode.SelectSingleNode("./DatabaseName");
            //if (node != null)
            //    m_DatabaseName = node.Value;

            //node = tableNode.SelectSingleNode("./Schema");
            //if (node != null)
            //    m_Schema = node.Value;

            //node = tableNode.SelectSingleNode("./Name");
            //if (node != null)
            //    m_Name = node.Value;

            //node = tableNode.SelectSingleNode("./Description");
            //if (node != null)
            //    m_Description = node.Value;

        }

        #endregion

        #region public database related properties

        /// <summary>
        /// DatabaseName of the table.
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
        /// Schema of the table.
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

        /// <summary>
        /// Contains the list of Table instances.
        /// </summary>
        //public TableList Tables
        //{
        //    get
        //    {
        //        return m_TableList;
        //    }
        //}

        /// <summary>
        /// Contains the list of View instances.
        /// </summary>
        //public ViewList Views
        //{
        //    get
        //    {
        //        return m_ViewList;
        //    }
        //}

        /// <summary>
        /// Contains the list of Function instances.
        /// </summary>
        //public FunctionList Functions
        //{
        //    get
        //    {
        //        return m_FunctionList;
        //    }
        //}

        /// <summary>
        /// Contains the list of Procedure instances.
        /// </summary>
        //public ProcedureList Procedures
        //{
        //    get
        //    {
        //        return m_ProcedureList;
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