using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Venice.Orders.IntegrationTests.Helpers;

public class TokenBuilder
{
    private string _secretKey = "TestSecretKey_ForIntegrationTests_Minimum32Characters";
    private string _issuer = "VeniceOrders";
    private string _audience = "VeniceOrders";
    private int _expirationMinutes = 60;
    private string _username = "testuser";
    private string _userId = "";

    public TokenBuilder WithSecretKey(string secretKey)
    {
        _secretKey = secretKey;
        return this;
    }

    public TokenBuilder WithIssuer(string issuer)
    {
        _issuer = issuer;
        return this;
    }

    public TokenBuilder WithAudience(string audience)
    {
        _audience = audience;
        return this;
    }

    public TokenBuilder WithExpirationMinutes(int minutes)
    {
        _expirationMinutes = minutes;
        return this;
    }

    public TokenBuilder WithUsername(string username)
    {
        _username = username;
        return this;
    }

    public TokenBuilder WithUserId(string userId)
    {
        _userId = userId;
        return this;
    }

    public string Build()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, _username)
        };

        if (!string.IsNullOrEmpty(_userId))
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, _userId));
        }
        else
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expirationMinutes),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

