using Application.User.ViewModels.Crud;

namespace Application.User.Interfaces;

public interface IUserAppService
{
    Task<Guid?> AddUser(AddUserViewModel addUserViewModel);
}