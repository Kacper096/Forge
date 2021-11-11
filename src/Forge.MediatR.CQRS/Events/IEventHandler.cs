using MediatR;

namespace Forge.MediatR.CQRS.Events
{
    public interface IEventHandler<in TEvent> : IRequestHandler<TEvent, Unit>
        where TEvent : IEvent
    {
    }
}
