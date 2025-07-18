using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeCount.Domain.Entities
{
    public class Goal
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("targetAmount")]
        public decimal TargetAmount { get; set; }

        [BsonElement("savedAmount")]
        public decimal SavedAmount { get; set; }

        [BsonElement("deadline")]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public DateTime Deadline { get; set; }

        [BsonElement("icon")]
        public string Icon { get; set; } = string.Empty;

        [BsonElement("coupleId")]
        public Guid CoupleId { get; set; }
    }
}
