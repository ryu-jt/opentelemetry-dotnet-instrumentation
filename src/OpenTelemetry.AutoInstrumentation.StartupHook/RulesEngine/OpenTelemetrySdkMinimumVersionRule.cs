// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Reflection;

namespace OpenTelemetry.AutoInstrumentation.RulesEngine;

internal class OpenTelemetrySdkMinimumVersionRule : Rule
{
    public OpenTelemetrySdkMinimumVersionRule()
    {
        Name = "OpenTelemetry SDK Validator";
        Description = "Ensure that the OpenTelemetry SDK version is not older than the version used by the Automatic Instrumentation";
    }

    internal override bool Evaluate()
    {
        string? oTelPackageVersion = null;

        try
        {
            var loadedOTelFileVersion = GetVersionFromApp();
            if (loadedOTelFileVersion != null)
            {
                var autoInstrumentationOTelFileVersion = GetVersionFromAutoInstrumentation();
                if (loadedOTelFileVersion < autoInstrumentationOTelFileVersion)
                {
                    oTelPackageVersion = loadedOTelFileVersion.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            // Exception in evaluation should not throw or crash the process.
            Logger.Instance.Info($"Rule Engine: Couldn't evaluate reference to OpenTelemetry Sdk in an app. Exception: {ex}");
            return true;
        }

        if (oTelPackageVersion != null)
        {
            Logger.Instance.Error($"Rule Engine: Application has direct or indirect reference to older version of OpenTelemetry package {oTelPackageVersion}.");
            return false;
        }

        Logger.Instance.Info("Rule Engine: OpenTelemetrySdkMinimumVersionRule evaluation success.");
        return true;
    }

    protected virtual Version? GetVersionFromApp()
    {
        var openTelemetryType = Type.GetType("OpenTelemetry.Sdk, OpenTelemetry");
        if (openTelemetryType != null)
        {
            var loadedOTelAssembly = Assembly.GetAssembly(openTelemetryType);
            var loadedOTelFileVersionInfo = FileVersionInfo.GetVersionInfo(loadedOTelAssembly?.Location);
            var loadedOTelFileVersion = new Version(loadedOTelFileVersionInfo.FileVersion);

            return loadedOTelFileVersion;
        }

        return null;
    }

    protected virtual Version? GetVersionFromAutoInstrumentation()
    {
        var autoInstrumentationOTelLocation = Path.Combine(StartupHook.LoaderAssemblyLocation ?? string.Empty, "OpenTelemetry.dll");
        var autoInstrumentationOTelFileVersionInfo = FileVersionInfo.GetVersionInfo(autoInstrumentationOTelLocation);
        var autoInstrumentationOTelFileVersion = new Version(autoInstrumentationOTelFileVersionInfo.FileVersion);

        return autoInstrumentationOTelFileVersion;
    }
}
