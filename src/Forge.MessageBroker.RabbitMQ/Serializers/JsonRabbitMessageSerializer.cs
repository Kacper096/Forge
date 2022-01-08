using System.Text.Json;
using System.Text.Json.Serialization;

namespace Forge.MessageBroker.RabbitMQ.Serializers
{
    internal sealed class JsonRabbitMessageSerializer : IRabbitMessageSerializer
    {
        private readonly JsonSerializerOptions _options;

        public JsonRabbitMessageSerializer(JsonSerializerOptions? options = null)
        {
            _options = options ?? CreateDefaultSerializerOptions();
        }

        public ReadOnlySpan<byte> Serialize(object value) => JsonSerializer.SerializeToUtf8Bytes(value, options: _options);
        public TModel Deserialize<TModel>(ReadOnlySpan<byte> value) => JsonSerializer.Deserialize<TModel>(value, options: _options);
        public object? Deserialize(ReadOnlySpan<byte> value, Type type) => JsonSerializer.Deserialize(value, type, options: _options);

        private JsonSerializerOptions CreateDefaultSerializerOptions() => new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
        };
    }
}
