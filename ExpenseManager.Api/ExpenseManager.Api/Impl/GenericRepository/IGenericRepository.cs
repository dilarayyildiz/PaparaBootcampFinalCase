using System.Linq.Expressions;
using ExpenseManager.Base;

namespace ExpenseManager.Api.Impl.GenericRepository;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<TEntity> GetByIdAsync(long id, params string[] includes);
    Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params string[] includes);

    Task<List<TEntity>> GetAllAsync(params string[] includes);
    Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, params string[] includes);
    Task<List<TEntity>> Where(Expression<Func<TEntity, bool>> predicate, params string[] includes);
    Task<TEntity> AddAsync(TEntity entity , CancellationToken cancellationToken);
    void Update(TEntity entity);
    Task<decimal> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, decimal>> selector);
    Task<TEntity> GetWithIncludesAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includes);

    void Delete(TEntity entity);
    Task DeleteByIdAsync(long id);
}
