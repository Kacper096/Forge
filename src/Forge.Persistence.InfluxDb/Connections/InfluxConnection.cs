using InfluxDB.Client;

namespace Forge.Persistence.InfluxDb.Connections
{
    public sealed class InfluxConnection : IInfluxConnection
    {
        public InfluxDBClient Connection { get; }

        public InfluxConnection(InfluxDBClient connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }
    }
}
