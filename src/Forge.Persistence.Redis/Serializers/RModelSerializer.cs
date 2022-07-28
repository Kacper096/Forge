using System.Text.Json;
using System.Text.Json.Serialization;

namespace Forge.Persistence.Redis.Serializers;

public sealed class RModelSerializer : IRModelSerializer
{
    private readonly JsonSerializerOptions _options;

    public RModelSerializer(JsonSerializerOptions? options = null)
    {
        _options = options ?? CreateDefaultSerializerOptions();
    }

    public byte[] Serialize(object @object) => JsonSerializer.SerializeToUtf8Bytes(@object, options: _options);
    public TModel Deserialize<TModel>(byte[] bytes) => JsonSerializer.Deserialize<TModel>(bytes, options: _options)!;

    private static JsonSerializerOptions CreateDefaultSerializerOptions() => new()
    {
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
    };
}
