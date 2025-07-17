using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeCount.Domain.Entities
{
    public class Budget
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("category")]
        public string Category { get; set; } = string.Empty;

        [BsonElement("amount")]
        public decimal Amount { get; set; }

        [BsonElement("spent")]
        public decimal Spent { get; set; }

        [BsonElement("startDate")]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public DateTime StartDate { get; set; }

        [BsonElement("endDate")]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public DateTime EndDate { get; set; }

        [BsonElement("coupleId")]
        public Guid CoupleId { get; set; }
    }
}
