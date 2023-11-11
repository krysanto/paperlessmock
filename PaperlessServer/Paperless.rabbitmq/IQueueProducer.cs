namespace Paperless.rabbitmq;

public interface IQueueProducer
{
    void Send(string body, Guid documentId);
}
