using OpenTelemetry.AutoInstrumentation.CallTarget;

namespace OpenTelemetry.AutoInstrumentation.Instrumentations.AspNetCore;

[InstrumentMethod(
    assemblyName: "Microsoft.AspNetCore.Mvc.Core",
    typeName: "Microsoft.AspNetCore.Mvc.ModelBinding.DefaultModelBindingContext",
    methodName: "set_Result",
    returnTypeName: ClrNames.Void,
    parameterTypeNames: new[] { "Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingResult" },
    minimumVersion: "2.0.0.0",
    maximumVersion: "8.*.*.*.*",
    integrationName: "AspNetCore",
    type: InstrumentationType.Trace)]
[InstrumentMethod(
    assemblyName: "Microsoft.AspNetCore.Mvc.Core",
    typeName: "Microsoft.AspNetCore.Mvc.ModelBinding.DefaultModelBindingContext",
    methodName: "set_Result",
    returnTypeName: ClrNames.Void,
    parameterTypeNames: new[] { "Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingResult" },
    minimumVersion: "2.0.0.0",
    maximumVersion: "8.*.*.*.*",
    integrationName: "AspNetCore",
    type: InstrumentationType.Trace)]
internal class DefaultModelBindingContext_SetResult_Integration
{
    internal static CallTargetReturn OnMethodEnd<TTarget>(TTarget instance, Exception? exception, in CallTargetState state)
    {
        Logger.Instance.Info("DefaultModelBindingContext_SetResult_Integration.OnMethodEnd()");

        return CallTargetReturn.GetDefault();
    }
}
