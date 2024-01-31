using System.Net;
using System.Net.Sockets;
using System.Text;

internal class UDPSocket
{
    private const int BUFFER_SIZE = 8 * 1024;
    private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    private State state = new State();
    private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);

    public class State
    {
        public byte[] buffer = new byte[BUFFER_SIZE];
    }

    public void Server(string address, int port)
    {
        _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
        _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
    }

    public void Client(string address, int port)
    {
        _socket.Connect(IPAddress.Parse(address), port);
    }

    public void Send(string text)
    {
        byte[] data = Encoding.ASCII.GetBytes(text);
        Send(data, data.Length);
    }

    public void Send(byte[] buff)
    {
        Send(buff, buff.Length);
    }

    public void Send(byte[] buff, int size)
    {
        try
        {
            _socket.BeginSend(buff, 0, size, SocketFlags.None, ar =>
            {
                int bytesSent = _socket.EndSend(ar);
            }, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
