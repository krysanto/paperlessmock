using Paperless.OCR;
using Paperless.rabbitmq;
using Paperless.services;

public class HostedService : IHostedService
{
    private readonly IQueueConsumer _consumer;
    private readonly ILogger _logger;

    public HostedService(IQueueConsumer consumer, ILogger logger)
    {
        _consumer = consumer;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _consumer.StartReceive();
        string filePath = "./docs/HelloWorld.pdf";

        try
        {
            using FileStream fileStream = new FileStream(filePath, FileMode.Open);
            using StreamReader reader = new StreamReader(fileStream);
            OcrClient ocrClient = new OcrClient(new OcrOptions());

            var ocrContentText = ocrClient.OcrPdf(fileStream);
            _logger.LogError("works");
        }
        catch (IOException e)
        {
            _logger.LogError("does not work");
        }


        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
