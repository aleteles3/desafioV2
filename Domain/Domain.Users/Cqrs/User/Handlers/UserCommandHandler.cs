using Domain.Core.Commands;
using Domain.Core.Interfaces;
using Domain.User.Cqrs.User.Commands;
using Domain.User.Interfaces;
using MediatR;
using UserDomain = Domain.User.Entities.User;

namespace Domain.User.Cqrs.User.Handlers;

public class UserCommandHandler : CommandHandler,
    IRequestHandler<UserAddCommand, Guid?>
{
    private readonly IUserRepository _userRepository;
    private readonly ISecurity _security;
    
    public UserCommandHandler(IUserRepository userRepository, ISecurity security, IMemoryBus memoryBus) 
        : base(memoryBus)
    {
        _userRepository = userRepository;
        _security = security;
    }

    public async Task<Guid?> Handle(UserAddCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetUsers(x => x.Login.ToUpper() == request.Login.ToUpper());

        if (existingUser.Any())
        {
            NotifyValidationErrors("User with the same login already exists.");
            return null;
        }
        
        var user = new UserDomain(request.Login, request.Password);

        if (!user.IsValid())
        {
            NotifyValidationErrors(user.ValidationResult);
            return null;
        }

        var hashPassword = _security.EncryptString(user.Password, user.Login);
        user.SetPassword(hashPassword);

        try
        {
            await _userRepository.BeginTransactionAsync();
            await _userRepository.AddUserAsync(user);
            await _userRepository.CommitTransactionAsync();

            return user.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            NotifyValidationErrors("A fatal error occurred. The operation could not be completed.");
            await _userRepository.RollBackTransactionAsync();

            return null;
        }
    }
}