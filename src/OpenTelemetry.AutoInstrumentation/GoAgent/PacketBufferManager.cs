using System;
using WhaTap.Trace.Utils;

namespace WhaTap.Trace.GoAgent
{
    class PacketBufferManager
    {
        public bool TryGetArray(Packet packet)
        {
            switch (packet.Type)
            {
                case PacketType.Start:
                    return GoPackHttpStart.TryGetArray(packet.Type, packet.Step, packetBuffer, out packetBufferSize);
                case PacketType.End:
                    return GoPackHttpEnd.TryGetArray(packet.Type, packet.Step, packetBuffer, out packetBufferSize);
                case PacketType.Httpc:
                    return GoPackHttpCall.TryGetArray(packet.Type, packet.Step, packetBuffer, out packetBufferSize);
                case PacketType.DBConn:
                    return GoPackDBConn.TryGetArray(packet.Type, packet.Step, packetBuffer, out packetBufferSize);
                case PacketType.DBSql:
                    return GoPackDBSql.TryGetArray(packet.Type, packet.Step, packetBuffer, out packetBufferSize);
                case PacketType.Msg:
                    return GoPackMessage.TryGetArray(packet.Type, packet.Step, packetBuffer, out packetBufferSize);
                case PacketType.Method:
                    return GoPackMethod.TryGetArray(packet.Type, packet.Step, packetBuffer, out packetBufferSize);
                case PacketType.Error:
                    return GoPackError.TryGetArray(packet.Type, packet.Step, packetBuffer, out packetBufferSize);
                case PacketType.ActiveStack:
                    return GoPackActiveStack.TryGetArray(packet.Type, packet.Step, packetBuffer, out packetBufferSize);
                default:
                    return false;
            }
        }

        public void EnqueuePacket(Packet packet)
        {
            try
            {
                if (!TryGetArray(packet)) return;

                if (largeBufferOffset + packetBufferSize > largeBuffer.Length)
                {
                    FlushBuffer();
                }

                Array.Copy(packetBuffer, 0, largeBuffer, largeBufferOffset, packetBufferSize);
                largeBufferOffset += packetBufferSize;
            }
            catch (Exception e)
            {
                Logger.Instance.Error("PacketBufferManager.EnqueuePacket - {e}");
            }
        }

        public void FlushBuffer()
        {
            if (largeBufferOffset > 0)
            {
                BufferReadyToFlush?.Invoke(largeBuffer, largeBufferOffset);
                largeBufferOffset = 0;
            }
        }

        private byte[] packetBuffer = new byte[0xFFFF];
        private int packetBufferSize = 0;
        private readonly byte[] largeBuffer = new byte[0xFFFF];
        private int largeBufferOffset = 0;

        public event Action<byte[], int> BufferReadyToFlush;
    }
}
