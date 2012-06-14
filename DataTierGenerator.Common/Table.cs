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
                        <fk_column constraint_column_name="TransactionTypeID" constraint_column_id="1" referenced_object="TransactionType" referenced_column_name="TransactionTypeID" />
                    </foreign_key>
                    <foreign_key name="FK_Transaction_Order1">
                        <fk_column constraint_column_name="OrderId" constraint_column_id="1" referenced_object="Order" referenced_column_name="OrderId" />
                    </foreign_key>
                    <foreign_key name="FK_Transaction_Transaction">
                        <fk_column constraint_column_name="TransactionId" constraint_column_id="1" referenced_object="Transaction" referenced_column_name="TransactionId" />
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
                PrimaryKey = new PrimaryKey();
                PrimaryKey.Name = keyNode.Attributes["name"].Value;
                list = keyNode.SelectNodes(".//pk_column");
                PkColumn pkc;
                List<PkColumn> pkcl = new List<PkColumn>();
                foreach (XmlNode node in list)
                {
                    pkc = new PkColumn();
                    pkc.ColumnName = node.Attributes["column_name"].Value;
                    pkc.KeyOrdinal = node.Attributes["key_ordinal"].Value;
                    pkcl.Add(pkc);
                }
                PrimaryKey.PkColumns = pkcl.ToArray();
            }

            // add foreign keys
            XmlNodeList keyList = tableNode.SelectNodes(".//foreign_keys/foreign_key");
            if (keyList.Count > 0)
            {
                ForeignKey foreignKey;
                FkColumn fkColumn;
                List<ForeignKey> foreignKeys = new List<ForeignKey>();
                List<FkColumn> fkColumnList = new List<FkColumn>();
                foreach (XmlNode node in keyList)
                {
                    foreignKey = new ForeignKey();
                    foreignKey.Name = node.Attributes["name"].Value;
                    list = node.SelectNodes(".//fk_column");
                    fkColumnList.Clear();
                    foreach (XmlNode columnNode in list)
                    {
                        fkColumn = new FkColumn();
                        fkColumn.constraint_column_name = columnNode.Attributes["constraint_column_name"].Value;
                        fkColumn.constraint_column_id = columnNode.Attributes["constraint_column_id"].Value;
                        fkColumn.referenced_table = columnNode.Attributes["referenced_table"].Value;
                        fkColumn.referenced_column_name = columnNode.Attributes["referenced_column_name"].Value;
                        fkColumnList.Add(fkColumn);
                    }
                    foreignKey.FkColumns = fkColumnList.ToArray();

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