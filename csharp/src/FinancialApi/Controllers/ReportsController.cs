using FinancialApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace FinancialApi.Controllers;

[ApiController]
public class ReportsController(AppDbContext db) : ControllerBase
{
    /// <summary>
    /// Returns financial reports for the authenticated user.
    ///
    /// TODO: Implement the RBAC layer using HttpContext.User (already populated by JwtMiddleware):
    ///
    ///   ADMIN   → return full report data as-is
    ///
    ///   ANALYST → return masked data:
    ///               - AccountNumber: mask all but the last 4 characters
    ///                 e.g.  "ACC-0012-3456"  →  "****3456"
    ///               - Revenue, NetProfit, Expenses: round each to the nearest 1,000
    ///
    ///   USER    → return 403 Forbidden
    ///
    /// HINT: Use User.IsInRole("ADMIN") / User.IsInRole("ANALYST") — HttpContext.User is
    ///       already populated by JwtMiddleware; do NOT re-validate the JWT or add new middleware.
    ///
    /// NOTE: Do NOT use [Authorize(Roles="...")] — it requires the built-in ASP.NET Core auth
    ///       pipeline which is not configured here. Use User.IsInRole() in the method body instead.
    /// </summary>
    [HttpGet("/get-financial-report")]
    public IActionResult GetFinancialReport()
    {
        if (User.Identity?.IsAuthenticated != true)
            return Unauthorized();

        var reports = db.FinancialReports.ToList();

        // Replace this with role-based logic
        return Ok(reports);
    }
}
