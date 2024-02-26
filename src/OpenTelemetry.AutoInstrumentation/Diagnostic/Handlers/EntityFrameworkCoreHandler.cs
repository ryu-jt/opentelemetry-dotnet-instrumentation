using System;

namespace OpenTelemetry.AutoInstrumentation.Diagnostic;

internal class EntityFrameworkCoreHandler : IDiagnosticHandler
{
    public void Handle(string eventName, object payload)
    {
        try
        {
            switch (eventName)
            {
                case "Microsoft.EntityFrameworkCore.Database.Connection.ConnectionOpened":
                    HandleConnectionOpened(payload);
                    break;
                case "Microsoft.EntityFrameworkCore.Database.Connection.ConnectionClosed":
                    HandleConnectionClosed(payload);
                    break;
                case "Microsoft.EntityFrameworkCore.Database.Command.CommandExecuting":
                    HandleCommandExecuting(payload);
                    break;
                case "Microsoft.EntityFrameworkCore.Database.Command.CommandExecuted":
                    HandleCommandExecuted(payload);
                    break;
                case "Microsoft.EntityFrameworkCore.Database.Transaction.TransactionStarted":
                    HandleTransactionStarted(payload);
                    break;
                case "Microsoft.EntityFrameworkCore.Database.Transaction.TransactionCommitted":
                    HandleTransactionCommitted(payload);
                    break;
                case "Microsoft.EntityFrameworkCore.Database.Transaction.TransactionRolledBack":
                    HandleTransactionRolledBack(payload);
                    break;
            }
        }
        catch (Exception ex)
        {
            Logger.Instance.Error($"Error handling event {eventName}: {ex}");
        }
    }

    private void HandleConnectionOpened(object payload)
    {
        Logger.Instance.Debug("Connection opened to database");
    }

    private void HandleConnectionClosed(object payload)
    {
        Logger.Instance.Debug("Connection closed to database");
    }

    private void HandleCommandExecuting(object payload)
    {
        Logger.Instance.Debug($"Executing command: ");
    }

    private void HandleCommandExecuted(object payload)
    {
        Logger.Instance.Debug($"Command executed: ");
    }

    private void HandleTransactionStarted(object payload)
    {
        Logger.Instance.Debug("Transaction started");
    }

    private void HandleTransactionCommitted(object payload)
    {
        Logger.Instance.Debug("Transaction committed");
    }

    private void HandleTransactionRolledBack(object payload)
    {
        Logger.Instance.Debug("Transaction rolled back");
    }
}
