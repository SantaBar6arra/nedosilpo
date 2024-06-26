using Cqrs.Core.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
// ReSharper disable once ConvertToPrimaryConstructor

namespace NedoSilpo.Query.Infrastructure.Consumers;

public class ConsumerHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ConsumerHostedService> _logger;

    public ConsumerHostedService(IServiceProvider serviceProvider, ILogger<ConsumerHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting consumer hosted background service");

        using var scope = _serviceProvider.CreateScope();
        var eventConsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
        var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC")
                    ?? throw new Exception("Could not find KAFKA_TOPIC environment variable");

        await Task.Run(() => eventConsumer.Consume(topic), stoppingToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping consumer hosted service");
        return Task.CompletedTask;
    }
}
