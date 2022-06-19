namespace Domain.MassTransit.Queues;

public class MessageQueue
{
    public string Name { get; }

    public MessageQueue(string name)
    {
        Name = name;
    }
}