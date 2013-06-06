using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CompassLib
{
    public class SimpleLocationSummary
    {
        public SimpleLocationSummary()
        {
        }

        public SimpleLocationSummary(IDataReader reader)
            : this()
        {
            int ordinal = reader.GetOrdinal("RecordCount");
            RecordCount = reader.GetInt32(ordinal);

            ordinal = reader.GetOrdinal("Custom");

            if (!reader.IsDBNull(ordinal))
            {
                Custom = reader.GetString(ordinal);
            }
        }

        public int RecordCount { get; set; }
        public string Custom { get; set; }
    }
}
