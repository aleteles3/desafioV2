using Domain.MassTransit.Interfaces;
using MassTransit;

namespace Domain.MassTransit;

public class MassTransit : IMassTransit
{
    private IBus Bus { get; set; }
    
    public MassTransit(IBus bus)
    {
        Bus = bus;
    }
    public async Task PublishMessage(string queueName, MessageModel messageModel)
    {
        var uri = new Uri($"rabbitmq://localhost/{queueName}");
        var endpoint = await Bus.GetSendEndpoint(uri);
        await endpoint.Send(messageModel);
    }
}