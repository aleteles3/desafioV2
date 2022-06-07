namespace Domain.Core.Interfaces;

public interface IUserToken
{
    string GetUserLogin();
    Guid? GetUserId();
}