using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeCount.Domain.Entities.Financial
{
    public class FinancialTotals
    {
        [BsonElement("totalIncome")]
        public decimal TotalIncome { get; set; }

        [BsonElement("totalExpenses")]
        public decimal TotalExpenses { get; set; }

        [BsonElement("totalSavings")]
        public decimal TotalSavings { get; set; }
    }
}
