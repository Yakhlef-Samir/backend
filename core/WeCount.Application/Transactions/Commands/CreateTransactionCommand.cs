using MediatR;
using WeCount.Application.DTOs;

namespace WeCount.Application.Transactions.Commands;

public record CreateTransactionCommand(
    string Description,
    decimal Amount,
    DateTime Date,
    string Category,
    Guid UserId,
    bool IsShared = false
) : IRequest<TransactionDto>;
