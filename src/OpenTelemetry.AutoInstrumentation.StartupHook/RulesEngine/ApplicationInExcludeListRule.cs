// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpenTelemetry.AutoInstrumentation.Helpers;

namespace OpenTelemetry.AutoInstrumentation.RulesEngine;

internal class ApplicationInExcludeListRule : Rule
{
    public ApplicationInExcludeListRule()
    {
        Name = "Application is in exclude list validator";
        Description = "This rule checks if the application is included in the exclusion list specified by the OTEL_DOTNET_AUTO_EXCLUDE_PROCESSES environment variable. If the application is in the exclusion list, the rule skips initialization.";
    }

    internal override bool Evaluate()
    {
        var appDomainName = GetAppDomainName();
        if (appDomainName.Equals("dotnet", StringComparison.InvariantCultureIgnoreCase))
        {
            Logger.Instance.Info($"Rule Engine: AppDomain name is dotnet. Skipping initialization.");
            return false;
        }

        var processModuleName = GetProcessModuleName();
        if (GetExcludedApplicationNames().Contains(processModuleName, StringComparer.InvariantCultureIgnoreCase))
        {
            Logger.Instance.Info($"Rule Engine: {processModuleName} is in the exclusion list. Skipping initialization.");
            return false;
        }

        Logger.Instance.Debug($"Rule Engine: {processModuleName} is not in the exclusion list. ApplicationInExcludeListRule evaluation success.");
        return true;
    }

    private static string GetProcessModuleName()
    {
        try
        {
            return Process.GetCurrentProcess().MainModule.ModuleName;
        }
        catch (Exception ex)
        {
            Logger.Instance.Error($"Error getting Process.MainModule.ModuleName: {ex}");
            return string.Empty;
        }
    }

    private static string GetAppDomainName()
    {
        try
        {
            return AppDomain.CurrentDomain.FriendlyName;
        }
        catch (Exception ex)
        {
            Logger.Instance.Error($"Error getting AppDomain.CurrentDomain.FriendlyName: {ex}");
            return string.Empty;
        }
    }

    private static ICollection<string> GetExcludedApplicationNames()
    {
        var excludedProcesses = new List<string>();

        var environmentValue = EnvironmentHelper.GetEnvironmentVariable("OTEL_DOTNET_AUTO_EXCLUDE_PROCESSES");

        if (environmentValue == null)
        {
            return excludedProcesses;
        }

        foreach (var processName in environmentValue.Split(','))
        {
            if (!string.IsNullOrWhiteSpace(processName))
            {
                excludedProcesses.Add(processName.Trim());
            }
        }

        return excludedProcesses;
    }
}
