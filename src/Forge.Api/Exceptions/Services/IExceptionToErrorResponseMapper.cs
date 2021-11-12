using System;

namespace Forge.Api.Exceptions.Services
{
    public interface IExceptionToErrorResponseMapper
    {
        ErrorResponse Map(Exception exception);
    }
}
