namespace WeCount.Application.DTOs.Couple
{
    public record CoupleScoreDto(
        int Score,
        int BudgetScore,
        int GoalsScore,
        int DebtScore,
        int SavingsScore,
        int TransactionsScore,
        string Message
    );
}
