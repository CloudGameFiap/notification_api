using MassTransit;
using NotificationApi.Consumer;
using CloudGame.Domain.Events.User;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserCreatedConsumer>();
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
app.UseSwagger();
app.UseSwaggerUI();
app.Run();
