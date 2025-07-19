using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeCount.Domain.Entities.Budget
{
    public class BudgetCategory
    {
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("spent")]
        public decimal Spent { get; set; }

        [BsonElement("budget")]
        public decimal Budget { get; set; }
    }
}
