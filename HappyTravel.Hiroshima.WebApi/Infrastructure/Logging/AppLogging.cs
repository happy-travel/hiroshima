using Microsoft.Extensions.Logging;

namespace HappyTravel.Hiroshima.WebApi.Infrastructure.Logging
{
    public static class AppLogging
    {
        public static ILoggerFactory LoggerFactory { get; set; }
        public static ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();        
        public static ILogger CreateLogger(string categoryName) => LoggerFactory.CreateLogger(categoryName);
    }
}