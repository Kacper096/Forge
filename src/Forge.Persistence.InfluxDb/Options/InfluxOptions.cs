namespace Forge.Persistence.InfluxDb.Options;

public sealed class InfluxOptions : IInfluxOptions
{
    private const string _http = "http";
    private const string _https = "https";
    public const string SectionName = "InfluxConnection";

    public string Ip { get; set; } = string.Empty;
    public int Port { get; set; }
    public bool IsHttps { get; set; }

    public string Token { get; set; } = string.Empty;
    public string Organization { get; set; } = string.Empty;
    public string[] Buckets { get; set; } = Array.Empty<string>();

    public string GetAddress() => $"{(IsHttps ? _https : _http)}://{Ip}:{Port}";
}
