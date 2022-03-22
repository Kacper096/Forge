using Forge.Persistence.InfluxDb.Models;
using System.Linq.Expressions;

namespace Forge.Persistence.InfluxDb.Builders
{
    public interface IFilter
    {
        IFilter And(Expression<Func<FilterParameter, bool>> expression);
        IFilter And<TMeasure>(Expression<Func<TMeasure, bool>> expression);
        IFilter Or(Expression<Func<FilterParameter, bool>> expression);
        IFilter Or<TMeasure>(Expression<Func<TMeasure, bool>> expression);

    }
}
