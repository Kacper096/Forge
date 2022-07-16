using Forge.Application.Data.Measurements;
using Forge.MediatR.CQRS.Queries;
using Forge.Persistence.Redis;

namespace Forge.Application.Queries;

internal class GetTemperatureFromCacheQueryHandler : IQueryHandler<GetTemperatureFromCacheQuery, Temperature>
{
    private readonly IRService _rService;

    public GetTemperatureFromCacheQueryHandler(IRService rService)
    {
        _rService = rService;
    }

    public Task<Temperature> Handle(GetTemperatureFromCacheQuery query, CancellationToken cancellationToken)
        => Task.Run(() =>_rService.Get<Temperature>(query.Key), cancellationToken);
}
