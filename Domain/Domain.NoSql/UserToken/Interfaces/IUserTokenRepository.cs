namespace Domain.NoSql.UserToken.Interfaces;

public interface IUserTokenRepository
{
    Task<UserRefreshToken> GetUserToken(Guid userId);
    Task InsertUserToken(UserRefreshToken userRefreshToken);
    Task DeleteUserToken(Guid userId);
}