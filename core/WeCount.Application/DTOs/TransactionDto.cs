namespace WeCount.Application.DTOs;

public record TransactionDto(
    Guid Id,
    string Description,
    decimal Amount,
    DateTime Date,
    string Category,
    Guid UserId,
    string UserName,
    Guid? CoupleId,
    bool IsShared
);

public record TransactionCategoryDto(string Code, string Label);
