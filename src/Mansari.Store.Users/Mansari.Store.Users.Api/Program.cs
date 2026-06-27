using Mansari.Store.Users.Api.Services;
using Mansari.Store.Users.Application;
using Mansari.Store.Users.Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
});

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapGrpcService<UsersGrpcService>();

app.MapGet("/", () => Results.Ok("Mansari Store Users gRPC service is running."));
app.MapHealthChecks("/health");

app.Run();
