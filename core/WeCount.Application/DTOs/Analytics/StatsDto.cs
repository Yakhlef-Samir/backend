namespace WeCount.Application.DTOs.Analytics
{
    public record StatsDto(
        decimal TotalBalance,
        decimal MonthlyIncome,
        decimal MonthlyExpenses,
        decimal SavingsRate,
        int CompletedGoals,
        int ActiveGoals,
        int PaidDebts,
        int ActiveDebts,
        decimal DebtToIncomeRatio
    );
}
