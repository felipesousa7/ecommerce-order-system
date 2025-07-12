using Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Configurations;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        // Garante que o banco de dados está criado e migrado
        await context.Database.MigrateAsync();

        // Verifica se já existem produtos
        if (await context.Products.AnyAsync())
        {
            return; // Banco já foi inicializado
        }

        // Adiciona produtos de exemplo
        var products = new List<Product>
        {
            new Product
            {
                Name = "Smartphone XYZ",
                Description = "Smartphone de última geração com câmera de alta resolução",
                Price = 1999.99m,
                IsAvailable = true,
                ImageUrl = "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=400&h=400&fit=crop"
            },
            new Product
            {
                Name = "Notebook Pro",
                Description = "Notebook potente para trabalho e jogos",
                Price = 4999.99m,
                IsAvailable = true,
                ImageUrl = "https://images.unsplash.com/photo-1496181133206-80ce9b88a853?w=400&h=400&fit=crop"
            },
            new Product
            {
                Name = "Smart TV 4K",
                Description = "TV 4K com HDR e sistema operacional integrado",
                Price = 2999.99m,
                IsAvailable = true,
                ImageUrl = "https://images.unsplash.com/photo-1593784991095-a205069470b6?w=400&h=400&fit=crop"
            },
            new Product
            {
                Name = "Fone de Ouvido Bluetooth",
                Description = "Fone sem fio com cancelamento de ruído",
                Price = 499.99m,
                IsAvailable = true,
                ImageUrl = "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=400&h=400&fit=crop"
            },
            new Product
            {
                Name = "Smartwatch",
                Description = "Relógio inteligente com monitor cardíaco",
                Price = 799.99m,
                IsAvailable = true,
                ImageUrl = "https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=400&h=400&fit=crop"
            },
            new Product
            {
                Name = "Tablet Pro",
                Description = "Tablet com tela retina e caneta digital",
                Price = 2499.99m,
                IsAvailable = true,
                ImageUrl = "https://images.unsplash.com/photo-1544244015-0df4b3ffc6b0?w=400&h=400&fit=crop"
            }
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }
} 