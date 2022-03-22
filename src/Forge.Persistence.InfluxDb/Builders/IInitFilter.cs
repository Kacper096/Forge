using Forge.Persistence.InfluxDb.Models;
using System.Linq.Expressions;

namespace Forge.Persistence.InfluxDb.Builders
{
    public interface IInitFilter
    {
        IFilter Init(Expression<Func<FilterParameter, bool>> expression);
        IFilter Init<TMeasure>(Expression<Func<TMeasure, bool>> expression, bool disableMeasurementCondition = false);
    }
}
