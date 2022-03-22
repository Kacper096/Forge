using Forge.Persistence.InfluxDb.Models;
using InfluxDB.Client.Api.Domain;

namespace Forge.Persistence.InfluxDb.Extensions
{
    public static class InfluxDataExtensions
    {
        public static InfluxData<T> ToInfluxData<T>(this T @object, WritePrecision precision) 
            where T : class 
            => new(@object, precision);
    }
}
