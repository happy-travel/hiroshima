using System;

namespace HappyTravel.Hiroshima.DirectManager.Infrastructure.Options
{
    public class BookingWebhookOptions
    {
        public string Key { get; set; }
        
        public Uri WebhookUrl { get; set; }
    }
}