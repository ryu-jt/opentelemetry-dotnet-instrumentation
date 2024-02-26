#nullable enable

using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace OpenTelemetry.Internal;

internal static class Guard
{
    [DebuggerHidden]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNull(object? value, [CallerArgumentExpression("value")] string? paramName = null)
    {
        if (value is null)
        {
            throw new ArgumentNullException(paramName, "Must not be null");
        }
    }

    [DebuggerHidden]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNullOrEmpty(string? value, [CallerArgumentExpression("value")] string? paramName = null)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException("Must not be null or empty", paramName);
        }
    }

    [DebuggerHidden]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNullOrWhitespace(string? value, [CallerArgumentExpression("value")] string? paramName = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Must not be null or whitespace", paramName);
        }
    }

    [DebuggerHidden]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfZero(int value, string message = "Must not be zero", [CallerArgumentExpression("value")] string? paramName = null)
    {
        if (value == 0)
        {
            throw new ArgumentException(message, paramName);
        }
    }

    [DebuggerHidden]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfInvalidTimeout(int value, [CallerArgumentExpression("value")] string? paramName = null)
    {
        ThrowIfOutOfRange(value, paramName, min: Timeout.Infinite, message: $"Must be non-negative or '{nameof(Timeout)}.{nameof(Timeout.Infinite)}'");
    }

    [DebuggerHidden]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfOutOfRange(int value, [CallerArgumentExpression("value")] string? paramName = null, int min = int.MinValue, int max = int.MaxValue, string? minName = null, string? maxName = null, string? message = null)
    {
        Range(value, paramName, min, max, minName, maxName, message);
    }

    [DebuggerHidden]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfOutOfRange(double value, [CallerArgumentExpression("value")] string? paramName = null, double min = double.MinValue, double max = double.MaxValue, string? minName = null, string? maxName = null, string? message = null)
    {
        Range(value, paramName, min, max, minName, maxName, message);
    }

    [DebuggerHidden]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ThrowIfNotOfType<T>(object? value, [CallerArgumentExpression("value")] string? paramName = null)
    {
        if (value is not T result)
        {
            throw new InvalidCastException($"Cannot cast '{paramName}' from '{value?.GetType().ToString() ?? "null"}' to '{typeof(T)}'");
        }

        return result;
    }

    [DebuggerHidden]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Range<T>(T value, string? paramName, T min, T max, string? minName, string? maxName, string? message)
        where T : IComparable<T>
    {
        if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
        {
            var minMessage = minName != null ? $": {minName}" : string.Empty;
            var maxMessage = maxName != null ? $": {maxName}" : string.Empty;
            var exMessage = message ?? string.Format(
                CultureInfo.InvariantCulture,
                "Must be in the range: [{0}{1}, {2}{3}]",
                min,
                minMessage,
                max,
                maxMessage);
            throw new ArgumentOutOfRangeException(paramName, value, exMessage);
        }
    }
}
