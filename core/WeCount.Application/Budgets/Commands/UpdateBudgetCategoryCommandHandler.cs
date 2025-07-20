using MediatR;
using WeCount.Application.DTOs.Budget;
using WeCount.Domain.Entities.Budget;
using WeCount.Domain.ValueObjects;
using WeCount.Application.Common.Interfaces.Repositories;

namespace WeCount.Application.Budgets.Commands;

public class UpdateBudgetCategoryCommandHandler
    : IRequestHandler<UpdateBudgetCategoryCommand, BudgetDto>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly ITransactionRepository _transactionRepository;

    public UpdateBudgetCategoryCommandHandler(
        IBudgetRepository budgetRepository,
        ITransactionRepository transactionRepository
    )
    {
        _budgetRepository = budgetRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<BudgetDto> Handle(
        UpdateBudgetCategoryCommand request,
        CancellationToken cancellationToken
    )
    {
        // Check if budget with this category exists
        var budgets = await _budgetRepository.GetByCoupleIdAsync(request.CoupleId);
        var budget = budgets.FirstOrDefault(b => b.Category.Code == request.Category);

        if (budget == null)
        {
            // Create new budget
            budget = new Budget
            {
                Id = Guid.NewGuid(),
                Category = new Category(request.Category, request.Category), // Using code as label for now
                Amount = request.Limit, // Using limit as amount
                Spent = 0,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1),
                CoupleId = request.CoupleId,
                Name = request.Category, // Using category as name for now
            };

            await _budgetRepository.CreateAsync(budget);
        }
        else
        {
            // Update existing budget
            budget.Amount = request.Limit;
            await _budgetRepository.UpdateAsync(budget);
        }

        // Calculate spent amount for this category
        decimal spent = 0;
        // In a real implementation, we would calculate the spent amount based on transactions
        // This is a placeholder for now

        return new BudgetDto(
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
        );
    }
}
