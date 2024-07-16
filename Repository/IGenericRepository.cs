using System.Linq.Expressions;

namespace TestMVC.Repository;

public interface IGenericRepository<T> where T : class
{
    IEnumerable<T>? GetAll();
    T? GetById(long id);
    Task<T> Insert(T t);
    void DeleteById(long id);
    void Update(T t);
    void SaveChanges();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T> GetByIdWithIncludesAsync(long id, params Expression<Func<T, object>>[] includes);
    Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);
    Task<List<TResult>> GetCustomAsync<TResult>(Expression<Func<T, TResult>> selector);
}