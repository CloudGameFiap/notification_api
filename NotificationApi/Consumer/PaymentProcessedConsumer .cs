using CloudGame.Contracts.Events;
using MassTransit;

namespace NotificationApi.Consumer
{
    public class PaymentProcessedConsumer : IConsumer<PaymentProcessedEvent>
    {

        private readonly ILogger<PaymentProcessedConsumer> _logger;

        public PaymentProcessedConsumer(ILogger<PaymentProcessedConsumer> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<PaymentProcessedEvent> context)
        {
            var payment = context.Message;

            if (payment.Status == "Approved")
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
        }
    }
}
