using Grpc.Net.Client;
using MeterReader.gRPC;

namespace MeterReadingClient;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ReadingGenerator _readingGenerator;
    private readonly int _customerId;
    private readonly string _serviceUrl;

    public Worker(ILogger<Worker> logger, ReadingGenerator readingGenerator, IConfiguration configuration)
    {
        _logger = logger;
        _readingGenerator = readingGenerator;
        _customerId = configuration.GetValue<int>("CustomerId");
        _serviceUrl = configuration["ServiceUrl"];
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            var channel = GrpcChannel.ForAddress(_serviceUrl);
            var client = new MeterReadingService.MeterReadingServiceClient(channel);

            var packet = new ReadingPacket()
            {
                Successful = ReadingStatus.Success
            };

            for (var i = 0; i < 5; i++)
            {
                var reading = await _readingGenerator.GenerateAsync(_customerId);
                packet.Readings.Add(reading);
            }

            var status = client.AddReading(packet);
            _logger.LogInformation(status.Status == ReadingStatus.Success
                ? "Successfully called grpc"
                : "Failed to call grpc");
            
            await Task.Delay(5000, stoppingToken);
        }
    }
}