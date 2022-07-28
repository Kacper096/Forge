using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Forge.MediatR.CQRS
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCQRS(this IServiceCollection services, params Assembly[] assemblies)
        {
            var requestTypes = GetTypes(assemblies, typeof(IRequest<>));
            var requestHandlerTypes = GetTypes(assemblies, typeof(IRequestHandler<,>));

            RegisterGenericTypes(services, typeof(IRequestHandler<,>), requestHandlerTypes);
            services.AddScoped<IMediator, Mediator>();
            services.AddScoped<ServiceFactory>(p => p.GetService);
            return services;
        }

        private static IEnumerable<TypeInfo> GetTypes(Assembly[] assemblies, Type genericType)
        {
            return assemblies.SelectMany(a => a.DefinedTypes.Where(ti => !ti.IsAbstract && !ti.IsInterface && ti.GetInterfaces().Any(i => i.IsInterface && i.IsGenericType && i.GetGenericTypeDefinition() == genericType)));
        }

        private static void RegisterGenericTypes(IServiceCollection services, Type genericType, IEnumerable<TypeInfo> types, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            foreach(var type in types)
            {
                var genType = genericType;
                var genericArguments = genericType.GetGenericArguments();
                if (genericArguments.Any())
                {
                    var genericInterfaces = type.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType);
                    if (!genericInterfaces.Any())
                    {
                        continue;
                    }
                    var specifiedArguments = genericInterfaces.First().GetGenericArguments();
                    genType = genericType.MakeGenericType(specifiedArguments);
                }
                services.Add(new ServiceDescriptor(genType, type, lifetime));
            }
        }
    }
}
