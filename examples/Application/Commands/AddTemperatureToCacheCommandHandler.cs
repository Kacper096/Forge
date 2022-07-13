using Forge.Application.Data.Measurements;
using Forge.MediatR.CQRS.Commands;
using Forge.Persistence.Redis;
using MediatR;

namespace Forge.Application.Commands;

internal class AddTemperatureToCacheCommandHandler : ICommandHandler<AddTemperatureToCacheCommand>
{
    private readonly IRService _rService;

    public AddTemperatureToCacheCommandHandler(IRService rService)
    {
        _rService = rService;
    }

    public Task<Unit> Handle(AddTemperatureToCacheCommand command, CancellationToken cancellationToken)
    {
        var temperature = new Temperature
        {
            Cold = command.IsCold,
            Value = command.Temperature,
        };
        _rService.Add(command.Key, temperature);
        return Unit.Task;
    }
}
