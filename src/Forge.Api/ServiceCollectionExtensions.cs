using Forge.Api.Exceptions;
using Forge.Api.Exceptions.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Forge.Api
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddErrorHandler(this IServiceCollection services) => services.AddSingleton<IExceptionToErrorResponseMapper, ExceptionToErrorResponseMapper>();
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder app) => app.UseMiddleware<ErrorMiddleware>();
    }
}
