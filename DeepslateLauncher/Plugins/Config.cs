using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace DeepslateLauncher.Plugins
{

    [JsonSerializable(typeof(Config))]
    public class Config(JsonSerializerOptions? options = default) : JsonSerializerContext(options)
    {
        [JsonInclude]
        [JsonPropertyName("plugins")]
        public string[]? Plugins { get; set; }

        protected override JsonSerializerOptions? GeneratedSerializerOptions => Options;
        public override JsonTypeInfo? GetTypeInfo(Type type)
        {
            return JsonTypeInfo.CreateJsonTypeInfo(type, Options);
        }
    }
}
