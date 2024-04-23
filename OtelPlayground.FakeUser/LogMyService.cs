using Microsoft.Extensions.Logging;

public static partial class LogMyService
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "GetAll response: {response}")]
    public static partial void GetAll(this ILogger logger, string response);

    [LoggerMessage(EventId = 500, Level = LogLevel.Error, Message = "GetAllError exception: {exception}")]
    public static partial void GetAllError(this ILogger logger, Exception exception);

    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "GetCity city: '{city}' response: {response}")]
    public static partial void GetCity(this ILogger logger, string city, string response);

    [LoggerMessage(EventId = 501, Level = LogLevel.Error, Message = "GetCityError city: '{city}' exception: {exception}")]
    public static partial void GetCityError(this ILogger logger, string city, Exception exception);
}