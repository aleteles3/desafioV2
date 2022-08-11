namespace Domain.NoSql;

public sealed class MongoDbSettings
{
    public string ConnectionUri { get; }
    public string Database { get; }

    public MongoDbSettings(string connectionUri, string database)
    {
        ConnectionUri = connectionUri;
        Database = database;
    }
}