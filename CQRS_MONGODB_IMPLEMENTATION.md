# Exemple d'implémentation CQRS avec MongoDB - Module Transactions

## Packages NuGet nécessaires

```xml
<PackageReference Include="MongoDB.Driver" Version="2.23.1" />
<PackageReference Include="MongoDB.Bson" Version="2.23.1" />
<PackageReference Include="MediatR" Version="12.2.0" />
<PackageReference Include="FluentValidation" Version="11.8.1" />
<PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="11.8.1" />
```

## 1. Configuration MongoDB

### MongoDbSettings.cs
```csharp
namespace WeCount.Infrastructure.Settings
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
```

### appsettings.json
```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "WeCountDB"
  }
}
```

## 2. Entités Domain avec MongoDB

### Transaction.cs
```csharp
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WeCount.Domain.Enums;

namespace WeCount.Domain.Entities
{
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("amount")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Amount { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Date { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }

        [BsonElement("coupleId")]
        [BsonRepresentation(BsonType.String)]
        public Guid CoupleId { get; set; }

        [BsonElement("type")]
        [BsonRepresentation(BsonType.String)]
        public TransactionType Type { get; set; }

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpdatedAt { get; set; }

        // Propriétés pour les données dénormalisées (optionnel)
        [BsonElement("userName")]
        public string UserName { get; set; }

        [BsonElement("userEmail")]
        public string UserEmail { get; set; }
    }
}
```

### User.cs
```csharp
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeCount.Domain.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("avatar")]
        public string Avatar { get; set; }

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpdatedAt { get; set; }
    }
}
```

## 3. Context MongoDB

### IMongoDbContext.cs
```csharp
using MongoDB.Driver;
using WeCount.Domain.Entities;

namespace WeCount.Infrastructure.Data
{
    public interface IMongoDbContext
    {
        IMongoCollection<Transaction> Transactions { get; }
        IMongoCollection<User> Users { get; }
        IMongoCollection<Couple> Couples { get; }
        IMongoCollection<Budget> Budgets { get; }
        IMongoCollection<Goal> Goals { get; }
        IMongoCollection<Debt> Debts { get; }
    }
}
```

### MongoDbContext.cs
```csharp
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WeCount.Domain.Entities;
using WeCount.Infrastructure.Settings;

namespace WeCount.Infrastructure.Data
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Transaction> Transactions => 
            _database.GetCollection<Transaction>("transactions");

        public IMongoCollection<User> Users => 
            _database.GetCollection<User>("users");

        public IMongoCollection<Couple> Couples => 
            _database.GetCollection<Couple>("couples");

        public IMongoCollection<Budget> Budgets => 
            _database.GetCollection<Budget>("budgets");

        public IMongoCollection<Goal> Goals => 
            _database.GetCollection<Goal>("goals");

        public IMongoCollection<Debt> Debts => 
            _database.GetCollection<Debt>("debts");
    }
}
```

## 4. Repository Pattern avec MongoDB

### ITransactionRepository.cs
```csharp
using WeCount.Domain.Entities;

namespace WeCount.Infrastructure.Repositories
{
    public interface ITransactionRepository
    {
        Task<Transaction> GetByIdAsync(Guid id);
        Task<IEnumerable<Transaction>> GetByCoupleIdAsync(Guid coupleId, int page, int pageSize);
        Task<IEnumerable<Transaction>> GetByCoupleIdAndDateRangeAsync(Guid coupleId, DateTime from, DateTime to);
        Task<IEnumerable<Transaction>> GetByCategoryAsync(Guid coupleId, string category);
        Task<Transaction> CreateAsync(Transaction transaction);
        Task<Transaction> UpdateAsync(Transaction transaction);
        Task<bool> DeleteAsync(Guid id);
        Task<long> CountByCoupleIdAsync(Guid coupleId);
        Task<decimal> GetTotalAmountByCoupleIdAsync(Guid coupleId, DateTime from, DateTime to);
    }
}
```

