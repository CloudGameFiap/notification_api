using MassTransit;
using NotificationApi.Model;

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

            if (payment.status == "Approved")
            {
                _logger.LogInformation(
                    "Pagamento aprovado! Enviando e-mail de confirmação para {Email}",
                    payment.email
                );

                // Aqui envia o e-mail de confirmação
            }
            else
            {
                _logger.LogInformation(
                    "Pagamento recusado para {Email}, nenhum e-mail enviado.",
                    payment.email
                );
            }
        }
    }
}
