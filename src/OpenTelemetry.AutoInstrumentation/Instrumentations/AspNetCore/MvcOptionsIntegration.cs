using System.ComponentModel;
using OpenTelemetry.AutoInstrumentation.CallTarget;

namespace OpenTelemetry.AutoInstrumentation.Instrumentations.AspNetCore;

[InstrumentMethod(
    assemblyName: "Microsoft.AspNetCore.Mvc.Core",
    typeName: "Microsoft.AspNetCore.Mvc.MvcOptions",
    methodName: ".ctor",
    returnTypeName: ClrNames.Void,
    parameterTypeNames: new string[] { },
    minimumVersion: "2",
    maximumVersion: "8",
    integrationName: "AspNetCore",
    type: InstrumentationType.Trace)]
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public class MvcOptionsIntegration
{

    internal static CallTargetReturn OnMethodEnd<TTarget>(TTarget instance, Exception? exception, in CallTargetState state)
    {
        Logger.Instance.Info("MvcOptionsIntegration.OnMethodEnd()");

        return CallTargetReturn.GetDefault();
    }
}
