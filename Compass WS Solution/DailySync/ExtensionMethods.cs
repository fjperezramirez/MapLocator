using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DailySync
{
    internal static class ExtensionMethods
    {
        public static string EnsureNotNull(this string value, bool trimValue)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (trimValue)
            {
                value = value.Trim();
            }

            return value;
        }
    }
}
