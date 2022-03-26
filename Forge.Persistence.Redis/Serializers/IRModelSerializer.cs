namespace Forge.Persistence.Redis.Serializers
{
    internal interface IRModelSerializer
    {
        TModel Deserialize<TModel>(byte[] bytes);
        byte[] Serialize(object @object);
    }
}