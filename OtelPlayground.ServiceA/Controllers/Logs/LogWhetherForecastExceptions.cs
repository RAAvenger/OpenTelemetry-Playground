namespace OtelPlayground.ServiceA.Controllers.Logs
{
    public static partial class LogWhetherForecastExceptions
    {
        [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "get data from server b failed {ex}")]
        public static partial void GetDataFromServerB(this ILogger logger, Exception ex);
    }
}