using CloudGame.Domain.Events.User;
using CloudGame.Contracts.Events;
using Amazon.Lambda.Core;

namespace NotificationApi.Services
{
    public class NotificacaoService
    {
        private readonly ILambdaContext _context;

        public NotificacaoService(ILambdaContext context)
        {
            _context = context;
        }

        public Task EnviarBoasVindas(UserCreatedEvent user)
        {
            _context.Logger.LogInformation(
                $"Boas vindas {user.Name} ao CloudGame!!");

            // Aqui entraria o envio real/simulado do email
            return Task.CompletedTask;
        }

        public Task ProcessarPagamento(PaymentProcessedEvent payment)
        {
            if (payment.Status == "Approved")
            {
                _context.Logger.LogInformation(
                    "Pagamento aprovado! Enviando e-mail de aprovação");

                // Aqui envia o e-mail de confirmação
            }
            else
            {
                _context.Logger.LogInformation("Pagamento recusado.");
            }

            return Task.CompletedTask;
        }
    }
}