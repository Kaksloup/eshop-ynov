using BuildingBlocks.Behaviors;
using Discount.Grpc.Data;
using Discount.Grpc.Data.Repositories;
using Discount.Grpc.Services;
using FluentValidation;
using Marten;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Configure Kestrel with multiple endpoints
builder.WebHost.ConfigureKestrel(options =>
{
    // gRPC endpoint - HTTP/2 only (port 6062)
    options.ListenAnyIP(6062, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
    
    // REST API endpoint - HTTP/1.1 (port 6063)
    options.ListenAnyIP(6063, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
    });
});

// Add services to the container.
builder.Services.AddGrpc();

// Add REST API Controllers
builder.Services.AddControllers();

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
app.MapControllers();

app.MapGet("/",
    () =>
        "Discount Service - gRPC and REST API endpoints available");

app.Run();