using FinancialApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(AuthService authService) : ControllerBase
{
    public record LoginRequest(string Email, string Password);
    public record TokenResponse(string AccessToken, string TokenType);

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest body)
    {
        var user = authService.Authenticate(body.Email, body.Password);
        if (user is null)
            return Unauthorized(new { detail = "Invalid credentials" });

        var token = authService.GenerateToken(user);
        return Ok(new TokenResponse(token, "Bearer"));
    }
}
