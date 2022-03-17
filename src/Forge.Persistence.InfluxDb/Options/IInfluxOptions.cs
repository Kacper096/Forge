namespace Forge.Persistence.InfluxDb.Options
{
    public interface IInfluxOptions
    {
        string[] Buckets { get; set; }
        string Ip { get; set; }
        bool IsHttps { get; set; }
        string Organization { get; set; }
        int Port { get; set; }
        string SectionName { get; }
        string Token { get; set; }

        string GetAddress();
    }
}