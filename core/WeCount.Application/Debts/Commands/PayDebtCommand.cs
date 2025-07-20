using MediatR;
using WeCount.Application.DTOs;

namespace WeCount.Application.Debts.Commands;

public record PayDebtCommand(Guid Id, decimal Amount) : IRequest<DebtDto>;
