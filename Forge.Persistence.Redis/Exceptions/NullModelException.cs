namespace Forge.Persistence.Redis.Exceptions
{
    public class NullModelException : Exception
    {
        public static string Code => ExceptionCodes.NullModel;
        
        public NullModelException()
            : base("Model cannot be null")
        { }
    }
}
