using Forge.MediatR.CQRS.Commands;

namespace Forge.Application.Commands;

public record AddTemperatureToCacheCommand(string Key, bool IsCold, decimal Temperature) : ICommand;
