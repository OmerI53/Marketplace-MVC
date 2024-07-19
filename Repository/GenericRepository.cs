using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TestMVC.Models;

namespace TestMVC.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _set;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _set = _context.Set<T>();
    }

    public IEnumerable<T>? GetAll()
    {
        return _set.ToList();
    }

    public T? GetById(object? id)
    {
        return _set.Find(id);
    }

    public Task<T> Insert(T t)
    {
        var result = _set.Add(t);
        SaveChanges();
        return Task.FromResult(result.Entity);
    }

    public void Delete(T t)
    {
        _set.Remove(t);
        SaveChanges();
    }

    public void DeleteById(long id)
    {
        var entity = _set.Find(id);
        if (entity != null) _set.Remove(entity);
    }

    public void Update(T t)
    {
        _set.Update(t);
        SaveChanges();
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _set.Where(predicate).ToListAsync();
    }

    public async Task<T> GetByIdWithIncludesAsync(T id, params Expression<Func<T, object>>[] includes)
    {
        var query = _set.AsQueryable();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return (await query.SingleOrDefaultAsync(e => Equals(EF.Property<string>(e, "Id"), id)))!;
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

    public void ExecuteRawSql(string sql, params object[] parameters)
    {
        _context.Database.ExecuteSqlRaw(sql, parameters);
    }

    public DbSet<T> GetSet()
    {
        return _set;
    }

    public AppDbContext GetContext()
    {
        return _context;
    }
}