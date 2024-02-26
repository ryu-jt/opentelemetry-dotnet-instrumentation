using OpenTelemetry.AutoInstrumentation.Helpers;

using static OpenTelemetry.AutoInstrumentation.Constants.EnvironmentVariables;

namespace OpenTelemetry.AutoInstrumentation.RulesEngine;

internal class NativeProfilerDiagnosticsRule : Rule
{
    public NativeProfilerDiagnosticsRule()
    {
        Name = "Native profiler diagnoser";
        Description = "Verifies that native profiler is correctly setup in case it's enabled.";
    }

    internal override bool Evaluate()
    {
        var isProfilerEnabled = EnvironmentHelper.GetEnvironmentVariable(ProfilerEnabledVariable) == "1";
        if (!isProfilerEnabled)
        {
            Logger.Instance.Warning($"{ProfilerEnabledVariable} environment variable is not set to '1'. The CLR Profiler is disabled and no bytecode instrumentations are going to be injected.");
            return true;
        }

        var profilerId = EnvironmentHelper.GetEnvironmentVariable(ProfilerIdVariable);
        if (profilerId != ProfilerId)
        {
            Logger.Instance.Warning($"The CLR profiler is enabled, but a different profiler ID was provided '{profilerId}'." );

            // Different native profiler not associated to OTel might be used. We don't want to fail here.
            return true;
        }

        try
        {
            if (NativeMethods.IsProfilerAttached())
            {
                return true;
            }

            Logger.Instance.Error("IsProfilerAttached returned false, the native log should describe the root cause.");
        }
        catch (Exception ex)
        {
            /* Native profiler is not attached. Continue with diagnosis */
            Logger.Instance.Debug($"Error checking if native profiler is attached. {ex}");
        }

        if (Environment.Is64BitProcess)
        {
            VerifyPathVariables(Profiler64BitPathVariable, "64bit");
        }
        else
        {
            VerifyPathVariables(Profiler32BitPathVariable, "32bit");
        }

        return false;
    }

    private static void VerifyPathVariables(string archPathVariable, string expectedBitness)
    {
        if (TryPathVariable(archPathVariable, expectedBitness))
        {
            return;
        }

        if (TryPathVariable(ProfilerPathVariable, expectedBitness))
        {
            return;
        }

        Logger.Instance.Error($"CLR profiler path is not defined. Define '{ProfilerPathVariable}' or '{archPathVariable}'.");
    }

    private static bool TryPathVariable(string profilerPathVariable, string expectedBitness)
    {
        var profilerPath = EnvironmentHelper.GetEnvironmentVariable(profilerPathVariable);

        // Nothing to verify. Signal that VerifyVariables can continue searching for issues.
        if (string.IsNullOrWhiteSpace(profilerPath))
        {
            return false;
        }

        if (File.Exists(profilerPath))
        {
            // File is found but profiler is not attaching.
            Logger.Instance.Error($"CLR profiler was not correctly loaded into the process. Profiler found at '{profilerPath}'.");
        }
        else
        {
            // File not found.
            Logger.Instance.Error($"CLR profiler ({expectedBitness}) is not found at '{profilerPath}'. Recheck '{profilerPathVariable}'.");
        }

        // Path issue verified. VerifyVariables should not continue.
        return true;
    }
}
