# Financial Reports API — C# (ASP.NET Core 8)

## Challenge

This codebase has a working JWT-authenticated API. Your task is to add an **RBAC layer** to the existing `/get-financial-report` endpoint using an AI coding assistant.

**Time budget: 30–45 minutes**

### Requirements

| Role | Expected behaviour |
|---|---|
| `ADMIN` | Full report data — all fields returned as-is |
| `ANALYST` | Masked data — `AccountNumber` shows only the last 4 chars (e.g. `****3456`); `Revenue`, `NetProfit`, `Expenses` rounded to the nearest 1,000 |
| `USER` | HTTP 403 Forbidden |

### Rules

- Implement the RBAC logic inside `src/FinancialApi/Controllers/ReportsController.cs`
- `HttpContext.User` is already populated with the authenticated user's claims (including role) by `JwtMiddleware` — **do not re-validate the JWT or add new auth middleware**
- Use `User.IsInRole("ADMIN")` / `User.IsInRole("ANALYST")` — **do NOT use `[Authorize(Roles="...")]`** (requires built-in auth pipeline not configured here)
- Write unit tests covering all three roles in `tests/FinancialApi.Tests/ReportsControllerTests.cs`

---

## Project structure

```
src/FinancialApi/
├── Program.cs                     # DI setup; SQLite vs InMemory database switch
├── appsettings.json               # JWT secret, DB settings
├── Controllers/
│   ├── AuthController.cs          # POST /auth/login
│   └── ReportsController.cs      # GET /get-financial-report  ← add RBAC here
├── Data/
│   ├── AppDbContext.cs
│   └── DbSeeder.cs                # Seeds mock users + reports on startup
├── Models/
│   ├── User.cs
│   └── FinancialReport.cs
├── Auth/
│   └── JwtMiddleware.cs           # Existing: validates JWT → populates HttpContext.User  ← don't change
└── Services/
    └── AuthService.cs             # Login + token generation
tests/FinancialApi.Tests/
├── ReportsControllerTests.cs      # xUnit scaffold — implement the 3 test stubs
└── TestHelpers.cs                 # Generates test JWTs for each role
```

---

## Setup

```bash
# From the csharp/ directory

# 1. Restore packages and build
dotnet build

# 2. Run the API (SQLite database seeded automatically on first run)
dotnet run --project src/FinancialApi
```

Swagger UI: https://localhost:5001/swagger

---

## Seeded users

| Email | Password | Role |
|---|---|---|
| admin@corp.com | Password1! | ADMIN |
| analyst@corp.com | Password1! | ANALYST |
| user@corp.com | Password1! | USER |

Obtain a token:
```bash
curl -X POST https://localhost:5001/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@corp.com","password":"Password1!"}'
```

Then call the endpoint:
```bash
curl https://localhost:5001/get-financial-report \
  -H "Authorization: Bearer <token>"
```

---

## Running tests

Tests use the EF Core in-memory provider — no SQLite file required.

```bash
dotnet test
```

The three test stubs will throw `NotImplementedException` until you implement them — that is expected.

---

## Evaluation notes (for interviewer)

**Key observation:** `Auth/JwtMiddleware.cs` already validates the JWT and sets `HttpContext.User` with a `ClaimsIdentity` containing the role claim. Watch whether the candidate:

1. **Feeds context to the AI** — shares `JwtMiddleware.cs` and `ReportsController.cs` together so the AI understands how auth is handled
2. **Catches the design trap** — a naive AI response often suggests `[Authorize(Roles="ADMIN")]` (which requires `AddAuthentication()` / `AddJwtBearer()`, not configured here) or attempts to re-decode the JWT; the correct solution uses `User.IsInRole("ADMIN")` directly
3. **Covers all three roles** in tests with realistic assertions
4. **Returns a mapped response DTO** for ANALYST role rather than mutating the EF entity
