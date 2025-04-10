using System.Text.Json;

namespace OntuPhdApi.Utilities
{
    public static class JsonSerializerHelper
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public static T Deserialize<T>(string json) =>
            string.IsNullOrEmpty(json) ? default : JsonSerializer.Deserialize<T>(json, _options);

        public static string Serialize<T>(T obj) =>
            obj == null ? null : JsonSerializer.Serialize(obj, _options);
    }
}