### TransactionRepository.cs
```csharp
using MongoDB.Driver;
using WeCount.Domain.Entities;
using WeCount.Infrastructure.Data;

namespace WeCount.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IMongoCollection<Transaction> _transactions;
        private readonly IMongoCollection<User> _users;

        public TransactionRepository(IMongoDbContext context)
        {
            _transactions = context.Transactions;
            _users = context.Users;
        }

        public async Task<Transaction> GetByIdAsync(Guid id)
        {
            var filter = Builders<Transaction>.Filter.Eq(t => t.Id, id);
            return await _transactions.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByCoupleIdAsync(Guid coupleId, int page, int pageSize)
        {
            var filter = Builders<Transaction>.Filter.Eq(t => t.CoupleId, coupleId);
            var sort = Builders<Transaction>.Sort.Descending(t => t.Date);
            
            return await _transactions
                .Find(filter)
                .Sort(sort)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByCoupleIdAndDateRangeAsync(Guid coupleId, DateTime from, DateTime to)
        {
            var filter = Builders<Transaction>.Filter.And(
                Builders<Transaction>.Filter.Eq(t => t.CoupleId, coupleId),
                Builders<Transaction>.Filter.Gte(t => t.Date, from),
                Builders<Transaction>.Filter.Lte(t => t.Date, to)
            );
            
            var sort = Builders<Transaction>.Sort.Descending(t => t.Date);
            
            return await _transactions
                .Find(filter)
                .Sort(sort)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByCategoryAsync(Guid coupleId, string category)
        {
            var filter = Builders<Transaction>.Filter.And(
                Builders<Transaction>.Filter.Eq(t => t.CoupleId, coupleId),
                Builders<Transaction>.Filter.Eq(t => t.Category, category)
            );
            
            return await _transactions.Find(filter).ToListAsync();
        }

        public async Task<Transaction> CreateAsync(Transaction transaction)
        {
            transaction.Id = Guid.NewGuid();
            transaction.CreatedAt = DateTime.UtcNow;
            transaction.UpdatedAt = DateTime.UtcNow;

            // Récupérer les informations utilisateur pour dénormalisation
            var user = await _users.Find(u => u.Id == transaction.UserId).FirstOrDefaultAsync();
            if (user != null)
            {
                transaction.UserName = $"{user.FirstName} {user.LastName}";
                transaction.UserEmail = user.Email;
            }

            await _transactions.InsertOneAsync(transaction);
            return transaction;
        }

        public async Task<Transaction> UpdateAsync(Transaction transaction)
        {
            transaction.UpdatedAt = DateTime.UtcNow;
            
            var filter = Builders<Transaction>.Filter.Eq(t => t.Id, transaction.Id);
            await _transactions.ReplaceOneAsync(filter, transaction);
            
            return transaction;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var filter = Builders<Transaction>.Filter.Eq(t => t.Id, id);
            var result = await _transactions.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        public async Task<long> CountByCoupleIdAsync(Guid coupleId)
        {
            var filter = Builders<Transaction>.Filter.Eq(t => t.CoupleId, coupleId);
            return await _transactions.CountDocumentsAsync(filter);
        }

        public async Task<decimal> GetTotalAmountByCoupleIdAsync(Guid coupleId, DateTime from, DateTime to)
        {
            var filter = Builders<Transaction>.Filter.And(
                Builders<Transaction>.Filter.Eq(t => t.CoupleId, coupleId),
                Builders<Transaction>.Filter.Gte(t => t.Date, from),
                Builders<Transaction>.Filter.Lte(t => t.Date, to)
            );

            var pipeline = new[]
            {
                new BsonDocument("$match", filter.Render(BsonSerializer.SerializerRegistry.GetSerializer<Transaction>(), BsonSerializer.SerializerRegistry)),
                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", BsonNull.Value },
                    { "total", new BsonDocument("$sum", "$amount") }
                })
            };

            var result = await _transactions.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
            return result?["total"].AsDecimal ?? 0;
        }
    }
}
```

