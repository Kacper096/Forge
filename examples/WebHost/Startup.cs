using Forge.Api;
using Forge.MediatR.CQRS;
using Forge.MessageBroker.RabbitMQ;
using Forge.Persistence.InfluxDb;
using Forge.Persistence.Redis;
using Forge.WebHost.Bootstrap;

namespace Forge.WebHost;

public static class Startup
{
    public static WebApplication CreateHost(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;
        builder.WebHost.ConfigureServices(services => services.RegisterServices(configuration));
        var app = builder.Build();
        app.UseServices(app.Environment);
        return app;
    }

    private static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.UseCQRS();
        services.AddErrorHandler();
        services.AddInfluxDb(configuration);
        services.AddRedis(configuration);
        services.AddMemoryCache();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }

    private static IApplicationBuilder UseServices(this IApplicationBuilder builder, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            builder.UseSwagger();
            builder.UseSwaggerUI();
        }

        builder.UseRabbitMq(MessageBrokerBootstrapper.ConfigureSubscribeMessages, MessageBrokerBootstrapper.ConfigurePublishMessages);
        builder.UseErrorHandler();
        builder.UseRequestLocalization();
        //app.UseLoggingMiddleware();
        builder.UseAuthorization();

        builder.UseEndpoints(endpoints => EndpointRouteBuilderBootstrapper.Bootstrap(endpoints));
        return builder;
    }
}
