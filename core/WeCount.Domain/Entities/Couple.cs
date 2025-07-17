using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeCount.Domain.Entities
{
    public class Couple
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; }

        [BsonElement("members")]
        public List<CoupleUser> Members { get; set; } = new List<CoupleUser>();
    }
}
