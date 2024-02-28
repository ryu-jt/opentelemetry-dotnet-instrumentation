using System.Reflection;

namespace OpenTelemetry.AutoInstrumentation.Loader;

internal partial class Loader
{
    private static readonly string ManagedProfilerDirectory;

    static Loader()
    {
        ManagedProfilerDirectory = ResolveManagedProfilerDirectory();

        try
        {
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve_ManagedProfilerDependencies;
        }
        catch (Exception ex)
        {
            Logger.Instance.Error($"Unable to register a callback to the CurrentDomain.AssemblyResolve event. {ex}");
        }

        TryLoadManagedAssembly();
    }

    private static void TryLoadManagedAssembly()
    {
        try
        {
            var assembly = Assembly.Load("OpenTelemetry.AutoInstrumentation");
            if (assembly == null)
            {
                throw new FileNotFoundException("The assembly OpenTelemetry.AutoInstrumentation could not be loaded");
            }

            var type = assembly.GetType("OpenTelemetry.AutoInstrumentation.Instrumentation", throwOnError: false);
            if (type == null)
            {
                throw new TypeLoadException("The type OpenTelemetry.AutoInstrumentation.Instrumentation could not be loaded");
            }

            var method = type.GetRuntimeMethod("Initialize", Type.EmptyTypes);
            if (method == null)
            {
                throw new MissingMethodException("The method OpenTelemetry.AutoInstrumentation.Instrumentation.Initialize could not be loaded");
            }

            method.Invoke(obj: null, parameters: null);
        }
        catch (Exception ex)
        {
            Logger.Instance.Error($"Error when loading managed assemblies. {ex}");
            throw;
        }
    }

    private static string? ReadEnvironmentVariable(string key)
    {
        try
        {
            return Environment.GetEnvironmentVariable(key);
        }
        catch (Exception ex)
        {
            Logger.Instance.Error($"Error getting store directory location. {ex}");
        }

        return null;
    }
}
