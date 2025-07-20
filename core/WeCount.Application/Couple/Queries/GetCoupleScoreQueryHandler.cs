using MediatR;
using WeCount.Application.Common.Interfaces.Repositories;
using WeCount.Application.DTOs.Couple;
using WeCount.Domain.Entities;
using WeCount.Domain.Entities.Budget;
using WeCount.Domain.Entities.Transaction;
using WeCount.Domain.Enums;

namespace WeCount.Application.Couple.Queries
{
    public class GetCoupleScoreQueryHandler : IRequestHandler<GetCoupleScoreQuery, CoupleScoreDto>
    {
        private readonly ICoupleRepository _coupleRepository;
        private readonly IBudgetRepository _budgetRepository;
        private readonly IGoalRepository _goalRepository;
        private readonly IDebtRepository _debtRepository;
        private readonly ITransactionRepository _transactionRepository;

        public GetCoupleScoreQueryHandler(
            ICoupleRepository coupleRepository,
            IBudgetRepository budgetRepository,
            IGoalRepository goalRepository,
            IDebtRepository debtRepository,
            ITransactionRepository transactionRepository
        )
        {
            _coupleRepository = coupleRepository;
            _budgetRepository = budgetRepository;
            _goalRepository = goalRepository;
            _debtRepository = debtRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<CoupleScoreDto> Handle(
            GetCoupleScoreQuery request,
            CancellationToken cancellationToken
        )
        {
            // Verify couple exists
            var couple = await _coupleRepository.GetByIdAsync(request.CoupleId);
            if (couple == null)
            {
                throw new Exception($"Couple with ID {request.CoupleId} not found");
            }

            // Get data for score calculation
            var budgets = await _budgetRepository.GetByCoupleIdAsync(request.CoupleId);
            var goals = await _goalRepository.GetByCoupleIdAsync(request.CoupleId);
            var debts = await _debtRepository.GetByCoupleIdAsync(request.CoupleId);
            var transactions = await _transactionRepository.GetByCoupleIdAsync(request.CoupleId);

            // Calculate component scores
            int budgetScore = CalculateBudgetScore(budgets.ToList(), transactions.ToList());
            int goalsScore = CalculateGoalsScore(goals.ToList());
            int debtScore = CalculateDebtScore(debts.ToList());
            int savingsScore = CalculateSavingsScore(transactions.ToList());
            int transactionsScore = CalculateTransactionsScore(transactions.ToList());

            // Calculate total score
            int totalScore =
                budgetScore + goalsScore + debtScore + savingsScore + transactionsScore;

            // Generate message based on score
            string message = GenerateScoreMessage(totalScore);

            return new CoupleScoreDto(
                Score: totalScore,
                BudgetScore: budgetScore,
                GoalsScore: goalsScore,
                DebtScore: debtScore,
                SavingsScore: savingsScore,
                TransactionsScore: transactionsScore,
                Message: message
            );
        }

        private int CalculateBudgetScore(List<Budget> budgets, List<Transaction> transactions)
        {
            // If no budgets, score is 0
            if (!budgets.Any())
            {
                return 0;
            }

            // Group transactions by category
            var transactionsByCategory = transactions
                .Where(t =>
                    t.Date.Month == DateTime.UtcNow.Month && t.Date.Year == DateTime.UtcNow.Year
                )
                .GroupBy(t => t.Category.Code)
                .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));

            int score = 0;
            int maxScore = 20;
            int budgetsWithinLimit = 0;
            // Check how many budgets are within their limits
            foreach (var budget in budgets)
            {
                var code = budget.Category.Code;
                if (transactionsByCategory.TryGetValue(code, out decimal spent))
                {
                    if (Math.Abs(spent) <= budget.Amount)
                    {
                        budgetsWithinLimit++;
                    }
                }
                else
                {
                    // No transactions for this category, so it's within budget
                    budgetsWithinLimit++;
                }
            }

            // Calculate score based on percentage of budgets within limit
            double percentWithinLimit = (double)budgetsWithinLimit / budgets.Count;
            score = (int)(percentWithinLimit * maxScore);

            return score;
        }

        private int CalculateGoalsScore(List<Goal> goals)
        {
            // If no goals, score is 0
            if (!goals.Any())
            {
                return 0;
            }

            int score = 0;
            int maxScore = 20;

            // Calculate score based on goal completion and progress
            int completedGoals = goals.Count(g => g.IsCompleted);
            double completionRatio = (double)completedGoals / goals.Count;

            // Calculate progress for incomplete goals
            double totalProgress = 0;
            int incompleteGoals = goals.Count - completedGoals;

            if (incompleteGoals > 0)
            {
                foreach (var goal in goals.Where(g => !g.IsCompleted))
                {
                    double progress =
                        goal.TargetAmount > 0
                            ? (double)goal.SavedAmount / (double)goal.TargetAmount
                            : 0;
                    totalProgress += progress;
                }

                double averageProgress = totalProgress / incompleteGoals;

                // Score is based on completed goals and progress on incomplete goals
                score = (int)((completionRatio * 0.7 + averageProgress * 0.3) * maxScore);
            }
            else
            {
                // All goals are completed
                score = maxScore;
            }

            return score;
        }

