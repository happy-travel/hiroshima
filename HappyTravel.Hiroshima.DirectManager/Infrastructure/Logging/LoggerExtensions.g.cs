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
            
            InvitationCreatedOccured = LoggerMessage.Define<string>(LogLevel.Information,
                new EventId(7002, "InvitationCreated"),
                $"INFORMATION | ManagerInvitationService: {{message}}");

            ManagerRegistrationFailedOccured = LoggerMessage.Define<string>(LogLevel.Warning,
                new EventId(7003, "ManagerRegistrationFailed"),
                $"WARNING | ManagerRegistrationService: {{message}}");

            ManagerRegistrationSuccessOccured = LoggerMessage.Define<string>(LogLevel.Information,
                new EventId(7004, "ManagerRegistrationSuccess"),
                $"INFORMATION | ManagerRegistrationService: {{message}}");

        }

        public static void LogBookingWebhookClientException(this ILogger logger, Exception exception)
            => BookingWebhookClientExceptionOccured(logger, exception);

        public static void LogInvitationCreated(this ILogger logger, string message)
            => InvitationCreatedOccured(logger, message, null);

        public static void LogManagerRegistrationFailed(this ILogger logger, string message)
           => ManagerRegistrationFailedOccured(logger, message, null);

        public static void LogManagerRegistrationSuccess(this ILogger logger, string message)
           => ManagerRegistrationSuccessOccured(logger, message, null);


        private static readonly Action<ILogger, Exception> BookingWebhookClientExceptionOccured;

        private static readonly Action<ILogger, string, Exception> InvitationCreatedOccured;

        private static readonly Action<ILogger, string, Exception> ManagerRegistrationFailedOccured;

        private static readonly Action<ILogger, string, Exception> ManagerRegistrationSuccessOccured;
    }
}