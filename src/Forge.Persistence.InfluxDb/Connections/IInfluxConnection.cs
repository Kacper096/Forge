using InfluxDB.Client;

namespace Forge.Persistence.InfluxDb.Connections
{
    public interface IInfluxConnection
    {
        InfluxDBClient Connection { get; }
    }
}