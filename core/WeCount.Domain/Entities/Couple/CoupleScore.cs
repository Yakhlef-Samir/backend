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
        [BsonElement("date")]
        public string Date { get; set; } = string.Empty;

        [BsonElement("score")]
        public int Score { get; set; }

        [BsonElement("metrics")]
        public CoupleScoreMetrics Metrics { get; set; } = new CoupleScoreMetrics();
    }
}
