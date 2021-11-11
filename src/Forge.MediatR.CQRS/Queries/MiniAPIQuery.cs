using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace Forge.MediatR.CQRS.Queries
{
    public abstract class MiniAPIQuery<TQuery, TResult> : IQuery<TResult>
        where TQuery : MiniAPIQuery<TQuery, TResult>
    {
        public static ValueTask<TQuery> BindAsync(HttpContext httpContext, ParameterInfo parameter)
        {
            var instance = Activator.CreateInstance(typeof(TQuery)) as TQuery;
            var properties = typeof(TQuery).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead && p.CanWrite);
            foreach (var property in properties)
            {
                var propertyName = property.Name.ToLowerInvariant();
                if (!httpContext.Request.Query.Any(q => q.Key == propertyName))
                {
                    continue;
                }

                var propertyType = property.PropertyType;
                var queryValue = httpContext.Request.Query[propertyName].ToString();
                var propertyValue = propertyType == typeof(string) ? queryValue : Convert.ChangeType(queryValue, property.PropertyType);
                if (propertyValue is null)
                {
                    continue;
                }

                property.SetValue(instance, propertyValue, null);
            }

            return ValueTask.FromResult(instance);
        }
    }
}
