using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace OpenTelemetry.Instrumentation.AspNetCore.Implementation;

internal class HttpInListener : ListenerHandler
{
    internal const string ActivityOperationName = "Microsoft.AspNetCore.Hosting.HttpRequestIn";
    internal const string OnStartEvent = "Microsoft.AspNetCore.Hosting.HttpRequestIn.Start";
    internal const string OnStopEvent = "Microsoft.AspNetCore.Hosting.HttpRequestIn.Stop";
    internal const string OnUnhandledHostingExceptionEvent = "Microsoft.AspNetCore.Hosting.UnhandledException";
    internal const string OnUnHandledDiagnosticsExceptionEvent = "Microsoft.AspNetCore.Diagnostics.UnhandledException";

    internal static readonly AssemblyName AssemblyName = typeof(HttpInListener).Assembly.GetName();
    internal static readonly string ActivitySourceName = AssemblyName.Name;
    internal static readonly Version Version = AssemblyName.Version;
    internal static readonly ActivitySource ActivitySource = new(ActivitySourceName, Version.ToString());

    private const string DiagnosticSourceName = "Microsoft.AspNetCore";

    private static readonly Func<HttpRequest, string, IEnumerable<string>> HttpRequestHeaderValuesGetter = (request, name) =>
    {
        DebugLogs.Instance.Log($"HttpRequestHeaderValuesGetter: {request}, {name}");

        if (request.Headers.TryGetValue(name, out var value))
        {
            return value;
        }
        return Enumerable.Empty<string>();
    };

    private readonly AspNetCoreTraceInstrumentationOptions options;

    public HttpInListener(AspNetCoreTraceInstrumentationOptions options)
        : base(DiagnosticSourceName)
    {
        this.options = options;
    }

    // OnEventWritten, OnStartActivity, OnStopActivity, OnException methods remain unchanged

    // Helper methods (e.g., TryGetGrpcMethod, AddGrpcAttributes, GetDisplayName) remain unchanged
}
