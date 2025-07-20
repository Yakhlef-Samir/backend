using MediatR;
using WeCount.Application.DTOs;
using WeCount.Domain.Enums;

namespace WeCount.Application.Debts.Commands;

public record CreateDebtCommand(
    string Title,
    decimal Amount,
    decimal InterestRate,
    DateTime StartDate,
    DateTime EndDate,
    DebtType Type,
    Guid CoupleId
) : IRequest<DebtDto>;
