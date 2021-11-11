using MediatR;

namespace Forge.MediatR.CQRS.Queries
{
    public interface IQuery<out T> : IRequest<T>
    {
    }
}
