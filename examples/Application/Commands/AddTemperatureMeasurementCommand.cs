using Forge.MediatR.CQRS.Commands;

namespace Forge.Application.Commands;

public record AddTemperatureMeasurementCommand(bool IsCold, decimal Temperature) : ICommand;
