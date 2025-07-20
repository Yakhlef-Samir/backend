using MediatR;
using WeCount.Application.DTOs;
using WeCount.Application.DTOs.Budget;

namespace WeCount.Application.Budgets.Commands;

public record UpdateBudgetCategoryCommand(string Category, decimal Limit, Guid CoupleId)
    : IRequest<BudgetDto>;
