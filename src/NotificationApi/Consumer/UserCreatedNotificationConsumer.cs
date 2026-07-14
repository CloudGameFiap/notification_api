using MassTransit;
using CloudGame.Domain.Events.User;

namespace NotificationApi.Consumer
{
    public class UserCreatedNotificationConsumer(ILogger<UserCreatedNotificationConsumer> logger) : IConsumer<UserCreatedEvent>
    {
        private readonly ILogger<UserCreatedNotificationConsumer> _logger = logger;

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            logger.LogInformation("UserCreatedEvent received.");
            var user = context.Message;

            _logger.LogInformation(
                "Boas vindas {Nome} ao CloudGame!!",
                user.Name
                );

            logger.LogInformation("UserCreatedEvent processed.");
        }
    }
}
