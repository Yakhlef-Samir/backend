namespace WeCount.Infrastructure.MongoDB;

public class MongoDbSettings
{
    public required string ConnectionString { get; set; }
    public required string DatabaseName { get; set; }
}
