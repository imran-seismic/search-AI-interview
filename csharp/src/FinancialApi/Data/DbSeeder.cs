using BCrypt.Net;
using FinancialApi.Models;

namespace FinancialApi.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        db.Database.EnsureCreated();

        if (db.Users.Any()) return;

        db.Users.AddRange(
            new User { Email = "admin@corp.com",   Username = "Admin User",   PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password1!"), Role = "ADMIN" },
            new User { Email = "analyst@corp.com", Username = "Analyst User", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password1!"), Role = "ANALYST" },
            new User { Email = "user@corp.com",    Username = "Regular User", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password1!"), Role = "USER" }
        );

        db.FinancialReports.AddRange(
            new FinancialReport { CompanyName = "Acme Corp",   Revenue = 5_230_000,  NetProfit = 1_045_000, Expenses = 4_185_000, AccountNumber = "ACC-0012-3456", ReportDate = "2024-03-31", Period = "Q1 2024" },
            new FinancialReport { CompanyName = "Acme Corp",   Revenue = 6_100_000,  NetProfit = 1_220_000, Expenses = 4_880_000, AccountNumber = "ACC-0012-3456", ReportDate = "2024-06-30", Period = "Q2 2024" },
            new FinancialReport { CompanyName = "Globex Inc",  Revenue = 12_400_000, NetProfit = 2_480_000, Expenses = 9_920_000, AccountNumber = "ACC-7890-1234", ReportDate = "2024-03-31", Period = "Q1 2024" },
            new FinancialReport { CompanyName = "Globex Inc",  Revenue = 11_750_000, NetProfit = 2_350_000, Expenses = 9_400_000, AccountNumber = "ACC-7890-1234", ReportDate = "2024-06-30", Period = "Q2 2024" },
            new FinancialReport { CompanyName = "Initech Ltd", Revenue = 3_890_000,  NetProfit = 778_000,   Expenses = 3_112_000, AccountNumber = "ACC-4567-8901", ReportDate = "2024-03-31", Period = "Q1 2024" },
            new FinancialReport { CompanyName = "Initech Ltd", Revenue = 4_210_000,  NetProfit = 842_000,   Expenses = 3_368_000, AccountNumber = "ACC-4567-8901", ReportDate = "2024-06-30", Period = "Q2 2024" },
            new FinancialReport { CompanyName = "Umbrella Co", Revenue = 8_600_000,  NetProfit = 1_720_000, Expenses = 6_880_000, AccountNumber = "ACC-2345-6789", ReportDate = "2024-03-31", Period = "Q1 2024" },
            new FinancialReport { CompanyName = "Umbrella Co", Revenue = 9_300_000,  NetProfit = 1_860_000, Expenses = 7_440_000, AccountNumber = "ACC-2345-6789", ReportDate = "2024-06-30", Period = "Q2 2024" }
        );

        db.SaveChanges();
    }
}
