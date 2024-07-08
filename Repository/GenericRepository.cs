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
        var result = await _set.AddAsync(t);
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
}