// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Security;

namespace OpenTelemetry.AutoInstrumentation.Helpers;

internal static class EnvironmentHelper
{

    public static string? GetEnvironmentVariable(string variableName)
    {
        try
        {
            return Environment.GetEnvironmentVariable(variableName);
        }
        catch (SecurityException ex)
        {
            return null;
        }
    }
}
