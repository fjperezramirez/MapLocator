using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CompassLib
{
    public class Parameter
    {
        public Parameter()
        {
        }

        public Parameter(IDataReader reader)
        {
            int ordinal = reader.GetOrdinal("Name");
            Name = reader.GetString(ordinal);

            ordinal = reader.GetOrdinal("Value");
            Value = reader.GetString(ordinal);
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}
