using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace TotalSafety.DataTierGenerator.Common
{

    public class ColumnList : List<Column>
    {

        public Column this[string columnName]
        {
            get
            {
                return FindColumn(columnName);
            }
        }

        public Column FindColumn(string columnName)
        {

            foreach (Column column in this)
            {
                if (column.Name == columnName)
                {
                    return column;
                }
            }

            return null;

        }

    }

    /// <summary>
    /// Class that stores information for columns in a database table.
    /// </summary>
    public partial class Column
    {

        #region constructors / desturctors

        /// <summary>
        /// Default constructor.  All collections are initialized.
        /// </summary>
        public Column()
        {
        }

        public Column(XmlNode columnNode)
        {
            bool isRowGuid;
            bool isIdentity;
            bool isNullable;
            bool isComputed;
            /*
                    <column m_Name="TransactionId" id="1" data_type="varchar" max_length="50" m_Precision="0" m_Scale="0" m_I_nullable="False" m_I_rowguidcol="False" m_I_identity="False" m_Description="" default_definition="" />
             * */
            Name = columnNode.Attributes["name"].Value;
            Description = columnNode.Attributes["description"].Value;
            DbType = columnNode.Attributes["data_type"].Value;
            //m_ClrType = columnNode.Attributes[""].Value;
            //m_LanguageType = columnNode.Attributes[""].Value;
            //m_EnumeratedTypeName = columnNode.Attributes[""].Value;
            Length = columnNode.Attributes["max_length"].Value;
            Precision = columnNode.Attributes["precision"].Value;
            Scale = columnNode.Attributes["scale"].Value;
            
            bool.TryParse(columnNode.Attributes["is_rowguidcol"].Value, out isRowGuid);
            IsRowGuid = IsRowGuid;
            bool.TryParse(columnNode.Attributes["is_identity"].Value, out isIdentity);
            IsIdentity = isIdentity;
            bool.TryParse(columnNode.Attributes["is_nullable"].Value, out isNullable);
            IsNullable = isNullable;
            bool.TryParse(columnNode.Attributes["is_computed"].Value, out isComputed);
            IsComputed = isComputed;

            if (columnNode.Attributes["default_definition"] != null)
            {
                DefaultValue = columnNode.Attributes["default_definition"].Value;
            }

            PropertyName = System.Text.RegularExpressions.Regex.Replace(Name, "\\W", "_");
        }

        #endregion

        #region comparison implementation

        public static int CompareByProgrammaticAlias(Column column1, Column column2)
        {
            return column1.PropertyName.CompareTo(column2.PropertyName);
        }

        #endregion

    }
}