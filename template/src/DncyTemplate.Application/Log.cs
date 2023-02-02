#pragma warning disable SYSLIB1006

namespace DncyTemplate.Application
{
    internal static partial class Log
    {
        [LoggerMessage(
            EventId = 0,
            Level = LogLevel.Debug,
            Message = "`{message}`")]
        internal static partial void Debug(this ILogger logger, string message);

        [LoggerMessage(
            EventId = 0,
            Level = LogLevel.Information,
            Message = "`{message}`")]
        internal static partial void Info(this ILogger logger, string message);

        [LoggerMessage(
            EventId = 0,
            Level = LogLevel.Warning,
            Message = "`{message}`")]
        internal static partial void Warning(this ILogger logger, string message);


        [LoggerMessage(
            EventId = 0,
            Level = LogLevel.Error,
            Message = "`{message}`")]
        internal static partial void Error(this ILogger logger, string message, Exception ex);
    }
}
