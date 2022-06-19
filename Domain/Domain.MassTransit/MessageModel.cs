namespace Domain.MassTransit;

public class MessageModel
{
    public string Message { get; set; }

    public MessageModel(string message)
    {
        Message = message;
    }
}