namespace Forge.Persistence.InfluxDb.Exceptions
{
    public class EmptyBucketException : Exception
    {
        public static string Code => ExceptionCodes.EmptyBucket;

        public EmptyBucketException()
            : base("Bucket cannot be null or empty.")
        {

        }
    }
}
