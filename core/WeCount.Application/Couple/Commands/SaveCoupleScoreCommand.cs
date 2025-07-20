using MediatR;

namespace WeCount.Application.Couple.Commands
{
    public class SaveCoupleScoreCommand : IRequest<bool>
    {
        public Guid CoupleId { get; }
        public int Score { get; }
        public int BudgetScore { get; }
        public int GoalsScore { get; }
        public int DebtScore { get; }
        public int SavingsScore { get; }
        public int TransactionsScore { get; }
        public string Message { get; }
        public DateTime Date { get; }

        public SaveCoupleScoreCommand(
            Guid coupleId,
            int score,
            int budgetScore,
            int goalsScore,
            int debtScore,
            int savingsScore,
            int transactionsScore,
            string message,
            DateTime? date = null
        )
        {
            CoupleId = coupleId;
            Score = score;
            BudgetScore = budgetScore;
            GoalsScore = goalsScore;
            DebtScore = debtScore;
            SavingsScore = savingsScore;
            TransactionsScore = transactionsScore;
            Message = message;
            Date = date ?? DateTime.UtcNow;
        }
    }
}
