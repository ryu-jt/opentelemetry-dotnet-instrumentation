#if !NETFRAMEWORK
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenTelemetry.AutoInstrumentation.Diagnostic;

class DiagnosticManager
{
    private static readonly HashSet<string> InterestedListeners = new()
    {
        "Microsoft.Extensions.Hosting",
        "Microsoft.AspNetCore",
        "Microsoft.EntityFrameworkCore",
        "SqlClientDiagnosticListener"
    };

    private readonly List<IDiagnosticHandler> handlers = new List<IDiagnosticHandler>();

    private static readonly Lazy<DiagnosticManager> instance = new Lazy<DiagnosticManager>(() => new DiagnosticManager());
    public static DiagnosticManager Instance => instance.Value;

    private IDisposable networkSubscription;
    private IDisposable listenerSubscription;
    private readonly object allListeners = new();

    // Private constructor to prevent instantiation outside of this class
    private DiagnosticManager()
    {
        handlers.Add(new AspNetCoreInHandler());
        handlers.Add(new EntityFrameworkCoreHandler());
        handlers.Add(new SqlClientHandler());
    }

    public void Listening()
    {
        Action<KeyValuePair<string, object>> whenHeard = delegate (KeyValuePair<string, object> data)
        {
            //Logger.Instance.Info($"* event: {data.Key}, {data.Value}");

            foreach (var handler in handlers)
            {
                handler.Handle(data.Key, data.Value);
            }
        };

        Action<DiagnosticListener> onNewListener = delegate (DiagnosticListener listener)
        {
            //Logger.Instance.Info($"* listener: {listener.Name}");

            if (!InterestedListeners.Contains(listener.Name)) return;

            lock (allListeners)
            {
                IObserver<KeyValuePair<string, object>> iobserver = new Observer<KeyValuePair<string, object>>(whenHeard, null);
                networkSubscription = listener.Subscribe(iobserver);
            }
        };

        IObserver<DiagnosticListener> observer = new Observer<DiagnosticListener>(onNewListener, null);
        listenerSubscription = DiagnosticListener.AllListeners.Subscribe(observer);
    }
}

class Observer<T> : IObserver<T>
{
    private readonly Action<T> _onNext;
    private readonly Action _onCompleted;

    public Observer(Action<T> onNext, Action onCompleted)
    {
        _onNext = onNext ?? throw new ArgumentNullException(nameof(onNext));
        _onCompleted = onCompleted ?? new Action(() => { });
    }

    public void OnCompleted() => _onCompleted();
    public void OnError(Exception error) { }
    public void OnNext(T value) => _onNext(value);
}
#endif
