using Forge.Api.Exceptions.Services;
using Forge.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Forge.Api.Exceptions
{
    internal sealed class ErrorMiddleware
    {
        private const string ResponseContentType = "application/json";

        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorMiddleware> _logger;
        private readonly IExceptionToErrorResponseMapper _exceptionMapper;

        public ErrorMiddleware(RequestDelegate next,
                               ILogger<ErrorMiddleware> logger,
                               IExceptionToErrorResponseMapper exceptionMapper)
        {
            _next = next;
            _logger = logger;
            _exceptionMapper = exceptionMapper;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{(!ex.IsDomainOrAppException() ? $"UNSUPPORTED EXCEPTION: {ex.Message}" : ex.Message)}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var errorResponse = _exceptionMapper.Map(exception);
            context.Response.ContentType = ResponseContentType;
            var serializeErrorResponse = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(serializeErrorResponse);
        }
    }
}
