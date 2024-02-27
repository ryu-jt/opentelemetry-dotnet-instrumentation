using System;
using System.Diagnostics;
using WhaTap.Trace;

namespace OpenTelemetry.AutoInstrumentation.Diagnostic;

internal class AspNetCoreInHandler : IDiagnosticHandler
{
    public void Handle(string eventName, object payload)
    {
        try
        {
            switch (eventName)
            {
                case "Microsoft.AspNetCore.Hosting.HttpRequestIn":
                    HandleHttpRequestIn(payload);
                    break;
                case "Microsoft.AspNetCore.Hosting.HttpRequestIn.Start":
                    HandleHttpRequestInStart(payload);
                    break;
                case "Microsoft.AspNetCore.Hosting.HttpRequestIn.Stop":
                    HandleHttpRequestInStop(payload);
                    break;
                case "Microsoft.AspNetCore.Diagnostics.UnhandledException":
                    HandleUnhandledException(payload);
                    break;
                case "Microsoft.AspNetCore.Hosting.UnhandledException":
                    HandleHostingUnhandledException(payload);
                    break;
                case "Microsoft.AspNetCore.Mvc.BeforeAction":
                    HandleMvcBeforeAction(payload);
                    break;
            }
        }
        catch (Exception ex)
        {
            Logger.Instance.Error($"Error handling event {eventName}: {ex}");
        }
    }

    private void HandleHttpRequestIn(object payload)
    {
        Logger.Instance.Debug($"Handling HttpRequestIn: {ExtractDataDetails(payload)}");
    }

    private void HandleHttpRequestInStart(object payload)
    {
        TraceControl.Instance.StartTransaction(CreateStep(payload));
    }

    private void HandleHttpRequestInStop(object payload)
    {
        TraceControl.Instance.EndTransaction();
    }

    private void HandleUnhandledException(object payload)
    {
        Logger.Instance.Debug($"Handling UnhandledException: {ExtractDataDetails(payload)}");
    }

    private void HandleHostingUnhandledException(object payload)
    {
        Logger.Instance.Debug($"Handling HostingUnhandledException: {ExtractDataDetails(payload)}");
    }

    private void HandleMvcBeforeAction(object payload)
    {
        Logger.Instance.Debug($"Handling MvcBeforeAction: {ExtractDataDetails(payload)}");
    }

    public static Step? CreateStep(object payload)
    {
        try
        {
            dynamic context = payload;

            var actionName = context.Request.Path.Value;
            var host = context.Request.Host.Value;
            var httpMethod = context.Request.Method;
            var url = context.Request.Path.Value;
            Logger.Instance.Debug($"Handling HttpRequestIn.Start: Action={actionName}, Method={httpMethod}");

            var step = new Step(null);
            step.Host = host;
            step.HttpMethod = httpMethod;
            step.Url = url;

            return step;
        }
        catch (Exception ex)
        {
            Logger.Instance.Error($"AspNetCoreInHandler.CreateStep: {ex}");
            return null;
        }
    }

    // 이 메소드는 data 객체 내부의 세부 정보를 추출하여 문자열로 반환합니다.
    // 실제 구현은 data 객체의 타입과 구조에 따라 달라질 수 있습니다.
    private string ExtractDataDetails(object data)
    {
        // 예제 구현입니다. 실제 구현에서는 data 객체의 구조에 따라 세부 정보를 추출해야 합니다.
        return data.ToString();
    }
}
