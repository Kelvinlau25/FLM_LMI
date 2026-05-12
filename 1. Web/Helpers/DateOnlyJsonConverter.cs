using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PAB.Helpers
{
    /// <summary>
    /// Custom JSON converter to handle date strings without timezone conversion
    /// Prevents date shifting issues when deserializing dates from JavaScript
    /// </summary>
    public class DateOnlyJsonConverter : JsonConverter<DateTime>
    {
        private const string DateFormat = "yyyy-MM-dd";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateString = reader.GetString();
            
            if (string.IsNullOrEmpty(dateString))
            {
                return DateTime.MinValue;
            }

            // Try to parse as date-only string (yyyy-MM-dd)
            if (DateTime.TryParseExact(dateString, DateFormat, 
                System.Globalization.CultureInfo.InvariantCulture, 
                System.Globalization.DateTimeStyles.None, 
                out DateTime result))
            {
                return result;
            }

            // Fallback: try to parse as ISO date and extract date part only
            if (DateTime.TryParse(dateString, out DateTime isoResult))
            {
                return isoResult.Date; // Return only the date part, ignoring time
            }

            throw new JsonException($"Unable to parse date: {dateString}");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(DateFormat));
        }
    }
}
