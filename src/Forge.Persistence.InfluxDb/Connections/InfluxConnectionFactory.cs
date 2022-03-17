using Forge.Persistence.InfluxDb.Options;
using InfluxDB.Client;

namespace Forge.Persistence.InfluxDb.Connections
{
    internal sealed class InfluxConnectionFactory
    {
        private readonly IInfluxOptions _options;
        public InfluxConnectionFactory(IInfluxOptions options)
        {
            _options = options;
        }

        public IInfluxConnection Create()
        {
            var client = InfluxDBClientFactory.Create(_options.GetAddress(), _options.Token);
            return new InfluxConnection(client);
        }
    }
}