## 5. Commands (identiques à l'exemple précédent)

### CreateTransactionCommandHandler.cs (adapté pour MongoDB)
```csharp
using MediatR;
using WeCount.Domain.Entities;
using WeCount.Infrastructure.Repositories;

namespace WeCount.Features.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, CreateTransactionResponse>
    {
        private readonly ITransactionRepository _transactionRepository;

        public CreateTransactionCommandHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<CreateTransactionResponse> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = new Transaction
            {
                Amount = request.Amount,
                Description = request.Description,
                Category = request.Category,
                Date = request.Date,
                UserId = request.UserId,
                CoupleId = request.CoupleId,
                Type = request.Type
            };

            var createdTransaction = await _transactionRepository.CreateAsync(transaction);

            return new CreateTransactionResponse
            {
                Id = createdTransaction.Id,
                Amount = createdTransaction.Amount,
                Description = createdTransaction.Description,
                Category = createdTransaction.Category,
                Date = createdTransaction.Date,
                Type = createdTransaction.Type,
                UserName = createdTransaction.UserName,
                CreatedAt = createdTransaction.CreatedAt
            };
        }
    }
}
```

## 6. Queries avec agrégations MongoDB

### GetTransactionAnalyticsQuery.cs
```csharp
using MediatR;
using WeCount.Domain.Enums;

namespace WeCount.Features.Transactions.Queries.GetTransactionAnalytics
{
    public class GetTransactionAnalyticsQuery : IRequest<GetTransactionAnalyticsResponse>
    {
        public Guid CoupleId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class GetTransactionAnalyticsResponse
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetAmount { get; set; }
        public IEnumerable<CategorySummary> CategoryBreakdown { get; set; }
        public IEnumerable<MonthlySummary> MonthlyTrends { get; set; }
    }

    public class CategorySummary
    {
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public int TransactionCount { get; set; }
    }

    public class MonthlySummary
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Income { get; set; }
        public decimal Expenses { get; set; }
    }
}
```

