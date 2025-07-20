using MongoDB.Bson.Serialization.Attributes;
using WeCount.Domain.Common;
using WeCount.Domain.ValueObjects;

namespace WeCount.Domain.Entities
{
    public class User : AuditableEntity
    {
        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("avatar")]
        public string Avatar { get; set; } = string.Empty;

        [BsonElement("name")]
        public FullName Name { get; set; } = new FullName(string.Empty, string.Empty);

        [BsonElement("phoneNumber")]
        public string? PhoneNumber { get; set; }

        [BsonElement("address")]
        public string? Address { get; set; }

        [BsonElement("city")]
        public string? City { get; set; }

        [BsonElement("zipCode")]
        public string? ZipCode { get; set; }

        [BsonElement("country")]
        public string? Country { get; set; }

        [BsonElement("coupleId")]
        public Guid CoupleId { get; set; }
    }
}
