namespace Domain.MassTransit.Interfaces;

public interface IMassTransit
{
    Task PublishMessage(string queueName, MessageModel messageModel);
}