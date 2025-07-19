using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeCount.Domain.Entities.Financial
{
    public class FinancialReport
    {
        [BsonElement("predictions")]
        public List<FinancialPrediction> Predictions { get; set; } =
            new List<FinancialPrediction>();

        [BsonElement("totals")]
        public FinancialTotals Totals { get; set; } = new FinancialTotals();
    }
}
