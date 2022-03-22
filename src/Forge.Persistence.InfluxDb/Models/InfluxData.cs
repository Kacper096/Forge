using InfluxDB.Client.Api.Domain;

namespace Forge.Persistence.InfluxDb.Models
{
    public sealed class InfluxData <T>
        where T : class
    {
        public T Data { get; }
        public WritePrecision Precision { get; }

        public InfluxData(T data, WritePrecision precision)
        {
            Data = data;
            Precision = precision;
        }
    }
}
