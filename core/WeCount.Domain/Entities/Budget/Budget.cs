using System;
using MongoDB.Bson.Serialization.Attributes;
using WeCount.Domain.Common;
using WeCount.Domain.ValueObjects;

namespace WeCount.Domain.Entities.Budget
{
    public class Budget : AuditableEntity
    {
        [BsonElement("category")]
        public Category Category { get; set; } = new Category(string.Empty, string.Empty);

        [BsonElement("amount")]
        public decimal Amount { get; set; }

        [BsonElement("spent")]
        public decimal Spent { get; set; }

        [BsonElement("startDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime StartDate { get; set; }

        [BsonElement("endDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime EndDate { get; set; }

        [BsonElement("coupleId")]
        public Guid CoupleId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
    }
}
