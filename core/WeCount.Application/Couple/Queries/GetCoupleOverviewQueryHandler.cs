using MediatR;
using WeCount.Application.Common.Interfaces.Repositories;
using WeCount.Application.DTOs;
using WeCount.Application.DTOs.Budget;
using WeCount.Application.DTOs.Couple;
using WeCount.Domain.Entities;
using WeCount.Domain.Entities.Budget;
using WeCount.Domain.Entities.Transaction;
using WeCount.Domain.Enums;
using WeCount.Domain.ValueObjects;

namespace WeCount.Application.Couple.Queries
{
    public class GetCoupleOverviewQueryHandler
        : IRequestHandler<GetCoupleOverviewQuery, CoupleOverviewDto>
    {
        private readonly ICoupleRepository _coupleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBudgetRepository _budgetRepository;
        private readonly IGoalRepository _goalRepository;
        private readonly IDebtRepository _debtRepository;
        private readonly ITransactionRepository _transactionRepository;

        public GetCoupleOverviewQueryHandler(
            ICoupleRepository coupleRepository,
            IUserRepository userRepository,
            IBudgetRepository budgetRepository,
            IGoalRepository goalRepository,
            IDebtRepository debtRepository,
            ITransactionRepository transactionRepository
        )
        {
            _coupleRepository = coupleRepository;
            _userRepository = userRepository;
            _budgetRepository = budgetRepository;
            _goalRepository = goalRepository;
            _debtRepository = debtRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<CoupleOverviewDto> Handle(
            GetCoupleOverviewQuery request,
            CancellationToken cancellationToken
        )
        {
            // Get couple
            var couple = await _coupleRepository.GetByIdAsync(request.CoupleId);
            if (couple == null)
            {
                throw new Exception($"Couple with ID {request.CoupleId} not found");
            }

            // Get members
            var members = new List<UserDto>();
            foreach (var member in couple.Members)
            {
                var user = await _userRepository.GetByIdAsync(member.UserId);
                if (user != null)
                {
                    members.Add(
                        new UserDto(
                            user.Id,
                            user.Email,
                            user.Avatar,
                            user.Name.ToString(),
                            user.PhoneNumber,
                            user.Address,
                            user.City,
                            user.ZipCode,
                            user.Country,
                            user.CoupleId
                        )
                    );
                }
            }

            // Get budgets
            var budgets = await _budgetRepository.GetByCoupleIdAsync(request.CoupleId);
            var topBudgets = budgets
                .OrderByDescending(b => b.Amount)
                .Take(3)
                .Select(budget => new BudgetDto(
                    budget.Id,
                    new WeCount.Application.DTOs.Budget.CategoryDto(
                        budget.Category.Code,
                        budget.Category.Label
                    ),
                    budget.Amount,
                    0, // Placeholder for spent amount
                    budget.StartDate,
                    budget.EndDate,
                    budget.CoupleId,
                    budget.Amount, // Using Amount as Limit
                    string.Empty, // Icon placeholder
                    string.Empty, // Color placeholder
                    budget.Name
                ))
                .ToList();

            // Get goals
            var goals = await _goalRepository.GetByCoupleIdAsync(request.CoupleId);
            var topGoals = goals
                .Take(3)
                .Select(g => new GoalDto(
                    g.Id,
                    g.Name,
                    g.TargetAmount,
                    g.SavedAmount,
                    g.Deadline,
                    g.Icon,
                    g.CoupleId,
                    g.IsCompleted,
                    g.Priority
                ))
                .ToList();

            // Get debts
            var debts = await _debtRepository.GetByCoupleIdAsync(request.CoupleId);
            var activeDebtsCount = debts.Count(d => d.Status != PaymentStatus.Paid);
            var totalDebtAmount = debts.Sum(d => d.Amount - d.PaidAmount);

            // Get transactions for income and expenses calculation
            var transactions = await _transactionRepository.GetByCoupleIdAsync(request.CoupleId);
            var currentMonth = DateTime.UtcNow.Month;
            var currentYear = DateTime.UtcNow.Year;

            var monthlyTransactions = transactions.Where(t =>
                t.Date.Month == currentMonth && t.Date.Year == currentYear
            );

            var incomeTransactions = monthlyTransactions.Where(t => t.Amount > 0);
            var expenseTransactions = monthlyTransactions.Where(t => t.Amount < 0);

            decimal balance = transactions.Sum(t => t.Amount);
            decimal monthlyIncome = incomeTransactions.Sum(t => t.Amount);
            decimal monthlyExpenses = Math.Abs(expenseTransactions.Sum(t => t.Amount));

            // Calculate couple score (placeholder implementation)
            int coupleScore = CalculateCoupleScore(
                budgets.ToList(),
                goals.ToList(),
                debts.ToList(),
                transactions.ToList()
            );

            return new CoupleOverviewDto(
                Id: couple.Id,
                Name: couple.Name,
                Members: members,
                Balance: balance,
                TopBudgets: topBudgets,
                TopGoals: topGoals,
                ActiveDebtsCount: activeDebtsCount,
                TotalDebtAmount: totalDebtAmount,
                MonthlyIncome: monthlyIncome,
                MonthlyExpenses: monthlyExpenses,
                CoupleScore: coupleScore
            );
        }

        private int CalculateCoupleScore(
            List<Budget> budgets,
            List<Goal> goals,
            List<Debt> debts,
            List<Transaction> transactions
        )
        {
            // This is a placeholder implementation for calculating the couple score
            // In a real implementation, this would be more sophisticated

            int budgetScore = 0;
            int goalsScore = 0;
            int debtScore = 0;
            int savingsScore = 0;
            int transactionsScore = 0;

            // Budget score: based on how well they stay within budget
            if (budgets.Count != 0)
            {
                // Simple implementation: if they have budgets, they get some points
                budgetScore = 20;
            }

            // Goals score: based on progress towards goals
            if (goals.Count != 0)
            {
                int completedGoals = goals.Count(g => g.IsCompleted);
                double completionRatio = goals.Count > 0 ? (double)completedGoals / goals.Count : 0;
                goalsScore = (int)(completionRatio * 20);
            }

            // Debt score: based on debt management
            if (debts.Count != 0)
            {
                int paidDebts = debts.Count(d => d.Status == PaymentStatus.Paid);
                double paidRatio = debts.Count > 0 ? (double)paidDebts / debts.Count : 0;
                debtScore = (int)(paidRatio * 20);
            }
            else
            {
                // No debts is good
                debtScore = 20;
            }

            // Savings score: based on savings rate
            if (transactions.Count != 0)
            {
                var currentMonth = DateTime.UtcNow.Month;
                var currentYear = DateTime.UtcNow.Year;

                var monthlyTransactions = transactions.Where(t =>
                    t.Date.Month == currentMonth && t.Date.Year == currentYear
                );

                var incomeTransactions = monthlyTransactions.Where(t => t.Amount > 0);
                var expenseTransactions = monthlyTransactions.Where(t => t.Amount < 0);

                decimal monthlyIncome = incomeTransactions.Sum(t => t.Amount);
                decimal monthlyExpenses = Math.Abs(expenseTransactions.Sum(t => t.Amount));

                if (monthlyIncome > 0)
                {
                    decimal savingsRate = (monthlyIncome - monthlyExpenses) / monthlyIncome;
                    savingsScore = (int)(savingsRate * 20);
                    savingsScore = Math.Max(0, Math.Min(20, savingsScore)); // Clamp between 0 and 20
                }
            }

            // Transactions score: based on regular financial activity
            if (transactions.Count != 0)
            {
                // Simple implementation: if they have transactions, they get some points
                transactionsScore = 20;
            }

            // Total score is the sum of all component scores
            return budgetScore + goalsScore + debtScore + savingsScore + transactionsScore;
        }
    }
}
