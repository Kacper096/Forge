namespace Forge.Api.Exceptions
{
    public class ErrorDetailsResponse
    {
        public string Code { get; }
        public string Message { get; }

        public ErrorDetailsResponse(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
