using Forge.Persistence.InfluxDb.Builders;
using Forge.Persistence.InfluxDb.Connections;
using Forge.Persistence.InfluxDb.Models;
using Forge.Persistence.InfluxDb.Options;
using InfluxDB.Client.Core;
using Microsoft.Extensions.Logging;

namespace Forge.Persistence.InfluxDb.Services
{
    public class InfluxService : IInfluxService
    {
        private readonly IInfluxConnection _connection;
        private readonly IInfluxOptions _options;
        private readonly IHealthCheckState _healthCheckState;
        private readonly ILogger<InfluxService> _logger;

        public InfluxService(IInfluxConnection connection,
                             IInfluxOptions options,
                             IHealthCheckState healthCheckState,
                             ILogger<InfluxService> logger)
        {
            _connection = connection;
            _options = options;
            _healthCheckState = healthCheckState;
            _logger = logger;
        }

        public bool Add<T>(string bucket, InfluxData<T> data)
            where T : class
        {
            if (!_healthCheckState.IsHealthy || !ValidateMeasurement(data.Data))
            {
                _logger.LogWarning("Cannot add measurement beacuse of not responding db or not correct measurement. Measurement: {0}", data.Data);
                return false;
            }

            using var writeApi = _connection.Connection.GetWriteApi();
            writeApi.WriteMeasurement(bucket, _options.Organization, data.Precision, data.Data);
            _logger.LogTrace("Measurement: {0} has been added to db.", data.Data);
            return true;
        }

        public async Task<List<T?>> GetAsync<T>(QueryBuilder queryBuilder)
            where T : class
        {
            var query = queryBuilder.Build();
            return await _connection.Connection.GetQueryApi().QueryAsync<T>(query, _options.Organization);
        }

        private static bool ValidateMeasurement(object measurement)
        {
            if (measurement == null)
            {
                return false;
            }
            var type = measurement.GetType();
            var hasMeasurementAttribute = type.GetCustomAttributes(false).Any(att => att.GetType() == typeof(Measurement));
            if (!hasMeasurementAttribute)
            {
                return false;
            }

            return true;
        }
    }
}
