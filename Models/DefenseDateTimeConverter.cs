using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace OntuPhdApi.Models
{
    public class DefenseDateTimeConverter : JsonConverter<DateTime>
    {
        private const string DateTimeFormat = "yyyy-MM-ddTHH:mm";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.GetString() is string dateString)
            {
                if (DateTime.TryParseExact(dateString, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                {
                    return date;
                }
                throw new JsonException($"Unable to parse '{dateString}' as DateTime in format {DateTimeFormat}.");
            }
            throw new JsonException("Expected a string for DateTime value.");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(DateTimeFormat, CultureInfo.InvariantCulture));
        }
    }
}
