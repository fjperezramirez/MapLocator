using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CompassLib
{
    public class SimpleLocation
    {
        public SimpleLocation()
        {
        }

        public SimpleLocation(IDataReader reader)
            : this()
        {
            int ordinal = reader.GetOrdinal("LocationID");
            LocationID = reader.GetInt32(ordinal);

            ordinal = reader.GetOrdinal("LocationTypeEnumCode");
            LocationTypeEnumCode = reader.GetInt32(ordinal);

            ordinal = reader.GetOrdinal("LocationGroupEnumCode");
            LocationGroupEnumCode = reader.GetInt32(ordinal);

            ordinal = reader.GetOrdinal("Name");
            Name = reader.GetString(ordinal);

            ordinal = reader.GetOrdinal("City");
            City = reader.GetString(ordinal);

            ordinal = reader.GetOrdinal("Latitude");

            if (!reader.IsDBNull(ordinal))
            {
                Latitude = reader.GetDouble(ordinal);
            }

            ordinal = reader.GetOrdinal("Longitude");

            if (!reader.IsDBNull(ordinal))
            {
                Longitude = reader.GetDouble(ordinal);
            }

            ordinal = reader.GetOrdinal("IncidentDate");

            if (!reader.IsDBNull(ordinal))
            {
                IncidentDate = reader.GetDateTime(ordinal);
            }

            ordinal = reader.GetOrdinal("Custom");

            if (!reader.IsDBNull(ordinal))
            {
                Custom = reader.GetString(ordinal);
            }
        }

        public int LocationID { get; set; }
        public int LocationTypeEnumCode { get; set; }
        public int LocationGroupEnumCode { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime? IncidentDate { get; set; }
        public string Custom { get; set; }
    }
}
