using MongoDB.Bson.Serialization.Attributes;
using WeCount.Domain.Common;

namespace WeCount.Domain.Entities
{
    public class Goal : AuditableEntity
    {
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("targetAmount")]
        public decimal TargetAmount { get; set; }

        [BsonElement("savedAmount")]
        public decimal SavedAmount { get; set; }

        [BsonElement("deadline")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Deadline { get; set; }

        [BsonElement("icon")]
        public string Icon { get; set; } = string.Empty;

        [BsonElement("coupleId")]
        public Guid CoupleId { get; set; }
    }
}
