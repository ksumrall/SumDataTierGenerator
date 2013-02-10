using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;

using SumDataTierGenerator.Common;

namespace SumDataTierGenerator.SchemaExtractor
{
    interface ISchemaExtractor
    {
        string ConnectionString{get; set;}
        XmlDocument GetSchemaDefinition();
    }
}
