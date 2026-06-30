using CloudGame.Domain.Model;
using MassTransit;
using NotificationApi.Consumer;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting up the application...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSwaggerGen();

    builder.Services.AddMassTransit(x =>
    {
        x.AddConsumer<UserCreatedConsumer>();
        x.UsingRabbitMq((ctx, cfg) =>
        {
            cfg.Host("host.docker.internal", "/", h =>
            {
                h.Username("guest");
                h.Password("guest");
            });

            cfg.ReceiveEndpoint("UserCreatedEvent", e =>
            {
                e.ConfigureConsumer<UserCreatedConsumer>(ctx);
            });

            cfg.Publish<UserCreatedEvent>(p =>
            {
                p.ExchangeType = RabbitMQ.Client.ExchangeType.Direct;
            });

        });
    });

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    Log.Information("The application has been built, and star the pipeline setup has started.");

    app.UseSwagger();
    app.UseSwaggerUI();

    Log.Information("Pipeline successfully configured and application initialized...");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.Information("Shutting down the application...");
    Log.CloseAndFlush();
}