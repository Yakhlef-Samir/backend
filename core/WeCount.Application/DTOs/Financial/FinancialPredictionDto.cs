namespace WeCount.Application.DTOs.Financial
{
    public record FinancialPredictionDto(
        string Month,
        decimal Income,
        decimal Expenses,
        decimal Savings
    );
}
