using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FinancialApi.Tests;

/// <summary>
/// Generates JWT tokens for integration tests using the same secret and
/// claim structure as AuthService and JwtMiddleware.
/// </summary>
public static class TestHelpers
{
    // Must match Jwt:Secret in appsettings.json
    public const string TestJwtSecret =
        "change-me-in-production-use-a-long-random-string-here-min-32-chars";

    // Seeded user IDs — match DbSeeder insertion order
    private static readonly Dictionary<string, (int Id, string Email)> Users = new()
    {
        ["ADMIN"]   = (1, "admin@corp.com"),
        ["ANALYST"] = (2, "analyst@corp.com"),
        ["USER"]    = (3, "user@corp.com"),
    };

    public static string GetToken(string role)
    {
        var (id, email) = Users[role];
        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestJwtSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("sub",   id.ToString()),
            new Claim("role",  role),
            new Claim("email", email),
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