        private static int CalculateDebtScore(List<Debt> debts)
        {
            // If no debts, score is maximum
            if (debts.Count == 0)
            {
                return 20;
            }

            int score = 0;
            int maxScore = 20;

            // Calculate score based on debt payment progress
            int paidDebts = debts.Count(d => d.Status == PaymentStatus.Paid);
            double paidRatio = (double)paidDebts / debts.Count;

            // Calculate payment progress for unpaid debts
            double totalPaymentProgress = 0;
            int unpaidDebts = debts.Count - paidDebts;

            if (unpaidDebts > 0)
            {
                foreach (var debt in debts.Where(d => d.Status != PaymentStatus.Paid))
                {
                    double progress =
                        debt.Amount > 0 ? (double)debt.PaidAmount / (double)debt.Amount : 0;
                    totalPaymentProgress += progress;
                }

                double averageProgress = totalPaymentProgress / unpaidDebts;

                // Score is based on paid debts and progress on unpaid debts
                score = (int)((paidRatio * 0.7 + averageProgress * 0.3) * maxScore);
            }
            else
            {
                // All debts are paid
                score = maxScore;
            }

            return score;
        }

        private static int CalculateSavingsScore(List<Transaction> transactions)
        {
            // If no transactions, score is 0
            if (transactions.Count == 0)
            {
                return 0;
            }

            int score = 0;
            int maxScore = 20;

            // Get current month's transactions
            var currentMonth = DateTime.UtcNow.Month;
            var currentYear = DateTime.UtcNow.Year;

            var monthlyTransactions = transactions.Where(t =>
                t.Date.Month == currentMonth && t.Date.Year == currentYear
            );

            var incomeTransactions = monthlyTransactions.Where(t => t.Amount > 0);
            var expenseTransactions = monthlyTransactions.Where(t => t.Amount < 0);

            decimal monthlyIncome = incomeTransactions.Sum(t => t.Amount);
            decimal monthlyExpenses = Math.Abs(expenseTransactions.Sum(t => t.Amount));

            // Calculate savings rate
            if (monthlyIncome > 0)
            {
                decimal savingsRate = (monthlyIncome - monthlyExpenses) / monthlyIncome;

                // Score based on savings rate
                // 20% savings rate or higher gets full score
                double scoreMultiplier = Math.Min(1.0, (double)savingsRate / 0.2);
                score = (int)(scoreMultiplier * maxScore);
            }

            return score;
        }

        private static int CalculateTransactionsScore(List<Transaction> transactions)
        {
            // If no transactions, score is 0
            if (transactions.Count == 0)
            {
                return 0;
            }

            int score = 0;

            // Calculate score based on transaction regularity and categorization

            // Check if transactions are spread throughout the month (not all at once)
            var currentMonth = DateTime.UtcNow.Month;
            var currentYear = DateTime.UtcNow.Year;

            var monthlyTransactions = transactions
                .Where(t => t.Date.Month == currentMonth && t.Date.Year == currentYear)
                .ToList();

            if (monthlyTransactions.Count > 0)
            {
                // Group transactions by day of month
                var transactionDays = monthlyTransactions
                    .Select(t => t.Date.Day)
                    .Distinct()
                    .Count();

                // More days with transactions = better score (up to 10 points)
                int daysScore = Math.Min(10, transactionDays);

                // Check if transactions are categorized
                int categorizedTransactions = monthlyTransactions.Count(t =>
                    !string.IsNullOrEmpty(t.Category.ToString())
                );
                double categorizedRatio =
                    (double)categorizedTransactions / monthlyTransactions.Count;
                int categorizationScore = (int)(categorizedRatio * 10);

                score = daysScore + categorizationScore;
            }
            else
            {
                // No transactions this month
                score = 0;
            }

            return score;
        }

        private static string GenerateScoreMessage(int score)
        {
            return score switch
            {
                >= 90 => "Excellent! Vous gérez vos finances de couple de manière exemplaire.",
                >= 75 => "Très bien! Votre couple a une bonne santé financière.",
                >= 60 =>
                    "Bien! Vous êtes sur la bonne voie, mais il y a encore place à l'amélioration.",
                >= 40 =>
                    "Moyen. Votre couple pourrait bénéficier d'une meilleure planification financière.",
                >= 20 =>
                    "Attention! Votre situation financière de couple nécessite des améliorations importantes.",
                _ => "Critique! Il est urgent de revoir votre gestion financière de couple.",
            };
        }
    }
}
