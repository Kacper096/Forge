using System.Linq.Expressions;

namespace Forge.Extensions
{
    public static class ExpressionExtensions
    {
        public static string GetMemberName(this Expression expression)
        {
            if (expression is MemberExpression memberExp)
            {
                return memberExp.Member.Name?.ToLowerInvariant();
            }
            return null;
        }

        public static string GetValue(this Expression expression)
        {
            var constantValue = (expression as ConstantExpression)?.Value as string;
            if (!string.IsNullOrEmpty(constantValue))
            {
                return constantValue;
            }
            return Expression.Lambda(expression).Compile().DynamicInvoke() as string;
        }
    }
}
