namespace FinancialApi.Models;

public class FinancialReport
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public decimal NetProfit { get; set; }
    public decimal Expenses { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public string ReportDate { get; set; } = string.Empty;  // ISO date YYYY-MM-DD
    public string Period { get; set; } = string.Empty;      // e.g. "Q1 2024"
}
