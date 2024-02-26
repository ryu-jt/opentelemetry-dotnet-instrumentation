/**
* Debug용 로그를 UDP로 전송합니다.
* System.Diagnostics.Debug.WriteLine(message)가 제대로 동작하지 않는 경우 사용합니다.
* 사용법:
* DebugLogs.Instance.Log("Hello World");
*/
internal class DebugLogs
{
    private static readonly DebugLogs instance = new DebugLogs();

    private DebugLogs()
    {
        _udpSocket.Client("127.0.0.1", 2222);
    }

    public static DebugLogs Instance
    {
        get
        {
            return instance;
        }
    }

    public void Log(string message)
    {
        sendMessage(message);
    }

    private void sendMessage(string message)
    {
#if DEBUG
        lock (_lock)
        {
            _udpSocket.Send(message);
        }
#endif
    }

    private object _lock = new object();
    private UDPSocket _udpSocket = new UDPSocket();
}
