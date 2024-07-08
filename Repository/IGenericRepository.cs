namespace TestMVC.Repository;

public interface IGenericRepository<T> where T : class
{
    IEnumerable<T?> GetAll();
    T? GetById(long id);
    Task<T> Insert(T t);
    void DeleteById(long id);
    void Update(T t);
    void SaveChanges();
}