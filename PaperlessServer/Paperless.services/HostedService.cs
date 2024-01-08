using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.DataModel.Args;
using Paperless.OCR;
using Paperless.rabbitmq;
using Paperless.rest;
using Paperless.services;

public class HostedService : IHostedService
{
    private readonly IQueueConsumer _consumer;
    private readonly ILogger<HostedService> _logger;
    private readonly IOcrClient _ocrClient;
    private readonly IMinioClient _minioClient;
    private readonly DefaultDbContext _context;

    public HostedService(IQueueConsumer consumer, ILogger<HostedService> logger, IOcrClient ocrClient, IMinioClient minioClient, DefaultDbContext context)
    {
        _consumer = consumer;
        _logger = logger;
        _ocrClient = ocrClient;
        _minioClient = minioClient;
        _context = context;
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

                var document = await _context.Documents
                    .FirstOrDefaultAsync(d => d.StoragePath == uniqueFileName);

                if (document != null)
                {
                    document.Content = ocrContentText;
                    document.Modified = DateTime.UtcNow;
                    _context.Documents.Update(document);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Document content updated in database.");
                }
                else
                {
                    _logger.LogWarning($"Document with unique name '{uniqueFileName}' not found in database.");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing file: {ex.Message}");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer.StopReceive();

        return Task.CompletedTask;
    }
}
