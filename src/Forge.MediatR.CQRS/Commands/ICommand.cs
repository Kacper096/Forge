using MediatR;

namespace Forge.MediatR.CQRS.Commands
{
    public interface ICommand : IRequest<Unit>
    {
    }
}
