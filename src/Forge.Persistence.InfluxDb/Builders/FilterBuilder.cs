using Forge.Extensions;
using Forge.Persistence.InfluxDb.Models;
using InfluxDB.Client.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace Forge.Persistence.InfluxDb.Builders
{
    public sealed class FilterBuilder
    {
        private const string EqualOperator = "==";
        private const string NotEqualOperator = "!=";
        private const string GreaterThanOperator = ">";
        private const string GreaterThanOrEqualOperator = ">=";
        private const string LessThanOperator = "<";
        private const string LessThanOrEqualOperator = "<=";
        private const string AndOperator = "and";
        private const string OrOperator = "or";

        private const string FilterContentTemplate = "fn: (r) => ";

        private readonly List<Func<string>> _parameters = new();

        internal Func<string> FilterContent;

        public FilterBuilder()
        {
            _parameters.Add(() => FilterContentTemplate);
            FilterContent = () =>
            {
                string content = null;
                _parameters.ForEach(p =>
                {
                    var partOfContent = p.Invoke();
                    var cutPartOfContent = partOfContent.Replace(AndOperator, null).Replace(OrOperator, null).Trim();
                    if (content?.Contains(cutPartOfContent) ?? false)
                    {
                        return;
                    }
                    content += partOfContent;
                });
                return content;
            };
        }

        public FilterBuilder Init(Expression<Func<FilterParameter, bool>> expression)
        {
            _parameters.Add(GetContentFromFilterParameter(null, expression));

            return this;
        }

        public FilterBuilder Init<TMeasure>(Expression<Func<TMeasure, bool>> expression, bool disableMeasurementCondition = false)
        {
            _parameters.Add(GetContentFromMeasure<TMeasure>(null, expression));
            if (!disableMeasurementCondition)
            {
                ApplyMeasurementCondition(typeof(TMeasure));
            }
            return this;
        }

        public FilterBuilder And(Expression<Func<FilterParameter, bool>> expression)
        {
            _parameters.Add(GetContentFromFilterParameter(AndOperator, expression));

            return this;
        }

        public FilterBuilder And<TMeasure>(Expression<Func<TMeasure, bool>> expression, bool disableMeasurementCondition = false)
        {
            _parameters.Add(GetContentFromMeasure<TMeasure>(AndOperator, expression));
            if (!disableMeasurementCondition)
            {
                ApplyMeasurementCondition(typeof(TMeasure));
            }

            return this;
        }

        public FilterBuilder Or(Expression<Func<FilterParameter, bool>> expression)
        {
            _parameters.Add(GetContentFromFilterParameter(OrOperator, expression));

            return this;
        }

        public FilterBuilder Or<TMeasure>(Expression<Func<TMeasure, bool>> expression, bool disableMeasurementCondition = false)
        {
            _parameters.Add(GetContentFromMeasure(OrOperator, expression));
            if (!disableMeasurementCondition)
            {
                ApplyMeasurementCondition(typeof(TMeasure));
            }

            return this;
        }

        private void ApplyMeasurementCondition(Type measureType)
        {
            var measurementName = measureType.GetCustomAttribute<Measurement>()?.Name;
            _parameters.Add(GetContentFromFilterParameter(AndOperator, fp => fp.Measurement == measurementName));
        }

        private Func<string> GetContentFromFilterParameter(string @operator, Expression<Func<FilterParameter, bool>> expression)
            => () =>
            {
                (string ParameterName, string Operator, string ParameterValue) = GetParamsFromExpression(expression);
                return BuildContent(@operator, ParameterName, Operator, ParameterValue);
            };

        private Func<string> GetContentFromMeasure<TMeasure>(string @operator, Expression<Func<TMeasure, bool>> expression)
            => () =>
            {
                (string PropertyName, string Operator, string Parameter) = GetParamsFromExpression(expression);

                if (string.IsNullOrEmpty(PropertyName))
                {
                    return null;
                }

                var measureType = typeof(TMeasure);
                var propertyName = string.Concat(PropertyName[0].ToString().ToUpper(), PropertyName[1..]);
                var columnAttribute = measureType.GetProperty(propertyName)?.GetCustomAttribute<Column>(inherit: true);
                return BuildContent(@operator, PropertyName, Operator, Parameter, isTagParameterName: columnAttribute.IsTag);
            };

        private static string GetOperator(ExpressionType expressionType) => expressionType switch
        {
            ExpressionType.Equal => EqualOperator,
            ExpressionType.NotEqual => NotEqualOperator,
            ExpressionType.GreaterThan => GreaterThanOperator,
            ExpressionType.GreaterThanOrEqual => GreaterThanOrEqualOperator,
            ExpressionType.LessThan => LessThanOperator,
            ExpressionType.LessThanOrEqual => LessThanOrEqualOperator,
            _ => string.Empty
        };

        private string BuildContent(string chainOperator, string parameterName, string conditionOperator, string parameterValue, bool isTagParameterName = false)
        {
            chainOperator = string.IsNullOrEmpty(chainOperator) ? "" : $" {chainOperator} ";
            if (!isTagParameterName)
            {
                parameterName = $"_{parameterName}";
            }
            //check the parameter value is numeric value which comes from expression
            var paramValue = double.TryParse(parameterValue, out var numParamValue) ? numParamValue.ToString() : $@"""{parameterValue}""";
            var buildedContent = $@"{chainOperator}r.{parameterName} {conditionOperator} " + paramValue;
            return buildedContent;
        }

        private (string PropertyName, string Operator, string ParameterValue) GetParamsFromExpression<TMeasure>(Expression<Func<TMeasure, bool>> expression)
        {
            if (expression.Body is BinaryExpression binaryExpression)
            {
                return (PropertyName: binaryExpression.Left.GetMemberName(),
                        Operator: GetOperator(binaryExpression.NodeType),
                        ParameterValue: binaryExpression.Right.GetValue());
            }

            if (expression.Body is MethodCallExpression methodCallExpression && (methodCallExpression.NodeType == ExpressionType.NotEqual || methodCallExpression.NodeType == ExpressionType.Equal) && methodCallExpression.Arguments.Count == 1)
            {
                return (PropertyName: methodCallExpression.Object.GetMemberName(),
                        Operator: GetOperator(methodCallExpression.NodeType),
                        ParameterValue: methodCallExpression.Arguments[0].GetValue());
            }
            return (PropertyName: null, Operator: null, ParameterValue: null);
        }
    }
}
