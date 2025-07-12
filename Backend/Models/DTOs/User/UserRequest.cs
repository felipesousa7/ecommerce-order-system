using System.ComponentModel.DataAnnotations;

namespace Backend.Models.DTOs.User;

public class CreateUserRequest
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class UpdateUserRequest
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
} 