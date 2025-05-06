using System.Linq.Expressions;
using ExpenseManager.Base;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.Api.Impl.GenericRepository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly ExpenseManagerDbContext dbContext;

    public GenericRepository(ExpenseManagerDbContext dbContext)
    {
        this.dbContext = dbContext;
    }


    public async Task<TEntity> AddAsync(TEntity entity , CancellationToken cancellationToken)
    {
        await dbContext.Set<TEntity>().AddAsync(entity , cancellationToken);
        return entity;
    }

    public async Task DeleteByIdAsync(long id)
    {
        var entity = await dbContext.Set<TEntity>().FindAsync(id);
        if (entity != null)
        {
            dbContext.Set<TEntity>().Remove(entity);
        }
    }

    public void Delete(TEntity entity)
    {
        dbContext.Set<TEntity>().Remove(entity);
    }

    public async Task<List<TEntity>> GetAllAsync(params string[] includes)
    {
        var query = dbContext.Set<TEntity>().AsQueryable();
        query = includes.Aggregate(query, (current, inc) => EntityFrameworkQueryableExtensions.Include(current, inc));
        return await EntityFrameworkQueryableExtensions.ToListAsync(query);
    }

    public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, params string[] includes)
    {
        var query = dbContext.Set<TEntity>().Where(predicate).AsQueryable();
        query = includes.Aggregate(query, (current, inc) => EntityFrameworkQueryableExtensions.Include(current, inc));
        return await EntityFrameworkQueryableExtensions.ToListAsync(query);
    }

    public async Task<TEntity> GetByIdAsync(long id, params string[] includes)
    {
        var query = dbContext.Set<TEntity>().AsQueryable();
        query = includes.Aggregate(query, (current, inc) => EntityFrameworkQueryableExtensions.Include(current, inc));
        return await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query, x => x.Id == id);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }


    public void Update(TEntity entity)
    {
        dbContext.Set<TEntity>().Update(entity);
    }

    public async Task<List<TEntity>> Where(Expression<Func<TEntity, bool>> predicate, params string[] includes)
    {
        var query = dbContext.Set<TEntity>().Where(predicate).AsQueryable();
        query = includes.Aggregate(query, (current, inc) => EntityFrameworkQueryableExtensions.Include(current, inc));
        return await EntityFrameworkQueryableExtensions.ToListAsync(query);
    }
    
    public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params string[] includes)
    {
        var query = dbContext.Set<TEntity>().Where(predicate).AsQueryable();
        query = includes.Aggregate(query, (current, inc) => current.Include(inc));
        return await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query);
    }
    
    public async Task<decimal> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, decimal>> selector)
    {
        return await dbContext.Set<TEntity>()
            .Where(predicate)
            .SumAsync(selector);
    }
    
    public async Task<TEntity> GetWithIncludesAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = dbContext.Set<TEntity>();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(predicate, cancellationToken);
    }


}
