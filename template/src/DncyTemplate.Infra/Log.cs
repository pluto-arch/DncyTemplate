using System.Data;

namespace DncyTemplate.Infra;

internal static partial class Log
{
    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Debug,
        Message = "Dapper DbCommand [CommandType='{commandType}', CommandTimeout='{commandTimeout}']{commandText}")]
    internal static partial void LogDapperDbCommand(this ILogger logger, CommandType? commandType,int? commandTimeout,string commandText);
}