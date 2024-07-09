using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace TestMVC.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly Context _context;
    private readonly DbSet<T> _set;

    public GenericRepository(Context context)
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

    public async Task<T> Insert(T t)
    {
        var result = _set.Add(t);
        SaveChanges();
        return result.Entity;
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
        IQueryable<T> query = _set;
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return await query.SingleOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }

    public async Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _set;
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return await query.ToListAsync();
    }
    
    public async Task<List<TResult>> GetCustomAsync<TResult>(Expression<Func<T, TResult>> selector)
    {
        return await _set.Select(selector).ToListAsync();
    }
    
}