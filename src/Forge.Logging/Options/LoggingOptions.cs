namespace Forge.Logging.Options
{
    public class LoggingOptions
    {
        public const string DefaultSectionName = "logger";
        public string Level { get; set; }
        public ConsoleOptions Console { get; set; }
    }
}
