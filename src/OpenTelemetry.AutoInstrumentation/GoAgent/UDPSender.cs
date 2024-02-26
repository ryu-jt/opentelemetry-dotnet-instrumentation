using System.Threading;
using WhaTap.Trace.Utils;

// TODO: MemoryStream 재활용 및 Object Pool 적용 검토

namespace WhaTap.Trace.GoAgent
{
    class UDPSender
    {
        static UDPSocket udpSocket;

        static UDPSender()
        {
            udpSocket = new UDPSocket();
            udpSocket.Client("127.0.0.1", 6600);

            bufferManager.BufferReadyToFlush += (buffer, length) =>
            {
                udpSocket.Send(buffer, length);
            };

            var thread = new Thread(() =>
            {
                while (true)
                {
                    Packet packet = _queue.Dequeue();
                    sendPacket(packet);
                    packet.Dispose();
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        public static void SendTransactionStart(Step step)
        {
            _queue.Enqueue(new Packet(PacketType.Start, step));
        }

        public static void SendTransactionEnd(Step step)
        {
            _queue.Enqueue(new Packet(PacketType.End, step));
        }

        public static void SendTransactionError(Step step)
        {
            _queue.Enqueue(new Packet(PacketType.Error, step));
        }

        public static void SendStep(Step step)
        {
            _queue.Enqueue(new Packet(step.Type, step));
        }

        private static void sendPacket(Packet packet)
        {
            bufferManager.EnqueuePacket(packet);
            if ((packet.Type == PacketType.Start) || (packet.Type == PacketType.End))
            {
                bufferManager.FlushBuffer();
            }
        }

        private static SuspensionQueue<Packet> _queue = new SuspensionQueue<Packet>();
        private static PacketBufferManager bufferManager = new PacketBufferManager();
    }
}
