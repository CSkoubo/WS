using Core;
using System.Text.Json.Serialization;

namespace Core
{
    public class DataDescriptor
    {
        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonPropertyName("Value")]
        public byte[] Value { get; set; }
    }

    public class DataClass
    {
        [JsonPropertyName("Text")]
        public string Text { get; set; }

        [JsonPropertyName("Number")]
        public int Number { get; set; }
    }

}