using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace OpenTelemetry.Instrumentation.AspNetCore;

public class AspNetCoreTraceInstrumentationOptions
{
    public AspNetCoreTraceInstrumentationOptions()
        : this(new ConfigurationBuilder().AddEnvironmentVariables().Build())
    {
        DebugLogs.Instance.Log("AspNetCoreTraceInstrumentationOptions()");

        //AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
        //{
        //    string assemblyName = new AssemblyName(args.Name).Name;
        //    DebugLogs.Instance.Log($"AspNetCoreTraceInstrumentationOptions - Resolving assembly {assemblyName}");

        //    if (assemblyName == "System.Diagnostics.DiagnosticSource")
        //    {
        //        return Assembly.LoadFrom(@"D:\Works\opentelemetry-dotnet-instrumentation-windows\net\System.Diagnostics.DiagnosticSource.dll");
        //    }
        //    return null;
        //};
    }

    internal AspNetCoreTraceInstrumentationOptions(IConfiguration configuration)
    {
        DebugLogs.Instance.Log("AspNetCoreTraceInstrumentationOptions(configurations)");

        Debug.Assert(configuration != null, "configuration was null");
    }

    public Func<HttpContext, bool> Filter { get; set; }
    public Action<Activity, HttpRequest> EnrichWithHttpRequest { get; set; }
    public Action<Activity, HttpResponse> EnrichWithHttpResponse { get; set; }
    public Action<Activity, Exception> EnrichWithException { get; set; }
    public bool RecordException { get; set; }
    internal bool EnableGrpcAspNetCoreSupport { get; set; }
}
