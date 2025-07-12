using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Backend.Configurations;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseNpgsql(connectionString);

        return new DatabaseContext(optionsBuilder.Options);
    }
} 