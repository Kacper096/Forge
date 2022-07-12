using Forge.Application.Constants;
using Forge.Application.Data.Measurements;
using Forge.MediatR.CQRS.Commands;
using Forge.Persistence.InfluxDb.Extensions;
using Forge.Persistence.InfluxDb.Services;
using MediatR;

namespace Forge.Application.Commands;

internal class AddTemperatureMeasurementCommandHandler : ICommandHandler<AddTemperatureMeasurementCommand>
{
    private readonly IInfluxService _influxService;

    public AddTemperatureMeasurementCommandHandler(IInfluxService influxService)
    {
        _influxService = influxService;
    }

    public Task<Unit> Handle(AddTemperatureMeasurementCommand command, CancellationToken cancellationToken)
    {
        var temperature = new Temperature
        {
            Cold = command.Cold,
            Value = command.Value,
        };
        var influxData = temperature.ToInfluxData(InfluxDB.Client.Api.Domain.WritePrecision.Ms);
        _influxService.Add(BucketNames.OwnBucket, influxData);
        return Unit.Task;
    }
}
