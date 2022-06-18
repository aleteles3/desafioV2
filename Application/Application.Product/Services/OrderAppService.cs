using Application.Core.Services;
using Application.Product.Interfaces;
using Application.Product.ViewModels.Crud;
using AutoMapper;
using Domain.Core.Interfaces;
using Domain.Product.Cqrs.Order.Commands;
using Domain.Product.Interfaces;
using MediatR;

namespace Application.Product.Services;

public class OrderAppService : AppServiceCore<IOrderRepository>, IOrderAppService
{
    private readonly IUserToken _userToken;

    protected OrderAppService(IMapper mapper, IOrderRepository repository, IMediator mediator, IMemoryBus memoryBus,
        IUserToken userToken)
        : base(mapper, repository, mediator, memoryBus)
    {
        _userToken = userToken;
    }

    public async Task AddOrder(AddOrderViewModel addOrderViewModel)
    {
        var command = Mapper.Map<OrderAddCommand>(addOrderViewModel, opt =>
        {
            opt.Items["UserId"] = _userToken.GetUserId();
        });

        await Mediator.Send(command);
        
        //ToDo Send Menssage to Process Order
    }
}