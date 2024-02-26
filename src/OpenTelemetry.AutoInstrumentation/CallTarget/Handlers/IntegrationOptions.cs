using System.Reflection;
using System.Runtime.CompilerServices;
using OpenTelemetry.AutoInstrumentation.DuckTyping;

namespace OpenTelemetry.AutoInstrumentation.CallTarget.Handlers;

internal static class IntegrationOptions<TIntegration, TTarget>
{
    private static volatile bool _disableIntegration = false;

    internal static bool IsIntegrationEnabled => !_disableIntegration;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void DisableIntegration() => _disableIntegration = true;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void LogException(Exception exception, string? message = null)
    {
        Logger.Instance.Debug($"LogException() - {typeof(TIntegration)}, {message ?? exception.Message}");

        if (exception is DuckTypeException or TargetInvocationException { InnerException: DuckTypeException })
        {
            Logger.Instance.Warning($"DuckTypeException has been detected, the integration <{typeof(TIntegration)}, {typeof(TTarget)}> will be disabled.");
            _disableIntegration = true;
        }
        else if (exception is CallTargetInvokerException)
        {
            Logger.Instance.Warning($"CallTargetInvokerException has been detected, the integration <{typeof(TIntegration)}, {typeof(TTarget)}> will be disabled.");
            _disableIntegration = true;
        }
        else if (exception is FileLoadException fileLoadException)
        {
            if (fileLoadException.FileName != null && (fileLoadException.FileName.StartsWith("System.Diagnostics.DiagnosticSource") || fileLoadException.FileName.StartsWith("System.Runtime.CompilerServices.Unsafe")))
            {
                Logger.Instance.Warning($"FileLoadException for '{fileLoadException.FileName}' has been detected, the integration <{typeof(TIntegration)}, {typeof(TTarget)}> will be disabled.");
                _disableIntegration = true;
            }
        }
    }
}
