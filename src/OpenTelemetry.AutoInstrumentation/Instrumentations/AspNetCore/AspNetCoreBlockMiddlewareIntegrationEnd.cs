using System.ComponentModel;
using OpenTelemetry.AutoInstrumentation.CallTarget;
using OpenTelemetry.AutoInstrumentation.DuckTyping;

namespace OpenTelemetry.AutoInstrumentation.Instrumentations.AspNetCore;

//[InstrumentMethod(
//    assemblyName: "Microsoft.AspNetCore.Http",
//    typeName: "Microsoft.AspNetCore.Builder.ApplicationBuilder",
//    methodName: "Build",
//    returnTypeName: "Microsoft.AspNetCore.Http.RequestDelegate",
//    parameterTypeNames: new string[] { },
//    minimumVersion: "3",
//    maximumVersion: "8",
//    integrationName: "AspNetCore",
//    type: InstrumentationType.Trace)]
//[InstrumentMethod(
//    assemblyName: "Microsoft.AspNetCore.Http",
//    typeName: "Microsoft.AspNetCore.Builder.Internal.ApplicationBuilder",
//    methodName: "Build",
//    returnTypeName: "Microsoft.AspNetCore.Http.RequestDelegate",
//    parameterTypeNames: new string[] { },
//    minimumVersion: "2",
//    maximumVersion: "2",
//    integrationName: "AspNetCore",
//    type: InstrumentationType.Trace)]
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
internal class AspNetCoreBlockMiddlewareIntegrationEnd
{
    public static CallTargetState OnMethodBegin<TTarget>(TTarget instance)
    {
        Logger.Instance.Info("AspNetCoreBlockMiddlewareIntegrationEnd.OnMethodBegin()");

        //instance.Components.Insert(0, rd => new BlockingMiddleware(rd, startPipeline: true).Invoke);
        //instance.Components.Add(rd => new BlockingMiddleware(rd, endPipeline: true).Invoke);

        return default;
    }
}

//[Browsable(false)]
//[EditorBrowsable(EditorBrowsableState.Never)]
//public interface IApplicationBuilder
//{
//    [DuckField(Name = "_components")]
//    IList<Func<RequestDelegate, RequestDelegate>> Components { get; }
//}

//internal class BlockingMiddleware
//{
//    private readonly bool _endPipeline;

//    // if we add support for ASP.NET Core on .NET Framework, we can't directly reference RequestDelegate, so this would need to be written
//    private readonly RequestDelegate? _next;
//    private readonly bool _startPipeline;

//    internal BlockingMiddleware(RequestDelegate? next = null, bool startPipeline = false, bool endPipeline = false)
//    {
//        _next = next;
//        _startPipeline = startPipeline;
//        _endPipeline = endPipeline;
//    }
//}

//internal async Task Invoke(HttpContext context)
//{
//    var endedResponse = false;
//    if (Tracer.Instance?.ActiveScope?.Span is Span span)
//    {
//        if (_endPipeline && !context.Response.HasStarted)
//        {
//            context.Response.StatusCode = 404;
//        }
//    }
//    else
//    {
//        WhaTapLogs.Error("No span available, can't check the request");
//    }

//    if (_next != null && !endedResponse)
//    {
//        // unlikely that security is disabled and there's a block exception, but might happen as race condition
//        try
//        {
//            await _next(context).ConfigureAwait(false);
//        }
//        catch
//        {
//        }
//    }
//}
