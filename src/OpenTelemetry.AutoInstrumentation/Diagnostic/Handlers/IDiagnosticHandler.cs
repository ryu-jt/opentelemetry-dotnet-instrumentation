using System;

namespace OpenTelemetry.AutoInstrumentation.Diagnostic;

internal interface IDiagnosticHandler
{
    void Handle(string eventName, object data);
}
