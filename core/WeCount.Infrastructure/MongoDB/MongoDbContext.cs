using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace WeCount.Infrastructure.MongoDB;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    private IMongoDatabase Database => _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }
}
