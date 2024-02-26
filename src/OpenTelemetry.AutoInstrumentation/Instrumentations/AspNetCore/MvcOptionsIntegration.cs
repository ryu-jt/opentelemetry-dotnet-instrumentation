using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Filters;
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
        where TTarget : IMvcOptions
    {
        Logger.Instance.Info("MvcOptionsIntegration.OnMethodEnd()");

        instance.Filters.Add(new ActionResponseFilter());

        return CallTargetReturn.GetDefault();
    }
}

[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IMvcOptions
{
    System.Collections.ObjectModel.Collection<Microsoft.AspNetCore.Mvc.Filters.IFilterMetadata> Filters { get; }
}

internal class ActionResponseFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        Logger.Instance.Info("ActionResponseFilter.OnActionExecuting()");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        Logger.Instance.Info("ActionResponseFilter.OnActionExecuted()");
    }
}
