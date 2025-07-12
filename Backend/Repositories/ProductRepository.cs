using Backend.Configurations;
using Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class ProductRepository : Repository<Product>
{
    public ProductRepository(DatabaseContext context) : base(context)
    {
    }

    public override async Task<bool> NameExistsAsync(string name)
    {
        return await _dbSet.AnyAsync(p => p.Name.ToLower() == name.ToLower());
    }
} 