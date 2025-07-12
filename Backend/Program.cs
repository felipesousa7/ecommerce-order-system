using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Backend.Configurations;
using Backend.Middleware;
using Backend.Models.Entities;
using Backend.Repositories;
using Backend.Services;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.Security.Claims;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configuração do banco de dados
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração do JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            RoleClaimType = ClaimTypes.Role
        };
    });

// Register Repositories
builder.Services.AddScoped<IRepository<Product>, Repository<Product>>();
builder.Services.AddScoped<IRepository<Order>, Repository<Order>>();
builder.Services.AddScoped<IRepository<User>, Repository<User>>();

// Register Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IPaymentSimulationService, PaymentSimulationService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order System API", Version = "v1" });

    // Configuração do JWT no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Aplicar migrações e seeds automaticamente
await DbInitializer.InitializeAsync(app.Services);

// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAll");

// Use Exception Middleware
app.UseMiddleware<ExceptionMiddleware>();

// Use JWT Middleware
app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
