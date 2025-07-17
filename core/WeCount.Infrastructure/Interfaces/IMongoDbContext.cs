using MongoDB.Driver;
using WeCount.Domain.Entities;

namespace WeCount.Infrastructure.Interfaces
{
    public interface IMongoDbContext
    {
        IMongoCollection<Transaction> Transactions { get; }
        IMongoCollection<User> Users { get; }
        IMongoCollection<Couple> Couples { get; }
        // IMongoCollection<Budget> Budgets { get; }
        // IMongoCollection<Goal> Goals { get; }
        // IMongoCollection<Debt> Debts { get; }
    }
}