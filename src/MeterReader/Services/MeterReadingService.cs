using Grpc.Core;
using MeterReader.gRPC;

namespace MeterReader.Services;

public class MeterReadingService : MeterReader.gRPC.MeterReadingService.MeterReadingServiceBase
{
    private readonly IReadingRepository _repository;
    private readonly ILogger<MeterReadingService> _logger;

    public MeterReadingService(IReadingRepository repository, ILogger<MeterReadingService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public override async Task<StatusMessage> AddReading(ReadingPacket request, ServerCallContext context)
    {
        if (request.Successful != ReadingStatus.Success)
            return new StatusMessage()
            {
                Message = "Failed to store to the database",
                Status = ReadingStatus.Failure
            };
        
        foreach (var reading in request.Readings)
        {
            var readingValue = new MeterReading()  // Entity type for storing to db, not from protobuf
            {
                CustomerId = reading.CustomerId,
                Value = reading.ReadingValue,
                ReadingDate = reading.ReadingTime.ToDateTime()
            };

            _repository.AddEntity(readingValue);
        }

        if (await _repository.SaveAllAsync())
        {
            return new StatusMessage()
            {
                Message = "Successfully added to the database",
                Status = ReadingStatus.Success
            };
        }

        return new StatusMessage()
        {
            Message = "Failed to store to the database",
            Status = ReadingStatus.Failure
        };
    }
}