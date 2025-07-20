using WeCount.Domain.Enums;

namespace WeCount.Application.DTOs.Transaction
{
    public record TransactionDto(
        Guid Id,
        decimal Amount,
        string Description,
        CategoryDto Category,
        DateTime Date,
        Guid UserId,
        Guid CoupleId,
        TransactionType Type
    );
}
