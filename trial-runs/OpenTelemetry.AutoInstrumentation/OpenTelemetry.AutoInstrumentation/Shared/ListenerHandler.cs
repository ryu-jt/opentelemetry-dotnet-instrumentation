using System.Diagnostics;

namespace OpenTelemetry.Instrumentation;

internal abstract class ListenerHandler
{
    public ListenerHandler(string sourceName)
    {
        this.SourceName = sourceName;
    }

    public string SourceName { get; }

    public virtual bool SupportsNullActivity { get; }

    public virtual void OnEventWritten(string name, object payload)
    {
    }
}
