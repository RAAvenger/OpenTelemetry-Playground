using System.Diagnostics;

namespace OtelPlayground.ServiceB.Commons
{
    public class AppActivitySource
    {
        public const string AppActivityName = "service1 internal";
        public static readonly ActivitySource Instance = new ActivitySource(AppActivityName); 
    }
}
