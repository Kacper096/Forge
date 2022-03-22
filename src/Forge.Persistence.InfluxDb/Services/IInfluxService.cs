using Forge.Persistence.InfluxDb.Builders;
using Forge.Persistence.InfluxDb.Models;

namespace Forge.Persistence.InfluxDb.Services
{
    public interface IInfluxService
    {
        bool Add<T>(string bucket, InfluxData<T> data) where T : class;
        Task<List<T?>> GetAsync<T>(QueryBuilder queryBuilder);
    }
}