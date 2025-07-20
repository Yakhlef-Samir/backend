using MediatR;
using WeCount.Application.DTOs;

namespace WeCount.Application.Transactions.Queries;

public record GetAllTransactionsQuery : IRequest<IEnumerable<TransactionDto>>;
