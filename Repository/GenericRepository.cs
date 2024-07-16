using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TestMVC.Data;

namespace TestMVC.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _set;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _set = _context.Set<T>();
    }

    public IEnumerable<T?> GetAll()
    {
        return _set.ToList();
    }

    public T? GetById(long id)
    {
        return _set.Find(id);
    }

    public Task<T> Insert(T t)
    {
        var result = _set.Add(t);
        SaveChanges();
        return Task.FromResult(result.Entity);
    }

    public void DeleteById(long id)
    {
        var entity = _set.Find(id);
        if (entity != null) _set.Remove(entity);
    }

    public void Update(T t)
    {
        _set.Update(t);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _set.Where(predicate).ToListAsync();
    }

    public async Task<T> GetByIdWithIncludesAsync(long id, params Expression<Func<T, object>>[] includes)
    {
        var query = includes.Aggregate<Expression<Func<T, object>>?, IQueryable<T>>(_set, (current, include) =>
        {
            Debug.Assert(include != null, nameof(include) + " != null");
            return current.Include(include);
        });

        return (await query.SingleOrDefaultAsync(e => EF.Property<int>(e, "Id") == id))!;
    }

    public async Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes)
    {
        var query = includes.Aggregate<Expression<Func<T, object>>?, IQueryable<T>>(_set, (current, include) =>
        {
            Debug.Assert(include != null, nameof(include) + " != null");
            return current.Include(include);
        });

        return await query.ToListAsync();
    }

    public async Task<List<TResult>> GetCustomAsync<TResult>(Expression<Func<T, TResult>> selector)
    {
        return await _set.Select(selector).ToListAsync();
    }
}