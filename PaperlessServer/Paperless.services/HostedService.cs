using Paperless.OCR;
using Paperless.rabbitmq;
using Paperless.services;

public class HostedService : IHostedService
{
    private readonly IQueueConsumer _consumer;
    private readonly ILogger<HostedService> _logger;
    private readonly IOcrClient _ocrClient;

    public HostedService(IQueueConsumer consumer, ILogger<HostedService> logger, IOcrClient ocrClient)
    {
        _consumer = consumer;
        _logger = logger;
        _ocrClient = ocrClient;
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
        using FileStream fileStream = new FileStream("/app/HelloWorld.pdf", FileMode.Open);
        _logger.LogInformation($"Received message: {e.Content}");
        var ocrContentText = _ocrClient.OcrPdf(fileStream);
        Console.WriteLine(ocrContentText);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer.StopReceive();

        return Task.CompletedTask;
    }
}
