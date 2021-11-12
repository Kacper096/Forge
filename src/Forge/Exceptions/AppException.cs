using System;

namespace Forge.Exceptions
{
    public abstract class AppException : Exception
    {
        public abstract string Code { get; }

        protected AppException(string message)
            : base(message)
        { }
    }
}
