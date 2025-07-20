using System.Collections.Generic;

namespace WeCount.Application.DTOs.Analytics
{
    public record MonthlyFinancialsDto(
        string Month,
        decimal Income,
        decimal Expenses,
        decimal Savings
    );

    public record FinancialsDto(
        List<MonthlyFinancialsDto> MonthlyData,
        decimal TotalIncome,
        decimal TotalExpenses,
        decimal TotalSavings,
        decimal AverageMonthlyIncome,
        decimal AverageMonthlyExpenses,
        decimal AverageMonthlySavings
    );
}
