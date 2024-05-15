using Cqrs.Core.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
// ReSharper disable once ConvertToPrimaryConstructor

namespace NedoSilpo.Query.Infrastructure.Consumers;

public class ConsumerHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ConsumerHostedService> _logger;

    public ConsumerHostedService(IServiceProvider serviceProvider, ILogger<ConsumerHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting consumer hosted service");

        using var scope = _serviceProvider.CreateScope();
        var eventConsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
        var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC")
            ?? throw new Exception("Could not find KAFKA_TOPIC environment variable");

        Task.Run(() => eventConsumer.Consume(topic), cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping consumer hosted service");
        return Task.CompletedTask;
    }
}
