namespace Backend.Models.DTOs.User;

public class UserResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class LoginResponse
{
    public string Token { get; set; }
    public UserResponse User { get; set; }
} 