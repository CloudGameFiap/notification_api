using CloudGame.Domain.Model;
using MassTransit;

namespace NotificationApi.Consumer
{
    public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
    {
        private readonly ILogger<UserCreatedConsumer> _logger;
        public UserCreatedConsumer(ILogger<UserCreatedConsumer> logger) {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var user = context.Message;

            _logger.LogInformation(
                "Boas vindas {Nome} ao CloudGame!!",
                user.Nome
                );
        }
    }
}
