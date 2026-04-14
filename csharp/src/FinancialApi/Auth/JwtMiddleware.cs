// ---------------------------------------------------------------
// EXISTING CODE — Do not modify this file.
//
// This middleware validates the JWT on every incoming request and
// populates HttpContext.User with the authenticated user's claims,
// including their Role.
//
// Downstream code (controllers) reads the role via:
//   User.IsInRole("ADMIN")
//   User.FindFirst(ClaimTypes.Role)?.Value
//
// Do NOT add a second JWT validation step or create new auth
// middleware — this file already handles authentication.
// ---------------------------------------------------------------

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FinancialApi.Auth;

public class JwtMiddleware(RequestDelegate next, IConfiguration config)
{
    private readonly string _secret = config["Jwt:Secret"]
        ?? throw new InvalidOperationException("Jwt:Secret is not configured.");

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers.Authorization
            .FirstOrDefault()
            ?.Split(' ')
            .Last();

        if (token is not null)
            AttachUserToContext(context, token);

        await next(context);
    }

    private void AttachUserToContext(HttpContext context, string token)
    {
        try
        {
            var key = Encoding.UTF8.GetBytes(_secret);
            var handler = new JwtSecurityTokenHandler();

            handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
            }, out var validatedToken);

            var jwt = (JwtSecurityToken)validatedToken;
            var userId = jwt.Claims.First(c => c.Type == "sub").Value;
            var role   = jwt.Claims.First(c => c.Type == "role").Value;
            var email  = jwt.Claims.First(c => c.Type == "email").Value;

            // Populate HttpContext.User — controllers read role from here
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role,            role),
                new Claim(ClaimTypes.Email,           email),
            };
            context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        }
        catch
        {
            // Invalid token — HttpContext.User remains unauthenticated
        }
    }
}
