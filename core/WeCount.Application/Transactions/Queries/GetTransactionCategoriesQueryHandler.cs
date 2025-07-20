using MediatR;
using WeCount.Application.DTOs;
using WeCount.Domain.Entities.Transaction;
using WeCount.Application.Common.Interfaces.Repositories;

namespace WeCount.Application.Transactions.Queries;

public class GetTransactionCategoriesQueryHandler
    : IRequestHandler<GetTransactionCategoriesQuery, IEnumerable<TransactionCategoryDto>>
{
    private readonly ITransactionCategoryRepository _categoryRepository;

    public GetTransactionCategoriesQueryHandler(ITransactionCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<TransactionCategoryDto>> Handle(
        GetTransactionCategoriesQuery request,
        CancellationToken cancellationToken
    )
    {
        var categories = await _categoryRepository.GetAllAsync();

        return categories.Select(c => new TransactionCategoryDto(c.Code, c.Label));
    }
}
