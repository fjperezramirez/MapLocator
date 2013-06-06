using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CompassLib
{
    public class EnumeratedType
    {
        public EnumeratedType()
        {
        }

        public EnumeratedType(IDataReader reader)
        {
            int ordinal = reader.GetOrdinal("EnumeratedTypeID");
            Code = reader.GetInt32(ordinal);

            ordinal = reader.GetOrdinal("Name");
            Name = reader.GetString(ordinal);

            ordinal = reader.GetOrdinal("DescriptionEn");
            DescriptionEn = reader.GetString(ordinal);

            ordinal = reader.GetOrdinal("DescriptionEs");
            DescriptionEs = reader.GetString(ordinal);
        }

        public int Code { get; set; }
        public string Name { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionEs { get; set; }
    }
}
