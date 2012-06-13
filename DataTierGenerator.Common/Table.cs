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
    public partial class Table : IView
    {

        #region private and protected member variables

        private string m_ClassName;

        #endregion

        #region constructors / desturctors

        /// <summary>
        /// Default constructor.  All collections are initialized.
        /// </summary>
        public Table()
        {
            m_ClassName = "";
        }

        public Table(XmlNode tableNode)
            : this()
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
            Name = tableNode.Attributes["name"].Value;

            XmlNodeList list = tableNode.SelectNodes(".//columns//column");

            List<Column> columnList = new List<Column>();
            foreach (XmlNode node in list)
            {
                columnList.Add(new Column(node));
            }
            Columns = columnList.ToArray();

            // add primary key
            XmlNode keyNode = tableNode.SelectSingleNode(".//primary_key");
            if (keyNode != null)
            {
                PrimaryKey = new TablePrimaryKey();
                PrimaryKey.Name = keyNode.Attributes["name"].Value;
                list = keyNode.SelectNodes(".//key_column");
                TablePrimaryKeyKeyColumn kk;
                List<TablePrimaryKeyKeyColumn> kkl = new List<TablePrimaryKeyKeyColumn>();
                foreach (XmlNode node in list)
                {
                    kk = new TablePrimaryKeyKeyColumn();
                    kk.ColumnName = node.Attributes["column_name"].Value;
                    kk.KeyOrdinal = node.Attributes["key_ordinal"].Value;
                    kkl.Add(kk);
                }
                PrimaryKey.KeyColumn = kkl.ToArray();
            }

            // add foreign keys
            XmlNodeList keyList = tableNode.SelectNodes(".//foreign_keys/foreign_key");
            if (keyList.Count > 0)
            {
                TableForeignKey foreignKey;
                List<TableForeignKey> foreignKeys = new List<TableForeignKey>();
                foreach (XmlNode node in keyList)
                {
                    foreignKey = new TableForeignKey();
                    foreignKey.Name = node.Attributes["name"].Value;
                    list = node.SelectNodes(".//column");
                    columnList.Clear();
                    foreach (XmlNode columnNode in list)
                    {
                        columnList.Add(new Column(columnNode));
                    }
                    foreignKey.Columns = columnList.ToArray();

                    foreignKeys.Add(foreignKey);
                }
                ForeignKeys = foreignKeys.ToArray();
            }
        }

        #endregion

        #region public methods

        public XmlNode GetNode(XmlDocument xmlDoc)
        {

            XmlNode node = xmlDoc.CreateElement(Name);

            return node;
        }

        public Column GetPkColumn(string name)
        {
            Column pkColumn = null;

            foreach (Column column in Columns)
            {
                if (column.Name == name)
                {
                    pkColumn = column;
                    break;
                }
            }

            return pkColumn;
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