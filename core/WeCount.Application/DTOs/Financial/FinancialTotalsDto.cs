namespace WeCount.Application.DTOs.Financial
{
    public record FinancialTotalsDto(
        decimal TotalIncome,
        decimal TotalExpenses,
        decimal TotalSavings
    );
}
