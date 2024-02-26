namespace OpenTelemetry.AutoInstrumentation.RulesEngine;

internal class MinSupportedFrameworkRule : Rule
{
    public MinSupportedFrameworkRule()
    {
        Name = "Minimum Supported Framework Version Validator";
        Description = "Verifies that the application is running on a supported version of the .NET runtime.";
    }

    internal override bool Evaluate()
    {
        Version minRequiredFrameworkVersion = new(6, 0);
        var frameworkVersion = Environment.Version;
        if (frameworkVersion < minRequiredFrameworkVersion)
        {
            Logger.Instance.Info($"Rule Engine: Error in StartupHook initialization: {frameworkVersion} is not supported");
            return false;
        }

        Logger.Instance.Info("Rule Engine: MinSupportedFrameworkRule evaluation success.");
        return true;
    }
}
