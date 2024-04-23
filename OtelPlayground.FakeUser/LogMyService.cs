using Microsoft.Extensions.Logging;

public static partial class LogMyService
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "GetAll response: {response}")]
    public static partial void GetAll(this ILogger logger, string response);

    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "GetCity city: '{city}' response: {response}")]
    public static partial void GetCity(this ILogger logger, string city, string response);
}