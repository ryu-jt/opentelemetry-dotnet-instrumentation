using OpenTelemetry.AutoInstrumentation.Diagnostic;

namespace OpenTelemetry.AutoInstrumentation;

internal static class Instrumentation
{
    private static int _initialized;

    public static void Initialize()
    {
        if (Interlocked.Exchange(ref _initialized, value: 1) != 0)
        {
            // Initialize() was already called before
            return;
        }

        Logger.Instance.Debug("OpenTelemetry SDK initialization - Start");

#if !NETFRAMEWORK
        DiagnosticManager.Instance.Listening();
#endif

        RegisterBytecodeInstrumentations(InstrumentationDefinitions.GetAllDefinitions());

        //try
        //{
        //    Logger.Instance.Debug("Sending CallTarget derived integration definitions to native library.");

        //    var payload = InstrumentationDefinitions.GetDerivedDefinitions();
        //    NativeMethods.AddDerivedInstrumentations(payload.DefinitionsId, payload.Definitions);
        //    foreach (var def in payload.Definitions)
        //    {
        //        def.Dispose();
        //    }

        //    Logger.Instance.Info($"The profiler has been initialized with {payload.Definitions.Length} derived definitions.");
        //}
        //catch (Exception ex)
        //{
        //    Logger.Instance.Error(ex.Message);
        //}
    }

    private static void RegisterBytecodeInstrumentations(InstrumentationDefinitions.Payload payload)
    {
        try
        {
            Logger.Instance.Debug($"The profiler has been initialized with {payload.DefinitionsId} derived definitions.");

            NativeMethods.AddInstrumentations(payload.DefinitionsId, payload.Definitions);
            foreach (var def in payload.Definitions)
            {
                def.Dispose();
            }

            Logger.Instance.Info($"The profiler has been initialized with {payload.Definitions.Length} definitions for {payload.DefinitionsId}.");
        }
        catch (Exception ex)
        {
            Logger.Instance.Error(ex.Message);
        }
    }
}
