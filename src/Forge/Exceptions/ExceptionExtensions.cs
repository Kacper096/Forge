using System;

namespace Forge.Exceptions
{
    public static class ExceptionExtensions
    {
        public static bool IsDomainException(this Exception exception) => exception is DomainException;
        public static bool IsAppException(this Exception exception) => exception is AppException;
        public static bool IsDomainOrAppException(this Exception exception) => IsDomainException(exception) || IsAppException(exception);
    }
}
