namespace Forge.Persistence.Redis.Exceptions
{
    public sealed class EmptyValueException : Exception
    {
        public static string Code => ExceptionCodes.EmptyValue;

        public EmptyValueException()
            : base("Value cannot be empty or null")
        { }
    }
}
