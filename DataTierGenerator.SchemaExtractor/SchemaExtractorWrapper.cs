using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

using SumDataTierGenerator.Common;

namespace SumDataTierGenerator.SchemaExtractor
{
    public sealed class SchemaExtractorWrapper
    {

        #region private / protected member variables

        private string m_ProviderType = "";
        private string m_ConnectionString = "";

        #endregion

        #region constructors / desturctors

        public SchemaExtractorWrapper()
        {
        }

        public SchemaExtractorWrapper(string providerType, string connectionString)
            : this()
        {

            m_ProviderType = providerType;
            m_ConnectionString = connectionString;

        }

        #endregion

        #region public properties

        public string ConnectionString
        {
            get
            {
                return m_ConnectionString;
            }
            set
            {
                m_ConnectionString = value;
            }
        }

        public string ProviderType
        {
            get { return m_ProviderType; }
            set { m_ProviderType = value; }
        }

        #endregion

        #region public methods

        public XmlDocument GetSchemaDefinition()
        {
            ISchemaExtractor se = null;
            XmlDocument xDoc = null;

            switch(m_ProviderType)
            {
                case "Microsoft SQL Server (SqlClient)":
                    se = new SqlServerSchemaExtractor(m_ConnectionString);
                    break;

                case "Microsoft SQL Server Compact 3.5 (SqlCeClient)":
                    se = new SqlCeServerSchemaExtractor(m_ConnectionString);
                    break;

                default:
                    se = null;
                    break;
            }

            if (se != null)
            {
                xDoc = se.GetSchemaDefinition();
            }

            return xDoc;
        }

        #endregion

    }
}