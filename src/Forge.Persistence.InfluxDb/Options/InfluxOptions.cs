using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forge.Persistence.InfluxDb.Options
{
    public sealed class InfluxOptions : IInfluxOptions
    {
        private const string _http = "http";
        private const string _https = "https";
        public const string SectionName = "InfluxConnection";

        public string Ip { get; set; }
        public int Port { get; set; }
        public bool IsHttps { get; set; }

        public string Token { get; set; }
        public string Organization { get; set; }
        public string[] Buckets { get; set; }

        public string GetAddress() => $"{(IsHttps ? _https : _http)}://{Ip}:{Port}";
    }
}
