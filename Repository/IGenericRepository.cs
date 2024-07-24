using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using TestMVC.Models;

namespace TestMVC.Repository;

public interface IGenericRepository<T> where T : class
{
    IEnumerable<T>? GetAll();
    T? GetById(object? id);
    Task<T> Insert(T t);
    void Update(T t);
    void Delete(T t);
    void SaveChanges();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    IQueryable<T> ExecuteRawSql(string sql, params object[] parameters);
    DbSet<T> GetSet();
    AppDbContext GetContext();
}