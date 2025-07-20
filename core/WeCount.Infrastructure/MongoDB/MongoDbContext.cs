using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using WeCount.Domain.Entities;
using WeCount.Domain.Entities.Budget;
using WeCount.Domain.Entities.Transaction;
using CoupleEntity = WeCount.Domain.Entities.Couple.Couple;
using GoalEntity = WeCount.Domain.Entities.Goal;
using DebtEntity = WeCount.Domain.Entities.Debt;
using WeCount.Application.Common.Interfaces;

namespace WeCount.Infrastructure.MongoDB;

public class MongoDbContext : IMongoDbContext
{
    private readonly IMongoDatabase _database;

    static MongoDbContext()
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
    }

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    public IMongoCollection<CoupleEntity> Couples => _database.GetCollection<CoupleEntity>("Couples");
    public IMongoCollection<Transaction> Transactions =>
        _database.GetCollection<Transaction>("Transactions");
    public IMongoCollection<Budget> Budgets => _database.GetCollection<Budget>("Budgets");
    public IMongoCollection<GoalEntity> Goals => _database.GetCollection<GoalEntity>("Goals");
    public IMongoCollection<DebtEntity> Debts => _database.GetCollection<DebtEntity>("Debts");
    public IMongoCollection<WeCount.Domain.Entities.Couple.CoupleScoreHistoryItem> CoupleScores =>
        _database.GetCollection<WeCount.Domain.Entities.Couple.CoupleScoreHistoryItem>("CoupleScores");
    public IMongoCollection<TransactionCategory> TransactionCategories =>
        _database.GetCollection<TransactionCategory>("TransactionCategories");
}
