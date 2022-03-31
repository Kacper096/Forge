namespace Forge.Persistence.Redis.Exceptions
{
    public sealed class EmptyKeyException : Exception
    {
        public static string Code => ExceptionCodes.EmptyKey;

        public EmptyKeyException()
            : base("Key cannot be empty or null")
        { }
    }
}
