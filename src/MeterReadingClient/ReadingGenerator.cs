using Google.Protobuf.WellKnownTypes;
using MeterReader.gRPC;

namespace MeterReadingClient;

/// <summary>
/// Dummy class for testing. Could be sensor data IRL
/// </summary>
public class ReadingGenerator
{
    public Task<ReadingMessage> GenerateAsync(int customerId)
    {
        var reading = new ReadingMessage()
        {
            CustomerId = customerId,
            ReadingTime = Timestamp.FromDateTime(DateTime.UtcNow),
            ReadingValue = new Random().Next(10000)
        };

        return Task.FromResult(reading);
    }
}