using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Models.DTOs.Auth;
using Backend.Models.DTOs.User;
using Backend.Models.Entities;
using Backend.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}

public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepository;
    private readonly string _jwtSecret;

    public AuthService(IRepository<User> userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _jwtSecret = configuration["JwtSettings:Secret"] ?? throw new ArgumentNullException("JwtSettings:Secret");
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = request.Role
        };

        await _userRepository.AddAsync(user);
        return new AuthResponse
        {
            Token = GenerateJwtToken(user),
            Email = user.Email,
            Name = user.Name,
            Role = user.Role
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var users = await _userRepository.FindAsync(u => u.Email == request.Email);
        var user = users.FirstOrDefault();

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidOperationException("Email ou senha inválidos.");
        }

        return new AuthResponse
        {
            Token = GenerateJwtToken(user),
            Email = user.Email,
            Name = user.Name,
            Role = user.Role
        };
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddMinutes(60),
            Issuer = "ordersystem",
            Audience = "ordersystem",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
} 