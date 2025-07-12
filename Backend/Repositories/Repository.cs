using Backend.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Backend.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DatabaseContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(DatabaseContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes)
    {
        var query = _dbSet.AsQueryable();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes)
    {
        var query = _dbSet.AsQueryable();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return await query.ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> FindWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
        var query = _dbSet.AsQueryable();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return await query.Where(predicate).ToListAsync();
    }

    public virtual async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();
    }

    public virtual void Remove(T entity)
    {
        _dbSet.Remove(entity);
        _context.SaveChanges();
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        return await _dbSet.FindAsync(id) != null;
    }

    public virtual async Task<bool> NameExistsAsync(string name)
    {
        // Este método será sobrescrito nas classes específicas
        throw new NotImplementedException("Este método deve ser implementado nas classes específicas");
    }
} 