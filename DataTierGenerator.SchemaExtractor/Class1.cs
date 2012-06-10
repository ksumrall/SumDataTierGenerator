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
            SchemaExtractor s = new SchemaExtractor();
            s.ProviderType = "System.Data.SqlClient";
            s.ConnectionString = "Data Source=HOUTFS01;Initial Catalog=IntelaTrac_2;Integrated Security=True";
            XmlDocument xDoc = s.GetSchemaDefinition();
        }
    }
}
