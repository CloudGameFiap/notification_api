using CloudGameCatalog.Consumer.Consumers.PaymentApi.PaymentProcessed;
using MassTransit;

namespace NotificationApi.Consumer
{
    public class PaymentProcessedNotificationConsumer(ILogger<PaymentProcessedNotificationConsumer> logger) : IConsumer<PaymentProcessedEvent>
    {

        private readonly ILogger<PaymentProcessedNotificationConsumer> _logger = logger;

        public async Task Consume(ConsumeContext<PaymentProcessedEvent> context)
        {
            logger.LogInformation("PaymentProcessedEvent received.");

            var payment = context.Message;

            if (payment.Status == UserGameStatus.PaymentApproved)
            {
                _logger.LogInformation(
                    "Pagamento aprovado! Enviando e-mail a aprovação"
                );

                // Aqui envia o e-mail de confirmação
            }
            else
            {
                _logger.LogInformation(
                    "Pagamento recusado."
                );
            }

            logger.LogInformation("PaymentProcessedEvent processed.");
        }
    }
}
