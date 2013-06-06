using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CompassLib
{
    public static class ExtensionMethods
    {
        public static void Load(this ICollection<Parameter> paramCollection, IDataReader reader)
        {
            while (reader.Read())
            {
                Parameter newParam = new Parameter(reader);

                paramCollection.Add(newParam);
            }
        }

        public static object GetValueOrDbNull(this string value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }

            return value;
        }

        public static object GetValueOrDbNull(this int? nullableInt)
        {
            if (nullableInt.HasValue)
            {
                return nullableInt;
            }

            return DBNull.Value; 
        }

        public static object GetValueOrDbNull(this DateTime? nullableDate)
        {
            if (nullableDate.HasValue)
            {
                return nullableDate.Value;
            }

            return DBNull.Value;
        }

        public static object GetValueOrDbNull(this double? nullableDouble)
        {
            if (nullableDouble.HasValue)
            {
                return nullableDouble.Value;
            }

            return DBNull.Value;
        }

        public static object EmptyIfNull(this string value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return value;
        }
    }
}
