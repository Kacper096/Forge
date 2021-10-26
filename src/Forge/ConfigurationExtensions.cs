using Microsoft.Extensions.Configuration;

namespace Forge
{
    public static class ConfigurationExtensions
    {
        public static TConfModel GetOptions<TConfModel>(this IConfiguration configuration, string sectionName)
            where TConfModel : new() => configuration.GetSection(sectionName).Get<TConfModel>();
    }
}
