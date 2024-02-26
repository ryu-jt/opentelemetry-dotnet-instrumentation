using System.Security.Claims;
using OpenTelemetry.AutoInstrumentation.CallTarget;

namespace OpenTelemetry.AutoInstrumentation.Instrumentations.AspNetCore;

[InstrumentMethod(
    assemblyName: "Microsoft.AspNetCore.Authentication.Abstractions",
    typeName: "Microsoft.AspNetCore.Authentication.AuthenticationHttpContextExtensions",
    methodName: "SignInAsync",
    returnTypeName: ClrNames.Task,
    parameterTypeNames: new[] { "Microsoft.AspNetCore.Http.HttpContext", ClrNames.String, "System.Security.Claims.ClaimsPrincipal", "Microsoft.AspNetCore.Authentication.AuthenticationProperties" },
    minimumVersion: "2.0.0.0",
    maximumVersion: "8.*.*.*.*",
    integrationName: "AspNetCore",
    type: InstrumentationType.Trace)]
internal class AuthenticationHttpContextExtensionsIntegration
{
    internal static CallTargetState OnMethodBegin<TTarget>(object httpContext, string scheme, ClaimsPrincipal claimPrincipal, object authProperties)
    {
        Logger.Instance.Info("AuthenticationHttpContextExtensionsIntegration.OnMethodBegin()");

        return CallTargetState.GetDefault();
    }

    internal static object OnAsyncMethodEnd<TTarget>(object returnValue, Exception exception, in CallTargetState state)
    {
        Logger.Instance.Info("AuthenticationHttpContextExtensionsIntegration.OnAsyncMethodEnd()");

        return returnValue;
    }
}
