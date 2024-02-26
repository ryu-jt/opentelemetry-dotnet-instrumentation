using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Instrumentation.AspNetCore.Implementation;

namespace OpenTelemetry.AutoInstrumentation;

public class Instrumentation
{
    private static int _initialized = 0;
    private static AspNetCoreInstrumentation? _instrumentation = null;

    public static void Initialize()
    {
        if (Interlocked.Exchange(ref _initialized, value: 1) != 0)
        {
            // Initialize() was already called before
            return;
        }

        DebugLogs.Instance.Log("Instrumentation.Initialize()");

        string frameworkDescription = RuntimeInformation.FrameworkDescription;
        DebugLogs.Instance.Log($".NET version: {frameworkDescription}");

        try
        {
            var options = new OpenTelemetry.Instrumentation.AspNetCore.AspNetCoreTraceInstrumentationOptions();
            //_pluginManager.ConfigureTracesOptions(options);
            var httpInListener = new HttpInListener(options);
            _instrumentation = new AspNetCoreInstrumentation(httpInListener);
            //lifespanManager.Track(instrumentation);
        }
        catch (Exception ex)
        {
            DebugLogs.Instance.Log("Instrumentation.Initialize() Exception: " + ex);
        }
    }
}
