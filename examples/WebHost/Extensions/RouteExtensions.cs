using Forge.MediatR.CQRS.Commands;
using Forge.MediatR.CQRS.Queries;
using MediatR;

namespace Forge.WebHost.Bootstrap;

public static class RouteExtensions
{
    public static IEndpointRouteBuilder Get<TQuery, TResponse>(this IEndpointRouteBuilder route, string query, Delegate? overrideHandler = null)
        where TQuery : MiniAPIQuery<TQuery, TResponse>
    {
        route.MapGet(query, overrideHandler ?? (async (IMediator mediator, TQuery queryParam) =>
        {
            var response = await mediator.Send(queryParam);
            return response;
        }));

        return route;
    }

    public static IEndpointRouteBuilder Post<TCommand>(this IEndpointRouteBuilder route, string query, Delegate? overrideHandler = null)
        where TCommand : ICommand
    {
        route.MapPost(query, overrideHandler ?? (async (IMediator mediator, TCommand command) => {
            var response = await mediator.Send(command);
            return response;
        }));

        return route;
    }
}
