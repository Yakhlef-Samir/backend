using MediatR;
using WeCount.Application.DTOs;
using WeCount.Application.Common.Interfaces.Repositories;

namespace WeCount.Application.Transactions.Queries;

public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, TransactionDto?>

{
    private readonly ITransactionRepository _transactionRepository;

    public GetTransactionByIdQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<TransactionDto?> Handle(
        GetTransactionByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.Id);
        if (transaction == null)
        {
            return null;
        }

        return new TransactionDto(
            transaction.Id,
            transaction.Description,
            transaction.Amount,
            transaction.Date,
            transaction.Category.ToString(),
            transaction.UserId,
            transaction.UserName,
            transaction.CoupleId,
            transaction.IsShared
        );
    }
}
