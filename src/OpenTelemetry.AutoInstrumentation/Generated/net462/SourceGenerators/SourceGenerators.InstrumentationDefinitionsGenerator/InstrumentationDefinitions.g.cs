﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the InstrumentationDefinitionsGenerator tool. To safely
//     modify this file, edit InstrumentMethodAttribute on the classes and
//     compile project.

//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using OpenTelemetry.AutoInstrumentation.Configurations;

namespace OpenTelemetry.AutoInstrumentation;

internal static partial class InstrumentationDefinitions
{
    private static readonly string AssemblyFullName = typeof(InstrumentationDefinitions).Assembly.FullName!;

    private static NativeCallTargetDefinition[] GetDefinitionsArray()
    {
        var nativeCallTargetDefinitions = new List<NativeCallTargetDefinition>(6);
        // Traces
        if (TracerSettings.Instance.TracesEnabled)
        {
            // AspNetCore
            if (TracerSettings.Instance.EnabledInstrumentations.Contains(TracerInstrumentation.AspNetCore))
            {
                nativeCallTargetDefinitions.Add(new("Microsoft.AspNetCore.Http", "Microsoft.AspNetCore.Builder.ApplicationBuilder", "Build", new[] {"Microsoft.AspNetCore.Http.RequestDelegate"}, 3, 0, 0, 8, 65535, 65535, AssemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.AspNetCore.AspNetCoreBlockMiddlewareIntegrationEnd"));
                nativeCallTargetDefinitions.Add(new("Microsoft.AspNetCore.Http", "Microsoft.AspNetCore.Builder.Internal.ApplicationBuilder", "Build", new[] {"Microsoft.AspNetCore.Http.RequestDelegate"}, 2, 0, 0, 2, 65535, 65535, AssemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.AspNetCore.AspNetCoreBlockMiddlewareIntegrationEnd"));
                nativeCallTargetDefinitions.Add(new("Microsoft.AspNetCore.Authentication.Abstractions", "Microsoft.AspNetCore.Authentication.AuthenticationHttpContextExtensions", "SignInAsync", new[] {"System.Threading.Tasks.Task", "Microsoft.AspNetCore.Http.HttpContext", "System.String", "System.Security.Claims.ClaimsPrincipal", "Microsoft.AspNetCore.Authentication.AuthenticationProperties"}, 2, 0, 0, 8, 65535, 65535, AssemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.AspNetCore.AuthenticationHttpContextExtensionsIntegration"));
                nativeCallTargetDefinitions.Add(new("Microsoft.AspNetCore.Mvc.Core", "Microsoft.AspNetCore.Mvc.ModelBinding.DefaultModelBindingContext", "set_Result", new[] {"System.Void", "Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingResult"}, 2, 0, 0, 8, 65535, 65535, AssemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.AspNetCore.DefaultModelBindingContext_SetResult_Integration"));
                nativeCallTargetDefinitions.Add(new("Microsoft.AspNetCore.Mvc.Core", "Microsoft.AspNetCore.Mvc.ModelBinding.DefaultModelBindingContext", "set_Result", new[] {"System.Void", "Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingResult"}, 2, 0, 0, 8, 65535, 65535, AssemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.AspNetCore.DefaultModelBindingContext_SetResult_Integration"));
                nativeCallTargetDefinitions.Add(new("Microsoft.AspNetCore.Mvc.Core", "Microsoft.AspNetCore.Mvc.MvcOptions", ".ctor", new[] {"System.Void"}, 2, 0, 0, 8, 65535, 65535, AssemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.AspNetCore.MvcOptionsIntegration"));
            }
        }

        return nativeCallTargetDefinitions.ToArray();
    }
}
