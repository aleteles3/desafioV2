using Domain.NoSql;
using Domain.NoSql.UserToken;
using Domain.NoSql.UserToken.Interfaces;
using MongoDB.Driver;

namespace Infra.Data.NoSql;

public class UserTokenRepository : MongoDbRepository, IUserTokenRepository
{
    private readonly IMongoDatabase _mongoDatabase;
    private readonly IMongoCollection<UserRefreshToken> _userTokenCollection;

    public UserTokenRepository(MongoDbSettings mongoDbSettings) : base(mongoDbSettings)
    {
        _mongoDatabase = MongoClient.GetDatabase(mongoDbSettings.Database);
        _userTokenCollection = _mongoDatabase.GetCollection<UserRefreshToken>(nameof(UserRefreshToken));
    }
    
    public async Task<UserRefreshToken> GetUserToken(Guid userId)
    {
        var userTokens = await _userTokenCollection.FindAsync(x => x.UserId == userId);
        return userTokens.FirstOrDefault();
    }

    public async Task InsertUserToken(UserRefreshToken userRefreshToken)
    {
        await _userTokenCollection.InsertOneAsync(userRefreshToken);
    }

    public async Task DeleteUserToken(Guid userId)
    {
        await _userTokenCollection.DeleteOneAsync(x => x.UserId == userId);
    }
}