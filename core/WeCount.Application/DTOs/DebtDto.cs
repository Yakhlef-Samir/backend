using System;
using WeCount.Domain.Enums;

namespace WeCount.Application.DTOs
{
    public record DebtDto(
        Guid Id,
        string Title,
        decimal Amount,
        decimal PaidAmount,
        decimal InterestRate,
        DateTime StartDate,
        DateTime EndDate,
        DebtType Type,
        PaymentStatus Status,
        Guid CoupleId
    );
}
