using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core
{
        public interface IDTOSerializer
        {
            JsonSerializerOptions Settings { get; }
            string MediaType { get; }
            T DeserializeObject<T>(string objAsString);

            T DeserializeObjectRaw<T>(string objAsString);

            string SerializeObject(object input);
        }

        public class SerializedObject
        {
            public SerializedObject(string objectAsString)
            {
                ObjectAsString = objectAsString;
            }

            public string ObjectAsString { get; private set; }
        }


    public class JsonSerializerWrapper : IDTOSerializer
    {
        public JsonSerializerOptions Settings { get; private set; }

        public JsonSerializerWrapper()
        {
            Settings = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = false,
                Converters =
                {
                    new ObjectToInferredTypesConverter()
                }
            };
        }

        public string MediaType => "text/plain";
        public T DeserializeObject<T>(string objAsString)
        {
            return JsonSerializer.Deserialize<T>(objAsString, Settings);
        }

        public T DeserializeObjectRaw<T>(string objAsString)
        {
            return JsonSerializer.Deserialize<T>(objAsString, Settings);
        }
        public string SerializeObject(object input)
        {
            return JsonSerializer.Serialize(input, Settings);
        }
    }

    internal class ObjectToInferredTypesConverter : JsonConverter<object>
    {
        public override object Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.True: return true;
                case JsonTokenType.False: return false;
                case JsonTokenType.Number:
                    {
                        if (reader.TryGetInt64(out long l))
                        {
                            return l;
                        }

                        return reader.GetDouble();
                    }
                case JsonTokenType.String:
                    {
                        if (reader.TryGetDateTime(out DateTime datetime))
                        {
                            return datetime;
                        }

                        return reader.GetString();
                    }
                default:
                    return JsonDocument.ParseValue(ref reader).RootElement.Clone();
            }
        }

        public override void Write(
            Utf8JsonWriter writer,
            object objectToWrite,
            JsonSerializerOptions options) =>
            JsonSerializer.Serialize(writer, objectToWrite, objectToWrite.GetType(), options);
    }

    internal class DictionaryConverter : JsonConverter<Dictionary<string, string[]>>
    {
        public override Dictionary<string, string[]> Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException($"JsonTokenType was of type {reader.TokenType}, only objects are supported");
            }

            var dictionary = new Dictionary<string, string[]>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return dictionary;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("JsonTokenType was not PropertyName");
                }

                var propertyName = reader.GetString();

                if (string.IsNullOrWhiteSpace(propertyName))
                {
                    throw new JsonException("Failed to get property name");
                }

                reader.Read();

                dictionary.Add(propertyName, ExtractValue(ref reader, options));
            }

            return dictionary;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<string, string[]> value,
            JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }

        private string[] ExtractValue(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            List<string> array = new List<string>();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.StartArray:
                        continue;
                    case JsonTokenType.EndArray:
                        break;
                    case JsonTokenType.String:
                        {
                            array.Add(reader.GetString());
                            continue;
                        }
                    default:
                        break;
                }
            }

            return array.ToArray();
        }
    }
}
