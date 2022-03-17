using InfluxDB.Client;

namespace Forge.Persistence.InfluxDb.Connections
{
    internal interface IInfluxConnection
    {
        InfluxDBClient Connection { get; }
    }
}