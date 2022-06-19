namespace Domain.MassTransit.Queues;

public static class MessageQueueProduct
{
    public static readonly MessageQueue AcceptOrder = new MessageQueue("AcceptOrder");
}