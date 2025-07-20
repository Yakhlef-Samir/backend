using MediatR;
using WeCount.Application.DTOs.Analytics;
using WeCount.Domain.Entities;
using WeCount.Domain.Entities.Transaction;
using WeCount.Domain.Enums;
using WeCount.Application.Common.Interfaces.Repositories;

namespace WeCount.Application.Analytics.Queries
{
    public class GetStatsQueryHandler : IRequestHandler<GetStatsQuery, StatsDto>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IGoalRepository _goalRepository;
        private readonly IDebtRepository _debtRepository;

        public GetStatsQueryHandler(
            ITransactionRepository transactionRepository,
            IGoalRepository goalRepository,
            IDebtRepository debtRepository
        )
        {
            _transactionRepository = transactionRepository;
            _goalRepository = goalRepository;
            _debtRepository = debtRepository;
        }

        public async Task<StatsDto> Handle(
            GetStatsQuery request,
            CancellationToken cancellationToken
        )
        {
            // Get transactions for the couple or all transactions
            IEnumerable<Transaction> transactions;
            if (request.CoupleId.HasValue)
            {
                transactions = await _transactionRepository.GetByCoupleIdAsync(
                    request.CoupleId.Value
                );
            }
            else
            {
                transactions = await _transactionRepository.GetAllAsync();
            }

            // Get goals for the couple or all goals
            IEnumerable<Goal> goals;
            if (request.CoupleId.HasValue)
            {
                goals = await _goalRepository.GetByCoupleIdAsync(request.CoupleId.Value);
            }
            else
            {
                goals = await _goalRepository.GetAllAsync();
            }

            // Get debts for the couple or all debts
            IEnumerable<Debt> debts;
            if (request.CoupleId.HasValue)
            {
                debts = await _debtRepository.GetByCoupleIdAsync(request.CoupleId.Value);
            }
            else
            {
                debts = await _debtRepository.GetAllAsync();
            }

            // Calculate statistics
            var currentMonth = DateTime.UtcNow.Month;
            var currentYear = DateTime.UtcNow.Year;

            var monthlyTransactions = transactions.Where(t =>
                t.Date.Month == currentMonth && t.Date.Year == currentYear
            );

            var incomeTransactions = monthlyTransactions.Where(t => t.Amount > 0);
            var expenseTransactions = monthlyTransactions.Where(t => t.Amount < 0);

            decimal totalBalance = transactions.Sum(t => t.Amount);
            decimal monthlyIncome = incomeTransactions.Sum(t => t.Amount);
            decimal monthlyExpenses = Math.Abs(expenseTransactions.Sum(t => t.Amount));

            // Calculate savings rate (income - expenses) / income * 100
            decimal savingsRate =
                monthlyIncome > 0 ? ((monthlyIncome - monthlyExpenses) / monthlyIncome) * 100 : 0;

            int completedGoals = goals.Count(g => g.IsCompleted);
            int activeGoals = goals.Count(g => !g.IsCompleted);

            int paidDebts = debts.Count(d => d.Status == PaymentStatus.Paid);
            int activeDebts = debts.Count(d => d.Status != PaymentStatus.Paid);

            // Calculate debt to income ratio
            decimal totalDebt = debts.Sum(d => d.Amount - d.PaidAmount);
            decimal debtToIncomeRatio = monthlyIncome > 0 ? (totalDebt / monthlyIncome) * 100 : 0;

            return new StatsDto(
                TotalBalance: totalBalance,
                MonthlyIncome: monthlyIncome,
                MonthlyExpenses: monthlyExpenses,
                SavingsRate: savingsRate,
                CompletedGoals: completedGoals,
                ActiveGoals: activeGoals,
                PaidDebts: paidDebts,
                ActiveDebts: activeDebts,
                DebtToIncomeRatio: debtToIncomeRatio
            );
        }
    }
}
