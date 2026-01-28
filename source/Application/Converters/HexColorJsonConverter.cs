using Data.ValueObjects;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Converters
{
    public sealed class HexColorJsonConverter : JsonConverter<HexColor>
    {
        public override HexColor Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return HexColor.Parse(value!);
        }

        public override void Write(Utf8JsonWriter writer, HexColor value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }
}