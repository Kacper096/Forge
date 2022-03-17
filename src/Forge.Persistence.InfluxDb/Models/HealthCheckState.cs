namespace Forge.Persistence.InfluxDb.Models
{
    public sealed class HealthCheckState : IHealthCheckState
    {
        public bool IsHealthy { get; internal set; }
    }
}
