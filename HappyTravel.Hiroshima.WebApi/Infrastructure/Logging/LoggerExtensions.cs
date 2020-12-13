using Microsoft.Extensions.Logging;
using System;

namespace HappyTravel.Hiroshima.WebApi.Infrastructure.Logging
{
    public static class LoggerExtensions
    {
        static LoggerExtensions()
        {
            InvitationCreatedOccured = LoggerMessage.Define<string>(LogLevel.Information,
                new EventId(1006, "InvitationCreated"),
                $"INFORMATION | ManagerRegistratioService: {{message}}");

            ManagerRegistrationFailedOccured = LoggerMessage.Define<string>(LogLevel.Warning,
                new EventId(1007, "ManagerRegistrationFailed"),
                $"WARNING | ManagerRegistrationService: {{message}}");

            ManagerRegistrationSuccessOccured = LoggerMessage.Define<string>(LogLevel.Information,
                new EventId(1008, "ManagerRegistrationSuccess"),
                $"INFORMATION | ManagerRegistrationService: {{message}}");
        }


        public static void LogInvitationCreated(this ILogger logger, string message)
            => InvitationCreatedOccured(logger, message, null);

        public static void LogManagerRegistrationFailed(this ILogger logger, string message)
           => ManagerRegistrationFailedOccured(logger, message, null);

        public static void LogManagerRegistrationSuccess(this ILogger logger, string message)
           => ManagerRegistrationSuccessOccured(logger, message, null);


        private static readonly Action<ILogger, string, Exception> InvitationCreatedOccured;

        private static readonly Action<ILogger, string, Exception> ManagerRegistrationFailedOccured;

        private static readonly Action<ILogger, string, Exception> ManagerRegistrationSuccessOccured;
    }
}
