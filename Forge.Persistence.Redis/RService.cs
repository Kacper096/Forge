using Forge.Persistence.Redis.Connections;
using Forge.Persistence.Redis.Exceptions;
using Forge.Persistence.Redis.Serializers;
using StackExchange.Redis;

namespace Forge.Persistence.Redis
{
    public class RService : IRService
    {
        private readonly IConnection _connection;
        private readonly IRModelSerializer _serializer;

        public RService(IConnection connection, IRModelSerializer serializer)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public void Add<RType>(string key, RType model, int db = 1) where RType : class
        {
            ValidateKey(key);
            ValidateModel(model);

            var serializedModel = _serializer.Serialize(model);
            var database = GetDatabase(db);
            database.StringSet(key, serializedModel);
        }

        public void Add(string key, string value, int db = 1)
        {
            ValidateKey(key);
            ValidateValue(value);

            var database = GetDatabase(db);
            database.StringSet(key, value);
        }

        public RType Get<RType>(string key, int db = 1) where RType : class
        {
            ValidateKey(key);

            var database = GetDatabase(db);
            var dbModel = database.StringGet(key);
            return _serializer.Deserialize<RType>(dbModel);
        }

        public string Get(string key, int db = 1)
        {
            ValidateKey(key);

            var database = GetDatabase(db);
            var dbValue = database.StringGet(key);
            return dbValue.ToString();
        }

        private IDatabase GetDatabase(int dbNumber) => _connection.Connection.GetDatabase(dbNumber);

        private void ValidateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new EmptyKeyException();
            }
        }

        private void ValidateValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new EmptyValueException();
            }
        }

        private void ValidateModel(object @object)
        {
            if (@object == null)
            {
                throw new NullModelException();
            }
        }
    }
}
