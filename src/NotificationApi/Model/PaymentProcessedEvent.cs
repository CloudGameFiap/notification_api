namespace NotificationApi.Model
{
    public class PaymentProcessedEvent
    {
        public int paymentiD {  get; set; }
        public string? nome { get; set; }
        public string? email { get; set; }
        public string? status { get; set; }
    }
}
