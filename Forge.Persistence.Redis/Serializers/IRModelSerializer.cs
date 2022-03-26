namespace Forge.Persistence.Redis.Serializers
{
    public interface IRModelSerializer
    {
        TModel Deserialize<TModel>(byte[] bytes);
        byte[] Serialize(object @object);
    }
}