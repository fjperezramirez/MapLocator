using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CompassLib
{
    public class Location
    {
        public Location()
        {
            Parameters = new List<Parameter>();
        }

        public Location(IDataReader reader)
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

            ordinal = reader.GetOrdinal("DescriptionEn");
            DescriptionEn = reader.GetString(ordinal);

            ordinal = reader.GetOrdinal("DescriptionEs");
            DescriptionEs = reader.GetString(ordinal);

            ordinal = reader.GetOrdinal("Address1");
            Address1 = reader.GetString(ordinal);

            ordinal = reader.GetOrdinal("Address2");
            Address2 = reader.GetString(ordinal);

            ordinal = reader.GetOrdinal("City");
            City = reader.GetString(ordinal);

            ordinal = reader.GetOrdinal("State");
            State = reader.GetString(ordinal);

            ordinal = reader.GetOrdinal("ZipCode");
            ZipCode = reader.GetString(ordinal);

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

            ordinal = reader.GetOrdinal("Z");

            if (!reader.IsDBNull(ordinal))
            {
                Z = reader.GetDouble(ordinal);
            }

            ordinal = reader.GetOrdinal("M");
            
            if (!reader.IsDBNull(ordinal))
            {
                M = reader.GetDouble(ordinal);
            }

            ordinal = reader.GetOrdinal("CreatedDate");
            CreatedDate = reader.GetDateTime(ordinal);

            ordinal = reader.GetOrdinal("ModifiedDate");

            if (!reader.IsDBNull(ordinal))
            {
                ModifiedDate = reader.GetDateTime(ordinal);
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

            ordinal = reader.GetOrdinal("SourceIntUniqueKey");

            if (!reader.IsDBNull(ordinal))
            {
                SourceIntUniqueKey = reader.GetInt32(ordinal);
            }
        }

        public int LocationID { get; set; }
        public int LocationTypeEnumCode { get; set; }
        public int LocationGroupEnumCode { get; set; }
        public string Name { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionEs { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Z { get; set; }
        public double? M { get; set; }
        public List<Parameter> Parameters { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? IncidentDate { get; set; }

        public string Custom { get; set; }
        public int? SourceIntUniqueKey { get; set; }
    }
}
