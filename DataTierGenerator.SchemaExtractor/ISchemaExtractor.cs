using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;

using TotalSafety.DataTierGenerator.Common;

namespace TotalSafety.DataTierGenerator.SchemaExtractor
{
    interface ISchemaExtractor
    {
        string ConnectionString{get; set;}
        XmlDocument GetSchemaDefinition();
    }
}
