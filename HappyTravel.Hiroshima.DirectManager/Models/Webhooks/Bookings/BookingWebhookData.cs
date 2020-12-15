namespace HappyTravel.Hiroshima.DirectManager.Models.Webhooks.Bookings
{
    public class BookingWebhookData
    {
        public BookingStatusData Data { get; set; }
        
        public string Signature { get; set; }
        
        public long Timestamp { get; set; }
    }
}