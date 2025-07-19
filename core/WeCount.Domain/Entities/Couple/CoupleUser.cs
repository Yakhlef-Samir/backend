using System;
using MongoDB.Bson.Serialization.Attributes;
using WeCount.Domain.Common;
using WeCount.Domain.Enums;

namespace WeCount.Domain.Entities.Couple
{
    public class CoupleUser : AuditableEntity
    {
        [BsonElement("userId")]
        public Guid UserId { get; set; }

        [BsonElement("coupleId")]
        public Guid CoupleId { get; set; }

        [BsonElement("role")]
        public string Role { get; set; } = string.Empty;

        [BsonElement("isCreator")]
        public bool IsCreator { get; set; }

        [BsonElement("invitationSentAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime InvitationSentAt { get; set; }

        [BsonElement("joinedAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? JoinedAt { get; set; }

        [BsonElement("status")]
        public CoupleUserStatus Status { get; set; }

        [BsonElement("avatar")]
        public string? Avatar { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }
    }
}
