using Application.Core.Services;
using Application.User.Interfaces;
using Application.User.ViewModels.Crud;
using AutoMapper;
using Domain.User.Cqrs.User.Commands;
using Domain.User.Interfaces;
using MediatR;

namespace Application.User.Services;

public class UserAppService : AppServiceCore<IUserRepository>, IUserAppService
{
    public UserAppService(IMapper mapper, IUserRepository repository, IMediator mediator) 
        : base(mapper, repository, mediator) { }

    public async Task<Guid?> AddUser(AddUserViewModel addUserViewModel)
    {
        var command = Mapper.Map<UserAddCommand>(addUserViewModel);
        return await Mediator.Send(command);
    }
}