using System.Linq.Expressions;

namespace EntryLog.Entities.Specifications
{
    public interface ISpecification<TEntity> where TEntity : class
    {
        Expression<Func<TEntity, bool>> Expression { get; }
    }
}