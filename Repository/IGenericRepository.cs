using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TestMVC.Data;
using TestMVC.Models;

namespace TestMVC.Repository;

public interface IGenericRepository<T> where T : class
{
    IEnumerable<T>? GetAll();
    T? GetById(object? id);
    Task<T> Insert(T t);
    void Update(T t);
    void Delete(T t);
    void DeleteById(long id);
    void SaveChanges();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T> GetByIdWithIncludesAsync(string? id, params Expression<Func<T, object>>[] includes);
    Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);
    Task<List<TResult>> GetCustomAsync<TResult>(Expression<Func<T, TResult>> selector);
    void ExecuteRawSql(string sql, params object[] parameters);
    public DbSet<T> GetSet();
    AppDbContext GetContext();
}