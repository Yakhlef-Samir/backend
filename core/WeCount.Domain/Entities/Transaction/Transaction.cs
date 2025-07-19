using System;
using MongoDB.Bson.Serialization.Attributes;
using WeCount.Domain.Common;
using WeCount.Domain.Enums;
using WeCount.Domain.ValueObjects;

namespace WeCount.Domain.Entities.Transaction
{
    public class Transaction : AuditableEntity
    {
        [BsonElement("amount")]
        public decimal Amount { get; set; }

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("category")]
        public Category Category { get; set; } = new Category(string.Empty, string.Empty);

        [BsonElement("date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Date { get; set; }

        [BsonElement("userId")]
        public Guid UserId { get; set; }

        [BsonElement("coupleId")]
        public Guid CoupleId { get; set; }

        [BsonElement("type")]
        public TransactionType Type { get; set; }
    }
}
