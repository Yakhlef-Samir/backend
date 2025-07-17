using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCount.Domain.Entities
{
    public class Transaction
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public Guid UserId { get; set; }
    public Guid CoupleId { get; set; }
    //public TransactionType Type { get; set; } // Income, Expense
}
}