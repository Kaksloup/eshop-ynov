using BuildingBlocks.Behaviors;
using Discount.Grpc.Data;
using Discount.Grpc.Data.Repositories;
using Discount.Grpc.Services;
using FluentValidation;
using Marten;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddGrpc();

// Marten - PostgreSQL Document Database (same as Catalog)
builder.Services.AddMarten(options =>
{
    options.Connection(configuration.GetConnectionString("DiscountConnection") ?? string.Empty);
    options.ConfigureMarten();
}).UseLightweightSessions();

// Repository Pattern
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

// Mediator Pattern - CQRS
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountServiceServer>();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();