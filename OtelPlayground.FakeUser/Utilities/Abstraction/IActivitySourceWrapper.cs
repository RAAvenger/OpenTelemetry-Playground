using OpenTelemetry.Trace;
using System.Diagnostics;

namespace OtelPlayground.FakeUser.Utilities.Abstraction
{
    internal interface IActivitySourceWrapper
    {
        TelemetrySpan CreateSpan(string name, SpanKind kind);
    }
}