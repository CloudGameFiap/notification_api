using Amazon.Lambda.Core;
using Amazon.Lambda.MQEvents;
using CloudGame.Contracts.Events;
using CloudGame.Domain.Events.User;
using NotificationApi.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;



[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace NotificationApi.Handler
{
    public class NotificationHandler
    {
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task FunctionHandler(RabbitMQEvent rmqEvent, ILambdaContext context)
        {
            var service = new NotificacaoService(context);

            foreach (var (queueName, mensagens) in rmqEvent.RmqMessagesByQueue)
            {
                foreach (var msg in mensagens)
                {
                    var bodyBytes = Convert.FromBase64String(msg.Data);
                    var json = Encoding.UTF8.GetString(bodyBytes);

                    context.Logger.LogInformation($"Fila: {queueName} | Payload: {json}");

                    if (queueName.Contains("UserCreated", StringComparison.OrdinalIgnoreCase))
                    {
                        var envelope = JsonSerializer.Deserialize<MassTransitEnvelope<UserCreatedEvent>>(json, _jsonOptions);
                        if (envelope?.Message is not null)
                            await service.EnviarBoasVindas(envelope.Message);
                    }
                    else if (queueName.Contains("PaymentProcessed", StringComparison.OrdinalIgnoreCase))
                    {
                        var envelope = JsonSerializer.Deserialize<MassTransitEnvelope<PaymentProcessedEvent>>(json, _jsonOptions);
                        if (envelope?.Message is not null)
                            await service.ProcessarPagamento(envelope.Message);
                    }
                    else
                    {
                        context.Logger.LogWarning($"Fila não mapeada: {queueName}");
                    }
                }
            }
        }
    }

    public class MassTransitEnvelope<T>
    {
        public string MessageId { get; set; } = "";
        public string ConversationId { get; set; } = "";
        public string[] MessageType { get; set; } = Array.Empty<string>();
        public T Message { get; set; } = default!;
        public DateTime SentTime { get; set; }
    }
}
