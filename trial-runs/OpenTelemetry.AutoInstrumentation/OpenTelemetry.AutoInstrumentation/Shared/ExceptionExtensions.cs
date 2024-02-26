#nullable enable

using System.Globalization;

namespace OpenTelemetry.Internal;

internal static class ExceptionExtensions
{
    public static string ToInvariantString(this Exception exception)
    {
        var originalUICulture = Thread.CurrentThread.CurrentUICulture;

        try
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            return exception.ToString();
        }
        finally
        {
            Thread.CurrentThread.CurrentUICulture = originalUICulture;
        }
    }
}
