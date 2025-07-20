using MediatR;
using WeCount.Application.DTOs;

namespace WeCount.Application.Transactions.Queries;

public record GetTransactionByIdQuery(Guid Id) : IRequest<TransactionDto?>;
