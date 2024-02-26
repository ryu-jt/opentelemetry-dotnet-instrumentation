using System;

namespace WhaTap.Trace.GoAgent
{
    public static class GoPackActiveStack
    {
        public static bool TryGetArray(PacketType packetType, Step step, byte[] packetBuffer, out int filledSize)
        {
            filledSize = 0;

            string dataString = $", {step.TraceId}, {step.ActiveStack}";
            var data = PackUtils.GetStringPack(dataString);

            int totalSize = PackHeader.UDP_PACKET_HEADER_SIZE + data.Length;

            if (packetBuffer.Length < totalSize) return false;

            int offset = 0;

            Array.Copy(PackHeader.GetBytes(packetType, data.Length), 0, packetBuffer, offset, PackHeader.UDP_PACKET_HEADER_SIZE);
            offset += PackHeader.UDP_PACKET_HEADER_SIZE;

            Array.Copy(data, 0, packetBuffer, offset, data.Length);

            filledSize = totalSize;

            return true;
        }
    }
}
