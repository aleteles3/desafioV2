using Application.User.ViewModels.Auth;
using Application.User.ViewModels.Crud;
using Application.Users.ViewModels.Auth;

namespace Application.User.Interfaces;

public interface IUserAppService
{
    Task<Guid?> AddUser(AddUserViewModel addUserViewModel);
    Task<TokenViewModel> GenerateUserAuthToken(UserAuthViewModel userAuthViewModel);
}