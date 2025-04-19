using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OntuPhdApi.Utilities
{
    public class IgnoreEmptyCollectionsConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        => typeof(IEnumerable).IsAssignableFrom(typeToConvert) && typeToConvert != typeof(string);

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var elementType = typeToConvert.IsGenericType
                ? typeToConvert.GetGenericArguments()[0]
                : typeof(object);

            var converterType = typeof(EmptyCollectionConverter<>).MakeGenericType(elementType);
            return (JsonConverter?)Activator.CreateInstance(converterType);
        }

        private class EmptyCollectionConverter<T> : JsonConverter<IEnumerable<T>>
        {
            public override IEnumerable<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                => JsonSerializer.Deserialize<List<T>>(ref reader, options);

            public override void Write(Utf8JsonWriter writer, IEnumerable<T> value, JsonSerializerOptions options)
            {
                if (value != null && value.Any())
                    JsonSerializer.Serialize(writer, value, options);
            }
        }


    }


    public class IgnoreEmptyObjectsConverter : JsonConverter<object>
    {
        public override bool CanConvert(Type typeToConvert) => true;

        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => JsonSerializer.Deserialize(ref reader, typeToConvert, options);

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            var props = value.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.GetValue(value) != null)
                .ToList();

            if (props.Count == 0)
                return;

            writer.WriteStartObject();
            foreach (var prop in props)
            {
                var propValue = prop.GetValue(value);
                if (propValue == null) continue;
                writer.WritePropertyName(JsonNamingPolicy.CamelCase.ConvertName(prop.Name));
                JsonSerializer.Serialize(writer, propValue, propValue.GetType(), options);
            }
            writer.WriteEndObject();
        }
    }






}
