namespace WeCount.Application.DTOs.Budget
{
    public record BudgetDto(
        Guid Id,
        CategoryDto Category,
        decimal Amount,
        decimal Spent,
        DateTime StartDate,
        DateTime EndDate,
        Guid CoupleId
    );
}
