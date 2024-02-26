public class Logger
{
    private static readonly Logger instance = new Logger();

    private Logger()
    {
        _udpSocket.Client("127.0.0.1", 2222);
    }

    public static Logger Instance
    {
        get
        {
            return instance;
        }
    }

    public void Debug(string message)
    {
        sendMessage("[Debug] " + message);
    }

    public void Info(string message)
    {
        sendMessage("[Info] " + message);
    }

    public void Warning(string message)
    {
        sendMessage("[Warning] " + message);
    }

    public void Error(string message)
    {
        sendMessage("[Error] " + message);
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
