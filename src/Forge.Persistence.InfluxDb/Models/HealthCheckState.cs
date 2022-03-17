namespace Forge.Persistence.InfluxDb.Models
{
    public sealed class HealthCheckState : IHealthCheckState, ISetHealthCheckState
    {
        public bool IsHealthy { get; internal set; }

        void ISetHealthCheckState.SetIsHealthy(bool isHealthy)
        {
            IsHealthy = isHealthy;
        }
    }
}
