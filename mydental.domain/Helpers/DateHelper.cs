using System;
using System.Globalization;

namespace mydental.domain.Helpers
{
    public static class DateHelper
    {
        private const string DateFormat = "yyyy-MM-dd";

        // ✅ Converts DateTime to a formatted string (for API responses)
        public static string FormatDate(DateTime date)
        {
            return date.ToString(DateFormat);
        }

        // ✅ Converts nullable DateTime? to formatted string (handles null values)
        public static string FormatDate(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString(DateFormat) : null;
        }

        // ✅ Parses a string date (yyyy-MM-dd) into DateTime (for API requests)
        public static DateTime? ParseDate(string dateString)
        {
            if (DateTime.TryParseExact(dateString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate;
            }
            return null; // Return null if parsing fails
        }
    }
}
