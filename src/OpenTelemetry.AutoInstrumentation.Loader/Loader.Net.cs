#if NETCOREAPP
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace OpenTelemetry.AutoInstrumentation.Loader;

internal partial class Loader
{
    internal static AssemblyLoadContext DependencyLoadContext { get; } = new ManagedProfilerAssemblyLoadContext();

    private static string ResolveManagedProfilerDirectory()
    {
        string tracerFrameworkDirectory = "net";
        string tracerHomeDirectory = ReadEnvironmentVariable("OTEL_DOTNET_AUTO_HOME") ?? string.Empty;
        return Path.Combine(tracerHomeDirectory, tracerFrameworkDirectory);
    }

    private static Assembly? AssemblyResolve_ManagedProfilerDependencies(object? sender, ResolveEventArgs args)
    {
        var assemblyName = new AssemblyName(args.Name);
        string assemblyPath = Path.Combine(ManagedProfilerDirectory, $"{assemblyName.Name}.dll");

        if (File.Exists(assemblyPath))
        {
            return DependencyLoadContext.LoadFromAssemblyPath(assemblyPath);
        }

        try
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName);
        }
        catch
        {
            return null;
        }
    }
}
#endif
