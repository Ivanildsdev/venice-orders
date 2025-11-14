namespace Venice.Orders.Api.Models;

/// <summary>
/// Response DTO para login
/// </summary>
public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
}

