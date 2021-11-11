using MediatR;

namespace Forge.MediatR.CQRS.Commands
{
    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Unit>
        where TCommand : ICommand
    {
    }
}
