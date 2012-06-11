using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;

namespace TotalSafety.DataTierGenerator.SchemaExtractor
{
    class Class1
    {
        [STAThread]
        static void Main()
        {
            SchemaExtractorWrapper s = new SchemaExtractorWrapper();
            s.ProviderType = "System.Data.SqlClient";
            s.ConnectionString = "Data Source=.\\sqlexpress;Initial Catalog=DecorStores;Integrated Security=True";
            XmlDocument xDoc = s.GetSchemaDefinition();
        }
    }
}
