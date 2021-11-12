using System.Net;

namespace Forge.Api.Exceptions
{
    public class ErrorResponse
    {
        public ErrorDetailsResponse Response { get; }
        public HttpStatusCode StatusCode { get; }

        public ErrorResponse(ErrorDetailsResponse response, HttpStatusCode statusCode)
        {
            Response = response;
            StatusCode = statusCode;
        }
    }
}
