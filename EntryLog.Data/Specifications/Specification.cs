using System.Linq.Expressions;

namespace EntryLog.Data.Specifications
{
    public abstract class Specification<TEntity> : ISpecification<TEntity> where TEntity : class
    {
        public Expression<Func<TEntity, bool>> Expression { get; private set; } = _ => true;

        public void AndAlso(Expression<Func<TEntity, bool>> expression)
        {
            Expression = SpecificationMethods<TEntity>.And(Expression, expression);
        }
    }
}