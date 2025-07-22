using MediatR;
using WeCount.Application.Common.Interfaces.Repositories;
using WeCount.Application.DTOs.Analytics;
using WeCount.Domain.Entities.Transaction;

namespace WeCount.Application.Analytics.Queries
{
    public class GetFinancialsQueryHandler : IRequestHandler<GetFinancialsQuery, FinancialsDto>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetFinancialsQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<FinancialsDto> Handle(
            GetFinancialsQuery request,
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

            // Get current date and calculate start date based on requested months
            DateTime endDate = DateTime.UtcNow;
            DateTime startDate = endDate.AddMonths(-request.Months);

            // Filter transactions within the date range
            IEnumerable<Transaction>? filteredTransactions = transactions.Where(t =>
                t.Date >= startDate && t.Date <= endDate
            );

            // Group transactions by month and year
            var groupedTransactions = filteredTransactions
                .GroupBy(t => new { t.Date.Year, t.Date.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .ToList();

            var monthlyData = new List<MonthlyFinancialsDto>();
            decimal totalIncome = 0;
            decimal totalExpenses = 0;
            decimal totalSavings = 0;

            // Calculate monthly data
            foreach (var group in groupedTransactions)
            {
                var monthName = new DateTime(group.Key.Year, group.Key.Month, 1).ToString(
                    "MMM yyyy"
                );
                var incomeTransactions = group.Where(t => t.Amount > 0);
                var expenseTransactions = group.Where(t => t.Amount < 0);

                decimal monthlyIncome = incomeTransactions.Sum(t => t.Amount);
                decimal monthlyExpenses = Math.Abs(expenseTransactions.Sum(t => t.Amount));
                decimal monthlySavings = monthlyIncome - monthlyExpenses;

                totalIncome += monthlyIncome;
                totalExpenses += monthlyExpenses;
                totalSavings += monthlySavings;

                monthlyData.Add(
                    new MonthlyFinancialsDto(
                        Month: monthName,
                        Income: monthlyIncome,
                        Expenses: monthlyExpenses,
                        Savings: monthlySavings
                    )
                );
            }

            // Calculate averages
            int monthCount = monthlyData.Count > 0 ? monthlyData.Count : 1; // Avoid division by zero
            decimal averageMonthlyIncome = totalIncome / monthCount;
            decimal averageMonthlyExpenses = totalExpenses / monthCount;
            decimal averageMonthlySavings = totalSavings / monthCount;

            return new FinancialsDto(
                MonthlyData: monthlyData,
                TotalIncome: totalIncome,
                TotalExpenses: totalExpenses,
                TotalSavings: totalSavings,
                AverageMonthlyIncome: averageMonthlyIncome,
                AverageMonthlyExpenses: averageMonthlyExpenses,
                AverageMonthlySavings: averageMonthlySavings
            );
        }
    }
}
