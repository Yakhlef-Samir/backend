using MediatR;
using WeCount.Application.DTOs;

namespace WeCount.Application.Debts.Queries;

public record GetDebtByIdQuery(Guid Id) : IRequest<DebtDto>;
