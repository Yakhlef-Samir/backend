using MediatR;
using WeCount.Application.DTOs;

namespace WeCount.Application.Debts.Queries;

public record GetAllDebtsQuery(Guid? CoupleId = null) : IRequest<IEnumerable<DebtDto>>;
