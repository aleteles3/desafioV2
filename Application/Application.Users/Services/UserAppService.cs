using Application.Core.Services;
using Application.Users.Interfaces;
using Application.Users.ViewModels.Crud;
using AutoMapper;
using Domain.Core.Interfaces;
using Domain.User.Cqrs.User.Commands;
using Domain.User.Interfaces;
using MediatR;

namespace Application.Users.Services
{
    public partial class UserAppService : AppServiceCore<IUserRepository>, IUserAppService
    {
        private readonly ISecurity _security;

        public UserAppService(IMapper mapper, IUserRepository repository, IMediator mediator, IMemoryBus memoryBus,
            ISecurity security)
            : base(mapper, repository, mediator, memoryBus)
        {
            _security = security;
        }

        public async Task<Guid?> AddUser(AddUserViewModel addUserViewModel)
        {
            var command = Mapper.Map<UserAddCommand>(addUserViewModel);
            return await Mediator.Send(command);
        }
    }
}