### GetTransactionAnalyticsQueryHandler.cs
```csharp
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using WeCount.Domain.Entities;
using WeCount.Domain.Enums;
using WeCount.Infrastructure.Data;

namespace WeCount.Features.Transactions.Queries.GetTransactionAnalytics
{
    public class GetTransactionAnalyticsQueryHandler : IRequestHandler<GetTransactionAnalyticsQuery, GetTransactionAnalyticsResponse>
    {
        private readonly IMongoCollection<Transaction> _transactions;

        public GetTransactionAnalyticsQueryHandler(IMongoDbContext context)
        {
            _transactions = context.Transactions;
        }

        public async Task<GetTransactionAnalyticsResponse> Handle(GetTransactionAnalyticsQuery request, CancellationToken cancellationToken)
        {
            var filter = Builders<Transaction>.Filter.And(
                Builders<Transaction>.Filter.Eq(t => t.CoupleId, request.CoupleId),
                Builders<Transaction>.Filter.Gte(t => t.Date, request.From),
                Builders<Transaction>.Filter.Lte(t => t.Date, request.To)
            );

            // Agrégation pour le total des revenus et dépenses
            var totalsPipeline = new[]
            {
                new BsonDocument("$match", filter.Render(BsonSerializer.SerializerRegistry.GetSerializer<Transaction>(), BsonSerializer.SerializerRegistry)),
                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", "$type" },
                    { "total", new BsonDocument("$sum", "$amount") }
                })
            };

            var totalsResult = await _transactions.Aggregate<BsonDocument>(totalsPipeline).ToListAsync();
            
            var totalIncome = totalsResult.FirstOrDefault(r => r["_id"].AsString == TransactionType.Income.ToString())?["total"].AsDecimal ?? 0;
            var totalExpenses = totalsResult.FirstOrDefault(r => r["_id"].AsString == TransactionType.Expense.ToString())?["total"].AsDecimal ?? 0;

            // Agrégation par catégorie
            var categoryPipeline = new[]
            {
                new BsonDocument("$match", filter.Render(BsonSerializer.SerializerRegistry.GetSerializer<Transaction>(), BsonSerializer.SerializerRegistry)),
                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", "$category" },
                    { "amount", new BsonDocument("$sum", "$amount") },
                    { "count", new BsonDocument("$sum", 1) }
                }),
                new BsonDocument("$sort", new BsonDocument("amount", -1))
            };

            var categoryResult = await _transactions.Aggregate<BsonDocument>(categoryPipeline).ToListAsync();
            
            var categoryBreakdown = categoryResult.Select(r => new CategorySummary
            {
                Category = r["_id"].AsString,
                Amount = r["amount"].AsDecimal,
                TransactionCount = r["count"].AsInt32
            });

            // Agrégation mensuelle
            var monthlyPipeline = new[]
            {
                new BsonDocument("$match", filter.Render(BsonSerializer.SerializerRegistry.GetSerializer<Transaction>(), BsonSerializer.SerializerRegistry)),
                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", new BsonDocument
                        {
                            { "year", new BsonDocument("$year", "$date") },
                            { "month", new BsonDocument("$month", "$date") },
                            { "type", "$type" }
                        }
                    },
                    { "amount", new BsonDocument("$sum", "$amount") }
                }),
                new BsonDocument("$sort", new BsonDocument("_id.year", 1).Add("_id.month", 1))
            };

            var monthlyResult = await _transactions.Aggregate<BsonDocument>(monthlyPipeline).ToListAsync();
            
            var monthlyTrends = monthlyResult
                .GroupBy(r => new { Year = r["_id"]["year"].AsInt32, Month = r["_id"]["month"].AsInt32 })
                .Select(g => new MonthlySummary
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Income = g.FirstOrDefault(x => x["_id"]["type"].AsString == TransactionType.Income.ToString())?["amount"].AsDecimal ?? 0,
                    Expenses = g.FirstOrDefault(x => x["_id"]["type"].AsString == TransactionType.Expense.ToString())?["amount"].AsDecimal ?? 0
                });

            return new GetTransactionAnalyticsResponse
            {
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                NetAmount = totalIncome - totalExpenses,
                CategoryBreakdown = categoryBreakdown,
                MonthlyTrends = monthlyTrends
            };
        }
    }
}
```

## 7. Configuration dans Program.cs

```csharp
using MongoDB.Driver;
using WeCount.Infrastructure.Data;
using WeCount.Infrastructure.Repositories;
using WeCount.Infrastructure.Settings;
using MediatR;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Configuration MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// MongoDB Context
builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

// Repositories
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

## 8. Index MongoDB recommandés

```javascript
// Dans MongoDB Shell ou MongoDB Compass

// Index pour les transactions
db.transactions.createIndex({ "coupleId": 1, "date": -1 })
db.transactions.createIndex({ "coupleId": 1, "category": 1 })
db.transactions.createIndex({ "coupleId": 1, "type": 1 })
db.transactions.createIndex({ "userId": 1 })
db.transactions.createIndex({ "date": -1 })

// Index pour les utilisateurs
db.users.createIndex({ "email": 1 }, { unique: true })

// Index pour les couples
db.couples.createIndex({ "members.userId": 1 })
```

## Avantages de MongoDB pour WeCount

1. **Flexibilité du schéma** : Facilite l'évolution des modèles
2. **Performance des requêtes** : Optimisé pour les requêtes complexes
3. **Agrégations puissantes** : Idéal pour les analytics financières
4. **Scalabilité horizontale** : Croissance avec l'application
5. **Dénormalisation** : Optimisation des lectures fréquentes

Cette implémentation tire parti des forces de MongoDB tout en maintenant une architecture CQRS propre et maintenable.
