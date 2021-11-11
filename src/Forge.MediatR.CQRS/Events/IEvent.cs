using MediatR;

namespace Forge.MediatR.CQRS.Events
{
    public interface IEvent : IRequest<Unit>
    {
    }
}
