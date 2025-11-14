using System.ComponentModel.DataAnnotations;

namespace Venice.Orders.Api.Models;

/// <summary>
/// Request DTO para login
/// </summary>
public class LoginRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
}

