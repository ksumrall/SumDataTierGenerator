using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SumDataTierGenerator.Common
{
    public interface IView
    {
        string Name { get; set; }
        string DatabaseName { get; set; }
        string Description { get; set; }
        string Schema { get; set; }
        bool Build { get; set; }
        Column[] Columns { get; set; }
    }
}
