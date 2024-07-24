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
    
    public IQueryable<T> ExecuteRawSql(string sql, params object[] parameters)
    {
        
        return _set.FromSqlRaw(sql, parameters);
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