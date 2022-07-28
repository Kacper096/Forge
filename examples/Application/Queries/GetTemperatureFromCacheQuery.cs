using Forge.Application.Data.Measurements;
using Forge.MediatR.CQRS.Queries;

namespace Forge.Application.Queries;

public class GetTemperatureFromCacheQuery : MiniAPIQuery<GetTemperatureFromCacheQuery, Temperature>
{
    public string Key { get; set; } = String.Empty;
}
