namespace Forge.Persistence.InfluxDb.Models
{
    internal interface ISetHealthCheckState
    {
        internal void SetIsHealthy(bool isHealthy);
    }
}
