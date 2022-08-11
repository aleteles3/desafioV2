using Application.Users.ViewModels.Auth;
using Application.Users.ViewModels.Crud;

namespace Application.Users.Interfaces;

public interface IUserAppService
{
    Task<Guid?> AddUser(AddUserViewModel addUserViewModel);
    Task<TokenViewModel> GenerateUserAuthToken(UserAuthViewModel userAuthViewModel);
    Task<TokenViewModel> RefreshToken(RefreshTokenViewModel refreshTokenViewModel);
}