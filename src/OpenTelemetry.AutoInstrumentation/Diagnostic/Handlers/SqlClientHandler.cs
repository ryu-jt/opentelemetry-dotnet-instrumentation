using System;
using WhaTap.Trace;
using WhaTap.Trace.Utils;

namespace OpenTelemetry.AutoInstrumentation.Diagnostic;

internal class SqlClientHandler : IDiagnosticHandler
{
    public void Handle(string eventName, object payload)
    {
        try
        {
            switch (eventName)
            {
                case "System.Data.SqlClient.WriteCommandBefore":
                case "Microsoft.Data.SqlClient.WriteCommandBefore":
                    HandleCommandBefore(payload);
                    break;

                case "System.Data.SqlClient.WriteCommandAfter":
                case "Microsoft.Data.SqlClient.WriteCommandAfter":
                    HandleCommandAfter(payload);
                    break;

                case "System.Data.SqlClient.WriteCommandError":
                case "Microsoft.Data.SqlClient.WriteCommandError":
                    HandleCommandError(payload);
                    break;
            }
        }
        catch (Exception ex)
        {
            Logger.Instance.Error($"Error handling event {eventName}: {ex}");
        }
    }

    private void HandleCommandBefore(object payload)
    {
        TraceControl.Instance.StartSection(CreateStep(payload));    
    }


    private void HandleCommandAfter(object payload)
    {
        var step = ActiveTransaction.Instance.Transaction?.CurrentStep ?? null;
        TraceControl.Instance.EndSection(step);
    }

    private void HandleCommandError(object payload)
    {
    }

    public static Step? CreateStep(object payload)
    {
        try
        {
            var commandProperty = payload.GetType().GetProperty("Command");
            if (commandProperty == null)
            {
                throw new Exception("Command property not found in payload.");
            }

            var command = commandProperty.GetValue(payload) as System.Data.Common.DbCommand;
            if (command == null)
            {
                throw new Exception("Command property is not of expected type.");
            }

            var commandType = command.GetType();
            DbUtils.TryGetIntegrationDetails(commandType?.FullName, out var dbType);
            Logger.Instance.Debug($"Handling WriteCommandBefore: {dbType}");

            var step = new Step(null);
            step.Type = PacketType.DBSql;
            step.DbType = dbType;
            //step.DbConn = $"Server={tagsFromConnectionString.OutHost}; Database={tagsFromConnectionString.DbName}; User={tagsFromConnectionString.DbUser}";
            step.ResourceName = command.CommandText;

            return step;
        }
        catch (Exception ex)
        {
            Logger.Instance.Error($"SqlClientHandler.CreateStep: {ex}");
            return null;
        }
    }
}
