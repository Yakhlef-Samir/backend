using MediatR;
using WeCount.Application.DTOs.Budget;

namespace WeCount.Application.Budgets.Queries;

public record GetBudgetCategoriesQuery(Guid? CoupleId = null) : IRequest<IEnumerable<BudgetDto>>;
