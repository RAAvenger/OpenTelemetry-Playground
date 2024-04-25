using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OtelPlayground.FakeUser.Utilities.Abstraction;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtelPlayground.FakeUser.Utilities
{

    internal class ActivitySourceWrapper : IActivitySourceWrapper
    {
        private readonly Tracer _tracer;

        public ActivitySourceWrapper(Tracer tracer)
        {
            _tracer = tracer ?? throw new ArgumentNullException(nameof(tracer));
        }

        public TelemetrySpan CreateSpan(string name, SpanKind kind)
        {
           return _tracer.StartSpan(name, kind);
        }
    }
}