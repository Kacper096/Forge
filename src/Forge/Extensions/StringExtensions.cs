using System.Linq;

namespace Forge.Extensions
{
    public static class StringExtensions
    {
        public static string ToSnakeCase(this string @this) => string.Concat(@this.Select((x, i) => i > 0 && char.IsUpper(x) ? $"_{x}" : x.ToString())).ToLower();
    }
}
