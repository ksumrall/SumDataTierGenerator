using System;
using System.Collections.Generic;
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
            SchemaExtractor s = new SchemaExtractor("Data Source=HOUTFS01;Initial Catalog=IntelaTrac_2;Integrated Security=True");
            XmlDocument xDoc = s.GetSchemaDefinition();
            int i = 0;
        }
    }
}
