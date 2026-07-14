using MassTransit;
using NotificationApi.Consumer;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
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
        x.AddConsumer<UserCreatedNotificationConsumer>();
        x.AddConsumer<PaymentProcessedNotificationConsumer>();
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

            cfg.ReceiveEndpoint("user-created-notification", e =>
            {
                e.Consumer<UserCreatedNotificationConsumer>(ctx);
            });

            cfg.ReceiveEndpoint("payment-processed-notification", e =>
            {
                e.Consumer<PaymentProcessedNotificationConsumer>(ctx);
            });

        });
    });

    builder.Services.AddOptions<MassTransitHostOptions>()
    .Configure(options =>
    {        
        options.WaitUntilStarted = true;
    });    

    var app = builder.Build();    

    Log.Information("Pipeline successfully configured and application initialized...");

    await app.RunAsync();
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