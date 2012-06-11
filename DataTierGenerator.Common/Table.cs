using System;
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

        private string m_Name;
        private DateTime m_CreateDate;
        private DateTime m_ModifyDate;
        private string m_DatabaseName;
        private string m_Schema;
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
            /*
            <table name="Transaction" create_date="8/21/2010 7:19:37 PM" modify_date="11/7/2010 3:10:50 PM">
                <columns>
                    <column name="TransactionId" id="1" data_type="varchar" max_length="50" precision="0" scale="0" is_nullable="False" is_rowguidcol="False" is_identity="False" description="" default_definition="" />
                    <column name="OrderId" id="2" data_type="int" max_length="4" precision="10" scale="0" is_nullable="False" is_rowguidcol="False" is_identity="False" description="" default_definition="" />
                    <column name="AuthorizationCode" id="3" data_type="nvarchar" max_length="100" precision="0" scale="0" is_nullable="True" is_rowguidcol="False" is_identity="False" description="" default_definition="" />
                    <column name="TransactionDate" id="4" data_type="datetime" max_length="8" precision="23" scale="3" is_nullable="False" is_rowguidcol="False" is_identity="False" description="" default_definition="(getdate())" />
                    <column name="TransactionTypeID" id="5" data_type="int" max_length="4" precision="10" scale="0" is_nullable="False" is_rowguidcol="False" is_identity="False" description="" default_definition="((1))" />
                    <column name="Amount" id="6" data_type="money" max_length="8" precision="19" scale="4" is_nullable="False" is_rowguidcol="False" is_identity="False" description="" default_definition="" />
                    <column name="TransactionNotes" id="7" data_type="nvarchar" max_length="1000" precision="0" scale="0" is_nullable="True" is_rowguidcol="False" is_identity="False" description="" default_definition="" />
                    <column name="CreatedOn" id="8" data_type="datetime" max_length="8" precision="23" scale="3" is_nullable="True" is_rowguidcol="False" is_identity="False" description="" default_definition="(getdate())" />
                    <column name="CreatedBy" id="9" data_type="nvarchar" max_length="100" precision="0" scale="0" is_nullable="True" is_rowguidcol="False" is_identity="False" description="" default_definition="(N'System')" />
                    <column name="ModifiedOn" id="10" data_type="datetime" max_length="8" precision="23" scale="3" is_nullable="True" is_rowguidcol="False" is_identity="False" description="" default_definition="(getdate())" />
                    <column name="ModifiedBy" id="11" data_type="nvarchar" max_length="100" precision="0" scale="0" is_nullable="True" is_rowguidcol="False" is_identity="False" description="" default_definition="(N'System')" />
                </columns>
                <primary_key name="PK_Transaction">
                    <column name="TransactionId" id="1" />
                </primary_key>
                <foreign_keys>
                    <foreign_key name="FK_OrderTransactions_TransactionTypes">
                        <column constraint_column_name="TransactionTypeID" constraint_column_id="1" referenced_object="TransactionType" referenced_column_name="TransactionTypeID" />
                    </foreign_key>
                    <foreign_key name="FK_Transaction_Order1">
                        <column constraint_column_name="OrderId" constraint_column_id="1" referenced_object="Order" referenced_column_name="OrderId" />
                    </foreign_key>
                    <foreign_key name="FK_Transaction_Transaction">
                        <column constraint_column_name="TransactionId" constraint_column_id="1" referenced_object="Transaction" referenced_column_name="TransactionId" />
                    </foreign_key>
                </foreign_keys>
            </table>
             * */
            m_Name = tableNode.Attributes["name"].Value;

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

            XmlNodeList list = tableNode.SelectNodes(".\\columns\\column");

            foreach (XmlNode node in list)
            {
                m_ColumnList.Add(new Column(node));
            }

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