using MassTransit;
using NotificationApi.Consumer;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting up the application...");

    var builder = Host.CreateApplicationBuilder(args);

    builder.Services.AddSerilog((hostingContext, configuration) =>
    {
        configuration
            .ReadFrom.Configuration(builder.Configuration);
    });
   

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserCreatedConsumer>();
    x.AddConsumer<PaymentProcessedConsumer>();
    x.UsingRabbitMq((ctx, cfg) =>
    {
        var rabbitMqSection = builder.Configuration.GetRequiredSection("RabbitMQ")!;
        var host = rabbitMqSection["Host"]!;
        var username = rabbitMqSection["Username"]!;
        var password = rabbitMqSection["Password"]!;

        cfg.Host(host, "/", h =>
        {
            h.Username(username);
            h.Password(password);
        });

        cfg.ConfigureEndpoints(ctx);       

        });
    });

    var app = builder.Build();    

    Log.Information("Pipeline successfully configured and application initialized...");

    app.Run();
}
catch (Exception ex) when (ex.GetType().Name != "HostAbortedException")
{
    Log.Fatal(ex, "Application failed to start");
}
catch (Exception)
{
    throw;
}
finally
{
    Log.Information("Shutting down the application...");
    Log.CloseAndFlush();
}