using AutoMapper;
using Domain.Core.Commands;
using Domain.Core.Interfaces;
using Domain.Product.Cqrs.Order.Commands;
using Domain.Product.Interfaces;
using MediatR;
using OrderDomain = Domain.Product.Entities.Order;

namespace Domain.Product.Cqrs.Order.Handlers;

public class OrderCommandHandler : CommandHandler,
    IRequestHandler<OrderAddCommand, Guid?>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    
    protected OrderCommandHandler(IOrderRepository orderRepository,
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
}