using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeCount.Domain.Entities.Transaction
{
    public class TransactionCategory
    {
        [BsonElement("code")]
        public string Code { get; set; } = string.Empty;

        [BsonElement("label")]
        public string Label { get; set; } = string.Empty;

        [BsonElement("icon")]
        public string Icon { get; set; } = string.Empty;

        [BsonElement("color")]
        public string Color { get; set; } = string.Empty;
    }
}
