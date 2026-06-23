using CloudGame.Domain.Model;
using MassTransit;
using NotificationApi.Consumer;

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
app.UseSwagger();
app.UseSwaggerUI();
app.Run();
