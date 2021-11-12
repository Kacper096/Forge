using Forge.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;

namespace Forge.Api.Exceptions.Services
{
    internal sealed class ExceptionToErrorResponseMapper : IExceptionToErrorResponseMapper
    {
        public ErrorResponse Map(Exception exception) => exception switch
        {
            DomainException domainEx => new ErrorResponse(new ErrorDetailsResponse(code: GetCode(domainEx), message: domainEx.Message),
                TryGetStatusCode(domainEx) ?? HttpStatusCode.BadRequest),
            AppException appEx => new ErrorResponse(new ErrorDetailsResponse(code: GetCode(appEx), message: appEx.Message),
                TryGetStatusCode(appEx) ?? HttpStatusCode.BadRequest),
            _ => new ErrorResponse(new ErrorDetailsResponse(code: "unexpceted error code", message: "There was unexcpeted error."),
                TryGetStatusCode(exception) ?? HttpStatusCode.BadRequest),
        };

        private static string GetCode(Exception exception) => exception switch
        {
            DomainException domain when !string.IsNullOrWhiteSpace(domain.Code) => domain.Code,
            AppException app when !string.IsNullOrWhiteSpace(app.Code) => app.Code,
            _ => "unexpected error code"
        };

        private static HttpStatusCode? TryGetStatusCode(Exception exception)
        {
            if (exception is null) return null;

            var statusCodeProp = exception.GetType().GetProperty(nameof(ErrorResponse.StatusCode));
            if (statusCodeProp is null
                || statusCodeProp.PropertyType != typeof(HttpStatusCode))
            {
                return null;
            }

            var statusCodeValue = statusCodeProp.GetValue(exception, null);
            return statusCodeValue as HttpStatusCode?;
        }
    }
}