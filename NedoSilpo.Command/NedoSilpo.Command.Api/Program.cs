using Confluent.Kafka;
using Cqrs.Core.Domain;
using Cqrs.Core.Events;
using Cqrs.Core.Handlers;
using Cqrs.Core.Infrastructure;
using Cqrs.Core.Producers;
using MongoDB.Bson.Serialization;
using NedoSilpo.Command.Api;
using NedoSilpo.Command.Api.Commands;
using NedoSilpo.Command.Domain.Aggregates;
using NedoSilpo.Command.Infrastructure.Config;
using NedoSilpo.Command.Infrastructure.Dispatchers;
using NedoSilpo.Command.Infrastructure.Handlers;
using NedoSilpo.Command.Infrastructure.Producers;
using NedoSilpo.Command.Infrastructure.Repositories;
using NedoSilpo.Command.Infrastructure.Stores;
using NedoSilpo.Common.Events;

var builder = WebApplication.CreateBuilder(args);

// todo replace this obvious shit
BsonClassMap.RegisterClassMap<BaseEvent>();
BsonClassMap.RegisterClassMap<ProductCreated>();
BsonClassMap.RegisterClassMap<ProductUpdated>();
BsonClassMap.RegisterClassMap<ProductSold>();
BsonClassMap.RegisterClassMap<ProductRemoved>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventProducer, EventProducer>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventSourcingHandler<ProductAggregate>, EventSourcingHandler>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();

// registering a command handlers
var commandHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
var dispatcher = new CommandDispatcher();
dispatcher.Register<CreateProduct>(commandHandler.HandleAsync);
dispatcher.Register<UpdateProduct>(commandHandler.HandleAsync);
dispatcher.Register<SellProduct>(commandHandler.HandleAsync);
dispatcher.Register<RemoveProduct>(commandHandler.HandleAsync);
builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

namespace NedoSilpo.Command.Api
{
    record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
