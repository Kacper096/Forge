using Forge.MediatR.CQRS.Commands;

namespace Forge.Application.Commands;

public record AddTemperatureMeasurementCommand(bool Cold, decimal Value) : ICommand;
