using Forge.Persistence.Redis.Connections;
using Forge.Persistence.Redis.Exceptions;
using Forge.Persistence.Redis.Serializers;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Forge.Persistence.Redis
{
    public class RService : IRService
    {
        private const string AddLog = "Redis service has added key: {0} with value: {1}";
        private const string GetLog = "Redis service has fetched value: {1} via key: {0}";

        private readonly IConnection _connection;
        private readonly IRModelSerializer _serializer;
        private readonly ILogger<RService> _logger;

        public RService(IConnection connection,
                        IRModelSerializer serializer,
                        ILogger<RService> logger)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Add<RType>(string key, RType model, int db = 1) where RType : class
        {
            ValidateKey(key);
            ValidateModel(model);

            var serializedModel = _serializer.Serialize(model);
            var database = GetDatabase(db);
            database.StringSet(key, serializedModel);

            if (_logger.IsEnabled(LogLevel.Trace))
            {
                var modelDescription = model.ToString();
                _logger.LogTrace(AddLog, key, modelDescription);
            }
        }

        public void Add(string key, string value, int db = 1)
        {
            ValidateKey(key);
            ValidateValue(value);

            var database = GetDatabase(db);
            database.StringSet(key, value);

            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace(AddLog, key, value);
            }
        }

        public RType Get<RType>(string key, int db = 1) where RType : class
        {
            ValidateKey(key);

            var database = GetDatabase(db);
            var dbModel = database.StringGet(key);
            var model = _serializer.Deserialize<RType>(dbModel);

            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace(GetLog, key, model.ToString());
            }

            return model;
        }

        public string Get(string key, int db = 1)
        {
            ValidateKey(key);

            var database = GetDatabase(db);
            var dbValue = database.StringGet(key);
            var value = dbValue.ToString();

            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace(GetLog, key, value);
            }

            return value;
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
