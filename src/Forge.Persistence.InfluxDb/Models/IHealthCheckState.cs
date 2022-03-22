namespace Forge.Persistence.InfluxDb.Models
{
    public interface IHealthCheckState
    {
        bool IsHealthy { get; }
    }
}