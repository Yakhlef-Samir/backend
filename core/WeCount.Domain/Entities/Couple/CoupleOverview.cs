using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace WeCount.Domain.Entities.Couple
{
    public class CoupleOverview
    {
        [BsonElement("members")]
        public List<CoupleMemberOverview> Members { get; set; } = new List<CoupleMemberOverview>();

        [BsonElement("expenses")]
        public List<CoupleExpense> Expenses { get; set; } = new List<CoupleExpense>();
    }

    public class CoupleMemberOverview
    {
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("avatar")]
        public string Avatar { get; set; } = string.Empty;

        [BsonElement("spent")]
        public decimal Spent { get; set; }

        [BsonElement("total")]
        public decimal Total { get; set; }
    }

    public class CoupleExpense
    {
        [BsonElement("category")]
        public string Category { get; set; } = string.Empty;

        [BsonElement("amount")]
        public decimal Amount { get; set; }

        [BsonElement("paidBy")]
        public string PaidBy { get; set; } = string.Empty;
    }
}
