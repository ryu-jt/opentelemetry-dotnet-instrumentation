using OpenTelemetry.Instrumentation.AspNetCore.Implementation;

namespace OpenTelemetry.Instrumentation.AspNetCore;

internal sealed class AspNetCoreInstrumentation : IDisposable
{
    private static readonly HashSet<string> DiagnosticSourceEvents = new()
    {
        "Microsoft.AspNetCore.Hosting.HttpRequestIn",
        "Microsoft.AspNetCore.Hosting.HttpRequestIn.Start",
        "Microsoft.AspNetCore.Hosting.HttpRequestIn.Stop",
        "Microsoft.AspNetCore.Diagnostics.UnhandledException",
        "Microsoft.AspNetCore.Hosting.UnhandledException",
    };

    private readonly Func<string, object, object, bool> isEnabled = (eventName, _, _)
        => DiagnosticSourceEvents.Contains(eventName);

    private readonly DiagnosticSourceSubscriber diagnosticSourceSubscriber;

    public AspNetCoreInstrumentation(HttpInListener httpInListener)
    {
        DebugLogs.Instance.Log("AspNetCoreInstrumentation(HttpInListener)");

        this.diagnosticSourceSubscriber = new DiagnosticSourceSubscriber(httpInListener, this.isEnabled, AspNetCoreInstrumentationEventSource.Log.UnknownErrorProcessingEvent);
        this.diagnosticSourceSubscriber.Subscribe();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.diagnosticSourceSubscriber?.Dispose();
    }
}
