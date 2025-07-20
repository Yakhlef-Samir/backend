using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeCount.Domain.Entities.Couple
{
    public class CoupleScore
    {
        [BsonElement("score")]
        public int Score { get; set; }

        [BsonElement("metrics")]
        public CoupleScoreMetrics Metrics { get; set; } = new CoupleScoreMetrics();
    }

    public class CoupleScoreMetrics
    {
        [BsonElement("communication")]
        public int Communication { get; set; }

        [BsonElement("sharedGoals")]
        public int SharedGoals { get; set; }

        [BsonElement("spendingHabits")]
        public int SpendingHabits { get; set; }

        [BsonElement("equity")]
        public int Equity { get; set; }

        [BsonElement("transparency")]
        public int Transparency { get; set; }
    }

    public class CoupleScoreHistoryItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.String)]
        public Guid CoupleId { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("score")]
        public int Score { get; set; }

        [BsonElement("budgetScore")]
        public int BudgetScore { get; set; }

        [BsonElement("goalsScore")]
        public int GoalsScore { get; set; }

        [BsonElement("debtScore")]
        public int DebtScore { get; set; }

        [BsonElement("savingsScore")]
        public int SavingsScore { get; set; }

        [BsonElement("transactionsScore")]
        public int TransactionsScore { get; set; }

        [BsonElement("message")]
        public string Message { get; set; } = string.Empty;
    }
}
