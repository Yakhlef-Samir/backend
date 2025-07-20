using MongoDB.Driver;
using WeCount.Domain.Entities;
using WeCount.Domain.Entities.Budget;
using WeCount.Domain.Entities.Couple;
using WeCount.Domain.Entities.Transaction;
using CoupleEntity = WeCount.Domain.Entities.Couple.Couple;
using DebtEntity = WeCount.Domain.Entities.Debt;
using GoalEntity = WeCount.Domain.Entities.Goal;

namespace WeCount.Application.Common.Interfaces
{
    public interface IMongoDbContext
    {
        IMongoCollection<User> Users { get; }
        IMongoCollection<CoupleEntity> Couples { get; }
        IMongoCollection<Transaction> Transactions { get; }
        IMongoCollection<Budget> Budgets { get; }
        IMongoCollection<GoalEntity> Goals { get; }
        IMongoCollection<DebtEntity> Debts { get; }
        IMongoCollection<CoupleScoreHistoryItem> CoupleScores { get; }
        IMongoCollection<TransactionCategory> TransactionCategories { get; }
    }
}
