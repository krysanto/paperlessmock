using Paperless.OCR;
using Paperless.rabbitmq;
using Paperless.services;

public class HostedService : IHostedService
{
    private readonly IQueueConsumer _consumer;
    private readonly ILogger<HostedService> _logger;

    public HostedService(IQueueConsumer consumer, ILogger<HostedService> logger)
    {
        _consumer = consumer;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _consumer.StartReceive();
        _consumer.OnReceived += Consumer_OnReceived;
        return Task.CompletedTask;
    }

    private void Consumer_OnReceived(object sender, QueueReceivedEventArgs e)
    {
        // Process the message here
        _logger.LogInformation($"Received message: {e.Content}");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer.StopReceive();

        return Task.CompletedTask;
    }
}
