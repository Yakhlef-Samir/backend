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
using WeCount.Domain.Entities.Couple;
using WeCount.Domain.Entities.Transaction;
using WeCount.Infrastructure.Interfaces;

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
    public IMongoCollection<Couple> Couples => _database.GetCollection<Couple>("Couples");
    public IMongoCollection<Transaction> Transactions =>
        _database.GetCollection<Transaction>("Transactions");
    public IMongoCollection<Budget> Budgets => _database.GetCollection<Budget>("Budgets");
    public IMongoCollection<Goal> Goals => _database.GetCollection<Goal>("Goals");
    public IMongoCollection<Debt> Debts => _database.GetCollection<Debt>("Debts");
}
