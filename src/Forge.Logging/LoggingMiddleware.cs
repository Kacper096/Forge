using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forge.Logging
{
    internal class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();
            var query = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : null;
            var headers = context.Request.Headers.Select(x => $" {x.Key}: {x.Value}");
            var body = context.Request?.Body;

            string readedBody = null;
            if (body != null)
            {
                using var stream = new StreamReader(body, encoding: Encoding.UTF8, leaveOpen: true);
                readedBody = await stream.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            var messageBuilder = new StringBuilder($"[REQUEST CATCHED]{Environment.NewLine}");
            messageBuilder.AppendLine($"HEADERS: {Environment.NewLine}{string.Join(Environment.NewLine, headers)}");
            messageBuilder.AppendLine($"QUERY: {query ?? "EMPTY"}");
            messageBuilder.AppendLine($"BODY: {(string.IsNullOrEmpty(readedBody) ? "EMPTY" : $"{Environment.NewLine}{readedBody}")}");
            _logger.LogTrace(messageBuilder.ToString());

            await _next(context);
        }
    }
}
