using System.Text.Json.Serialization;
using System.Text.Json;

namespace ConMaster.Deepslate.Protocol.Types.Skin
{
    public class JsonBase64BufferConverter : JsonConverter<byte[]>
    {
        public override byte[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetBytesFromBase64();
        }

        public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
        {
            writer.WriteBase64StringValue(value);
        }
    }
}
