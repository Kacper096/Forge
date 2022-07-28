using Forge.MediatR.CQRS.Commands;

namespace Forge.Application.Commands;

public class TestRabbitCommand : ICommand
{
    public string Name { get; set; } = string.Empty;
}
