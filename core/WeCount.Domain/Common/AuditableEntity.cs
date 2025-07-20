using WeCount.Domain.Common;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeCount.Domain.Common
{
    public abstract class AuditableEntity : EntityBase
    {
        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpdatedAt { get; set; }
    }
}
