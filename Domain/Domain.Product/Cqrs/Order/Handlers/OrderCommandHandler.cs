using AutoMapper;
using Domain.Core.Commands;
using Domain.Core.Enums.Product;
using Domain.Core.Interfaces;
using Domain.Product.Cqrs.Order.Commands;
using Domain.Product.Interfaces;
using MediatR;
using OrderDomain = Domain.Product.Entities.Order;

namespace Domain.Product.Cqrs.Order.Handlers;

public partial class OrderCommandHandler : CommandHandler,
    IRequestHandler<OrderAddCommand, Guid?>,
    IRequestHandler<OrderAcceptCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    
    public OrderCommandHandler(IOrderRepository orderRepository,
        IProductRepository productRepository, IMapper mapper, IMemoryBus memoryBus) : base(memoryBus)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Guid?> Handle(OrderAddCommand request, CancellationToken cancellationToken)
    {
        var order = _mapper.Map<OrderDomain>(request);

        if (!order.IsValid())
        {
            NotifyValidationErrors(order.ValidationResult);

            return null;
        }

        try
        {
            await _orderRepository.BeginTransactionAsync();
            await _orderRepository.AddOrderAsync(order);
            await _orderRepository.CommitTransactionAsync();

            return order.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            NotifyValidationErrors("A fatal error occurred. The operation could not be completed.");
            await _orderRepository.RollBackTransactionAsync();
            return null;
        }
    }

    public async Task<Unit> Handle(OrderAcceptCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderByIdAsync(request.OrderId);

        if (order == null)
        {
            NotifyValidationErrors($"The order could not be found. Id: {request.OrderId}");
            return Unit.Value;
        }

        if (order.OrderStatus != OrderStatus.Pending)
        {
            NotifyValidationErrors("The order status does not allow this operation.");
            return Unit.Value;
        }
        
        var productIds = order.OrderItems.Select(x => x.ProductId);
        var products = await _productRepository.GetProductsAsync(x => productIds.Contains(x.Id));

        await CheckOrderItemsAvailability(order, products);

        order.SetOrderStatus(HasValidationErrors() ? OrderStatus.Declined : OrderStatus.Accepted);

        try
        {
            await _orderRepository.BeginTransactionAsync();
            await _orderRepository.UpdateOrderAsync(order);
            if (order.OrderStatus == OrderStatus.Accepted)
                await ReserveOrderItems(order, products);
            await _orderRepository.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            NotifyValidationErrors("A fatal error occurred. The operation could not be completed.");
            await _orderRepository.RollBackTransactionAsync();
        }
        
        return Unit.Value;
    }
}