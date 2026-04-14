using FinancialApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<FinancialReport> FinancialReports { get; set; }
}
