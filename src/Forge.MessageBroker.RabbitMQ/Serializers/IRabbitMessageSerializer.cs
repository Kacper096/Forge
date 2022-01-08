namespace Forge.MessageBroker.RabbitMQ.Serializers
{
    public interface IRabbitMessageSerializer
    {
        public ReadOnlySpan<byte> Serialize(object value);
        public TModel Deserialize<TModel>(ReadOnlySpan<byte> value);
        public object? Deserialize(ReadOnlySpan<byte> value, Type type);
    }
}
