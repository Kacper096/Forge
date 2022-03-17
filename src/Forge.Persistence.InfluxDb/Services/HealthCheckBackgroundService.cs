using Forge.Persistence.InfluxDb.Connections;
using Forge.Persistence.InfluxDb.Models;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Api.Service;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Forge.Persistence.InfluxDb.Services
{
    internal sealed class HealthCheckBackgroundService : IHostedService, IDisposable
    {
        private readonly IInfluxConnection _connection;
        private readonly ILogger<HealthCheckBackgroundService> _logger;
        private readonly ISetHealthCheckState _healthCheckState;
        private readonly System.Timers.Timer _timer = null!;

        public HealthCheckBackgroundService(IInfluxConnection influxConnection,
                                            ILogger<HealthCheckBackgroundService> logger,
                                            ISetHealthCheckState healthCheckState)
        {
            _connection = influxConnection;
            _logger = logger;
            _healthCheckState = healthCheckState;
            _timer = new System.Timers.Timer()
            {
                Interval = 100,
                Enabled = false,
                AutoReset = true
            };
            AddHealthCheckWork();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("InfluxDB HealthCheck is running.");
            _timer.Stop();
            _timer.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("InfluxDB HealthCheck is stopping.");
            _timer?.Stop();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Close();
            _timer?.Dispose();
        }

        private void AddHealthCheckWork()
        {
            var healthService = _connection.Connection.CreateService<HealthService>(typeof(HealthService));
            _timer.Elapsed += async (s, e) =>
            {
                var isHealthy = false;
                try
                {
                    var health = await healthService.GetHealthAsync();
                    isHealthy = health.Status == HealthCheck.StatusEnum.Pass;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Influx database is not responding");
                }
                _healthCheckState.SetIsHealthy(isHealthy);
            };
        }
    }
}
