using System.Collections.Generic;
using WeCount.Application.DTOs.Budget;

namespace WeCount.Application.DTOs.Couple
{
    public record CoupleOverviewDto(
        Guid Id,
        string Name,
        List<UserDto> Members,
        decimal Balance,
        List<BudgetDto> TopBudgets,
        List<GoalDto> TopGoals,
        int ActiveDebtsCount,
        decimal TotalDebtAmount,
        decimal MonthlyIncome,
        decimal MonthlyExpenses,
        int CoupleScore
    );
}
