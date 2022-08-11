using Domain.NoSql;
using MongoDB.Driver;

namespace Infra.Data.NoSql;

public abstract class MongoDbRepository
{
    protected MongoClient MongoClient { get; }
    
    public MongoDbRepository(MongoDbSettings mongoDbSettings)
    {
        MongoClient = new MongoClient(mongoDbSettings.ConnectionUri);
    }
}