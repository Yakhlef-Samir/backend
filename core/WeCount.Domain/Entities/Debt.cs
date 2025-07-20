using System;
using MongoDB.Bson.Serialization.Attributes;
using WeCount.Domain.Common;
using WeCount.Domain.Enums;

namespace WeCount.Domain.Entities
{
    public class Debt : AuditableEntity
    {
        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("amount")]
        public decimal Amount { get; set; }

        [BsonElement("paidAmount")]
        public decimal PaidAmount { get; set; }

        [BsonElement("interestRate")]
        public decimal InterestRate { get; set; }

        [BsonElement("startDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime StartDate { get; set; }

        [BsonElement("endDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime EndDate { get; set; }

        [BsonElement("type")]
        public DebtType Type { get; set; } // Loan, Borrowing

        [BsonElement("status")]
        public PaymentStatus Status { get; set; } // Active, Paid, Late

        [BsonElement("coupleId")]
        public Guid CoupleId { get; set; }
    }
}
