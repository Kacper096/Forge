using Forge.Logging.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace Forge.Logging
{
    public static class ServiceCollectionExtensions
    {
        public static IHostBuilder UseLogging(this IHostBuilder hostBuilder,
            Action<HostBuilderContext, LoggerConfiguration> configure = null,
            string appSectionName = ApplicationOptions.DefaultSectionName,
            string loggerSectionName = LoggingOptions.DefaultSectionName) 
            => 
            hostBuilder.UseSerilog((context, configuration) =>
            {
                if (string.IsNullOrWhiteSpace(appSectionName)) appSectionName = ApplicationOptions.DefaultSectionName;
                if (string.IsNullOrWhiteSpace(loggerSectionName)) loggerSectionName = LoggingOptions.DefaultSectionName;

                var appOptions = context.Configuration.GetOptions<ApplicationOptions>(appSectionName);
                var loggingOptions = context.Configuration.GetOptions<LoggingOptions>(loggerSectionName);
                                                                          
                SetLoggerConfiguration(loggingOptions, configuration, appOptions);
                configure?.Invoke(context, configuration);
            });

        public static IWebHostBuilder UseLogging(this IWebHostBuilder webHostBuilder,
            Action<WebHostBuilderContext, LoggerConfiguration> configure = null,
            string appSectionName = ApplicationOptions.DefaultSectionName,
            string loggerSectionName = LoggingOptions.DefaultSectionName)
            =>
            webHostBuilder.UseSerilog((context, configuration) =>
            {
                if (string.IsNullOrWhiteSpace(appSectionName)) appSectionName = ApplicationOptions.DefaultSectionName;
                if (string.IsNullOrWhiteSpace(loggerSectionName)) loggerSectionName = LoggingOptions.DefaultSectionName;

                var appOptions = context.Configuration.GetOptions<ApplicationOptions>(appSectionName);
                var loggingOptions = context.Configuration.GetOptions<LoggingOptions>(loggerSectionName);

                SetLoggerConfiguration(loggingOptions, configuration, appOptions);
                configure?.Invoke(context, configuration);
            });

        private static void SetLoggerConfiguration(LoggingOptions loggingOptions, LoggerConfiguration configuration,
            ApplicationOptions appOptions)
        {
            var logLevel = GetLogLevel(loggingOptions.Level);

            configuration
                .Enrich.FromLogContext().MinimumLevel.Is(logLevel)
                .Enrich.WithProperty("MachineName", Environment.MachineName)
                .Enrich.WithProperty("ApplicationName", appOptions.Name)
                .Enrich.WithProperty("Version", appOptions.Version);

            ConfigureLoggerOptions(loggingOptions, configuration);
        }

        private static void ConfigureLoggerOptions(LoggingOptions loggingOptions, LoggerConfiguration loggerConfiguration)
        {
            if (loggingOptions.Console?.Enabled ?? false)
            {
                loggerConfiguration.WriteTo.Console();
            }
        }

        public static LogEventLevel GetLogLevel(string level)
            => Enum.TryParse<LogEventLevel>(level, ignoreCase: true, out LogEventLevel logEventLevel)
            ? logEventLevel
            : LogEventLevel.Information;
    }
}
