using System.Linq.Expressions;

namespace Backend.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> FindWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task<bool> ExistsAsync(int id);
    Task<bool> NameExistsAsync(string name);
} 