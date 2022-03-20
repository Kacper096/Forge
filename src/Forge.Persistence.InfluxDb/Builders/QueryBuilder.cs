using Forge.Persistence.InfluxDb.Exceptions;
using Forge.Persistence.InfluxDb.Models;

namespace Forge.Persistence.InfluxDb.Builders
{
    public sealed class QueryBuilder
    {
        private const string DateTimeFormat = "yyyy-MM-ddTHH:mm:01Z";
        private const string BucketParameter = "#BUCKET";
        private const string StartParameter = "#START";
        private const string StopParameter = "#STOP";
        private const string SelectNameParameter = "#NAME";
        private const string FilterConditionParameter = "#CONDITION";

        private const string FromTemplate = "from(bucket: \"" + BucketParameter + "\")";
        private const string FullRangeTemplate = " |> range(start: " + StartParameter + ", stop: " + StopParameter + ")";
        private const string StartRangeTemplate = " |> range(start: " + StartParameter + ")";
        private const string SelectTemplate = " |> yield(name: \"" + SelectNameParameter + "\")";
        private const string FilterTemplate = " |> filter(" + FilterConditionParameter + ")";

        private string _query;
        private readonly string _bucketName;
        private readonly FilterBuilder _filterBuilder;
        private readonly List<Func<FilterBuilder, string>> _steps;
        private Func<FilterBuilder, string> _latestStep;
        public QueryBuilder(string bucketName)
        {
            _bucketName = bucketName;
            ValidateBucket();
            _filterBuilder = new FilterBuilder();
            _steps = new List<Func<FilterBuilder, string>>()
            {
                _ => FromTemplate.Replace(BucketParameter, _bucketName),
            };
        }

        public QueryBuilder Range(Time start, Time? stop = null)
        {
            if (stop.HasValue)
            {
                _latestStep = _ => FullRangeTemplate.Replace(StartParameter, start.ToString()).Replace(StopParameter, stop.ToString());
                return this;
            }

            _latestStep = _ => StartRangeTemplate.Replace(StartParameter, start.ToString());
            _steps.Add(_latestStep);
            return this;
        }

        public QueryBuilder Range(DateTime start, DateTime? stop = null)
        {
            if (stop.HasValue)
            {
                _latestStep = _ => FullRangeTemplate.Replace(StartParameter, start.ToString(DateTimeFormat)).Replace(StopParameter, stop.Value.ToString(DateTimeFormat));
                return this;
            }

            _latestStep = _ => StartRangeTemplate.Replace(StartParameter, start.ToString(DateTimeFormat));
            _steps.Add(_latestStep);
            return this;
        }

        public QueryBuilder Filter(Action<IInitFilter> filter)
        {
            _latestStep = (filterBuilder) =>
            {
                filter.Invoke(filterBuilder);
                return FilterTemplate.Replace(FilterConditionParameter, filterBuilder.FilterContent.Invoke());
            };
            _steps.Add(_latestStep);

            return this;
        }

        public QueryBuilder Filter<TMeasurement>(Action<IFilter> filter) where TMeasurement : class
        {
            _latestStep = (filterBuilder) =>
            {
                var baseFilter = filterBuilder.BaseInit<TMeasurement>();
                filter.Invoke(baseFilter);
                return FilterTemplate.Replace(FilterConditionParameter, filterBuilder.FilterContent.Invoke());
            };
            _steps.Add(_latestStep);
            return this;
        }

        public QueryBuilder Select(string name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = "_results";
            }

            _latestStep = _ => SelectTemplate.Replace(SelectNameParameter, name);
            _steps.Add(_latestStep);
            return this;
        }

        public string Build()
        {
            _steps.ForEach(step => _query += step.Invoke(_filterBuilder));
            return _query;
        }

        public override string ToString() => Build();

        private void ValidateBucket()
        {
            if (string.IsNullOrWhiteSpace(_bucketName))
            {
                throw new EmptyBucketException();
            }
        }
    }
}
