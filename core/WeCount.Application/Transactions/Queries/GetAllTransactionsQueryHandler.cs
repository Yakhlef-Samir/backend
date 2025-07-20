using MediatR;
using WeCount.Application.DTOs;
using WeCount.Domain.ValueObjects;
using WeCount.Application.Common.Interfaces.Repositories;

namespace WeCount.Application.Transactions.Queries;

public class GetAllTransactionsQueryHandler
    : IRequestHandler<GetAllTransactionsQuery, IEnumerable<TransactionDto>>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetAllTransactionsQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<IEnumerable<TransactionDto>> Handle(
        GetAllTransactionsQuery request,
        CancellationToken cancellationToken
    )
    {
        var transactions = await _transactionRepository.GetAllAsync();

        return transactions.Select(t => new TransactionDto(
            t.Id,
            t.Description,
            t.Amount,
            t.Date,
            t.Category.Code,
            t.UserId,
            t.UserName,
            t.CoupleId,
            t.IsShared
        ));
    }
}
