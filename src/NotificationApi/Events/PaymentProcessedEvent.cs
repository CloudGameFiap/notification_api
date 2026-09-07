namespace CloudGameCatalog.Consumer.Consumers.PaymentApi.PaymentProcessed;

public class PaymentProcessedEvent
{
    public int GameId { get; set; }

    public int UserId { get; set; }

    public UserGameStatus Status { get; set; }
}

public enum UserGameStatus
{
    WaitingPayment = 1,
    PaymentApproved = 2,
    PaymentRejected = 3,
}
