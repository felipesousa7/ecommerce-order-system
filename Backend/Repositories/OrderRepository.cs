using Backend.Configurations;
using Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Backend.Repositories;

public class OrderRepository : Repository<Order>
{
    public OrderRepository(DatabaseContext context) : base(context)
    {
    }

    public override async Task<Order?> GetByIdWithIncludesAsync(int id, params Expression<Func<Order, object>>[] includes)
    {
        var query = _dbSet.AsQueryable();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return await query
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public override async Task<IEnumerable<Order>> FindWithIncludesAsync(Expression<Func<Order, bool>> predicate, params Expression<Func<Order, object>>[] includes)
    {
        var query = _dbSet.AsQueryable();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return await query
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .Where(predicate)
            .ToListAsync();
    }
} 