using Application.Product.Interfaces;
using Domain.MassTransit;
using MassTransit;

namespace Service.Product.Consumers;

public class AcceptOrderConsumer : IConsumer<MessageModel>
{
    private readonly IOrderAppService _orderAppService;
    
    public AcceptOrderConsumer(IOrderAppService orderAppService)
    {
        _orderAppService = orderAppService;
    }

    public async Task Consume(ConsumeContext<MessageModel> context)
    {
        var parsed = Guid.TryParse(context.Message.Message, out var orderId);

        if (!parsed)
        {
            Console.WriteLine("Failed to parse the message.");
            return;
        }

        await _orderAppService.AcceptOrder(orderId);
    }
}