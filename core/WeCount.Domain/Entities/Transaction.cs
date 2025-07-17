using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WeCount.Domain.Enums;

namespace WeCount.Domain.Entities
{
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("amount")]
        public decimal Amount { get; set; }

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("category")]
        public string Category { get; set; } = string.Empty;

        [BsonElement("date")]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public DateTime Date { get; set; }

        [BsonElement("userId")]
        public Guid UserId { get; set; }

        [BsonElement("coupleId")]
        public Guid CoupleId { get; set; }

        [BsonElement("type")]
        public TransactionType Type { get; set; } // Income, Expense
    }
}
