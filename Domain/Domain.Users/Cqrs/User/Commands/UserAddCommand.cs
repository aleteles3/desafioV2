using MediatR;

namespace Domain.User.Cqrs.User.Commands;

public class UserAddCommand : IRequest<Guid?>
{
    public string Login { get; set; }
    public string Password { get; set; }
}