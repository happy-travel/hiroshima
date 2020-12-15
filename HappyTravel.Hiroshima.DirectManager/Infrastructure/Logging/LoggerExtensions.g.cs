using System;
using Microsoft.Extensions.Logging;

namespace HappyTravel.Hiroshima.DirectManager.Infrastructure.Logging
{
    public static class LoggerExtensions
    {
        static LoggerExtensions()
        {
            BookingWebhookClientExceptionOccured = LoggerMessage.Define(LogLevel.Error,
                new EventId(7001, "BookingWebhookClientException"),
                $"ERROR | BookingWebhookClient: ");
            
        }
    
                
         public static void LogBookingWebhookClientException(this ILogger logger, Exception exception)
            => BookingWebhookClientExceptionOccured(logger, exception);
    
    
        
        private static readonly Action<ILogger, Exception> BookingWebhookClientExceptionOccured;
    }
}