using MassTransit;
using NotificationApi.Consumer;
using Serilog;
using CloudGame.Domain.Events.User;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting up the application...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

    builder.Services.AddSwaggerGen();

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

        //cfg.Message<UserCreatedEvent>(m => m.SetEntityName("UserCreatedEvent"));

        //cfg.ReceiveEndpoint("UserCreatedEvent", e =>
        //{
        //    e.ConfigureConsumer<UserCreatedConsumer>(ctx);

        //    //// Vincula a fila diretamente ao exchange que o producer usa
        //    //e.Bind("CloudGame.Domain.Events.User:UserCreatedEvent", s =>
        //    //{
        //    //    s.ExchangeType = RabbitMQ.Client.ExchangeType.Direct;
        //    //});
        //});

        //cfg.Publish<UserCreatedEvent>(p =>
        //{
        //    p.ExchangeType = RabbitMQ.Client.ExchangeType.Direct;
        //});

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