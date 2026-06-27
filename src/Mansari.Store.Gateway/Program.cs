using Mansari.Store.Gateway.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGatewayServices()
    .AddGrpcClients(builder.Configuration)
    .AddAggregationServices();

var app = builder.Build();

app.MapControllers();

app.Run();