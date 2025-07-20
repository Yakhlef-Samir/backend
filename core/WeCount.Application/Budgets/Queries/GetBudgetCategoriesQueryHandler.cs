using MediatR;
using WeCount.Application.DTOs.Budget;
using WeCount.Application.Common.Interfaces.Repositories;

namespace WeCount.Application.Budgets.Queries;

public class GetBudgetCategoriesQueryHandler
    : IRequestHandler<GetBudgetCategoriesQuery, IEnumerable<BudgetDto>>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly ITransactionRepository _transactionRepository;

    public GetBudgetCategoriesQueryHandler(
        IBudgetRepository budgetRepository,
        ITransactionRepository transactionRepository
    )
    {
        _budgetRepository = budgetRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<IEnumerable<BudgetDto>> Handle(
        GetBudgetCategoriesQuery request,
        CancellationToken cancellationToken
    )
    {
        IEnumerable<Domain.Entities.Budget.Budget> budgets;

        if (request.CoupleId.HasValue)
        {
            budgets = await _budgetRepository.GetByCoupleIdAsync(request.CoupleId.Value);
        }
        else
        {
            budgets = await _budgetRepository.GetAllAsync();
        }
        var result = new List<BudgetDto>();

        foreach (var budget in budgets)
        {
            // Calculate spent amount for this category
            decimal spent = 0;
            // In a real implementation, we would calculate the spent amount based on transactions
            // This is a placeholder for now

            result.Add(
                new BudgetDto(
                    budget.Id,
                    new CategoryDto(budget.Category.Code, budget.Category.Label),
                    budget.Amount,
                    spent,
                    budget.StartDate,
                    budget.EndDate,
                    budget.CoupleId,
                    budget.Amount, // Using Amount as Limit
                    string.Empty, // Icon placeholder
                    string.Empty, // Color placeholder
                    budget.Name
                )
            );
        }

        return result;
    }
}
