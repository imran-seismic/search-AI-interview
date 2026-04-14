/*
 * Test scaffold for GET /get-financial-report
 * ============================================
 * Your task — implement the body of each test using the AI assistant:
 *
 *   1. Test_Admin_SeesFullData       — ADMIN gets all fields, including the real AccountNumber
 *   2. Test_Analyst_SeesMaskedData   — ANALYST gets masked AccountNumber and rounded figures
 *   3. Test_User_Gets403             — USER is denied with HTTP 403
 *
 * Helpers available:
 *   TestHelpers.GetToken(role)  returns a valid JWT for the seeded user with that role
 *   _client                     HttpClient backed by an in-memory WebApplicationFactory
 *
 * Seeded credentials (password "Password1!" for all — only needed for /auth/login flow):
 *   ADMIN   → admin@corp.com
 *   ANALYST → analyst@corp.com
 *   USER    → user@corp.com
 */

using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace FinancialApi.Tests;

public class ReportsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ReportsControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory
            .WithWebHostBuilder(builder =>
            {
                // Override to use EF Core in-memory database for tests
                builder.UseSetting("Database:Provider", "inmemory");
            })
            .CreateClient();
    }

    private void SetAuthHeader(string role) =>
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", TestHelpers.GetToken(role));

    // -------------------------------------------------------------------------
    // Tests — implement the body of each test using your AI assistant
    // -------------------------------------------------------------------------

    [Fact]
    public async Task Test_Admin_SeesFullData()
    {
        // ADMIN should receive the full report including the real AccountNumber
        SetAuthHeader("ADMIN");
        // TODO: GET /get-financial-report and assert:
        //   - response status is 200
        //   - at least one report is returned
        //   - AccountNumber contains the real value (e.g. "ACC-0012-3456")
        throw new NotImplementedException("Implement this test");
    }

    [Fact]
    public async Task Test_Analyst_SeesMaskedData()
    {
        // ANALYST should receive reports with a masked AccountNumber and rounded figures
        SetAuthHeader("ANALYST");
        // TODO: GET /get-financial-report and assert:
        //   - response status is 200
        //   - AccountNumber does NOT expose the raw value (masked, e.g. "****3456")
        //   - Revenue, NetProfit, and Expenses are each rounded to the nearest 1,000
        throw new NotImplementedException("Implement this test");
    }

    [Fact]
    public async Task Test_User_Gets403()
    {
        // USER role should be denied with HTTP 403
        SetAuthHeader("USER");
        // TODO: GET /get-financial-report and assert HTTP 403
        throw new NotImplementedException("Implement this test");
    }
}
