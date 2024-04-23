namespace OtelPlayground.ServiceA.Controllers.Logs
{
    public static partial class LogWhetherForecastExceptions
    {
        [LoggerMessage(EventId = 0, Level = LogLevel.Error)]
        public static partial void CouldNotGenerateWhetherForecast(this ILogger logger, Exception ex);
    }
}