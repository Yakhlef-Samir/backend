using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeCount.Domain.Entities.Financial
{
    public class FinancialPrediction
    {
        [BsonElement("month")]
        public string Month { get; set; } = string.Empty;

        [BsonElement("income")]
        public decimal Income { get; set; }

        [BsonElement("expenses")]
        public decimal Expenses { get; set; }

        [BsonElement("savings")]
        public decimal Savings { get; set; }
    }
}
