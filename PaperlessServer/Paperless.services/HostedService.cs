using Microsoft.AspNetCore.Http.HttpResults;
using Minio;
using Minio.DataModel.Args;
using Paperless.OCR;
using Paperless.rabbitmq;
using Paperless.services;

public class HostedService : IHostedService
{
    private readonly IQueueConsumer _consumer;
    private readonly ILogger<HostedService> _logger;
    private readonly IOcrClient _ocrClient;
    private readonly IMinioClient _minioClient;

    public HostedService(IQueueConsumer consumer, ILogger<HostedService> logger, IOcrClient ocrClient, IMinioClient minioClient)
    {
        _consumer = consumer;
        _logger = logger;
        _ocrClient = ocrClient;
        _minioClient = minioClient;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _consumer.StartReceive();
        _consumer.OnReceived += Consumer_OnReceivedAsync;
        return Task.CompletedTask;
    }

    private async void Consumer_OnReceivedAsync(object sender, QueueReceivedEventArgs e)
    {
        _logger.LogInformation($"Received message: {e.Content}");

        var uniqueFileName = e.Content;
        var bucketName = "documents";

        try
        {
            using (var memoryStream = new MemoryStream())
            {
                await _minioClient.GetObjectAsync(new GetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(uniqueFileName)
                    .WithCallbackStream(s => s.CopyTo(memoryStream)));

                memoryStream.Position = 0;

                var ocrContentText = _ocrClient.OcrPdf(memoryStream);
                Console.WriteLine(ocrContentText);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing file from MinIO: {ex.Message}");
        }

        // Process the message here
        //using FileStream fileStream = new FileStream("/app/HelloWorld.pdf", FileMode.Open);
        //_logger.LogInformation($"Received message: {e.Content}");
        //var ocrContentText = _ocrClient.OcrPdf(fileStream);
        //Console.WriteLine(ocrContentText);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer.StopReceive();

        return Task.CompletedTask;
    }
}
