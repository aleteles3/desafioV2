using Application.Core.Services;
using Application.Product.Interfaces;
using Application.Product.ViewModels.Crud;
using AutoMapper;
using Domain.Core.Interfaces;
using Domain.MassTransit;
using Domain.MassTransit.Interfaces;
using Domain.MassTransit.Queues;
using Domain.Product.Cqrs.Order.Commands;
using Domain.Product.Interfaces;
using MediatR;

namespace Application.Product.Services;

public class OrderAppService : AppServiceCore<IOrderRepository>, IOrderAppService
{
    private readonly IUserToken _userToken;
    private readonly IMassTransit _massTransit;

    public OrderAppService(IMapper mapper, IOrderRepository repository, IMediator mediator, IMemoryBus memoryBus,
        IUserToken userToken, IMassTransit massTransit)
        : base(mapper, repository, mediator, memoryBus)
    {
        _userToken = userToken;
        _massTransit = massTransit;
    }

    public async Task AddOrder(AddOrderViewModel addOrderViewModel)
    {
        var command = Mapper.Map<OrderAddCommand>(addOrderViewModel, opt =>
        {
            opt.Items["UserId"] = _userToken.GetUserId();
        });

        var result = await Mediator.Send(command);

        await _massTransit.PublishMessage(MessageQueueProduct.AcceptOrder.Name, new MessageModel(result.ToString()));
    }

    public async Task AcceptOrder(Guid orderId)
    {
        var command = new OrderAcceptCommand(orderId);

        await Mediator.Send(command);
    }
}